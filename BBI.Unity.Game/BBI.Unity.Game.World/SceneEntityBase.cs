using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Simulation;
using BBI.Unity.Core.Utility;
using BBI.Unity.Game.Data;
using UnityEngine;
using System.Linq;

namespace BBI.Unity.Game.World
{
	// Token: 0x02000373 RID: 883
	public class SceneEntityBase : MonoBehaviour
	{
		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001E05 RID: 7685 RVA: 0x000B12AC File Offset: 0x000AF4AC
		public AssetContainer EntityToSpawn
		{
			get
			{
				if (this.m_EntityToSpawn != null)
				{
					return this.m_EntityToSpawn;
				}
				Log.Error(Log.Channel.Gameplay, "Tried to get EntityToSpawn in SceneEntity but it is null!", new object[0]);
				return null;
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001E06 RID: 7686 RVA: 0x000B12D9 File Offset: 0x000AF4D9
		public string TypeID
		{
			get
			{
				if (this.m_EntityToSpawn != null)
				{
					return this.m_EntityToSpawn.name;
				}
				Log.Error(Log.Channel.Gameplay, "Tried to get m_EntityToSpawn's name, but m_EntityToSpawn was null!", new object[0]);
				return string.Empty;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001E07 RID: 7687 RVA: 0x000B130F File Offset: 0x000AF50F
		protected Vector2r Position
		{
			get
			{
				if (MapModManager.BannedEntities.Contains<string>(this.TypeID) && MapModManager.CustomLayout)
				{
					return new Vector2r(Fixed64.FromInt(0xf4240), Fixed64.FromInt(0xf4240));
				}
				return VectorHelper.UnityVector3ToSimVector2(base.transform.position, 0.25f);
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001E08 RID: 7688 RVA: 0x000B1326 File Offset: 0x000AF526
		protected Fixed64 Height
		{
			get
			{
				return Fixed64.UnsafeFromFloat(base.transform.position.y, 0.25f);
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001E09 RID: 7689 RVA: 0x000B1342 File Offset: 0x000AF542
		protected Orientation2 Orientation
		{
			get
			{
				return Orientation2.FromDirectionOrDefault(VectorHelper.UnityVector3ToSimVector2(base.transform.forward), Orientation2.LocalForward);
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001E0A RID: 7690 RVA: 0x000B1360 File Offset: 0x000AF560
		protected Vector3r ModelOrientationEulersDegrees
		{
			get
			{
				return VectorHelper.UnityVector3ToSimVector3(base.transform.rotation.eulerAngles);
			}
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x000B1388 File Offset: 0x000AF588
		protected string[] GetAllTags()
		{
			if (this.m_AdditionalTags == null)
			{
				return new string[0];
			}
			List<string> list = new List<string>(this.m_AdditionalTags.Length + 1);
			list.AddRange(this.m_AdditionalTags);
			if (this.m_TagWithName)
			{
				list.Add(base.gameObject.name);
			}
			return list.ToArray();
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x000B13E0 File Offset: 0x000AF5E0
		public SceneEntityDescriptor CreateDataDescriptor()
		{
			SceneEntityDescriptor result = null;
			if (this.m_EntityToSpawn != null || this is SceneRelicEntity || this is SceneAIHintEntity)
			{
				result = this.GetDataDescriptor();
			}
			else
			{
				Log.Error(Log.Channel.Gameplay, "Entity to spawn was null! Scene Entity object {0} will not function!", new object[]
				{
					base.gameObject.name
				});
			}
			return result;
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x000B143C File Offset: 0x000AF63C
		protected virtual SceneEntityDescriptor GetDataDescriptor()
		{
			if (this.mDescriptor == null)
			{
				this.mDescriptor = SceneEntityDescriptor.CreateEntityBaseDescriptor(this.TypeID, this.Position, this.Orientation, this.GetAllTags(), this.m_SkipWhenPersisting);
			}
			return this.mDescriptor;
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x000B1678 File Offset: 0x000AF878
		public static IEnumerable<SceneEntityDescriptor> GetAllEntityCreationData(bool includeMissionOnlyEntities, bool includeArtifactRetrievalOnlyEntities)
		{
			IEnumerable<SceneEntityBase> bases = SceneHelper.FindComponentsOfType<SceneEntityBase>(true);
			foreach (SceneEntityBase sceneEntity in bases)
			{
				SceneEntityDescriptor data = sceneEntity.CreateDataDescriptor();
				if (data != null)
				{
					if ((includeMissionOnlyEntities || !data.MissionsOnly) && (includeArtifactRetrievalOnlyEntities || !data.RetrievalOnly))
					{
						yield return data;
					}
					else
					{
						sceneEntity.SetEnable(false, true);
					}
				}
			}
			yield break;
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x000B169C File Offset: 0x000AF89C
		public void SetEnable(bool enable, bool addExcludedSuffix)
		{
			base.gameObject.SetActive(enable);
			if (addExcludedSuffix)
			{
				base.gameObject.name = string.Format("{0}_Excluded", base.gameObject.name);
			}
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x000B16CD File Offset: 0x000AF8CD
		public SceneEntityBase()
		{
		}

		// Token: 0x040018BD RID: 6333
		private const float kFloatPointWorkingPrecision = 0.25f;

		// Token: 0x040018BE RID: 6334
		[SerializeField]
		protected AssetContainer m_EntityToSpawn;

		// Token: 0x040018BF RID: 6335
		[SerializeField]
		protected bool m_TagWithName = true;

		// Token: 0x040018C0 RID: 6336
		[SerializeField]
		protected string[] m_AdditionalTags;

		// Token: 0x040018C1 RID: 6337
		[SerializeField]
		protected bool m_SkipWhenPersisting;

		// Token: 0x040018C2 RID: 6338
		public Entity EntityID = Entity.None;

		// Token: 0x040018C3 RID: 6339
		protected SceneEntityDescriptor mDescriptor;
	}
}
