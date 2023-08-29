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
	public class SceneEntityBase : MonoBehaviour
	{
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

		protected Fixed64 Height
		{
			get
			{
				return Fixed64.UnsafeFromFloat(base.transform.position.y, 0.25f);
			}
		}

		protected Orientation2 Orientation
		{
			get
			{
				return Orientation2.FromDirectionOrDefault(VectorHelper.UnityVector3ToSimVector2(base.transform.forward), Orientation2.LocalForward);
			}
		}

		protected Vector3r ModelOrientationEulersDegrees
		{
			get
			{
				return VectorHelper.UnityVector3ToSimVector3(base.transform.rotation.eulerAngles);
			}
		}

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

		protected virtual SceneEntityDescriptor GetDataDescriptor()
		{
			if (this.mDescriptor == null)
			{
				this.mDescriptor = SceneEntityDescriptor.CreateEntityBaseDescriptor(this.TypeID, this.Position, this.Orientation, this.GetAllTags(), this.m_SkipWhenPersisting);
			}
			return this.mDescriptor;
		}

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

		public void SetEnable(bool enable, bool addExcludedSuffix)
		{
			base.gameObject.SetActive(enable);
			if (addExcludedSuffix)
			{
				base.gameObject.name = string.Format("{0}_Excluded", base.gameObject.name);
			}
		}

		public SceneEntityBase()
		{
		}

		private const float kFloatPointWorkingPrecision = 0.25f;

		[SerializeField]
		protected AssetContainer m_EntityToSpawn;

		[SerializeField]
		protected bool m_TagWithName = true;

		[SerializeField]
		protected string[] m_AdditionalTags;

		[SerializeField]
		protected bool m_SkipWhenPersisting;

		public Entity EntityID = Entity.None;

		protected SceneEntityDescriptor mDescriptor;
	}
}
