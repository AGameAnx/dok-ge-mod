using System;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Data;

namespace BBI.Game.Simulation
{
	// Token: 0x02000271 RID: 625
	public sealed class Timer
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x000275CC File Offset: 0x000257CC
		public Entity Entity
		{
			get
			{
				return this.mEntity;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x000275D4 File Offset: 0x000257D4
		public Fixed64 DurationSeconds
		{
			get
			{
				return this.mDurationSeconds;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x000275DC File Offset: 0x000257DC
		public TimerDirection TimerDirection
		{
			get
			{
				return this.mTimerDirection;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x000275E4 File Offset: 0x000257E4
		public OnTimerCompleteAction ActionOnTimerComplete
		{
			get
			{
				return this.mActionOnTimerComplete;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x000275EC File Offset: 0x000257EC
		// (set) Token: 0x06000867 RID: 2151 RVA: 0x000275F4 File Offset: 0x000257F4
		public Fixed64 CurrentTimeSeconds
		{
			get
			{
				return this.mCurrentTimeSeconds;
			}
			set
			{
				if (this.mTimerDirection != TimerDirection.Countdown)
				{
					TimerDirection timerDirection = this.mTimerDirection;
				}
				this.mCurrentTimeSeconds = Fixed64.Clamp(value, Fixed64.Zero, this.mDurationSeconds);
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x0002761E File Offset: 0x0002581E
		public Fixed64 CurrentTimePercentage
		{
			get
			{
				if (!Fixed64.BigEnough(this.DurationSeconds))
				{
					return Fixed64.One;
				}
				return this.CurrentTimeSeconds / this.DurationSeconds;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x00027644 File Offset: 0x00025844
		public bool Completed
		{
			get
			{
				if (this.mTimerDirection == TimerDirection.Countdown)
				{
					return this.mCurrentTimeSeconds <= Fixed64.Zero;
				}
				return this.mCurrentTimeSeconds >= this.mDurationSeconds;
			}
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00027670 File Offset: 0x00025870
		[ObjectConstructor(new string[]
		{

		})]
		private Timer()
		{
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x000276A0 File Offset: 0x000258A0
		private Timer(Entity owner, Fixed64 durationSeconds, TimerDirection timerDirection, OnTimerCompleteAction actionOnTimerComplete)
		{
			this.mEntity = owner;
			this.mTimerDirection = timerDirection;
			this.mDurationSeconds = durationSeconds;
			this.mCurrentTimeSeconds = ((timerDirection == TimerDirection.Countdown) ? durationSeconds : Fixed64.Zero);
			this.mActionOnTimerComplete = actionOnTimerComplete;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00027709 File Offset: 0x00025909
		public static Timer Create(Entity owner, Fixed64 durationSeconds, TimerDirection timerDirection, OnTimerCompleteAction actionOnTimerComplete)
		{
			return new Timer(owner, durationSeconds, timerDirection, actionOnTimerComplete);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00027714 File Offset: 0x00025914
		public int GetChecksum()
		{
			return Checksum.Combine((int)this.mTimerDirection, this.mCurrentTimeSeconds.GetChecksum(), this.mDurationSeconds.GetChecksum(), (int)this.mActionOnTimerComplete);
		}

		// Token: 0x04000A2D RID: 2605
		[StateData("Entity")]
		private Entity mEntity = Entity.None;

		// Token: 0x04000A2E RID: 2606
		[StateData("DurationSeconds")]
		private Fixed64 mDurationSeconds = Fixed64.Zero;

		// Token: 0x04000A2F RID: 2607
		[StateData("CurrentTimeSeconds")]
		private Fixed64 mCurrentTimeSeconds = Fixed64.Zero;

		// Token: 0x04000A30 RID: 2608
		[StateData("TimerDirection")]
		private TimerDirection mTimerDirection;

		// Token: 0x04000A31 RID: 2609
		[StateData("ActionOnTimerComplete")]
		private OnTimerCompleteAction mActionOnTimerComplete = OnTimerCompleteAction.Despawn;
	}
}
