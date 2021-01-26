using System;
using System.Collections;
using BBI.Unity.Core.Data;
using BBI.Unity.Core.Input;
using BBI.Unity.Core.Rendering;
using BBI.Unity.Core.World;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Events;
using UnityEngine;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;

namespace BBI.Unity.Game.World
{
	// Token: 0x020000B8 RID: 184
	public partial class UserCamera : CameraBase
	{
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x000340C4 File Offset: 0x000322C4
		public Bounds ViewConstraints
		{
			get
			{
				if (this.ViewConstraintsCollider == null)
				{
					return new Bounds(Vector3.zero, Vector3.one * float.PositiveInfinity);
				}
				// MOD: change camera constraints
				Vector2r center = (MapModManager.boundsMin + MapModManager.boundsMax) / Fixed64.FromConstFloat(2.0f);
				Vector2r size = (MapModManager.boundsMax - MapModManager.boundsMin);
				Bounds customBounds = new Bounds(new Vector3(float.Parse(center.X.ToString()), 0, float.Parse(center.Y.ToString())),
				                                 new Vector3(float.Parse(size.X.ToString()), 0, float.Parse(size.Y.ToString())));
				Bounds bounds = (MapModManager.overrideBounds) ? customBounds : this.ViewConstraintsCollider.bounds;
				// MOD
				bounds.size -= new Vector3(200f, 0f, 200f);
				return bounds;
			}
		}
	}
}
