// Fixing carrier pathing on new maps

using System;
using System.Collections.Generic;
using BBI.Core;
using BBI.Core.Utility.FixedPoint;
using BBI.Unity.Game.Data;

namespace BBI.Game.Data
{
	// Token: 0x020000E1 RID: 225
	[Serializable]
	internal sealed partial class UnitAttributesData : NamedObjectBase, UnitAttributes, INamed, CostAttributes
	{
		// Token: 0x17000334 RID: 820
		// (get) Token: 0x060003B6 RID: 950
		NavMeshAttributes UnitAttributes.NavMeshAttributes
		{
			get
			{
				// MOD
				if (MapModManager.CustomLayout && new string[]
				{
					"G_Carrier_UnitAttributesAsset_MP",
					"C_Carrier_UnitAttributesAsset_MP",
					"C_Sob_Carrier_UnitAttributesAsset_MP",
					"K_Carrier_UnitAttributesAsset_MP"
				}.Contains(base.Name))
				{
					return new NavMeshData
					{
						m_BlockedBy = (UnitClass.Ground | UnitClass.Carrier),
						m_DistanceErrorPercentageTolerance = float.Parse(this.m_NavMesh.DistanceErrorPercentageTolerance.ToString()),
						m_DistanceFromObstacles = 135f
					};
				}
				// MOD
				return this.m_NavMesh;
			}
		}
	}
}
