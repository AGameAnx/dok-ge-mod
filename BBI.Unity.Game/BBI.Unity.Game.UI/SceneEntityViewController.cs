using System;
using System.Collections.Generic;
using BBI.Core;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Events;
using BBI.Core.Utility;
using BBI.Game.Events;
using BBI.Game.SaveLoad;
using BBI.Game.Simulation;
using BBI.Unity.Game.Events;
using BBI.Unity.Game.HUD;
using BBI.Unity.Game.World;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	public class SceneEntityViewController : BlackbirdPanelBase
	{
		protected override void Update()
		{
			if (!base.IsSessionActive)
			{
				return;
			}
			base.Update();
			this.UpdateTriggerVolumes();
		}

		private void OnMapObjectInteractionEvent(MapObjectInteractionEvent ev)
		{
			if (ev.MapObject is CollectibleEntityView)
			{
				if (this.mRelicViewController != null)
				{
					Entity entity = ((CollectibleEntityView)ev.MapObject).Entity;
					RelicState relicState = ShipbreakersMain.CurrentSimFrame.FindObject<RelicState>(entity);
					if (relicState != null)
					{
						this.mRelicViewController.OnMapObjectInteractionEvent(ev);
						return;
					}
				}
			}
			else if (ev.MapObject is ResourceView && this.mResourceViewController != null)
			{
				this.mResourceViewController.OnMapObjectInteractionEvent(ev);
			}
		}

		private void OnToggleTriggerVolumeEvent(ToggleTriggerVolumeEvent ev)
		{
			foreach (GameObject gameObject in ev.TriggerVolumes)
			{
				if (gameObject != null)
				{
					TriggerCircleEntity[] componentsInChildren = gameObject.GetComponentsInChildren<TriggerCircleEntity>(true);
					if (componentsInChildren != null)
					{
						foreach (TriggerCircleEntity triggerCircleEntity in componentsInChildren)
						{
							this.ToggleTriggerCircleEntity(triggerCircleEntity, ev.Enabled);
						}
					}
				}
				else
				{
					Log.Error(Log.Channel.Scripting, "None-existing (NULL) gameObject supplied to ToggleTriggerVolumeEvent handler in {0}.", new object[]
					{
						base.GetType()
					});
				}
			}
		}

		private void OnSceneEntityCreated(SceneEntityCreatedEvent ev)
		{
			Entity entity = ev.Entity;
			SceneEntityDescriptor sceneEntityDescriptor = ev.SceneEntityDescriptor;
			SceneEntityBase sceneEntityBase = sceneEntityDescriptor.SceneObject as SceneEntityBase;
			sceneEntityBase.EntityID = entity;
			foreach (MonoBehaviour monoBehaviour in sceneEntityBase.GetComponents<MonoBehaviour>())
			{
				ISceneEntityCreationListener sceneEntityCreationListener = monoBehaviour as ISceneEntityCreationListener;
				if (sceneEntityCreationListener != null)
				{
					sceneEntityCreationListener.OnSceneEntityCreated(ev);
				}
			}
			switch (sceneEntityDescriptor.EntityType)
			{
			case SceneEntityType.Base:
			case SceneEntityType.Unit:
			case SceneEntityType.ResourcePoint:
			case SceneEntityType.AIHint:
				break;
			case SceneEntityType.TriggerCircle:
				this.CreateTriggerObjectiveEntityView((TriggerCircleDescriptor)sceneEntityDescriptor);
				return;
			case SceneEntityType.Relic:
				if (this.mRelicViewController != null)
				{
					this.mRelicViewController.OnSceneEntityCreated(ev);
					return;
				}
				break;
			case SceneEntityType.ExtractionZone:
				if (this.mExtractionZoneViewController != null)
				{
					this.mExtractionZoneViewController.OnSceneEntityCreated(ev);
					return;
				}
				break;
			default:
				Log.Warn(Log.Channel.Data | Log.Channel.UI, "Unhandled SceneEntityType {0}", new object[]
				{
					sceneEntityDescriptor.EntityType
				});
				break;
			}
		}

		private void OnHideSceneObjectEvent(HideSceneObjectEvent ev)
		{
			if (ev.SceneEntityDescriptor != null)
			{
				SceneEntityBase sceneEntityBase = ev.SceneEntityDescriptor.SceneObject as SceneEntityBase;
				if (sceneEntityBase != null)
				{
					sceneEntityBase.SetEnable(false, true);
				}
			}
		}

		private void OnUIHighlightTriggerVolume(UIHighlightTriggerVolumeEvent ev)
		{
			foreach (GameObject gameObject in ev.Objects)
			{
				TriggerCircleEntity[] componentsInChildren = gameObject.GetComponentsInChildren<TriggerCircleEntity>(true);
				if (componentsInChildren != null)
				{
					foreach (TriggerCircleEntity triggerCircleEntity in componentsInChildren)
					{
						UISprite uisprite = null;
						this.mTriggerObjectiveCircles.TryGetValue(triggerCircleEntity, out uisprite);
						TweenColor[] colorTweens = null;
						TweenScale[] scaleTweens = null;
						if (uisprite != null)
						{
							colorTweens = uisprite.GetComponentsInParent<TweenColor>(true);
							scaleTweens = uisprite.GetComponentsInParent<TweenScale>(true);
						}
						else
						{
							Log.Error(Log.Channel.UI, "Failed to find objective circle UISprite component for trigger circle scene entity {0} in {1}! Occurred in OnUIHighlightTriggerVolume event handler.", new object[]
							{
								triggerCircleEntity,
								base.GetType()
							});
						}
						Promise<NGUIIconController> promise;
						this.mTriggerObjectiveArrows.TryGetValue(triggerCircleEntity, out promise);
						NGUIIconController nguiiconController = (promise != null) ? (promise.Current as NGUIIconController) : null;
						TweenColor[] colorTweens2 = null;
						TweenScale[] scaleTweens2 = null;
						if (nguiiconController != null)
						{
							colorTweens2 = nguiiconController.GetComponentsInParent<TweenColor>(true);
							scaleTweens2 = nguiiconController.GetComponentsInParent<TweenScale>(true);
						}
						else
						{
							Log.Error(Log.Channel.UI, "Failed to find objective arrow NGUIIconController component for trigger circle scene entity {0} in {1}! Occurred in OnUIHighlightTriggerVolume event handler.", new object[]
							{
								triggerCircleEntity,
								base.GetType()
							});
						}
						this.UpdateColorTween(ev, colorTweens);
						this.UpdateColorTween(ev, colorTweens2);
						this.UpdateScaleTween(ev, scaleTweens);
						this.UpdateScaleTween(ev, scaleTweens2);
					}
				}
			}
		}

		private void CreateTriggerObjectiveEntityView(TriggerCircleDescriptor descriptor)
		{
			if (descriptor.MarkerRadius > 0)
			{
				GameObject gameObject = null;
				NGUIIconController nguiiconController = null;
				switch (descriptor.Type)
				{
				case TriggerCircleDescriptor.PrefabType.Regular:
					gameObject = this.m_Settings.ObjectiveCircleMarkerPrefab;
					nguiiconController = this.m_Settings.ObjectiveArrowMarkerPrefab;
					break;
				case TriggerCircleDescriptor.PrefabType.Threat:
					gameObject = this.m_Settings.ObjectiveThreatCircleMarkerPrefab;
					nguiiconController = this.m_Settings.ObjectiveThreatArrowMarkerPrefab;
					break;
				default:
					Log.Error(Log.Channel.UI, "Used a TriggerCircleDescriptor.PrefabType in ScenceEntityViewController that has no prefabs setup! Enum Id: {0} ", new object[]
					{
						descriptor.Type
					});
					break;
				}
				TriggerCircleEntity triggerCircleEntity = (TriggerCircleEntity)descriptor.SceneObject;
				if (gameObject != null)
				{
					Vector3 vector = triggerCircleEntity.transform.position;
					if (descriptor.ProjectRingToGround)
					{
						vector = UnitView.RaycastToGroundFromPoint(vector);
					}
					this.CreateTriggerObjectiveCircle(triggerCircleEntity, gameObject, vector, descriptor.MarkerRadius);
				}
				else
				{
					Log.Error(Log.Channel.UI, "Failed to create objective circle for trigger circle scene entity {0}! Objective circle icon prefab not found!", new object[]
					{
						triggerCircleEntity
					});
				}
				if (nguiiconController != null)
				{
					this.CreateTriggerObjectiveArrow(triggerCircleEntity, nguiiconController, this.m_Settings.ObjectiveMarkerAltitudeOffset + descriptor.ArrowAltitudeOffset, descriptor.MarkerLabel, descriptor.StartEnabled);
				}
				else
				{
					Log.Error(Log.Channel.UI, "Failed to create objective arrow for trigger circle scene entity {0}! Objective arrow icon prefab not found!", new object[]
					{
						triggerCircleEntity
					});
				}
				if (!triggerCircleEntity.gameObject.activeInHierarchy)
				{
					this.ToggleTriggerCircleEntity(triggerCircleEntity, false);
				}
			}
		}

		private void CreateTriggerObjectiveCircle(TriggerCircleEntity sceneObject, GameObject iconPrefab, Vector3 targetPos, int radius)
		{
			if (!(iconPrefab != null) || !(this.m_Settings.ObjectiveIconContainer != null))
			{
				Log.Error(Log.Channel.UI, "Objective circle marker prefab is null for {0}!", new object[]
				{
					sceneObject
				});
				return;
			}
			GameObject gameObject = NGUITools.AddChild(this.m_Settings.ObjectiveIconContainer, iconPrefab);
			UISprite component = gameObject.GetComponent<UISprite>();
			if (component != null)
			{
				int num = radius * 2;
				component.height = num;
				component.width = num;
				component.cachedTransform.eulerAngles = new Vector3(90f, this.mHUDSystem.ActiveUserCamera.transform.eulerAngles.y, 0f);
				component.cachedTransform.position = targetPos;
				if (sceneObject.HiddenIn3D || !sceneObject.gameObject.activeInHierarchy)
				{
					component.enabled = false;
				}
				this.mTriggerObjectiveCircles.Add(sceneObject, component);
				return;
			}
			Log.Error(Log.Channel.UI, "Couldn't instantiate the Marker Circle {0} for Triggervolume {1}", new object[]
			{
				iconPrefab,
				sceneObject
			});
		}

		private void CreateTriggerObjectiveArrow(TriggerCircleEntity sceneObject, NGUIIconController iconPrefab, float altitudeOffset, string label, bool visibleAtStart)
		{
			if (iconPrefab != null)
			{
				HUDLayer layerWithName = this.mHUDSystem.GetLayerWithName("Feedback");
				Promise<NGUIIconController> promise = this.mHUDSystem.SpawnIconPrefab(iconPrefab, base.gameObject, null, layerWithName);
				this.mTriggerObjectiveArrows.Add(sceneObject, promise);
				promise.OnDone(delegate(NGUIIconController objectiveArrow)
				{
					objectiveArrow.ObjectToTrack = sceneObject.transform;
					objectiveArrow.IconYOffsetWorldSpace = altitudeOffset;
					objectiveArrow.transform.eulerAngles = new Vector3(0f, 0f, 0f);
					objectiveArrow.Visible = visibleAtStart;
					objectiveArrow.SetValueString("ObjectiveLabel", label);
					if (sceneObject.HiddenIn3D || !sceneObject.gameObject.activeInHierarchy)
					{
						objectiveArrow.Visible = false;
					}
					if (string.IsNullOrEmpty(objectiveArrow.GetStringValue("ObjectiveLabel")))
					{
						objectiveArrow.SetShowing("ObjectiveLabel", false);
					}
				}).OnReject(delegate(string reason)
				{
					this.mTriggerObjectiveArrows.Remove(sceneObject);
				});
				return;
			}
			Log.Error(Log.Channel.UI, "Objective Arrow marker prefab is null for {0}!", new object[]
			{
				sceneObject
			});
		}

		private void ToggleTriggerCircleEntity(TriggerCircleEntity triggerCircleEntity, bool toggleValue)
		{
			UISprite uisprite;
			if (this.mTriggerObjectiveCircles.TryGetValue(triggerCircleEntity, out uisprite) && uisprite != null)
			{
				if (toggleValue)
				{
					if (triggerCircleEntity.CustomVisibility != TriggerCircleDescriptor.CustomVisibility.None)
					{
						if (UserCamera.IsSensorsModeActive)
						{
							uisprite.enabled = ((triggerCircleEntity.CustomVisibility & TriggerCircleDescriptor.CustomVisibility.ShowCircleInSM) == TriggerCircleDescriptor.CustomVisibility.ShowCircleInSM);
						}
						else
						{
							uisprite.enabled = ((triggerCircleEntity.CustomVisibility & TriggerCircleDescriptor.CustomVisibility.ShowCircleIn3D) == TriggerCircleDescriptor.CustomVisibility.ShowCircleIn3D);
						}
					}
					else if (UserCamera.IsSensorsModeActive || !triggerCircleEntity.HiddenIn3D)
					{
						uisprite.enabled = true;
					}
					else
					{
						uisprite.enabled = false;
					}
				}
				else
				{
					uisprite.enabled = false;
				}
			}
			Promise<NGUIIconController> promise;
			if (this.mTriggerObjectiveArrows.TryGetValue(triggerCircleEntity, out promise))
			{
				NGUIIconController nguiiconController = promise.Current as NGUIIconController;
				if (nguiiconController != null)
				{
					if (toggleValue)
					{
						if (triggerCircleEntity.CustomVisibility != TriggerCircleDescriptor.CustomVisibility.None)
						{
							if (UserCamera.IsSensorsModeActive)
							{
								nguiiconController.Visible = ((triggerCircleEntity.CustomVisibility & TriggerCircleDescriptor.CustomVisibility.ShowArrowInSM) == TriggerCircleDescriptor.CustomVisibility.ShowArrowInSM);
								nguiiconController.SetShowing("ObjectiveLabel", (triggerCircleEntity.CustomVisibility & TriggerCircleDescriptor.CustomVisibility.ShowTextInSM) == TriggerCircleDescriptor.CustomVisibility.ShowTextInSM);
								return;
							}
							nguiiconController.Visible = ((triggerCircleEntity.CustomVisibility & TriggerCircleDescriptor.CustomVisibility.ShowArrowIn3D) == TriggerCircleDescriptor.CustomVisibility.ShowArrowIn3D);
							nguiiconController.SetShowing("ObjectiveLabel", (triggerCircleEntity.CustomVisibility & TriggerCircleDescriptor.CustomVisibility.ShowTextIn3D) == TriggerCircleDescriptor.CustomVisibility.ShowTextIn3D);
							return;
						}
						else
						{
							if (UserCamera.IsSensorsModeActive || !triggerCircleEntity.HiddenIn3D)
							{
								nguiiconController.Visible = true;
								nguiiconController.SetShowing("ObjectiveLabel", true);
								return;
							}
							nguiiconController.Visible = false;
							nguiiconController.SetShowing("ObjectiveLabel", false);
							return;
						}
					}
					else
					{
						nguiiconController.Visible = false;
						nguiiconController.SetShowing("ObjectiveLabel", false);
					}
				}
			}
		}

		private void UpdateColorTween(UIHighlightTriggerVolumeEvent ev, TweenColor[] colorTweens)
		{
			foreach (TweenColor tweenColor in colorTweens)
			{
				if (ev.Enabled && ev.UseColourEffect)
				{
					tweenColor.enabled = true;
					if (ev.UseCustomColour)
					{
						tweenColor.to = ev.CustomColour;
					}
				}
				else
				{
					tweenColor.PlayForward();
					tweenColor.ResetToBeginning();
					tweenColor.enabled = false;
				}
			}
		}

		private void UpdateScaleTween(UIHighlightTriggerVolumeEvent ev, TweenScale[] scaleTweens)
		{
			foreach (TweenScale tweenScale in scaleTweens)
			{
				if (ev.Enabled && ev.UseScaleEffect)
				{
					tweenScale.enabled = true;
					if (ev.UseCustomScaleFactor)
					{
						tweenScale.to = ev.CustomScaleFactor;
					}
				}
				else
				{
					tweenScale.PlayForward();
					tweenScale.ResetToBeginning();
					tweenScale.enabled = false;
				}
			}
		}

		private void UpdateTriggerVolumes()
		{
			if (this.mHUDSystem != null)
			{
				float y = this.mHUDSystem.ActiveUserCamera.transform.eulerAngles.y;
				foreach (KeyValuePair<TriggerCircleEntity, UISprite> keyValuePair in this.mTriggerObjectiveCircles)
				{
					TriggerCircleEntity key = keyValuePair.Key;
					UISprite value = keyValuePair.Value;
					Promise<NGUIIconController> promise;
					if (this.mTriggerObjectiveArrows.TryGetValue(key, out promise))
					{
						NGUIIconController nguiiconController = promise.Current as NGUIIconController;
						bool visible = false;
						bool state = false;
						bool enabled = false;
						if (key.gameObject.activeInHierarchy)
						{
							visible = true;
							state = (nguiiconController != null && !string.IsNullOrEmpty(nguiiconController.GetStringValue("ObjectiveLabel")));
							if (UserCamera.IsSensorsModeActive || !key.HiddenIn3D)
							{
								enabled = true;
							}
							if (key.CustomVisibility != TriggerCircleDescriptor.CustomVisibility.None)
							{
								if (UserCamera.IsSensorsModeActive)
								{
									visible = ((key.CustomVisibility & TriggerCircleDescriptor.CustomVisibility.ShowArrowInSM) == TriggerCircleDescriptor.CustomVisibility.ShowArrowInSM);
									state = ((key.CustomVisibility & TriggerCircleDescriptor.CustomVisibility.ShowTextInSM) == TriggerCircleDescriptor.CustomVisibility.ShowTextInSM);
									enabled = ((key.CustomVisibility & TriggerCircleDescriptor.CustomVisibility.ShowCircleInSM) == TriggerCircleDescriptor.CustomVisibility.ShowCircleInSM);
								}
								else
								{
									visible = ((key.CustomVisibility & TriggerCircleDescriptor.CustomVisibility.ShowArrowIn3D) == TriggerCircleDescriptor.CustomVisibility.ShowArrowIn3D);
									state = ((key.CustomVisibility & TriggerCircleDescriptor.CustomVisibility.ShowTextIn3D) == TriggerCircleDescriptor.CustomVisibility.ShowTextIn3D);
									enabled = ((key.CustomVisibility & TriggerCircleDescriptor.CustomVisibility.ShowCircleIn3D) == TriggerCircleDescriptor.CustomVisibility.ShowCircleIn3D);
								}
							}
						}
						if (nguiiconController != null)
						{
							nguiiconController.Visible = visible;
							nguiiconController.SetShowing("ObjectiveLabel", state);
						}
						value.enabled = enabled;
						value.cachedTransform.eulerAngles = new Vector3(90f, y, 0f);
						if (key.ProjectRingToGround)
						{
							Vector3 position = key.transform.position;
							Vector3 position2 = value.cachedTransform.position;
							if (!Mathf.Approximately(position.x, position2.x) || !Mathf.Approximately(position.z, position2.z))
							{
								Vector3 position3 = UnitView.RaycastToGroundFromPoint(position);
								value.cachedTransform.position = position3;
							}
						}
					}
				}
			}
		}

		private void DespawnTriggerArrowIcon(Promise<NGUIIconController> triggerArrowIconPromise)
		{
			if (triggerArrowIconPromise.Current != null)
			{
				NGUIIconController nguiiconController = triggerArrowIconPromise.Current as NGUIIconController;
				if (nguiiconController != null)
				{
					this.mHUDSystem.RetireIcon(nguiiconController);
					return;
				}
			}
			else
			{
				this.mHUDSystem.CancelSpawnRequest(triggerArrowIconPromise);
			}
		}

		protected override void OnInitialized()
		{
			UISystem uisystem;
			if (base.GlobalDependencyContainer.Get<UISystem>(out uisystem))
			{
				this.mUnitInterfaceController = uisystem.GetPanel<UnitInterfaceController>();
				if (this.mUnitInterfaceController == null)
				{
					Log.Error(base.gameObject, Log.Channel.UI, "No UnitInterfaceController found for {0}", new object[]
					{
						base.GetType()
					});
				}
			}
			else
			{
				Log.Error(Log.Channel.UI, "No UISystem found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			this.mTriggerObjectiveArrows = new Dictionary<TriggerCircleEntity, Promise<NGUIIconController>>();
			this.mTriggerObjectiveCircles = new Dictionary<TriggerCircleEntity, UISprite>();
			ShipbreakersMain.PresentationEventSystem.AddHandler<MapObjectInteractionEvent>(new BBI.Core.Events.EventHandler<MapObjectInteractionEvent>(this.OnMapObjectInteractionEvent));
			ShipbreakersMain.PresentationEventSystem.AddHandler<UIHighlightTriggerVolumeEvent>(new BBI.Core.Events.EventHandler<UIHighlightTriggerVolumeEvent>(this.OnUIHighlightTriggerVolume));
			ShipbreakersMain.PresentationEventSystem.AddHandler<ToggleTriggerVolumeEvent>(new BBI.Core.Events.EventHandler<ToggleTriggerVolumeEvent>(this.OnToggleTriggerVolumeEvent));
			ShipbreakersMain.PresentationEventSystem.AddHandler<PresentationSaveLoadedEvent>(new BBI.Core.Events.EventHandler<PresentationSaveLoadedEvent>(this.OnSaveLoadedEvent));
		}

		protected override void OnAttachListeners(IEventReceiver simToPresEventSystem)
		{
			simToPresEventSystem.AddHandler<SceneEntityCreatedEvent>(new BBI.Core.Events.EventHandler<SceneEntityCreatedEvent>(this.OnSceneEntityCreated));
			simToPresEventSystem.AddHandler<HideSceneObjectEvent>(new BBI.Core.Events.EventHandler<HideSceneObjectEvent>(this.OnHideSceneObjectEvent));
			UISystem.AttachOnSaveHandler(new UISystem.PresentationSaveCallback(this.OnSessionSaved));
		}

		private void OnSessionSaved(ref PresentationSaveState state, bool persistenceDataOnly)
		{
			if (!persistenceDataOnly)
			{
				Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
				foreach (KeyValuePair<TriggerCircleEntity, UISprite> keyValuePair in this.mTriggerObjectiveCircles)
				{
					dictionary.Add(keyValuePair.Key.name, keyValuePair.Key.gameObject.activeInHierarchy);
				}
				ExtractorManager.Save<Dictionary<string, bool>, KeyValuePair<string, bool>[]>(dictionary, ref state.TriggerCircleEntityStates);
			}
		}

		private void OnSaveLoadedEvent(PresentationSaveLoadedEvent ev)
		{
			if (!ev.FromPersistence)
			{
				foreach (KeyValuePair<string, bool> keyValuePair in ev.State.TriggerCircleEntityStates)
				{
					foreach (KeyValuePair<TriggerCircleEntity, UISprite> keyValuePair2 in this.mTriggerObjectiveCircles)
					{
						if (keyValuePair2.Key.name == keyValuePair.Key)
						{
							keyValuePair2.Key.gameObject.SetActive(keyValuePair.Value);
							break;
						}
					}
				}
			}
		}

		protected override void OnSessionStarted()
		{
			if (this.m_Settings != null)
			{
				if (this.m_Settings.ObjectiveArrowMarkerPrefab == null)
				{
					Log.Error(Log.Channel.UI, "NULL ObjectiveArrowMarkerPrefab reference in settings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.m_Settings.ObjectiveCircleMarkerPrefab == null)
				{
					Log.Error(Log.Channel.UI, "NULL ObjectiveCircleMarkerPrefab reference in settings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.m_Settings.ObjectiveIconContainer == null)
				{
					Log.Error(Log.Channel.UI, "NULL ObjectiveIconContainer reference in settings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.m_Settings.ObjectiveThreatCircleMarkerPrefab == null)
				{
					Log.Error(Log.Channel.UI, "NULL ObjectiveThreatCircleMarkerPrefab reference in settings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.m_Settings.ObjectiveThreatArrowMarkerPrefab == null)
				{
					Log.Error(Log.Channel.UI, "NULL ObjectiveThreatArrowMarkerPrefab reference in settings for {0}", new object[]
					{
						base.GetType()
					});
				}
			}
			if (this.mHUDSystem == null)
			{
				Log.Error(Log.Channel.UI, "NULL HUDSystem instance found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			this.mResourceViewController = new ResourceViewController(this.mUnitInterfaceController, this.m_ResourceViewControllerSettings, base.GlobalDependencyContainer, base.SessionDependencies);
			this.mExtractionZoneViewController = new ExtractionZoneViewController(this.mUnitInterfaceController, this.m_ExtractionZoneViewControllerSettings, base.GlobalDependencyContainer, base.SessionDependencies);
			this.mRelicViewController = new RelicViewController(this.mUnitInterfaceController, this.m_RelicViewControllerSettings, base.GlobalDependencyContainer, base.SessionDependencies);
		}

		protected override void OnSessionEnded()
		{
			if (this.mResourceViewController != null)
			{
				this.mResourceViewController.Dispose();
				this.mResourceViewController = null;
			}
			if (this.mExtractionZoneViewController != null)
			{
				this.mExtractionZoneViewController.Dispose();
				this.mExtractionZoneViewController = null;
			}
			if (this.mRelicViewController != null)
			{
				this.mRelicViewController.Dispose();
				this.mRelicViewController = null;
			}
			UISystem.RemoveOnSaveHandler(new UISystem.PresentationSaveCallback(this.OnSessionSaved));
			ShipbreakersMain.SimToPresentationEventSystem.RemoveHandler<SceneEntityCreatedEvent>(new BBI.Core.Events.EventHandler<SceneEntityCreatedEvent>(this.OnSceneEntityCreated));
			ShipbreakersMain.SimToPresentationEventSystem.RemoveHandler<HideSceneObjectEvent>(new BBI.Core.Events.EventHandler<HideSceneObjectEvent>(this.OnHideSceneObjectEvent));
			if (this.mHUDSystem != null && !this.mTriggerObjectiveArrows.IsNullOrEmpty<KeyValuePair<TriggerCircleEntity, Promise<NGUIIconController>>>())
			{
				foreach (KeyValuePair<TriggerCircleEntity, Promise<NGUIIconController>> keyValuePair in this.mTriggerObjectiveArrows)
				{
					Promise<NGUIIconController> value = keyValuePair.Value;
					this.DespawnTriggerArrowIcon(value);
				}
				this.mTriggerObjectiveArrows.Clear();
			}
			if (!this.mTriggerObjectiveCircles.IsNullOrEmpty<KeyValuePair<TriggerCircleEntity, UISprite>>())
			{
				foreach (KeyValuePair<TriggerCircleEntity, UISprite> keyValuePair2 in this.mTriggerObjectiveCircles)
				{
					UISystem.DeactivateAndDestroy(keyValuePair2.Value.gameObject);
				}
				this.mTriggerObjectiveCircles.Clear();
			}
		}

		private void OnDestroy()
		{
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<MapObjectInteractionEvent>(new BBI.Core.Events.EventHandler<MapObjectInteractionEvent>(this.OnMapObjectInteractionEvent));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<UIHighlightTriggerVolumeEvent>(new BBI.Core.Events.EventHandler<UIHighlightTriggerVolumeEvent>(this.OnUIHighlightTriggerVolume));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<ToggleTriggerVolumeEvent>(new BBI.Core.Events.EventHandler<ToggleTriggerVolumeEvent>(this.OnToggleTriggerVolumeEvent));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<PresentationSaveLoadedEvent>(new BBI.Core.Events.EventHandler<PresentationSaveLoadedEvent>(this.OnSaveLoadedEvent));
		}

		public SceneEntityViewController()
		{
		}

		private const string kObjectiveLabelVariableName = "ObjectiveLabel";

		[SerializeField]
		private SceneEntityViewController.SceneEntityViewControllerSettings m_Settings = new SceneEntityViewController.SceneEntityViewControllerSettings();

		[SerializeField]
		private ResourceViewController.ResourceViewControllerSettings m_ResourceViewControllerSettings = new ResourceViewController.ResourceViewControllerSettings();

		[SerializeField]
		private ExtractionZoneViewController.ExtractionZoneViewControllerSettings m_ExtractionZoneViewControllerSettings = new ExtractionZoneViewController.ExtractionZoneViewControllerSettings();

		[SerializeField]
		private RelicViewController.RelicViewControllerSettings m_RelicViewControllerSettings = new RelicViewController.RelicViewControllerSettings();

		private UnitInterfaceController mUnitInterfaceController;

		private ResourceViewController mResourceViewController;

		private ExtractionZoneViewController mExtractionZoneViewController;

		private RelicViewController mRelicViewController;

		private Dictionary<TriggerCircleEntity, UISprite> mTriggerObjectiveCircles;

		private Dictionary<TriggerCircleEntity, Promise<NGUIIconController>> mTriggerObjectiveArrows;

		[Serializable]
		public class SceneEntityViewControllerSettings
		{
			public float MarkerAltitudeOffset
			{
				get
				{
					return this.m_MarkerAltitudeOffset;
				}
			}

			public float ObjectiveMarkerAltitudeOffset
			{
				get
				{
					return this.m_ObjectiveMarkerAltitudeOffset;
				}
			}

			public GameObject ObjectiveCircleMarkerPrefab
			{
				get
				{
					return this.m_ObjectiveCircleMarkerPrefab;
				}
			}

			public NGUIIconController ObjectiveArrowMarkerPrefab
			{
				get
				{
					return this.m_ObjectiveArrowMarkerPrefab;
				}
			}

			public GameObject ObjectiveThreatCircleMarkerPrefab
			{
				get
				{
					return this.m_ObjectiveThreatCircleMarkerPrefab;
				}
			}

			public NGUIIconController ObjectiveThreatArrowMarkerPrefab
			{
				get
				{
					return this.m_ObjectiveThreatArrowMarkerPrefab;
				}
			}

			public GameObject ObjectiveIconContainer
			{
				get
				{
					return this.m_ObjectiveIconContainer;
				}
			}

			public SceneEntityViewControllerSettings()
			{
			}

			[SerializeField]
			private float m_MarkerAltitudeOffset = 20f;

			[SerializeField]
			private float m_ObjectiveMarkerAltitudeOffset = 100f;

			[SerializeField]
			private GameObject m_ObjectiveCircleMarkerPrefab;

			[SerializeField]
			private NGUIIconController m_ObjectiveArrowMarkerPrefab;

			[SerializeField]
			private GameObject m_ObjectiveThreatCircleMarkerPrefab;

			[SerializeField]
			private NGUIIconController m_ObjectiveThreatArrowMarkerPrefab;

			[Tooltip("The GameObject to parent objective circles under. Make sure this has a UIPanel.")]
			[SerializeField]
			private GameObject m_ObjectiveIconContainer;
		}
	}
}
