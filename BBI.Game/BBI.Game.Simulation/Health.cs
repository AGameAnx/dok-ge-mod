using System;
using BBI.Core.Data;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Commands;
using BBI.Game.Data;

namespace BBI.Game.Simulation
{
	// Token: 0x0200033F RID: 831
	internal sealed class Health
	{
		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000EE0 RID: 3808 RVA: 0x0004B40E File Offset: 0x0004960E
		// (set) Token: 0x06000EE1 RID: 3809 RVA: 0x0004B416 File Offset: 0x00049616
		internal int CurrentHealth
		{
			get
			{
				return this.mCurrentHealth;
			}
			set
			{
				this.SetCurrentHealth(value, true);
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000EE2 RID: 3810 RVA: 0x0004B420 File Offset: 0x00049620
		internal Fixed64 CurrentHealthPercentage
		{
			get
			{
				return Fixed64.FromRational(this.mCurrentHealth, this.MaxHealth);
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x0004B433 File Offset: 0x00049633
		internal uint LastDamagedAtFrame
		{
			get
			{
				return this.mLastDamagedAtFrame;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000EE4 RID: 3812 RVA: 0x0004B43B File Offset: 0x0004963B
		internal uint LastRepairedAtFrame
		{
			get
			{
				return this.mLastRepairedAtFrame;
			}
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0004B444 File Offset: 0x00049644
		[ObjectConstructor(new string[]
		{
			"MaxHealth",
			"CurrentHealth"
		})]
		private Health(int max, int initial)
		{
			if (max > 1)
			{
				this.MaxHealth = max;
			}
			else
			{
				this.MaxHealth = 1;
				Log.Error(Log.Channel.Data | Log.Channel.Gameplay, "Attempting to create Health component with max health {0} which is less than {1}, defaulting to {1}!", new object[]
				{
					max,
					1
				});
			}
			this.SetCurrentHealth(initial, false);
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0004B4BC File Offset: 0x000496BC
		internal static Health Create(int max, int initial)
		{
			return new Health(max, initial);
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0004B4C8 File Offset: 0x000496C8
		private void SetCurrentHealth(int value, bool setLastModifiedFrame)
		{
			if (setLastModifiedFrame)
			{
				if (value < this.mCurrentHealth)
				{
					this.mLastDamagedAtFrame = Sim.Instance.GlobalFrameCount.FrameNumber;
				}
				else if (value > this.mCurrentHealth)
				{
					this.mLastRepairedAtFrame = Sim.Instance.GlobalFrameCount.FrameNumber;
				}
			}
			this.mCurrentHealth = Fixed64.Clamp(value, 0, this.MaxHealth);
		}

		// Token: 0x04000D31 RID: 3377
		public const int kMinMaxHealth = 1;

		// Token: 0x04000D32 RID: 3378
		[StateData("MaxHealth")]
		internal int MaxHealth;

		// Token: 0x04000D33 RID: 3379
		[StateData("CurrentHealth")]
		private int mCurrentHealth;

		// Token: 0x04000D34 RID: 3380
		[StateData("LastDamagedAtFrame")]
		private uint mLastDamagedAtFrame = SimFrameNumber.Zero.FrameNumber;

		// Token: 0x04000D35 RID: 3381
		[StateData("GodMode")]
		internal GodModeType GodMode;

		// Token: 0x04000D36 RID: 3382
		[StateData("LastRepairedAtFrame")]
		private uint mLastRepairedAtFrame = SimFrameNumber.Zero.FrameNumber;

		// Token: 0x04000D37 RID: 3383
		[Obsolete("Deprecated, kept for saveload and persistence compatibility!")]
		[StateData("RepairedThisTick")]
		private bool RepairedThisTick;
	}
}
