using System;
using System.Collections.Generic;
using BBI.Core;
using BBI.Core.ComponentModel;
using BBI.Core.Events;
using BBI.Core.Localization;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Commands;
using BBI.Game.Data;
using BBI.Game.Events;
using BBI.Game.Simulation;
using BBI.Unity.Game.Audio;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.HUD;
using BBI.Unity.Game.Localize;
using BBI.Unity.Game.Network;
using BBI.Unity.Game.World;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	public sealed class WinConditionPanelController : IDisposable
	{
		private bool IsPanelShowing
		{
			get
			{
				return this.mSettings.WinConditionPanelContainer != null && NGUITools.GetActive(this.mSettings.WinConditionPanelContainer);
			}
		}

		private bool IsMatchTimerShowing
		{
			get
			{
				return this.mSettings.MatchTimerContainer != null && NGUITools.GetActive(this.mSettings.MatchTimerContainer);
			}
		}

		public WinConditionPanelController(UnitInterfaceController unitInterfaceController, WinConditionPanelController.WinConditionPanelSettings settings, BlackbirdPanelBase.BlackbirdPanelGlobalLifetimeDependencyContainer globalDependencies, BlackbirdPanelBase.BlackbirdPanelSessionDependencyContainer sessionDependencies)
		{
			this.mSettings = settings;
			if (this.mSettings == null)
			{
				Log.Error(Log.Channel.UI, "NULL WinConditionPanelSettings instance specified for {0}", new object[]
				{
					base.GetType()
				});
			}
			else
			{
				if (this.mSettings.WinConditionPanelContainer == null)
				{
					Log.Error(Log.Channel.UI, "No WinConditionPanelContainer specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.TeamsGrid == null)
				{
					Log.Error(Log.Channel.UI, "No TeamsGrid specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.TeamIconPrefab == null)
				{
					Log.Error(Log.Channel.UI, "No TeamIconPrefab specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.LeftTeamPlayersGrid == null)
				{
					Log.Error(Log.Channel.UI, "No LeftTeamPlayersGrid specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.RightTeamPlayersGrid == null)
				{
					Log.Error(Log.Channel.UI, "No RightTeamPlayersGrid specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.LeftPlayerPrefab == null)
				{
					Log.Error(Log.Channel.UI, "No LeftPlayerPrefab specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.RightPlayerPrefab == null)
				{
					Log.Error(Log.Channel.UI, "No RightPlayerPrefab specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.FreeArtifactsGrid == null)
				{
					Log.Error(Log.Channel.UI, "No FreeArtifactsGrid specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.FreeArtifactPrefab == null)
				{
					Log.Error(Log.Channel.UI, "No FreeArtifactPrefab specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.MatchTimerContainer == null)
				{
					Log.Error(Log.Channel.UI, "No MatchTimerContainer specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.MatchTimerLabel == null)
				{
					Log.Error(Log.Channel.UI, "No MatchTimerLabel specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				else
				{
					this.mInitialTimerLabelColour = this.mSettings.MatchTimerLabel.color;
				}
				if (this.mSettings.SuddenDeathLabel == null)
				{
					Log.Error(Log.Channel.UI, "No SuddenDeathLabel specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (this.mSettings.TeamIconTooltip == null)
				{
					Log.Error(Log.Channel.UI, "No TeamIconTooltip specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (string.IsNullOrEmpty(this.mSettings.MatchTimerFormatString))
				{
					Log.Error(Log.Channel.UI, "No MatchTimerFormatString specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (string.IsNullOrEmpty(this.mSettings.TeamNameFormatStringLocID))
				{
					Log.Error(Log.Channel.UI, "No TeamNameFormatStringLocID specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (string.IsNullOrEmpty(this.mSettings.TeamPointCountFormatString))
				{
					Log.Error(Log.Channel.UI, "No TeamPointCountFormatString specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (string.IsNullOrEmpty(this.mSettings.TeamShortDescriptionStringLocID))
				{
					Log.Error(Log.Channel.UI, "No TeamShortDescriptionStringLocID specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
				if (string.IsNullOrEmpty(this.mSettings.TeamLongDescriptionStringLocID))
				{
					Log.Error(Log.Channel.UI, "No TeamLongDescriptionStringLocID specified in WinConditionPanelSettings for {0}", new object[]
					{
						base.GetType()
					});
				}
			}
			bool flag = false;
			bool flag2 = false;
			if (sessionDependencies.Get<GameStartSettings>(out this.mMPGameStartSettings))
			{
				flag = ((this.mMPGameStartSettings.VictoryConditions & VictoryConditions.Retrieval) != (VictoryConditions)0);
				flag2 = (this.mMPGameStartSettings.TeamSetting == TeamSetting.Team);
				bool flag3 = this.mMPGameStartSettings.IsMatchTimeLimitEnabled();
				this.ShowMatchTimerContainer(flag3);
				this.ShowGameClock(!flag3);
				this.mPreviousMatchTimeRemainingSeconds = (flag3 ? ((float)this.mMPGameStartSettings.GameModeSettings.MatchTimeLimitSeconds) : 0f);
				bool flag4 = this.mPreviousMatchTimeRemainingSeconds <= 0f;
				this.ShowMatchTimerLabel(flag3 && !flag4);
				this.ShowSuddenDeathLabel(flag3 && flag4);
			}
			if (!flag || !flag2)
			{
				this.ShowWinConditionPanel(false);
				return;
			}
			this.mInterfaceController = unitInterfaceController;
			if (this.mInterfaceController != null)
			{
				this.mInterfaceController.NewFrameFromSim += this.OnNewStateFrame;
			}
			else
			{
				Log.Error(Log.Channel.UI, "NULL UnitInterfaceController instance specified for {0}", new object[]
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
			if (!globalDependencies.Get<PlatformStatsUpdater>(out this.mStatsUpdater))
			{
				Log.Error(Log.Channel.Online, "NULL Stats Updater instance found in dependency container for {0}", new object[]
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
			ShipbreakersMain.SimToPresentationEventSystem.AddHandler<RelicEvent>(new BBI.Core.Events.EventHandler<RelicEvent>(this.OnRelicEvent));
			ShipbreakersMain.SimToPresentationEventSystem.AddHandler<SceneEntityCreatedEvent>(new BBI.Core.Events.EventHandler<SceneEntityCreatedEvent>(this.OnSceneEntityCreatedEvent));
			this.mTeamToSideMapping = new Dictionary<TeamID, WinConditionPanelController.TeamSide>(2);
			this.mTeamIcons = new Dictionary<TeamID, NGUIIconController>(2);
			this.mFreeArtifactIcons = new Dictionary<Entity, NGUIIconController>(3);
			this.mHeldArtifactPlayerIcons = new Dictionary<Entity, NGUIIconController>(6);
			this.RebuildTeamIcons();
		}

		public void Dispose()
		{
			ShipbreakersMain.SimToPresentationEventSystem.RemoveHandler<RelicEvent>(new BBI.Core.Events.EventHandler<RelicEvent>(this.OnRelicEvent));
			ShipbreakersMain.SimToPresentationEventSystem.RemoveHandler<SceneEntityCreatedEvent>(new BBI.Core.Events.EventHandler<SceneEntityCreatedEvent>(this.OnSceneEntityCreatedEvent));
			if (this.mInterfaceController != null)
			{
				this.mInterfaceController.NewFrameFromSim -= this.OnNewStateFrame;
				this.mInterfaceController = null;
			}
			if (this.mTeamToSideMapping != null)
			{
				this.mTeamToSideMapping.Clear();
				this.mTeamToSideMapping = null;
			}
			this.DestroyTeamIcons();
			this.mTeamIcons = null;
			this.DestroyFreeArtifactIcons();
			this.mFreeArtifactIcons = null;
			this.DestroyHeldArtifactPlayerIcons();
			this.mHeldArtifactPlayerIcons = null;
			if (this.mSettings != null)
			{
				if (this.mSettings.WinConditionPanelContainer != null)
				{
					this.ShowWinConditionPanel(false);
				}
				this.ShowMatchTimerContainer(false);
				this.ShowGameClock(false);
				if (this.mSettings.MatchTimerLabel != null)
				{
					this.mSettings.MatchTimerLabel.color = this.mInitialTimerLabelColour;
					this.mSettings.MatchTimerLabel.text = string.Empty;
				}
				this.mSettings = null;
			}
			this.mLocalizationManager = null;
			this.mHUDSystem = null;
			this.mCommanderManager = null;
			this.mMPGameStartSettings = null;
			this.mStatsUpdater = null;
			this.mLocalCommanderTeamID = TeamID.None;
			this.mPreviousMatchTimeRemainingSeconds = 0f;
		}

		private void OnNewStateFrame(SimStateFrame stateFrame)
		{
			if (stateFrame != null && this.IsPanelShowing)
			{
				if (this.IsMatchTimerShowing)
				{
					this.UpdateMatchTimer();
				}
				this.UpdateFreeArtifacts(stateFrame);
				this.UpdateHeldArtifacts(stateFrame);
			}
		}

		private void OnTeamIconTooltip(NGUIEventHandler handler, bool isHovered)
		{
			if (isHovered)
			{
				if (this.mSettings.TeamIconTooltip != null)
				{
					TeamID teamID = (TeamID)handler.Data;
					WinConditionPanelController.TeamIconTooltipData teamIconTooltipData = default(WinConditionPanelController.TeamIconTooltipData);
					teamIconTooltipData.LocalizedTitleStringID = this.mSettings.TeamNameFormatStringLocID.TokenFormat(new object[]
					{
						teamID.ID
					}, this.mLocalizationManager);
					teamIconTooltipData.LocalizedShortDescriptionStringID = this.mSettings.TeamShortDescriptionStringLocID;
					teamIconTooltipData.LocalizedLongDescriptionStringID = this.mSettings.TeamLongDescriptionStringLocID;
					this.mShowingTooltip = true;
					UISystem.ShowTooltip(new object[]
					{
						teamIconTooltipData
					}, this.mSettings.TeamIconTooltip, null);
					return;
				}
			}
			else
			{
				this.mShowingTooltip = false;
				UISystem.HideTooltip();
			}
		}

		private void OnRelicEvent(RelicEvent ev)
		{
			switch (ev.Reason)
			{
			case RelicEvent.EventType.Ready:
				this.UpdateFreeArtifactSpriteForEntity(ev.RelicEntity);
				return;
			case RelicEvent.EventType.PickedUp:
				this.RemoveFreeArtifactPlayerIcon(ev.RelicEntity);
				this.AddHeldArtifactPlayerIcon(ev.RelicEntity, ev.TriggeringEntity);
				return;
			case RelicEvent.EventType.Dropped:
				this.RemoveHeldArtifactPlayerIcon(ev.RelicEntity);
				break;
			case RelicEvent.EventType.ExtractionStarted:
			case RelicEvent.EventType.Transferred:
			case RelicEvent.EventType.AboutToExpire:
				break;
			case RelicEvent.EventType.Extracted:
				this.IncrementTeamPointCount(ev.TriggeringEntity);
				this.RemoveHeldArtifactPlayerIcon(ev.RelicEntity);
				return;
			case RelicEvent.EventType.Expired:
				this.RemoveHeldArtifactPlayerIcon(ev.RelicEntity);
				return;
			default:
				return;
			}
		}

		private void OnSceneEntityCreatedEvent(SceneEntityCreatedEvent ev)
		{
			SceneEntityDescriptor sceneEntityDescriptor = ev.SceneEntityDescriptor;
			if (sceneEntityDescriptor.EntityType == SceneEntityType.Relic)
			{
				this.AddFreeArtifactPlayerIcon(ev.Entity);
			}
		}

		private void ShowWinConditionPanel(bool show)
		{
			if (this.mSettings != null && this.mSettings.WinConditionPanelContainer != null)
			{
				NGUITools.SetActiveSelf(this.mSettings.WinConditionPanelContainer, show);
				if (!show && this.mShowingTooltip)
				{
					this.mShowingTooltip = false;
					UISystem.HideTooltip();
				}
				this.RepositionGrids();
			}
		}

		private void RepositionGrids()
		{
			this.RepositionTeamsGrid();
			this.RepositionFreeArtifactsGrid();
			this.RepositionLeftTeamGrid();
			this.RepositionRightTeamGrid();
		}

		private void RepositionTeamsGrid()
		{
			if (this.mSettings != null && this.mSettings.TeamsGrid != null)
			{
				this.mSettings.TeamsGrid.Reposition();
				this.mSettings.TeamsGrid.repositionNow = true;
			}
		}

		private void RepositionFreeArtifactsGrid()
		{
			if (this.mSettings != null && this.mSettings.FreeArtifactsGrid != null)
			{
				this.mSettings.FreeArtifactsGrid.Reposition();
				this.mSettings.FreeArtifactsGrid.repositionNow = true;
			}
		}

		private void RepositionLeftTeamGrid()
		{
			if (this.mSettings != null && this.mSettings.LeftTeamPlayersGrid != null)
			{
				this.mSettings.LeftTeamPlayersGrid.Reposition();
				this.mSettings.LeftTeamPlayersGrid.repositionNow = true;
			}
		}

		private void RepositionRightTeamGrid()
		{
			if (this.mSettings != null && this.mSettings.RightTeamPlayersGrid != null)
			{
				this.mSettings.RightTeamPlayersGrid.Reposition();
				this.mSettings.RightTeamPlayersGrid.repositionNow = true;
			}
		}

		private void AssignTeamsToSides()
		{
			if (this.mTeamToSideMapping != null && this.mMPGameStartSettings != null && this.mMPGameStartSettings.PlayerSelections != null)
			{
				TeamID teamID = TeamID.None;
				TeamID teamID2 = TeamID.None;
				foreach (KeyValuePair<CommanderID, PlayerSelection> keyValuePair in this.mMPGameStartSettings.PlayerSelections)
				{
					PlayerSelection value = keyValuePair.Value;
					if (keyValuePair.Key == this.mCommanderManager.LocalCommanderID)
					{
						teamID = value.Desc.TeamID;
						this.mLocalCommanderTeamID = teamID;
						break;
					}
				}
				foreach (KeyValuePair<CommanderID, PlayerSelection> keyValuePair2 in this.mMPGameStartSettings.PlayerSelections)
				{
					PlayerSelection value2 = keyValuePair2.Value;
					if (value2.Desc.TeamID != teamID)
					{
						teamID2 = value2.Desc.TeamID;
						break;
					}
				}
				TeamID key = teamID;
				TeamID key2 = teamID2;
				if (teamID.ID > teamID2.ID)
				{
					key = teamID2;
					key2 = teamID;
				}
				this.mTeamToSideMapping.Add(key, WinConditionPanelController.TeamSide.Left);
				this.mTeamToSideMapping.Add(key2, WinConditionPanelController.TeamSide.Right);
			}
		}

		private void RebuildTeamIcons()
		{
			if (this.mSettings != null && this.mTeamToSideMapping != null)
			{
				this.AssignTeamsToSides();
				this.DestroyTeamIcons();
				if (this.mSettings.TeamsGrid != null && this.mSettings.TeamIconPrefab != null)
				{
					foreach (KeyValuePair<TeamID, WinConditionPanelController.TeamSide> keyValuePair in this.mTeamToSideMapping)
					{
						TeamID key = keyValuePair.Key;
						if (key == TeamID.None)
						{
							Log.Error(Log.Channel.Gameplay, "Expected TeamIDs of either 1 or 2, but got {0}!", new object[]
							{
								key.ID
							});
						}
						else
						{
							NGUIIconController nguiiconController = this.mHUDSystem.SpawnIconPrefabImmediate(this.mSettings.TeamIconPrefab, null, this.mSettings.TeamsGrid.transform);
							if (nguiiconController == null)
							{
								Log.Error(Log.Channel.UI, "Failed to pool spawn icon for prefab {0}!", new object[]
								{
									this.mSettings.TeamIconPrefab
								});
							}
							else
							{
								nguiiconController.TrackPosition = false;
								nguiiconController.transform.localPosition = Vector3.zero;
								nguiiconController.name = string.Format("{0}-Team", key);
								nguiiconController.Data = key;
								nguiiconController.Visible = true;
								nguiiconController.SetValueString("Name", this.mSettings.TeamNameFormatStringLocID.TokenFormat(new object[]
								{
									key.ID
								}, this.mLocalizationManager));
								int newValue = 0;
								this.SetTeamPointCount(nguiiconController, newValue);
								nguiiconController.HoverEvent += this.OnTeamIconTooltip;
								this.mTeamIcons.Add(key, nguiiconController);
								if (this.mLocalCommanderTeamID != TeamID.None && this.mLocalCommanderTeamID != key)
								{
									nguiiconController.SetColour("Name", this.mSettings.EnemyLabelColor);
									nguiiconController.SetColour("Count", this.mSettings.EnemyLabelColor);
								}
							}
						}
					}
					this.RepositionTeamsGrid();
					bool show = this.mTeamIcons.Count > 0;
					this.ShowWinConditionPanel(show);
				}
			}
		}

		private void ShowGameClock(bool showGameClock)
		{
			if (this.mSettings != null && this.mSettings.GameClockContainer != null)
			{
				NGUITools.SetActive(this.mSettings.GameClockContainer, showGameClock);
			}
		}

		private void ShowMatchTimerContainer(bool showMatchTimer)
		{
			if (this.mSettings != null && this.mSettings.MatchTimerContainer != null)
			{
				NGUITools.SetActiveSelf(this.mSettings.MatchTimerContainer, showMatchTimer);
			}
		}

		private void ShowMatchTimerLabel(bool show)
		{
			if (this.mSettings != null && this.mSettings.MatchTimerLabel != null)
			{
				NGUITools.SetActive(this.mSettings.MatchTimerLabel.gameObject, show);
			}
		}

		private void ShowSuddenDeathLabel(bool show)
		{
			if (this.mSettings != null && this.mSettings.SuddenDeathLabel != null)
			{
				NGUITools.SetActive(this.mSettings.SuddenDeathLabel.gameObject, show);
			}
		}

		private void AddFreeArtifactPlayerIcon(Entity relicEntity)
		{
			SimStateFrame currentSimFrame = ShipbreakersMain.CurrentSimFrame;
			if (this.mSettings.FreeArtifactsGrid != null && this.mSettings.FreeArtifactPrefab != null)
			{
				NGUIIconController nguiiconController = this.mHUDSystem.SpawnIconPrefabImmediate(this.mSettings.FreeArtifactPrefab, null, this.mSettings.FreeArtifactsGrid.transform);
				if (nguiiconController == null)
				{
					Log.Error(Log.Channel.UI, "Failed to pool spawn icon for prefab {0}!", new object[]
					{
						this.mSettings.FreeArtifactPrefab
					});
					return;
				}
				nguiiconController.TrackPosition = false;
				nguiiconController.transform.localPosition = Vector3.zero;
				nguiiconController.name = string.Format("FreeArtifact-{0}", this.mSettings.FreeArtifactsGrid.transform.childCount);
				nguiiconController.Data = relicEntity;
				nguiiconController.Visible = true;
				this.UpdateFreeArtifactSprite(currentSimFrame, nguiiconController);
				this.UpdateFreeArtifactProgress(currentSimFrame, nguiiconController);
				this.mFreeArtifactIcons.Add(relicEntity, nguiiconController);
				this.RepositionFreeArtifactsGrid();
			}
		}

		private void RemoveFreeArtifactPlayerIcon(Entity relicEntity)
		{
			NGUIIconController icon;
			if (this.mFreeArtifactIcons != null && this.mFreeArtifactIcons.TryGetValue(relicEntity, out icon) && this.mHUDSystem != null)
			{
				this.mHUDSystem.RetireIcon(icon);
				this.mFreeArtifactIcons.Remove(relicEntity);
				this.RepositionFreeArtifactsGrid();
			}
		}

		private void AddHeldArtifactPlayerIcon(Entity relicEntity, Entity holderEntity)
		{
			SimStateFrame currentSimFrame = ShipbreakersMain.CurrentSimFrame;
			CommanderID commanderID = currentSimFrame.FindEntityCommanderID(holderEntity);
			CommanderState commanderState = currentSimFrame.FindCommanderState(commanderID);
			WinConditionPanelController.TeamSide teamSide;
			if (commanderState != null && this.mTeamToSideMapping.TryGetValue(commanderState.TeamID, out teamSide))
			{
				UIGrid uigrid = (teamSide == WinConditionPanelController.TeamSide.Left) ? this.mSettings.LeftTeamPlayersGrid : this.mSettings.RightTeamPlayersGrid;
				NGUIIconController nguiiconController = (teamSide == WinConditionPanelController.TeamSide.Left) ? this.mSettings.LeftPlayerPrefab : this.mSettings.RightPlayerPrefab;
				if (uigrid != null && nguiiconController != null)
				{
					NGUIIconController nguiiconController2 = this.mHUDSystem.SpawnIconPrefabImmediate(nguiiconController, null, uigrid.transform);
					if (nguiiconController2 == null)
					{
						Log.Error(Log.Channel.UI, "Failed to pool spawn icon for prefab {0}!", new object[]
						{
							nguiiconController
						});
						return;
					}
					nguiiconController2.TrackPosition = false;
					nguiiconController2.transform.localPosition = Vector3.zero;
					nguiiconController2.name = string.Format("HeldArtifactPlayer-{0}", commanderState.Name);
					nguiiconController2.Data = relicEntity;
					nguiiconController2.Visible = true;
					CommanderDirectorAttributes commanderDirectorAttributes = null;
					if (this.mCommanderManager != null)
					{
						commanderDirectorAttributes = this.mCommanderManager.GetCommanderDirectorFromID(commanderState.CommanderID);
					}
					nguiiconController2.SetValueString("PlayerLabel", (commanderDirectorAttributes != null) ? SharedLocIDConstants.GetLocalizedCommanderName(commanderDirectorAttributes.PlayerType, commanderState.Name, commanderDirectorAttributes.PlayerID, commanderDirectorAttributes.AIDifficulty) : commanderState.Name);
					nguiiconController2.SetShowing("PlayerLabel", true);
					nguiiconController2.SetShowing("RetrievalSprite", true);
					this.UpdateHeldArtifact(currentSimFrame, nguiiconController2);
					this.mHeldArtifactPlayerIcons.Add(relicEntity, nguiiconController2);
					if (teamSide == WinConditionPanelController.TeamSide.Left)
					{
						this.RepositionLeftTeamGrid();
						return;
					}
					this.RepositionRightTeamGrid();
				}
			}
		}

		private void RemoveHeldArtifactPlayerIcon(Entity relicEntity)
		{
			NGUIIconController icon;
			if (this.mHeldArtifactPlayerIcons != null && this.mHeldArtifactPlayerIcons.TryGetValue(relicEntity, out icon) && this.mHUDSystem != null)
			{
				this.mHUDSystem.RetireIcon(icon);
				this.mHeldArtifactPlayerIcons.Remove(relicEntity);
				this.RepositionLeftTeamGrid();
				this.RepositionRightTeamGrid();
			}
		}

		private void DestroyTeamIcons()
		{
			if (this.mTeamIcons != null)
			{
				if (this.mHUDSystem != null)
				{
					foreach (KeyValuePair<TeamID, NGUIIconController> keyValuePair in this.mTeamIcons)
					{
						NGUIIconController value = keyValuePair.Value;
						if (value != null)
						{
							this.mHUDSystem.RetireIcon(value);
						}
					}
				}
				this.mTeamIcons.Clear();
			}
		}

		private void DestroyFreeArtifactIcons()
		{
			if (this.mFreeArtifactIcons != null)
			{
				if (this.mHUDSystem != null)
				{
					foreach (KeyValuePair<Entity, NGUIIconController> keyValuePair in this.mFreeArtifactIcons)
					{
						NGUIIconController value = keyValuePair.Value;
						if (value != null)
						{
							this.mHUDSystem.RetireIcon(value);
						}
					}
				}
				this.mFreeArtifactIcons.Clear();
			}
		}

		private void DestroyHeldArtifactPlayerIcons()
		{
			if (this.mHeldArtifactPlayerIcons != null)
			{
				if (this.mHUDSystem != null)
				{
					foreach (KeyValuePair<Entity, NGUIIconController> keyValuePair in this.mHeldArtifactPlayerIcons)
					{
						NGUIIconController value = keyValuePair.Value;
						if (value != null)
						{
							this.mHUDSystem.RetireIcon(value);
						}
					}
				}
				this.mHeldArtifactPlayerIcons.Clear();
			}
		}

		private void SetTeamPointCount(NGUIIconController icon, int newValue)
		{
			if (icon != null)
			{
				icon.SetValueInt("Count", newValue);
				if (this.mMPGameStartSettings.IsWinCountEnabled())
				{
					int winCount = this.mMPGameStartSettings.GameModeSettings.Retrieval.WinCount;
					icon.SetValueString("Count", string.Format(this.mSettings.TeamPointCountFormatString, newValue, winCount));
					return;
				}
				icon.SetValueString("Count", string.Format("{0}", newValue));
			}
		}

		private void IncrementTeamPointCount(Entity extractingEntity)
		{
			if (this.mTeamIcons != null)
			{
				SimStateFrame currentSimFrame = ShipbreakersMain.CurrentSimFrame;
				CommanderID commanderID = currentSimFrame.FindEntityCommanderID(extractingEntity);
				CommanderState commanderState = currentSimFrame.FindCommanderState(commanderID);
				NGUIIconController nguiiconController;
				if (commanderState != null && this.mTeamIcons.TryGetValue(commanderState.TeamID, out nguiiconController))
				{
					int newValue = nguiiconController.GetIntValue("Count") + 1;
					this.SetTeamPointCount(nguiiconController, newValue);
				}
				if (this.mCommanderManager.LocalCommanderID == commanderID)
				{
					this.mStatsUpdater.UpdateArtifactRetrievedCount(commanderState);
				}
			}
		}

		private void UpdateMatchTimer()
		{
			if (this.mSettings != null && this.mSettings.MatchTimerLabel != null && this.mPreviousMatchTimeRemainingSeconds > 0f)
			{
				float num = Fixed64.UnsafeFloatValue(ShipbreakersMain.SimTimeSinceStartUp);
				float num2 = (float)this.mMPGameStartSettings.GameModeSettings.MatchTimeLimitSeconds;
				float num3 = num2 - num;
				if ((this.mMPGameStartSettings.VictoryConditions & VictoryConditions.Retrieval) != (VictoryConditions)0)
				{
					this.EvaluateTimeRemainingVO(num3, this.mPreviousMatchTimeRemainingSeconds);
				}
				if (num3 > 0f)
				{
					TimeSpan timeSpan = TimeSpan.FromSeconds((double)num3);
					string text = string.Format(this.mSettings.MatchTimerFormatString, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
					this.mSettings.MatchTimerLabel.text = text;
					Color color;
					if (this.TryGetMatchTimerColour(Mathf.Floor(num3), out color))
					{
						this.mSettings.MatchTimerLabel.color = color;
					}
				}
				else
				{
					this.ShowMatchTimerLabel(false);
					this.ShowSuddenDeathLabel(true);
				}
				this.mPreviousMatchTimeRemainingSeconds = num3;
			}
		}

		private bool TryGetMatchTimerColour(float matchTimeRemainingSeconds, out Color colour)
		{
			colour = Color.white;
			if (ShipbreakersMain.GlobalSettingsAttributes.GameSettings != null)
			{
				GameModeSettingsAttributesData gameModeSettingsAttributesData = ShipbreakersMain.GlobalSettingsAttributes.GameSettings as GameModeSettingsAttributesData;
				if (gameModeSettingsAttributesData != null && gameModeSettingsAttributesData.MatchTimeLimitThresholds != null)
				{
					for (int i = 0; i < gameModeSettingsAttributesData.MatchTimeLimitThresholds.Length; i++)
					{
						MatchTimeLimitThreshold matchTimeLimitThreshold = gameModeSettingsAttributesData.MatchTimeLimitThresholds[i];
						if (matchTimeRemainingSeconds <= matchTimeLimitThreshold.MatchTimeRemainingSecondsThreshold)
						{
							colour = matchTimeLimitThreshold.Colour;
							return true;
						}
					}
				}
			}
			return false;
		}

		private void UpdateFreeArtifacts(SimStateFrame stateFrame)
		{
			if (this.mFreeArtifactIcons != null)
			{
				foreach (KeyValuePair<Entity, NGUIIconController> keyValuePair in this.mFreeArtifactIcons)
				{
					this.UpdateFreeArtifactProgress(stateFrame, keyValuePair.Value);
				}
			}
		}

		private void UpdateFreeArtifactSpriteForEntity(Entity entity)
		{
			if (this.mFreeArtifactIcons != null)
			{
				SimStateFrame currentSimFrame = ShipbreakersMain.CurrentSimFrame;
				NGUIIconController icon;
				if (this.mFreeArtifactIcons.TryGetValue(entity, out icon))
				{
					this.UpdateFreeArtifactSprite(currentSimFrame, icon);
				}
			}
		}

		private void UpdateFreeArtifactSprite(SimStateFrame stateFrame, NGUIIconController icon)
		{
			if (stateFrame != null && icon != null)
			{
				Entity forEntity = (Entity)icon.Data;
				RelicState relicState = stateFrame.FindObject<RelicState>(forEntity);
				if (relicState != null && !relicState.IsDocked)
				{
					CollectibleState collectibleState = stateFrame.FindObject<CollectibleState>(relicState.EntityID);
					if (collectibleState != null && collectibleState.AvailableForCollection)
					{
						if (!relicState.HasTimer || (relicState.TimerDirection == TimerDirection.Countup && relicState.TimerCompleted))
						{
							icon.SetShowing("ReadySprite", true);
							icon.SetShowing("SpawningSprite", false);
							icon.SetShowing("ProgressBar", false);
							return;
						}
						if (relicState.HasTimer && relicState.TimerDirection == TimerDirection.Countup && !relicState.TimerCompleted)
						{
							icon.SetShowing("ReadySprite", false);
							icon.SetShowing("SpawningSprite", true);
							icon.SetShowing("ProgressBar", true);
						}
					}
				}
			}
		}

		private void UpdateFreeArtifactProgress(SimStateFrame stateFrame, NGUIIconController icon)
		{
			if (stateFrame != null && icon != null)
			{
				Entity forEntity = (Entity)icon.Data;
				RelicState relicState = stateFrame.FindObject<RelicState>(forEntity);
				if (relicState != null && !relicState.IsDocked)
				{
					CollectibleState collectibleState = stateFrame.FindObject<CollectibleState>(relicState.EntityID);
					if (collectibleState != null && collectibleState.AvailableForCollection && relicState.HasTimer && relicState.TimerDirection == TimerDirection.Countup && !relicState.TimerCompleted)
					{
						float value = Fixed64.UnsafeFloatValue(relicState.CurrentTimePercentage01);
						icon.SetValueFloat("ProgressBar", value);
					}
				}
			}
		}

		private void UpdateHeldArtifacts(SimStateFrame stateFrame)
		{
			if (this.mHeldArtifactPlayerIcons != null)
			{
				foreach (KeyValuePair<Entity, NGUIIconController> keyValuePair in this.mHeldArtifactPlayerIcons)
				{
					this.UpdateHeldArtifact(stateFrame, keyValuePair.Value);
				}
			}
		}

		private void UpdateHeldArtifact(SimStateFrame stateFrame, NGUIIconController icon)
		{
			if (stateFrame != null && icon != null)
			{
				Entity forEntity = (Entity)icon.Data;
				RelicState relicState = stateFrame.FindObject<RelicState>(forEntity);
				if (relicState != null)
				{
					icon.SetShowing("ExpirationLabel", relicState.AboutToExpire);
					if (relicState.AboutToExpire)
					{
						float value = Fixed64.UnsafeFloatValue(relicState.CurrentTimeSeconds);
						icon.SetValueFloat("ExpirationLabel", value);
					}
					float num = Fixed64.UnsafeFloatValue(relicState.ExtractionCompletion01);
					bool flag = num > 0f && num <= 1f;
					icon.SetShowing("ExtractionProgressBar", flag);
					if (flag)
					{
						icon.SetValueFloat("ExtractionProgressBar", num);
					}
				}
			}
		}

		private void EvaluateTimeRemainingVO(float currentMatchTimeRemaining, float previousMatchTimeRemaining)
		{
			VOController.FieldIdentifiers.TimeRemainingTuple[] timeTuples = ShipbreakersMain.VOController.Fields.TimeRemaining.TimeTuples;
			if (timeTuples.IsNullOrEmpty<VOController.FieldIdentifiers.TimeRemainingTuple>())
			{
				return;
			}
			foreach (VOController.FieldIdentifiers.TimeRemainingTuple timeRemainingTuple in timeTuples)
			{
				if (previousMatchTimeRemaining > (float)timeRemainingTuple.SecondsLeft && currentMatchTimeRemaining <= (float)timeRemainingTuple.SecondsLeft)
				{
					DialogueTunable timeRemaining = ShipbreakersMain.VOController.Commands.TimeRemaining;
					ShipbreakersMain.PresentationEventSystem.Post(new VoiceOverEvent(timeRemaining.Priority, timeRemaining.Command, timeRemaining.Group, null, ShipbreakersMain.VOController.Voices.OpsFleet, null, timeRemainingTuple.TimeField, null, ShipbreakersMain.VOController.Fields.Intensity.Calm));
				}
			}
		}

		private const string kTeamNameLabelVariableName = "Name";

		private const string kTeamCountLabelVariableName = "Count";

		private const string kFreeArtifactProgressBarVariableName = "ProgressBar";

		private const string kFreeArtifactReadySpriteVariableName = "ReadySprite";

		private const string kFreeArtifactSpawningSpriteVariableName = "SpawningSprite";

		private const string kHeldArtifactPlayerLabelVariableName = "PlayerLabel";

		private const string kHeldArtifactPlayerExpirationLabelVariableName = "ExpirationLabel";

		private const string kHeldArtifactPlayerExtractionProgressBarVariableName = "ExtractionProgressBar";

		private const string kHeldArtifactPlayerRetrievalSpriteVariableName = "RetrievalSprite";

		private WinConditionPanelController.WinConditionPanelSettings mSettings;

		private UnitInterfaceController mInterfaceController;

		private HUDSystem mHUDSystem;

		private IGameLocalization mLocalizationManager;

		private ICommanderManager mCommanderManager;

		private GameStartSettings mMPGameStartSettings;

		private PlatformStatsUpdater mStatsUpdater;

		private Dictionary<TeamID, WinConditionPanelController.TeamSide> mTeamToSideMapping;

		private Dictionary<TeamID, NGUIIconController> mTeamIcons;

		private Dictionary<Entity, NGUIIconController> mFreeArtifactIcons;

		private Dictionary<Entity, NGUIIconController> mHeldArtifactPlayerIcons;

		private Color mInitialTimerLabelColour;

		private float mPreviousMatchTimeRemainingSeconds;

		private bool mShowingTooltip;

		private TeamID mLocalCommanderTeamID = TeamID.None;

		public enum TeamSide
		{
			Left,
			Right
		}

		public struct TeamIconTooltipData
		{
			public string LocalizedTitleStringID;

			public string LocalizedShortDescriptionStringID;

			public string LocalizedLongDescriptionStringID;
		}

		[Serializable]
		public class WinConditionPanelSettings
		{
			public GameObject WinConditionPanelContainer
			{
				get
				{
					return this.m_WinConditionPanelContainer;
				}
			}

			public UIGrid TeamsGrid
			{
				get
				{
					return this.m_WinConditionTeamsGrid;
				}
			}

			public UIGrid LeftTeamPlayersGrid
			{
				get
				{
					return this.m_LeftTeamPlayersGrid;
				}
			}

			public UIGrid RightTeamPlayersGrid
			{
				get
				{
					return this.m_RightTeamPlayersGrid;
				}
			}

			public UIGrid FreeArtifactsGrid
			{
				get
				{
					return this.m_FreeArtifactsGrid;
				}
			}

			public GameObject MatchTimerContainer
			{
				get
				{
					return this.m_MatchTimerContainer;
				}
			}

			public GameObject GameClockContainer
			{
				get
				{
					return this.m_GameClockContainer;
				}
			}

			public UILabel MatchTimerLabel
			{
				get
				{
					return this.m_MatchTimerLabel;
				}
			}

			public UILabel SuddenDeathLabel
			{
				get
				{
					return this.m_SuddenDeathLabel;
				}
			}

			public NGUIIconController TeamIconPrefab
			{
				get
				{
					return this.m_WinConditionTeamIconPrefab;
				}
			}

			public NGUIIconController FreeArtifactPrefab
			{
				get
				{
					return this.m_FreeArtifactPrefab;
				}
			}

			public NGUIIconController LeftPlayerPrefab
			{
				get
				{
					return this.m_LeftPlayerPrefab;
				}
			}

			public NGUIIconController RightPlayerPrefab
			{
				get
				{
					return this.m_RightPlayerPrefab;
				}
			}

			public TooltipObjectAsset TeamIconTooltip
			{
				get
				{
					return this.m_TeamIconTooltip;
				}
			}

			public string MatchTimerFormatString
			{
				get
				{
					return this.m_MatchTimerFormatString;
				}
			}

			public string TeamNameFormatStringLocID
			{
				get
				{
					return this.m_TeamNameFormatStringLocID;
				}
			}

			public string TeamPointCountFormatString
			{
				get
				{
					return this.m_TeamPointCountFormatString;
				}
			}

			public string TeamShortDescriptionStringLocID
			{
				get
				{
					return this.m_ShortDescriptionStringLocID;
				}
			}

			public string TeamLongDescriptionStringLocID
			{
				get
				{
					return this.m_LongDescriptionStringLocID;
				}
			}

			public Color EnemyLabelColor
			{
				get
				{
					return this.m_EnemyLabelColor;
				}
			}

			public WinConditionPanelSettings()
			{
			}

			[Header("Scene instances")]
			[SerializeField]
			private GameObject m_WinConditionPanelContainer;

			[SerializeField]
			private UIGrid m_WinConditionTeamsGrid;

			[SerializeField]
			private UIGrid m_LeftTeamPlayersGrid;

			[SerializeField]
			private UIGrid m_RightTeamPlayersGrid;

			[SerializeField]
			private UIGrid m_FreeArtifactsGrid;

			[SerializeField]
			private GameObject m_MatchTimerContainer;

			[SerializeField]
			private UILabel m_MatchTimerLabel;

			[SerializeField]
			private UILabel m_SuddenDeathLabel;

			[SerializeField]
			[Tooltip("Add a link to the game clock so it can be hidden when the match timer is active.")]
			private GameObject m_GameClockContainer;

			[SerializeField]
			[Header("Prefabs")]
			private NGUIIconController m_WinConditionTeamIconPrefab;

			[SerializeField]
			private NGUIIconController m_FreeArtifactPrefab;

			[SerializeField]
			private NGUIIconController m_LeftPlayerPrefab;

			[SerializeField]
			private NGUIIconController m_RightPlayerPrefab;

			[SerializeField]
			private TooltipObjectAsset m_TeamIconTooltip;

			[SerializeField]
			private string m_MatchTimerFormatString = "{0:00}:{1:00}";

			[SerializeField]
			private string m_TeamNameFormatStringLocID = "ID_WINCONDITIONPANEL_TEAMNAME";

			[SerializeField]
			private string m_TeamPointCountFormatString = "{0}/{1}";

			[SerializeField]
			private string m_ShortDescriptionStringLocID = "ID_WINCONDITIONPANEL_SHORTDESCRIPTION";

			[SerializeField]
			private string m_LongDescriptionStringLocID = "ID_WINCONDITIONPANEL_LONGDESCRIPTION";

			[SerializeField]
			private Color m_EnemyLabelColor = Color.red;
		}
	}
}
