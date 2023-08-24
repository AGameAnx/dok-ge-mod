using System;
using System.Collections.Generic;
using BBI.Core;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Localization;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Commands;
using BBI.Game.Data;
using BBI.Game.Events;
using BBI.Game.Simulation;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.HUD;
using BBI.Unity.Game.Localize;
using BBI.Unity.Game.World;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	// Token: 0x020001DA RID: 474
	public class ExtractionZoneViewController : IDisposable
	{
		// Token: 0x06000BBA RID: 3002 RVA: 0x00037BFC File Offset: 0x00035DFC
		public ExtractionZoneViewController(UnitInterfaceController unitInterfaceController, ExtractionZoneViewController.ExtractionZoneViewControllerSettings settings, BlackbirdPanelBase.BlackbirdPanelGlobalLifetimeDependencyContainer globalDependencies, BlackbirdPanelBase.BlackbirdPanelSessionDependencyContainer sessionDependencies)
		{
			this.mSettings = settings;
			if (this.mSettings == null)
			{
				Log.Error(Log.Channel.UI, "No ExtractionZoneViewControllerSettings supplied to {0}", new object[]
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
			if (sessionDependencies.Get<HUDSystem>(out this.mHUDSystem))
			{
				this.mUnitHUDInterfaceAttributes = this.mHUDSystem.GetHUDSkinAttributes<UnitHUDInteractionAttributes>();
				if (this.mUnitHUDInterfaceAttributes == null)
				{
					Log.Error(Log.Channel.UI, "No UnitHUDInteractionAttributes found in HUDSystem for {0}", new object[]
					{
						base.GetType()
					});
				}
			}
			else
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

		// Token: 0x06000BBB RID: 3003 RVA: 0x00037D48 File Offset: 0x00035F48
		public void Dispose()
		{
			foreach (KeyValuePair<Entity, ExtractionZoneView> keyValuePair in this.mExtractionZoneViews)
			{
				ExtractionZoneView value = keyValuePair.Value;
				if (this.mHUDSystem != null)
				{
					value.DespawnArrowIcon(this.mHUDSystem);
				}
				if (value.Circle != null)
				{
					UISystem.DeactivateAndDestroy(value.Circle.gameObject);
				}
			}
			this.mExtractionZoneViews.Clear();
			this.mHUDSystem = null;
			this.mSettings = null;
			this.mCommanderManager = null;
			this.mInteractionProvider = null;
			this.mLocalizationManager = null;
			this.mUnitHUDInterfaceAttributes = null;
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x00037E04 File Offset: 0x00036004
		public void OnSceneEntityCreated(SceneEntityCreatedEvent ev)
		{
			Entity entity = ev.Entity;
			SceneEntityDescriptor sceneEntityDescriptor = ev.SceneEntityDescriptor;
			this.CreateExtractionZoneEntityView(entity, (ExtractionZoneDescriptor)sceneEntityDescriptor);
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x00037E2C File Offset: 0x0003602C
		private void CreateExtractionZoneEntityView(Entity entity, ExtractionZoneDescriptor descriptor)
		{
			Assert.Release(!this.mExtractionZoneViews.ContainsKey(entity), "[RH]: Trying to create an ExtractionZoneView for entity but one already exists!");
			ExtractionZoneAttributes extractionZoneAttribs = descriptor.ExtractionZoneAttribs;
			if (extractionZoneAttribs == null)
			{
				Log.Error(Log.Channel.UI, "Missing ExtractionZoneAttributes for extraction zone entity {0}", new object[]
				{
					entity.ToFriendlyString()
				});
				return;
			}
			int num = Fixed64.IntValue(extractionZoneAttribs.RadiusMeters);
			if (num <= 0)
			{
				Log.Error(Log.Channel.UI, "Attempting to create an ExtractionZoneView for extraction zone entity {0} with non-positive radius {1}!", new object[]
				{
					entity.ToFriendlyString(),
					num
				});
				return;
			}
			SceneEntityHUDAttributes typeAttributes = entity.GetTypeAttributes<SceneEntityHUDAttributes>();
			if (typeAttributes == null)
			{
				Log.Error(Log.Channel.UI, "Missing SceneEntityHUDAttributes for extraction zone entity {0}", new object[]
				{
					entity.ToFriendlyString()
				});
				return;
			}
			if (typeAttributes.CirclePrefab != null && typeAttributes.ArrowPrefab != null)
			{
				Vector3 vector = VectorHelper.SimVector2ToUnityVector3(descriptor.Position);
				vector = UnitView.RaycastToGroundFromPoint(vector);
				Color colour;
				TeamID teamID;
				if (!this.DetermineColorAndTeamIDForExtractionZone(entity, out colour, out teamID))
				{
					Log.Error(Log.Channel.Gameplay, "Failed to determine colour and TeamID for extraction zone entity {0}! Using defaults.", new object[]
					{
						entity.ToFriendlyString()
					});
				}
				UISprite circle = this.CreateExtractionZoneCircle(entity, typeAttributes.CirclePrefab, vector, num, colour);
				string label = (!string.IsNullOrEmpty(typeAttributes.LocalizedTitleStringID)) ? typeAttributes.LocalizedTitleStringID.TokenFormat(new object[]
				{
					teamID.ID
				}, this.mLocalizationManager) : string.Empty;
				ExtractionZoneView extractionZoneView = new ExtractionZoneView(entity, descriptor, circle, typeAttributes.ArrowPrefab, this.mSettings.MarkerAltitudeOffset, label, this.mHUDSystem, colour);
				extractionZoneView.Show(true);
				this.mExtractionZoneViews.Add(entity, extractionZoneView);
				return;
			}
			Log.Error(Log.Channel.UI, "Missing circle or arrow prefab for extraction zone scene entity {0}!", new object[]
			{
				entity.ToFriendlyString()
			});
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x00038010 File Offset: 0x00036210
		private UISprite CreateExtractionZoneCircle(Entity entity, GameObject iconPrefab, Vector3 targetPos, int radius, Color colour)
		{
			UISprite uisprite = null;
			Transform dynamicRoot = ShipbreakersMain.GetDynamicRoot(ShipbreakersMain.DynamicRootIndex.Misc);
			if (iconPrefab != null && dynamicRoot != null)
			{
				GameObject gameObject = NGUITools.AddChild(dynamicRoot.gameObject, iconPrefab);
				NGUITools.SetActive(gameObject, false);
				uisprite = gameObject.GetComponent<UISprite>();
				if (uisprite != null)
				{
					uisprite.height = radius * 2;
					uisprite.width = radius * 2;
					uisprite.transform.eulerAngles = new Vector3(90f, this.mHUDSystem.ActiveUserCamera.transform.eulerAngles.y, 0f);
					uisprite.transform.position = targetPos;
					uisprite.color = colour;
				}
				else
				{
					Log.Error(Log.Channel.UI, "Missing UISprite on circle prefab {0} for extraction zone entity {1}", new object[]
					{
						iconPrefab,
						entity.ToFriendlyString()
					});
				}
			}
			else
			{
				Log.Error(Log.Channel.UI, "Circle marker icon prefab or dynamic root for {0} is null for extraction zone entity {1}!", new object[]
				{
					entity.ToFriendlyString(),
					ShipbreakersMain.DynamicRootIndex.Misc
				});
			}
			return uisprite;
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00038114 File Offset: 0x00036314
		private void ShowExtractionZone(Entity entity, bool show)
		{
			ExtractionZoneView extractionZoneView;
			if (this.mExtractionZoneViews != null && this.mExtractionZoneViews.TryGetValue(entity, out extractionZoneView))
			{
				extractionZoneView.Show(show);
			}
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00038140 File Offset: 0x00036340
		private List<CommanderID> GetCommanderIDsForExtractionZoneEntity(Entity zoneEntity)
		{
			List<CommanderID> list = new List<CommanderID>(3);
			SimStateFrame currentSimFrame = ShipbreakersMain.CurrentSimFrame;
			foreach (CommanderState commanderState in currentSimFrame.FindAllOftype<CommanderState>())
			{
				if (commanderState.ExtractionZone == zoneEntity)
				{
					list.Add(commanderState.CommanderID);
				}
			}
			return list;
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x000381B0 File Offset: 0x000363B0
		private bool DetermineColorAndTeamIDForExtractionZone(Entity zoneEntity, out Color colour, out TeamID teamID)
		{
			colour = Color.white;
			List<CommanderID> commanderIDsForExtractionZoneEntity = this.GetCommanderIDsForExtractionZoneEntity(zoneEntity);
			if (!commanderIDsForExtractionZoneEntity.IsNullOrEmpty<CommanderID>() && this.mCommanderManager != null)
			{
				bool flag = true;
				CommanderID commanderID = commanderIDsForExtractionZoneEntity.Contains(this.mCommanderManager.LocalCommanderID) ? this.mCommanderManager.LocalCommanderID : commanderIDsForExtractionZoneEntity[0];
				Commander commanderFromID = this.mCommanderManager.GetCommanderFromID(commanderID);
				if (commanderFromID != null)
				{
					if (flag && this.mUnitHUDInterfaceAttributes.ExtractionZone != null)
					{
						switch (this.mInteractionProvider.GetRelationship(this.mCommanderManager.LocalCommanderID, commanderID))
						{
						case CommanderRelationship.Self:
						case CommanderRelationship.Ally:
							colour = this.mUnitHUDInterfaceAttributes.ExtractionZone.FriendlyColour;
							break;
						case CommanderRelationship.Enemy:
							colour = this.mUnitHUDInterfaceAttributes.ExtractionZone.EnemyColour;
							break;
						}
					}
					else
					{
						UnitHUDInteractionAttributes.PlayerUnitColours playerUnitColours = new UnitHUDInteractionAttributes.PlayerUnitColours(commanderFromID.UnitColors);
						if (playerUnitColours != null)
						{
							colour = playerUnitColours.BaseColour;
						}
					}
					teamID = commanderFromID.TeamID;
					return true;
				}
			}
			teamID = TeamID.None;
			return false;
		}

		// Token: 0x04000ABB RID: 2747
		private const bool kUsePlayerRelationshipColours = true;

		// Token: 0x04000ABC RID: 2748
		private ExtractionZoneViewController.ExtractionZoneViewControllerSettings mSettings;

		// Token: 0x04000ABD RID: 2749
		private HUDSystem mHUDSystem;

		// Token: 0x04000ABE RID: 2750
		private ICommanderManager mCommanderManager;

		// Token: 0x04000ABF RID: 2751
		private ICommanderInteractionProvider mInteractionProvider;

		// Token: 0x04000AC0 RID: 2752
		private IGameLocalization mLocalizationManager;

		// Token: 0x04000AC1 RID: 2753
		private UnitHUDInteractionAttributes mUnitHUDInterfaceAttributes;

		// Token: 0x04000AC2 RID: 2754
		private Dictionary<Entity, ExtractionZoneView> mExtractionZoneViews = new Dictionary<Entity, ExtractionZoneView>(2);

		// Token: 0x020001DB RID: 475
		[Serializable]
		public class ExtractionZoneViewControllerSettings
		{
			// Token: 0x17000203 RID: 515
			// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x000382D9 File Offset: 0x000364D9
			public float MarkerAltitudeOffset
			{
				get
				{
					return this.m_MarkerAltitudeOffset;
				}
			}

			// Token: 0x06000BC3 RID: 3011 RVA: 0x000382E1 File Offset: 0x000364E1
			public ExtractionZoneViewControllerSettings()
			{
			}

			// Token: 0x04000AC3 RID: 2755
			[SerializeField]
			private float m_MarkerAltitudeOffset = 100f;
		}
	}
}
