using System;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Data;

namespace BBI.Game.Simulation
{
	public sealed class Timer
	{
		public Entity Entity
		{
			get
			{
				return this.mEntity;
			}
		}

		public Fixed64 DurationSeconds
		{
			get
			{
				return this.mDurationSeconds;
			}
		}

		public TimerDirection TimerDirection
		{
			get
			{
				return this.mTimerDirection;
			}
		}

		public OnTimerCompleteAction ActionOnTimerComplete
		{
			get
			{
				return this.mActionOnTimerComplete;
			}
		}

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

		[ObjectConstructor(new string[]
		{

		})]
		private Timer()
		{
		}

		private Timer(Entity owner, Fixed64 durationSeconds, TimerDirection timerDirection, OnTimerCompleteAction actionOnTimerComplete)
		{
			this.mEntity = owner;
			this.mTimerDirection = timerDirection;
			this.mDurationSeconds = durationSeconds;
			this.mCurrentTimeSeconds = ((timerDirection == TimerDirection.Countdown) ? durationSeconds : Fixed64.Zero);
			this.mActionOnTimerComplete = actionOnTimerComplete;
		}

		public static Timer Create(Entity owner, Fixed64 durationSeconds, TimerDirection timerDirection, OnTimerCompleteAction actionOnTimerComplete)
		{
			return new Timer(owner, durationSeconds, timerDirection, actionOnTimerComplete);
		}

		public int GetChecksum()
		{
			return Checksum.Combine((int)this.mTimerDirection, this.mCurrentTimeSeconds.GetChecksum(), this.mDurationSeconds.GetChecksum(), (int)this.mActionOnTimerComplete);
		}

		[StateData("Entity")]
		private Entity mEntity = Entity.None;

		[StateData("DurationSeconds")]
		private Fixed64 mDurationSeconds = Fixed64.Zero;

		[StateData("CurrentTimeSeconds")]
		private Fixed64 mCurrentTimeSeconds = Fixed64.Zero;

		[StateData("TimerDirection")]
		private TimerDirection mTimerDirection;

		[StateData("ActionOnTimerComplete")]
		private OnTimerCompleteAction mActionOnTimerComplete = OnTimerCompleteAction.Despawn;
	}
}
