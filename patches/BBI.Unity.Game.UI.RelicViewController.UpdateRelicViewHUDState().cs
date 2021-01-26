using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Commands;
using BBI.Game.Data;
using BBI.Game.Events;
using BBI.Game.Simulation;
using BBI.Unity.Game.Events;
using BBI.Unity.Game.HUD;
using BBI.Unity.Game.Localize;
using BBI.Unity.Game.World;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	// Token: 0x020001A8 RID: 424
	public partial class RelicViewController : IDisposable
	{
		// Token: 0x060010BE RID: 4286
		private void UpdateRelicViewHUDState(CollectibleEntityView view)
		{
			// MOD: remove artifact views that are moved off the map
			if (view.Entity.GetComponent(10).Position2D.X > Fixed64.FromInt(100000))
			{
				this.DespawnAndRemoveRelicView(view.Entity, view);
				return;
			}
			// MOD
			
			SimStateFrame currentSimFrame = ShipbreakersMain.CurrentSimFrame;
			if (currentSimFrame == null)
			{
				return;
			}
			RelicState relicState = currentSimFrame.FindObject<RelicState>(view.Entity);
			if (relicState == null)
			{
				return;
			}
			CollectibleState collectibleState = currentSimFrame.FindObject<CollectibleState>(view.Entity);
			if (collectibleState == null)
			{
				return;
			}
			DetectionState visibility = relicState.Visibility;
			if (ReplayPanelController.RevealAll || SimController.sSpectatorModeEnabled)
			{
				visibility = DetectionState.Sensed;
			}
			bool flag = relicState.HasTimer && relicState.TimerDirection == TimerDirection.Countup && !relicState.TimerCompleted;
			if (collectibleState.AvailableForCollection)
			{
				this.UpdateRelicDefaultIconSprite(view, visibility, flag);
			}
			else
			{
				this.UpdateRelicAttachedIconSprite(view, DetectionState.Sensed, false);
			}
			this.UpdateRelicIconTrackOnScreenEdge(view, !flag);
			bool flag2 = view.ShouldShowIcon();
			view.ShowIcon(flag2);
			if (!flag2 && this.mShowingTooltipForEntity != Entity.None && this.mShowingTooltipForEntity == view.Entity)
			{
				this.mShowingTooltipForEntity = Entity.None;
				UISystem.HideTooltip();
			}
		}
	}
}
