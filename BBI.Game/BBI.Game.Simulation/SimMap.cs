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
	// Token: 0x020003C6 RID: 966
	internal sealed class SimMap : IMapQuerier
	{
		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x060013DB RID: 5083 RVA: 0x0006E7BB File Offset: 0x0006C9BB
		// (set) Token: 0x060013DC RID: 5084 RVA: 0x0006E7C3 File Offset: 0x0006C9C3
		public Vector2r Min { get; private set; }

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x060013DD RID: 5085 RVA: 0x0006E7CC File Offset: 0x0006C9CC
		// (set) Token: 0x060013DE RID: 5086 RVA: 0x0006E7D4 File Offset: 0x0006C9D4
		public Vector2r Max { get; private set; }

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x060013DF RID: 5087 RVA: 0x0006E7DD File Offset: 0x0006C9DD
		// (set) Token: 0x060013E0 RID: 5088 RVA: 0x0006E7E5 File Offset: 0x0006C9E5
		public int AmbientHeatPoints { get; private set; }

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x060013E1 RID: 5089 RVA: 0x0006E7EE File Offset: 0x0006C9EE
		public SimHeightMap HeightMap
		{
			get
			{
				return this.mHeightMap;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x0006E7F6 File Offset: 0x0006C9F6
		// (set) Token: 0x060013E3 RID: 5091 RVA: 0x0006E7FE File Offset: 0x0006C9FE
		public EntitySpatialHashTable SpatialHash { get; private set; }

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x060013E4 RID: 5092 RVA: 0x0006E807 File Offset: 0x0006CA07
		public List<Shape> DynamicLineOfFireBlockers
		{
			get
			{
				return this.mDynamicLineOfFireBlockers;
			}
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0006E810 File Offset: 0x0006CA10
		public SimMap(SimHeightMap heightMap, int ambientHeatPoints)
		{
			this.mHeightMap = heightMap;
			this.AmbientHeatPoints = ambientHeatPoints;
			this.mDynamicLineOfFireBlockers = new List<Shape>(200);
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x0006E85C File Offset: 0x0006CA5C
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

		// Token: 0x060013E7 RID: 5095 RVA: 0x0006E8C1 File Offset: 0x0006CAC1
		public void ModifyAmbientHeat(int delta)
		{
			this.AmbientHeatPoints = Math.Max(this.AmbientHeatPoints + delta, 0);
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0006E8D8 File Offset: 0x0006CAD8
		public void AddObstacle(ConvexBase obstacleShape, UnitClass restrictedTypes, bool blocksLOF, bool blocksAllHeights)
		{
			MapObstacle mapObstacle = new MapObstacle(blocksLOF, blocksAllHeights, restrictedTypes, obstacleShape);
			this.AddObstacle(mapObstacle);
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x0006E8F8 File Offset: 0x0006CAF8
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

		// Token: 0x060013EA RID: 5098 RVA: 0x0006E978 File Offset: 0x0006CB78
		public void AddRidgeLine(SimRidgeLine ridgeLine)
		{
			foreach (Capsule2 shape in ridgeLine.GetCapsuleSequence())
			{
				this.mStaticLineOfFireHash.AddShape(shape);
			}
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x0006E9CC File Offset: 0x0006CBCC
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

		// Token: 0x060013EC RID: 5100 RVA: 0x0006EA6A File Offset: 0x0006CC6A
		public void AddBakedPathNodeCache(NavMeshAttributes attributes, PathNodeNavigationCache bakedCache)
		{
			this.mBakedMeshes[attributes] = bakedCache;
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x0006EA7C File Offset: 0x0006CC7C
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

		// Token: 0x060013EE RID: 5102 RVA: 0x0006EAB0 File Offset: 0x0006CCB0
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

		// Token: 0x060013EF RID: 5103 RVA: 0x0006EBB0 File Offset: 0x0006CDB0
		internal bool LineOfFireValid(Segment2 lineOfFire, Fixed64 fromWorldHeightOffset, Fixed64 toWorldHeightOffset, out Vector2r positionOfBlockage)
		{
			return this.LineOfFireValid(lineOfFire, fromWorldHeightOffset, toWorldHeightOffset, true, out positionOfBlockage);
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x0006EBC0 File Offset: 0x0006CDC0
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

		// Token: 0x060013F1 RID: 5105 RVA: 0x0006ED54 File Offset: 0x0006CF54
		private bool LineOfFireIsHeightBlockedByObstacle(ConvexBase obstacle, Orientation2 direction, Vector2r from, Vector2r to, ref Vector2r blockagePoint, Fixed64 fromWorldHeightOffset, Fixed64 toWorldHeightOffset)
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

		// Token: 0x060013F2 RID: 5106 RVA: 0x0006EEFC File Offset: 0x0006D0FC
		private bool PositionInsideMap(Vector2r position)
		{
			return position.X >= this.Min.X && position.X <= this.Max.X && position.Y >= this.Min.Y && position.Y <= this.Max.Y;
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x0006EF70 File Offset: 0x0006D170
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

		// Token: 0x060013F4 RID: 5108 RVA: 0x0006EF9C File Offset: 0x0006D19C
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

		// Token: 0x060013F5 RID: 5109 RVA: 0x0006EFD0 File Offset: 0x0006D1D0
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

		// Token: 0x060013F6 RID: 5110 RVA: 0x0006F030 File Offset: 0x0006D230
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

		// Token: 0x060013F7 RID: 5111 RVA: 0x0006F0F0 File Offset: 0x0006D2F0
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

		// Token: 0x060013F8 RID: 5112 RVA: 0x0006F178 File Offset: 0x0006D378
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

		// Token: 0x060013F9 RID: 5113 RVA: 0x0006F1D0 File Offset: 0x0006D3D0
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

		// Token: 0x060013FA RID: 5114 RVA: 0x0006F230 File Offset: 0x0006D430
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

		// Token: 0x060013FB RID: 5115 RVA: 0x0006F288 File Offset: 0x0006D488
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

		// Token: 0x060013FC RID: 5116 RVA: 0x0006F33C File Offset: 0x0006D53C
		private void FindSegmentIntersectingObstacleShapes(Segment2 segment, UnitClass targetLayer, List<ConvexBase> blockingObstaclesList)
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

		// Token: 0x060013FD RID: 5117 RVA: 0x0006F394 File Offset: 0x0006D594
		public IEnumerator<ConvexBase> GetObstacleCircleBroadSearch(Vector2r centerPoint, Fixed64 searchRadius, UnitClass targetLayer)
		{
			return this.mPathfindingObstaclesHash.BroadSearchForCircle(new Circle(centerPoint, searchRadius), (uint)targetLayer);
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x0006F3A9 File Offset: 0x0006D5A9
		public Fixed64 GroundHeightDelta(Vector2r from, Vector2r relativeTo)
		{
			if (this.HeightMap == null)
			{
				return Fixed64.Zero;
			}
			return this.HeightMap.GetWorldHeight(from) - this.HeightMap.GetWorldHeight(relativeTo);
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x0006F3D8 File Offset: 0x0006D5D8
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

		// Token: 0x06001400 RID: 5120 RVA: 0x0006FA0C File Offset: 0x0006DC0C
		public IEnumerator Read(BinaryStreamReader reader)
		{
			this.Min = reader.ReadVector2r();
			this.Max = reader.ReadVector2r();
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

		// Token: 0x06001401 RID: 5121 RVA: 0x0006FA30 File Offset: 0x0006DC30
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

		// Token: 0x06001402 RID: 5122 RVA: 0x0006FA9D File Offset: 0x0006DC9D
		// Note: this type is marked as 'beforefieldinit'.
		static SimMap()
		{
		}

		// Token: 0x04001032 RID: 4146
		private const int kMaxStepsForRaycast = 100;

		// Token: 0x04001033 RID: 4147
		private static readonly SimMap.NavMeshAttributesComparer sStaticNavMeshAttributesComparer = new SimMap.NavMeshAttributesComparer();

		// Token: 0x04001034 RID: 4148
		private readonly SimHeightMap mHeightMap;

		// Token: 0x04001035 RID: 4149
		private Dictionary<NavMeshAttributes, PathNodeNavigationCache> mBakedMeshes = new Dictionary<NavMeshAttributes, PathNodeNavigationCache>(SimMap.sStaticNavMeshAttributesComparer);

		// Token: 0x04001036 RID: 4150
		private QuadTree mPathfindingObstaclesHash;

		// Token: 0x04001037 RID: 4151
		private HashSet<ConvexBase> mAllHeightBlockers = new HashSet<ConvexBase>();

		// Token: 0x04001038 RID: 4152
		private QuadTree mStaticLineOfFireHash;

		// Token: 0x04001039 RID: 4153
		private List<Shape> mDynamicLineOfFireBlockers;

		// Token: 0x0400103A RID: 4154
		public static readonly Fixed64 kSegmentLengthThresholdSquared = Fixed64.FromInt(3600);

		// Token: 0x0400103B RID: 4155
		private static readonly Fixed64 kRaytraceStepSize = Fixed64.FromInt(10);

		// Token: 0x0400103C RID: 4156
		private static readonly Fixed64 kRaytraceInverseStepSize = Fixed64.Tenth(1);

		// Token: 0x020003C7 RID: 967
		private class NavMeshAttributesComparer : IEqualityComparer<NavMeshAttributes>
		{
			// Token: 0x06001403 RID: 5123 RVA: 0x0006FACF File Offset: 0x0006DCCF
			public bool Equals(NavMeshAttributes x, NavMeshAttributes y)
			{
				return x.Equals(y);
			}

			// Token: 0x06001404 RID: 5124 RVA: 0x0006FAD8 File Offset: 0x0006DCD8
			public int GetHashCode(NavMeshAttributes obj)
			{
				return Checksum.Combine((int)obj.BlockedBy, obj.DistanceErrorPercentageTolerance.GetChecksum(), obj.DistanceFromObstacles.GetChecksum());
			}

			// Token: 0x06001405 RID: 5125 RVA: 0x0006FB0C File Offset: 0x0006DD0C
			public NavMeshAttributesComparer()
			{
			}
		}
	}
}
