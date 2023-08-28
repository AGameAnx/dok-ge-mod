using System;
using System.Collections;
using System.Collections.Generic;
using BBI.Core.Data;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Game.Replay;
using BBI.Game.Simulation;
using BBI.Steam;
using BBI.Unity.Core.Data;
using BBI.Unity.Core.DLC;
using BBI.Unity.Core.Rendering;
using BBI.Unity.Core.Utility;
using BBI.Unity.Game.Audio;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.DLC;
using BBI.Unity.Game.Localize;
using BBI.Unity.Game.UI;
using BBI.Unity.Game.Utility;
using UnityEngine;

namespace BBI.Unity.Game.World
{
	// Token: 0x02000380 RID: 896
	[Serializable]
	public class LevelManager : ResettableSubsystemBase
	{
		// Token: 0x14000078 RID: 120
		// (add) Token: 0x06001E45 RID: 7749 RVA: 0x000B26A0 File Offset: 0x000B08A0
		// (remove) Token: 0x06001E46 RID: 7750 RVA: 0x000B26D8 File Offset: 0x000B08D8
		public event LevelManager.LoadHandler SceneLoadStart;

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x06001E47 RID: 7751 RVA: 0x000B2710 File Offset: 0x000B0910
		// (remove) Token: 0x06001E48 RID: 7752 RVA: 0x000B2748 File Offset: 0x000B0948
		public event LevelManager.LoadHandler SceneLoadComplete;

		// Token: 0x1400007A RID: 122
		// (add) Token: 0x06001E49 RID: 7753 RVA: 0x000B2780 File Offset: 0x000B0980
		// (remove) Token: 0x06001E4A RID: 7754 RVA: 0x000B27B8 File Offset: 0x000B09B8
		public event LevelManager.LoadFailedHandler SceneLoadFailed;

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x06001E4B RID: 7755 RVA: 0x000B27F0 File Offset: 0x000B09F0
		// (remove) Token: 0x06001E4C RID: 7756 RVA: 0x000B2828 File Offset: 0x000B0A28
		public event LevelManager.UnloadHandler SceneUnloadStart;

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x06001E4D RID: 7757 RVA: 0x000B2860 File Offset: 0x000B0A60
		// (remove) Token: 0x06001E4E RID: 7758 RVA: 0x000B2898 File Offset: 0x000B0A98
		public event LevelManager.UnloadHandler SceneUnloadComplete;

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x06001E4F RID: 7759 RVA: 0x000B28D0 File Offset: 0x000B0AD0
		// (remove) Token: 0x06001E50 RID: 7760 RVA: 0x000B2908 File Offset: 0x000B0B08
		public event LevelManager.PlayerLoadedHandler PlayerLoadedScene;

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x06001E51 RID: 7761 RVA: 0x000B2940 File Offset: 0x000B0B40
		// (remove) Token: 0x06001E52 RID: 7762 RVA: 0x000B2978 File Offset: 0x000B0B78
		public event LevelManager.AllPlayersLoadedHandler AllPlayersLoaded;

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001E53 RID: 7763 RVA: 0x000B29AD File Offset: 0x000B0BAD
		public LevelDefinition[] LevelEntriesSP
		{
			get
			{
				return this.m_LevelEntriesSP;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001E54 RID: 7764 RVA: 0x000B29B5 File Offset: 0x000B0BB5
		public LevelDefinition[] LevelEntriesMP
		{
			get
			{
				LevelDefinition[] levelDefinitionArray = new LevelDefinition[(int)this.m_LevelEntriesMP.Length + MapModManager.maps.Count];
				LevelDefinition[] mLevelEntriesMP = this.m_LevelEntriesMP;
				for (int i = 0; i < (int)mLevelEntriesMP.Length; i++)
				{
					LevelDefinition item = mLevelEntriesMP[i];
					item.m_NameLocId = MapModManager.mapNameOverrides[string.Concat(item.SceneName, (item.IsFFAOnly ? "" : "-"))];
					item.m_LoadArtStreamingPath = null;
				}
				this.m_LevelEntriesMP.CopyTo(levelDefinitionArray, 0);
				int length = (int)this.m_LevelEntriesMP.Length;
				foreach (KeyValuePair<string, MapModManager.MapMetaData> map in MapModManager.maps)
				{
					LevelDefinition key = this.FindLevelFromSceneName(map.Value.name, GameMode.SinglePlayer).Copy();
					key.m_SceneName = map.Key;
					key.m_IsFFAOnly = map.Value.gameMode == TeamSetting.FFA;
					key.m_NameLocId = map.Value.locName;
					key.m_LoadArtStreamingPath = null;
					int num = length;
					length = num + 1;
					levelDefinitionArray[num] = key;
				}
				return levelDefinitionArray;
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001E55 RID: 7765 RVA: 0x000B29BD File Offset: 0x000B0BBD
		// (set) Token: 0x06001E56 RID: 7766 RVA: 0x000B29C5 File Offset: 0x000B0BC5
		public LevelManager.State ManagerState { get; private set; }

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001E57 RID: 7767 RVA: 0x000B29CE File Offset: 0x000B0BCE
		// (set) Token: 0x06001E58 RID: 7768 RVA: 0x000B29D6 File Offset: 0x000B0BD6
		public LevelDefinition LoadedLevel { get; private set; }

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001E59 RID: 7769 RVA: 0x000B29DF File Offset: 0x000B0BDF
		// (set) Token: 0x06001E5A RID: 7770 RVA: 0x000B29E7 File Offset: 0x000B0BE7
		public CommanderAttributes[] CommanderAttributes { get; private set; }

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001E5B RID: 7771 RVA: 0x000B29F0 File Offset: 0x000B0BF0
		// (set) Token: 0x06001E5C RID: 7772 RVA: 0x000B29F8 File Offset: 0x000B0BF8
		public CommanderAttributes[] DefaultMissionCommanders { get; private set; }

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001E5D RID: 7773 RVA: 0x000B2A04 File Offset: 0x000B0C04
		public bool AllPlayersReportLoaded
		{
			get
			{
				foreach (NetworkPlayerID item in this.mPlayersInSession)
				{
					if (!this.mPlayersFinishedLoading.Contains(item))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x000B2A68 File Offset: 0x000B0C68
		public LevelManager()
		{
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x000B2A94 File Offset: 0x000B0C94
		public LevelManager(LevelManager other)
		{
			this.m_LevelEntriesSP = other.m_LevelEntriesSP;
			this.m_LevelEntriesMP = other.m_LevelEntriesMP;
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x000B2AE3 File Offset: 0x000B0CE3
		public void Initialize(CommanderAttributes[] commanderLoadouts, CommanderAttributes[] defaultMissionCommanders, ICoroutineHost coroutineHost, IGameLocalization localizationManager, DLCManager dlcManager)
		{
			base.Initialize();
			this.ManagerState = LevelManager.State.Startup;
			this.mCoroutineHost = coroutineHost;
			this.CommanderAttributes = commanderLoadouts;
			this.DefaultMissionCommanders = defaultMissionCommanders;
			this.mLocalizationManager = localizationManager;
			this.mDLCManager = dlcManager;
			this.mAudioAssetBundleLoader = new AudioAssetBundleLoader(coroutineHost);
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x000B2B23 File Offset: 0x000B0D23
		public override void Reset()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x000B2B2C File Offset: 0x000B0D2C
		public override void Shutdown()
		{
			base.Shutdown();
			this.SceneLoadStart = null;
			this.SceneLoadComplete = null;
			this.SceneLoadFailed = null;
			this.SceneUnloadStart = null;
			this.SceneUnloadComplete = null;
			this.LoadedLevel = null;
			this.mPlayersInSession.Clear();
			this.mPlayersFinishedLoading.Clear();
			this.mLocalPlayer = NetworkPlayerID.kInvalidID;
			NetworkPlayerIDManager.Instance.ObservingPlayerID = NetworkPlayerID.kInvalidID;
			this.mLocalizationManager = null;
			this.mAudioAssetBundleLoader.Shutdown();
			this.mAudioAssetBundleLoader = null;
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x000B2BB4 File Offset: 0x000B0DB4
		public void StartExitLevel()
		{
			Log.Info(Log.Channel.Online, "Local player is exiting the level...", new object[0]);
			this.mPlayersInSession.Clear();
			this.mPlayersFinishedLoading.Clear();
			this.mLocalPlayer = NetworkPlayerID.kInvalidID;
			NetworkPlayerIDManager.Instance.ObservingPlayerID = NetworkPlayerID.kInvalidID;
			if (this.ManagerState == LevelManager.State.Game)
			{
				this.mCoroutineHost.StartNewCoroutine(this.ExitGame(SessionChangeReason.ExitGame));
			}
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x000B2C22 File Offset: 0x000B0E22
		public void LoadLevelForSave(LevelDefinition levelToLoad, GameMode gameMode, ReplayMode replayMode, DependencyContainerBase dependencies)
		{
			if (!LevelManager.ValidateCanLoadGame(gameMode))
			{
				return;
			}
			if (this.ManagerState == LevelManager.State.Game)
			{
				this.mCoroutineHost.StartNewCoroutine(this.UnloadAndLoadLevel(gameMode, replayMode, levelToLoad, dependencies));
				return;
			}
			this.StartLoadLevel(gameMode, replayMode, levelToLoad, dependencies);
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x000B2C5C File Offset: 0x000B0E5C
		public void ChangeLevel(LevelDefinition levelToLoad)
		{
			if (ShipbreakersMain.GameMode != GameMode.SinglePlayer)
			{
				Log.Error(Log.Channel.Gameplay, "Only single player is currently supported. GameMode was {0}", new object[]
				{
					ShipbreakersMain.GameMode
				});
				return;
			}
			if (!LevelManager.ValidateCanLoadGame(ShipbreakersMain.GameMode))
			{
				return;
			}
			if (this.ManagerState == LevelManager.State.Game)
			{
				DependencyContainer<SessionBase, SessionChangeReason> dependencies = new DependencyContainer<SessionBase, SessionChangeReason>(new NullSession(), SessionChangeReason.Transition);
				this.mCoroutineHost.StartNewCoroutine(this.UnloadAndLoadLevel(GameMode.SinglePlayer, ReplayMode.ReplaysDisabled, levelToLoad, dependencies));
				return;
			}
			Log.Error(Log.Channel.Gameplay, "Tried to load a new level while GameState was {0}!", new object[]
			{
				this.ManagerState
			});
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x000B2CF4 File Offset: 0x000B0EF4
		public void StartLoadLevel(GameMode gameMode, ReplayMode replayMode, LevelDefinition level, DependencyContainerBase dependencies)
		{
			if (!LevelManager.ValidateCanLoadGame(gameMode))
			{
				return;
			}
			this.mPlayersFinishedLoading.Clear();
			SessionBase sessionBase;
			dependencies.Get<SessionBase>(out sessionBase);
			this.mPlayersInSession = sessionBase.PlayerIDs;
			this.mLocalPlayer = sessionBase.LocalPlayerID;
			sessionBase.StartGame();
			if (replayMode == ReplayMode.ReplayingGame)
			{
				ReplayHelpers.ReplayableGameSessionHeader replayableGameSessionHeader;
				dependencies.Get<ReplayHelpers.ReplayableGameSessionHeader>(out replayableGameSessionHeader);
				NetworkPlayerIDManager.Instance.ObservingPlayerID = replayableGameSessionHeader.LocalPlayerID;
			}
			if (this.ManagerState == LevelManager.State.Startup)
			{
				this.mCoroutineHost.StartNewCoroutine(this.LoadGame(gameMode, replayMode, level, dependencies));
			}
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x000B2D7A File Offset: 0x000B0F7A
		public void ClearLoadingPlayers()
		{
			if (this.ManagerState != LevelManager.State.Startup)
			{
				this.mPlayersInSession.Clear();
				this.mPlayersInSession.Add(this.mLocalPlayer);
				this.CheckIfAllPlayersHaveFinishedLoading();
			}
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x000B2DA7 File Offset: 0x000B0FA7
		public void RemoveLoadingPlayer(NetworkPlayerID playerID)
		{
			if (this.ManagerState != LevelManager.State.Startup)
			{
				this.mPlayersInSession.Remove(playerID);
				this.CheckIfAllPlayersHaveFinishedLoading();
			}
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x000B2DC5 File Offset: 0x000B0FC5
		public void SetLoadingPlayerFinished(NetworkPlayerID playerID)
		{
			if (this.ManagerState != LevelManager.State.Startup)
			{
				if (!this.mPlayersFinishedLoading.Contains(playerID))
				{
					this.mPlayersFinishedLoading.Add(playerID);
					this.OnPlayerLoadedScene(playerID);
				}
				this.CheckIfAllPlayersHaveFinishedLoading();
			}
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x000B2DF8 File Offset: 0x000B0FF8
		public LevelDefinition FindLevelFromNameLocID(string nameLocID, GameMode mode)
		{
			IEnumerable<LevelDefinition> levelDefinitions = this.GetLevelDefinitions(mode);
			foreach (LevelDefinition levelDefinition in levelDefinitions)
			{
				if (levelDefinition.NameLocId == nameLocID)
				{
					return levelDefinition;
				}
			}
			Log.Error(Log.Channel.UI, "Unable to find level by display name '{0}'!", new object[]
			{
				nameLocID
			});
			return null;
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x000B2E74 File Offset: 0x000B1074
		public LevelDefinition FindLevelFromSceneName(string sceneName, GameMode mode)
		{
			IEnumerable<LevelDefinition> levelDefinitions = this.GetLevelDefinitions(mode);
			if (levelDefinitions != null)
			{
				foreach (LevelDefinition levelDefinition in levelDefinitions)
				{
					if (levelDefinition.SceneName == sceneName)
					{
						return levelDefinition;
					}
				}
			}
			Log.Error(Log.Channel.UI, "Unable to find level by scene name '{0}'!", new object[]
			{
				sceneName
			});
			return null;
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x000B2EF4 File Offset: 0x000B10F4
		public LevelDefinition FindLevelByIndex(int index, GameMode mode)
		{
			if (index >= 0)
			{
				LevelDefinition[] levelDefinitions = this.GetLevelDefinitions(mode);
				if (levelDefinitions != null && index < levelDefinitions.Length)
				{
					return levelDefinitions[index];
				}
			}
			return null;
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x000B2F1B File Offset: 0x000B111B
		public string GetLocalizedLevelDisplayName(LevelDefinition def)
		{
			return this.mLocalizationManager.GetText(def.NameLocId);
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x000B2F30 File Offset: 0x000B1130
		public string GetLocalizedLevelDisplayName(string sceneName, GameMode mode)
		{
			string result = string.Empty;
			LevelDefinition levelDefinition = this.FindLevelFromSceneName(sceneName, mode);
			if (levelDefinition != null)
			{
				result = this.GetLocalizedLevelDisplayName(levelDefinition);
			}
			return result;
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x000B2F58 File Offset: 0x000B1158
		public string GetLocalizedMapDescription(LevelDefinition def)
		{
			return this.mLocalizationManager.GetText(def.MapDescriptionLocId);
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x000B2F6C File Offset: 0x000B116C
		public bool IsSceneAvailable(string sceneName, GameMode mode)
		{
			if (string.IsNullOrEmpty(sceneName))
			{
				return false;
			}
			IEnumerable<LevelDefinition> levelDefinitions = this.GetLevelDefinitions(mode);
			foreach (LevelDefinition levelDefinition in levelDefinitions)
			{
				if (levelDefinition.SceneName == sceneName)
				{
					return levelDefinition.IsAvailableForLoad;
				}
			}
			return false;
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x000B31A4 File Offset: 0x000B13A4
		private IEnumerator OnSceneLoadStart(LevelDefinition level, Scene scene, GameMode gameMode, ReplayMode replayMode, DependencyContainerBase dependencies)
		{
			if (this.SceneLoadStart != null)
			{
				Delegate[] listeners = this.SceneLoadStart.GetInvocationList();
				foreach (Delegate listener in listeners)
				{
					IEnumerator listenerResult = (IEnumerator)listener.DynamicInvoke(new object[]
					{
						level,
						scene,
						gameMode,
						replayMode,
						dependencies
					});
					if (listenerResult.MoveNext())
					{
						yield return this.mCoroutineHost.StartNewCoroutine(listenerResult);
					}
				}
			}
			yield break;
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x000B33C4 File Offset: 0x000B15C4
		private IEnumerator OnSceneLoadComplete(LevelDefinition level, Scene scene, GameMode gameMode, ReplayMode replayMode, DependencyContainerBase dependencies)
		{
			if (this.SceneLoadComplete != null)
			{
				Delegate[] listeners = this.SceneLoadComplete.GetInvocationList();
				foreach (Delegate listener in listeners)
				{
					IEnumerator listenerResult = (IEnumerator)listener.DynamicInvoke(new object[]
					{
						level,
						scene,
						gameMode,
						replayMode,
						dependencies
					});
					if (listenerResult.MoveNext())
					{
						yield return this.mCoroutineHost.StartNewCoroutine(listenerResult);
					}
				}
			}
			this.SetLoadingPlayerFinished(this.mLocalPlayer);
			yield break;
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x000B3405 File Offset: 0x000B1605
		private void OnSceneLoadFailed(LevelDefinition level, Scene scene, GameMode gameMode, ReplayMode replayMode, DependencyContainerBase dependencies)
		{
			if (this.SceneLoadFailed != null)
			{
				this.SceneLoadFailed(level, scene, gameMode, replayMode, dependencies);
			}
			this.SetLoadingPlayerFinished(this.mLocalPlayer);
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x000B342D File Offset: 0x000B162D
		private void OnSceneUnloadStart(Scene loadedScene, SessionChangeReason reason)
		{
			if (this.SceneUnloadStart != null)
			{
				this.SceneUnloadStart(loadedScene, reason);
			}
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x000B3444 File Offset: 0x000B1644
		private void OnSceneUnloadComplete(Scene loadedScene, SessionChangeReason reason)
		{
			if (this.SceneUnloadComplete != null)
			{
				this.SceneUnloadComplete(loadedScene, reason);
				this.LoadedLevel = null;
			}
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x000B3462 File Offset: 0x000B1662
		private void OnPlayerLoadedScene(NetworkPlayerID playerID)
		{
			if (this.PlayerLoadedScene != null)
			{
				this.PlayerLoadedScene(playerID);
			}
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x000B3478 File Offset: 0x000B1678
		private void OnAllPlayersLoaded()
		{
			if (this.AllPlayersLoaded != null)
			{
				this.AllPlayersLoaded();
			}
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x000B3B08 File Offset: 0x000B1D08
		private IEnumerator LoadGame(GameMode gameMode, ReplayMode replayMode, LevelDefinition levelDef, DependencyContainerBase dependencies)
		{
			yield return Resources.UnloadUnusedAssets();
			GC.Collect();
			this.mGameMode = gameMode;
			this.mReplayMode = replayMode;
			this.ManagerState = LevelManager.State.GameLoading;
			this.LoadedLevel = levelDef;
			Scene scene = Scene.Create(levelDef.SceneName, levelDef.GetAllSubscenes());
			if (!string.IsNullOrEmpty(levelDef.AudioAssetBundle))
			{
				this.mAudioAssetBundleLoader.LoadAssetBundle(levelDef.AudioAssetBundle);
			}
			GameStartSettings startSettings;
			dependencies.Get<GameStartSettings>(out startSettings);
			HashSet<DLCPackID> dlcToLoad = new HashSet<DLCPackID>();
			HashSet<DLCPackID> staleLoadedDLC = new HashSet<DLCPackID>(this.mDLCManager.AllLoadedDLCPackIDs);
			int numPacksToLoad = 0;
			if (startSettings != null)
			{
				foreach (KeyValuePair<CommanderID, PlayerSelection> keyValuePair in startSettings.PlayerSelections)
				{
					CommanderDescription desc = keyValuePair.Value.Desc;
					if (desc.SelectedUnitSkinPack != DLCPackID.kInvalidID && this.mDLCManager.DoesPackExist(desc.SelectedUnitSkinPack))
					{
						staleLoadedDLC.Remove(desc.SelectedUnitSkinPack);
						if (!this.mDLCManager.IsPackLoaded(desc.SelectedUnitSkinPack) && dlcToLoad.Add(desc.SelectedUnitSkinPack))
						{
							numPacksToLoad++;
							this.mDLCManager.LoadDLC(desc.SelectedUnitSkinPack).OnDone(delegate(UnityEngine.Object loadedAsset)
							{
								numPacksToLoad--;
							}).OnReject(delegate(string reason)
							{
								numPacksToLoad--;
							});
						}
					}
				}
			}
			using (HashSet<DLCPackID>.Enumerator enumerator2 = staleLoadedDLC.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					DLCPackID id = enumerator2.Current;
					this.mDLCManager.UnloadDLC(this.mDLCManager.GetDLCPackHeader(id));
				}
				goto IL_2F4;
			}
			IL_2DD:
			yield return null;
			IL_2F4:
			if (numPacksToLoad <= 0)
			{
				yield return this.mCoroutineHost.StartNewCoroutine(this.OnSceneLoadStart(levelDef, scene, this.mGameMode, this.mReplayMode, dependencies));
				yield return this.mCoroutineHost.StartNewCoroutine(scene.AsyncLoad());
				if (!scene.IsLoaded)
				{
					Log.Error(Log.Channel.Online, "[LOAD] - Scene {0} failed to load!", new object[]
					{
						levelDef.SceneName
					});
					this.OnSceneLoadFailed(levelDef, scene, this.mGameMode, this.mReplayMode, dependencies);
				}
				else
				{
					VictoryConditions victoryConditions;
					string[] randomWreckArtifacts;
					int spawnedWreckArtifactCount;
					this.GenerateDependenciesForGameMode(this.mGameMode, levelDef, dependencies, out victoryConditions, out randomWreckArtifacts, out spawnedWreckArtifactCount);
					bool includeArtifactRetrievalOnlyEntities = (victoryConditions & VictoryConditions.Retrieval) != (VictoryConditions)0;
					bool includeMissionOnlyEntities = this.mGameMode == GameMode.SinglePlayer;
					IDictionary<CommanderID, CommanderAttributes> missionCommanders = CommanderLoadoutHelper.GetMissionCommandersFromManifest(this.DefaultMissionCommanders);
					DependencyContainer<MissionDependencies> missionDep = new DependencyContainer<MissionDependencies>(new MissionDependencies(SceneEntityGroup.GetAllEntityGroupCreationData(includeMissionOnlyEntities, includeArtifactRetrievalOnlyEntities), SceneEntityBase.GetAllEntityCreationData(includeMissionOnlyEntities, includeArtifactRetrievalOnlyEntities), missionCommanders, randomWreckArtifacts, spawnedWreckArtifactCount));
					dependencies.Combine(missionDep);
					dependencies.Combine(LevelManager.ProcessTerrainInfo(levelDef));
					FactionAttributesData factionAttributes = this.GetLocalPlayerFaction(gameMode, startSettings, missionCommanders) as FactionAttributesData;
					if (factionAttributes != null)
					{
						this.mAudioAssetBundleLoader.LoadAssetBundle(factionAttributes.m_VOCoreSpeechHierarchy);
					}
					else
					{
						Log.Error(Log.Channel.Audio | Log.Channel.Data, "Failed to find FactionAttributes for local Commander. Unable to load VO asset bundle!", new object[0]);
					}
					while (this.mAudioAssetBundleLoader.BundlesToLoad > 0)
					{
						yield return null;
					}
					yield return this.mCoroutineHost.StartNewCoroutine(this.OnSceneLoadComplete(levelDef, scene, this.mGameMode, this.mReplayMode, dependencies));
					this.mLoadedScene = scene;
					this.ManagerState = LevelManager.State.Game;
				}
				yield break;
			}
			goto IL_2DD;
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x000B3B44 File Offset: 0x000B1D44
		private static DependencyContainerBase ProcessTerrainInfo(LevelDefinition levelDef)
		{
			SimMapDependencies simMapDependencies = new SimMapDependencies();
			NavMeshManager navMeshManager = SceneHelper.FindObjectOfType<NavMeshManager>();
			if (navMeshManager != null && navMeshManager.NavMeshSerializedData != null)
			{
				simMapDependencies.SerializedSimMapData = navMeshManager.NavMeshSerializedData;
			}
			MapObstacleBase[] array = SceneHelper.FindObjectsOfType<MapObstacleBase>();
			List<MapObstacle> list = new List<MapObstacle>();
			foreach (MapObstacleBase mapObstacleBase in array)
			{
				UnitClass unitClass = UnitClass.None;
				if (mapObstacleBase.BlockedTypes != null)
				{
					foreach (UnitClass unitClass2 in mapObstacleBase.BlockedTypes)
					{
						unitClass |= unitClass2;
					}
				}
				ConvexBase convexBase = mapObstacleBase.CreateSimObstacle();
				if (convexBase == null)
				{
					Debug.LogError(string.Format("error: {0}: failed to create sim obstacle", mapObstacleBase.name), mapObstacleBase);
				}
				else
				{
					MapObstacle item = new MapObstacle(mapObstacleBase.BlocksLOF, mapObstacleBase.BlocksAllHeights, unitClass, convexBase);
					list.Add(item);
				}
			}
			simMapDependencies.Obstacles = list.ToArray();
			RidgeLine[] array3 = SceneHelper.FindObjectsOfType<RidgeLine>();
			List<SimRidgeLine> list2 = new List<SimRidgeLine>();
			foreach (RidgeLine ridgeLine in array3)
			{
				list2.Add(new SimRidgeLine(ridgeLine.SimPolyline));
			}
			simMapDependencies.RidgeLines = list2.ToArray();
			MapBounds[] array5 = SceneHelper.FindObjectsOfType<MapBounds>();
			if (array5 != null && array5.Length > 1)
			{
				Debug.LogWarning("More than one object in the map specifies map bounds!");
			}
			if (array5 != null && (array5.Length == 0 || array5[0] == null))
			{
				Debug.LogError("No map bounds defined! IGNORING ALL PATHFINDING INFORMATION!");
				simMapDependencies = new SimMapDependencies();
			}
			if (array5 != null && array5.Length > 0 && array5[0] != null)
			{
				simMapDependencies.MinPathfindingBounds = array5[0].Min;
				simMapDependencies.MaxPathfindingBounds = array5[0].Max;
			}
			SetRenderSettings setRenderSettings = SceneHelper.FindObjectOfType<SetRenderSettings>();
			if (setRenderSettings != null)
			{
				if (setRenderSettings.ParticleOcclusionSettings != null)
				{
					ParticleOcclusionMapAttributes particleOcclusionMapAttributes = setRenderSettings.ParticleOcclusionSettings.Data as ParticleOcclusionMapAttributes;
					if (particleOcclusionMapAttributes != null)
					{
						HeightMapAttributesAsset heightMap = particleOcclusionMapAttributes.HeightMap;
						if (heightMap != null)
						{
							HeightMapAttributes heightMapAttributes = heightMap.Data as HeightMapAttributes;
							simMapDependencies.HeightMapAttributes = heightMapAttributes;
						}
						else
						{
							Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "No OcclusionMap set for level. Unable to populate sim height map!", new object[0]);
						}
					}
					else
					{
						Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "No ParticleOcclusionMapAttributes specified in ParticleOcclusionSettings for level. Unable to populate sim height map!", new object[0]);
					}
				}
				else
				{
					Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "No ParticleOcclusionSettings set for level. Unable to populate sim height map!", new object[0]);
				}
			}
			else
			{
				Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "No SetRenderSettings set for level. Unable to populate sim height map!", new object[0]);
			}
			if (levelDef.AmbientHeatPoints < 0)
			{
				Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Level {0} has Ambient Heat Points set to negative amount {1}, which is not supported! Clamping to 0!", new object[]
				{
					levelDef.SceneName,
					levelDef.AmbientHeatPoints
				});
				simMapDependencies.AmbientHeatPoints = 0;
			}
			else
			{
				simMapDependencies.AmbientHeatPoints = levelDef.AmbientHeatPoints;
			}
			return new DependencyContainer<SimMapDependencies>(simMapDependencies);
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x000B4044 File Offset: 0x000B2244
		private IEnumerator ExitGame(SessionChangeReason reason)
		{
			this.ManagerState = LevelManager.State.GameExiting;
			if (this.mLoadedScene != null)
			{
				ShipbreakersMain.SuspendAudio(ShipbreakersMain.AudioPreset.Loading);
				yield return ShipbreakersMain.ScreenFader.FadeDown(1f, null);
				ShipbreakersMain.ResumeAudio(ShipbreakersMain.AudioPreset.Pause);
				ShipbreakersMain.ResumeAudio(ShipbreakersMain.AudioPreset.PostMission);
				this.mAudioAssetBundleLoader.UnloadBundledClips();
				AudioAssetBundleLoader.UnloadAudioData();
				try
				{
					this.OnSceneUnloadStart(this.mLoadedScene, reason);
					this.mLoadedScene.Unload();
				}
				catch (Exception ex)
				{
					Log.Exception(Log.Channel.UI, ex);
				}
				yield return new WaitForEndOfFrame();
				List<DLCAssetBundleBase> loadedDLC = new List<DLCAssetBundleBase>(this.mDLCManager.AllLoadedDLCAssetBundleHeaders);
				foreach (DLCAssetBundleBase dlcHeader in loadedDLC)
				{
					this.mDLCManager.UnloadDLC(dlcHeader);
				}
				try
				{
					this.OnSceneUnloadComplete(this.mLoadedScene, reason);
				}
				catch (Exception ex2)
				{
					Log.Exception(Log.Channel.UI, ex2);
				}
				yield return Resources.UnloadUnusedAssets();
				GC.Collect();
				if (reason == SessionChangeReason.ExitGame)
				{
					PlayerCustomizationPanel.LoadPreviewContent();
				}
				this.mLoadedScene = null;
			}
			ShipbreakersMain.ResetDynamicMixer();
			this.ManagerState = LevelManager.State.Startup;
			yield break;
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x000B4140 File Offset: 0x000B2340
		private IEnumerator UnloadAndLoadLevel(GameMode gameMode, ReplayMode replayMode, LevelDefinition levelToLoad, DependencyContainerBase dependencies)
		{
			yield return this.mCoroutineHost.StartNewCoroutine(this.ExitGame(SessionChangeReason.Transition));
			yield return this.mCoroutineHost.StartNewCoroutine(this.LoadGame(gameMode, replayMode, levelToLoad, dependencies));
			yield break;
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x000B417C File Offset: 0x000B237C
		private void GenerateDependenciesForGameMode(GameMode gameMode, LevelDefinition levelDef, DependencyContainerBase dependencies, out VictoryConditions victoryConditions, out string[] randomWreckArtifacts, out int spawnedWreckArtifactCount)
		{
			victoryConditions = VictoryConditions.None;
			randomWreckArtifacts = null;
			spawnedWreckArtifactCount = 0;
			switch (gameMode)
			{
			case GameMode.SinglePlayer:
				if (levelDef != null)
				{
					randomWreckArtifacts = levelDef.RandomArtifacts;
					spawnedWreckArtifactCount = levelDef.MaximumSpawnedWreckArtifacts;
					return;
				}
				Log.Error(Log.Channel.Core, "No levelDef defined while looking for listed RandomArtifacts", new object[0]);
				return;
			case GameMode.Multiplayer:
			case GameMode.AISkirmish:
			{
				GameStartSettings gameStartSettings;
				if (dependencies.Get<GameStartSettings>(out gameStartSettings))
				{
					victoryConditions = gameStartSettings.VictoryConditions;
				}
				else
				{
					Log.Error(Log.Channel.Core, "No GameStartSettings specified", new object[0]);
				}
				if (levelDef != null)
				{
					randomWreckArtifacts = levelDef.RandomArtifacts;
					spawnedWreckArtifactCount = levelDef.MaximumSpawnedWreckArtifacts;
					return;
				}
				Log.Error(Log.Channel.Core, "No levelDef defined while looking for listed RandomArtifacts", new object[0]);
				return;
			}
			default:
				Log.Error(Log.Channel.UI, "Unable to generate dependencies for game mode {0}", new object[]
				{
					this.mGameMode
				});
				return;
			}
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x000B4248 File Offset: 0x000B2448
		private bool CheckIfAllPlayersHaveFinishedLoading()
		{
			if (this.AllPlayersReportLoaded)
			{
				if (this.mGameMode == GameMode.Multiplayer)
				{
					Log.Info(Log.Channel.Online, "All players report being loaded.", new object[0]);
				}
				this.OnAllPlayersLoaded();
				return true;
			}
			return false;
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x000B427C File Offset: 0x000B247C
		private LevelDefinition[] GetLevelDefinitions(GameMode mode)
		{
			LevelDefinition[] result;
			switch (mode)
			{
			case GameMode.SinglePlayer:
				result = this.LevelEntriesSP;
				break;
			case GameMode.Multiplayer:
			case GameMode.AISkirmish:
				result = this.LevelEntriesMP;
				break;
			default:
				Log.Error(Log.Channel.UI, "Game mode '{0}' not supported by GetLevelDefinitions()!", new object[]
				{
					mode.ToString()
				});
				return null;
			}
			return result;
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x000B42DC File Offset: 0x000B24DC
		private FactionAttributes GetLocalPlayerFaction(GameMode gameMode, GameStartSettings startSettings, IDictionary<CommanderID, CommanderAttributes> missionCommanders)
		{
			if (gameMode == GameMode.Multiplayer || gameMode == GameMode.AISkirmish)
			{
				using (IEnumerator<KeyValuePair<CommanderID, PlayerSelection>> enumerator = startSettings.PlayerSelections.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<CommanderID, PlayerSelection> keyValuePair = enumerator.Current;
						PlayerSelection value = keyValuePair.Value;
						if (value.Desc.PlayerType == PlayerType.Human && value.Desc.IsLocal(true))
						{
							return value.Attributes.Faction;
						}
					}
					goto IL_8A;
				}
			}
			if (missionCommanders != null)
			{
				CommanderAttributes commanderAttributes;
				missionCommanders.TryGetValue(CommanderID.DefaultLocalCommander, out commanderAttributes);
				if (commanderAttributes != null)
				{
					return commanderAttributes.Faction;
				}
			}
			IL_8A:
			return null;
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x000B4388 File Offset: 0x000B2588
		private static bool ValidateCanLoadGame(GameMode gameMode)
		{
			if (gameMode == GameMode.SinglePlayer && SteamAPIIntegration.IsRunningUsingFreeWeekendPromo)
			{
				Application.Quit();
				return false;
			}
			return true;
		}

		// Token: 0x040018F3 RID: 6387
		public const string kEmptyMapName = "<NO MAP>";

		// Token: 0x040018F4 RID: 6388
		public const int kInvalidMapIndex = -1;

		// Token: 0x040018F5 RID: 6389
		[SerializeField]
		private LevelDefinition[] m_LevelEntriesSP;

		// Token: 0x040018F6 RID: 6390
		[SerializeField]
		private LevelDefinition[] m_LevelEntriesMP;

		// Token: 0x040018F7 RID: 6391
		private ICoroutineHost mCoroutineHost;

		// Token: 0x040018F8 RID: 6392
		private Scene mLoadedScene;

		// Token: 0x040018F9 RID: 6393
		private GameMode mGameMode = GameMode.SinglePlayer;

		// Token: 0x040018FA RID: 6394
		private ReplayMode mReplayMode = ReplayMode.ReplaysDisabled;

		// Token: 0x040018FB RID: 6395
		private DLCManager mDLCManager;

		// Token: 0x040018FC RID: 6396
		private AudioAssetBundleLoader mAudioAssetBundleLoader;

		// Token: 0x040018FD RID: 6397
		private List<NetworkPlayerID> mPlayersInSession = new List<NetworkPlayerID>();

		// Token: 0x040018FE RID: 6398
		private List<NetworkPlayerID> mPlayersFinishedLoading = new List<NetworkPlayerID>();

		// Token: 0x040018FF RID: 6399
		private NetworkPlayerID mLocalPlayer;

		// Token: 0x04001900 RID: 6400
		private IGameLocalization mLocalizationManager;

		// Token: 0x02000381 RID: 897
		public enum State
		{
			// Token: 0x0400190D RID: 6413
			Startup,
			// Token: 0x0400190E RID: 6414
			GameLoading,
			// Token: 0x0400190F RID: 6415
			Game,
			// Token: 0x04001910 RID: 6416
			GameExiting
		}

		// Token: 0x02000382 RID: 898
		// (Invoke) Token: 0x06001E82 RID: 7810
		public delegate IEnumerator LoadHandler(LevelDefinition level, Scene scene, GameMode gameMode, ReplayMode replayMode, DependencyContainerBase loadDependencies);

		// Token: 0x02000383 RID: 899
		// (Invoke) Token: 0x06001E86 RID: 7814
		public delegate void LoadFailedHandler(LevelDefinition level, Scene scene, GameMode gameMode, ReplayMode replayMode, DependencyContainerBase loadDependencies);

		// Token: 0x02000384 RID: 900
		// (Invoke) Token: 0x06001E8A RID: 7818
		public delegate void UnloadHandler(Scene scene, SessionChangeReason reason);

		// Token: 0x02000385 RID: 901
		// (Invoke) Token: 0x06001E8E RID: 7822
		public delegate void PlayerLoadedHandler(NetworkPlayerID playerID);

		// Token: 0x02000386 RID: 902
		// (Invoke) Token: 0x06001E92 RID: 7826
		public delegate void AllPlayersLoadedHandler();
	}
}
