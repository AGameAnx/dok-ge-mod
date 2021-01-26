using System;
using System.Collections.Generic;
using BBI.Core.Events;
using BBI.Core.Utility;
using BBI.Game.Events;
using BBI.Game.SaveLoad;
using BBI.Game.Simulation;
using BBI.Unity.Game.Events;
using BBI.Unity.Game.HUD;
using BBI.Unity.Game.World;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	// Token: 0x02000105 RID: 261
	public partial class SceneEntityViewController : BlackbirdPanelBase
	{
		// Token: 0x06000EC8 RID: 3784 RVA: 0x00061750 File Offset: 0x0005F950
		protected override void OnSessionStarted()
		{
			if (this.m_Settings != null)
			{
				if (this.m_Settings.ObjectiveArrowMarkerPrefab == null)
				{
					Log.Error(Log.Channel.UI, "NULL ObjectiveArrowMarkerPrefab reference in settings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.m_Settings.ObjectiveCircleMarkerPrefab == null)
				{
					Log.Error(Log.Channel.UI, "NULL ObjectiveCircleMarkerPrefab reference in settings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.m_Settings.ObjectiveIconContainer == null)
				{
					Log.Error(Log.Channel.UI, "NULL ObjectiveIconContainer reference in settings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.m_Settings.ObjectiveThreatCircleMarkerPrefab == null)
				{
					Log.Error(Log.Channel.UI, "NULL ObjectiveThreatCircleMarkerPrefab reference in settings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.m_Settings.ObjectiveThreatArrowMarkerPrefab == null)
				{
					Log.Error(Log.Channel.UI, "NULL ObjectiveThreatArrowMarkerPrefab reference in settings for {0}", new object[]
					{
						base.GetType()
					});
				}
			}
			if (this.mHUDSystem == null)
			{
				Log.Error(Log.Channel.UI, "NULL HUDSystem instance found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			this.mResourceViewController = new ResourceViewController(this.mUnitInterfaceController, this.m_ResourceViewControllerSettings, base.GlobalDependencyContainer, base.SessionDependencies);
			this.mExtractionZoneViewController = new ExtractionZoneViewController(this.mUnitInterfaceController, this.m_ExtractionZoneViewControllerSettings, base.GlobalDependencyContainer, base.SessionDependencies);
			this.mRelicViewController = new RelicViewController(this.mUnitInterfaceController, this.m_RelicViewControllerSettings, base.GlobalDependencyContainer, base.SessionDependencies);

			// MOD: hack to make the instance of this class accessible
			MapModManager.SExtractionZoneViewController = this.mExtractionZoneViewController; // So that the entity event handler can be called
			// MOD
		}
	}
}
