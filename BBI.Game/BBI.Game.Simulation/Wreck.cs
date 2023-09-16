using System;
using System.Collections.Generic;
using BBI.Core.Collections;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Pathfinding;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Data;
using BBI.Game.Events;
using BBI.Game.SaveLoad;

namespace BBI.Game.Simulation
{
	public sealed class Wreck
	{
		internal Entity Entity
		{
			get
			{
				return this.mEntity;
			}
		}

		internal int RemainingHealth
		{
			get
			{
				return this.mRemainingHealth;
			}
		}

		internal WreckSection[] WreckSections
		{
			get
			{
				return this.mWreckSections;
			}
		}

		internal bool SpawnsArtifact
		{
			get
			{
				return !string.IsNullOrEmpty(this.mWreckArtifactType) && this.mWreckArtifactType != "NA";
			}
		}

		[ObjectConstructor(new string[]
		{
			"Entity",
			"WreckArtifactType",
			"WreckArtifactSpawnPositionOffsetX",
			"WreckArtifactSpawnPositionOffsetY",
			"WreckSections"
		})]
		private Wreck(Entity owner, string wreckArtifactType, int wreckArtifactSpawnPositionOffsetX, int wreckArtifactSpawnPositionOffsetY, WreckSection[] wreckSections)
		{
			this.mEntity = owner;
			this.mWreckArtifactType = wreckArtifactType;
			this.mWreckArtifactSpawnPositionOffsetX = wreckArtifactSpawnPositionOffsetX;
			this.mWreckArtifactSpawnPositionOffsetY = wreckArtifactSpawnPositionOffsetY;
			this.mWreckSections = wreckSections;
		}

		private Wreck(Entity owner, WreckSectionAttributes[] sections, string wreckArtifactType, int wreckArtifactSpawnPositionOffsetX, int wreckArtifactSpawnPositionOffsetY, string explosionWeaponTypeID)
		{
			this.mEntity = owner;
			this.mWreckArtifactType = wreckArtifactType;
			this.mWreckArtifactSpawnPositionOffsetX = wreckArtifactSpawnPositionOffsetX;
			this.mWreckArtifactSpawnPositionOffsetY = wreckArtifactSpawnPositionOffsetY;
			List<WreckSection> list = null;
			int num = 0;
			if (sections != null && sections.Length > 0)
			{
				list = new List<WreckSection>(sections.Length);
				for (int i = 0; i < sections.Length; i++)
				{
					WreckSectionAttributes wreckSectionAttributes = sections[i];
					if (wreckSectionAttributes == null)
					{
						Log.Error(Log.Channel.Data, "Null WreckSectionAttributes at index {0} for entity {1}", new object[]
						{
							i,
							owner.ToFriendlyString()
						});
					}
					else
					{
						WreckSection item = this.CreateWreckSection(i, wreckSectionAttributes);
						list.Add(item);
						num += wreckSectionAttributes.Health;
					}
				}
			}
			this.mWreckSections = ((list != null) ? list.ToArray() : null);
			this.mRemainingHealth = num;
			WeaponAttributes weaponAttributes = null;
			if (!string.IsNullOrEmpty(explosionWeaponTypeID))
			{
				weaponAttributes = Sim.GetEntityTypeAttributes<WeaponAttributes>(explosionWeaponTypeID);
				if (weaponAttributes == null)
				{
					Log.Error(Log.Channel.Data, "Failed to find WeaponAttributes for weapon type {0}. Is it missing from the EntityTypeRegistry?", new object[]
					{
						explosionWeaponTypeID
					});
				}
				else if (weaponAttributes.AreaOfEffectRadius <= Fixed64.Zero)
				{
					Log.Error(Log.Channel.Data, "Weapon {0} has non-positive AreaOfEffectRadius {1}!", new object[]
					{
						explosionWeaponTypeID,
						weaponAttributes.AreaOfEffectRadius
					});
				}
			}
			this.mWeaponAttributes = weaponAttributes;
		}

		[CustomConverter(ConverterDirection.Save, ClassStateConversionOrder.RunStateDataConversionAfter)]
		public void OnSave(ref WreckSaveState state)
		{
			state.ExplosionWeaponAttributes = ((this.mWeaponAttributes == null) ? string.Empty : this.mWeaponAttributes.Name);
		}

		[CustomConverter(ConverterDirection.Load, ClassStateConversionOrder.RunStateDataConversionAfter)]
		public void OnLoad(WreckSaveState state)
		{
			this.mWeaponAttributes = (string.IsNullOrEmpty(state.ExplosionWeaponAttributes) ? null : Sim.GetEntityTypeAttributes<WeaponAttributes>(state.ExplosionWeaponAttributes));
		}

		internal static Wreck Create(Entity owner, WreckSectionAttributes[] sectionAttributes, string wreckArtifactType, int wreckArtifactSpawnPositionOffsetX, int wreckArtifactSpawnPositionOffsetY, string explosionWeaponTypeID)
		{
			return new Wreck(owner, sectionAttributes, wreckArtifactType, wreckArtifactSpawnPositionOffsetX, wreckArtifactSpawnPositionOffsetY, explosionWeaponTypeID);
		}

		internal int CurrentSectionIndex()
		{
			if (this.mWreckSections != null)
			{
				for (int i = 0; i < this.mWreckSections.Length; i++)
				{
					if (this.mWreckSections[i].RemainingHealth > 0)
					{
						return i;
					}
				}
			}
			return -1;
		}

		internal int SalvageWreck(Entity harvester, int amount)
		{
			int num = 0;
			if (this.mRemainingHealth > 0)
			{
				int num2 = this.CurrentSectionIndex();
				if (num2 < 0)
				{
					return 0;
				}
				bool flag = true;
				foreach (WreckSection wreckSection in this.mWreckSections)
				{
					if (wreckSection.MaxHealth != wreckSection.RemainingHealth)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					Sim.PostEvent(new SalvageWreckProgressEvent(harvester, this.mEntity, num2, num, Entity.None, SalvageWreckProgressEvent.ProgressType.SectionSalvageWreckBegun));
				}
				WreckSection wreckSection2 = this.mWreckSections[num2];
				if (wreckSection2.RemainingHealth == wreckSection2.MaxHealth)
				{
					Sim.PostEvent(new SalvageWreckProgressEvent(harvester, this.mEntity, num2, num, Entity.None, SalvageWreckProgressEvent.ProgressType.SectionSalvageBegun));
				}
				else
				{
					Sim.PostEvent(new SalvageWreckProgressEvent(harvester, this.mEntity, num2, num, Entity.None, SalvageWreckProgressEvent.ProgressType.SectionSalvageProgress));
				}
				num = Math.Min(amount, wreckSection2.RemainingHealth);
				this.mRemainingHealth -= num;
				wreckSection2.RemainingHealth -= num;
				if (wreckSection2.RemainingHealth <= 0)
				{
					Sim.PostEvent(new SalvageWreckProgressEvent(harvester, this.mEntity, num2, num, Entity.None, SalvageWreckProgressEvent.ProgressType.SectionSalvageComplete));
					this.SpawnSectionResources(wreckSection2, num2);
					this.TriggerWreckSectionExplosion(wreckSection2, num2);
				}
				if (this.mRemainingHealth <= 0)
				{
					Entity spawnedArtifact = this.TrySpawnArtifact(harvester);
					Sim.PostEvent(new SalvageWreckProgressEvent(harvester, this.mEntity, num2, num, spawnedArtifact, SalvageWreckProgressEvent.ProgressType.SectionSalvageWreckFinished));
				}
			}
			return num;
		}

		private WreckSection CreateWreckSection(int sectionIndex, WreckSectionAttributes sectionAttributes)
		{
			List<WreckSectionResourceSpawnable> list = null;
			if (sectionAttributes.ResourceSpawnables != null && sectionAttributes.ResourceSpawnables.Length > 0)
			{
				list = new List<WreckSectionResourceSpawnable>(sectionAttributes.ResourceSpawnables.Length);
				for (int i = 0; i < sectionAttributes.ResourceSpawnables.Length; i++)
				{
					WreckSectionResourceSpawnableAttributes wreckSectionResourceSpawnableAttributes = sectionAttributes.ResourceSpawnables[i];
					if (wreckSectionResourceSpawnableAttributes == null)
					{
						Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Null WreckSectionResourceSpawnableAttributes at index {0} for wreck section index {1} for entity {2}!", new object[]
						{
							i,
							sectionIndex,
							this.mEntity.ToFriendlyString()
						});
					}
					else
					{
						WreckSectionResourceSpawnable item = new WreckSectionResourceSpawnable(wreckSectionResourceSpawnableAttributes.TypeIDToSpawn, wreckSectionResourceSpawnableAttributes.Tags, wreckSectionResourceSpawnableAttributes.SpawnPositionOffsetRandomDistanceMin, wreckSectionResourceSpawnableAttributes.SpawnPositionOffsetRandomDistanceMax, wreckSectionResourceSpawnableAttributes.UseNonRandomSpawnPositionOffset, wreckSectionResourceSpawnableAttributes.SpawnPositionOffsetFromSectionCenterX, wreckSectionResourceSpawnableAttributes.SpawnPositionOffsetFromSectionCenterY, wreckSectionResourceSpawnableAttributes.SpawnOrientationOffsetRandomDegreesMin, wreckSectionResourceSpawnableAttributes.SpawnOrientationOffsetRandomDegreesMax, wreckSectionResourceSpawnableAttributes.UseOverriddenResourceAttributes, wreckSectionResourceSpawnableAttributes.OverriddenResourceAttributes, wreckSectionResourceSpawnableAttributes.UseOverriddenDetectableAttributes, wreckSectionResourceSpawnableAttributes.OverriddenDetectableAttributes);
						list.Add(item);
					}
				}
			}
			return new WreckSection(sectionIndex, sectionAttributes.Health, sectionAttributes.Health, sectionAttributes.ExplosionChance, sectionAttributes.OffsetFromWreckCenterInLocalSpace, (list != null) ? list.ToArray() : null);
		}

		private void SpawnSectionResources(WreckSection wreckSection, int sectionIndex)
		{
			WreckSectionResourceSpawnable[] spawnables = wreckSection.Spawnables;
			if (spawnables != null && spawnables.Length > 0)
			{
				Position component = this.mEntity.GetComponent<Position>(10);
				if (component == null)
				{
					Log.Error(Log.Channel.Gameplay, "Unable to spawn resources for wreck section index {0} for wreck entity {1}, due to missing Position!", new object[]
					{
						sectionIndex,
						this.mEntity.ToFriendlyString()
					});
					return;
				}
				for (int i = 0; i < spawnables.Length; i++)
				{
					WreckSectionResourceSpawnable wreckSectionResourceSpawnable = spawnables[i];
					if (wreckSectionResourceSpawnable == null)
					{
						Log.Error(Log.Channel.Gameplay, "Null WreckSectionResourceSpawnable at index {0} for wreck section index {1} for wreck entity {2}!", new object[]
						{
							i,
							sectionIndex,
							this.mEntity.ToFriendlyString()
						});
					}
					else
					{
						Vector2r b;
						if (wreckSectionResourceSpawnable.UseNonRandomSpawnPositionOffset)
						{
							Vector2r vector2r = new Vector2r(Fixed64.FromInt(wreckSectionResourceSpawnable.SpawnPositionOffsetFromSectionCenterX), Fixed64.FromInt(wreckSectionResourceSpawnable.SpawnPositionOffsetFromSectionCenterY));
							vector2r = component.Orientation * vector2r;
							b = vector2r;
						}
						else
						{
							Fixed64 radius = Fixed64.FromInt(Sim.RandRange(wreckSectionResourceSpawnable.SpawnPositionOffsetRandomDistanceMin, wreckSectionResourceSpawnable.SpawnPositionOffsetRandomDistanceMax));
							Fixed64 radians = Fixed64.TwoPI * Sim.Rand01();
							Vector2r vector2r2 = Vector2r.PointOnCircle(radians, radius);
							b = vector2r2;
						}
						Vector2r a = component.Position2D + component.Orientation * wreckSection.OffsetFromWreckCenterInLocalSpace;
						Vector2r vector2r3 = a + b;
						Vector2r modelSpawnPosition;
						if (this.IsSpawnPositionValidForWreckSpawnable(vector2r3, out modelSpawnPosition))
						{
							Fixed64 y = Fixed64.FromInt(Sim.RandRange(wreckSectionResourceSpawnable.SpawnOrientationOffsetRandomDegreesMin, wreckSectionResourceSpawnable.SpawnOrientationOffsetRandomDegreesMax));
							Orientation2 orientation = component.Orientation;
							orientation.RotatedBy(Fixed64.DegreesToRadians * y);
							ResourceAttributes resourceAttributes = wreckSectionResourceSpawnable.UseOverriddenResourceAttributes ? wreckSectionResourceSpawnable.OverriddenResourceAttributes : Sim.GetEntityTypeAttributes<ResourceAttributes>(wreckSectionResourceSpawnable.TypeIDToSpawn);
							DetectableAttributes detectableAttributes = wreckSectionResourceSpawnable.UseOverriddenDetectableAttributes ? wreckSectionResourceSpawnable.OverriddenDetectableAttributes : Sim.GetEntityTypeAttributes<DetectableAttributes>(wreckSectionResourceSpawnable.TypeIDToSpawn);
							Vector3r modelOrientationEulersDegrees = new Vector3r(Fixed64.Zero, orientation.AngleTo(Orientation2.LocalForward) * Fixed64.RadiansToDegrees, Fixed64.Zero);
							ResourcePositionalVariations positionalVariations = new ResourcePositionalVariations(modelOrientationEulersDegrees, false, Fixed64.Zero, Vector2r.Zero, Fixed64.Zero);
							Entity entity = SceneEntityCreator.CreateResourcePoint(wreckSectionResourceSpawnable.TypeIDToSpawn, modelSpawnPosition, orientation, wreckSectionResourceSpawnable.Tags, resourceAttributes, detectableAttributes, false, positionalVariations, false);
							if (!entity.IsValid())
							{
								Log.Error(Log.Channel.Gameplay, "Failed to spawn a section resource of type {0} for wreck section index {1}", new object[]
								{
									wreckSectionResourceSpawnable.TypeIDToSpawn,
									sectionIndex
								});
							}
						}
						else
						{
							Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Failed to find valid map position trying to spawn WreckSectionResourceSpawnable entity type {0} at index {1} for wreck section {2}! Make sure it isn't being spawned inside an obstacle! Spawn position = {3}.", new object[]
							{
								wreckSectionResourceSpawnable.TypeIDToSpawn,
								i,
								wreckSection.WreckSectionIndex,
								vector2r3
							});
						}
					}
				}
			}
		}

		private Entity TrySpawnArtifact(Entity extractingEntity)
		{
			Entity entity = Entity.None;
			if (this.SpawnsArtifact)
			{
				WreckArtifactAttributes entityTypeAttributes = Sim.GetEntityTypeAttributes<WreckArtifactAttributes>(this.mWreckArtifactType);
				if (entityTypeAttributes != null)
				{
					CommanderID entityCommanderID = Sim.GetEntityCommanderID(extractingEntity);
					Commander commanderFromID = Sim.Instance.CommanderManager.GetCommanderFromID(entityCommanderID);
					if (commanderFromID != null)
					{
						if (!commanderFromID.HasArtifact(this.mWreckArtifactType))
						{
							Position position = this.mEntity.IsValid() ? this.mEntity.GetComponent<Position>(10) : null;
							if (position != null)
							{
								Vector2r vector = new Vector2r(Fixed64.FromInt(this.mWreckArtifactSpawnPositionOffsetX), Fixed64.FromInt(this.mWreckArtifactSpawnPositionOffsetY));
								Vector2r vector2r = position.Position2D + position.Orientation * vector;
								Vector2r vector2r2;
								if (this.IsSpawnPositionValidForWreckSpawnable(vector2r, out vector2r2))
								{
									entity = SceneEntityCreator.CreateCollectibleEntity(this.mWreckArtifactType, CollectibleType.WreckArtifact, vector2r2, position.Orientation);
									if (entity.IsValid())
									{
										CollectibleEntityProcessor.ExtractCollectible(entity, extractingEntity, vector2r2);
										UnitHangar component = extractingEntity.GetComponent<UnitHangar>(23);
										if (component != null && component.StoresEntityType(UnitHangar.kWreckArtifactTypeName) && CollectibleEntityProcessor.CollectCollectibleEntity(entity, extractingEntity))
										{
											Harvester component2 = extractingEntity.GetComponent<Harvester>(16);
											if (component2 != null)
											{
												HarvesterProcessor.StopHarvesting(component2);
												component2.IdleOnReturn = true;
												component2.TargetResourceController = Entity.None;
												component2.ForceTargetResourceController = false;
												HarvesterProcessor.SetCurrentState(component2, ResourcingState.ReturnToDeposit);
											}
										}
									}
								}
								else
								{
									Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Failed to find valid map position trying to spawn wreck artifact entity type {0}! Make sure it isn't being spawned inside an obstacle! Spawn position = {1}.", new object[]
									{
										this.mWreckArtifactType,
										vector2r
									});
								}
							}
						}
					}
					else
					{
						Log.Error(Log.Channel.Data, "[wreck] SpawnArtifact Could not find a valid commander.. ID:{0} from the unit responsible for finishing the wreck.", new object[]
						{
							entityCommanderID.ID
						});
					}
				}
				else
				{
					Log.Error(Log.Channel.Data, "[wreck] Artifact WreckArtifactType {0} was not found in the EntityTypeRegistry.  is it misspelled? or did you forget to add it..", new object[]
					{
						this.mWreckArtifactType
					});
				}
			}
			return entity;
		}

		private bool IsSpawnPositionValidForWreckSpawnable(Vector2r spawnPosition, out Vector2r validPosition)
		{
			NavMeshAttributes navMeshAttributes = (Sim.Instance.Settings != null) ? Sim.Instance.Settings.WreckSpawnableNavMesh : null;
			if (navMeshAttributes != null)
			{
				PathNodeNavigationCache navMeshFor = Sim.Instance.Map.GetNavMeshFor(navMeshAttributes);
				Vector2r vector2r;
				if (navMeshFor != null && navMeshFor.NearbyValidPosition(spawnPosition, out vector2r))
				{
					validPosition = vector2r;
					return true;
				}
			}
			else
			{
				Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Missing WreckSpawnableNavMesh. Unable to validate spawn position for a spawnable for wreck entity {0}!", new object[]
				{
					this.mEntity.ToFriendlyString()
				});
			}
			validPosition = spawnPosition;
			return false;
		}

		private void TriggerWreckSectionExplosion(WreckSection wreckSection, int sectionIndex)
		{
			int explosionChance = wreckSection.ExplosionChance;
			if (explosionChance > 0)
			{
				Fixed64 x = Sim.Rand(Fixed64.OneHundred);
				if (x <= Fixed64.FromInt(explosionChance))
				{
					Sim.PostEvent(new SalvageWreckProgressEvent(Entity.None, this.mEntity, sectionIndex, 0, Entity.None, SalvageWreckProgressEvent.ProgressType.SectionSalvageExplosion));
					if (this.mWeaponAttributes != null)
					{
						if (this.mEntity.IsValid())
						{
							Position component = this.mEntity.GetComponent<Position>(10);
							ShotSenderState shotSender = new ShotSenderState(this.mEntity, this.mEntity, this.mWeaponAttributes, component.Position2D, Orientation2.LocalForward, true, 0);
							WeaponsProcessor.PerformAoEImpact(shotSender, component.Position2D);
							return;
						}
						Log.Error(Log.Channel.Data, "[wreck] invalid Explosion attempt. Wreck entity is invalid:", new object[]
						{
							this.mEntity.ToFriendlyString()
						});
						return;
					}
					else
					{
						Log.Error(Log.Channel.Data, "[wreck] invalid Explosion Weapon Attribute Data for: {0}. section:{1}. possibly not added to EntityTypeRegistry", new object[]
						{
							this.mEntity.ToFriendlyString(),
							sectionIndex
						});
					}
				}
			}
		}

		internal int GetChecksum()
		{
			int num = Checksum.Combine(this.mRemainingHealth, this.mWreckArtifactType.GetHashCode());
			if (this.mWreckSections != null)
			{
				for (int i = 0; i < this.mWreckSections.Length; i++)
				{
					num = Checksum.Combine(num, this.mWreckSections[i].GetChecksum());
				}
			}
			return num;
		}

		[StateData("Entity", ConverterDirection.Save)]
		private readonly Entity mEntity = Entity.None;

		[StateData("RemainingHealth")]
		public int mRemainingHealth;

		[StateData("WreckArtifactType", ConverterDirection.Save)]
		private readonly string mWreckArtifactType = string.Empty;

		[StateData("WreckArtifactSpawnPositionOffsetX", ConverterDirection.Save)]
		private readonly int mWreckArtifactSpawnPositionOffsetX;

		[StateData("WreckArtifactSpawnPositionOffsetY", ConverterDirection.Save)]
		private readonly int mWreckArtifactSpawnPositionOffsetY;

		[StateData("WreckSections", ConverterDirection.Save)]
		private readonly WreckSection[] mWreckSections = EmptyArray<WreckSection>.Empty;

		private WeaponAttributes mWeaponAttributes;
	}
}
