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
	public class UserCamera : CameraBase
	{
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

		public override Camera ActiveCamera
		{
			get
			{
				return this.mCam;
			}
		}

		public override float CamAltitudeMeters
		{
			get
			{
				return this.Position.y;
			}
		}

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

		public float DistanceToTargetMeters
		{
			get
			{
				return this.mDistanceToTarget;
			}
		}

		public override float ScreenWidthInMeters
		{
			get
			{
				return this.mScreenWidthInMeters;
			}
		}

		public override Transform CameraTarget
		{
			get
			{
				return this.CamTargetInternal;
			}
		}

		public override float MetersPerPixel
		{
			get
			{
				return this.mScreenWidthInMetersUndamped / (float)Screen.width;
			}
		}

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

		public override float PercentageOfAltitude
		{
			get
			{
				return Mathf.Clamp01(1f - this.CamAltitudeMeters / this.MaxAltitudeM);
			}
		}

		private void StartTransitionEventDelay()
		{
			this.mLastSensorsTransition = Time.unscaledTime;
			if (this.mTransitionEventDelayCoroutine == null)
			{
				this.mTransitionEventDelayCoroutine = this.DelayTransitionEvent();
				base.StartCoroutine(this.mTransitionEventDelayCoroutine);
			}
		}

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

		public bool ResetViewToDefaultOnSensorsTransition
		{
			get
			{
				return ShipbreakersMain.UserSettings.Gameplay.CameraResetOnExitSensors;
			}
		}

		public static bool IsOrbiting
		{
			get
			{
				return UnityEngine.Input.GetMouseButton(1);
			}
		}

		public static bool IsPanning
		{
			get
			{
				return UnityEngine.Input.GetMouseButton(2);
			}
		}

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

		public override event CameraBase.CameraZoomChanged CameraZoomLevelChanged;

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

		private float DesiredDistanceToTarget
		{
			get
			{
				return Mathf.Lerp(this.ClosestCameraDistanceToTarget, this.CurrentMaxDistance, this.CurrentDistanceLERPCurve.Evaluate(this.mDesiredDistanceLERPValue));
			}
		}

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

		public bool IsCinematicMode
		{
			get
			{
				return this.mActiveCinematicController != null;
			}
		}

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

		private bool LockCursorWhenOrbiting
		{
			get
			{
				return ShipbreakersMain.UserSettings.Gameplay.CameraLockCursorOnOrbit;
			}
		}

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

		private event CameraBase.CameraMoved mCameraMovedCallbacks;

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

		public static bool IsSensorsModeActive
		{
			get
			{
				return UserCamera.mSensorsManagerActive;
			}
		}

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

		private float MaxPanVelocityPercentScreenWidthPerSecond
		{
			get
			{
				return this.MotionValues.ScreenSpacePanVelocityMSPerScreen;
			}
		}

		private float MouseXOrbitSensitivity
		{
			get
			{
				return 2.5f * ShipbreakersMain.UserSettings.Gameplay.CameraOrbitVertSpeed;
			}
		}

		private float MouseYOrbitSensitivity
		{
			get
			{
				return 2.5f * ShipbreakersMain.UserSettings.Gameplay.CameraOrbitHorizSpeed;
			}
		}

		private float MouseScrollSpeed
		{
			get
			{
				return 1f;
			}
		}

		private float EdgePanSpeedScalar
		{
			get
			{
				return 2f * ShipbreakersMain.UserSettings.Gameplay.CameraEdgePanSpeed;
			}
		}

		public UserCamera.CamFramingSettings GameViewSettings
		{
			get
			{
				return this.mGameViewSettings;
			}
		}

		public UserCamera.CamFramingSettings SensorsViewSettings
		{
			get
			{
				return this.mSensorsViewSettings;
			}
		}

		public void SetViewSettings(UserCamera.CamFramingSettings gameViewSettings, UserCamera.CamFramingSettings sensorsViewSettings)
		{
			this.mGameViewSettings = gameViewSettings;
			this.mSensorsViewSettings = sensorsViewSettings;
		}

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

		public Vector2 HalfScreen
		{
			get
			{
				return new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
			}
		}

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

		private void OnEnable()
		{
			ShipbreakersMain.PresentationEventSystem.AddHandler<SensorsManagerEvent>(new BBI.Core.Events.EventHandler<SensorsManagerEvent>(this.OnSensorsManagerToggleEvent));
			ShipbreakersMain.PresentationEventSystem.AddHandler<BandBoxStateEvent>(new BBI.Core.Events.EventHandler<BandBoxStateEvent>(this.OnBandBoxStateEvent));
			ShipbreakersMain.PresentationEventSystem.AddHandler<InGameMenuShowingEvent>(new BBI.Core.Events.EventHandler<InGameMenuShowingEvent>(this.OnInGameMenuShowingEvent));
		}

		private void OnDisable()
		{
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<SensorsManagerEvent>(new BBI.Core.Events.EventHandler<SensorsManagerEvent>(this.OnSensorsManagerToggleEvent));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<BandBoxStateEvent>(new BBI.Core.Events.EventHandler<BandBoxStateEvent>(this.OnBandBoxStateEvent));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<InGameMenuShowingEvent>(new BBI.Core.Events.EventHandler<InGameMenuShowingEvent>(this.OnInGameMenuShowingEvent));
		}

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

		public void SetAutoPitch(bool autoPitch)
		{
			this.AutoPitch = autoPitch;
		}

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

		public bool IsFollowingObject(Transform obj)
		{
			return (this.m_BaseCamTarget == obj || this.m_CamTarget == obj) && this.mMotionState == UserCamera.CameraMotionState.Following;
		}

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

		public void PointAtScreenPoint(Vector2 screenPoint)
		{
			if (this.IsCinematicMode)
			{
				return;
			}
			this.ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState.PointTo, screenPoint);
		}

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

		public void TeleportTo(Vector2 gamePosition)
		{
			this.TeleportTo(gamePosition, this.DistanceToTargetMeters);
		}

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

		public bool RaycastFromPointToCamera(Vector3 point, out RaycastHit hit)
		{
			return Physics.Raycast(point, this.Position - point, out hit, this.mCam.farClipPlane * 2f, this.CollisionValues.CollideWith.value);
		}

		public bool SphereCastScreenPointToWorld(Vector2 screenPoint, float distance, out RaycastHit hit)
		{
			return Physics.SphereCast(this.mCam.ScreenPointToRay(screenPoint), this.mCollider.radius, out hit, distance, this.CollisionValues.CollideWith.value);
		}

		public bool SphereCastScreenPointToWorld(Vector2 screenPoint, out RaycastHit hit)
		{
			return this.SphereCastScreenPointToWorld(screenPoint, this.mCam.farClipPlane * 2f, out hit);
		}

		public bool SphereCastIntoWorld(Vector3 fromPoint, float radius, Vector3 direction, float distance, out RaycastHit hit)
		{
			return Physics.CapsuleCast(fromPoint, fromPoint, radius, direction, out hit, distance, this.CollisionValues.CollideWith.value);
		}

		public bool RaycastScreenPointToWorld(Vector2 screenPoint, float distance, out RaycastHit hit)
		{
			return Physics.Raycast(this.mCam.ScreenPointToRay(screenPoint), out hit, distance, this.CollisionValues.CollideWith.value);
		}

		public override bool RaycastScreenPointToWorld(Vector2 screenPoint, out RaycastHit hit)
		{
			return this.RaycastScreenPointToWorld(screenPoint, this.mCam.farClipPlane * 2f, out hit);
		}

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

		public bool RaycastRayToWorld(Ray ray, out RaycastHit hit)
		{
			return Physics.Raycast(ray, out hit, this.mCam.farClipPlane * 2f, this.CollisionValues.CollideWith.value);
		}

		public override void RunCameraQuery(CameraBase.CameraQuery camQuery)
		{
			if (camQuery != null)
			{
				Matrix4x4 mCameraToScreenMatrix = this.mCameraToScreenMatrix;
				Matrix4x4 gameCameraToUIMatrix = Matrix4x4.Scale(new Vector3(HUDSystem.NGUIPixelScale, HUDSystem.NGUIPixelScale, 1f)) * mCameraToScreenMatrix;
				camQuery(this, UICamera.mainCamera, mCameraToScreenMatrix, gameCameraToUIMatrix);
			}
		}

		private Vector2 GUIToScreenPoint(Vector2 guiPoint)
		{
			return new Vector2(guiPoint.x, Mathf.Abs(guiPoint.y - (float)Screen.height));
		}

		private Vector3 GetInstantaneousOrbitVelocityKMS()
		{
			float magnitude = (this.Position - this.CamTargetOrWorldAnchorPoint).magnitude;
			Vector3 a = -this.mTransform.right * this.mOrbitAngularVelocityDegPerSec.y + Vector3.up * this.mOrbitAngularVelocityDegPerSec.x;
			return a * (3.1415927f * magnitude);
		}

		private Vector3 GetWorldVelocityVectorMS()
		{
			return this.mWorldVelocityMS + this.GetInstantaneousOrbitVelocityKMS();
		}

		private Vector3 ConvertWorldDirectionToLocal(Vector3 worldVelocity)
		{
			return Quaternion.AngleAxis(-this.mTransform.eulerAngles.y, Vector3.up) * worldVelocity;
		}

		private Vector3 ConvertLocalDirectionToWorld(Vector3 worldVelocity)
		{
			return Quaternion.AngleAxis(this.mTransform.eulerAngles.y, Vector3.up) * worldVelocity;
		}

		private void ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState toState, Vector2 screenAnchorPoint)
		{
			this.ChangeCameraStateWithMutationApply(toState, screenAnchorPoint, this.CamTargetInternal);
		}

		private void ChangeCameraStateWithMutationApply(UserCamera.CameraMotionState toState, Vector2 screenAnchorPoint, Transform target)
		{
			Vector3 position = this.Position;
			Quaternion rotation = this.Rotation;
			this.ChangeCameraState(toState, screenAnchorPoint, target, ref position, ref rotation);
			this.Position = position;
			this.Rotation = rotation;
		}

		private void ChangeCameraState(UserCamera.CameraMotionState toState, Vector2 screenAnchorPoint, ref Vector3 newPosition, ref Quaternion newOrientation)
		{
			this.ChangeCameraState(toState, screenAnchorPoint, this.CamTargetInternal, ref newPosition, ref newOrientation);
		}

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

		private void MutateZoomVelocity(float zoomAmount)
		{
			this.MutateZoomVelocity(zoomAmount, false);
		}

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

		private float CalculateLowestPitchForDistance(float distanceKm)
		{
			float result = Mathf.Clamp(this.MotionValues.MinPitchVersusDistanceFromTarget.Evaluate(distanceKm), this.Thresholds.LowestPitchDeg, 87f);
			if (this.mAutoPitch && !this.mDisableAutoPitchUntilNextZoom)
			{
				result = Mathf.Clamp(this.MotionValues.DefaultPitchVersusDistance.Evaluate(distanceKm), this.Thresholds.LowestPitchDeg, 87f);
			}
			return result;
		}

		private void UpdateViewWidthInM(float deltaTime)
		{
			this.mScreenWidthInMetersUndamped = 2f * this.DistanceToTargetMeters * Mathf.Tan(0.017453292f * this.ActiveCamera.fieldOfView);
			if (Mathf.Abs((this.mScreenWidthInMetersUndamped - this.mScreenWidthInMeters) / this.mScreenWidthInMetersUndamped) > 0.02f)
			{
				this.mScreenWidthInMeters = Mathf.Lerp(this.mScreenWidthInMeters, this.mScreenWidthInMetersUndamped, 25f * deltaTime);
			}
		}

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

		private void OnDefaultCameraKeyPressed(InputEventArgs args)
		{
			this.SteppedHomeCamera();
		}

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

		private float FindLERPValueForDistanceInCurrentLERPCurve(float distance)
		{
			return this.FindLERPValueForDistanceInLERPCurve(this.CurrentDistanceLERPCurve, this.ClosestCameraDistanceToTarget, this.CurrentMaxDistance, distance);
		}

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

		private void OnBandBoxStateEvent(BandBoxStateEvent eventReceived)
		{
			this.mBandBoxing = eventReceived.Active;
		}

		private void OnInGameMenuShowingEvent(InGameMenuShowingEvent ev)
		{
			this.mInGameMenuShowing = ev.IsShowing;
		}

		private bool IsPointOnViewConstraints(Vector3 point)
		{
			point.y = this.ViewConstraints.center.y;
			return this.ViewConstraints.Contains(point);
		}

		private Vector3 ProjectRayToCameraBounds(Ray ray)
		{
			float num = this.ViewConstraints.center.y - ray.origin.y;
			return ray.origin + ray.direction * Mathf.Abs(num / ray.direction.y);
		}

		private void OnNewQualitySettings(QualitySettingsChangedEvent ev)
		{
			this.ApplyUpdatedQualitySettings(ev.NewSettings);
		}

		private void OnRebindHotkeysEvent(RebindHotkeysEvent ev)
		{
			if (this.mInputSystem != null)
			{
				this.RegisterHotkeys();
			}
		}

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

		public static Camera UnityCameraForUserCamera(UserCamera cam)
		{
			Camera[] componentsInChildren = cam.gameObject.GetComponentsInChildren<Camera>(true);
			if (componentsInChildren.Length != 1)
			{
				return null;
			}
			return componentsInChildren[0];
		}

		private void ApplyUpdatedQualitySettings(VideoSettings newSettings)
		{
			if (this.mScreenSpaceFog != null)
			{
				this.mScreenSpaceFog.SetEnabled(newSettings.FogEnabled);
			}
		}

		private void RegisterHotkeys()
		{
			this.mInputSystem.HotKeys.Register(new Action<InputEventArgs>(this.OnDefaultCameraKeyPressed), ShipbreakersMain.UserSettings.Controls.HotKeyDefinitions[HotKeyOperation.DefaultCameraPosition]);
		}

		private void DeregisterHotkeys()
		{
			this.mInputSystem.HotKeys.Deregister(ShipbreakersMain.UserSettings.Controls.HotKeyDefinitions[HotKeyOperation.DefaultCameraPosition]);
		}

		public UserCamera()
		{
		}

		// Note: this type is marked as 'beforefieldinit'.
		static UserCamera()
		{
		}

		private const float kMaxAltitudeMoveToFramePercent = 0.98f;

		private const string kMouseScrollWheelAxisName = "Mouse ScrollWheel";

		private const string kPlayMakerEnableCameraDistortion = "EnableCameraDistortion";

		private const float kHighestCameraPitch = 87f;

		private const float kAltitudePublishingDeltaM = 10f;

		private const float kMinimumFollowDistanceToSnapM = 500f;

		private const float kMinimumCollisionRadiusM = 1f;

		private const float kMinimumDistanceScalarGroundClippingDuringFollow = 0.5f;

		private const float kCameraBodyRaiseOnCollisionFudgeMeters = 0.1f;

		[NonSerialized]
		public bool OverrideAllowedMaxAltitude;

		private Collider mViewConstraintsCollider;

		public bool HasInputFocus;

		public UserCamera.CollisionData CollisionValues = new UserCamera.CollisionData();

		public UserCamera.MotionData MotionValues = new UserCamera.MotionData();

		public UserCamera.ThresholdData Thresholds = new UserCamera.ThresholdData();

		public Vector3 CamMoveToOffsetForSM = new Vector3(0f, 2400f, -1900f);

		public Vector3 CamMoveToOffsetForNotSM = new Vector3(0f, 350f, -200f);

		[SerializeField]
		private Collider CamViewBounds;

		[SerializeField]
		private TransformNoiseFilterAsset m_DefaultCameraNoise;

		[SerializeField]
		private bool m_ZoomOnTargetRefocus;

		private float mDesiredDistanceLERPValue = 0.5f;

		private float mCurrentDistanceLERPValue = 0.5f;

		private float mLastRealTimeSinceStartup;

		private float OverridenMaxAltitudeValue;

		private Vector3 mWorldVelocityMS = Vector3.zero;

		private Vector2 mOrbitAngularVelocityDegPerSec = Vector2.zero;

		private Camera mCam;

		private ScreenSpaceFog mScreenSpaceFog;

		private bool mColliding;

		public bool DisableAllCollision;

		private UserCamera.CameraMotionState mMotionState;

		private float mScreenWidthInMeters;

		private float mScreenWidthInMetersUndamped;

		private bool mBandBoxing;

		private bool mInGameMenuShowing;

		private Vector3 mWorldAnchorPoint = Vector3.zero;

		private Vector2 mGrabScreenPoint = Vector2.zero;

		private Vector3 mDragWorldVelocityMS = Vector3.zero;

		private int mOriginalMousePosX;

		private int mOriginalMousePosY;

		private SphereCollider mCollider;

		private Transform mTransform;

		private bool mPanOrOrbitCanBegin = true;

		private bool mAutoPitch = true;

		private bool mDisableAutoPitchUntilNextZoom;

		private float mDistanceToTarget;

		private float mLastPublishedAltitude;

		private IEnumerator mTransitionEventDelayCoroutine;

		private float mLastSensorsTransition;

		private Vector3 mPreviousFrameTargetPoint = Vector3.zero;

		private Transform m_BaseCamTarget;

		private Transform m_CamTarget;

		private static bool mSensorsManagerActive;

		private bool mIntelMomentActive;

		private bool mWindowInFocus = true;

		private UserCamera.CinematicControllerToken mActiveCinematicController;

		private UserCamera.CamFramingSettings mGameViewSettings = new UserCamera.CamFramingSettings();

		private UserCamera.CamFramingSettings mSensorsViewSettings = new UserCamera.CamFramingSettings();

		private UserCamera.CamFramingSettings mGoalViewSettings;

		private UserCamera.CamFramingSettings mTransitioningFromSettings;

		private UserCamera.CamFramingSettings mDefaultViewOnInstantiate;

		private float mSensorsTransitionTime;

		private CameraClearFlags mOriginalClearFlags = CameraClearFlags.Nothing;

		private bool mAllowSensorManagerMode = true;

		private int mDefaultCullingMask;

		private int mSensorsCullingMask;

		private CrossFadingNoiseFilter mNoiseFilter;

		private UnitViewAttributes.CameraFocusSettings mCurrentFocusSettings;

		private InputSystem mInputSystem;

		private enum CameraMotionState
		{
			None,
			Pan,
			PanScreenSpace,
			Orbit,
			Homing,
			PointTo,
			PointToAndFollow,
			Following,
			MoveToTargetCameraFrame = 9,
			CinematicCamera
		}

		[Flags]
		public enum CameraMotionTypes
		{
			None = 0,
			Orbit = 1,
			Pitch = 2,
			Zoom = 4,
			Pan = 8,
			FreeRotate = 3,
			Constraints = 16,
			ConstrainedPitch = 32,
			ConstrainedPitchFreeRotate = 35,
			ConstrainedFreeRotate = 51,
			ConstrainedPan = 56,
			ConstraintedZoom = 52,
			All = -1
		}

		[Serializable]
		public class CamFramingSettings
		{
			public CamFramingSettings()
			{
			}

			public CamFramingSettings(UserCamera.CamFramingSettings other)
			{
				this.Orientation = new Vector2(other.Orientation.x, other.Orientation.y);
				this.FinalDistanceToTarget = other.FinalDistanceToTarget;
				this.LERPCurvePosition = other.LERPCurvePosition;
				this.WorldAnchorPoint = other.WorldAnchorPoint;
			}

			public void SetValues(UserCamera cam)
			{
				this.Orientation = new Vector2(Mathf.Clamp(cam.transform.eulerAngles.x, cam.Thresholds.LowestPitchDeg, 87f), cam.transform.eulerAngles.y);
				this.LERPCurvePosition = cam.mCurrentDistanceLERPValue;
				this.FinalDistanceToTarget = cam.DistanceToTargetMeters;
				this.WorldAnchorPoint = cam.mWorldAnchorPoint;
			}

			public Vector2 Orientation = new Vector2(87f, 0f);

			[HideInInspector]
			public float LERPCurvePosition = 0.5f;

			[HideInInspector]
			public Vector3 WorldAnchorPoint = Vector3.zero;

			public float FinalDistanceToTarget = 500f;
		}

		public class CinematicControllerToken
		{
			public UserCamera ActiveCamera
			{
				get
				{
					return this.mUserCam;
				}
			}

			public CinematicControllerToken(UserCamera cinematicCam, Action setupCamera, Action tearDownCamera, UserCamera.CinematicControllerToken.RequestCinematicEnd cinematicCancelDelegate, UserCamera.CameraMotionTypes allowedMotion)
			{
				this.mUserCam = cinematicCam;
				this.mSetupCamera = setupCamera;
				this.mTeardownCamera = tearDownCamera;
				this.mCancelCinematicModeRequestAction = cinematicCancelDelegate;
				this.AllowedCameraMotion = allowedMotion;
			}

			public bool IsCameraMotionAllowed(UserCamera.CameraMotionTypes motionType)
			{
				return (motionType & this.AllowedCameraMotion) == motionType;
			}

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

			public void EndCinematicSequence()
			{
				if (this.mTeardownCamera != null)
				{
					this.mTeardownCamera();
				}
				this.mUserCam.ReleaseCinematicToken(this);
			}

			private UserCamera.CinematicControllerToken.RequestCinematicEnd mCancelCinematicModeRequestAction;

			private Action mSetupCamera;

			private Action mTeardownCamera;

			private UserCamera mUserCam;

			public UserCamera.CameraMotionTypes AllowedCameraMotion = UserCamera.CameraMotionTypes.All;

			public Vector3 OriginalPosition = Vector3.zero;

			public float OriginalDistanceToTarget;

			public bool OriginallyInSensors;

			public delegate bool RequestCinematicEnd();
		}

		[Serializable]
		public class CollisionData
		{
			public CollisionData()
			{
			}

			public LayerMask CollideWith;

			public AnimationCurve ColliderSizeVersusVelocityMS = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0.5f)
			});
		}

		[Serializable]
		public class MotionDampeningData
		{
			public MotionDampeningData()
			{
			}

			public float CameraTranslationDampening = 4f;

			public float CameraOrbitXAngularDampening = 4f;

			public float CameraOrbitYAngularDampening = 4f;

			public float CameraDollyLERPEasingThreshold = 0.05f;

			public float CameraDollyDistanceEasing = 10f;
		}

		[Serializable]
		public class MotionData
		{
			public MotionData()
			{
			}

			public UserCamera.CamFramingSettings DefaultView = new UserCamera.CamFramingSettings();

			public UserCamera.CamFramingSettings DefaultSensorsView = new UserCamera.CamFramingSettings();

			public AnimationCurve DefaultPitchVersusDistance = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 10f),
				new Keyframe(10f, 90f)
			});

			public AnimationCurve MinPitchVersusDistanceFromTarget = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 10f),
				new Keyframe(10f, 90f)
			});

			public float ScreenSpacePanVelocityMSPerScreen = 240f;

			public float AngularVelocityMaxDegPerSec = 360f;

			public float AngularVelocityAccelerationDegPerSec = 720f;

			public float PitchCorrectionAngularVelocityDegPerSec = 1.5f;

			public float DefaultPitchRecoveryAngularVelocityDegPerSec = 45f;

			public float PitchCorrectionAngularVelocitySensorsDegPerSec = 10f;

			public float HomingTranslationRateMS = 5f;

			public AnimationCurve ScrollTickLERPCurveGameplay = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0f),
				new Keyframe(1f, 1f)
			});

			public int NumScrollTicksGameplay = 20;

			public AnimationCurve ScrollTickLERPCurveSensors = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0f),
				new Keyframe(1f, 1f)
			});

			public int NumScrollTicksSensors = 10;

			public float SensorsTransitionPeriodSeconds = 1f;

			public float SensorsEventDelaySeconds = 0.1f;

			public AnimationCurve SensorsTransitionLERPCurve = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0f),
				new Keyframe(1f, 1f)
			});

			public Vector2 EdgePanningScreenspacePercents = Vector2.zero;

			public UserCamera.MotionDampeningData DampeningValues = new UserCamera.MotionDampeningData();
		}

		[Serializable]
		public class ThresholdData
		{
			public ThresholdData()
			{
			}

			public float MinimumDistanceToTargetM = 0.2f;

			public float AutoPitchEngageAngleErrorDegrees = 2f;

			public float AutoPitchEngageDistance = 500f;

			public float LowestPitchDeg = 10f;

			public float MinPointToAngleError = 5f;

			public float ScreenSpacePanCutOffAngleFromHorizon = 5f;

			public float FurthestCameraDistanceGameplay = 1500f;

			public float MinimumCameraDistanceSensors = 5000f;

			public float FurthestCameraDistanceSensors = 6000f;
		}
	}
}
