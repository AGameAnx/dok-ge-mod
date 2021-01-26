using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Localization;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
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
	public partial class ExtractionZoneViewController : IDisposable
	{
		// Token: 0x06000B1A RID: 2842 RVA: 0x0003E800 File Offset: 0x0003CA00
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
					// MOD: Fix for incorrect colors of extraction zones
					if (this.mCommanderManager.GetCommanderFromID(this.mCommanderManager.LocalCommanderID).TeamID.ID - 1 == descriptor.TeamSpawnIndex) {
						colour = this.mUnitHUDInterfaceAttributes.ExtractionZone.FriendlyColour;
					} else {
						colour = this.mUnitHUDInterfaceAttributes.ExtractionZone.EnemyColour;
					}
					// MOD
					
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
	}
}
