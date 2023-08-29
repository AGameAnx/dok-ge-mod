using System;
using System.Collections.Generic;
using System.IO;
using BBI.Core.IO;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Game.Network;
using BBI.Game.Replay;
using BBI.Game.Simulation;
using BBI.Unity.Core.DLC;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.DLC;
using BBI.Unity.Game.Events;
using BBI.Unity.Game.HUD;
using BBI.Unity.Game.Utility;
using BBI.Unity.Game.World;
using UnityEngine;
using Subsystem;
using System.Text;

namespace BBI.Unity.Game.UI
{
	public sealed class ReplayLoadController : BlackbirdModalPanelBase
	{
		protected override void OnInitialized()
		{
			base.OnInitialized();
			this.mLoadEvent = new EventDelegate(new EventDelegate.Callback(this.OnLoadClicked));
			this.mDeleteEvent = new EventDelegate(new EventDelegate.Callback(this.OnDeleteClicked));
			if (!base.GlobalDependencyContainer.Get<LevelManager>(out this.mLevelManager))
			{
				Log.Error(Log.Channel.UI, "NULL LevelManager specified in {0}", new object[]
				{
					base.GetType()
				});
			}
			if (!base.GlobalDependencyContainer.Get<DLCManager>(out this.mDLCManager))
			{
				Log.Error(Log.Channel.UI, "Null DLCManager specified in {0}", new object[]
				{
					base.GetType()
				});
			}
			this.mDataInitialized = true;
			this.mDataInitialized &= (this.m_SavedReplaysGrid != null);
			this.mDataInitialized &= (this.m_Load != null);
			this.mDataInitialized &= (this.m_Cancel != null);
			this.mDataInitialized &= (this.m_Delete != null);
			this.mDataInitialized &= (this.m_SaveEntryPrefab != null);
			this.mDataInitialized &= (this.m_ReplayInfoPanel != null);
			this.mSelectedReplay.FilePath = string.Empty;
		}

		public override void Show()
		{
			base.Show();
			if (this.mDataInitialized)
			{
				this.UpdateSelectionState();
				this.PopulateLoadGrid();
			}
		}

		private void OnEnable()
		{
			this.Show();
			if (this.mDataInitialized)
			{
				this.m_Load.onClick.Add(this.mLoadEvent);
				this.m_Delete.onClick.Add(this.mDeleteEvent);
			}
		}

		private void OnDisable()
		{
			if (this.mDataInitialized)
			{
				this.m_Load.onClick.Clear();
				this.m_Delete.onClick.Clear();
			}
		}

		private void UpdateSelectionState()
		{
			BlackbirdPanelBase.SetButtonState(this.m_Load.gameObject, this.mHasSelectedReplay);
			BlackbirdPanelBase.SetButtonState(this.m_Delete.gameObject, this.mHasSelectedReplay);
			if (this.mHasSelectedReplay)
			{
				this.m_ReplayInfoPanel.PopulateInfo(this.mSelectedReplay);
				this.m_ReplayInfoPanel.ShowInfo();
				return;
			}
			this.m_ReplayInfoPanel.HideInfo();
		}

		private void PopulateLoadGrid()
		{
			if (this.m_SavedReplaysGrid != null && this.m_SaveEntryPrefab != null)
			{
				BlackbirdPanelBase.ClearGrid(this.m_SavedReplaysGrid);
				this.mReplayHeaders.Clear();
				this.mEmptyReplays.Clear();
				string[] replays = ReplayHelpers.GetReplays();
				if (replays == null || replays.Length == 0)
				{
					this.m_SavedReplaysGrid.repositionNow = true;
					return;
				}
				bool flag = false;
				foreach (string text in replays)
				{
					try
					{
						using (TextReader textReader = BBI.Core.IO.File.OpenText(text, true))
						{
							ReplayHelpers.ReplayableGameSessionHeader item;
							bool flag2 = ReplayHelpers.ReadHeaderFromStream(textReader, true, out item);
							if (flag2 && !string.IsNullOrEmpty(item.SceneName))
							{
								item.FilePath = text;
								this.mReplayHeaders.Add(item);
							}
							else if (!flag2)
							{
								FileInfo fileInfo = new FileInfo(text);
								if (fileInfo.Length <= 0L)
								{
									Log.Info(Log.Channel.UI, "Empty replay file {0} found, deleting...", new object[]
									{
										text
									});
									this.mEmptyReplays.Add(text);
								}
							}
						}
					}
					catch (Exception ex)
					{
						flag = true;
						Log.Error(Log.Channel.UI, "Unable to create replay for {0} because {1}", new object[]
						{
							text,
							ex.ToString()
						});
					}
				}
				this.mReplayHeaders.Sort(new Comparison<ReplayHelpers.ReplayableGameSessionHeader>(ReplayLoadController.SortBySaveTimeDecending));
				bool flag3 = false;
				for (int j = 0; j < this.mReplayHeaders.Count; j++)
				{
					ReplayHelpers.ReplayableGameSessionHeader replayableGameSessionHeader = this.mReplayHeaders[j];
					GameMode gameMode = replayableGameSessionHeader.GameSessionSettings.GameMode;
					GameMode mode = GameMode.None;
					LevelDefinition levelDefinition = this.mLevelManager.FindLevelFromSceneName(replayableGameSessionHeader.SceneName, gameMode);
					if (levelDefinition != null)
					{
						GameObject gameObject = NGUITools.AddChild(this.m_SavedReplaysGrid.gameObject, this.m_SaveEntryPrefab.gameObject);
						NGUIEventHandler component = gameObject.GetComponent<NGUIEventHandler>();
						SaveFileSlot component2 = gameObject.GetComponent<SaveFileSlot>();
						if (component != null && component2 != null)
						{
							bool flag4 = replayableGameSessionHeader.VersionNumber != 6;
							if (!flag4)
							{
								mode = gameMode;
							}
							component2.SetBackgroundSpriteColor(flag4);
							component.Data = replayableGameSessionHeader;
							component.ClickAction = new NGUIEventHandler.NGUIEventDelegate(this.OnReplayItemClicked);
							if (!flag3)
							{
								UIToggle component3 = gameObject.GetComponent<UIToggle>();
								if (component3 != null)
								{
									component3.Set(true);
								}
								this.SetSelectedReplay(replayableGameSessionHeader);
								flag3 = true;
							}
							string savename = Localization.Get(levelDefinition.NameLocId) + " (" + replayableGameSessionHeader.SaveTime.ToString() + ")";
							component2.SetData(savename, mode);
						}
						else
						{
							Log.Error(Log.Channel.UI, "NULL NGUIEventHandler or SaveFileSlot while creating replay list entry: {0}", new object[]
							{
								replayableGameSessionHeader.FilePath
							});
						}
					}
				}
				this.m_SavedReplaysGrid.repositionNow = true;
				if (flag && !this.mLoadErrorShown)
				{
					UIHelper.ShowMessageBox(MessageBoxLayout.OK, this.m_GameLoadErrorLocID, null);
					this.mLoadErrorShown = true;
				}
				for (int k = 0; k < this.mEmptyReplays.Count; k++)
				{
					string path = this.mEmptyReplays[k];
					ReplayHelpers.DeleteReplay(path);
				}
			}
		}

		private static int SortBySaveTimeDecending(ReplayHelpers.ReplayableGameSessionHeader a, ReplayHelpers.ReplayableGameSessionHeader b)
		{
			return b.SaveTime.CompareTo(a.SaveTime);
		}

		private void OnReplayItemClicked(NGUIEventHandler handler)
		{
			this.SetSelectedReplay((ReplayHelpers.ReplayableGameSessionHeader)handler.Data);
		}

		private void SetSelectedReplay(ReplayHelpers.ReplayableGameSessionHeader header)
		{
			if (this.mSelectedReplay.FilePath != header.FilePath)
			{
				this.mSelectedReplay = header;
				this.mHasSelectedReplay = true;
				this.UpdateSelectionState();
			}
		}

		private void OnLoadClicked()
		{
			if (!string.IsNullOrEmpty(this.mSelectedReplay.FilePath) && BBI.Core.IO.File.Exists(this.mSelectedReplay.FilePath))
			{
				if (this.mSelectedReplay.VersionNumber == 6)
				{
					this.LoadReplay();
					return;
				}
				UIHelper.ShowMessageBox(MessageBoxLayout.YesOrNo, this.m_VersionMismatchLocID, new MessageBoxResultHandler(this.OnVersionMismatchConfirmationResult));
			}
		}

		private void OnVersionMismatchConfirmationResult(MessageBoxResult result)
		{
			if (result == MessageBoxResult.Yes)
			{
				this.LoadReplay();
				return;
			}
			this.VersionMismatchDeclined();
		}

		private void VersionMismatchDeclined()
		{
			NGUITools.SetActive(base.gameObject.transform.parent.gameObject, true);
		}

		private void LoadReplay()
		{
			LevelDefinition levelDefinition = this.mLevelManager.FindLevelFromSceneName(this.mSelectedReplay.SceneName, this.mSelectedReplay.GameSessionSettings.GameMode);
			if (levelDefinition != null)
			{
				// Tell the mod what map is being loaded
				try {
					byte[] replayX = System.IO.File.ReadAllBytes(this.mSelectedReplay.FilePath + "x");
					int layoutLength = (replayX[0] << 0) + (replayX[1] << 8) + (replayX[2] << 16) + (replayX[3] << 24);
					int patchLength = (replayX[4] << 0) + (replayX[5] << 8) + (replayX[6] << 16) + (replayX[7] << 24);
					int reserved0 = (replayX[8] << 0) + (replayX[9] << 8) + (replayX[10] << 16) + (replayX[11] << 24);
					int reserved1 = (replayX[12] << 0) + (replayX[13] << 8) + (replayX[14] << 16) + (replayX[15] << 24);
					MapModManager.MapXml = Encoding.UTF8.GetString(replayX, 16, layoutLength);
					Subsystem.AttributeLoader.PatchOverrideData = Encoding.UTF8.GetString(replayX, 16 + layoutLength, patchLength);
				} catch {}

				Dictionary<CommanderID, TeamID> teams = new Dictionary<CommanderID, TeamID>();
				foreach (var tuple in this.mSelectedReplay.SessionPlayers) {
					teams[new CommanderID(tuple.CID)] = new TeamID(tuple.TeamID);
				}
				MapModManager.SetMap(levelDefinition, this.mSelectedReplay.GameSessionSettings.GameMode, this.mSelectedReplay.GameSessionSettings.TeamSetting, teams);

				Dictionary<CommanderID, PlayerSelection> dictionary = new Dictionary<CommanderID, PlayerSelection>(this.mSelectedReplay.SessionPlayers.Length);
				List<NetworkPlayerID> list = new List<NetworkPlayerID>(this.mSelectedReplay.SessionPlayers.Length);
				for (int i = 0; i < this.mSelectedReplay.SessionPlayers.Length; i++)
				{
					ReplayHelpers.ReplayDataTuple replayDataTuple = this.mSelectedReplay.SessionPlayers[i];
					CommanderAttributes commanderAttributes = null;
					foreach (DLCAssetBundleBase dlcassetBundleBase in this.mDLCManager.GetAllLoadedHeadersOfType(DLCType.Faction))
					{
						UnitFactionDLCPack unitFactionDLCPack = dlcassetBundleBase as UnitFactionDLCPack;
						if (unitFactionDLCPack != null && unitFactionDLCPack.CommanderAttrs != null && unitFactionDLCPack.CommanderAttrs.Name == replayDataTuple.CommanderAttributesName)
						{
							commanderAttributes = unitFactionDLCPack.CommanderAttrs;
							break;
						}
					}
					int j = 0;
					if (commanderAttributes == null)
					{
						for (j = 0; j < this.mLevelManager.CommanderAttributes.Length; j++)
						{
							CommanderAttributes commanderAttributes2 = this.mLevelManager.CommanderAttributes[j];
							if (commanderAttributes2.Name == replayDataTuple.CommanderAttributesName)
							{
								commanderAttributes = commanderAttributes2;
								break;
							}
						}
					}
					if (commanderAttributes == null)
					{
						Log.Error(Log.Channel.Data, "Couldn't resolve commander attributes from saved replay.  The replay that was saved didn't have commander attributes that match the commander attributes in the level ({0})", new object[]
						{
							replayDataTuple.CommanderAttributesName
						});
					}
					else
					{
						CommanderDescription desc = new CommanderDescription(replayDataTuple.PlayerName, new CommanderID(replayDataTuple.CID), replayDataTuple.PlayerID, replayDataTuple.SpawnIndex, j, new TeamID(replayDataTuple.TeamID), replayDataTuple.PlayerType, replayDataTuple.UnitColors, replayDataTuple.AIDifficulty, replayDataTuple.UnitSkinPack, replayDataTuple.RandomFaction);
						dictionary.Add(desc.CommanderID, new PlayerSelection(commanderAttributes, desc));
						if (replayDataTuple.PlayerID != NetworkPlayerID.kInvalidID)
						{
							list.Add(replayDataTuple.PlayerID);
						}
					}
				}
				GameStartSettings t = new GameStartSettings(dictionary, this.mSelectedReplay.GameSessionSettings.VictoryConditions, this.mSelectedReplay.GameSessionSettings.TeamSetting, this.mSelectedReplay.GameSessionSettings.GameModeSettings, this.mSelectedReplay.RandomSeed, false, false, AutomatchSizeType.ThreeVSThree);
				DependencyContainer<GameStartSettings, ReplayHelpers.ReplayableGameSessionHeader, SessionBase> dependencies = new DependencyContainer<GameStartSettings, ReplayHelpers.ReplayableGameSessionHeader, SessionBase>(t, this.mSelectedReplay, new NullSession(list));
				this.mLevelManager.LoadLevelForSave(levelDefinition, this.mSelectedReplay.GameSessionSettings.GameMode, ReplayMode.ReplayingGame, dependencies);
				return;
			}
			Log.Error(Log.Channel.Data, "The level associated with {0} could not be found", new object[]
			{
				this.mSelectedReplay.SceneName
			});
		}

		private void OnDeleteConfirmationResult(MessageBoxResult result)
		{
			if (result == MessageBoxResult.Yes)
			{
				ReplayHelpers.DeleteReplay(this.mSelectedReplay.FilePath);
				this.mHasSelectedReplay = false;
				this.UpdateSelectionState();
				this.PopulateLoadGrid();
			}
		}

		private void OnDeleteClicked()
		{
			UIHelper.ShowMessageBox(MessageBoxLayout.YesOrNo, this.m_DeleteConfirmationLocID, new MessageBoxResultHandler(this.OnDeleteConfirmationResult));
		}

		public ReplayLoadController()
		{
		}

		[SerializeField]
		private UIGrid m_SavedReplaysGrid;

		[SerializeField]
		private UIButton m_Load;

		[SerializeField]
		private UIButton m_Cancel;

		[SerializeField]
		private UIButton m_Delete;

		[SerializeField]
		private NGUIEventHandler m_SaveEntryPrefab;

		[SerializeField]
		private SaveInfoPanel m_ReplayInfoPanel;

		[SerializeField]
		private string m_DeleteConfirmationLocID = "(Unlocalized) Are you sure you want to delete the selected replay file?";

		[SerializeField]
		private string m_GameLoadErrorLocID = "ID_UI_FE_GEN_SAVE_LOAD_POP_UP_ERROR_839";

		[SerializeField]
		private string m_VersionMismatchLocID = "ID_UI_FE_LOAD_REPLAYS_OUTDATED_WARNING_884";

		private LevelManager mLevelManager;

		private DLCManager mDLCManager;

		private bool mDataInitialized;

		private bool mHasSelectedReplay;

		private EventDelegate mLoadEvent;

		private EventDelegate mDeleteEvent;

		private bool mLoadErrorShown;

		private List<ReplayHelpers.ReplayableGameSessionHeader> mReplayHeaders = new List<ReplayHelpers.ReplayableGameSessionHeader>();

		private List<string> mEmptyReplays = new List<string>(4);

		private ReplayHelpers.ReplayableGameSessionHeader mSelectedReplay;
	}
}
