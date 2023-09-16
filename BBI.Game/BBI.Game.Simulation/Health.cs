using System;
using BBI.Core.Data;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Commands;
using BBI.Game.Data;

namespace BBI.Game.Simulation
{
	public sealed class Health
	{
		public int CurrentHealth
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

		internal Fixed64 CurrentHealthPercentage
		{
			get
			{
				return Fixed64.FromRational(this.mCurrentHealth, this.MaxHealth);
			}
		}

		internal uint LastDamagedAtFrame
		{
			get
			{
				return this.mLastDamagedAtFrame;
			}
		}

		internal uint LastRepairedAtFrame
		{
			get
			{
				return this.mLastRepairedAtFrame;
			}
		}

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

		internal static Health Create(int max, int initial)
		{
			return new Health(max, initial);
		}

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

		public const int kMinMaxHealth = 1;

		[StateData("MaxHealth")]
		internal int MaxHealth;

		[StateData("CurrentHealth")]
		private int mCurrentHealth;

		[StateData("LastDamagedAtFrame")]
		private uint mLastDamagedAtFrame = SimFrameNumber.Zero.FrameNumber;

		[StateData("GodMode")]
		internal GodModeType GodMode;

		[StateData("LastRepairedAtFrame")]
		private uint mLastRepairedAtFrame = SimFrameNumber.Zero.FrameNumber;

		[Obsolete("Deprecated, kept for saveload and persistence compatibility!")]
		[StateData("RepairedThisTick")]
		private bool RepairedThisTick;
	}
}
