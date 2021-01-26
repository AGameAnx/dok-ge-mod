using System;
using System.Collections.Generic;
using System.IO;
using BBI.Core.IO;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Commands;
using BBI.Game.Simulation;
using Subsystem;
using System.Text;

namespace BBI.Game.Replay
{
	// Token: 0x02000180 RID: 384
	internal partial class ReplayManager
	{
		// Token: 0x06000683 RID: 1667
		public bool InitializeRecordingSession(string sessionName, int randomSeed, NetworkPlayerID localPlayerID, CommanderManager commanderManager, GameModeBase gameMode) {
			ReplayHelpers.MostRecentReplayFile = string.Empty;
			if (this.mReplayMode == ReplayMode.ReplaysDisabled) {
				return false;
			}
			string locationForReplay = ReplayHelpers.GetLocationForReplay(sessionName);
			
			if (BBI.Core.IO.File.Exists(locationForReplay)) {
				BBI.Core.IO.File.Delete(locationForReplay);
				// MOD: load replay files
				BBI.Core.IO.File.Delete(locationForReplay + "x");
			}
			List<byte> bytes = new List<byte>();
			byte[] layout = Encoding.UTF8.GetBytes(MapModManager.GetLayoutData());
			byte[] patch = Encoding.UTF8.GetBytes(Subsystem.AttributeLoader.GetPatchData());
			bytes.AddRange(new byte[] { (byte)((layout.Length >> 0) & 0xff), (byte)((layout.Length >> 8) & 0xff), (byte)((layout.Length >> 16) & 0xff), (byte)((layout.Length >> 24) & 0xff) });
			bytes.AddRange(new byte[] { (byte)((patch.Length >> 0) & 0xff), (byte)((patch.Length >> 8) & 0xff), (byte)((patch.Length >> 16) & 0xff), (byte)((patch.Length >> 24) & 0xff) });
			bytes.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 });
			bytes.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 });
			bytes.AddRange(layout);
			bytes.AddRange(patch);
			System.IO.File.WriteAllBytes(locationForReplay + "x", bytes.ToArray());
			// MOD
			
			GameSessionSettings gameSessionSettings = (gameMode != null) ? gameMode.GameSessionSettings : null;
			this.mGameSession = new ReplayableGameSession(sessionName, this.mCurrentGameVersion, BBI.Core.IO.File.CreateText(locationForReplay, true), localPlayerID, commanderManager, randomSeed, gameSessionSettings);
			this.mReplayMode = ReplayMode.RecordingGame;
			ReplayHelpers.MostRecentReplayFile = locationForReplay;
			return true;
		}
	}
}
