using System;
using BBI.Core.Network;
using BBI.Core.Utility;

namespace GG.EpicGames
{
	public partial class EpicNetworkPlayerIDManager : NetworkPlayerIDManager
	{
		public override string GetDisplayName(NetworkPlayerID playerID)
		{
			if (playerID == NetworkPlayerID.kInvalidID)
			{
				Log.Warn(Log.Channel.Online, "GetGamePlatformType: playerID was invalid.", new object[0]);
				return "'INVALID'";
			}
			CachedUserInfo cachedUserInfo = EpicUserIntegration.GetCachedUserInfo(playerID.ID, false);
			return cachedUserInfo == null ? null : cachedUserInfo.DisplayName;
		}
	}
}
