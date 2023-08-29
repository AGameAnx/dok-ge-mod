using System;
using BBI.Core;
using BBI.Core.Data;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Data;
using BBI.Unity.Game.Data;
using UnityEngine;
using System.Collections.Generic;

namespace BBI.Unity.Game.World
{
	public class SceneSpawnInfo : MonoBehaviour
	{
		public SceneSpawnInfo.TeamSpawnInfo[] TeamSpawnInfos
		{
			get
			{
				if (MapModManager.CustomLayout)
				{
					SceneSpawnInfo.TeamSpawnInfo[] teamSpawnInfoArray = new SceneSpawnInfo.TeamSpawnInfo[2];
					SceneSpawnInfo.TeamSpawnInfo teamSpawnInfo = new SceneSpawnInfo.TeamSpawnInfo()
					{
						m_PlayerSpawnInfos = new SceneSpawnInfo.PlayerSpawnInfo[] { new SceneSpawnInfo.PlayerSpawnInfo(), new SceneSpawnInfo.PlayerSpawnInfo(), new SceneSpawnInfo.PlayerSpawnInfo() }
					};
					teamSpawnInfoArray[0] = teamSpawnInfo;
					teamSpawnInfo = new SceneSpawnInfo.TeamSpawnInfo()
					{
						m_PlayerSpawnInfos = new SceneSpawnInfo.PlayerSpawnInfo[] { new SceneSpawnInfo.PlayerSpawnInfo(), new SceneSpawnInfo.PlayerSpawnInfo(), new SceneSpawnInfo.PlayerSpawnInfo() }
					};
					teamSpawnInfoArray[1] = teamSpawnInfo;
					this.m_TeamSpawnInfos = teamSpawnInfoArray;
					for (int i = 0; i < 6; i++)
					{
						this.m_TeamSpawnInfos[i / 3].m_PlayerSpawnInfos[i % 3].m_SpawnPoint = (new GameObject()).transform;
					}
					foreach (MapModManager.MapSpawnData spawn in MapModManager.spawns)
					{
						this.m_TeamSpawnInfos[(spawn.team == 0 ? 1 : 0)].m_PlayerSpawnInfos[spawn.index].m_SpawnPoint.Translate(spawn.position);
						this.m_TeamSpawnInfos[(spawn.team == 0 ? 1 : 0)].m_PlayerSpawnInfos[spawn.index].m_SpawnPoint.Rotate(0f, spawn.angle, 0f);
						this.m_TeamSpawnInfos[(spawn.team == 0 ? 1 : 0)].m_PlayerSpawnInfos[spawn.index].m_DefaultCameraHeading = spawn.cameraAngle;
					}
				}
				return this.m_TeamSpawnInfos;
			}
		}

		public SceneSpawnInfo.PlayerSpawnInfo[] FFASpawnInfos
		{
			get
			{
				if (MapModManager.CustomLayout)
				{
					this.m_FFASpawnInfos = new SceneSpawnInfo.PlayerSpawnInfo[] { new SceneSpawnInfo.PlayerSpawnInfo(), new SceneSpawnInfo.PlayerSpawnInfo(), new SceneSpawnInfo.PlayerSpawnInfo(), new SceneSpawnInfo.PlayerSpawnInfo(), new SceneSpawnInfo.PlayerSpawnInfo(), new SceneSpawnInfo.PlayerSpawnInfo() };
					for (int i = 0; i < 6; i++)
					{
						this.m_FFASpawnInfos[i].m_SpawnPoint = (new GameObject()).transform;
					}
					foreach (MapModManager.MapSpawnData spawn in MapModManager.spawns)
					{
						this.m_FFASpawnInfos[spawn.team].m_SpawnPoint.Translate(spawn.position);
						this.m_FFASpawnInfos[spawn.team].m_SpawnPoint.Rotate(0f, spawn.angle, 0f);
						this.m_FFASpawnInfos[spawn.team].m_DefaultCameraHeading = spawn.cameraAngle;
					}
				}
				return this.m_FFASpawnInfos;
			}
		}

		private void Awake()
		{
			for (int i = 0; i < this.TeamSpawnInfos.Length; i++)
			{
				SceneSpawnInfo.TeamSpawnInfo teamSpawnInfo = this.TeamSpawnInfos[i];
				if (teamSpawnInfo != null && teamSpawnInfo.ExtractionZoneGroup != null)
				{
					SceneEntityBase[] sceneEntitiesInGroup = teamSpawnInfo.ExtractionZoneGroup.GetSceneEntitiesInGroup();
					if (!sceneEntitiesInGroup.IsNullOrEmpty<SceneEntityBase>())
					{
						foreach (SceneEntityBase sceneEntityBase in sceneEntitiesInGroup)
						{
							SceneExtractionZoneEntity sceneExtractionZoneEntity = sceneEntityBase as SceneExtractionZoneEntity;
							if (sceneExtractionZoneEntity != null)
							{
								sceneExtractionZoneEntity.SetTeamSpawnIndex(i);
							}
							else
							{
								Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "SceneExtractionZoneEntityGroup {0} has a non-SceneExtractionZoneEntity child {1}!", new object[]
								{
									teamSpawnInfo.ExtractionZoneGroup.name,
									sceneEntityBase.name
								});
							}
						}
					}
				}
			}
		}

		private void OnDrawGizmos()
		{
			foreach (SceneSpawnInfo.TeamSpawnInfo teamSpawnInfo in this.TeamSpawnInfos)
			{
				if (teamSpawnInfo != null && teamSpawnInfo.ExtractionZoneGroup != null)
				{
					SceneEntityBase[] sceneEntitiesInGroup = teamSpawnInfo.ExtractionZoneGroup.GetSceneEntitiesInGroup();
					if (!sceneEntitiesInGroup.IsNullOrEmpty<SceneEntityBase>())
					{
						foreach (SceneEntityBase sceneEntityBase in sceneEntitiesInGroup)
						{
							if (sceneEntityBase.EntityToSpawn != null)
							{
								SceneEntityAssetContainer sceneEntityAssetContainer = sceneEntityBase.EntityToSpawn as SceneEntityAssetContainer;
								if (sceneEntityAssetContainer != null && sceneEntityAssetContainer.ExtractionZoneAttributes != null)
								{
									ExtractionZoneAttributes extractionZoneAttributes = sceneEntityAssetContainer.ExtractionZoneAttributes.Data as ExtractionZoneAttributes;
									if (extractionZoneAttributes != null)
									{
										Gizmos.color = Color.yellow;
										Gizmos.DrawWireSphere(sceneEntityBase.transform.position, Fixed64.UnsafeFloatValue(extractionZoneAttributes.RadiusMeters));
									}
								}
							}
						}
					}
				}
			}
		}

		public SceneSpawnInfo()
		{
		}

		[SerializeField]
		public SceneSpawnInfo.TeamSpawnInfo[] m_TeamSpawnInfos;

		[SerializeField]
		private SceneSpawnInfo.PlayerSpawnInfo[] m_FFASpawnInfos;

		[Serializable]
		public class PlayerSpawnInfo : SpawnAttributes
		{
			public Transform PresSpawnTransform
			{
				get
				{
					return this.m_SpawnPoint;
				}
			}

			public Vector3 PresSpawnOrigin
			{
				get
				{
					if (!(this.m_SpawnPoint == null))
					{
						return this.m_SpawnPoint.position;
					}
					return Vector3.zero;
				}
			}

			public Quaternion DefaultCameraOrientation
			{
				get
				{
					return Quaternion.AngleAxis(this.m_DefaultCameraHeading, Vector3.up);
				}
			}

			Vector2r SpawnAttributes.SpawnPoint
			{
				get
				{
					if (this.m_SpawnPoint != null)
					{
						return VectorHelper.UnityVector3ToSimVector2(this.m_SpawnPoint.position);
					}
					return Vector2r.Zero;
				}
			}

			Orientation2 SpawnAttributes.SpawnOrientation
			{
				get
				{
					if (this.m_SpawnPoint != null)
					{
						return Orientation2.FromDirection(VectorHelper.UnityVector3ToSimVector2(this.m_SpawnPoint.forward));
					}
					return default(Orientation2);
				}
			}

			public PlayerSpawnInfo()
			{
			}

			[SerializeField]
			public Transform m_SpawnPoint;

			[SerializeField]
			public float m_DefaultCameraHeading;
		}

		[Serializable]
		public class TeamSpawnInfo
		{
			public SceneSpawnInfo.PlayerSpawnInfo[] PlayerSpawnInfos
			{
				get
				{
					return this.m_PlayerSpawnInfos;
				}
			}

			public SceneEntityGroup ExtractionZoneGroup
			{
				get
				{
					return this.m_ExtractionZoneGroup;
				}
			}

			public TeamSpawnInfo()
			{
			}

			[SerializeField]
			public SceneSpawnInfo.PlayerSpawnInfo[] m_PlayerSpawnInfos;

			[SerializeField]
			private SceneEntityGroup m_ExtractionZoneGroup;
		}
	}
}
