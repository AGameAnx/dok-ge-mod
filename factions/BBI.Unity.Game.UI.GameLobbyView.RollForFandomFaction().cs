using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BBI.Core.Localization;
using BBI.Core.Network;
using BBI.Core.Utility;
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
	// Token: 0x02000141 RID: 321
	public partial class GameLobbyView : BlackbirdPanelBase
	{
		// Token: 0x06000BEE RID: 3054
		public static bool RollForRandomFaction(Dictionary<string, PlayerFactionSelection> factionOptions, out PlayerFactionSelection randomSelectedFaction)
		{
			// MOD: remove spectator and fathership races
			factionOptions.Remove("SPECTATOR");
			factionOptions.Remove("FATHERSHIP");
			factionOptions.Remove("NONE");
			// MOD
			if (factionOptions.Count > 0)
			{
				List<PlayerFactionSelection> list = new List<PlayerFactionSelection>(factionOptions.Values);
				int index = UnityEngine.Random.Range(0, list.Count);
				randomSelectedFaction = list[index];
				return true;
			}
			Log.Error(Log.Channel.UI, "No faction options provided! Unable to pick a random faction.", new object[0]);
			randomSelectedFaction = PlayerFactionSelection.kInvalid;
			return false;
		}
	}
}
