using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Data;
using BBI.Game.Events;
using BBI.Game.Queries;

namespace BBI.Game.Simulation
{
	// Token: 0x02000396 RID: 918
	internal static class SceneEntityCreator
	{
		// Token: 0x06001275 RID: 4725 RVA: 0x00064200 File Offset: 0x00062400
		internal static Entity CreateCollectibleEntity(string entityType, CollectibleType collectibleType, Vector2r spawnPosition, Orientation2 spawnOrientation)
		{
			Entity entity = Entity.None;
			switch (collectibleType)
			{
			case CollectibleType.Artifact:
				entity = SceneEntityCreator.CreateRelic(entityType, spawnPosition, spawnOrientation, null, null);
				goto IL_4F;
			case CollectibleType.WreckArtifact:
				entity = SceneEntityCreator.CreateWreckArtifact(entityType, spawnPosition, spawnOrientation);
				goto IL_4F;
			}
			Log.Error(Log.Channel.Gameplay, "Unsupported Entity type found in descriptor!", new object[0]);
			entity = Entity.None;
			IL_4F:
			if (entity.IsValid())
			{
				Sim.PostEvent(new CollectibleEntityCreatedEvent(entity, entityType, collectibleType, spawnPosition, spawnOrientation));
			}
			return entity;
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x00064274 File Offset: 0x00062474
		internal static Entity CreateEntityFromDescriptor(SceneEntityDescriptor descriptor, ref int artifactsCreated)
		{
			if (descriptor == null)
			{
				return Entity.None;
			}
			Entity entity = Entity.None;
			switch (descriptor.EntityType)
			{
			case SceneEntityType.Base:
				entity = SceneEntityCreator.CreateBasicEntity(descriptor.TypeID, descriptor.Position, descriptor.Orientation, descriptor.Tags);
				goto IL_E2;
			case SceneEntityType.TriggerCircle:
				entity = SceneEntityCreator.CreateTriggerCircle((TriggerCircleDescriptor)descriptor);
				goto IL_E2;
			case SceneEntityType.ResourcePoint:
			{
				ResourcePointDescriptor resourcePointDescriptor = (ResourcePointDescriptor)descriptor;
				if (resourcePointDescriptor.ResourceAttributes.ResourceType == ResourceType.Resource3)
				{
					WreckDescriptor descriptor2 = descriptor as WreckDescriptor;
					entity = SceneEntityCreator.CreateWreckFromDescriptor(descriptor2);
					goto IL_E2;
				}
				entity = SceneEntityCreator.CreateResourcePointFromDescriptor(resourcePointDescriptor);
				goto IL_E2;
			}
			case SceneEntityType.Relic:
				entity = SceneEntityCreator.CreateRelicFromDescriptor((RelicDescriptor)descriptor, ref artifactsCreated);
				goto IL_E2;
			case SceneEntityType.ExtractionZone:
				entity = SceneEntityCreator.CreateExtractionZoneEntity((ExtractionZoneDescriptor)descriptor);
				goto IL_E2;
			case SceneEntityType.AIHint:
				entity = SceneEntityCreator.CreateAIHint((AIHintDescriptor)descriptor);
				goto IL_E2;
			}
			Log.Error(Log.Channel.Gameplay, "Unsupported Entity type found in descriptor!", new object[0]);
			entity = Entity.None;
			IL_E2:
			if (entity.IsValid())
			{
				Sim.PostEvent(new SceneEntityCreatedEvent(entity, descriptor));
			}
			return entity;
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x00064378 File Offset: 0x00062578
		internal static Entity CreateEntity(string typeID, CommanderID commanderID, Vector2r spawnPosition, Orientation2 spawnOrientation)
		{
			Entity entity = Entity.None;
			if (!Sim.IsValidEntityType(typeID))
			{
				Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Tried to create invalid entity type name {0}! Is it in the entity type registry?", new object[]
				{
					typeID
				});
				return entity;
			}
			EntityTypeAttributes entityType = Sim.Instance.Settings.EntityTypes.GetEntityType(typeID);
			bool flag = entityType.Get<UnitAttributes>() != null;
			if (flag)
			{
				Sim.Instance.UnitManager.SpawnNewUnitIntoWorld(typeID, commanderID, spawnPosition, spawnOrientation, out entity, null);
				return entity;
			}
			bool flag2 = entityType.Get<ProjectileAttributes>() != null;
			if (flag2)
			{
				Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Spawning a projectile entity in this manner is currently not supported! Failed to spawn projectile entity type {0}.", new object[]
				{
					typeID
				});
				return entity;
			}
			entity = SceneEntityCreator.CreateBasicEntity(typeID, spawnPosition, spawnOrientation, null);
			if (entity.IsValid())
			{
				ShapeAttributes typeAttributes = entity.GetTypeAttributes<ShapeAttributes>();
				if (typeAttributes != null)
				{
					Shape component = Shape.Create(spawnPosition, spawnOrientation, typeAttributes.Radius, typeAttributes.Length, typeAttributes.BlocksLOF, typeAttributes.BlocksAllHeights, typeAttributes.WorldHeightOffset);
					entity.AddComponent(33, component);
				}
				TimerAttributes typeAttributes2 = entity.GetTypeAttributes<TimerAttributes>();
				if (typeAttributes2 != null)
				{
					Timer component2 = Timer.Create(entity, typeAttributes2.DurationSeconds, typeAttributes2.Direction, typeAttributes2.ActionOnTimerComplete);
					entity.AddComponent(26, component2);
				}
			}
			return entity;
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x000644AC File Offset: 0x000626AC
		private static Entity CreateBasicEntity(string typeID, Vector2r position, Orientation2 orientation, string[] tags)
		{
			Entity entity = Sim.CreateCommanderlessEntity(typeID);
			if (entity.IsValid())
			{
				entity.AddComponent(10, Position.Create(position, orientation, Fixed64.Zero));
				Sim.Instance.Map.SpatialHash.InsertEntity(entity);
				if (tags != null && tags.Length > 0)
				{
					entity.AddComponent(1, Tags.Create(tags));
				}
			}
			return entity;
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00064508 File Offset: 0x00062708
		private static Entity CreateRelicFromDescriptor(RelicDescriptor descriptor, ref int artifactsCreated)
		{
			Entity result = Entity.None;
			if (descriptor.RelicProbabilityAttribs != null)
			{
				bool flag = true;
				string text = string.Empty;
				if (flag)
				{
					text = CollectibleEntityProcessor.RollForRelicEntityToSpawn(descriptor.RelicProbabilityAttribs.ArtifactProbabilities);
				}
				if (!string.IsNullOrEmpty(text))
				{
					artifactsCreated++;
				}
				else
				{
					Log.Warn(Log.Channel.Gameplay, "As of February 2015, expecting to only spawn Artifacts! Failed to spawn an artifact!", new object[0]);
					text = CollectibleEntityProcessor.RollForRelicEntityToSpawn(descriptor.RelicProbabilityAttribs.TechProbabilities);
				}
				if (string.IsNullOrEmpty(text))
				{
					Log.Warn(Log.Channel.Gameplay, "Failed to choose a relic entity type, or missing relic entity reference in probabilities, for relic {0}", new object[]
					{
						descriptor.SceneObject
					});
				}
				else
				{
					result = SceneEntityCreator.CreateRelic(text, descriptor.Position, descriptor.Orientation, descriptor.Tags, descriptor);
				}
			}
			else
			{
				Log.Error(Log.Channel.Gameplay, "Tried to create a relic with null RelicProbabilityAttributes!", new object[0]);
			}
			return result;
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x000645D4 File Offset: 0x000627D4
		private static Entity CreateRelic(string relicTypeID, Vector2r spawnPosition, Orientation2 spawnOrientation, string[] tags, RelicDescriptor descriptor)
		{
			Entity entity = SceneEntityCreator.CreateBasicEntity(relicTypeID, spawnPosition, spawnOrientation, tags);
			if (entity.IsValid())
			{
				RelicAttributes typeAttributes = entity.GetTypeAttributes<RelicAttributes>();
				if (typeAttributes != null)
				{
					Fixed64 extractionDuration = Fixed64.FromInt(Sim.Instance.Settings.GameMode.GameSessionSettings.GameModeSettings.Retrieval.ArtifactExtractionTimeSeconds);
					Relic component = Relic.Create(entity, relicTypeID, extractionDuration);
					entity.AddComponent(25, component);
					Collectible component2 = Collectible.Create(entity);
					entity.AddComponent(36, component2);
					Fixed64 @fixed = Fixed64.FromInt(Sim.Instance.Settings.GameMode.GameSessionSettings.GameModeSettings.Retrieval.ArtifactInitialSpawnTimeSeconds);
					Fixed64 fixed2 = Fixed64.FromInt(Sim.Instance.Settings.GameMode.GameSessionSettings.GameModeSettings.Retrieval.ArtifactRespawnTimeSeconds);
					HashSet<Entity> hashSet = (descriptor != null) ? DynamicSceneSpawner.GetSimEntitiesFromDescriptor(descriptor) : null;
					Fixed64 durationSeconds = (hashSet == null || hashSet.Count == 0) ? @fixed : fixed2;
					Timer component3 = Timer.Create(entity, durationSeconds, TimerDirection.Countup, OnTimerCompleteAction.None);
					entity.AddComponent(26, component3);
					if (descriptor != null)
					{
						DynamicSceneSpawner.RegisterSceneEntity(entity, 0, descriptor);
						if (!DynamicSceneSpawner.RegisterDescriptorWithEntity(descriptor, entity))
						{
							Log.Error(Log.Channel.Gameplay, "Failed to link entity {0} to the descriptor it was created from!", new object[]
							{
								entity.ToFriendlyString()
							});
						}
					}
				}
				else
				{
					Log.Error(Log.Channel.Data, "Missing RelicAttributes when trying to create relic entity type {0}", new object[]
					{
						relicTypeID
					});
				}
				DetectableAttributes typeAttributes2 = entity.GetTypeAttributes<DetectableAttributes>();
				if (typeAttributes2 != null)
				{
					Detectable component4 = Detectable.Create(typeAttributes2, entity);
					entity.AddComponent(21, component4);
				}
			}
			return entity;
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00064764 File Offset: 0x00062964
		private static Entity CreateWreckArtifact(string entityType, Vector2r spawnPosition, Orientation2 spawnOrientation)
		{
			Entity entity = SceneEntityCreator.CreateBasicEntity(entityType, spawnPosition, spawnOrientation, null);
			if (entity.IsValid())
			{
				WreckArtifactAttributes typeAttributes = entity.GetTypeAttributes<WreckArtifactAttributes>();
				if (typeAttributes != null)
				{
					WreckArtifact component = WreckArtifact.Create(entity, typeAttributes.Name, typeAttributes.AnalysisTime);
					Collectible component2 = Collectible.Create(entity);
					entity.AddComponent(36, component2);
					entity.AddComponent(35, component);
				}
				else
				{
					Log.Error(Log.Channel.Data, "Missing WreckArtifactAttributes when trying to create wreckArtifact entity type {0}", new object[]
					{
						entityType
					});
				}
				DetectableAttributes typeAttributes2 = entity.GetTypeAttributes<DetectableAttributes>();
				if (typeAttributes2 != null)
				{
					Detectable component3 = Detectable.Create(typeAttributes2, entity);
					entity.AddComponent(21, component3);
				}
			}
			return entity;
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x000647F8 File Offset: 0x000629F8
		private static Entity CreateResourcePointFromDescriptor(ResourcePointDescriptor descriptor)
		{
			Orientation2 simSpawnOrientation = descriptor.Orientation.RotatedBy(descriptor.SimLocalSpawnOrientationOffsetDegrees * Fixed64.DegreesToRadians);
			ResourcePositionalVariations positionalVariations = new ResourcePositionalVariations(descriptor.ModelOrientationEulersDegrees, descriptor.UseHeight, descriptor.Height, descriptor.SimLocalSpawnPositionOffset, descriptor.SimLocalSpawnOrientationOffsetDegrees);
			return SceneEntityCreator.CreateResourcePoint(descriptor.TypeID, descriptor.Position, simSpawnOrientation, descriptor.Tags, descriptor.ResourceAttributes, descriptor.DetectableAttributes, !descriptor.StartEnabled, positionalVariations, false);
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0006487C File Offset: 0x00062A7C
		internal static Entity CreateResourcePoint(string typeID, Vector2r modelSpawnPosition, Orientation2 simSpawnOrientation, string[] tags, ResourceAttributes resourceAttributes, DetectableAttributes detectableAttributes, bool startDisabled, ResourcePositionalVariations positionalVariations, bool skipResourceEntityCreatedEvent = false)
		{
			Vector2r position = modelSpawnPosition + simSpawnOrientation * positionalVariations.SimLocalSpawnPositionOffset;
			Entity entity = SceneEntityCreator.CreateBasicEntity(typeID, position, simSpawnOrientation, tags);
			if (entity.IsValid())
			{
				EntityTypeAttributes entityTypeAttributes = entity.GetTypeAttributes();
				entity.RemoveComponent(0);
				entityTypeAttributes = new EntityTypeAttributes(entityTypeAttributes);
				entity.AddComponent(0, entityTypeAttributes);
				if (resourceAttributes != null)
				{
					ResourceAttributes typeAttributes = entity.GetTypeAttributes<ResourceAttributes>();
					if (entityTypeAttributes != null && typeAttributes != null)
					{
						entityTypeAttributes.Replace(typeAttributes, resourceAttributes);
					}
					else
					{
						Log.Error(Log.Channel.Gameplay, "Failed to get EntityTypeAttributes or default ResourceAttributes for resource entity type {0}", new object[]
						{
							typeID
						});
					}
					Resource component = Resource.Create(entity, resourceAttributes.StartingAmount, resourceAttributes.StartingAmount, resourceAttributes.ResourceType, resourceAttributes.MaxHarvesters, startDisabled, positionalVariations);
					entity.AddComponent(11, component);
				}
				else
				{
					Log.Error(Log.Channel.Data, "Missing ResourceAttributes when trying to create resource entity type {0}", new object[]
					{
						typeID
					});
				}
				if (detectableAttributes != null)
				{
					DetectableAttributes typeAttributes2 = entity.GetTypeAttributes<DetectableAttributes>();
					if (entityTypeAttributes != null && typeAttributes2 != null)
					{
						entityTypeAttributes.Replace(typeAttributes2, detectableAttributes);
					}
					else
					{
						Log.Error(Log.Channel.Gameplay, "Failed to get EntityTypeAttributes or default DetectableAttributes for resource entity type {0}", new object[]
						{
							typeID
						});
					}
					Detectable component2 = Detectable.Create(detectableAttributes, entity);
					entity.AddComponent(21, component2);
				}
				if (!skipResourceEntityCreatedEvent)
				{
					Sim.PostEvent(new ResourceEntityCreatedEvent(entity, typeID, modelSpawnPosition, positionalVariations.UseHeight, positionalVariations.Height, positionalVariations.ModelOrientationEulersDegrees));
				}
			}
			return entity;
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x000649D4 File Offset: 0x00062BD4
		private static Entity CreateWreckFromDescriptor(WreckDescriptor descriptor)
		{
			Orientation2 simSpawnOrientation = descriptor.Orientation.RotatedBy(descriptor.SimLocalSpawnOrientationOffsetDegrees * Fixed64.DegreesToRadians);
			ResourcePositionalVariations positionalVariations = new ResourcePositionalVariations(descriptor.ModelOrientationEulersDegrees, descriptor.UseHeight, descriptor.Height, descriptor.SimLocalSpawnPositionOffset, descriptor.SimLocalSpawnOrientationOffsetDegrees);
			Entity entity = SceneEntityCreator.CreateWreck(descriptor.TypeID, descriptor.Position, simSpawnOrientation, descriptor.Tags, descriptor.WreckAttributes, descriptor.WreckArtifactType, descriptor.ShapeAttributes, descriptor.ResourceAttributes, descriptor.DetectableAttributes, !descriptor.StartEnabled, positionalVariations, false);
			SceneEntityCreator.LinkSimEntityToDescriptor(descriptor, entity);
			return entity;
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00064A70 File Offset: 0x00062C70
		internal static Entity CreateWreck(string typeID, Vector2r modelSpawnPosition, Orientation2 simSpawnOrientation, string[] tags, WreckAttributes wreckAttributes, string wreckArtifactType, ShapeAttributes shapeAttributes, ResourceAttributes resourceAttributes, DetectableAttributes detectableAttributes, bool startDisabled, ResourcePositionalVariations positionalVariations, bool skipResourceEntityCreatedEvent = false)
		{
			Vector2r vector2r = modelSpawnPosition + simSpawnOrientation * positionalVariations.SimLocalSpawnPositionOffset;
			Entity entity = SceneEntityCreator.CreateBasicEntity(typeID, vector2r, simSpawnOrientation, tags);
			if (entity.IsValid())
			{
				EntityTypeAttributes entityTypeAttributes = entity.GetTypeAttributes();
				entity.RemoveComponent(0);
				entityTypeAttributes = new EntityTypeAttributes(entityTypeAttributes);
				entity.AddComponent(0, entityTypeAttributes);
				if (wreckAttributes != null)
				{
					WreckAttributes typeAttributes = entity.GetTypeAttributes<WreckAttributes>();
					if (entityTypeAttributes != null && typeAttributes != null)
					{
						entityTypeAttributes.Replace(typeAttributes, wreckAttributes);
					}
					else
					{
						Log.Error(Log.Channel.Gameplay, "Failed to get EntityTypeAttributes or default WreckAttributes for wreck entity type {0}", new object[]
						{
							typeID
						});
					}
					Wreck wreck = Wreck.Create(entity, wreckAttributes.WreckSections, wreckArtifactType, wreckAttributes.WreckArtifactSpawnPositionOffsetX, wreckAttributes.WreckArtifactSpawnPositionOffsetY, wreckAttributes.ExplosionWeaponTypeID);
					entity.AddComponent(37, wreck);
					if (resourceAttributes != null)
					{
						ResourceAttributes typeAttributes2 = entity.GetTypeAttributes<ResourceAttributes>();
						if (entityTypeAttributes != null && typeAttributes2 != null)
						{
							entityTypeAttributes.Replace(typeAttributes2, resourceAttributes);
						}
						else
						{
							Log.Error(Log.Channel.Gameplay, "Failed to get EntityTypeAttributes or default ResourceAttributes for wreck entity type {0}", new object[]
							{
								typeID
							});
						}
						Resource component = Resource.Create(entity, wreck.RemainingHealth, wreck.RemainingHealth, resourceAttributes.ResourceType, resourceAttributes.MaxHarvesters, startDisabled, positionalVariations);
						entity.AddComponent(11, component);
					}
					else
					{
						Log.Error(Log.Channel.Data, "Missing ResourceAttributes when trying to create wreck entity type {0}", new object[]
						{
							typeID
						});
					}
				}
				else
				{
					Log.Error(Log.Channel.Data, "Missing WreckAttributes when trying to create wreck entity type {0}", new object[]
					{
						typeID
					});
				}
				if (shapeAttributes != null)
				{
					ShapeAttributes typeAttributes3 = entity.GetTypeAttributes<ShapeAttributes>();
					if (entityTypeAttributes != null && typeAttributes3 != null)
					{
						entityTypeAttributes.Replace(typeAttributes3, shapeAttributes);
					}
					else
					{
						Log.Error(Log.Channel.Gameplay, "Failed to get EntityTypeAttributes or default ShapeAttributes for wreck entity type {0}", new object[]
						{
							typeID
						});
					}
					Shape component2 = Shape.Create(vector2r, simSpawnOrientation, shapeAttributes.Radius, shapeAttributes.Length, shapeAttributes.BlocksLOF, shapeAttributes.BlocksAllHeights, shapeAttributes.WorldHeightOffset);
					entity.AddComponent(33, component2);
				}
				if (detectableAttributes != null)
				{
					DetectableAttributes typeAttributes4 = entity.GetTypeAttributes<DetectableAttributes>();
					if (entityTypeAttributes != null && typeAttributes4 != null)
					{
						entityTypeAttributes.Replace(typeAttributes4, detectableAttributes);
					}
					else
					{
						Log.Error(Log.Channel.Gameplay, "Failed to get EntityTypeAttributes or default DetectableAttributes for wreck entity type {0}", new object[]
						{
							typeID
						});
					}
					Detectable component3 = Detectable.Create(detectableAttributes, entity);
					entity.AddComponent(21, component3);
				}
				if (!skipResourceEntityCreatedEvent)
				{
					Sim.PostEvent(new ResourceEntityCreatedEvent(entity, typeID, modelSpawnPosition, positionalVariations.UseHeight, positionalVariations.Height, positionalVariations.ModelOrientationEulersDegrees));
				}
			}
			return entity;
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00064CC8 File Offset: 0x00062EC8
		private static bool LinkSimEntityToDescriptor(SceneEntityDescriptor descriptor, Entity simEntity)
		{
			return simEntity.IsValid() && descriptor.TypeID == simEntity.GetTypeName() && DynamicSceneSpawner.RegisterDescriptorWithEntity(descriptor, simEntity);
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00064CF0 File Offset: 0x00062EF0
		private static Entity CreateTriggerCircle(TriggerCircleDescriptor descriptor)
		{
			Entity entity = Entity.None;
			if (descriptor.TriggerRadius > Fixed64.Zero)
			{
				entity = SceneEntityCreator.CreateBasicEntity(descriptor.TypeID, descriptor.Position, descriptor.Orientation, descriptor.Tags);
				if (entity.IsValid())
				{
					entity.AddComponent(14, TriggerVolume.Create(entity, descriptor.TriggerRadius, descriptor.QueryContainer, descriptor.StartEnabled));
					if (descriptor.TriggeredByDetection)
					{
						entity.AddComponent(21, Detectable.Create(TriggerCircleDescriptor.DefaultTriggerVolumeDetectableAttributes, entity));
					}
				}
			}
			else
			{
				Log.Error(Log.Channel.Gameplay, "Tried to create a Trigger circle with radius {0}. Must be greater than zero!", new object[]
				{
					descriptor.TriggerRadius
				});
			}
			return entity;
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00064DA0 File Offset: 0x00062FA0
		internal static Entity CreateExtractionZoneEntity(ExtractionZoneDescriptor descriptor)
		{
			Entity entity = Entity.None;
			ExtractionZoneAttributes extractionZoneAttribs = descriptor.ExtractionZoneAttribs;
			if (extractionZoneAttribs != null)
			{
				if (extractionZoneAttribs.RadiusMeters > Fixed64.Zero)
				{
					entity = SceneEntityCreator.CreateBasicEntity(descriptor.TypeID, descriptor.Position, descriptor.Orientation, descriptor.Tags);
					if (!entity.IsValid())
					{
						return entity;
					}
					entity.AddComponent(41, null);
					QueryHelper.CreatePredicateFromQueryData(extractionZoneAttribs.QueryData);
					entity.AddComponent(14, TriggerVolume.Create(entity, extractionZoneAttribs.RadiusMeters, extractionZoneAttribs.QueryData, descriptor.StartEnabled));
					DynamicSceneSpawner.RegisterSceneEntity(entity, 0, descriptor);
					using (DictionaryExtensions.KeyIterator<CommanderID, Commander> enumerator = Sim.Instance.CommanderManager.CommanderIDs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							CommanderID commanderID = enumerator.Current;
							CommanderDirectorAttributes commanderDirectorFromID = Sim.Instance.CommanderManager.GetCommanderDirectorFromID(commanderID);
							if (commanderDirectorFromID != null)
							{
								int num = commanderDirectorFromID.TeamSelection - 1;
								if (commanderDirectorFromID.TeamSelection == TeamID.None.ID)
								{
									Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Extraction zones are per-team and require explicit team assignments! Commander {0} wasn't assigned a team! Unable to assign extraction zones to Commanders!", new object[]
									{
										commanderID
									});
								}
								else if (num == descriptor.TeamSpawnIndex)
								{
									Commander commanderFromID = Sim.Instance.CommanderManager.GetCommanderFromID(commanderID);
									if (commanderFromID != null)
									{
										if (commanderFromID.ExtractionZone != Entity.None)
										{
											Log.Warn(Log.Channel.Data | Log.Channel.Gameplay, "More than one ExtractionZone entity being created for spawn point index {0}! Commander {1} will be assigned the newest one", new object[]
											{
												descriptor.TeamSpawnIndex,
												commanderFromID.ID
											});
										}
										commanderFromID.SetExtractionZone(entity);
									}
								}
							}
						}
						return entity;
					}
				}
				Log.Error(Log.Channel.Gameplay, "Tried to create an Extraction Zone entity with radius {0}. Must be greater than zero!", new object[]
				{
					extractionZoneAttribs.RadiusMeters
				});
			}
			else
			{
				Log.Error(Log.Channel.Gameplay, "Tried to create an extraction zone entity with null ExtractionZoneAttributes!", new object[0]);
			}
			return entity;
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x00064F9C File Offset: 0x0006319C
		private static Entity CreateAIHint(AIHintDescriptor descriptor)
		{
			Entity entity = Entity.None;
			entity = SceneEntityCreator.CreateBasicEntity(descriptor.TypeID, descriptor.Position, descriptor.Orientation, descriptor.Tags);
			if (entity.IsValid())
			{
				AIHint component = AIHint.Create(descriptor.AIHint, descriptor.AIHintData);
				entity.AddComponent(29, component);
			}
			else
			{
				Log.Error(Log.Channel.Gameplay, "Tried to create an AIHint of type {0} and failed!", new object[]
				{
					descriptor.AIHint
				});
			}
			return entity;
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x00065018 File Offset: 0x00063218
		internal static void CreateSceneEntitiesForGameSession(IEnumerable<SceneEntityGroupDescriptor> groupDescriptors, IEnumerable<SceneEntityDescriptor> descriptors, IList<string> randomWreckArtifacts, int maxSpawnedWreckArtifacts, bool skipPersistentSceneEntities)
		{
			int num = 0;
			List<SceneEntityDescriptor> list = new List<SceneEntityDescriptor>();
			foreach (SceneEntityGroupDescriptor sceneEntityGroupDescriptor in groupDescriptors)
			{
				list.AddRange(sceneEntityGroupDescriptor.ChooseRandomEntitiesToExclude());
			}
			SceneEntityCreator.InjectRandomWreckArtifacts(descriptors, randomWreckArtifacts, maxSpawnedWreckArtifacts);
			foreach (SceneEntityDescriptor sceneEntityDescriptor in descriptors)
			{
				if (!skipPersistentSceneEntities || !sceneEntityDescriptor.SkipWhenPersisting)
				{
					if (list.Contains(sceneEntityDescriptor))
					{
						Sim.PostEvent(new HideSceneObjectEvent(sceneEntityDescriptor));
					}
					else
					{
						SceneEntityCreator.CreateEntityFromDescriptor(sceneEntityDescriptor, ref num);
					}
				}
			}
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x000650DC File Offset: 0x000632DC
		private static void InjectRandomWreckArtifacts(IEnumerable<SceneEntityDescriptor> descriptors, IEnumerable<string> randomWreckArtifacts, int maxSpawnedWreckArtifacts)
		{
			if (maxSpawnedWreckArtifacts == 0)
			{
				return;
			}
			if (randomWreckArtifacts == null)
			{
				Log.Error(Log.Channel.Data, "jbax [wreck] expected randomWreckArtifacts list to NOT be empty when maxSpawnedWreckArtifacts was :{0} ", new object[]
				{
					maxSpawnedWreckArtifacts
				});
				return;
			}
			int num = maxSpawnedWreckArtifacts;
			int num2 = 0;
			List<string> list = new List<string>(randomWreckArtifacts);
			if (list != null && list.Count > 0)
			{
				SceneEntityCreator.StripLocalCommandersAcquiredArtifacts(list);
				if (num > list.Count)
				{
					Log.Info(Log.Channel.Data, "jbax [wreck] expected randomWreckArtifacts trimmed to be placing only[{0}] of the desired Max[{1}] of them. consider that the player already has ALL of the possible artifacts listed or your list was invalid. resetting maxRandomWreckArtifacts to:{2}", new object[]
					{
						list.Count,
						num,
						list.Count
					});
					num = list.Count;
				}
				list.Sort();
				list.ShuffleList<string>();
				List<WreckDescriptor> list2;
				SceneEntityCreator.GetEmptyWreckDescriptors(descriptors, out list2);
				if (list2.Count > 0)
				{
					list2.ShuffleList<WreckDescriptor>();
					using (List<WreckDescriptor>.Enumerator enumerator = list2.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							WreckDescriptor wreckDescriptor = enumerator.Current;
							string wreckArtifact = list[num2];
							if (SceneEntityCreator.PopulateWreckArtifactType(wreckDescriptor, wreckArtifact))
							{
								num2++;
								if (num2 >= num)
								{
									break;
								}
							}
						}
						return;
					}
				}
				Log.Error(Log.Channel.Data, "jbax [wreck] expected some empty wreckResources to add artifacts too.  Consider that your planning for this level is suboptimal. Desired empty wrecks: {0}", new object[]
				{
					maxSpawnedWreckArtifacts
				});
			}
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00065220 File Offset: 0x00063420
		private static void GetEmptyWreckDescriptors(IEnumerable<SceneEntityDescriptor> descriptors, out List<WreckDescriptor> wreckDescriptors)
		{
			wreckDescriptors = new List<WreckDescriptor>();
			foreach (SceneEntityDescriptor sceneEntityDescriptor in descriptors)
			{
				if (sceneEntityDescriptor.EntityType == SceneEntityType.ResourcePoint)
				{
					ResourcePointDescriptor resourcePointDescriptor = sceneEntityDescriptor as ResourcePointDescriptor;
					if (resourcePointDescriptor != null && resourcePointDescriptor.ResourceAttributes != null && resourcePointDescriptor.ResourceAttributes.ResourceType == ResourceType.Resource3)
					{
						WreckDescriptor wreckDescriptor = resourcePointDescriptor as WreckDescriptor;
						if (wreckDescriptor == null || wreckDescriptor.WreckAttributes == null)
						{
							Log.Error(Log.Channel.Core, "jbax [wreck] failed to get a valid WreckDescriptor or WreckAttributes from descriptor: {0} ", new object[]
							{
								sceneEntityDescriptor.TypeID
							});
						}
						else if (string.IsNullOrEmpty(wreckDescriptor.WreckArtifactType))
						{
							wreckDescriptors.Add(wreckDescriptor);
						}
					}
				}
			}
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x000652DC File Offset: 0x000634DC
		private static bool PopulateWreckArtifactType(WreckDescriptor wreckDescriptor, string wreckArtifact)
		{
			if (!string.IsNullOrEmpty(wreckArtifact) && string.IsNullOrEmpty(wreckDescriptor.WreckArtifactType))
			{
				wreckDescriptor.WreckArtifactType = wreckArtifact;
				return true;
			}
			return false;
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x00065300 File Offset: 0x00063500
		private static void StripLocalCommandersAcquiredArtifacts(IList<string> randomWreckArtifacts)
		{
			foreach (Commander commander in Sim.Instance.CommanderManager.Commanders)
			{
				CommanderDirectorAttributes commanderDirectorFromID = Sim.Instance.CommanderManager.GetCommanderDirectorFromID(commander.ID);
				if (commanderDirectorFromID.PlayerType == PlayerType.Human)
				{
					foreach (KeyValuePair<string, Commander.WreckArtifactState> keyValuePair in commander.AnalyzedWreckArtifacts)
					{
						string key = keyValuePair.Key;
						if (randomWreckArtifacts.Contains(key))
						{
							randomWreckArtifacts.Remove(key);
						}
					}
				}
			}
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x000653D0 File Offset: 0x000635D0
		private static void ShuffleList<E>(this IList<E> inputList)
		{
			for (int i = inputList.Count - 1; i >= 0; i--)
			{
				int index = Sim.Rand(i + 1);
				E value = inputList[index];
				inputList[index] = inputList[i];
				inputList[i] = value;
			}
		}
	}
}
