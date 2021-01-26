using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using BBI.Core;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Events;
using BBI.Core.IO.Streams;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.AI;
using BBI.Game.Commands;
using BBI.Game.Data;
using BBI.Game.Replay;
using BBI.Game.SaveLoad;

namespace BBI.Game.Simulation
{
	// Token: 0x020003DB RID: 987
	public partial class Sim
	{
		// Token: 0x0600141D RID: 5149
		internal IEnumerator PrepareSimMap()
		{
			if (this.mSimInitializationState != Sim.InitializationState.LoadingMap)
			{
				this.mSimInitializationState = Sim.InitializationState.LoadingMap;
				SimMapDependencies mapDependencies;
				if (this.mSimStartupDependencies.Get<SimMapDependencies>(out mapDependencies))
				{
					DateTime startTime = DateTime.Now;
					SimHeightMap heightMap = (mapDependencies.HeightMapAttributes == null) ? null : new SimHeightMap(mapDependencies.HeightMapAttributes.HeightMap, mapDependencies.HeightMapAttributes.GridWidth, mapDependencies.HeightMapAttributes.GridLength, mapDependencies.HeightMapAttributes.MinBounds, mapDependencies.HeightMapAttributes.MaxBounds, mapDependencies.HeightMapAttributes.MinHeight, mapDependencies.HeightMapAttributes.MaxHeight);
					bool needsRuntimeBake = true;
					if (mapDependencies.SerializedSimMapData != null)
					{
						MemoryStream memStream = new MemoryStream(mapDependencies.SerializedSimMapData);
						BinaryStreamReader steamReader = new BinaryStreamReader(memStream);
						DateTime start = DateTime.Now;
						this.Map = new SimMap(heightMap, mapDependencies.AmbientHeatPoints);
						IEnumerator mapReadEnumerator = this.Map.Read(steamReader);
						bool canContinue = true;
						while (canContinue)
						{
							yield return null;
							try
							{
								canContinue = mapReadEnumerator.MoveNext();
							}
							catch (Exception ex)
							{
								Log.Error(Log.Channel.Core, "Failed to deserialize SimMap from baked data with exception '{0}'. Falling back to runtime mesh baking.", new object[]
								{
									ex
								});
								canContinue = false;
							}
						}
						memStream.Close();
						Log.Info(Log.Channel.Core, "Map deserialization took {0}ms", new object[]
						{
							(DateTime.Now - start).TotalMilliseconds
						});
						needsRuntimeBake = false;
						memStream = null;
						mapReadEnumerator = null;
					}
					if (needsRuntimeBake)
					{
						this.Map = new SimMap(mapDependencies.MinPathfindingBounds, mapDependencies.MaxPathfindingBounds, heightMap, mapDependencies.AmbientHeatPoints);
						TimeSpan timeSpan = DateTime.Now.Subtract(startTime);
						startTime = DateTime.Now;
						if (mapDependencies.Obstacles != null)
						{
							foreach (MapObstacle mapObstacle in mapDependencies.Obstacles)
							{
								this.Map.AddObstacle(mapObstacle);
							}
						}
						if (mapDependencies.RidgeLines != null)
						{
							foreach (SimRidgeLine ridgeLine in mapDependencies.RidgeLines)
							{
								this.Map.AddRidgeLine(ridgeLine);
							}
						}
						TimeSpan timeSpan2 = DateTime.Now.Subtract(startTime);
						startTime = DateTime.Now;
						if (this.Settings.NavMeshes != null)
{
							this.Map.BakeSpatialPartition(this.Settings.NavMeshes);
						}
						TimeSpan timeSpan3 = DateTime.Now.Subtract(startTime);
						Log.Warn(Log.Channel.Core, "Map init: {0}ms. Obstacles & ridge lines: {1}ms. Nav mesh: {2}ms", new object[]
						{
							timeSpan.TotalMilliseconds,
							timeSpan2.TotalMilliseconds,
							timeSpan3.TotalMilliseconds
						});
					}
					heightMap = null;
				}
				else
				{
					this.Map = new SimMap(new Vector2r(Fixed64.MinValue, Fixed64.MinValue), new Vector2r(Fixed64.MaxValue, Fixed64.MaxValue), null, 0);
				}
				this.mSimInitializationState = Sim.InitializationState.MapLoaded;
				// MOD
				if (MapModManager.CustomLayout) {
					bool changedNavs = false;
					if (MapModManager.DisableAllBlockers) {
						Map.mPathfindingObstaclesHash.Clear();
						changedNavs = true;
					}
					foreach (MapModManager.MapColliderData collider in MapModManager.colliders) {
						Map.AddObstacle(collider.collider, collider.mask, collider.blockLof, collider.blockAllHeights);
						if (collider.blockLof) {
							Map.mStaticLineOfFireHash.AddShape(collider.collider);
						}
						if (collider.blockAllHeights) {
							Map.mAllHeightBlockers.Add(collider.collider);
						}
						changedNavs = true;
					}
					if (changedNavs || MapModManager.overrideBounds) { // Overriding bounds also needs a re-bake
						this.Map.BakeSpatialPartition(this.Settings.NavMeshes);
					}
				}
				// MOD
				mapDependencies = null;
			}
			yield break;
		}
	}
}
