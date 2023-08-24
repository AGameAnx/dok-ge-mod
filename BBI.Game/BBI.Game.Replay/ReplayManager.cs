using System;
using System.Collections.Generic;
using BBI.Core.IO;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Commands;
using BBI.Game.Simulation;

namespace BBI.Game.Replay
{
	// Token: 0x0200018C RID: 396
	internal class ReplayManager
	{
		// Token: 0x060006B9 RID: 1721 RVA: 0x0002234A File Offset: 0x0002054A
		public ReplayManager(int currentGameVersion, ReplayMode mode)
		{
			this.mCurrentGameVersion = currentGameVersion;
			this.mReplayMode = mode;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00022360 File Offset: 0x00020560
		public bool InitializePlaybackSession(string fullFilepath)
		{
			ReplayHelpers.MostRecentReplayFile = string.Empty;
			if (this.mReplayMode == ReplayMode.ReplaysDisabled)
			{
				return false;
			}
			bool flag = File.Exists(fullFilepath);
			if (flag)
			{
				this.mGameSession = new ReplayableGameSession(File.OpenText(fullFilepath, true));
				this.mReplayMode = ReplayMode.ReplayingGame;
				return true;
			}
			return false;
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x000223A8 File Offset: 0x000205A8
		public bool InitializeRecordingSession(string sessionName, int randomSeed, NetworkPlayerID localPlayerID, CommanderManager commanderManager, GameModeBase gameMode)
		{
			ReplayHelpers.MostRecentReplayFile = string.Empty;
			if (this.mReplayMode == ReplayMode.ReplaysDisabled)
			{
				return false;
			}
			string locationForReplay = ReplayHelpers.GetLocationForReplay(sessionName);
			if (File.Exists(locationForReplay))
			{
				File.Delete(locationForReplay);
			}
			GameSessionSettings gameSessionSettings = (gameMode != null) ? gameMode.GameSessionSettings : null;
			this.mGameSession = new ReplayableGameSession(sessionName, this.mCurrentGameVersion, File.CreateText(locationForReplay, true), localPlayerID, commanderManager, randomSeed, gameSessionSettings);
			this.mReplayMode = ReplayMode.RecordingGame;
			ReplayHelpers.MostRecentReplayFile = locationForReplay;
			return true;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0002241C File Offset: 0x0002061C
		public bool FinalizeGameSession(SimFrameNumber finalFrame, Checksum finalChecksum)
		{
			if (this.mReplayMode == ReplayMode.ReplaysDisabled)
			{
				return false;
			}
			bool result = true;
			if (this.ReplayMode == ReplayMode.RecordingGame)
			{
				this.mGameSession.WriteCommandToBuffer(finalFrame, new NoOpCommand(CommanderID.None));
				this.mGameSession.WriteChecksumToFrame(finalFrame, finalChecksum);
				result = this.mGameSession.FlushBufferToStream(true);
			}
			this.mGameSession.ShutdownStreams();
			this.mGameSession = null;
			return result;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00022483 File Offset: 0x00020683
		public bool WriteCommand(SimFrameNumber atFrame, SimCommandBase command)
		{
			this.mGameSession.WriteCommandToBuffer(atFrame, command);
			this.mLatestFrame = atFrame;
			return true;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0002249A File Offset: 0x0002069A
		public bool WriteChecksumToFrame(SimFrameNumber atFrame, Checksum checksum)
		{
			return this.mGameSession.WriteChecksumToFrame(atFrame, checksum);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x000224AC File Offset: 0x000206AC
		public IEnumerable<SimCommandBase> ReadCommandsForNextFrame(SimFrameNumber requestedFrame, out SimFrameNumber frame, out Checksum frameChecksum)
		{
			return this.mGameSession.GetNextFrame(requestedFrame, out frame, out frameChecksum);
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x000224C9 File Offset: 0x000206C9
		public bool EndOfReplay
		{
			get
			{
				return this.mReplayMode == ReplayMode.ReplayingGame && this.mGameSession.EndOfBuffer;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x000224E0 File Offset: 0x000206E0
		public string SessionName
		{
			get
			{
				if (this.mGameSession == null)
				{
					return "NONE";
				}
				return this.mGameSession.SessionName;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x000224FB File Offset: 0x000206FB
		public ReplayMode ReplayMode
		{
			get
			{
				return this.mReplayMode;
			}
		}

		// Token: 0x04000650 RID: 1616
		private SimFrameNumber mLatestFrame;

		// Token: 0x04000651 RID: 1617
		private ReplayableGameSession mGameSession;

		// Token: 0x04000652 RID: 1618
		private int mCurrentGameVersion;

		// Token: 0x04000653 RID: 1619
		private ReplayMode mReplayMode;
	}
}
