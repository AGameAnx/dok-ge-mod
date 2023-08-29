using System;
using BBI.Core.ComponentModel;
using BBI.Core.Utility;
using BBI.Game.Simulation;
using BBI.Unity.Game.HUD;
using UnityEngine;

namespace BBI.Unity.Game.World
{
	public class ExtractionZoneView
	{
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

		public Entity Entity
		{
			get
			{
				return this.mEntity;
			}
		}

		public UISprite Circle
		{
			get
			{
				return this.mCircle;
			}
		}

		public NGUIIconController Arrow
		{
			get
			{
				return this.mArrow;
			}
		}

		public ExtractionZoneView(Entity entity, ExtractionZoneDescriptor descriptor, UISprite circle, GameObject arrowPrefab, float altitudeOffset, string label, HUDSystem hudSystem, Color colour)
		{
			this.mEntity = entity;
			this.mCircle = circle;
			this.CreateArrow(hudSystem, entity, arrowPrefab, (SceneExtractionZoneEntity)descriptor.SceneObject, altitudeOffset, label, colour);
		}

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

		private void CreateArrow(HUDSystem hudSystem, Entity entity, GameObject iconPrefab, SceneExtractionZoneEntity sceneObject, float altitudeOffset, string labelText, Color iconColour)
		{
		}

		private const string kExtractionArrowLabelVariableName = "ObjectiveLabel";

		private const string kExtractionArrowSpriteVariableName = "Sprite";

		private readonly Entity mEntity;

		private readonly UISprite mCircle;

		private Promise<NGUIIconController> mArrowPromise;
	}
}
