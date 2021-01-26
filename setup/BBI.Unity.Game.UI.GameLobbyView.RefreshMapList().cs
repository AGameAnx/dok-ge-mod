using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BBI.Core.Localization;
using BBI.Core.Network;
using BBI.Game.Data;
using BBI.Game.Simulation;
using BBI.Unity.Core.Utility;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.DLC;
using BBI.Unity.Game.Network;
using BBI.Unity.Game.UI.Frontend.Helpers;
using BBI.Unity.Game.Utility;
using BBI.Unity.Game.World;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	// Token: 0x02000327 RID: 807
	public partial class GameLobbyView : BlackbirdPanelBase
	{
		// Token: 0x060018A6 RID: 6310 RVA: 0x000865A0 File Offset: 0x000847A0
		private void RefreshMapList()
		{
			this.m_MapPopupList.Clear();
			if (this.mLevelManager.LevelEntriesMP != null)
			{
				for (int i = 0; i < this.mLevelManager.LevelEntriesMP.Length; i++)
				{
					LevelDefinition levelDefinition = this.mLevelManager.LevelEntriesMP[i];
					if (levelDefinition.IsAvailableForLoad)
					{
						if (this.ActiveTeamSetting == TeamSetting.FFA)
						{
							if (levelDefinition.IsFFAOnly)
							{
								this.m_MapPopupList.AddItem(levelDefinition.NameLocId, i);
							}
						}
						else if (!levelDefinition.IsFFAOnly)
						{
							this.m_MapPopupList.AddItem(levelDefinition.NameLocId, i);
						}
						
						// MOD: fix list going off the screen
						m_MapPopupList.position = (this.ActiveTeamSetting == TeamSetting.FFA) ? UIPopupList.Position.Below : UIPopupList.Position.Above;
						m_MapPopupList.fontSize = 16;
						// MOD
					}
				}
			}
			if (this.m_MapPopupList.items.Count > 0)
			{
				this.m_MapPopupList.value = this.m_MapPopupList.items[0];
			}
			else
			{
				this.m_MapPopupList.value = "<NO MAP>";
			}
			this.UpdateMapInfo();
		}
	}
}
