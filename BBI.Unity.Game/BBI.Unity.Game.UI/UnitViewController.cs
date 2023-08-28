using System;
using System.Collections.Generic;
using BBI.Core;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Events;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Commands;
using BBI.Game.Data;
using BBI.Game.Events;
using BBI.Game.Replay;
using BBI.Game.SaveLoad;
using BBI.Game.Simulation;
using BBI.Unity.Core.Rendering;
using BBI.Unity.Core.Utility;
using BBI.Unity.Core.World;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.DLC;
using BBI.Unity.Game.Events;
using BBI.Unity.Game.Events.UserInterface;
using BBI.Unity.Game.FX;
using BBI.Unity.Game.HUD;
using BBI.Unity.Game.World;
using PathologicalGames;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	public class UnitViewController : IDisposable
	{
		private InCombatTweenParameters mInCombatTweenParameters
		{
			get
			{
				if (!Application.isEditor)
				{
					return this.mCachedInCombatTweenParameters;
				}
				if (ShipbreakersMain.GlobalSettingsAttributes.UnitStatusAttributes != null)
				{
					return ShipbreakersMain.GlobalSettingsAttributes.UnitStatusAttributes.InCombatIconTweenSettings;
				}
				Log.Error(Log.Channel.UI, "No GlobalUnitStatusAttributes found for {0}. Using default InCombatIconTweenSettings.", new object[]
				{
					base.GetType()
				});
				return InCombatTweenParameters.kDefaultSettings;
			}
		}

		public UnitViewController(UnitInterfaceController interfaceCon, BlackbirdPanelBase.BlackbirdPanelGlobalLifetimeDependencyContainer globalDependencies, BlackbirdPanelBase.BlackbirdPanelSessionDependencyContainer sessionDependencies)
		{
			ShipbreakersMain.PresentationEventSystem.AddHandler<SensorsManagerEvent>(new BBI.Core.Events.EventHandler<SensorsManagerEvent>(this.OnSensorsManagerTransition));
			ShipbreakersMain.PresentationEventSystem.AddHandler<UIHighlightEntityEvent>(new BBI.Core.Events.EventHandler<UIHighlightEntityEvent>(this.OnUIHighlightEntityEvent));
			ShipbreakersMain.PresentationEventSystem.AddHandler<QualitySettingsChangedEvent>(new BBI.Core.Events.EventHandler<QualitySettingsChangedEvent>(this.OnNewQualitySettings));
			ShipbreakersMain.PresentationEventSystem.AddHandler<CutsceneShotStartEvent>(new BBI.Core.Events.EventHandler<CutsceneShotStartEvent>(this.OnCutSceneStart));
			ShipbreakersMain.PresentationEventSystem.AddHandler<CutsceneFinishedEvent>(new BBI.Core.Events.EventHandler<CutsceneFinishedEvent>(this.OnCutSceneFinished));
			ShipbreakersMain.PresentationEventSystem.AddHandler<TacticalOverlayToggleEvent>(new BBI.Core.Events.EventHandler<TacticalOverlayToggleEvent>(this.OnTacticalOverlayToggleEvent));
			ShipbreakersMain.PresentationEventSystem.AddHandler<PresentationSaveLoadedEvent>(new BBI.Core.Events.EventHandler<PresentationSaveLoadedEvent>(this.OnSaveLoaded));
			if (ShipbreakersMain.ReplayMode == ReplayMode.ReplayingGame)
			{
				ShipbreakersMain.PresentationEventSystem.AddHandler<ToggleReplayRevealAllEvent>(new BBI.Core.Events.EventHandler<ToggleReplayRevealAllEvent>(this.OnToggleReplayRevealAllEvent));
			}
			UISystem.AttachOnSaveHandler(new UISystem.PresentationSaveCallback(this.OnSessionSaved));
			IEventReceiver simToPresentationEventSystem = ShipbreakersMain.SimToPresentationEventSystem;
			simToPresentationEventSystem.AddHandler<SimUnitCreatedEvent>(new BBI.Core.Events.EventHandler<SimUnitCreatedEvent>(this.OnSimUnitCreatedEvent));
			simToPresentationEventSystem.AddHandler<UnitViewableEvent>(new BBI.Core.Events.EventHandler<UnitViewableEvent>(this.OnUnitViewable));
			simToPresentationEventSystem.AddHandler<UnitRemovedFeedbackEvent>(new BBI.Core.Events.EventHandler<UnitRemovedFeedbackEvent>(this.OnUnitDespawnedInSim));
			simToPresentationEventSystem.AddHandler<WeaponEvent>(new BBI.Core.Events.EventHandler<WeaponEvent>(this.OnWeaponEvent));
			simToPresentationEventSystem.AddHandler<WeaponTriggerEvent>(new BBI.Core.Events.EventHandler<WeaponTriggerEvent>(this.OnWeaponTriggerEvent));
			simToPresentationEventSystem.AddHandler<VisibilityChangedEvent>(new BBI.Core.Events.EventHandler<VisibilityChangedEvent>(this.OnVisibilityChanged));
			simToPresentationEventSystem.AddHandler<UnitTransferredEvent>(new BBI.Core.Events.EventHandler<UnitTransferredEvent>(this.OnUnitTransferred));
			simToPresentationEventSystem.AddHandler<StatusEffectEvent>(new BBI.Core.Events.EventHandler<StatusEffectEvent>(this.OnStatusEffectEvent));
			simToPresentationEventSystem.AddHandler<PowerShuntEvent>(new BBI.Core.Events.EventHandler<PowerShuntEvent>(this.OnPowerShuntEvent));
			simToPresentationEventSystem.AddHandler<SalvageEvent>(new BBI.Core.Events.EventHandler<SalvageEvent>(this.OnSalvageEvent));
			simToPresentationEventSystem.AddHandler<RelicEvent>(new BBI.Core.Events.EventHandler<RelicEvent>(this.OnRelicEvent));
			simToPresentationEventSystem.AddHandler<HangarOperationEvent>(new BBI.Core.Events.EventHandler<HangarOperationEvent>(this.OnHangarOperationEvent));
			simToPresentationEventSystem.AddHandler<DockingUnitOperationEvent>(new BBI.Core.Events.EventHandler<DockingUnitOperationEvent>(this.OnDockingUnitOperationEvent));
			simToPresentationEventSystem.AddHandler<QuadrantDamageEvent>(new BBI.Core.Events.EventHandler<QuadrantDamageEvent>(this.OnQuadrantDamageEvent));
			simToPresentationEventSystem.AddHandler<UnitTeleportFeedbackEvent>(new BBI.Core.Events.EventHandler<UnitTeleportFeedbackEvent>(this.OnUnitTelepotedInSimEvent));
			simToPresentationEventSystem.AddHandler<BuffChangedEvent>(new BBI.Core.Events.EventHandler<BuffChangedEvent>(this.OnBuffChangedEvent));
			simToPresentationEventSystem.AddHandler<UnitStatusChangedEvent>(new BBI.Core.Events.EventHandler<UnitStatusChangedEvent>(this.OnUnitStatusChangedEvent));
			this.mInterfaceController = interfaceCon;
			if (this.mInterfaceController == null)
			{
				Log.Error(Log.Channel.UI, "No UnitInterfaceController supplied to {0}", new object[]
				{
					base.GetType()
				});
			}
			else
			{
				this.mInterfaceController.NewSelection += this.OnSelectionChanged;
				this.mInterfaceController.NewFrameFromSimPhysics += this.OnNewFrame;
			}
			if (!sessionDependencies.Get<ICommandScheduler>(out this.mCommandScheduler))
			{
				Log.Error(Log.Channel.UI, "No ICommandScheduler found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			if (!sessionDependencies.Get<ICommanderManager>(out this.mCommanderManager))
			{
				Log.Error(Log.Channel.UI, "No ICommanderManager found in dependency container for {0}", new object[]
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
			if (!sessionDependencies.Get<SimStateFrameManager>(out this.mFrameManager))
			{
				Log.Error(Log.Channel.UI, "No SimStateFrameManager found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			if (!sessionDependencies.Get<HUDSystem>(out this.mHUDSystem))
			{
				Log.Error(Log.Channel.UI, "No HUDSystem found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			else
			{
				this.mUnitHUDInterfaceAttributes = this.mHUDSystem.GetHUDSkinAttributes<UnitHUDInteractionAttributes>();
			}
			if (this.mUnitHUDInterfaceAttributes == null)
			{
				Log.Error(Log.Channel.UI, "No UnitHUDInteractionAttributes found in HUDSystem for {0}", new object[]
				{
					base.GetType()
				});
			}
			if (ShipbreakersMain.GlobalSettingsAttributes.UnitStatusAttributes != null)
			{
				this.mCachedInCombatTweenParameters = ShipbreakersMain.GlobalSettingsAttributes.UnitStatusAttributes.InCombatIconTweenSettings;
			}
			else
			{
				Log.Error(Log.Channel.UI, "No GlobalUnitStatusAttributes found for {0}. Using default InCombatIconTweenSettings.", new object[]
				{
					base.GetType()
				});
			}
			if (!globalDependencies.Get<DLCManager>(out this.mDLCManager))
			{
				Log.Error(Log.Channel.UI, "No DLCManager found in dependency container for {0}", new object[]
				{
					base.GetType()
				});
			}
			this.mUnitParentObj = ShipbreakersMain.GetDynamicRoot(ShipbreakersMain.DynamicRootIndex.Units);
			this.mUnitLODController = new LODGroupController();
			this.mUnitLODController.Initialize(this.mHUDSystem.ActiveUserCamera, ShipbreakersMain.UnitLODSettings, "Unit LOD");
			this.ApplyNewQualitySettings(ShipbreakersMain.UserSettings.Video);
		}

		public void Dispose()
		{
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<SensorsManagerEvent>(new BBI.Core.Events.EventHandler<SensorsManagerEvent>(this.OnSensorsManagerTransition));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<UIHighlightEntityEvent>(new BBI.Core.Events.EventHandler<UIHighlightEntityEvent>(this.OnUIHighlightEntityEvent));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<QualitySettingsChangedEvent>(new BBI.Core.Events.EventHandler<QualitySettingsChangedEvent>(this.OnNewQualitySettings));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<CutsceneShotStartEvent>(new BBI.Core.Events.EventHandler<CutsceneShotStartEvent>(this.OnCutSceneStart));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<CutsceneFinishedEvent>(new BBI.Core.Events.EventHandler<CutsceneFinishedEvent>(this.OnCutSceneFinished));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<TacticalOverlayToggleEvent>(new BBI.Core.Events.EventHandler<TacticalOverlayToggleEvent>(this.OnTacticalOverlayToggleEvent));
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<PresentationSaveLoadedEvent>(new BBI.Core.Events.EventHandler<PresentationSaveLoadedEvent>(this.OnSaveLoaded));
			if (ShipbreakersMain.ReplayMode == ReplayMode.ReplayingGame)
			{
				ShipbreakersMain.PresentationEventSystem.RemoveHandler<ToggleReplayRevealAllEvent>(new BBI.Core.Events.EventHandler<ToggleReplayRevealAllEvent>(this.OnToggleReplayRevealAllEvent));
			}
			UISystem.RemoveOnSaveHandler(new UISystem.PresentationSaveCallback(this.OnSessionSaved));
			IEventReceiver simToPresentationEventSystem = ShipbreakersMain.SimToPresentationEventSystem;
			simToPresentationEventSystem.RemoveHandler<SimUnitCreatedEvent>(new BBI.Core.Events.EventHandler<SimUnitCreatedEvent>(this.OnSimUnitCreatedEvent));
			simToPresentationEventSystem.RemoveHandler<UnitViewableEvent>(new BBI.Core.Events.EventHandler<UnitViewableEvent>(this.OnUnitViewable));
			simToPresentationEventSystem.RemoveHandler<UnitRemovedFeedbackEvent>(new BBI.Core.Events.EventHandler<UnitRemovedFeedbackEvent>(this.OnUnitDespawnedInSim));
			simToPresentationEventSystem.RemoveHandler<WeaponEvent>(new BBI.Core.Events.EventHandler<WeaponEvent>(this.OnWeaponEvent));
			simToPresentationEventSystem.RemoveHandler<WeaponTriggerEvent>(new BBI.Core.Events.EventHandler<WeaponTriggerEvent>(this.OnWeaponTriggerEvent));
			simToPresentationEventSystem.RemoveHandler<VisibilityChangedEvent>(new BBI.Core.Events.EventHandler<VisibilityChangedEvent>(this.OnVisibilityChanged));
			simToPresentationEventSystem.RemoveHandler<UnitTransferredEvent>(new BBI.Core.Events.EventHandler<UnitTransferredEvent>(this.OnUnitTransferred));
			simToPresentationEventSystem.RemoveHandler<StatusEffectEvent>(new BBI.Core.Events.EventHandler<StatusEffectEvent>(this.OnStatusEffectEvent));
			simToPresentationEventSystem.RemoveHandler<PowerShuntEvent>(new BBI.Core.Events.EventHandler<PowerShuntEvent>(this.OnPowerShuntEvent));
			simToPresentationEventSystem.RemoveHandler<SalvageEvent>(new BBI.Core.Events.EventHandler<SalvageEvent>(this.OnSalvageEvent));
			simToPresentationEventSystem.RemoveHandler<RelicEvent>(new BBI.Core.Events.EventHandler<RelicEvent>(this.OnRelicEvent));
			simToPresentationEventSystem.RemoveHandler<HangarOperationEvent>(new BBI.Core.Events.EventHandler<HangarOperationEvent>(this.OnHangarOperationEvent));
			simToPresentationEventSystem.RemoveHandler<DockingUnitOperationEvent>(new BBI.Core.Events.EventHandler<DockingUnitOperationEvent>(this.OnDockingUnitOperationEvent));
			simToPresentationEventSystem.RemoveHandler<QuadrantDamageEvent>(new BBI.Core.Events.EventHandler<QuadrantDamageEvent>(this.OnQuadrantDamageEvent));
			simToPresentationEventSystem.RemoveHandler<UnitTeleportFeedbackEvent>(new BBI.Core.Events.EventHandler<UnitTeleportFeedbackEvent>(this.OnUnitTelepotedInSimEvent));
			simToPresentationEventSystem.RemoveHandler<BuffChangedEvent>(new BBI.Core.Events.EventHandler<BuffChangedEvent>(this.OnBuffChangedEvent));
			simToPresentationEventSystem.RemoveHandler<UnitStatusChangedEvent>(new BBI.Core.Events.EventHandler<UnitStatusChangedEvent>(this.OnUnitStatusChangedEvent));
			foreach (UnitView unitView in this.mAllUnits)
			{
				this.DespawnUnitView(unitView);
			}
			this.mAllUnits.Clear();
			foreach (UnitView unitView2 in this.mUnitViewsCreatedThisFrame.Values<Entity, UnitView>())
			{
				this.DespawnUnitView(unitView2);
			}
			this.mUnitViewsCreatedThisFrame.Clear();
			this.mUnitViewsRemovedThisFrame.Clear();
			this.mWaitingForFirstUnitView.Clear();
			this.mWaitingForPresentationOnlyModeRelease.Clear();
			this.mDelayedDockingOperations.Clear();
			this.mDelayedHangarOperations.Clear();
			this.mReportedDestructions.Clear();
			if (this.mCurrentSelection != null)
			{
				this.mCurrentSelection.Clear();
			}
			this.ClearAllHighlightEntities();
			if (this.mInterfaceController != null)
			{
				this.mInterfaceController.NewSelection -= this.OnSelectionChanged;
				this.mInterfaceController.NewFrameFromSimPhysics -= this.OnNewFrame;
				this.mInterfaceController = null;
			}
			this.mNLIPSDisabledByNIS = false;
			this.mCommandScheduler = null;
			this.mCommanderManager = null;
			this.mInteractionProvider = null;
			this.mFrameManager = null;
			this.mHUDSystem = null;
			this.mUnitHUDInterfaceAttributes = null;
			this.mUnitParentObj = null;
			this.mCachedInCombatTweenParameters = InCombatTweenParameters.kDefaultSettings;
			if (this.mUnitLODController != null)
			{
				this.mUnitLODController.Shutdown();
				this.mUnitLODController = null;
			}
		}

		private void ClearAllHighlightEntities()
		{
			if (this.mEntitiesWithHighlights != null)
			{
				foreach (KeyValuePair<Entity, UnitViewController.HighlightContainer> keyValuePair in this.mEntitiesWithHighlights)
				{
					Transform effectTransform = keyValuePair.Value.EffectTransform;
					if (effectTransform != null)
					{
						FXElement.Despawn(effectTransform);
					}
				}
				this.mEntitiesWithHighlights.Clear();
			}
		}

		public void FixedUpdate(float deltaTime)
		{
			foreach (UnitView unitView in this.mAllUnits)
			{
				if (!this.SkipIfNullOrRemoved(unitView))
				{
					unitView.UnitFixedUpdate(deltaTime);
				}
			}
		}

		public void Update(float deltaTime)
		{
			this.DoAddRemoveUnitsForThisFrame();
			this.DoHangarOperationsForThisFrame();
			SimStateFrame currentSimFrame = ShipbreakersMain.CurrentSimFrame;
			Profiler.BeginSample("LOD Updates");
			this.mUnitLODController.Update();
			Profiler.EndSample();
			bool nlipsEnabled = this.mNLIPSEnabled && !this.mNLIPSDisabledByNIS;
			foreach (UnitView unitView in this.mAllUnits)
			{
				if (!this.SkipIfNullOrRemoved(unitView))
				{
					Entity entityID = unitView.EntityID;
					Profiler.BeginSample("Get Unit State From Frame");
					UnitState unitState = currentSimFrame.FindObject<UnitState>(entityID);
					Profiler.EndSample();
					Profiler.BeginSample("Unit View Update loop");
					if (unitState == null)
					{
						if (!this.UnitIsDestroyedInSim(unitView.EntityID, currentSimFrame.FrameNumber))
						{
							Log.Error(Log.Channel.UI, "Unable to locate UnitState for unit {0}! Removing and despawning UnitView.", new object[]
							{
								unitView.name
							});
							this.RemoveAndDespawnUnit(unitView);
						}
					}
					else
					{
						unitView.UnitUpdate(nlipsEnabled);
					}
					Profiler.EndSample();
				}
			}
		}

		private void OnSessionSaved(ref PresentationSaveState state, bool persistenceDataOnly)
		{
			if (!persistenceDataOnly)
			{
				Dictionary<float, List<Entity>> dictionary = new Dictionary<float, List<Entity>>();
				foreach (KeyValuePair<Entity, UnitViewController.HighlightContainer> keyValuePair in this.mEntitiesWithHighlights)
				{
					List<Entity> list;
					if (!dictionary.TryGetValue(keyValuePair.Value.ScaleFactor, out list))
					{
						list = new List<Entity>();
						dictionary[keyValuePair.Value.ScaleFactor] = list;
					}
					list.Add(keyValuePair.Key);
				}
				ExtractorManager.Save<Dictionary<float, List<Entity>>, KeyValuePair<float, Entity[]>[]>(dictionary, ref state.UnitViewControllerState.HighlightedEntities);
				ExtractorManager.Save<HashSet<Entity>, Entity[]>(this.mWaitingForPresentationOnlyModeRelease, ref state.UnitViewControllerState.WaitingPresentationModeOnlyRelease);
				ExtractorManager.Save<HashSet<Entity>, Entity[]>(this.mWaitingForFirstUnitView, ref state.UnitViewControllerState.WaitingForFirstUnitView);
			}
		}

		private void OnSaveLoaded(PresentationSaveLoadedEvent ev)
		{
			if (!ev.FromPersistence)
			{
				this.ClearAllHighlightEntities();
				foreach (KeyValuePair<float, Entity[]> keyValuePair in ev.State.UnitViewControllerState.HighlightedEntities)
				{
					this.ChangeEntityHighlights(keyValuePair.Value, true, keyValuePair.Key);
				}
				ExtractorManager.Load<HashSet<Entity>, Entity[]>(ref this.mWaitingForPresentationOnlyModeRelease, ev.State.UnitViewControllerState.WaitingPresentationModeOnlyRelease);
				ExtractorManager.Load<HashSet<Entity>, Entity[]>(ref this.mWaitingForFirstUnitView, ev.State.UnitViewControllerState.WaitingForFirstUnitView);
			}
		}

		private void OnNewFrame(SimStateFrame newFrame)
		{
			Profiler.BeginSample("Apply state frame updates to units");
			foreach (UnitView unitView in this.mAllUnits)
			{
				if (!this.SkipIfNullOrRemoved(unitView))
				{
					UnitState unitState = newFrame.FindObject<UnitState>(unitView.EntityID);
					if (unitState != null)
					{
						unitView.OnNewStateFrame(unitState);
						unitView.CurrentPowerShuntState = newFrame.FindObject<PowerShuntState>(unitView.EntityID);
						UnitIconController iconControllerForView = this.GetIconControllerForView(unitView);
						if (iconControllerForView != null)
						{
							iconControllerForView.UpdateFromStates(unitState);
							iconControllerForView.UpdateIconVisibilityAndSize(newFrame);
						}
						unitView.UpdateAbilityAnimations(newFrame);
					}
				}
			}
			Profiler.EndSample();
		}

		private void OnTacticalOverlayToggleEvent(TacticalOverlayToggleEvent ev)
		{
			this.UpdateAllHUDStates();
		}

		private void OnWeaponTriggerEvent(WeaponTriggerEvent ev)
		{
			UnitView unitView = this.GetUnitView(ev.Entity);
			if (unitView != null)
			{
				unitView.OnWeaponTriggerEvent(ev);
				return;
			}
			Log.Warn(Log.Channel.UI, "Failed to find UnitView for entity {0} for WeaponTriggerEvent!", new object[]
			{
				ev.Entity.ToFriendlyString()
			});
		}

		private void OnUnitTelepotedInSimEvent(UnitTeleportFeedbackEvent ev)
		{
			if (!ev.WasRun)
			{
				return;
			}
			Vector3 position = VectorHelper.SimVector2ToUnityVector3(ev.ToPoint);
			SimStateFrame stateForFrame = this.mFrameManager.GetStateForFrame(this.mFrameManager.MostRecentFrame);
			foreach (Entity entity in ev.TargetEntityIDs)
			{
				UnitView unitView = this.GetUnitView(entity);
				if (unitView != null)
				{
					UnitState unitState = stateForFrame.FindObject<UnitState>(entity);
					Quaternion rotation = unitView.Rotation;
					if (unitState != null)
					{
						rotation = VectorHelper.SimOrientationToUnityOrientation(unitState.StartOfFrameDynamicState.Orientation);
					}
					unitView.TeleportUnitTo(position, rotation * Vector3.forward, true);
				}
				else
				{
					Log.Warn(Log.Channel.UI, "Failed to find UnitView for entity {0} for UnitTeleportFeedbackEvent!", new object[]
					{
						entity.ToFriendlyString()
					});
				}
			}
		}

		private void OnSimUnitCreatedEvent(SimUnitCreatedEvent ev)
		{
			switch (ev.State)
			{
			case SimUnitCreatedEvent.SimUnitReadyState.SpawnedInWorld:
				this.mWaitingForFirstUnitView.Add(ev.NewEntity);
				return;
			case SimUnitCreatedEvent.SimUnitReadyState.TrackedInHangar:
			case SimUnitCreatedEvent.SimUnitReadyState.UntrackedInHangar:
				ShipbreakersMain.PresentationEventSystem.Post(new PresUnitReadyEvent(ev.NewEntity, PresUnitReadyEvent.PresUnitReadyState.Stored));
				return;
			case SimUnitCreatedEvent.SimUnitReadyState.DeployedFromHangar:
				this.mWaitingForPresentationOnlyModeRelease.Add(ev.NewEntity);
				return;
			default:
				Log.Error(Log.Channel.Gameplay, "A unit was spawned in the sim with a state that the Presentation doesn't know how to deal with...  you need to add the state {0} to the OnSimUnitReadyEvent", new object[]
				{
					ev.State
				});
				return;
			}
		}

		private void OnUnitViewable(UnitViewableEvent ev)
		{
			Entity newEntity = ev.NewEntity;
			if (this.GetUnitView(newEntity) != null)
			{
				Assert.Release(false, "[RH]: The sim tried to add an entity with the same ID twice! Offending callstack follows:");
				return;
			}
			if (!newEntity.IsValid())
			{
				return;
			}
			SimStateFrame currentSimFrame = ShipbreakersMain.CurrentSimFrame;
			UnitState unitState = currentSimFrame.FindObject<UnitState>(newEntity);
			Assert.Release(unitState != null, string.Format("[RH]: Couldn't find unit {0} in state frame {1}", newEntity.ToFriendlyString(), currentSimFrame.FrameNumber));
			if (unitState != null)
			{
				UnitView unitView = this.CreateUnitView(unitState);
				if (unitView != null)
				{
					this.mUnitViewsCreatedThisFrame.Add(newEntity, unitView);
					if (this.mWaitingForFirstUnitView.Remove(newEntity))
					{
						ShipbreakersMain.PresentationEventSystem.Post(new PresUnitReadyEvent(unitView.EntityID, PresUnitReadyEvent.PresUnitReadyState.InWorld));
						return;
					}
				}
				else
				{
					Log.Error(Log.Channel.Gameplay, "Failed to spawn UnitView for unit {0}!", new object[]
					{
						newEntity.ToFriendlyString()
					});
				}
			}
		}

		private void OnUnitDespawnedInSim(UnitRemovedFeedbackEvent ev)
		{
			if (!ev.WasRun)
			{
				Log.Info(Log.Channel.UI, "Command {0} was supposed to despawn or destroy a unit but instead wasn't run. Was run against frame {1}", new object[]
				{
					ev.Command,
					ev.AgainstFrame
				});
				return;
			}
			switch (ev.Reason)
			{
			case UnitRemoveReason.Despawn:
			case UnitRemoveReason.UnitDock:
				this.OnUnitDockedOrDespawnedInSim(ev.TargetEntityID, ev.AgainstFrame);
				return;
			case UnitRemoveReason.Destroy:
				this.OnUnitDestroyed(ev.TargetEntityID, ev.SkipDeathSequence, ev.AgainstFrame);
				return;
			default:
				Debug.LogError("Unknown remove unit reason!");
				Log.Info(Log.Channel.UI, "Command {0} was supposed to despawn or destroy a unit but instead wasn't run. Was run against frame {1}", new object[]
				{
					ev.Command,
					ev.AgainstFrame
				});
				return;
			}
		}

		private void OnWeaponEvent(WeaponEvent ev)
		{
			UnitView unitView = this.GetUnitView(ev.WeaponOwner);
			if (unitView != null)
			{
				unitView.OnWeaponEvent(ev);
				return;
			}
			Log.Warn(Log.Channel.UI, "Failed to find UnitView for entity {0} for WeaponEvent!", new object[]
			{
				ev.WeaponOwner.ToFriendlyString()
			});
		}

		private void OnStatusEffectEvent(StatusEffectEvent ev)
		{
			UnitView unitView = this.GetUnitView(ev.AffectedEntity);
			if (unitView != null)
			{
				unitView.OnStatusEffectEvent(ev);
				return;
			}
			Log.Warn(Log.Channel.UI, "Failed to find UnitView for entity {0} for StatusEffectEvent!", new object[]
			{
				ev.AffectedEntity.ToFriendlyString()
			});
		}

		private void OnPowerShuntEvent(PowerShuntEvent ev)
		{
			UnitView unitView = this.GetUnitView(ev.Entity);
			if (unitView != null)
			{
				unitView.OnPowerShuntEvent(ev);
				return;
			}
			Log.Warn(Log.Channel.UI, "Failed to find UnitView for entity {0} for PowerShuntEvent!", new object[]
			{
				ev.Entity.ToFriendlyString()
			});
		}

		private void OnSalvageEvent(SalvageEvent ev)
		{
			UnitView unitView = this.GetUnitView(ev.Source);
			if (unitView != null)
			{
				unitView.OnSalvageEvent(ev);
				return;
			}
			Log.Warn(Log.Channel.UI, "Failed to find UnitView for entity {0} for SalvageEvent!", new object[]
			{
				ev.Source.ToFriendlyString()
			});
		}

		private void OnRelicEvent(RelicEvent ev)
		{
			if (ev.TriggeringEntity != Entity.None)
			{
				UnitView unitView = this.GetUnitView(ev.TriggeringEntity);
				if (unitView != null)
				{
					unitView.OnRelicEvent(ev);
					this.UpdateUnitHUDState(unitView);
				}
				else
				{
					Log.Warn(Log.Channel.UI, "Failed to find UnitView for entity {0} for RelicEvent!", new object[]
					{
						ev.TriggeringEntity.ToFriendlyString()
					});
				}
			}
			if (ev.Reason == RelicEvent.EventType.Transferred && ev.TransferredFromEntity != Entity.None)
			{
				UnitView unitView2 = this.GetUnitView(ev.TransferredFromEntity);
				if (unitView2 != null)
				{
					unitView2.OnRelicEvent(ev);
					this.UpdateUnitHUDState(unitView2);
					return;
				}
				Log.Warn(Log.Channel.UI, "Failed to find UnitView for entity {0} for RelicEvent!", new object[]
				{
					ev.TransferredFromEntity.ToFriendlyString()
				});
			}
		}

		private void OnDockingUnitOperationEvent(DockingUnitOperationEvent ev)
		{
			this.mDelayedDockingOperations.Add(ev);
		}

		private void OnQuadrantDamageEvent(QuadrantDamageEvent ev)
		{
			UnitView unitView = this.GetUnitView(ev.Entity);
			if (unitView != null)
			{
				unitView.OnQuadrantDamageEvent(ev);
				return;
			}
			Log.Warn(Log.Channel.UI, "Failed to find UnitView for entity {0} for QuadrantDamageEvent!", new object[]
			{
				ev.Entity.ToFriendlyString()
			});
		}

		private void OnHangarOperationEvent(HangarOperationEvent ev)
		{
			this.mDelayedHangarOperations.Add(ev);
		}

		private void OnVisibilityChanged(VisibilityChangedEvent ev)
		{
			DetectionState toState = ev.ToState;
			Entity entity = ev.Entity;
			UnitView unitView = this.GetUnitView(entity);
			if (unitView != null)
			{
				this.RefreshUnitViewVisibility(unitView, toState);
			}
		}

		private void OnToggleReplayRevealAllEvent(ToggleReplayRevealAllEvent ev)
		{
			foreach (UnitView unitView in this.mAllUnits)
			{
				if (unitView != null)
				{
					this.RefreshUnitViewVisibility(unitView, unitView.NonInterpolatedTickState.Visibility);
				}
			}
		}

		private void RefreshUnitViewVisibility(UnitView unitView, DetectionState detectionState)
		{
			unitView.UpdateVisibility(detectionState);
			this.UpdateUnitHUDState(unitView);
			if (detectionState != DetectionState.Sensed)
			{
				UnitIconController iconControllerForView = this.GetIconControllerForView(unitView);
				if (iconControllerForView != null)
				{
					iconControllerForView.SetHover(false, true);
					iconControllerForView.SetHover(false, false);
				}
			}
		}

		public void OnUnitTransferred(UnitTransferredEvent ev)
		{
			UnitView unitView = this.GetUnitView(ev.Entity);
			if (unitView != null)
			{
				UnitHUDInteractionAttributes.IconColourings iconColourings = this.mUnitHUDInterfaceAttributes.AllyIconColours;
				CommanderRelationship relationship = this.mInteractionProvider.GetRelationship(this.mCommanderManager.LocalCommanderID, ev.To);
				switch (relationship)
				{
				case CommanderRelationship.Self:
					iconColourings = this.mUnitHUDInterfaceAttributes.PlayerIconColours;
					break;
				case CommanderRelationship.Ally:
					iconColourings = this.mUnitHUDInterfaceAttributes.AllyIconColours;
					break;
				case CommanderRelationship.Enemy:
					iconColourings = this.mUnitHUDInterfaceAttributes.EnemyIconColours;
					break;
				}
				unitView.IconColours = iconColourings;
				UnitIconController iconControllerForView = this.GetIconControllerForView(unitView);
				if (iconControllerForView != null)
				{
					iconControllerForView.SetUnitIconColors(iconColourings, relationship);
					return;
				}
			}
			else
			{
				Log.Warn(Log.Channel.UI, "Failed to find UnitView for entity {0} for UnitTransferredEvent!", new object[]
				{
					ev.Entity.ToFriendlyString()
				});
			}
		}

		private void OnSelectionChanged(IList<Entity> newSelectionList, SimStateFrame forFrame)
		{
			Profiler.BeginSample("UnitViewController.OnSelectionChanged");
			bool flag = false;
			foreach (Entity entity in newSelectionList)
			{
				if (entity.IsValid())
				{
					UnitState unitState = forFrame.FindObject<UnitState>(entity);
					if (unitState != null && this.mCommanderManager != null)
					{
						flag = (unitState.OwnerCommander == this.mCommanderManager.LocalCommanderID);
					}
				}
				if (this.mAnySelectedUnitsLocalUnits != flag)
				{
					break;
				}
			}
			if (this.mCurrentSelection != null)
			{
				foreach (Entity entity2 in this.mCurrentSelection)
				{
					if (!newSelectionList.Contains(entity2))
					{
						UnitView unitView = this.GetUnitView(entity2);
						if (unitView != null)
						{
							unitView.Selected = false;
							this.UpdateUnitHUDState(unitView);
						}
					}
				}
				using (IEnumerator<Entity> enumerator3 = newSelectionList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Entity entity3 = enumerator3.Current;
						if (!this.mCurrentSelection.Contains(entity3))
						{
							UnitView unitView2 = this.GetUnitView(entity3);
							if (unitView2 != null)
							{
								unitView2.Selected = true;
								this.UpdateUnitHUDState(unitView2);
							}
						}
					}
					goto IL_182;
				}
			}
			foreach (Entity forEntity in newSelectionList)
			{
				UnitView unitView3 = this.GetUnitView(forEntity);
				if (unitView3 != null)
				{
					unitView3.Selected = true;
					this.UpdateUnitHUDState(unitView3);
				}
			}
			IL_182:
			if (this.mCurrentSelection == null)
			{
				this.mCurrentSelection = new List<Entity>(newSelectionList.Count);
			}
			this.mCurrentSelection.Clear();
			this.mCurrentSelection.AddRange(newSelectionList);
			if (flag != this.mAnySelectedUnitsLocalUnits)
			{
				this.mAnySelectedUnitsLocalUnits = flag;
				foreach (UnitView view in this.mAllUnits)
				{
					this.UpdateUnitHUDState(view);
				}
			}
			Profiler.EndSample();
		}

		private void OnSensorsManagerTransition(SensorsManagerEvent ev)
		{
			this.mSensorsActive = ev.Active;
			this.UpdateAllHUDStates();
		}

		private void ChangeEntityHighlights(IEnumerable<Entity> entitiesToChange, bool highlightEnabled, float scaleFactor)
		{
			foreach (Entity entity in entitiesToChange)
			{
				UnitView unitView = this.GetUnitView(entity);
				if (unitView != null)
				{
					UnitViewController.HighlightContainer highlightContainer;
					if (this.mEntitiesWithHighlights.TryGetValue(entity, out highlightContainer) && highlightContainer.EffectTransform == null)
					{
						this.mEntitiesWithHighlights.Remove(entity);
					}
					if (highlightEnabled)
					{
						if (highlightContainer.EffectTransform == null)
						{
							if (this.mUnitHUDInterfaceAttributes != null && this.mUnitHUDInterfaceAttributes.UnitHighlight != null)
							{
								Transform transform = FXElement.Spawn(this.mUnitHUDInterfaceAttributes.UnitHighlight.transform, unitView.transform.position, Quaternion.identity, unitView.transform);
								if (transform != null)
								{
									ParticleSystem componentInChildren = transform.GetComponentInChildren<ParticleSystem>();
									if (componentInChildren != null)
									{
										string typeID = unitView.NonInterpolatedTickState.TypeID;
										UnitMovementAttributes entityTypeAttributes = ShipbreakersMain.GetEntityTypeAttributes<UnitMovementAttributes>(typeID);
										if (entityTypeAttributes != null)
										{
											Fixed64 width = entityTypeAttributes.Dynamics.Width;
											Fixed64 length = entityTypeAttributes.Dynamics.Length;
											int num = Math.Max(Fixed64.IntValue(width), Fixed64.IntValue(length));
											componentInChildren.startSize = (float)num * scaleFactor;
										}
										else
										{
											Log.Error(Log.Channel.UI, "Could not get UnitMovementAtttributes from entity type {0} in OnHighlightEntityEvent in {1}. Adjusting particle system for entity type failed!", new object[]
											{
												typeID,
												base.GetType()
											});
										}
									}
									else
									{
										Log.Error(Log.Channel.UI, "Could not get particle system from entity highlight {0} in OnHighlightEntityEvent in {1}. Adjusting particle system for entity type failed!", new object[]
										{
											transform.name,
											base.GetType()
										});
									}
									this.mEntitiesWithHighlights.Add(entity, new UnitViewController.HighlightContainer
									{
										EffectTransform = transform,
										ScaleFactor = scaleFactor
									});
								}
							}
							else
							{
								Log.Error(Log.Channel.UI, "UnitHighlight is NULL in {0}. Probably not set in UnitHUDInteractionAttributes. Highlighting of entity failed!", new object[]
								{
									base.GetType()
								});
							}
						}
					}
					else
					{
						this.RemoveEntityHighlight(entity);
					}
				}
			}
		}

		private void OnUIHighlightEntityEvent(UIHighlightEntityEvent ev)
		{
			if (!ev.Entities.IsNullOrEmpty<Entity>())
			{
				this.ChangeEntityHighlights(ev.Entities, ev.Enabled, ev.ScaleFactor);
			}
		}

		private void OnUnitColliderMouseInteraction(UnitView view, bool mousedOver)
		{
			UnitIconController iconControllerForView = this.GetIconControllerForView(view);
			if (iconControllerForView != null)
			{
				iconControllerForView.SetHover(mousedOver, true);
				return;
			}
			InteractionType type = mousedOver ? InteractionType.MouseOver : InteractionType.MouseOff;
			ShipbreakersMain.PresentationEventSystem.Post(new MapObjectInteractionEvent(type, HUDSystem.CurrentButton, view));
		}

		private void OnNewQualitySettings(QualitySettingsChangedEvent ev)
		{
			this.ApplyNewQualitySettings(ev.NewSettings);
		}

		private void OnBuffChangedEvent(BuffChangedEvent ev)
		{
			if (!(ev.AffectedEntity != Entity.None))
			{
				foreach (UnitView unitView in this.mAllUnits)
				{
					if (ev.IsForEntity(unitView.EntityID))
					{
						unitView.OnBuffChangedEvent(ev);
					}
				}
				return;
			}
			UnitView unitView2 = this.GetUnitView(ev.AffectedEntity);
			if (unitView2 != null)
			{
				unitView2.OnBuffChangedEvent(ev);
				return;
			}
			Log.Warn(Log.Channel.UI, "Failed to find UnitView for entity {0} for BuffChangedEvent!", new object[]
			{
				ev.AffectedEntity.ToFriendlyString()
			});
		}

		private void OnCutSceneStart(CutsceneShotStartEvent ev)
		{
			this.mNLIPSDisabledByNIS = true;
			if (ShipbreakersMain.UserSettings.Video.NLIPSEnabled)
			{
				foreach (UnitView unitView in this.mAllUnits)
				{
					if (unitView.Visibility == DetectionState.Sensed)
					{
						unitView.RecalculateNLIPSScale(false);
						unitView.UpdateUnitMaterialPropertyBlock(false);
						unitView.ProcessAndUpdateAllFXTriggers(0f);
					}
				}
				FXManager.BroadastRequiredNLIPSUpdate();
			}
		}

		private void OnCutSceneFinished(CutsceneFinishedEvent ev)
		{
			this.mNLIPSDisabledByNIS = false;
		}

		private void OnUnitStatusChangedEvent(UnitStatusChangedEvent ev)
		{
			Profiler.BeginSample("UnitViewController.OnUnitStatusChangedEvent");
			UnitView unitView = this.GetUnitView(ev.Entity);
			if (unitView != null)
			{
				UnitIconController iconControllerForView = this.GetIconControllerForView(unitView);
				if (iconControllerForView != null)
				{
					if (ev.EnteredCombat)
					{
						iconControllerForView.EnableInCombatIconTween(this.mInCombatTweenParameters);
					}
					else if (ev.ExitedCombat)
					{
						iconControllerForView.DisableInCombatIconTween(false);
					}
				}
			}
			else
			{
				Log.Warn(Log.Channel.UI, "Failed to find UnitView for entity {0} for UnitStatusChangedEvent!", new object[]
				{
					ev.Entity.ToFriendlyString()
				});
			}
			Profiler.EndSample();
		}

		internal UnitIconController GetIconControllerForView(UnitView view)
		{
			UnitIconController result;
			this.mActiveUnitIconControllers.TryGetValue(view, out result);
			return result;
		}

		private void UpdateAllHUDStates()
		{
			foreach (UnitView unitView in this.mAllUnits)
			{
				if (!unitView.IsDriverDead)
				{
					this.UpdateUnitHUDState(unitView);
				}
			}
		}

		private void UpdateUnitHUDState(UnitView view)
		{
			if (view.PresentationControlOnly)
			{
				if (view.SensorView != null)
				{
					view.SensorView.Enabled = false;
				}
				view.UnderlayActive = false;
				view.AltitudeIndicatorActive = false;
				return;
			}
			SimStateFrame currentSimFrame = ShipbreakersMain.CurrentSimFrame;
			if (currentSimFrame == null)
			{
				return;
			}
			UnitState nonInterpolatedTickState = view.NonInterpolatedTickState;
			if (nonInterpolatedTickState == null || nonInterpolatedTickState.IsDocked)
			{
				return;
			}
			CommanderRelationship relationship = this.mInteractionProvider.GetRelationship(this.mCommanderManager.LocalCommanderID, nonInterpolatedTickState.OwnerCommander);
			bool flag = relationship != CommanderRelationship.Self;
			bool flag2 = relationship == CommanderRelationship.Enemy;
			DetectionState detectionState = nonInterpolatedTickState.Visibility;
			if (ReplayPanelController.RevealAll || SimController.sSpectatorModeEnabled)
			{
				detectionState = DetectionState.Sensed;
			}
			bool flag3 = detectionState == DetectionState.Sensed;
			bool sTacticalOverlayEnabled = TacticalOverlayController.sTacticalOverlayEnabled;
			bool underlayActive = Cursor.visible && (view.Selected || view.IsHovered) && sTacticalOverlayEnabled && !this.mSensorsActive && flag3;
			view.UnderlayActive = underlayActive;
			bool altitudeIndicatorActive = (view.Selected || this.mSensorsActive || sTacticalOverlayEnabled) && flag3;
			view.AltitudeIndicatorActive = altitudeIndicatorActive;
			if (view.SensorView != null)
			{
				bool enabled = !view.IsDriverDead && flag3 && (!flag || !flag2 || 
					(ShipbreakersMain.ReplayMode == ReplayMode.ReplayingGame || 
					mCommanderManager.GetCommanderFromID(mCommanderManager.LocalCommanderID).CommanderAttributes.Name == "SPECTATOR")
					&& MapModManager.EnableEnemySensors); // Check if the player is spectating
				view.SensorView.IsFriendly = (relationship == CommanderRelationship.Self || relationship == CommanderRelationship.Ally);

				view.SensorView.Enabled = enabled;
				view.SensorView.Mode = (this.mSensorsActive ? UnitSensorView.SensorMode.SensorsView : UnitSensorView.SensorMode.GameView);
			}
			view.ForceIconOnlyMode = (this.mSensorsActive && view.ViewAttrs.LODTunings.ForceIconOnlyInSensors);
			UnitIconController iconControllerForView = this.GetIconControllerForView(view);
			if (iconControllerForView != null)
			{
				iconControllerForView.UpdateIconVisibilityAndSize(currentSimFrame);
			}
		}

		internal UnitView GetUnitView(Entity forEntity)
		{
			UnitView result = null;
			if (!this.mUnitViewsRemovedThisFrame.ContainsKey(forEntity) && !this.mAllUnits.TryGetValue(forEntity, out result))
			{
				this.mUnitViewsCreatedThisFrame.TryGetValue(forEntity, out result);
			}
			return result;
		}

		private bool SkipIfNullOrRemoved(UnitView unitView)
		{
			if (unitView == null)
			{
				Log.Error(Log.Channel.UI, "Null UnitView being updated in UnitViewController!", new object[0]);
				return true;
			}
			return this.mUnitViewsRemovedThisFrame.ContainsKey(unitView.EntityID);
		}

		private void OnUnitDestroyed(Entity unitEntityID, bool isInstantDeath, SimFrameNumber frameNumber)
		{
			this.RemoveEntityHighlight(unitEntityID);
			SimStateFrame stateForFrame = this.mFrameManager.GetStateForFrame(frameNumber);
			if (stateForFrame != null)
			{
				UnitState unitState = stateForFrame.FindObject<UnitState>(unitEntityID);
				if (unitState != null)
				{
					if (unitState.IsDocked)
					{
						return;
					}
					UnitView unitView = this.GetUnitView(unitEntityID);
					if (unitView != null)
					{
						unitView.TriggerUnitDestruction(isInstantDeath);
					}
					if (!this.mReportedDestructions.ContainsKey(unitEntityID))
					{
						this.mReportedDestructions.Add(unitEntityID, frameNumber);
					}
				}
			}
		}

		private void OnUnitDockedOrDespawnedInSim(Entity despawnedUnit, SimFrameNumber frameNumber)
		{
			UnitView unitView = this.GetUnitView(despawnedUnit);
			if (unitView != null)
			{
				this.RemoveEntityHighlight(despawnedUnit);
				this.RemoveAndDespawnUnit(unitView);
			}
		}

		private bool UnitIsDestroyedInSim(Entity unitEntityID, SimFrameNumber presentFrameNumber)
		{
			SimFrameNumber simFrameNumber;
			return this.mReportedDestructions.TryGetValue(unitEntityID, out simFrameNumber) && simFrameNumber != null && presentFrameNumber >= simFrameNumber;
		}

		private void RemoveAndDespawnUnit(UnitView unitRemoved)
		{
			if (!this.mUnitViewsRemovedThisFrame.ContainsKey(unitRemoved.EntityID))
			{
				unitRemoved.Despawned -= this.RemoveUnitView;
				unitRemoved.OnPresentationControlOnlyChanged -= this.UpdateUnitHUDState;
				unitRemoved.MousedOver -= this.OnUnitColliderMouseInteraction;
				this.DespawnUnitView(unitRemoved);
				this.DoRemoveUnitView(unitRemoved);
			}
		}

		private void RemoveUnitView(UnitView unitRemoved)
		{
			if (!this.mUnitViewsRemovedThisFrame.ContainsKey(unitRemoved.EntityID))
			{
				unitRemoved.Despawned -= this.RemoveUnitView;
				unitRemoved.OnPresentationControlOnlyChanged -= this.UpdateUnitHUDState;
				unitRemoved.MousedOver -= this.OnUnitColliderMouseInteraction;
				this.DeallocateIconController(unitRemoved);
				this.DoRemoveUnitView(unitRemoved);
			}
		}

		private void DoHangarOperationsForThisFrame()
		{
			foreach (DockingUnitOperationEvent ev in this.mDelayedDockingOperations)
			{
				this.DoDockingUnitOperations(ev);
			}
			this.mDelayedDockingOperations.Clear();
			foreach (HangarOperationEvent ev2 in this.mDelayedHangarOperations)
			{
				this.DoHangarOperationEvent(ev2);
			}
			this.mDelayedHangarOperations.Clear();
		}

		private void DoAddRemoveUnitsForThisFrame()
		{
			if (this.mUnitViewsRemovedThisFrame.Count > 0)
			{
				foreach (UnitView unitView in this.mUnitViewsRemovedThisFrame.Values<Entity, UnitView>())
				{
					if (unitView == null)
					{
						Log.Error(Log.Channel.Gameplay, "Tried to remove a null unit view.", new object[0]);
					}
					else
					{
						this.mAllUnits.Remove(unitView);
					}
				}
				this.mUnitViewsRemovedThisFrame.Clear();
			}
			if (this.mUnitViewsCreatedThisFrame.Count > 0)
			{
				foreach (UnitView item in this.mUnitViewsCreatedThisFrame.Values<Entity, UnitView>())
				{
					this.mAllUnits.Add(item);
				}
				this.mUnitViewsCreatedThisFrame.Clear();
			}
		}

		private void DoHangarOperationEvent(HangarOperationEvent ev)
		{
			UnitView unitView = this.GetUnitView(ev.HangarOwner);
			if (unitView == null)
			{
				Log.Error(Log.Channel.Core, "[hangar] UVC -  UnitViewController > OnHangarOperationEvent  hangarOwner unitView missing! {0}", new object[]
				{
					ev.HangarOwner.ToFriendlyString()
				});
				return;
			}
			if (unitView.enabled)
			{
				unitView.OnHangarOperationEvent(ev);
				return;
			}
			Log.Error(Log.Channel.Core, "[hangar] UVC -  UnitViewController > OnHangarOperationEvent  hangarOwner unit not enabled? for unit {0}", new object[]
			{
				ev.HangarOwner.ToFriendlyString()
			});
		}

		private void DoDockingUnitOperations(DockingUnitOperationEvent ev)
		{
			UnitView unitView = this.GetUnitView(ev.HangarOwner);
			if (unitView == null)
			{
				Log.Error(Log.Channel.Core, "[hangar] UVC - Hangar Owner{0} missing: OnDockingMessage  for unit {1}", new object[]
				{
					ev.HangarOwner.ToFriendlyString(),
					ev.DockingEntity.ToFriendlyString()
				});
				return;
			}
			if (ev.DockingEntity.IsValid())
			{
				UnitView unitView2 = this.GetUnitView(ev.DockingEntity);
				if (unitView2 == null)
				{
					Log.Error(Log.Channel.Core, "Unit view tried to handle a unit docking event for an entity it doesn't have a unit for!", new object[0]);
					return;
				}
				unitView2.OnDockingUnitOperationEvent(ev);
				if (ev.Action == HangarOperationTriggers.DockingUnitTrigger.SetEntityAsSimEntity)
				{
					ShipbreakersMain.PresentationEventSystem.Post(new UnitControllableEvent(ev.DockingEntity));
					if (this.mWaitingForPresentationOnlyModeRelease.Remove(ev.DockingEntity))
					{
						ShipbreakersMain.PresentationEventSystem.Post(new PresUnitReadyEvent(ev.DockingEntity, PresUnitReadyEvent.PresUnitReadyState.InWorld));
						return;
					}
				}
			}
			else
			{
				Log.Error(Log.Channel.Core, "[hangar] UVC -  UnitViewController > OnDockingMessage  docking entity is invalid", new object[0]);
			}
		}

		private void DoRemoveUnitView(UnitView unitRemoved)
		{
			this.mReportedDestructions.Remove(unitRemoved.EntityID);
			this.mUnitViewsRemovedThisFrame.Add(unitRemoved.EntityID, unitRemoved);
		}

		private void DespawnUnitView(UnitView unitView)
		{
			this.DeallocateIconController(unitView);
			SpawnPool spawnPool;
			if (PoolManager.Pools.TryGetValue("Models", out spawnPool))
			{
				spawnPool.Despawn(unitView.transform, spawnPool.transform, unitView.transform);
				return;
			}
			unitView.gameObject.BroadcastMessage("OnDespawned", SendMessageOptions.DontRequireReceiver);
		}

		private void RemoveEntityHighlight(Entity entity)
		{
			UnitViewController.HighlightContainer highlightContainer;
			if (this.mEntitiesWithHighlights.TryGetValue(entity, out highlightContainer))
			{
				if (highlightContainer.EffectTransform != null)
				{
					FXElement.Despawn(highlightContainer.EffectTransform);
				}
				this.mEntitiesWithHighlights.Remove(entity);
			}
		}

		private UnitView CreateUnitView(UnitState initialState)
		{
			CommanderRelationship relationship = this.mInteractionProvider.GetRelationship(this.mCommanderManager.LocalCommanderID, initialState.OwnerCommander);
			UnitHUDInteractionAttributes.IconColourings iconColourings = this.mUnitHUDInterfaceAttributes.AllyIconColours;
			switch (relationship)
			{
			case CommanderRelationship.Self:
				iconColourings = this.mUnitHUDInterfaceAttributes.PlayerIconColours;
				break;
			case CommanderRelationship.Ally:
				iconColourings = this.mUnitHUDInterfaceAttributes.AllyIconColours;
				break;
			case CommanderRelationship.Enemy:
				iconColourings = this.mUnitHUDInterfaceAttributes.EnemyIconColours;
				break;
			}
			Commander commanderFromID = this.mCommanderManager.GetCommanderFromID(initialState.OwnerCommander);
			UnitHUDInteractionAttributes.PlayerUnitColours commanderColours = new UnitHUDInteractionAttributes.PlayerUnitColours(commanderFromID.UnitColors);
			UnitFactionDLCPack.UnitTexturePack kEmptyTexturePack = UnitFactionDLCPack.UnitTexturePack.kEmptyTexturePack;
			if (commanderFromID.UnitSkinPackID != DLCPackID.kInvalidID)
			{
				UnitFactionDLCPack unitFactionDLCPack = this.mDLCManager.GetDLCPackHeader(commanderFromID.UnitSkinPackID) as UnitFactionDLCPack;
				if (unitFactionDLCPack != null)
				{
					unitFactionDLCPack.SortedPackTextures.TryGetValue(initialState.TypeID, out kEmptyTexturePack);
				}
			}
			UnitView.SpawnSettings spawnSettings = new UnitView.SpawnSettings(initialState, initialState.EntityID, this.mUnitLODController, this.mHUDSystem, this.mUnitHUDInterfaceAttributes, iconColourings, commanderColours, relationship, kEmptyTexturePack);
			Transform transform = null;
			UnitViewAttributes unitViewSettings = spawnSettings.UnitViewSettings;
			if (unitViewSettings != null && unitViewSettings.ModelPrefab != null)
			{
				transform = unitViewSettings.ModelPrefab.transform;
			}
			if (transform == null)
			{
				Log.Error(Log.Channel.UI, "No unit view prefab found for unit type {0}!", new object[]
				{
					initialState.EntityID.ToFriendlyString()
				});
				return null;
			}
			UnitDynamicState startOfFrameDynamicState = initialState.StartOfFrameDynamicState;
			if (!startOfFrameDynamicState.IsValid())
			{
				Log.Error(Log.Channel.Gameplay | Log.Channel.Graphics, "Failed to find UnitDynamicState for unit {0}. Unabled to spawn UnitView!", new object[]
				{
					initialState.EntityID.ToFriendlyString()
				});
				return null;
			}
			Vector3 vector = VectorHelper.SimVector2ToUnityVector3(startOfFrameDynamicState.Position);
			Quaternion rot = Quaternion.LookRotation(Vector3.forward, Vector3.up);
			Vector3 origin = vector;
			origin.y = 10000f;
			Vector3 vector2 = VectorHelper.SimVector2ToUnityVector3(startOfFrameDynamicState.Orientation.Forward);
			RaycastHit hit;
			if (Physics.Raycast(new Ray(origin, Vector3.down), out hit, 20000f, 1 << RenderLayer.TerrainLayer))
			{
				Vector3 normalized = vector2.ProjectOntoPlane(hit.normal).normalized;
				rot = Quaternion.LookRotation(normalized, hit.normal);
			}
			else
			{
				Log.Warn(Log.Channel.Graphics, "Couldn't find terrain at ({0},{1}); failed to place unit {2} on ground", new object[]
				{
					vector.x,
					vector.z,
					transform.name
				});
				hit.point = vector;
				hit.normal = Vector3.up;
			}
			UnitView component = PoolManager.Pools["Models"].Spawn(transform, hit.point, rot, spawnSettings).GetComponent<UnitView>();
			if (component == null)
			{
				Log.Error(Log.Channel.Graphics, "Could not pool spawn a UnitView for {0}", new object[]
				{
					transform.name
				});
				return null;
			}
			component.MousedOver += this.OnUnitColliderMouseInteraction;
			component.Despawned += this.RemoveUnitView;
			component.OnPresentationControlOnlyChanged += this.UpdateUnitHUDState;
			component.OnNewStateFrame(initialState);
			component.TeleportUnitTo(hit, vector2, false);
			component.transform.parent = this.mUnitParentObj;
			UnitIconController unitIconController = this.AllocateIconControllerForUnitView(component);
			unitIconController.Initialize(component, spawnSettings.HUDAttributes, iconColourings, relationship);
			unitIconController.Hovered += this.OnUnitIconHovered;
			return component;
		}

		private void ApplyNewQualitySettings(VideoSettings newSettings)
		{
			this.mUnitLODController.SetMaximumLOD(newSettings.UnitMaxLOD);
			this.mUnitLODController.SetLODBias(newSettings.UnitLODBias);
			this.mNLIPSEnabled = newSettings.NLIPSEnabled;
		}

		private UnitIconController AllocateIconControllerForUnitView(UnitView forView)
		{
			UnitIconController unitIconController;
			if (this.mAvailableUnitIconControllers.Count > 0)
			{
				unitIconController = this.mAvailableUnitIconControllers[0];
				this.mAvailableUnitIconControllers.RemoveAt(0);
			}
			else
			{
				unitIconController = new UnitIconController(this.mHUDSystem, this.mInteractionProvider);
			}
			unitIconController.IconSpawned += this.OnIconSpawned;
			this.mActiveUnitIconControllers.Add(forView, unitIconController);
			return unitIconController;
		}

		private void DeallocateIconController(UnitView forView)
		{
			UnitIconController unitIconController;
			if (this.mActiveUnitIconControllers.TryGetValue(forView, out unitIconController))
			{
				unitIconController.Hovered -= this.OnUnitIconHovered;
				unitIconController.Shutdown();
				this.mActiveUnitIconControllers.Remove(forView);
				this.mAvailableUnitIconControllers.Add(unitIconController);
			}
		}

		private void OnUnitIconHovered(UnitIconController forController, bool hovered)
		{
			forController.View.IsHovered = hovered;
			this.UpdateUnitHUDState(forController.View);
			if (hovered)
			{
				ShipbreakersMain.PresentationEventSystem.Post(new MapObjectInteractionEvent(InteractionType.MouseOver, HUDSystem.CurrentButton, forController.View));
				return;
			}
			ShipbreakersMain.PresentationEventSystem.Post(new MapObjectInteractionEvent(InteractionType.MouseOff, HUDSystem.CurrentButton, forController.View));
		}

		private void OnIconSpawned(UnitView unitView)
		{
			ShipbreakersMain.PresentationEventSystem.Post(new UnitControllableEvent(unitView.EntityID));
			UnitIconController iconControllerForView = this.GetIconControllerForView(unitView);
			if (iconControllerForView != null)
			{
				iconControllerForView.IconSpawned -= this.OnIconSpawned;
				this.UpdateUnitHUDState(unitView);
			}
		}

		private const int kMaxUnits = 1000;

		private ICommandScheduler mCommandScheduler;

		private ICommanderManager mCommanderManager;

		private ICommanderInteractionProvider mInteractionProvider;

		private SimStateFrameManager mFrameManager;

		private HUDSystem mHUDSystem;

		private UnitInterfaceController mInterfaceController;

		private UnitHUDInteractionAttributes mUnitHUDInterfaceAttributes;

		private LODGroupController mUnitLODController;

		private DLCManager mDLCManager;

		private InCombatTweenParameters mCachedInCombatTweenParameters = InCombatTweenParameters.kDefaultSettings;

		private UnitViewCollection mAllUnits = new UnitViewCollection(1000);

		private List<UnitIconController> mAvailableUnitIconControllers = new List<UnitIconController>(100);

		private Dictionary<UnitView, UnitIconController> mActiveUnitIconControllers = new Dictionary<UnitView, UnitIconController>(100);

		private Dictionary<Entity, UnitView> mUnitViewsCreatedThisFrame = new Dictionary<Entity, UnitView>(100);

		private Dictionary<Entity, UnitView> mUnitViewsRemovedThisFrame = new Dictionary<Entity, UnitView>(100);

		private HashSet<Entity> mWaitingForFirstUnitView = new HashSet<Entity>();

		private HashSet<Entity> mWaitingForPresentationOnlyModeRelease = new HashSet<Entity>();

		private HashSet<HangarOperationEvent> mDelayedHangarOperations = new HashSet<HangarOperationEvent>();

		private HashSet<DockingUnitOperationEvent> mDelayedDockingOperations = new HashSet<DockingUnitOperationEvent>();

		private Dictionary<Entity, SimFrameNumber> mReportedDestructions = new Dictionary<Entity, SimFrameNumber>();

		private Dictionary<Entity, UnitViewController.HighlightContainer> mEntitiesWithHighlights = new Dictionary<Entity, UnitViewController.HighlightContainer>();

		private Transform mUnitParentObj;

		private bool mSensorsActive;

		private bool mAnySelectedUnitsLocalUnits;

		private bool mNLIPSEnabled = true;

		private bool mNLIPSDisabledByNIS;

		private List<Entity> mCurrentSelection;

		private struct HighlightContainer
		{
			public Transform EffectTransform;

			public float ScaleFactor;
		}
	}
}
