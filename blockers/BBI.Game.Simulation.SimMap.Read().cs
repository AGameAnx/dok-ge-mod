using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BBI.Core.ComponentModel;
using BBI.Core.IO.Streams;
using BBI.Core.Pathfinding;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Data;

namespace BBI.Game.Simulation
{
	// Token: 0x020003E2 RID: 994
	internal sealed partial class SimMap : IMapQuerier
	{
		// Token: 0x060014AB RID: 5291 RVA: 0x0000D904 File Offset: 0x0000BB04
		public IEnumerator Read(BinaryStreamReader reader)
		{
			this.Min = reader.ReadVector2r();
			this.Max = reader.ReadVector2r();
			// MOD
			if (MapModManager.overrideBounds) 
			{
				this.Max = MapModManager.boundsMax;
				this.Min = MapModManager.boundsMin;
			}
			// MOD
			int numPathFindingObstacles = reader.ReadInt32();
			this.mPathfindingObstaclesHash = new QuadTree(this.Min, this.Max);
			this.mStaticLineOfFireHash = new QuadTree(this.Min, this.Max);
			this.mAllHeightBlockers = new HashSet<ConvexBase>();
			this.SpatialHash = new EntitySpatialHashTable(this.Min, this.Max);
			List<ConvexBase> pathObstacles = new List<ConvexBase>(numPathFindingObstacles);
			int num;
			for (int i = 0; i < numPathFindingObstacles; i = num + 1)
			{
				ConvexBase convexBase = SimMap.ReadConvexShapeFromStream(reader);
				pathObstacles.Add(convexBase);
				this.AddObstacle(convexBase, (UnitClass)convexBase.LayerFlag, false, false);
				yield return null;
				num = i;
			}
			int numRidgeLines = reader.ReadInt32();
			for (int i = 0; i < numRidgeLines; i = num + 1)
			{
				this.mStaticLineOfFireHash.AddShape(SimMap.ReadConvexShapeFromStream(reader));
				yield return null;
				num = i;
			}
			yield return null;
			int numAllHeightsObstacles = reader.ReadInt32();
			for (int i = 0; i < numAllHeightsObstacles; i = num + 1)
			{
				ConvexBase other = SimMap.ReadConvexShapeFromStream(reader);
				foreach (ConvexBase convexBase2 in this.mStaticLineOfFireHash.AllObstacles)
				{
					if (convexBase2.Equals(other))
					{
						this.mAllHeightBlockers.Add(convexBase2);
						break;
					}
				}
				yield return null;
				num = i;
			}
			this.mPathfindingObstaclesHash.Bake(2, 6);
			yield return null;
			this.mStaticLineOfFireHash.Bake(2, 6);
			yield return null;
			int numBakedMeshes = reader.ReadInt32();
			this.mBakedMeshes = new Dictionary<NavMeshAttributes, PathNodeNavigationCache>(numBakedMeshes, SimMap.sStaticNavMeshAttributesComparer);
			for (int i = 0; i < numBakedMeshes; i = num + 1)
			{
				SerializableNavMeshAttributes navMeshAttrs = new SerializableNavMeshAttributes();
				navMeshAttrs.Read(reader);
				yield return null;
				PathNodeNavigationCache navCache = new PathNodeNavigationCache();
				navCache.Read(reader);
				yield return null;
				this.mBakedMeshes.Add(navMeshAttrs, navCache);
				yield return null;
				navMeshAttrs = null;
				navCache = null;
				num = i;
				navMeshAttrs = null;
				navCache = null;
			}
			yield break;
		}
	}
}
