using System;
using BBI.Core;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.DLC;
using UnityEngine;

namespace BBI.Unity.Game
{
	public class CustomizationSettings : SettingsBase
	{
		public UnitColors UnitColors
		{
			get
			{
				return new UnitColors(NGUIMath.ColorToInt(this.PrimaryColor), NGUIMath.ColorToInt(this.TrimColor));
			}
		}

		public CustomizationSettings()
		{
			this.SetToDefaults();
		}

		public override void SetToDefaults()
		{
			this.UseCustomizationValues = false;
			this.PrimaryColor = Color.white;
			this.TrimColor = Color.white;
			this.DecalIndex = 0;
			this.UnitSkinPackID = new DLCPackID[2];
			for (int i = 0; i < this.UnitSkinPackID.Length; i++)
			{
				this.UnitSkinPackID[i] = DLCPackID.kInvalidID;
			}
			this.FactionIndex = 0;
			this.RandomFaction = false;
		}

		public DLCPackID GetLocalPlayerSkinPackIDForFaction(DLCManager dlcManager, CustomizationFactionSetting forFaction)
		{
			if (forFaction > CustomizationFactionSetting.Gaalsien)
			{
				forFaction = CustomizationFactionSetting.Coalition;
			}
			if (this.UseCustomizationValues && dlcManager.DoesPackExist(this.UnitSkinPackID[(int)forFaction]))
			{
				return this.UnitSkinPackID[(int)forFaction];
			}
			return DLCPackID.kInvalidID;
		}

		public UnitColors GetLocalPlayerUnitColors(UnitHUDInteractionAttributes unitHUDInterfaceAttributes)
		{
			if (this.UseCustomizationValues)
			{
				return this.UnitColors;
			}
			return CustomizationSettings.GetPresetUnitColors(0, unitHUDInterfaceAttributes);
		}

		public static UnitColors GetPresetUnitColors(int colorIndex, UnitHUDInteractionAttributes unitHUDInterfaceAttributes)
		{
			if (unitHUDInterfaceAttributes == null)
			{
				Log.Error(Log.Channel.UI, "null UnitHUDInteractionAttributes", new object[0]);
				return UnitColors.kDefault;
			}
			UnitHUDInteractionAttributes.PlayerUnitColours[] playerUnitColors = unitHUDInterfaceAttributes.PlayerUnitColors;
			return CustomizationSettings.GetPresetUnitColors(colorIndex, playerUnitColors);
		}

		public static UnitColors GetPresetUnitColors(int colorIndex, UnitHUDInteractionAttributes.PlayerUnitColours[] unitColors)
		{
			if (unitColors.IsNullOrEmpty<UnitHUDInteractionAttributes.PlayerUnitColours>())
			{
				Log.Error(Log.Channel.UI, "no UnitHUDInteractionAttributes.PlayerUnitColors found in UnitHUDInteractionAttributes", new object[0]);
				return UnitColors.kDefault;
			}
			if (colorIndex < 0)
			{
				colorIndex = 0;
			}
			colorIndex %= unitColors.Length;
			UnitHUDInteractionAttributes.PlayerUnitColours playerUnitColours = unitColors[colorIndex];
			return new UnitColors(NGUIMath.ColorToInt(playerUnitColours.BaseColour), NGUIMath.ColorToInt(playerUnitColours.TrimColour));
		}

		public bool UseCustomizationValues;

		public Color32 PrimaryColor;

		public Color32 TrimColor;

		public int DecalIndex;

		public DLCPackID[] UnitSkinPackID;

		public int FactionIndex;

		public bool RandomFaction;
	}
}
