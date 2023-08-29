using System;
using System.Collections.Generic;
using BBI.Core;
using BBI.Core.ComponentModel;
using BBI.Core.Events;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Commands;
using BBI.Game.Data;
using BBI.Game.Events;
using BBI.Game.Replay;
using BBI.Game.Simulation;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Events;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	public sealed class ResearchPanelsController : BlackbirdPanelBase
	{
		private bool ArePanelsShowing
		{
			get
			{
				return (this.mEngineeringController != null && this.mEngineeringController.IsPanelShowing) || (this.mUpgradesController != null && this.mUpgradesController.IsPanelShowing);
			}
		}

		protected override void OnSessionStarted()
		{
			if (ShipbreakersMain.GameMode == GameMode.SinglePlayer)
			{
				this.mUpgradesController = new UpgradesPanelController(this.m_UpgradesPanelSettingsSP, base.GlobalDependencyContainer, base.SessionDependencies);
			}
			else
			{
				this.mUpgradesController = new UpgradesPanelController(this.m_UpgradesPanelSettings, base.GlobalDependencyContainer, base.SessionDependencies);
			}
			this.mEngineeringController = new EngineeringPanelController(this.m_EngineeringPanelSettings, base.GlobalDependencyContainer, base.SessionDependencies);
			UISystem uisystem;
			if (base.GlobalDependencyContainer.Get<UISystem>(out uisystem))
			{
				this.mInterfaceController = uisystem.GetPanel<UnitInterfaceController>();
				if (this.mInterfaceController == null)
				{
					Log.Error(Log.Channel.UI, "NULL UnitInterfaceController instance not found for {0}", new object[]
					{
						base.GetType()
					});
				}
				else
				{
					this.mInterfaceController.NewFrameFromSim += this.OnNewStateFrame;
					this.mInterfaceController.NewSelection += this.OnNewSelection;
					this.mInterfaceController.NewLeadUnitSelected += this.OnNewLeadUnitSelected;
				}
			}
			else
			{
				Log.Error(Log.Channel.UI, "NULL UISystem instance found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			if (!base.SessionDependencies.Get<ICommanderManager>(out this.mCommanderManager))
			{
				Log.Error(Log.Channel.UI, "NULL ICommanderManager instance found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			if (!base.SessionDependencies.Get<ICommanderInteractionProvider>(out this.mInteractionProvider))
			{
				Log.Error(Log.Channel.UI, "NULL ICommanderInteractionProvider instance found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			ShipbreakersMain.SimToPresentationEventSystem.AddHandler<ResearchEvent>(new BBI.Core.Events.EventHandler<ResearchEvent>(this.OnResearchEvent));
			this.mEngineeringController.OnShowEngineeringTooltip += this.OnShowResearchTooltip;
			this.mUpgradesController.OnShowUpgradeTooltip += this.OnShowResearchTooltip;
			this.selectedCarrierCommanderId = null;
		}

		protected override void OnSessionEnded()
		{
			this.ToggleUpgradesPanel(true);
			if (this.mInterfaceController != null)
			{
				this.mInterfaceController.NewFrameFromSim -= this.OnNewStateFrame;
				this.mInterfaceController.NewSelection -= this.OnNewSelection;
				this.mInterfaceController.NewLeadUnitSelected -= this.OnNewLeadUnitSelected;
				this.mInterfaceController = null;
			}
			ShipbreakersMain.SimToPresentationEventSystem.RemoveHandler<ResearchEvent>(new BBI.Core.Events.EventHandler<ResearchEvent>(this.OnResearchEvent));
			this.ShowHidePanels(false);
			if (this.mEngineeringController != null)
			{
				this.mEngineeringController.OnShowEngineeringTooltip -= this.OnShowResearchTooltip;
				this.mEngineeringController.Dispose();
				this.mEngineeringController = null;
			}
			if (this.mUpgradesController != null)
			{
				this.mUpgradesController.OnShowUpgradeTooltip -= this.OnShowResearchTooltip;
				this.mUpgradesController.Dispose();
				this.mUpgradesController = null;
			}
			this.mMostRecentLocalCommanderState = null;
			this.mInteractionProvider = null;
			this.mCommanderManager = null;
		}

		public void ToggleUpgradesPanel(bool value)
		{
			if (this.mUpgradesController != null)
			{
				this.mUpgradesController.TogglePanel(value);
				return;
			}
			Log.Error(Log.Channel.UI, "Could not Toggle Upgrades Panel in {0} because the UpgradesController is NULL!", new object[]
			{
				base.GetType()
			});
		}

		public bool PanelIsActive()
		{
			if (this.mUpgradesController != null)
			{
				return this.mUpgradesController.PanelIsActive();
			}
			Log.Error(Log.Channel.UI, "Could not determine if Upgrades panel is active in {0} because UpgradesController is NULL!", new object[]
			{
				base.GetType()
			});
			return true;
		}

		private void ShowHidePanels(bool show)
		{
			if (this.mEngineeringController != null)
			{
				this.mEngineeringController.ShowHidePanel(show);
			}
			if (this.mUpgradesController != null)
			{
				this.mUpgradesController.ShowHidePanel(show);
			}
		}

		private void OnNewStateFrame(SimStateFrame stateFrame)
		{
			if (ShipbreakersMain.ReplayMode != BBI.Game.Replay.ReplayMode.ReplayingGame || !this.selectedCarrierCommanderId.HasValue)
			{
				this.mMostRecentLocalCommanderState = stateFrame.FindCommanderState(this.mCommanderManager.LocalCommanderID);
			}
			else
			{
				this.mMostRecentLocalCommanderState = stateFrame.FindCommanderState(this.selectedCarrierCommanderId.Value);
			}
			if (this.mMostRecentLocalCommanderState.LockedResearch != null)
			{
				int num = this.mMostRecentLocalCommanderState.LockedResearch.Length;
				if (this.mNumberOfLockedResearchItems != num)
				{
					this.mNumberOfLockedResearchItems = num;
					this.mResearchTreeDirty = true;
				}
			}
			if (this.ArePanelsShowing)
			{
				this.UpdatePanels(this.mMostRecentLocalCommanderState, this.mResearchTreeDirty);
				if (this.mResearchTreeDirty)
				{
					this.mResearchTreeDirty = false;
				}
			}
		}

		private void OnNewSelection(IList<Entity> newSelection, SimStateFrame stateFrame)
		{
			Profiler.BeginSample("ResearchPanelsController.OnSelectionChanged");
			bool show = false;
			bool panelsForeign = false;
			if (!newSelection.IsNullOrEmpty<Entity>() && stateFrame != null && this.mInterfaceController != null)
			{
				UnitState unitState = stateFrame.FindObject<UnitState>(this.mInterfaceController.LeadUnit);
				if (unitState != null && this.mInteractionProvider.LocalCommanderCanControl(unitState.OwnerCommander))
				{
					UnitAttributes entityTypeAttributes = ShipbreakersMain.GetEntityTypeAttributes<UnitAttributes>(unitState.TypeID);
					if (entityTypeAttributes != null && (entityTypeAttributes.Class & UnitClass.Carrier) != UnitClass.None && entityTypeAttributes.Controllable)
					{
						if (ShipbreakersMain.ReplayMode == ReplayMode.ReplayingGame)
						{
							show = true;
							CommanderID ownerCommander = unitState.OwnerCommander;
							if (!this.selectedCarrierCommanderId.HasValue || this.selectedCarrierCommanderId.Value != ownerCommander)
							{
								panelsForeign = true;
								this.selectedCarrierCommanderId = new CommanderID?(ownerCommander);
								this.mMostRecentLocalCommanderState = stateFrame.FindCommanderState(ownerCommander);
							}
						}
						else if (this.mInteractionProvider.LocalCommanderCanControl(unitState.OwnerCommander))
						{
							show = true;
						}
					}
				}
			}
			bool arePanelsShowing = this.ArePanelsShowing;
			this.ShowHidePanels(show);
			if (show)
			{
				this.UpdatePanels(this.mMostRecentLocalCommanderState, (panelsForeign ? true : !arePanelsShowing));
			}
			Profiler.EndSample();
		}

		private void OnNewLeadUnitSelected(Entity selection)
		{
			this.OnNewSelection(new Entity[]
			{
				selection
			}, ShipbreakersMain.CurrentSimFrame);
		}

		private void OnResearchEvent(ResearchEvent ev)
		{
			if (this.mMostRecentLocalCommanderState == null || ev.Commander != this.mMostRecentLocalCommanderState.CommanderID)
			{
				return;
			}
			switch (ev.Reason)
			{
			case ResearchEvent.EventReason.Started:
			case ResearchEvent.EventReason.Cancelled:
				this.UpdatePanels(this.mMostRecentLocalCommanderState, false);
				return;
			case ResearchEvent.EventReason.Completed:
				this.mResearchTreeDirty = true;
				return;
			default:
				Log.Error(Log.Channel.UI, "{0}: Unhandled reason {1} in ResearchEvent event handler!", new object[]
				{
					base.GetType(),
					ev.Reason
				});
				return;
			}
		}

		private void OnShowResearchTooltip(ResearchButtonController controller, TooltipObjectAsset tooltipObject, Transform anchor, bool onHover)
		{
			if (!onHover)
			{
				UISystem.HideTooltip();
				return;
			}
			ResearchItemAttributes researchItem = controller.ResearchItem;
			if (researchItem != null && tooltipObject != null && anchor != null)
			{
				this.mTooltipData.Reset();
				this.mTooltipData.LocalizedTitleStringID = researchItem.LocalizedResearchTitleStringID;
				this.mTooltipData.LocalizedShortDescriptionStringID = researchItem.LocalizedShortDescriptionStringID;
				this.mTooltipData.LocalizedLongDescriptionStringID = researchItem.LocalizedLongDescriptionStringID;
				this.mTooltipData.ResourceCost1 = researchItem.Resource1Cost;
				this.mTooltipData.ResourceCost2 = researchItem.Resource2Cost;
				this.mTooltipData.TimeCost = Fixed64.IntValue(researchItem.ResearchTime);
				bool flag = false;
				foreach (ActiveResearchState activeResearchState in this.mMostRecentLocalCommanderState.ActiveResearchItems)
				{
					if (activeResearchState.ResearchItemName == researchItem.Name)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					TooltipAttributes hudskinAttributes = this.mHUDSystem.GetHUDSkinAttributes<TooltipAttributes>();
					if (hudskinAttributes != null)
					{
						ResearchHelperShared.ResearchPrereqCheckResult researchPrereqCheckResult;
						ResearchHelperShared.CanResearchItem(this.mMostRecentLocalCommanderState, researchItem, out researchPrereqCheckResult);
						ResearchHelperShared.ResearchPrereqCheckResult researchPrereqCheckResult2 = researchPrereqCheckResult;
						if (researchPrereqCheckResult2 != ResearchHelperShared.ResearchPrereqCheckResult.AnotherResearchInProgress)
						{
							if (researchPrereqCheckResult2 != ResearchHelperShared.ResearchPrereqCheckResult.InsufficientResource1)
							{
								if (researchPrereqCheckResult2 == ResearchHelperShared.ResearchPrereqCheckResult.InsufficientResource2)
								{
									this.mTooltipData.Cost2NotMet = hudskinAttributes.Cost2NotMetLocId;
								}
							}
							else
							{
								this.mTooltipData.Cost1NotMet = hudskinAttributes.Cost1NotMetLocId;
							}
						}
						else
						{
							this.mTooltipData.ResearchAlreadyInProgress = hudskinAttributes.ResearchAlreadyInProgressLocId;
						}
					}
				}
				UISystem.ShowTooltip(new object[]
				{
					this.mTooltipData
				}, tooltipObject, anchor);
				return;
			}
			Log.Warn(Log.Channel.UI, "Research Item Not Found for tooltip info {0}", new object[]
			{
				researchItem.Name
			});
		}

		public static void ScheduleStartResearchCommand(ICommandScheduler commandScheduler, CommanderID commanderID, ResearchItemAttributes researchItem)
		{
			SimCommandBase simCommandBase = SimCommandFactory.CreateStartResearchCommand(commanderID, researchItem);
			if (!commandScheduler.ScheduleCommand(simCommandBase))
			{
				StartResearchCommand startResearchCommand = simCommandBase as StartResearchCommand;
				ResearchHelperShared.ResearchPrereqCheckResult validationResult = startResearchCommand.ValidationResult;
				ShipbreakersMain.PresentationEventSystem.Post(new ResearchDeniedEvent(researchItem.Name, validationResult, commanderID));
			}
		}

		private void UpdatePanels(CommanderState commanderState, bool rebuild)
		{
			if (this.mEngineeringController != null)
			{
				this.mEngineeringController.Update(commanderState, rebuild);
			}
			if (this.mUpgradesController != null)
			{
				this.mUpgradesController.Update(commanderState, rebuild);
			}
		}

		public ResearchPanelsController()
		{
		}

		[SerializeField]
		private UpgradesPanelController.UpgradesPanelSettings m_UpgradesPanelSettings = new UpgradesPanelController.UpgradesPanelSettings();

		[SerializeField]
		private UpgradesPanelController.UpgradesPanelSettings m_UpgradesPanelSettingsSP = new UpgradesPanelController.UpgradesPanelSettings();

		[SerializeField]
		private EngineeringPanelController.EngineeringPanelSettings m_EngineeringPanelSettings = new EngineeringPanelController.EngineeringPanelSettings();

		private UnitInterfaceController mInterfaceController;

		private CommanderState mMostRecentLocalCommanderState;

		private ICommanderInteractionProvider mInteractionProvider;

		private ICommanderManager mCommanderManager;

		private EngineeringPanelController mEngineeringController;

		private UpgradesPanelController mUpgradesController;

		private bool mResearchTreeDirty;

		private int mNumberOfLockedResearchItems = -1;

		private ResearchTooltipData mTooltipData = default(ResearchTooltipData);

		private CommanderID? selectedCarrierCommanderId = null;
	}
}
