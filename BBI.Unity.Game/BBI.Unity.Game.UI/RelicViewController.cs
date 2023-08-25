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
	// Token: 0x020001F8 RID: 504
	public class RelicViewController : IDisposable
	{
		// Token: 0x06000C9F RID: 3231 RVA: 0x0003CE20 File Offset: 0x0003B020
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

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0003D024 File Offset: 0x0003B224
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

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0003D19C File Offset: 0x0003B39C
		private void OnNewFrameFromSim(SimStateFrame newFrame)
		{
			foreach (KeyValuePair<Entity, CollectibleEntityView> keyValuePair in this.mRelicViews)
			{
				Entity key = keyValuePair.Key;
				this.UpdateRelicProgressBar(newFrame, key, keyValuePair.Value);
				this.UpdateRelicViewPosition(newFrame, key, keyValuePair.Value);
			}
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x0003D210 File Offset: 0x0003B410
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

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0003D2A4 File Offset: 0x0003B4A4
		private void OnCollectibleEntityCreatedEvent(CollectibleEntityCreatedEvent ev)
		{
			if (ev.CollectibleType == CollectibleType.Artifact)
			{
				this.CreateRelicEntityView(ev.Entity);
			}
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0003D2BA File Offset: 0x0003B4BA
		public void OnSceneEntityCreated(SceneEntityCreatedEvent ev)
		{
			SceneEntityDescriptor sceneEntityDescriptor = ev.SceneEntityDescriptor;
			this.CreateRelicEntityView(ev.Entity);
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0003D2D0 File Offset: 0x0003B4D0
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

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0003D328 File Offset: 0x0003B528
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

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0003D3F4 File Offset: 0x0003B5F4
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

		// Token: 0x06000CA8 RID: 3240 RVA: 0x0003D4DA File Offset: 0x0003B6DA
		private void OnTacticalOverlayToggleEvent(TacticalOverlayToggleEvent ev)
		{
			this.SetAllRelicIconVisibility();
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0003D4E2 File Offset: 0x0003B6E2
		private void OnSensorsManagerEvent(SensorsManagerEvent ev)
		{
			this.SetAllRelicIconVisibility();
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0003D4EC File Offset: 0x0003B6EC
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

		// Token: 0x06000CAB RID: 3243 RVA: 0x0003D554 File Offset: 0x0003B754
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

		// Token: 0x06000CAC RID: 3244 RVA: 0x0003D5BC File Offset: 0x0003B7BC
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

		// Token: 0x06000CAD RID: 3245 RVA: 0x0003D620 File Offset: 0x0003B820
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

		// Token: 0x06000CAE RID: 3246 RVA: 0x0003D684 File Offset: 0x0003B884
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

		// Token: 0x06000CAF RID: 3247 RVA: 0x0003D88A File Offset: 0x0003BA8A
		private void DespawnAndRemoveRelicView(Entity relicEntity, CollectibleEntityView view)
		{
			this.mRelicViews.Remove(relicEntity);
			this.DespawnRelicView(view);
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0003D8A0 File Offset: 0x0003BAA0
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

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0003D924 File Offset: 0x0003BB24
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

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0003D980 File Offset: 0x0003BB80
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

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0003D9E4 File Offset: 0x0003BBE4
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

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0003DA70 File Offset: 0x0003BC70
		private void UpdateRelicDefaultIconSprite(CollectibleEntityView view, DetectionState visibility, bool useUnidentifiedIcon)
		{
			string spriteName;
			Color colour;
			if (view != null && this.GetRelicIconHUDSettings(view.Entity, visibility, useUnidentifiedIcon, out spriteName, out colour))
			{
				view.UpdateDefaultIconSprite(spriteName, colour);
			}
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0003DAA4 File Offset: 0x0003BCA4
		private void UpdateRelicAttachedIconSprite(CollectibleEntityView view, DetectionState visibility, bool useUnidentifiedIcon)
		{
			string spriteName;
			Color colour;
			if (view != null && this.GetRelicIconHUDSettings(view.Entity, visibility, useUnidentifiedIcon, out spriteName, out colour))
			{
				view.UpdateAttachedIconSprite(spriteName, colour);
			}
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0003DAD6 File Offset: 0x0003BCD6
		private void UpdateRelicIconTrackOnScreenEdge(CollectibleEntityView view, bool trackOnScreenEdge)
		{
			if (view != null)
			{
				view.UpdateTrackOnScreenEdge(trackOnScreenEdge);
			}
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0003DAE8 File Offset: 0x0003BCE8
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

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0003DB94 File Offset: 0x0003BD94
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

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0003DCC4 File Offset: 0x0003BEC4
		private void UpdateRelicView(CollectibleEntityView view, RelicState relicState, CollectibleState collectibleState)
		{
			view.UpdateCollectibleView(relicState.Visibility);
			this.UpdateRelicViewHUDState(view);
			bool active = (!relicState.HasTimer || relicState.TimerCompleted || relicState.TimerDirection != TimerDirection.Countup) && collectibleState.AvailableForCollection && relicState.ExtractionCompletion01 < Fixed64.One;
			view.gameObject.SetActive(active);
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0003DD2C File Offset: 0x0003BF2C
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

		// Token: 0x06000CBB RID: 3259 RVA: 0x0003DE0D File Offset: 0x0003C00D
		private void TriggerPickup(CollectibleEntityView view, Entity newOwnerEntity)
		{
			view.PickedUp(newOwnerEntity);
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0003DE16 File Offset: 0x0003C016
		private void TriggerDetonation(CollectibleEntityView view)
		{
			view.Detonate();
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0003DE1E File Offset: 0x0003C01E
		private void TriggerExtractionFX(CollectibleEntityView view)
		{
			view.Extract();
		}

		// Token: 0x04000B77 RID: 2935
		private RelicViewController.RelicViewControllerSettings mSettings;

		// Token: 0x04000B78 RID: 2936
		private UnitInterfaceController mInterfaceController;

		// Token: 0x04000B79 RID: 2937
		private HUDSystem mHUDSystem;

		// Token: 0x04000B7A RID: 2938
		private IGameLocalization mLocalizationManager;

		// Token: 0x04000B7B RID: 2939
		private ICommanderManager mCommanderManager;

		// Token: 0x04000B7C RID: 2940
		private ICommanderInteractionProvider mInteractionProvider;

		// Token: 0x04000B7D RID: 2941
		private Dictionary<Entity, CollectibleEntityView> mRelicViews = new Dictionary<Entity, CollectibleEntityView>(20);

		// Token: 0x04000B7E RID: 2942
		private Entity mShowingTooltipForEntity = Entity.None;

		// Token: 0x020001F9 RID: 505
		public struct RelicTooltipData
		{
			// Token: 0x04000B7F RID: 2943
			public string LocalizedTitleStringID;

			// Token: 0x04000B80 RID: 2944
			public string LocalizedShortDescriptionStringID;

			// Token: 0x04000B81 RID: 2945
			public string LocalizedLongDescriptionStringID;
		}

		// Token: 0x020001FA RID: 506
		[Serializable]
		public class RelicViewControllerSettings
		{
			// Token: 0x1700021B RID: 539
			// (get) Token: 0x06000CBE RID: 3262 RVA: 0x0003DE26 File Offset: 0x0003C026
			public float MarkerAltitudeOffset
			{
				get
				{
					return this.m_MarkerAltitudeOffset;
				}
			}

			// Token: 0x1700021C RID: 540
			// (get) Token: 0x06000CBF RID: 3263 RVA: 0x0003DE2E File Offset: 0x0003C02E
			public Transform TooltipAnchor
			{
				get
				{
					return this.m_TooltipAnchor;
				}
			}

			// Token: 0x06000CC0 RID: 3264 RVA: 0x0003DE36 File Offset: 0x0003C036
			public RelicViewControllerSettings()
			{
			}

			// Token: 0x04000B82 RID: 2946
			[SerializeField]
			private float m_MarkerAltitudeOffset = 20f;

			// Token: 0x04000B83 RID: 2947
			[SerializeField]
			private Transform m_TooltipAnchor;
		}
	}
}
