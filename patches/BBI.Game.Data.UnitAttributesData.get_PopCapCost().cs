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
		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060003BD RID: 957 RVA: 0x000031D6 File Offset: 0x000013D6
		int UnitAttributes.PopCapCost
		{
			get
			{
				if (MapModManager.GameType != GameMode.SinglePlayer && Name == "G_Fathership_UnitAttributesAsset") return 50;
				return this.m_PopCapCost;
			}
		}
	}
}
