using System;
using System.Collections.Generic;
using System.Globalization;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Game.Network;
using BBI.Game.Simulation;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Network;
using BBI.Unity.Game.UI.Frontend.Helpers;
using BBI.Unity.Game.Utility;
using BBI.Unity.Game.World;
using BBI.Steam; // Steam only
using GG.EpicGames;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	public class MultiplayerMissionPanel : LobbySetupPanelBase
	{
		private UIWidgetState ConvertToPublicButtonState
		{
			set
			{
				UIHelper.SetUIWidgetState(this.m_ConvertToPublicButton, ref this.mConvertToPublicButtonState, value);
			}
		}

		protected override LobbyRole SetupLobbyRole
		{
			get
			{
				return LobbyRole.CustomMP;
			}
		}

		protected override int InitialLobbySize
		{
			get
			{
				return 6;
			}
		}

		protected override int DebugWindowID
		{
			get
			{
				return 454278;
			}
		}

		protected bool AreTeamsBalanced
		{
			get
			{
				if (this.m_LobbyViewPanel == null)
				{
					return false;
				}
				LevelDefinition selectedMap = this.m_LobbyViewPanel.SelectedMap;
				if (selectedMap == null)
				{
					return false;
				}
				int num = selectedMap.MaxPlayers / 2;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				foreach (CommanderDescription commanderDescription in this.mSharedLobbyData.CommanderCollection.Descriptions)
				{
					if (commanderDescription.TeamID == TeamID.None)
					{
						num4++;
					}
					else if (commanderDescription.TeamID.ID == 1)
					{
						num2++;
					}
					else if (commanderDescription.TeamID.ID == 2)
					{
						num3++;
					}
					else
					{
						Log.Error(Log.Channel.UI, "JZ - Unspported custom game team selection {0} for player {1}", new object[]
						{
							commanderDescription.TeamID,
							commanderDescription.LocalizedName
						});
					}
				}
				return num2 <= num && num3 <= num && (num4 != 0 || (num2 != 0 && num3 != 0));
			}
		}

		protected override UIWidgetState GetDesiredStartButtonState()
		{
			UIWidgetState desiredStartButtonState = base.GetDesiredStartButtonState();
			if (desiredStartButtonState != UIWidgetState.Enabled)
			{
				return desiredStartButtonState;
			}
			if (!this.IsMapValid)
			{
				return UIWidgetState.Disabled;
			}
			if (!base.AreAllLobbyPlayersPropagated)
			{
				return UIWidgetState.Disabled;
			}
			if (!this.HasEnoughPlayers)
			{
				return UIWidgetState.Disabled;
			}
			if (this.m_LobbyViewPanel.ActiveVictorySettings.TeamSetting == TeamSetting.Team && !this.AreTeamsBalanced)
			{
				return UIWidgetState.Disabled;
			}
			return UIWidgetState.Enabled;
		}

		protected override UIWidgetState GetDesiredLeaveButtonState()
		{
			if (this.mUIEventHandler.TargetState == MultiplayerUIEventsHandler.SubState.StartingGame)
			{
				return UIWidgetState.Disabled;
			}
			return UIWidgetState.Enabled;
		}

		private bool CanMapHoldAllPlayers
		{
			get
			{
				return this.m_LobbyViewPanel.SelectedMap != null && this.mSharedLobbyData.CommanderCollection.TotalCommanderCount <= this.m_LobbyViewPanel.SelectedMap.MaxPlayers;
			}
		}

		private bool HasEnoughPlayersWithAI
		{
			get
			{
				return this.mSharedLobbyData.CommanderCollection.NetworkCommanderCount > 1 || this.mSharedLobbyData.CommanderCollection.AICommanderCount <= 0;
			}
		}

		private bool HasEnoughPlayers
		{
			get
			{
				return this.mSharedLobbyData.CommanderCollection.TotalCommanderCount > 1;
			}
		}

		private bool IsMapValid
		{
			get
			{
				if (this.m_LobbyViewPanel.SelectedMap == null)
				{
					return false;
				}
				int selectedSceneIndex = this.mSharedLobbyData.SelectedSceneIndex;
				return selectedSceneIndex != -1 && this.m_LobbyViewPanel.ActiveMapIndex == selectedSceneIndex && this.CanMapHoldAllPlayers;
			}
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			this.m_LobbyViewPanel.MapChanged += this.OnMapChanged;
			this.m_LobbyViewPanel.AIAdded += this.OnAIAdded;
			this.m_LobbyViewPanel.FactionChanged += this.OnFactionChanged;
			this.m_LobbyViewPanel.TeamChanged += this.OnTeamChanged;
			this.m_LobbyViewPanel.AIDifficultyChanged += this.OnAIDifficultyChanged;
			this.m_LobbyViewPanel.PlayerInvited += this.OnPlayerInvited;
			this.m_LobbyViewPanel.PlayerRemoved += this.OnPlayerRemoved;
			this.m_LobbyViewPanel.VictorySettingsChanged += this.OnVictorySettingsChanged;
			this.m_LobbyViewPanel.GameModeSettingsChanged += this.OnGameModeSettingsChanged;
			this.m_LobbyViewPanel.EmptySlotControlClicked += this.OnEmptySlotControlClicked;
			this.m_ConvertToPublicButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnConvertToPublicButtonClicked)));
			this.mHostLobbyFiltersCoroutine = new NetworkTimedCoroutine(0f, 5f, null, new NetworkTimedCoroutine.EvaluateHandler(this.OnHostLobbyFiltersEvaluate), new NetworkTimedCoroutine.SuccessHandler(this.OnHostLobbyFiltersSuccess), null);
		}

		private void OnEnable()
		{
			this.ProcessSetupEvents();
		}

		private void DebugDrawPlayerHeaders()
		{
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label("Name:", new GUILayoutOption[0]);
			GUILayout.Label("Player Type:", new GUILayoutOption[0]);
			GUILayout.Label("Team ID:", new GUILayoutOption[0]);
			GUILayout.Label("Faction:", new GUILayoutOption[0]);
			GUILayout.Label("Skin AppID:", new GUILayoutOption[0]);
			GUILayout.Label("Random Faction:", new GUILayoutOption[0]);
			GUILayout.Label("Commander ID:", new GUILayoutOption[0]);
			GUILayout.Label("Spawn Index:", new GUILayoutOption[0]);
			GUILayout.EndVertical();
		}

		private void DebugDrawPlayerInfo(CommanderDescription commanderDesc)
		{
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label(commanderDesc.LocalizedName, new GUILayoutOption[0]);
			GUILayout.Label(commanderDesc.PlayerType.ToString(), new GUILayoutOption[0]);
			GUILayout.Label(commanderDesc.TeamID.ToString(), new GUILayoutOption[0]);
			GUILayout.Label(commanderDesc.FactionIndex.ToString(CultureInfo.InvariantCulture), new GUILayoutOption[0]);
			GUILayout.Label(commanderDesc.SelectedUnitSkinPack.ToString(), new GUILayoutOption[0]);
			GUILayout.Label(commanderDesc.RandomFaction.ToString(), new GUILayoutOption[0]);
			GUILayout.Label(commanderDesc.CommanderID.ToString(), new GUILayoutOption[0]);
			GUILayout.Label(commanderDesc.SpawnIndex.ToString(CultureInfo.InvariantCulture), new GUILayoutOption[0]);
			GUILayout.EndVertical();
		}

		protected override void DebugDrawLobbyData(int windowID)
		{
			GUILayout.Label(string.Format("Player Count: {0}", this.mSharedLobbyData.CommanderCollection.TotalCommanderCount), new GUILayoutOption[0]);
			GUILayout.Label("Commanders", new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			this.DebugDrawPlayerHeaders();
			foreach (CommanderDescription commanderDesc in this.mSharedLobbyData.CommanderCollection.Descriptions)
			{
				this.DebugDrawPlayerInfo(commanderDesc);
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
			if (this.mLobby != null && this.mLobby.IsActive)
			{
				GUILayout.Label(string.Format("Lobby name: {0}", EpicLobbyFinder.GetLobbyName(this.mLobby.GroupID)), new GUILayoutOption[0]);
				GUILayout.Label(string.Format("Lobby role: {0}", EpicLobbyFinder.GetLobbyRole(this.mLobby.GroupID)), new GUILayoutOption[0]);
				GUILayout.Label(string.Format("Victory conditions: {0}", EpicLobbyFinder.GetCustomGameVictoryConditions(this.mLobby.GroupID)), new GUILayoutOption[0]);
				GUILayout.Label(string.Format("Team setting: {0}", EpicLobbyFinder.GetCustomGameTeamSetting(this.mLobby.GroupID)), new GUILayoutOption[0]);
				GUILayout.Label(string.Format("Scene Index: {0}", EpicLobbyFinder.GetSceneIndex(this.mLobby.GroupID)), new GUILayoutOption[0]);
				GUILayout.Label(string.Format("Closed slots: {0}", EpicLobbyFinder.GetCustomGameClosedSlotsCount(this.mLobby.GroupID)), new GUILayoutOption[0]);
			}
			else
			{
				GUILayout.Label("Not in a lobby. Lobby filters are not set", new GUILayoutOption[0]);
			}
			GUILayout.EndVertical();
			base.DebugDrawLobbyData(windowID);
		}

		private void OnMapChanged(int mapIndex)
		{
			if (base.IsLobbyHost)
			{
				this.mSharedLobbyData.SelectedSceneIndex = this.m_LobbyViewPanel.ActiveMapIndex;
			}
		}

		public void OnTransitIn()
		{
		}

		private void OnConvertToPublicButtonClicked()
		{
			this.mLobby.LobbyType = LobbyType.Public;
			this.mDummySteamLobby.LobbyType = this.mLobby.LobbyType; // Steam only
		}

		private void OnAIAdded()
		{
			int totalCommanderCount = this.mSharedLobbyData.CommanderCollection.TotalCommanderCount;
			if (totalCommanderCount < 6)
			{
				UnitColors presetUnitColors = CustomizationSettings.GetPresetUnitColors(totalCommanderCount, this.mUnitHUDInteractionAttributes);
				this.AddAIHelper(0, DLCPackID.kInvalidID, true, CommanderDescription.kDefaultTeamID, presetUnitColors, Difficulty.Medium);
			}
		}

		private void AddAIHelper(int factionIndex, DLCPackID factionPackID, bool randomFaction, TeamID teamID, UnitColors unitColors, Difficulty difficulty)
		{
			int totalCommanderCount = this.mSharedLobbyData.CommanderCollection.TotalCommanderCount;
			if (totalCommanderCount < 6)
			{
				this.mSharedLobbyData.AddAICommander(factionIndex, factionPackID, randomFaction, teamID, unitColors, difficulty);
				if (this.mSharedLobbyData.IsLobbyFull && this.mSharedLobbyData.ClosedSlotsCount > 0)
				{
					this.ChangeClosedSlotsCountByOffset(-1);
					return;
				}
				this.UpdateLobbyMemberLimit();
			}
		}

		private void OnPlayerInvited()
		{
			// EGS:
			if (EpicAPIIntegration.IsConnectedToEpicServers && EpicAPIIntegration.IsEpicOverlayEnabled)
			{
				EpicFriendsIntegration.ShowFriendsOverlay(null);
			}
			// Steam:
			if (EpicAPIIntegration.IsConnectedToEpicServers && SteamAPIIntegration.IsSteamOverlayEnabled)
			{
				SteamFriendsIntegration.ActivateGameOverlayInviteDialog(this.mDummySteamLobby.GroupID);
			}
		}

		private void OnPlayerRemoved(CommanderID commanderID)
		{
			if (base.IsLobbyHost)
			{
				if (this.mSharedLobbyData.CommanderCollection.IsAICommander(commanderID))
				{
					this.mSharedLobbyData.RemoveCommander(commanderID);
					this.m_LobbyViewPanel.RemoveCommander(commanderID);
					this.UpdateLobbyMemberLimit();
					this.UpdateLobbyJoinableStatus();
					return;
				}
				NetworkPlayerID playerID = this.mSharedLobbyData.CommanderCollection.FindNetworkPlayerID(commanderID);
				this.mLobby.KickPlayer(playerID);
			}
		}

		private void OnFactionChanged(CommanderID commanderID, PlayerFactionSelection factionSelection)
		{
			if (this.mSharedLobbyData.CommanderCollection.IsLocalCommander(commanderID))
			{
				base.SetFactionSelectionForLocalPlayer(factionSelection, true);
				return;
			}
			if (base.IsLobbyHost && this.mSharedLobbyData.CommanderCollection.IsAICommander(commanderID))
			{
				this.SetFactionSelectionForAIPlayer(commanderID, factionSelection);
			}
		}

		private void OnTeamChanged(CommanderID commanderID, TeamID newTeam)
		{
			if (this.mSharedLobbyData.CommanderCollection.IsLocalCommander(commanderID))
			{
				this.mSharedLobbyData.LocalPlayerTeamID = newTeam;
				PartyPersistentData partyPersistentData = this.mParty.PersistentData as PartyPersistentData;
				if (partyPersistentData != null)
				{
					partyPersistentData.LocalPlayerTeamID = newTeam;
					return;
				}
			}
			else if (base.IsLobbyHost && this.mSharedLobbyData.CommanderCollection.IsAICommander(commanderID))
			{
				this.mSharedLobbyData.UpdateAICommanderTeamID(commanderID, newTeam);
			}
		}

		private void OnAIDifficultyChanged(CommanderID commanderID, Difficulty aiDifficulty)
		{
			if (base.IsLobbyHost && this.mSharedLobbyData.CommanderCollection.IsAICommander(commanderID))
			{
				this.mSharedLobbyData.UpdateAICommanderDifficulty(commanderID, aiDifficulty);
			}
		}

		private void OnVictorySettingsChanged(VictorySettings newVictorySettings)
		{
			if (base.IsLobbyHost)
			{
				this.mMultiplayerLobbyData.VictorySettings = newVictorySettings;
				this.mMultiplayerLobbyData.UpdateVictorySettingsFilters();
			}
		}

		private void OnGameModeSettingsChanged(GameModeSettings newGameModeSettings)
		{
			if (base.IsLobbyHost)
			{
				this.mMultiplayerLobbyData.GameModeSettings = newGameModeSettings;
				this.mMultiplayerLobbyData.UpdateGameModeSettingsFilters();
			}
		}

		private void OnEmptySlotControlClicked(EmptySlotController emptySlot)
		{
			if (emptySlot.CurrentSlotState == EmptySlotController.SlotState.Open)
			{
				this.ChangeClosedSlotsCountByOffset(1);
				return;
			}
			if (emptySlot.CurrentSlotState == EmptySlotController.SlotState.Close)
			{
				this.ChangeClosedSlotsCountByOffset(-1);
			}
		}

		protected override void OnLobbyEnter(PlayerGroupSetupStatus status)
		{
			if (status == PlayerGroupSetupStatus.Successful)
			{
				base.OnLobbyEnter(status);
				PartyPersistentData partyPersistentData = (PartyPersistentData)this.mParty.PersistentData;
				if (partyPersistentData != null)
				{
					this.mSharedLobbyData.LocalPlayerTeamID = partyPersistentData.LocalPlayerTeamID;
				}
				MultiplayerSetupMode setupMode = base.IsLobbyHost ? MultiplayerSetupMode.Host : MultiplayerSetupMode.Client;
				this.m_LobbyViewPanel.Initialize(this.mLobby, setupMode);
				this.mMultiplayerLobbyData = new MultiplayerLobbyData();
				this.mMultiplayerLobbyData.Initialize(this.mLobby);
				if (base.IsLobbyHost)
				{
					if (partyPersistentData != null)
					{
						this.m_LobbyViewPanel.SetActiveSettings(partyPersistentData.CustomMPVictorySettings, partyPersistentData.CustomMPGameModeSettings, partyPersistentData.CustomMPSelectedSceneIndex);
						foreach (AIInfo aiinfo in partyPersistentData.CustomMPAISelections)
						{
							DLCPackID dlcpackID = aiinfo.FactionPackID;
							if (dlcpackID != DLCPackID.kInvalidID && !this.mDLCManager.IsPackOwned(dlcpackID))
							{
								Log.Error(Log.Channel.UI, "Host attempted to set AI faction to unowned DLCPackID {0}, reseting to invalid!", new object[]
								{
									dlcpackID
								});
								dlcpackID = DLCPackID.kInvalidID;
							}
							this.AddAIHelper(aiinfo.FactionIndex, dlcpackID, aiinfo.RandomFaction, aiinfo.TeamID, aiinfo.UnitColors, aiinfo.AIDifficulty);
						}
					}
					this.mSharedLobbyData.SelectedSceneIndex = this.m_LobbyViewPanel.ActiveMapIndex;
					this.mMultiplayerLobbyData.GameModeSettings = this.m_LobbyViewPanel.ActiveGameModeSettings;
					this.mMultiplayerLobbyData.VictorySettings = this.m_LobbyViewPanel.ActiveVictorySettings;
					return;
				}
			}
			else
			{
				base.OnLobbyEnter(status);
			}
		}

		protected override void OnEverythingInitialized()
		{
			base.OnEverythingInitialized();
			base.SetRichPresence(LobbySetupPanelBase.RichPresenceMode.EnableOverlayJoins);
			if (base.IsLobbyHost)
			{
				this.mHostLobbyFiltersCoroutine.Start();
			}
		}

		protected override void OnLobbyHostMigrate(NetworkPlayerID previousHostID, NetworkPlayerID newHostID)
		{
			if (this.mLobby == null)
			{
				return;
			}
			base.OnLobbyHostMigrate(previousHostID, newHostID);
			base.ShowSystemMessage("ID_UI_FE_MP_SYSMSG_NEW_HOST_231", new object[]
			{
				this.mLobby.HostPlayerID.GetDisplayName()
			});
			if (base.IsLobbyHost)
			{
				this.m_LobbyViewPanel.SetHostOptions(MultiplayerSetupMode.Host);
				this.m_LobbyViewPanel.ResetAIPlayerSlotFactionOptions();
				this.mHostLobbyFiltersCoroutine.Start();
			}
		}

		protected override void OnLobbyGlobalMetaDataUpdate(bool initialUpdate)
		{
			base.OnLobbyGlobalMetaDataUpdate(initialUpdate);
			PartyPersistentData partyPersistentData = this.mParty.PersistentData as PartyPersistentData;
			if (partyPersistentData != null)
			{
				partyPersistentData.CustomMPSelectedSceneIndex = this.mSharedLobbyData.SelectedSceneIndex;
				partyPersistentData.CustomMPGameModeSettings = this.mMultiplayerLobbyData.GameModeSettings;
				partyPersistentData.CustomMPVictorySettings = this.mMultiplayerLobbyData.VictorySettings;
				List<AIInfo> list = new List<AIInfo>(6);
				foreach (CommanderDescription commanderDescription in this.mSharedLobbyData.CommanderCollection.Descriptions)
				{
					if (commanderDescription.PlayerType == PlayerType.AI)
					{
						list.Add(new AIInfo(commanderDescription.FactionIndex, commanderDescription.SelectedUnitSkinPack, commanderDescription.RandomFaction, commanderDescription.TeamID, commanderDescription.UnitColors, commanderDescription.AIDifficulty));
					}
				}
				partyPersistentData.CustomMPAISelections = list;
			}
			if (!base.IsLobbyHost)
			{
				this.m_LobbyViewPanel.SetActiveSettings(this.mMultiplayerLobbyData.VictorySettings, this.mMultiplayerLobbyData.GameModeSettings, this.mSharedLobbyData.SelectedSceneIndex);
			}
			this.UpdateLobbyViewCommanders();
			if (initialUpdate)
			{
				this.m_LobbyViewPanel.UpdateLocalCommander(this.mLobby.LocalPlayerID, this.mSharedLobbyData.LocalPlayerFactionIndex, this.mSharedLobbyData.LocalPlayerUnitSkinPackID, this.mSharedLobbyData.LocalPlayerRandomFaction, this.mSharedLobbyData.LocalPlayerTeamID, this.mSharedLobbyData.LocalPlayerUnitColors);
			}
		}

		protected override void OnLobbyPlayerJoin(NetworkPlayerID playerID)
		{
			base.OnLobbyPlayerJoin(playerID);
			if (base.IsLobbyHost)
			{
				if (this.mSharedLobbyData.CommanderCollection.TotalCommanderCount > 6 && this.mSharedLobbyData.CommanderCollection.AICommanderCount > 0)
				{
					Log.Error(Log.Channel.Online, "Player {0} snuck in before the lobby completed its lock. Removing an AI commander to make room...", new object[]
					{
						playerID
					});
					List<CommanderID> commanderIDs = this.mSharedLobbyData.CommanderCollection.GetCommanderIDs(PlayerType.AI);
					if (commanderIDs.Count > 0)
					{
						this.OnPlayerRemoved(commanderIDs[commanderIDs.Count - 1]);
					}
				}
				this.UpdateLobbyJoinableStatus();
			}
		}

		protected override void OnLobbyPlayerLeave(NetworkPlayerID playerID, PlayerGroupLeaveReason reason)
		{
			this.UpdateLobbyJoinableStatus();
			base.OnLobbyPlayerLeave(playerID, reason);
		}

		protected override void OnLobbyCountdownStart()
		{
			base.OnLobbyCountdownStart();
			this.m_LobbyViewPanel.SetHostOptions(MultiplayerSetupMode.Disabled);
			this.m_LobbyViewPanel.LocalPlayerAllowedToChangeSettings = false;
			this.UpdateLobbyJoinableStatus();
		}

		protected override void OnLobbyCountdownFinish()
		{
			this.mUIEventHandler.TargetState = MultiplayerUIEventsHandler.SubState.StartingGame;
			base.OnLobbyCountdownFinish();
		}

		protected override void OnLobbyCountdownCancel()
		{
			this.mUIEventHandler.TargetState = MultiplayerUIEventsHandler.SubState.InLobbySetup;
			base.OnLobbyCountdownCancel();
			MultiplayerSetupMode hostOptions = base.IsLobbyHost ? MultiplayerSetupMode.Host : MultiplayerSetupMode.Client;
			this.m_LobbyViewPanel.SetHostOptions(hostOptions);
			this.m_LobbyViewPanel.LocalPlayerAllowedToChangeSettings = true;
			this.UpdateLobbyJoinableStatus();
		}

		protected override void OnLobbyKicked(ulong groupID, NetworkPlayerID targePlayerID)
		{
			if (targePlayerID.IsLocalPlayer())
			{
				this.LeaveLobbyPanel(PlayerGroupLeaveReason.Kick);
				EpicLobbyFinder.AddKickedCustomGameGroupID(groupID);
				return;
			}
			base.ShowSystemMessage("ID_UI_FE_MP_SYSMSG_KICK_PLAYER_230", new object[]
			{
				targePlayerID.GetDisplayName()
			});
		}

		protected override void OnLobbyMigrate(ulong newLobbyID)
		{
		}

		protected override void OnPartyJoinLobby(ulong lobbyID, string epicLobbyID)
		{
		}

		protected override bool ProcessSetupEvents()
		{
			if (this.mLobbyJoinRequestEvent != null)
			{
				Log.Info(Log.Channel.Online, "Processing custom MP lobby join event...", new object[0]);
				this.StopAllLobbyCoroutines();
				this.JoinLobby(this.mLobbyJoinRequestEvent.EpicLobbyID, this.mLobbyJoinRequestEvent.SteamLobbyID);
				this.mLobbyJoinRequestEvent = null;
				this.mLobbyCreateRequestEvent = null;
				return true;
			}
			if (this.mLobbyCreateRequestEvent != null)
			{
				Log.Info(Log.Channel.Online, "Processing custom MP lobby create event...", new object[0]);
				this.StopAllLobbyCoroutines();
				this.CreateLobby(this.mLobbyCreateRequestEvent.LobbyName, this.SetupLobbyRole, this.mLobbyCreateRequestEvent.LobbyType, this.InitialLobbySize);
				this.mLobbyCreateRequestEvent = null;
				return true;
			}
			return false;
		}

		protected override void OnCustomizationSettingsApplied(UnitColors newColors, DLCPackID skinPackID, CustomizationFactionSetting newFactionIndex, bool randomFaction)
		{
			this.m_LobbyViewPanel.UpdateLocalCommander(this.mLobby.LocalPlayerID, this.mSharedLobbyData.LocalPlayerFactionIndex, this.mSharedLobbyData.LocalPlayerUnitSkinPackID, this.mSharedLobbyData.LocalPlayerRandomFaction, this.mSharedLobbyData.LocalPlayerTeamID, this.mSharedLobbyData.LocalPlayerUnitColors);
		}

		protected override DependencyContainerBase GetStartDependencies()
		{
			Dictionary<CommanderID, PlayerSelection> selectedLoadouts = SpawnAssigner.GeneratePlayerSelections(this.mLevelManager.CommanderAttributes, this.mSharedLobbyData.CommanderCollection.Descriptions, this.mSharedLobbyData.CommanderSpawnDetails, this.mDLCManager);
			GameStartSettings t = new GameStartSettings(selectedLoadouts, this.m_LobbyViewPanel.ActiveVictorySettings.VictoryConditions, this.m_LobbyViewPanel.ActiveVictorySettings.TeamSetting, this.m_LobbyViewPanel.ActiveGameModeSettings, this.mSharedLobbyData.SimRandomSeed, this.mMultiplayerLobbyData.CheatsEnabled, false, AutomatchSizeType.ThreeVSThree);
			return new DependencyContainer<GameStartSettings, ILobby, SessionBase>(t, this.mLobby, this.mSession);
		}

		private void UpdateLobbyViewCommanders()
		{
			this.m_LobbyViewPanel.UpdateEmptySlotState(this.mSharedLobbyData.ClosedSlotsCount);
			List<CommanderID> list = new List<CommanderID>(this.m_LobbyViewPanel.ActiveCommanderSlotCount);
			foreach (CommanderID item in this.m_LobbyViewPanel.ActiveCommanderSlots)
			{
				list.Add(item);
			}
			foreach (CommanderDescription commanderDescription in this.mSharedLobbyData.CommanderCollection.Descriptions)
			{
				if (list.Contains(commanderDescription.CommanderID))
				{
					if (commanderDescription.PlayerType == PlayerType.AI || !commanderDescription.IsLocal(base.IsLobbyHost))
					{
						this.m_LobbyViewPanel.UpdateCommander(commanderDescription);
					}
				}
				else if (commanderDescription.PlayerType == PlayerType.AI || (this.mSharedLobbyData.HasPlayerReceivedGlobalUpdate(commanderDescription.NetworkPlayerID) && this.mSharedLobbyData.LocalPlayerSessionIDs.Contains(commanderDescription.NetworkPlayerID)))
				{
					bool useOwnedDLCOnly = commanderDescription.NetworkPlayerID.IsLocalPlayer() || (commanderDescription.PlayerType == PlayerType.AI && base.IsLobbyHost);
					Dictionary<string, PlayerFactionSelection> factionOptions = GameLobbyView.GetFactionOptions(this.mLevelManager, this.mDLCManager, commanderDescription.PlayerType, useOwnedDLCOnly);
					this.m_LobbyViewPanel.AddCommander(commanderDescription, factionOptions);
				}
				list.Remove(commanderDescription.CommanderID);
			}
			foreach (CommanderID commanderID in list)
			{
				this.m_LobbyViewPanel.RemoveCommander(commanderID);
			}
		}

		protected override void LeaveLobbyPanel(PlayerGroupLeaveReason reason)
		{
			this.m_LobbyViewPanel.Shutdown();
			if (this.mMultiplayerLobbyData != null)
			{
				this.mMultiplayerLobbyData.Shutdown();
				this.mMultiplayerLobbyData = null;
			}
			base.LeaveLobbyPanel(reason);
		}

		protected override bool TryGetTeams(out Dictionary<CommanderID, TeamID> commanderTeamIDs, out TeamSetting teamSetting)
		{
			commanderTeamIDs = this.m_LobbyViewPanel.GetPlayerTeamIDs();
			teamSetting = this.m_LobbyViewPanel.ActiveTeamSetting;
			return true;
		}

		protected override void InitiateStartGame()
		{
			base.InitiateStartGame();
			if (this.m_LobbyViewPanel != null)
			{
				this.m_LobbyViewPanel.UnloadPreviewImageStreamedAssets();
			}
			this.StartLoadingGame();
		}

		protected override bool StartLoadingGame()
		{
			if (this.mLobby.HostPlayerID != this.mParty.HostPlayerID)
			{
				Log.Error(Log.Channel.Online, "Host migration has resulted in a different host for the lobby ({0}) and party ({1}).  The new party owner will be the host in the next game setup!", new object[]
				{
					this.mLobby.HostPlayerID,
					this.mParty.HostPlayerID
				});
			}
			if (base.IsLobbyHost)
			{
				if (!this.PopulateHostStartLobbyData())
				{
					return false;
				}
			}
			else
			{
				UIHelper.HideFESpinnerOverlay();
			}
			return base.StartLoadingGame();
		}

		private bool PopulateHostStartLobbyData()
		{
			if (!base.PopulateHostSpawnPointsAssignment(this.m_LobbyViewPanel.SelectedMap))
			{
				Log.Error(Log.Channel.UI, "Failed to generate spawn points. Cannot start game!", new object[0]);
				return false;
			}
			return true;
		}

		protected override void UpdateAllLobbyCoroutines()
		{
			this.mHostLobbyFiltersCoroutine.Update();
			base.UpdateAllLobbyCoroutines();
		}

		protected override void StopAllLobbyCoroutines()
		{
			this.mHostLobbyFiltersCoroutine.Stop();
			base.StopAllLobbyCoroutines();
		}

		protected override void UpdateUIState()
		{
			if (base.IsEverythingInitialized)
			{
				base.UpdateUIState();
				if (base.IsLobbyHost)
				{
					if (this.mLobby.LobbyType == LobbyType.Private)
					{
						this.ConvertToPublicButtonState = UIWidgetState.Enabled;
					}
					else
					{
						this.ConvertToPublicButtonState = UIWidgetState.Hidden;
					}
				}
				else
				{
					this.ConvertToPublicButtonState = UIWidgetState.Hidden;
				}
				if (this.m_LobbyViewPanel.SelectedMap != null)
				{
					this.m_LobbyViewPanel.ShowTooManyPlayersWarning = !this.CanMapHoldAllPlayers;
					this.m_LobbyViewPanel.ShowTeamBalanceWarning = (!this.AreTeamsBalanced && this.m_LobbyViewPanel.ActiveVictorySettings.TeamSetting == TeamSetting.Team);
				}
				if (this.mConnectionManager != null)
				{
					foreach (CommanderDescription commanderDescription in this.mSharedLobbyData.CommanderCollection.Descriptions)
					{
						NetworkPlayerID networkPlayerID = commanderDescription.NetworkPlayerID;
						if (!networkPlayerID.IsLocalPlayer() && networkPlayerID.IsValid())
						{
							ConnectionData connectionDataForPlayerID = this.mConnectionManager.GetConnectionDataForPlayerID(networkPlayerID);
							if (connectionDataForPlayerID != null && connectionDataForPlayerID.ConnectionState == ConnectionState.Open)
							{
								this.m_LobbyViewPanel.UpdatePlayerPingSprite(commanderDescription.CommanderID, (int)connectionDataForPlayerID.RoundTripLatencyMS);
							}
						}
					}
				}
			}
		}

		private void ChangeClosedSlotsCountByOffset(int offset)
		{
			int closedSlotsCount = this.mSharedLobbyData.ClosedSlotsCount + offset;
			this.mSharedLobbyData.ClosedSlotsCount = closedSlotsCount;
			this.UpdateLobbyMemberLimit();
			this.UpdateLobbyJoinableStatus();
		}

		private void UpdateLobbyMemberLimit()
		{
			int num = this.mSharedLobbyData.CommanderCollection.AICommanderCount + this.mSharedLobbyData.ClosedSlotsCount;
			this.mLobby.MemberLimit = 6 - num;
		}

		private void UpdateHostLobbyFilters()
		{
			if (base.IsLobbyHost && this.mLobby.IsActive)
			{
				EpicLobbyFinder.SetLobbyName(this.mLobby.GroupID, this.mLobby.LobbyName);
				EpicLobbyFinder.SetLobbyRole(this.mLobby.GroupID, LobbyRole.CustomMP);
				if (this.mSharedLobbyData != null)
				{
					EpicLobbyFinder.SetCustomGameClosedSlotsCount(this.mLobby.GroupID, this.mSharedLobbyData.ClosedSlotsCount);
					EpicLobbyFinder.SetSceneIndex(this.mLobby.GroupID, this.mSharedLobbyData.SelectedSceneIndex);
				}
				this.mMultiplayerLobbyData.UpdateGameModeSettingsFilters();
				this.mMultiplayerLobbyData.UpdateVictorySettingsFilters();
			}
		}

		private void UpdateLobbyJoinableStatus()
		{
			if (base.IsLobbyHost && this.mLobby.IsActive)
			{
				if (base.IsEverythingInitialized && !this.mSharedLobbyData.IsLobbyFull && this.mGameStartCountdown.CurrentState == GameStartCountdown.CountdownState.NotActive && this.mLevelManager.ManagerState != LevelManager.State.GameLoading)
				{
					this.mLobby.Joinable = true;
					return;
				}
				this.mLobby.Joinable = false;
			}
		}

		private void SetFactionSelectionForAIPlayer(CommanderID commanderID, PlayerFactionSelection factionSelection)
		{
			int num = (int)factionSelection.Faction;
			DLCPackID dlcpackID = factionSelection.DLCPackID;
			bool randomFaction = factionSelection.RandomFaction;
			if (randomFaction)
			{
				Dictionary<string, PlayerFactionSelection> factionOptions = GameLobbyView.GetFactionOptions(this.mLevelManager, this.mDLCManager, PlayerType.AI, true);
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
			this.mSharedLobbyData.UpdateAICommanderFactionIndex(commanderID, (CustomizationFactionSetting)num, dlcpackID, randomFaction);
		}

		private NetworkTimedCoroutine.EvaluateStatus OnHostLobbyFiltersEvaluate(float totalElapsedTimeSec)
		{
			this.UpdateHostLobbyFilters();
			this.UpdateLobbyJoinableStatus();
			return NetworkTimedCoroutine.EvaluateStatus.ContinueWaiting;
		}

		private void OnHostLobbyFiltersSuccess()
		{
		}

		public MultiplayerMissionPanel()
		{
		}

		[SerializeField]
		public GameLobbyView m_LobbyViewPanel;

		[SerializeField]
		private UIButton m_ConvertToPublicButton;

		private UIWidgetState mConvertToPublicButtonState;

		private MultiplayerLobbyData mMultiplayerLobbyData;

		private NetworkTimedCoroutine mHostLobbyFiltersCoroutine;
	}
}
