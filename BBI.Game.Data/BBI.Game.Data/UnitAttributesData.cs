using System;
using System.Collections.Generic;
using BBI.Core;
using BBI.Core.Utility.FixedPoint;

namespace BBI.Game.Data
{
	// Token: 0x020000E0 RID: 224
	[Serializable]
	internal sealed class UnitAttributesData : NamedObjectBase, UnitAttributes, INamed, CostAttributes
	{
		// Token: 0x17000330 RID: 816
		// (get) Token: 0x060003AC RID: 940 RVA: 0x00003C9B File Offset: 0x00001E9B
		UnitClass UnitAttributes.Class
		{
			get
			{
				return this.m_Class;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00003CA3 File Offset: 0x00001EA3
		UnitSelectionFlags UnitAttributes.SelectionFlags
		{
			get
			{
				return this.m_SelectionFlags;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x060003AE RID: 942 RVA: 0x00003CAB File Offset: 0x00001EAB
		NavMeshAttributes UnitAttributes.NavMeshAttributes
		{
			get
			{
				return this.m_NavMesh;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x060003AF RID: 943 RVA: 0x00003CB3 File Offset: 0x00001EB3
		int UnitAttributes.MaxHealth
		{
			get
			{
				return this.m_MaxHealth;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x00003CBB File Offset: 0x00001EBB
		int UnitAttributes.Armour
		{
			get
			{
				return this.m_Armour;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x00003CC3 File Offset: 0x00001EC3
		Fixed64 UnitAttributes.DamageReceivedMultiplier
		{
			get
			{
				return Fixed64.FromConstFloat(this.m_DamageReceivedMultiplier);
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x00003CD0 File Offset: 0x00001ED0
		Fixed64 UnitAttributes.AccuracyReceivedMultiplier
		{
			get
			{
				return Fixed64.FromConstFloat(this.m_AccuracyReceivedMultiplier);
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x00003CDD File Offset: 0x00001EDD
		int CostAttributes.Resource1Cost
		{
			get
			{
				return this.m_Resource1Cost;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x00003CE5 File Offset: 0x00001EE5
		int CostAttributes.Resource2Cost
		{
			get
			{
				return this.m_Resource2Cost;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x00003CED File Offset: 0x00001EED
		int UnitAttributes.PopCapCost
		{
			get
			{
				return this.m_PopCapCost;
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x00003CF5 File Offset: 0x00001EF5
		int UnitAttributes.ExperienceValue
		{
			get
			{
				return this.m_ExperienceValue;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x00003CFD File Offset: 0x00001EFD
		Fixed64 UnitAttributes.ProductionTime
		{
			get
			{
				return Fixed64.UnsafeFromFloat(this.m_ProductionTime);
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x00003D0A File Offset: 0x00001F0A
		Fixed64 UnitAttributes.AggroRange
		{
			get
			{
				return Fixed64.FromInt(this.m_AggroRange);
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00003D17 File Offset: 0x00001F17
		Fixed64 UnitAttributes.LeashRange
		{
			get
			{
				return Fixed64.FromInt(this.m_LeashRange);
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060003BA RID: 954 RVA: 0x00003D24 File Offset: 0x00001F24
		Fixed64 UnitAttributes.AlertRange
		{
			get
			{
				return Fixed64.FromInt(this.m_AlertRange);
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00003D31 File Offset: 0x00001F31
		Fixed64 UnitAttributes.RepairPickupRange
		{
			get
			{
				return Fixed64.FromInt(this.m_RepairPickupRange);
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060003BC RID: 956 RVA: 0x00003D3E File Offset: 0x00001F3E
		UnitPositionReaggroConditions UnitAttributes.UnitPositionReaggroConditions
		{
			get
			{
				return this.m_UnitPositionReaggroConditions;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060003BD RID: 957 RVA: 0x00003D46 File Offset: 0x00001F46
		LeashPositionReaggroConditions UnitAttributes.LeashPositionReaggroConditions
		{
			get
			{
				return this.m_LeashPositionReaggroConditions;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060003BE RID: 958 RVA: 0x00003D4E File Offset: 0x00001F4E
		int UnitAttributes.LeadPriority
		{
			get
			{
				return this.m_LeadPriority;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060003BF RID: 959 RVA: 0x00003D56 File Offset: 0x00001F56
		bool UnitAttributes.Selectable
		{
			get
			{
				return this.m_Selectable;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x00003D5E File Offset: 0x00001F5E
		bool UnitAttributes.Controllable
		{
			get
			{
				return this.m_Controllable;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00003D66 File Offset: 0x00001F66
		bool UnitAttributes.Targetable
		{
			get
			{
				return this.m_Targetable;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x00003D6E File Offset: 0x00001F6E
		bool UnitAttributes.NonAutoTargetable
		{
			get
			{
				return this.m_NonAutoTargetable;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00003D76 File Offset: 0x00001F76
		bool UnitAttributes.RetireTargetable
		{
			get
			{
				return this.m_RetireTargetable;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x00003D7E File Offset: 0x00001F7E
		bool UnitAttributes.HackedReturnTargetable
		{
			get
			{
				return this.m_HackedReturnTargetable;
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x00003D86 File Offset: 0x00001F86
		HackableProperties UnitAttributes.HackableProperties
		{
			get
			{
				return this.m_HackableProperties;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x00003D8E File Offset: 0x00001F8E
		bool UnitAttributes.ExcludeFromUnitStats
		{
			get
			{
				return this.m_ExcludeFromUnitStats;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x00003D96 File Offset: 0x00001F96
		bool UnitAttributes.BlocksLOF
		{
			get
			{
				return this.m_BlocksLOF;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x00003D9E File Offset: 0x00001F9E
		Fixed64 UnitAttributes.WorldHeightOffset
		{
			get
			{
				return Fixed64.UnsafeFromFloat(this.m_WorldHeightOffset);
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x00003DAB File Offset: 0x00001FAB
		bool UnitAttributes.DoNotPersist
		{
			get
			{
				return this.m_DoNotPersist;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060003CA RID: 970 RVA: 0x00003DB3 File Offset: 0x00001FB3
		bool UnitAttributes.LevelBound
		{
			get
			{
				return this.m_LevelBound;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x060003CB RID: 971 RVA: 0x00003DBB File Offset: 0x00001FBB
		bool UnitAttributes.StartsInHangar
		{
			get
			{
				return this.m_StartsInHangar;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x060003CC RID: 972 RVA: 0x00003DC3 File Offset: 0x00001FC3
		Fixed64 UnitAttributes.SensorRadius
		{
			get
			{
				return Fixed64.UnsafeFromFloat(this.m_SensorRadius);
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x060003CD RID: 973 RVA: 0x00003DD0 File Offset: 0x00001FD0
		Fixed64 UnitAttributes.ContactRadius
		{
			get
			{
				return Fixed64.UnsafeFromFloat(this.m_ContactRadius);
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060003CE RID: 974 RVA: 0x00003DDD File Offset: 0x00001FDD
		int UnitAttributes.NumProductionQueues
		{
			get
			{
				return this.m_NumProductionQueues;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060003CF RID: 975 RVA: 0x00003DE5 File Offset: 0x00001FE5
		int UnitAttributes.ProductionQueueDepth
		{
			get
			{
				return this.m_ProductionQueueDepth;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x00003DED File Offset: 0x00001FED
		bool UnitAttributes.ShowProductionQueues
		{
			get
			{
				return this.m_ShowProductionQueues;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x00003DF5 File Offset: 0x00001FF5
		bool UnitAttributes.NoTextNotifications
		{
			get
			{
				return this.m_NoTextNotifications;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x00003DFD File Offset: 0x00001FFD
		UnitNotificationFlags UnitAttributes.NotificationFlags
		{
			get
			{
				return this.m_NotificationFlags;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x00003E05 File Offset: 0x00002005
		int UnitAttributes.FireRateDisplay
		{
			get
			{
				return (int)this.m_FireRate;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x00003E0D File Offset: 0x0000200D
		WeaponBinding[] UnitAttributes.WeaponLoadout
		{
			get
			{
				return this.m_WeaponLoadout;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x00003E15 File Offset: 0x00002015
		Fixed64 UnitAttributes.PriorityAsTarget
		{
			get
			{
				return Fixed64.FromConstFloat(this.m_PriorityAsTarget);
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x00003E22 File Offset: 0x00002022
		ThreatData UnitAttributes.ThreatData
		{
			get
			{
				return this.m_ThreatData;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x00003E2A File Offset: 0x0000202A
		IEnumerable<ThreatCounter> UnitAttributes.ThreatCounters
		{
			get
			{
				return this.m_ThreatCounters;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00003E32 File Offset: 0x00002032
		IEnumerable<ThreatCounter> UnitAttributes.ThreatCounteredBys
		{
			get
			{
				return this.m_ThreatCounteredBys;
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00003E3C File Offset: 0x0000203C
		public UnitAttributesData()
		{
		}

		// Token: 0x04000348 RID: 840
		public UnitClass m_Class = UnitClass.Ground;

		// Token: 0x04000349 RID: 841
		public UnitSelectionFlags m_SelectionFlags = UnitSelectionFlags.CombatUnitSelect;

		// Token: 0x0400034A RID: 842
		public NavMeshAttributes m_NavMesh;

		// Token: 0x0400034B RID: 843
		public int m_MaxHealth = 1000;

		// Token: 0x0400034C RID: 844
		public int m_Armour = 100;

		// Token: 0x0400034D RID: 845
		public float m_DamageReceivedMultiplier = 1f;

		// Token: 0x0400034E RID: 846
		public float m_AccuracyReceivedMultiplier = 1f;

		// Token: 0x0400034F RID: 847
		public int m_Resource1Cost;

		// Token: 0x04000350 RID: 848
		public int m_Resource2Cost;

		// Token: 0x04000351 RID: 849
		public int m_PopCapCost = 1;

		// Token: 0x04000352 RID: 850
		public int m_ExperienceValue;

		// Token: 0x04000353 RID: 851
		public float m_ProductionTime;

		// Token: 0x04000354 RID: 852
		public int m_AggroRange = 400;

		// Token: 0x04000355 RID: 853
		public int m_LeashRange = 400;

		// Token: 0x04000356 RID: 854
		public int m_AlertRange = 350;

		// Token: 0x04000357 RID: 855
		public int m_RepairPickupRange = 1000;

		// Token: 0x04000358 RID: 856
		public UnitPositionReaggroConditions m_UnitPositionReaggroConditions = UnitPositionReaggroConditions.TargetWithinAggroRange;

		// Token: 0x04000359 RID: 857
		public LeashPositionReaggroConditions m_LeashPositionReaggroConditions;

		// Token: 0x0400035A RID: 858
		public int m_LeadPriority;

		// Token: 0x0400035B RID: 859
		public bool m_Selectable = true;

		// Token: 0x0400035C RID: 860
		public bool m_Controllable = true;

		// Token: 0x0400035D RID: 861
		public bool m_Targetable = true;

		// Token: 0x0400035E RID: 862
		public bool m_NonAutoTargetable;

		// Token: 0x0400035F RID: 863
		public bool m_RetireTargetable;

		// Token: 0x04000360 RID: 864
		public bool m_HackedReturnTargetable;

		// Token: 0x04000361 RID: 865
		public HackableProperties m_HackableProperties = HackableProperties.Hackable;

		// Token: 0x04000362 RID: 866
		public bool m_ExcludeFromUnitStats;

		// Token: 0x04000363 RID: 867
		public bool m_BlocksLOF;

		// Token: 0x04000364 RID: 868
		public float m_WorldHeightOffset;

		// Token: 0x04000365 RID: 869
		public bool m_DoNotPersist;

		// Token: 0x04000366 RID: 870
		public bool m_LevelBound;

		// Token: 0x04000367 RID: 871
		public bool m_StartsInHangar;

		// Token: 0x04000368 RID: 872
		public float m_SensorRadius;

		// Token: 0x04000369 RID: 873
		public float m_ContactRadius;

		// Token: 0x0400036A RID: 874
		public int m_NumProductionQueues;

		// Token: 0x0400036B RID: 875
		public int m_ProductionQueueDepth;

		// Token: 0x0400036C RID: 876
		public bool m_ShowProductionQueues = true;

		// Token: 0x0400036D RID: 877
		public bool m_NoTextNotifications;

		// Token: 0x0400036E RID: 878
		public UnitNotificationFlags m_NotificationFlags;

		// Token: 0x0400036F RID: 879
		public float m_PriorityAsTarget;

		// Token: 0x04000370 RID: 880
		public FireRateCategory m_FireRate = FireRateCategory.Low;

		// Token: 0x04000371 RID: 881
		public WeaponBinding[] m_WeaponLoadout;

		// Token: 0x04000372 RID: 882
		public ThreatData m_ThreatData;

		// Token: 0x04000373 RID: 883
		public ThreatCounter[] m_ThreatCounters;

		// Token: 0x04000374 RID: 884
		public ThreatCounter[] m_ThreatCounteredBys;
	}
}
