using System;
using BBI.Core.Data;
using BBI.Core.Utility;
using UnityEngine;

namespace BBI.Unity.Game.World
{
	public class MapBounds : MonoBehaviour
	{
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

		public void Awake()
		{
			this.SetSimCoordinates();
		}

		public Vector2r Min
		{
			get
			{
				if (MapModManager.overrideBounds)
				{
					return MapModManager.boundsMin;
				}
				return this.mMin;
			}
		}

		public Vector2r Max
		{
			get
			{
				if (MapModManager.overrideBounds)
				{
					return MapModManager.boundsMax;
				}
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
