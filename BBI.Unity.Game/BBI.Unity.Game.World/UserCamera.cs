using System;
using System.Collections;
using BBI.Core.Events;
using BBI.Core.Utility;
using BBI.Platform;
using BBI.Unity.Core.Data;
using BBI.Unity.Core.Input;
using BBI.Unity.Core.Rendering;
using BBI.Unity.Core.Utility;
using BBI.Unity.Core.World;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Events;
using BBI.Unity.Game.HUD;
using HutongGames.PlayMaker;
using UnityEngine;
using BBI.Core.Utility.FixedPoint;

namespace BBI.Unity.Game.World
{
	// Token: 0x02000359 RID: 857
	public class UserCamera : CameraBase
	{
		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001CBF RID: 7359 RVA: 0x000A8B70 File Offset: 0x000A6D70
		public Bounds ViewConstraints
		{
			get
			{
				if (this.ViewConstraintsCollider == null)
				{
					return new Bounds(Vector3.zero, Vector3.one * float.PositiveInfinity);
				}
				Vector2r vector2r = (MapModManager.boundsMin + MapModManager.boundsMax) / Fixed64.FromConstFloat(2f);
				Vector2r vector2r1 = MapModManager.boundsMax - MapModManager.boundsMin;
				Bounds bound = new Bounds(new Vector3(float.Parse(vector2r.X.ToString()), 0f, float.Parse(vector2r.Y.ToString())), new Vector3(float.Parse(vector2r1.X.ToString()), 0f, float.Parse(vector2r1.Y.ToString())));
				Bounds bound1 = (MapModManager.overrideBounds ? bound : this.ViewConstraintsCollider.bounds);
				bound1.size = bound1.size - new Vector3(200f, 0f, 200f);
				return bound1;
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001CC0 RID: 7360 RVA: 0x000A8BD8 File Offset: 0x000A6DD8
		// (set) Token: 0x06001CC1 RID: 7361 RVA: 0x000A8C08 File Offset: 0x000A6E08
		public Collider ViewConstraintsCollider
		{
			get
			{
				if (this.mViewConstraintsCollider == null && this.CamViewBounds != null)
				{
					this.mViewConstraintsCollider = this.CamViewBounds;
				}
				return this.mViewConstraintsCollider;
			}
			set
			{
				this.mViewConstraintsCollider = value;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001CC2 RID: 7362 RVA: 0x000A8C11 File Offset: 0x000A6E11
		public override Camera ActiveCamera
		{
			get
			{
				return this.mCam;
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06001CC3 RID: 7363 RVA: 0x000A8C19 File Offset: 0x000A6E19
		public override float CamAltitudeMeters
		{
			get
			{
				return this.Position.y;
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001CC4 RID: 7364 RVA: 0x000A8C28 File Offset: 0x000A6E28
		public override float ClosestCameraDistanceToTarget
		{
			get
			{
				float result = this.Thresholds.MinimumDistanceToTargetM;
				if (this.mCurrentFocusSettings != null)
				{
					result = this.mCurrentFocusSettings.ClosestCameraFocus;
				}
				if (UserCamera.mSensorsManagerActive)
				{
					result = this.Thresholds.MinimumCameraDistanceSensors;
				}
				return result;
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001CC5 RID: 7365 RVA: 0x000A8C69 File Offset: 0x000A6E69
		public float DistanceToTargetMeters
		{
			get
			{
				return this.mDistanceToTarget;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001CC6 RID: 7366 RVA: 0x000A8C71 File Offset: 0x000A6E71
		public override float ScreenWidthInMeters
		{
			get
			{
				return this.mScreenWidthInMeters;
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001CC7 RID: 7367 RVA: 0x000A8C79 File Offset: 0x000A6E79
		public override Transform CameraTarget
		{
			get
			{
				return this.CamTargetInternal;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001CC8 RID: 7368 RVA: 0x000A8C81 File Offset: 0x000A6E81
		public override float MetersPerPixel
		{
			get
			{
				return this.mScreenWidthInMetersUndamped / (float)Screen.width;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001CC9 RID: 7369 RVA: 0x000A8C90 File Offset: 0x000A6E90
		public override float MaxAltitudeM
		{
			get
			{
				if (UserCamera.mSensorsManagerActive)
				{
					return this.Thresholds.FurthestCameraDistanceSensors;
				}
				return MapModManager.GetMaxCameraDistance(this.Thresholds.FurthestCameraDistanceGameplay);
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001CCA RID: 7370 RVA: 0x000A8CB0 File Offset: 0x000A6EB0
		public override float CurrentMaxDistance
		{
			get
			{
				if (!this.OverrideAllowedMaxAltitude)
				{
					return this.MaxAltitudeM;
				}
				return this.OverridenMaxAltitudeValue;
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001CCB RID: 7371 RVA: 0x000A8CC7 File Offset: 0x000A6EC7
		public override float PercentageOfAltitude
		{
			get
			{
				return Mathf.Clamp01(1f - this.CamAltitudeMeters / this.MaxAltitudeM);
			}
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x000A8CE1 File Offset: 0x000A6EE1
		private void StartTransitionEventDelay()
		{
			this.mLastSensorsTransition = Time.unscaledTime;
			if (this.mTransitionEventDelayCoroutine == null)
			{
				this.mTransitionEventDelayCoroutine = this.DelayTransitionEvent();
				base.StartCoroutine(this.mTransitionEventDelayCoroutine);
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001CCD RID: 7373 RVA: 0x000A8D0F File Offset: 0x000A6F0F
		// (set) Token: 0x06001CCE RID: 7374 RVA: 0x000A8D16 File Offset: 0x000A6F16
		public bool SensorsManagerActive
		{
			get
			{
				return UserCamera.mSensorsManagerActive;
			}
			private set
			{
				if (UserCamera.mSensorsManagerActive != value)
				{
					UserCamera.mSensorsManagerActive = value;
					this.StartTransitionEventDelay();
				}
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001CCF RID: 7375 RVA: 0x000A8D2C File Offset: 0x000A6F2C
		// (set) Token: 0x06001CD0 RID: 7376 RVA: 0x000A8D34 File Offset: 0x000A6F34
		public bool IntelMomentActive
		{
			get
			{
				return this.mIntelMomentActive;
			}
			set
			{
				if (this.mIntelMomentActive != value)
				{
					this.mIntelMomentActive = value;
					ShipbreakersMain.PresentationEventSystem.Post(new IntelMomentEvent(this.mIntelMomentActive));
				}
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001CD1 RID: 7377 RVA: 0x000A8D5B File Offset: 0x000A6F5B
		public bool ResetViewToDefaultOnSensorsTransition
		{
			get
			{
				return ShipbreakersMain.UserSettings.Gameplay.CameraResetOnExitSensors;
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001CD2 RID: 7378 RVA: 0x000A8D6C File Offset: 0x000A6F6C
		public static bool IsOrbiting
		{
			get
			{
				return UnityEngine.Input.GetMouseButton(1);
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001CD3 RID: 7379 RVA: 0x000A8D74 File Offset: 0x000A6F74
		public static bool IsPanning
		{
			get
			{
				return UnityEngine.Input.GetMouseButton(2);
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001CD4 RID: 7380 RVA: 0x000A8D7C File Offset: 0x000A6F7C
		public bool IsDistorted
		{
			get
			{
				PlayMakerFSM componentInChildren = base.GetComponentInChildren<PlayMakerFSM>();
				if (componentInChildren != null)
				{
					FsmBool fsmBool = componentInChildren.FsmVariables.FindFsmBool("EnableCameraDistortion");
					if (fsmBool != null)
					{
						return fsmBool.Value;
					}
				}
				return false;
			}
		}

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06001CD5 RID: 7381 RVA: 0x000A8DB5 File Offset: 0x000A6FB5
		// (remove) Token: 0x06001CD6 RID: 7382 RVA: 0x000A8DBE File Offset: 0x000A6FBE
		public override event CameraBase.CameraMoved CameraMotionComplete
		{
			add
			{
				this.mCameraMovedCallbacks += value;
			}
			remove
			{
				this.mCameraMovedCallbacks -= value;
			}
		}

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06001CD7 RID: 7383 RVA: 0x000A8DC8 File Offset: 0x000A6FC8
		// (remove) Token: 0x06001CD8 RID: 7384 RVA: 0x000A8E00 File Offset: 0x000A7000
		public override event CameraBase.CameraZoomChanged CameraZoomLevelChanged;

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001CD9 RID: 7385 RVA: 0x000A8E38 File Offset: 0x000A7038
		private Matrix4x4 mCameraToScreenMatrix
		{
			get
			{
				Camera sCameraOverride = this.mCam;
				if (LODGroupController.sCameraOverride != null)
				{
					sCameraOverride = LODGroupController.sCameraOverride;
				}
				Matrix4x4 lhs = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 1f));
				Matrix4x4 rhs = sCameraOverride.projectionMatrix * sCameraOverride.worldToCameraMatrix;
				return lhs * rhs;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06001CDA RID: 7386 RVA: 0x000A8EAA File Offset: 0x000A70AA
		private float DesiredDistanceToTarget
		{
			get
			{
				return Mathf.Lerp(this.ClosestCameraDistanceToTarget, this.CurrentMaxDistance, this.CurrentDistanceLERPCurve.Evaluate(this.mDesiredDistanceLERPValue));
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06001CDB RID: 7387 RVA: 0x000A8ECE File Offset: 0x000A70CE
		private float mDeltaTime
		{
			get
			{
				if (Time.timeScale != 1f)
				{
					return Time.realtimeSinceStartup - this.mLastRealTimeSinceStartup;
				}
				return Time.smoothDeltaTime;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001CDC RID: 7388 RVA: 0x000A8EEE File Offset: 0x000A70EE
		public bool IsCinematicMode
		{
			get
			{
				return this.mActiveCinematicController != null;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001CDD RID: 7389 RVA: 0x000A8EFC File Offset: 0x000A70FC
		// (set) Token: 0x06001CDE RID: 7390 RVA: 0x000A8F2B File Offset: 0x000A712B
		public bool AutoPitch
		{
			get
			{
				return this.mAutoPitch && !this.mColliding && this.mMotionState != UserCamera.CameraMotionState.Pan && this.mMotionState != UserCamera.CameraMotionState.PanScreenSpace && !this.mDisableAutoPitchUntilNextZoom;
			}
			private set
			{
				this.mAutoPitch = (value && ShipbreakersMain.UserSettings.Gameplay.CameraZoomAutoPitch);
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001CDF RID: 7391 RVA: 0x000A8F48 File Offset: 0x000A7148
		private bool LockCursorWhenOrbiting
		{
			get
			{
				return ShipbreakersMain.UserSettings.Gameplay.CameraLockCursorOnOrbit;
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001CE0 RID: 7392 RVA: 0x000A8F59 File Offset: 0x000A7159
		// (set) Token: 0x06001CE1 RID: 7393 RVA: 0x000A8F66 File Offset: 0x000A7166
		public Vector3 Position
		{
			get
			{
				return this.mTransform.position;
			}
			private set
			{
				this.mTransform.position = value;
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001CE2 RID: 7394 RVA: 0x000A8F74 File Offset: 0x000A7174
		// (set) Token: 0x06001CE3 RID: 7395 RVA: 0x000A8F81 File Offset: 0x000A7181
		public Quaternion Rotation
		{
			get
			{
				return this.mTransform.rotation;
			}
			private set
			{
				this.mTransform.rotation = value;
			}
		}

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x06001CE4 RID: 7396 RVA: 0x000A8F90 File Offset: 0x000A7190
		// (remove) Token: 0x06001CE5 RID: 7397 RVA: 0x000A8FC8 File Offset: 0x000A71C8
		private event CameraBase.CameraMoved mCameraMovedCallbacks;

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001CE6 RID: 7398 RVA: 0x000A8FFD File Offset: 0x000A71FD
		// (set) Token: 0x06001CE7 RID: 7399 RVA: 0x000A9008 File Offset: 0x000A7208
		private Transform CamTargetInternal
		{
			get
			{
				return this.m_CamTarget;
			}
			set
			{
				if (this.m_BaseCamTarget != value && this.m_CamTarget != value)
				{
					this.m_BaseCamTarget = value;
					Transform transform = value;
					if (transform != null)
					{
						UnitView component = value.GetComponent<UnitView>();
						if (component != null)
						{
							this.mCurrentFocusSettings = component.EntityID.GetTypeAttributes<UnitViewAttributes>().FocusSettings;
							transform = component.CameraTarget;
						}
						else
						{
							this.mCurrentFocusSettings = null;
						}
					}
					else
					{
						this.mCurrentFocusSettings = null;
					}
					if (this.mNoiseFilter != null)
					{
						this.mNoiseFilter.SetFilter2((this.mCurrentFocusSettings != null) ? this.mCurrentFocusSettings.NoiseFilter : null);
					}
					this.m_CamTarget = transform;
				}
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001CE8 RID: 7400 RVA: 0x000A90BE File Offset: 0x000A72BE
		public Vector3 CamTargetOrWorldAnchorPoint
		{
			get
			{
				if (this.CamTargetInternal != null)
				{
					this.mWorldAnchorPoint = this.CamTargetInternal.position;
				}
				return this.mWorldAnchorPoint;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x000A90E5 File Offset: 0x000A72E5
		public static bool IsSensorsModeActive
		{
			get
			{
				return UserCamera.mSensorsManagerActive;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001CEA RID: 7402 RVA: 0x000A90EC File Offset: 0x000A72EC
		private int NumPixelsEdgePanWidth
		{
			get
			{
				if (Application.isEditor || !this.mWindowInFocus || this.mBandBoxing)
				{
					return 0;
				}
				return Mathf.Max(1, Mathf.CeilToInt(this.MotionValues.EdgePanningScreenspacePercents.x * (float)Screen.width));
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001CEB RID: 7403 RVA: 0x000A9129 File Offset: 0x000A7329
		private int NumPixelsEdgePanHeight
		{
			get
			{
				if (Application.isEditor || !this.mWindowInFocus || this.mBandBoxing)
				{
					return 0;
				}
				return Mathf.Max(1, Mathf.CeilToInt(this.MotionValues.EdgePanningScreenspacePercents.y * (float)Screen.height));
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001CEC RID: 7404 RVA: 0x000A9166 File Offset: 0x000A7366
		private float MaxPanVelocityPercentScreenWidthPerSecond
		{
			get
			{
				return this.MotionValues.ScreenSpacePanVelocityMSPerScreen;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001CED RID: 7405 RVA: 0x000A9173 File Offset: 0x000A7373
		private float MouseXOrbitSensitivity
		{
			get
			{
				return 2.5f * ShipbreakersMain.UserSettings.Gameplay.CameraOrbitVertSpeed;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001CEE RID: 7406 RVA: 0x000A918A File Offset: 0x000A738A
		private float MouseYOrbitSensitivity
		{
			get
			{
				return 2.5f * ShipbreakersMain.UserSettings.Gameplay.CameraOrbitHorizSpeed;
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001CEF RID: 7407 RVA: 0x000A91A1 File Offset: 0x000A73A1
		private float MouseScrollSpeed
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001CF0 RID: 7408 RVA: 0x000A91A8 File Offset: 0x000A73A8
		private float EdgePanSpeedScalar
		{
			get
			{
				return 2f * ShipbreakersMain.UserSettings.Gameplay.CameraEdgePanSpeed;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001CF1 RID: 7409 RVA: 0x000A91BF File Offset: 0x000A73BF
		public UserCamera.CamFramingSettings GameViewSettings
		{
			get
			{
				return this.mGameViewSettings;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x000A91C7 File Offset: 0x000A73C7
		public UserCamera.CamFramingSettings SensorsViewSettings
		{
			get
			{
				return this.mSensorsViewSettings;
			}
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x000A91CF File Offset: 0x000A73CF
		public void SetViewSettings(UserCamera.CamFramingSettings gameViewSettings, UserCamera.CamFramingSettings sensorsViewSettings)
		{
			this.mGameViewSettings = gameViewSettings;
			this.mSensorsViewSettings = sensorsViewSettings;
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001CF4 RID: 7412 RVA: 0x000A91DF File Offset: 0x000A73DF
		// (set) Token: 0x06001CF5 RID: 7413 RVA: 0x000A91E7 File Offset: 0x000A73E7
		public bool AllowSensorManagerMode
		{
			get
			{
				return this.mAllowSensorManagerMode;
			}
			set
			{
				this.mAllowSensorManagerMode = value;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x000A91F0 File Offset: 0x000A73F0
		private float mNoiseFilterCrossfadeDistanceStart
		{
			get
			{
				float result = 100f;
				if (this.mCurrentFocusSettings != null)
				{
					result = this.mCurrentFocusSettings.NoiseFilterBeginDistance;
				}
				return result;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001CF7 RID: 7415 RVA: 0x000A9218 File Offset: 0x000A7418
		private AnimationCurve CurrentDistanceLERPCurve
		{
			get
			{
				if (UserCamera.mSensorsManagerActive)
				{
					return this.MotionValues.ScrollTickLERPCurveSensors;
				}
				return this.MotionValues.ScrollTickLERPCurveGameplay;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x000A9238 File Offset: 0x000A7438
		private int ScrollTicksForCameraMode
		{
			get
			{
				if (UserCamera.mSensorsManagerActive)
				{
					return this.MotionValues.NumScrollTicksSensors;
				}
				return this.MotionValues.NumScrollTicksGameplay;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001CF9 RID: 7417 RVA: 0x000A9258 File Offset: 0x000A7458
		public Vector2 HalfScreen
		{
			get
			{
				return new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
			}
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x000A9278 File Offset: 0x000A7478
		private void Awake()
		{
			this.mTransform = base.transform;
			if (this.CamViewBounds != null)
			{
				this.ViewConstraintsCollider = this.CamViewBounds;
			}
			this.mLastRealTimeSinceStartup = Time.realtimeSinceStartup;
			if (RenderSettings.fog)
			{
				Log.Warn(Log.Channel.Graphics, "Fog is enabled for scene {0}. Disabling!", new object[]
				{
					Application.loadedLevelName
				});
				RenderSettings.fog = false;
			}
			Collider collider = base.GetComponent<Collider>();
			if (collider == null || !(collider is SphereCollider))
			{
				if (!(collider is SphereCollider))
				{
					UnityEngine.Object.Destroy(collider);
				}
				collider = base.gameObject.AddComponent<SphereCollider>();
			}
			this.mCollider = (collider as SphereCollider);
			this.mCollider.radius = this.CollisionValues.ColliderSizeVersusVelocityMS.Evaluate(0f);
			this.mScreenSpaceFog = null;
			this.mCam = UserCamera.UnityCameraForUserCamera(this);
			if (this.mCam == null)
			{
				base.gameObject.SetActive(false);
				Debug.LogError(string.Format("{0} doesn't contain an active camera! Disabling our camera!", base.name), this);
			}
			else
			{
				this.mAllowSensorManagerMode = true;
				this.mDefaultCullingMask = this.mCam.cullingMask;
				this.mSensorsCullingMask = (1 << RenderLayer.DefaultLayer | 1 << RenderLayer.ModelsLayer | 1 << RenderLayer.CollidingModelLayer);
				this.mOriginalClearFlags = this.mCam.clearFlags;
				this.mScreenSpaceFog = this.mCam.GetComponent<ScreenSpaceFog>();
			}
			this.mLastPublishedAltitude = this.CamAltitudeMeters;
			this.HasInputFocus = true;
			if (collider)
			{
				collider.isTrigger = true;
			}
			this.mNoiseFilter = base.GetComponentInChildren<CrossFadingNoiseFilter>();
			if (this.mNoiseFilter != null)
			{
				this.mNoiseFilter.SetFilter1(this.m_DefaultCameraNoise);
			}
			this.mDefaultViewOnInstantiate = new UserCamera.CamFramingSettings(this.MotionValues.DefaultView);
			this.mBandBoxing = false;
			this.mInGameMenuShowing = false;
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x000A9458 File Offset: 0x000A7658
		private void OnEnable()
		{
			ShipbreakersMain.PresentationEventSystem.AddHandler<SensorsManagerEvent>(new BBI.Core.Events.EventHandler<SensorsManagerEvent>(this.OnSensorsManagerToggleEvent));
			ShipbreakersMain.PresentationEventSystem.AddHandler<BandBoxStateEvent>(new BBI.Core.Events.EventHandler<BandBoxStateEvent>(this.OnBandBoxStateEvent));
			ShipbreakersMain.PresentationEventSystem.AddHandler<InGameMenuShowingEvent>(new BBI.Core.Events.EventHandler<InGameMenuShowingEvent>(this.OnInGameMenuShowingEvent));
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x000A94A8 File Offset: 0x000A76A8
		private void OnDisable()
		{
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<SensorsManagerEvent>(new BBI.Core.Events.EventHandler<SensorsManagerEvent>(this.OnSensorsManagerToggleEvent));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<BandBoxStateEvent>(new BBI.Core.Events.EventHandler<BandBoxStateEvent>(this.OnBandBoxStateEvent));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<InGameMenuShowingEvent>(new BBI.Core.Events.EventHandler<InGameMenuShowingEvent>(this.OnInGameMenuShowingEvent));
		}

		// Token: 0x06001CFD RID: 7421 RVA: 0x000A94F8 File Offset: 0x000A76F8
		private void LateUpdate()
		{
			float mDeltaTime = this.mDeltaTime;
			this.mLastRealTimeSinceStartup = Time.realtimeSinceStartup;
			if (mDeltaTime <= 0f)
			{
				return;
			}
			if (this.CamTargetInternal == null || !this.CamTargetInternal.gameObject.activeInHierarchy)
			{
				this.CamTargetInternal = null;
			}
			Vector3 position = this.Position;
			Quaternion quaternion = this.Rotation;
			float currentMaxDistance = this.CurrentMaxDistance;
			this.UpdateViewWidthInM(mDeltaTime);
			this.ConstrainCameraVelocities();
			Vector3 point = this.mWorldAnchorPoint;
			RaycastHit raycastHit;
			if (this.RaycastScreenPointToWorld(this.HalfScreen, out raycastHit))
			{
				point = raycastHit.point;
			}
			float num = Mathf.Abs(this.GetWorldVelocityVectorMS().magnitude);
			if (float.IsNaN(num))
			{
				Debug.LogError(string.Format("Camera world velocity is bad! {0}", num));
				num = 0f;
			}
			float value = this.CollisionValues.ColliderSizeVersusVelocityMS.Evaluate(num);
			this.mCollider.radius = Mathf.Clamp(value, 1f, float.MaxValue);
			this.MutateCameraPositionOrientation(ref position, ref quaternion, point, mDeltaTime);
			if (float.IsNaN(quaternion.eulerAngles.x) || float.IsNaN(quaternion.eulerAngles.y) || float.IsNaN(quaternion.eulerAngles.y))
			{
				Debug.LogError(string.Format("Camera orientation is bad! {0}", quaternion));
				quaternion = Quaternion.Euler(87f, 0f, 0f);
			}
			this.Rotation = quaternion;
			this.Position = position;
			if (!this.IsCinematicMode || this.mActiveCinematicController.IsCameraMotionAllowed(UserCamera.CameraMotionTypes.Constraints))
			{
				this.mWorldVelocityMS = Vector3.Lerp(this.mWorldVelocityMS, Vector3.zero, this.MotionValues.DampeningValues.CameraTranslationDampening * mDeltaTime);
				if (this.mMotionState != UserCamera.CameraMotionState.Orbit)
				{
					this.mOrbitAngularVelocityDegPerSec.x = Mathf.Lerp(this.mOrbitAngularVelocityDegPerSec.x, 0f, mDeltaTime * this.MotionValues.DampeningValues.CameraOrbitXAngularDampening);
					this.mOrbitAngularVelocityDegPerSec.y = Mathf.Lerp(this.mOrbitAngularVelocityDegPerSec.y, 0f, mDeltaTime * this.MotionValues.DampeningValues.CameraOrbitYAngularDampening);
				}
				if (this.mWorldVelocityMS.sqrMagnitude < 1f)
				{
					this.mWorldVelocityMS = Vector3.zero;
				}
				if (this.mOrbitAngularVelocityDegPerSec.sqrMagnitude < 0.001f)
				{
					this.mOrbitAngularVelocityDegPerSec = Vector2.zero;
				}
			}
			if (this.mMotionState != UserCamera.CameraMotionState.Pan && this.mMotionState != UserCamera.CameraMotionState.PanScreenSpace)
			{
				bool flag = true;
				if (this.CamTargetInternal && !this.IsPointOnViewConstraints(this.CamTargetOrWorldAnchorPoint + Vector3.up * this.Thresholds.FurthestCameraDistanceSensors))
				{
					flag = false;
					this.RaycastScreenPointToWorld(this.HalfScreen, out raycastHit);
					this.mDistanceToTarget = (this.Position - raycastHit.point).magnitude;
				}
				if (flag)
				{
					this.mDistanceToTarget = (this.Position - this.CamTargetOrWorldAnchorPoint).magnitude;
				}
			}
			if (this.mCameraMovedCallbacks != null)
			{
				Profiler.BeginSample("Camera motion callback");
				Matrix4x4 mCameraToScreenMatrix = this.mCameraToScreenMatrix;
				Matrix4x4 gameCameraToUIMatrix = Matrix4x4.Scale(new Vector3(HUDSystem.NGUIPixelScale, HUDSystem.NGUIPixelScale, 1f)) * mCameraToScreenMatrix;
				this.mCameraMovedCallbacks(this, UICamera.mainCamera, mCameraToScreenMatrix, gameCameraToUIMatrix);
				Profiler.EndSample();
			}
			if (!this.IsCinematicMode && Mathf.Abs(this.CamAltitudeMeters - this.mLastPublishedAltitude) > 10f)
			{
				if (this.CameraZoomLevelChanged != null)
				{
					float oldZoomPercent = Mathf.Clamp01(1f - this.mLastPublishedAltitude / this.MaxAltitudeM);
					float newZoomPercent = Mathf.Clamp01(1f - this.CamAltitudeMeters / this.MaxAltitudeM);
					this.CameraZoomLevelChanged(this, this.mLastPublishedAltitude, currentMaxDistance, oldZoomPercent, this.CamAltitudeMeters, this.CurrentMaxDistance, newZoomPercent);
				}
				this.mLastPublishedAltitude = this.CamAltitudeMeters;
			}
			if (this.mNoiseFilter != null)
			{
				float num2 = 1f;
				if (this.CamTargetInternal != null)
				{
					UnitView componentInParent = this.CamTargetInternal.GetComponentInParent<UnitView>();
					if (null != componentInParent)
					{
						num2 = componentInParent.SpeedPercent;
					}
					else
					{
						num2 = 0f;
					}
				}
				this.mNoiseFilter.CrossFade = Mathf.InverseLerp(this.mNoiseFilterCrossfadeDistanceStart, this.ClosestCameraDistanceToTarget, this.mDistanceToTarget) * num2;
				if (this.IsCinematicMode || this.SensorsManagerActive)
				{
					this.mNoiseFilter.MasterBlendStrength = 0f;
				}
				else
				{
					this.mNoiseFilter.MasterBlendStrength = 1f;
				}
			}
			if (this.CamTargetInternal != null)
			{
				this.mPreviousFrameTargetPoint = this.CamTargetInternal.transform.position;
			}
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x000A99B4 File Offset: 0x000A7BB4
		private void OnGUI()
		{
			if (Event.current.type == EventType.Repaint && (this.mInputSystem == null || !this.mInputSystem.InputIsBlocked) && (this.mMotionState == UserCamera.CameraMotionState.None || this.mMotionState == UserCamera.CameraMotionState.Following))
			{
				Vector2 vector = new Vector2(Event.current.mousePosition.x, (float)Screen.height - Event.current.mousePosition.y);
				float num = 0f;
				float num2 = 0f;
				bool flag = false;
				bool flag2 = false;
				if (vector.x < (float)this.NumPixelsEdgePanWidth && vector.x >= 0f)
				{
					num = -this.EdgePanSpeedScalar;
				}
				else if (vector.x > (float)(Screen.width - this.NumPixelsEdgePanWidth) && vector.x <= (float)Screen.width)
				{
					num = this.EdgePanSpeedScalar;
				}
				else if (vector.x < 0f || vector.x > (float)Screen.width)
				{
					flag = true;
				}
				if (vector.y < (float)this.NumPixelsEdgePanHeight && vector.y >= 0f)
				{
					num2 = -this.EdgePanSpeedScalar;
				}
				else if (vector.y > (float)(Screen.height - this.NumPixelsEdgePanHeight) && vector.y <= (float)Screen.height)
				{
					num2 = this.EdgePanSpeedScalar;
				}
				else if (vector.y < 0f || vector.y > (float)Screen.height)
				{
					flag2 = true;
				}
				float num3 = (ShipbreakersMain.IsGamePaused(ShipbreakersMain.PauseMode.PresentationOnly) && !this.mInGameMenuShowing) ? this.mDeltaTime : Time.deltaTime;
				float num4 = this.mScreenWidthInMeters * this.MaxPanVelocityPercentScreenWidthPerSecond * num3;
				Vector3 worldVelocity = (flag2 || flag) ? Vector3.zero : (new Vector3(num * num4, 0f, num2 * num4) * ShipbreakersMain.UserSettings.Gameplay.CameraEdgePanSpeed);
				if (worldVelocity.sqrMagnitude > 0f)
				{
					if (this.mMotionState == UserCamera.CameraMotionState.Following)
					{
						this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.None, this.HalfScreen, null);
					}
					this.mOrbitAngularVelocityDegPerSec = Vector2.zero;
					this.mWorldVelocityMS += this.ConvertLocalDirectionToWorld(worldVelocity);
				}
			}
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x000A9BD8 File Offset: 0x000A7DD8
		private void OnApplicationFocus(bool focusState)
		{
			this.mPanOrOrbitCanBegin = false;
			this.mWindowInFocus = focusState;
			if (this.mMotionState == UserCamera.CameraMotionState.Pan || this.mMotionState == UserCamera.CameraMotionState.PanScreenSpace || this.mMotionState == UserCamera.CameraMotionState.Orbit)
			{
				if (this.IsCinematicMode)
				{
					this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.CinematicCamera, this.HalfScreen);
					return;
				}
				this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.None, this.HalfScreen);
			}
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x000A9C34 File Offset: 0x000A7E34
		public override void Initialize(InputSystem inputSystem)
		{
			Assert.Release(inputSystem != null, "[RZ] UserCamera needs a valid referene to an InputSystem instance.");
			this.mInputSystem = inputSystem;
			if (inputSystem != null)
			{
				inputSystem.RegisterHandler(new Action<InputEventArgs>(this.CameraPanInputHandler), InputEventType.Tap, HandlerPriority.Camera);
				inputSystem.RegisterHandler(new Action<InputEventArgs>(this.CameraGestureHandler), InputEventType.Gesture, HandlerPriority.Camera);
				this.RegisterHotkeys();
			}
			else
			{
				Log.Error(this, Log.Channel.UI, "No input system specified during camera {0} initialization!", new object[]
				{
					base.name
				});
			}
			SetRenderSettings setRenderSettings = SceneHelper.FindObjectOfType<SetRenderSettings>();
			if (setRenderSettings != null && setRenderSettings.CameraSettings != null)
			{
				InitialCameraSettingsAsset cameraSettings = setRenderSettings.CameraSettings;
				Quaternion rotation = Quaternion.Euler(this.MotionValues.DefaultView.Orientation.x, cameraSettings.DefaultHeading, 0f);
				this.Position = cameraSettings.InitialLookAtPosition - rotation * Vector3.back * this.MotionValues.DefaultView.FinalDistanceToTarget;
				this.Rotation = rotation;
				this.MotionValues.DefaultView.Orientation = new Vector2(rotation.eulerAngles.x, rotation.eulerAngles.y);
				this.MotionValues.DefaultView.WorldAnchorPoint = this.mWorldAnchorPoint;
			}
			else
			{
				Log.Error(Log.Channel.Graphics, "[GO] Level does not contain camera settings. Using instanced defaults", new object[0]);
				this.MotionValues.DefaultView = new UserCamera.CamFramingSettings(this.mDefaultViewOnInstantiate);
			}
			this.InitializeDefaultViewSettings(this.MotionValues.DefaultView.Orientation.y);
			if (this.Thresholds != null)
			{
				this.OverridenMaxAltitudeValue = this.Thresholds.FurthestCameraDistanceSensors;
			}
			this.ApplyUpdatedQualitySettings(ShipbreakersMain.UserSettings.Video);
			ShipbreakersMain.PresentationEventSystem.AddHandler<QualitySettingsChangedEvent>(new BBI.Core.Events.EventHandler<QualitySettingsChangedEvent>(this.OnNewQualitySettings));
			ShipbreakersMain.PresentationEventSystem.AddHandler<RebindHotkeysEvent>(new BBI.Core.Events.EventHandler<RebindHotkeysEvent>(this.OnRebindHotkeysEvent));
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x000A9E18 File Offset: 0x000A8018
		public override void TearDown(InputSystem inputSystem)
		{
			if (inputSystem != null)
			{
				inputSystem.UnregisterHandler(new Action<InputEventArgs>(this.CameraPanInputHandler), InputEventType.Tap, HandlerPriority.Camera);
				inputSystem.UnregisterHandler(new Action<InputEventArgs>(this.CameraGestureHandler), InputEventType.Gesture, HandlerPriority.Camera);
				this.DeregisterHotkeys();
				this.mInputSystem = null;
			}
			else
			{
				Log.Error(this, Log.Channel.UI, "No input system specified during camera {0} tear down!!", new object[]
				{
					base.name
				});
			}
			if (this.SensorsManagerActive)
			{
				this.ToggleSensorsManager(null, true, true, true);
			}
			this.ResetCamera();
			this.mCameraMovedCallbacks = null;
			this.CameraZoomLevelChanged = null;
			this.mNoiseFilter.SetFilter2(null);
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<QualitySettingsChangedEvent>(new BBI.Core.Events.EventHandler<QualitySettingsChangedEvent>(this.OnNewQualitySettings));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<RebindHotkeysEvent>(new BBI.Core.Events.EventHandler<RebindHotkeysEvent>(this.OnRebindHotkeysEvent));
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x000A9EE8 File Offset: 0x000A80E8
		public void ResetCamera()
		{
			if (this.mTransitionEventDelayCoroutine != null)
			{
				base.StopCoroutine(this.mTransitionEventDelayCoroutine);
				this.mTransitionEventDelayCoroutine = null;
			}
			if (this.mActiveCinematicController != null)
			{
				this.ReleaseCinematicToken(this.mActiveCinematicController);
			}
			this.mCam.clearFlags = this.mOriginalClearFlags;
			this.mCam.cullingMask = this.mDefaultCullingMask;
			this.OverrideAllowedMaxAltitude = false;
			this.mIntelMomentActive = false;
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x000A9F54 File Offset: 0x000A8154
		public void SetAutoPitch(bool autoPitch)
		{
			this.AutoPitch = autoPitch;
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x000A9F60 File Offset: 0x000A8160
		public UserCamera.CinematicControllerToken BeginCinematicSequence(Action cameraSetup, Action cameraTeardown, UserCamera.CinematicControllerToken.RequestCinematicEnd endRequestCallback, UserCamera.CameraMotionTypes cameraMotionConstraints)
		{
			if (this.mActiveCinematicController == null)
			{
				this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.None, this.HalfScreen, null);
				this.mActiveCinematicController = new UserCamera.CinematicControllerToken(this, cameraSetup, cameraTeardown, endRequestCallback, cameraMotionConstraints);
				return this.mActiveCinematicController;
			}
			Log.Error(this, Log.Channel.Graphics, "{0} already has a cinematic controller token active!", new object[]
			{
				base.name
			});
			return null;
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x000A9FC0 File Offset: 0x000A81C0
		private void ReleaseCinematicToken(UserCamera.CinematicControllerToken token)
		{
			if (this.mActiveCinematicController == token && token != null)
			{
				this.mActiveCinematicController = null;
				RaycastHit raycastHit;
				if (this.RaycastScreenPointToWorld(this.HalfScreen, out raycastHit))
				{
					this.mDistanceToTarget = Vector3.Distance(this.Position, raycastHit.point);
				}
				this.mCurrentDistanceLERPValue = this.FindLERPValueForDistanceInCurrentLERPCurve(this.mDistanceToTarget);
				this.mDesiredDistanceLERPValue = this.mCurrentDistanceLERPValue;
				this.mDisableAutoPitchUntilNextZoom = true;
				this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.None, this.HalfScreen, null);
				return;
			}
			Log.Error(this, Log.Channel.Graphics, "Attempted to release cinematic token but it did not match our current token or the token is NULL", new object[0]);
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x000AA054 File Offset: 0x000A8254
		public void ToggleSensorsManager(Transform followTarget, bool instantTransition)
		{
			if (this.IsCinematicMode)
			{
				return;
			}
			this.m_CamTarget = followTarget;
			if (this.m_CamTarget != null)
			{
				this.ToggleSensorsManager(new Vector3?(this.m_CamTarget.position), false, false, instantTransition);
				return;
			}
			this.ToggleSensorsManager(null, false, false, instantTransition);
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x000AA0AC File Offset: 0x000A82AC
		public void ToggleSensorsManager(Vector3? worldPosition, bool removeFocus, bool cinematicBypass, bool instantTransition)
		{
			if (this.IsCinematicMode)
			{
				if (cinematicBypass)
				{
					this.SensorsManagerActive = !this.SensorsManagerActive;
				}
				return;
			}
			if (!this.mAllowSensorManagerMode)
			{
				return;
			}
			if (!this.SensorsManagerActive)
			{
				if (this.mMotionState != UserCamera.CameraMotionState.MoveToTargetCameraFrame)
				{
					this.mGameViewSettings.SetValues(this);
					this.mSensorsViewSettings.Orientation.y = this.mGameViewSettings.Orientation.y;
					this.mSensorsViewSettings.WorldAnchorPoint = this.mWorldAnchorPoint;
				}
				this.MoveToTargetDistance(this.mSensorsViewSettings, this.mGameViewSettings);
				this.SensorsManagerActive = true;
			}
			else if (this.SensorsManagerActive)
			{
				if (this.mMotionState != UserCamera.CameraMotionState.MoveToTargetCameraFrame)
				{
					this.mSensorsViewSettings.SetValues(this);
					this.mGameViewSettings.Orientation.y = this.mSensorsViewSettings.Orientation.y;
					if (worldPosition != null)
					{
						if (removeFocus)
						{
							this.CamTargetInternal = null;
						}
						this.mWorldAnchorPoint = worldPosition.Value;
						this.mGameViewSettings.WorldAnchorPoint = this.mWorldAnchorPoint;
					}
					else
					{
						this.mGameViewSettings.WorldAnchorPoint = this.mWorldAnchorPoint;
					}
					if (this.ResetViewToDefaultOnSensorsTransition)
					{
						this.mGameViewSettings.FinalDistanceToTarget = this.MotionValues.DefaultView.FinalDistanceToTarget;
						this.mGameViewSettings.LERPCurvePosition = this.MotionValues.DefaultView.LERPCurvePosition;
						this.mGameViewSettings.Orientation.x = this.MotionValues.DefaultView.Orientation.x;
					}
					this.AutoPitch = true;
				}
				this.MoveToTargetDistance(this.mGameViewSettings, this.mSensorsViewSettings);
				this.SensorsManagerActive = false;
			}
			if (instantTransition)
			{
				this.mSensorsTransitionTime = this.MotionValues.SensorsTransitionPeriodSeconds;
			}
			else
			{
				this.mSensorsTransitionTime = 0f;
			}
			this.ApplyUpdatedQualitySettings(ShipbreakersMain.UserSettings.Video);
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x000AA288 File Offset: 0x000A8488
		public void UpdateZoomDataRelativeToCameraPosition()
		{
			RaycastHit raycastHit;
			if (this.RaycastScreenPointToWorld(this.HalfScreen, out raycastHit) || this.CamTargetInternal != null)
			{
				Vector3 a = raycastHit.point;
				if (this.CamTargetInternal != null)
				{
					a = this.CamTargetInternal.transform.position;
				}
				float magnitude = (a - this.Position).magnitude;
				this.OverridenMaxAltitudeValue = magnitude;
				this.mCurrentDistanceLERPValue = this.FindLERPValueForDistanceInLERPCurve(this.CurrentDistanceLERPCurve, this.ClosestCameraDistanceToTarget, this.CurrentMaxDistance, magnitude);
				this.mDesiredDistanceLERPValue = this.mCurrentDistanceLERPValue;
				return;
			}
			Debug.LogError(string.Format("Could not update Zoom data for camera!", new object[0]), this);
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x000AA338 File Offset: 0x000A8538
		private void MoveToTargetDistance(UserCamera.CamFramingSettings frame, UserCamera.CamFramingSettings fromFrame)
		{
			if (this.mMotionState != UserCamera.CameraMotionState.MoveToTargetCameraFrame)
			{
				if (this.AutoPitch)
				{
					float num = this.MotionValues.DefaultPitchVersusDistance.Evaluate(frame.FinalDistanceToTarget) - frame.Orientation.x;
					if (num < 0f || num > this.Thresholds.AutoPitchEngageAngleErrorDegrees)
					{
						this.AutoPitch = false;
					}
				}
				this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.MoveToTargetCameraFrame, this.HalfScreen, this.CamTargetInternal);
			}
			this.mTransitioningFromSettings = fromFrame;
			this.mGoalViewSettings = frame;
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x000AA3B9 File Offset: 0x000A85B9
		public bool IsFollowingObject(Transform obj)
		{
			return (this.m_BaseCamTarget == obj || this.m_CamTarget == obj) && this.mMotionState == UserCamera.CameraMotionState.Following;
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x000AA3E4 File Offset: 0x000A85E4
		public void JumpToObject(Transform obj, bool follow)
		{
			if (this.IsCinematicMode)
			{
				return;
			}
			if (this.CamTargetInternal == null)
			{
				float camAltitudeMeters = this.CamAltitudeMeters;
				float y = obj.position.y;
				this.Position = obj.transform.position - this.mTransform.forward * this.DistanceToTargetMeters;
				if (follow)
				{
					this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.Following, this.HalfScreen, obj);
					return;
				}
			}
			else if (obj == this.CamTargetInternal)
			{
				if (this.SensorsManagerActive)
				{
					this.ToggleSensorsManager(null, true, false, false);
					return;
				}
			}
			else
			{
				float y2 = this.CamTargetInternal.position.y;
				float y3 = obj.position.y;
				this.Position = obj.transform.position - this.mTransform.forward * this.DistanceToTargetMeters;
				if (follow)
				{
					this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.Following, this.HalfScreen, obj);
					return;
				}
				this.CamTargetInternal = null;
			}
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x000AA4E8 File Offset: 0x000A86E8
		public void ToggleCameraDistortion(bool enabled)
		{
			PlayMakerFSM componentInChildren = base.GetComponentInChildren<PlayMakerFSM>();
			if (!(componentInChildren != null))
			{
				Log.Error(this, Log.Channel.Graphics, "Could not retrieve PlayMakerFSM component from camera. ToggleCameraDistortion method can't execute in {0}!", new object[]
				{
					base.GetType()
				});
				return;
			}
			FsmBool fsmBool = componentInChildren.FsmVariables.FindFsmBool("EnableCameraDistortion");
			if (fsmBool != null)
			{
				fsmBool.Value = enabled;
				return;
			}
			Log.Error(this, Log.Channel.Graphics, "Could not retrieve the variable \"{0}\" from PlayMakerFSM which determines the state of the camera distortion. Did it's name change? ToggleCameraDistortion method can't execute in {1}!", new object[]
			{
				"EnableCameraDistortion",
				base.GetType()
			});
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x000AA56C File Offset: 0x000A876C
		public void PointAtScreenPoint(Vector2 screenPoint)
		{
			if (this.IsCinematicMode)
			{
				return;
			}
			this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.PointTo, screenPoint);
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x000AA580 File Offset: 0x000A8780
		public void TeleportTo(Vector3 cameraPosition, Quaternion cameraRotation)
		{
			RaycastHit raycastHit;
			if (this.RaycastRayToWorld(new Ray(cameraPosition, cameraRotation * Vector3.forward), out raycastHit))
			{
				this.mWorldAnchorPoint = raycastHit.point;
			}
			else
			{
				Log.Error(this, Log.Channel.Graphics, "RaycastRayToWorld failed with cameraPosition: {0} and vector: {1} in UserCamera's TeleportTo function.", new object[]
				{
					cameraPosition,
					cameraRotation * Vector3.forward
				});
			}
			this.Position = cameraPosition;
			this.Rotation = cameraRotation;
			this.UpdateZoomDataRelativeToCameraPosition();
			this.mAutoPitch = false;
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x000AA606 File Offset: 0x000A8806
		public void TeleportTo(Vector2 gamePosition)
		{
			this.TeleportTo(gamePosition, this.DistanceToTargetMeters);
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x000AA618 File Offset: 0x000A8818
		public void TeleportTo(Vector2 gamePosition, float distanceToTarget)
		{
			if (this.IsCinematicMode)
			{
				return;
			}
			Bounds viewConstraints = this.ViewConstraints;
			Vector2 vector = new Vector2(Mathf.Clamp(gamePosition.x, viewConstraints.min.x, viewConstraints.max.x), Mathf.Clamp(gamePosition.y, viewConstraints.min.z, viewConstraints.max.z));
			Vector3 a = new Vector3(vector.x, 0f, vector.y);
			RaycastHit raycastHit;
			if (Physics.Raycast(new Ray(a + Vector3.up * this.Thresholds.FurthestCameraDistanceSensors, Vector3.down), out raycastHit, this.Thresholds.FurthestCameraDistanceSensors * 2f, 1 << RenderLayer.TerrainLayer))
			{
				a.y = raycastHit.point.y;
			}
			distanceToTarget = Mathf.Clamp(distanceToTarget, this.ClosestCameraDistanceToTarget, this.CurrentMaxDistance);
			this.Position = a - this.Rotation * Vector3.forward * distanceToTarget;
			this.mWorldAnchorPoint = a;
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x000AA738 File Offset: 0x000A8938
		public override void HomeCamera()
		{
			if (this.IsCinematicMode)
			{
				return;
			}
			this.mGameViewSettings = new UserCamera.CamFramingSettings(this.MotionValues.DefaultView);
			this.mSensorsViewSettings = new UserCamera.CamFramingSettings(this.MotionValues.DefaultSensorsView);
			this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.Homing, UnityEngine.Input.mousePosition, this.CamTargetInternal);
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x000AA794 File Offset: 0x000A8994
		public override void SteppedHomeCamera()
		{
			if (this.IsCinematicMode)
			{
				return;
			}
			float x = this.MotionValues.DefaultPitchVersusDistance.Evaluate(this.DistanceToTargetMeters);
			Vector3 eulerAngles = base.transform.eulerAngles;
			this.mOrbitAngularVelocityDegPerSec = Vector2.zero;
			if (!this.mAutoPitch)
			{
				eulerAngles.x = x;
				base.transform.eulerAngles = eulerAngles;
				this.mDistanceToTarget = this.CurrentMaxDistance;
				this.mDesiredDistanceLERPValue = 1f;
				this.mCurrentDistanceLERPValue = 1f;
				base.transform.position = this.CamTargetOrWorldAnchorPoint + base.transform.rotation * Vector3.back * this.mDistanceToTarget;
				this.mAutoPitch = true;
				return;
			}
			this.HomeCamera();
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x000AA85A File Offset: 0x000A8A5A
		public bool RaycastFromPointToCamera(Vector3 point, out RaycastHit hit)
		{
			return Physics.Raycast(point, this.Position - point, out hit, this.mCam.farClipPlane * 2f, this.CollisionValues.CollideWith.value);
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x000AA890 File Offset: 0x000A8A90
		public bool SphereCastScreenPointToWorld(Vector2 screenPoint, float distance, out RaycastHit hit)
		{
			return Physics.SphereCast(this.mCam.ScreenPointToRay(screenPoint), this.mCollider.radius, out hit, distance, this.CollisionValues.CollideWith.value);
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x000AA8C5 File Offset: 0x000A8AC5
		public bool SphereCastScreenPointToWorld(Vector2 screenPoint, out RaycastHit hit)
		{
			return this.SphereCastScreenPointToWorld(screenPoint, this.mCam.farClipPlane * 2f, out hit);
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x000AA8E0 File Offset: 0x000A8AE0
		public bool SphereCastIntoWorld(Vector3 fromPoint, float radius, Vector3 direction, float distance, out RaycastHit hit)
		{
			return Physics.CapsuleCast(fromPoint, fromPoint, radius, direction, out hit, distance, this.CollisionValues.CollideWith.value);
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x000AA8FF File Offset: 0x000A8AFF
		public bool RaycastScreenPointToWorld(Vector2 screenPoint, float distance, out RaycastHit hit)
		{
			return Physics.Raycast(this.mCam.ScreenPointToRay(screenPoint), out hit, distance, this.CollisionValues.CollideWith.value);
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x000AA929 File Offset: 0x000A8B29
		public override bool RaycastScreenPointToWorld(Vector2 screenPoint, out RaycastHit hit)
		{
			return this.RaycastScreenPointToWorld(screenPoint, this.mCam.farClipPlane * 2f, out hit);
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x000AA944 File Offset: 0x000A8B44
		private bool RaycastScreenPointToWorldFromPositionAndOrientation(Vector2 screenPoint, Vector3 position, Quaternion orientation, out RaycastHit hit)
		{
			Vector3 position2 = this.Position;
			Quaternion rotation = this.Rotation;
			this.Position = position;
			this.Rotation = orientation;
			bool result = this.RaycastScreenPointToWorld(screenPoint, this.mCam.farClipPlane * 2f, out hit);
			this.Position = position2;
			this.Rotation = rotation;
			return result;
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x000AA998 File Offset: 0x000A8B98
		private Ray GetRayFromScreenPointWithPositionAndRotation(Vector2 screenPoint, Vector3 position, Quaternion orientation)
		{
			Vector3 position2 = this.Position;
			Quaternion rotation = this.Rotation;
			this.Position = position;
			this.Rotation = orientation;
			Ray result = this.mCam.ScreenPointToRay(screenPoint);
			this.Position = position2;
			this.Rotation = rotation;
			return result;
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x000AA9E2 File Offset: 0x000A8BE2
		public bool RaycastRayToWorld(Ray ray, out RaycastHit hit)
		{
			return Physics.Raycast(ray, out hit, this.mCam.farClipPlane * 2f, this.CollisionValues.CollideWith.value);
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x000AAA0C File Offset: 0x000A8C0C
		public override void RunCameraQuery(CameraBase.CameraQuery camQuery)
		{
			if (camQuery != null)
			{
				Matrix4x4 mCameraToScreenMatrix = this.mCameraToScreenMatrix;
				Matrix4x4 gameCameraToUIMatrix = Matrix4x4.Scale(new Vector3(HUDSystem.NGUIPixelScale, HUDSystem.NGUIPixelScale, 1f)) * mCameraToScreenMatrix;
				camQuery(this, UICamera.mainCamera, mCameraToScreenMatrix, gameCameraToUIMatrix);
			}
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x000AAA51 File Offset: 0x000A8C51
		private Vector2 GUIToScreenPoint(Vector2 guiPoint)
		{
			return new Vector2(guiPoint.x, Mathf.Abs(guiPoint.y - (float)Screen.height));
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x000AAA74 File Offset: 0x000A8C74
		private Vector3 GetInstantaneousOrbitVelocityKMS()
		{
			float magnitude = (this.Position - this.CamTargetOrWorldAnchorPoint).magnitude;
			Vector3 a = -this.mTransform.right * this.mOrbitAngularVelocityDegPerSec.y + Vector3.up * this.mOrbitAngularVelocityDegPerSec.x;
			return a * (3.1415927f * magnitude);
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x000AAAE5 File Offset: 0x000A8CE5
		private Vector3 GetWorldVelocityVectorMS()
		{
			return this.mWorldVelocityMS + this.GetInstantaneousOrbitVelocityKMS();
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x000AAAF8 File Offset: 0x000A8CF8
		private Vector3 ConvertWorldDirectionToLocal(Vector3 worldVelocity)
		{
			return Quaternion.AngleAxis(-this.mTransform.eulerAngles.y, Vector3.up) * worldVelocity;
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x000AAB1B File Offset: 0x000A8D1B
		private Vector3 ConvertLocalDirectionToWorld(Vector3 worldVelocity)
		{
			return Quaternion.AngleAxis(this.mTransform.eulerAngles.y, Vector3.up) * worldVelocity;
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x000AAB3D File Offset: 0x000A8D3D
		private void ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState toState, Vector2 screenAnchorPoint)
		{
			this.ChangeCameraStateWithMutationApply(toState, screenAnchorPoint, this.CamTargetInternal);
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x000AAB50 File Offset: 0x000A8D50
		private void ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState toState, Vector2 screenAnchorPoint, Transform target)
		{
			Vector3 position = this.Position;
			Quaternion rotation = this.Rotation;
			this.ChangeCameraState(toState, screenAnchorPoint, target, ref position, ref rotation);
			this.Position = position;
			this.Rotation = rotation;
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x000AAB86 File Offset: 0x000A8D86
		private void ChangeCameraState(UserCamera.CameraMotionState toState, Vector2 screenAnchorPoint, ref Vector3 newPosition, ref Quaternion newOrientation)
		{
			this.ChangeCameraState(toState, screenAnchorPoint, this.CamTargetInternal, ref newPosition, ref newOrientation);
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x000AAB9C File Offset: 0x000A8D9C
		private void ChangeCameraState(UserCamera.CameraMotionState toState, Vector2 screenAnchorPoint, Transform target, ref Vector3 newPosition, ref Quaternion newOrientation)
		{
			if (this.mMotionState == toState && (target == this.CamTargetInternal || target == this.m_BaseCamTarget))
			{
				if (this.m_ZoomOnTargetRefocus && toState == UserCamera.CameraMotionState.Following)
				{
					this.mDesiredDistanceLERPValue = 0f;
				}
				return;
			}
			if (this.CamTargetInternal != null && target == null)
			{
				RaycastHit raycastHit;
				if (!this.RaycastScreenPointToWorldFromPositionAndOrientation(this.HalfScreen, newPosition, newOrientation, out raycastHit))
				{
					this.RaycastRayToWorld(new Ray(this.CamTargetInternal.position, Vector3.down), out raycastHit);
				}
				float distance = Mathf.Clamp((newPosition - raycastHit.point).magnitude, this.ClosestCameraDistanceToTarget, this.CurrentMaxDistance);
				float num = this.FindLERPValueForDistanceInCurrentLERPCurve(distance);
				this.mDesiredDistanceLERPValue = num;
				this.mCurrentDistanceLERPValue = num;
				this.mDistanceToTarget = distance;
				if (toState == UserCamera.CameraMotionState.Pan || toState == UserCamera.CameraMotionState.PanScreenSpace)
				{
					if (this.RaycastScreenPointToWorldFromPositionAndOrientation(this.mGrabScreenPoint, newPosition, newOrientation, out raycastHit))
					{
						this.mWorldAnchorPoint = raycastHit.point;
					}
				}
				else
				{
					this.mWorldAnchorPoint = raycastHit.point;
				}
				newPosition = raycastHit.point + newOrientation * Vector3.back * this.mDistanceToTarget;
			}
			UserCamera.CameraMotionState cameraMotionState = this.mMotionState;
			switch (cameraMotionState)
			{
			case UserCamera.CameraMotionState.Pan:
			case UserCamera.CameraMotionState.PanScreenSpace:
				this.mWorldVelocityMS = this.mDragWorldVelocityMS;
				this.mDesiredDistanceLERPValue = this.mCurrentDistanceLERPValue;
				this.mDragWorldVelocityMS = Vector3.zero;
				break;
			case UserCamera.CameraMotionState.Orbit:
				if (this.CamTargetInternal != null && toState == UserCamera.CameraMotionState.None)
				{
					toState = UserCamera.CameraMotionState.Following;
				}
				else if (target == null)
				{
					RaycastHit raycastHit;
					this.RaycastScreenPointToWorldFromPositionAndOrientation(this.HalfScreen, newPosition, newOrientation, out raycastHit);
					float num2 = Mathf.Clamp((newPosition - raycastHit.point).magnitude, this.ClosestCameraDistanceToTarget, this.CurrentMaxDistance);
					if (!Mathf.Approximately(num2, this.mDistanceToTarget))
					{
						float num3 = this.FindLERPValueForDistanceInCurrentLERPCurve(num2);
						this.mDesiredDistanceLERPValue = num3;
						this.mCurrentDistanceLERPValue = num3;
						this.mDistanceToTarget = num2;
						this.mWorldAnchorPoint = raycastHit.point;
					}
				}
				this.mInputSystem.SetHideCursor(false, InputSystem.InputControlFlags.Camera);
				this.mDesiredDistanceLERPValue = this.mCurrentDistanceLERPValue;
				break;
			default:
				if (cameraMotionState == UserCamera.CameraMotionState.MoveToTargetCameraFrame)
				{
					float num4 = Mathf.Clamp01(this.mSensorsTransitionTime / this.MotionValues.SensorsTransitionPeriodSeconds);
					if (num4 < 1f)
					{
						this.CamTargetInternal = target;
						return;
					}
				}
				break;
			}
			this.CamTargetInternal = target;
			if (this.CamTargetInternal && !this.IsPointOnViewConstraints(this.CamTargetInternal.position) && toState != UserCamera.CameraMotionState.Following)
			{
				this.CamTargetInternal = null;
			}
			switch (toState)
			{
			case UserCamera.CameraMotionState.None:
			{
				RaycastHit raycastHit;
				if (this.mMotionState != UserCamera.CameraMotionState.Following && this.RaycastScreenPointToWorld(this.HalfScreen, out raycastHit))
				{
					this.mWorldAnchorPoint = raycastHit.point;
				}
				this.mMotionState = UserCamera.CameraMotionState.None;
				return;
			}
			case UserCamera.CameraMotionState.Pan:
			{
				this.mWorldVelocityMS = Vector3.zero;
				this.mDragWorldVelocityMS = Vector3.zero;
				this.mOrbitAngularVelocityDegPerSec = Vector2.zero;
				this.mGrabScreenPoint = screenAnchorPoint;
				RaycastHit raycastHit;
				if (this.RaycastScreenPointToWorld(this.mGrabScreenPoint, out raycastHit))
				{
					this.mWorldAnchorPoint = raycastHit.point;
				}
				else
				{
					Ray ray = this.mCam.ScreenPointToRay(this.mGrabScreenPoint);
					this.mWorldAnchorPoint = this.ProjectRayToCameraBounds(ray);
				}
				this.mMotionState = toState;
				return;
			}
			case UserCamera.CameraMotionState.PanScreenSpace:
				this.mWorldVelocityMS = Vector3.zero;
				this.mDragWorldVelocityMS = Vector3.zero;
				this.mOrbitAngularVelocityDegPerSec = Vector2.zero;
				this.mGrabScreenPoint = screenAnchorPoint;
				this.mMotionState = toState;
				return;
			case UserCamera.CameraMotionState.Orbit:
			{
				this.mWorldVelocityMS = Vector3.zero;
				this.mDragWorldVelocityMS = Vector3.zero;
				this.mOrbitAngularVelocityDegPerSec = Vector2.zero;
				this.mGrabScreenPoint = screenAnchorPoint;
				this.mInputSystem.SetHideCursor(true, InputSystem.InputControlFlags.Camera);
				RaycastHit raycastHit;
				if (this.RaycastScreenPointToWorld(this.HalfScreen, out raycastHit))
				{
					this.mWorldAnchorPoint = raycastHit.point;
				}
				else
				{
					this.mWorldAnchorPoint = this.Position;
				}
				this.AutoPitch = false;
				this.mMotionState = toState;
				return;
			}
			case UserCamera.CameraMotionState.Homing:
			{
				this.mWorldVelocityMS = Vector3.zero;
				this.mDragWorldVelocityMS = Vector3.zero;
				this.mDesiredDistanceLERPValue = this.mCurrentDistanceLERPValue;
				this.mOrbitAngularVelocityDegPerSec = Vector2.zero;
				this.AutoPitch = true;
				RaycastHit raycastHit;
				if (this.RaycastScreenPointToWorld(this.HalfScreen, out raycastHit))
				{
					this.mWorldAnchorPoint = raycastHit.point;
				}
				else
				{
					this.mWorldAnchorPoint = this.Position;
				}
				this.mMotionState = toState;
				return;
			}
			case UserCamera.CameraMotionState.PointTo:
			{
				RaycastHit raycastHit;
				if (this.RaycastScreenPointToWorld(screenAnchorPoint, out raycastHit))
				{
					this.mWorldAnchorPoint = raycastHit.point;
					this.mMotionState = toState;
					return;
				}
				Debug.LogError(string.Format("Attempted to point to a place in the world but couldn't ray cast to it!", new object[0]), this);
				this.mMotionState = UserCamera.CameraMotionState.None;
				return;
			}
			case UserCamera.CameraMotionState.PointToAndFollow:
				if (target != null)
				{
					this.CamTargetInternal = target;
					this.mMotionState = toState;
					return;
				}
				Debug.LogError(string.Format("Attempted to point to a target but it wasn't set!", new object[0]), this);
				this.mMotionState = UserCamera.CameraMotionState.None;
				return;
			case UserCamera.CameraMotionState.Following:
				this.mWorldVelocityMS = Vector3.zero;
				this.mDragWorldVelocityMS = Vector3.zero;
				this.mMotionState = toState;
				this.mGrabScreenPoint = this.HalfScreen;
				if (this.CamTargetInternal != null)
				{
					this.mPreviousFrameTargetPoint = this.CamTargetInternal.position;
					return;
				}
				this.ChangeCameraState(UserCamera.CameraMotionState.None, this.HalfScreen, null, ref newPosition, ref newOrientation);
				return;
			case UserCamera.CameraMotionState.MoveToTargetCameraFrame:
			case UserCamera.CameraMotionState.CinematicCamera:
			{
				this.mWorldVelocityMS = Vector3.zero;
				this.mDragWorldVelocityMS = Vector3.zero;
				if (!this.IsCinematicMode || !this.mActiveCinematicController.IsCameraMotionAllowed(UserCamera.CameraMotionTypes.Orbit))
				{
					this.mOrbitAngularVelocityDegPerSec = Vector2.zero;
				}
				this.mGrabScreenPoint = screenAnchorPoint;
				RaycastHit raycastHit;
				this.RaycastScreenPointToWorld(this.mGrabScreenPoint, out raycastHit);
				this.mWorldAnchorPoint = raycastHit.point;
				this.mMotionState = toState;
				return;
			}
			}
			Debug.LogError(string.Format("Camera unable to transition to state {0}", toState), this);
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x000AB19C File Offset: 0x000A939C
		private void MutateCameraPositionOrientation(ref Vector3 newPosition, ref Quaternion newOrientation, Vector3 worldScreenCenterPoint, float deltaTime)
		{
			Vector3 originalPosition = newPosition;
			Quaternion originalOrientation = newOrientation;
			Vector2 vector = UnityEngine.Input.mousePosition;
			RaycastHit raycastHit;
			switch (this.mMotionState)
			{
			case UserCamera.CameraMotionState.Pan:
				this.MutatePan(ref newPosition, ref newOrientation, vector, deltaTime);
				this.mGrabScreenPoint = vector;
				goto IL_374;
			case UserCamera.CameraMotionState.PanScreenSpace:
				this.MutatePanScreenSpace(ref newPosition, ref newOrientation, vector, this.mGrabScreenPoint, deltaTime);
				this.mGrabScreenPoint = vector;
				goto IL_374;
			case UserCamera.CameraMotionState.Orbit:
				this.MutateOrbit(ref newPosition, ref newOrientation, deltaTime);
				if (this.CamTargetInternal != null)
				{
					bool flag;
					this.MutateFollow(ref newPosition, ref newOrientation, vector, originalPosition, originalOrientation, deltaTime, out flag);
				}
				worldScreenCenterPoint = this.mWorldAnchorPoint;
				this.mGrabScreenPoint = vector;
				goto IL_374;
			case UserCamera.CameraMotionState.Homing:
				if (!this.MutateHoming(ref newPosition, ref newOrientation, vector, deltaTime))
				{
					newPosition += this.mWorldVelocityMS * deltaTime;
					goto IL_374;
				}
				if (this.CamTargetInternal != null)
				{
					this.ChangeCameraState(UserCamera.CameraMotionState.Following, this.HalfScreen, this.CamTargetInternal, ref newPosition, ref newOrientation);
					return;
				}
				this.ChangeCameraState(UserCamera.CameraMotionState.None, this.HalfScreen, null, ref newPosition, ref newOrientation);
				return;
			case UserCamera.CameraMotionState.PointTo:
				if (this.MutatePointTo(ref newPosition, ref newOrientation, vector, deltaTime))
				{
					this.ChangeCameraState(UserCamera.CameraMotionState.None, this.HalfScreen, ref newPosition, ref newOrientation);
					goto IL_374;
				}
				goto IL_374;
			case UserCamera.CameraMotionState.PointToAndFollow:
			{
				if (this.CamTargetInternal == null)
				{
					this.ChangeCameraState(UserCamera.CameraMotionState.None, this.HalfScreen, null, ref newPosition, ref newOrientation);
					goto IL_374;
				}
				bool flag2 = this.RaycastScreenPointToWorld(this.HalfScreen, out raycastHit);
				bool flag3;
				this.MutateFollow(ref newPosition, ref newOrientation, vector, originalPosition, originalOrientation, deltaTime, out flag3);
				if (flag3 && (this.SensorsManagerActive || !this.MutateMoveToTargetFramedView(ref newPosition, ref newOrientation, vector, flag2 ? raycastHit.point : this.mWorldAnchorPoint, deltaTime)))
				{
					this.ChangeCameraState(UserCamera.CameraMotionState.Following, this.HalfScreen, ref newPosition, ref newOrientation);
					goto IL_374;
				}
				goto IL_374;
			}
			case UserCamera.CameraMotionState.Following:
			{
				worldScreenCenterPoint = this.mWorldAnchorPoint;
				bool flag4;
				if (!this.MutateFollow(ref newPosition, ref newOrientation, vector, originalPosition, originalOrientation, deltaTime, out flag4))
				{
					this.ChangeCameraState(UserCamera.CameraMotionState.None, this.HalfScreen, null, ref newPosition, ref newOrientation);
				}
				UserCamera.CameraMotionState cameraMotionState = this.ProcessKeyMouseInput(deltaTime);
				if (cameraMotionState != this.mMotionState)
				{
					this.ChangeCameraState(cameraMotionState, this.HalfScreen, null, ref newPosition, ref newOrientation);
					goto IL_374;
				}
				goto IL_374;
			}
			case UserCamera.CameraMotionState.MoveToTargetCameraFrame:
				if (this.CamTargetInternal != null)
				{
					worldScreenCenterPoint = this.mWorldAnchorPoint;
					bool flag5;
					this.MutateFollow(ref newPosition, ref newOrientation, vector, originalPosition, originalOrientation, deltaTime, out flag5);
				}
				if (this.MutateMoveToTargetFramedView(ref newPosition, ref newOrientation, vector, this.CamTargetOrWorldAnchorPoint, deltaTime))
				{
					goto IL_374;
				}
				if (this.CamTargetInternal != null)
				{
					this.ChangeCameraState(UserCamera.CameraMotionState.Following, this.HalfScreen, this.CamTargetInternal, ref newPosition, ref newOrientation);
					goto IL_374;
				}
				this.ChangeCameraState(UserCamera.CameraMotionState.None, this.HalfScreen, null, ref newPosition, ref newOrientation);
				goto IL_374;
			case UserCamera.CameraMotionState.CinematicCamera:
				this.mGrabScreenPoint = this.HalfScreen;
				if (this.mActiveCinematicController.IsCameraMotionAllowed(UserCamera.CameraMotionTypes.Pan))
				{
					UserCamera.CameraMotionState cameraMotionState2 = this.ProcessKeyMouseInput(deltaTime);
					if (cameraMotionState2 != this.mMotionState)
					{
						this.ChangeCameraState(cameraMotionState2, this.HalfScreen, ref newPosition, ref newOrientation);
					}
				}
				newPosition += this.mWorldVelocityMS * deltaTime;
				goto IL_374;
			}
			this.mGrabScreenPoint = this.HalfScreen;
			this.ProcessKeyMouseInput(deltaTime);
			newPosition += this.mWorldVelocityMS * deltaTime;
			if (this.RaycastRayToWorld(new Ray(newPosition, newOrientation * Vector3.forward), out raycastHit))
			{
				this.mWorldAnchorPoint = raycastHit.point;
				worldScreenCenterPoint = raycastHit.point;
			}
			IL_374:
			if (this.mMotionState != UserCamera.CameraMotionState.MoveToTargetCameraFrame)
			{
				float num = this.mDesiredDistanceLERPValue - this.mCurrentDistanceLERPValue;
				float cameraDollyLERPEasingThreshold = this.MotionValues.DampeningValues.CameraDollyLERPEasingThreshold;
				float num2 = this.mCurrentDistanceLERPValue + Mathf.Clamp(num, -cameraDollyLERPEasingThreshold, cameraDollyLERPEasingThreshold) / cameraDollyLERPEasingThreshold * this.MouseScrollSpeed * deltaTime;
				if (Mathf.Sign(num) != Mathf.Sign(this.mDesiredDistanceLERPValue - num2))
				{
					num2 = this.mDesiredDistanceLERPValue;
				}
				this.mCurrentDistanceLERPValue = num2;
			}
			if ((!this.IsCinematicMode || this.mActiveCinematicController.IsCameraMotionAllowed(UserCamera.CameraMotionTypes.Constraints)) && this.mMotionState != UserCamera.CameraMotionState.MoveToTargetCameraFrame)
			{
				if (this.RaycastScreenPointToWorldFromPositionAndOrientation(this.HalfScreen, newPosition, newOrientation, out raycastHit) || this.CamTargetInternal != null)
				{
					Vector3 a = raycastHit.point;
					if (this.CamTargetInternal != null)
					{
						a = this.CamTargetInternal.transform.position;
					}
					else if (this.mMotionState == UserCamera.CameraMotionState.Orbit)
					{
						a = this.mWorldAnchorPoint;
					}
					Vector3 vector2 = a - newPosition;
					float magnitude = vector2.magnitude;
					if (magnitude < this.ClosestCameraDistanceToTarget)
					{
						newPosition -= (this.ClosestCameraDistanceToTarget - magnitude) * vector2.normalized;
						if (this.mMotionState == UserCamera.CameraMotionState.Orbit)
						{
							newOrientation = Quaternion.LookRotation(this.mWorldAnchorPoint - newPosition);
						}
					}
					else
					{
						float num3 = Mathf.Lerp(this.ClosestCameraDistanceToTarget, this.CurrentMaxDistance, this.CurrentDistanceLERPCurve.Evaluate(this.mCurrentDistanceLERPValue));
						num3 = Mathf.Lerp(magnitude, num3, this.MotionValues.DampeningValues.CameraDollyDistanceEasing * deltaTime);
						newPosition = a - vector2.normalized * num3;
					}
				}
				Vector3 vector3 = newOrientation * Vector3.forward;
				Vector3 vector4 = vector3;
				float y = this.ViewConstraints.center.y;
				if (this.RaycastRayToWorld(new Ray(newPosition, vector3), out raycastHit))
				{
					y = raycastHit.point.y;
				}
				else
				{
					y = (newPosition + vector3 * this.mDistanceToTarget).y;
				}
				vector4 *= (newPosition.y - y) / Mathf.Abs(vector4.y);
				vector4 += newPosition;
				vector4.y = y;
				if (!this.IsPointOnViewConstraints(vector4) && this.CamTargetInternal == null)
				{
					Vector3 a2 = this.ViewConstraints.ClosestPoint(vector4);
					a2.Set(a2.x, vector4.y, a2.z);
					if (this.RaycastRayToWorld(new Ray(a2 - vector3 * 10000f, vector3), out raycastHit))
					{
						a2 = raycastHit.point;
					}
					newPosition = a2 - newOrientation * Vector3.forward * this.DistanceToTargetMeters;
					this.mDragWorldVelocityMS = Vector3.zero;
					this.mWorldVelocityMS = Vector3.zero;
					if (this.mMotionState == UserCamera.CameraMotionState.Orbit)
					{
						newOrientation = Quaternion.LookRotation(this.mWorldAnchorPoint - newPosition);
					}
					else if (this.mMotionState == UserCamera.CameraMotionState.PointTo || this.mMotionState == UserCamera.CameraMotionState.PointToAndFollow || this.mMotionState == UserCamera.CameraMotionState.Homing)
					{
						this.ChangeCameraState(UserCamera.CameraMotionState.None, UnityEngine.Input.mousePosition, null, ref newPosition, ref newOrientation);
					}
					else if (this.mMotionState == UserCamera.CameraMotionState.Following)
					{
						this.mDistanceToTarget = Vector3.Distance(this.CamTargetInternal.position, newPosition);
					}
					else if (this.mMotionState == UserCamera.CameraMotionState.Pan)
					{
						this.RaycastScreenPointToWorldFromPositionAndOrientation(vector, newPosition, newOrientation, out raycastHit);
						this.mWorldAnchorPoint = raycastHit.point;
					}
					else if (this.mMotionState == UserCamera.CameraMotionState.PanScreenSpace)
					{
						Ray rayFromScreenPointWithPositionAndRotation = this.GetRayFromScreenPointWithPositionAndRotation(vector, newPosition, newOrientation);
						this.mWorldAnchorPoint = this.ProjectRayToCameraBounds(rayFromScreenPointWithPositionAndRotation);
					}
					else if (this.mMotionState == UserCamera.CameraMotionState.None)
					{
						this.mWorldAnchorPoint = a2;
					}
				}
			}
			if (!this.IsCinematicMode || this.mActiveCinematicController.IsCameraMotionAllowed(UserCamera.CameraMotionTypes.Orbit))
			{
				this.PerformCameraOrbit(ref newPosition, ref newOrientation, worldScreenCenterPoint, deltaTime);
			}
			if (!this.IsCinematicMode || this.mActiveCinematicController.IsCameraMotionAllowed(UserCamera.CameraMotionTypes.Constraints))
			{
				this.CorrectForCameraCollision(ref newPosition, ref newOrientation, originalPosition, originalOrientation, worldScreenCenterPoint, deltaTime);
			}
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x000AB984 File Offset: 0x000A9B84
		private void MutatePan(ref Vector3 newPosition, ref Quaternion newOrientation, Vector2 mousePosition, float deltaTime)
		{
			Ray rayFromScreenPointWithPositionAndRotation = this.GetRayFromScreenPointWithPositionAndRotation(mousePosition, newPosition, newOrientation);
			RaycastHit raycastHit;
			if (!this.RaycastRayToWorld(rayFromScreenPointWithPositionAndRotation, out raycastHit) || Vector3.Angle(Vector3.up, rayFromScreenPointWithPositionAndRotation.direction) < 90f + this.Thresholds.ScreenSpacePanCutOffAngleFromHorizon)
			{
				this.ChangeCameraState(UserCamera.CameraMotionState.PanScreenSpace, mousePosition, ref newPosition, ref newOrientation);
			}
			Vector3 camTargetOrWorldAnchorPoint = this.CamTargetOrWorldAnchorPoint;
			RaycastHit raycastHit2;
			bool flag = this.RaycastScreenPointToWorldFromPositionAndOrientation(mousePosition, newPosition, newOrientation, out raycastHit2);
			Vector3 vector = raycastHit2.point;
			if (flag)
			{
				float num = camTargetOrWorldAnchorPoint.y - vector.y;
				if (num != 0f)
				{
					Vector3 vector2 = (this.Position - camTargetOrWorldAnchorPoint).normalized;
					vector2 *= num / vector2.y;
					vector += vector2;
				}
				Vector3 a = camTargetOrWorldAnchorPoint - vector;
				Vector3 vector3 = this.CamTargetOrWorldAnchorPoint - vector;
				if (vector3.sqrMagnitude < a.sqrMagnitude)
				{
					a = vector3;
				}
				a.Set(a.x, 0f, a.z);
				this.mDragWorldVelocityMS = Vector3.Lerp(this.mDragWorldVelocityMS, a / deltaTime, 20f * deltaTime);
				this.mWorldVelocityMS = this.mDragWorldVelocityMS;
				newPosition += Vector3.Lerp(a, Vector3.zero, deltaTime);
			}
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x000ABAF0 File Offset: 0x000A9CF0
		private void MutatePanScreenSpace(ref Vector3 newPosition, ref Quaternion newOrientation, Vector2 mousePosition, Vector2 oldMousePosition, float deltaTime)
		{
			Ray rayFromScreenPointWithPositionAndRotation = this.GetRayFromScreenPointWithPositionAndRotation(mousePosition, newPosition, newOrientation);
			bool flag = Vector3.Angle(Vector3.up, rayFromScreenPointWithPositionAndRotation.direction) > 90f + this.Thresholds.ScreenSpacePanCutOffAngleFromHorizon;
			RaycastHit raycastHit;
			if (this.RaycastRayToWorld(rayFromScreenPointWithPositionAndRotation, out raycastHit) && flag)
			{
				this.mWorldAnchorPoint = raycastHit.point;
				this.ChangeCameraState(UserCamera.CameraMotionState.Pan, mousePosition, ref newPosition, ref newOrientation);
				return;
			}
			if (flag)
			{
				Vector3 b = this.ProjectRayToCameraBounds(rayFromScreenPointWithPositionAndRotation);
				Vector3 a = this.mWorldAnchorPoint - b;
				a.Set(a.x, 0f, a.z);
				this.mDragWorldVelocityMS = Vector3.Lerp(this.mDragWorldVelocityMS, a / deltaTime, 20f * deltaTime);
				this.mWorldVelocityMS = this.mDragWorldVelocityMS;
				newPosition += Vector3.Lerp(a, Vector3.zero, deltaTime);
				return;
			}
			Vector2 a2 = mousePosition - oldMousePosition;
			a2.Set(a2.x / (float)Screen.width, a2.y / (float)Screen.height);
			a2 *= -this.MotionValues.ScreenSpacePanVelocityMSPerScreen * deltaTime;
			Vector3 vector = rayFromScreenPointWithPositionAndRotation.direction * a2.y + Quaternion.AngleAxis(90f, Vector3.up) * rayFromScreenPointWithPositionAndRotation.direction * a2.x;
			vector.y = 0f;
			this.mDragWorldVelocityMS = Vector3.Lerp(this.mDragWorldVelocityMS, vector / deltaTime, 20f * deltaTime);
			this.mWorldVelocityMS = this.mDragWorldVelocityMS;
			newPosition += vector;
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x000ABCB0 File Offset: 0x000A9EB0
		private void MutateOrbit(ref Vector3 newPosition, ref Quaternion newOrientation, float deltaTime)
		{
			bool flag = !this.IsCinematicMode || (this.IsCinematicMode && this.mActiveCinematicController.IsCameraMotionAllowed(UserCamera.CameraMotionTypes.Orbit));
			float num = (!this.IsCinematicMode || (this.IsCinematicMode && this.mActiveCinematicController.IsCameraMotionAllowed(UserCamera.CameraMotionTypes.Pitch))) ? this.MouseXOrbitSensitivity : 0f;
			float num2 = flag ? this.MouseYOrbitSensitivity : 0f;
			Vector2 vector = new Vector2(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y"));
			float num3 = vector.y * this.MotionValues.AngularVelocityAccelerationDegPerSec;
			float num4 = vector.x * this.MotionValues.AngularVelocityAccelerationDegPerSec;
			this.mOrbitAngularVelocityDegPerSec += new Vector2(-num3 * num, num4 * num2);
			float distanceToTargetMeters = this.DistanceToTargetMeters;
			float num5 = this.MotionValues.MinPitchVersusDistanceFromTarget.Evaluate(distanceToTargetMeters);
			if (Mathf.Abs(newOrientation.eulerAngles.x - num5) <= 0.5f && this.mOrbitAngularVelocityDegPerSec.x < 0f)
			{
				this.mOrbitAngularVelocityDegPerSec.x = 0f;
			}
			if (vector.magnitude < 1f)
			{
				this.mOrbitAngularVelocityDegPerSec.x = Mathf.Lerp(this.mOrbitAngularVelocityDegPerSec.x, 0f, deltaTime * this.MotionValues.DampeningValues.CameraOrbitXAngularDampening);
				this.mOrbitAngularVelocityDegPerSec.y = Mathf.Lerp(this.mOrbitAngularVelocityDegPerSec.y, 0f, deltaTime * this.MotionValues.DampeningValues.CameraOrbitYAngularDampening);
			}
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x000ABE5C File Offset: 0x000AA05C
		private bool MutateHoming(ref Vector3 newPosition, ref Quaternion newOrientation, Vector2 mousePosition, float deltaTime)
		{
			if (!this.SensorsManagerActive)
			{
				UserCamera.CamFramingSettings defaultView = this.MotionValues.DefaultView;
				newOrientation = Quaternion.Euler(defaultView.Orientation.x, defaultView.Orientation.y, 0f);
				newPosition = this.mWorldAnchorPoint - newOrientation * Vector3.forward * defaultView.FinalDistanceToTarget;
				this.mDesiredDistanceLERPValue = defaultView.LERPCurvePosition;
				this.mCurrentDistanceLERPValue = this.mDesiredDistanceLERPValue;
			}
			else
			{
				UserCamera.CamFramingSettings defaultSensorsView = this.MotionValues.DefaultSensorsView;
				newOrientation = Quaternion.Euler(defaultSensorsView.Orientation.x, defaultSensorsView.Orientation.y, 0f);
				newPosition = this.mWorldAnchorPoint - newOrientation * Vector3.forward * defaultSensorsView.FinalDistanceToTarget;
				this.mDesiredDistanceLERPValue = defaultSensorsView.LERPCurvePosition;
				this.mCurrentDistanceLERPValue = this.mDesiredDistanceLERPValue;
			}
			return true;
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x000ABF60 File Offset: 0x000AA160
		private bool MutatePointTo(ref Vector3 newPosition, ref Quaternion newOrientation, Vector2 mousePosition, float deltaTime)
		{
			Quaternion quaternion = Quaternion.LookRotation(this.CamTargetOrWorldAnchorPoint - this.Position);
			float num = newOrientation.eulerAngles.y;
			float num2 = quaternion.eulerAngles.y;
			float num3 = this.CalculateLowestPitchForDistance((this.CamTargetOrWorldAnchorPoint - this.Position).magnitude);
			float num4 = newOrientation.eulerAngles.x;
			float num5 = quaternion.eulerAngles.x;
			if (num5 < num3)
			{
				num5 = num4;
			}
			if (num >= 270f && num2 <= 90f && num > num2)
			{
				num2 += 360f;
			}
			else if (num2 >= 270f && num <= 90f && num2 > num)
			{
				num += 360f;
			}
			if (num4 >= 270f && num5 <= 90f && num4 > num5)
			{
				num5 += 360f;
			}
			else if (num5 >= 270f && num4 <= 90f && num5 > num4)
			{
				num4 += 360f;
			}
			float num6 = num5 - num4;
			float num7 = num6 * this.MotionValues.PitchCorrectionAngularVelocityDegPerSec * deltaTime;
			float num8 = num2 - num;
			float num9 = num8 * this.MotionValues.PitchCorrectionAngularVelocityDegPerSec * deltaTime;
			if (new Vector2(num6, num8).magnitude < this.Thresholds.MinPointToAngleError)
			{
				return true;
			}
			newOrientation = Quaternion.Euler(newOrientation.eulerAngles.x + num7, newOrientation.eulerAngles.y + num9, newOrientation.eulerAngles.z);
			return false;
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x000AC0E8 File Offset: 0x000AA2E8
		private bool MutateFollow(ref Vector3 newPosition, ref Quaternion newOrientation, Vector2 mousePosition, Vector3 originalPosition, Quaternion originalOrientation, float deltaTime, out bool locked)
		{
			if (this.CamTargetInternal == null)
			{
				locked = false;
				return false;
			}
			bool flag = false;
			RaycastHit raycastHit;
			if (this.RaycastRayToWorld(new Ray(this.CamTargetInternal.position + Vector3.up * this.Thresholds.FurthestCameraDistanceSensors, Vector3.down), out raycastHit))
			{
				flag = (raycastHit.point.y - this.Thresholds.MinimumDistanceToTargetM * 0.5f > this.CamTargetInternal.position.y);
			}
			if (flag)
			{
				this.CamTargetInternal = null;
				locked = false;
				return false;
			}
			newPosition = this.CamTargetInternal.position + newOrientation * Vector3.back * this.DistanceToTargetMeters;
			locked = true;
			return true;
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x000AC1BC File Offset: 0x000AA3BC
		private bool MutateMoveToTargetFramedView(ref Vector3 newPosition, ref Quaternion newOrientation, Vector2 mousePosition, Vector3 anchorPoint, float deltaTime)
		{
			float num = Mathf.Clamp01(this.mSensorsTransitionTime / this.MotionValues.SensorsTransitionPeriodSeconds);
			float furthestCameraDistanceSensors = this.Thresholds.FurthestCameraDistanceSensors;
			if (num >= 1f)
			{
				this.mDesiredDistanceLERPValue = this.mGoalViewSettings.LERPCurvePosition;
				this.mCurrentDistanceLERPValue = this.mDesiredDistanceLERPValue;
				this.mOrbitAngularVelocityDegPerSec = Vector2.zero;
				this.mWorldVelocityMS = Vector3.zero;
				newOrientation = Quaternion.Euler(new Vector3(this.mGoalViewSettings.Orientation.x, this.mGoalViewSettings.Orientation.y, 0f));
				newPosition = this.mGoalViewSettings.WorldAnchorPoint - newOrientation * Vector3.forward * this.mGoalViewSettings.FinalDistanceToTarget;
				return false;
			}
			num = Mathf.Clamp01(this.MotionValues.SensorsTransitionLERPCurve.Evaluate(num));
			Quaternion a = Quaternion.Euler(this.mTransitioningFromSettings.Orientation.x, this.mTransitioningFromSettings.Orientation.y, 0f);
			Quaternion b = Quaternion.Euler(this.mGoalViewSettings.Orientation.x, this.mGoalViewSettings.Orientation.y, 0f);
			newOrientation = Quaternion.Slerp(a, b, num);
			Vector3 a2 = newOrientation * Vector3.forward;
			newPosition = Vector3.Lerp(this.mSensorsViewSettings.WorldAnchorPoint, this.mGameViewSettings.WorldAnchorPoint, num) - a2 * Mathf.Lerp(this.mTransitioningFromSettings.FinalDistanceToTarget, this.mGoalViewSettings.FinalDistanceToTarget, num);
			this.mSensorsTransitionTime += deltaTime;
			return true;
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x000AC37D File Offset: 0x000AA57D
		private void MutateZoomVelocity(float zoomAmount)
		{
			this.MutateZoomVelocity(zoomAmount, false);
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x000AC388 File Offset: 0x000AA588
		private void MutateZoomVelocity(float zoomAmount, bool forceToSpeed)
		{
			this.mDesiredDistanceLERPValue += zoomAmount / (float)this.ScrollTicksForCameraMode;
			this.mDesiredDistanceLERPValue = Mathf.Clamp01(this.mDesiredDistanceLERPValue);
			this.mOrbitAngularVelocityDegPerSec = Vector2.zero;
			if (!Mathf.Approximately(this.mDesiredDistanceLERPValue, this.mCurrentDistanceLERPValue) && Mathf.Sign(zoomAmount) != Mathf.Sign(this.mDesiredDistanceLERPValue - this.mCurrentDistanceLERPValue))
			{
				this.mDesiredDistanceLERPValue = this.mCurrentDistanceLERPValue;
				return;
			}
			if (this.DistanceToTargetMeters >= this.CurrentMaxDistance * 0.98f && zoomAmount > 0f)
			{
				this.mDesiredDistanceLERPValue = this.mCurrentDistanceLERPValue;
				return;
			}
			if (this.DistanceToTargetMeters <= this.ClosestCameraDistanceToTarget * 1.02f && zoomAmount < 0f)
			{
				this.mDesiredDistanceLERPValue = this.mCurrentDistanceLERPValue;
			}
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x000AC454 File Offset: 0x000AA654
		private UserCamera.CameraMotionState ProcessKeyMouseInput(float deltaTime)
		{
			UserCamera.CameraMotionState result = this.mMotionState;
			if (!this.HasInputFocus)
			{
				return result;
			}
			bool flag = false;
			float num = 0f;
			float num2 = 0f;
			if (!this.mInputSystem.BlockHotkeys)
			{
				float num3 = this.mScreenWidthInMeters * this.MaxPanVelocityPercentScreenWidthPerSecond * deltaTime;
				ControlSettings controls = ShipbreakersMain.UserSettings.Controls;
				if (UnityEngine.Input.GetKey(controls.HotKeyDefinitions[HotKeyOperation.PanCameraLeft].Combo.Primary))
				{
					num -= num3;
					flag = true;
				}
				if (UnityEngine.Input.GetKey(controls.HotKeyDefinitions[HotKeyOperation.PanCameraRight].Combo.Primary))
				{
					num += num3;
					flag = true;
				}
				if (UnityEngine.Input.GetKey(controls.HotKeyDefinitions[HotKeyOperation.PanCameraUp].Combo.Primary))
				{
					num2 += num3;
					flag = true;
				}
				if (UnityEngine.Input.GetKey(controls.HotKeyDefinitions[HotKeyOperation.PanCameraDown].Combo.Primary))
				{
					num2 -= num3;
					flag = true;
				}
			}
			if (flag)
			{
				this.mOrbitAngularVelocityDegPerSec = Vector2.zero;
				if (this.mMotionState == UserCamera.CameraMotionState.Following)
				{
					result = UserCamera.CameraMotionState.None;
				}
				this.mWorldVelocityMS += this.ConvertLocalDirectionToWorld(new Vector3(num, 0f, num2) * ShipbreakersMain.UserSettings.Gameplay.CameraEdgePanSpeed);
			}
			return result;
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x000AC598 File Offset: 0x000AA798
		private void PerformCameraOrbit(ref Vector3 newPosition, ref Quaternion newOrientation, Vector3 originalCamHalfScreenWorldPoint, float deltaTime)
		{
			float num = this.mOrbitAngularVelocityDegPerSec.x * deltaTime;
			if (num + newOrientation.eulerAngles.x > 87f)
			{
				num = 87f - newOrientation.eulerAngles.x;
				this.mOrbitAngularVelocityDegPerSec.x = num / deltaTime;
			}
			Vector3 a;
			RaycastHit raycastHit;
			if (this.CamTargetInternal != null)
			{
				a = this.CamTargetInternal.position;
			}
			else if (this.mMotionState == UserCamera.CameraMotionState.Orbit)
			{
				a = this.mWorldAnchorPoint;
			}
			else if (this.RaycastRayToWorld(new Ray(newPosition, newOrientation * Vector3.forward), out raycastHit))
			{
				a = raycastHit.point;
			}
			else
			{
				a = originalCamHalfScreenWorldPoint;
			}
			float magnitude = (a - newPosition).magnitude;
			if (!this.IsCinematicMode && this.mMotionState != UserCamera.CameraMotionState.Orbit && this.mMotionState != UserCamera.CameraMotionState.MoveToTargetCameraFrame)
			{
				float num2 = this.MotionValues.DefaultPitchVersusDistance.Evaluate(magnitude);
				float num3 = num2 - newOrientation.eulerAngles.x;
				if (!this.AutoPitch && this.mDesiredDistanceLERPValue - this.mCurrentDistanceLERPValue >= 0f && !Mathf.Approximately(this.mDesiredDistanceLERPValue, this.mCurrentDistanceLERPValue) && magnitude >= this.Thresholds.AutoPitchEngageDistance && num3 > 0f)
				{
					num = Mathf.Min(num3, this.MotionValues.DefaultPitchRecoveryAngularVelocityDegPerSec * deltaTime);
					this.mOrbitAngularVelocityDegPerSec.x = num / deltaTime;
					num3 -= num;
				}
				if (num3 >= 0f && num3 <= this.Thresholds.AutoPitchEngageAngleErrorDegrees && magnitude >= this.Thresholds.AutoPitchEngageDistance)
				{
					this.AutoPitch = true;
				}
			}
			if (!this.IsCinematicMode || this.mActiveCinematicController.IsCameraMotionAllowed(UserCamera.CameraMotionTypes.ConstrainedPitch))
			{
				float num4 = this.CalculateLowestPitchForDistance(magnitude);
				if (num + newOrientation.eulerAngles.x < num4)
				{
					float num5 = num4 - newOrientation.eulerAngles.x;
					if (Mathf.Abs(num4) < 0.05f)
					{
						num = 0f;
						this.mOrbitAngularVelocityDegPerSec.x = 0f;
					}
					else if (num <= 0f && num4 > newOrientation.eulerAngles.x)
					{
						num = num5;
						this.mOrbitAngularVelocityDegPerSec.x = 0f;
					}
				}
				else if (this.AutoPitch && num + newOrientation.eulerAngles.x > num4)
				{
					float num6 = this.SensorsManagerActive ? this.MotionValues.PitchCorrectionAngularVelocitySensorsDegPerSec : this.MotionValues.PitchCorrectionAngularVelocityDegPerSec;
					if (this.mMotionState == UserCamera.CameraMotionState.MoveToTargetCameraFrame)
					{
						num6 = Mathf.Max(num6, this.MotionValues.PitchCorrectionAngularVelocitySensorsDegPerSec);
					}
					num = Mathf.Clamp(num4 - newOrientation.eulerAngles.x, -1f, 1f) * num6 * deltaTime;
					this.mOrbitAngularVelocityDegPerSec.x = num / deltaTime;
				}
			}
			if (this.AutoPitch)
			{
				float num7 = this.MotionValues.DefaultPitchVersusDistance.Evaluate(magnitude) - newOrientation.eulerAngles.x;
				if (Mathf.Abs(num7) < 0.05f)
				{
					num = 0f;
					this.mOrbitAngularVelocityDegPerSec.x = 0f;
				}
				else if (Mathf.Abs(num7) < Mathf.Abs(num))
				{
					num = num7;
					this.mOrbitAngularVelocityDegPerSec.x = num / deltaTime;
				}
			}
			float num8 = this.mOrbitAngularVelocityDegPerSec.y * deltaTime;
			if (num8 != 0f || num != 0f)
			{
				Vector3 vector = newOrientation.eulerAngles + new Vector3(num, num8, 0f);
				newOrientation = Quaternion.Euler(new Vector3(Mathf.Clamp(vector.x, this.Thresholds.LowestPitchDeg, 87f), vector.y, vector.z));
				newPosition = a - newOrientation * Vector3.forward * magnitude;
			}
		}

		// Token: 0x06001D32 RID: 7474 RVA: 0x000AC97C File Offset: 0x000AAB7C
		private void CorrectForCameraCollision(ref Vector3 newPosition, ref Quaternion newOrientation, Vector3 originalPosition, Quaternion originalOrientation, Vector3 originalCamHalfScreenWorldPoint, float deltaTime)
		{
			if (this.DisableAllCollision)
			{
				return;
			}
			Vector3 origin = newPosition;
			origin.y = 10000f;
			Vector3 vector = newPosition - originalPosition;
			float magnitude = vector.magnitude;
			RaycastHit raycastHit;
			this.mColliding = this.SphereCastIntoWorld(originalPosition, this.mCollider.radius, vector.normalized, magnitude, out raycastHit);
			RaycastHit raycastHit2;
			bool flag = this.RaycastRayToWorld(new Ray(origin, Vector3.down), out raycastHit2);
			if (!this.mColliding && flag)
			{
				float num = newPosition.y - this.mCollider.radius;
				bool flag2 = num - raycastHit2.point.y <= 0f;
				this.mColliding = (this.mColliding || flag2);
				if (flag2)
				{
					raycastHit = raycastHit2;
				}
			}
			if (this.mColliding)
			{
				this.mDragWorldVelocityMS.Set(0f, 0f, 0f);
				if (this.mOrbitAngularVelocityDegPerSec.x < 0f)
				{
					this.mOrbitAngularVelocityDegPerSec.Set(0f, this.mOrbitAngularVelocityDegPerSec.y);
				}
				Vector3 vector2 = this.mWorldAnchorPoint;
				if (this.mMotionState == UserCamera.CameraMotionState.Pan)
				{
					vector2 = originalCamHalfScreenWorldPoint;
				}
				this.SphereCastIntoWorld(newPosition + Vector3.up * 100f, this.mCollider.radius, Vector3.down, 200f, out raycastHit);
				float y = raycastHit2.point.y + this.mCollider.radius;
				newPosition.y = y;
				newOrientation = Quaternion.LookRotation(vector2 - newPosition);
				if (this.mMotionState == UserCamera.CameraMotionState.Pan)
				{
					if (this.RaycastScreenPointToWorldFromPositionAndOrientation(this.mGrabScreenPoint, newPosition, newOrientation, out raycastHit))
					{
						this.mWorldAnchorPoint = raycastHit.point;
						this.mDistanceToTarget = Vector3.Distance(newPosition, vector2);
						this.mCurrentDistanceLERPValue = this.FindLERPValueForDistanceInCurrentLERPCurve(this.mDistanceToTarget);
						this.mDesiredDistanceLERPValue = this.mCurrentDistanceLERPValue;
						return;
					}
				}
				else if (this.mMotionState == UserCamera.CameraMotionState.Orbit || !Mathf.Approximately(this.mOrbitAngularVelocityDegPerSec.sqrMagnitude, 0f))
				{
					newOrientation = Quaternion.LookRotation(this.CamTargetOrWorldAnchorPoint - newPosition);
					return;
				}
			}
			else if (!flag && this.IsPointOnViewConstraints(newPosition))
			{
				newPosition = originalPosition;
				newOrientation = originalOrientation;
				this.mWorldVelocityMS = Vector3.zero;
				this.mDragWorldVelocityMS = Vector3.zero;
			}
		}

		// Token: 0x06001D33 RID: 7475 RVA: 0x000ACBF8 File Offset: 0x000AADF8
		private float CalculateLowestPitchForDistance(float distanceKm)
		{
			float result = Mathf.Clamp(this.MotionValues.MinPitchVersusDistanceFromTarget.Evaluate(distanceKm), this.Thresholds.LowestPitchDeg, 87f);
			if (this.mAutoPitch && !this.mDisableAutoPitchUntilNextZoom)
			{
				result = Mathf.Clamp(this.MotionValues.DefaultPitchVersusDistance.Evaluate(distanceKm), this.Thresholds.LowestPitchDeg, 87f);
			}
			return result;
		}

		// Token: 0x06001D34 RID: 7476 RVA: 0x000ACC64 File Offset: 0x000AAE64
		private void UpdateViewWidthInM(float deltaTime)
		{
			this.mScreenWidthInMetersUndamped = 2f * this.DistanceToTargetMeters * Mathf.Tan(0.017453292f * this.ActiveCamera.fieldOfView);
			if (Mathf.Abs((this.mScreenWidthInMetersUndamped - this.mScreenWidthInMeters) / this.mScreenWidthInMetersUndamped) > 0.02f)
			{
				this.mScreenWidthInMeters = Mathf.Lerp(this.mScreenWidthInMeters, this.mScreenWidthInMetersUndamped, 25f * deltaTime);
			}
		}

		// Token: 0x06001D35 RID: 7477 RVA: 0x000ACCD8 File Offset: 0x000AAED8
		private void ConstrainCameraVelocities()
		{
			float num = this.mWorldVelocityMS.x;
			float num2 = this.mWorldVelocityMS.z;
			float num3 = this.MaxPanVelocityPercentScreenWidthPerSecond * this.mScreenWidthInMeters;
			num = Mathf.Clamp(num, -num3, num3);
			num2 = Mathf.Clamp(num2, -num3, num3);
			this.mWorldVelocityMS.Set(num, this.mWorldVelocityMS.y, num2);
			float mouseXOrbitSensitivity = this.MouseXOrbitSensitivity;
			float mouseYOrbitSensitivity = this.MouseYOrbitSensitivity;
			if (float.IsNaN(this.mOrbitAngularVelocityDegPerSec.x))
			{
				Debug.LogError(string.Format("Camera X orbit velocity is bad! {0}", this.mOrbitAngularVelocityDegPerSec.x));
				this.mOrbitAngularVelocityDegPerSec.x = 0f;
			}
			if (float.IsNaN(this.mOrbitAngularVelocityDegPerSec.y))
			{
				Debug.LogError(string.Format("Camera Y orbit velocity is bad! {0}", this.mOrbitAngularVelocityDegPerSec.y));
				this.mOrbitAngularVelocityDegPerSec.y = 0f;
			}
			this.mOrbitAngularVelocityDegPerSec.Set(Mathf.Clamp(this.mOrbitAngularVelocityDegPerSec.x, -this.MotionValues.AngularVelocityMaxDegPerSec * mouseXOrbitSensitivity, this.MotionValues.AngularVelocityMaxDegPerSec * mouseXOrbitSensitivity), Mathf.Clamp(this.mOrbitAngularVelocityDegPerSec.y, -this.MotionValues.AngularVelocityMaxDegPerSec * mouseYOrbitSensitivity, this.MotionValues.AngularVelocityMaxDegPerSec * mouseYOrbitSensitivity));
		}

		// Token: 0x06001D36 RID: 7478 RVA: 0x000ACE2C File Offset: 0x000AB02C
		private void CameraGestureHandler(InputEventArgs args)
		{
			if (!this.HasInputFocus)
			{
				return;
			}
			bool flag = this.mMotionState == UserCamera.CameraMotionState.None || this.mMotionState == UserCamera.CameraMotionState.Following || (args.Modifiers == EventModifiers.None && this.mMotionState == UserCamera.CameraMotionState.MoveToTargetCameraFrame);
			if (!(flag & !this.IsCinematicMode) && (!this.IsCinematicMode || !this.mActiveCinematicController.IsCameraMotionAllowed(UserCamera.CameraMotionTypes.Zoom)))
			{
				return;
			}
			if (args.SubType == InputEventSubType.Gesture_Scroll)
			{
				float zoomAmount = Mathf.Sign(args.Value) * this.MouseScrollSpeed;
				this.MutateZoomVelocity(zoomAmount);
				args.HandledByFlags |= InputHandledFlags.Camera;
				this.mDisableAutoPitchUntilNextZoom = false;
			}
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x000ACED0 File Offset: 0x000AB0D0
		private void CameraPanInputHandler(InputEventArgs args)
		{
			if (!this.HasInputFocus)
			{
				return;
			}
			bool flag = this.mMotionState == UserCamera.CameraMotionState.Pan || this.mMotionState == UserCamera.CameraMotionState.PanScreenSpace || this.mMotionState == UserCamera.CameraMotionState.Orbit;
			switch (args.Phase)
			{
			case InputEventPhase.Begin:
				if ((args.HandledByFlags & InputHandledFlags.Abilities) != InputHandledFlags.None || (args.HandledByFlags & InputHandledFlags.Bandbox) != InputHandledFlags.None)
				{
					return;
				}
				if (args.Modifiers != EventModifiers.Shift && args.Code == -2)
				{
					this.mPanOrOrbitCanBegin = true;
					args.HandledByFlags |= InputHandledFlags.Camera;
					return;
				}
				if (args.Code == -3 || (args.Modifiers == EventModifiers.Shift && args.Code == -2))
				{
					this.mPanOrOrbitCanBegin = true;
					args.HandledByFlags |= InputHandledFlags.Camera;
					return;
				}
				break;
			case InputEventPhase.Move:
				if (flag || !this.mPanOrOrbitCanBegin || this.mMotionState == UserCamera.CameraMotionState.MoveToTargetCameraFrame)
				{
					if (this.LockCursorWhenOrbiting && this.mMotionState == UserCamera.CameraMotionState.Orbit)
					{
						NativePlatform.SetCursorPos(this.mOriginalMousePosX, this.mOriginalMousePosY);
					}
					args.HandledByFlags |= InputHandledFlags.Camera;
					return;
				}
				if (args.Modifiers != EventModifiers.Shift && args.Code == -2)
				{
					if (!this.IsCinematicMode || this.mActiveCinematicController.IsCameraMotionAllowed(UserCamera.CameraMotionTypes.Orbit))
					{
						this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.Orbit, args.ScreenPosition);
						if (this.LockCursorWhenOrbiting)
						{
							NativePlatform.GetCursorPos(out this.mOriginalMousePosX, out this.mOriginalMousePosY);
						}
					}
					args.HandledByFlags |= InputHandledFlags.Camera;
					return;
				}
				if (args.Code == -3 || (args.Modifiers == EventModifiers.Shift && args.Code == -2))
				{
					if (!this.IsCinematicMode || this.mActiveCinematicController.IsCameraMotionAllowed(UserCamera.CameraMotionTypes.Pan))
					{
						RaycastHit raycastHit;
						if (this.RaycastScreenPointToWorld(this.mGrabScreenPoint, out raycastHit))
						{
							Vector3 position = new Vector3(args.ScreenPosition.x, args.ScreenPosition.y, 0f);
							float num = Vector3.Angle(Vector3.up, this.mCam.ScreenPointToRay(position).direction);
							if (num < this.Thresholds.ScreenSpacePanCutOffAngleFromHorizon + 90f)
							{
								this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.PanScreenSpace, args.ScreenPosition, null);
							}
							else
							{
								this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.Pan, args.ScreenPosition, null);
							}
							args.HandledByFlags |= InputHandledFlags.Camera;
						}
						else
						{
							this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.PanScreenSpace, args.ScreenPosition, null);
						}
					}
					args.HandledByFlags |= InputHandledFlags.Camera;
					return;
				}
				break;
			case InputEventPhase.Stationary:
				break;
			case InputEventPhase.End:
			case InputEventPhase.Cancel:
				this.mPanOrOrbitCanBegin = false;
				if (flag)
				{
					if (this.LockCursorWhenOrbiting && this.mMotionState == UserCamera.CameraMotionState.Orbit)
					{
						NativePlatform.SetCursorPos(this.mOriginalMousePosX, this.mOriginalMousePosY);
					}
					args.HandledByFlags |= InputHandledFlags.Camera;
					if (this.IsCinematicMode)
					{
						this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.CinematicCamera, this.HalfScreen);
						return;
					}
					this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.None, args.ScreenPosition);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x000AD19F File Offset: 0x000AB39F
		private void OnDefaultCameraKeyPressed(InputEventArgs args)
		{
			this.SteppedHomeCamera();
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x000AD1A8 File Offset: 0x000AB3A8
		private float FindLERPValueForDistanceInLERPCurve(AnimationCurve forCurve, float minValue, float maxValue, float desiredDistance)
		{
			float num = 0.5f;
			float num2 = num;
			float num3 = Mathf.InverseLerp(minValue, maxValue, desiredDistance);
			if (num3 <= 0f || num3 >= 1f)
			{
				return Mathf.Clamp01(num3);
			}
			float num4 = forCurve.Evaluate(num2);
			while (num > 1.525878E-05f && !Mathf.Approximately(num4, num3))
			{
				num /= 2f;
				if (Mathf.Approximately(num4, num3))
				{
					break;
				}
				if (num3 > num4)
				{
					num2 += num;
				}
				else if (num3 < num4)
				{
					num2 -= num;
				}
				num4 = forCurve.Evaluate(num2);
			}
			return num2;
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x000AD22D File Offset: 0x000AB42D
		private float FindLERPValueForDistanceInCurrentLERPCurve(float distance)
		{
			return this.FindLERPValueForDistanceInLERPCurve(this.CurrentDistanceLERPCurve, this.ClosestCameraDistanceToTarget, this.CurrentMaxDistance, distance);
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x000AD248 File Offset: 0x000AB448
		public void InitializeDefaultViewSettings(float defaultHeading)
		{
			this.MotionValues.DefaultView.Orientation = new Vector2(Mathf.Clamp(this.MotionValues.DefaultView.Orientation.x, this.Thresholds.LowestPitchDeg, 87f), defaultHeading);
			float num = this.FindLERPValueForDistanceInLERPCurve(this.MotionValues.ScrollTickLERPCurveGameplay, this.Thresholds.MinimumDistanceToTargetM, this.Thresholds.FurthestCameraDistanceGameplay, this.MotionValues.DefaultView.FinalDistanceToTarget);
			this.mDesiredDistanceLERPValue = num;
			this.mCurrentDistanceLERPValue = this.mDesiredDistanceLERPValue;
			this.MotionValues.DefaultView.FinalDistanceToTarget = Mathf.Lerp(this.Thresholds.MinimumDistanceToTargetM, this.Thresholds.FurthestCameraDistanceGameplay, this.MotionValues.ScrollTickLERPCurveGameplay.Evaluate(num));
			this.MotionValues.DefaultView.LERPCurvePosition = num;
			this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.Homing, this.HalfScreen, null);
			this.mWorldAnchorPoint = this.MotionValues.DefaultView.WorldAnchorPoint;
			this.mDistanceToTarget = this.MotionValues.DefaultView.FinalDistanceToTarget;
			float num2 = this.FindLERPValueForDistanceInLERPCurve(this.MotionValues.ScrollTickLERPCurveSensors, this.Thresholds.MinimumCameraDistanceSensors, this.Thresholds.FurthestCameraDistanceSensors, this.MotionValues.DefaultSensorsView.FinalDistanceToTarget);
			this.MotionValues.DefaultSensorsView.LERPCurvePosition = num2;
			this.MotionValues.DefaultSensorsView.FinalDistanceToTarget = Mathf.Lerp(this.Thresholds.MinimumCameraDistanceSensors, this.Thresholds.FurthestCameraDistanceSensors, this.MotionValues.ScrollTickLERPCurveSensors.Evaluate(num2));
			this.MotionValues.DefaultSensorsView.Orientation.y = this.MotionValues.DefaultView.Orientation.y;
			this.mSensorsViewSettings = new UserCamera.CamFramingSettings(this.MotionValues.DefaultSensorsView);
			this.mSensorsViewSettings.Orientation.x = Mathf.Clamp(this.mSensorsViewSettings.Orientation.x, this.MotionValues.MinPitchVersusDistanceFromTarget.Evaluate(this.mSensorsViewSettings.FinalDistanceToTarget), 87f);
			this.mTransitioningFromSettings = new UserCamera.CamFramingSettings(this.MotionValues.DefaultView);
			this.mGoalViewSettings = new UserCamera.CamFramingSettings(this.MotionValues.DefaultView);
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x000AD4A0 File Offset: 0x000AB6A0
		private void OnSensorsManagerToggleEvent(SensorsManagerEvent ev)
		{
			if (ev.Active)
			{
				this.mCam.cullingMask = this.mSensorsCullingMask;
				this.mCam.clearFlags = CameraClearFlags.Color;
				return;
			}
			this.mCam.cullingMask = this.mDefaultCullingMask;
			this.mCam.clearFlags = this.mOriginalClearFlags;
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x000AD4F5 File Offset: 0x000AB6F5
		private void OnBandBoxStateEvent(BandBoxStateEvent eventReceived)
		{
			this.mBandBoxing = eventReceived.Active;
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x000AD503 File Offset: 0x000AB703
		private void OnInGameMenuShowingEvent(InGameMenuShowingEvent ev)
		{
			this.mInGameMenuShowing = ev.IsShowing;
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x000AD514 File Offset: 0x000AB714
		private bool IsPointOnViewConstraints(Vector3 point)
		{
			point.y = this.ViewConstraints.center.y;
			return this.ViewConstraints.Contains(point);
		}

		// Token: 0x06001D40 RID: 7488 RVA: 0x000AD54C File Offset: 0x000AB74C
		private Vector3 ProjectRayToCameraBounds(Ray ray)
		{
			float num = this.ViewConstraints.center.y - ray.origin.y;
			return ray.origin + ray.direction * Mathf.Abs(num / ray.direction.y);
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x000AD5A5 File Offset: 0x000AB7A5
		private void OnNewQualitySettings(QualitySettingsChangedEvent ev)
		{
			this.ApplyUpdatedQualitySettings(ev.NewSettings);
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x000AD5B3 File Offset: 0x000AB7B3
		private void OnRebindHotkeysEvent(RebindHotkeysEvent ev)
		{
			if (this.mInputSystem != null)
			{
				this.RegisterHotkeys();
			}
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x000AD684 File Offset: 0x000AB884
		private IEnumerator DelayTransitionEvent()
		{
			while (Time.unscaledTime - this.mLastSensorsTransition <= this.MotionValues.SensorsEventDelaySeconds)
			{
				yield return null;
			}
			ShipbreakersMain.ToggleSceneLights(!UserCamera.mSensorsManagerActive);
			ShipbreakersMain.PresentationEventSystem.Post(new SensorsManagerEvent(UserCamera.mSensorsManagerActive));
			this.mTransitionEventDelayCoroutine = null;
			yield break;
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x000AD6A0 File Offset: 0x000AB8A0
		public static Camera UnityCameraForUserCamera(UserCamera cam)
		{
			Camera[] componentsInChildren = cam.gameObject.GetComponentsInChildren<Camera>(true);
			if (componentsInChildren.Length != 1)
			{
				return null;
			}
			return componentsInChildren[0];
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x000AD6C5 File Offset: 0x000AB8C5
		private void ApplyUpdatedQualitySettings(VideoSettings newSettings)
		{
			if (this.mScreenSpaceFog != null)
			{
				this.mScreenSpaceFog.SetEnabled(newSettings.FogEnabled);
			}
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x000AD6E6 File Offset: 0x000AB8E6
		private void RegisterHotkeys()
		{
			this.mInputSystem.HotKeys.Register(new Action<InputEventArgs>(this.OnDefaultCameraKeyPressed), ShipbreakersMain.UserSettings.Controls.HotKeyDefinitions[HotKeyOperation.DefaultCameraPosition]);
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x000AD719 File Offset: 0x000AB919
		private void DeregisterHotkeys()
		{
			this.mInputSystem.HotKeys.Deregister(ShipbreakersMain.UserSettings.Controls.HotKeyDefinitions[HotKeyOperation.DefaultCameraPosition]);
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x000AD744 File Offset: 0x000AB944
		public UserCamera()
		{
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x000AD740 File Offset: 0x000AB940
		// Note: this type is marked as 'beforefieldinit'.
		static UserCamera()
		{
		}

		// Token: 0x040017D1 RID: 6097
		private const float kMaxAltitudeMoveToFramePercent = 0.98f;

		// Token: 0x040017D2 RID: 6098
		private const string kMouseScrollWheelAxisName = "Mouse ScrollWheel";

		// Token: 0x040017D3 RID: 6099
		private const string kPlayMakerEnableCameraDistortion = "EnableCameraDistortion";

		// Token: 0x040017D4 RID: 6100
		private const float kHighestCameraPitch = 87f;

		// Token: 0x040017D5 RID: 6101
		private const float kAltitudePublishingDeltaM = 10f;

		// Token: 0x040017D6 RID: 6102
		private const float kMinimumFollowDistanceToSnapM = 500f;

		// Token: 0x040017D7 RID: 6103
		private const float kMinimumCollisionRadiusM = 1f;

		// Token: 0x040017D8 RID: 6104
		private const float kMinimumDistanceScalarGroundClippingDuringFollow = 0.5f;

		// Token: 0x040017D9 RID: 6105
		private const float kCameraBodyRaiseOnCollisionFudgeMeters = 0.1f;

		// Token: 0x040017DA RID: 6106
		[NonSerialized]
		public bool OverrideAllowedMaxAltitude;

		// Token: 0x040017DB RID: 6107
		private Collider mViewConstraintsCollider;

		// Token: 0x040017DC RID: 6108
		public bool HasInputFocus;

		// Token: 0x040017DD RID: 6109
		public UserCamera.CollisionData CollisionValues = new UserCamera.CollisionData();

		// Token: 0x040017DE RID: 6110
		public UserCamera.MotionData MotionValues = new UserCamera.MotionData();

		// Token: 0x040017DF RID: 6111
		public UserCamera.ThresholdData Thresholds = new UserCamera.ThresholdData();

		// Token: 0x040017E0 RID: 6112
		public Vector3 CamMoveToOffsetForSM = new Vector3(0f, 2400f, -1900f);

		// Token: 0x040017E1 RID: 6113
		public Vector3 CamMoveToOffsetForNotSM = new Vector3(0f, 350f, -200f);

		// Token: 0x040017E2 RID: 6114
		[SerializeField]
		private Collider CamViewBounds;

		// Token: 0x040017E3 RID: 6115
		[SerializeField]
		private TransformNoiseFilterAsset m_DefaultCameraNoise;

		// Token: 0x040017E4 RID: 6116
		[SerializeField]
		private bool m_ZoomOnTargetRefocus;

		// Token: 0x040017E6 RID: 6118
		private float mDesiredDistanceLERPValue = 0.5f;

		// Token: 0x040017E7 RID: 6119
		private float mCurrentDistanceLERPValue = 0.5f;

		// Token: 0x040017E8 RID: 6120
		private float mLastRealTimeSinceStartup;

		// Token: 0x040017E9 RID: 6121
		private float OverridenMaxAltitudeValue;

		// Token: 0x040017EA RID: 6122
		private Vector3 mWorldVelocityMS = Vector3.zero;

		// Token: 0x040017EB RID: 6123
		private Vector2 mOrbitAngularVelocityDegPerSec = Vector2.zero;

		// Token: 0x040017EC RID: 6124
		private Camera mCam;

		// Token: 0x040017ED RID: 6125
		private ScreenSpaceFog mScreenSpaceFog;

		// Token: 0x040017EE RID: 6126
		private bool mColliding;

		// Token: 0x040017EF RID: 6127
		public bool DisableAllCollision;

		// Token: 0x040017F0 RID: 6128
		private UserCamera.CameraMotionState mMotionState;

		// Token: 0x040017F1 RID: 6129
		private float mScreenWidthInMeters;

		// Token: 0x040017F2 RID: 6130
		private float mScreenWidthInMetersUndamped;

		// Token: 0x040017F3 RID: 6131
		private bool mBandBoxing;

		// Token: 0x040017F4 RID: 6132
		private bool mInGameMenuShowing;

		// Token: 0x040017F5 RID: 6133
		private Vector3 mWorldAnchorPoint = Vector3.zero;

		// Token: 0x040017F6 RID: 6134
		private Vector2 mGrabScreenPoint = Vector2.zero;

		// Token: 0x040017F7 RID: 6135
		private Vector3 mDragWorldVelocityMS = Vector3.zero;

		// Token: 0x040017F8 RID: 6136
		private int mOriginalMousePosX;

		// Token: 0x040017F9 RID: 6137
		private int mOriginalMousePosY;

		// Token: 0x040017FA RID: 6138
		private SphereCollider mCollider;

		// Token: 0x040017FB RID: 6139
		private Transform mTransform;

		// Token: 0x040017FC RID: 6140
		private bool mPanOrOrbitCanBegin = true;

		// Token: 0x040017FD RID: 6141
		private bool mAutoPitch = true;

		// Token: 0x040017FE RID: 6142
		private bool mDisableAutoPitchUntilNextZoom;

		// Token: 0x040017FF RID: 6143
		private float mDistanceToTarget;

		// Token: 0x04001800 RID: 6144
		private float mLastPublishedAltitude;

		// Token: 0x04001802 RID: 6146
		private IEnumerator mTransitionEventDelayCoroutine;

		// Token: 0x04001803 RID: 6147
		private float mLastSensorsTransition;

		// Token: 0x04001804 RID: 6148
		private Vector3 mPreviousFrameTargetPoint = Vector3.zero;

		// Token: 0x04001805 RID: 6149
		private Transform m_BaseCamTarget;

		// Token: 0x04001806 RID: 6150
		private Transform m_CamTarget;

		// Token: 0x04001807 RID: 6151
		private static bool mSensorsManagerActive;

		// Token: 0x04001808 RID: 6152
		private bool mIntelMomentActive;

		// Token: 0x04001809 RID: 6153
		private bool mWindowInFocus = true;

		// Token: 0x0400180A RID: 6154
		private UserCamera.CinematicControllerToken mActiveCinematicController;

		// Token: 0x0400180B RID: 6155
		private UserCamera.CamFramingSettings mGameViewSettings = new UserCamera.CamFramingSettings();

		// Token: 0x0400180C RID: 6156
		private UserCamera.CamFramingSettings mSensorsViewSettings = new UserCamera.CamFramingSettings();

		// Token: 0x0400180D RID: 6157
		private UserCamera.CamFramingSettings mGoalViewSettings;

		// Token: 0x0400180E RID: 6158
		private UserCamera.CamFramingSettings mTransitioningFromSettings;

		// Token: 0x0400180F RID: 6159
		private UserCamera.CamFramingSettings mDefaultViewOnInstantiate;

		// Token: 0x04001810 RID: 6160
		private float mSensorsTransitionTime;

		// Token: 0x04001811 RID: 6161
		private CameraClearFlags mOriginalClearFlags = CameraClearFlags.Nothing;

		// Token: 0x04001812 RID: 6162
		private bool mAllowSensorManagerMode = true;

		// Token: 0x04001813 RID: 6163
		private int mDefaultCullingMask;

		// Token: 0x04001814 RID: 6164
		private int mSensorsCullingMask;

		// Token: 0x04001815 RID: 6165
		private CrossFadingNoiseFilter mNoiseFilter;

		// Token: 0x04001816 RID: 6166
		private UnitViewAttributes.CameraFocusSettings mCurrentFocusSettings;

		// Token: 0x04001817 RID: 6167
		private InputSystem mInputSystem;

		// Token: 0x0200035A RID: 858
		private enum CameraMotionState
		{
			// Token: 0x04001819 RID: 6169
			None,
			// Token: 0x0400181A RID: 6170
			Pan,
			// Token: 0x0400181B RID: 6171
			PanScreenSpace,
			// Token: 0x0400181C RID: 6172
			Orbit,
			// Token: 0x0400181D RID: 6173
			Homing,
			// Token: 0x0400181E RID: 6174
			PointTo,
			// Token: 0x0400181F RID: 6175
			PointToAndFollow,
			// Token: 0x04001820 RID: 6176
			Following,
			// Token: 0x04001821 RID: 6177
			MoveToTargetCameraFrame = 9,
			// Token: 0x04001822 RID: 6178
			CinematicCamera
		}

		// Token: 0x0200035B RID: 859
		[Flags]
		public enum CameraMotionTypes
		{
			// Token: 0x04001824 RID: 6180
			None = 0,
			// Token: 0x04001825 RID: 6181
			Orbit = 1,
			// Token: 0x04001826 RID: 6182
			Pitch = 2,
			// Token: 0x04001827 RID: 6183
			Zoom = 4,
			// Token: 0x04001828 RID: 6184
			Pan = 8,
			// Token: 0x04001829 RID: 6185
			FreeRotate = 3,
			// Token: 0x0400182A RID: 6186
			Constraints = 16,
			// Token: 0x0400182B RID: 6187
			ConstrainedPitch = 32,
			// Token: 0x0400182C RID: 6188
			ConstrainedPitchFreeRotate = 35,
			// Token: 0x0400182D RID: 6189
			ConstrainedFreeRotate = 51,
			// Token: 0x0400182E RID: 6190
			ConstrainedPan = 56,
			// Token: 0x0400182F RID: 6191
			ConstraintedZoom = 52,
			// Token: 0x04001830 RID: 6192
			All = -1
		}

		// Token: 0x0200035C RID: 860
		[Serializable]
		public class CamFramingSettings
		{
			// Token: 0x06001D4A RID: 7498 RVA: 0x000AD83D File Offset: 0x000ABA3D
			public CamFramingSettings()
			{
			}

			// Token: 0x06001D4B RID: 7499 RVA: 0x000AD87C File Offset: 0x000ABA7C
			public CamFramingSettings(UserCamera.CamFramingSettings other)
			{
				this.Orientation = new Vector2(other.Orientation.x, other.Orientation.y);
				this.FinalDistanceToTarget = other.FinalDistanceToTarget;
				this.LERPCurvePosition = other.LERPCurvePosition;
				this.WorldAnchorPoint = other.WorldAnchorPoint;
			}

			// Token: 0x06001D4C RID: 7500 RVA: 0x000AD90C File Offset: 0x000ABB0C
			public void SetValues(UserCamera cam)
			{
				this.Orientation = new Vector2(Mathf.Clamp(cam.transform.eulerAngles.x, cam.Thresholds.LowestPitchDeg, 87f), cam.transform.eulerAngles.y);
				this.LERPCurvePosition = cam.mCurrentDistanceLERPValue;
				this.FinalDistanceToTarget = cam.DistanceToTargetMeters;
				this.WorldAnchorPoint = cam.mWorldAnchorPoint;
			}

			// Token: 0x04001831 RID: 6193
			public Vector2 Orientation = new Vector2(87f, 0f);

			// Token: 0x04001832 RID: 6194
			[HideInInspector]
			public float LERPCurvePosition = 0.5f;

			// Token: 0x04001833 RID: 6195
			[HideInInspector]
			public Vector3 WorldAnchorPoint = Vector3.zero;

			// Token: 0x04001834 RID: 6196
			public float FinalDistanceToTarget = 500f;
		}

		// Token: 0x0200035D RID: 861
		public class CinematicControllerToken
		{
			// Token: 0x17000513 RID: 1299
			// (get) Token: 0x06001D4D RID: 7501 RVA: 0x000AD97D File Offset: 0x000ABB7D
			public UserCamera ActiveCamera
			{
				get
				{
					return this.mUserCam;
				}
			}

			// Token: 0x06001D4E RID: 7502 RVA: 0x000AD985 File Offset: 0x000ABB85
			public CinematicControllerToken(UserCamera cinematicCam, Action setupCamera, Action tearDownCamera, UserCamera.CinematicControllerToken.RequestCinematicEnd cinematicCancelDelegate, UserCamera.CameraMotionTypes allowedMotion)
			{
				this.mUserCam = cinematicCam;
				this.mSetupCamera = setupCamera;
				this.mTeardownCamera = tearDownCamera;
				this.mCancelCinematicModeRequestAction = cinematicCancelDelegate;
				this.AllowedCameraMotion = allowedMotion;
			}

			// Token: 0x06001D4F RID: 7503 RVA: 0x000AD9C4 File Offset: 0x000ABBC4
			public bool IsCameraMotionAllowed(UserCamera.CameraMotionTypes motionType)
			{
				return (motionType & this.AllowedCameraMotion) == motionType;
			}

			// Token: 0x06001D50 RID: 7504 RVA: 0x000AD9D4 File Offset: 0x000ABBD4
			public bool StartCinematicSequence()
			{
				if (this.mUserCam.IsCinematicMode && this.mUserCam.mActiveCinematicController == this)
				{
					if (this.mSetupCamera != null)
					{
						this.mSetupCamera();
						this.mUserCam.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.CinematicCamera, this.mUserCam.HalfScreen);
						return true;
					}
					Log.Error(Log.Channel.Graphics, "No camera setup delegate set in cinematic camera token", new object[0]);
				}
				else
				{
					Log.Error(this.mUserCam, Log.Channel.Graphics, "Attempted to start a cinematic sequence before the camera was in the cinematic state! Camera is in state {0}", new object[]
					{
						this.mUserCam.mMotionState
					});
				}
				return false;
			}

			// Token: 0x06001D51 RID: 7505 RVA: 0x000ADA71 File Offset: 0x000ABC71
			public bool CancelCinematicSequence()
			{
				if (this.mCancelCinematicModeRequestAction == null)
				{
					this.EndCinematicSequence();
					return true;
				}
				if (this.mCancelCinematicModeRequestAction())
				{
					this.EndCinematicSequence();
					return true;
				}
				return false;
			}

			// Token: 0x06001D52 RID: 7506 RVA: 0x000ADA99 File Offset: 0x000ABC99
			public void EndCinematicSequence()
			{
				if (this.mTeardownCamera != null)
				{
					this.mTeardownCamera();
				}
				this.mUserCam.ReleaseCinematicToken(this);
			}

			// Token: 0x04001835 RID: 6197
			private UserCamera.CinematicControllerToken.RequestCinematicEnd mCancelCinematicModeRequestAction;

			// Token: 0x04001836 RID: 6198
			private Action mSetupCamera;

			// Token: 0x04001837 RID: 6199
			private Action mTeardownCamera;

			// Token: 0x04001838 RID: 6200
			private UserCamera mUserCam;

			// Token: 0x04001839 RID: 6201
			public UserCamera.CameraMotionTypes AllowedCameraMotion = UserCamera.CameraMotionTypes.All;

			// Token: 0x0400183A RID: 6202
			public Vector3 OriginalPosition = Vector3.zero;

			// Token: 0x0400183B RID: 6203
			public float OriginalDistanceToTarget;

			// Token: 0x0400183C RID: 6204
			public bool OriginallyInSensors;

			// Token: 0x0200035E RID: 862
			// (Invoke) Token: 0x06001D54 RID: 7508
			public delegate bool RequestCinematicEnd();
		}

		// Token: 0x0200035F RID: 863
		[Serializable]
		public class CollisionData
		{
			// Token: 0x06001D57 RID: 7511 RVA: 0x000ADABC File Offset: 0x000ABCBC
			public CollisionData()
			{
			}

			// Token: 0x0400183D RID: 6205
			public LayerMask CollideWith;

			// Token: 0x0400183E RID: 6206
			public AnimationCurve ColliderSizeVersusVelocityMS = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0.5f)
			});
		}

		// Token: 0x02000360 RID: 864
		[Serializable]
		public class MotionDampeningData
		{
			// Token: 0x06001D58 RID: 7512 RVA: 0x000ADAFD File Offset: 0x000ABCFD
			public MotionDampeningData()
			{
			}

			// Token: 0x0400183F RID: 6207
			public float CameraTranslationDampening = 4f;

			// Token: 0x04001840 RID: 6208
			public float CameraOrbitXAngularDampening = 4f;

			// Token: 0x04001841 RID: 6209
			public float CameraOrbitYAngularDampening = 4f;

			// Token: 0x04001842 RID: 6210
			public float CameraDollyLERPEasingThreshold = 0.05f;

			// Token: 0x04001843 RID: 6211
			public float CameraDollyDistanceEasing = 10f;
		}

		// Token: 0x02000361 RID: 865
		[Serializable]
		public class MotionData
		{
			// Token: 0x06001D59 RID: 7513 RVA: 0x000ADB3C File Offset: 0x000ABD3C
			public MotionData()
			{
			}

			// Token: 0x04001844 RID: 6212
			public UserCamera.CamFramingSettings DefaultView = new UserCamera.CamFramingSettings();

			// Token: 0x04001845 RID: 6213
			public UserCamera.CamFramingSettings DefaultSensorsView = new UserCamera.CamFramingSettings();

			// Token: 0x04001846 RID: 6214
			public AnimationCurve DefaultPitchVersusDistance = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 10f),
				new Keyframe(10f, 90f)
			});

			// Token: 0x04001847 RID: 6215
			public AnimationCurve MinPitchVersusDistanceFromTarget = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 10f),
				new Keyframe(10f, 90f)
			});

			// Token: 0x04001848 RID: 6216
			public float ScreenSpacePanVelocityMSPerScreen = 240f;

			// Token: 0x04001849 RID: 6217
			public float AngularVelocityMaxDegPerSec = 360f;

			// Token: 0x0400184A RID: 6218
			public float AngularVelocityAccelerationDegPerSec = 720f;

			// Token: 0x0400184B RID: 6219
			public float PitchCorrectionAngularVelocityDegPerSec = 1.5f;

			// Token: 0x0400184C RID: 6220
			public float DefaultPitchRecoveryAngularVelocityDegPerSec = 45f;

			// Token: 0x0400184D RID: 6221
			public float PitchCorrectionAngularVelocitySensorsDegPerSec = 10f;

			// Token: 0x0400184E RID: 6222
			public float HomingTranslationRateMS = 5f;

			// Token: 0x0400184F RID: 6223
			public AnimationCurve ScrollTickLERPCurveGameplay = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0f),
				new Keyframe(1f, 1f)
			});

			// Token: 0x04001850 RID: 6224
			public int NumScrollTicksGameplay = 20;

			// Token: 0x04001851 RID: 6225
			public AnimationCurve ScrollTickLERPCurveSensors = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0f),
				new Keyframe(1f, 1f)
			});

			// Token: 0x04001852 RID: 6226
			public int NumScrollTicksSensors = 10;

			// Token: 0x04001853 RID: 6227
			public float SensorsTransitionPeriodSeconds = 1f;

			// Token: 0x04001854 RID: 6228
			public float SensorsEventDelaySeconds = 0.1f;

			// Token: 0x04001855 RID: 6229
			public AnimationCurve SensorsTransitionLERPCurve = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0f),
				new Keyframe(1f, 1f)
			});

			// Token: 0x04001856 RID: 6230
			public Vector2 EdgePanningScreenspacePercents = Vector2.zero;

			// Token: 0x04001857 RID: 6231
			public UserCamera.MotionDampeningData DampeningValues = new UserCamera.MotionDampeningData();
		}

		// Token: 0x02000362 RID: 866
		[Serializable]
		public class ThresholdData
		{
			// Token: 0x06001D5A RID: 7514 RVA: 0x000ADD60 File Offset: 0x000ABF60
			public ThresholdData()
			{
			}

			// Token: 0x04001858 RID: 6232
			public float MinimumDistanceToTargetM = 0.2f;

			// Token: 0x04001859 RID: 6233
			public float AutoPitchEngageAngleErrorDegrees = 2f;

			// Token: 0x0400185A RID: 6234
			public float AutoPitchEngageDistance = 500f;

			// Token: 0x0400185B RID: 6235
			public float LowestPitchDeg = 10f;

			// Token: 0x0400185C RID: 6236
			public float MinPointToAngleError = 5f;

			// Token: 0x0400185D RID: 6237
			public float ScreenSpacePanCutOffAngleFromHorizon = 5f;

			// Token: 0x0400185E RID: 6238
			public float FurthestCameraDistanceGameplay = 1500f;

			// Token: 0x0400185F RID: 6239
			public float MinimumCameraDistanceSensors = 5000f;

			// Token: 0x04001860 RID: 6240
			public float FurthestCameraDistanceSensors = 6000f;
		}
	}
}
