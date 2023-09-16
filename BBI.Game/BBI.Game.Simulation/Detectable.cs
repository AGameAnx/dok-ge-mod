using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Game.Data;

namespace BBI.Game.Simulation
{
	public sealed class Detectable
	{
		internal Dictionary<CommanderID, ForcedDetectionState> ForcedDetectionState
		{
			get
			{
				return this.mForcedDetectionState;
			}
		}

		public Dictionary<CommanderID, Entity> LastSensedByEntity
		{
			get
			{
				return this.mLastSensedByEntity;
			}
		}

		public int TimeVisibleAfterFiringMS
		{
			get
			{
				return this.mTimeVisibleAfterFiringMS;
			}
		}

		public bool Disabled
		{
			get
			{
				return this.mDisabled;
			}
		}

		[ObjectConstructor(new string[]
		{
			"SessionDetectionState"
		})]
		[Obsolete("For save/load only.", true)]
		private Detectable(KeyValuePair<CommanderID, DetectionState>[] detectionStates)
		{
			this.mSessionDetectionState = new Dictionary<CommanderID, DetectionState>();
			this.mPreviousSessionDetectionState = new Dictionary<CommanderID, DetectionState>();
			this.mForcedDetectionState = new Dictionary<CommanderID, ForcedDetectionState>();
			this.mHasBeenSeenBefore = new Dictionary<CommanderID, bool>();
			this.mLastSensedByEntity = new Dictionary<CommanderID, Entity>();
			foreach (KeyValuePair<CommanderID, DetectionState> keyValuePair in detectionStates)
			{
				this.mPreviousSessionDetectionState.Add(keyValuePair.Key, DetectionState.Hidden);
			}
		}

		private Detectable(DetectableAttributes attrib)
		{
			this.mSessionDetectionState = new Dictionary<CommanderID, DetectionState>();
			this.mPreviousSessionDetectionState = new Dictionary<CommanderID, DetectionState>();
			this.mForcedDetectionState = new Dictionary<CommanderID, ForcedDetectionState>();
			this.mLastSensedByEntity = new Dictionary<CommanderID, Entity>();
			this.mHasBeenSeenBefore = new Dictionary<CommanderID, bool>();
			if (attrib.SetHasBeenSeenBeforeOnSpawn)
			{
				foreach (CommanderID hasBeenSeenBefore in Sim.Instance.CommanderManager.CommanderIDs)
				{
					this.SetHasBeenSeenBefore(hasBeenSeenBefore);
				}
			}
			this.mDisabled = false;
			this.mAlwaysVisible = attrib.AlwaysVisible;
			this.mMinimumStateAfterDetection = attrib.MinimumStateAfterDetection;
			this.mTimeVisibleAfterFiringMS = attrib.TimeVisibleAfterFiring;
		}

		public static Detectable Create(DetectableAttributes attrib, Entity entity)
		{
			Detectable result = new Detectable(attrib);
			DetectableProcessor.AddNewDetectable(entity);
			return result;
		}

		public DetectionState GetDefaultState(bool fogOfWarDisabled, Entity self, CommanderID viewerID, bool commandersAreFriendly)
		{
			if (this.mDisabled)
			{
				return DetectionState.Hidden;
			}
			if (commandersAreFriendly || this.mAlwaysVisible || fogOfWarDisabled)
			{
				return DetectionState.Sensed;
			}
			DetectionState result = DetectionState.Hidden;
			if (this.HasBeenSeenBefore(viewerID))
			{
				result = this.mMinimumStateAfterDetection;
			}
			return result;
		}

		public void ForceStateForMilliseconds(CommanderID forPlayer, DetectionState state, int milliseconds)
		{
			ForcedDetectionState forcedDetectionState;
			if (!this.mForcedDetectionState.TryGetValue(forPlayer, out forcedDetectionState))
			{
				forcedDetectionState = new ForcedDetectionState(milliseconds, state);
				this.mForcedDetectionState[forPlayer] = forcedDetectionState;
			}
			forcedDetectionState.Lifetime = DetectionStateLifetime.Finite;
			forcedDetectionState.State = state;
			forcedDetectionState.TimeLeftMS = milliseconds;
		}

		public void ForceState(CommanderID forPlayer, DetectionState state)
		{
			ForcedDetectionState forcedDetectionState;
			if (!this.mForcedDetectionState.TryGetValue(forPlayer, out forcedDetectionState))
			{
				this.mForcedDetectionState[forPlayer] = new ForcedDetectionState(state);
				return;
			}
			forcedDetectionState.TimeLeftMS = 0;
			forcedDetectionState.Lifetime = DetectionStateLifetime.Infinite;
			forcedDetectionState.State = state;
		}

		public void RemoveForcedState(CommanderID forPlayer)
		{
			this.mForcedDetectionState.Remove(forPlayer);
		}

		public void SwapDetectionMaps()
		{
			Dictionary<CommanderID, DetectionState> dictionary = this.mPreviousSessionDetectionState;
			this.mPreviousSessionDetectionState = this.mSessionDetectionState;
			this.mSessionDetectionState = dictionary;
		}

		public bool SetForcedState(CommanderID forCommander)
		{
			ForcedDetectionState forcedDetectionState;
			if (this.mForcedDetectionState.TryGetValue(forCommander, out forcedDetectionState))
			{
				forcedDetectionState = this.mForcedDetectionState[forCommander];
				bool flag = forcedDetectionState.TimeLeftMS > 0;
				if (flag && forcedDetectionState.Lifetime == DetectionStateLifetime.Finite)
				{
					forcedDetectionState.TimeLeftMS -= 125;
				}
				bool flag2 = flag || forcedDetectionState.Lifetime == DetectionStateLifetime.Infinite;
				if (flag2)
				{
					this.SetDetectionState(forCommander, forcedDetectionState.State);
				}
				return flag2;
			}
			return false;
		}

		public void SetDetectionState(CommanderID sensingCommander, DetectionState targetState)
		{
			this.mSessionDetectionState[sensingCommander] = targetState;
			if (targetState == DetectionState.Sensed)
			{
				this.SetHasBeenSeenBefore(sensingCommander);
			}
		}

		public DetectionState GetSharedDetectionState(CommanderID viewingCommander, bool useCurrentState)
		{
			DetectionState detectionState = DetectionState.Hidden;
			Dictionary<CommanderID, DetectionState> dictionary = useCurrentState ? this.mSessionDetectionState : this.mPreviousSessionDetectionState;
			if (dictionary.TryGetValue(viewingCommander, out detectionState) && detectionState < DetectionState.Sensed)
			{
				foreach (KeyValuePair<CommanderID, DetectionState> keyValuePair in dictionary)
				{
					if (Sim.Instance.InteractionProvider.AreFriendly(viewingCommander, keyValuePair.Key))
					{
						DetectionState value = keyValuePair.Value;
						if (value > detectionState)
						{
							detectionState = value;
							if (detectionState == DetectionState.Sensed)
							{
								break;
							}
						}
					}
				}
			}
			return detectionState;
		}

		public DetectionState GetRawDetectionState(CommanderID viewingCommander)
		{
			return this.mSessionDetectionState[viewingCommander];
		}

		public void SetAlwaysVisible(bool alwaysVisible)
		{
			this.mAlwaysVisible = alwaysVisible;
		}

		public void Disable(bool disabled)
		{
			this.mDisabled = disabled;
		}

		public void SetHasBeenSeenBefore(CommanderID playerID)
		{
			this.mHasBeenSeenBefore[playerID] = true;
		}

		public bool HasBeenSeenBefore(CommanderID playerID)
		{
			bool result = false;
			this.mHasBeenSeenBefore.TryGetValue(playerID, out result);
			return result;
		}

		public bool HasSharedDetectionStateChangedSinceLastFrame(CommanderID playerID, out DetectionState previousState, out DetectionState currentState)
		{
			previousState = this.GetSharedDetectionState(playerID, false);
			currentState = this.GetSharedDetectionState(playerID, true);
			return previousState != currentState;
		}

		[StateData("SessionDetectionState")]
		private Dictionary<CommanderID, DetectionState> mSessionDetectionState;

		private Dictionary<CommanderID, DetectionState> mPreviousSessionDetectionState;

		[StateData("ForcedDetectionState")]
		private Dictionary<CommanderID, ForcedDetectionState> mForcedDetectionState;

		[StateData("HasBeenSeenBefore")]
		private Dictionary<CommanderID, bool> mHasBeenSeenBefore;

		[StateData("LastSensedByEntity")]
		private Dictionary<CommanderID, Entity> mLastSensedByEntity;

		[StateData("Disabled")]
		private bool mDisabled;

		[StateData("AlwaysVisible")]
		private bool mAlwaysVisible;

		[StateData("MinimumStateAfterDetection")]
		private DetectionState mMinimumStateAfterDetection;

		[StateData("TimeVisibleAfterFiringMS")]
		private int mTimeVisibleAfterFiringMS;
	}
}
