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
	// Token: 0x020003D4 RID: 980
	internal sealed class Unit : IWeaponTrigger
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600143A RID: 5178 RVA: 0x000705D0 File Offset: 0x0006E7D0
		// (remove) Token: 0x0600143B RID: 5179 RVA: 0x00070608 File Offset: 0x0006E808
		public event UnitHandlerDestroyed OnUnitDestroyed;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600143C RID: 5180 RVA: 0x00070640 File Offset: 0x0006E840
		// (remove) Token: 0x0600143D RID: 5181 RVA: 0x00070678 File Offset: 0x0006E878
		public event UnitHandlerRemoved OnUnitDespawned;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600143E RID: 5182 RVA: 0x000706B0 File Offset: 0x0006E8B0
		// (remove) Token: 0x0600143F RID: 5183 RVA: 0x000706E8 File Offset: 0x0006E8E8
		public event UnitHandlerRemoved OnUnitDocked;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06001440 RID: 5184 RVA: 0x00070720 File Offset: 0x0006E920
		// (remove) Token: 0x06001441 RID: 5185 RVA: 0x00070758 File Offset: 0x0006E958
		public event UnitUndockedHandler OnUnitUndocked;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06001442 RID: 5186 RVA: 0x00070790 File Offset: 0x0006E990
		// (remove) Token: 0x06001443 RID: 5187 RVA: 0x000707C8 File Offset: 0x0006E9C8
		public event UnitHandlerFiredWeapon OnUnitFiredWeapon;

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001444 RID: 5188 RVA: 0x000707FD File Offset: 0x0006E9FD
		public Entity Entity
		{
			get
			{
				return this.mEntity;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06001445 RID: 5189 RVA: 0x00070805 File Offset: 0x0006EA05
		public string TypeName
		{
			get
			{
				return this.mEntity.GetTypeName();
			}
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x00070812 File Offset: 0x0006EA12
		public UnitState GetState()
		{
			return new UnitState(this);
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06001447 RID: 5191 RVA: 0x0007081A File Offset: 0x0006EA1A
		public UnitAttributes Attributes
		{
			get
			{
				return this.mUnitAttributes;
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001448 RID: 5192 RVA: 0x00070822 File Offset: 0x0006EA22
		public UnitMovementAttributes MovementAttributes
		{
			get
			{
				return this.mUnitMovementAttributes;
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x0007082A File Offset: 0x0006EA2A
		public UnitStatus Status
		{
			get
			{
				return this.mUnitStatus;
			}
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x00070832 File Offset: 0x0006EA32
		public bool IsStatusActive(UnitStatus status)
		{
			return (this.mUnitStatus & status) != UnitStatus.None;
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x00070844 File Offset: 0x0006EA44
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

		// Token: 0x0600144C RID: 5196 RVA: 0x000708E4 File Offset: 0x0006EAE4
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

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x0600144D RID: 5197 RVA: 0x00070950 File Offset: 0x0006EB50
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

		// Token: 0x0600144E RID: 5198 RVA: 0x000709A8 File Offset: 0x0006EBA8
		public static Unit CreateUnit(Entity entity)
		{
			Unit unit = new Unit(entity);
			entity.AddComponent(2, unit);
			Unit.AttachUnitRelatedComponentsToEntity(entity);
			return unit;
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x000709CC File Offset: 0x0006EBCC
		internal static void AttachUnitRelatedComponentsToEntity(Entity entity)
		{
			Unit.CreateUnitRelatedComponents(entity);
			OwningCommander owningCommander = entity.IsValid() ? entity.GetComponent(5) : null;
			if (owningCommander != null)
			{
				AbilityHelper.AttachAbilitiesToUnitOnSpawn(entity, owningCommander.ID);
			}
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x00070A04 File Offset: 0x0006EC04
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

		// Token: 0x06001451 RID: 5201 RVA: 0x00070A6C File Offset: 0x0006EC6C
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
				Health component = this.mEntity.GetComponent(7);
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
				Sensor component2 = this.mEntity.GetComponent(20);
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
				Production component3 = this.mEntity.GetComponent(15);
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
			List<Ability> listComponent = this.mEntity.GetListComponent(17);
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
			Maneuvering component4 = this.mEntity.GetComponent(12);
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
			Storage component5 = this.mEntity.GetComponent(18);
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
			UnitHangar component6 = this.mEntity.GetComponent(23);
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
			List<WeaponFireOperation> listComponent2 = this.mEntity.GetListComponent(6);
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

		// Token: 0x06001452 RID: 5202 RVA: 0x00070EE4 File Offset: 0x0006F0E4
		private static void CreateUnitRelatedComponents(Entity unitEntity)
		{
			unitEntity.AddComponent(28, StatusEffects.Create(unitEntity));
			UnitMovementAttributes typeAttributes = unitEntity.GetTypeAttributes<UnitMovementAttributes>();
			if (typeAttributes != null)
			{
				if (typeAttributes.ReversePolarity.Enabled)
				{
					Fixed64 repelRadius = Fixed64.Half * typeAttributes.ReversePolarity.PushRadiusMultiplier * typeAttributes.Dynamics.Length;
					unitEntity.AddComponent(34, ReversePolarity.Create(repelRadius, typeAttributes.ReversePolarity.RelativeWeight, typeAttributes.ReversePolarity.SquishinessFactor));
				}
				if (typeAttributes.Dynamics != null && typeAttributes.Dynamics.PermanentlyImmobile)
				{
					StatusEffectProcessor.AddModifierStack(unitEntity, ref StatusEffectProcessor.sImmobilizeModifierAttributes);
				}
			}
			UnitAttributes typeAttributes2 = unitEntity.GetTypeAttributes<UnitAttributes>();
			if (typeAttributes2 != null)
			{
				unitEntity.AddComponent(7, Health.Create(typeAttributes2.MaxHealth, typeAttributes2.MaxHealth));
				if (typeAttributes2.NumProductionQueues > 0)
				{
					unitEntity.AddComponent(15, Production.Create(unitEntity, typeAttributes2));
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
			unitEntity.AddComponent(4, Targets.Create());
			unitEntity.AddComponent(19, EntityGoalsCollection.Create(unitEntity));
			DetectableAttributes typeAttributes3 = unitEntity.GetTypeAttributes<DetectableAttributes>();
			if (typeAttributes3 != null)
			{
				unitEntity.AddComponent(21, Detectable.Create(typeAttributes3, unitEntity));
			}
			UnitQuadrantDamageAttributes typeAttributes4 = unitEntity.GetTypeAttributes<UnitQuadrantDamageAttributes>();
			if (typeAttributes4 != null)
			{
				unitEntity.AddComponent(30, QuadrantDamage.Create(typeAttributes4));
			}
			UnitHangarAttributes typeAttributes5 = unitEntity.GetTypeAttributes<UnitHangarAttributes>();
			if (typeAttributes5 != null)
			{
				unitEntity.AddComponent(23, UnitHangar.Create(unitEntity, typeAttributes5));
			}
			HarvesterAttributes typeAttributes6 = unitEntity.GetTypeAttributes<HarvesterAttributes>();
			if (typeAttributes6 != null && !unitEntity.HasComponent(16))
			{
				unitEntity.AddComponent(16, Harvester.Create(unitEntity));
			}
			ExperienceAttributes typeAttributes7 = unitEntity.GetTypeAttributes<ExperienceAttributes>();
			if (typeAttributes7 != null)
			{
				unitEntity.AddComponent(39, Experience.Create(unitEntity, typeAttributes7));
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
					unitEntity.AddComponent(18, Storage.Create(unitEntity, list, typeAttributes8.LinkToPlayerBank, typeAttributes8.IsResourceController));
				}
			}
			PowerShuntAttributes typeAttributes9 = unitEntity.GetTypeAttributes<PowerShuntAttributes>();
			if (typeAttributes9 != null && !typeAttributes9.PowerSystems.IsNullOrEmpty<PowerSystemAttributes>())
			{
				unitEntity.AddComponent(22, PowerShunt.Create(unitEntity, typeAttributes9));
			}
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x000711BA File Offset: 0x0006F3BA
		internal void Tick(Checksum check, Fixed64 tickPeriod)
		{
			this.AddChecksum(check);
			if (this.mPresentationOnly)
			{
				return;
			}
			this.UpdateGuards();
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x000711D4 File Offset: 0x0006F3D4
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
						EntityGoalsCollection component = entity.GetComponent(19);
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

		// Token: 0x06001455 RID: 5205 RVA: 0x000712F8 File Offset: 0x0006F4F8
		internal void DespawnUnit()
		{
			if (this.OnUnitDespawned != null)
			{
				this.OnUnitDespawned(this);
			}
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x00071310 File Offset: 0x0006F510
		private bool DestroyUnit(UnitRemoveReason reason, bool skipDeathSequence, CommanderID killingCommander, Entity killingEntity)
		{
			UnitHangar component = this.Entity.GetComponent(23);
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

		// Token: 0x06001457 RID: 5207 RVA: 0x00071383 File Offset: 0x0006F583
		public void UndockSequenceComplete(Unit undockedUnit, int bayId)
		{
			if (this.OnUnitUndocked != null)
			{
				this.OnUnitUndocked(undockedUnit, bayId);
			}
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x0007139C File Offset: 0x0006F59C
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

		// Token: 0x06001459 RID: 5209 RVA: 0x000713EB File Offset: 0x0006F5EB
		public void RetireDespawn()
		{
			ModifyHealthProcessor.DestroyEntity(this.Entity, UnitRemoveReason.Despawn);
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x000713FA File Offset: 0x0006F5FA
		private void ClearEntityPropertiesForDespawn()
		{
			this.SetStatus(UnitStatus.None);
			UnitManager.DeactivatePassiveAbilities(this);
			StatusEffectProcessor.ClearStatusEffects(this.Entity, true);
			EntityGoalsCollection.ClearGoals(this.Entity);
			Targets.ClearTargets(this.Entity);
			WeaponsProcessor.DeactivateWeaponsForUnit(this.Entity);
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x00071438 File Offset: 0x0006F638
		internal void AddChecksum(Checksum check)
		{
			check.Add(this.Entity.ToReference());
			Position component = this.Entity.GetComponent(10);
			if (component != null)
			{
				check.Add(component.Position2D.GetHashCode());
				check.Add(component.Orientation.GetHashCode());
			}
			Maneuvering component2 = this.Entity.GetComponent(12);
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
				Health component3 = this.Entity.GetComponent(7);
				check.Add(component3.CurrentHealth);
				check.Add(component3.MaxHealth);
				check.Add((int)component3.GodMode);
			}
			EntityGoalsCollection component4 = this.Entity.GetComponent(19);
			if (component4 != null && component4.CurrentGoal != null)
			{
				check.Add((int)component4.CurrentGoal.GoalType);
			}
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x00071581 File Offset: 0x0006F781
		void IWeaponTrigger.PrepareWeapon(WeaponFireOperation weaponFireOperation)
		{
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x00071583 File Offset: 0x0006F783
		void IWeaponTrigger.TriggerWeapon(Entity attackingEntity, WeaponFireOperation weaponFireOperation, WeaponFireTrigger weaponTrigger)
		{
			if (this.OnUnitFiredWeapon != null)
			{
				this.OnUnitFiredWeapon(this, weaponFireOperation, weaponTrigger);
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x0007159B File Offset: 0x0006F79B
		public bool PresentationOnly
		{
			get
			{
				return this.mPresentationOnly;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x0600145F RID: 5215 RVA: 0x000715A3 File Offset: 0x0006F7A3
		public int BonePositionOverrideIndex
		{
			get
			{
				return this.mBonePositionIndexOverride;
			}
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x000715AC File Offset: 0x0006F7AC
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
					this.mEntity.AddComponent(3, null);
				}
				else if (!value && this.mEntity.HasComponent(3))
				{
					this.mEntity.RemoveComponent(3);
				}
			}
			this.mPresentationOnly = value;
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0007161D File Offset: 0x0006F81D
		public void Die(UnitRemoveReason reason, bool skipDeathSequence, CommanderID killingCommanderID, Entity killingEntity)
		{
			this.DestroyUnit(reason, skipDeathSequence, killingCommanderID, killingEntity);
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0007162B File Offset: 0x0006F82B
		[CustomConverter(ConverterDirection.Save, ClassStateConversionOrder.RunStateDataConversionAfter)]
		private void OnSave(ref UnitSaveState state)
		{
			state.HasAGuardGroupAssigned = (this.EntityGuardGroup != null);
			if (state.HasAGuardGroupAssigned)
			{
				ExtractorManager.Save<EntityGuardGroup, EntityGuardGroupSaveState>(this.EntityGuardGroup, ref state.GuardGroup);
			}
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x00071658 File Offset: 0x0006F858
		[CustomConverter(ConverterDirection.Load, ClassStateConversionOrder.RunStateDataConversionAfter)]
		private void OnLoad(ref UnitSaveState state)
		{
			if (state.HasAGuardGroupAssigned)
			{
				ExtractorManager.Load<EntityGuardGroup, EntityGuardGroupSaveState>(ref this.EntityGuardGroup, state.GuardGroup);
			}
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x00071674 File Offset: 0x0006F874
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

		// Token: 0x0400108C RID: 4236
		private UnitAttributes mUnitAttributes;

		// Token: 0x0400108D RID: 4237
		private UnitMovementAttributes mUnitMovementAttributes;

		// Token: 0x0400108E RID: 4238
		[StateData("Entity", ConverterDirection.Save)]
		private readonly Entity mEntity;

		// Token: 0x0400108F RID: 4239
		public EntityGuardGroup EntityGuardGroup;

		// Token: 0x04001090 RID: 4240
		[StateData("DeathScheduled")]
		private bool mDeathScheduled;

		// Token: 0x04001091 RID: 4241
		[StateData("HasBeenSpawnedIntoWorldBefore")]
		internal bool HasBeenSpawnedIntoWorldBefore;

		// Token: 0x04001092 RID: 4242
		[StateData("PresentationOnly")]
		private bool mPresentationOnly;

		// Token: 0x04001093 RID: 4243
		[StateData("BonePositionIndexOverride")]
		private int mBonePositionIndexOverride = -1;

		// Token: 0x04001094 RID: 4244
		[StateData("UnitStatus")]
		private UnitStatus mUnitStatus;

		// Token: 0x04001095 RID: 4245
		[StateData("AttackingTimerMS")]
		internal int AttackingTimerMS;

		// Token: 0x04001096 RID: 4246
		[StateData("UnderAttackTimerMS")]
		internal int UnderAttackTimerMS;

		// Token: 0x04001097 RID: 4247
		[StateData("LastAttackedByEntity")]
		internal Entity LastAttackedByEntity;

		// Token: 0x04001098 RID: 4248
		public readonly UnitClass UnitClass;
	}
}
