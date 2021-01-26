using System;
using System.Collections;
using BBI.Unity.Core.Data;
using BBI.Unity.Core.Input;
using BBI.Unity.Core.Rendering;
using BBI.Unity.Core.World;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Events;
using UnityEngine;

namespace BBI.Unity.Game.World
{
	// Token: 0x02000381 RID: 897
	public partial class UserCamera : CameraBase
	{
		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001CDA RID: 7386
		public override float MaxAltitudeM
		{
			get
			{
				// MOD: camera distance customization
				if (UserCamera.mSensorsManagerActive)
				{
					return this.Thresholds.FurthestCameraDistanceSensors;
				}
				return MapModManager.GetMaxCameraDistance(this.Thresholds.FurthestCameraDistanceGameplay);
				// MOD
			}
		}
	}
}
