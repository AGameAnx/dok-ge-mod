using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BBI.Core;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Events;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.AI;
using BBI.Game.Commands;
using BBI.Game.Data;
using BBI.Game.Replay;
using BBI.Game.SaveLoad;

namespace BBI.Game.Simulation
{
	// Token: 0x020003DB RID: 987
	public partial class Sim
	{
		// Token: 0x06001415 RID: 5141 RVA: 0x0006A47C File Offset: 0x0006867C
		internal Checksum Tick(SimFrameNumber frameNumber)
		{
			Checksum checksum = new Checksum();
			this.UnitManager.Tick(checksum, frameNumber);
			this.CommanderManager.Tick(checksum);
			// MOD
			MapModManager.Tick(frameNumber);
			// MOD
			AllEntityProcessor.Process(this.Settings.EntitySystem, checksum);
			if (this.Settings.GameMode != null && frameNumber > SimFrameNumber.First)
			{
				this.Settings.GameMode.EvaluateGameConditions(frameNumber, checksum);
			}
			if (this.AIManager != null)
			{
				this.AIManager.Tick(checksum);
			}
			checksum.Add((int)this.GlobalFrameCount.FrameNumber);
			return checksum;
		}
	}
}
