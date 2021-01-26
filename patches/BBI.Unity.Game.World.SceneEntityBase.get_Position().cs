// Remove wrecks from maps

using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Simulation;
using BBI.Unity.Game.Data;
using UnityEngine;

namespace BBI.Unity.Game.World
{
	// Token: 0x0200039F RID: 927
	public partial class SceneEntityBase : MonoBehaviour
	{
		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001E24 RID: 7716
		protected Vector2r Position
		{
			get
			{
				// MOD: remove all wrecks
				// Tried doing this on level load complete but after the first 2 or so level loads, it stops working, which will probably cause desync
				// No reason to spend more time on this if this is all thats needed
				if (MapModManager.BannedEntities.Contains(this.TypeID) && MapModManager.CustomLayout)
				{
					return new Vector2r(Fixed64.FromInt(1000000), Fixed64.FromInt(1000000));
				}
				// MOD
				return VectorHelper.UnityVector3ToSimVector2(base.transform.position, 0.25f);
			}
		}
	}
}
