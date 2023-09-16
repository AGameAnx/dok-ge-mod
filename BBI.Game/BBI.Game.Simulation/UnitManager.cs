using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Events;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Commands;
using BBI.Game.Data;
using BBI.Game.Events;
using BBI.Game.SaveLoad;

namespace BBI.Game.Simulation
{
	public sealed class UnitManager
	{
		internal HashSet<Entity> DeployingUnits
		{
			get
			{
				return this.mDeployingUnits;
			}
		}

		public static Fixed64 sMaxUnitDiagonal { get; private set; }

		public UnitManager(Sim sim)
		{
			this.Sim = sim;
			UnitManager.sMaxUnitDiagonal = Fixed64.Zero;
			Sim.Instance.SimEventSystem.AddHandler<BuffChangedEvent>(new BBI.Core.Events.EventHandler<BuffChangedEvent>(this.OnBuffChangedEvent));
			Sim.Instance.SimEventSystem.AddHandler<DockingUnitOperationEvent>(new BBI.Core.Events.EventHandler<DockingUnitOperationEvent>(this.OnDockingMessage));
			Sim.Instance.SimEventSystem.AddHandler<HangarDespawnUnitEvent>(new BBI.Core.Events.EventHandler<HangarDespawnUnitEvent>(this.OnHangarDespawnUnitMessage));
			Sim.Instance.SimEventSystem.AddHandler<ModifyAbilityListEvent>(new BBI.Core.Events.EventHandler<ModifyAbilityListEvent>(this.OnModifyAbilityListEvent));
			Sim.Instance.SimEventSystem.AddHandler<UnitDestroyedEvent>(new BBI.Core.Events.EventHandler<UnitDestroyedEvent>(this.OnUnitDestroyed));
		}

		internal void Shutdown()
		{
			this.mUnits.Clear();
			Sim.Instance.SimEventSystem.RemoveHandler<BuffChangedEvent>(new BBI.Core.Events.EventHandler<BuffChangedEvent>(this.OnBuffChangedEvent));
			Sim.Instance.SimEventSystem.RemoveHandler<DockingUnitOperationEvent>(new BBI.Core.Events.EventHandler<DockingUnitOperationEvent>(this.OnDockingMessage));
			Sim.Instance.SimEventSystem.RemoveHandler<HangarDespawnUnitEvent>(new BBI.Core.Events.EventHandler<HangarDespawnUnitEvent>(this.OnHangarDespawnUnitMessage));
			Sim.Instance.SimEventSystem.RemoveHandler<ModifyAbilityListEvent>(new BBI.Core.Events.EventHandler<ModifyAbilityListEvent>(this.OnModifyAbilityListEvent));
			Sim.Instance.SimEventSystem.RemoveHandler<UnitDestroyedEvent>(new BBI.Core.Events.EventHandler<UnitDestroyedEvent>(this.OnUnitDestroyed));
		}

		internal static void TransferUnitToCommander(Unit unitToTransfer, CommanderID newCommanderID)
		{
			if (unitToTransfer == null || !unitToTransfer.Entity.IsValid())
			{
				Log.Warn(Log.Channel.Core, "Trying to transfer an invalid unit/entity to another commander. Ignoring.", new object[0]);
				return;
			}
			OwningCommander component = unitToTransfer.Entity.GetComponent<OwningCommander>(5);
			if (component != null)
			{
				if (component.ID != newCommanderID)
				{
					UnitManager.DeactivatePassiveAbilities(unitToTransfer);
					CommanderID id = component.ID;
					component.SetNewCommanderID(newCommanderID);
					EntityTypeAttributes typeAttributes = unitToTransfer.Entity.GetTypeAttributes();
					int flags = typeAttributes.Flags;
					string typeName = unitToTransfer.TypeName;
					IDBuffSaveState[] array = null;
					if (typeAttributes.IsInstancedBuffed())
					{
						array = typeAttributes.GetAllEntityTypeBuffs();
					}
					EntityTypeAttributes commanderSpecificEntityType = Sim.Instance.Settings.EntityTypes.GetCommanderSpecificEntityType(typeName, newCommanderID.ID);
					unitToTransfer.Entity.RemoveComponent<EntityTypeAttributes>(0);
					unitToTransfer.Entity.AddComponent<EntityTypeAttributes>(0, commanderSpecificEntityType);
					int flags2 = commanderSpecificEntityType.Flags;
					Sim.PostEvent(new BuffChangedEvent(null, unitToTransfer.Entity, BuffChangedEvent.BuffChangeType.CommanderChanged, flags, flags2));
					if (array != null)
					{
						unitToTransfer.Entity.AddBuffsFromBuffStates(array);
					}
					Storage component2 = unitToTransfer.Entity.GetComponent<Storage>(18);
					if (component2 != null && component2.IsLinkedToCommanderBank)
					{
						component2.CreateCommanderInventoryLinks(newCommanderID);
					}
					AbilityHelper.UpdateAbilityAvailabilityOnCommanderTransfer(unitToTransfer.Entity, newCommanderID);
					UnitManager.ActivatePassiveAbilities(unitToTransfer);
					Experience component3 = unitToTransfer.Entity.GetComponent<Experience>(39);
					if (component3 != null)
					{
						bool flag = Experience.ShouldHideName(typeName, newCommanderID);
						if (flag != component3.HideName)
						{
							component3.HideName = flag;
							component3.AssignName();
						}
					}
					CommanderRelationship relationship = Sim.Instance.InteractionProvider.GetRelationship(newCommanderID, SimController.LocalPlayerCommanderID);
					Sim.PostEvent(new UnitTransferredEvent(unitToTransfer.Entity, id, newCommanderID, relationship));
					return;
				}
			}
			else
			{
				Log.Error(Log.Channel.Core, "A unit has no commander associated to it!", new object[0]);
			}
		}

		private static void PostUnitRemovedEvent(Entity removedUnitEntityID, SimFrameNumber frameNumber)
		{
			Unit component = removedUnitEntityID.GetComponent<Unit>(2);
			if (component != null)
			{
				UnitRemoveReason reason = UnitRemoveReason.Despawn;
				Sim.PostEvent(new UnitRemovedFeedbackEvent(removedUnitEntityID, false, Sim.GetEntityCommanderID(component.Entity), frameNumber, true, reason));
			}
		}

		private static void PostUnitDiedEvent(Entity dyingUnitEntityID, bool skipDeathSequence, SimFrameNumber frameNumber)
		{
			Unit component = dyingUnitEntityID.GetComponent<Unit>(2);
			if (component != null)
			{
				UnitRemoveReason reason = UnitRemoveReason.Destroy;
				Sim.PostEvent(new UnitRemovedFeedbackEvent(dyingUnitEntityID, skipDeathSequence, Sim.GetEntityCommanderID(component.Entity), frameNumber, true, reason));
			}
		}

		private static void PostUnitDockedEvent(Entity dockingUnitEntityID, SimFrameNumber frameNumber)
		{
			Unit component = dockingUnitEntityID.GetComponent<Unit>(2);
			if (component != null)
			{
				UnitRemoveReason reason = UnitRemoveReason.UnitDock;
				Sim.PostEvent(new UnitRemovedFeedbackEvent(dockingUnitEntityID, false, Sim.GetEntityCommanderID(component.Entity), frameNumber, true, reason));
			}
		}

		private void OnUnitDestroyed(UnitDestroyedEvent ev)
		{
			if (ev.KillingEntity.IsValid() && ev.KillingEntity.HasComponent(39))
			{
				Experience component = ev.KillingEntity.GetComponent<Experience>(39);
				component.OnUnitDestroyed(ev);
			}
		}

		private void OnHangarDespawnUnitMessage(HangarDespawnUnitEvent ce)
		{
			if (!ce.DespawnUnit.IsValid())
			{
				Log.Error(Log.Channel.Core, "jbax [hangar] OnHangarDespawnUnitMessage failed to find valid entity", new object[0]);
				return;
			}
			Unit component = ce.DespawnUnit.GetComponent<Unit>(2);
			if (component != null)
			{
				this.mUnitsReadyDelayedDocking.Add(component);
				return;
			}
			Log.Error(Log.Channel.Core, "jbax [hangar] OnHangarDespawnUnitMessage failed to find valid unit from ebt {0} ", new object[]
			{
				ce.DespawnUnit
			});
		}

		private void OnDockingMessage(DockingUnitOperationEvent ce)
		{
			Entity dockingEntity = ce.DockingEntity;
			if (dockingEntity.IsValid())
			{
				Unit component = dockingEntity.GetComponent<Unit>(2);
				if (component != null)
				{
					switch (ce.Action)
					{
					case HangarOperationTriggers.DockingUnitTrigger.SetEntityAsPresentationOnly:
						component.SetPresentationOnlyAndSuspend(true);
						return;
					case HangarOperationTriggers.DockingUnitTrigger.SetEntityAsSimEntity:
						if (this.mDeployingUnits.Remove(dockingEntity))
						{
							Sim.PostEvent(new SimUnitCreatedEvent(dockingEntity, SimUnitCreatedEvent.SimUnitReadyState.DeployedFromHangar));
						}
						component.SetPresentationOnlyAndSuspend(false);
						return;
					case HangarOperationTriggers.DockingUnitTrigger.SetEntityLandingMode:
						component.SetPresentationOnlyAndSuspend(true);
						return;
					case HangarOperationTriggers.DockingUnitTrigger.SetGroundReceivingMode:
						component.SetPresentationOnlyAndSuspend(true);
						return;
					default:
						Log.Error(Log.Channel.Core, "jbax[hangar] OnDockingMessage unknown action detected!", new object[0]);
						return;
					}
				}
			}
			else
			{
				Log.Error(Log.Channel.Core, "jbax[hangar] OnDockingMessage invalid docking entity detected!", new object[0]);
			}
		}

		private void OnBuffChangedEvent(BuffChangedEvent ev)
		{
			foreach (Unit unit in this.mUnits)
			{
				if (ev.IsForEntity(unit.Entity))
				{
					unit.OnBuffChangedEvent(ev);
				}
			}
		}

		private void OnModifyAbilityListEvent(ModifyAbilityListEvent ev)
		{
			if (ev.TargetType == ModifyAbilityListTargetType.UnitTypeForCommander || ev.TargetType == ModifyAbilityListTargetType.Commander)
			{
				Commander commanderFromID = Sim.Instance.CommanderManager.GetCommanderFromID(ev.CommanderID);
				foreach (Unit unit in this.GetUnitsForCommander(ev.CommanderID))
				{
					if (ev.TargetType == ModifyAbilityListTargetType.Commander || unit.TypeName == ev.UnitTypeName)
					{
						switch (ev.Action)
						{
						case ModifyAbilityListAction.Add:
							if (ev.TargetType == ModifyAbilityListTargetType.UnitTypeForCommander || !AbilityHelper.ShouldAbilityBeRemovedForUnit(unit.Entity, ev.AbilityTypeName, commanderFromID))
							{
								AbilityHelper.AddAbilityToUnitInstance(unit.Entity, ev.AbilityTypeName, commanderFromID);
							}
							break;
						case ModifyAbilityListAction.Remove:
						{
							bool flag = ev.TargetType == ModifyAbilityListTargetType.UnitTypeForCommander || AbilityHelper.ShouldAbilityBeRemovedForUnit(unit.Entity, ev.AbilityTypeName, commanderFromID);
							if (flag)
							{
								AbilityHelper.RemoveAbilityFromUnitInstance(unit.Entity, ev.AbilityTypeName);
							}
							break;
						}
						case ModifyAbilityListAction.Unhide:
							if (ev.TargetType == ModifyAbilityListTargetType.UnitTypeForCommander || !AbilityHelper.ShouldAbilityBeHiddenForUnit(unit.Entity, ev.AbilityTypeName, commanderFromID))
							{
								AbilityHelper.UnhideAbilityOnUnitInstance(unit.Entity, ev.AbilityTypeName);
							}
							break;
						case ModifyAbilityListAction.Hide:
						{
							bool flag2 = ev.TargetType == ModifyAbilityListTargetType.UnitTypeForCommander || AbilityHelper.ShouldAbilityBeHiddenForUnit(unit.Entity, ev.AbilityTypeName, commanderFromID);
							if (flag2)
							{
								AbilityHelper.HideAbilityOnUnitInstance(unit.Entity, ev.AbilityTypeName);
							}
							break;
						}
						default:
							Log.Warn(Log.Channel.Gameplay, "Unhandled ModifyAbilityListAction {0}!", new object[]
							{
								ev.Action
							});
							break;
						}
					}
				}
			}
		}

		internal void Tick(Checksum check, SimFrameNumber frameNumber)
		{
			foreach (Unit unit in this.mUnitsReadyForSimRemoval)
			{
				this.RemoveUnitFromSim(unit, frameNumber, true);
			}
			this.mUnitsReadyForSimRemoval.Clear();
			foreach (Unit unit2 in this.mUnits)
			{
				unit2.Tick(check, SimController.kSimTickPeriodS);
			}
			foreach (KeyValuePair<Unit, bool> keyValuePair in this.mUnitsDestroyedThisFrame)
			{
				UnitManager.PostUnitDiedEvent(keyValuePair.Key.Entity, keyValuePair.Value, frameNumber);
			}
			this.mUnitsDestroyedThisFrame.Clear();
			foreach (Unit unit3 in this.mUnitsReadyDelayedDocking)
			{
				if (unit3 != null)
				{
					unit3.DockDespawn();
				}
			}
			this.mUnitsReadyDelayedDocking.Clear();
		}

		public IEnumerable<Unit> Units
		{
			get
			{
				return this.mUnits;
			}
		}

		internal IEnumerable<Unit> GetUnitsForCommander(CommanderID commanderID)
		{
			foreach (Unit unit in this.mUnits)
			{
				if (Sim.GetEntityCommanderID(unit.Entity) == commanderID)
				{
					yield return unit;
				}
			}
			yield break;
		}

		private void RemoveUnitFromSim(Unit unit, SimFrameNumber frame, bool removeFromUnitList)
		{
			UnitManager.PostUnitRemovedEvent(unit.Entity, frame);
			this.UnsubscribeToUnitEvents(unit);
			UnitManager.RemoveComponentsOnDespawnFromWorld(unit, false);
			Sim.DestroyEntity(unit.Entity);
			if (removeFromUnitList)
			{
				this.mUnits.Remove(unit);
			}
		}

		internal void RemoveAllUnits(SimFrameNumber frame)
		{
			UnitManager.sMaxUnitDiagonal = Fixed64.Zero;
			foreach (Unit unit in this.mUnits)
			{
				this.RemoveUnitFromSim(unit, frame, false);
			}
			this.mUnits.Clear();
		}

		internal void OnUnitsLoaded(IEnumerable<Entity> loadedEntities, Entity[] deployingUnits)
		{
			foreach (Entity entity in loadedEntities)
			{
				Unit component = entity.GetComponent<Unit>(2);
				if (component != null)
				{
					if (component.Diagonal > UnitManager.sMaxUnitDiagonal)
					{
						UnitManager.sMaxUnitDiagonal = component.Diagonal;
					}
					this.SubscribeToUnitEvents(component);
					this.mUnits.Add(component);
					if (entity.IsValid() && entity.HasComponent(10) && !entity.HasComponent(9))
					{
						Sim.PostEvent(new UnitViewableEvent(entity));
					}
				}
			}
			this.mDeployingUnits.Clear();
			this.mDeployingUnits.UnionWith(deployingUnits);
		}

		internal bool SpawnNewUntrackedUnitFromHangar(string unitType, UnitHangar hangar, CommanderID commanderID, UntrackedUnitSpawnMode untrackedSpawnMode, out Entity newEntity, string[] tags = null)
		{
			if (hangar == null || !hangar.StoresUntrackedUnitClass(unitType))
			{
				Log.Error(Log.Channel.Gameplay, "Tried to produce a unit into a hangar, but there is no hangar or it does not store the untracked unit class!", new object[0]);
				newEntity = Entity.None;
				return false;
			}
			newEntity = Sim.CreateEntity(unitType, commanderID);
			if (newEntity.IsValid())
			{
				Unit unit = Unit.CreateUnit(newEntity);
				this.mUnits.Add(unit);
				this.SubscribeToUnitEvents(unit);
				Tags.AddTagsToEntity(newEntity, tags);
				switch (untrackedSpawnMode)
				{
				case UntrackedUnitSpawnMode.Cache:
					hangar.CacheUntrackedUnit(newEntity, unitType);
					Sim.PostEvent(new SimUnitCreatedEvent(newEntity, SimUnitCreatedEvent.SimUnitReadyState.UntrackedInHangar));
					break;
				case UntrackedUnitSpawnMode.Deploy:
					hangar.DeployUntrackedUnit(newEntity, unitType);
					this.mDeployingUnits.Add(newEntity);
					break;
				default:
					Log.Error(Log.Channel.Core, "Unrecognized untracked spawn mode: {0}", new object[]
					{
						untrackedSpawnMode
					});
					break;
				}
				return true;
			}
			return false;
		}

		internal bool SpawnNewTrackedUnitIntoHangar(string typeToSpawn, UnitHangar hangar, CommanderID commanderID, bool reserveNewSlot, out Entity newEntity, string[] tags = null)
		{
			newEntity = Entity.None;
			if (hangar != null && (!reserveNewSlot || (hangar.FreeSlotsAvailableOfType(typeToSpawn) && hangar.ReserveSlotIfAvailable(typeToSpawn))))
			{
				newEntity = Sim.CreateEntity(typeToSpawn, commanderID);
				if (newEntity.IsValid())
				{
					Unit unit = Unit.CreateUnit(newEntity);
					this.mUnits.Add(unit);
					this.SubscribeToUnitEvents(unit);
					hangar.AttachEntityToReservedHangarSlot(newEntity, typeToSpawn, UnitHangar.HangarOccupancyState.OccupiedReady);
					Tags.AddTagsToEntity(newEntity, tags);
					Sim.PostEvent(new SimUnitCreatedEvent(newEntity, SimUnitCreatedEvent.SimUnitReadyState.TrackedInHangar));
					return true;
				}
			}
			return false;
		}

		internal bool SpawnNewUnitIntoWorld(string typeToSpawn, CommanderID commanderID, Vector2r spawnPoint, Orientation2 spawnOrientation, out Entity newEntity, string[] tags = null)
		{
			newEntity = Sim.CreateEntity(typeToSpawn, commanderID);
			if (newEntity.IsValid())
			{
				Unit unit = Unit.CreateUnit(newEntity);
				this.mUnits.Add(unit);
				this.SubscribeToUnitEvents(unit);
				if (unit.Diagonal > UnitManager.sMaxUnitDiagonal)
				{
					UnitManager.sMaxUnitDiagonal = unit.Diagonal;
				}
				UnitManager.AddComponentsOnSpawnIntoWorld(unit, spawnPoint, spawnOrientation);
				Tags.AddTagsToEntity(newEntity, tags);
				Sim.PostEvent(new SimUnitCreatedEvent(newEntity, SimUnitCreatedEvent.SimUnitReadyState.SpawnedInWorld));
				Sim.PostEvent(new UnitViewableEvent(newEntity));
				UnitManager.ActivateAbilitiesOnSpawnIntoWorld(unit);
				unit.HasBeenSpawnedIntoWorldBefore = true;
				return true;
			}
			return false;
		}

		internal static bool SpawnDockedUnitIntoWorld(Unit unit, Vector2r spawnPoint, Orientation2 spawnOrientation)
		{
			if (unit != null && unit.Entity.IsValid())
			{
				if (unit.Diagonal > UnitManager.sMaxUnitDiagonal)
				{
					UnitManager.sMaxUnitDiagonal = unit.Diagonal;
				}
				UnitManager.AddComponentsOnSpawnIntoWorld(unit, spawnPoint, spawnOrientation);
				Sim.PostEvent(new UnitViewableEvent(unit.Entity));
				UnitManager.ActivateAbilitiesOnSpawnIntoWorld(unit);
				unit.HasBeenSpawnedIntoWorldBefore = true;
				return true;
			}
			return false;
		}

		internal static void AddComponentsOnSpawnIntoWorld(Unit unit, Vector2r position, Orientation2 orientation)
		{
			if (unit != null && unit.Entity.IsValid())
			{
				Maneuvering component = Maneuvering.Create(unit.Entity, position, orientation, unit.Attributes.NavMeshAttributes);
				unit.Entity.AddComponent<Maneuvering>(12, component);
				if (unit.MovementAttributes != null && unit.MovementAttributes.Dynamics != null)
				{
					Fixed64 @fixed = unit.MovementAttributes.Dynamics.Width >> 1;
					Fixed64 capsuleAxisLength = unit.MovementAttributes.Dynamics.Length - (@fixed << 1);
					Shape component2 = Shape.Create(position, orientation, @fixed, capsuleAxisLength, unit.Attributes.BlocksLOF, false, unit.Attributes.WorldHeightOffset);
					unit.Entity.AddComponent<Shape>(33, component2);
				}
				Detectable component3 = unit.Entity.GetComponent<Detectable>(21);
				if (component3 != null)
				{
					foreach (CommanderID sensingCommander in Sim.Instance.CommanderManager.CommanderIDs)
					{
						component3.SetDetectionState(sensingCommander, DetectionState.Hidden);
					}
				}
				unit.Entity.AddComponent<Sensor>(20, Sensor.Create(unit.Attributes, unit.Entity));
				SimMap map = Sim.Instance.Map;
			}
		}

		internal static void ActivateAbilitiesOnSpawnIntoWorld(Unit unit)
		{
			UnitManager.ActivatePassiveAbilities(unit);
			if (!unit.HasBeenSpawnedIntoWorldBefore)
			{
				UnitManager.ActivateAutocastAbilities(unit);
			}
		}

		internal static void RemoveComponentsOnDespawnFromWorld(Unit unit, bool dockDespawn)
		{
			if (unit != null && unit.Entity.IsValid())
			{
				if (!dockDespawn)
				{
					unit.Entity.AddComponent<InstantDespawn>(42, default(InstantDespawn));
				}
				UnitManager.DeactivatePassiveAbilities(unit);
				Maneuvering.RemoveComponent(unit.Entity);
				unit.Entity.RemoveComponent<Position>(10);
				unit.Entity.RemoveComponent<Shape>(33);
				unit.Entity.RemoveComponent<Sensor>(20);
			}
		}

		private static void ActivateAutocastAbilities(Unit unit)
		{
			List<Ability> listComponent = unit.Entity.GetListComponent<Ability>(17);
			if (listComponent != null)
			{
				foreach (Ability ability in listComponent)
				{
					if (ability.AbilityAttributes.Autocast != null && ability.AbilityAttributes.Autocast.IsAutocastable && ability.AbilityAttributes.Autocast.AutocastEnabledOnSpawn)
					{
						ability.SetAutocastEnabled(true, true);
					}
				}
			}
		}

		private static void ActivatePassiveAbilities(Unit unit)
		{
			List<Ability> listComponent = unit.Entity.GetListComponent<Ability>(17);
			if (listComponent != null)
			{
				CommanderID entityCommanderID = Sim.GetEntityCommanderID(unit.Entity);
				foreach (Ability ability in listComponent)
				{
					if ((ability.AbilityAttributes.TargetingType & AbilityTargetingType.Passive) != AbilityTargetingType.None && !ability.PassiveActivated)
					{
						AbilityHelper.ActivatePassive(ability, unit.Entity, entityCommanderID);
					}
				}
			}
		}

		internal static void DeactivatePassiveAbilities(Unit unit)
		{
			List<Ability> listComponent = unit.Entity.GetListComponent<Ability>(17);
			if (listComponent != null)
			{
				CommanderID entityCommanderID = Sim.GetEntityCommanderID(unit.Entity);
				foreach (Ability ability in listComponent)
				{
					if ((ability.AbilityAttributes.TargetingType & AbilityTargetingType.Passive) != AbilityTargetingType.None && ability.PassiveActivated)
					{
						AbilityHelper.DeactivatePassive(ability, unit.Entity, entityCommanderID);
					}
				}
			}
		}

		private void SubscribeToUnitEvents(Unit unit)
		{
			if (unit != null)
			{
				unit.OnUnitDespawned += this.HandleUnitDespawned;
				unit.OnUnitDestroyed += this.HandleUnitDestroyed;
				unit.OnUnitDocked += this.HandleUnitDocked;
			}
		}

		private void UnsubscribeToUnitEvents(Unit unit)
		{
			if (unit != null)
			{
				unit.OnUnitDespawned -= this.HandleUnitDespawned;
				unit.OnUnitDestroyed -= this.HandleUnitDestroyed;
				unit.OnUnitDocked -= this.HandleUnitDocked;
			}
		}

		private void HandleUnitDespawned(Unit dyingUnit)
		{
			this.mUnitsReadyForSimRemoval.Add(dyingUnit);
		}

		private void HandleUnitDestroyed(Unit destroyedUnit, bool skipDeathSequence, CommanderID killingCommander, Entity killingEntity)
		{
			this.mUnitsDestroyedThisFrame.Add(destroyedUnit, skipDeathSequence);
		}

		private void HandleUnitDocked(Unit dockedUnit)
		{
			UnitManager.PostUnitDockedEvent(dockedUnit.Entity, Sim.Instance.GlobalFrameCount);
		}

		internal static Unit FindLongestUnit(IEnumerable<Unit> units)
		{
			Fixed64 y = Fixed64.Zero;
			Unit result = null;
			foreach (Unit unit in units)
			{
				Fixed64 length = unit.MovementAttributes.Dynamics.Length;
				if (length >= y)
				{
					y = length;
					result = unit;
				}
			}
			return result;
		}

		internal static Unit FindSlowestUnit(IEnumerable<Unit> units)
		{
			Fixed64 y = Fixed64.PositiveInfinity;
			Unit result = null;
			foreach (Unit unit in units)
			{
				UnitDynamicsAttributes dynamics = unit.MovementAttributes.Dynamics;
				if (dynamics.MaxSpeed <= y)
				{
					y = dynamics.MaxSpeed;
					result = unit;
				}
			}
			return result;
		}

		private readonly HashSet<Unit> mUnits = new HashSet<Unit>();

		private readonly HashSet<Entity> mDeployingUnits = new HashSet<Entity>();

		private readonly List<Unit> mUnitsReadyForSimRemoval = new List<Unit>(100);

		private readonly List<Unit> mUnitsReadyDelayedDocking = new List<Unit>();

		public readonly Sim Sim;

		private Dictionary<Unit, bool> mUnitsDestroyedThisFrame = new Dictionary<Unit, bool>();
	}
}
