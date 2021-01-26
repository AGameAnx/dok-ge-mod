using System;
using System.Collections.Generic;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Game.Replay;
using BBI.Game.Simulation;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.DLC;
using BBI.Unity.Game.Network;
using BBI.Unity.Game.Utility;
using BBI.Unity.Game.World;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	// Token: 0x0200028C RID: 652
	public sealed partial class SkirmishSetupPanel : BlackbirdPanelBase
	{
		// Token: 0x06001279 RID: 4729 RVA: 0x000671F0 File Offset: 0x000653F0
		private void OnStartButtonPress()
		{
			
			if (m_LobbyView != null)
			{
				m_LobbyView.UnloadPreviewImageStreamedAssets();
				// MOD: tell the mod what level is being loaded
				MapModManager.SetMap(m_LobbyView.SelectedMap, GameMode.AISkirmish, m_LobbyView.ActiveTeamSetting, m_LobbyView.GetPlayerTeamIDs());
				// MOD
				mLevelManager.StartLoadLevel(GameMode.AISkirmish, ReplayMode.RecordingGame, m_LobbyView.SelectedMap, GetStartDependencies());
				return;
			}
			Log.Error(Log.Channel.UI, "Cannot start game. No lobby view specified!", new object[0]);
		}
	}
}
