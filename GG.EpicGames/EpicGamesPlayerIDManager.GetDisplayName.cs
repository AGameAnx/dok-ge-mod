using System;
using BBI.Core.Network;
using BBI.Core.Utility;

namespace GG.EpicGames
{
	public partial class EpicNetworkPlayerIDManager : NetworkPlayerIDManager
	{
		public override string GetDisplayName(NetworkPlayerID playerID)
		{
			string result = "'INVALID'";
			if (playerID == NetworkPlayerID.kInvalidID)
			{
				Log.Warn(Log.Channel.Online, "GetGamePlatformType: playerID was invalid.", new object[0]);
				return result;
			}
			CachedUserInfo cachedUserInfo = EpicUserIntegration.GetCachedUserInfo(playerID.ID, false);
			if (cachedUserInfo != null)
			{
				result = cachedUserInfo.DisplayName;
			}
			return result;
		}
	}
}
