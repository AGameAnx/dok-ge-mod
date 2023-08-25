using System;
using System.Collections.Generic;
using System.Globalization;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Game.Network;
using BBI.Game.Simulation;
using BBI.Steam;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Network;
using BBI.Unity.Game.UI.Frontend.Helpers;
using BBI.Unity.Game.Utility;
using BBI.Unity.Game.World;
using GG.EpicGames;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	// Token: 0x020002FF RID: 767
	public class MultiplayerMissionPanel : LobbySetupPanelBase
	{
		// Token: 0x170003FF RID: 1023
		// (set) Token: 0x060017DF RID: 6111 RVA: 0x00085181 File Offset: 0x00083381
		private UIWidgetState ConvertToPublicButtonState
		{
			set
			{
				UIHelper.SetUIWidgetState(this.m_ConvertToPublicButton, ref this.mConvertToPublicButtonState, value);
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060017E0 RID: 6112 RVA: 0x00085196 File Offset: 0x00083396
		protected override LobbyRole SetupLobbyRole
		{
			get
			{
				return LobbyRole.CustomMP;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060017E1 RID: 6113 RVA: 0x00085199 File Offset: 0x00083399
		protected override int InitialLobbySize
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060017E2 RID: 6114 RVA: 0x0008519C File Offset: 0x0008339C
		protected override int DebugWindowID
		{
			get
			{
				return 454278;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060017E3 RID: 6115 RVA: 0x000851A4 File Offset: 0x000833A4
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

		// Token: 0x060017E4 RID: 6116 RVA: 0x000852C4 File Offset: 0x000834C4
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

		// Token: 0x060017E5 RID: 6117 RVA: 0x0008531A File Offset: 0x0008351A
		protected override UIWidgetState GetDesiredLeaveButtonState()
		{
			if (this.mUIEventHandler.TargetState == MultiplayerUIEventsHandler.SubState.StartingGame)
			{
				return UIWidgetState.Disabled;
			}
			return UIWidgetState.Enabled;
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060017E6 RID: 6118 RVA: 0x0008532D File Offset: 0x0008352D
		private bool CanMapHoldAllPlayers
		{
			get
			{
				return this.m_LobbyViewPanel.SelectedMap != null && this.mSharedLobbyData.CommanderCollection.TotalCommanderCount <= this.m_LobbyViewPanel.SelectedMap.MaxPlayers;
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060017E7 RID: 6119 RVA: 0x00085361 File Offset: 0x00083561
		private bool HasEnoughPlayersWithAI
		{
			get
			{
				return this.mSharedLobbyData.CommanderCollection.NetworkCommanderCount > 1 || this.mSharedLobbyData.CommanderCollection.AICommanderCount <= 0;
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060017E8 RID: 6120 RVA: 0x0008538C File Offset: 0x0008358C
		private bool HasEnoughPlayers
		{
			get
			{
				return this.mSharedLobbyData.CommanderCollection.TotalCommanderCount > 1;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060017E9 RID: 6121 RVA: 0x000853A4 File Offset: 0x000835A4
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

		// Token: 0x060017EA RID: 6122 RVA: 0x000853F0 File Offset: 0x000835F0
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

		// Token: 0x060017EB RID: 6123 RVA: 0x00085539 File Offset: 0x00083739
		private void OnEnable()
		{
			this.ProcessSetupEvents();
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x00085544 File Offset: 0x00083744
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

		// Token: 0x060017ED RID: 6125 RVA: 0x000855E4 File Offset: 0x000837E4
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

		// Token: 0x060017EE RID: 6126 RVA: 0x000856D8 File Offset: 0x000838D8
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

		// Token: 0x060017EF RID: 6127 RVA: 0x000858CC File Offset: 0x00083ACC
		private void OnMapChanged(int mapIndex)
		{
			if (base.IsLobbyHost)
			{
				this.mSharedLobbyData.SelectedSceneIndex = this.m_LobbyViewPanel.ActiveMapIndex;
			}
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x000858EC File Offset: 0x00083AEC
		public void OnTransitIn()
		{
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x000858EE File Offset: 0x00083AEE
		private void OnConvertToPublicButtonClicked()
		{
			this.mLobby.LobbyType = LobbyType.Public;
			this.mDummySteamLobby.LobbyType = this.mLobby.LobbyType;
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x00085914 File Offset: 0x00083B14
		private void OnAIAdded()
		{
			int totalCommanderCount = this.mSharedLobbyData.CommanderCollection.TotalCommanderCount;
			if (totalCommanderCount < 6)
			{
				UnitColors presetUnitColors = CustomizationSettings.GetPresetUnitColors(totalCommanderCount, this.mUnitHUDInteractionAttributes);
				this.AddAIHelper(0, DLCPackID.kInvalidID, true, CommanderDescription.kDefaultTeamID, presetUnitColors, Difficulty.Medium);
			}
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x00085958 File Offset: 0x00083B58
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

		// Token: 0x060017F4 RID: 6132 RVA: 0x000859B7 File Offset: 0x00083BB7
		private void OnPlayerInvited()
		{
			if (EpicAPIIntegration.IsConnectedToEpicServers && SteamAPIIntegration.IsSteamOverlayEnabled)
			{
				SteamFriendsIntegration.ActivateGameOverlayInviteDialog(this.mLobby.GroupID);
			}
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x000859D8 File Offset: 0x00083BD8
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

		// Token: 0x060017F6 RID: 6134 RVA: 0x00085A44 File Offset: 0x00083C44
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

		// Token: 0x060017F7 RID: 6135 RVA: 0x00085A90 File Offset: 0x00083C90
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

		// Token: 0x060017F8 RID: 6136 RVA: 0x00085B00 File Offset: 0x00083D00
		private void OnAIDifficultyChanged(CommanderID commanderID, Difficulty aiDifficulty)
		{
			if (base.IsLobbyHost && this.mSharedLobbyData.CommanderCollection.IsAICommander(commanderID))
			{
				this.mSharedLobbyData.UpdateAICommanderDifficulty(commanderID, aiDifficulty);
			}
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x00085B2A File Offset: 0x00083D2A
		private void OnVictorySettingsChanged(VictorySettings newVictorySettings)
		{
			if (base.IsLobbyHost)
			{
				this.mMultiplayerLobbyData.VictorySettings = newVictorySettings;
				this.mMultiplayerLobbyData.UpdateVictorySettingsFilters();
			}
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x00085B4C File Offset: 0x00083D4C
		private void OnGameModeSettingsChanged(GameModeSettings newGameModeSettings)
		{
			if (base.IsLobbyHost)
			{
				this.mMultiplayerLobbyData.GameModeSettings = newGameModeSettings;
				this.mMultiplayerLobbyData.UpdateGameModeSettingsFilters();
			}
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x00085B6E File Offset: 0x00083D6E
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

		// Token: 0x060017FC RID: 6140 RVA: 0x00085B90 File Offset: 0x00083D90
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

		// Token: 0x060017FD RID: 6141 RVA: 0x00085D3C File Offset: 0x00083F3C
		protected override void OnEverythingInitialized()
		{
			base.OnEverythingInitialized();
			base.SetRichPresence(LobbySetupPanelBase.RichPresenceMode.EnableOverlayJoins);
			if (base.IsLobbyHost)
			{
				this.mHostLobbyFiltersCoroutine.Start();
			}
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x00085D60 File Offset: 0x00083F60
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

		// Token: 0x060017FF RID: 6143 RVA: 0x00085DD0 File Offset: 0x00083FD0
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

		// Token: 0x06001800 RID: 6144 RVA: 0x00085F50 File Offset: 0x00084150
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

		// Token: 0x06001801 RID: 6145 RVA: 0x00085FE7 File Offset: 0x000841E7
		protected override void OnLobbyPlayerLeave(NetworkPlayerID playerID, PlayerGroupLeaveReason reason)
		{
			this.UpdateLobbyJoinableStatus();
			base.OnLobbyPlayerLeave(playerID, reason);
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x00085FF7 File Offset: 0x000841F7
		protected override void OnLobbyCountdownStart()
		{
			base.OnLobbyCountdownStart();
			this.m_LobbyViewPanel.SetHostOptions(MultiplayerSetupMode.Disabled);
			this.m_LobbyViewPanel.LocalPlayerAllowedToChangeSettings = false;
			this.UpdateLobbyJoinableStatus();
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x0008601D File Offset: 0x0008421D
		protected override void OnLobbyCountdownFinish()
		{
			this.mUIEventHandler.TargetState = MultiplayerUIEventsHandler.SubState.StartingGame;
			base.OnLobbyCountdownFinish();
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x00086034 File Offset: 0x00084234
		protected override void OnLobbyCountdownCancel()
		{
			this.mUIEventHandler.TargetState = MultiplayerUIEventsHandler.SubState.InLobbySetup;
			base.OnLobbyCountdownCancel();
			MultiplayerSetupMode hostOptions = base.IsLobbyHost ? MultiplayerSetupMode.Host : MultiplayerSetupMode.Client;
			this.m_LobbyViewPanel.SetHostOptions(hostOptions);
			this.m_LobbyViewPanel.LocalPlayerAllowedToChangeSettings = true;
			this.UpdateLobbyJoinableStatus();
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x00086080 File Offset: 0x00084280
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

		// Token: 0x06001806 RID: 6150 RVA: 0x000860C0 File Offset: 0x000842C0
		protected override void OnLobbyMigrate(ulong newLobbyID)
		{
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x000860C2 File Offset: 0x000842C2
		protected override void OnPartyJoinLobby(ulong lobbyID, string epicLobbyID)
		{
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x000860C4 File Offset: 0x000842C4
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

		// Token: 0x06001809 RID: 6153 RVA: 0x00086178 File Offset: 0x00084378
		protected override void OnCustomizationSettingsApplied(UnitColors newColors, DLCPackID skinPackID, CustomizationFactionSetting newFactionIndex, bool randomFaction)
		{
			this.m_LobbyViewPanel.UpdateLocalCommander(this.mLobby.LocalPlayerID, this.mSharedLobbyData.LocalPlayerFactionIndex, this.mSharedLobbyData.LocalPlayerUnitSkinPackID, this.mSharedLobbyData.LocalPlayerRandomFaction, this.mSharedLobbyData.LocalPlayerTeamID, this.mSharedLobbyData.LocalPlayerUnitColors);
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x000861D4 File Offset: 0x000843D4
		protected override DependencyContainerBase GetStartDependencies()
		{
			Dictionary<CommanderID, PlayerSelection> selectedLoadouts = SpawnAssigner.GeneratePlayerSelections(this.mLevelManager.CommanderAttributes, this.mSharedLobbyData.CommanderCollection.Descriptions, this.mSharedLobbyData.CommanderSpawnDetails, this.mDLCManager);
			GameStartSettings t = new GameStartSettings(selectedLoadouts, this.m_LobbyViewPanel.ActiveVictorySettings.VictoryConditions, this.m_LobbyViewPanel.ActiveVictorySettings.TeamSetting, this.m_LobbyViewPanel.ActiveGameModeSettings, this.mSharedLobbyData.SimRandomSeed, this.mMultiplayerLobbyData.CheatsEnabled, false, AutomatchSizeType.ThreeVSThree);
			return new DependencyContainer<GameStartSettings, ILobby, SessionBase>(t, this.mLobby, this.mSession);
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x00086274 File Offset: 0x00084474
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

		// Token: 0x0600180C RID: 6156 RVA: 0x00086458 File Offset: 0x00084658
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

		// Token: 0x0600180D RID: 6157 RVA: 0x00086486 File Offset: 0x00084686
		protected override bool TryGetTeams(out Dictionary<CommanderID, TeamID> commanderTeamIDs, out TeamSetting teamSetting)
		{
			commanderTeamIDs = this.m_LobbyViewPanel.GetPlayerTeamIDs();
			teamSetting = this.m_LobbyViewPanel.ActiveTeamSetting;
			return true;
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x000864A3 File Offset: 0x000846A3
		protected override void InitiateStartGame()
		{
			base.InitiateStartGame();
			this.UpdateLobbyJoinableStatus();
			if (this.m_LobbyViewPanel != null)
			{
				this.m_LobbyViewPanel.UnloadPreviewImageStreamedAssets();
			}
			this.StartLoadingGame();
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x000864D4 File Offset: 0x000846D4
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

		// Token: 0x06001810 RID: 6160 RVA: 0x00086558 File Offset: 0x00084758
		private bool PopulateHostStartLobbyData()
		{
			if (!base.PopulateHostSpawnPointsAssignment(this.m_LobbyViewPanel.SelectedMap))
			{
				Log.Error(Log.Channel.UI, "Failed to generate spawn points. Cannot start game!", new object[0]);
				return false;
			}
			return true;
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x00086585 File Offset: 0x00084785
		protected override void UpdateAllLobbyCoroutines()
		{
			this.mHostLobbyFiltersCoroutine.Update();
			base.UpdateAllLobbyCoroutines();
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x00086598 File Offset: 0x00084798
		protected override void StopAllLobbyCoroutines()
		{
			this.mHostLobbyFiltersCoroutine.Stop();
			base.StopAllLobbyCoroutines();
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x000865AC File Offset: 0x000847AC
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

		// Token: 0x06001814 RID: 6164 RVA: 0x000866E0 File Offset: 0x000848E0
		private void ChangeClosedSlotsCountByOffset(int offset)
		{
			int closedSlotsCount = this.mSharedLobbyData.ClosedSlotsCount + offset;
			this.mSharedLobbyData.ClosedSlotsCount = closedSlotsCount;
			this.UpdateLobbyMemberLimit();
			this.UpdateLobbyJoinableStatus();
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x00086714 File Offset: 0x00084914
		private void UpdateLobbyMemberLimit()
		{
			int num = this.mSharedLobbyData.CommanderCollection.AICommanderCount + this.mSharedLobbyData.ClosedSlotsCount;
			this.mLobby.MemberLimit = 6 - num;
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x0008674C File Offset: 0x0008494C
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

		// Token: 0x06001817 RID: 6167 RVA: 0x000867F8 File Offset: 0x000849F8
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

		// Token: 0x06001818 RID: 6168 RVA: 0x00086864 File Offset: 0x00084A64
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

		// Token: 0x06001819 RID: 6169 RVA: 0x00086940 File Offset: 0x00084B40
		private NetworkTimedCoroutine.EvaluateStatus OnHostLobbyFiltersEvaluate(float totalElapsedTimeSec)
		{
			this.UpdateHostLobbyFilters();
			this.UpdateLobbyJoinableStatus();
			return NetworkTimedCoroutine.EvaluateStatus.ContinueWaiting;
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x0008694F File Offset: 0x00084B4F
		private void OnHostLobbyFiltersSuccess()
		{
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x00086951 File Offset: 0x00084B51
		public MultiplayerMissionPanel()
		{
		}

		// Token: 0x04001430 RID: 5168
		[SerializeField]
		public GameLobbyView m_LobbyViewPanel;

		// Token: 0x04001431 RID: 5169
		[SerializeField]
		private UIButton m_ConvertToPublicButton;

		// Token: 0x04001432 RID: 5170
		private UIWidgetState mConvertToPublicButtonState;

		// Token: 0x04001433 RID: 5171
		private MultiplayerLobbyData mMultiplayerLobbyData;

		// Token: 0x04001434 RID: 5172
		private NetworkTimedCoroutine mHostLobbyFiltersCoroutine;
	}
}
