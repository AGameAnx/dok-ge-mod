using System;
using System.Collections;
using System.Collections.Generic;
using BBI.Core.Events;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Game.Events;
using BBI.Game.Network;
using BBI.Game.Replay;
using BBI.Game.Simulation;
using BBI.Steam;
using BBI.Unity.Core.Utility;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.DLC;
using BBI.Unity.Game.Events;
using BBI.Unity.Game.HUD;
using BBI.Unity.Game.Network;
using BBI.Unity.Game.Utility;
using BBI.Unity.Game.World;
using Epic.OnlineServices;
using GG.EpicGames;
using UnityEngine;
using Subsystem;
using System.Net;
using System.Xml;
using System.Text.RegularExpressions;

namespace BBI.Unity.Game.UI.Frontend.Helpers
{
	public abstract class LobbySetupPanelBase : BlackbirdPanelBase
	{
		protected virtual UIWidgetState StartButtonState
		{
			set
			{
				UIHelper.SetUIEventHandlerState(this.m_StartCountdownButton, ref this.mStartButtonState, value);
			}
		}

		protected virtual UIWidgetState CancelCountdownButtonState
		{
			get
			{
				return this.mCancelCoutdownButtonState;
			}
			set
			{
				UIHelper.SetUIEventHandlerState(this.m_CancelCountDownButton, ref this.mCancelCoutdownButtonState, value);
			}
		}

		protected virtual UIWidgetState LeaveButtonState
		{
			set
			{
				UIHelper.SetUIEventHandlerState(this.m_LeaveButton, ref this.mLeaveButtonState, value);
			}
		}

		protected abstract LobbyRole SetupLobbyRole { get; }

		protected abstract int InitialLobbySize { get; }

		protected abstract int DebugWindowID { get; }

		protected bool IsLobbyHost
		{
			get
			{
				return this.mLobby != null && this.mLobby.IsHost;
			}
		}

		protected bool IsEverythingInitialized
		{
			get
			{
				return this.mLobby != null && this.mLobby.IsGroupIDValid && this.mLobby.IsPlayerInGroup(this.mConnectionManager.LocalPlayerID) && this.mSession != null && this.mSession.IsGroupIDValid && this.mSession.IsPlayerInGroup(this.mConnectionManager.LocalPlayerID) && this.mParty != null && this.mParty.IsGroupIDValid && this.mParty.IsPlayerInGroup(this.mConnectionManager.LocalPlayerID);
			}
		}

		protected virtual UIWidgetState GetDesiredStartButtonState()
		{
			if (!this.IsEverythingInitialized)
			{
				return UIWidgetState.NotSet;
			}
			if (!this.IsLobbyHost)
			{
				return UIWidgetState.Hidden;
			}
			if (this.mGameStartCountdown.IsActive)
			{
				return UIWidgetState.Hidden;
			}
			if (this.mUIEventHandler.TargetState == MultiplayerUIEventsHandler.SubState.StartingGame)
			{
				return UIWidgetState.Hidden;
			}
			return UIWidgetState.Enabled;
		}

		protected virtual UIWidgetState GetDesiredCancelButtonState()
		{
			if (!this.IsEverythingInitialized)
			{
				return UIWidgetState.NotSet;
			}
			if (!this.IsLobbyHost)
			{
				return UIWidgetState.Hidden;
			}
			if (!this.mLobby.IsPerformingCountdown)
			{
				return UIWidgetState.Hidden;
			}
			if (!this.mGameStartCountdown.IsActive)
			{
				return UIWidgetState.Hidden;
			}
			if (this.mUIEventHandler.TargetState == MultiplayerUIEventsHandler.SubState.StartingGame)
			{
				return UIWidgetState.Hidden;
			}
			return UIWidgetState.Enabled;
		}

		protected virtual UIWidgetState GetDesiredLeaveButtonState()
		{
			return UIWidgetState.Enabled;
		}

		protected bool IsLocalUserInLobby
		{
			get
			{
				return this.mLobby != null && this.mLobby.IsGroupIDValid && this.mLobby.IsPlayerInGroup(this.mConnectionManager.LocalPlayerID);
			}
		}

		protected bool AreAllLobbyPlayersPropagated
		{
			get
			{
				if (this.mLobby.PlayerCount != this.mSharedLobbyData.CommanderCollection.NetworkCommanderCount)
				{
					return false;
				}
				if (this.mLobby.PlayerCount != this.mSharedLobbyData.PlayerInfo.Count)
				{
					return false;
				}
				foreach (NetworkPlayerInfo networkPlayerInfo in this.mSharedLobbyData.PlayerInfo)
				{
					if (!NetworkPlayerUtils.ArePlayersOnBothLists(this.mLobby.PlayerIDs, networkPlayerInfo.SessionPlayers))
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool AreAllCommandersAssignedToTeams
		{
			get
			{
				if (this.mSharedLobbyData.CommanderCollection.Descriptions.Count == 0)
				{
					return false;
				}
				foreach (CommanderDescription commanderDescription in this.mSharedLobbyData.CommanderCollection.Descriptions)
				{
					if (commanderDescription.TeamID == TeamID.None)
					{
						return false;
					}
				}
				return true;
			}
		}

		protected virtual bool HasClientReceivedAllStartData
		{
			get
			{
				return this.mSharedLobbyData.CommanderSpawnDetails.Count != 0 && this.mSharedLobbyData.SimRandomSeed != 0 && this.mSharedLobbyData.SelectedSceneIndex != -1;
			}
		}

		public bool IsCreatingOrJoiningLobby
		{
			get
			{
				return this.mCreateOrJoinLobbyCoroutine.IsActive;
			}
		}

		protected virtual void CreateLobby(string lobbyName, LobbyRole lobbyRole, LobbyType lobbyType, int memberLimit)
		{
			Log.Info(Log.Channel.Online, "Player {0} creating a {1} lobby...", new object[]
			{
				this.mConnectionManager.LocalPlayerID,
				this.SetupLobbyRole
			});
			if (string.IsNullOrEmpty(lobbyName))
			{
				lobbyName = EpicAPIIntegration.UserName;
			}
			this.InitializeHostLobby(lobbyName, lobbyRole, lobbyType);
			this.InitializeSession();
			this.InitializeParty();
			this.RegisterCallbacks();
			this.mLobby.CreateGroup(memberLimit);
			this.mDummySteamLobby.CreateGroup(0);
			UIHelper.ShowFESpinnerOverlay("ID_UI_FE_GEN_SPINNER_CONNECTING_236");
			this.mUIEventHandler.OnCreate();
			this.mCreateOrJoinLobbyCoroutine.Start();
		}

		protected virtual void JoinLobby(ulong epicLobbyID, ulong steamLobbyID = 0UL)
		{
			Log.Info(Log.Channel.Online, "Player {0} joining a {1} lobby...", new object[]
			{
				this.mConnectionManager.LocalPlayerID,
				this.SetupLobbyRole
			});
			this.InitializeClientLobby();
			this.InitializeSession();
			this.InitializeParty();
			this.RegisterCallbacks();
			if (steamLobbyID != 0UL)
			{
				this.mDummySteamLobby.JoinGroup(steamLobbyID);
			}
			else
			{
				this.mLobby.JoinGroup(epicLobbyID);
				if (this.mDummySteamLobby.IsGroupIDValid)
				{
					Log.Warn(Log.Channel.Online, "Was already in an existing steam lobby, leaving.", new object[0]);
					this.mDummySteamLobby.LeaveGroup(PlayerGroupLeaveReason.Migrate);
				}
				this.mDummySteamLobby.CreateGroup(0);
			}
			UIHelper.ShowFESpinnerOverlay("ID_UI_FE_GEN_SPINNER_CONNECTING_236");
			this.mUIEventHandler.OnCreate();
			this.mCreateOrJoinLobbyCoroutine.Start();
		}

		protected override void OnInitialized()
		{
			if (this.m_ClanChatPanel == null)
			{
				Log.Error(Log.Channel.UI, "CustomGameListPanel -> ClanChatPanel is not assigned!", new object[0]);
			}
			if (this.m_PlayerCustomizationPanel == null)
			{
				Log.Error(Log.Channel.UI, "CustomGameListPanel -> PlayerCustomizationPanel is not assigned!", new object[0]);
			}
			if (!base.GlobalDependencyContainer.Get<ConnectionManagerBase>(out this.mConnectionManager))
			{
				Log.Error(Log.Channel.UI, "No ConnectionManager in global dependencies for {0}! This is a serious error!", new object[]
				{
					base.GetType()
				});
			}
			if (!base.GlobalDependencyContainer.Get<DLCManager>(out this.mDLCManager))
			{
				Log.Error(Log.Channel.UI, "No DLCManager in global dependencies for {0}! This is a serious error!", new object[]
				{
					base.GetType()
				});
			}
			if (!base.GlobalDependencyContainer.Get<LevelManager>(out this.mLevelManager))
			{
				Log.Error(Log.Channel.UI, "No LevelManager in global dependencies for {0}! This is a serious error!", new object[]
				{
					base.GetType()
				});
			}
			if (!base.GlobalDependencyContainer.Get<IParty>(out this.mParty))
			{
				Log.Error(Log.Channel.UI, "No party in global dependencies for {0}! This is a serious error!", new object[]
				{
					base.GetType()
				});
			}
			if (!base.GlobalDependencyContainer.Get<UnitHUDInteractionAttributes>(out this.mUnitHUDInteractionAttributes))
			{
				Log.Error(Log.Channel.UI, "No UnitHUDInteractionAttributes supplied in global dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			ShipbreakersMain.PresentationEventSystem.AddHandler<LobbyJoinRequestEvent>(new BBI.Core.Events.EventHandler<LobbyJoinRequestEvent>(this.OnLobbyJoinRequestEvent));
			ShipbreakersMain.PresentationEventSystem.AddHandler<LobbyCreateRequestEventBase>(new BBI.Core.Events.EventHandler<LobbyCreateRequestEventBase>(this.OnLobbyCreateRequestEvent));
			ShipbreakersMain.PresentationEventSystem.AddHandler<MatchMakingMonitorEvent>(new BBI.Core.Events.EventHandler<MatchMakingMonitorEvent>(this.OnNetworkMonitoringEvent));
			this.mUIEventHandler = new MultiplayerUIEventsHandler();
			this.mUIEventHandler.Initialize(this.SetupLobbyRole, this.m_FEFSM, this.m_LobbyToggleFSM);
			this.mClientJoinPartyCoroutine = new NetworkTimedCoroutine(new NetworkTimedCoroutine.TimeoutHandler(this.OnClientJoinPartyTimeout), new NetworkTimedCoroutine.EvaluateHandler(this.OnClientJoinPartyEvaluate), new NetworkTimedCoroutine.SuccessHandler(this.OnClientJoinPartySuccess), new NetworkTimedCoroutine.FailureHandler(this.OnClientJoinPartyFailure));
			this.mClientSyncGameStartDataCoroutine = new NetworkTimedCoroutine(new NetworkTimedCoroutine.TimeoutHandler(this.OnClientSyncGameStartDataTimeout), new NetworkTimedCoroutine.EvaluateHandler(this.OnClientSyncGameStartDataEvaluate), new NetworkTimedCoroutine.SuccessHandler(this.OnClientSyncGameStartDataSuccess), null);
			this.mCreateOrJoinLobbyCoroutine = new NetworkTimedCoroutine(30f, 0.25f, new NetworkTimedCoroutine.TimeoutHandler(this.OnCreateOrJoinLobbyTimeout), new NetworkTimedCoroutine.EvaluateHandler(this.OnCreateOrJoinLobbyEvaluate), new NetworkTimedCoroutine.SuccessHandler(this.OnCreateOrJoinLobbySuccess), new NetworkTimedCoroutine.FailureHandler(this.OnCreateOrJoinLobbyFailure));
			this.mGameStartCountdown = new GameStartCountdown((float)this.m_GameStartCountdownSec, this.m_GameStartCountdown, this.m_GameStartCountdownValue, new NetworkTimedCoroutine.SuccessHandler(this.OnStartGameCountdownSuccess));
			this.m_StartCountdownButton.ClickAction = new NGUIEventHandler.NGUIEventDelegate(this.OnClickStartCountdown);
			this.m_CancelCountDownButton.ClickAction = new NGUIEventHandler.NGUIEventDelegate(this.OnClickCancelCountdown);
			this.m_LeaveButton.ClickAction = new NGUIEventHandler.NGUIEventDelegate(this.OnClickLeave);
		}

		protected override void Update()
		{
			if (this.mLobby != null)
			{
				this.mLobby.Update();
			}
			if (this.mDummySteamLobby != null && this.mDummySteamLobby.IsGroupIDValid)
			{
				this.mDummySteamLobby.Update();
			}
			if (this.mSession != null)
			{
				this.mSession.Update();
			}
			this.UpdateAllLobbyCoroutines();
			base.Update();
			this.UpdateUIState();
		}

		protected virtual void OnGUI()
		{
			if (this.IsEverythingInitialized && this.mDebugLobbyDataShow)
			{
				this.mDebugLobbyDataRect = GUILayout.Window(this.DebugWindowID, this.mDebugLobbyDataRect, new GUI.WindowFunction(this.DebugDrawLobbyData), "Lobby Debug", new GUILayoutOption[0]);
			}
		}

		protected virtual void DebugDrawLobbyData(int windowID)
		{
			GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
			if (this.mLobby != null && this.mLobby.IsActive)
			{
				GUILayout.Label(string.Format("Lobby ID: {0}", this.mLobby.GroupID), new GUILayoutOption[0]);
			}
			else
			{
				GUILayout.Label("Lobby ID: Invalid", new GUILayoutOption[0]);
			}
			if (this.mParty != null && this.mParty.IsActive)
			{
				GUILayout.Label(string.Format("Party ID: {0}", this.mParty.GroupID), new GUILayoutOption[0]);
			}
			else
			{
				GUILayout.Label("Party ID: Invalid", new GUILayoutOption[0]);
			}
			if (this.mSession != null && this.mSession.IsActive)
			{
				GUILayout.Label(string.Format("Session ID: {0}", this.mSession.GroupID), new GUILayoutOption[0]);
			}
			else
			{
				GUILayout.Label("Session ID: Invalid", new GUILayoutOption[0]);
			}
			GUILayout.EndVertical();
			GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
			GUILayout.Label("Global Metadata", new GUILayoutOption[0]);
			IEnumerable<PlayerGroupMetaDataItem> globalMetaDataItems = this.mLobby.GetGlobalMetaDataItems();
			foreach (PlayerGroupMetaDataItem playerGroupMetaDataItem in globalMetaDataItems)
			{
				GUILayout.Label(string.Format("{0}:{1}", playerGroupMetaDataItem.Key, playerGroupMetaDataItem.Data), new GUILayoutOption[0]);
			}
			GUILayout.EndVertical();
			GUI.DragWindow();
		}

		protected virtual void UpdateUIState()
		{
			this.CancelCountdownButtonState = this.GetDesiredCancelButtonState();
			if (this.CancelCountdownButtonState == UIWidgetState.Enabled)
			{
				this.StartButtonState = UIWidgetState.Hidden;
			}
			else
			{
				this.StartButtonState = this.GetDesiredStartButtonState();
			}
			this.LeaveButtonState = this.GetDesiredLeaveButtonState();
		}

		protected void ShowSystemMessage(string messageFormatLocID, params object[] args)
		{
			if (this.m_ChatPanel != null)
			{
				string text = string.Format(this.mLocalizationManager.GetText(messageFormatLocID), args);
				this.m_ChatPanel.SendLobbySystemAnnouncement(text);
			}
		}

		protected void SetRichPresence(LobbySetupPanelBase.RichPresenceMode mode)
		{
			if (mode == LobbySetupPanelBase.RichPresenceMode.EnableOverlayJoins)
			{
				Log.Info(Log.Channel.Online, "Enabling Steam rich presence so friends can join our match.", new object[0]);
				ulong steamLobbyID = 0UL;
				if (this.mDummySteamLobby != null)
				{
					steamLobbyID = this.mDummySteamLobby.GroupID;
				}
				string lobbyIDString = EpicMatchmakingIntegration.GetLobbyIDString(this.mLobby.GroupID);
				SteamFriendsIntegration.SetRichPresence("connect", FriendInviteInfo.LobbyToCommandString(lobbyIDString, steamLobbyID, this.SetupLobbyRole));
				return;
			}
			Log.Info(Log.Channel.Online, "Disabling Steam rich presence so friends cannot join our match.", new object[0]);
			SteamFriendsIntegration.SetRichPresence("connect", null);
		}

		protected virtual void InitiateStartGame()
		{
			this.SetRichPresence(LobbySetupPanelBase.RichPresenceMode.DisableOverlayJoins);
		}

		protected virtual bool StartLoadingGame()
		{
			Log.Info(Log.Channel.Online, "Player {0} starting game...", new object[]
			{
				this.mLobby.LocalPlayerID
			});
			this.StopAllLobbyCoroutines();
			LevelDefinition level = this.mLevelManager.FindLevelByIndex(this.mSharedLobbyData.SelectedSceneIndex, GameMode.Multiplayer);

			Dictionary<CommanderID, TeamID> commanderTeamIDs;
			TeamSetting teamSettings;
			TryGetTeams(out commanderTeamIDs, out teamSettings);
			MapModManager.SetMap(level, GameMode.Multiplayer, teamSettings, commanderTeamIDs);

			this.mLevelManager.StartLoadLevel(GameMode.Multiplayer, ReplayMode.RecordingGame, level, this.GetStartDependencies());
			return true;
		}

		protected abstract bool TryGetTeams(out Dictionary<CommanderID, TeamID> commanderTeamIDs, out TeamSetting teamSetting);

		protected bool PopulateHostSpawnPointsAssignment(LevelDefinition selectedLevel)
		{
			Dictionary<CommanderID, TeamID> dictionary;
			TeamSetting teamSetting;
			if (!this.TryGetTeams(out dictionary, out teamSetting))
			{
				Log.Error(Log.Channel.Online, "Unable to get team entries for the commanders!", new object[0]);
				return false;
			}
			MapModManager.SetMap(selectedLevel, GameMode.Multiplayer, teamSetting, dictionary);
			if (dictionary.Count != this.mSharedLobbyData.CommanderCollection.TotalCommanderCount)
			{
				Log.Error(Log.Channel.Online, "There are not enough team entries for each commander ID! Unable to start the game!", new object[0]);
				return false;
			}
			List<CommanderSpawnInfo> commanderSpawnDetails;
			if (!SpawnAssigner.GenerateSpawnPoints(selectedLevel, dictionary, teamSetting, out commanderSpawnDetails))
			{
				Log.Error(Log.Channel.Online, "Unable to generate commander spawn points!", new object[0]);
				return false;
			}
			this.mSharedLobbyData.CommanderSpawnDetails = commanderSpawnDetails;
			Log.Info(Log.Channel.Online, "Player {0} sending spawn details to clients...", new object[]
			{
				this.mLobby.LocalPlayerID
			});
			return true;
		}

		protected void SetFactionSelectionForLocalPlayer(PlayerFactionSelection factionSelection, bool applyToCustomizationSettings)
		{
			int num = (int)factionSelection.Faction;
			DLCPackID dlcpackID = factionSelection.DLCPackID;
			bool randomFaction = factionSelection.RandomFaction;
			if (randomFaction)
			{
				Dictionary<string, PlayerFactionSelection> factionOptions = GameLobbyView.GetFactionOptions(this.mLevelManager, this.mDLCManager, PlayerType.Human, true);
				PlayerFactionSelection playerFactionSelection;
				if (GameLobbyView.RollForRandomFaction(factionOptions, out playerFactionSelection))
				{
					num = (int)playerFactionSelection.Faction;
					dlcpackID = playerFactionSelection.DLCPackID;
				}
			}
			if (num < 0)
			{
				Log.Error(Log.Channel.UI, "Local player attempted to set faction index to invalid index {0}, reseting to {1}!", new object[]
				{
					num,
					0
				});
				num = 0;
			}
			if (dlcpackID != DLCPackID.kInvalidID && !this.mDLCManager.IsPackOwned(dlcpackID))
			{
				Log.Error(Log.Channel.UI, "Local player attempted to set faction to unowned DLCPackID {0}, reseting to invalid!", new object[]
				{
					dlcpackID
				});
				dlcpackID = DLCPackID.kInvalidID;
			}
			this.mSharedLobbyData.LocalPlayerFactionIndex = num;
			this.mSharedLobbyData.LocalPlayerUnitSkinPackID = dlcpackID;
			this.mSharedLobbyData.LocalPlayerRandomFaction = factionSelection.RandomFaction;
			if (applyToCustomizationSettings)
			{
				if (!randomFaction)
				{
					ShipbreakersMain.UserSettings.Customization.FactionIndex = num;
					ShipbreakersMain.UserSettings.Customization.UnitSkinPackID[num] = dlcpackID;
				}
				ShipbreakersMain.UserSettings.Customization.RandomFaction = factionSelection.RandomFaction;
			}
		}

		protected abstract DependencyContainerBase GetStartDependencies();

		protected void InitializeHostLobby(string lobbyName, LobbyRole lobbyRole, LobbyType lobbyType)
		{
			this.mLobby = new EpicLobby(this.mConnectionManager, lobbyName, lobbyRole, lobbyType, false);
			this.mDummySteamLobby = new DummySteamLobby(this.mConnectionManager, lobbyName, lobbyRole, lobbyType);
			this.InitializeLobby();
		}

		protected void InitializeClientLobby()
		{
			this.mLobby = new EpicLobby(this.mConnectionManager, false);
			this.mDummySteamLobby = new DummySteamLobby(this.mConnectionManager);
			this.InitializeLobby();
		}

		private void InitializeLobby()
		{
			this.mLobby.GroupCreate += this.OnLobbyCreate;
			this.mLobby.GroupEnter += this.OnLobbyEnter;
			this.mLobby.GroupMigrate += this.OnLobbyMigrate;
			this.mLobby.GroupHostMigrate += this.OnLobbyHostMigrate;
			this.mLobby.GroupDisconnect += this.OnLobbyDisconnect;
			this.mLobby.CountdownStart += this.OnLobbyCountdownStart;
			this.mLobby.CountdownFinish += this.OnLobbyCountdownFinish;
			this.mLobby.CountdownCancel += this.OnLobbyCountdownCancel;
			this.mLobby.LoadingStart += this.OnLobbyLoadingStart;
			this.mLobby.LoadingFinish += this.OnLobbyLoadingFinish;
			this.mLobby.GlobalMetaDataUpdate += this.OnLobbyGlobalMetaDataUpdate;
			this.mLobby.PlayerMetaDataUpdate += this.OnLobbyPlayerMetaDataUpdate;
			this.mLobby.PlayerJoin += this.OnLobbyPlayerJoin;
			this.mLobby.PlayerLeave += this.OnLobbyPlayerLeave;
			this.mLobby.PlayerKick += this.OnLobbyKicked;
			this.mDummySteamLobby.GroupCreate += this.OnSteamLobbyCreate;
			this.mDummySteamLobby.GroupEnter += this.OnSteamLobbyEnter;
		}

		protected void ShutdownLobby(PlayerGroupLeaveReason reason)
		{
			if (this.mLobby != null)
			{
				this.mLobby.GroupCreate -= this.OnLobbyCreate;
				this.mLobby.GroupEnter -= this.OnLobbyEnter;
				this.mLobby.GroupMigrate -= this.OnLobbyMigrate;
				this.mLobby.GroupHostMigrate -= this.OnLobbyHostMigrate;
				this.mLobby.GroupDisconnect -= this.OnLobbyDisconnect;
				this.mLobby.CountdownStart -= this.OnLobbyCountdownStart;
				this.mLobby.CountdownFinish -= this.OnLobbyCountdownFinish;
				this.mLobby.CountdownCancel -= this.OnLobbyCountdownCancel;
				this.mLobby.LoadingStart -= this.OnLobbyLoadingStart;
				this.mLobby.LoadingFinish -= this.OnLobbyLoadingFinish;
				this.mLobby.GlobalMetaDataUpdate -= this.OnLobbyGlobalMetaDataUpdate;
				this.mLobby.PlayerMetaDataUpdate -= this.OnLobbyPlayerMetaDataUpdate;
				this.mLobby.PlayerJoin -= this.OnLobbyPlayerJoin;
				this.mLobby.PlayerLeave -= this.OnLobbyPlayerLeave;
				this.mLobby.PlayerKick -= this.OnLobbyKicked;
				ShipbreakersMain.PresentationEventSystem.Post(new LobbyLeftEvent(this.mLobby.GroupID));
				this.mDummySteamLobby.GroupCreate -= this.OnSteamLobbyCreate;
				this.mDummySteamLobby.GroupEnter -= this.OnSteamLobbyEnter;
				this.mDummySteamLobby.LeaveGroup(reason);
				this.mDummySteamLobby = null;
				this.mLobby.LeaveGroup(reason);
				this.mLobby = null;
			}
		}

		protected virtual void InitializeSession()
		{
			this.mSession = new EpicSession(this.mConnectionManager);
			this.mSession.GroupCreate += this.OnSessionCreate;
			this.mSession.GroupEnter += this.OnSessionEnter;
			this.mSession.PlayerJoin += this.OnSessionPlayerJoin;
			this.mSession.PlayerLeave += this.OnSessionPlayerLeave;
		}

		protected virtual void ShutdownSession(PlayerGroupLeaveReason reason)
		{
			if (this.mSession != null)
			{
				this.mSession.GroupCreate -= this.OnSessionCreate;
				this.mSession.GroupEnter -= this.OnSessionEnter;
				this.mSession.PlayerJoin -= this.OnSessionPlayerJoin;
				this.mSession.PlayerLeave -= this.OnSessionPlayerLeave;
				if (reason != PlayerGroupLeaveReason.StartGame)
				{
					this.mSession.LeaveGroup(reason);
				}
				this.mSession = null;
			}
		}

		protected virtual void InitializeParty()
		{
			if (this.mParty != null)
			{
				this.mParty.GroupCreate += this.OnPartyCreate;
				this.mParty.GroupEnter += this.OnPartyEnter;
				this.mParty.JoinLobby += this.OnPartyJoinLobby;
				this.mParty.PlayerJoin += this.OnPartyPlayerJoin;
				this.mParty.PlayerLeave += this.OnPartyPlayerLeave;
			}
		}

		protected virtual void ShutdownParty(PlayerGroupLeaveReason reason)
		{
			if (this.mParty != null)
			{
				this.mParty.GroupCreate -= this.OnPartyCreate;
				this.mParty.GroupEnter -= this.OnPartyEnter;
				this.mParty.JoinLobby -= this.OnPartyJoinLobby;
				this.mParty.PlayerJoin -= this.OnPartyPlayerJoin;
				this.mParty.PlayerLeave -= this.OnPartyPlayerLeave;
				if (reason != PlayerGroupLeaveReason.StartGame && reason != PlayerGroupLeaveReason.Migrate)
				{
					this.mParty.LeaveGroup(reason);
				}
			}
		}

		protected virtual void LeaveLobbyPanel(PlayerGroupLeaveReason reason)
		{
			Log.Info(Log.Channel.Online, "Player {0} is leaving a {1} lobby...", new object[]
			{
				this.mConnectionManager.LocalPlayerID,
				this.SetupLobbyRole
			});
			this.SetRichPresence(LobbySetupPanelBase.RichPresenceMode.DisableOverlayJoins);
			if (reason != PlayerGroupLeaveReason.Migrate)
			{
				this.mUIEventHandler.OnLeave(reason);
			}
			UIHelper.HideFESpinnerOverlay();
			this.StopAllLobbyCoroutines();
			this.mLobbyCreateRequestEvent = null;
			this.mLobbyJoinRequestEvent = null;
			this.DeregisterCallbacks();
			if (this.m_ChatPanel != null)
			{
				this.m_ChatPanel.Shutdown();
			}
			if (this.mSharedLobbyData != null)
			{
				this.mSharedLobbyData.Shutdown();
				this.mSharedLobbyData = null;
			}
			this.ShutdownLobby(reason);
			this.ShutdownParty(reason);
			this.ShutdownSession(reason);
			if (reason != PlayerGroupLeaveReason.StartGame && reason != PlayerGroupLeaveReason.Migrate && this.mConnectionManager != null)
			{
				this.mConnectionManager.RequestCloseAllConnections();
			}
			if (this.m_ClanChatPanel != null)
			{
				this.m_ClanChatPanel.Leave();
			}
		}

		protected virtual void UpdateAllLobbyCoroutines()
		{
			this.mClientJoinPartyCoroutine.Update();
			this.mClientSyncGameStartDataCoroutine.Update();
			this.mCreateOrJoinLobbyCoroutine.Update();
			this.mGameStartCountdown.Update();
		}

		protected virtual void StopAllLobbyCoroutines()
		{
			this.mClientJoinPartyCoroutine.Stop();
			this.mClientSyncGameStartDataCoroutine.Stop();
			this.mCreateOrJoinLobbyCoroutine.Stop();
			this.mGameStartCountdown.Stop();
		}

		protected virtual void OnClickStartCountdown(NGUIEventHandler handler)
		{
			this.mLobby.StartCountdown();
		}

		protected virtual void OnClickCancelCountdown(NGUIEventHandler handler)
		{
			if (this.mLobby != null)
			{
				this.mLobby.CancelCountdown();
			}
		}

		protected virtual void OnClickLeave(NGUIEventHandler handler)
		{
			this.LeaveLobbyPanel(PlayerGroupLeaveReason.Normal);
		}

		protected abstract void OnCustomizationSettingsApplied(UnitColors newColors, DLCPackID skinPackID, CustomizationFactionSetting newFactionIndex, bool randomFaction);

		private void RegisterCallbacks()
		{
			this.mLevelManager.SceneLoadStart += this.OnSceneLoadStart;
			this.mLevelManager.SceneLoadFailed += this.OnSceneLoadFailed;
			this.mLevelManager.SceneLoadComplete += this.OnSceneLoadComplete;
			this.mLevelManager.AllPlayersLoaded += this.OnAllPlayersLoaded;
			EpicMatchmakingIntegration.LobbyCreate += this.OnEpicLobbyCreate;
			if (this.m_PlayerCustomizationPanel != null)
			{
				this.m_PlayerCustomizationPanel.SettingsApplied += this.OnInternalCustomizationSettingsApplied;
				return;
			}
			Log.Error(Log.Channel.UI, "Player Customization Panel is not set", new object[0]);
		}

		private void DeregisterCallbacks()
		{
			this.mLevelManager.SceneLoadStart -= this.OnSceneLoadStart;
			this.mLevelManager.SceneLoadFailed -= this.OnSceneLoadFailed;
			this.mLevelManager.SceneLoadComplete -= this.OnSceneLoadComplete;
			this.mLevelManager.AllPlayersLoaded -= this.OnAllPlayersLoaded;
			EpicMatchmakingIntegration.LobbyCreate -= this.OnEpicLobbyCreate;
			if (this.m_PlayerCustomizationPanel != null)
			{
				this.m_PlayerCustomizationPanel.SettingsApplied -= this.OnInternalCustomizationSettingsApplied;
			}
		}

		private void OnNetworkMonitoringEvent(MatchMakingMonitorEvent ev)
		{
			if (base.isActiveAndEnabled && ev.EventType == NetworkConnectivityState.Disconnected)
			{
				UIHelper.ShowMessageBox(MessageBoxLayout.OK, "ID_UI_FE_GEN_MSG_MATCHMAKING_DISCONNECTED_TO_MAINMENU_647", new MessageBoxResultHandler(this.OnDisconnectConfirmed));
			}
		}

		private void OnDisconnectConfirmed(MessageBoxResult result)
		{
			this.LeaveLobbyPanel(PlayerGroupLeaveReason.Disconnect);
			UITransitionEvents.JumpToMainMenu();
		}

		private void OnLobbyCreateRequestEvent(LobbyCreateRequestEventBase e)
		{
			if (e.LobbyRole == this.SetupLobbyRole)
			{
				this.mLobbyCreateRequestEvent = e;
				if (base.isActiveAndEnabled)
				{
					this.ProcessSetupEvents();
				}
			}
		}

		private void OnLobbyJoinRequestEvent(LobbyJoinRequestEvent e)
		{
			if (e.LobbyRole == this.SetupLobbyRole && !this.IsLocalUserInLobby)
			{
				this.mLobbyJoinRequestEvent = e;
				if (base.isActiveAndEnabled)
				{
					this.ProcessSetupEvents();
				}
			}
		}

		protected virtual void OnLobbyCreate(PlayerGroupSetupStatus status)
		{
			if (status == PlayerGroupSetupStatus.Successful)
			{
				int seed = (int)(this.mLobby.GroupID / 4UL);
				UnityEngine.Random.seed = seed;
				return;
			}
			this.LeaveLobbyPanel(PlayerGroupLeaveReason.UnableToCreateOrJoin);
		}

		protected virtual void OnSteamLobbyCreate(PlayerGroupSetupStatus status)
		{
			if (status == PlayerGroupSetupStatus.Successful)
			{
				Log.Info(Log.Channel.Online, "OnSteamLobbyCreate: Created a steam lobby. '{0}'", new object[]
				{
					this.mDummySteamLobby.GroupID
				});
				string lobbyIDString = EpicMatchmakingIntegration.GetLobbyIDString(this.mLobby.GroupID);
				if (SteamMatchmakingIntegration.SetLobbyData(this.mDummySteamLobby.GroupID, "epiclobbyGUID", lobbyIDString))
				{
					Log.Info(Log.Channel.Online, "OnSteamLobbyCreate: Successfully set epic lobby id as metadata for steam lobby. '{0}', with Epic lobby {1}", new object[]
					{
						this.mDummySteamLobby.GroupID,
						lobbyIDString
					});
				}
			}
		}

		protected virtual void OnSteamLobbyEnter(PlayerGroupSetupStatus status)
		{
			if (status == PlayerGroupSetupStatus.Successful)
			{
				Log.Info(Log.Channel.Online, "OnSteamLobbyEnter: Entered a steam lobby. '{0}'", new object[]
				{
					this.mDummySteamLobby.GroupID
				});
				if (!this.mDummySteamLobby.IsHost)
				{
					string lobbyData = SteamMatchmakingIntegration.GetLobbyData(this.mDummySteamLobby.GroupID, "epiclobbyGUID");
					ulong num = 0UL;
					if (!string.IsNullOrEmpty(lobbyData))
					{
						num = UInt64Converter.ToUInt64(lobbyData);
					}
					if (num == 0UL)
					{
						Log.Error(Log.Channel.Online, "OnSteamLobbyEnter: Epic lobby is invalid or not unavailable for steam lobby '{0}'.", new object[]
						{
							this.mDummySteamLobby.GroupID
						});
						return;
					}
					this.mLobby.JoinGroup(num);
				}
			}
		}

		protected virtual void OnLobbyEnter(PlayerGroupSetupStatus status)
		{
			if (status == PlayerGroupSetupStatus.Successful)
			{
				Log.Info(Log.Channel.Online, "OnLobbyEnter: Entered an epic lobby.", new object[0]);
				if (this.m_ChatPanel != null)
				{
					this.m_ChatPanel.Initialize(this.mLobby);
				}
				this.mSharedLobbyData = new SharedLobbyData();
				this.mSharedLobbyData.Initialize(this.mLobby);
				this.mSharedLobbyData.LocalPlayerUnitColors = ShipbreakersMain.UserSettings.Customization.GetLocalPlayerUnitColors(this.mUnitHUDInteractionAttributes);
				EpicMatchmakingIntegration.CacheParty(string.Empty);
				CustomizationFactionSetting factionIndex = (CustomizationFactionSetting)ShipbreakersMain.UserSettings.Customization.FactionIndex;
				PlayerFactionSelection factionSelection = new PlayerFactionSelection(factionIndex, ShipbreakersMain.UserSettings.Customization.GetLocalPlayerSkinPackIDForFaction(this.mDLCManager, factionIndex), ShipbreakersMain.UserSettings.Customization.RandomFaction);
				this.SetFactionSelectionForLocalPlayer(factionSelection, false);
				if (this.IsLobbyHost)
				{
					this.mSession.GroupID = this.mLobby.GroupID;
					this.mSession.PlayerIDs = this.mLobby.PlayerIDs;
					this.mSession.CreateGroup(this.mLobby.MemberLimit);
				}
				if (this.mParty != null && this.mParty.IsHost && this.mParty.IsPlayerInGroup(this.mLobby.HostPlayerID))
				{
					PartyPersistentData partyPersistentData = (PartyPersistentData)this.mParty.PersistentData;
					if (partyPersistentData != null)
					{
						partyPersistentData.LobbyName = this.mLobby.LobbyName;
						partyPersistentData.LobbyRole = this.SetupLobbyRole;
						partyPersistentData.LobbyType = this.mLobby.LobbyType;
					}
				}
				string lobbyIDString = EpicMatchmakingIntegration.GetLobbyIDString(this.mLobby.GroupID);
				if (string.IsNullOrEmpty(lobbyIDString))
				{
					Log.Error(Log.Channel.Online, "LobbySetupPanelBase::OnLobbyEnter The epic lobby guid is unavailable or invalid!", new object[0]);
				}
				ShipbreakersMain.PresentationEventSystem.Post(new LobbyJoinedEvent(UInt64Converter.ToUInt64(lobbyIDString)));
				if (this.m_ClanChatPanel != null)
				{
					this.m_ClanChatPanel.Join();
					return;
				}
			}
			else
			{
				this.LeaveLobbyPanel(PlayerGroupLeaveReason.UnableToCreateOrJoin);
			}
		}

		protected virtual void OnLobbyHostMigrate(NetworkPlayerID previousHostID, NetworkPlayerID newHostID)
		{
			if (this.mUIEventHandler.CurrentState == MultiplayerUIEventsHandler.SubState.StartingGame && this.mSession != null)
			{
				this.mSession.HostPlayerID = newHostID;
			}
		}

		protected virtual void OnLobbyCountdownStart()
		{
			this.mGameStartCountdown.Start();
		}

		protected virtual void OnLobbyCountdownFinish()
		{
			if (this.IsLobbyHost)
			{
				this.InitiateStartGame();
			}
			this.mGameStartCountdown.Stop();
		}

		protected virtual void OnLobbyCountdownCancel()
		{
			this.mGameStartCountdown.Stop();
		}

		protected virtual void OnLobbyLoadingStart(NetworkPlayerID playerID)
		{
			if (this.mLobby.IsPlayerHost(playerID))
			{
				Log.Info(Log.Channel.Online, "Received loading start from host, waiting for all start data to arrive before starting game...", new object[0]);
				this.StopAllLobbyCoroutines();
				this.mClientSyncGameStartDataCoroutine.Start();
			}
		}

		private void OnLobbyLoadingFinish(NetworkPlayerID playerID)
		{
			this.mLevelManager.SetLoadingPlayerFinished(playerID);
		}

		protected virtual void OnLobbyGlobalMetaDataUpdate(bool initialUpdate)
		{
			if (!this.IsLobbyHost)
			{
				Log.Info(Log.Channel.Online, "Is not lobby host", new object[0]);
				if (!this.mSession.IsGroupIDValid && this.mSharedLobbyData.AssociatedSessionID != 0UL)
				{
					Log.Info(Log.Channel.Online, "Sesion group id is not valid", new object[0]);
					Log.Info(Log.Channel.Online, "Session Id from shared lobby data is not invalid", new object[0]);
					this.mSession.PlayerIDs = this.mLobby.PlayerIDs;
					this.mSession.JoinGroup(this.mSharedLobbyData.AssociatedSessionID);
					this.mSharedLobbyData.LocalPlayerSessionIDs = this.mSession.PlayerIDs;
				}
			}
			PartyPersistentData partyPersistentData = this.mParty.PersistentData as PartyPersistentData;
			if (partyPersistentData != null)
			{
				if (this.mLobby.HostPlayerID == this.mParty.HostPlayerID)
				{
					partyPersistentData.LobbyName = this.mLobby.LobbyName;
				}
				partyPersistentData.LobbyRole = this.SetupLobbyRole;
				partyPersistentData.LobbyType = this.mLobby.LobbyType;
			}
			if (initialUpdate)
			{
				this.mSharedLobbyData.LocalPlayerUpdated = true;
			}
		}

		protected virtual void OnLobbyPlayerMetaDataUpdate(NetworkPlayerID playerID)
		{
		}

		protected virtual void OnLobbyPlayerJoin(NetworkPlayerID playerID)
		{
			if (!playerID.IsLocalPlayer())
			{
				this.mConnectionManager.OpenConnection(playerID);
			}
		}

		protected virtual void OnLobbyPlayerLeave(NetworkPlayerID playerID, PlayerGroupLeaveReason reason)
		{
			if (this.IsLobbyHost && reason == PlayerGroupLeaveReason.Disconnect)
			{
				this.mLobby.KickPlayer(playerID);
			}
			if (reason != PlayerGroupLeaveReason.StartGame)
			{
				this.mSession.RemovePlayer(playerID, reason);
			}
			if (reason != PlayerGroupLeaveReason.StartGame && reason != PlayerGroupLeaveReason.Migrate)
			{
				this.mConnectionManager.RequestCloseConnection(playerID);
			}
		}

		protected abstract void OnLobbyKicked(ulong groupID, NetworkPlayerID targePlayerID);

		protected abstract void OnLobbyMigrate(ulong newLobbyID);

		protected virtual void OnLobbyDisconnect()
		{
			this.mLevelManager.ClearLoadingPlayers();
			this.LeaveLobbyPanel(PlayerGroupLeaveReason.Disconnect);
		}

		protected virtual void OnSessionCreate(PlayerGroupSetupStatus status)
		{
			if (status == PlayerGroupSetupStatus.Successful)
			{
				this.mSharedLobbyData.LocalPlayerSessionIDs = this.mSession.PlayerIDs;
				return;
			}
			this.LeaveLobbyPanel(PlayerGroupLeaveReason.UnableToCreateOrJoin);
		}

		protected virtual void OnSessionEnter(PlayerGroupSetupStatus status)
		{
			if (status == PlayerGroupSetupStatus.Successful)
			{
				this.mSession.HostPlayerID = this.mLobby.HostPlayerID;
				if (this.IsLobbyHost)
				{
					this.mSharedLobbyData.AssociatedSessionID = this.mSession.GroupID;
					if (!this.mParty.IsGroupIDValid)
					{
						this.mParty.CreateGroup(6);
					}
					else
					{
						Log.Info(Log.Channel.Online, "Setting the associated Party ID to '{0}'.", new object[]
						{
							this.mParty
						});
						this.mSharedLobbyData.AssociatedPartyID = EpicMatchmakingIntegration.GetLobbyIDString(this.mParty.GroupID);
						this.mParty.NotifyPlayersToJoinLobby(this.mLobby.GroupID);
					}
				}
				else if (!this.mParty.IsGroupIDValid)
				{
					Log.Info(Log.Channel.Online, "Player {0} is waiting for the associated party ID to join...", new object[]
					{
						this.mLobby.LocalPlayerID
					});
					this.mClientJoinPartyCoroutine.Start();
				}
				else
				{
					Log.Info(Log.Channel.Online, "Player {0} is already in party {1}, will not attempt to join the hosts party.", new object[]
					{
						this.mLobby.LocalPlayerID,
						this.mParty
					});
				}
				base.StartCoroutine(this.CheckLocalPlayerInfo());
				return;
			}
			this.LeaveLobbyPanel(PlayerGroupLeaveReason.UnableToCreateOrJoin);
		}

		private IEnumerator CheckLocalPlayerInfo()
		{
			bool checkName = true;
			while (checkName)
			{
				yield return new WaitForSeconds(2f);
				foreach (CommanderDescription commanderDescription in this.mSharedLobbyData.CommanderCollection.Descriptions)
				{
					if (commanderDescription.CommanderID == this.mSharedLobbyData.LocalCommanderID)
					{
						if (string.IsNullOrEmpty(commanderDescription.Name))
						{
							EpicMatchmakingIntegration.AddLocalUserAttributes(EpicMatchmakingIntegration.EpicLobbyType.Regular);
							break;
						}
						checkName = false;
						break;
					}
				}
			}
			yield break;
		}

		protected virtual void OnSessionPlayerJoin(NetworkPlayerID playerID)
		{
			this.mSharedLobbyData.LocalPlayerSessionIDs = this.mSession.PlayerIDs;
		}

		protected virtual void OnSessionPlayerLeave(NetworkPlayerID playerID, PlayerGroupLeaveReason reason)
		{
			this.mSharedLobbyData.LocalPlayerSessionIDs = this.mSession.PlayerIDs;
			this.mLevelManager.RemoveLoadingPlayer(playerID);
			if (this.mLobby != null && this.mSharedLobbyData != null && reason != PlayerGroupLeaveReason.StartGame && reason != PlayerGroupLeaveReason.Migrate && this.mLobby.IsHost)
			{
				CommanderID commanderID = this.mSharedLobbyData.CommanderCollection.FindCommanderID(playerID);
				if (commanderID != CommanderID.None)
				{
					this.mSharedLobbyData.RemoveCommander(commanderID);
				}
				if (this.mLobby.IsPlayerInGroup(playerID))
				{
					this.mLobby.KickPlayer(playerID);
				}
			}
		}

		protected virtual void OnPartyCreate(PlayerGroupSetupStatus status)
		{
			if (status == PlayerGroupSetupStatus.Successful)
			{
				return;
			}
			this.LeaveLobbyPanel(PlayerGroupLeaveReason.UnableToCreateOrJoin);
		}

		protected abstract void OnPartyJoinLobby(ulong lobbyID, string epicLobbyID);

		protected virtual void OnPartyEnter(PlayerGroupSetupStatus status)
		{
			if (status == PlayerGroupSetupStatus.Successful)
			{
				if (this.IsLobbyHost)
				{
					Log.Info(Log.Channel.Online, "Setting the associated Party ID to '{0}'.", new object[]
					{
						this.mParty
					});
					this.mSharedLobbyData.AssociatedPartyID = EpicMatchmakingIntegration.GetLobbyIDString(this.mParty.GroupID);
					return;
				}
			}
			else
			{
				this.LeaveLobbyPanel(PlayerGroupLeaveReason.UnableToCreateOrJoin);
			}
		}

		protected virtual void OnPartyPlayerJoin(NetworkPlayerID playerID)
		{
		}

		protected virtual void OnPartyPlayerLeave(NetworkPlayerID playerID, PlayerGroupLeaveReason reason)
		{
		}

		protected virtual void OnEverythingInitialized()
		{
			Log.Info(Log.Channel.Online, "The lobby, session and party are completely initialized for player {0}.", new object[]
			{
				this.mLobby.LocalPlayerID
			});
			if (this.IsLobbyHost)
			{
				this.mLobby.FinalizeLobby();
			}
			if (this.mDummySteamLobby.IsHost)
			{
				this.mDummySteamLobby.FinalizeLobby();
			}
			this.mUIEventHandler.TargetState = MultiplayerUIEventsHandler.SubState.InLobbySetup;
			UIHelper.HideFESpinnerOverlay();
		}

		private void OnEpicLobbyCreate(Result resultCode, ulong lobbyGUID, EpicMatchmakingIntegration.EpicLobbyType lobbyType)
		{
			if (this.mLobby.IsHost && this.mLobby.LobbyName.StartsWith("DoKGlobalChatRoom"))
			{
				Log.Info(Log.Channel.Online, "OnEpicLobbyCreate: Early exiting", new object[0]);
				return;
			}
			string lobbyData = SteamMatchmakingIntegration.GetLobbyData(this.mDummySteamLobby.GroupID, "epiclobbyGUID");
			if (string.IsNullOrEmpty(lobbyData))
			{
				string lobbyIDString = EpicMatchmakingIntegration.GetLobbyIDString(this.mLobby.GroupID);
				if (SteamMatchmakingIntegration.SetLobbyData(this.mDummySteamLobby.GroupID, "epiclobbyGUID", lobbyIDString))
				{
					Log.Info(Log.Channel.Online, "OnEpicLobbyCreate: Successfully set epic lobby id as metadata for steam lobby. '{0}', with Epic lobby {1}", new object[]
					{
						this.mDummySteamLobby.GroupID,
						lobbyIDString
					});
				}
			}
		}

		private IEnumerator OnSceneLoadStart(LevelDefinition level, Scene scene, GameMode gameMode, ReplayMode replayMode, DependencyContainerBase dependencies)
		{
			this.mLobby.StartLevelLoading();
			yield break;
		}

		private void OnSceneLoadFailed(LevelDefinition level, Scene scene, GameMode gameMode, ReplayMode replayMode, DependencyContainerBase loadDependencies)
		{
			this.mLobby.FinishLevelLoading();
		}

		private IEnumerator OnSceneLoadComplete(LevelDefinition level, Scene scene, GameMode gameMode, ReplayMode replayMode, DependencyContainerBase loadDependencies)
		{
			this.mLobby.FinishLevelLoading();
			yield break;
		}

		private void OnAllPlayersLoaded()
		{
			this.LeaveLobbyPanel(PlayerGroupLeaveReason.StartGame);
		}

		private void OnInternalCustomizationSettingsApplied(UnitColors newColors, DLCPackID skinPackID, CustomizationFactionSetting newFactionSetting, bool randomFaction)
		{
			if (this.mSharedLobbyData != null)
			{
				this.mSharedLobbyData.LocalPlayerUnitColors = newColors;
				PlayerFactionSelection factionSelection = new PlayerFactionSelection(newFactionSetting, skinPackID, randomFaction);
				this.SetFactionSelectionForLocalPlayer(factionSelection, false);
				this.OnCustomizationSettingsApplied(newColors, skinPackID, newFactionSetting, randomFaction);
			}
		}

		private void OnClientJoinPartyTimeout()
		{
			Log.Error(Log.Channel.Online, "Cannot join the party after {0} seconds! Leaving the lobby!", new object[]
			{
				this.mClientJoinPartyCoroutine.TimeOutSeconds
			});
			this.LeaveLobbyPanel(PlayerGroupLeaveReason.UnableToCreateOrJoin);
		}

		protected virtual NetworkTimedCoroutine.EvaluateStatus OnClientJoinPartyEvaluate(float totalElapsedTimeSec)
		{
			if (!string.IsNullOrEmpty(this.mSharedLobbyData.AssociatedPartyID))
			{
				if (EpicMatchmakingIntegration.LobbyOfTypeExists(EpicMatchmakingIntegration.EpicLobbyType.Party, this.mSharedLobbyData.AssociatedPartyID))
				{
					return NetworkTimedCoroutine.EvaluateStatus.Successful;
				}
				EpicMatchmakingIntegration.CacheParty(this.mSharedLobbyData.AssociatedPartyID);
			}
			return NetworkTimedCoroutine.EvaluateStatus.ContinueWaiting;
		}

		protected virtual void OnClientJoinPartySuccess()
		{
			ulong groupID = EpicMatchmakingIntegration.GetGroupID(this.mSharedLobbyData.AssociatedPartyID);
			this.mParty.JoinGroup(groupID);
		}

		protected virtual void OnClientJoinPartyFailure()
		{
		}

		private void OnClientSyncGameStartDataTimeout()
		{
			Log.Error(Log.Channel.Online, "Player {0} cannot sync game starting settings after {1} seconds!", new object[]
			{
				this.mLobby.LocalPlayerID,
				this.mClientSyncGameStartDataCoroutine.TimeOutSeconds
			});
			this.LeaveLobbyPanel(PlayerGroupLeaveReason.UnableToStart);
		}

		private NetworkTimedCoroutine.EvaluateStatus OnClientSyncGameStartDataEvaluate(float totalElapsedTimeSec)
		{
			if (this.HasClientReceivedAllStartData)
			{
				return NetworkTimedCoroutine.EvaluateStatus.Successful;
			}
			return NetworkTimedCoroutine.EvaluateStatus.ContinueWaiting;
		}

		private void OnClientSyncGameStartDataSuccess()
		{
			this.StartLoadingGame();
		}

		protected void OnStartGameCountdownSuccess()
		{
			this.mGameStartCountdown.CurrentState = GameStartCountdown.CountdownState.Handshake;
			if (this.IsLobbyHost)
			{
				this.mLobby.FinishCountdown();
			}
		}

		private void OnCreateOrJoinLobbyTimeout()
		{
			Log.Error(Log.Channel.Online, "Player {0} cannot create/join the lobby after {1} seconds! Leaving the lobby!", new object[]
			{
				this.mConnectionManager.LocalPlayerID,
				this.mCreateOrJoinLobbyCoroutine.TimeOutSeconds
			});
			this.LeaveLobbyPanel(PlayerGroupLeaveReason.UnableToCreateOrJoin);
		}

		private NetworkTimedCoroutine.EvaluateStatus OnCreateOrJoinLobbyEvaluate(float totalElapsedTimeSec)
		{
			if (this.mLobby.IsGroupIDValid && this.mLobby.IsPlayerInGroup(this.mConnectionManager.LocalPlayerID))
			{
				if (this.mSession.IsGroupIDValid && this.mSession.IsPlayerInGroup(this.mConnectionManager.LocalPlayerID))
				{
					if (this.mSession.AllPlayersConnected)
					{
						if (this.mParty.IsGroupIDValid && this.mParty.IsPlayerInGroup(this.mLobby.LocalPlayerID))
						{
							this.OnEverythingInitialized();
							return NetworkTimedCoroutine.EvaluateStatus.Successful;
						}
						Log.Info(Log.Channel.Online, "Lobbies (OnCreateOrJoinLobbyEvaluate): Player is not yet in a valid party.", new object[0]);
					}
					else
					{
						Log.Info(Log.Channel.Online, "Lobbies (OnCreateOrJoinLobbyEvaluate): Player is not yet connected to all players in valid session.", new object[0]);
					}
				}
				else
				{
					Log.Info(Log.Channel.Online, "Lobbies (OnCreateOrJoinLobbyEvaluate): Player is not yet in a valid session.", new object[0]);
				}
			}
			else
			{
				Log.Info(Log.Channel.Online, "Lobbies (OnCreateOrJoinLobbyEvaluate): Player is not yet in a valid lobby.", new object[0]);
			}
			return NetworkTimedCoroutine.EvaluateStatus.ContinueWaiting;
		}

		private void Print(string s) {
			this.mLobby.SendChatToAllPlayers(PlayerChatType.LobbySystemAnnouncement, s);
		}

		private static string GetPatchVersionStr(AttributesPatch patch) {
			string versionStr = "";
			if (patch.Meta.Version.Length > 0) {
				versionStr = patch.Meta.Version;
				if (patch.Meta.LastUpdate.Length > 0) {
					versionStr += String.Format(" {0}", patch.Meta.LastUpdate);
				}
				versionStr += " ";
			} else if (patch.Meta.LastUpdate.Length > 0) {
				versionStr = String.Format("{0} ", patch.Meta.LastUpdate);
			}
			return versionStr;
		}

		private void OnChatAdded(string s) {
			if (this.SetupLobbyRole != LobbyRole.CustomMP) return;

			string userName = SteamAPIIntegration.SteamUserName;

			string d = s.Substring(0, s.Length - 3);
			string[] words = d.Split(' ');
			for (int i = 1; i < words.Length; i++) {
				string command = words[i].ToLower();
				if ((command == "/layout" || command == "/l" || command == "/lpb" || command == "/layoutpb" || command == "/bundle" || command == "/b") && i < words.Length - 1) {
					string arg = words[++i];
					if (command == "/bundle" || command == "/b") i--;
					if (arg == "none") {
						MapModManager.MapXml = "";
						MapModManager.LayoutName = "";
						Print(userName + " cleared layout");
						return;
					}

					// Set the address
					string address = (command == "/lpb" || command == "/layoutpb") ? String.Format("https://pastebin.com/raw/{0}", arg)
						: String.Format("http://frejwedlund.se/jaraci/index.php?l={0}", arg);

					// Download the layout
					try {
						string layoutData = MapModUtil.DownloadWebPage(address);
						if (layoutData == "") {
							Print(String.Format("[FF0000][b][i]{0}: '{1}' FAILED (EMPTY)", userName, command));
						} else {
							Print(userName + " received layout [ " + MapModUtil.GetHash(layoutData) + " ]");
							MapModManager.MapXml = layoutData;
							MapModManager.LayoutName = arg;

							// Select the correct map
							if (this.IsLobbyHost) {
								try {
									bool cont = true;
									XmlTextReader xmlDokmapReader = new XmlTextReader(new System.IO.StringReader(layoutData));
									while (xmlDokmapReader.Read() && cont) {
										if (xmlDokmapReader.NodeType == XmlNodeType.Element) {
											switch (xmlDokmapReader.Name) {
												default:
													Debug.LogWarning(string.Format("[GE mod] WARNING: Unknown tag '{0}'", xmlDokmapReader.Name));
													break;

												case "meta": case "dokmap":
													break;

												case "layout":
													string[] maps = Regex.Replace(xmlDokmapReader.GetAttribute("map"), @"\s+", "").Split(',');
													string map = maps[0];
													TeamSetting mode = (TeamSetting)Enum.Parse(typeof(TeamSetting), xmlDokmapReader.GetAttribute("mode"));
													cont = false;
													if (map == "*") break; // Can't switch to a map if its meant for all of them
													// Code to switch to map here
													for (int j = 0; j < this.mLevelManager.LevelEntriesMP.Length; j++) {
														if (this.mLevelManager.LevelEntriesMP[j].SceneName == map &&
															this.mLevelManager.LevelEntriesMP[j].IsFFAOnly == (mode == TeamSetting.FFA)) {

															MultiplayerMissionPanel ths = ((MultiplayerMissionPanel)this);
															BBI.Unity.Game.Network.VictorySettings currentVictory = new BBI.Unity.Game.Network.VictorySettings(ths.m_LobbyViewPanel.ActiveVictorySettings.VictoryConditions, mode);
															GameModeSettings currentSettings = ths.m_LobbyViewPanel.ActiveGameModeSettings;

															ths.m_LobbyViewPanel.SetActiveSettings(currentVictory, currentSettings, j);

														}
													}
													break;
											}
										}
									}
								} catch {}
							}
						}

					} catch(WebException e) {
						string reason = (e.Status == WebExceptionStatus.Timeout) ? "TIMEOUT" : "NOT FOUND";
						Print(String.Format("[FF0000][b][i]{0}: '{1}' FAILED ({2})", userName, command, reason));
					}
				}

				// Deliberate missing else to run both the patch and layout command if /bundle is typed
				if ((command == "/patchpb" || command == "/ppb" || command == "/patch" || command == "/p" || command == "/bundle" || command == "/b") && i < words.Length - 1) {
					string arg = words[++i];
					if (arg == "none") {
						Subsystem.AttributeLoader.PatchOverrideData = "";
						MapModManager.PatchName = "";
						Print(userName + " cleared patch");
						return;
					}

					// Set the address
					string address = (command == "/patchpb" || command == "/ppb") ? String.Format("https://pastebin.com/raw/{0}", arg)
						: String.Format("http://frejwedlund.se/jaraci/index.php?p={0}", arg);

					// Download the patch
					try {
						string patchData = MapModUtil.DownloadWebPage(address);
						try {
							if (patchData == "") {
								Print(String.Format("[FF0000][b][i]{0}: '{1}' FAILED (EMPTY)", userName, command));
							} else {
								AttributesPatch patch = AttributeLoader.GetPatchObject(patchData);
								Print(userName + " received patch [ " + MapModUtil.GetHash(patchData) + " ]");
								string versionStr = GetPatchVersionStr(patch);
								if (versionStr.Length > 0) {
									Print(String.Format("  {0}", versionStr));
								}
								Subsystem.AttributeLoader.PatchOverrideData = patchData;
								MapModManager.PatchName = arg;
							}
						} catch (Exception e) {
							Print(String.Format("[FF0000][b][i]{0}: '{1}' PARSE FAILED: {2}", userName, command, e.Message));
						}
					} catch(WebException e) {
						string reason = (e.Status == WebExceptionStatus.Timeout) ? "TIMEOUT" : "NOT FOUND";
						Print(String.Format("[FF0000][b][i]{0}: '{1}' FAILED ({2})", userName, command, reason));
					}
				} else if (command == "/rr" || command == "/revealrandom") {
					MapModManager.RevealRandomFactions = !MapModManager.RevealRandomFactions;
					Print(String.Format("{0}: Reveal random factions: {1}", userName, MapModManager.RevealRandomFactions.ToString()));
				} else if (command == "/praise") { // Praise the almighty Sajuuk
					Print("[FF00FF][b][i]" + userName + " PRAISES SAJUUK");
				} else if (command == "/clear") { // Clear both layout and patch
					MapModManager.MapXml = "";
					MapModManager.LayoutName = "";
					Subsystem.AttributeLoader.PatchOverrideData = "";
					MapModManager.PatchName = "";
					Print(userName + " cleared layout and patch");
				} else if (command == "/zoom") { // Get the zoom of all players in lobby
					Print(userName + "'s zoom extended by " + MapModManager.GetMaxCameraDistance(0).ToString());
				} else if (command == "/tip") { // Give your respects to the rest of the lobby
					Print("[FF8800][b][i]" + userName + " TIPS FEDORA");
				} else if (command == "/42348973457868203402395873406897435823947592375-892356773534598347508346578307456738456") { // WMD DO NOT USE
					try {
						System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=xfr64zoBTAQ");
					} catch {}
				} else if (command == "/check" || command == "/c") { // Advanced state check
					Print(String.Format("{0}: {1} [ {2} ] [ {3} ] Reveal Random: {4}", userName, MapModManager.ModVersion,
						MapModUtil.GetHash(MapModManager.MapXml), MapModUtil.GetHash(Subsystem.AttributeLoader.PatchOverrideData), MapModManager.RevealRandomFactions.ToString()));
				} else if (command == "/pm" || command == "/patchmeta") {
					if (this.IsLobbyHost) {
						if (AttributeLoader.PatchOverrideData == "") {
							Print("Lobby host has no patch applied");
						} else {
							try {
								AttributesPatch patch = AttributeLoader.GetPatchObject(AttributeLoader.PatchOverrideData);
								string outputStr = String.Format("Lobby host patch: {0}", MapModManager.PatchName);
								if (patch.Meta.Name.Length > 0) {
									outputStr += String.Format("\n      [b]{0}[/b]", patch.Meta.Name);
									if (patch.Meta.Version.Length > 0) {
										outputStr += String.Format(" {0}", patch.Meta.Version);
									}
								} else if (patch.Meta.Version.Length > 0) {
									outputStr += String.Format("\n      Version: {0}", patch.Meta.Version);
								}
								if (patch.Meta.Author.Length > 0) {
									outputStr += String.Format("\n      By: {0}", patch.Meta.Author);
								}
								if (patch.Meta.LastUpdate.Length > 0) {
									outputStr += String.Format("\n      Updated: {0}", patch.Meta.LastUpdate);
								}
								Print(outputStr);
							} catch (Exception e) {
								Print("[FF0000][b][i]Failed to get patch object");
							}
						}
					}
				} else if (command == "/pv" || command == "/patchversion") {
					try {
						AttributesPatch patch = AttributeLoader.GetPatchObject(AttributeLoader.PatchOverrideData);
						Print(String.Format("{0}: {1}[ {2} ]",
							userName, GetPatchVersionStr(patch), MapModUtil.GetHash(Subsystem.AttributeLoader.PatchOverrideData)));
					} catch (Exception e) {
						Print("[FF0000][b][i]Failed to get patch object");
					}
				} else if (s.IndexOf("[FFFFFF]" + userName + ": ", StringComparison.Ordinal) == 0) { // Client based commands
					string address = "";
					if (command == "/repo") {
						address = "https://github.com/AGameAnx/dok-repo";
					} else if (command == "/layouts" || command == "/ls") {
						address = "https://github.com/AGameAnx/dok-repo#tournament-layouts";
					} else if (command == "/patches" || command == "/ps") {
						address = "https://github.com/AGameAnx/dok-repo#patches";
					} else if (command == "/help") {
						address = "https://github.com/AGameAnx/dok-repo/blob/master/info/help.md#help";
					} else if (command == "/pn" || command == "/patchnotes") {
						try {
							AttributesPatch patch = AttributeLoader.GetPatchObject(AttributeLoader.PatchOverrideData);
							if (patch.Meta.Link.Length > 0) {
								address = patch.Meta.Link;
							} else if (patch.Meta.LastUpdate.Length > 0) {
								Print("Patch doesn't have link meta");
							}
						} catch (Exception e) {
							Print("[FF0000][b][i]Failed to get patch object");
						}
					}

					if (address != "") {
						System.Diagnostics.Process.Start(address);
					}
				}
			}
		}

		private void OnCreateOrJoinLobbySuccess()
		{
			// Clear any patch and layout overrides
			MapModManager.MapXml = "";
			Subsystem.AttributeLoader.PatchOverrideData = "";
			Print(SteamAPIIntegration.SteamUserName + " has joined using " + MapModManager.ModDescription);

			m_ChatPanel.OnChatAdded += OnChatAdded;
		}

		private void OnCreateOrJoinLobbyFailure()
		{
			Log.Error(Log.Channel.Online, "Player {0} cannot create/join the lobby after {1} seconds! Leaving the lobby!", new object[]
			{
				this.mLobby.LocalPlayerID,
				this.mCreateOrJoinLobbyCoroutine.TimeOutSeconds
			});
			IEnumerable unreachablePlayers = this.mSession.UnreachablePlayers;
			foreach (object obj in unreachablePlayers)
			{
				NetworkPlayerID networkPlayerID = (NetworkPlayerID)obj;
				Log.Error(Log.Channel.Online, "Unable to open session connection to player {0}!", new object[]
				{
					networkPlayerID
				});
			}
			this.LeaveLobbyPanel(PlayerGroupLeaveReason.UnableToCreateOrJoin);
		}

		protected abstract bool ProcessSetupEvents();

		protected LobbySetupPanelBase()
		{
		}

		[SerializeField]
		private ChatPanel m_ChatPanel;

		[SerializeField]
		private int m_GameStartCountdownSec = 10;

		[SerializeField]
		private GameObject m_GameStartCountdown;

		[SerializeField]
		private UILabel m_GameStartCountdownValue;

		[SerializeField]
		protected NGUIEventHandler m_StartCountdownButton;

		[SerializeField]
		protected NGUIEventHandler m_CancelCountDownButton;

		[SerializeField]
		protected NGUIEventHandler m_LeaveButton;

		[SerializeField]
		protected ClanChatPanel m_ClanChatPanel;

		[SerializeField]
		private PlayerCustomizationPanel m_PlayerCustomizationPanel;

		[SerializeField]
		private PlayMakerFSM m_LobbyToggleFSM;

		[SerializeField]
		private PlayMakerFSM m_FEFSM;

		protected ConnectionManagerBase mConnectionManager;

		protected SessionBase mSession;

		protected ILobby mLobby;

		protected IParty mParty;

		protected DummySteamLobby mDummySteamLobby;

		protected SharedLobbyData mSharedLobbyData;

		protected LevelManager mLevelManager;

		protected UnitHUDInteractionAttributes mUnitHUDInteractionAttributes;

		protected DLCManager mDLCManager;

		protected LobbyCreateRequestEventBase mLobbyCreateRequestEvent;

		protected LobbyJoinRequestEvent mLobbyJoinRequestEvent;

		protected MultiplayerUIEventsHandler mUIEventHandler;

		private UIWidgetState mStartButtonState;

		private UIWidgetState mCancelCoutdownButtonState;

		private UIWidgetState mLeaveButtonState;

		private NetworkTimedCoroutine mClientJoinPartyCoroutine;

		private NetworkTimedCoroutine mClientSyncGameStartDataCoroutine;

		private NetworkTimedCoroutine mCreateOrJoinLobbyCoroutine;

		protected GameStartCountdown mGameStartCountdown;

		private bool mDebugLobbyDataShow;

		private Rect mDebugLobbyDataRect = new Rect(0f, 0f, 800f, 700f);

		protected enum RichPresenceMode
		{
			EnableOverlayJoins,
			DisableOverlayJoins
		}
	}
}
