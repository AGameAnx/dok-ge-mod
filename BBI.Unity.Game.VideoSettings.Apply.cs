using System;
using System.Runtime.CompilerServices;
using BBI.Unity.Core;
using BBI.Unity.Core.Rendering;
using BBI.Unity.Core.World;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Events;
using LitJson;
using UnityEngine;

namespace BBI.Unity.Game
{
	// Token: 0x02000057 RID: 87
	[Serializable]
	public partial class VideoSettings : IEquatable<VideoSettings>
	{
		// Token: 0x060002F1 RID: 753
		public bool Apply()
		{
			if (this.IsDirty)
			{
				this.IsDirty = false;
				QualitySettings.SetQualityLevel((int)this.m_ShadowQuality);
				if (this.m_ShadowQuality == LODGroupController.ModelLOD.Off)
				{
					this.ShadowDrawDistance = 0f;
				}
				else
				{
					this.ShadowDrawDistance = QualitySettings.shadowDistance;
				}
				if (this.m_AnisotropicFiltering == 0)
				{
					QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.Enable;
				}
				else
				{
					QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.ForceEnable;
				}
				Texture.SetGlobalAnisotropicFilteringLimits((this.m_AnisotropicFiltering == 0) ? -1 : this.m_AnisotropicFiltering, 16);
				ShipbreakersMain.PresentationEventSystem.Post(new QualitySettingsChangedEvent(this));
				DecalSystem.DecalDrawDistance = ShipbreakersMain.DecalDrawDistancesForLOD[(int)this.DecalDrawDistance];
				return true;
			}
			return false;
		}
	}
}
