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
	// Token: 0x020003C1 RID: 961
	public class Sim
	{
		// Token: 0x170003AD RID: 941
		// (get) Token: 0x0600135F RID: 4959 RVA: 0x0006ABF4 File Offset: 0x00068DF4
		// (set) Token: 0x06001360 RID: 4960 RVA: 0x0006ABFC File Offset: 0x00068DFC
		internal SimSettings Settings { get; private set; }

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06001361 RID: 4961 RVA: 0x0006AC05 File Offset: 0x00068E05
		// (set) Token: 0x06001362 RID: 4962 RVA: 0x0006AC0D File Offset: 0x00068E0D
		internal IEntitySystem EntitySystem { get; private set; }

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001363 RID: 4963 RVA: 0x0006AC16 File Offset: 0x00068E16
		// (set) Token: 0x06001364 RID: 4964 RVA: 0x0006AC1E File Offset: 0x00068E1E
		internal SimMap Map { get; private set; }

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001365 RID: 4965 RVA: 0x0006AC27 File Offset: 0x00068E27
		// (set) Token: 0x06001366 RID: 4966 RVA: 0x0006AC2F File Offset: 0x00068E2F
		internal IEventSystem SimEventSystem { get; private set; }

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06001367 RID: 4967 RVA: 0x0006AC38 File Offset: 0x00068E38
		// (set) Token: 0x06001368 RID: 4968 RVA: 0x0006AC40 File Offset: 0x00068E40
		internal IAsynchronousEventSystem SimToPresentationEventSystem { get; set; }

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001369 RID: 4969 RVA: 0x0006AC49 File Offset: 0x00068E49
		// (set) Token: 0x0600136A RID: 4970 RVA: 0x0006AC51 File Offset: 0x00068E51
		internal UnitManager UnitManager { get; private set; }

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x0600136B RID: 4971 RVA: 0x0006AC5A File Offset: 0x00068E5A
		// (set) Token: 0x0600136C RID: 4972 RVA: 0x0006AC62 File Offset: 0x00068E62
		internal CommanderManager CommanderManager { get; private set; }

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x0600136D RID: 4973 RVA: 0x0006AC6B File Offset: 0x00068E6B
		// (set) Token: 0x0600136E RID: 4974 RVA: 0x0006AC73 File Offset: 0x00068E73
		internal ICommanderInteractionProvider InteractionProvider { get; private set; }

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x0600136F RID: 4975 RVA: 0x0006AC7C File Offset: 0x00068E7C
		// (set) Token: 0x06001370 RID: 4976 RVA: 0x0006AC84 File Offset: 0x00068E84
		internal SelectedUnitManager SelectedUnits { get; private set; }

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x0006AC8D File Offset: 0x00068E8D
		// (set) Token: 0x06001372 RID: 4978 RVA: 0x0006AC95 File Offset: 0x00068E95
		internal AIManager AIManager { get; private set; }

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x0006AC9E File Offset: 0x00068E9E
		// (set) Token: 0x06001374 RID: 4980 RVA: 0x0006ACA6 File Offset: 0x00068EA6
		internal SimFrameNumber GlobalFrameCount { get; set; }

		// Token: 0x06001375 RID: 4981 RVA: 0x0006ACB0 File Offset: 0x00068EB0
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
			Maneuver.InitializeManeuverPool();
			this.UnitManager = new UnitManager(this);
			this.SelectedUnits = new SelectedUnitManager();
			startupDependencies.Get<IAnalyticsService>(out this.mAnalyticsService);
			DynamicSceneSpawner.Initialize();
			this.mSimInitializationState = Sim.InitializationState.Constructed;
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x0006AE20 File Offset: 0x00069020
		internal Checksum Tick(SimFrameNumber frameNumber)
		{
			Checksum checksum = new Checksum();
			this.UnitManager.Tick(checksum, frameNumber);
			this.CommanderManager.Tick(checksum);
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

		// Token: 0x06001377 RID: 4983 RVA: 0x0006AEAF File Offset: 0x000690AF
		internal void ShutdownGame()
		{
			if (this.Settings.GameMode != null)
			{
				this.Settings.GameMode.Shutdown();
				this.Settings.GameMode = null;
			}
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x0006AEDC File Offset: 0x000690DC
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
			Maneuver.ShutdownManeuverPool();
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

		// Token: 0x06001379 RID: 4985 RVA: 0x0006AFD0 File Offset: 0x000691D0
		private void OnEntityTypeAttributesSaved(Entity entity, ref EntityTypeAttributesSaveState state)
		{
			CommanderID entityCommanderID = Sim.GetEntityCommanderID(entity);
			state.IsCommanderTypeBuffed = (entityCommanderID != CommanderID.None);
			state.EntityTypeCommanderOwner = entityCommanderID.ID;
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0006B004 File Offset: 0x00069204
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

		// Token: 0x0600137B RID: 4987 RVA: 0x0006B264 File Offset: 0x00069464
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
				Resource component = entity.GetComponent(11);
				Position component2 = entity.GetComponent(10);
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
				WreckArtifact component3 = entity2.GetComponent(35);
				Position component4 = entity2.GetComponent(10);
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
					Collectible component5 = entity2.GetComponent(36);
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

		// Token: 0x0600137C RID: 4988 RVA: 0x0006B8D4 File Offset: 0x00069AD4
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
					Storage component = unit.Entity.GetComponent(18);
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
						Experience component2 = unit.Entity.GetComponent(39);
						if (component2 != null)
						{
							dictionary3.Add(unit.Entity, component2);
						}
						if ((unit.UnitClass & UnitClass.Carrier) == UnitClass.Carrier)
						{
							PowerShunt component3 = unit.Entity.GetComponent(22);
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
							Health component4 = unit.Entity.GetComponent(7);
							if (component4 != null)
							{
								dictionary.Add(unit.Entity, component4);
							}
						}
						if (unit.Entity.HasComponent(15))
						{
							Production component5 = unit.Entity.GetComponent(15);
							for (int i = 0; i < component5.NumQueues; i++)
							{
								IList<Production.ProductionQueueEntry> queue = component5.GetQueue(i);
								foreach (Production.ProductionQueueEntry productionQueueEntry in queue)
								{
									commanderFromID.Storage.RefundBill(productionQueueEntry.OriginalResourceBill, Fixed64.One);
								}
							}
						}
						Position component6 = unit.Entity.GetComponent(10);
						Health component7 = unit.Entity.GetComponent(7);
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
						Resource component8 = entity.GetComponent(11);
						Position component9 = entity.GetComponent(10);
						Detectable component10 = entity.GetComponent(21);
						Tags component11 = entity.GetComponent(1);
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
								Wreck component12 = entity2.GetComponent(37);
								Resource component13 = entity2.GetComponent(11);
								Position component14 = entity2.GetComponent(10);
								Detectable component15 = entity2.GetComponent(21);
								Shape component16 = entity2.GetComponent(33);
								Tags component17 = entity2.GetComponent(1);
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
				Collectible component18 = entity3.GetComponent(36);
				WreckArtifact component19 = entity3.GetComponent(35);
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

		// Token: 0x0600137D RID: 4989 RVA: 0x0006C274 File Offset: 0x0006A474
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
			this.mSimInitializationState = Sim.InitializationState.Initialized;
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x0006C824 File Offset: 0x0006AA24
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
			}
			yield break;
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x0006C840 File Offset: 0x0006AA40
		internal static CommanderID GetEntityCommanderID(Entity entity)
		{
			if (!entity.IsValid())
			{
				return CommanderID.None;
			}
			OwningCommander component = entity.GetComponent(5);
			if (component != null)
			{
				return component.ID;
			}
			return CommanderID.None;
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x0006C874 File Offset: 0x0006AA74
		internal static bool EntityShouldMaintainAggroThroughFOW(Entity entity)
		{
			CommanderID entityCommanderID = Sim.GetEntityCommanderID(entity);
			Commander commanderFromID = Sim.Instance.CommanderManager.GetCommanderFromID(entityCommanderID);
			return commanderFromID != null && commanderFromID.CommanderAttributes != null && commanderFromID.CommanderAttributes.MaintainAggroThroughFOW;
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x0006C8B4 File Offset: 0x0006AAB4
		internal static bool IsValidEntityType(string typeID)
		{
			if (!string.IsNullOrEmpty(typeID))
			{
				EntityTypeAttributes entityType = Sim.Instance.Settings.EntityTypes.GetEntityType(typeID);
				return entityType != null;
			}
			return false;
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x0006C8E8 File Offset: 0x0006AAE8
		internal static bool IsEntityDead(Entity entity)
		{
			return !entity.IsValid() || (entity.HasComponent(9) && !entity.GetComponent(9).WaitingForOnDeathActivity);
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x0006C910 File Offset: 0x0006AB10
		internal static bool IsEntityDocked(Entity entity)
		{
			return entity.IsValid() && entity.HasComponent(2) && !entity.HasComponent(10);
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x0006C932 File Offset: 0x0006AB32
		internal static Entity CreateEmptyEntity()
		{
			return Sim.CreateCommanderlessEntity(string.Empty);
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x0006C940 File Offset: 0x0006AB40
		internal static Entity CreateCommanderlessEntity(string typeID)
		{
			return Sim.Instance.EntitySystem.CreateTypedEntity(typeID);
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x0006C960 File Offset: 0x0006AB60
		internal static Entity CreateEntity(string typeID, CommanderID commanderID)
		{
			Entity entity = Sim.Instance.EntitySystem.CreateTypedEntity(typeID, commanderID.ID);
			entity.AddComponent(5, OwningCommander.Create(commanderID));
			return entity;
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x0006C993 File Offset: 0x0006AB93
		internal static void AddEntityTypeBuffs(string unitType, bool useAsPrefix, UnitClass unitClass, FlagOperator classOperator, AttributeBuffSet buffSet, CommanderID commanderID)
		{
			Sim.AddEntityTypeBuffs(unitType, useAsPrefix, unitClass, classOperator, buffSet, commanderID, false);
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x0006C9A4 File Offset: 0x0006ABA4
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

		// Token: 0x06001389 RID: 5001 RVA: 0x0006CB10 File Offset: 0x0006AD10
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

		// Token: 0x0600138A RID: 5002 RVA: 0x0006CC4C File Offset: 0x0006AE4C
		internal static T GetEntityTypeAttributes<T>(string typeID) where T : class
		{
			EntityTypeAttributes entityType = Sim.Instance.Settings.EntityTypes.GetEntityType(typeID);
			if (entityType != null)
			{
				return entityType.Get<T>();
			}
			return default(T);
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0006CC84 File Offset: 0x0006AE84
		internal static T GetBuffedEntityTypeAttributes<T>(string typeID, CommanderID commanderID) where T : class
		{
			EntityTypeAttributes commanderSpecificEntityType = Sim.Instance.Settings.EntityTypes.GetCommanderSpecificEntityType(typeID, commanderID.ID);
			if (commanderSpecificEntityType != null)
			{
				return commanderSpecificEntityType.Get<T>();
			}
			return default(T);
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x0006CCC1 File Offset: 0x0006AEC1
		internal static void DestroyEntity(Entity entity)
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

		// Token: 0x0600138D RID: 5005 RVA: 0x0006CCFA File Offset: 0x0006AEFA
		internal static int Rand()
		{
			return Sim.Instance.Settings.RandomNumberGenerator.Next();
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x0006CD10 File Offset: 0x0006AF10
		internal static int Rand(int max)
		{
			return Sim.Instance.Settings.RandomNumberGenerator.Next(max);
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x0006CD27 File Offset: 0x0006AF27
		internal static int RandRange(int min, int max)
		{
			return Sim.Instance.Settings.RandomNumberGenerator.Next(min, max);
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x0006CD3F File Offset: 0x0006AF3F
		internal static Fixed64 Rand01()
		{
			return Sim.Instance.Settings.RandomNumberGenerator.NextFixed64();
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x0006CD55 File Offset: 0x0006AF55
		internal static Fixed64 Rand(Fixed64 max)
		{
			return Sim.Instance.Settings.RandomNumberGenerator.Next(max);
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x0006CD6C File Offset: 0x0006AF6C
		internal static Fixed64 RandRange(Fixed64 min, Fixed64 max)
		{
			return Sim.Instance.Settings.RandomNumberGenerator.Next(min, max);
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x0006CD84 File Offset: 0x0006AF84
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

		// Token: 0x06001394 RID: 5012 RVA: 0x0006CE64 File Offset: 0x0006B064
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

		// Token: 0x06001395 RID: 5013 RVA: 0x0006CF14 File Offset: 0x0006B114
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

		// Token: 0x06001396 RID: 5014 RVA: 0x0006CF64 File Offset: 0x0006B164
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

		// Token: 0x06001397 RID: 5015 RVA: 0x0006D0B0 File Offset: 0x0006B2B0
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

		// Token: 0x06001398 RID: 5016 RVA: 0x0006D2A0 File Offset: 0x0006B4A0
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

		// Token: 0x06001399 RID: 5017 RVA: 0x0006D44C File Offset: 0x0006B64C
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
						hangar = entity.GetComponent(23);
						if (hangar != null)
						{
							hasCarrierHangar = true;
						}
					}
					formationTracker.CurrentCount++;
				}
			}
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x0006D60C File Offset: 0x0006B80C
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

		// Token: 0x0600139B RID: 5019 RVA: 0x0006D758 File Offset: 0x0006B958
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

		// Token: 0x0600139C RID: 5020 RVA: 0x0006D858 File Offset: 0x0006BA58
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

		// Token: 0x0600139D RID: 5021 RVA: 0x0006D9FC File Offset: 0x0006BBFC
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

		// Token: 0x0600139E RID: 5022 RVA: 0x0006DB14 File Offset: 0x0006BD14
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

		// Token: 0x0600139F RID: 5023 RVA: 0x0006DC4C File Offset: 0x0006BE4C
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

		// Token: 0x060013A0 RID: 5024 RVA: 0x0006DC80 File Offset: 0x0006BE80
		internal static void PostAnalyticsEvent(GameAnalyticsEventBase analyticsEvent)
		{
			IAnalyticsService analyticsService = Sim.Instance.mAnalyticsService;
			if (analyticsService != null)
			{
				Sim.Instance.mAnalyticsService.Post(analyticsEvent, (int)Sim.Instance.GlobalFrameCount.FrameNumber);
			}
		}

		// Token: 0x04000FF0 RID: 4080
		private const bool kSilentEventsOnCommanderInitialization = true;

		// Token: 0x04000FF1 RID: 4081
		internal static Sim Instance;

		// Token: 0x04000FF2 RID: 4082
		private IAnalyticsService mAnalyticsService;

		// Token: 0x04000FF3 RID: 4083
		private DependencyContainerBase mSimStartupDependencies;

		// Token: 0x04000FF4 RID: 4084
		private Sim.InitializationState mSimInitializationState;

		// Token: 0x04000FF5 RID: 4085
		private readonly Fixed64 kGapScale = Fixed64.FromConstFloat(1.5f);

		// Token: 0x04000FF6 RID: 4086
		private readonly Fixed64 kFormationGap = Fixed64.OneHundred;

		// Token: 0x020003C2 RID: 962
		private enum InitializationState
		{
			// Token: 0x04001003 RID: 4099
			Uninitialized,
			// Token: 0x04001004 RID: 4100
			Constructed,
			// Token: 0x04001005 RID: 4101
			LoadingMap,
			// Token: 0x04001006 RID: 4102
			MapLoaded,
			// Token: 0x04001007 RID: 4103
			Initialized
		}

		// Token: 0x020003C3 RID: 963
		private class FormationTracker
		{
			// Token: 0x060013A1 RID: 5025 RVA: 0x0006DCBA File Offset: 0x0006BEBA
			public FormationTracker()
			{
			}

			// Token: 0x04001008 RID: 4104
			public FormationPatternBase Pattern;

			// Token: 0x04001009 RID: 4105
			public bool StartsInHangar;

			// Token: 0x0400100A RID: 4106
			public int CurrentCount;

			// Token: 0x0400100B RID: 4107
			public int TotalUnits;

			// Token: 0x0400100C RID: 4108
			public int MaxRankCount;

			// Token: 0x0400100D RID: 4109
			public Fixed64 MemberGap;

			// Token: 0x0400100E RID: 4110
			public Fixed64 CapsuleWidth;

			// Token: 0x0400100F RID: 4111
			public Fixed64 CapsuleHeight;

			// Token: 0x04001010 RID: 4112
			public Vector2r Position;

			// Token: 0x04001011 RID: 4113
			public Orientation2 Orientation;

			// Token: 0x04001012 RID: 4114
			public UnitClass Class;

			// Token: 0x04001013 RID: 4115
			public SpawnFormation FormationOverride;
		}
	}
}
