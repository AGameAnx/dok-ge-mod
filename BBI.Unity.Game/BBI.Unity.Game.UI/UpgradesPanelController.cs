using System;
using System.Collections.Generic;
using BBI.Core.Events;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Commands;
using BBI.Game.Data;
using BBI.Game.Simulation;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Events;
using BBI.Unity.Game.HUD;
using BBI.Unity.Game.Localize;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	// Token: 0x02000211 RID: 529
	public sealed class UpgradesPanelController : IDisposable
	{
		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06000DBB RID: 3515 RVA: 0x0004541C File Offset: 0x0004361C
		// (remove) Token: 0x06000DBC RID: 3516 RVA: 0x00045454 File Offset: 0x00043654
		public event UpgradesPanelController.ShowUpgradeTooltip OnShowUpgradeTooltip;

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x00045489 File Offset: 0x00043689
		public bool IsPanelShowing
		{
			get
			{
				return this.mSettings.SelectionPanel != null && NGUITools.GetActive(this.mSettings.SelectionPanel);
			}
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x000454B0 File Offset: 0x000436B0
		public UpgradesPanelController(UpgradesPanelController.UpgradesPanelSettings settings, BlackbirdPanelBase.BlackbirdPanelGlobalLifetimeDependencyContainer globalDependencies, BlackbirdPanelBase.BlackbirdPanelSessionDependencyContainer sessionDependencies)
		{
			this.mSettings = settings;
			if (this.mSettings == null)
			{
				Log.Error(Log.Channel.UI, "NULL UpgradesPanelSettings instance specified for {0}", new object[]
				{
					base.GetType()
				});
			}
			else
			{
				if (this.mSettings.OpenCloseButton != null)
				{
					this.mSettings.OpenCloseButton.ClickAction = new NGUIEventHandler.NGUIEventDelegate(this.OnOpenCloseButtonClick);
					this.mOpenCloseButton = this.mSettings.OpenCloseButton.GetComponent<UIButton>();
					if (this.mOpenCloseButton == null)
					{
						Log.Error(Log.Channel.UI, "No UIButton component found in Open Close Button for {0}", new object[]
						{
							base.GetType()
						});
					}
				}
				else
				{
					Log.Error(Log.Channel.UI, "No Open Close button specified in UpgradesPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.SelectionPanel != null)
				{
					this.ShowHidePanel(false);
				}
				else
				{
					Log.Error(Log.Channel.UI, "No upgrades panel specified in UpgradesPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.UpgradeTable == null)
				{
					Log.Error(Log.Channel.UI, "No upgrades parent grid has been specified in UpgradesPanelSettings for {0}. We cannot conduct research!", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.NumAvailableLabel == null)
				{
					Log.Error(Log.Channel.UI, "NULL NumAvailableLabel has been specified in UpgradesPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.UpgradeListView == null)
				{
					Log.Error(Log.Channel.UI, "NULL UpgradeListView has been specified in UpgradesPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.UpgradeListBackground != null)
				{
					this.mUpgradeListBackgroundShowHeight = Math.Max(this.mUpgradeListBackgroundShowHeight, this.mSettings.UpgradeListBackground.height);
				}
				else
				{
					Log.Error(Log.Channel.UI, "NULL UpgradeListBackground has been specified in UpgradesPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.UpgradeListScrollView == null)
				{
					Log.Error(Log.Channel.UI, "NULL UpgradeListScrollView has been specified in UpgradesPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.GroupPrefab == null)
				{
					Log.Error(Log.Channel.UI, "No upgrades group has been specified in UpgradesPanelSettings for {0}. We cannot conduct research!", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.ButtonPrefab == null)
				{
					Log.Error(Log.Channel.UI, "No upgrades item prefab has been specified in UpgradesPanelSettings for {0}. We cannot conduct research!", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.ResearchTooltip == null)
				{
					Log.Error(Log.Channel.UI, "No upgrades tooltip has been specified in UpgradesPanelSettings for {0}!", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.TooltipAnchor == null)
				{
					Log.Error(Log.Channel.UI, "No upgrades tooltip anchor has been specified in UpgradesPanelSettings for {0}!", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.HighlightPanelGameObject != null)
				{
					this.mHighlightPanelWidget = this.mSettings.HighlightPanelGameObject.GetComponent<UIWidget>();
				}
			}
			if (!globalDependencies.Get<IGameLocalization>(out this.mLocalizationManager))
			{
				Log.Error(Log.Channel.UI, "NULL LocalizationManager instance found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			if (!sessionDependencies.Get<ICommanderManager>(out this.mCommanderManager))
			{
				Log.Error(Log.Channel.UI, "NULL ICommanderManager instance found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			else
			{
				this.mCommanderTechTree = this.mCommanderManager.GetCommanderTechTree(this.mCommanderManager.LocalCommanderID);
				this.lastCommanderId = this.mCommanderManager.LocalCommanderID;
			}
			if (!sessionDependencies.Get<ICommandScheduler>(out this.mCommandScheduler))
			{
				Log.Error(Log.Channel.UI, "NULL ICommandScheduler instance found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			ShipbreakersMain.PresentationEventSystem.AddHandler<UIHighlightPanelEvent>(new BBI.Core.Events.EventHandler<UIHighlightPanelEvent>(this.OnUIHighlightPanelEvent));
			this.CreateAndHideAllUpgrades(this.mLocalizationManager);
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x000458EC File Offset: 0x00043AEC
		public void Dispose()
		{
			if (this.mSettings != null && this.mSettings.SelectionPanel != null)
			{
				this.ShowHidePanel(false);
			}
			this.DestroyUpgradeGroups();
			this.mUpgradeGroups = null;
			this.mCommanderTechTree = null;
			this.mMostRecentLocalCommanderState = null;
			this.mLocalizationManager = null;
			this.mCommandScheduler = null;
			this.mCommanderManager = null;
			this.mSettings = null;
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<UIHighlightPanelEvent>(new BBI.Core.Events.EventHandler<UIHighlightPanelEvent>(this.OnUIHighlightPanelEvent));
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00045968 File Offset: 0x00043B68
		public void ShowHidePanel(bool show)
		{
			if (this.mSettings != null && this.mSettings.SelectionPanel != null)
			{
				NGUITools.SetActive(this.mSettings.SelectionPanel, show);
				this.RepositionGrids();
			}
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0004599C File Offset: 0x00043B9C
		public void Update(CommanderState commanderState, bool rebuild)
		{
			this.mMostRecentLocalCommanderState = commanderState;
			this.mActiveResearchState = ((this.mMostRecentLocalCommanderState != null) ? this.mMostRecentLocalCommanderState.GetActiveResearchStateForType(ResearchType.Upgrade) : null);
			if (rebuild)
			{
				this.RebuildList();
			}
			this.UpdateButtonStates();
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x000459D4 File Offset: 0x00043BD4
		public void TogglePanel(bool value)
		{
			if (this.mSettings != null && this.mSettings.TogglePanel != null)
			{
				this.mSettings.TogglePanel.SetActive(value);
				return;
			}
			Log.Error(Log.Channel.UI, "Could not toggle Upgrades Panel in {0} because UpgradesPanel is NULL!", new object[]
			{
				base.GetType()
			});
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x00045A30 File Offset: 0x00043C30
		public bool PanelIsActive()
		{
			if (this.mSettings != null && this.mSettings.TogglePanel != null)
			{
				return this.mSettings.TogglePanel.activeSelf;
			}
			Log.Error(Log.Channel.UI, "Could not determine if Upgrades panel is active in {0} because UpgradesPanel is NULL!", new object[]
			{
				base.GetType()
			});
			return false;
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00045A8A File Offset: 0x00043C8A
		private void RepositionGrids()
		{
			if (this.mSettings != null && this.mSettings.UpgradeTable != null)
			{
				this.mSettings.UpgradeTable.Reposition();
				this.mSettings.UpgradeTable.repositionNow = true;
			}
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00045AC8 File Offset: 0x00043CC8
		private void HideAllUpgrades()
		{
			foreach (UpgradeGroupController upgradeGroupController in this.mUpgradeGroups)
			{
				if (upgradeGroupController != null)
				{
					foreach (ResearchButtonController researchButtonController in upgradeGroupController.ResearchButtons)
					{
						if (researchButtonController != null)
						{
							NGUITools.SetActiveSelf(researchButtonController.gameObject, false);
						}
					}
					NGUITools.SetActiveSelf(upgradeGroupController.gameObject, false);
				}
			}
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00045B74 File Offset: 0x00043D74
		private void DestroyUpgradeGroups()
		{
			foreach (UpgradeGroupController upgradeGroupController in this.mUpgradeGroups)
			{
				UISystem.DeactivateAndDestroy(upgradeGroupController.gameObject);
			}
			this.mUpgradeGroups.Clear();
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x00045BD8 File Offset: 0x00043DD8
		private void UpdateButtonStates()
		{
			if (this.mMostRecentLocalCommanderState == null || this.mCommanderTechTree == null)
			{
				return;
			}
			foreach (UpgradeGroupController upgradeGroupController in this.mUpgradeGroups)
			{
				foreach (ResearchButtonController researchButtonController in upgradeGroupController.ResearchButtons)
				{
					ResearchItemAttributes researchItem = researchButtonController.ResearchItem;
					if (researchItem == null)
					{
						Log.Error(Log.Channel.UI, "Found a research button controller with no assigned research item!", new object[0]);
					}
					else if (this.mActiveResearchState != null)
					{
						if (researchItem.Name == this.mActiveResearchState.ResearchItemName)
						{
							researchButtonController.TimeRemaining = Fixed64.UnsafeFloatValue(this.mActiveResearchState.TimeRemaining);
							researchButtonController.IsActiveResearch = true;
						}
						else
						{
							researchButtonController.IsActiveResearch = false;
						}
						researchButtonController.IsButtonEnabled = false;
					}
					else
					{
						researchButtonController.IsButtonEnabled = ResearchHelperShared.CanResearchItem(this.mMostRecentLocalCommanderState, researchItem);
						researchButtonController.IsActiveResearch = false;
					}
				}
			}
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00045D04 File Offset: 0x00043F04
		private void RebuildList()
		{
			int num = 0;
			if (this.mMostRecentLocalCommanderState != null)
			{
				CommanderID commanderID = this.mMostRecentLocalCommanderState.CommanderID;
				if (this.lastCommanderId != commanderID)
				{
					this.lastCommanderId = commanderID;
					this.DestroyUpgradeGroups();
					this.mCommanderTechTree = this.mCommanderManager.GetCommanderTechTree(commanderID);
					this.CreateAndHideAllUpgrades(this.mLocalizationManager);
					this.RebuildList();
				}
				ResearchItemAttributes researchItemAttributes = null;
				if (this.mListState == UpgradesPanelController.ListState.Minimized && this.mActiveResearchState != null)
				{
					researchItemAttributes = ShipbreakersMain.GetEntityTypeAttributes<ResearchItemAttributes>(this.mActiveResearchState.ResearchItemName);
				}
				foreach (UpgradeGroupController upgradeGroupController in this.mUpgradeGroups)
				{
					if (upgradeGroupController != null)
					{
						upgradeGroupController.UpdateVisibility(this.mMostRecentLocalCommanderState, researchItemAttributes);
						num += upgradeGroupController.NumAvailableUpgrades;
						if (this.mListState == UpgradesPanelController.ListState.Minimized && researchItemAttributes == null)
						{
							upgradeGroupController.Hide();
						}
					}
				}
				this.RepositionGrids();
				this.RefreshBackgroundSize();
				this.RefreshScrollPanel();
			}
			if (this.mSettings != null && this.mSettings.NumAvailableLabel != null)
			{
				this.mSettings.NumAvailableLabel.text = string.Format("{0}", num);
			}
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x00045E00 File Offset: 0x00044000
		private void RefreshBackgroundSize()
		{
			if (this.mSettings == null || this.mSettings.UpgradeTable == null || this.mSettings.UpgradeListBackground == null)
			{
				return;
			}
			int num = Mathf.Clamp((int)NGUIMath.CalculateRelativeWidgetBounds(this.mSettings.UpgradeTable.transform, false).size.y, 0, this.mUpgradeListBackgroundShowHeight - this.mSettings.UpgradeListBackground.minHeight);
			this.mSettings.UpgradeListBackground.height = num + this.mSettings.UpgradeListBackground.minHeight;
			if (this.mHighlightPanelWidget != null)
			{
				int num2 = 4;
				this.mHighlightPanelWidget.height = this.mSettings.UpgradeListBackground.height + num2;
			}
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x00045ED0 File Offset: 0x000440D0
		private void RefreshScrollPanel()
		{
			if (this.mSettings == null || this.mSettings.UpgradeListScrollView == null)
			{
				return;
			}
			this.mSettings.UpgradeListScrollView.ResetPosition();
			if (this.mSettings.UpgradeListScrollView.verticalScrollBar != null)
			{
				NGUITools.SetActiveSelf(this.mSettings.UpgradeListScrollView.verticalScrollBar.gameObject, this.mListState == UpgradesPanelController.ListState.ShowAll);
			}
			if (this.mSettings.UpgradeListBackground != null)
			{
				bool flag = this.mSettings.UpgradeListBackground.height >= this.mUpgradeListBackgroundShowHeight - 1;
				this.mSettings.UpgradeListScrollView.enabled = flag;
				if (!flag)
				{
					NGUITools.SetActiveSelf(this.mSettings.UpgradeListScrollView.verticalScrollBar.gameObject, false);
				}
			}
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x00045FA4 File Offset: 0x000441A4
		private void CreateAndHideAllUpgrades(IGameLocalization localizationManager)
		{
			if (this.mCommanderTechTree != null && this.mSettings != null && this.mSettings.UpgradeTable != null && this.mSettings.GroupPrefab != null && this.mSettings.ButtonPrefab != null)
			{
				int num = this.mCommanderTechTree.TechTrees.Length;
				for (int i = 0; i < num; i++)
				{
					TechTree techTree = this.mCommanderTechTree.TechTrees[i];
					UpgradeGroupController component = NGUITools.AddChild(this.mSettings.UpgradeTable.gameObject, this.mSettings.GroupPrefab.gameObject).GetComponent<UpgradeGroupController>();
					this.mUpgradeGroups.Add(component);
					if (component != null)
					{
						component.Setup(techTree, this.mSettings.ButtonPrefab, localizationManager);
						foreach (ResearchButtonController researchButtonController in component.ResearchButtons)
						{
							researchButtonController.Hovered += this.OnResearchItemTooltip;
							researchButtonController.Clicked += this.OnResearchItemClicked;
							researchButtonController.CancelClicked += this.OnResearchItemCancelClicked;
						}
						component.Hide();
					}
				}
			}
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0004610C File Offset: 0x0004430C
		private void OnUIHighlightPanelEvent(UIHighlightPanelEvent ev)
		{
			if (ev.PanelToHighlight == UIPanelToHighlight.Upgrades)
			{
				if (this.mSettings.HighlightPanelGameObject != null)
				{
					this.mSettings.HighlightPanelGameObject.SetActive(ev.Enabled);
					return;
				}
				Log.Error(Log.Channel.UI, "Could not toggle Highlight for Upgrades Panel in {0} because m_HighlightPanelGameObject is unassigned!", new object[]
				{
					base.GetType()
				});
			}
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0004616D File Offset: 0x0004436D
		private void OnResearchItemTooltip(ResearchButtonController controller, bool show)
		{
			if (this.OnShowUpgradeTooltip != null && this.mSettings != null)
			{
				this.OnShowUpgradeTooltip(controller, this.mSettings.ResearchTooltip, this.mSettings.TooltipAnchor, show);
			}
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x000461A4 File Offset: 0x000443A4
		private void OnResearchItemClicked(ResearchButtonController controller)
		{
			ResearchItemAttributes researchItem = controller.ResearchItem;
			if (researchItem != null && this.mMostRecentLocalCommanderState != null)
			{
				ResearchPanelsController.ScheduleStartResearchCommand(this.mCommandScheduler, this.mMostRecentLocalCommanderState.CommanderID, researchItem);
			}
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x000461DC File Offset: 0x000443DC
		private void OnResearchItemCancelClicked(ResearchButtonController controller)
		{
			if (this.mMostRecentLocalCommanderState == null)
			{
				Log.Error(Log.Channel.UI, "Can't cancel research without a user state!", new object[0]);
				return;
			}
			if (controller != null && controller.ResearchItem != null)
			{
				CommanderID commanderID = this.mMostRecentLocalCommanderState.CommanderID;
				SimCommandBase command = SimCommandFactory.CreateCancelResearchCommand(commanderID, controller.ResearchItem);
				if (!this.mCommandScheduler.ScheduleCommand(command))
				{
					Log.Error(Log.Channel.UI, "Couldn't schedule cancel research command in {0}", new object[]
					{
						base.GetType()
					});
					return;
				}
			}
			else
			{
				Log.Error(Log.Channel.UI, "Can't cancel research item: NULL ResearchButtonController or ResearchItem in {0}", new object[]
				{
					base.GetType()
				});
			}
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x00046280 File Offset: 0x00044480
		private void OnOpenCloseButtonClick(NGUIEventHandler handler)
		{
			if (this.mSettings != null && this.mSettings.UpgradeListView != null && this.mSettings.UpgradeListBackground != null)
			{
				this.mListState = ((this.mListState == UpgradesPanelController.ListState.ShowAll) ? UpgradesPanelController.ListState.Minimized : UpgradesPanelController.ListState.ShowAll);
				this.RebuildList();
			}
		}

		// Token: 0x04000C45 RID: 3141
		private const ResearchType kResearchType = ResearchType.Upgrade;

		// Token: 0x04000C46 RID: 3142
		private UpgradesPanelController.UpgradesPanelSettings mSettings;

		// Token: 0x04000C47 RID: 3143
		private CommanderState mMostRecentLocalCommanderState;

		// Token: 0x04000C48 RID: 3144
		private ICommanderManager mCommanderManager;

		// Token: 0x04000C49 RID: 3145
		private TechTreeAttributes mCommanderTechTree;

		// Token: 0x04000C4A RID: 3146
		private ICommandScheduler mCommandScheduler;

		// Token: 0x04000C4B RID: 3147
		private IGameLocalization mLocalizationManager;

		// Token: 0x04000C4C RID: 3148
		private UpgradesPanelController.ListState mListState = UpgradesPanelController.ListState.ShowAll;

		// Token: 0x04000C4D RID: 3149
		private List<UpgradeGroupController> mUpgradeGroups = new List<UpgradeGroupController>();

		// Token: 0x04000C4E RID: 3150
		private UIButton mOpenCloseButton;

		// Token: 0x04000C4F RID: 3151
		private readonly int mUpgradeListBackgroundShowHeight = 338;

		// Token: 0x04000C50 RID: 3152
		private ActiveResearchState mActiveResearchState;

		// Token: 0x04000C51 RID: 3153
		private UIWidget mHighlightPanelWidget;

		private CommanderID lastCommanderId;

		// Token: 0x02000212 RID: 530
		[Serializable]
		public class UpgradesPanelSettings
		{
			// Token: 0x17000247 RID: 583
			// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x000462D4 File Offset: 0x000444D4
			public GameObject TogglePanel
			{
				get
				{
					return this.m_TogglePanel;
				}
			}

			// Token: 0x17000248 RID: 584
			// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x000462DC File Offset: 0x000444DC
			public GameObject SelectionPanel
			{
				get
				{
					return this.m_SelectionPanel;
				}
			}

			// Token: 0x17000249 RID: 585
			// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x000462E4 File Offset: 0x000444E4
			public Transform TooltipAnchor
			{
				get
				{
					return this.m_TooltipAnchor;
				}
			}

			// Token: 0x1700024A RID: 586
			// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x000462EC File Offset: 0x000444EC
			public TooltipObjectAsset ResearchTooltip
			{
				get
				{
					return this.m_ResearchTooltip;
				}
			}

			// Token: 0x1700024B RID: 587
			// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x000462F4 File Offset: 0x000444F4
			public NGUIEventHandler OpenCloseButton
			{
				get
				{
					return this.m_OpenCloseButton;
				}
			}

			// Token: 0x1700024C RID: 588
			// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x000462FC File Offset: 0x000444FC
			public UITable UpgradeTable
			{
				get
				{
					return this.m_UpgradeTable;
				}
			}

			// Token: 0x1700024D RID: 589
			// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x00046304 File Offset: 0x00044504
			public UILabel NumAvailableLabel
			{
				get
				{
					return this.m_NumAvailableLabel;
				}
			}

			// Token: 0x1700024E RID: 590
			// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x0004630C File Offset: 0x0004450C
			public GameObject UpgradeListView
			{
				get
				{
					return this.m_UpgradeListView;
				}
			}

			// Token: 0x1700024F RID: 591
			// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x00046314 File Offset: 0x00044514
			public UISprite UpgradeListBackground
			{
				get
				{
					return this.m_UpgradeListBackground;
				}
			}

			// Token: 0x17000250 RID: 592
			// (get) Token: 0x06000DDA RID: 3546 RVA: 0x0004631C File Offset: 0x0004451C
			public UIScrollView UpgradeListScrollView
			{
				get
				{
					return this.m_UpgradeListScrollView;
				}
			}

			// Token: 0x17000251 RID: 593
			// (get) Token: 0x06000DDB RID: 3547 RVA: 0x00046324 File Offset: 0x00044524
			public UpgradeGroupController GroupPrefab
			{
				get
				{
					return this.m_GroupPrefab;
				}
			}

			// Token: 0x17000252 RID: 594
			// (get) Token: 0x06000DDC RID: 3548 RVA: 0x0004632C File Offset: 0x0004452C
			public ResearchButtonController ButtonPrefab
			{
				get
				{
					return this.m_ButtonPrefab;
				}
			}

			// Token: 0x17000253 RID: 595
			// (get) Token: 0x06000DDD RID: 3549 RVA: 0x00046334 File Offset: 0x00044534
			public float TableWidth
			{
				get
				{
					return this.m_TableWidth;
				}
			}

			// Token: 0x17000254 RID: 596
			// (get) Token: 0x06000DDE RID: 3550 RVA: 0x0004633C File Offset: 0x0004453C
			public float MaxTableHeight
			{
				get
				{
					return this.m_MaxTableHeight;
				}
			}

			// Token: 0x17000255 RID: 597
			// (get) Token: 0x06000DDF RID: 3551 RVA: 0x00046344 File Offset: 0x00044544
			public float ResearchItemPadding
			{
				get
				{
					return this.m_ResearchItemPadding;
				}
			}

			// Token: 0x17000256 RID: 598
			// (get) Token: 0x06000DE0 RID: 3552 RVA: 0x0004634C File Offset: 0x0004454C
			public GameObject HighlightPanelGameObject
			{
				get
				{
					return this.m_HighlightPanelGameObject;
				}
			}

			// Token: 0x06000DE1 RID: 3553 RVA: 0x00046354 File Offset: 0x00044554
			public UpgradesPanelSettings()
			{
			}

			// Token: 0x04000C53 RID: 3155
			[Header("GameObject that toggles when the panel is Enabled/Disabled")]
			[SerializeField]
			private GameObject m_TogglePanel;

			// Token: 0x04000C54 RID: 3156
			[Header("GameObject that toggles when the unit is selected")]
			[SerializeField]
			private GameObject m_SelectionPanel;

			// Token: 0x04000C55 RID: 3157
			[SerializeField]
			private Transform m_TooltipAnchor;

			// Token: 0x04000C56 RID: 3158
			[SerializeField]
			private TooltipObjectAsset m_ResearchTooltip;

			// Token: 0x04000C57 RID: 3159
			[SerializeField]
			private NGUIEventHandler m_OpenCloseButton;

			// Token: 0x04000C58 RID: 3160
			[SerializeField]
			private UITable m_UpgradeTable;

			// Token: 0x04000C59 RID: 3161
			[SerializeField]
			private UILabel m_NumAvailableLabel;

			// Token: 0x04000C5A RID: 3162
			[SerializeField]
			private GameObject m_UpgradeListView;

			// Token: 0x04000C5B RID: 3163
			[SerializeField]
			private UISprite m_UpgradeListBackground;

			// Token: 0x04000C5C RID: 3164
			[SerializeField]
			private UIScrollView m_UpgradeListScrollView;

			// Token: 0x04000C5D RID: 3165
			[SerializeField]
			private UpgradeGroupController m_GroupPrefab;

			// Token: 0x04000C5E RID: 3166
			[SerializeField]
			private ResearchButtonController m_ButtonPrefab;

			// Token: 0x04000C5F RID: 3167
			[SerializeField]
			private float m_TableWidth = 380f;

			// Token: 0x04000C60 RID: 3168
			[SerializeField]
			private float m_MaxTableHeight = 200f;

			// Token: 0x04000C61 RID: 3169
			[SerializeField]
			private float m_ResearchItemPadding = 6f;

			// Token: 0x04000C62 RID: 3170
			[SerializeField]
			private GameObject m_HighlightPanelGameObject;
		}

		// Token: 0x02000213 RID: 531
		private enum ListState
		{
			// Token: 0x04000C64 RID: 3172
			Minimized,
			// Token: 0x04000C65 RID: 3173
			ShowAll
		}

		// Token: 0x02000214 RID: 532
		// (Invoke) Token: 0x06000DE3 RID: 3555
		public delegate void ShowUpgradeTooltip(ResearchButtonController controller, TooltipObjectAsset tooltipObject, Transform anchor, bool onHover);
	}
}
