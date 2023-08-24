using System;
using BBI.Core;
using BBI.Core.Data;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Data;
using BBI.Unity.Game.Data;
using UnityEngine;

namespace BBI.Unity.Game.World
{
	// Token: 0x0200037A RID: 890
	public class SceneSpawnInfo : MonoBehaviour
	{
		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001E2D RID: 7725 RVA: 0x000B226B File Offset: 0x000B046B
		public SceneSpawnInfo.TeamSpawnInfo[] TeamSpawnInfos
		{
			get
			{
				return this.m_TeamSpawnInfos;
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001E2E RID: 7726 RVA: 0x000B2273 File Offset: 0x000B0473
		public SceneSpawnInfo.PlayerSpawnInfo[] FFASpawnInfos
		{
			get
			{
				return this.m_FFASpawnInfos;
			}
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x000B227C File Offset: 0x000B047C
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

		// Token: 0x06001E30 RID: 7728 RVA: 0x000B2340 File Offset: 0x000B0540
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

		// Token: 0x06001E31 RID: 7729 RVA: 0x000B242E File Offset: 0x000B062E
		public SceneSpawnInfo()
		{
		}

		// Token: 0x040018D8 RID: 6360
		[SerializeField]
		private SceneSpawnInfo.TeamSpawnInfo[] m_TeamSpawnInfos;

		// Token: 0x040018D9 RID: 6361
		[SerializeField]
		private SceneSpawnInfo.PlayerSpawnInfo[] m_FFASpawnInfos;

		// Token: 0x0200037B RID: 891
		[Serializable]
		public class PlayerSpawnInfo : SpawnAttributes
		{
			// Token: 0x17000535 RID: 1333
			// (get) Token: 0x06001E32 RID: 7730 RVA: 0x000B2436 File Offset: 0x000B0636
			public Transform PresSpawnTransform
			{
				get
				{
					return this.m_SpawnPoint;
				}
			}

			// Token: 0x17000536 RID: 1334
			// (get) Token: 0x06001E33 RID: 7731 RVA: 0x000B243E File Offset: 0x000B063E
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

			// Token: 0x17000537 RID: 1335
			// (get) Token: 0x06001E34 RID: 7732 RVA: 0x000B245F File Offset: 0x000B065F
			public Quaternion DefaultCameraOrientation
			{
				get
				{
					return Quaternion.AngleAxis(this.m_DefaultCameraHeading, Vector3.up);
				}
			}

			// Token: 0x17000538 RID: 1336
			// (get) Token: 0x06001E35 RID: 7733 RVA: 0x000B2471 File Offset: 0x000B0671
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

			// Token: 0x17000539 RID: 1337
			// (get) Token: 0x06001E36 RID: 7734 RVA: 0x000B2494 File Offset: 0x000B0694
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

			// Token: 0x06001E37 RID: 7735 RVA: 0x000B24C8 File Offset: 0x000B06C8
			public PlayerSpawnInfo()
			{
			}

			// Token: 0x040018DA RID: 6362
			[SerializeField]
			private Transform m_SpawnPoint;

			// Token: 0x040018DB RID: 6363
			[SerializeField]
			public float m_DefaultCameraHeading;
		}

		// Token: 0x0200037C RID: 892
		[Serializable]
		public class TeamSpawnInfo
		{
			// Token: 0x1700053A RID: 1338
			// (get) Token: 0x06001E38 RID: 7736 RVA: 0x000B24D0 File Offset: 0x000B06D0
			public SceneSpawnInfo.PlayerSpawnInfo[] PlayerSpawnInfos
			{
				get
				{
					return this.m_PlayerSpawnInfos;
				}
			}

			// Token: 0x1700053B RID: 1339
			// (get) Token: 0x06001E39 RID: 7737 RVA: 0x000B24D8 File Offset: 0x000B06D8
			public SceneEntityGroup ExtractionZoneGroup
			{
				get
				{
					return this.m_ExtractionZoneGroup;
				}
			}

			// Token: 0x06001E3A RID: 7738 RVA: 0x000B24E0 File Offset: 0x000B06E0
			public TeamSpawnInfo()
			{
			}

			// Token: 0x040018DC RID: 6364
			[SerializeField]
			private SceneSpawnInfo.PlayerSpawnInfo[] m_PlayerSpawnInfos;

			// Token: 0x040018DD RID: 6365
			[SerializeField]
			private SceneEntityGroup m_ExtractionZoneGroup;
		}
	}
}
