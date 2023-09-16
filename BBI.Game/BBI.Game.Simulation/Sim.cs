using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BBI.Core;
using BBI.Core.Collections;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Events;
using BBI.Core.IO.Streams;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.AI;
using BBI.Game.Commands;
using BBI.Game.Data;
using BBI.Game.Events;
using BBI.Game.Replay;
using BBI.Game.SaveLoad;
using BBI.Game.Simulation.Maneuver;
using BBI.Game.Utility;

namespace BBI.Game.Simulation
{
	public class Sim
	{
		public SimSettings Settings { get; private set; }

		public IEntitySystem EntitySystem { get; private set; }

		internal SimMap Map { get; private set; }

		internal IEventSystem SimEventSystem { get; private set; }

		internal IAsynchronousEventSystem SimToPresentationEventSystem { get; set; }

		public UnitManager UnitManager { get; private set; }

		internal CommanderManager CommanderManager { get; private set; }

		public ICommanderInteractionProvider InteractionProvider { get; private set; }

		internal SelectedUnitManager SelectedUnits { get; private set; }

		internal AIManager AIManager { get; private set; }

		internal SimFrameNumber GlobalFrameCount { get; set; }

		internal Sim(ReplayMode replayMode, IAsynchronousEventSystem simToPresentationEventSystem, SimSettings settings, DependencyContainerBase startupDependencies)
		{
			this.mSimStartupDependencies = startupDependencies;
			TransientLists.Initialize();
			Sim.Instance = this;
			this.GlobalFrameCount = SimFrameNumber.First;
			EntityGoalBase.ResetGoalIDCounter();
			this.SimEventSystem = new SynchronousEventSystem();
			this.SimToPresentationEventSystem = simToPresentationEventSystem;
			this.Settings = settings;
			this.EntitySystem = this.Settings.EntitySystem;
			EntityComponentExtensions.SimEntitySystem = this.EntitySystem;
			this.CommanderManager = new CommanderManager();
			this.CommanderManager.Initialize(settings.CommanderAttributesMap, settings.CommanderDirectorMap);
			if (replayMode == ReplayMode.ReplayingGame)
			{
				this.InteractionProvider = new ReplayInteractionProvider(this.CommanderManager);
			}
			else
			{
				this.InteractionProvider = new CommanderInteractionProvider(this.CommanderManager);
			}
			this.CommanderManager.AddSimEventSystemHandlers();
			foreach (CommanderID commanderID in this.CommanderManager.CommanderIDs)
			{
				this.Settings.EntityTypes.MakeAllTypesBuffableForCommander(commanderID.ID);
			}
			Maneuver.Maneuver.InitializeManeuverPool();
			this.UnitManager = new UnitManager(this);
			this.SelectedUnits = new SelectedUnitManager();
			startupDependencies.Get<IAnalyticsService>(out this.mAnalyticsService);
			DynamicSceneSpawner.Initialize();
			this.mSimInitializationState = Sim.InitializationState.Constructed;
		}

		internal Checksum Tick(SimFrameNumber frameNumber)
		{
			Checksum checksum = new Checksum();
			this.UnitManager.Tick(checksum, frameNumber);
			this.CommanderManager.Tick(checksum);
			MapModManager.Tick(frameNumber);
			AllEntityProcessor.Process(this.Settings.EntitySystem, checksum);
			if (this.Settings.GameMode != null && frameNumber > SimFrameNumber.First)
			{
				this.Settings.GameMode.EvaluateGameConditions(frameNumber, checksum);
			}
			if (this.AIManager != null)
			{
				this.AIManager.Tick(checksum);
			}
			checksum.Add((int)this.GlobalFrameCount.FrameNumber);
			return checksum;
		}

		internal void ShutdownGame()
		{
			if (this.Settings.GameMode != null)
			{
				this.Settings.GameMode.Shutdown();
				this.Settings.GameMode = null;
			}
		}

		internal void Shutdown()
		{
			this.mSimInitializationState = Sim.InitializationState.Uninitialized;
			AllEntityProcessor.Shutdown();
			DynamicSceneSpawner.Shutdown();
			this.ShutdownGame();
			if (this.InteractionProvider != null)
			{
				this.InteractionProvider.Shutdown();
				this.InteractionProvider = null;
			}
			if (this.CommanderManager != null)
			{
				this.CommanderManager.RemoveSimEventSystemHandlers();
				this.CommanderManager.Shutdown();
				this.CommanderManager = null;
			}
			if (this.AIManager != null)
			{
				this.AIManager.Shutdown();
				this.AIManager = null;
			}
			if (this.UnitManager != null)
			{
				this.UnitManager.Shutdown();
				this.UnitManager = null;
			}
			Maneuver.Maneuver.ShutdownManeuverPool();
			if (this.SimEventSystem != null)
			{
				this.SimEventSystem.RemoveAllHandlers();
				this.SimEventSystem = null;
			}
			TransientLists.Release();
			this.SimToPresentationEventSystem = null;
			this.Map = null;
			this.SelectedUnits = null;
			EntityComponentExtensions.SimEntitySystem = null;
			this.Settings = null;
			this.mSimStartupDependencies = null;
			this.mAnalyticsService = null;
			Sim.Instance = null;
		}

		private void OnEntityTypeAttributesSaved(Entity entity, ref EntityTypeAttributesSaveState state)
		{
			CommanderID entityCommanderID = Sim.GetEntityCommanderID(entity);
			state.IsCommanderTypeBuffed = (entityCommanderID != CommanderID.None);
			state.EntityTypeCommanderOwner = entityCommanderID.ID;
		}

		[CustomConverter(ConverterDirection.Save, ClassStateConversionOrder.RunStateDataConversionAfter)]
		internal void OnSave(ref SkirmishSaveState skirmishSaveState)
		{
			this.EntitySystem.SetLoadSaveConfigData(ComponentID.ComponentStateMap, this.Settings.EntityTypes, new EntityTypeAttributes.EntityTypeAttributesSaveCallback(this.OnEntityTypeAttributesSaved));
			ExtractorManager.Save<EntitySystem<BoolArray64>, EntitySystemSaveState<BoolArray64>>((EntitySystem<BoolArray64>)this.EntitySystem, ref skirmishSaveState.EntitySystem);
			ExtractorManager.Save<CommanderManager, CommanderManagerSaveState>(this.CommanderManager, ref skirmishSaveState.CommanderManager);
			EntityTypeCollection entityTypes = Sim.Instance.Settings.EntityTypes;
			Dictionary<CommanderEntityTypeKeySaveState, IDBuffSaveState[]> dictionary = new Dictionary<CommanderEntityTypeKeySaveState, IDBuffSaveState[]>();
			Dictionary<CommanderID, IDBuffSaveState> dictionary2 = new Dictionary<CommanderID, IDBuffSaveState>();
			foreach (Commander commander in this.CommanderManager.Commanders)
			{
				List<AttributeBuff> buffs = commander.GetBuffs();
				if (!buffs.IsNullOrEmpty<AttributeBuff>())
				{
					IDBuffSaveState value = new IDBuffSaveState
					{
						ID = string.Empty,
						Category = Buff.Category.Commander,
						Buffs = buffs.OutToArray<AttributeBuff>()
					};
					dictionary2.Add(commander.ID, value);
				}
				foreach (string text in entityTypes.GetAllEntityTypeNames())
				{
					EntityTypeAttributes commanderSpecificEntityType = entityTypes.GetCommanderSpecificEntityType(text, commander.ID.ID);
					IDBuffSaveState[] allEntityTypeBuffs = commanderSpecificEntityType.GetAllEntityTypeBuffs();
					if (allEntityTypeBuffs.Length > 0)
					{
						dictionary.Add(new CommanderEntityTypeKeySaveState
						{
							CommanderID = commander.ID,
							Type = text
						}, allEntityTypeBuffs);
					}
				}
			}
			ExtractorManager.Save<Dictionary<CommanderID, IDBuffSaveState>, KeyValuePair<CommanderID, IDBuffSaveState>[]>(dictionary2, ref skirmishSaveState.CommanderBuffs);
			ExtractorManager.Save<Dictionary<CommanderEntityTypeKeySaveState, IDBuffSaveState[]>, KeyValuePair<CommanderEntityTypeKeySaveState, IDBuffSaveState[]>[]>(dictionary, ref skirmishSaveState.CommanderTypeBuffs);
			Dictionary<Entity, IDBuffSaveState[]> dictionary3 = new Dictionary<Entity, IDBuffSaveState[]>();
			IEnumerable<Entity> enumerable = this.EntitySystem.Query().Has(0);
			foreach (Entity entity in enumerable)
			{
				EntityTypeAttributes typeAttributes = entity.GetTypeAttributes();
				if (typeAttributes.IsInstancedBuffed())
				{
					IDBuffSaveState[] allEntityTypeBuffs2 = typeAttributes.GetAllEntityTypeBuffs();
					dictionary3.Add(entity, allEntityTypeBuffs2);
				}
			}
			ExtractorManager.Save<Dictionary<Entity, IDBuffSaveState[]>, KeyValuePair<Entity, IDBuffSaveState[]>[]>(dictionary3, ref skirmishSaveState.BuffMap);
			ExtractorManager.Save<HashSet<Entity>, Entity[]>(this.UnitManager.DeployingUnits, ref skirmishSaveState.DeployingUnits);
			skirmishSaveState.CurrentEntityGoalIDCounter = EntityGoalBase.CurrentGoalIDCounter();
		}

		[CustomConverter(ConverterDirection.Load, ClassStateConversionOrder.RunStateDataConversionAfter)]
		internal void OnLoad(SkirmishSaveState skirmishSaveState)
		{
			EntityTypeCollection entityTypes = this.Settings.EntityTypes;
			this.EntitySystem.SetLoadSaveConfigData(ComponentID.ComponentStateMap, entityTypes, new EntityTypeAttributes.EntityTypeAttributesSaveCallback(this.OnEntityTypeAttributesSaved));
			entityTypes.ResetUserSpecificEntityTypes();
			this.UnitManager.RemoveAllUnits(this.GlobalFrameCount);
			EntityGoalBase.SetGoalIDCounter(skirmishSaveState.CurrentEntityGoalIDCounter);
			CommanderManager commanderManager = this.CommanderManager;
			ExtractorManager.Load<CommanderManager, CommanderManagerSaveState>(ref commanderManager, skirmishSaveState.CommanderManager);
			foreach (Commander commander in commanderManager.Commanders)
			{
				entityTypes.MakeAllTypesBuffableForCommander(commander.ID.ID);
			}
			foreach (KeyValuePair<CommanderID, IDBuffSaveState> keyValuePair in skirmishSaveState.CommanderBuffs)
			{
				Commander commanderFromID = commanderManager.GetCommanderFromID(keyValuePair.Key);
				GeneratedAttributeBuff[] array = new GeneratedAttributeBuff[keyValuePair.Value.Buffs.Length];
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = new GeneratedAttributeBuff(keyValuePair.Value.Category, keyValuePair.Value.ID, keyValuePair.Value.Buffs[j]);
				}
				GeneratedBuffSet buffset = new GeneratedBuffSet(array);
				commanderFromID.AddBuffs(buffset);
			}
			EntitySystem<BoolArray64> entitySystem = (EntitySystem<BoolArray64>)this.EntitySystem;
			ExtractorManager.Load<EntitySystem<BoolArray64>, EntitySystemSaveState<BoolArray64>>(ref entitySystem, skirmishSaveState.EntitySystem);
			entitySystem.Query().Has(16).Do(new Action<Entity>(HarvesterProcessor.RehookOnGoalStateChangedHandlers));
			entitySystem.Query().Has(23).Do(new Action<Entity>(HangarQueueProcessor.RehookUnitRemovedHandlers));
			this.UnitManager.OnUnitsLoaded(this.EntitySystem, skirmishSaveState.DeployingUnits);
			foreach (KeyValuePair<CommanderEntityTypeKeySaveState, IDBuffSaveState[]> keyValuePair2 in skirmishSaveState.CommanderTypeBuffs)
			{
				List<GeneratedAttributeBuff> list = new List<GeneratedAttributeBuff>();
				for (int l = 0; l < keyValuePair2.Value.Length; l++)
				{
					IDBuffSaveState idbuffSaveState = keyValuePair2.Value[l];
					foreach (AttributeBuff buff in idbuffSaveState.Buffs)
					{
						list.Add(new GeneratedAttributeBuff(idbuffSaveState.Category, idbuffSaveState.ID, buff));
					}
				}
				GeneratedBuffSet buffset2 = new GeneratedBuffSet(list.ToArray());
				entityTypes.AddBuffsForCommander(keyValuePair2.Key.Type, buffset2, keyValuePair2.Key.CommanderID, false);
			}
			foreach (KeyValuePair<Entity, IDBuffSaveState[]> keyValuePair3 in skirmishSaveState.BuffMap)
			{
				Entity key = keyValuePair3.Key;
				if (!key.IsValid())
				{
					Log.Error(Log.Channel.Core, "Load tried to apply entity buffs to an invalid entity! e = {0}", new object[]
					{
						key.ToFriendlyString()
					});
				}
				else
				{
					key.AddBuffsFromBuffStates(keyValuePair3.Value);
				}
			}
			DetectableProcessor.RefreshAllDetectablesOnLoad();
			this.EntitySystem.Query().Has(21).Do(new Action<Entity>(DetectableProcessor.PostDetectionStateEvents));
			IEnumerable<Entity> enumerable = this.EntitySystem.Query().Has(11).Has(10);
			SceneEntitiesLoadedEvent.ResourceCreatedEventData[] array2 = new SceneEntitiesLoadedEvent.ResourceCreatedEventData[enumerable.Count()];
			int num = 0;
			foreach (Entity entity in enumerable)
			{
				Resource component = entity.GetComponent<Resource>(11);
				Position component2 = entity.GetComponent<Position>(10);
				Vector2r b = component2.Orientation * component.PositionalVariations.SimLocalSpawnPositionOffset;
				Vector2r modelSpawnPosition = component2.Position2D - b;
				SceneEntitiesLoadedEvent.ResourceCreatedEventData resourceCreatedEventData = new SceneEntitiesLoadedEvent.ResourceCreatedEventData
				{
					ResourceEntity = entity,
					ResourceAttributeName = entity.GetTypeName(),
					ModelSpawnPosition = modelSpawnPosition,
					UseHeight = component.PositionalVariations.UseHeight,
					Height = component.PositionalVariations.Height,
					ModelOrientationEulersDegrees = component.PositionalVariations.ModelOrientationEulersDegrees
				};
				array2[num] = resourceCreatedEventData;
				num++;
			}
			IEnumerable<Entity> enumerable2 = this.EntitySystem.Query().Has(35);
			List<SceneEntitiesLoadedEvent.WreckArtifactCreatedEventData> list2 = new List<SceneEntitiesLoadedEvent.WreckArtifactCreatedEventData>(enumerable2.Count());
			foreach (Entity entity2 in enumerable2)
			{
				WreckArtifact component3 = entity2.GetComponent<WreckArtifact>(35);
				Position component4 = entity2.GetComponent<Position>(10);
				SceneEntitiesLoadedEvent.WreckArtifactCreatedEventData item;
				if (component4 != null)
				{
					item = new SceneEntitiesLoadedEvent.WreckArtifactCreatedEventData
					{
						WreckArtifactEntity = entity2,
						EntityTypeName = entity2.GetTypeName(),
						SpawnPosition = component4.Position2D,
						SpawnOrientation = component4.Orientation,
						CollectingEntity = Entity.None
					};
				}
				else
				{
					Collectible component5 = entity2.GetComponent<Collectible>(36);
					bool flag = false;
					foreach (Commander commander2 in this.CommanderManager.Commanders)
					{
						if (commander2.ArtifactHasBeenReturned(component3.WreckArtifactType))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						continue;
					}
					item = new SceneEntitiesLoadedEvent.WreckArtifactCreatedEventData
					{
						WreckArtifactEntity = entity2,
						EntityTypeName = entity2.GetTypeName(),
						SpawnPosition = Vector2r.Zero,
						SpawnOrientation = Orientation2.LocalForward,
						CollectingEntity = component5.CurrentHolderEntity
					};
				}
				list2.Add(item);
			}
			Entity[] newRelicEntities = this.EntitySystem.Query().Has(25).Has(10).OutToArray<Entity>();
			Sim.PostEvent(new SceneEntitiesLoadedEvent(array2, list2.ToArray(), newRelicEntities));
		}

		[CustomConverter(ConverterDirection.Save, ClassStateConversionOrder.RunStateDataConversionAfter)]
		internal void OnSave(ref CampaignPersistenceSaveState persistenceStruct)
		{
			Commander commanderFromID = this.CommanderManager.GetCommanderFromID(this.CommanderManager.LocalCommanderID);
			IEnumerable<Unit> unitsForCommander = this.UnitManager.GetUnitsForCommander(commanderFromID.ID);
			List<PersistentUnitDescription> list = new List<PersistentUnitDescription>();
			Dictionary<Entity, Health> dictionary = new Dictionary<Entity, Health>();
			Dictionary<Entity, Storage> dictionary2 = new Dictionary<Entity, Storage>();
			Dictionary<Entity, Experience> dictionary3 = new Dictionary<Entity, Experience>();
			Dictionary<Entity, PowerShuntDescriptionSaveState> dictionary4 = new Dictionary<Entity, PowerShuntDescriptionSaveState>();
			foreach (Unit unit in unitsForCommander)
			{
				if (!Sim.IsEntityDead(unit.Entity))
				{
					Storage component = unit.Entity.GetComponent<Storage>(18);
					if (component != null)
					{
						IEnumerable<ResourceType> carriedResources = component.GetCarriedResources();
						foreach (ResourceType resourceType in carriedResources)
						{
							string type = resourceType.ToString();
							int num;
							Storage.TransferAmountsForType(type, component, commanderFromID.Storage, out num);
						}
					}
					if (!unit.Attributes.DoNotPersist)
					{
						if (component != null)
						{
							dictionary2.Add(unit.Entity, component);
						}
						Experience component2 = unit.Entity.GetComponent<Experience>(39);
						if (component2 != null)
						{
							dictionary3.Add(unit.Entity, component2);
						}
						if ((unit.UnitClass & UnitClass.Carrier) == UnitClass.Carrier)
						{
							PowerShunt component3 = unit.Entity.GetComponent<PowerShunt>(22);
							if (component3 != null && component3.PowerSystems != null)
							{
								PowerSystemLevelSaveState[] array = new PowerSystemLevelSaveState[component3.PowerSystems.Count];
								int num2 = 0;
								foreach (KeyValuePair<PowerSystemType, PowerSystem> keyValuePair in component3.PowerSystems)
								{
									PowerSystemLevelSaveState powerSystemLevelSaveState = new PowerSystemLevelSaveState
									{
										SystemType = keyValuePair.Key,
										PowerLevel = keyValuePair.Value.CurrentTargetPowerLevelIndex
									};
									array[num2] = powerSystemLevelSaveState;
									num2++;
								}
								PowerShuntDescriptionSaveState value = new PowerShuntDescriptionSaveState
								{
									PowerLevels = array
								};
								dictionary4.Add(unit.Entity, value);
							}
							Health component4 = unit.Entity.GetComponent<Health>(7);
							if (component4 != null)
							{
								dictionary.Add(unit.Entity, component4);
							}
						}
						if (unit.Entity.HasComponent(15))
						{
							Production component5 = unit.Entity.GetComponent<Production>(15);
							for (int i = 0; i < component5.NumQueues; i++)
							{
								IList<Production.ProductionQueueEntry> queue = component5.GetQueue(i);
								foreach (Production.ProductionQueueEntry productionQueueEntry in queue)
								{
									commanderFromID.Storage.RefundBill(productionQueueEntry.OriginalResourceBill, Fixed64.One);
								}
							}
						}
						Position component6 = unit.Entity.GetComponent<Position>(10);
						Health component7 = unit.Entity.GetComponent<Health>(7);
						PersistentUnitDescription item = default(PersistentUnitDescription);
						item.Entity = unit.Entity;
						item.HpPercent = component7.CurrentHealthPercentage;
						item.UnitType = unit.TypeName;
						item.IsLevelBound = unit.Attributes.LevelBound;
						item.CommanderID = Sim.GetEntityCommanderID(unit.Entity);
						if ((unit.UnitClass & UnitClass.Air) == UnitClass.Air || component6 == null)
						{
							item.DockEntity = UnitHelperShared.FindStoredEntitysHangarOwner(unit.Entity);
							if (item.DockEntity == Entity.None)
							{
								item.DockEntity = commanderFromID.Carrier;
							}
						}
						else
						{
							item.Position = component6.Position2D;
							item.Orientation = component6.Orientation;
						}
						list.Add(item);
					}
				}
			}
			IEnumerable<Entity> enumerable = this.EntitySystem.Query().Has(10).Has(11).HasNot(37);
			List<LevelBoundResourceSaveState> list2 = new List<LevelBoundResourceSaveState>();
			foreach (Entity entity in enumerable)
			{
				LevelBoundResourceSaveState item2 = default(LevelBoundResourceSaveState);
				item2.Entity = entity;
				DetectableAttributesData detectableAttributesData = entity.GetTypeAttributes<DetectableAttributes>() as DetectableAttributesData;
				if (detectableAttributesData == null)
				{
					Log.Error(Log.Channel.Core, "Detectable attributes for {0} had detectable attributes that weren't of type DetectableAttributesData!", new object[]
					{
						entity.ToFriendlyString()
					});
				}
				else
				{
					ResourceAttributesData resourceAttributesData = entity.GetTypeAttributes<ResourceAttributes>() as ResourceAttributesData;
					if (resourceAttributesData == null)
					{
						Log.Error(Log.Channel.Core, "Resource attributes for {0} had Resource attributes that weren't of type ResourceAttribuesData!", new object[]
						{
							entity.ToFriendlyString()
						});
					}
					else
					{
						Resource component8 = entity.GetComponent<Resource>(11);
						Position component9 = entity.GetComponent<Position>(10);
						Detectable component10 = entity.GetComponent<Detectable>(21);
						Tags component11 = entity.GetComponent<Tags>(1);
						item2.TypeID = entity.GetTypeName();
						if (component11 != null)
						{
							ExtractorManager.Save<Tags, TagsSaveState>(component11, ref item2.Tags);
						}
						ExtractorManager.Save<ResourceAttributesData, ResourceAttributesSaveState>(resourceAttributesData, ref item2.ResourceAttributes);
						ExtractorManager.Save<DetectableAttributesData, DetectableAttributesDataSaveState>(detectableAttributesData, ref item2.DetectableAttributes);
						ExtractorManager.Save<Resource, ResourceSaveState>(component8, ref item2.ResourceComponent);
						ExtractorManager.Save<Position, PositionSaveState>(component9, ref item2.PositionComponent);
						ExtractorManager.Save<Detectable, DetectableSaveState>(component10, ref item2.DetectableComponent);
						list2.Add(item2);
					}
				}
			}
			IEnumerable<Entity> enumerable2 = this.EntitySystem.Query().Has(37).Has(10);
			List<LevelBoundWreckSaveState> list3 = new List<LevelBoundWreckSaveState>();
			foreach (Entity entity2 in enumerable2)
			{
				LevelBoundWreckSaveState item3 = default(LevelBoundWreckSaveState);
				item3.Entity = entity2;
				DetectableAttributesData detectableAttributesData2 = entity2.GetTypeAttributes<DetectableAttributes>() as DetectableAttributesData;
				if (detectableAttributesData2 == null)
				{
					Log.Error(Log.Channel.Core, "Detectable attributes for {0} had detectable attributes that weren't of type DetectableAttributesData!", new object[]
					{
						entity2.ToFriendlyString()
					});
				}
				else
				{
					ResourceAttributesData resourceAttributesData2 = entity2.GetTypeAttributes<ResourceAttributes>() as ResourceAttributesData;
					if (resourceAttributesData2 == null)
					{
						Log.Error(Log.Channel.Core, "Resource attributes for {0} had Resource attributes that weren't of type ResourceAttribuesData!", new object[]
						{
							entity2.ToFriendlyString()
						});
					}
					else
					{
						SimWreckAttributesData simWreckAttributesData = entity2.GetTypeAttributes<WreckAttributes>() as SimWreckAttributesData;
						if (simWreckAttributesData == null)
						{
							Log.Error(Log.Channel.Core, "Wreck attributes for {0} had wreck attributes that weren't of type SimWreckAttribuesData!", new object[]
							{
								entity2.ToFriendlyString()
							});
						}
						else
						{
							ShapeAttributesData shapeAttributesData = entity2.GetTypeAttributes<ShapeAttributes>() as ShapeAttributesData;
							if (shapeAttributesData == null)
							{
								Log.Error(Log.Channel.Core, "Shape attributes for {0} had shape attributes that weren't of type ShapeAttributesData!", new object[]
								{
									entity2.ToFriendlyString()
								});
							}
							else
							{
								Wreck component12 = entity2.GetComponent<Wreck>(37);
								Resource component13 = entity2.GetComponent<Resource>(11);
								Position component14 = entity2.GetComponent<Position>(10);
								Detectable component15 = entity2.GetComponent<Detectable>(21);
								Shape component16 = entity2.GetComponent<Shape>(33);
								Tags component17 = entity2.GetComponent<Tags>(1);
								item3.TypeID = entity2.GetTypeName();
								if (component17 != null)
								{
									ExtractorManager.Save<Tags, TagsSaveState>(component17, ref item3.Tags);
								}
								ExtractorManager.Save<ResourceAttributesData, ResourceAttributesSaveState>(resourceAttributesData2, ref item3.ResourceAttributes);
								ExtractorManager.Save<DetectableAttributesData, DetectableAttributesDataSaveState>(detectableAttributesData2, ref item3.DetectableAttributes);
								ExtractorManager.Save<SimWreckAttributesData, SimWreckAttributesDataSaveState>(simWreckAttributesData, ref item3.WreckAttributes);
								ExtractorManager.Save<ShapeAttributesData, ShapeAttributesSaveState>(shapeAttributesData, ref item3.ShapeAttributes);
								ExtractorManager.Save<Wreck, WreckSaveState>(component12, ref item3.WreckComponent);
								ExtractorManager.Save<Resource, ResourceSaveState>(component13, ref item3.ResourceComponent);
								ExtractorManager.Save<Position, PositionSaveState>(component14, ref item3.PositionComponent);
								ExtractorManager.Save<Detectable, DetectableSaveState>(component15, ref item3.DetectableComponent);
								ExtractorManager.Save<Shape, ShapeSaveState>(component16, ref item3.ShapeComponent);
								list3.Add(item3);
							}
						}
					}
				}
			}
			persistenceStruct.ResourceSaveStates = list2.ToArray();
			persistenceStruct.WreckSaveStates = list3.ToArray();
			persistenceStruct.CompletedResearch = new string[commanderFromID.CompletedResearch.Count];
			int num3 = 0;
			foreach (string text in commanderFromID.CompletedResearch)
			{
				persistenceStruct.CompletedResearch[num3] = text;
				num3++;
			}
			persistenceStruct.DynamicDifficultyCoefficient = commanderFromID.DynamicDifficultyCoefficient;
			IEnumerable<Entity> enumerable3 = this.EntitySystem.Query().Has(35).Has(36);
			foreach (Entity entity3 in enumerable3)
			{
				Collectible component18 = entity3.GetComponent<Collectible>(36);
				WreckArtifact component19 = entity3.GetComponent<WreckArtifact>(35);
				if (component18.CurrentHolderEntity != Entity.None)
				{
					CommanderID entityCommanderID = Sim.GetEntityCommanderID(component18.CurrentHolderEntity);
					if (entityCommanderID == commanderFromID.ID && !commanderFromID.ArtifactHasBeenReturned(component19.WreckArtifactType))
					{
						commanderFromID.ReturnWreckArtifact(component19.WreckArtifactType, true);
					}
				}
			}
			commanderFromID.CompleteAllPendingWreckArtifactAnalysis();
			ExtractorManager.Save<IDictionary<string, Commander.WreckArtifactState>, KeyValuePair<string, WreckArtifactStateSaveState>[]>(commanderFromID.AnalyzedWreckArtifacts, ref persistenceStruct.AnalyzedWreckArtifacts);
			ExtractorManager.Save<Storage, StorageSaveState>(commanderFromID.Storage, ref persistenceStruct.Storage);
			ExtractorManager.Save<List<PersistentUnitDescription>, PersistentUnitDescription[]>(list, ref persistenceStruct.PersistentUnits);
			ExtractorManager.Save<Dictionary<Entity, PowerShuntDescriptionSaveState>, KeyValuePair<Entity, PowerShuntDescriptionSaveState>[]>(dictionary4, ref persistenceStruct.PowerShuntDescriptions);
			ExtractorManager.Save<Dictionary<Entity, Health>, KeyValuePair<Entity, HealthSaveState>[]>(dictionary, ref persistenceStruct.HealthComponents);
			ExtractorManager.Save<Dictionary<Entity, Storage>, KeyValuePair<Entity, StorageSaveState>[]>(dictionary2, ref persistenceStruct.EntityStorage);
			ExtractorManager.Save<Dictionary<Entity, Experience>, KeyValuePair<Entity, ExperienceSaveState>[]>(dictionary3, ref persistenceStruct.ExperienceComponents);
		}

		internal void PostMapProcessInit()
		{
			if (this.mSimInitializationState != Sim.InitializationState.MapLoaded)
			{
				return;
			}
			Assert.Release(this.mSimInitializationState == Sim.InitializationState.MapLoaded, "Sim attempted to post process the map but it hadn't been read yet!");
			SimMapDependencies mapDependencies;
			this.mSimStartupDependencies.Get<SimMapDependencies>(out mapDependencies);
			this.AIManager = new AIManager(this.SimEventSystem, this.CommanderManager.CPUCommanders);
			if (this.Settings.GameMode != null)
			{
				this.Settings.GameMode.Initialize();
			}
			SessionChangeReason sessionChangeReason;
			if (!this.mSimStartupDependencies.Get<SessionChangeReason>(out sessionChangeReason))
			{
				sessionChangeReason = SessionChangeReason.NewGame;
			}
			MissionDependencies missionDependencies;
			if (this.mSimStartupDependencies.Get<MissionDependencies>(out missionDependencies) && !missionDependencies.EntityDescriptors.IsNullOrEmpty<SceneEntityDescriptor>())
			{
				SceneEntityCreator.CreateSceneEntitiesForGameSession(missionDependencies.EntityGroupDescriptors, missionDependencies.EntityDescriptors, missionDependencies.RandomWreckArtifacts, missionDependencies.MaxSpawnedWreckArtifacts, sessionChangeReason == SessionChangeReason.Transition);
			}
			AllEntityProcessor.Initialize();
			if (sessionChangeReason != SessionChangeReason.LoadGame)
			{
				foreach (KeyValuePair<CommanderID, CommanderAttributes> keyValuePair in this.Settings.CommanderAttributesMap)
				{
					Commander commanderFromID = Sim.Instance.CommanderManager.GetCommanderFromID(keyValuePair.Key);
					if (commanderFromID != null)
					{
						this.InitializeResearchForCommander(commanderFromID);
						this.ApplyInitialBuffsForCommander(commanderFromID, true);
						this.GrantStartingGrantedAbilitiesForCommander(commanderFromID, true);
						this.SpawnInitialUnitsForCommander(commanderFromID, this.Settings, mapDependencies);
						commanderFromID.GrantStartingWreckArtifacts();
					}
				}
			}
			MapModManager.LoadMapLayout();
			this.mSimInitializationState = Sim.InitializationState.Initialized;
		}

		internal IEnumerator PrepareSimMap()
		{
			if (this.mSimInitializationState != Sim.InitializationState.LoadingMap)
			{
				this.mSimInitializationState = Sim.InitializationState.LoadingMap;
				SimMapDependencies mapDependencies;
				if (this.mSimStartupDependencies.Get<SimMapDependencies>(out mapDependencies))
				{
					DateTime startTime = DateTime.Now;
					SimHeightMap heightMap = (mapDependencies.HeightMapAttributes == null) ? null : new SimHeightMap(mapDependencies.HeightMapAttributes.HeightMap, mapDependencies.HeightMapAttributes.GridWidth, mapDependencies.HeightMapAttributes.GridLength, mapDependencies.HeightMapAttributes.MinBounds, mapDependencies.HeightMapAttributes.MaxBounds, mapDependencies.HeightMapAttributes.MinHeight, mapDependencies.HeightMapAttributes.MaxHeight);
					bool needsRuntimeBake = true;
					if (mapDependencies.SerializedSimMapData != null)
					{
						MemoryStream memStream = new MemoryStream(mapDependencies.SerializedSimMapData);
						BinaryStreamReader steamReader = new BinaryStreamReader(memStream);
						DateTime start = DateTime.Now;
						this.Map = new SimMap(heightMap, mapDependencies.AmbientHeatPoints);
						IEnumerator mapReadEnumerator = this.Map.Read(steamReader);
						bool canContinue = true;
						while (canContinue)
						{
							yield return null;
							try
							{
								canContinue = mapReadEnumerator.MoveNext();
							}
							catch (Exception ex)
							{
								Log.Error(Log.Channel.Core, "Failed to deserialize SimMap from baked data with exception '{0}'. Falling back to runtime mesh baking.", new object[]
								{
									ex
								});
								canContinue = false;
							}
						}
						memStream.Close();
						Log.Info(Log.Channel.Core, "Map deserialization took {0}ms", new object[]
						{
							(DateTime.Now - start).TotalMilliseconds
						});
						needsRuntimeBake = false;
					}
					if (needsRuntimeBake)
					{
						this.Map = new SimMap(mapDependencies.MinPathfindingBounds, mapDependencies.MaxPathfindingBounds, heightMap, mapDependencies.AmbientHeatPoints);
						TimeSpan timeSpan = DateTime.Now.Subtract(startTime);
						startTime = DateTime.Now;
						if (mapDependencies.Obstacles != null)
						{
							foreach (MapObstacle mapObstacle in mapDependencies.Obstacles)
							{
								this.Map.AddObstacle(mapObstacle);
							}
						}
						if (mapDependencies.RidgeLines != null)
						{
							foreach (SimRidgeLine ridgeLine in mapDependencies.RidgeLines)
							{
								this.Map.AddRidgeLine(ridgeLine);
							}
						}
						TimeSpan timeSpan2 = DateTime.Now.Subtract(startTime);
						startTime = DateTime.Now;
						if (this.Settings.NavMeshes != null)
						{
							this.Map.BakeSpatialPartition(this.Settings.NavMeshes);
						}
						TimeSpan timeSpan3 = DateTime.Now.Subtract(startTime);
						Log.Warn(Log.Channel.Core, "Map init: {0}ms. Obstacles & ridge lines: {1}ms. Nav mesh: {2}ms", new object[]
						{
							timeSpan.TotalMilliseconds,
							timeSpan2.TotalMilliseconds,
							timeSpan3.TotalMilliseconds
						});
					}
				}
				else
				{
					this.Map = new SimMap(new Vector2r(Fixed64.MinValue, Fixed64.MinValue), new Vector2r(Fixed64.MaxValue, Fixed64.MaxValue), null, 0);
				}
				this.mSimInitializationState = Sim.InitializationState.MapLoaded;
				if (MapModManager.CustomLayout)
				{
					bool disableAllBlockers = false;
					if (MapModManager.DisableAllBlockers)
					{
						this.Map.mPathfindingObstaclesHash.Clear();
						disableAllBlockers = true;
					}
					foreach (MapModManager.MapColliderData collider in MapModManager.colliders)
					{
						this.Map.AddObstacle(collider.collider, collider.mask, collider.blockLof, collider.blockAllHeights);
						if (collider.blockLof)
						{
							this.Map.mStaticLineOfFireHash.AddShape(collider.collider);
						}
						if (collider.blockAllHeights)
						{
							this.Map.mAllHeightBlockers.Add(collider.collider);
						}
						disableAllBlockers = true;
					}
					if (disableAllBlockers || MapModManager.overrideBounds)
					{
						this.Map.BakeSpatialPartition(this.Settings.NavMeshes);
					}
				}
			}
			yield break;
		}

		internal static CommanderID GetEntityCommanderID(Entity entity)
		{
			if (!entity.IsValid())
			{
				return CommanderID.None;
			}
			OwningCommander component = entity.GetComponent<OwningCommander>(5);
			if (component != null)
			{
				return component.ID;
			}
			return CommanderID.None;
		}

		internal static bool EntityShouldMaintainAggroThroughFOW(Entity entity)
		{
			CommanderID entityCommanderID = Sim.GetEntityCommanderID(entity);
			Commander commanderFromID = Sim.Instance.CommanderManager.GetCommanderFromID(entityCommanderID);
			return commanderFromID != null && commanderFromID.CommanderAttributes != null && commanderFromID.CommanderAttributes.MaintainAggroThroughFOW;
		}

		internal static bool IsValidEntityType(string typeID)
		{
			if (!string.IsNullOrEmpty(typeID))
			{
				EntityTypeAttributes entityType = Sim.Instance.Settings.EntityTypes.GetEntityType(typeID);
				return entityType != null;
			}
			return false;
		}

		internal static bool IsEntityDead(Entity entity)
		{
			return !entity.IsValid() || (entity.HasComponent(9) && !entity.GetComponent<Death>(9).WaitingForOnDeathActivity);
		}

		internal static bool IsEntityDocked(Entity entity)
		{
			return entity.IsValid() && entity.HasComponent(2) && !entity.HasComponent(10);
		}

		internal static Entity CreateEmptyEntity()
		{
			return Sim.CreateCommanderlessEntity(string.Empty);
		}

		internal static Entity CreateCommanderlessEntity(string typeID)
		{
			return Sim.Instance.EntitySystem.CreateTypedEntity(typeID);
		}

		internal static Entity CreateEntity(string typeID, CommanderID commanderID)
		{
			Entity entity = Sim.Instance.EntitySystem.CreateTypedEntity(typeID, commanderID.ID);
			entity.AddComponent<OwningCommander>(5, OwningCommander.Create(commanderID));
			return entity;
		}

		internal static void AddEntityTypeBuffs(string unitType, bool useAsPrefix, UnitClass unitClass, FlagOperator classOperator, AttributeBuffSet buffSet, CommanderID commanderID)
		{
			Sim.AddEntityTypeBuffs(unitType, useAsPrefix, unitClass, classOperator, buffSet, commanderID, false);
		}

		internal static void AddEntityTypeBuffs(string unitType, bool useAsPrefix, UnitClass unitClass, FlagOperator classOperator, AttributeBuffSet buffSet, CommanderID commanderID, bool silent)
		{
			Commander commanderFromID = Sim.Instance.CommanderManager.GetCommanderFromID(commanderID);
			if (buffSet == null)
			{
				Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "[GO] - Expected a non-null buffset in starting buffs associated with commander {0} for type buff {1}", new object[]
				{
					commanderFromID.Name,
					unitType
				});
				return;
			}
			commanderFromID.AddBuffs(buffSet);
			foreach (string text in Sim.Instance.Settings.EntityTypes.GetAllEntityTypeNames())
			{
				if (useAsPrefix)
				{
					if (!text.StartsWith(unitType))
					{
						continue;
					}
				}
				else if (text != unitType)
				{
					continue;
				}
				EntityTypeAttributes entityType = Sim.Instance.Settings.EntityTypes.GetEntityType(text);
				UnitAttributes unitAttributes = entityType.Get<UnitAttributes>();
				if (unitAttributes != null)
				{
					bool flag = true;
					if (unitClass != UnitClass.None)
					{
						switch (classOperator)
						{
						case FlagOperator.Or:
							flag = ((unitAttributes.Class & unitClass) != UnitClass.None);
							break;
						case FlagOperator.And:
							flag = ((unitAttributes.Class & unitClass) == unitClass);
							break;
						default:
							Log.Warn(Log.Channel.Data | Log.Channel.Gameplay, "Unhandled FlagOperator type {0}!", new object[]
							{
								classOperator
							});
							flag = false;
							break;
						}
					}
					if (flag)
					{
						Sim.Instance.Settings.EntityTypes.AddBuffsForCommander(text, buffSet, commanderID, silent);
					}
				}
			}
		}

		internal static void RemoveEntityTypeBuffs(string unitType, bool useAsPrefix, UnitClass unitClass, FlagOperator classOperator, AttributeBuffSet buffSet, CommanderID commanderID)
		{
			Commander commanderFromID = Sim.Instance.CommanderManager.GetCommanderFromID(commanderID);
			commanderFromID.RemoveBuffs(buffSet);
			foreach (string text in Sim.Instance.Settings.EntityTypes.GetAllEntityTypeNames())
			{
				if (useAsPrefix)
				{
					if (!text.StartsWith(unitType))
					{
						continue;
					}
				}
				else if (text != unitType)
				{
					continue;
				}
				EntityTypeAttributes entityType = Sim.Instance.Settings.EntityTypes.GetEntityType(text);
				UnitAttributes unitAttributes = entityType.Get<UnitAttributes>();
				if (unitAttributes != null)
				{
					bool flag = true;
					if (unitClass != UnitClass.None)
					{
						switch (classOperator)
						{
						case FlagOperator.Or:
							flag = ((unitAttributes.Class & unitClass) != UnitClass.None);
							break;
						case FlagOperator.And:
							flag = ((unitAttributes.Class & unitClass) == unitClass);
							break;
						default:
							Log.Warn(Log.Channel.Data | Log.Channel.Gameplay, "Unhandled FlagOperator type {0}!", new object[]
							{
								classOperator
							});
							flag = false;
							break;
						}
					}
					if (flag)
					{
						Sim.Instance.Settings.EntityTypes.RemoveBuffsForCommander(text, buffSet, commanderID);
					}
				}
			}
		}

		internal static T GetEntityTypeAttributes<T>(string typeID) where T : class
		{
			EntityTypeAttributes entityType = Sim.Instance.Settings.EntityTypes.GetEntityType(typeID);
			if (entityType != null)
			{
				return entityType.Get<T>();
			}
			return default(T);
		}

		internal static T GetBuffedEntityTypeAttributes<T>(string typeID, CommanderID commanderID) where T : class
		{
			EntityTypeAttributes commanderSpecificEntityType = Sim.Instance.Settings.EntityTypes.GetCommanderSpecificEntityType(typeID, commanderID.ID);
			if (commanderSpecificEntityType != null)
			{
				return commanderSpecificEntityType.Get<T>();
			}
			return default(T);
		}

		public static void DestroyEntity(Entity entity)
		{
			if (entity.IsValid())
			{
				if (entity.HasComponent(10))
				{
					Sim.Instance.Map.SpatialHash.RemoveEntity(entity);
				}
				Sim.Instance.EntitySystem.DestroyEntity(entity);
			}
		}

		internal static int Rand()
		{
			return Sim.Instance.Settings.RandomNumberGenerator.Next();
		}

		internal static int Rand(int max)
		{
			return Sim.Instance.Settings.RandomNumberGenerator.Next(max);
		}

		internal static int RandRange(int min, int max)
		{
			return Sim.Instance.Settings.RandomNumberGenerator.Next(min, max);
		}

		internal static Fixed64 Rand01()
		{
			return Sim.Instance.Settings.RandomNumberGenerator.NextFixed64();
		}

		internal static Fixed64 Rand(Fixed64 max)
		{
			return Sim.Instance.Settings.RandomNumberGenerator.Next(max);
		}

		internal static Fixed64 RandRange(Fixed64 min, Fixed64 max)
		{
			return Sim.Instance.Settings.RandomNumberGenerator.Next(min, max);
		}

		private void ApplyInitialBuffsForCommander(Commander commander, bool silent)
		{
			if (commander == null)
			{
				return;
			}
			if (commander.CommanderAttributes.StartingBuffs != null)
			{
				foreach (UnitTypeBuff unitTypeBuff in commander.CommanderAttributes.StartingBuffs)
				{
					Sim.AddEntityTypeBuffs(unitTypeBuff.UnitType, unitTypeBuff.UseAsPrefix, unitTypeBuff.UnitClass, unitTypeBuff.ClassOperator, unitTypeBuff.BuffSet, commander.ID, silent);
				}
			}
			if (commander.CommanderAttributes.Faction != null && !commander.CommanderAttributes.Faction.GlobalBuffs.IsNullOrEmpty<UnitTypeBuff>())
			{
				foreach (UnitTypeBuff unitTypeBuff2 in commander.CommanderAttributes.Faction.GlobalBuffs)
				{
					Sim.AddEntityTypeBuffs(unitTypeBuff2.UnitType, unitTypeBuff2.UseAsPrefix, unitTypeBuff2.UnitClass, unitTypeBuff2.ClassOperator, unitTypeBuff2.BuffSet, commander.ID, silent);
				}
			}
		}

		private void InitializeResearchForCommander(Commander commander)
		{
			if (commander == null)
			{
				return;
			}
			if (commander.CommanderAttributes.LockedTech != null)
			{
				foreach (string researchName in commander.CommanderAttributes.LockedTech)
				{
					commander.LockResearch(researchName);
				}
			}
			if (commander.CommanderAttributes.StartingTech != null)
			{
				foreach (string text in commander.CommanderAttributes.StartingTech)
				{
					ResearchItemAttributes entityTypeAttributes = Sim.GetEntityTypeAttributes<ResearchItemAttributes>(text);
					if (entityTypeAttributes != null)
					{
						commander.GrantResearch(entityTypeAttributes, true);
					}
					else
					{
						Log.Error(Log.Channel.Gameplay, "Commander was initialized with a starting research that does not match a research in it's tech tree: {0}", new object[]
						{
							text
						});
					}
				}
			}
		}

		private void GrantStartingGrantedAbilitiesForCommander(Commander commander, bool silent)
		{
			if (commander == null)
			{
				return;
			}
			if (commander.CommanderAttributes.StartingGrantedAbilities != null)
			{
				for (int i = 0; i < commander.CommanderAttributes.StartingGrantedAbilities.Length; i++)
				{
					string abilityTypeName = commander.CommanderAttributes.StartingGrantedAbilities[i];
					AbilityHelper.AddAbilityToCommander(abilityTypeName, commander, silent);
				}
			}
		}

		private void GetCommanderSpawnPoint(SimSettings simSettings, SimMapDependencies mapDependencies, CommanderID commanderID, out Vector2r spawnPoint, out Orientation2 orientation)
		{
			CommanderDirectorAttributes commanderDirectorAttributes = simSettings.CommanderDirectorMap[commanderID];
			int spawnIndex = commanderDirectorAttributes.SpawnIndex;
			spawnPoint = Vector2r.Zero;
			orientation = Orientation2.LocalForward;
			if (simSettings.SpawnPoints.IsNullOrEmpty<SpawnAttributes>())
			{
				if (mapDependencies != null)
				{
					Log.Error(Log.Channel.Gameplay, "No spawn point data found, randomly placing commander {0}", new object[]
					{
						commanderID
					});
					spawnPoint = new Vector2r(Sim.RandRange(mapDependencies.MinPathfindingBounds.X, mapDependencies.MaxPathfindingBounds.X), Sim.RandRange(mapDependencies.MinPathfindingBounds.Y, mapDependencies.MaxPathfindingBounds.Y));
				}
				return;
			}
			if (spawnIndex < simSettings.SpawnPoints.Length)
			{
				SpawnAttributes spawnAttributes = simSettings.SpawnPoints[spawnIndex];
				spawnPoint = spawnAttributes.SpawnPoint;
				orientation = spawnAttributes.SpawnOrientation;
				return;
			}
			Log.Error(Log.Channel.Gameplay, "Assigning psuedo-random start point to commander {0} because their spawn index is outside the range of provided spawn points (requested index {1} but there are {2})", new object[]
			{
				commanderID,
				spawnIndex,
				simSettings.SpawnPoints.Length
			});
			SpawnAttributes spawnAttributes2 = simSettings.SpawnPoints[spawnIndex % simSettings.SpawnPoints.Length];
			spawnPoint = spawnAttributes2.SpawnPoint;
			orientation = spawnAttributes2.SpawnOrientation;
		}

		private void SplitUnitTypesAndPrepareFormationTrackers(Commander commander, Orientation2 orientation, out List<StartingUnit> carrierUnits, out List<StartingUnit> groundUnits, out List<StartingUnit> airUnits, out Dictionary<string, Sim.FormationTracker> formationTrackers)
		{
			carrierUnits = new List<StartingUnit>();
			groundUnits = new List<StartingUnit>();
			airUnits = new List<StartingUnit>();
			formationTrackers = new Dictionary<string, Sim.FormationTracker>();
			foreach (StartingUnit startingUnit in commander.CommanderAttributes.StartingUnits)
			{
				UnitAttributes entityTypeAttributes = Sim.GetEntityTypeAttributes<UnitAttributes>(startingUnit.Unit);
				if (!Sim.IsValidEntityType(startingUnit.Unit) || entityTypeAttributes == null)
				{
					Log.Error(Log.Channel.Gameplay, "Commander manifest tried to give the commander {0} the unit type {1}, but the unit type does not exist!", new object[]
					{
						commander.Name,
						startingUnit.Unit
					});
				}
				else if ((entityTypeAttributes.Class & UnitClass.Air) != UnitClass.None)
				{
					airUnits.Add(startingUnit);
				}
				else
				{
					Sim.FormationTracker formationTracker;
					if (!formationTrackers.ContainsKey(startingUnit.Unit))
					{
						formationTracker = (formationTrackers[startingUnit.Unit] = new Sim.FormationTracker());
						UnitMovementAttributes entityTypeAttributes2 = Sim.GetEntityTypeAttributes<UnitMovementAttributes>(startingUnit.Unit);
						formationTracker.Class = entityTypeAttributes.Class;
						formationTracker.MaxRankCount = entityTypeAttributes2.Maneuvers.ColumnCount;
						formationTracker.MemberGap = entityTypeAttributes2.Maneuvers.FormationSpacingDistance;
						formationTracker.CapsuleHeight = entityTypeAttributes2.Dynamics.Length;
						formationTracker.CapsuleWidth = entityTypeAttributes2.Dynamics.Width;
						formationTracker.StartsInHangar = entityTypeAttributes.StartsInHangar;
						formationTracker.Orientation = ((formationTracker.FormationOverride == null) ? orientation : formationTracker.FormationOverride.Orientation);
						foreach (SpawnFormation spawnFormation in commander.CommanderAttributes.OverriddenSpawnFormations)
						{
							if (spawnFormation.UnitType == startingUnit.Unit)
							{
								formationTracker.FormationOverride = spawnFormation;
							}
						}
					}
					else
					{
						formationTracker = formationTrackers[startingUnit.Unit];
					}
					formationTracker.TotalUnits++;
					if ((formationTracker.Class & UnitClass.Carrier) == UnitClass.Carrier)
					{
						carrierUnits.Add(startingUnit);
					}
					else
					{
						groundUnits.Add(startingUnit);
					}
				}
			}
		}

		private void GetMaxClassPatternDimensions(Dictionary<string, Sim.FormationTracker> formationTrackers, out Vector2r maxSmlDimensions, out Vector2r maxMedDimensions)
		{
			maxMedDimensions = Vector2r.Zero;
			maxSmlDimensions = Vector2r.Zero;
			foreach (KeyValuePair<string, Sim.FormationTracker> keyValuePair in formationTrackers)
			{
				Sim.FormationTracker value = keyValuePair.Value;
				if (value.FormationOverride != null)
				{
					value.Pattern = FormationPatternBase.CreateSpawnFormationPattern(value.FormationOverride.FormationPattern, value.MemberGap, Math.Min(value.MaxRankCount, value.TotalUnits));
				}
				else
				{
					value.Pattern = FormationPatternBase.CreateSpawnFormationPattern(FormationPatternType.Rectangular, value.MemberGap, Math.Min(value.MaxRankCount, value.TotalUnits));
					Fixed64 y = value.Pattern.Width(value.TotalUnits) + value.CapsuleWidth;
					Fixed64 y2 = value.Pattern.Height(value.TotalUnits) + value.CapsuleHeight;
					if ((value.Class & UnitClass.Carrier) == UnitClass.Carrier)
					{
						maxMedDimensions.X = Fixed64.Max(maxMedDimensions.X, y);
						maxMedDimensions.Y = Fixed64.Max(maxMedDimensions.Y, y2);
					}
					if ((value.Class & UnitClass.Small) == UnitClass.Small)
					{
						maxSmlDimensions.X = Fixed64.Max(maxSmlDimensions.X, y);
						maxSmlDimensions.Y = Fixed64.Max(maxSmlDimensions.Y, y2);
					}
					if ((value.Class & UnitClass.Medium) == UnitClass.Medium)
					{
						maxMedDimensions.X = Fixed64.Max(maxMedDimensions.X, y);
						maxMedDimensions.Y = Fixed64.Max(maxMedDimensions.Y, y2);
					}
				}
			}
		}

		private void SpawnCarriersInMediumUnitRow(CommanderID commanderID, Dictionary<string, Sim.FormationTracker> formationTrackers, List<StartingUnit> carrierUnits, out Fixed64 oddSide, out Fixed64 evenSide, Vector2r initialSpawnPoint, Orientation2 orientation, ref int medUnitTypes, out UnitHangar hangar, out bool hasCarrierHangar)
		{
			oddSide = Fixed64.Zero;
			evenSide = Fixed64.Zero;
			hangar = null;
			hasCarrierHangar = false;
			foreach (KeyValuePair<string, Sim.FormationTracker> keyValuePair in formationTrackers)
			{
				Sim.FormationTracker value = keyValuePair.Value;
				if (value.FormationOverride != null)
				{
					value.Position = value.FormationOverride.Position;
				}
				else if ((value.Class & UnitClass.Carrier) == UnitClass.Carrier)
				{
					this.SetTrackerRowPosition(value, initialSpawnPoint, orientation, ref oddSide, ref evenSide, medUnitTypes);
					medUnitTypes++;
				}
			}
			foreach (StartingUnit startingUnit in carrierUnits)
			{
				Sim.FormationTracker formationTracker = formationTrackers[startingUnit.Unit];
				Vector2r spawnPoint;
				if (carrierUnits.Count == 1)
				{
					spawnPoint = formationTracker.Position;
				}
				else
				{
					spawnPoint = formationTracker.Pattern.Position(formationTracker.Position, formationTracker.Orientation, formationTracker.CurrentCount);
				}
				Entity entity;
				if (!this.UnitManager.SpawnNewUnitIntoWorld(startingUnit.Unit, commanderID, spawnPoint, formationTracker.Orientation, out entity, new string[]
				{
					startingUnit.Tag
				}))
				{
					Log.Error(Log.Channel.Gameplay, "Commander Manifest failed to spawn a carrier type unit into the world ({0})!", new object[]
					{
						startingUnit.Unit
					});
				}
				else
				{
					if (startingUnit.OnSpawnedInSim != null)
					{
						startingUnit.OnSpawnedInSim(entity);
					}
					if (!hasCarrierHangar)
					{
						hangar = entity.GetComponent<UnitHangar>(23);
						if (hangar != null)
						{
							hasCarrierHangar = true;
						}
					}
					formationTracker.CurrentCount++;
				}
			}
		}

		private void InitializeTrackerPositions(Dictionary<string, Sim.FormationTracker> formationTrackers, Vector2r initialSpawnPoint, Orientation2 orientation, Fixed64 medOddRowGap, Fixed64 medEvenRowGap, Fixed64 smallOddRowGap, Fixed64 smallEvenRowGap, Vector2r maxSmlDimensions, Vector2r maxMedDimensions, int smallUnitTypes, int medUnitTypes)
		{
			Vector2r startPosition = initialSpawnPoint + orientation.Forward * ((maxMedDimensions.Y >> 1) + maxSmlDimensions.Y);
			Vector2r vector2r = initialSpawnPoint + orientation.Back * (maxMedDimensions.Y >> 1);
			foreach (KeyValuePair<string, Sim.FormationTracker> keyValuePair in formationTrackers)
			{
				Sim.FormationTracker value = keyValuePair.Value;
				if (value.FormationOverride != null)
				{
					value.Position = value.FormationOverride.Position;
				}
				else if ((value.Class & UnitClass.Small) == UnitClass.Small)
				{
					this.SetTrackerRowPosition(value, startPosition, orientation, ref smallOddRowGap, ref smallEvenRowGap, smallUnitTypes);
					smallUnitTypes++;
				}
				else if ((value.Class & UnitClass.Medium) == UnitClass.Medium)
				{
					this.SetTrackerRowPosition(value, initialSpawnPoint, orientation, ref medOddRowGap, ref medEvenRowGap, medUnitTypes);
					medUnitTypes++;
				}
				else
				{
					value.Position = vector2r;
					vector2r += orientation.Back * value.Pattern.Height(value.TotalUnits) * this.kGapScale;
				}
			}
		}

		private void SetTrackerRowPosition(Sim.FormationTracker tracker, Vector2r startPosition, Orientation2 orientation, ref Fixed64 oddSide, ref Fixed64 evenSide, int count)
		{
			Fixed64 @fixed = tracker.Pattern.Width(tracker.TotalUnits) * Fixed64.Half;
			if (count == 0)
			{
				tracker.Position = startPosition;
				evenSide = @fixed + this.kFormationGap;
				oddSide = @fixed + this.kFormationGap;
				return;
			}
			Vector2r right = orientation.Right;
			if (count % 2 == 0)
			{
				evenSide += @fixed;
				tracker.Position = startPosition - right * evenSide;
				evenSide += @fixed + this.kFormationGap;
				return;
			}
			oddSide += @fixed;
			tracker.Position = startPosition + right * oddSide;
			oddSide += @fixed + this.kFormationGap;
		}

		private void SpawnGroundUnits(Dictionary<string, Sim.FormationTracker> formationTrackers, List<StartingUnit> groundUnits, Commander commander, bool hasCarrierHangar, UnitHangar hangar)
		{
			string[] array = new string[1];
			foreach (StartingUnit startingUnit in groundUnits)
			{
				array[0] = startingUnit.Tag;
				Sim.FormationTracker formationTracker = formationTrackers[startingUnit.Unit];
				Vector2r vector2r = formationTracker.Pattern.Position(formationTracker.Position, formationTracker.Orientation, formationTracker.CurrentCount);
				Entity entity;
				if (hasCarrierHangar && formationTracker.StartsInHangar && !commander.CommanderAttributes.BeginDeployed)
				{
					UntrackedUnitSpawnMode untrackedSpawnMode = commander.CommanderAttributes.WaitForDeployCommand ? UntrackedUnitSpawnMode.Cache : UntrackedUnitSpawnMode.Deploy;
					if (!this.UnitManager.SpawnNewUntrackedUnitFromHangar(startingUnit.Unit, hangar, commander.ID, untrackedSpawnMode, out entity, array))
					{
						Log.Error(Log.Channel.Gameplay, "Commander manifest tried to deploy an untracked unit ({0}) from a hangar ({1}), but failed!", new object[]
						{
							startingUnit.Unit,
							hangar.Owner.ToFriendlyString()
						});
						continue;
					}
					EntityGoalsCollection.AddGoal(entity, new MoveToGoal(vector2r, formationTracker.Orientation));
				}
				else if (!this.UnitManager.SpawnNewUnitIntoWorld(startingUnit.Unit, commander.ID, vector2r, formationTracker.Orientation, out entity, array))
				{
					Log.Error(Log.Channel.Gameplay, "Commander manifest tried to spawn a unit ({0}) into the world, but failed!", new object[]
					{
						startingUnit.Unit
					});
					continue;
				}
				if (startingUnit.OnSpawnedInSim != null)
				{
					startingUnit.OnSpawnedInSim(entity);
				}
				formationTracker.CurrentCount++;
			}
		}

		private void SpawnAirUnits(CommanderID commanderID, List<StartingUnit> airUnits, bool hasCarrierHangar, UnitHangar hangar, Vector2r initialSpawnPoint, Orientation2 orientation)
		{
			Fixed64 @fixed = Fixed64.Zero;
			foreach (StartingUnit startingUnit in airUnits)
			{
				Entity obj;
				if (!hasCarrierHangar || !this.UnitManager.SpawnNewTrackedUnitIntoHangar(startingUnit.Unit, hangar, commanderID, true, out obj, null))
				{
					Fixed64 width = Sim.GetBuffedEntityTypeAttributes<UnitMovementAttributes>(startingUnit.Unit, commanderID).Dynamics.Width;
					Vector2r right = orientation.Right;
					right.ScaleBy(@fixed);
					Vector2r spawnPoint = initialSpawnPoint + right;
					@fixed += width;
					if (!this.UnitManager.SpawnNewUnitIntoWorld(startingUnit.Unit, commanderID, spawnPoint, orientation, out obj, new string[]
					{
						startingUnit.Tag
					}))
					{
						Log.Error(Log.Channel.Gameplay, "Commander manifest tried to spawn an undockable air unit ({0}) into the world, but failed!", new object[]
						{
							startingUnit.Unit
						});
						continue;
					}
				}
				if (startingUnit.OnSpawnedInSim != null)
				{
					startingUnit.OnSpawnedInSim(obj);
				}
			}
		}

		private void SpawnInitialUnitsForCommander(Commander commander, SimSettings simSettings, SimMapDependencies mapDependencies)
		{
			if (commander != null)
			{
				if (commander.CommanderAttributes.StartingUnits.IsNullOrEmpty<StartingUnit>())
				{
					return;
				}
				Vector2r initialSpawnPoint;
				Orientation2 orientation;
				this.GetCommanderSpawnPoint(simSettings, mapDependencies, commander.ID, out initialSpawnPoint, out orientation);
				List<StartingUnit> carrierUnits;
				List<StartingUnit> groundUnits;
				List<StartingUnit> airUnits;
				Dictionary<string, Sim.FormationTracker> formationTrackers;
				this.SplitUnitTypesAndPrepareFormationTrackers(commander, orientation, out carrierUnits, out groundUnits, out airUnits, out formationTrackers);
				Vector2r maxSmlDimensions;
				Vector2r maxMedDimensions;
				this.GetMaxClassPatternDimensions(formationTrackers, out maxSmlDimensions, out maxMedDimensions);
				int smallUnitTypes = 0;
				int medUnitTypes = 0;
				Fixed64 zero = Fixed64.Zero;
				Fixed64 zero2 = Fixed64.Zero;
				Fixed64 medOddRowGap;
				Fixed64 medEvenRowGap;
				UnitHangar hangar;
				bool hasCarrierHangar;
				this.SpawnCarriersInMediumUnitRow(commander.ID, formationTrackers, carrierUnits, out medOddRowGap, out medEvenRowGap, initialSpawnPoint, orientation, ref medUnitTypes, out hangar, out hasCarrierHangar);
				this.InitializeTrackerPositions(formationTrackers, initialSpawnPoint, orientation, medOddRowGap, medEvenRowGap, zero, zero2, maxSmlDimensions, maxMedDimensions, smallUnitTypes, medUnitTypes);
				this.SpawnGroundUnits(formationTrackers, groundUnits, commander, hasCarrierHangar, hangar);
				this.SpawnAirUnits(commander.ID, airUnits, hasCarrierHangar, hangar, initialSpawnPoint, orientation);
				List<Entity> list = new List<Entity>();
				foreach (Unit unit in this.UnitManager.GetUnitsForCommander(commander.ID))
				{
					list.Add(unit.Entity);
				}
				Sim.PostEvent(new FleetSpawnEvent(commander.ID, list.ToArray()));
			}
		}

		internal static void PostEvent(EventBase evt)
		{
			if (evt is IPostToSimulation)
			{
				Sim.Instance.SimEventSystem.Post(evt);
			}
			if (evt is IPostToPresentation)
			{
				Sim.Instance.SimToPresentationEventSystem.Post(evt);
			}
		}

		internal static void PostAnalyticsEvent(GameAnalyticsEventBase analyticsEvent)
		{
			IAnalyticsService analyticsService = Sim.Instance.mAnalyticsService;
			if (analyticsService != null)
			{
				Sim.Instance.mAnalyticsService.Post(analyticsEvent, (int)Sim.Instance.GlobalFrameCount.FrameNumber);
			}
		}

		private const bool kSilentEventsOnCommanderInitialization = true;

		public static Sim Instance;

		private IAnalyticsService mAnalyticsService;

		private DependencyContainerBase mSimStartupDependencies;

		private Sim.InitializationState mSimInitializationState;

		private readonly Fixed64 kGapScale = Fixed64.FromConstFloat(1.5f);

		private readonly Fixed64 kFormationGap = Fixed64.OneHundred;

		private enum InitializationState
		{
			Uninitialized,
			Constructed,
			LoadingMap,
			MapLoaded,
			Initialized
		}

		private class FormationTracker
		{
			public FormationTracker()
			{
			}

			public FormationPatternBase Pattern;

			public bool StartsInHangar;

			public int CurrentCount;

			public int TotalUnits;

			public int MaxRankCount;

			public Fixed64 MemberGap;

			public Fixed64 CapsuleWidth;

			public Fixed64 CapsuleHeight;

			public Vector2r Position;

			public Orientation2 Orientation;

			public UnitClass Class;

			public SpawnFormation FormationOverride;
		}
	}
}
