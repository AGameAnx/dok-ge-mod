using System;
using System.Collections.Generic;
using BBI.Core.Data;
using BBI.Core.Events;
using BBI.Core.Utility;
using BBI.Game.Commands;
using BBI.Game.Events;
using BBI.Game.Simulation;
using BBI.Unity.Core.Rendering;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Events;
using BBI.Unity.Game.Tween;
using BBI.Unity.Game.World;
using UnityEngine;
using UnityEngine.Rendering;

namespace BBI.Unity.Game.Rendering
{
	// Token: 0x02000281 RID: 641
	public sealed partial class HUDMapController
	{
		// Token: 0x0600197B RID: 6523
		public HUDMapController(HUDMapControllerAsset settings, SimMapDependencies mapDependencies, UserCamera camera, ICommandScheduler simScheduler, bool showBoundariesGame, bool showBoundariesSensors)
		{
			this.mHUDEnabled = true;
			this.mFOWSimEnabled = true;
			this.mFOWRenderEnabled = true;
			this.mBoundsEnabled = true;
			this.mDefaultFOWVolumeBuffer = new CommandBuffer();
			this.mFoWCubeMeshTransform = Matrix4x4.identity;
			this.mSimScheduler = simScheduler;
			this.mMainCamera = camera.ActiveCamera;
			this.mFogBuffers = new Dictionary<HUDMapController.RenderModeType, CommandBuffer>(2, HUDMapController.sRenderModeTypeComparer);
			this.mBoundsEnabledForLevelGame = showBoundariesGame;
			this.mBoundsEnabledForLevelSensors = showBoundariesSensors;
			this.mSettings = settings;
			if (this.mSettings != null)
			{
				if (mapDependencies != null)
				{
					Vector3 min = VectorHelper.SimVector2ToUnityVector3(mapDependencies.MinPathfindingBounds, -5000f);
					Vector3 max = VectorHelper.SimVector2ToUnityVector3(mapDependencies.MaxPathfindingBounds, 5000f);
					// MOD: fix sensors on non square maps
					float s = Math.Max(max.x - min.x, max.z - min.z);
					max.x = min.x + s;
					max.z = min.z + s;
					// MOD
					this.mMapBounds = default(Bounds);
					this.mMapBounds.SetMinMax(min, max);
					this.mFOWCamera = HUDMapController.InstantiateFOWCamera(settings.FOWCameraPrefab, this.mMapBounds, ShipbreakersMain.GetDynamicRoot(ShipbreakersMain.DynamicRootIndex.Misc));
					if (this.mMainCamera != null && this.mFOWCamera != null)
					{
						if (this.mFOWCamera.targetTexture != null)
						{
							Log.Error(Log.Channel.Graphics, "RenderTexture asset incorrectly set as target for FoW camera, discarding!", new object[0]);
							this.mFOWCamera.targetTexture.Release();
						}
						this.mFOWTexture = HUDMapController.CreateFOWRenderTexture(this.mMapBounds);
						this.mBlurMaterial = new Material(settings.FOWBlurShader);
						this.mFOWCamera.targetTexture = this.mFOWTexture;
						this.mFOWCamera.AddCommandBuffer(CameraEvent.AfterForwardOpaque, HUDMapController.CreateFOWBlurCmdBuffer(this.mBlurMaterial, this.mFOWTexture));
						this.mFOWMaterial = new Material(settings.FOWMaterial);
						this.mFOWMaterial.SetTexture("_MainTex", this.mFOWTexture);
						Vector3 size = Vector3.one * 5000f + this.mMapBounds.size;
						GameObject gameObject = HUDMapController.InstantiateFOWVolume(new Bounds(this.mMapBounds.center, size));
						this.mFoWCubeMesh = gameObject.GetComponent<MeshFilter>().mesh;
						this.mFoWCubeMeshTransform = gameObject.transform.localToWorldMatrix;
						HUDMapController.RebuildFOWVolumeCmdBuffer(this.mDefaultFOWVolumeBuffer, this.mFoWCubeMesh, this.mFoWCubeMeshTransform, this.mFOWMaterial, "FOW Combine (Default)");
						this.mFogBuffers.Add(HUDMapController.RenderModeType.Default, this.mDefaultFOWVolumeBuffer);
						UnityEngine.Object.Destroy(gameObject);
						this.CreateMapBoundsBuffers(settings);
						this.SetMode(HUDMapController.RenderModeType.Default);
						this.mAnimFade = new FloatAnimFadeInOut(null, 1f, 1f, 0f);
						this.mAnimFade.FadeOutCallback = new Action(this.OnAnimFadeOut);
						this.mAnimFade.Value = 1f;
						ShipbreakersMain.PresentationEventSystem.AddHandler<SensorsManagerEvent>(new BBI.Core.Events.EventHandler<SensorsManagerEvent>(this.OnSensorsManagerEvent));
						ShipbreakersMain.PresentationEventSystem.AddHandler<HUDMapToggleEvent>(new BBI.Core.Events.EventHandler<HUDMapToggleEvent>(this.OnHUDMapToggleEvent));
						ShipbreakersMain.PresentationEventSystem.AddHandler<HUDMapSettingsEvent>(new BBI.Core.Events.EventHandler<HUDMapSettingsEvent>(this.OnHUDMapSettingsEvent));
						ShipbreakersMain.PresentationEventSystem.AddHandler<QualitySettingsChangedEvent>(new BBI.Core.Events.EventHandler<QualitySettingsChangedEvent>(this.OnQualitySettingsChanged));
						ShipbreakersMain.PresentationEventSystem.AddHandler<TacticalOverlayToggleEvent>(new BBI.Core.Events.EventHandler<TacticalOverlayToggleEvent>(this.OnTacticalOverlayToggleEvent));
						ShipbreakersMain.SimToPresentationEventSystem.AddHandler<MatchGameOverEvent>(new BBI.Core.Events.EventHandler<MatchGameOverEvent>(this.OnMatchGameOverEvent));
						ShipbreakersMain.SimToPresentationEventSystem.AddHandler<ReplayCompleteEvent>(new BBI.Core.Events.EventHandler<ReplayCompleteEvent>(this.OnReplayCompleteEvent));
					}
					else
					{
						Log.Error(Log.Channel.Graphics, "Cannot find child Camera for provided UserCamera", new object[]
						{
							camera
						});
					}
					DecalSystem.sFOWCamera = this.mFOWCamera;
					return;
				}
				Log.Error(Log.Channel.Graphics, "[FogOfWar] - Cannot initialize Presentaiton side FoW for level! Make sure it contains a SetRenderSettings and has particle occlusion baked!", new object[0]);
			}
		}
	}
}
