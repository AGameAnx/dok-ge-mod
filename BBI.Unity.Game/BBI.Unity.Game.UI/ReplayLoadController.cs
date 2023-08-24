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

namespace BBI.Unity.Game.UI
{
	// Token: 0x020001EC RID: 492
	public sealed class ReplayLoadController : BlackbirdModalPanelBase
	{
		// Token: 0x06000C1E RID: 3102 RVA: 0x00039CEC File Offset: 0x00037EEC
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

		// Token: 0x06000C1F RID: 3103 RVA: 0x00039E40 File Offset: 0x00038040
		public override void Show()
		{
			base.Show();
			if (this.mDataInitialized)
			{
				this.UpdateSelectionState();
				this.PopulateLoadGrid();
			}
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00039E5C File Offset: 0x0003805C
		private void OnEnable()
		{
			this.Show();
			if (this.mDataInitialized)
			{
				this.m_Load.onClick.Add(this.mLoadEvent);
				this.m_Delete.onClick.Add(this.mDeleteEvent);
			}
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00039E98 File Offset: 0x00038098
		private void OnDisable()
		{
			if (this.mDataInitialized)
			{
				this.m_Load.onClick.Clear();
				this.m_Delete.onClick.Clear();
			}
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00039EC4 File Offset: 0x000380C4
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

		// Token: 0x06000C23 RID: 3107 RVA: 0x00039F30 File Offset: 0x00038130
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

		// Token: 0x06000C24 RID: 3108 RVA: 0x0003A278 File Offset: 0x00038478
		private static int SortBySaveTimeDecending(ReplayHelpers.ReplayableGameSessionHeader a, ReplayHelpers.ReplayableGameSessionHeader b)
		{
			return b.SaveTime.CompareTo(a.SaveTime);
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x0003A28D File Offset: 0x0003848D
		private void OnReplayItemClicked(NGUIEventHandler handler)
		{
			this.SetSelectedReplay((ReplayHelpers.ReplayableGameSessionHeader)handler.Data);
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x0003A2A0 File Offset: 0x000384A0
		private void SetSelectedReplay(ReplayHelpers.ReplayableGameSessionHeader header)
		{
			if (this.mSelectedReplay.FilePath != header.FilePath)
			{
				this.mSelectedReplay = header;
				this.mHasSelectedReplay = true;
				this.UpdateSelectionState();
			}
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x0003A2D0 File Offset: 0x000384D0
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

		// Token: 0x06000C28 RID: 3112 RVA: 0x0003A32E File Offset: 0x0003852E
		private void OnVersionMismatchConfirmationResult(MessageBoxResult result)
		{
			if (result == MessageBoxResult.Yes)
			{
				this.LoadReplay();
				return;
			}
			this.VersionMismatchDeclined();
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x0003A340 File Offset: 0x00038540
		private void VersionMismatchDeclined()
		{
			NGUITools.SetActive(base.gameObject.transform.parent.gameObject, true);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x0003A360 File Offset: 0x00038560
		private void LoadReplay()
		{
			LevelDefinition levelDefinition = this.mLevelManager.FindLevelFromSceneName(this.mSelectedReplay.SceneName, this.mSelectedReplay.GameSessionSettings.GameMode);
			if (levelDefinition != null)
			{
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

		// Token: 0x06000C2B RID: 3115 RVA: 0x0003A620 File Offset: 0x00038820
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

		// Token: 0x06000C2C RID: 3116 RVA: 0x0003A648 File Offset: 0x00038848
		private void OnDeleteClicked()
		{
			UIHelper.ShowMessageBox(MessageBoxLayout.YesOrNo, this.m_DeleteConfirmationLocID, new MessageBoxResultHandler(this.OnDeleteConfirmationResult));
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x0003A662 File Offset: 0x00038862
		public ReplayLoadController()
		{
		}

		// Token: 0x04000AE8 RID: 2792
		[SerializeField]
		private UIGrid m_SavedReplaysGrid;

		// Token: 0x04000AE9 RID: 2793
		[SerializeField]
		private UIButton m_Load;

		// Token: 0x04000AEA RID: 2794
		[SerializeField]
		private UIButton m_Cancel;

		// Token: 0x04000AEB RID: 2795
		[SerializeField]
		private UIButton m_Delete;

		// Token: 0x04000AEC RID: 2796
		[SerializeField]
		private NGUIEventHandler m_SaveEntryPrefab;

		// Token: 0x04000AED RID: 2797
		[SerializeField]
		private SaveInfoPanel m_ReplayInfoPanel;

		// Token: 0x04000AEE RID: 2798
		[SerializeField]
		private string m_DeleteConfirmationLocID = "(Unlocalized) Are you sure you want to delete the selected replay file?";

		// Token: 0x04000AEF RID: 2799
		[SerializeField]
		private string m_GameLoadErrorLocID = "ID_UI_FE_GEN_SAVE_LOAD_POP_UP_ERROR_839";

		// Token: 0x04000AF0 RID: 2800
		[SerializeField]
		private string m_VersionMismatchLocID = "ID_UI_FE_LOAD_REPLAYS_OUTDATED_WARNING_884";

		// Token: 0x04000AF1 RID: 2801
		private LevelManager mLevelManager;

		// Token: 0x04000AF2 RID: 2802
		private DLCManager mDLCManager;

		// Token: 0x04000AF3 RID: 2803
		private bool mDataInitialized;

		// Token: 0x04000AF4 RID: 2804
		private bool mHasSelectedReplay;

		// Token: 0x04000AF5 RID: 2805
		private EventDelegate mLoadEvent;

		// Token: 0x04000AF6 RID: 2806
		private EventDelegate mDeleteEvent;

		// Token: 0x04000AF7 RID: 2807
		private bool mLoadErrorShown;

		// Token: 0x04000AF8 RID: 2808
		private List<ReplayHelpers.ReplayableGameSessionHeader> mReplayHeaders = new List<ReplayHelpers.ReplayableGameSessionHeader>();

		// Token: 0x04000AF9 RID: 2809
		private List<string> mEmptyReplays = new List<string>(4);

		// Token: 0x04000AFA RID: 2810
		private ReplayHelpers.ReplayableGameSessionHeader mSelectedReplay;
	}
}
