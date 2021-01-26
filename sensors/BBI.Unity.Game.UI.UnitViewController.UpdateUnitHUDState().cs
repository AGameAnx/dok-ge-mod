using System;
using System.Collections.Generic;
using BBI.Core;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Events;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Commands;
using BBI.Game.Data;
using BBI.Game.Events;
using BBI.Game.Replay;
using BBI.Game.SaveLoad;
using BBI.Game.Simulation;
using BBI.Unity.Core.Rendering;
using BBI.Unity.Core.Utility;
using BBI.Unity.Core.World;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.DLC;
using BBI.Unity.Game.Events;
using BBI.Unity.Game.Events.UserInterface;
using BBI.Unity.Game.FX;
using BBI.Unity.Game.HUD;
using BBI.Unity.Game.World;
using PathologicalGames;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	// Token: 0x02000206 RID: 518
	public partial class UnitViewController : IDisposable
	{
		// Token: 0x06001491 RID: 5265 RVA: 0x00083F90 File Offset: 0x00082190
		private void UpdateUnitHUDState(UnitView view)
		{
			if (view.PresentationControlOnly)
			{
				if (view.SensorView != null)
				{
					view.SensorView.Enabled = false;
				}
				view.UnderlayActive = false;
				view.AltitudeIndicatorActive = false;
				return;
			}
			SimStateFrame currentSimFrame = ShipbreakersMain.CurrentSimFrame;
			if (currentSimFrame == null)
			{
				return;
			}
			UnitState nonInterpolatedTickState = view.NonInterpolatedTickState;
			if (nonInterpolatedTickState == null || nonInterpolatedTickState.IsDocked)
			{
				return;
			}
			CommanderRelationship relationship = this.mInteractionProvider.GetRelationship(this.mCommanderManager.LocalCommanderID, nonInterpolatedTickState.OwnerCommander);
			bool flag = relationship != CommanderRelationship.Self;
			bool flag2 = relationship == CommanderRelationship.Enemy;
			DetectionState detectionState = nonInterpolatedTickState.Visibility;
			if (ReplayPanelController.RevealAll || SimController.sSpectatorModeEnabled)
			{
				detectionState = DetectionState.Sensed;
			}
			bool flag3 = detectionState == DetectionState.Sensed;
			bool sTacticalOverlayEnabled = TacticalOverlayController.sTacticalOverlayEnabled;
			bool underlayActive = Cursor.visible && (view.Selected || view.IsHovered) && sTacticalOverlayEnabled && !this.mSensorsActive && flag3;
			view.UnderlayActive = underlayActive;
			bool altitudeIndicatorActive = (view.Selected || this.mSensorsActive || sTacticalOverlayEnabled) && flag3;
			view.AltitudeIndicatorActive = altitudeIndicatorActive;
			if (view.SensorView != null)
			{
				// MOD
				bool enabled = !view.IsDriverDead && flag3 && (!flag || !flag2 || 
				(ShipbreakersMain.ReplayMode == ReplayMode.ReplayingGame || 
				mCommanderManager.GetCommanderFromID(mCommanderManager.LocalCommanderID).CommanderAttributes.Name == "SPECTATOR")
				&& MapModManager.EnableEnemySensors); // Check if the player is spectating
				view.SensorView.IsFriendly = (relationship == CommanderRelationship.Self || relationship == CommanderRelationship.Ally);
				//MOD
				view.SensorView.Enabled = enabled;

				view.SensorView.Mode = (this.mSensorsActive ? UnitSensorView.SensorMode.SensorsView : UnitSensorView.SensorMode.GameView);
			}
			view.ForceIconOnlyMode = (this.mSensorsActive && view.ViewAttrs.LODTunings.ForceIconOnlyInSensors);
			UnitIconController iconControllerForView = this.GetIconControllerForView(view);
			if (iconControllerForView != null)
			{
				iconControllerForView.UpdateIconVisibilityAndSize(currentSimFrame);
			}
		}
	}
}
