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
	public sealed class HUDMapController
	{
		public bool FOWSimEnabled
		{
			get
			{
				return this.mFOWSimEnabled;
			}
		}

		public bool FOWRenderEnabled
		{
			get
			{
				return this.mFOWRenderEnabled;
			}
		}

		public HUDMapController.RenderModeType RenderMode
		{
			get
			{
				return this.mMode;
			}
		}

		public HUDMapController(HUDMapControllerAsset settings, SimMapDependencies mapDependencies, UserCamera camera, ICommandScheduler simScheduler, bool showBoundariesGame, bool showBoundariesSensors)
		{
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
						Bounds bounds = new Bounds(this.mMapBounds.center, size);
						GameObject gameObject = HUDMapController.InstantiateFOWVolume(bounds);
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

		private static CommandBuffer CreateMapBoundsCmdBuffer(GameObject[] objs, Material mat, string name)
		{
			CommandBuffer commandBuffer = new CommandBuffer();
			commandBuffer.name = name;
			if (mat != null)
			{
				foreach (GameObject gameObject in objs)
				{
					MeshFilter component = gameObject.GetComponent<MeshFilter>();
					if (component != null)
					{
						Mesh mesh = component.mesh;
						if (mesh != null)
						{
							commandBuffer.DrawMesh(mesh, gameObject.transform.localToWorldMatrix, mat, 0, -1);
						}
					}
				}
			}
			else
			{
				Log.Error(Log.Channel.Graphics, "Failed to create map bounds buffer named '{0}' because its material was NULL!", new object[]
				{
					name
				});
			}
			return commandBuffer;
		}

		private static CommandBuffer CreateFOWBlurCmdBuffer(Material blurMaterial, RenderTexture texture)
		{
			CommandBuffer commandBuffer = new CommandBuffer();
			commandBuffer.name = "FoW Blur";
			int width = texture.width;
			int height = texture.height;
			int nameID = Shader.PropertyToID("_Temp1");
			commandBuffer.GetTemporaryRT(nameID, -1, -1, 0, texture.filterMode, texture.format);
			commandBuffer.SetGlobalVector("offsets", new Vector4(1f / (float)width, 0f, 0f, 0f));
			commandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, nameID, blurMaterial);
			commandBuffer.SetGlobalVector("offsets", new Vector4(0f, 1f / (float)height, 0f, 0f));
			commandBuffer.Blit(nameID, BuiltinRenderTextureType.CameraTarget, blurMaterial);
			commandBuffer.ReleaseTemporaryRT(nameID);
			return commandBuffer;
		}

		private static void RebuildFOWVolumeCmdBuffer(CommandBuffer bufferToPopulate, Mesh mesh, Matrix4x4 xform, Material fowMaterial, string name)
		{
			bufferToPopulate.Clear();
			bufferToPopulate.name = name;
			bufferToPopulate.DrawMesh(mesh, xform, fowMaterial, 0, -1);
		}

		private static RenderTexture CreateFOWRenderTexture(Bounds mapBounds)
		{
			float num = Mathf.Max(mapBounds.size.x, mapBounds.size.z);
			int num2 = (int)(num / 30000f * 1024f);
			return new RenderTexture(num2, num2, 0, RenderTextureFormat.R8)
			{
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear,
				antiAliasing = 8
			};
		}

		private static Camera InstantiateFOWCamera(GameObject cameraPrefab, Bounds mapBounds, Transform parent)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(cameraPrefab);
			Camera component = gameObject.GetComponent<Camera>();
			if (component != null)
			{
				component.transform.position = new Vector3(mapBounds.center.x, 12000f, mapBounds.center.z);
				float num = Mathf.Max(mapBounds.size.x, mapBounds.size.z);
				component.orthographicSize = num * 0.5f;
				component.aspect = 1f;
				component.nearClipPlane = 0f;
				component.farClipPlane = 14000f;
				component.transform.parent = parent;
			}
			else
			{
				Log.Error(gameObject, Log.Channel.Graphics, "No camera component on FOW camera prefab", new object[0]);
			}
			return component;
		}

		private static GameObject InstantiateFOWVolume(Bounds bounds)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			gameObject.transform.position = new Vector3(bounds.center.x, 0f, bounds.center.z);
			Vector3 b = new Vector3(0f, 10000f, 0f);
			gameObject.transform.localScale = bounds.size + b;
			return gameObject;
		}

		private void SetActiveFogBuffer(CommandBuffer buffer)
		{
			this.ClearActiveFogBuffer();
			if (buffer != null)
			{
				this.mActiveFogBuffer = buffer;
				this.mMainCamera.AddCommandBuffer(CameraEvent.AfterForwardAlpha, this.mActiveFogBuffer);
			}
		}

		private void ClearActiveFogBuffer()
		{
			if (this.mActiveFogBuffer != null)
			{
				this.mMainCamera.RemoveCommandBuffer(CameraEvent.AfterForwardAlpha, this.mActiveFogBuffer);
				this.mActiveFogBuffer = null;
			}
		}

		private void SetActiveBoundsBuffer(CommandBuffer buffer)
		{
			this.ClearActiveBoundsBuffer();
			if (buffer != null)
			{
				this.mActiveBoundsBuffer = buffer;
				this.mMainCamera.AddCommandBuffer(CameraEvent.AfterForwardAlpha, this.mActiveBoundsBuffer);
			}
		}

		private void ClearActiveBoundsBuffer()
		{
			if (this.mActiveBoundsBuffer != null)
			{
				this.mMainCamera.RemoveCommandBuffer(CameraEvent.AfterForwardAlpha, this.mActiveBoundsBuffer);
				this.mActiveBoundsBuffer = null;
			}
		}

		private void SetHUDEnabled(bool state)
		{
			if (state != this.mHUDEnabled)
			{
				this.mHUDEnabled = state;
				this.SetDisplayForCurrentState();
			}
		}

		private void ToggleFOWSimEnabled(bool state)
		{
			if (state != this.mFOWSimEnabled)
			{
				this.mFOWSimEnabled = state;
				SimCommandBase command = SimCommandFactory.CreateToggleFogOfWarCommand(SimController.LocalPlayerCommanderID, !this.mFOWSimEnabled);
				this.mSimScheduler.ScheduleCommand(command);
			}
		}

		private void SetMode(HUDMapController.RenderModeType mode)
		{
			this.mMode = mode;
			this.SetDisplayForCurrentState();
		}

		private void SetVisibility(bool enabled, bool boundsEnabled, HUDMapSettingsEvent.FOWToggleMode mode)
		{
			if (mode == HUDMapSettingsEvent.FOWToggleMode.All)
			{
				this.ToggleFOWSimEnabled(enabled);
			}
			this.mFOWRenderEnabled = enabled;
			this.mBoundsEnabled = boundsEnabled;
			this.SetDisplayForCurrentState();
		}

		private void SetDisplayForCurrentState()
		{
			if (this.mHUDEnabled && this.mFOWSimEnabled && this.mFOWRenderEnabled && this.mMode == HUDMapController.RenderModeType.Tactical)
			{
				CommandBuffer activeFogBuffer;
				bool flag = this.mFogBuffers.TryGetValue(this.mMode, out activeFogBuffer);
				if (flag)
				{
					this.SetActiveFogBuffer(activeFogBuffer);
				}
				else
				{
					CommandBuffer activeFogBuffer2;
					bool flag2 = this.mFogBuffers.TryGetValue(HUDMapController.RenderModeType.Default, out activeFogBuffer2);
					if (flag2)
					{
						this.SetActiveFogBuffer(activeFogBuffer2);
					}
					else
					{
						Debug.LogError("Default Command buffer  not found for FoW object");
						this.ClearActiveFogBuffer();
					}
				}
			}
			else
			{
				this.ClearActiveFogBuffer();
			}
			if (this.mHUDEnabled && this.mBoundsEnabled && (this.mSensorsViewActive || this.mMode == HUDMapController.RenderModeType.Tactical))
			{
				CommandBuffer activeBoundsBuffer = this.mSensorsViewActive ? this.mBoundsBufferSensors : this.mBoundsBufferDefault;
				this.SetActiveBoundsBuffer(activeBoundsBuffer);
				return;
			}
			this.ClearActiveBoundsBuffer();
		}

		private void CreateMapBoundsBuffers(HUDMapControllerAsset settings)
		{
			Bounds[] bounds = this.CalcMapEdgeBounds(this.mMapBounds, settings.MapBoundsThickness);
			GameObject[] array = this.CreateCubeMeshes(bounds);
			this.mBoundsMaterial = new Material(settings.MapBoundsMaterial);
			this.mBoundsBufferDefault = HUDMapController.CreateMapBoundsCmdBuffer(array, this.mBoundsMaterial, "HUDMap (Bounds)");
			this.mBoundsMaterialSensors = new Material(settings.MapBoundsMaterialSensors);
			this.mBoundsBufferSensors = HUDMapController.CreateMapBoundsCmdBuffer(array, this.mBoundsMaterialSensors, "HUDMap (Bounds-Sensors)");
			foreach (GameObject obj in array)
			{
				UnityEngine.Object.Destroy(obj);
			}
		}

		private Bounds[] CalcMapEdgeBounds(Bounds bounds, float thickness)
		{
			Bounds[] array = new Bounds[4];
			Vector3 size = new Vector3(thickness, bounds.size.y, bounds.size.z + thickness * 2f);
			Vector3 b = new Vector3(bounds.extents.x + thickness * 0.5f, 0f, 0f);
			array[0] = new Bounds(bounds.center + b, size);
			array[1] = new Bounds(bounds.center - b, size);
			Vector3 size2 = new Vector3(bounds.size.x + thickness * 2f, bounds.size.y, thickness);
			Vector3 b2 = new Vector3(0f, 0f, bounds.extents.z + thickness * 0.5f);
			array[2] = new Bounds(bounds.center + b2, size2);
			array[3] = new Bounds(bounds.center - b2, size2);
			return array;
		}

		private GameObject[] CreateCubeMeshes(Bounds[] bounds)
		{
			GameObject[] array = new GameObject[bounds.Length];
			for (int i = 0; i < bounds.Length; i++)
			{
				array[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
				array[i].transform.localScale = bounds[i].size;
				array[i].transform.position = bounds[i].center;
			}
			return array;
		}

		private bool BoundsVisiblityForSensorsState()
		{
			if (!this.mSensorsViewActive)
			{
				return this.mBoundsEnabledForLevelGame;
			}
			return this.mBoundsEnabledForLevelSensors;
		}

		private void OnHUDMapToggleEvent(HUDMapToggleEvent ev)
		{
			this.SetHUDEnabled(ev.IsVisible);
		}

		private void OnSensorsManagerEvent(SensorsManagerEvent ev)
		{
			this.mSensorsViewActive = ev.Active;
			this.SetVisibility(!ev.Active, this.BoundsVisiblityForSensorsState(), HUDMapSettingsEvent.FOWToggleMode.RenderingOnly);
		}

		private void OnTacticalOverlayToggleEvent(TacticalOverlayToggleEvent ev)
		{
			if (ev.Enabled)
			{
				this.SetMode(HUDMapController.RenderModeType.Tactical);
				return;
			}
			this.SetMode(HUDMapController.RenderModeType.Default);
		}

		private void OnHUDMapSettingsEvent(HUDMapSettingsEvent e)
		{
			if (!e.Animate)
			{
				this.SetVisibility(e.IsVisible, e.IsVisible && this.BoundsVisiblityForSensorsState(), e.Mode);
				this.mAnimFade.Value = (e.IsVisible ? 1f : 0f);
				return;
			}
			if (e.IsVisible)
			{
				this.SetVisibility(true, this.BoundsVisiblityForSensorsState(), e.Mode);
				this.mAnimFade.AnimateFadeIn();
				return;
			}
			this.mAnimToggleMode = e.Mode;
			this.mAnimFade.AnimateFadeOut();
		}

		private void OnMatchGameOverEvent(MatchGameOverEvent e)
		{
			this.SetHUDEnabled(false);
		}

		private void OnReplayCompleteEvent(ReplayCompleteEvent e)
		{
			this.SetHUDEnabled(false);
		}

		private void OnAnimFadeOut()
		{
			this.SetVisibility(false, false, this.mAnimToggleMode);
		}

		private void OnQualitySettingsChanged(QualitySettingsChangedEvent ev)
		{
			HUDMapController.RebuildFOWVolumeCmdBuffer(this.mDefaultFOWVolumeBuffer, this.mFoWCubeMesh, this.mFoWCubeMeshTransform, this.mFOWMaterial, "FOW Combine (Default)");
		}

		public void Update()
		{
			Vector3 from = this.mMainCamera.transform.rotation * Vector3.forward;
			float value = Vector3.Angle(from, Vector3.down);
			float time = Mathf.Clamp01(Mathf.InverseLerp(0f, 90f, value));
			bool flag = this.mMode == HUDMapController.RenderModeType.Default;
			float num = (flag && this.mSettings != null) ? this.mSettings.FadeCurve.Evaluate(time) : 1f;
			num *= this.mAnimFade.Value;
			Shader.SetGlobalFloat("_HUDHorizonFade", num);
			float num2 = Math.Max(this.mMapBounds.size.x, this.mMapBounds.size.z);
			Shader.SetGlobalVector("_NavRegionOrigin", this.mMapBounds.min + new Vector3(this.mMapBounds.size.x - num2, 0f, this.mMapBounds.size.z - num2) * 0.5f);
			Vector3 v = new Vector3(1f / num2, 1f / this.mMapBounds.size.y, 1f / num2);
			Shader.SetGlobalVector("_NavInverseRegionSize", v);
		}

		public void Cleanup()
		{
			this.ClearActiveFogBuffer();
			this.ClearActiveBoundsBuffer();
			UnityEngine.Object.Destroy(this.mFOWCamera.gameObject);
			this.mFOWCamera = null;
			UnityEngine.Object.Destroy(this.mFOWTexture);
			this.mFOWTexture = null;
			UnityEngine.Object.Destroy(this.mFOWMaterial);
			this.mFOWMaterial = null;
			UnityEngine.Object.Destroy(this.mBlurMaterial);
			this.mBlurMaterial = null;
			UnityEngine.Object.Destroy(this.mBoundsMaterial);
			this.mBoundsMaterial = null;
			UnityEngine.Object.Destroy(this.mBoundsMaterialSensors);
			this.mBoundsMaterialSensors = null;
			this.mBoundsBufferDefault.Dispose();
			this.mBoundsBufferDefault = null;
			this.mBoundsBufferSensors.Dispose();
			this.mBoundsBufferSensors = null;
			this.mDefaultFOWVolumeBuffer.Dispose();
			this.mDefaultFOWVolumeBuffer = null;
			this.mFoWCubeMesh = null;
			this.mAnimFade.CancelAnimation();
			this.mAnimFade = null;
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<SensorsManagerEvent>(new BBI.Core.Events.EventHandler<SensorsManagerEvent>(this.OnSensorsManagerEvent));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<HUDMapToggleEvent>(new BBI.Core.Events.EventHandler<HUDMapToggleEvent>(this.OnHUDMapToggleEvent));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<HUDMapSettingsEvent>(new BBI.Core.Events.EventHandler<HUDMapSettingsEvent>(this.OnHUDMapSettingsEvent));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<TacticalOverlayToggleEvent>(new BBI.Core.Events.EventHandler<TacticalOverlayToggleEvent>(this.OnTacticalOverlayToggleEvent));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<QualitySettingsChangedEvent>(new BBI.Core.Events.EventHandler<QualitySettingsChangedEvent>(this.OnQualitySettingsChanged));
			ShipbreakersMain.SimToPresentationEventSystem.RemoveHandler<MatchGameOverEvent>(new BBI.Core.Events.EventHandler<MatchGameOverEvent>(this.OnMatchGameOverEvent));
			ShipbreakersMain.SimToPresentationEventSystem.RemoveHandler<ReplayCompleteEvent>(new BBI.Core.Events.EventHandler<ReplayCompleteEvent>(this.OnReplayCompleteEvent));
		}

		// Note: this type is marked as 'beforefieldinit'.
		static HUDMapController()
		{
		}

		private const float kFOWMaxWorldSize = 30000f;

		private const float kFOWMaxTextureSize = 1024f;

		private const float kFOWSizeOffset = 2000f;

		private const float kFOWVolumeBorder = 5000f;

		private static readonly EnumComparer<HUDMapController.RenderModeType> sRenderModeTypeComparer = new EnumComparer<HUDMapController.RenderModeType>();

		private Camera mMainCamera;

		private bool mSensorsViewActive;

		private HUDMapControllerAsset mSettings;

		private bool mHUDEnabled = true;

		private HUDMapController.RenderModeType mMode;

		private bool mFOWSimEnabled = true;

		private bool mFOWRenderEnabled = true;

		private Dictionary<HUDMapController.RenderModeType, CommandBuffer> mFogBuffers;

		private CommandBuffer mActiveFogBuffer;

		private Camera mFOWCamera;

		private Material mFOWMaterial;

		private RenderTexture mFOWTexture;

		private FloatAnimFadeInOut mAnimFade;

		private HUDMapSettingsEvent.FOWToggleMode mAnimToggleMode;

		private bool mBoundsEnabledForLevelGame;

		private bool mBoundsEnabledForLevelSensors;

		private bool mBoundsEnabled = true;

		private CommandBuffer mBoundsBufferDefault;

		private CommandBuffer mBoundsBufferSensors;

		private CommandBuffer mActiveBoundsBuffer;

		private Material mBoundsMaterial;

		private Material mBoundsMaterialSensors;

		private Material mBlurMaterial;

		private ICommandScheduler mSimScheduler;

		private Bounds mMapBounds;

		private CommandBuffer mDefaultFOWVolumeBuffer = new CommandBuffer();

		private Mesh mFoWCubeMesh;

		private Matrix4x4 mFoWCubeMeshTransform = Matrix4x4.identity;

		public enum RenderModeType
		{
			Default,
			Tactical
		}
	}
}
