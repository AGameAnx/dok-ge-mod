using System;
using BBI.Game.Data;
using UnityEngine;

namespace BBI.Unity.Game.Data
{
	// Token: 0x0200009C RID: 156
	[Serializable]
	public sealed class CostAttributesData : CostAttributes
	{
		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000282 RID: 642 RVA: 0x00006904 File Offset: 0x00004B04
		int CostAttributes.Resource1Cost
		{
			get
			{
				return this.m_Resource1Cost;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000690C File Offset: 0x00004B0C
		int CostAttributes.Resource2Cost
		{
			get
			{
				return this.m_Resource2Cost;
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00006914 File Offset: 0x00004B14
		public CostAttributesData()
		{
		}

		// Token: 0x04000252 RID: 594
		[SerializeField]
		private int m_Resource1Cost;

		// Token: 0x04000253 RID: 595
		[SerializeField]
		private int m_Resource2Cost;
	}
}
