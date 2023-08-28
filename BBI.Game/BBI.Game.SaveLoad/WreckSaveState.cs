using System;
using BBI.Core.ComponentModel;
using BBI.Core.Data;

namespace BBI.Game.SaveLoad
{
	// Token: 0x02000176 RID: 374
	public struct WreckSaveState : IComponentState
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00021076 File Offset: 0x0001F276
		// (set) Token: 0x06000650 RID: 1616 RVA: 0x0002107E File Offset: 0x0001F27E
		public int Index { get; set; }

		// Token: 0x040005FA RID: 1530
		public const int kVersion = 0;

		// Token: 0x040005FB RID: 1531
		public Entity Entity;

		// Token: 0x040005FC RID: 1532
		public int RemainingHealth;

		// Token: 0x040005FD RID: 1533
		public string WreckArtifactType;

		// Token: 0x040005FE RID: 1534
		public int WreckArtifactSpawnPositionOffsetX;

		// Token: 0x040005FF RID: 1535
		public int WreckArtifactSpawnPositionOffsetY;

		// Token: 0x04000600 RID: 1536
		public WreckSectionSaveState[] WreckSections;

		// Token: 0x04000601 RID: 1537
		public string ExplosionWeaponAttributes;
	}
}
