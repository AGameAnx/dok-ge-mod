using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using BBI.Core;
using BBI.Core.Localization;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Game.Simulation;
using BBI.Unity.Core.DLC;
using BBI.Unity.Core.Utility;
using BBI.Unity.Core.World;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.DLC;
using BBI.Unity.Game.Network;
using BBI.Unity.Game.UI.Frontend.Helpers;
using BBI.Unity.Game.Utility;
using BBI.Unity.Game.World;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	public class GameLobbyView : BlackbirdPanelBase
	{
		public bool IsInitialized { get; private set; }

		public UIWidgetState MapState
		{
			set
			{
				UIHelper.SetUIWidgetState(this.m_MapPopupList, ref this.mMapState, value);
			}
		}

		public int ActiveMapIndex
		{
			get
			{
				if (this.m_MapPopupList == null || this.m_MapPopupList.data == null || this.m_MapPopupList.items.Count == 0 || string.IsNullOrEmpty(this.m_MapPopupList.value) || this.m_MapPopupList.value == "<NO MAP>")
				{
					return -1;
				}
				return (int)this.m_MapPopupList.data;
			}
			private set
			{
				LevelDefinition levelDefinition = this.mLevelManager.FindLevelByIndex(value, GameMode.Multiplayer);
				if (levelDefinition != null)
				{
					this.m_MapPopupList.value = levelDefinition.NameLocId;
				}
			}
		}

		public LevelDefinition SelectedMap
		{
			get
			{
				return this.mLevelManager.FindLevelByIndex(this.ActiveMapIndex, GameMode.Multiplayer);
			}
		}

		public string SelectedMapSceneName
		{
			get
			{
				if (this.m_MapPopupList == null || this.m_MapPopupList.data == null || this.m_MapPopupList.items.Count == 0 || string.IsNullOrEmpty(this.m_MapPopupList.value) || this.m_MapPopupList.value == "<NO MAP>")
				{
					return "<NO MAP>";
				}
				return this.SelectedMap.SceneName;
			}
		}

		public UIWidgetState AddAIState
		{
			set
			{
				UIHelper.SetUIWidgetState(this.m_AddAIButton, ref this.mAddAIState, value);
			}
		}

		public UIWidgetState VictoryConditionsState
		{
			set
			{
				if (value != this.mVictoryConditionsState)
				{
					bool enable = value == UIWidgetState.Enabled;
					foreach (GameLobbyView.VictoryConditionButtonContainer victoryConditionButtonContainer in this.m_VictoryConditionToggles)
					{
						if ((victoryConditionButtonContainer.VictoryConditionToSet & VictoryConditions.Annihilation) != (VictoryConditions)0)
						{
							victoryConditionButtonContainer.Enable = false;
						}
						else
						{
							victoryConditionButtonContainer.Enable = enable;
						}
					}
					this.mVictoryConditionsState = value;
				}
			}
		}

		public UIWidgetState RetrievalWinCountSliderState
		{
			set
			{
				UIHelper.SetUIWidgetState(this.m_ArtifactRetrievalWinCountSlider, ref this.mRetrievalWinConditionState, value);
			}
		}

		public UIWidgetState RetrievalToggleState
		{
			set
			{
				UIHelper.SetUIWidgetState(this.m_RetrievalToggle, ref this.mRetrievalToggleState, value);
			}
		}

		public UIWidgetState InvitePlayersState
		{
			get
			{
				return this.mInvitePlayersState;
			}
			set
			{
				UIHelper.SetUIWidgetState(this.m_InviteFriendButton, ref this.mInvitePlayersState, value);
			}
		}

		public bool LocalPlayerAllowedToChangeSettings
		{
			set
			{
				foreach (KeyValuePair<CommanderID, PlayerSlotController> keyValuePair in this.mPlayerSlotLookup)
				{
					if (keyValuePair.Value.PlayerID.IsLocalPlayer())
					{
						keyValuePair.Value.LocalPlayerAllowedToChangeSettings = value;
						break;
					}
				}
			}
		}

		private VictoryConditions ActiveVictoryConditions
		{
			get
			{
				VictoryConditions victoryConditions = VictoryConditions.Annihilation;
				if (this.m_VictoryConditionToggles != null)
				{
					foreach (GameLobbyView.VictoryConditionButtonContainer victoryConditionButtonContainer in this.m_VictoryConditionToggles)
					{
						if (victoryConditionButtonContainer.Toggle != null && victoryConditionButtonContainer.Toggle.value)
						{
							victoryConditions |= victoryConditionButtonContainer.VictoryConditionToSet;
						}
					}
				}
				return victoryConditions;
			}
			set
			{
				if (this.m_VictoryConditionToggles != null)
				{
					foreach (GameLobbyView.VictoryConditionButtonContainer victoryConditionButtonContainer in this.m_VictoryConditionToggles)
					{
						victoryConditionButtonContainer.Toggle.value = ((value & victoryConditionButtonContainer.VictoryConditionToSet) != (VictoryConditions)0);
					}
				}
			}
		}

		public VictorySettings ActiveVictorySettings
		{
			get
			{
				return new VictorySettings(this.ActiveVictoryConditions, this.ActiveTeamSetting);
			}
		}

		private bool IsShowingSettingDisabledOverlay
		{
			set
			{
				if (this.m_SettingDisabledOverlay != null)
				{
					NGUITools.SetActive(this.m_SettingDisabledOverlay, value);
				}
			}
		}

		private int RetrievalWinCount
		{
			get
			{
				this.mRetrievalWinCount = 1;
				if (this.m_ArtifactRetrievalWinCountSlider != null && this.m_ArtifactRetrievalWinCountSlider.numberOfSteps > 1)
				{
					this.mRetrievalWinCount = Mathf.RoundToInt(this.m_ArtifactRetrievalWinCountSlider.value * (float)(this.m_ArtifactRetrievalWinCountSlider.numberOfSteps - 1)) + 1;
				}
				return this.mRetrievalWinCount;
			}
			set
			{
				if (this.m_ArtifactRetrievalWinCountSlider.numberOfSteps > 1)
				{
					this.mRetrievalWinCount = value;
					this.m_ArtifactRetrievalWinCountSlider.value = ((float)this.mRetrievalWinCount - 1f) / ((float)this.m_ArtifactRetrievalWinCountSlider.numberOfSteps - 1f);
				}
				else
				{
					this.mRetrievalWinCount = 1;
					this.m_ArtifactRetrievalWinCountSlider.value = (float)this.mRetrievalWinCount;
				}
				if (this.m_ArtifactRetrievalWinCountLabel != null)
				{
					this.m_ArtifactRetrievalWinCountLabel.text = this.mRetrievalWinCount.ToString(CultureInfo.InvariantCulture);
				}
			}
		}

		private RetrievalSettings ActiveRetrievalSettings
		{
			get
			{
				return new RetrievalSettings(this.mWinCountEnabled, this.RetrievalWinCount, this.mArtifactExtractionTimeSeconds, this.mArtifactInitialSpawnTimeSeconds, this.mArtifactRespawnTimeSeconds, this.mArtifactExpirationTimeSeconds, ShipbreakersMain.GlobalSettingsAttributes.GameSettings.Retrieval.DefaultSettings.ArtifactAboutToExpireThresholdSeconds, ShipbreakersMain.GlobalSettingsAttributes.GameSettings.Retrieval.DefaultSettings.EndOfGameDelayInSeconds, ShipbreakersMain.GlobalSettingsAttributes.GameSettings.Retrieval.DefaultSettings.ArtifactHolderAlwaysVisible);
			}
			set
			{
				this.mWinCountEnabled = value.WinCountEnabled;
				this.RetrievalWinCount = value.WinCount;
				this.mArtifactExtractionTimeSeconds = value.ArtifactExtractionTimeSeconds;
				this.mArtifactInitialSpawnTimeSeconds = value.ArtifactInitialSpawnTimeSeconds;
				this.mArtifactRespawnTimeSeconds = value.ArtifactRespawnTimeSeconds;
				this.mArtifactExpirationTimeSeconds = value.ArtifactExpirationTimeSeconds;
			}
		}

		private CarrierAnnihilationSettings ActiveCarrierAnnihilationSettings
		{
			get
			{
				return ShipbreakersMain.GlobalSettingsAttributes.GameSettings.CarrierAnnihilation.DefaultSettings;
			}
		}

		private AnnihilationSettings ActiveAnnihilationSettings
		{
			get
			{
				return ShipbreakersMain.GlobalSettingsAttributes.GameSettings.Annihilation.DefaultSettings;
			}
		}

		public GameModeSettings ActiveGameModeSettings
		{
			get
			{
				return new GameModeSettings(this.mMatchTimeLimitEnabled, this.mMatchTimeLimitSeconds, this.ActiveRetrievalSettings, this.ActiveCarrierAnnihilationSettings, this.ActiveAnnihilationSettings);
			}
			private set
			{
				this.mMatchTimeLimitEnabled = value.MatchTimeLimitEnabled;
				this.mMatchTimeLimitSeconds = value.MatchTimeLimitSeconds;
				this.ActiveRetrievalSettings = value.Retrieval;
				this.ValidateGameModeSettings();
			}
		}

		public DictionaryExtensions.KeyIterator<CommanderID, PlayerSlotController> ActiveCommanderSlots
		{
			get
			{
				return this.mPlayerSlotLookup.Keys<CommanderID, PlayerSlotController>();
			}
		}

		public int ActiveCommanderSlotCount
		{
			get
			{
				return this.mPlayerSlotLookup.Count;
			}
		}

		public TeamSetting ActiveTeamSetting
		{
			get
			{
				TeamSetting result = TeamSetting.Invalid;
				if (this.m_TeamSettingTogglGroup == null || !this.m_TeamSettingTogglGroup.TryParseChecked<TeamSetting>(ref result))
				{
					Log.Error(Log.Channel.UI, "JZ - Failed to parse the current team setting", new object[0]);
				}
				return result;
			}
			private set
			{
				if (this.m_TeamSettingTogglGroup != null)
				{
					TeamSetting activeTeamSetting = this.ActiveTeamSetting;
					if (activeTeamSetting != value)
					{
						this.m_TeamSettingTogglGroup.SetChecked<TeamSetting>(value);
						this.RefreshMapList();
						return;
					}
				}
				else
				{
					Log.Error(Log.Channel.UI, "JZ - Failed to set the current team setting", new object[0]);
				}
			}
		}

		public bool ShowTooManyPlayersWarning
		{
			set
			{
				if (this.m_TooManyPlayersDisplay != null)
				{
					NGUITools.SetActive(this.m_TooManyPlayersDisplay, value);
				}
			}
		}

		public bool ShowTeamsNotAssignedWarning
		{
			set
			{
				if (this.m_TeamMisassignedDisplay != null)
				{
					NGUITools.SetActive(this.m_TeamMisassignedDisplay, value);
				}
			}
		}

		public bool ShowTeamBalanceWarning
		{
			set
			{
				if (this.m_TeamUnbalancedDisplay != null)
				{
					NGUITools.SetActive(this.m_TeamUnbalancedDisplay, value);
				}
			}
		}

		public bool ShowTeamMismatchedWarning
		{
			set
			{
				if (this.m_TeamMismatchedDisplay != null)
				{
					NGUITools.SetActive(this.m_TeamMismatchedDisplay, value);
				}
			}
		}

		public bool ShowNotReadyWarning
		{
			set
			{
				if (this.m_NotReadyWarning != null)
				{
					NGUITools.SetActive(this.m_NotReadyWarning, value);
				}
			}
		}

		public bool ShowNotEnoughPlayerWarning
		{
			set
			{
				if (this.m_NotEnoughPlayerWarning != null)
				{
					NGUITools.SetActive(this.m_NotEnoughPlayerWarning, value);
				}
			}
		}

		public event Action<int> MapChanged;

		public event Action AIAdded;

		public event Action<VictorySettings> VictorySettingsChanged;

		public event Action<GameModeSettings> GameModeSettingsChanged;

		public event Action PlayerInvited;

		public event Action<NetworkPlayerID> InvitationCanceled;

		public event Action<CommanderID> PlayerRemoved;

		public event Action<CommanderID, TeamID> TeamChanged;

		public event Action<CommanderID, PlayerFactionSelection> FactionChanged;

		public event Action<CommanderID, Difficulty> AIDifficultyChanged;

		public event Action<EmptySlotController> EmptySlotControlClicked;

		protected override void OnInitialized()
		{
			if (!base.GlobalDependencyContainer.Get<LevelManager>(out this.mLevelManager))
			{
				Log.Error(Log.Channel.UI, "A NULL Level Manager was passed into {0}. It will not function!", new object[]
				{
					base.GetType()
				});
			}
			if (!base.GlobalDependencyContainer.Get<DLCManager>(out this.mDLCManager))
			{
				Log.Error(Log.Channel.UI, "A NULL DLCManager was passed into {0}. It will not function!", new object[]
				{
					base.GetType()
				});
			}
			this.mMapChangedDelegate = new EventDelegate(new EventDelegate.Callback(this.OnMapChanged));
			NGUITools.SetActive(this.m_AllowCheatsToggle.gameObject, false);
			this.m_AllowCheatsToggle.value = false;
			this.mEmptySlots = new List<EmptySlotController>(this.m_MaxPlayerSlots);
			this.InitVictorySettingToggles();
			this.m_ArtifactRetrievalWinCountSlider.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnRetrievalWinCountChanged)));
			UISlider artifactRetrievalWinCountSlider = this.m_ArtifactRetrievalWinCountSlider;
			artifactRetrievalWinCountSlider.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(artifactRetrievalWinCountSlider.onDragFinished, new UIProgressBar.OnDragFinished(this.OnRetrievalWinCountDragFinished));
			if (!base.GlobalDependencyContainer.Get<UnitHUDInteractionAttributes>(out this.mUnitHUDInteractionAttributes))
			{
				Log.Error(Log.Channel.UI, "[JZ] No UnitHUDInteractionAttributes supplied in global dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			this.m_InviteFriendButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnInvitePlayerClicked)));
			this.m_AddAIButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnAddAIClicked)));
			this.ActiveGameModeSettings = ShipbreakersMain.GlobalSettingsAttributes.GameSettings.DefaultSettings;
			this.ActiveTeamSetting = VictorySettings.kDefaultSettings.TeamSetting;
			this.ActiveVictoryConditions = VictorySettings.kDefaultSettings.VictoryConditions;
		}

		private void OnEnable()
		{
			if (this.mLocalizationManager != null)
			{
				this.mLocalizationManager.LanguageChanged += this.OnLanguageChanged;
				this.UpdateVictoryConditionDescription();
			}
			if (this.m_PlayerSlotGrid != null)
			{
				this.m_PlayerSlotGrid.Reposition();
			}
		}

		private void OnDisable()
		{
			if (this.mLocalizationManager != null)
			{
				this.mLocalizationManager.LanguageChanged -= this.OnLanguageChanged;
			}
			this.UnloadPreviewImageStreamedAssets();
		}

		public void Initialize(ILobby lobby, MultiplayerSetupMode setupMode)
		{
			Log.Info(Log.Channel.Online, "Initializing lobby view...", new object[0]);
			this.mLobby = lobby;
			this.mSetupMode = MultiplayerSetupMode.NotSet;
			this.RefreshMapList();
			this.m_MapPopupList.onChange.Add(this.mMapChangedDelegate);
			if (this.m_ChatPanel != null)
			{
				this.m_ChatPanel.Initialize(lobby);
			}
			this.ActiveGameModeSettings = this.ActiveGameModeSettings;
			this.SetHostOptions(setupMode);
			this.ClearPlayerSlots();
			this.ShowTeamsNotAssignedWarning = false;
			this.ShowTeamBalanceWarning = false;
			this.ShowTeamMismatchedWarning = false;
			this.ShowNotEnoughPlayerWarning = false;
			this.ShowNotReadyWarning = true;
			this.IsInitialized = true;
		}

		public void Shutdown()
		{
			Log.Info(Log.Channel.Online, "Shutting down lobby view...", new object[0]);
			this.IsInitialized = false;
			this.m_MapPopupList.onChange.Remove(this.mMapChangedDelegate);
			if (this.m_ChatPanel != null)
			{
				this.m_ChatPanel.Shutdown();
			}
			this.mLobby = null;
		}

		public void SetHostOptions(MultiplayerSetupMode setupMode)
		{
			this.mMapState = UIWidgetState.NotSet;
			this.MapState = (MultiplayerSetupModeUtils.EnableSetupOptions(setupMode) ? UIWidgetState.Enabled : UIWidgetState.Disabled);
			this.mAddAIState = UIWidgetState.NotSet;
			this.AddAIState = (MultiplayerSetupModeUtils.EnableSetupOptions(setupMode) ? UIWidgetState.Enabled : UIWidgetState.Hidden);
			this.mVictoryConditionsState = UIWidgetState.NotSet;
			this.VictoryConditionsState = (MultiplayerSetupModeUtils.EnableSetupOptions(setupMode) ? UIWidgetState.Enabled : UIWidgetState.Disabled);
			if (this.ActiveTeamSetting == TeamSetting.FFA)
			{
				this.mRetrievalToggleState = UIWidgetState.NotSet;
				this.RetrievalToggleState = UIWidgetState.Disabled;
			}
			if (this.m_TeamSettingTogglGroup != null)
			{
				if (MultiplayerSetupModeUtils.EnableSetupOptions(setupMode))
				{
					this.m_TeamSettingTogglGroup.EnableAll();
				}
				else
				{
					this.m_TeamSettingTogglGroup.DisableAll();
				}
			}
			this.mInvitePlayersState = UIWidgetState.NotSet;
			this.InvitePlayersState = (MultiplayerSetupModeUtils.EnableMultiplayerSetupOptions(setupMode) ? UIWidgetState.Enabled : UIWidgetState.Hidden);
			if (this.mSetupMode != setupMode)
			{
				this.mSetupMode = setupMode;
				foreach (KeyValuePair<CommanderID, PlayerSlotController> keyValuePair in this.mPlayerSlotLookup)
				{
					keyValuePair.Value.SetHostOptions(this.mSetupMode);
				}
				UIWidgetState slotControlButtonState = MultiplayerSetupModeUtils.EnableSetupOptions(setupMode) ? UIWidgetState.Enabled : UIWidgetState.Hidden;
				foreach (EmptySlotController emptySlotController in this.mEmptySlots)
				{
					emptySlotController.SlotControlButtonState = slotControlButtonState;
				}
			}
			this.IsShowingSettingDisabledOverlay = !MultiplayerSetupModeUtils.EnableMultiplayerSetupOptions(setupMode);
			this.UpdateRetrivalWinCountSliderState();
		}

		public void SetActiveSettings(VictorySettings victorySettings, GameModeSettings gameModeSettings, int mapIndex)
		{
			this.ActiveGameModeSettings = gameModeSettings;
			this.ActiveVictoryConditions = victorySettings.VictoryConditions;
			this.ActiveTeamSetting = victorySettings.TeamSetting;
			if (mapIndex != -1)
			{
				this.ActiveMapIndex = mapIndex;
			}
		}

		public void AddCommander(CommanderDescription commanderDescription, Dictionary<string, PlayerFactionSelection> factionOptions)
		{
			PlayerSlotController playerSlotController = null;
			if (commanderDescription.PlayerType == PlayerType.Human && this.mInvitationSlotLookup.TryGetValue(commanderDescription.NetworkPlayerID, out playerSlotController))
			{
				playerSlotController.HideInvitationSlot();
				this.mInvitationSlotLookup.Remove(commanderDescription.NetworkPlayerID);
			}
			if (playerSlotController == null)
			{
				playerSlotController = this.CreatePlayerSlot();
			}
			this.SetPlayerSlot(playerSlotController, commanderDescription, factionOptions);
			EmptySlotController emptySlotController = this.FindFirstEmptySlot(EmptySlotController.SlotState.Open);
			if (emptySlotController != null)
			{
				NGUITools.SetActive(emptySlotController.gameObject, false);
				NGUITools.Destroy(emptySlotController.gameObject);
				this.mEmptySlots.Remove(emptySlotController);
			}
			else
			{
				Log.Error(Log.Channel.UI, "JZ - Failed to find a open slot!", new object[0]);
			}
			this.mPlayerSlotLookup.Add(commanderDescription.CommanderID, playerSlotController);
			this.m_PlayerSlotGrid.Reposition();
		}

		public void ResetAIPlayerSlotFactionOptions()
		{
			foreach (KeyValuePair<CommanderID, PlayerSlotController> keyValuePair in this.mPlayerSlotLookup)
			{
				PlayerSlotController value = keyValuePair.Value;
				if (value.IsAIPlayer)
				{
					Dictionary<string, PlayerFactionSelection> factionOptions = GameLobbyView.GetFactionOptions(this.mLevelManager, this.mDLCManager, value.PlayerType, true);
					PlayerFactionSelection playerFactionSelection;
					value.GetFactionSelection(out playerFactionSelection);
					value.PopulateFactionOptions(factionOptions);
					CustomizationFactionSetting faction = playerFactionSelection.Faction;
					DLCPackID dlcpackID = playerFactionSelection.DLCPackID;
					if (dlcpackID != DLCPackID.kInvalidID && !this.mDLCManager.IsPackOwned(dlcpackID))
					{
						dlcpackID = DLCPackID.kInvalidID;
						PlayerFactionSelection playerFactionSelection2;
						if (playerFactionSelection.RandomFaction && GameLobbyView.RollForRandomFaction(factionOptions, out playerFactionSelection2))
						{
							faction = playerFactionSelection2.Faction;
							dlcpackID = playerFactionSelection2.DLCPackID;
						}
						value.SetFactionSelection(faction, dlcpackID, playerFactionSelection.RandomFaction);
					}
				}
			}
		}

		public bool AddLobbyInvitationSlot(NetworkPlayerID playerID)
		{
			if (this.IsPlayerInLobby(playerID))
			{
				Log.Error(Log.Channel.Online, "[JZ] You cannot invite a player already in lobby", new object[0]);
				return false;
			}
			if (this.mInvitationSlotLookup.ContainsKey(playerID))
			{
				return false;
			}
			GameObject gameObject = NGUITools.AddChild(this.m_PlayerSlotGrid.gameObject, this.m_PlayerSlotControllerPrefab.gameObject);
			gameObject.name = "Y" + this.mInvitationSlotLookup.Count;
			PlayerSlotController component = gameObject.GetComponent<PlayerSlotController>();
			component.ShowPendingSlot(playerID.GetDisplayName());
			component.InviteCanceled += this.OnInvitationCanceled;
			this.mInvitationSlotLookup.Add(playerID, component);
			if (this.mEmptySlots.Count > 0)
			{
				EmptySlotController emptySlotController = this.mEmptySlots[0];
				NGUITools.SetActive(emptySlotController.gameObject, false);
				NGUITools.Destroy(emptySlotController.gameObject);
				this.mEmptySlots.RemoveAt(0);
			}
			this.m_PlayerSlotGrid.Reposition();
			return true;
		}

		public void RemoveLastLobbyInviteSlot()
		{
			if (this.mInvitationSlotLookup.Count == 0)
			{
				return;
			}
			NetworkPlayerID playerID = NetworkPlayerID.kInvalidID;
			string text = "";
			foreach (KeyValuePair<NetworkPlayerID, PlayerSlotController> keyValuePair in this.mInvitationSlotLookup)
			{
				if (text.CompareTo(keyValuePair.Value.name) <= 0)
				{
					text = keyValuePair.Value.name;
					playerID = keyValuePair.Key;
				}
			}
			this.CancelInvitation(playerID);
		}

		public void UpdateCommander(CommanderDescription commanderDescription)
		{
			PlayerSlotController playerSlotController;
			if (this.mPlayerSlotLookup.TryGetValue(commanderDescription.CommanderID, out playerSlotController))
			{
				playerSlotController.SetCommanderDetails(commanderDescription);
			}
		}

		public void UpdatePlayerPingSprite(CommanderID commanderID, int pingMS)
		{
			PlayerSlotController playerSlotController;
			if (this.mPlayerSlotLookup.TryGetValue(commanderID, out playerSlotController))
			{
				playerSlotController.PingMS = pingMS;
			}
		}

		public void UpdateLocalCommander(NetworkPlayerID playerID, int factionIndex, DLCPackID dlcPackID, bool randomFaction, TeamID teamID, UnitColors unitColors)
		{
			foreach (KeyValuePair<CommanderID, PlayerSlotController> keyValuePair in this.mPlayerSlotLookup)
			{
				PlayerSlotController value = keyValuePair.Value;
				if (value.PlayerID == playerID)
				{
					value.TeamSelection = teamID;
					if (dlcPackID != DLCPackID.kInvalidID && !this.mDLCManager.IsPackOwned(dlcPackID))
					{
						Log.Error(Log.Channel.UI, "Local player attempted to set faction to unowned DLCPackID {0}, reseting to invalid!", new object[]
						{
							dlcPackID
						});
						dlcPackID = DLCPackID.kInvalidID;
					}
					value.SetFactionSelection((CustomizationFactionSetting)factionIndex, dlcPackID, randomFaction);
					value.PlayerBanner.PrimaryColor = NGUIMath.IntToColor(unitColors.Base);
					value.PlayerBanner.TrimColor = NGUIMath.IntToColor(unitColors.Trim);
					break;
				}
			}
		}

		public void RemoveCommander(CommanderID commanderID)
		{
			this.RemovePlayerSlot(commanderID);
		}

		public Dictionary<CommanderID, TeamID> GetPlayerTeamIDs()
		{
			Dictionary<CommanderID, TeamID> dictionary = new Dictionary<CommanderID, TeamID>(6);
			if (this.ActiveTeamSetting == TeamSetting.FFA)
			{
				using (Dictionary<CommanderID, PlayerSlotController>.Enumerator enumerator = this.mPlayerSlotLookup.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<CommanderID, PlayerSlotController> keyValuePair = enumerator.Current;
						dictionary[keyValuePair.Key] = TeamID.None;
					}
					return dictionary;
				}
			}
			foreach (KeyValuePair<CommanderID, PlayerSlotController> keyValuePair2 in this.mPlayerSlotLookup)
			{
				dictionary[keyValuePair2.Key] = keyValuePair2.Value.TeamSelection;
			}
			return dictionary;
		}

		public void UpdateEmptySlotState(int closedSlotsCount)
		{
			foreach (EmptySlotController emptySlotController in this.mEmptySlots)
			{
				if (closedSlotsCount > 0)
				{
					emptySlotController.CurrentSlotState = EmptySlotController.SlotState.Close;
					emptySlotController.name = "Z";
					closedSlotsCount--;
				}
				else
				{
					emptySlotController.CurrentSlotState = EmptySlotController.SlotState.Open;
					emptySlotController.name = "Y";
				}
			}
			if (closedSlotsCount != 0)
			{
				Log.Error(Log.Channel.UI, "JZ - More closing slots ({0}) than availabe empty slots ({1})!", new object[]
				{
					closedSlotsCount,
					this.mEmptySlots.Count
				});
			}
			this.m_PlayerSlotGrid.Reposition();
		}

		public void UnloadPreviewImageStreamedAssets()
		{
			if (this.mPreviewImageAssetContainer != null)
			{
				this.mPreviewImageAssetContainer.Unload();
				if (this.mPreviewImageAssetContainer.IsValid())
				{
					AssetBundleManager.Instance.UnloadBundle(this.mPreviewImageAssetContainer.AssetBundleName, true);
				}
				this.mPreviewImageAssetContainer = null;
			}
		}

		private void OnTeamSelectionChanged(PlayerSlotController controller, TeamID newTeam)
		{
			if (this.mUnitHUDInteractionAttributes != null)
			{
				UnitHUDInteractionAttributes.PlayerUnitColours[] playerUnitColors = this.mUnitHUDInteractionAttributes.PlayerUnitColors;
				if (playerUnitColors != null)
				{
					if (newTeam.ID >= 0 && newTeam.ID < playerUnitColors.Length)
					{
						controller.TeamColor = playerUnitColors[newTeam.ID].BaseColour;
					}
					else
					{
						Log.Error(Log.Channel.UI, "[JZ] newTeam index {0} out of range of the color array (size {1}). ", new object[]
						{
							newTeam,
							playerUnitColors.Length
						});
					}
				}
				else
				{
					Log.Error(Log.Channel.UI, "[JZ] PlayerUnitColours not found in UnitHUDInteractionAttributes for {0}", new object[]
					{
						base.GetType()
					});
				}
			}
			this.OnTeamChanged(controller.CommanderID, newTeam);
		}

		private void OnFactionChanged(PlayerSlotController controller, PlayerFactionSelection factionSelection)
		{
			if (this.FactionChanged != null)
			{
				this.FactionChanged(controller.CommanderID, factionSelection);
			}
		}

		private void OnAIDifficultyChanged(PlayerSlotController controller, Difficulty aiDifficulty)
		{
			if (this.AIDifficultyChanged != null)
			{
				this.AIDifficultyChanged(controller.CommanderID, aiDifficulty);
			}
		}

		private void OnMapChanged()
		{
			if (this.m_MapPopupList.data != null && this.MapChanged != null)
			{
				this.MapChanged((int)this.m_MapPopupList.data);
			}
			this.UpdateMapInfo();
		}

		private void OnVictoryConditionsChanged()
		{
			this.OnVictorySettingsChanged();
		}

		private void OnTeamSettingToggleChanged(ToggleGroup toggleGroup)
		{
			TeamSetting activeTeamSetting = this.ActiveTeamSetting;
			if (activeTeamSetting == TeamSetting.FFA)
			{
				this.SetTeamSelections(GameLobbyView.FFAMode.Enabled);
				this.RetrievalToggleState = UIWidgetState.Disabled;
				if (this.m_RetrievalToggle.value)
				{
					this.m_RetrievalToggle.value = false;
				}
			}
			else
			{
				this.SetTeamSelections(GameLobbyView.FFAMode.Disabled);
				if (MultiplayerSetupModeUtils.EnableSetupOptions(this.mSetupMode))
				{
					this.RetrievalToggleState = UIWidgetState.Enabled;
					if (this.m_RetrievalToggle.value)
					{
						this.RetrievalToggleState = UIWidgetState.Enabled;
					}
				}
			}
			this.RefreshMapList();
			this.OnVictorySettingsChanged();
		}

		private void OnRetrievalWinCountChanged()
		{
			if (this.m_ArtifactRetrievalWinCountLabel != null)
			{
				this.m_ArtifactRetrievalWinCountLabel.text = this.RetrievalWinCount.ToString(CultureInfo.InvariantCulture);
			}
		}

		private void OnRetrievalWinCountDragFinished()
		{
			this.OnGameModeSettingsChanged();
		}

		private void OnRemoveClicked(PlayerSlotController controller)
		{
			if (this.PlayerRemoved != null)
			{
				this.PlayerRemoved(controller.CommanderID);
			}
		}

		private void ClearPlayerSlots()
		{
			if (!this.m_PlayerSlotGrid || !this.m_PlayerSlotGrid.transform)
			{
				return;
			}
			BlackbirdPanelBase.ClearGrid(this.m_PlayerSlotGrid);
			this.mEmptySlots.Clear();
			if (this.m_EmptySlotPrefab != null)
			{
				for (int i = 0; i < this.m_MaxPlayerSlots; i++)
				{
					this.AddEmptyPlayerSlot();
				}
			}
			this.mPlayerSlotLookup.Clear();
			this.mInvitationSlotLookup.Clear();
			this.m_PlayerSlotGrid.Reposition();
		}

		private void OnAddAIClicked()
		{
			if (this.AIAdded != null)
			{
				this.AIAdded();
			}
		}

		private void OnInvitePlayerClicked()
		{
			if (this.PlayerInvited != null)
			{
				this.PlayerInvited();
			}
		}

		private void OnInvitationCanceled(PlayerSlotController controller)
		{
			NetworkPlayerID networkPlayerID = NetworkPlayerID.kInvalidID;
			foreach (KeyValuePair<NetworkPlayerID, PlayerSlotController> keyValuePair in this.mInvitationSlotLookup)
			{
				if (keyValuePair.Value == controller)
				{
					networkPlayerID = keyValuePair.Key;
					break;
				}
			}
			if (networkPlayerID != NetworkPlayerID.kInvalidID)
			{
				this.CancelInvitation(networkPlayerID);
				return;
			}
			Log.Error(Log.Channel.Online, "[JZ] Responded to an unchanged PlayerSlotController", new object[0]);
		}

		private void CancelInvitation(NetworkPlayerID playerID)
		{
			PlayerSlotController playerSlotController = this.mInvitationSlotLookup[playerID];
			this.mInvitationSlotLookup.Remove(playerID);
			NGUITools.SetActive(playerSlotController.gameObject, false);
			NGUITools.Destroy(playerSlotController.gameObject);
			this.AddEmptyPlayerSlot();
			this.m_PlayerSlotGrid.Reposition();
			if (this.InvitationCanceled != null)
			{
				this.InvitationCanceled(playerID);
			}
		}

		private PlayerSlotController CreatePlayerSlot()
		{
			GameObject gameObject = NGUITools.AddChild(this.m_PlayerSlotGrid.gameObject, this.m_PlayerSlotControllerPrefab.gameObject);
			return gameObject.GetComponent<PlayerSlotController>();
		}

		private void SetPlayerSlot(PlayerSlotController controller, CommanderDescription commanderDescription, Dictionary<string, PlayerFactionSelection> factionOptions)
		{
			controller.Initialize(this.mSetupMode, factionOptions, commanderDescription, this.mLocalizationManager);
			if (this.ActiveTeamSetting == TeamSetting.FFA)
			{
				controller.TeamState = UIWidgetState.Hidden;
			}
			controller.FactionChanged += this.OnFactionChanged;
			controller.TeamChanged += this.OnTeamSelectionChanged;
			controller.DifficultyChanged += this.OnAIDifficultyChanged;
			controller.Removed += this.OnRemoveClicked;
		}

		private void RemovePlayerSlot(CommanderID commanderID)
		{
			PlayerSlotController playerSlotController;
			if (this.mPlayerSlotLookup.TryGetValue(commanderID, out playerSlotController))
			{
				this.mPlayerSlotLookup.Remove(commanderID);
				NGUITools.SetActive(playerSlotController.gameObject, false);
				NGUITools.Destroy(playerSlotController.gameObject);
				this.AddEmptyPlayerSlot();
				this.m_PlayerSlotGrid.Reposition();
			}
		}

		private void AddEmptyPlayerSlot()
		{
			if (this.m_PlayerSlotGrid != null && this.m_EmptySlotPrefab != null)
			{
				GameObject gameObject = NGUITools.AddChild(this.m_PlayerSlotGrid.gameObject, this.m_EmptySlotPrefab.gameObject);
				EmptySlotController component = gameObject.GetComponent<EmptySlotController>();
				if (MultiplayerSetupModeUtils.EnableMultiplayerSetupOptions(this.mSetupMode))
				{
					component.CurrentSlotState = EmptySlotController.SlotState.Open;
					component.SlotControlButtonState = UIWidgetState.Enabled;
				}
				else if (this.mSetupMode == MultiplayerSetupMode.Client)
				{
					component.CurrentSlotState = EmptySlotController.SlotState.Open;
					component.SlotControlButtonState = UIWidgetState.Disabled;
				}
				else
				{
					component.SlotControlButtonState = UIWidgetState.Hidden;
				}
				component.SlotControlClicked += this.OnEmptySlotControlClicked;
				component.name = "Y";
				this.mEmptySlots.Add(component);
				return;
			}
			Log.Error(Log.Channel.UI, "Unable to add an empty slot. Check for NULL grid and empty slot prefab", new object[0]);
		}

		private void OnTeamChanged(CommanderID commanderID, TeamID newTeam)
		{
			if (this.TeamChanged != null)
			{
				this.TeamChanged(commanderID, newTeam);
			}
		}

		private void UpdateMapInfo()
		{
			if (this.SelectedMap == null)
			{
				this.m_MapDescription.text = string.Empty;
				this.m_MapTexture.mainTexture = null;
				this.m_MapTexture.enabled = false;
				return;
			}
			this.m_MapDescription.text = this.mLevelManager.GetLocalizedMapDescription(this.SelectedMap);
			if (this.mPreviewImageAssetContainer != this.SelectedMap.SplashArtAsset)
			{
				this.UnloadPreviewImageStreamedAssets();
				this.m_MapTexture.enabled = false;
				this.mPreviewImageAssetContainer = this.SelectedMap.SplashArtAsset;
				this.mPreviewImageAssetContainer.Load(false).OnDone(new Action<UnityEngine.Object>(this.OnSplashArtLoaded));
			}
		}

		private bool IsPlayerInLobby(NetworkPlayerID playerID)
		{
			foreach (KeyValuePair<CommanderID, PlayerSlotController> keyValuePair in this.mPlayerSlotLookup)
			{
				if (keyValuePair.Value.PlayerID == playerID)
				{
					return true;
				}
			}
			return false;
		}

		private void SetVictoryConditionToggle(VictoryConditions singleCondition, bool value)
		{
			if (!this.m_VictoryConditionToggles.IsNullOrEmpty<GameLobbyView.VictoryConditionButtonContainer>())
			{
				foreach (GameLobbyView.VictoryConditionButtonContainer victoryConditionButtonContainer in this.m_VictoryConditionToggles)
				{
					if (victoryConditionButtonContainer.VictoryConditionToSet == singleCondition)
					{
						if (victoryConditionButtonContainer.Toggle != null)
						{
							victoryConditionButtonContainer.Toggle.value = value;
							return;
						}
						Log.Error(Log.Channel.UI, "[JZ] Failed to update toggle {0} in {1}, since no toggle button attached.", new object[]
						{
							singleCondition.ToString(),
							base.GetType()
						});
					}
				}
			}
		}

		private void SetTeamSelections(GameLobbyView.FFAMode mode)
		{
			foreach (KeyValuePair<CommanderID, PlayerSlotController> keyValuePair in this.mPlayerSlotLookup)
			{
				UIWidgetState teamState;
				if (mode == GameLobbyView.FFAMode.Enabled)
				{
					teamState = UIWidgetState.Hidden;
				}
				else if (keyValuePair.Value.PlayerID.IsLocalPlayer() || (MultiplayerSetupModeUtils.EnableSetupOptions(this.mSetupMode) && keyValuePair.Value.IsAIPlayer))
				{
					teamState = UIWidgetState.Enabled;
				}
				else
				{
					teamState = UIWidgetState.Disabled;
				}
				keyValuePair.Value.TeamState = teamState;
			}
		}

		private void UpdateVictoryConditionDescription()
		{
			if (this.m_VictoryConditionDescription != null)
			{
				StringBuilder stringBuilder = new StringBuilder(500);
				foreach (GameLobbyView.VictoryConditionButtonContainer victoryConditionButtonContainer in this.m_VictoryConditionToggles)
				{
					if (victoryConditionButtonContainer.Toggle != null && victoryConditionButtonContainer.Toggle.value)
					{
						stringBuilder.AppendFormat("{0}\n", this.mLocalizationManager.GetText(victoryConditionButtonContainer.VictoryConditionDescription));
					}
				}
				if (this.ActiveTeamSetting == TeamSetting.FFA)
				{
					stringBuilder.AppendFormat("{0}\n", this.mLocalizationManager.GetText(this.m_FFADescription));
				}
				this.m_VictoryConditionDescription.text = stringBuilder.ToString();
				return;
			}
			Log.Error(Log.Channel.UI, "Cannot set victory condition descriptions! Make sure both the description labal and victory condition containers are setup!", new object[0]);
		}

		private void InitVictorySettingToggles()
		{
			if (this.m_VictoryConditionToggles != null)
			{
				foreach (GameLobbyView.VictoryConditionButtonContainer victoryConditionButtonContainer in this.m_VictoryConditionToggles)
				{
					VictoryConditions victoryConditionToSet = victoryConditionButtonContainer.VictoryConditionToSet;
					if ((victoryConditionToSet & victoryConditionToSet - 1) != (VictoryConditions)0)
					{
						Log.Error(Log.Channel.UI, "Victory condition checkbox doesn't accept combined VictoryConditions. Please fix in {0}", new object[]
						{
							base.GetType()
						});
					}
					if (victoryConditionButtonContainer.Toggle != null)
					{
						victoryConditionButtonContainer.Toggle.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnVictoryConditionsChanged)));
					}
					else
					{
						Log.Error(Log.Channel.UI, "[JZ] Toggle button not set in Victory Condition Toggles in {0}", new object[]
						{
							base.GetType()
						});
					}
				}
			}
			if (this.m_TeamSettingTogglGroup != null)
			{
				this.m_TeamSettingTogglGroup.Initialize(new Action<ToggleGroup>(this.OnTeamSettingToggleChanged));
			}
			else
			{
				Log.Error(Log.Channel.UI, "No TeamSettingTogglGroup specifed in {0}", new object[]
				{
					base.GetType()
				});
			}
			this.ValidateVictorySettingToggles();
			this.UpdateVictoryConditionDescription();
		}

		private void OnVictorySettingsChanged()
		{
			this.ValidateVictorySettingToggles();
			this.UpdateRetrivalWinCountSliderState();
			this.UpdateVictoryConditionDescription();
			if (this.IsInitialized && this.VictorySettingsChanged != null)
			{
				this.VictorySettingsChanged(this.ActiveVictorySettings);
			}
		}

		private void ValidateVictorySettingToggles()
		{
			if (!this.m_VictoryConditionToggles.IsNullOrEmpty<GameLobbyView.VictoryConditionButtonContainer>() && this.ActiveTeamSetting == TeamSetting.FFA)
			{
				foreach (GameLobbyView.VictoryConditionButtonContainer victoryConditionButtonContainer in this.m_VictoryConditionToggles)
				{
					if (victoryConditionButtonContainer.VictoryConditionToSet == VictoryConditions.Retrieval && victoryConditionButtonContainer.Toggle.value)
					{
						victoryConditionButtonContainer.Toggle.value = false;
						Log.Error(Log.Channel.UI, "[JZ] Invalid victory setting. Artifact Retrieval and FFA cannot be active at the same time. Disabling retrival!", new object[0]);
					}
				}
			}
		}

		private void OnGameModeSettingsChanged()
		{
			this.ValidateGameModeSettings();
			this.UpdateRetrivalWinCountSliderState();
			if (this.IsInitialized && this.GameModeSettingsChanged != null)
			{
				this.GameModeSettingsChanged(this.ActiveGameModeSettings);
			}
		}

		private void UpdateRetrivalWinCountSliderState()
		{
			this.mRetrievalWinConditionState = UIWidgetState.NotSet;
			bool flag = (this.ActiveVictoryConditions & VictoryConditions.Retrieval) != (VictoryConditions)0;
			this.RetrievalWinCountSliderState = ((MultiplayerSetupModeUtils.EnableSetupOptions(this.mSetupMode) && flag && this.ActiveGameModeSettings.Retrieval.WinCountEnabled && this.ActiveTeamSetting != TeamSetting.FFA) ? UIWidgetState.Enabled : UIWidgetState.Disabled);
		}

		private void ValidateGameModeSettings()
		{
			if (this.mMatchTimeLimitEnabled && this.mMatchTimeLimitSeconds <= 0)
			{
				this.mMatchTimeLimitSeconds = ShipbreakersMain.GlobalSettingsAttributes.GameSettings.DefaultSettings.MatchTimeLimitSeconds;
				if (this.mMatchTimeLimitSeconds <= 0)
				{
					this.mMatchTimeLimitSeconds = 1200;
					Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Non-positive default MatchTimeLimitSeconds value {0}! Setting to {1}!", new object[]
					{
						ShipbreakersMain.GlobalSettingsAttributes.GameSettings.DefaultSettings.MatchTimeLimitSeconds,
						1200
					});
				}
			}
			if ((this.ActiveVictoryConditions & VictoryConditions.Retrieval) != (VictoryConditions)0)
			{
				if (!this.mMatchTimeLimitEnabled && !this.mWinCountEnabled)
				{
					this.mWinCountEnabled = true;
					Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Match time limit and win count cannot both be disabled! Enabling win count!", new object[0]);
				}
				if (this.RetrievalWinCount <= 0)
				{
					this.RetrievalWinCount = ShipbreakersMain.GlobalSettingsAttributes.GameSettings.Retrieval.DefaultSettings.WinCount;
					if (this.RetrievalWinCount <= 0)
					{
						this.RetrievalWinCount = RetrievalSettings.kDefaultSettings.WinCount;
						Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Non-positive default WinCount value {0}! Setting to {1}!", new object[]
						{
							ShipbreakersMain.GlobalSettingsAttributes.GameSettings.Retrieval.DefaultSettings.WinCount,
							RetrievalSettings.kDefaultSettings.WinCount
						});
					}
				}
				if (this.mArtifactExtractionTimeSeconds < 0)
				{
					this.mArtifactExtractionTimeSeconds = ShipbreakersMain.GlobalSettingsAttributes.GameSettings.Retrieval.DefaultSettings.ArtifactExtractionTimeSeconds;
					if (this.mArtifactExtractionTimeSeconds < 0)
					{
						this.mArtifactExtractionTimeSeconds = RetrievalSettings.kDefaultSettings.ArtifactExtractionTimeSeconds;
						Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Negative default ArtifactExtractionTimeSeconds value {0}! Setting to {1}!", new object[]
						{
							ShipbreakersMain.GlobalSettingsAttributes.GameSettings.Retrieval.DefaultSettings.ArtifactExtractionTimeSeconds,
							RetrievalSettings.kDefaultSettings.ArtifactExtractionTimeSeconds
						});
					}
				}
				if (this.mArtifactInitialSpawnTimeSeconds < 0)
				{
					this.mArtifactInitialSpawnTimeSeconds = ShipbreakersMain.GlobalSettingsAttributes.GameSettings.Retrieval.DefaultSettings.ArtifactInitialSpawnTimeSeconds;
					if (this.mArtifactInitialSpawnTimeSeconds < 0)
					{
						this.mArtifactInitialSpawnTimeSeconds = RetrievalSettings.kDefaultSettings.ArtifactInitialSpawnTimeSeconds;
						Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Negative default ArtifactInitialSpawnTimeSeconds value {0}! Setting to {1}!", new object[]
						{
							ShipbreakersMain.GlobalSettingsAttributes.GameSettings.Retrieval.DefaultSettings.ArtifactInitialSpawnTimeSeconds,
							RetrievalSettings.kDefaultSettings.ArtifactInitialSpawnTimeSeconds
						});
					}
				}
				if (this.mArtifactRespawnTimeSeconds < 0)
				{
					this.mArtifactRespawnTimeSeconds = ShipbreakersMain.GlobalSettingsAttributes.GameSettings.Retrieval.DefaultSettings.ArtifactRespawnTimeSeconds;
					if (this.mArtifactRespawnTimeSeconds < 0)
					{
						this.mArtifactRespawnTimeSeconds = RetrievalSettings.kDefaultSettings.ArtifactRespawnTimeSeconds;
						Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Negative default ArtifactRespawnTimeSeconds value {0}! Setting to {1}!", new object[]
						{
							ShipbreakersMain.GlobalSettingsAttributes.GameSettings.Retrieval.DefaultSettings.ArtifactRespawnTimeSeconds,
							RetrievalSettings.kDefaultSettings.ArtifactRespawnTimeSeconds
						});
					}
				}
				if (this.mArtifactExpirationTimeSeconds < 0)
				{
					this.mArtifactExpirationTimeSeconds = ShipbreakersMain.GlobalSettingsAttributes.GameSettings.Retrieval.DefaultSettings.ArtifactExpirationTimeSeconds;
					if (this.mArtifactExpirationTimeSeconds < 0)
					{
						this.mArtifactExpirationTimeSeconds = RetrievalSettings.kDefaultSettings.ArtifactExpirationTimeSeconds;
						Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Negative default ArtifactExpirationTimeSeconds value {0}! Setting to {1}!", new object[]
						{
							ShipbreakersMain.GlobalSettingsAttributes.GameSettings.Retrieval.DefaultSettings.ArtifactExpirationTimeSeconds,
							RetrievalSettings.kDefaultSettings.ArtifactExpirationTimeSeconds
						});
					}
				}
			}
		}

		private void OnEmptySlotControlClicked(EmptySlotController controller)
		{
			if (this.EmptySlotControlClicked != null)
			{
				this.EmptySlotControlClicked(controller);
			}
		}

		private EmptySlotController FindFirstEmptySlot(EmptySlotController.SlotState status)
		{
			EmptySlotController result = null;
			foreach (EmptySlotController emptySlotController in this.mEmptySlots)
			{
				if (emptySlotController.CurrentSlotState == status)
				{
					result = emptySlotController;
					break;
				}
			}
			return result;
		}

		private void OnSplashArtLoaded(UnityEngine.Object obj)
		{
			this.m_MapTexture.enabled = true;
			this.m_MapTexture.mainTexture = (obj as Texture2D);
		}

		private void RefreshMapList()
		{
			this.m_MapPopupList.Clear();
			if (this.mLevelManager.LevelEntriesMP != null)
			{
				for (int i = 0; i < this.mLevelManager.LevelEntriesMP.Length; i++)
				{
					LevelDefinition levelDefinition = this.mLevelManager.LevelEntriesMP[i];
					if (levelDefinition.IsAvailableForLoad)
					{
						if (this.ActiveTeamSetting == TeamSetting.FFA)
						{
							if (levelDefinition.IsFFAOnly)
							{
								this.m_MapPopupList.AddItem(levelDefinition.NameLocId, i);
							}
						}
						else if (!levelDefinition.IsFFAOnly)
						{
							this.m_MapPopupList.AddItem(levelDefinition.NameLocId, i);
						}
					}
				}
			}
			if (this.m_MapPopupList.items.Count > 0)
			{
				this.m_MapPopupList.value = this.m_MapPopupList.items[0];
			}
			else
			{
				this.m_MapPopupList.value = "<NO MAP>";
			}
			this.UpdateMapInfo();
		}

		private void OnLanguageChanged(Language newLanguage)
		{
			this.UpdateVictoryConditionDescription();
		}

		public static Dictionary<string, PlayerFactionSelection> GetFactionOptions(LevelManager levelManager, DLCManager dlcManager, PlayerType playerType, bool useOwnedDLCOnly)
		{
			Dictionary<string, PlayerFactionSelection> dictionary = new Dictionary<string, PlayerFactionSelection>(6);
			if (levelManager != null)
			{
				CommanderAttributes[] commanderAttributes = levelManager.CommanderAttributes;
				if (commanderAttributes != null)
				{
					for (int i = 0; i < commanderAttributes.Length; i++)
					{
						CommanderAttributes commanderAttributes2 = commanderAttributes[i];
						string key = string.IsNullOrEmpty(commanderAttributes2.NameLocID) ? commanderAttributes2.Name : commanderAttributes2.NameLocID;
						PlayerFactionSelection value = new PlayerFactionSelection((CustomizationFactionSetting)i, DLCPackID.kInvalidID, false);
						dictionary.Add(key, value);
					}
				}
			}
			if (dlcManager != null)
			{
				IEnumerable<DLCPackDescriptor> enumerable = useOwnedDLCOnly ? dlcManager.OwnedDLCPacks : dlcManager.AllDLCPacks;
				foreach (DLCPackDescriptor dlcpackDescriptor in enumerable)
				{
					if (dlcpackDescriptor.DLCPackType == DLCType.Faction)
					{
						UnitFactionDLCPack unitFactionDLCPack = (UnitFactionDLCPack)dlcManager.GetDLCPackHeader(dlcpackDescriptor.DLCPackID);
						if (!(unitFactionDLCPack == null))
						{
							if (playerType == PlayerType.AI)
							{
								CommanderAttributesData commanderAttributesData = unitFactionDLCPack.CommanderAttrs as CommanderAttributesData;
								if (commanderAttributesData == null || commanderAttributesData.AIArchetypeAssets.IsNullOrEmpty<AIArchetypeAttributesAsset>())
								{
									continue;
								}
							}
							string dlcpackLocID = dlcpackDescriptor.DLCPackLocID;
							PlayerFactionSelection value2 = new PlayerFactionSelection(unitFactionDLCPack.CustomizesFaction, dlcpackDescriptor.DLCPackID, false);
							dictionary.Add(dlcpackLocID, value2);
						}
					}
				}
			}
			PlayerFactionSelection value3 = new PlayerFactionSelection(CustomizationFactionSetting.Coalition, DLCPackID.kInvalidID, true);
			dictionary.Add("ID_UI_FE_MP_RANDOM_253", value3);
			return dictionary;
		}

		public static bool RollForRandomFaction(Dictionary<string, PlayerFactionSelection> factionOptions, out PlayerFactionSelection randomSelectedFaction)
		{
			if (factionOptions.Count > 0)
			{
				List<PlayerFactionSelection> list = new List<PlayerFactionSelection>(factionOptions.Values);
				int index = UnityEngine.Random.Range(0, list.Count);
				randomSelectedFaction = list[index];
				return true;
			}
			Log.Error(Log.Channel.UI, "No faction options provided! Unable to pick a random faction.", new object[0]);
			randomSelectedFaction = PlayerFactionSelection.kInvalid;
			return false;
		}

		public GameLobbyView()
		{
		}

		private const string kOpenEmptySlotName = "Y";

		private const string kClosedEmptySlotName = "Z";

		[SerializeField]
		private UIGrid m_PlayerSlotGrid;

		[SerializeField]
		private PlayerSlotController m_PlayerSlotControllerPrefab;

		[SerializeField]
		private EmptySlotController m_EmptySlotPrefab;

		[SerializeField]
		private UIPopupList m_MapPopupList;

		[SerializeField]
		private UIToggle m_AllowCheatsToggle;

		[SerializeField]
		private GameObject m_TooManyPlayersDisplay;

		[SerializeField]
		private GameObject m_TeamMisassignedDisplay;

		[SerializeField]
		private GameObject m_TeamUnbalancedDisplay;

		[SerializeField]
		private GameObject m_TeamMismatchedDisplay;

		[SerializeField]
		private GameObject m_NotReadyWarning;

		[SerializeField]
		private GameObject m_NotEnoughPlayerWarning;

		[SerializeField]
		private GameLobbyView.VictoryConditionButtonContainer[] m_VictoryConditionToggles;

		[SerializeField]
		private UIToggle m_RetrievalToggle;

		[SerializeField]
		private ToggleGroup m_TeamSettingTogglGroup;

		[SerializeField]
		private string m_FFADescription = string.Empty;

		[SerializeField]
		private UILabel m_VictoryConditionDescription;

		[SerializeField]
		private UILabel m_ArtifactRetrievalWinCountLabel;

		[SerializeField]
		private UISlider m_ArtifactRetrievalWinCountSlider;

		[SerializeField]
		private int m_MaxPlayerSlots = 6;

		[SerializeField]
		private ChatPanel m_ChatPanel;

		[SerializeField]
		private UILabel m_MapDescription;

		[SerializeField]
		private UITexture m_MapTexture;

		[SerializeField]
		private UIButton m_AddAIButton;

		[SerializeField]
		private UIButton m_InviteFriendButton;

		[SerializeField]
		private GameObject m_SettingDisabledOverlay;

		private List<EmptySlotController> mEmptySlots;

		private UnitHUDInteractionAttributes mUnitHUDInteractionAttributes;

		private readonly Dictionary<CommanderID, PlayerSlotController> mPlayerSlotLookup = new Dictionary<CommanderID, PlayerSlotController>(6);

		private readonly Dictionary<NetworkPlayerID, PlayerSlotController> mInvitationSlotLookup = new Dictionary<NetworkPlayerID, PlayerSlotController>(6);

		private ILobby mLobby;

		private LevelManager mLevelManager;

		private DLCManager mDLCManager;

		private EventDelegate mMapChangedDelegate;

		private MultiplayerSetupMode mSetupMode;

		private UIWidgetState mMapState;

		private UIWidgetState mAddAIState;

		private UIWidgetState mVictoryConditionsState;

		private UIWidgetState mRetrievalWinConditionState;

		private UIWidgetState mRetrievalToggleState;

		private UIWidgetState mInvitePlayersState;

		private int mRetrievalWinCount = 1;

		private bool mMatchTimeLimitEnabled;

		private int mMatchTimeLimitSeconds;

		private bool mWinCountEnabled;

		private int mArtifactExtractionTimeSeconds;

		private int mArtifactInitialSpawnTimeSeconds;

		private int mArtifactRespawnTimeSeconds;

		private int mArtifactExpirationTimeSeconds;

		private StreamableAssetContainer mPreviewImageAssetContainer;

		private enum FFAMode
		{
			Disabled,
			Enabled
		}

		[Serializable]
		public class VictoryConditionButtonContainer
		{
			public UIToggle Toggle
			{
				get
				{
					return this.m_Toggle;
				}
			}

			public string VictoryConditionDescription
			{
				get
				{
					return this.m_VictoryConditionDescription;
				}
			}

			public bool Enable
			{
				set
				{
					if (this.m_Toggle != null)
					{
						Collider component = this.m_Toggle.GetComponent<Collider>();
						if (component != null)
						{
							component.enabled = value;
						}
					}
				}
			}

			public VictoryConditions VictoryConditionToSet
			{
				get
				{
					return this.m_VictoryConditionToSet;
				}
			}

			public VictoryConditionButtonContainer()
			{
			}

			[SerializeField]
			private UIToggle m_Toggle;

			[SerializeField]
			private string m_VictoryConditionDescription = string.Empty;

			[SerializeField]
			private VictoryConditions m_VictoryConditionToSet = VictoryConditions.CarrierAnnihilation;
		}
	}
}
