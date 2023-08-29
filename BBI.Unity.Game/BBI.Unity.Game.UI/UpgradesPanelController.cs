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
	public sealed class UpgradesPanelController : IDisposable
	{
		public event UpgradesPanelController.ShowUpgradeTooltip OnShowUpgradeTooltip;

		public bool IsPanelShowing
		{
			get
			{
				return this.mSettings.SelectionPanel != null && NGUITools.GetActive(this.mSettings.SelectionPanel);
			}
		}

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

		public void ShowHidePanel(bool show)
		{
			if (this.mSettings != null && this.mSettings.SelectionPanel != null)
			{
				NGUITools.SetActive(this.mSettings.SelectionPanel, show);
				this.RepositionGrids();
			}
		}

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

		private void RepositionGrids()
		{
			if (this.mSettings != null && this.mSettings.UpgradeTable != null)
			{
				this.mSettings.UpgradeTable.Reposition();
				this.mSettings.UpgradeTable.repositionNow = true;
			}
		}

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

		private void DestroyUpgradeGroups()
		{
			foreach (UpgradeGroupController upgradeGroupController in this.mUpgradeGroups)
			{
				UISystem.DeactivateAndDestroy(upgradeGroupController.gameObject);
			}
			this.mUpgradeGroups.Clear();
		}

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
						upgradeGroupController.UpdateVisibility(this.mMostRecentLocalCommanderState, researchItemAttributes, this.mCommanderTechTree);
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

		private void OnResearchItemTooltip(ResearchButtonController controller, bool show)
		{
			if (this.OnShowUpgradeTooltip != null && this.mSettings != null)
			{
				this.OnShowUpgradeTooltip(controller, this.mSettings.ResearchTooltip, this.mSettings.TooltipAnchor, show);
			}
		}

		private void OnResearchItemClicked(ResearchButtonController controller)
		{
			ResearchItemAttributes researchItem = controller.ResearchItem;
			if (researchItem != null && this.mMostRecentLocalCommanderState != null)
			{
				ResearchPanelsController.ScheduleStartResearchCommand(this.mCommandScheduler, this.mMostRecentLocalCommanderState.CommanderID, researchItem);
			}
		}

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

		private void OnOpenCloseButtonClick(NGUIEventHandler handler)
		{
			if (this.mSettings != null && this.mSettings.UpgradeListView != null && this.mSettings.UpgradeListBackground != null)
			{
				this.mListState = ((this.mListState == UpgradesPanelController.ListState.ShowAll) ? UpgradesPanelController.ListState.Minimized : UpgradesPanelController.ListState.ShowAll);
				this.RebuildList();
			}
		}

		private const ResearchType kResearchType = ResearchType.Upgrade;

		private UpgradesPanelController.UpgradesPanelSettings mSettings;

		private CommanderState mMostRecentLocalCommanderState;

		private ICommanderManager mCommanderManager;

		private TechTreeAttributes mCommanderTechTree;

		private ICommandScheduler mCommandScheduler;

		private IGameLocalization mLocalizationManager;

		private UpgradesPanelController.ListState mListState = UpgradesPanelController.ListState.ShowAll;

		private List<UpgradeGroupController> mUpgradeGroups = new List<UpgradeGroupController>();

		private UIButton mOpenCloseButton;

		private readonly int mUpgradeListBackgroundShowHeight = 338;

		private ActiveResearchState mActiveResearchState;

		private UIWidget mHighlightPanelWidget;

		private CommanderID lastCommanderId;

		[Serializable]
		public class UpgradesPanelSettings
		{
			public GameObject TogglePanel
			{
				get
				{
					return this.m_TogglePanel;
				}
			}

			public GameObject SelectionPanel
			{
				get
				{
					return this.m_SelectionPanel;
				}
			}

			public Transform TooltipAnchor
			{
				get
				{
					return this.m_TooltipAnchor;
				}
			}

			public TooltipObjectAsset ResearchTooltip
			{
				get
				{
					return this.m_ResearchTooltip;
				}
			}

			public NGUIEventHandler OpenCloseButton
			{
				get
				{
					return this.m_OpenCloseButton;
				}
			}

			public UITable UpgradeTable
			{
				get
				{
					return this.m_UpgradeTable;
				}
			}

			public UILabel NumAvailableLabel
			{
				get
				{
					return this.m_NumAvailableLabel;
				}
			}

			public GameObject UpgradeListView
			{
				get
				{
					return this.m_UpgradeListView;
				}
			}

			public UISprite UpgradeListBackground
			{
				get
				{
					return this.m_UpgradeListBackground;
				}
			}

			public UIScrollView UpgradeListScrollView
			{
				get
				{
					return this.m_UpgradeListScrollView;
				}
			}

			public UpgradeGroupController GroupPrefab
			{
				get
				{
					return this.m_GroupPrefab;
				}
			}

			public ResearchButtonController ButtonPrefab
			{
				get
				{
					return this.m_ButtonPrefab;
				}
			}

			public float TableWidth
			{
				get
				{
					return this.m_TableWidth;
				}
			}

			public float MaxTableHeight
			{
				get
				{
					return this.m_MaxTableHeight;
				}
			}

			public float ResearchItemPadding
			{
				get
				{
					return this.m_ResearchItemPadding;
				}
			}

			public GameObject HighlightPanelGameObject
			{
				get
				{
					return this.m_HighlightPanelGameObject;
				}
			}

			public UpgradesPanelSettings()
			{
			}

			[Header("GameObject that toggles when the panel is Enabled/Disabled")]
			[SerializeField]
			private GameObject m_TogglePanel;

			[Header("GameObject that toggles when the unit is selected")]
			[SerializeField]
			private GameObject m_SelectionPanel;

			[SerializeField]
			private Transform m_TooltipAnchor;

			[SerializeField]
			private TooltipObjectAsset m_ResearchTooltip;

			[SerializeField]
			private NGUIEventHandler m_OpenCloseButton;

			[SerializeField]
			private UITable m_UpgradeTable;

			[SerializeField]
			private UILabel m_NumAvailableLabel;

			[SerializeField]
			private GameObject m_UpgradeListView;

			[SerializeField]
			private UISprite m_UpgradeListBackground;

			[SerializeField]
			private UIScrollView m_UpgradeListScrollView;

			[SerializeField]
			private UpgradeGroupController m_GroupPrefab;

			[SerializeField]
			private ResearchButtonController m_ButtonPrefab;

			[SerializeField]
			private float m_TableWidth = 380f;

			[SerializeField]
			private float m_MaxTableHeight = 200f;

			[SerializeField]
			private float m_ResearchItemPadding = 6f;

			[SerializeField]
			private GameObject m_HighlightPanelGameObject;
		}

		private enum ListState
		{
			Minimized,
			ShowAll
		}

		public delegate void ShowUpgradeTooltip(ResearchButtonController controller, TooltipObjectAsset tooltipObject, Transform anchor, bool onHover);
	}
}
