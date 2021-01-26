using System;
using BBI.Core.Data;
using BBI.Core.Utility;
using UnityEngine;

namespace BBI.Unity.Game.World
{
	// Token: 0x02000081 RID: 129
	public class MapBounds : MonoBehaviour
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000422 RID: 1058
		public Vector2r Min
		{
			get
			{
				// MOD
				if (MapModManager.overrideBounds) return MapModManager.boundsMin;
				// MOD
				return this.mMin;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000423 RID: 1059
		public Vector2r Max
		{
			get
			{
				// MOD
				if (MapModManager.overrideBounds) return MapModManager.boundsMax;
				// MOD
				return this.mMax;
			}
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00028374 File Offset: 0x00026574
		public void SetSimCoordinates()
		{
			if (this.SizeX < 0f || this.SizeY < 0f)
			{
				this.SizeX = Math.Abs(this.SizeX);
				this.SizeY = Math.Abs(this.SizeY);
				Debug.LogError("MapBounds is negative! Please set to positive values.");
			}
			this.mMin = VectorHelper.XYToSimVector2(this.CenterX - 0.5f * this.SizeX, this.CenterY - 0.5f * this.SizeY);
			this.mMax = VectorHelper.XYToSimVector2(this.CenterX + 0.5f * this.SizeX, this.CenterY + 0.5f * this.SizeY);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00004FBF File Offset: 0x000031BF
		public void Awake()
		{
			this.SetSimCoordinates();
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00004FC7 File Offset: 0x000031C7
		public MapBounds()
		{
		}

		// Token: 0x0400038F RID: 911
		public float SizeX = 1000f;

		// Token: 0x04000390 RID: 912
		public float SizeY = 1000f;

		// Token: 0x04000391 RID: 913
		public float CenterX = 500f;

		// Token: 0x04000392 RID: 914
		public float CenterY = 500f;

		// Token: 0x04000393 RID: 915
		private Vector2r mMin;

		// Token: 0x04000394 RID: 916
		private Vector2r mMax;
	}
}
