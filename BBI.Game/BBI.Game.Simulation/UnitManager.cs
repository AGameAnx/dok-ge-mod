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
	// Token: 0x020003DF RID: 991
	internal sealed class UnitManager
	{
		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060014F5 RID: 5365 RVA: 0x00074BBB File Offset: 0x00072DBB
		internal HashSet<Entity> DeployingUnits
		{
			get
			{
				return this.mDeployingUnits;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060014F6 RID: 5366 RVA: 0x00074BC3 File Offset: 0x00072DC3
		// (set) Token: 0x060014F7 RID: 5367 RVA: 0x00074BCA File Offset: 0x00072DCA
		public static Fixed64 sMaxUnitDiagonal { get; private set; }

		// Token: 0x060014F8 RID: 5368 RVA: 0x00074BD4 File Offset: 0x00072DD4
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

		// Token: 0x060014F9 RID: 5369 RVA: 0x00074CB8 File Offset: 0x00072EB8
		internal void Shutdown()
		{
			this.mUnits.Clear();
			Sim.Instance.SimEventSystem.RemoveHandler<BuffChangedEvent>(new BBI.Core.Events.EventHandler<BuffChangedEvent>(this.OnBuffChangedEvent));
			Sim.Instance.SimEventSystem.RemoveHandler<DockingUnitOperationEvent>(new BBI.Core.Events.EventHandler<DockingUnitOperationEvent>(this.OnDockingMessage));
			Sim.Instance.SimEventSystem.RemoveHandler<HangarDespawnUnitEvent>(new BBI.Core.Events.EventHandler<HangarDespawnUnitEvent>(this.OnHangarDespawnUnitMessage));
			Sim.Instance.SimEventSystem.RemoveHandler<ModifyAbilityListEvent>(new BBI.Core.Events.EventHandler<ModifyAbilityListEvent>(this.OnModifyAbilityListEvent));
			Sim.Instance.SimEventSystem.RemoveHandler<UnitDestroyedEvent>(new BBI.Core.Events.EventHandler<UnitDestroyedEvent>(this.OnUnitDestroyed));
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x00074D58 File Offset: 0x00072F58
		internal static void TransferUnitToCommander(Unit unitToTransfer, CommanderID newCommanderID)
		{
			if (unitToTransfer == null || !unitToTransfer.Entity.IsValid())
			{
				Log.Warn(Log.Channel.Core, "Trying to transfer an invalid unit/entity to another commander. Ignoring.", new object[0]);
				return;
			}
			OwningCommander component = unitToTransfer.Entity.GetComponent(5);
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
					unitToTransfer.Entity.RemoveComponent(0);
					unitToTransfer.Entity.AddComponent(0, commanderSpecificEntityType);
					int flags2 = commanderSpecificEntityType.Flags;
					Sim.PostEvent(new BuffChangedEvent(null, unitToTransfer.Entity, BuffChangedEvent.BuffChangeType.CommanderChanged, flags, flags2));
					if (array != null)
					{
						unitToTransfer.Entity.AddBuffsFromBuffStates(array);
					}
					Storage component2 = unitToTransfer.Entity.GetComponent(18);
					if (component2 != null && component2.IsLinkedToCommanderBank)
					{
						component2.CreateCommanderInventoryLinks(newCommanderID);
					}
					AbilityHelper.UpdateAbilityAvailabilityOnCommanderTransfer(unitToTransfer.Entity, newCommanderID);
					UnitManager.ActivatePassiveAbilities(unitToTransfer);
					Experience component3 = unitToTransfer.Entity.GetComponent(39);
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

		// Token: 0x060014FB RID: 5371 RVA: 0x00074F00 File Offset: 0x00073100
		private static void PostUnitRemovedEvent(Entity removedUnitEntityID, SimFrameNumber frameNumber)
		{
			Unit component = removedUnitEntityID.GetComponent(2);
			if (component != null)
			{
				UnitRemoveReason reason = UnitRemoveReason.Despawn;
				Sim.PostEvent(new UnitRemovedFeedbackEvent(removedUnitEntityID, false, Sim.GetEntityCommanderID(component.Entity), frameNumber, true, reason));
			}
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x00074F34 File Offset: 0x00073134
		private static void PostUnitDiedEvent(Entity dyingUnitEntityID, bool skipDeathSequence, SimFrameNumber frameNumber)
		{
			Unit component = dyingUnitEntityID.GetComponent(2);
			if (component != null)
			{
				UnitRemoveReason reason = UnitRemoveReason.Destroy;
				Sim.PostEvent(new UnitRemovedFeedbackEvent(dyingUnitEntityID, skipDeathSequence, Sim.GetEntityCommanderID(component.Entity), frameNumber, true, reason));
			}
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x00074F68 File Offset: 0x00073168
		private static void PostUnitDockedEvent(Entity dockingUnitEntityID, SimFrameNumber frameNumber)
		{
			Unit component = dockingUnitEntityID.GetComponent(2);
			if (component != null)
			{
				UnitRemoveReason reason = UnitRemoveReason.UnitDock;
				Sim.PostEvent(new UnitRemovedFeedbackEvent(dockingUnitEntityID, false, Sim.GetEntityCommanderID(component.Entity), frameNumber, true, reason));
			}
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x00074F9C File Offset: 0x0007319C
		private void OnUnitDestroyed(UnitDestroyedEvent ev)
		{
			if (ev.KillingEntity.IsValid() && ev.KillingEntity.HasComponent(39))
			{
				Experience component = ev.KillingEntity.GetComponent(39);
				component.OnUnitDestroyed(ev);
			}
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x00074FDC File Offset: 0x000731DC
		private void OnHangarDespawnUnitMessage(HangarDespawnUnitEvent ce)
		{
			if (!ce.DespawnUnit.IsValid())
			{
				Log.Error(Log.Channel.Core, "jbax [hangar] OnHangarDespawnUnitMessage failed to find valid entity", new object[0]);
				return;
			}
			Unit component = ce.DespawnUnit.GetComponent(2);
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

		// Token: 0x06001500 RID: 5376 RVA: 0x00075048 File Offset: 0x00073248
		private void OnDockingMessage(DockingUnitOperationEvent ce)
		{
			Entity dockingEntity = ce.DockingEntity;
			if (dockingEntity.IsValid())
			{
				Unit component = dockingEntity.GetComponent(2);
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

		// Token: 0x06001501 RID: 5377 RVA: 0x000750EC File Offset: 0x000732EC
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

		// Token: 0x06001502 RID: 5378 RVA: 0x00075150 File Offset: 0x00073350
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

		// Token: 0x06001503 RID: 5379 RVA: 0x00075330 File Offset: 0x00073530
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

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06001504 RID: 5380 RVA: 0x0007548C File Offset: 0x0007368C
		internal IEnumerable<Unit> Units
		{
			get
			{
				return this.mUnits;
			}
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x00075650 File Offset: 0x00073850
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

		// Token: 0x06001506 RID: 5382 RVA: 0x00075674 File Offset: 0x00073874
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

		// Token: 0x06001507 RID: 5383 RVA: 0x000756AC File Offset: 0x000738AC
		internal void RemoveAllUnits(SimFrameNumber frame)
		{
			UnitManager.sMaxUnitDiagonal = Fixed64.Zero;
			foreach (Unit unit in this.mUnits)
			{
				this.RemoveUnitFromSim(unit, frame, false);
			}
			this.mUnits.Clear();
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x00075718 File Offset: 0x00073918
		internal void OnUnitsLoaded(IEnumerable<Entity> loadedEntities, Entity[] deployingUnits)
		{
			foreach (Entity entity in loadedEntities)
			{
				Unit component = entity.GetComponent(2);
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

		// Token: 0x06001509 RID: 5385 RVA: 0x000757D4 File Offset: 0x000739D4
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

		// Token: 0x0600150A RID: 5386 RVA: 0x000758D8 File Offset: 0x00073AD8
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

		// Token: 0x0600150B RID: 5387 RVA: 0x0007597C File Offset: 0x00073B7C
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

		// Token: 0x0600150C RID: 5388 RVA: 0x00075A30 File Offset: 0x00073C30
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

		// Token: 0x0600150D RID: 5389 RVA: 0x00075A94 File Offset: 0x00073C94
		internal static void AddComponentsOnSpawnIntoWorld(Unit unit, Vector2r position, Orientation2 orientation)
		{
			if (unit != null && unit.Entity.IsValid())
			{
				Maneuvering component = Maneuvering.Create(unit.Entity, position, orientation, unit.Attributes.NavMeshAttributes);
				unit.Entity.AddComponent(12, component);
				if (unit.MovementAttributes != null && unit.MovementAttributes.Dynamics != null)
				{
					Fixed64 @fixed = unit.MovementAttributes.Dynamics.Width >> 1;
					Fixed64 capsuleAxisLength = unit.MovementAttributes.Dynamics.Length - (@fixed << 1);
					Shape component2 = Shape.Create(position, orientation, @fixed, capsuleAxisLength, unit.Attributes.BlocksLOF, false, unit.Attributes.WorldHeightOffset);
					unit.Entity.AddComponent(33, component2);
				}
				Detectable component3 = unit.Entity.GetComponent(21);
				if (component3 != null)
				{
					foreach (CommanderID sensingCommander in Sim.Instance.CommanderManager.CommanderIDs)
					{
						component3.SetDetectionState(sensingCommander, DetectionState.Hidden);
					}
				}
				unit.Entity.AddComponent(20, Sensor.Create(unit.Attributes, unit.Entity));
				SimMap map = Sim.Instance.Map;
			}
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x00075BEC File Offset: 0x00073DEC
		internal static void ActivateAbilitiesOnSpawnIntoWorld(Unit unit)
		{
			UnitManager.ActivatePassiveAbilities(unit);
			if (!unit.HasBeenSpawnedIntoWorldBefore)
			{
				UnitManager.ActivateAutocastAbilities(unit);
			}
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x00075C04 File Offset: 0x00073E04
		internal static void RemoveComponentsOnDespawnFromWorld(Unit unit, bool dockDespawn)
		{
			if (unit != null && unit.Entity.IsValid())
			{
				if (!dockDespawn)
				{
					unit.Entity.AddComponent(42, default(InstantDespawn));
				}
				UnitManager.DeactivatePassiveAbilities(unit);
				Maneuvering.RemoveComponent(unit.Entity);
				unit.Entity.RemoveComponent(10);
				unit.Entity.RemoveComponent(33);
				unit.Entity.RemoveComponent(20);
			}
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x00075C74 File Offset: 0x00073E74
		private static void ActivateAutocastAbilities(Unit unit)
		{
			List<Ability> listComponent = unit.Entity.GetListComponent(17);
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

		// Token: 0x06001511 RID: 5393 RVA: 0x00075D08 File Offset: 0x00073F08
		private static void ActivatePassiveAbilities(Unit unit)
		{
			List<Ability> listComponent = unit.Entity.GetListComponent(17);
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

		// Token: 0x06001512 RID: 5394 RVA: 0x00075D94 File Offset: 0x00073F94
		internal static void DeactivatePassiveAbilities(Unit unit)
		{
			List<Ability> listComponent = unit.Entity.GetListComponent(17);
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

		// Token: 0x06001513 RID: 5395 RVA: 0x00075E20 File Offset: 0x00074020
		private void SubscribeToUnitEvents(Unit unit)
		{
			if (unit != null)
			{
				unit.OnUnitDespawned += this.HandleUnitDespawned;
				unit.OnUnitDestroyed += this.HandleUnitDestroyed;
				unit.OnUnitDocked += this.HandleUnitDocked;
			}
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x00075E5B File Offset: 0x0007405B
		private void UnsubscribeToUnitEvents(Unit unit)
		{
			if (unit != null)
			{
				unit.OnUnitDespawned -= this.HandleUnitDespawned;
				unit.OnUnitDestroyed -= this.HandleUnitDestroyed;
				unit.OnUnitDocked -= this.HandleUnitDocked;
			}
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x00075E96 File Offset: 0x00074096
		private void HandleUnitDespawned(Unit dyingUnit)
		{
			this.mUnitsReadyForSimRemoval.Add(dyingUnit);
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x00075EA4 File Offset: 0x000740A4
		private void HandleUnitDestroyed(Unit destroyedUnit, bool skipDeathSequence, CommanderID killingCommander, Entity killingEntity)
		{
			this.mUnitsDestroyedThisFrame.Add(destroyedUnit, skipDeathSequence);
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x00075EB3 File Offset: 0x000740B3
		private void HandleUnitDocked(Unit dockedUnit)
		{
			UnitManager.PostUnitDockedEvent(dockedUnit.Entity, Sim.Instance.GlobalFrameCount);
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x00075ECC File Offset: 0x000740CC
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

		// Token: 0x06001519 RID: 5401 RVA: 0x00075F3C File Offset: 0x0007413C
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

		// Token: 0x040010D3 RID: 4307
		private readonly HashSet<Unit> mUnits = new HashSet<Unit>();

		// Token: 0x040010D4 RID: 4308
		private readonly HashSet<Entity> mDeployingUnits = new HashSet<Entity>();

		// Token: 0x040010D5 RID: 4309
		private readonly List<Unit> mUnitsReadyForSimRemoval = new List<Unit>(100);

		// Token: 0x040010D6 RID: 4310
		private readonly List<Unit> mUnitsReadyDelayedDocking = new List<Unit>();

		// Token: 0x040010D7 RID: 4311
		public readonly Sim Sim;

		// Token: 0x040010D8 RID: 4312
		private Dictionary<Unit, bool> mUnitsDestroyedThisFrame = new Dictionary<Unit, bool>();
	}
}
