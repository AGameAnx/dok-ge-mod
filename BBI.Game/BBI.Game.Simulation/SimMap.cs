using System;
using System.Collections;
using System.Collections.Generic;
using BBI.Core;
using BBI.Core.ComponentModel;
using BBI.Core.IO.Streams;
using BBI.Core.Pathfinding;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Data;
using BBI.Game.Utility;

namespace BBI.Game.Simulation
{
	internal sealed class SimMap : IMapQuerier
	{
		public Vector2r Min { get; set; }

		public Vector2r Max { get; set; }

		public int AmbientHeatPoints { get; set; }

		public SimHeightMap HeightMap
		{
			get
			{
				return this.mHeightMap;
			}
		}

		public EntitySpatialHashTable SpatialHash { get; set; }

		public List<Shape> DynamicLineOfFireBlockers
		{
			get
			{
				return this.mDynamicLineOfFireBlockers;
			}
		}

		public SimMap(SimHeightMap heightMap, int ambientHeatPoints)
		{
			this.mHeightMap = heightMap;
			this.AmbientHeatPoints = ambientHeatPoints;
			this.mDynamicLineOfFireBlockers = new List<Shape>(200);
		}

		public SimMap(Vector2r min, Vector2r max, SimHeightMap heightMap, int ambientHeatPoints) : this(heightMap, ambientHeatPoints)
		{
			this.Min = min;
			this.Max = max;
			this.SpatialHash = new EntitySpatialHashTable(min, max);
			this.mPathfindingObstaclesHash = new QuadTree(min, max);
			this.mStaticLineOfFireHash = new QuadTree(min, max);
			this.mPathfindingObstaclesHash.Bake(2, 6);
			this.mStaticLineOfFireHash.Bake(2, 6);
		}

		public void ModifyAmbientHeat(int delta)
		{
			this.AmbientHeatPoints = Math.Max(this.AmbientHeatPoints + delta, 0);
		}

		public void AddObstacle(ConvexBase obstacleShape, UnitClass restrictedTypes, bool blocksLOF, bool blocksAllHeights)
		{
			MapObstacle mapObstacle = new MapObstacle(blocksLOF, blocksAllHeights, restrictedTypes, obstacleShape);
			this.AddObstacle(mapObstacle);
		}

		public void AddObstacle(MapObstacle mapObstacle)
		{
			if (mapObstacle != null)
			{
				if (mapObstacle.ObstacleShape.LayerFlag > 0U)
				{
					this.mPathfindingObstaclesHash.AddShape(mapObstacle.ObstacleShape);
				}
				if (mapObstacle.BlocksLineOfFire)
				{
					if (mapObstacle.ObstacleShape.LayerFlag == 0U)
					{
						mapObstacle.ObstacleShape.SetLayerFlag(1073741824U);
					}
					this.mStaticLineOfFireHash.AddShape(mapObstacle.ObstacleShape);
					if (mapObstacle.BlockAllHeights)
					{
						this.mAllHeightBlockers.Add(mapObstacle.ObstacleShape);
					}
				}
			}
		}

		public void AddRidgeLine(SimRidgeLine ridgeLine)
		{
			foreach (Capsule2 shape in ridgeLine.GetCapsuleSequence())
			{
				this.mStaticLineOfFireHash.AddShape(shape);
			}
		}

		public void BakeSpatialPartition(NavMeshAttributes[] registeredNavMeshAttributes)
		{
			this.mPathfindingObstaclesHash.Bake(2, 6);
			this.mStaticLineOfFireHash.Bake(2, 6);
			if (!registeredNavMeshAttributes.IsNullOrEmpty<NavMeshAttributes>())
			{
				foreach (NavMeshAttributes navMeshAttributes in registeredNavMeshAttributes)
				{
					PathNodeNavigationCache pathNodeNavigationCache = new PathNodeNavigationCache();
					pathNodeNavigationCache.BakeNavigationCache(this.mPathfindingObstaclesHash.AllObstacles, this.Min, this.Max, navMeshAttributes.DistanceFromObstacles, navMeshAttributes.DistanceErrorPercentageTolerance, (uint)navMeshAttributes.BlockedBy);
					this.AddBakedPathNodeCache(navMeshAttributes, pathNodeNavigationCache);
				}
			}
			if (this.mBakedMeshes.Count == 0)
			{
				Log.Warn(Log.Channel.Core, "No nav meshes registered! Pathfinding might be broken!", new object[0]);
			}
		}

		public void AddBakedPathNodeCache(NavMeshAttributes attributes, PathNodeNavigationCache bakedCache)
		{
			this.mBakedMeshes[attributes] = bakedCache;
		}

		public PathNodeNavigationCache GetNavMeshFor(NavMeshAttributes navAttributes)
		{
			if (navAttributes != null)
			{
				PathNodeNavigationCache result;
				if (this.mBakedMeshes.TryGetValue(navAttributes, out result))
				{
					return result;
				}
			}
			else
			{
				Log.Warn(Log.Channel.Core, "A unit or formation has a null nav mesh assigned to it!", new object[0]);
			}
			return null;
		}

		internal bool LineOfFireValid(Segment2 lineOfFire, Fixed64 fromWorldHeightOffset, Fixed64 toWorldHeightOffset, bool checkDynamicLOFBlockers)
		{
			if (!Fixed64.BigEnoughSquared(lineOfFire.LengthSqr))
			{
				return true;
			}
			if (checkDynamicLOFBlockers)
			{
				for (int i = 0; i < this.mDynamicLineOfFireBlockers.Count; i++)
				{
					Shape shape = this.mDynamicLineOfFireBlockers[i];
					Vector2r vector2r;
					if (shape.IntersectsSegment(lineOfFire, out vector2r) && (shape.BlocksAllHeights || this.LineOfFireIsHeightBlockedByObstacle(shape, lineOfFire.Direction, lineOfFire.From, lineOfFire.To, ref vector2r, fromWorldHeightOffset, toWorldHeightOffset)))
					{
						return false;
					}
				}
			}
			using (IEnumerator<ConvexBase> enumerator = this.mStaticLineOfFireHash.BroadSearchForSegment(lineOfFire, uint.MaxValue))
			{
				while (enumerator.MoveNext())
				{
					ConvexBase convexBase = enumerator.Current;
					Vector2r vector2r2;
					if (convexBase.IntersectsSegment(lineOfFire, out vector2r2) && (this.mAllHeightBlockers.Contains(convexBase) || this.LineOfFireIsHeightBlockedByObstacle(convexBase, lineOfFire.Direction, lineOfFire.From, lineOfFire.To, ref vector2r2, fromWorldHeightOffset, toWorldHeightOffset)))
					{
						return false;
					}
				}
			}
			return true;
		}

		internal bool LineOfFireValid(Segment2 lineOfFire, Fixed64 fromWorldHeightOffset, Fixed64 toWorldHeightOffset, out Vector2r positionOfBlockage)
		{
			return this.LineOfFireValid(lineOfFire, fromWorldHeightOffset, toWorldHeightOffset, true, out positionOfBlockage);
		}

		internal bool LineOfFireValid(Segment2 lineOfFire, Fixed64 fromWorldHeightOffset, Fixed64 toWorldHeightOffset, bool checkDynamicLOFBlockers, out Vector2r positionOfBlockage)
		{
			positionOfBlockage = lineOfFire.To;
			if (!Fixed64.BigEnoughSquared(lineOfFire.LengthSqr))
			{
				return true;
			}
			bool flag = false;
			Segment2 segment = lineOfFire;
			if (checkDynamicLOFBlockers && Fixed64.BigEnoughSquared(segment.LengthSqr))
			{
				for (int i = 0; i < this.mDynamicLineOfFireBlockers.Count; i++)
				{
					Shape shape = this.mDynamicLineOfFireBlockers[i];
					Vector2r vector2r;
					if (shape.IntersectsSegment(segment, out vector2r) && (shape.BlocksAllHeights || this.LineOfFireIsHeightBlockedByObstacle(shape, lineOfFire.Direction, lineOfFire.From, lineOfFire.To, ref vector2r, fromWorldHeightOffset, toWorldHeightOffset)))
					{
						flag = true;
						positionOfBlockage = vector2r;
						segment = new Segment2(segment.From, positionOfBlockage);
						if (!Fixed64.BigEnoughSquared(segment.LengthSqr))
						{
							break;
						}
					}
				}
			}
			if (Fixed64.BigEnoughSquared(segment.LengthSqr))
			{
				using (IEnumerator<ConvexBase> enumerator = this.mStaticLineOfFireHash.BroadSearchForSegment(lineOfFire, uint.MaxValue))
				{
					while (enumerator.MoveNext())
					{
						ConvexBase convexBase = enumerator.Current;
						Vector2r vector2r2;
						if (convexBase.IntersectsSegment(segment, out vector2r2) && (this.mAllHeightBlockers.Contains(convexBase) || this.LineOfFireIsHeightBlockedByObstacle(convexBase, lineOfFire.Direction, lineOfFire.From, lineOfFire.To, ref vector2r2, fromWorldHeightOffset, toWorldHeightOffset)))
						{
							flag = true;
							positionOfBlockage = vector2r2;
							segment = new Segment2(segment.From, positionOfBlockage);
							if (!Fixed64.BigEnoughSquared(segment.LengthSqr))
							{
								break;
							}
						}
					}
				}
			}
			return !flag;
		}

		public bool LineOfFireIsHeightBlockedByObstacle(ConvexBase obstacle, Orientation2 direction, Vector2r from, Vector2r to, ref Vector2r blockagePoint, Fixed64 fromWorldHeightOffset, Fixed64 toWorldHeightOffset)
		{
			if (this.mHeightMap != null)
			{
				Fixed64 y = this.mHeightMap.GetWorldHeight(from) + fromWorldHeightOffset;
				Fixed64 x = this.mHeightMap.GetWorldHeight(to) + toWorldHeightOffset;
				Vector2r other = to - from;
				Fixed64 y2 = Fixed64.One / other.LengthSquared;
				Vector2r forward = direction.Forward;
				forward.X *= Fixed64.FromRational(1, this.mHeightMap.GridWidth) * (this.Max.X - this.Min.X);
				forward.Y *= Fixed64.FromRational(1, this.mHeightMap.GridLength) * (this.Max.Y - this.Min.Y);
				Vector2r vector2r = blockagePoint;
				for (;;)
				{
					Fixed64 @fixed = this.mHeightMap.GetWorldHeight(vector2r);
					Shape shape = obstacle as Shape;
					if (shape != null)
					{
						@fixed += shape.WorldHeightOffset;
					}
					Fixed64 x2 = (vector2r - from).Dot(other) * y2;
					Fixed64 x3 = x2 * (x - y) + y;
					if (x3 < @fixed)
					{
						break;
					}
					vector2r += forward;
					if (direction.Forward.Dot(vector2r - to) > Fixed64.Zero)
					{
						return false;
					}
					if (!obstacle.ContainsPoint(vector2r))
					{
						return false;
					}
				}
				blockagePoint = vector2r;
				return true;
			}
			return false;
		}

		public bool PositionInsideMap(Vector2r position)
		{
			return position.X >= this.Min.X && position.X <= this.Max.X && position.Y >= this.Min.Y && position.Y <= this.Max.Y;
		}

		public bool MapPositionValid(Vector2r position, UnitClass layerInfo)
		{
			bool result = true;
			if (!this.PositionInsideMap(position))
			{
				result = false;
			}
			else if (this.PositionInsideObstacles(position, layerInfo))
			{
				result = false;
			}
			return result;
		}

		public bool MapPositionValid(Vector2r position, UnitClass targetLayer, out InvalidMapPositionReason reason)
		{
			bool result = true;
			reason = InvalidMapPositionReason.Unspecified;
			if (!this.PositionInsideMap(position))
			{
				result = false;
				reason = InvalidMapPositionReason.OutOfBounds;
			}
			else if (this.PositionInsideObstacles(position, targetLayer))
			{
				result = false;
				reason = InvalidMapPositionReason.InsideObstacle;
			}
			return result;
		}

		public bool LineOfFireClear(Vector2r from, Vector2r to, Entity fromEntity, Entity targetEntity, out Vector2r positionOfBlockage)
		{
			Fixed64 fromWorldHeightOffset = Fixed64.Zero;
			if (fromEntity.IsValid())
			{
				UnitAttributes typeAttributes = fromEntity.GetTypeAttributes<UnitAttributes>();
				if (typeAttributes != null)
				{
					fromWorldHeightOffset = typeAttributes.WorldHeightOffset;
				}
			}
			Fixed64 toWorldHeightOffset = Fixed64.Zero;
			if (targetEntity.IsValid())
			{
				UnitAttributes typeAttributes2 = targetEntity.GetTypeAttributes<UnitAttributes>();
				if (typeAttributes2 != null)
				{
					toWorldHeightOffset = typeAttributes2.WorldHeightOffset;
				}
			}
			return this.LineOfFireValid(new Segment2(from, to), fromWorldHeightOffset, toWorldHeightOffset, false, out positionOfBlockage);
		}

		public Vector2r GetPositionOfHighestValueBetweenPoints(Vector2r from, Vector2r to, out Fixed64 highestPoint)
		{
			highestPoint = Fixed64.Zero;
			if (!this.PositionInsideMap(from) || !this.PositionInsideMap(to))
			{
				return Vector2r.Zero;
			}
			Vector2r b = to - from;
			highestPoint = this.mHeightMap.GetWorldHeight(from);
			int num = Fixed64.IntValue(Fixed64.Floor(b.Length * SimMap.kRaytraceInverseStepSize));
			num = Math.Min(num, 100);
			b.NormalizeAndScale(SimMap.kRaytraceStepSize);
			Vector2r vector2r = from;
			Vector2r result = from;
			for (int i = 0; i < num; i++)
			{
				vector2r += b;
				Fixed64 worldHeight = this.mHeightMap.GetWorldHeight(vector2r);
				if (worldHeight > highestPoint)
				{
					result = vector2r;
					highestPoint = worldHeight;
				}
			}
			return result;
		}

		public bool NearbyValidPositionAvailable(Vector2r position, UnitClass targetLayer, InvalidMapPositionReason reasonInvalid, Fixed64 distanceFromObstacle, out Vector2r validPosition)
		{
			int i = 0;
			validPosition = position;
			while (i < 8)
			{
				switch (reasonInvalid)
				{
				case InvalidMapPositionReason.OutOfBounds:
					validPosition.Clamp(this.Min, this.Max);
					break;
				case InvalidMapPositionReason.InsideObstacle:
				{
					ConvexBase convexBase = null;
					this.PositionInsideObstacles(position, targetLayer, out convexBase);
					if (convexBase != null)
					{
						validPosition = convexBase.PointAtMinimumDistanceClosestTo(position, distanceFromObstacle);
					}
					break;
				}
				}
				if (this.MapPositionValid(validPosition, targetLayer, out reasonInvalid))
				{
					return true;
				}
				i++;
				position = validPosition;
			}
			return false;
		}

		public bool PositionInsideObstacles(Vector2r position, UnitClass targetLayer)
		{
			using (IEnumerator<ConvexBase> enumerator = this.mPathfindingObstaclesHash.BroadSearchForPoint(position, (uint)targetLayer))
			{
				while (enumerator.MoveNext())
				{
					ConvexBase convexBase = enumerator.Current;
					if (convexBase.ContainsPoint(position))
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool PositionInsideObstacles(Vector2r position, UnitClass targetLayer, out ConvexBase containingObstacle)
		{
			containingObstacle = null;
			using (IEnumerator<ConvexBase> enumerator = this.mPathfindingObstaclesHash.BroadSearchForPoint(position, (uint)targetLayer))
			{
				while (enumerator.MoveNext())
				{
					ConvexBase convexBase = enumerator.Current;
					if (convexBase.ContainsPoint(position))
					{
						containingObstacle = convexBase;
						return true;
					}
				}
			}
			return false;
		}

		public bool SegmentBlockedByObstacles(Segment2 segment, UnitClass targetLayer)
		{
			using (IEnumerator<ConvexBase> enumerator = this.mPathfindingObstaclesHash.BroadSearchForSegment(segment, (uint)targetLayer))
			{
				while (enumerator.MoveNext())
				{
					ConvexBase convexBase = enumerator.Current;
					if (convexBase.IntersectsSegment(segment))
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool SegmentRaycastHits(Segment2 segment, UnitClass targetLayer, out Vector2r hitLocation)
		{
			hitLocation = segment.To;
			List<ConvexBase> list = TransientLists.GetList<ConvexBase>();
			this.FindSegmentIntersectingObstacleShapes(segment, targetLayer, list);
			if (!list.IsNullOrEmpty<ConvexBase>())
			{
				foreach (ConvexBase convexBase in list)
				{
					Vector2r to;
					if (convexBase.IntersectsSegment(segment, out to))
					{
						segment = new Segment2(segment.From, to);
						if (!segment.DirectionVector.LongEnough())
						{
							break;
						}
					}
				}
				hitLocation = segment.To;
				TransientLists.ReturnList<ConvexBase>(list);
				return true;
			}
			TransientLists.ReturnList<ConvexBase>(list);
			return false;
		}

		public void FindSegmentIntersectingObstacleShapes(Segment2 segment, UnitClass targetLayer, List<ConvexBase> blockingObstaclesList)
		{
			using (IEnumerator<ConvexBase> enumerator = this.mPathfindingObstaclesHash.BroadSearchForSegment(segment, (uint)targetLayer))
			{
				while (enumerator.MoveNext())
				{
					ConvexBase convexBase = enumerator.Current;
					if (convexBase.IntersectsSegment(segment))
					{
						blockingObstaclesList.Add(convexBase);
					}
				}
			}
		}

		public IEnumerator<ConvexBase> GetObstacleCircleBroadSearch(Vector2r centerPoint, Fixed64 searchRadius, UnitClass targetLayer)
		{
			return this.mPathfindingObstaclesHash.BroadSearchForCircle(new Circle(centerPoint, searchRadius), (uint)targetLayer);
		}

		public Fixed64 GroundHeightDelta(Vector2r from, Vector2r relativeTo)
		{
			if (this.HeightMap == null)
			{
				return Fixed64.Zero;
			}
			return this.HeightMap.GetWorldHeight(from) - this.HeightMap.GetWorldHeight(relativeTo);
		}

		public void Write(BinaryStreamWriter writer)
		{
			writer.WriteVector2r(this.Min);
			writer.WriteVector2r(this.Max);
			List<ConvexBase> list = new List<ConvexBase>(this.mPathfindingObstaclesHash.AllObstacles);
			writer.WriteInt32(list.Count);
			foreach (ConvexBase convexBase in list)
			{
				writer.WriteString(convexBase.GetType().Name);
				convexBase.Write(writer);
			}
			ICollection<ConvexBase> allObstacles = this.mStaticLineOfFireHash.AllObstacles;
			writer.WriteInt32(allObstacles.Count);
			foreach (ConvexBase convexBase2 in allObstacles)
			{
				writer.WriteString(convexBase2.GetType().Name);
				convexBase2.Write(writer);
			}
			writer.WriteInt32(this.mAllHeightBlockers.Count);
			foreach (ConvexBase convexBase3 in this.mAllHeightBlockers)
			{
				writer.WriteString(convexBase3.GetType().Name);
				convexBase3.Write(writer);
			}
			writer.WriteInt32(this.mBakedMeshes.Count);
			foreach (KeyValuePair<NavMeshAttributes, PathNodeNavigationCache> keyValuePair in this.mBakedMeshes)
			{
				SerializableNavMeshAttributes serializableNavMeshAttributes = new SerializableNavMeshAttributes(keyValuePair.Key);
				serializableNavMeshAttributes.Write(writer);
				keyValuePair.Value.Write(writer);
			}
		}

		public IEnumerator Read(BinaryStreamReader reader)
		{
			this.Min = reader.ReadVector2r();
			this.Max = reader.ReadVector2r();
			if (MapModManager.overrideBounds)
			{
				this.Max = MapModManager.boundsMax;
				this.Min = MapModManager.boundsMin;
			}
			int numPathFindingObstacles = reader.ReadInt32();
			this.mPathfindingObstaclesHash = new QuadTree(this.Min, this.Max);
			this.mStaticLineOfFireHash = new QuadTree(this.Min, this.Max);
			this.mAllHeightBlockers = new HashSet<ConvexBase>();
			this.SpatialHash = new EntitySpatialHashTable(this.Min, this.Max);
			List<ConvexBase> pathObstacles = new List<ConvexBase>(numPathFindingObstacles);
			for (int i = 0; i < numPathFindingObstacles; i++)
			{
				ConvexBase shape = SimMap.ReadConvexShapeFromStream(reader);
				pathObstacles.Add(shape);
				this.AddObstacle(shape, (UnitClass)shape.LayerFlag, false, false);
				yield return null;
			}
			int numRidgeLines = reader.ReadInt32();
			for (int j = 0; j < numRidgeLines; j++)
			{
				this.mStaticLineOfFireHash.AddShape(SimMap.ReadConvexShapeFromStream(reader));
				yield return null;
			}
			yield return null;
			int numAllHeightsObstacles = reader.ReadInt32();
			for (int k = 0; k < numAllHeightsObstacles; k++)
			{
				ConvexBase allHeightsObstacle = SimMap.ReadConvexShapeFromStream(reader);
				foreach (ConvexBase convexBase in this.mStaticLineOfFireHash.AllObstacles)
				{
					if (convexBase.Equals(allHeightsObstacle))
					{
						this.mAllHeightBlockers.Add(convexBase);
						break;
					}
				}
				yield return null;
			}
			this.mPathfindingObstaclesHash.Bake(2, 6);
			yield return null;
			this.mStaticLineOfFireHash.Bake(2, 6);
			yield return null;
			int numBakedMeshes = reader.ReadInt32();
			this.mBakedMeshes = new Dictionary<NavMeshAttributes, PathNodeNavigationCache>(numBakedMeshes, SimMap.sStaticNavMeshAttributesComparer);
			for (int l = 0; l < numBakedMeshes; l++)
			{
				SerializableNavMeshAttributes navMeshAttrs = new SerializableNavMeshAttributes();
				navMeshAttrs.Read(reader);
				yield return null;
				PathNodeNavigationCache navCache = new PathNodeNavigationCache();
				navCache.Read(reader);
				yield return null;
				this.mBakedMeshes.Add(navMeshAttrs, navCache);
				yield return null;
			}
			yield break;
		}

		private static ConvexBase ReadConvexShapeFromStream(BinaryStreamReader reader)
		{
			string text = reader.ReadString();
			string a;
			if ((a = text) != null)
			{
				ConvexBase convexBase;
				if (!(a == "Capsule2"))
				{
					if (!(a == "ConvexPoly") && !(a == "ConvexPolygon"))
					{
						goto IL_47;
					}
					convexBase = new ConvexPolygon();
				}
				else
				{
					convexBase = new Capsule2();
				}
				convexBase.Read(reader);
				return convexBase;
			}
			IL_47:
			throw new NotImplementedException(string.Format("Convex shape type {0} not supported for serialization in SimMap!", text));
		}

		// Note: this type is marked as 'beforefieldinit'.
		static SimMap()
		{
		}

		public const int kMaxStepsForRaycast = 100;

		public static readonly SimMap.NavMeshAttributesComparer sStaticNavMeshAttributesComparer = new SimMap.NavMeshAttributesComparer();

		public readonly SimHeightMap mHeightMap;

		public Dictionary<NavMeshAttributes, PathNodeNavigationCache> mBakedMeshes = new Dictionary<NavMeshAttributes, PathNodeNavigationCache>(SimMap.sStaticNavMeshAttributesComparer);

		public QuadTree mPathfindingObstaclesHash;

		public HashSet<ConvexBase> mAllHeightBlockers = new HashSet<ConvexBase>();

		public QuadTree mStaticLineOfFireHash;

		public List<Shape> mDynamicLineOfFireBlockers;

		public static readonly Fixed64 kSegmentLengthThresholdSquared = Fixed64.FromInt(3600);

		public static readonly Fixed64 kRaytraceStepSize = Fixed64.FromInt(10);

		public static readonly Fixed64 kRaytraceInverseStepSize = Fixed64.Tenth(1);

		public class NavMeshAttributesComparer : IEqualityComparer<NavMeshAttributes>
		{
			public bool Equals(NavMeshAttributes x, NavMeshAttributes y)
			{
				return x.Equals(y);
			}

			public int GetHashCode(NavMeshAttributes obj)
			{
				return Checksum.Combine((int)obj.BlockedBy, obj.DistanceErrorPercentageTolerance.GetChecksum(), obj.DistanceFromObstacles.GetChecksum());
			}

			public NavMeshAttributesComparer()
			{
			}
		}
	}
}
