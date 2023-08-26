using System;
using System.Collections.Generic;
using System.Diagnostics;
using BBI.Core;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Game.Simulation;
using BBI.Unity.Core.DLC;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.DLC;
using BBI.Unity.Game.World;
using UnityEngine;

namespace BBI.Unity.Game.Utility
{
	public static class SpawnAssigner
	{
		public static bool GenerateSpawnPoints(LevelDefinition selectedLevel, Dictionary<CommanderID, TeamID> commanderTeamIDs, TeamSetting teamSetting, out List<CommanderSpawnInfo> commanderSpawnInfo)
		{
			if (!SpawnAssigner.TryToAssignSpawnInfo(selectedLevel.SceneSpawnInfo.GetComponent<SceneSpawnInfo>(), teamSetting, commanderTeamIDs, out commanderSpawnInfo))
			{
				Log.Error(Log.Channel.Online, "Unable to create commander spawn points!", new object[0]);
				return false;
			}
			return true;
		}

		public static Dictionary<CommanderID, PlayerSelection> GeneratePlayerSelections(CommanderAttributes[] commanderAttributes, List<CommanderDescription> commanderDescriptions, List<CommanderSpawnInfo> commanderSpawnInfo, DLCManager dlcMan)
		{
			Dictionary<CommanderID, PlayerSelection> dictionary = new Dictionary<CommanderID, PlayerSelection>(commanderDescriptions.Count);
			for (int i = 0; i < commanderDescriptions.Count; i++)
			{
				CommanderDescription commanderDescription = commanderDescriptions[i];
				int factionIndex = commanderDescription.FactionIndex;
				DLCPackID selectedUnitSkinPack = commanderDescription.SelectedUnitSkinPack;
				CommanderAttributes commanderAttributes2 = null;
				DLCPackDescriptor dlcpackDescriptor = dlcMan.GetDLCPackDescriptor(selectedUnitSkinPack);
				if (selectedUnitSkinPack != DLCPackID.kInvalidID && dlcpackDescriptor != null && dlcpackDescriptor.DLCPackType == DLCType.Faction)
				{
					UnitFactionDLCPack unitFactionDLCPack = dlcMan.GetDLCPackHeader(selectedUnitSkinPack) as UnitFactionDLCPack;
					if (unitFactionDLCPack != null)
					{
						commanderAttributes2 = unitFactionDLCPack.CommanderAttrs;
					}
				}
				if (commanderAttributes2 == null)
				{
					commanderAttributes2 = commanderAttributes[factionIndex];
				}
				int num = SpawnAssigner.FindCommanderSpawnInfo(commanderDescription.CommanderID, commanderSpawnInfo);
				if (num != -1)
				{
					commanderDescription.TeamID = commanderSpawnInfo[num].TeamID;
					commanderDescription.SpawnIndex = commanderSpawnInfo[num].SpawnIndex;
				}
				else
				{
					Log.Error(Log.Channel.Gameplay, "[JZ] CommanderID {0} does not have a spawn point! Defaulting to 0...", new object[]
					{
						commanderDescription.CommanderID
					});
					commanderDescription.SpawnIndex = 0;
				}
				string text = (commanderDescription.PlayerType == PlayerType.Human) ? commanderDescription.NetworkPlayerID.ToString() : commanderDescription.LocalizedName;
				Log.Info(Log.Channel.Gameplay, "Player '{0}' assigned commander '{1}' assigned '{2}', faction '{3}' (skin '{4}') and spawn point '{5}'.", new object[]
				{
					text,
					commanderDescription.CommanderID,
					commanderDescription.TeamID,
					factionIndex,
					selectedUnitSkinPack,
					commanderDescription.SpawnIndex
				});
				CommanderDescription desc = commanderDescription;
				dictionary.Add(commanderDescription.CommanderID, new PlayerSelection(commanderAttributes2, desc));
			}
			return dictionary;
		}

		public static SceneSpawnInfo.PlayerSpawnInfo[] FlattenSceneSpawnInfo(SceneSpawnInfo sceneSpawnInfo, TeamSetting teamSetting)
		{
			if (sceneSpawnInfo == null)
			{
				Log.Error(Log.Channel.Gameplay, "[JZ] passing in null sceneSpawnInfo to FlattenSceneSpawnInfo", new object[0]);
				return null;
			}
			List<SceneSpawnInfo.PlayerSpawnInfo> list = new List<SceneSpawnInfo.PlayerSpawnInfo>(6);
			if (teamSetting != TeamSetting.Team)
			{
				if (teamSetting != TeamSetting.FFA)
				{
					Log.Error(Log.Channel.Gameplay, "Unsupported TeamSetting {0} in FlattenSceneSpawnInfo!", new object[]
					{
						teamSetting
					});
				}
				else if (sceneSpawnInfo.FFASpawnInfos != null)
				{
					for (int i = 0; i < sceneSpawnInfo.FFASpawnInfos.Length; i++)
					{
						SceneSpawnInfo.PlayerSpawnInfo playerSpawnInfo = sceneSpawnInfo.FFASpawnInfos[i];
						if (playerSpawnInfo != null)
						{
							list.Add(playerSpawnInfo);
						}
					}
				}
				else
				{
					Log.Error(Log.Channel.Gameplay, "No FFASpawnInfos attached to sceneSpawnInfo", new object[0]);
				}
			}
			else if (sceneSpawnInfo.TeamSpawnInfos != null)
			{
				for (int j = 0; j < sceneSpawnInfo.TeamSpawnInfos.Length; j++)
				{
					SceneSpawnInfo.TeamSpawnInfo teamSpawnInfo = sceneSpawnInfo.TeamSpawnInfos[j];
					if (teamSpawnInfo != null && !teamSpawnInfo.PlayerSpawnInfos.IsNullOrEmpty<SceneSpawnInfo.PlayerSpawnInfo>())
					{
						list.AddRange(teamSpawnInfo.PlayerSpawnInfos);
					}
				}
			}
			else
			{
				Log.Error(Log.Channel.Gameplay, "No TeamSpawnInfos attached to sceneSpawnInfo", new object[0]);
			}
			return list.ToArray();
		}

		private static bool TryToAssignSpawnInfo(SceneSpawnInfo sceneSpawnInfo, TeamSetting teamSetting, Dictionary<CommanderID, TeamID> commanderTeamIDs, out List<CommanderSpawnInfo> assignedSpawnInfo)
		{
			assignedSpawnInfo = new List<CommanderSpawnInfo>();
			SpawnAttributes[] array = SpawnAssigner.FlattenSceneSpawnInfo(sceneSpawnInfo, teamSetting);
			SpawnAttributes[] flattenedSpawnPoints = array;
			Dictionary<TeamID, int[]> dictionary = SpawnAssigner.GenerateTeamSpawnIndices(sceneSpawnInfo, flattenedSpawnPoints);
			if (dictionary == null)
			{
				Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Failed to generate team spawn indices. Unable to assign spawn points!", new object[0]);
				return false;
			}
			if (teamSetting != TeamSetting.Team)
			{
				if (teamSetting != TeamSetting.FFA)
				{
					Log.Error(Log.Channel.Gameplay, "Unsupported TeamSetting {0}! Unable to generate spawn points!", new object[]
					{
						teamSetting.ToString()
					});
					return false;
				}
				if (!SpawnAssigner.AssignFFASpawnPoints(dictionary, commanderTeamIDs, SpawnAssigner.AssignType.Random, out assignedSpawnInfo))
				{
					return false;
				}
			}
			else if (!SpawnAssigner.AssignTeamSpawnPoints(dictionary, commanderTeamIDs, SpawnAssigner.AssignType.Random, SpawnAssigner.AssignType.Random, out assignedSpawnInfo))
			{
				return false;
			}
			return true;
		}

		private static Dictionary<TeamID, int[]> GenerateTeamSpawnIndices(SceneSpawnInfo sceneSpawnInfo, SpawnAttributes[] flattenedSpawnPoints)
		{
			if (sceneSpawnInfo == null)
			{
				Log.Error(Log.Channel.Gameplay, "[JZ] passing in null sceneSpawnInfo to GenerateSpawnIndicesMap", new object[0]);
				return null;
			}
			if (flattenedSpawnPoints.IsNullOrEmpty<SpawnAttributes>())
			{
				Log.Error(Log.Channel.Gameplay, "Passing in null or empty flattenedSpawnPoints to GenerateSpawnIndicesMap", new object[0]);
				return null;
			}
			Dictionary<TeamID, int[]> dictionary = new Dictionary<TeamID, int[]>(6);
			int[] array = new int[flattenedSpawnPoints.Length];
			for (int i = 0; i < flattenedSpawnPoints.Length; i++)
			{
				array[i] = i;
			}
			dictionary[TeamID.None] = array;
			int num = 0;
			for (int j = 0; j < sceneSpawnInfo.TeamSpawnInfos.Length; j++)
			{
				int num2 = sceneSpawnInfo.TeamSpawnInfos[j].PlayerSpawnInfos.Length;
				int[] array2 = new int[num2];
				for (int k = 0; k < array2.Length; k++)
				{
					array2[k] = num + k;
				}
				num += num2;
				dictionary.Add(new TeamID(j + 1), array2);
			}
			return dictionary;
		}

		private static bool AssignTeamSpawnPoints(Dictionary<TeamID, int[]> spawnIndicies, Dictionary<CommanderID, TeamID> commanderTeamIDs, SpawnAssigner.AssignType randomTeamSpawn, SpawnAssigner.AssignType randomInTeamSpawn, out List<CommanderSpawnInfo> commanderSpawnInfo)
		{
			commanderSpawnInfo = new List<CommanderSpawnInfo>();
			List<TeamID> list = new List<TeamID>(spawnIndicies.Count);
			foreach (TeamID item in spawnIndicies.Keys<TeamID, int[]>())
			{
				list.Add(item);
			}
			list.Remove(TeamID.None);
			if (list.Count < 2)
			{
				Log.Error(Log.Channel.Gameplay, "We have less than 2 teams assigned to players.  Failed to set spawn points!", new object[0]);
				return false;
			}
			if (randomTeamSpawn == SpawnAssigner.AssignType.Random)
			{
				list = SpawnAssigner.Shuffle<TeamID>(list);
			}
			List<CommanderID> list2 = new List<CommanderID>(3);
			List<CommanderID> list3 = new List<CommanderID>(3);
			int num = commanderTeamIDs.Count / 2;
			foreach (KeyValuePair<CommanderID, TeamID> keyValuePair in commanderTeamIDs)
			{
				TeamID value = keyValuePair.Value;
				if (value == SpawnAssigner.kTeamID1)
				{
					list2.Add(keyValuePair.Key);
				}
				else if (value == SpawnAssigner.kTeamID2)
				{
					list3.Add(keyValuePair.Key);
				}
			}
			foreach (KeyValuePair<CommanderID, TeamID> keyValuePair2 in commanderTeamIDs)
			{
				if (keyValuePair2.Value == TeamID.None)
				{
					if (list2.Count >= num)
					{
						list3.Add(keyValuePair2.Key);
					}
					else if (list3.Count >= num)
					{
						list2.Add(keyValuePair2.Key);
					}
					else if (SpawnAssigner.RandRange(0, 2) == 0)
					{
						list2.Add(keyValuePair2.Key);
					}
					else
					{
						list3.Add(keyValuePair2.Key);
					}
				}
			}
			return SpawnAssigner.AssignSpawnIndicesForTeam(spawnIndicies, list[0], SpawnAssigner.kTeamID1, list2, randomInTeamSpawn, ref commanderSpawnInfo) && SpawnAssigner.AssignSpawnIndicesForTeam(spawnIndicies, list[1], SpawnAssigner.kTeamID2, list3, randomInTeamSpawn, ref commanderSpawnInfo);
		}

		private static bool AssignSpawnIndicesForTeam(Dictionary<TeamID, int[]> spawnIndicies, TeamID teamSpawnGroup, TeamID targetTeamID, List<CommanderID> commanders, SpawnAssigner.AssignType randomInTeamSpawn, ref List<CommanderSpawnInfo> output)
		{
			int[] array;
			if (spawnIndicies.TryGetValue(teamSpawnGroup, out array))
			{
				if (randomInTeamSpawn == SpawnAssigner.AssignType.Random)
				{
					array = SpawnAssigner.Shuffle<int>(array);
				}
				for (int i = 0; i < commanders.Count; i++)
				{
					CommanderID commanderID = commanders[i];
					if (i >= array.Length)
					{
						Log.Error(Log.Channel.Gameplay, "Failed to initialize spawn point for commander {0}. Not enough spawn points in team spawn group {1} for team {2}!", new object[]
						{
							commanderID,
							teamSpawnGroup.ID - 1,
							targetTeamID
						});
						return false;
					}
					output.Add(new CommanderSpawnInfo(commanderID, targetTeamID, array[i]));
				}
			}
			return true;
		}

		private static bool AssignFFASpawnPoints(Dictionary<TeamID, int[]> teamSpawnIndices, Dictionary<CommanderID, TeamID> commanderTeamIDs, SpawnAssigner.AssignType assignType, out List<CommanderSpawnInfo> commanderSpawnInfo)
		{
			commanderSpawnInfo = new List<CommanderSpawnInfo>(commanderTeamIDs.Count);
			int[] array;
			if (!teamSpawnIndices.TryGetValue(TeamID.None, out array) || array.IsNullOrEmpty<int>())
			{
				Log.Error(Log.Channel.Gameplay, "No FFA spawn points specified in the map!", new object[0]);
				return false;
			}
			if (array.Length < commanderTeamIDs.Count)
			{
				Log.Error(Log.Channel.Gameplay, "Failed to assign spawn points because the map does not contain enough spawn points!", new object[0]);
				return false;
			}
			foreach (KeyValuePair<CommanderID, TeamID> keyValuePair in commanderTeamIDs)
			{
				if (keyValuePair.Value != TeamID.None)
				{
					Log.Info(Log.Channel.Gameplay, "Commander {0} TeamID {1} will be reassigned in FFA mode.", new object[]
					{
						keyValuePair.Key,
						keyValuePair.Value
					});
				}
			}
			List<CommanderID> list = new List<CommanderID>(commanderTeamIDs.Count);
			foreach (CommanderID item in commanderTeamIDs.Keys<CommanderID, TeamID>())
			{
				list.Add(item);
			}
			if (assignType == SpawnAssigner.AssignType.Random)
			{
				array = SpawnAssigner.Shuffle<int>(array);
				list = SpawnAssigner.Shuffle<CommanderID>(list);
			}
			for (int i = 0; i < list.Count; i++)
			{
				commanderSpawnInfo.Add(new CommanderSpawnInfo(list[i], new TeamID(i + 1), array[i]));
			}
			return true;
		}

		private static T[] Shuffle<T>(T[] array)
		{
			T[] array2 = (T[])array.Clone();
			for (int i = array2.Length - 1; i > 0; i--)
			{
				int num = SpawnAssigner.RandRange(0, i);
				T t = array2[i];
				array2[i] = array2[num];
				array2[num] = t;
			}
			return array2;
		}

		private static List<T> Shuffle<T>(List<T> list)
		{
			List<T> list2 = new List<T>(list);
			for (int i = list2.Count - 1; i > 0; i--)
			{
				int index = SpawnAssigner.RandRange(0, i);
				T value = list2[i];
				list2[i] = list2[index];
				list2[index] = value;
			}
			return list2;
		}

		private static int RandRange(int min, int max)
		{
			return UnityEngine.Random.Range(min, max);
		}

		[Conditional("DEBUG")]
		private static void AssertSpawnInfoIsValid(SceneSpawnInfo sceneSpawnInfo)
		{
			for (int i = 0; i < sceneSpawnInfo.TeamSpawnInfos.Length; i++)
			{
			}
			for (int j = 0; j < sceneSpawnInfo.FFASpawnInfos.Length; j++)
			{
			}
		}

		private static int FindCommanderSpawnInfo(CommanderID commanderID, List<CommanderSpawnInfo> spawnInfo)
		{
			for (int i = 0; i < spawnInfo.Count; i++)
			{
				if (spawnInfo[i].CommanderID == commanderID)
				{
					return i;
				}
			}
			return -1;
		}

		static SpawnAssigner()
		{
		}

		private const int kInvalidCommanderSpawnIndex = -1;

		public static readonly TeamID kTeamID1 = new TeamID(1);

		public static readonly TeamID kTeamID2 = new TeamID(2);

		private enum AssignType
		{
			NotRandom,
			Random
		}
	}
}
