using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Game.Data;

namespace BBI.Game.Simulation
{
	// Token: 0x0200033B RID: 827
	internal sealed class Detectable
	{
		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x000494A1 File Offset: 0x000476A1
		internal Dictionary<CommanderID, ForcedDetectionState> ForcedDetectionState
		{
			get
			{
				return this.mForcedDetectionState;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x000494A9 File Offset: 0x000476A9
		internal Dictionary<CommanderID, Entity> LastSensedByEntity
		{
			get
			{
				return this.mLastSensedByEntity;
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x000494B1 File Offset: 0x000476B1
		internal int TimeVisibleAfterFiringMS
		{
			get
			{
				return this.mTimeVisibleAfterFiringMS;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x000494B9 File Offset: 0x000476B9
		internal bool Disabled
		{
			get
			{
				return this.mDisabled;
			}
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x000494C4 File Offset: 0x000476C4
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

		// Token: 0x06000E87 RID: 3719 RVA: 0x00049540 File Offset: 0x00047740
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

		// Token: 0x06000E88 RID: 3720 RVA: 0x00049610 File Offset: 0x00047810
		internal static Detectable Create(DetectableAttributes attrib, Entity entity)
		{
			Detectable result = new Detectable(attrib);
			DetectableProcessor.AddNewDetectable(entity);
			return result;
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x0004962C File Offset: 0x0004782C
		internal DetectionState GetDefaultState(bool fogOfWarDisabled, Entity self, CommanderID viewerID, bool commandersAreFriendly)
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

		// Token: 0x06000E8A RID: 3722 RVA: 0x00049668 File Offset: 0x00047868
		internal void ForceStateForMilliseconds(CommanderID forPlayer, DetectionState state, int milliseconds)
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

		// Token: 0x06000E8B RID: 3723 RVA: 0x000496B0 File Offset: 0x000478B0
		internal void ForceState(CommanderID forPlayer, DetectionState state)
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

		// Token: 0x06000E8C RID: 3724 RVA: 0x000496F5 File Offset: 0x000478F5
		internal void RemoveForcedState(CommanderID forPlayer)
		{
			this.mForcedDetectionState.Remove(forPlayer);
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00049704 File Offset: 0x00047904
		internal void SwapDetectionMaps()
		{
			Dictionary<CommanderID, DetectionState> dictionary = this.mPreviousSessionDetectionState;
			this.mPreviousSessionDetectionState = this.mSessionDetectionState;
			this.mSessionDetectionState = dictionary;
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x0004972C File Offset: 0x0004792C
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

		// Token: 0x06000E8F RID: 3727 RVA: 0x0004979D File Offset: 0x0004799D
		internal void SetDetectionState(CommanderID sensingCommander, DetectionState targetState)
		{
			this.mSessionDetectionState[sensingCommander] = targetState;
			if (targetState == DetectionState.Sensed)
			{
				this.SetHasBeenSeenBefore(sensingCommander);
			}
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x000497B8 File Offset: 0x000479B8
		internal DetectionState GetSharedDetectionState(CommanderID viewingCommander, bool useCurrentState)
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

		// Token: 0x06000E91 RID: 3729 RVA: 0x00049850 File Offset: 0x00047A50
		internal DetectionState GetRawDetectionState(CommanderID viewingCommander)
		{
			return this.mSessionDetectionState[viewingCommander];
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x0004985E File Offset: 0x00047A5E
		internal void SetAlwaysVisible(bool alwaysVisible)
		{
			this.mAlwaysVisible = alwaysVisible;
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00049867 File Offset: 0x00047A67
		internal void Disable(bool disabled)
		{
			this.mDisabled = disabled;
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00049870 File Offset: 0x00047A70
		internal void SetHasBeenSeenBefore(CommanderID playerID)
		{
			this.mHasBeenSeenBefore[playerID] = true;
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00049880 File Offset: 0x00047A80
		internal bool HasBeenSeenBefore(CommanderID playerID)
		{
			bool result = false;
			this.mHasBeenSeenBefore.TryGetValue(playerID, out result);
			return result;
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x0004989F File Offset: 0x00047A9F
		internal bool HasSharedDetectionStateChangedSinceLastFrame(CommanderID playerID, out DetectionState previousState, out DetectionState currentState)
		{
			previousState = this.GetSharedDetectionState(playerID, false);
			currentState = this.GetSharedDetectionState(playerID, true);
			return previousState != currentState;
		}

		// Token: 0x04000D10 RID: 3344
		[StateData("SessionDetectionState")]
		private Dictionary<CommanderID, DetectionState> mSessionDetectionState;

		// Token: 0x04000D11 RID: 3345
		private Dictionary<CommanderID, DetectionState> mPreviousSessionDetectionState;

		// Token: 0x04000D12 RID: 3346
		[StateData("ForcedDetectionState")]
		private Dictionary<CommanderID, ForcedDetectionState> mForcedDetectionState;

		// Token: 0x04000D13 RID: 3347
		[StateData("HasBeenSeenBefore")]
		private Dictionary<CommanderID, bool> mHasBeenSeenBefore;

		// Token: 0x04000D14 RID: 3348
		[StateData("LastSensedByEntity")]
		private Dictionary<CommanderID, Entity> mLastSensedByEntity;

		// Token: 0x04000D15 RID: 3349
		[StateData("Disabled")]
		private bool mDisabled;

		// Token: 0x04000D16 RID: 3350
		[StateData("AlwaysVisible")]
		private bool mAlwaysVisible;

		// Token: 0x04000D17 RID: 3351
		[StateData("MinimumStateAfterDetection")]
		private DetectionState mMinimumStateAfterDetection;

		// Token: 0x04000D18 RID: 3352
		[StateData("TimeVisibleAfterFiringMS")]
		private int mTimeVisibleAfterFiringMS;
	}
}
