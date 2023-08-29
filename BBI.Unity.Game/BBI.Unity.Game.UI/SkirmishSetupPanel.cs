using System;
using System.Collections.Generic;
using BBI.Core;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Game.Network;
using BBI.Game.Replay;
using BBI.Game.Simulation;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.DLC;
using BBI.Unity.Game.Network;
using BBI.Unity.Game.UI.Frontend.Helpers;
using BBI.Unity.Game.Utility;
using BBI.Unity.Game.World;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	public sealed class SkirmishSetupPanel : BlackbirdPanelBase
	{
		private bool AreTeamsBalanced
		{
			get
			{
				if (this.m_LobbyView == null)
				{
					return false;
				}
				LevelDefinition selectedMap = this.m_LobbyView.SelectedMap;
				if (selectedMap == null)
				{
					return false;
				}
				int num = selectedMap.MaxPlayers / 2;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				foreach (CommanderDescription commanderDescription in this.mCommanderDescriptions)
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

		private void OnEnable()
		{
			this.mCurrentCommanderIndex = CommanderID.None.ID + 1;
			this.mSession = new NullSession();
			this.mCommanderDescriptions = new List<CommanderDescription>();
			new TeamID(TeamID.None.ID + 1);
			UnitColors localPlayerUnitColors = ShipbreakersMain.UserSettings.Customization.GetLocalPlayerUnitColors(this.mUnitHUDInteractionAttributes);
			CommanderID commanderID = new CommanderID(this.mCurrentCommanderIndex++);
			PlayerCustomizationPanel.LoadPreviewContent();
			int factionIndex = ShipbreakersMain.UserSettings.Customization.FactionIndex;
			CustomizationFactionSetting customizationFactionSetting = (CustomizationFactionSetting)factionIndex;
			DLCPackID localPlayerSkinPackIDForFaction = ShipbreakersMain.UserSettings.Customization.GetLocalPlayerSkinPackIDForFaction(this.mDLCManager, customizationFactionSetting);
			bool randomFaction = ShipbreakersMain.UserSettings.Customization.RandomFaction;
			CommanderDescription commanderDescription = new CommanderDescription(this.mSession.LocalPlayerID.GetDisplayName(), commanderID, this.mSession.LocalPlayerID, CommanderSpawnInfo.kInvalidSpawnIndex, factionIndex, new TeamID(TeamID.None.ID + 1), PlayerType.Human, localPlayerUnitColors, Difficulty.None, localPlayerSkinPackIDForFaction, randomFaction);
			PlayerFactionSelection factionSelection = new PlayerFactionSelection(customizationFactionSetting, localPlayerSkinPackIDForFaction, randomFaction);
			this.UpdateFactionSelectionForCommanderDescription(ref commanderDescription, factionSelection);
			this.mCommanderDescriptions.Add(commanderDescription);
			if (this.mLevelManager != null)
			{
				this.m_LobbyView.Initialize(null, MultiplayerSetupMode.HostSkirmish);
				Dictionary<string, PlayerFactionSelection> factionOptions = GameLobbyView.GetFactionOptions(this.mLevelManager, this.mDLCManager, commanderDescription.PlayerType, true);
				this.m_LobbyView.AddCommander(commanderDescription, factionOptions);
				this.UpdateStartButtonState();
			}
			else
			{
				Log.Error(Log.Channel.UI, "Unable to initialize {0}. Level manager is NULL", new object[]
				{
					base.GetType()
				});
			}
			if (this.m_PlayerCustomizationPanel != null)
			{
				this.m_PlayerCustomizationPanel.SettingsApplied += this.OnCustomiziationSettingsApplied;
				return;
			}
			Log.Error(Log.Channel.UI, "Player Customization Panel is not set", new object[0]);
		}

		private void OnDisable()
		{
			this.m_LobbyView.Shutdown();
			if (this.m_PlayerCustomizationPanel != null)
			{
				this.m_PlayerCustomizationPanel.SettingsApplied -= this.OnCustomiziationSettingsApplied;
			}
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			if (!base.GlobalDependencyContainer.Get<LevelManager>(out this.mLevelManager))
			{
				Log.Error(Log.Channel.UI, "Unable to get level manager from global dependencies. Won't be able to select levels!", new object[0]);
			}
			if (!base.GlobalDependencyContainer.Get<UnitHUDInteractionAttributes>(out this.mUnitHUDInteractionAttributes))
			{
				Log.Error(Log.Channel.UI, "No UnitHUDInteractionAttributes supplied in global dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			if (!base.GlobalDependencyContainer.Get<DLCManager>(out this.mDLCManager))
			{
				Log.Error(Log.Channel.UI, "No DLCManager supplied in global dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			if (this.m_GameStartButton != null)
			{
				this.m_GameStartButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnStartButtonPress)));
			}
			else
			{
				Log.Error(Log.Channel.UI, "No game start button specified for SP skirmish game mode!", new object[0]);
			}
			if (this.m_LobbyView != null)
			{
				this.m_LobbyView.AIAdded += this.OnAIAdded;
				this.m_LobbyView.PlayerRemoved += this.OnPlayerRemoved;
				this.m_LobbyView.FactionChanged += this.OnFactionChanged;
				this.m_LobbyView.TeamChanged += this.OnTeamChanged;
				this.m_LobbyView.AIDifficultyChanged += this.OnAIDifficultyChanged;
				this.m_LobbyView.MapChanged += this.OnMapChanged;
				this.m_LobbyView.VictorySettingsChanged += this.OnVictorySettingsChanged;
				return;
			}
			Log.Error(Log.Channel.UI, "Lobby view is not specified. Will not be able to start the game!", new object[0]);
		}

		protected override void Update()
		{
			base.Update();
			if (this.mSession != null)
			{
				this.mSession.Update();
			}
		}

		private DependencyContainerBase GetStartDependencies()
		{
			List<CommanderSpawnInfo> commanderSpawnInfo;
			if (!SpawnAssigner.GenerateSpawnPoints(this.m_LobbyView.SelectedMap, this.m_LobbyView.GetPlayerTeamIDs(), this.m_LobbyView.ActiveTeamSetting, out commanderSpawnInfo))
			{
				Log.Error(Log.Channel.Gameplay, "Failed to initialize spawn points! This will cause failures!", new object[0]);
			}
			Dictionary<CommanderID, PlayerSelection> selectedLoadouts = SpawnAssigner.GeneratePlayerSelections(this.mLevelManager.CommanderAttributes, this.mCommanderDescriptions, commanderSpawnInfo, this.mDLCManager);
			System.Random random = new System.Random();
			GameStartSettings t = new GameStartSettings(selectedLoadouts, this.m_LobbyView.ActiveVictorySettings.VictoryConditions, this.m_LobbyView.ActiveVictorySettings.TeamSetting, this.m_LobbyView.ActiveGameModeSettings, random.Next(), true, false, AutomatchSizeType.ThreeVSThree);
			return new DependencyContainer<GameStartSettings, SessionBase>(t, this.mSession);
		}

		private void UpdateStartButtonState()
		{
			if (this.m_LobbyView != null && this.m_GameStartButton != null)
			{
				int num = (this.m_LobbyView.SelectedMap != null) ? this.m_LobbyView.SelectedMap.MaxPlayers : 0;
				bool flag = this.mCommanderDescriptions.Count > num;
				bool flag2 = !flag && this.mCommanderDescriptions.Count >= 1;
				this.m_LobbyView.ShowTooManyPlayersWarning = flag;
				this.m_LobbyView.ShowTeamBalanceWarning = !this.AreTeamsBalanced;
				flag2 = (flag2 && this.AreTeamsBalanced);
				if (this.m_GameStartButton != null)
				{
					BlackbirdPanelBase.SetButtonState(this.m_GameStartButton.gameObject, flag2);
				}
				if (this.mCommanderDescriptions.Count < 1 && this.m_GameStartButton != null)
				{
					BlackbirdPanelBase.SetButtonState(this.m_GameStartButton.gameObject, false);
				}
			}
		}

		private void UpdateFactionSelectionForCommanderDescription(ref CommanderDescription commanderDescription, PlayerFactionSelection factionSelection)
		{
			int num = (int)factionSelection.Faction;
			DLCPackID dlcpackID = factionSelection.DLCPackID;
			bool randomFaction = factionSelection.RandomFaction;
			if (randomFaction)
			{
				Dictionary<string, PlayerFactionSelection> factionOptions = GameLobbyView.GetFactionOptions(this.mLevelManager, this.mDLCManager, commanderDescription.PlayerType, true);
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
			commanderDescription.SelectedUnitSkinPack = dlcpackID;
			commanderDescription.FactionIndex = num;
			commanderDescription.RandomFaction = randomFaction;
		}

		private void OnCustomiziationSettingsApplied(UnitColors newColors, DLCPackID skinPackID, CustomizationFactionSetting newFactionSetting, bool randomFaction)
		{
			if (this.mCommanderDescriptions.IsNullOrEmpty<CommanderDescription>())
			{
				Log.Warn(Log.Channel.UI, "could not update player's commander colours", new object[0]);
				return;
			}
			for (int i = 0; i < this.mCommanderDescriptions.Count; i++)
			{
				CommanderDescription commanderDescription = this.mCommanderDescriptions[i];
				if (commanderDescription.NetworkPlayerID.IsLocalPlayer())
				{
					commanderDescription.UnitColors = newColors;
					PlayerFactionSelection factionSelection = new PlayerFactionSelection(newFactionSetting, skinPackID, randomFaction);
					this.UpdateFactionSelectionForCommanderDescription(ref commanderDescription, factionSelection);
					this.mCommanderDescriptions[i] = commanderDescription;
					this.m_LobbyView.UpdateCommander(commanderDescription);
					return;
				}
			}
		}

		private void OnMapChanged(int mapIndex)
		{
			this.UpdateStartButtonState();
		}

		private void OnAIAdded()
		{
			if (this.mCommanderDescriptions.Count >= 6)
			{
				return;
			}
			UnitColors presetUnitColors = CustomizationSettings.GetPresetUnitColors(this.mCurrentCommanderIndex, this.mUnitHUDInteractionAttributes);
			CommanderID commanderID = new CommanderID(this.mCurrentCommanderIndex++);
			CommanderDescription commanderDescription = new CommanderDescription(string.Empty, commanderID, NetworkPlayerID.kInvalidID, CommanderSpawnInfo.kInvalidSpawnIndex, 0, CommanderDescription.kDefaultTeamID, PlayerType.AI, presetUnitColors, Difficulty.Medium, DLCPackID.kInvalidID, true);
			this.mCommanderDescriptions.Add(commanderDescription);
			this.RefreshAICommanderNames();
			Dictionary<string, PlayerFactionSelection> factionOptions = GameLobbyView.GetFactionOptions(this.mLevelManager, this.mDLCManager, commanderDescription.PlayerType, true);
			this.m_LobbyView.AddCommander(commanderDescription, factionOptions);
			this.UpdateStartButtonState();
		}

		private void RefreshAICommanderNames()
		{
			int num = 1;
			for (int i = 0; i < this.mCommanderDescriptions.Count; i++)
			{
				CommanderDescription commanderDescription = this.mCommanderDescriptions[i];
				if (commanderDescription.PlayerType == PlayerType.AI)
				{
					commanderDescription.Name = num.ToString();
					this.mCommanderDescriptions[i] = commanderDescription;
					num++;
					this.m_LobbyView.UpdateCommander(commanderDescription);
				}
			}
		}

		private void OnPlayerRemoved(CommanderID commanderID)
		{
			int num = this.mCommanderDescriptions.FindIndex((CommanderDescription c) => c.CommanderID == commanderID);
			if (num >= 0)
			{
				CommanderDescription commanderDescription = this.mCommanderDescriptions[num];
				this.mCommanderDescriptions.RemoveAt(num);
				if (this.m_LobbyView != null)
				{
					this.m_LobbyView.RemoveCommander(commanderID);
				}
				this.RefreshAICommanderNames();
				this.UpdateStartButtonState();
			}
		}

		private void OnFactionChanged(CommanderID commanderID, PlayerFactionSelection factionSelection)
		{
			int num = this.mCommanderDescriptions.FindIndex((CommanderDescription c) => c.CommanderID == commanderID);
			if (num >= 0)
			{
				CommanderDescription value = this.mCommanderDescriptions[num];
				this.UpdateFactionSelectionForCommanderDescription(ref value, factionSelection);
				this.mCommanderDescriptions[num] = value;
				if (value.NetworkPlayerID.IsLocalPlayer())
				{
					if (!value.RandomFaction)
					{
						ShipbreakersMain.UserSettings.Customization.FactionIndex = value.FactionIndex;
						ShipbreakersMain.UserSettings.Customization.UnitSkinPackID[value.FactionIndex] = value.SelectedUnitSkinPack;
					}
					ShipbreakersMain.UserSettings.Customization.RandomFaction = value.RandomFaction;
				}
			}
		}

		private void OnTeamChanged(CommanderID commanderID, TeamID newTeam)
		{
			int num = this.mCommanderDescriptions.FindIndex((CommanderDescription c) => c.CommanderID == commanderID);
			if (num >= 0)
			{
				CommanderDescription value = this.mCommanderDescriptions[num];
				value.TeamID = newTeam;
				this.mCommanderDescriptions[num] = value;
				this.UpdateStartButtonState();
			}
		}

		private void OnAIDifficultyChanged(CommanderID commanderID, Difficulty aiDifficulty)
		{
			int num = this.mCommanderDescriptions.FindIndex((CommanderDescription c) => c.CommanderID == commanderID);
			if (num >= 0)
			{
				CommanderDescription value = this.mCommanderDescriptions[num];
				value.AIDifficulty = aiDifficulty;
				this.mCommanderDescriptions[num] = value;
				this.RefreshAICommanderNames();
				this.UpdateStartButtonState();
			}
		}

		private void OnVictorySettingsChanged(VictorySettings newConditions)
		{
			this.UpdateStartButtonState();
		}

		private void OnStartButtonPress()
		{
			if (this.m_LobbyView != null)
			{
				this.m_LobbyView.UnloadPreviewImageStreamedAssets();
				MapModManager.SetMap(this.m_LobbyView.SelectedMap, GameMode.AISkirmish, this.m_LobbyView.ActiveTeamSetting, this.m_LobbyView.GetPlayerTeamIDs());
				this.mLevelManager.StartLoadLevel(GameMode.AISkirmish, ReplayMode.RecordingGame, this.m_LobbyView.SelectedMap, this.GetStartDependencies());
				return;
			}
			Log.Error(Log.Channel.UI, "Cannot start game. No lobby view specified!", new object[0]);
		}

		public SkirmishSetupPanel()
		{
		}

		private const int kMinPlayers = 1;

		[SerializeField]
		private GameLobbyView m_LobbyView;

		[SerializeField]
		private UIButton m_GameStartButton;

		[SerializeField]
		private PlayerCustomizationPanel m_PlayerCustomizationPanel;

		private DLCManager mDLCManager;

		private List<CommanderDescription> mCommanderDescriptions;

		private LevelManager mLevelManager;

		private UnitHUDInteractionAttributes mUnitHUDInteractionAttributes;

		private SessionBase mSession;

		private int mCurrentCommanderIndex;
	}
}
