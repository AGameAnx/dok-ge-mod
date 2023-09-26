using System;
using BBI.Core.Data;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using UnityEngine;

namespace BBI.Unity.Game.World
{
	public class MapBounds : MonoBehaviour
	{
		public void SetSimCoordinates()
		{
			if (MapModManager.overrideBounds)
			{
				this.mMin = MapModManager.boundsMin;
				this.mMax = MapModManager.boundsMax;
				this.SizeX = Fixed64.UnsafeFloatValue(this.mMax.X - this.mMin.X);
				this.SizeY = Fixed64.UnsafeFloatValue(this.mMax.Y - this.mMin.Y);
				this.CenterX = Fixed64.UnsafeFloatValue(this.mMin.X) + this.SizeX * 0.5f;
				this.CenterY = Fixed64.UnsafeFloatValue(this.mMin.Y) + this.SizeY * 0.5f;
			}
			else
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
		}

		public void Awake()
		{
			this.SetSimCoordinates();
		}

		public Vector2r Min
		{
			get
			{
				return this.mMin;
			}
		}

		public Vector2r Max
		{
			get
			{
				return this.mMax;
			}
		}

		public MapBounds()
		{
		}

		public float SizeX = 1000f;

		public float SizeY = 1000f;

		public float CenterX = 500f;

		public float CenterY = 500f;

		private Vector2r mMin;

		private Vector2r mMax;
	}
}
