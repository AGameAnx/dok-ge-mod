using System;
using System.Collections.Generic;
using BBI.Core;
using BBI.Core.Utility.FixedPoint;

namespace BBI.Game.Data
{
	// Token: 0x020000E1 RID: 225
	[Serializable]
	internal sealed partial class UnitAttributesData : NamedObjectBase, UnitAttributes, INamed, CostAttributes
	{
		// Token: 0x17000335 RID: 821
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x0000319C File Offset: 0x0000139C
		int UnitAttributes.MaxHealth
		{
			get
			{
				if (MapModManager.GameType != GameMode.SinglePlayer && Name == "G_Fathership_UnitAttributesAsset") return 18000;
				return this.m_MaxHealth;
			}
		}
	}
}
