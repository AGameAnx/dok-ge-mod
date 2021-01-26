using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Game.Replay;
using BBI.Game.Simulation;
using BBI.Unity.Core.Utility;
using BBI.Unity.Game.Audio;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.DLC;
using BBI.Unity.Game.Localize;
using UnityEngine;

namespace BBI.Unity.Game.World
{
	// Token: 0x02000074 RID: 116
	[Serializable]
	public partial class LevelManager : ResettableSubsystemBase
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060003C0 RID: 960
		public LevelDefinition[] LevelEntriesMP
		{
			get
			{
				// MOD: add new levels to the list of levels
				LevelDefinition[] array = new LevelDefinition[this.m_LevelEntriesMP.Length + MapModManager.maps.Count];
				foreach (LevelDefinition def in m_LevelEntriesMP) {
					def.m_NameLocId = MapModManager.mapNameOverrides[def.SceneName + (def.IsFFAOnly ? "" : "-")];
				}
				
				
				this.m_LevelEntriesMP.CopyTo(array, 0);
				int num = this.m_LevelEntriesMP.Length;
				foreach (var map in MapModManager.maps) {
					LevelDefinition def = this.FindLevelFromSceneName(map.Value.name, GameMode.SinglePlayer).Copy(); // Copy is so the sp definition is different to the mp one
					def.m_SceneName = map.Key;
					def.m_IsFFAOnly = map.Value.gameMode == TeamSetting.FFA;
					def.m_NameLocId = map.Value.locName;
					def.m_LoadArtStreamingPath = null; // Fix the loading screen (null will show default loading background)
					array[num++] = def;
				}
				return array;
				// MOD
			}
		}
	}
}
