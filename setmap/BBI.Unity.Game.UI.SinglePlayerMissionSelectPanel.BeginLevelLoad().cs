using System;
using System.Collections.Generic;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Game.Replay;
using BBI.Unity.Core.Utility;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Events;
using BBI.Unity.Game.HUD;
using BBI.Unity.Game.World;
using UnityEngine;
using BBI.Game.Simulation;

namespace BBI.Unity.Game.UI
{
	// Token: 0x02000322 RID: 802
	public partial class SinglePlayerMissionSelectPanel : BlackbirdPanelBase
	{
		// Token: 0x06001822 RID: 6178 RVA: 0x00083DB0 File Offset: 0x00081FB0
		private void BeginLevelLoad()
		{
			DependencyContainer<SessionBase> dependencies = new DependencyContainer<SessionBase>(new NullSession());
			this.mLevelToLoad.Difficulty = this.mSelectedDifficulty;
			if (this.m_UseDefaultFleet != null)
			{
				this.mLevelToLoad.CampaignPersistenceToggled = !this.m_UseDefaultFleet.value;
			}
			else
			{
				Log.Error(Log.Channel.Data, "The UseDefaultFleet toggle has become unhooked from the MissionSelect window - The default fleet will always be used!", new object[0]);
			}
			// MOD
			MapModManager.SetMap(mLevelToLoad, GameMode.SinglePlayer, TeamSetting.Invalid, new Dictionary<CommanderID, TeamID>());
			// MOD
			this.mLevelManager.StartLoadLevel(GameMode.SinglePlayer, ReplayMode.ReplaysDisabled, this.mLevelToLoad, dependencies);
		}
	}
}
