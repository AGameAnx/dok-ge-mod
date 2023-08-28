using System;
using System.Collections.Generic;
using BBI.Game.Data;
using BBI.Unity.Core.Data;
using BBI.Unity.Core.PropertyAttributes;
using BBI.Unity.Core.Utility;
using BBI.Unity.Game.Data.Network;
using UnityEngine;
using BBI.Unity.Game.World;

namespace BBI.Unity.Game.Data
{
	// Token: 0x020000BD RID: 189
	[Serializable]
	public class LevelDefinition
	{
		// Token: 0x17000261 RID: 609
		// (get) Token: 0x0600037F RID: 895 RVA: 0x00007B37 File Offset: 0x00005D37
		public string SceneName
		{
			get
			{
				return this.m_SceneName;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00007B3F File Offset: 0x00005D3F
		public int SaveLoadVersion
		{
			get
			{
				return this.m_SaveLoadVersion;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000381 RID: 897 RVA: 0x00007B47 File Offset: 0x00005D47
		public string NameLocId
		{
			get
			{
				return this.m_NameLocId;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00007B4F File Offset: 0x00005D4F
		public string MapDescriptionLocId
		{
			get
			{
				return this.m_MapDescriptionLocId;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000383 RID: 899 RVA: 0x00007B57 File Offset: 0x00005D57
		public MultiSceneAsset MultiScene
		{
			get
			{
				return this.m_MultiScene;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00007B5F File Offset: 0x00005D5F
		public GameMode GameMode
		{
			get
			{
				if (MapModManager.CustomLayout)
				{
					return BBI.Game.Data.GameMode.Multiplayer;
				}
				return this.m_GameMode;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00007B67 File Offset: 0x00005D67
		public LevelDefinition.MissionIsLoadingScreenAuthoredData MissionIsLoadingAuthoredData
		{
			get
			{
				return this.m_MissionIsLoadingScreenAuthoredData;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000386 RID: 902 RVA: 0x00007B6F File Offset: 0x00005D6F
		public LevelDefinition.PostMissionScreenAuthoredData PostMissionAuthoredData
		{
			get
			{
				return this.m_PostMissionAuthoredData;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000387 RID: 903 RVA: 0x00007B77 File Offset: 0x00005D77
		public int MaxPlayers
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00007B7F File Offset: 0x00005D7F
		public StreamableAssetContainer SplashArtAsset
		{
			get
			{
				return this.m_SplashArtStreamingPath;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000389 RID: 905 RVA: 0x00007B87 File Offset: 0x00005D87
		public StreamableAssetContainer LoadArtContainer
		{
			get
			{
				return this.m_LoadArtStreamingPath;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x0600038A RID: 906 RVA: 0x00007B8F File Offset: 0x00005D8F
		public string NextMissionSceneName
		{
			get
			{
				return this.m_NextMissionSceneName;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00007B97 File Offset: 0x00005D97
		public bool EnabledCampaignPersistence
		{
			get
			{
				return this.m_EnableCampaignPersistence;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00007B9F File Offset: 0x00005D9F
		public bool ShowDifficultyInMenus
		{
			get
			{
				return this.m_ShowDifficultyInMenus;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00007BA7 File Offset: 0x00005DA7
		public int MaximumSpawnedWreckArtifacts
		{
			get
			{
				return this.m_MaximumSpawnedWreckArtifacts;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x0600038E RID: 910 RVA: 0x00007BAF File Offset: 0x00005DAF
		public string[] RandomArtifacts
		{
			get
			{
				return this.m_RandomArtifacts;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00007BB7 File Offset: 0x00005DB7
		public bool UsesSameMapAsPreviousMission
		{
			get
			{
				return this.m_UsesSameMapAsPreviousMission;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000390 RID: 912 RVA: 0x00007BBF File Offset: 0x00005DBF
		public bool PreserveUnitPositionsFromPreviousMap
		{
			get
			{
				return this.m_PreserveUnitPositionsFromPreviousMap;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00007BC7 File Offset: 0x00005DC7
		public int DynamicDifficultyResourceTotal
		{
			get
			{
				return this.m_DynamicDifficultyResourceTotal;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000392 RID: 914 RVA: 0x00007BCF File Offset: 0x00005DCF
		public int AmbientHeatPoints
		{
			get
			{
				if (MapModManager.maps.ContainsKey(this.SceneName))
				{
					return MapModManager.heatPoints;
				}
				return this.m_AmbientHeatPoints;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000393 RID: 915 RVA: 0x00007BD7 File Offset: 0x00005DD7
		public bool IsDevelopmentLevel
		{
			get
			{
				return this.m_IsDevelopmentLevel;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000394 RID: 916 RVA: 0x00007BDF File Offset: 0x00005DDF
		public GameObject SceneSpawnInfo
		{
			get
			{
				if (!MapModManager.CustomLayout)
				{
					return this.m_SceneSpawnInfo;
				}
				GameObject gameObject = new GameObject();
				BBI.Unity.Game.World.SceneSpawnInfo teamSpawnInfo = gameObject.AddComponent<BBI.Unity.Game.World.SceneSpawnInfo>();
				teamSpawnInfo.m_TeamSpawnInfos = new BBI.Unity.Game.World.SceneSpawnInfo.TeamSpawnInfo[] { new BBI.Unity.Game.World.SceneSpawnInfo.TeamSpawnInfo(), new BBI.Unity.Game.World.SceneSpawnInfo.TeamSpawnInfo() };
				teamSpawnInfo.m_TeamSpawnInfos[0].m_PlayerSpawnInfos = new BBI.Unity.Game.World.SceneSpawnInfo.PlayerSpawnInfo[] { new BBI.Unity.Game.World.SceneSpawnInfo.PlayerSpawnInfo(), new BBI.Unity.Game.World.SceneSpawnInfo.PlayerSpawnInfo(), new BBI.Unity.Game.World.SceneSpawnInfo.PlayerSpawnInfo() };
				teamSpawnInfo.m_TeamSpawnInfos[1].m_PlayerSpawnInfos = new BBI.Unity.Game.World.SceneSpawnInfo.PlayerSpawnInfo[] { new BBI.Unity.Game.World.SceneSpawnInfo.PlayerSpawnInfo(), new BBI.Unity.Game.World.SceneSpawnInfo.PlayerSpawnInfo(), new BBI.Unity.Game.World.SceneSpawnInfo.PlayerSpawnInfo() };
				for (int i = 0; i < 6; i++)
				{
					teamSpawnInfo.m_TeamSpawnInfos[i / 3].m_PlayerSpawnInfos[i % 3].m_SpawnPoint = (new GameObject()).transform;
				}
				return gameObject;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000395 RID: 917 RVA: 0x00007BE7 File Offset: 0x00005DE7
		public StatID SteamStatId
		{
			get
			{
				return this.m_SteamStatId;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000396 RID: 918 RVA: 0x00007BEF File Offset: 0x00005DEF
		public bool CompleteToProgress
		{
			get
			{
				return this.m_CompleteToProgress;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00007BF7 File Offset: 0x00005DF7
		public bool ShowMapBoundaries
		{
			get
			{
				return this.m_ShowMapBoundaries;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000398 RID: 920 RVA: 0x00007BFF File Offset: 0x00005DFF
		public bool ShowMapBoundariesSensors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00007C07 File Offset: 0x00005E07
		public bool IsFFAOnly
		{
			get
			{
				return this.m_IsFFAOnly;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x0600039A RID: 922 RVA: 0x00007C0F File Offset: 0x00005E0F
		public string AudioAssetBundle
		{
			get
			{
				return this.m_AudioAssetBundle;
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00007C18 File Offset: 0x00005E18
		public string[] GetAllSubscenes()
		{
			if (this.m_MultiScene != null)
			{
				List<string> list = new List<string>(this.m_MultiScene.Subscenes.Length);
				foreach (MultiSceneAsset.SceneInfo sceneInfo in this.m_MultiScene.Subscenes)
				{
					if (sceneInfo.Include)
					{
						list.Add(sceneInfo.SceneName);
					}
				}
				return list.ToArray();
			}
			return new string[]
			{
				this.m_SceneName
			};
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x0600039C RID: 924 RVA: 0x00007CA0 File Offset: 0x00005EA0
		public bool IsAvailableForLoad
		{
			get
			{
				foreach (string levelName in this.GetAllSubscenes())
				{
					if (!Application.CanStreamedLevelBeLoaded(levelName))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00007CE0 File Offset: 0x00005EE0
		public bool IsAutomatchTeamSizeSupported(int teamSize)
		{
			if (this.m_SupportedAutomatchTeamSizes == null)
			{
				return false;
			}
			foreach (int num in this.m_SupportedAutomatchTeamSizes)
			{
				if (teamSize == num)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00007D1C File Offset: 0x00005F1C
		public LevelDefinition()
		{
		}

		public LevelDefinition Copy()
		{
			return (LevelDefinition)this.MemberwiseClone();
		}

		// Token: 0x04000339 RID: 825
		[SerializeField]
		public string m_SceneName = string.Empty;

		// Token: 0x0400033A RID: 826
		[SerializeField]
		public int m_SaveLoadVersion;

		// Token: 0x0400033B RID: 827
		[SerializeField]
		public string m_NameLocId = string.Empty;

		// Token: 0x0400033C RID: 828
		[SerializeField]
		public string m_MapDescriptionLocId = string.Empty;

		// Token: 0x0400033D RID: 829
		[SerializeField]
		public MultiSceneAsset m_MultiScene;

		// Token: 0x0400033E RID: 830
		[SerializeField]
		public GameMode m_GameMode = GameMode.Multiplayer;

		// Token: 0x0400033F RID: 831
		[SerializeField]
		public LevelDefinition.MissionIsLoadingScreenAuthoredData m_MissionIsLoadingScreenAuthoredData;

		// Token: 0x04000340 RID: 832
		[SerializeField]
		public LevelDefinition.PostMissionScreenAuthoredData m_PostMissionAuthoredData;

		// Token: 0x04000341 RID: 833
		[SerializeField]
		public int m_MaxPlayers = 1;

		// Token: 0x04000342 RID: 834
		[StreamableAssetTypeProperty(typeof(Texture2D))]
		[SerializeField]
		public StreamableAssetContainer m_SplashArtStreamingPath = new StreamableAssetContainer();

		// Token: 0x04000343 RID: 835
		[SerializeField]
		[StreamableAssetTypeProperty(typeof(Texture2D))]
		public StreamableAssetContainer m_LoadArtStreamingPath = new StreamableAssetContainer();

		// Token: 0x04000344 RID: 836
		[SerializeField]
		public string m_NextMissionSceneName = string.Empty;

		// Token: 0x04000345 RID: 837
		[SerializeField]
		public bool m_EnableCampaignPersistence;

		// Token: 0x04000346 RID: 838
		[SerializeField]
		public bool m_ShowDifficultyInMenus = true;

		// Token: 0x04000347 RID: 839
		[SerializeField]
		public int m_MaximumSpawnedWreckArtifacts;

		// Token: 0x04000348 RID: 840
		[SerializeField]
		public string[] m_RandomArtifacts;

		// Token: 0x04000349 RID: 841
		[SerializeField]
		public bool m_UsesSameMapAsPreviousMission;

		// Token: 0x0400034A RID: 842
		[SerializeField]
		public bool m_PreserveUnitPositionsFromPreviousMap;

		// Token: 0x0400034B RID: 843
		[SerializeField]
		public int m_DynamicDifficultyResourceTotal;

		// Token: 0x0400034C RID: 844
		[SerializeField]
		public int m_AmbientHeatPoints;

		// Token: 0x0400034D RID: 845
		[SerializeField]
		public bool m_IsDevelopmentLevel;

		// Token: 0x0400034E RID: 846
		[SerializeField]
		public GameObject m_SceneSpawnInfo;

		// Token: 0x0400034F RID: 847
		[SerializeField]
		public StatID m_SteamStatId;

		// Token: 0x04000350 RID: 848
		[SerializeField]
		public bool m_CompleteToProgress = true;

		// Token: 0x04000351 RID: 849
		[SerializeField]
		public bool m_ShowMapBoundaries = true;

		// Token: 0x04000352 RID: 850
		[SerializeField]
		public bool m_ShowMapBoundariesSensors;

		// Token: 0x04000353 RID: 851
		[SerializeField]
		public bool m_IsFFAOnly;

		// Token: 0x04000354 RID: 852
		[SerializeField]
		[AssetBundlePath]
		public string m_AudioAssetBundle = string.Empty;

		// Token: 0x04000355 RID: 853
		[Tooltip("Supported values: 1 (1v1), 2(2v2), 3(3v3). Otherwise, it's not a valid map for automatch.")]
		[SerializeField]
		public int[] m_SupportedAutomatchTeamSizes;

		// Token: 0x04000356 RID: 854
		[NonSerialized]
		public bool CampaignPersistenceToggled;

		// Token: 0x04000357 RID: 855
		[NonSerialized]
		public Difficulty Difficulty;

		// Token: 0x020000BE RID: 190
		[Serializable]
		public struct MissionIsLoadingScreenAuthoredData
		{
			// Token: 0x04000358 RID: 856
			public string MissionTitle;
		}

		// Token: 0x020000BF RID: 191
		[Serializable]
		public struct PostMissionScreenAuthoredData
		{
			// Token: 0x04000359 RID: 857
			public string TitleBoxHeading1;

			// Token: 0x0400035A RID: 858
			public string TitleBoxHeading2;

			// Token: 0x0400035B RID: 859
			public string TitleBoxTimeAndTemperature;

			// Token: 0x0400035C RID: 860
			public string TitleBoxLocation;

			// Token: 0x0400035D RID: 861
			public string ReportBoxTitle;

			// Token: 0x0400035E RID: 862
			public string ReportBoxText;

			// Token: 0x0400035F RID: 863
			public string FleetStatusBoxDistanceToAnomaly;

			// Token: 0x04000360 RID: 864
			public string FleetStatusBoxTemperature;

			// Token: 0x04000361 RID: 865
			public string FleetStatusPercentageOfSystemsOnline;

			// Token: 0x04000362 RID: 866
			public string FleetStatusFuelRemaining;

			// Token: 0x04000363 RID: 867
			public string FleetStatusWaterRemaining;
		}
	}
}
