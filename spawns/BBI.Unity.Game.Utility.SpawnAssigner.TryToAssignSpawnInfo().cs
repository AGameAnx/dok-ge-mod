using System;
using System.Collections.Generic;
using System.Diagnostics;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Game.Simulation;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.DLC;
using BBI.Unity.Game.World;

namespace BBI.Unity.Game.Utility
{
	// Token: 0x0200036E RID: 878
	public static partial class SpawnAssigner
	{
		// Token: 0x06001C38 RID: 7224 RVA: 0x0009EA40 File Offset: 0x0009CC40
		private static bool TryToAssignSpawnInfo(SceneSpawnInfo sceneSpawnInfo, TeamSetting teamSetting, Dictionary<CommanderID, TeamID> commanderTeamIDs, out List<CommanderSpawnInfo> assignedSpawnInfo)
		{
			assignedSpawnInfo = new List<CommanderSpawnInfo>();
			SpawnAttributes[] flattenedSpawnPoints = SpawnAssigner.FlattenSceneSpawnInfo(sceneSpawnInfo, teamSetting);
			Dictionary<TeamID, int[]> dictionary = SpawnAssigner.GenerateTeamSpawnIndices(sceneSpawnInfo, flattenedSpawnPoints);
			if (dictionary == null)
			{
				Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Failed to generate team spawn indices. Unable to assign spawn points!", new object[0]);
				return false;
			}
			switch (teamSetting)
			{
			case TeamSetting.Team:
				// MOD
				Dictionary<TeamID, int[]> modifiedSpawns = new Dictionary<TeamID, int[]>();
				foreach(KeyValuePair<TeamID, int[]> entry in dictionary)
				{
					int[] spawns = entry.Value;
					if (entry.Key != TeamID.None) {
						spawns = new int[] { entry.Value[0], entry.Value[0], entry.Value[0] };
						entry.Value.CopyTo(spawns, 0);
					}
					modifiedSpawns[entry.Key] = spawns;
				}
				// Changed AssignType from Random to NotRandom
				if (!SpawnAssigner.AssignTeamSpawnPoints(modifiedSpawns, commanderTeamIDs, SpawnAssigner.AssignType.Random, SpawnAssigner.AssignType.NotRandom, out assignedSpawnInfo))
				// MOD
				{
					return false;
				}
				break;
			case TeamSetting.FFA:
				// MOD
				Dictionary<TeamID, int[]> modifiedSpawnsFFA = new Dictionary<TeamID, int[]>();
				foreach(KeyValuePair<TeamID, int[]> entry in dictionary)
				{
					int[] spawns = entry.Value;
					if (entry.Key == TeamID.None) {
						spawns = new int[] { entry.Value[0], entry.Value[0], entry.Value[0], entry.Value[0], entry.Value[0], entry.Value[0] };
						entry.Value.CopyTo(spawns, 0);
					}
					modifiedSpawnsFFA[entry.Key] = spawns;
				}
				// Changed AssignType from Random to NotRandom
				if (!SpawnAssigner.AssignFFASpawnPoints(modifiedSpawnsFFA, commanderTeamIDs, SpawnAssigner.AssignType.NotRandom, out assignedSpawnInfo))
				// MOD
				{
					return false;
				}
				break;
			default:
				Log.Error(Log.Channel.Gameplay, "Unsupported TeamSetting {0}! Unable to generate spawn points!", new object[]
				{
					teamSetting.ToString()
				});
				return false;
			}
			return true;
		}
	}
}
