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
using Subsystem;
using UnityEngine;
using System.Net;

namespace BBI.Unity.Game.UI.Frontend.Helpers
{
	// Token: 0x020001A2 RID: 418
	public abstract partial class LobbySetupPanelBase : BlackbirdPanelBase
	{
		// Token: 0x06000954 RID: 2388
		private void OnCreateOrJoinLobbySuccess()
		{
			// MOD
			// Clear any patch and layout overrides
			MapModManager.MapXml = "";
			Subsystem.AttributeLoader.PatchOverrideData = "";
			Print(SteamAPIIntegration.SteamUserName + " has joined using " + MapModManager.ModDescription);
			//if (Subsystem.AttributeLoader.GetPatchData() != "") { // getPatchData checks file system too, not just PatchOverrideData
			//	Print("[FF0000][b][u][i]" + SteamAPIIntegration.SteamUserName + " HAS A PATCH.JSON! [ " + MapModUtil.GetHash(Subsystem.AttributeLoader.GetPatchData()) + " ]");
			//}
			m_ChatPanel.OnChatAdded += OnChatAdded;
			// MOD
		}
		
		// MOD
		private void Print(string text) {
			mLobby.SendChatToAllPlayers(PlayerChatType.LobbySystemAnnouncement, text);
		}
		// MOD
	}
}
