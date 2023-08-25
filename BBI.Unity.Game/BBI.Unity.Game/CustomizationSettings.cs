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
	// Token: 0x0200016D RID: 365
	public class CustomizationSettings : SettingsBase
	{
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x00026100 File Offset: 0x00024300
		public UnitColors UnitColors
		{
			get
			{
				return new UnitColors(NGUIMath.ColorToInt(this.PrimaryColor), NGUIMath.ColorToInt(this.TrimColor));
			}
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00026127 File Offset: 0x00024327
		public CustomizationSettings()
		{
			this.SetToDefaults();
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00026138 File Offset: 0x00024338
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

		// Token: 0x0600077D RID: 1917 RVA: 0x000261B8 File Offset: 0x000243B8
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

		// Token: 0x0600077E RID: 1918 RVA: 0x000261FF File Offset: 0x000243FF
		public UnitColors GetLocalPlayerUnitColors(UnitHUDInteractionAttributes unitHUDInterfaceAttributes)
		{
			if (this.UseCustomizationValues)
			{
				return this.UnitColors;
			}
			return CustomizationSettings.GetPresetUnitColors(0, unitHUDInterfaceAttributes);
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00026218 File Offset: 0x00024418
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

		// Token: 0x06000780 RID: 1920 RVA: 0x00026254 File Offset: 0x00024454
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

		// Token: 0x040007EB RID: 2027
		public bool UseCustomizationValues;

		// Token: 0x040007EC RID: 2028
		public Color32 PrimaryColor;

		// Token: 0x040007ED RID: 2029
		public Color32 TrimColor;

		// Token: 0x040007EE RID: 2030
		public int DecalIndex;

		// Token: 0x040007EF RID: 2031
		public DLCPackID[] UnitSkinPackID;

		// Token: 0x040007F0 RID: 2032
		public int FactionIndex;

		// Token: 0x040007F1 RID: 2033
		public bool RandomFaction;
	}
}
