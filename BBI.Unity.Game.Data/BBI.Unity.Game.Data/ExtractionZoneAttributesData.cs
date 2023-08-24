using System;
using BBI.Core;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Data;
using BBI.Game.Data.Queries;
using UnityEngine;

namespace BBI.Unity.Game.Data
{
	// Token: 0x020000B7 RID: 183
	[Serializable]
	public sealed class ExtractionZoneAttributesData : NamedObjectBase, ExtractionZoneAttributes, INamed
	{
		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600033D RID: 829 RVA: 0x00007544 File Offset: 0x00005744
		Fixed64 ExtractionZoneAttributes.RadiusMeters
		{
			get
			{
				return Fixed64.FromConstFloat(this.m_RadiusMeters);
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00007551 File Offset: 0x00005751
		QueryContainerBase ExtractionZoneAttributes.QueryData
		{
			get
			{
				return this.m_Query;
			}
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00007559 File Offset: 0x00005759
		public ExtractionZoneAttributesData()
		{
		}

		// Token: 0x04000309 RID: 777
		[SerializeField]
		private float m_RadiusMeters = 1f;

		// Token: 0x0400030A RID: 778
		[SerializeField]
		private QueryContainerBase m_Query;
	}
}
