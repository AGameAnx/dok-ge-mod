using System;
using BBI.Core.Data;
using BBI.Core.Utility;
using UnityEngine;

namespace BBI.Unity.Game.World
{
	// Token: 0x02000367 RID: 871
	public class MapBounds : MonoBehaviour
	{
		// Token: 0x06001D71 RID: 7537 RVA: 0x000AE424 File Offset: 0x000AC624
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

		// Token: 0x06001D72 RID: 7538 RVA: 0x000AE4D9 File Offset: 0x000AC6D9
		public void Awake()
		{
			this.SetSimCoordinates();
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001D73 RID: 7539 RVA: 0x000AE4E1 File Offset: 0x000AC6E1
		public Vector2r Min
		{
			get
			{
				return this.mMin;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001D74 RID: 7540 RVA: 0x000AE4E9 File Offset: 0x000AC6E9
		public Vector2r Max
		{
			get
			{
				return this.mMax;
			}
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x000AE4F1 File Offset: 0x000AC6F1
		public MapBounds()
		{
		}

		// Token: 0x04001872 RID: 6258
		public float SizeX = 1000f;

		// Token: 0x04001873 RID: 6259
		public float SizeY = 1000f;

		// Token: 0x04001874 RID: 6260
		public float CenterX = 500f;

		// Token: 0x04001875 RID: 6261
		public float CenterY = 500f;

		// Token: 0x04001876 RID: 6262
		private Vector2r mMin;

		// Token: 0x04001877 RID: 6263
		private Vector2r mMax;
	}
}
