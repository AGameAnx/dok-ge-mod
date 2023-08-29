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
	[Serializable]
	public class LevelManager : ResettableSubsystemBase
	{
		public event LevelManager.LoadHandler SceneLoadStart;

		public event LevelManager.LoadHandler SceneLoadComplete;

		public event LevelManager.LoadFailedHandler SceneLoadFailed;

		public event LevelManager.UnloadHandler SceneUnloadStart;

		public event LevelManager.UnloadHandler SceneUnloadComplete;

		public event LevelManager.PlayerLoadedHandler PlayerLoadedScene;

		public event LevelManager.AllPlayersLoadedHandler AllPlayersLoaded;

		public LevelDefinition[] LevelEntriesSP
		{
			get
			{
				return this.m_LevelEntriesSP;
			}
		}

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

		public LevelManager.State ManagerState { get; private set; }

		public LevelDefinition LoadedLevel { get; private set; }

		public CommanderAttributes[] CommanderAttributes { get; private set; }

		public CommanderAttributes[] DefaultMissionCommanders { get; private set; }

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

		public LevelManager()
		{
		}

		public LevelManager(LevelManager other)
		{
			this.m_LevelEntriesSP = other.m_LevelEntriesSP;
			this.m_LevelEntriesMP = other.m_LevelEntriesMP;
		}

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

		public override void Reset()
		{
			throw new NotImplementedException();
		}

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

		public void ClearLoadingPlayers()
		{
			if (this.ManagerState != LevelManager.State.Startup)
			{
				this.mPlayersInSession.Clear();
				this.mPlayersInSession.Add(this.mLocalPlayer);
				this.CheckIfAllPlayersHaveFinishedLoading();
			}
		}

		public void RemoveLoadingPlayer(NetworkPlayerID playerID)
		{
			if (this.ManagerState != LevelManager.State.Startup)
			{
				this.mPlayersInSession.Remove(playerID);
				this.CheckIfAllPlayersHaveFinishedLoading();
			}
		}

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

		public string GetLocalizedLevelDisplayName(LevelDefinition def)
		{
			return this.mLocalizationManager.GetText(def.NameLocId);
		}

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

		public string GetLocalizedMapDescription(LevelDefinition def)
		{
			return this.mLocalizationManager.GetText(def.MapDescriptionLocId);
		}

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

		private void OnSceneLoadFailed(LevelDefinition level, Scene scene, GameMode gameMode, ReplayMode replayMode, DependencyContainerBase dependencies)
		{
			if (this.SceneLoadFailed != null)
			{
				this.SceneLoadFailed(level, scene, gameMode, replayMode, dependencies);
			}
			this.SetLoadingPlayerFinished(this.mLocalPlayer);
		}

		private void OnSceneUnloadStart(Scene loadedScene, SessionChangeReason reason)
		{
			if (this.SceneUnloadStart != null)
			{
				this.SceneUnloadStart(loadedScene, reason);
			}
		}

		private void OnSceneUnloadComplete(Scene loadedScene, SessionChangeReason reason)
		{
			if (this.SceneUnloadComplete != null)
			{
				this.SceneUnloadComplete(loadedScene, reason);
				this.LoadedLevel = null;
			}
		}

		private void OnPlayerLoadedScene(NetworkPlayerID playerID)
		{
			if (this.PlayerLoadedScene != null)
			{
				this.PlayerLoadedScene(playerID);
			}
		}

		private void OnAllPlayersLoaded()
		{
			if (this.AllPlayersLoaded != null)
			{
				this.AllPlayersLoaded();
			}
		}

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

		private IEnumerator UnloadAndLoadLevel(GameMode gameMode, ReplayMode replayMode, LevelDefinition levelToLoad, DependencyContainerBase dependencies)
		{
			yield return this.mCoroutineHost.StartNewCoroutine(this.ExitGame(SessionChangeReason.Transition));
			yield return this.mCoroutineHost.StartNewCoroutine(this.LoadGame(gameMode, replayMode, levelToLoad, dependencies));
			yield break;
		}

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

		private static bool ValidateCanLoadGame(GameMode gameMode)
		{
			if (gameMode == GameMode.SinglePlayer && SteamAPIIntegration.IsRunningUsingFreeWeekendPromo)
			{
				Application.Quit();
				return false;
			}
			return true;
		}

		public const string kEmptyMapName = "<NO MAP>";

		public const int kInvalidMapIndex = -1;

		[SerializeField]
		private LevelDefinition[] m_LevelEntriesSP;

		[SerializeField]
		private LevelDefinition[] m_LevelEntriesMP;

		private ICoroutineHost mCoroutineHost;

		private Scene mLoadedScene;

		private GameMode mGameMode = GameMode.SinglePlayer;

		private ReplayMode mReplayMode = ReplayMode.ReplaysDisabled;

		private DLCManager mDLCManager;

		private AudioAssetBundleLoader mAudioAssetBundleLoader;

		private List<NetworkPlayerID> mPlayersInSession = new List<NetworkPlayerID>();

		private List<NetworkPlayerID> mPlayersFinishedLoading = new List<NetworkPlayerID>();

		private NetworkPlayerID mLocalPlayer;

		private IGameLocalization mLocalizationManager;

		public enum State
		{
			Startup,
			GameLoading,
			Game,
			GameExiting
		}

		public delegate IEnumerator LoadHandler(LevelDefinition level, Scene scene, GameMode gameMode, ReplayMode replayMode, DependencyContainerBase loadDependencies);

		public delegate void LoadFailedHandler(LevelDefinition level, Scene scene, GameMode gameMode, ReplayMode replayMode, DependencyContainerBase loadDependencies);

		public delegate void UnloadHandler(Scene scene, SessionChangeReason reason);

		public delegate void PlayerLoadedHandler(NetworkPlayerID playerID);

		public delegate void AllPlayersLoadedHandler();
	}
}
