using System;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Data;
using BBI.Game.Data.Queries;

namespace BBI.Game.Simulation
{
	public class SceneEntityDescriptor
	{
		public SceneEntityDescriptor(string typeID, Vector2r position, Orientation2 orientation, string[] tags, SceneEntityType type, bool skipWhenPersisting) : this(typeID, position, false, Fixed64.Zero, orientation, tags, type, null, true, skipWhenPersisting)
		{
		}

		protected SceneEntityDescriptor(string typeID, Vector2r position, bool useHeight, Fixed64 height, Orientation2 orientation, string[] tags, SceneEntityType type, object sceneObject, bool startEnabled, bool skipWhenPersisting)
		{
			this.TypeID = typeID;
			this.Position = position;
			this.UseHeight = useHeight;
			this.Height = height;
			this.Orientation = orientation;
			this.Tags = tags;
			this.EntityType = type;
			this.SceneObject = sceneObject;
			this.StartEnabled = startEnabled;
			this.SkipWhenPersisting = skipWhenPersisting;
			switch (this.EntityType)
			{
			case SceneEntityType.Base:
			case SceneEntityType.TriggerCircle:
			case SceneEntityType.Unit:
				this.MissionsOnly = true;
				this.RetrievalOnly = false;
				return;
			case SceneEntityType.ResourcePoint:
			case SceneEntityType.AIHint:
				this.MissionsOnly = false;
				this.RetrievalOnly = false;
				return;
			case SceneEntityType.Relic:
			case SceneEntityType.ExtractionZone:
				this.MissionsOnly = false;
				this.RetrievalOnly = true;
				return;
			default:
				throw new NotImplementedException();
			}
		}

		public static TriggerCircleDescriptor CreateTriggerCircleDescriptor(string typeID, Vector2r position, Orientation2 orientation, string[] tags, object sceneObject, QueryContainerBase queryContainer, TriggerCircleDescriptor.PrefabType type, bool hiddenIn3D, TriggerCircleDescriptor.CustomVisibility customVisibility, Fixed64 triggerRadius, float arrowAltitudeOffset, int markerRadius, bool projectRingToGround, string markerLabel, bool startEnabled, bool triggeredByDetection)
		{
			return new TriggerCircleDescriptor(typeID, position, orientation, tags, sceneObject, queryContainer, type, hiddenIn3D, customVisibility, triggerRadius, arrowAltitudeOffset, markerRadius, projectRingToGround, markerLabel, startEnabled, triggeredByDetection);
		}

		public static SceneEntityDescriptor CreateEntityBaseDescriptor(string typeID, Vector2r position, Orientation2 orientation, string[] tags, bool skipWhenPersisting)
		{
			return new SceneEntityDescriptor(typeID, position, orientation, tags, SceneEntityType.Base, skipWhenPersisting);
		}

		public static ResourcePointDescriptor CreateResourcePointDescriptor(string typeID, Vector2r position, bool useHeight, Fixed64 height, Orientation2 orientation, Vector3r orientationEulersDegrees, string[] tags, bool skipWhenPersisting, object sceneObject, ResourceAttributes resourceAttributes, DetectableAttributes detectableAttributes, bool startDisabled, Vector2r simLocalSpawnPositionOffset, Fixed64 simLocalSpawnOrientationOffsetDegrees)
		{
			return new ResourcePointDescriptor(typeID, position, useHeight, height, orientation, orientationEulersDegrees, tags, skipWhenPersisting, sceneObject, resourceAttributes, detectableAttributes, startDisabled, simLocalSpawnPositionOffset, simLocalSpawnOrientationOffsetDegrees);
		}

		public static WreckDescriptor CreateWreckPointDescriptor(string typeID, Vector2r position, bool useHeight, Fixed64 height, Orientation2 orientation, Vector3r orientationEulersDegrees, string[] tags, bool skipWhenPersisting, object sceneObject, ResourceAttributes resourceAttributes, DetectableAttributes detectableAttributes, WreckAttributes wreckAttributes, ShapeAttributes shapeAttributes, bool startDisabled, Vector2r simLocalSpawnPositionOffset, Fixed64 simLocalSpawnOrientationOffsetDegrees)
		{
			return new WreckDescriptor(typeID, position, useHeight, height, orientation, orientationEulersDegrees, tags, skipWhenPersisting, sceneObject, resourceAttributes, detectableAttributes, wreckAttributes, shapeAttributes, startDisabled, simLocalSpawnPositionOffset, simLocalSpawnOrientationOffsetDegrees);
		}

		public static RelicDescriptor CreateRelicDescriptor(Vector2r position, Orientation2 orientation, string[] tags, object sceneObject, RelicProbabilityAttributes relicProbabilityAttribs)
		{
			return new RelicDescriptor(string.Empty, position, orientation, tags, sceneObject, relicProbabilityAttribs);
		}

		public static ExtractionZoneDescriptor CreateExtractionZoneDescriptor(string typeID, Vector2r position, Orientation2 orientation, string[] tags, object sceneObject, ExtractionZoneAttributes extractionZoneAttributes, int teamSpawnIndex)
		{
			return new ExtractionZoneDescriptor(typeID, position, orientation, tags, sceneObject, extractionZoneAttributes, teamSpawnIndex);
		}

		public static AIHintDescriptor CreateAIHintDescriptor(Vector2r position, Orientation2 orientation, string[] tags, object sceneObject, AIHintType aiHintType, int aiHintData)
		{
			return new AIHintDescriptor(string.Empty, position, orientation, tags, sceneObject, aiHintType, aiHintData);
		}

		internal readonly string TypeID = string.Empty;

		public readonly Vector2r Position = Vector2r.Zero;

		public readonly bool UseHeight;

		public readonly Fixed64 Height = Fixed64.Zero;

		public readonly Orientation2 Orientation = Orientation2.LocalForward;

		internal readonly string[] Tags;

		public readonly SceneEntityType EntityType;

		public readonly bool MissionsOnly;

		public readonly bool RetrievalOnly;

		public readonly object SceneObject;

		public readonly bool StartEnabled;

		public readonly bool SkipWhenPersisting;
	}
}
