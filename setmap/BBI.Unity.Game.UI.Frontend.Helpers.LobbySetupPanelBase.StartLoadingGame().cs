using System;
using System.Collections;
using System.Collections.Generic;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Game.Events;
using BBI.Game.Replay;
using BBI.Game.Simulation;
using BBI.Steam;
using BBI.Unity.Core.Utility;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.DLC;
using BBI.Unity.Game.Events;
using BBI.Unity.Game.HUD;
using BBI.Unity.Game.World;
using UnityEngine;

namespace BBI.Unity.Game.UI.Frontend.Helpers
{
	// Token: 0x020001A2 RID: 418
	public abstract partial class LobbySetupPanelBase : BlackbirdPanelBase
	{
		// Token: 0x06000912 RID: 2322 RVA: 0x000378B0 File Offset: 0x00035AB0
		protected virtual bool StartLoadingGame()
		{
			Log.Info(Log.Channel.Online, "Player {0} starting game...", new object[]
			{
				this.mLobby.LocalPlayerID
			});
			if (this.m_ClanChatPanel != null)
			{
				this.m_ClanChatPanel.Leave();
			}
			this.StopAllLobbyCoroutines();
			LevelDefinition level = this.mLevelManager.FindLevelByIndex(this.mSharedLobbyData.SelectedSceneIndex, GameMode.Multiplayer);
			// MOD
			Dictionary<CommanderID, TeamID> commanderTeamIDs;
			TeamSetting teamSettings;
			TryGetTeams(out commanderTeamIDs, out teamSettings);
			MapModManager.SetMap(level, GameMode.Multiplayer, teamSettings, commanderTeamIDs);
			// MOD
			this.mLevelManager.StartLoadLevel(GameMode.Multiplayer, ReplayMode.RecordingGame, level, this.GetStartDependencies());
			return true;
		}
	}
}
