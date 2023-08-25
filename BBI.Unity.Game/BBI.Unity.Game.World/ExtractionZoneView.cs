using System;
using BBI.Core.ComponentModel;
using BBI.Core.Utility;
using BBI.Game.Simulation;
using BBI.Unity.Game.HUD;
using UnityEngine;

namespace BBI.Unity.Game.World
{
	// Token: 0x0200036F RID: 879
	public class ExtractionZoneView
	{
		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001DC7 RID: 7623 RVA: 0x000AFC53 File Offset: 0x000ADE53
		private NGUIIconController mArrow
		{
			get
			{
				if (this.mArrowPromise == null)
				{
					return null;
				}
				return this.mArrowPromise.Current as NGUIIconController;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001DC8 RID: 7624 RVA: 0x000AFC6F File Offset: 0x000ADE6F
		public Entity Entity
		{
			get
			{
				return this.mEntity;
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001DC9 RID: 7625 RVA: 0x000AFC77 File Offset: 0x000ADE77
		public UISprite Circle
		{
			get
			{
				return this.mCircle;
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001DCA RID: 7626 RVA: 0x000AFC7F File Offset: 0x000ADE7F
		public NGUIIconController Arrow
		{
			get
			{
				return this.mArrow;
			}
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x000AFC87 File Offset: 0x000ADE87
		public ExtractionZoneView(Entity entity, ExtractionZoneDescriptor descriptor, UISprite circle, GameObject arrowPrefab, float altitudeOffset, string label, HUDSystem hudSystem, Color colour)
		{
			this.mEntity = entity;
			this.mCircle = circle;
			this.CreateArrow(hudSystem, entity, arrowPrefab, (SceneExtractionZoneEntity)descriptor.SceneObject, altitudeOffset, label, colour);
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x000AFCB9 File Offset: 0x000ADEB9
		public void Show(bool show)
		{
			if (this.mCircle != null)
			{
				NGUITools.SetActive(this.mCircle.gameObject, show);
			}
			if (this.mArrow != null)
			{
				this.mArrow.Visible = show;
			}
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x000AFCF4 File Offset: 0x000ADEF4
		public void DespawnArrowIcon(HUDSystem hudSystem)
		{
			NGUIIconController mArrow = this.mArrow;
			if (mArrow != null)
			{
				hudSystem.RetireIcon(mArrow);
			}
			else if (this.mArrowPromise != null)
			{
				hudSystem.CancelSpawnRequest(this.mArrowPromise);
			}
			this.mArrowPromise = null;
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x000AFDE8 File Offset: 0x000ADFE8
		private void CreateArrow(HUDSystem hudSystem, Entity entity, GameObject iconPrefab, SceneExtractionZoneEntity sceneObject, float altitudeOffset, string labelText, Color iconColour)
		{
		}

		// Token: 0x040018A0 RID: 6304
		private const string kExtractionArrowLabelVariableName = "ObjectiveLabel";

		// Token: 0x040018A1 RID: 6305
		private const string kExtractionArrowSpriteVariableName = "Sprite";

		// Token: 0x040018A2 RID: 6306
		private readonly Entity mEntity;

		// Token: 0x040018A3 RID: 6307
		private readonly UISprite mCircle;

		// Token: 0x040018A4 RID: 6308
		private Promise<NGUIIconController> mArrowPromise;
	}
}
