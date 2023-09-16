using System;
using System.Collections.Generic;
using BBI.Core;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Data;
using BBI.Game.Events;
using BBI.Game.SaveLoad;
using BBI.Game.Utility;

namespace BBI.Game.Simulation
{
	public sealed class Unit : IWeaponTrigger
	{
		internal event UnitHandlerDestroyed OnUnitDestroyed;

		internal event UnitHandlerRemoved OnUnitDespawned;

		internal event UnitHandlerRemoved OnUnitDocked;

		internal event UnitUndockedHandler OnUnitUndocked;

		internal event UnitHandlerFiredWeapon OnUnitFiredWeapon;

		public Entity Entity
		{
			get
			{
				return this.mEntity;
			}
		}

		public string TypeName
		{
			get
			{
				return this.mEntity.GetTypeName();
			}
		}

		public UnitState GetState()
		{
			return new UnitState(this);
		}

		public UnitAttributes Attributes
		{
			get
			{
				return this.mUnitAttributes;
			}
		}

		public UnitMovementAttributes MovementAttributes
		{
			get
			{
				return this.mUnitMovementAttributes;
			}
		}

		public UnitStatus Status
		{
			get
			{
				return this.mUnitStatus;
			}
		}

		public bool IsStatusActive(UnitStatus status)
		{
			return (this.mUnitStatus & status) != UnitStatus.None;
		}

		public void SetStatus(UnitStatus status)
		{
			if (status == UnitStatus.Attacking)
			{
				this.AttackingTimerMS = Sim.Instance.Settings.GlobalUnitStatusAttributes.AttackingTimerDurationMS;
			}
			else if (status == UnitStatus.UnderAttack)
			{
				this.UnderAttackTimerMS = Sim.Instance.Settings.GlobalUnitStatusAttributes.UnderAttackTimerDurationMS;
			}
			if (!this.IsStatusActive(status) || (this.mUnitStatus != UnitStatus.None && status == UnitStatus.None))
			{
				UnitStatus unitStatus = this.mUnitStatus;
				if (status != UnitStatus.None)
				{
					this.mUnitStatus |= status;
				}
				else
				{
					this.mUnitStatus = UnitStatus.None;
				}
				if (unitStatus != this.mUnitStatus)
				{
					Sim.PostEvent(new UnitStatusChangedEvent(this.Entity, unitStatus, this.mUnitStatus));
				}
			}
		}

		public void ClearStatus(UnitStatus status)
		{
			if (this.IsStatusActive(status) && this.mUnitStatus != UnitStatus.None)
			{
				UnitStatus unitStatus = this.mUnitStatus;
				this.mUnitStatus &= ~status;
				if (status == UnitStatus.Attacking)
				{
					this.AttackingTimerMS = 0;
				}
				else if (status == UnitStatus.UnderAttack)
				{
					this.UnderAttackTimerMS = 0;
				}
				if (unitStatus != this.mUnitStatus)
				{
					Sim.PostEvent(new UnitStatusChangedEvent(this.Entity, unitStatus, this.mUnitStatus));
				}
			}
		}

		public Fixed64 Diagonal
		{
			get
			{
				if (this.mUnitMovementAttributes != null)
				{
					Fixed64 length = this.MovementAttributes.Dynamics.Length;
					Fixed64 width = this.MovementAttributes.Dynamics.Width;
					return Fixed64.Sqrt(length * length + width * width);
				}
				return Fixed64.Zero;
			}
		}

		public static Unit CreateUnit(Entity entity)
		{
			Unit unit = new Unit(entity);
			entity.AddComponent<Unit>(2, unit);
			Unit.AttachUnitRelatedComponentsToEntity(entity);
			return unit;
		}

		internal static void AttachUnitRelatedComponentsToEntity(Entity entity)
		{
			Unit.CreateUnitRelatedComponents(entity);
			OwningCommander owningCommander = entity.IsValid() ? entity.GetComponent<OwningCommander>(5) : null;
			if (owningCommander != null)
			{
				AbilityHelper.AttachAbilitiesToUnitOnSpawn(entity, owningCommander.ID);
			}
		}

		[ObjectConstructor(new string[]
		{
			"Entity"
		})]
		private Unit(Entity unitEntity)
		{
			this.mEntity = unitEntity;
			this.mUnitAttributes = unitEntity.GetTypeAttributes<UnitAttributes>();
			this.mPresentationOnly = false;
			this.mBonePositionIndexOverride = -1;
			this.mUnitMovementAttributes = unitEntity.GetTypeAttributes<UnitMovementAttributes>();
			this.UnitClass = this.mUnitAttributes.Class;
			this.mDeathScheduled = false;
			this.HasBeenSpawnedIntoWorldBefore = false;
		}

		internal void OnBuffChangedEvent(BuffChangedEvent ev)
		{
			this.mUnitAttributes = this.mEntity.GetTypeAttributes<UnitAttributes>();
			this.mUnitMovementAttributes = this.mEntity.GetTypeAttributes<UnitMovementAttributes>();
			bool flag = ev.BuffChangedType == BuffChangedEvent.BuffChangeType.CommanderChanged;
			AttributeBuffSet buffSet = ev.BuffSet;
			bool flag2 = flag;
			bool flag3 = flag;
			bool flag4 = flag;
			bool flag5 = flag;
			if (!flag && buffSet != null)
			{
				foreach (AttributeBuff attributeBuff in buffSet.GetBuffs(Buff.Category.Unit, null))
				{
					ushort attributeID = attributeBuff.AttributeID;
					if (attributeID != 0)
					{
						switch (attributeID)
						{
						case 6:
							flag3 = true;
							break;
						case 7:
							flag4 = true;
							break;
						case 8:
							flag5 = true;
							break;
						}
					}
					else
					{
						flag2 = true;
					}
				}
			}
			if (flag2)
			{
				Health component = this.mEntity.GetComponent<Health>(7);
				if (component != null)
				{
					Fixed64 @fixed = Fixed64.FromInt(component.MaxHealth);
					Fixed64 x = Fixed64.FromInt(component.CurrentHealth);
					component.MaxHealth = this.Attributes.MaxHealth;
					Fixed64 x2 = Fixed64.BigEnough(@fixed) ? (x / @fixed) : Fixed64.One;
					component.CurrentHealth = Fixed64.IntValue(x2 * Fixed64.FromInt(this.Attributes.MaxHealth));
				}
			}
			if (flag3 || flag4)
			{
				Sensor component2 = this.mEntity.GetComponent<Sensor>(20);
				if (component2 != null)
				{
					if (flag3)
					{
						component2.SensorRadius = this.Attributes.SensorRadius;
					}
					if (flag4)
					{
						component2.ContactRadius = this.Attributes.ContactRadius;
					}
				}
			}
			if (flag5)
			{
				Production component3 = this.mEntity.GetComponent<Production>(15);
				if (component3 != null)
				{
					if (component3.NumQueues > this.Attributes.NumProductionQueues)
					{
						int num = component3.NumQueues - this.Attributes.NumProductionQueues;
						for (int i = 0; i < num; i++)
						{
							component3.RemoveQueue();
						}
					}
					else if (component3.NumQueues < this.Attributes.NumProductionQueues)
					{
						int num2 = this.Attributes.NumProductionQueues - component3.NumQueues;
						for (int j = 0; j < num2; j++)
						{
							component3.AddQueue();
						}
					}
				}
			}
			List<Ability> listComponent = this.mEntity.GetListComponent<Ability>(17);
			if (!listComponent.IsNullOrEmpty<Ability>())
			{
				foreach (Ability ability in listComponent)
				{
					bool flag6 = flag;
					if (!flag6 && buffSet != null)
					{
						IEnumerable<AttributeBuff> buffs = buffSet.GetBuffs(Buff.Category.Ability, ability.AbilityAttributes.Name);
						flag6 = !buffs.IsNullOrEmpty<AttributeBuff>();
					}
					if (flag6)
					{
						ability.OnBuff();
					}
				}
			}
			Maneuvering component4 = this.mEntity.GetComponent<Maneuvering>(12);
			if (component4 != null)
			{
				bool flag7 = flag;
				if (!flag7 && buffSet != null)
				{
					IEnumerable<AttributeBuff> buffs2 = buffSet.GetBuffs(Buff.Category.UnitDynamics, null);
					flag7 = !buffs2.IsNullOrEmpty<AttributeBuff>();
					if (!flag7)
					{
						IEnumerable<AttributeBuff> buffs3 = buffSet.GetBuffs(Buff.Category.UnitCombatBehaviour, null);
						flag7 = !buffs3.IsNullOrEmpty<AttributeBuff>();
					}
				}
				if (flag7)
				{
					component4.BuffAppliedThisFrame = true;
				}
			}
			Storage component5 = this.mEntity.GetComponent<Storage>(18);
			if (component5 != null)
			{
				bool flag8 = flag;
				if (!flag8 && buffSet != null)
				{
					IEnumerable<AttributeBuff> buffs4 = buffSet.GetBuffs(Buff.Category.Inventory, null);
					flag8 = !buffs4.IsNullOrEmpty<AttributeBuff>();
				}
				if (flag8)
				{
					component5.OnBuffApplied();
				}
			}
			UnitHangar component6 = this.mEntity.GetComponent<UnitHangar>(23);
			if (component6 != null)
			{
				bool flag9 = flag;
				if (!flag9 && buffSet != null)
				{
					IEnumerable<AttributeBuff> buffs5 = buffSet.GetBuffs(Buff.Category.HangarBay, null);
					flag9 = !buffs5.IsNullOrEmpty<AttributeBuff>();
				}
				if (flag9)
				{
					component6.OnBuffApplied();
				}
			}
			this.mEntity.CheckRebindWeapons(ev.PreviousFlags);
			List<WeaponFireOperation> listComponent2 = this.mEntity.GetListComponent<WeaponFireOperation>(6);
			if (!listComponent2.IsNullOrEmpty<WeaponFireOperation>())
			{
				bool flag10 = flag;
				if (!flag10 && buffSet != null)
				{
					IEnumerable<AttributeBuff> buffs6 = buffSet.GetBuffs(Buff.Category.UnitWeapon, null);
					if (!buffs6.IsNullOrEmpty<AttributeBuff>())
					{
						for (int k = 0; k < listComponent2.Count; k++)
						{
							WeaponFireOperation weaponFireOperation = listComponent2[k];
							WeaponBinding weaponBinding = weaponFireOperation.WeaponBinding;
							if (weaponBinding != null)
							{
								foreach (AttributeBuff attributeBuff2 in buffSet.GetBuffs(Buff.Category.UnitWeapon, weaponBinding.WeaponID))
								{
									if (attributeBuff2.AttributeID == 9)
									{
										flag10 = true;
										break;
									}
								}
							}
							if (flag10)
							{
								break;
							}
						}
					}
				}
				if (flag10)
				{
					Targets.ForceTargetAcquisition(this.mEntity);
				}
			}
		}

		private static void CreateUnitRelatedComponents(Entity unitEntity)
		{
			unitEntity.AddComponent<StatusEffects>(28, StatusEffects.Create(unitEntity));
			UnitMovementAttributes typeAttributes = unitEntity.GetTypeAttributes<UnitMovementAttributes>();
			if (typeAttributes != null)
			{
				if (typeAttributes.ReversePolarity.Enabled)
				{
					Fixed64 repelRadius = Fixed64.Half * typeAttributes.ReversePolarity.PushRadiusMultiplier * typeAttributes.Dynamics.Length;
					unitEntity.AddComponent<ReversePolarity>(34, ReversePolarity.Create(repelRadius, typeAttributes.ReversePolarity.RelativeWeight, typeAttributes.ReversePolarity.SquishinessFactor));
				}
				if (typeAttributes.Dynamics != null && typeAttributes.Dynamics.PermanentlyImmobile)
				{
					StatusEffectProcessor.AddModifierStack(unitEntity, ref StatusEffectProcessor.sImmobilizeModifierAttributes);
				}
			}
			UnitAttributes typeAttributes2 = unitEntity.GetTypeAttributes<UnitAttributes>();
			if (typeAttributes2 != null)
			{
				unitEntity.AddComponent<Health>(7, Health.Create(typeAttributes2.MaxHealth, typeAttributes2.MaxHealth));
				if (typeAttributes2.NumProductionQueues > 0)
				{
					unitEntity.AddComponent<Production>(15, Production.Create(unitEntity, typeAttributes2));
				}
				if (!typeAttributes2.WeaponLoadout.IsNullOrEmpty<WeaponBinding>())
				{
					for (int i = 0; i < typeAttributes2.WeaponLoadout.Length; i++)
					{
						WeaponBinding weaponBinding = typeAttributes2.WeaponLoadout[i];
						if (weaponBinding != null)
						{
							WeaponFireOperation componentData = WeaponFireOperation.Create(unitEntity, weaponBinding);
							unitEntity.AddListComponent(6, componentData);
						}
						else
						{
							Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Null WeaponBinding at index {0} in WeaponLoadout for unit {1}! Weapon won't be created properly!", new object[]
							{
								i,
								typeAttributes2.Name
							});
						}
					}
				}
			}
			unitEntity.AddComponent<Targets>(4, Targets.Create());
			unitEntity.AddComponent<EntityGoalsCollection>(19, EntityGoalsCollection.Create(unitEntity));
			DetectableAttributes typeAttributes3 = unitEntity.GetTypeAttributes<DetectableAttributes>();
			if (typeAttributes3 != null)
			{
				unitEntity.AddComponent<Detectable>(21, Detectable.Create(typeAttributes3, unitEntity));
			}
			UnitQuadrantDamageAttributes typeAttributes4 = unitEntity.GetTypeAttributes<UnitQuadrantDamageAttributes>();
			if (typeAttributes4 != null)
			{
				unitEntity.AddComponent<QuadrantDamage>(30, QuadrantDamage.Create(typeAttributes4));
			}
			UnitHangarAttributes typeAttributes5 = unitEntity.GetTypeAttributes<UnitHangarAttributes>();
			if (typeAttributes5 != null)
			{
				unitEntity.AddComponent<UnitHangar>(23, UnitHangar.Create(unitEntity, typeAttributes5));
			}
			HarvesterAttributes typeAttributes6 = unitEntity.GetTypeAttributes<HarvesterAttributes>();
			if (typeAttributes6 != null && !unitEntity.HasComponent(16))
			{
				unitEntity.AddComponent<Harvester>(16, Harvester.Create(unitEntity));
			}
			ExperienceAttributes typeAttributes7 = unitEntity.GetTypeAttributes<ExperienceAttributes>();
			if (typeAttributes7 != null)
			{
				unitEntity.AddComponent<Experience>(39, Experience.Create(unitEntity, typeAttributes7));
			}
			StorageAttributes typeAttributes8 = unitEntity.GetTypeAttributes<StorageAttributes>();
			if (typeAttributes8 != null && typeAttributes8.InventoryLoadout != null)
			{
				List<Inventory> list = new List<Inventory>(typeAttributes8.InventoryLoadout.Length);
				foreach (InventoryBinding inventoryBinding in typeAttributes8.InventoryLoadout)
				{
					if (inventoryBinding != null)
					{
						Inventory item = new Inventory(inventoryBinding);
						list.Add(item);
					}
					else
					{
						Log.Error(Log.Channel.Data, "Inventory loadout has empty entries! {0}", new object[]
						{
							unitEntity.ToFriendlyString()
						});
					}
				}
				if (typeAttributes8.LinkToPlayerBank || !list.IsNullOrEmpty<Inventory>())
				{
					unitEntity.AddComponent<Storage>(18, Storage.Create(unitEntity, list, typeAttributes8.LinkToPlayerBank, typeAttributes8.IsResourceController));
				}
			}
			PowerShuntAttributes typeAttributes9 = unitEntity.GetTypeAttributes<PowerShuntAttributes>();
			if (typeAttributes9 != null && !typeAttributes9.PowerSystems.IsNullOrEmpty<PowerSystemAttributes>())
			{
				unitEntity.AddComponent<PowerShunt>(22, PowerShunt.Create(unitEntity, typeAttributes9));
			}
		}

		internal void Tick(Checksum check, Fixed64 tickPeriod)
		{
			this.AddChecksum(check);
			if (this.mPresentationOnly)
			{
				return;
			}
			this.UpdateGuards();
		}

		private void UpdateGuards()
		{
			if (this.EntityGuardGroup != null)
			{
				List<Entity> list = TransientLists.GetList<Entity>();
				foreach (Entity entity in this.EntityGuardGroup.GuardingEntities)
				{
					if (!entity.IsValid())
					{
						list.Add(entity);
					}
					else
					{
						EntityGoalsCollection component = entity.GetComponent<EntityGoalsCollection>(19);
						if (component != null)
						{
							if (component.CurrentGoal == null)
							{
								list.Add(entity);
							}
							else if (component.CurrentGoal.GoalType == GoalType.Guard)
							{
								GuardGoal guardGoal = component.CurrentGoal as GuardGoal;
								if (guardGoal != null && guardGoal.GuardedEntity != this.Entity)
								{
									list.Add(entity);
								}
							}
							else
							{
								list.Add(entity);
							}
						}
					}
				}
				foreach (Entity guardMember in list)
				{
					this.EntityGuardGroup.RemoveFromGuard(guardMember);
				}
				TransientLists.ReturnList<Entity>(list);
			}
		}

		public void DespawnUnit()
		{
			if (this.OnUnitDespawned != null)
			{
				this.OnUnitDespawned(this);
			}
		}

		private bool DestroyUnit(UnitRemoveReason reason, bool skipDeathSequence, CommanderID killingCommander, Entity killingEntity)
		{
			UnitHangar component = this.Entity.GetComponent<UnitHangar>(23);
			if (component != null)
			{
				CollectibleEntityProcessor.DropAllCollectibleEntities(component);
			}
			this.ClearEntityPropertiesForDespawn();
			switch (reason)
			{
			case UnitRemoveReason.Despawn:
				this.DespawnUnit();
				return true;
			case UnitRemoveReason.Destroy:
				if (!this.mDeathScheduled)
				{
					this.mDeathScheduled = true;
					if (this.OnUnitDestroyed != null)
					{
						this.OnUnitDestroyed(this, skipDeathSequence, killingCommander, killingEntity);
					}
					return true;
				}
				break;
			}
			return false;
		}

		public void UndockSequenceComplete(Unit undockedUnit, int bayId)
		{
			if (this.OnUnitUndocked != null)
			{
				this.OnUnitUndocked(undockedUnit, bayId);
			}
		}

		public void DockDespawn()
		{
			this.SetPresentationOnlyAndSuspend(false);
			this.ClearEntityPropertiesForDespawn();
			Sim.Instance.Map.SpatialHash.RemoveEntity(this.Entity);
			UnitManager.RemoveComponentsOnDespawnFromWorld(this, true);
			if (this.OnUnitDocked != null)
			{
				this.OnUnitDocked(this);
			}
		}

		public void RetireDespawn()
		{
			ModifyHealthProcessor.DestroyEntity(this.Entity, UnitRemoveReason.Despawn);
		}

		private void ClearEntityPropertiesForDespawn()
		{
			this.SetStatus(UnitStatus.None);
			UnitManager.DeactivatePassiveAbilities(this);
			StatusEffectProcessor.ClearStatusEffects(this.Entity, true);
			EntityGoalsCollection.ClearGoals(this.Entity);
			Targets.ClearTargets(this.Entity);
			WeaponsProcessor.DeactivateWeaponsForUnit(this.Entity);
		}

		internal void AddChecksum(Checksum check)
		{
			check.Add(this.Entity.ToReference());
			Position component = this.Entity.GetComponent<Position>(10);
			if (component != null)
			{
				check.Add(component.Position2D.GetHashCode());
				check.Add(component.Orientation.GetHashCode());
			}
			Maneuvering component2 = this.Entity.GetComponent<Maneuvering>(12);
			if (component2 != null)
			{
				check.Add(component2.CurrentDestination.GetHashCode());
			}
			check.Add(this.Entity.HasComponent(13).GetHashCode());
			check.Add(this.Entity.HasComponent(16).GetHashCode());
			bool flag = this.Entity.HasComponent(7);
			check.Add(flag.GetHashCode());
			if (flag)
			{
				Health component3 = this.Entity.GetComponent<Health>(7);
				check.Add(component3.CurrentHealth);
				check.Add(component3.MaxHealth);
				check.Add((int)component3.GodMode);
			}
			EntityGoalsCollection component4 = this.Entity.GetComponent<EntityGoalsCollection>(19);
			if (component4 != null && component4.CurrentGoal != null)
			{
				check.Add((int)component4.CurrentGoal.GoalType);
			}
		}

		void IWeaponTrigger.PrepareWeapon(WeaponFireOperation weaponFireOperation)
		{
		}

		void IWeaponTrigger.TriggerWeapon(Entity attackingEntity, WeaponFireOperation weaponFireOperation, WeaponFireTrigger weaponTrigger)
		{
			if (this.OnUnitFiredWeapon != null)
			{
				this.OnUnitFiredWeapon(this, weaponFireOperation, weaponTrigger);
			}
		}

		public bool PresentationOnly
		{
			get
			{
				return this.mPresentationOnly;
			}
		}

		public int BonePositionOverrideIndex
		{
			get
			{
				return this.mBonePositionIndexOverride;
			}
		}

		public void SetPresentationOnlyAndSuspend(bool value)
		{
			if (!value)
			{
				this.mBonePositionIndexOverride = -1;
			}
			if (this.mPresentationOnly != value && this.mEntity.IsValid())
			{
				if (value && !this.mEntity.HasComponent(3))
				{
					this.mEntity.AddComponent<object>(3, null);
				}
				else if (!value && this.mEntity.HasComponent(3))
				{
					this.mEntity.RemoveComponent<object>(3);
				}
			}
			this.mPresentationOnly = value;
		}

		public void Die(UnitRemoveReason reason, bool skipDeathSequence, CommanderID killingCommanderID, Entity killingEntity)
		{
			this.DestroyUnit(reason, skipDeathSequence, killingCommanderID, killingEntity);
		}

		[CustomConverter(ConverterDirection.Save, ClassStateConversionOrder.RunStateDataConversionAfter)]
		private void OnSave(ref UnitSaveState state)
		{
			state.HasAGuardGroupAssigned = (this.EntityGuardGroup != null);
			if (state.HasAGuardGroupAssigned)
			{
				ExtractorManager.Save<EntityGuardGroup, EntityGuardGroupSaveState>(this.EntityGuardGroup, ref state.GuardGroup);
			}
		}

		[CustomConverter(ConverterDirection.Load, ClassStateConversionOrder.RunStateDataConversionAfter)]
		private void OnLoad(ref UnitSaveState state)
		{
			if (state.HasAGuardGroupAssigned)
			{
				ExtractorManager.Load<EntityGuardGroup, EntityGuardGroupSaveState>(ref this.EntityGuardGroup, state.GuardGroup);
			}
		}

		public static string EntityDebugString(Entity entity)
		{
			string text = "invalid";
			if (entity.IsValid())
			{
				text = entity.GetTypeName();
				if (text == null)
				{
					text = "unknown";
				}
			}
			return entity.ToReference().ToString() + "-" + text;
		}

		private UnitAttributes mUnitAttributes;

		private UnitMovementAttributes mUnitMovementAttributes;

		[StateData("Entity", ConverterDirection.Save)]
		private readonly Entity mEntity;

		internal EntityGuardGroup EntityGuardGroup;

		[StateData("DeathScheduled")]
		private bool mDeathScheduled;

		[StateData("HasBeenSpawnedIntoWorldBefore")]
		internal bool HasBeenSpawnedIntoWorldBefore;

		[StateData("PresentationOnly")]
		private bool mPresentationOnly;

		[StateData("BonePositionIndexOverride")]
		private int mBonePositionIndexOverride = -1;

		[StateData("UnitStatus")]
		private UnitStatus mUnitStatus;

		[StateData("AttackingTimerMS")]
		internal int AttackingTimerMS;

		[StateData("UnderAttackTimerMS")]
		internal int UnderAttackTimerMS;

		[StateData("LastAttackedByEntity")]
		internal Entity LastAttackedByEntity;

		public readonly UnitClass UnitClass;
	}
}
