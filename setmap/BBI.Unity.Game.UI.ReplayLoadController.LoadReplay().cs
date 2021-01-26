using System;
using System.Collections.Generic;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Game.Replay;
using BBI.Game.Simulation;
using BBI.Steam;
using BBI.Unity.Core.DLC;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.DLC;
using BBI.Unity.Game.Events;
using BBI.Unity.Game.HUD;
using BBI.Unity.Game.Utility;
using BBI.Unity.Game.World;
using UnityEngine;
using System.Text;

namespace BBI.Unity.Game.UI
{
	// Token: 0x020001EC RID: 492
	public sealed partial class ReplayLoadController : BlackbirdModalPanelBase
	{
		// Token: 0x06000B87 RID: 2951 RVA: 0x0004090C File Offset: 0x0003EB0C
		private void LoadReplay()
		{
			LevelDefinition levelDefinition = this.mLevelManager.FindLevelFromSceneName(this.mSelectedReplay.SceneName, this.mSelectedReplay.GameSessionSettings.GameMode);
			if (levelDefinition != null)
			{
				// MOD: tell the mod what map is being loaded
				try {
					byte[] replayX = System.IO.File.ReadAllBytes(mSelectedReplay.FilePath + "x");
					int layoutLength = (replayX[0] << 0) + (replayX[1] << 8) + (replayX[2] << 16) + (replayX[3] << 24);
					int patchLength = (replayX[4] << 0) + (replayX[5] << 8) + (replayX[6] << 16) + (replayX[7] << 24);
					int reserved0 = (replayX[8] << 0) + (replayX[9] << 8) + (replayX[10] << 16) + (replayX[11] << 24);
					int reserved1 = (replayX[12] << 0) + (replayX[13] << 8) + (replayX[14] << 16) + (replayX[15] << 24);
					MapModManager.MapXml = Encoding.UTF8.GetString(replayX, 16, layoutLength);
					Subsystem.AttributeLoader.PatchOverrideData = Encoding.UTF8.GetString(replayX, 16 + layoutLength, patchLength);
				} catch {}
				Dictionary<CommanderID, TeamID> teams = new Dictionary<CommanderID, TeamID>();
				foreach (var tuple in mSelectedReplay.SessionPlayers) {
					teams[new CommanderID(tuple.CID)] = new TeamID(tuple.TeamID);
				}
				MapModManager.SetMap(levelDefinition, mSelectedReplay.GameSessionSettings.GameMode, mSelectedReplay.GameSessionSettings.TeamSetting, teams);
				// MOD
				
				Dictionary<CommanderID, PlayerSelection> dictionary = new Dictionary<CommanderID, PlayerSelection>(this.mSelectedReplay.SessionPlayers.Length);
				List<NetworkPlayerID> list = new List<NetworkPlayerID>(this.mSelectedReplay.SessionPlayers.Length);
				for (int i = 0; i < this.mSelectedReplay.SessionPlayers.Length; i++)
				{
					ReplayHelpers.ReplayDataTuple replayDataTuple = this.mSelectedReplay.SessionPlayers[i];
					CommanderAttributes commanderAttributes = null;
					foreach (DLCAssetBundleBase dlcassetBundleBase in this.mDLCManager.GetAllLoadedHeadersOfType(DLCType.Faction))
					{
						UnitFactionDLCPack unitFactionDLCPack = dlcassetBundleBase as UnitFactionDLCPack;
						if (unitFactionDLCPack != null && unitFactionDLCPack.CommanderAttrs != null && unitFactionDLCPack.CommanderAttrs.Name == replayDataTuple.CommanderAttributesName)
						{
							commanderAttributes = unitFactionDLCPack.CommanderAttrs;
							break;
						}
					}
					int j = 0;
					if (commanderAttributes == null)
					{
						for (j = 0; j < this.mLevelManager.CommanderAttributes.Length; j++)
						{
							CommanderAttributes commanderAttributes2 = this.mLevelManager.CommanderAttributes[j];
							if (commanderAttributes2.Name == replayDataTuple.CommanderAttributesName)
							{
								commanderAttributes = commanderAttributes2;
								break;
							}
						}
					}
					if (commanderAttributes == null)
					{
						Log.Error(Log.Channel.Data, "Couldn't resolve commander attributes from saved replay.  The replay that was saved didn't have commander attributes that match the commander attributes in the level ({0})", new object[]
						{
							replayDataTuple.CommanderAttributesName
						});
					}
					else
					{
						CommanderDescription commanderDescription = new CommanderDescription(replayDataTuple.PlayerName, new CommanderID(replayDataTuple.CID), replayDataTuple.PlayerID, replayDataTuple.SpawnIndex, j, new TeamID(replayDataTuple.TeamID), replayDataTuple.PlayerType, replayDataTuple.UnitColors, replayDataTuple.AIDifficulty, replayDataTuple.UnitSkinPack, replayDataTuple.RandomFaction);
						dictionary.Add(commanderDescription.CommanderID, new PlayerSelection(commanderAttributes, commanderDescription));
						if (replayDataTuple.PlayerID != NetworkPlayerID.kInvalidID)
						{
							list.Add(replayDataTuple.PlayerID);
						}
					}
				}
				DependencyContainer<GameStartSettings, ReplayHelpers.ReplayableGameSessionHeader, SessionBase> dependencies = new DependencyContainer<GameStartSettings, ReplayHelpers.ReplayableGameSessionHeader, SessionBase>(new GameStartSettings(dictionary, this.mSelectedReplay.GameSessionSettings.VictoryConditions, this.mSelectedReplay.GameSessionSettings.TeamSetting, this.mSelectedReplay.GameSessionSettings.GameModeSettings, this.mSelectedReplay.RandomSeed, false, false, AutomatchSizeType.ThreeVSThree), this.mSelectedReplay, new NullSession(list));
				this.mLevelManager.LoadLevelForSave(levelDefinition, this.mSelectedReplay.GameSessionSettings.GameMode, ReplayMode.ReplayingGame, dependencies);
				return;
			}
			Log.Error(Log.Channel.Data, "The level associated with {0} could not be found", new object[]
			{
				this.mSelectedReplay.SceneName
			});
		}
	}
}
