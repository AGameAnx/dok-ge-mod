using System;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Data;
using BBI.Game.Data.Queries;

namespace BBI.Game.Simulation
{
	// Token: 0x02000398 RID: 920
	public class SceneEntityDescriptor
	{
		// Token: 0x0600128A RID: 4746 RVA: 0x00065418 File Offset: 0x00063618
		private SceneEntityDescriptor(string typeID, Vector2r position, Orientation2 orientation, string[] tags, SceneEntityType type, bool skipWhenPersisting) : this(typeID, position, false, Fixed64.Zero, orientation, tags, type, null, true, skipWhenPersisting)
		{
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x0006543C File Offset: 0x0006363C
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

		// Token: 0x0600128C RID: 4748 RVA: 0x00065528 File Offset: 0x00063728
		public static TriggerCircleDescriptor CreateTriggerCircleDescriptor(string typeID, Vector2r position, Orientation2 orientation, string[] tags, object sceneObject, QueryContainerBase queryContainer, TriggerCircleDescriptor.PrefabType type, bool hiddenIn3D, TriggerCircleDescriptor.CustomVisibility customVisibility, Fixed64 triggerRadius, float arrowAltitudeOffset, int markerRadius, bool projectRingToGround, string markerLabel, bool startEnabled, bool triggeredByDetection)
		{
			return new TriggerCircleDescriptor(typeID, position, orientation, tags, sceneObject, queryContainer, type, hiddenIn3D, customVisibility, triggerRadius, arrowAltitudeOffset, markerRadius, projectRingToGround, markerLabel, startEnabled, triggeredByDetection);
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x00065556 File Offset: 0x00063756
		public static SceneEntityDescriptor CreateEntityBaseDescriptor(string typeID, Vector2r position, Orientation2 orientation, string[] tags, bool skipWhenPersisting)
		{
			return new SceneEntityDescriptor(typeID, position, orientation, tags, SceneEntityType.Base, skipWhenPersisting);
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x00065564 File Offset: 0x00063764
		public static ResourcePointDescriptor CreateResourcePointDescriptor(string typeID, Vector2r position, bool useHeight, Fixed64 height, Orientation2 orientation, Vector3r orientationEulersDegrees, string[] tags, bool skipWhenPersisting, object sceneObject, ResourceAttributes resourceAttributes, DetectableAttributes detectableAttributes, bool startDisabled, Vector2r simLocalSpawnPositionOffset, Fixed64 simLocalSpawnOrientationOffsetDegrees)
		{
			return new ResourcePointDescriptor(typeID, position, useHeight, height, orientation, orientationEulersDegrees, tags, skipWhenPersisting, sceneObject, resourceAttributes, detectableAttributes, startDisabled, simLocalSpawnPositionOffset, simLocalSpawnOrientationOffsetDegrees);
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x00065590 File Offset: 0x00063790
		public static WreckDescriptor CreateWreckPointDescriptor(string typeID, Vector2r position, bool useHeight, Fixed64 height, Orientation2 orientation, Vector3r orientationEulersDegrees, string[] tags, bool skipWhenPersisting, object sceneObject, ResourceAttributes resourceAttributes, DetectableAttributes detectableAttributes, WreckAttributes wreckAttributes, ShapeAttributes shapeAttributes, bool startDisabled, Vector2r simLocalSpawnPositionOffset, Fixed64 simLocalSpawnOrientationOffsetDegrees)
		{
			return new WreckDescriptor(typeID, position, useHeight, height, orientation, orientationEulersDegrees, tags, skipWhenPersisting, sceneObject, resourceAttributes, detectableAttributes, wreckAttributes, shapeAttributes, startDisabled, simLocalSpawnPositionOffset, simLocalSpawnOrientationOffsetDegrees);
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x000655BE File Offset: 0x000637BE
		public static RelicDescriptor CreateRelicDescriptor(Vector2r position, Orientation2 orientation, string[] tags, object sceneObject, RelicProbabilityAttributes relicProbabilityAttribs)
		{
			return new RelicDescriptor(string.Empty, position, orientation, tags, sceneObject, relicProbabilityAttribs);
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x000655D0 File Offset: 0x000637D0
		public static ExtractionZoneDescriptor CreateExtractionZoneDescriptor(string typeID, Vector2r position, Orientation2 orientation, string[] tags, object sceneObject, ExtractionZoneAttributes extractionZoneAttributes, int teamSpawnIndex)
		{
			return new ExtractionZoneDescriptor(typeID, position, orientation, tags, sceneObject, extractionZoneAttributes, teamSpawnIndex);
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x000655E1 File Offset: 0x000637E1
		public static AIHintDescriptor CreateAIHintDescriptor(Vector2r position, Orientation2 orientation, string[] tags, object sceneObject, AIHintType aiHintType, int aiHintData)
		{
			return new AIHintDescriptor(string.Empty, position, orientation, tags, sceneObject, aiHintType, aiHintData);
		}

		// Token: 0x04000EDD RID: 3805
		internal readonly string TypeID = string.Empty;

		// Token: 0x04000EDE RID: 3806
		public readonly Vector2r Position = Vector2r.Zero;

		// Token: 0x04000EDF RID: 3807
		public readonly bool UseHeight;

		// Token: 0x04000EE0 RID: 3808
		public readonly Fixed64 Height = Fixed64.Zero;

		// Token: 0x04000EE1 RID: 3809
		public readonly Orientation2 Orientation = Orientation2.LocalForward;

		// Token: 0x04000EE2 RID: 3810
		internal readonly string[] Tags;

		// Token: 0x04000EE3 RID: 3811
		public readonly SceneEntityType EntityType;

		// Token: 0x04000EE4 RID: 3812
		public readonly bool MissionsOnly;

		// Token: 0x04000EE5 RID: 3813
		public readonly bool RetrievalOnly;

		// Token: 0x04000EE6 RID: 3814
		public readonly object SceneObject;

		// Token: 0x04000EE7 RID: 3815
		public readonly bool StartEnabled;

		// Token: 0x04000EE8 RID: 3816
		public readonly bool SkipWhenPersisting;
	}
}
