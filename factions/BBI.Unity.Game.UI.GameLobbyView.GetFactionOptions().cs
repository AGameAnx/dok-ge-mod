using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BBI.Core;
using BBI.Core.Localization;
using BBI.Core.Network;
using BBI.Game.Data;
using BBI.Game.Simulation;
using BBI.Unity.Core.DLC;
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
	// Token: 0x02000141 RID: 321
	public partial class GameLobbyView : BlackbirdPanelBase
	{
		// Token: 0x06000BF1 RID: 3057 RVA: 0x00054608 File Offset: 0x00052808
		public static Dictionary<string, PlayerFactionSelection> GetFactionOptions(LevelManager levelManager, DLCManager dlcManager, PlayerType playerType, bool useOwnedDLCOnly)
		{
			Dictionary<string, PlayerFactionSelection> dictionary = new Dictionary<string, PlayerFactionSelection>(6);
			if (levelManager != null)
			{
				CommanderAttributes[] commanderAttributes = levelManager.CommanderAttributes;
				if (commanderAttributes != null)
				{
					for (int i = 0; i < commanderAttributes.Length; i++)
					{
						CommanderAttributes commanderAttributes2 = commanderAttributes[i];
						string key = string.IsNullOrEmpty(commanderAttributes2.NameLocID) ? commanderAttributes2.Name : commanderAttributes2.NameLocID;
						PlayerFactionSelection value = new PlayerFactionSelection((CustomizationFactionSetting)i, DLCPackID.kInvalidID, false);
						dictionary.Add(key, value);
					}
				}
			}
			if (dlcManager != null)
			{
				IEnumerable<DLCPackDescriptor> enumerable = useOwnedDLCOnly ? dlcManager.OwnedDLCPacks : dlcManager.AllDLCPacks;
				foreach (DLCPackDescriptor dlcpackDescriptor in enumerable)
				{
					if (dlcpackDescriptor.DLCPackType == DLCType.Faction)
					{
						UnitFactionDLCPack unitFactionDLCPack = (UnitFactionDLCPack)dlcManager.GetDLCPackHeader(dlcpackDescriptor.DLCPackID);
						if (!(unitFactionDLCPack == null))
						{
							if (playerType == PlayerType.AI)
							{
								CommanderAttributesData commanderAttributesData = unitFactionDLCPack.CommanderAttrs as CommanderAttributesData;
								if (commanderAttributesData == null || commanderAttributesData.AIArchetypeAssets.IsNullOrEmpty<AIArchetypeAttributesAsset>())
								{
									continue;
								}
							}
							string dlcpackLocID = dlcpackDescriptor.DLCPackLocID;
							PlayerFactionSelection value2 = new PlayerFactionSelection(unitFactionDLCPack.CustomizesFaction, dlcpackDescriptor.DLCPackID, false);
							dictionary.Add(dlcpackLocID, value2);
						}
					}
				}
			}
			PlayerFactionSelection value3 = new PlayerFactionSelection(CustomizationFactionSetting.Coalition, DLCPackID.kInvalidID, true);
			// MOD
			if (playerType == PlayerType.AI) 
			{
				dictionary.Remove("SPECTATOR");
				dictionary.Remove("FATHERSHIP");
				dictionary.Remove("NONE");
			}		
			// MOD
			dictionary.Add("ID_UI_FE_MP_RANDOM_253", value3);
			return dictionary;
		}
	}
}
