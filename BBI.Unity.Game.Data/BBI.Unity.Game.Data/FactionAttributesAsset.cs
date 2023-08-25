using System;
using BBI.Unity.Core.Data;
using UnityEngine;

namespace BBI.Unity.Game.Data
{
	// Token: 0x02000061 RID: 97
	public sealed class FactionAttributesAsset : AssetBase, IEntityTypeRegistrationRequired
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600016E RID: 366 RVA: 0x000052E1 File Offset: 0x000034E1
		public override object Data
		{
			get
			{
				return this.m_FactionAttributes;
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000052E9 File Offset: 0x000034E9
		public FactionAttributesAsset()
		{
		}

		// Token: 0x04000133 RID: 307
		[SerializeField]
		public FactionAttributesData m_FactionAttributes;
	}
}
