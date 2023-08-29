using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Events;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Commands;
using BBI.Game.Data;
using BBI.Game.Events;
using BBI.Game.Simulation;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Events;
using BBI.Unity.Game.Events.UserInterface;
using BBI.Unity.Game.FX;
using BBI.Unity.Game.HUD;
using BBI.Unity.Game.Localize;
using BBI.Unity.Game.World;
using PathologicalGames;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	public class RelicViewController : IDisposable
	{
		public RelicViewController(UnitInterfaceController unitInterfaceController, RelicViewController.RelicViewControllerSettings settings, BlackbirdPanelBase.BlackbirdPanelGlobalLifetimeDependencyContainer globalDependencies, BlackbirdPanelBase.BlackbirdPanelSessionDependencyContainer sessionDependencies)
		{
			ShipbreakersMain.SimToPresentationEventSystem.AddHandler<CollectibleEntityCreatedEvent>(new BBI.Core.Events.EventHandler<CollectibleEntityCreatedEvent>(this.OnCollectibleEntityCreatedEvent));
			ShipbreakersMain.SimToPresentationEventSystem.AddHandler<VisibilityChangedEvent>(new BBI.Core.Events.EventHandler<VisibilityChangedEvent>(this.OnVisibilityChangedEvent));
			ShipbreakersMain.SimToPresentationEventSystem.AddHandler<RelicEvent>(new BBI.Core.Events.EventHandler<RelicEvent>(this.OnRelicEvent));
			ShipbreakersMain.SimToPresentationEventSystem.AddHandler<SceneEntitiesLoadedEvent>(new BBI.Core.Events.EventHandler<SceneEntitiesLoadedEvent>(this.OnSceneEntitiesLoadedEvent));
			ShipbreakersMain.PresentationEventSystem.AddHandler<TacticalOverlayToggleEvent>(new BBI.Core.Events.EventHandler<TacticalOverlayToggleEvent>(this.OnTacticalOverlayToggleEvent));
			ShipbreakersMain.PresentationEventSystem.AddHandler<SensorsManagerEvent>(new BBI.Core.Events.EventHandler<SensorsManagerEvent>(this.OnSensorsManagerEvent));
			this.mSettings = settings;
			if (this.mSettings == null)
			{
				Log.Error(Log.Channel.UI, "No RelicViewControllerSettings supplied to {0}", new object[]
				{
					base.GetType()
				});
			}
			this.mInterfaceController = unitInterfaceController;
			if (this.mInterfaceController != null)
			{
				this.mInterfaceController.NewFrameFromSim += this.OnNewFrameFromSim;
				this.mInterfaceController.ArtifactHighlightToggle += this.OnArtifactHighlightToggle;
			}
			else
			{
				Log.Error(Log.Channel.UI, "No UnitInterfaceController supplied to {0}", new object[]
				{
					base.GetType()
				});
			}
			if (!globalDependencies.Get<IGameLocalization>(out this.mLocalizationManager))
			{
				Log.Error(Log.Channel.UI, "NULL Localization Manager instance found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			if (!sessionDependencies.Get<HUDSystem>(out this.mHUDSystem))
			{
				Log.Error(Log.Channel.UI, "NULL HUDSystem instance found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			if (!sessionDependencies.Get<ICommanderManager>(out this.mCommanderManager))
			{
				Log.Error(Log.Channel.UI, "NULL ICommanderManager instance found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			if (!sessionDependencies.Get<ICommanderInteractionProvider>(out this.mInteractionProvider))
			{
				Log.Error(Log.Channel.UI, "No ICommanderInteractionProvider found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
		}

		public void Dispose()
		{
			ShipbreakersMain.SimToPresentationEventSystem.RemoveHandler<VisibilityChangedEvent>(new BBI.Core.Events.EventHandler<VisibilityChangedEvent>(this.OnVisibilityChangedEvent));
			ShipbreakersMain.SimToPresentationEventSystem.RemoveHandler<RelicEvent>(new BBI.Core.Events.EventHandler<RelicEvent>(this.OnRelicEvent));
			ShipbreakersMain.SimToPresentationEventSystem.RemoveHandler<CollectibleEntityCreatedEvent>(new BBI.Core.Events.EventHandler<CollectibleEntityCreatedEvent>(this.OnCollectibleEntityCreatedEvent));
			ShipbreakersMain.SimToPresentationEventSystem.RemoveHandler<SceneEntitiesLoadedEvent>(new BBI.Core.Events.EventHandler<SceneEntitiesLoadedEvent>(this.OnSceneEntitiesLoadedEvent));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<TacticalOverlayToggleEvent>(new BBI.Core.Events.EventHandler<TacticalOverlayToggleEvent>(this.OnTacticalOverlayToggleEvent));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<SensorsManagerEvent>(new BBI.Core.Events.EventHandler<SensorsManagerEvent>(this.OnSensorsManagerEvent));
			if (this.mInterfaceController != null)
			{
				this.mInterfaceController.NewFrameFromSim -= this.OnNewFrameFromSim;
				this.mInterfaceController.ArtifactHighlightToggle -= this.OnArtifactHighlightToggle;
				this.mInterfaceController = null;
			}
			if (this.mHUDSystem != null)
			{
				foreach (KeyValuePair<Entity, CollectibleEntityView> keyValuePair in this.mRelicViews)
				{
					this.DespawnRelicView(keyValuePair.Value);
				}
				this.mRelicViews.Clear();
				this.mHUDSystem = null;
			}
			if (this.mShowingTooltipForEntity != Entity.None)
			{
				this.mShowingTooltipForEntity = Entity.None;
				UISystem.HideTooltip();
			}
			this.mLocalizationManager = null;
			this.mCommanderManager = null;
			this.mInteractionProvider = null;
			this.mSettings = null;
		}

		private void OnNewFrameFromSim(SimStateFrame newFrame)
		{
			foreach (KeyValuePair<Entity, CollectibleEntityView> keyValuePair in this.mRelicViews)
			{
				Entity key = keyValuePair.Key;
				this.UpdateRelicProgressBar(newFrame, key, keyValuePair.Value);
				this.UpdateRelicViewPosition(newFrame, key, keyValuePair.Value);
			}
		}

		private void OnSceneEntitiesLoadedEvent(SceneEntitiesLoadedEvent ev)
		{
			foreach (KeyValuePair<Entity, CollectibleEntityView> keyValuePair in this.mRelicViews)
			{
				this.DespawnRelicView(keyValuePair.Value);
			}
			this.mRelicViews.Clear();
			foreach (Entity entity in ev.NewRelicEntities)
			{
				this.CreateRelicEntityView(entity);
			}
		}

		private void OnCollectibleEntityCreatedEvent(CollectibleEntityCreatedEvent ev)
		{
			if (ev.CollectibleType == CollectibleType.Artifact)
			{
				this.CreateRelicEntityView(ev.Entity);
			}
		}

		public void OnSceneEntityCreated(SceneEntityCreatedEvent ev)
		{
			SceneEntityDescriptor sceneEntityDescriptor = ev.SceneEntityDescriptor;
			this.CreateRelicEntityView(ev.Entity);
		}

		public void OnMapObjectInteractionEvent(MapObjectInteractionEvent ev)
		{
			CollectibleEntityView collectibleEntityView = ev.MapObject as CollectibleEntityView;
			if (collectibleEntityView != null)
			{
				collectibleEntityView.OnMapObjectInteractionEvent(ev);
				if (ev.Type == InteractionType.MouseOver)
				{
					this.ShowRelicTooltip(collectibleEntityView.Entity);
					return;
				}
				if (ev.Type == InteractionType.MouseOff)
				{
					this.mShowingTooltipForEntity = Entity.None;
					UISystem.HideTooltip();
				}
			}
		}

		private void OnVisibilityChangedEvent(VisibilityChangedEvent ev)
		{
			if (ev.FromState == ev.ToState)
			{
				return;
			}
			SimStateFrame currentSimFrame = ShipbreakersMain.CurrentSimFrame;
			Entity entity = ev.Entity;
			RelicState relicState = currentSimFrame.FindObject<RelicState>(entity);
			CollectibleState collectibleState = currentSimFrame.FindObject<CollectibleState>(entity);
			if (relicState != null && collectibleState != null)
			{
				CollectibleEntityView view;
				if (this.mRelicViews.TryGetValue(entity, out view))
				{
					this.UpdateRelicView(view, relicState, collectibleState);
					return;
				}
			}
			else
			{
				UnitState unitState = currentSimFrame.FindObject<UnitState>(entity);
				if (unitState != null)
				{
					foreach (KeyValuePair<Entity, CollectibleEntityView> keyValuePair in this.mRelicViews)
					{
						CollectibleEntityView value = keyValuePair.Value;
						if (value.HeldByEntity == entity)
						{
							this.UpdateRelicViewHUDState(value);
							break;
						}
					}
				}
			}
		}

		private void OnRelicEvent(RelicEvent ev)
		{
			SimStateFrame currentSimFrame = ShipbreakersMain.CurrentSimFrame;
			Entity relicEntity = ev.RelicEntity;
			RelicState relicState = currentSimFrame.FindObject<RelicState>(relicEntity);
			CollectibleState collectibleState = currentSimFrame.FindObject<CollectibleState>(relicEntity);
			if (relicState != null && collectibleState != null)
			{
				CollectibleEntityView view;
				if (this.mRelicViews.TryGetValue(relicEntity, out view))
				{
					bool flag = false;
					switch (ev.Reason)
					{
					case RelicEvent.EventType.PickedUp:
					case RelicEvent.EventType.Transferred:
						this.TriggerPickup(view, ev.TriggeringEntity);
						break;
					case RelicEvent.EventType.Dropped:
					case RelicEvent.EventType.Expired:
						this.TriggerDetonation(view);
						this.DespawnAndRemoveRelicView(relicEntity, view);
						flag = true;
						break;
					case RelicEvent.EventType.Extracted:
						this.TriggerExtractionFX(view);
						this.DespawnAndRemoveRelicView(relicEntity, view);
						flag = true;
						break;
					}
					if (!flag)
					{
						this.UpdateRelicView(view, relicState, collectibleState);
						return;
					}
				}
				else
				{
					Log.Warn(Log.Channel.UI, "Failed to find CollectibleEntityView for relic entity {0} for RelicEvent!", new object[]
					{
						relicEntity.ToFriendlyString()
					});
				}
			}
		}

		private void OnTacticalOverlayToggleEvent(TacticalOverlayToggleEvent ev)
		{
			this.SetAllRelicIconVisibility();
		}

		private void OnSensorsManagerEvent(SensorsManagerEvent ev)
		{
			this.SetAllRelicIconVisibility();
		}

		private void OnArtifactHighlightToggle(bool active)
		{
			foreach (KeyValuePair<Entity, CollectibleEntityView> keyValuePair in this.mRelicViews)
			{
				if (keyValuePair.Value != null)
				{
					keyValuePair.Value.ArtifactHighlightToggle(active);
				}
			}
		}

		private void OnRelicTooltip(NGUIEventHandler handler, bool show)
		{
			if (!show)
			{
				this.mShowingTooltipForEntity = Entity.None;
				UISystem.HideTooltip();
				return;
			}
			CollectibleEntityView collectibleEntityView = handler.Data as CollectibleEntityView;
			if (collectibleEntityView != null)
			{
				Entity entity = collectibleEntityView.Entity;
				this.ShowRelicTooltip(entity);
				return;
			}
			Log.Error(Log.Channel.UI, "Non-CollectibleEntityView type object set for icon {0} triggering relic tooltip!", new object[]
			{
				handler.name
			});
		}

		private void OnDefaultIconSpawned(CollectibleEntityView view)
		{
			if (view != null)
			{
				view.OnDefaultIconSpawned = (Action<CollectibleEntityView>)Delegate.Remove(view.OnDefaultIconSpawned, new Action<CollectibleEntityView>(this.OnDefaultIconSpawned));
				if (view.DefaultIcon != null)
				{
					view.DefaultIcon.HoverAction = new NGUIEventHandler.NGUIEventDelegateTyped<bool>(this.OnRelicTooltip);
				}
				this.UpdateRelicViewHUDState(view);
			}
		}

		private void OnAttachedIconSpawned(CollectibleEntityView view)
		{
			if (view != null)
			{
				view.OnAttachedIconSpawned = (Action<CollectibleEntityView>)Delegate.Remove(view.OnAttachedIconSpawned, new Action<CollectibleEntityView>(this.OnAttachedIconSpawned));
				if (view.AttachedIcon != null)
				{
					view.AttachedIcon.HoverAction = new NGUIEventHandler.NGUIEventDelegateTyped<bool>(this.OnRelicTooltip);
				}
				this.UpdateRelicViewHUDState(view);
			}
		}

		private void CreateRelicEntityView(Entity entity)
		{
			Assert.Release(!this.mRelicViews.ContainsKey(entity), "[RH]: Trying to creating a CollectibleEntityView for relic entity but one already exists!");
			SimStateFrame currentSimFrame = ShipbreakersMain.CurrentSimFrame;
			RelicState relicState = currentSimFrame.FindObject<RelicState>(entity);
			CollectibleState collectibleState = currentSimFrame.FindObject<CollectibleState>(entity);
			if (relicState == null || collectibleState == null)
			{
				return;
			}
			GameObject gameObject = null;
			SceneEntityViewAttributes entityTypeAttributes = ShipbreakersMain.GetEntityTypeAttributes<SceneEntityViewAttributes>(relicState.TypeID);
			if (entityTypeAttributes != null && entityTypeAttributes.ModelPrefab != null)
			{
				gameObject = entityTypeAttributes.ModelPrefab;
			}
			if (gameObject == null)
			{
				Log.Error(Log.Channel.Data | Log.Channel.UI, "No model prefab found for entity type {0}!", new object[]
				{
					entity.ToFriendlyString()
				});
				return;
			}
			GameObject gameObject2 = null;
			string labelText = relicState.TypeID;
			SceneEntityHUDAttributes entityTypeAttributes2 = ShipbreakersMain.GetEntityTypeAttributes<SceneEntityHUDAttributes>(relicState.TypeID);
			if (entityTypeAttributes2 != null)
			{
				labelText = entityTypeAttributes2.LocalizedTitleStringID;
				gameObject2 = entityTypeAttributes2.IconPrefab;
			}
			if (gameObject2 == null)
			{
				Log.Error(Log.Channel.Data | Log.Channel.UI, "No default icon prefab found for entity type {0}!", new object[]
				{
					entity.ToFriendlyString()
				});
			}
			Vector3 vector = VectorHelper.SimVector2ToUnityVector3(relicState.MapPosition);
			vector = UnitView.RaycastToGroundFromPoint(vector);
			CollectibleEntityView.SpawnSettings spawnSettings = new CollectibleEntityView.SpawnSettings(entity, gameObject2, gameObject2, vector, this.mHUDSystem, labelText, this.mSettings.MarkerAltitudeOffset);
			Transform transform = PoolManager.Pools["Models"].Spawn(gameObject.transform, vector, Quaternion.identity, new FXSpawnDependencies(new object[]
			{
				spawnSettings
			}));
			CollectibleEntityView collectibleEntityView = (transform != null) ? transform.GetComponent<CollectibleEntityView>() : null;
			if (collectibleEntityView == null)
			{
				Log.Error(Log.Channel.Graphics, "Could not pool spawn a CollectibleEntityView for prefab {0}", new object[]
				{
					gameObject.name
				});
				return;
			}
			this.mRelicViews.Add(entity, collectibleEntityView);
			CollectibleEntityView collectibleEntityView2 = collectibleEntityView;
			collectibleEntityView2.OnDefaultIconSpawned = (Action<CollectibleEntityView>)Delegate.Combine(collectibleEntityView2.OnDefaultIconSpawned, new Action<CollectibleEntityView>(this.OnDefaultIconSpawned));
			CollectibleEntityView collectibleEntityView3 = collectibleEntityView;
			collectibleEntityView3.OnAttachedIconSpawned = (Action<CollectibleEntityView>)Delegate.Combine(collectibleEntityView3.OnAttachedIconSpawned, new Action<CollectibleEntityView>(this.OnAttachedIconSpawned));
			this.UpdateRelicView(collectibleEntityView, relicState, collectibleState);
		}

		private void DespawnAndRemoveRelicView(Entity relicEntity, CollectibleEntityView view)
		{
			this.mRelicViews.Remove(relicEntity);
			this.DespawnRelicView(view);
		}

		private void DespawnRelicView(CollectibleEntityView view)
		{
			if (view != null)
			{
				if (this.mShowingTooltipForEntity != Entity.None && this.mShowingTooltipForEntity == view.Entity)
				{
					this.mShowingTooltipForEntity = Entity.None;
					UISystem.HideTooltip();
				}
				SpawnPool spawnPool;
				if (PoolManager.Pools.TryGetValue("Models", out spawnPool))
				{
					view.gameObject.SetActive(true);
					spawnPool.Despawn(view.transform, spawnPool.transform, view.transform);
				}
			}
		}

		private void UpdateRelicProgressBar(SimStateFrame stateFrame, Entity relicEntity, CollectibleEntityView view)
		{
			RelicState relicState = stateFrame.FindObject<RelicState>(relicEntity);
			if (relicState != null)
			{
				bool flag = relicState.Visibility == DetectionState.Sensed;
				if (flag)
				{
					float value = Fixed64.UnsafeFloatValue(relicState.CurrentTimePercentage01);
					view.UpdateProgressBarValue(value);
					bool show = relicState.HasTimer && !relicState.TimerCompleted && relicState.TimerDirection == TimerDirection.Countup;
					view.ShowProgressBar(show);
				}
			}
		}

		private void UpdateRelicViewPosition(SimStateFrame stateFrame, Entity relicEntity, CollectibleEntityView view)
		{
			CollectibleState collectibleState = stateFrame.FindObject<CollectibleState>(relicEntity);
			if (collectibleState != null && !collectibleState.AvailableForCollection)
			{
				Entity currentHolderEntity = collectibleState.CurrentHolderEntity;
				UnitState unitState = ShipbreakersMain.CurrentSimFrame.FindObject<UnitState>(currentHolderEntity);
				if (unitState != null && unitState.Visibility != DetectionState.Sensed)
				{
					Vector3 vector = VectorHelper.SimVector2ToUnityVector3(unitState.StartOfFrameDynamicState.Position);
					vector = UnitView.RaycastToGroundFromPoint(vector);
					view.transform.position = vector;
				}
			}
		}

		private bool GetRelicIconHUDSettings(Entity relicEntity, DetectionState visibility, bool useUnidentifiedIcon, out string iconName, out Color iconColorToUse)
		{
			iconColorToUse = Color.white;
			iconName = string.Empty;
			string typeName = relicEntity.GetTypeName();
			if (string.IsNullOrEmpty(typeName))
			{
				return false;
			}
			SceneEntityHUDAttributes entityTypeAttributes = ShipbreakersMain.GetEntityTypeAttributes<SceneEntityHUDAttributes>(typeName);
			if (entityTypeAttributes != null)
			{
				bool flag = visibility == DetectionState.Contacted;
				if (flag)
				{
					iconName = entityTypeAttributes.ContactedIconName;
					iconColorToUse = entityTypeAttributes.ContactedIconColor;
				}
				else
				{
					iconName = ((!useUnidentifiedIcon) ? entityTypeAttributes.IconName : entityTypeAttributes.UnidentifiedIconName);
					iconColorToUse = ((!useUnidentifiedIcon) ? entityTypeAttributes.IconColour : entityTypeAttributes.UnidentifiedIconColour);
				}
				return true;
			}
			return false;
		}

		private void UpdateRelicDefaultIconSprite(CollectibleEntityView view, DetectionState visibility, bool useUnidentifiedIcon)
		{
			string spriteName;
			Color colour;
			if (view != null && this.GetRelicIconHUDSettings(view.Entity, visibility, useUnidentifiedIcon, out spriteName, out colour))
			{
				view.UpdateDefaultIconSprite(spriteName, colour);
			}
		}

		private void UpdateRelicAttachedIconSprite(CollectibleEntityView view, DetectionState visibility, bool useUnidentifiedIcon)
		{
			string spriteName;
			Color colour;
			if (view != null && this.GetRelicIconHUDSettings(view.Entity, visibility, useUnidentifiedIcon, out spriteName, out colour))
			{
				view.UpdateAttachedIconSprite(spriteName, colour);
			}
		}

		private void UpdateRelicIconTrackOnScreenEdge(CollectibleEntityView view, bool trackOnScreenEdge)
		{
			if (view != null)
			{
				view.UpdateTrackOnScreenEdge(trackOnScreenEdge);
			}
		}

		private void SetAllRelicIconVisibility()
		{
			SimStateFrame currentSimFrame = ShipbreakersMain.CurrentSimFrame;
			foreach (KeyValuePair<Entity, CollectibleEntityView> keyValuePair in this.mRelicViews)
			{
				CollectibleEntityView value = keyValuePair.Value;
				if (value != null)
				{
					bool flag = value.ShouldShowIcon();
					value.ShowIcon(flag);
					if (!flag && this.mShowingTooltipForEntity != Entity.None && this.mShowingTooltipForEntity == value.Entity)
					{
						this.mShowingTooltipForEntity = Entity.None;
						UISystem.HideTooltip();
					}
				}
			}
		}

		private void ShowRelicTooltip(Entity relicEntity)
		{
			SimStateFrame currentSimFrame = ShipbreakersMain.CurrentSimFrame;
			RelicState relicState = currentSimFrame.FindObject<RelicState>(relicEntity);
			if (relicState != null)
			{
				bool flag = relicState.Visibility == DetectionState.Sensed;
				if (flag)
				{
					SceneEntityHUDAttributes entityTypeAttributes = ShipbreakersMain.GetEntityTypeAttributes<SceneEntityHUDAttributes>(relicState.TypeID);
					if (entityTypeAttributes != null && entityTypeAttributes.Tooltip != null)
					{
						RelicViewController.RelicTooltipData relicTooltipData = default(RelicViewController.RelicTooltipData);
						CollectibleType collectibleType = relicState.CollectibleType;
						if (relicState.TimerCompleted)
						{
							relicTooltipData.LocalizedTitleStringID = this.mLocalizationManager.GetText(entityTypeAttributes.LocalizedTitleStringID);
							relicTooltipData.LocalizedShortDescriptionStringID = this.mLocalizationManager.GetText(entityTypeAttributes.LocalizedShortDescriptionStringID);
							relicTooltipData.LocalizedLongDescriptionStringID = this.mLocalizationManager.GetText(entityTypeAttributes.LocalizedLongDescriptionStringID);
						}
						else
						{
							relicTooltipData.LocalizedTitleStringID = this.mLocalizationManager.GetText(entityTypeAttributes.LocalizedUnknownTitleStringID);
							relicTooltipData.LocalizedShortDescriptionStringID = this.mLocalizationManager.GetText(entityTypeAttributes.LocalizedUnknownShortDescriptionStringID);
							relicTooltipData.LocalizedLongDescriptionStringID = this.mLocalizationManager.GetText(entityTypeAttributes.LocalizedUnknownLongDescriptionStringID);
						}
						this.mShowingTooltipForEntity = relicEntity;
						UISystem.ShowTooltip(new object[]
						{
							relicTooltipData
						}, entityTypeAttributes.Tooltip, this.mSettings.TooltipAnchor);
					}
				}
			}
		}

		private void UpdateRelicView(CollectibleEntityView view, RelicState relicState, CollectibleState collectibleState)
		{
			view.UpdateCollectibleView(relicState.Visibility);
			this.UpdateRelicViewHUDState(view);
			bool active = (!relicState.HasTimer || relicState.TimerCompleted || relicState.TimerDirection != TimerDirection.Countup) && collectibleState.AvailableForCollection && relicState.ExtractionCompletion01 < Fixed64.One;
			view.gameObject.SetActive(active);
		}

		private void UpdateRelicViewHUDState(CollectibleEntityView view)
		{
			SimStateFrame currentSimFrame = ShipbreakersMain.CurrentSimFrame;
			if (MapModManager.CustomLayout && view.Entity != Entity.None && view.Entity.HasComponent(10) && view.Entity.GetComponent<Position>(10).Position2D.X > Fixed64.FromInt(0x186a0))
			{
				this.DespawnAndRemoveRelicView(view.Entity, view);
				return;
			}
			if (currentSimFrame == null)
			{
				return;
			}
			RelicState relicState = currentSimFrame.FindObject<RelicState>(view.Entity);
			if (relicState == null)
			{
				return;
			}
			CollectibleState collectibleState = currentSimFrame.FindObject<CollectibleState>(view.Entity);
			if (collectibleState == null)
			{
				return;
			}
			DetectionState visibility = relicState.Visibility;
			if (ReplayPanelController.RevealAll || SimController.sSpectatorModeEnabled)
			{
				visibility = DetectionState.Sensed;
			}
			bool flag = relicState.HasTimer && relicState.TimerDirection == TimerDirection.Countup && !relicState.TimerCompleted;
			if (collectibleState.AvailableForCollection)
			{
				this.UpdateRelicDefaultIconSprite(view, visibility, flag);
			}
			else
			{
				this.UpdateRelicAttachedIconSprite(view, DetectionState.Sensed, false);
			}
			this.UpdateRelicIconTrackOnScreenEdge(view, !flag);
			bool flag2 = view.ShouldShowIcon();
			view.ShowIcon(flag2);
			if (!flag2 && this.mShowingTooltipForEntity != Entity.None && this.mShowingTooltipForEntity == view.Entity)
			{
				this.mShowingTooltipForEntity = Entity.None;
				UISystem.HideTooltip();
			}
		}

		private void TriggerPickup(CollectibleEntityView view, Entity newOwnerEntity)
		{
			view.PickedUp(newOwnerEntity);
		}

		private void TriggerDetonation(CollectibleEntityView view)
		{
			view.Detonate();
		}

		private void TriggerExtractionFX(CollectibleEntityView view)
		{
			view.Extract();
		}

		private RelicViewController.RelicViewControllerSettings mSettings;

		private UnitInterfaceController mInterfaceController;

		private HUDSystem mHUDSystem;

		private IGameLocalization mLocalizationManager;

		private ICommanderManager mCommanderManager;

		private ICommanderInteractionProvider mInteractionProvider;

		private Dictionary<Entity, CollectibleEntityView> mRelicViews = new Dictionary<Entity, CollectibleEntityView>(20);

		private Entity mShowingTooltipForEntity = Entity.None;

		public struct RelicTooltipData
		{
			public string LocalizedTitleStringID;

			public string LocalizedShortDescriptionStringID;

			public string LocalizedLongDescriptionStringID;
		}

		[Serializable]
		public class RelicViewControllerSettings
		{
			public float MarkerAltitudeOffset
			{
				get
				{
					return this.m_MarkerAltitudeOffset;
				}
			}

			public Transform TooltipAnchor
			{
				get
				{
					return this.m_TooltipAnchor;
				}
			}

			public RelicViewControllerSettings()
			{
			}

			[SerializeField]
			private float m_MarkerAltitudeOffset = 20f;

			[SerializeField]
			private Transform m_TooltipAnchor;
		}
	}
}
