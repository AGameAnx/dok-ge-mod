using System;
using System.Xml;
using BBI.Core.Utility;
using BBI.Game.Data;
using UnityEngine;

namespace BBI.Unity.Game.World
{
	// Token: 0x020003A8 RID: 936
	public partial class SceneSpawnInfo : MonoBehaviour
	{
		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001E5B RID: 7771
		public SceneSpawnInfo.TeamSpawnInfo[] TeamSpawnInfos
		{
			get
			{
				// MOD: load new spawn locations
				if (MapModManager.CustomLayout) {
					m_TeamSpawnInfos = new SceneSpawnInfo.TeamSpawnInfo[] {
						new SceneSpawnInfo.TeamSpawnInfo {
							m_PlayerSpawnInfos = new SceneSpawnInfo.PlayerSpawnInfo[] {
								new SceneSpawnInfo.PlayerSpawnInfo(), new SceneSpawnInfo.PlayerSpawnInfo(), new SceneSpawnInfo.PlayerSpawnInfo()
							}
						}, new SceneSpawnInfo.TeamSpawnInfo {
							m_PlayerSpawnInfos = new SceneSpawnInfo.PlayerSpawnInfo[] {
								new SceneSpawnInfo.PlayerSpawnInfo(), new SceneSpawnInfo.PlayerSpawnInfo(), new SceneSpawnInfo.PlayerSpawnInfo()
							}
						}
					};
					
					for (int i = 0; i < 6; i++)  {
						m_TeamSpawnInfos[i / 3].m_PlayerSpawnInfos[i % 3].m_SpawnPoint = new GameObject().transform;
					}
					
					foreach (MapModManager.MapSpawnData spawn in MapModManager.spawns)  {
						m_TeamSpawnInfos[(spawn.team == 0) ? 1 : 0].m_PlayerSpawnInfos[spawn.index].m_SpawnPoint.Translate(spawn.position);
						m_TeamSpawnInfos[(spawn.team == 0) ? 1 : 0].m_PlayerSpawnInfos[spawn.index].m_SpawnPoint.Rotate(0f, spawn.angle, 0f);
					}
				}
				// MOD
				return this.m_TeamSpawnInfos;
			}
		}
	}
}
