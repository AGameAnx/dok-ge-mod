using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BBI.Core.Localization;
using BBI.Core.Network;

namespace BBI.Steam
{
	// Token: 0x0200003A RID: 58
	public static partial class SteamAPIIntegration
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00005EC0 File Offset: 0x000040C0
		public static string SteamUserName
		{
			get
			{
				if (SteamAPIIntegration.IsSteamRunning)
				{
					return Marshal.PtrToStringAnsi(SteamAPIIntegration.BBI_SteamUser_GetUserName());
				}
				return NullNetworkPlayerIDManager.kLocalPlayerName;
			}
		}
	}
}
