// PopulateHostSpawnPointsAssignment gets called before level is loaded
// Mod needs to know what map is being played before spawn points are set

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
using BBI.Unity.Game.Utility;
using BBI.Unity.Game.World;
using UnityEngine;

namespace BBI.Unity.Game.UI.Frontend.Helpers
{
	// Token: 0x020001A2 RID: 418
	public abstract partial class LobbySetupPanelBase : BlackbirdPanelBase
	{
		// Token: 0x06000914 RID: 2324
		protected bool PopulateHostSpawnPointsAssignment(LevelDefinition selectedLevel)
		{
			Dictionary<CommanderID, TeamID> dictionary;
			TeamSetting teamSetting;
			if (!this.TryGetTeams(out dictionary, out teamSetting))
			{
				Log.Error(Log.Channel.Online, "Unable to get team entries for the commanders!", new object[0]);
				return false;
			}
			// MOD
			MapModManager.SetMap(selectedLevel, GameMode.Multiplayer, teamSetting, dictionary);
			// MOD
			if (dictionary.Count != this.mSharedLobbyData.CommanderCollection.TotalCommanderCount)
			{
				Log.Error(Log.Channel.Online, "There are not enough team entries for each commander ID! Unable to start the game!", new object[0]);
				return false;
			}
			List<CommanderSpawnInfo> commanderSpawnDetails;
			if (!SpawnAssigner.GenerateSpawnPoints(selectedLevel, dictionary, teamSetting, out commanderSpawnDetails))
			{
				Log.Error(Log.Channel.Online, "Unable to generate commander spawn points!", new object[0]);
				return false;
			}
			this.mSharedLobbyData.CommanderSpawnDetails = commanderSpawnDetails;
			Log.Info(Log.Channel.Online, "Player {0} sending spawn details to clients...", new object[]
			{
				this.mLobby.LocalPlayerID
			});
			return true;
		}
	}
}
