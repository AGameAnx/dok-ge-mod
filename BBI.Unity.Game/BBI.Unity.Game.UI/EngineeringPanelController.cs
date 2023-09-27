using System;
using System.Collections.Generic;
using BBI.Core;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Commands;
using BBI.Game.Data;
using BBI.Game.Simulation;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Localize;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	public sealed class EngineeringPanelController : IDisposable
	{
		public event EngineeringPanelController.ShowEngineeringTooltip OnShowEngineeringTooltip;

		public bool IsPanelShowing
		{
			get
			{
				return this.mSettings.ResearchPanel != null && NGUITools.GetActive(this.mSettings.ResearchPanel);
			}
		}

		public EngineeringPanelController(EngineeringPanelController.EngineeringPanelSettings settings, BlackbirdPanelBase.BlackbirdPanelGlobalLifetimeDependencyContainer globalDependencies, BlackbirdPanelBase.BlackbirdPanelSessionDependencyContainer sessionDependencies)
		{
			this.mSettings = settings;
			if (this.mSettings == null)
			{
				Log.Error(Log.Channel.UI, "NULL EngineeringPanelSettings instance specified for {0}", new object[]
				{
					base.GetType()
				});
			}
			else
			{
				if (this.mSettings.OpenCloseButton != null)
				{
					this.mSettings.OpenCloseButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnOpenCloseButtonClick)));
				}
				else
				{
					Log.Error(Log.Channel.UI, "No Open Close button specified in EngineeringPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.ResearchPanel != null)
				{
					this.ShowHidePanel(false);
				}
				else
				{
					Log.Error(Log.Channel.UI, "No research panel specified in EngineeringPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.TechTreeGrids.IsNullOrEmpty<UIGrid>())
				{
					Log.Error(Log.Channel.UI, "No tech tree grids are hooked up in EngineeringPanelSettings for {0}. Research cannot be conducted!", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.ParentGrid == null)
				{
					Log.Error(Log.Channel.UI, "No research parent grid has been specified in EngineeringPanelSettings for {0}. We cannot conduct research!", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.ButtonPrefab == null)
				{
					Log.Error(Log.Channel.UI, "No tech tree research item prefab has been specified in EngineeringPanelSettings for {0}. We cannot conduct research!", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.ResearchTooltip == null)
				{
					Log.Error(Log.Channel.UI, "No research tooltip has been specified in EngineeringPanelSettings for {0}!", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.TooltipAnchor == null)
				{
					Log.Error(Log.Channel.UI, "No research tooltip anchor has been specified in EngineeringPanelSettings for {0}!", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.BackgroundSprite == null)
				{
					Log.Error(Log.Channel.UI, "NULL BackgroundSprite has been specified in EngineeringPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
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
			}
			if (!sessionDependencies.Get<ICommandScheduler>(out this.mCommandScheduler))
			{
				Log.Error(Log.Channel.UI, "NULL ICommandScheduler instance found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			this.mResearchButtons = new List<ResearchButtonController>();
		}

		public void Dispose()
		{
			if (this.mSettings != null)
			{
				if (this.mSettings.OpenCloseButton != null)
				{
					this.mSettings.OpenCloseButton.onClick.Clear();
				}
				if (this.mSettings.ResearchPanel != null)
				{
					this.ShowHidePanel(false);
				}
			}
			this.DestroyResearchButtons();
			this.mResearchButtons = null;
			this.mCommanderTechTree = null;
			this.mMostRecentLocalCommanderState = null;
			this.mLocalizationManager = null;
			this.mCommandScheduler = null;
			this.mCommanderManager = null;
			this.mSettings = null;
		}

		public void ShowHidePanel(bool show)
		{
			if (this.mSettings != null && this.mSettings.ResearchPanel != null)
			{
				NGUITools.SetActive(this.mSettings.ResearchPanel, show);
				this.RepositionGrids();
			}
		}

		public void Update(CommanderState commanderState, bool rebuild)
		{
			this.mMostRecentLocalCommanderState = commanderState;
			if (rebuild)
			{
				this.RebuildTreeView();
			}
			this.UpdateButtonStates();
		}

		private void RepositionGrids()
		{
			if (this.mSettings != null && this.mSettings.ParentGrid != null)
			{
				this.mSettings.ParentGrid.Reposition();
				this.mSettings.ParentGrid.repositionNow = true;
			}
			this.RefreshBackgroundSize();
		}

		private void DestroyResearchButtons()
		{
			if (!this.mResearchButtons.IsNullOrEmpty<ResearchButtonController>())
			{
				foreach (ResearchButtonController researchButtonController in this.mResearchButtons)
				{
					UISystem.DeactivateAndDestroy(researchButtonController.gameObject);
				}
				this.mResearchButtons.Clear();
			}
		}

		private void UpdateButtonStates()
		{
			if (this.mMostRecentLocalCommanderState == null || this.mCommanderTechTree == null)
			{
				return;
			}
			ActiveResearchState activeResearchStateForType = this.mMostRecentLocalCommanderState.GetActiveResearchStateForType(ResearchType.Engineering);
			foreach (ResearchButtonController researchButtonController in this.mResearchButtons)
			{
				ResearchItemAttributes researchItem = researchButtonController.ResearchItem;
				if (researchItem == null)
				{
					Log.Error(Log.Channel.UI, "Found a research button controller with now assigned research item!", new object[0]);
				}
				else if (activeResearchStateForType != null)
				{
					if (researchItem.Name == activeResearchStateForType.ResearchItemName)
					{
						researchButtonController.TimeRemaining = Fixed64.UnsafeFloatValue(activeResearchStateForType.TimeRemaining);
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

		private void RebuildTreeView()
		{
			this.mCommanderTechTree = this.mCommanderManager.GetCommanderTechTree(this.mMostRecentLocalCommanderState.CommanderID);
			if (this.mMostRecentLocalCommanderState != null && this.mCommanderTechTree != null && this.mSettings != null && this.mSettings.TechTreeGrids != null && this.mSettings.ButtonPrefab != null)
			{
				this.DestroyResearchButtons();
				int num = this.mCommanderTechTree.TechTrees.Length;
				int num2 = 0;
				while (num2 < num && num2 < this.mSettings.TechTreeGrids.Length)
				{
					UIGrid uigrid = this.mSettings.TechTreeGrids[num2];
					TechTree techTree = this.mCommanderTechTree.TechTrees[num2];
					int num3 = this.mMostRecentLocalCommanderState.CurrentTechTreeTiers[num2];
					if (num3 >= techTree.Tiers.Length)
					{
						NGUITools.SetActive(uigrid.gameObject, false);
					}
					else
					{
						NGUITools.SetActive(uigrid.gameObject, true);
						TechTreeTier techTreeTier = techTree.Tiers[num3];
						for (int i = 0; i < techTreeTier.ResearchItems.Length; i++)
						{
							ResearchItemAttributes entityTypeAttributes = ShipbreakersMain.GetEntityTypeAttributes<ResearchItemAttributes>(techTreeTier.ResearchItems[i]);
							if (entityTypeAttributes == null)
							{
								Log.Error(Log.Channel.UI, "Unable to find research item {0}. Please make sure that it is added to the entity list in master!", new object[]
								{
									techTreeTier.ResearchItems[i]
								});
							}
							else
							{
								bool flag = ResearchHelperShared.AreResearchDependenciesMet(this.mMostRecentLocalCommanderState, entityTypeAttributes);
								float y = this.mSettings.ButtonPrefab.Size.y;
								if (flag)
								{
									ResearchButtonController component = NGUITools.AddChild(uigrid.gameObject, this.mSettings.ButtonPrefab.gameObject).GetComponent<ResearchButtonController>();
									component.gameObject.name = i.ToString();
									component.InitializeResearchItem(entityTypeAttributes, this.mLocalizationManager);
									component.IsActiveResearch = false;
									component.Size = new Vector2(this.mSettings.TableWidth / (float)techTreeTier.ResearchItems.Length - ((i > 0) ? this.mSettings.ResearchItemPadding : 0f), y);
									component.Hovered += this.OnResearchItemTooltip;
									component.Clicked += this.OnResearchItemClicked;
									component.CancelClicked += this.OnResearchItemCancelClicked;
									this.mResearchButtons.Add(component);
								}
							}
						}
						uigrid.Reposition();
						uigrid.repositionNow = true;
					}
					num2++;
				}
				this.RepositionGrids();
			}
		}

		private void RefreshBackgroundSize()
		{
			if (this.mSettings == null || this.mSettings.ParentGrid == null || this.mSettings.BackgroundSprite == null)
			{
				return;
			}
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(this.mSettings.ParentGrid.transform, false);
			this.mSettings.BackgroundSprite.height = (int)bounds.size.y + this.mSettings.BackgroundSprite.minHeight;
			if (this.mSettings.UpgradesPanel != null && this.mSettings.ResearchPanel != null)
			{
				Bounds bounds2 = NGUIMath.CalculateRelativeWidgetBounds(this.mSettings.BackgroundSprite.transform, false);
				Vector3 localPosition = this.mSettings.UpgradesPanel.transform.localPosition;
				localPosition.y = bounds2.min.y;
				this.mSettings.UpgradesPanel.transform.localPosition = localPosition;
			}
		}

		private void OnResearchItemTooltip(ResearchButtonController controller, bool show)
		{
			if (this.OnShowEngineeringTooltip != null && this.mSettings != null)
			{
				this.OnShowEngineeringTooltip(controller, this.mSettings.ResearchTooltip, this.mSettings.TooltipAnchor, show);
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

		private void OnOpenCloseButtonClick()
		{
			if (this.mSettings != null && this.mSettings.ParentGrid != null && this.mSettings.BackgroundSprite != null)
			{
				if (this.mIsShowing)
				{
					this.DestroyResearchButtons();
					this.RepositionGrids();
				}
				else
				{
					this.RebuildTreeView();
					this.RepositionGrids();
				}
				this.mIsShowing = !this.mIsShowing;
			}
		}

		private const string kCancelButtonVariableName = "Button";

		private const ResearchType kResearchType = ResearchType.Engineering;

		private EngineeringPanelController.EngineeringPanelSettings mSettings;

		private CommanderState mMostRecentLocalCommanderState;

		private ICommanderManager mCommanderManager;

		private TechTreeAttributes mCommanderTechTree;

		private ICommandScheduler mCommandScheduler;

		private IGameLocalization mLocalizationManager;

		private List<ResearchButtonController> mResearchButtons;

		private bool mIsShowing = true;

		[Serializable]
		public class EngineeringPanelSettings
		{
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

			public GameObject ResearchPanel
			{
				get
				{
					return this.m_ResearchPanel;
				}
			}

			public UIGrid ParentGrid
			{
				get
				{
					return this.m_ParentGrid;
				}
			}

			public UIGrid[] TechTreeGrids
			{
				get
				{
					return this.m_TechTreeGrids;
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

			public float ResearchItemPadding
			{
				get
				{
					return this.m_ResearchItemPadding;
				}
			}

			public UISprite BackgroundSprite
			{
				get
				{
					return this.m_BackgroundSprite;
				}
			}

			public GameObject UpgradesPanel
			{
				get
				{
					return this.m_UpgradesPanel;
				}
			}

			public UIButton OpenCloseButton
			{
				get
				{
					return this.m_OpenCloseButton;
				}
			}

			public EngineeringPanelSettings()
			{
			}

			[SerializeField]
			private Transform m_TooltipAnchor;

			[SerializeField]
			private TooltipObjectAsset m_ResearchTooltip;

			[SerializeField]
			private GameObject m_ResearchPanel;

			[SerializeField]
			private UIGrid m_ParentGrid;

			[SerializeField]
			private UIGrid[] m_TechTreeGrids;

			[SerializeField]
			private ResearchButtonController m_ButtonPrefab;

			[SerializeField]
			private float m_TableWidth = 660f;

			[SerializeField]
			private float m_ResearchItemPadding = 6f;

			[SerializeField]
			private UISprite m_BackgroundSprite;

			[SerializeField]
			private GameObject m_UpgradesPanel;

			[SerializeField]
			private UIButton m_OpenCloseButton;
		}

		public delegate void ShowEngineeringTooltip(ResearchButtonController controller, TooltipObjectAsset tooltipObject, Transform anchor, bool onHover);
	}
}
