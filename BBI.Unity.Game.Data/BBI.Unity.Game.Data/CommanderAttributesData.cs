using System;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Data;
using BBI.Unity.Core.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace BBI.Unity.Game.Data
{
	// Token: 0x020000B9 RID: 185
	[Serializable]
	public sealed class CommanderAttributesData : CommanderAttributes
	{
		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000345 RID: 837 RVA: 0x000075B7 File Offset: 0x000057B7
		public FactionAttributesAsset FactionAttributes
		{
			get
			{
				return this.m_FactionAttributes;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000346 RID: 838 RVA: 0x000075BF File Offset: 0x000057BF
		string CommanderAttributes.Name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000347 RID: 839 RVA: 0x000075C7 File Offset: 0x000057C7
		string CommanderAttributes.NameLocID
		{
			get
			{
				return this.m_NameLocID;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000348 RID: 840 RVA: 0x000075CF File Offset: 0x000057CF
		int CommanderAttributes.StartingPopCap
		{
			get
			{
				return this.m_StartingPopCap;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000349 RID: 841 RVA: 0x000075D7 File Offset: 0x000057D7
		int CommanderAttributes.MaximumPopCap
		{
			get
			{
				return this.m_MaximumPopCap;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x0600034A RID: 842 RVA: 0x000075DF File Offset: 0x000057DF
		int CommanderAttributes.StartingPopCapEasyDifficulty
		{
			get
			{
				return this.m_StartingPopCapEasyDifficulty;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x0600034B RID: 843 RVA: 0x000075E7 File Offset: 0x000057E7
		int CommanderAttributes.MaximumPopCapEasyDifficulty
		{
			get
			{
				return this.m_MaximumPopCapEasyDifficulty;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x0600034C RID: 844 RVA: 0x000075EF File Offset: 0x000057EF
		int CommanderAttributes.StartingPopCapHardDifficulty
		{
			get
			{
				return this.m_StartingPopCapHardDifficulty;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x0600034D RID: 845 RVA: 0x000075F7 File Offset: 0x000057F7
		int CommanderAttributes.MaximumPopCapHardDifficulty
		{
			get
			{
				return this.m_MaximumPopCapHardDifficulty;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x0600034E RID: 846 RVA: 0x000075FF File Offset: 0x000057FF
		Fixed64 CommanderAttributes.RefundPercentage
		{
			get
			{
				return Fixed64.FromConstFloat(this.m_RefundPercentage);
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x0600034F RID: 847 RVA: 0x0000760C File Offset: 0x0000580C
		// (set) Token: 0x06000350 RID: 848 RVA: 0x00007614 File Offset: 0x00005814
		int CommanderAttributes.StartingResource1
		{
			get
			{
				return this.m_StartingResource1;
			}
			set
			{
				this.m_StartingResource1 = value;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0000761D File Offset: 0x0000581D
		// (set) Token: 0x06000352 RID: 850 RVA: 0x00007625 File Offset: 0x00005825
		int CommanderAttributes.StartingResource2
		{
			get
			{
				return this.m_StartingResource2;
			}
			set
			{
				this.m_StartingResource2 = value;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000353 RID: 851 RVA: 0x0000762E File Offset: 0x0000582E
		Fixed64 CommanderAttributes.Resource1RetirePercentage
		{
			get
			{
				return Fixed64.FromConstFloat(this.m_Resource1RetirePercentage);
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0000763B File Offset: 0x0000583B
		Fixed64 CommanderAttributes.Resource2RetirePercentage
		{
			get
			{
				return Fixed64.FromConstFloat(this.m_Resource2RetirePercentage);
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000355 RID: 853 RVA: 0x00007648 File Offset: 0x00005848
		int CommanderAttributes.TeamID
		{
			get
			{
				return this.m_TeamID;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000356 RID: 854 RVA: 0x00007650 File Offset: 0x00005850
		int CommanderAttributes.TeamColorIndex
		{
			get
			{
				return this.m_TeamColorIndex;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000357 RID: 855 RVA: 0x00007658 File Offset: 0x00005858
		FactionAttributes CommanderAttributes.Faction
		{
			get
			{
				if (this.m_FactionAttributes != null)
				{
					return this.m_FactionAttributes.Data as FactionAttributes;
				}
				return null;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00007674 File Offset: 0x00005874
		// (set) Token: 0x06000359 RID: 857 RVA: 0x0000767C File Offset: 0x0000587C
		StartingUnit[] CommanderAttributes.StartingUnits
		{
			get
			{
				return this.m_StartingUnits;
			}
			set
			{
				this.m_StartingUnits = value;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x0600035A RID: 858 RVA: 0x00007685 File Offset: 0x00005885
		bool CommanderAttributes.HackedTransferCommander
		{
			get
			{
				return this.m_HackedTransferCommander;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000768D File Offset: 0x0000588D
		bool CommanderAttributes.BeginDeployed
		{
			get
			{
				return this.m_BeginDeployed;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x0600035C RID: 860 RVA: 0x00007695 File Offset: 0x00005895
		bool CommanderAttributes.WaitForDeployCommand
		{
			get
			{
				return this.m_WaitForDeployCommand;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0000769D File Offset: 0x0000589D
		bool CommanderAttributes.UseAggroAlert
		{
			get
			{
				return this.m_UseAggroAlert;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x0600035E RID: 862 RVA: 0x000076A5 File Offset: 0x000058A5
		bool CommanderAttributes.MaintainAggroThroughFOW
		{
			get
			{
				return this.m_MaintainAggroThroughFOW;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x0600035F RID: 863 RVA: 0x000076AD File Offset: 0x000058AD
		SpawnFormation[] CommanderAttributes.OverriddenSpawnFormations
		{
			get
			{
				return this.m_OverriddenSpawnFormations;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000360 RID: 864 RVA: 0x000076B5 File Offset: 0x000058B5
		// (set) Token: 0x06000361 RID: 865 RVA: 0x000076BD File Offset: 0x000058BD
		string[] CommanderAttributes.StartingTech
		{
			get
			{
				return this.m_StartingTech;
			}
			set
			{
				this.m_StartingTech = value;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000362 RID: 866 RVA: 0x000076C6 File Offset: 0x000058C6
		// (set) Token: 0x06000363 RID: 867 RVA: 0x000076CE File Offset: 0x000058CE
		string[] CommanderAttributes.LockedTech
		{
			get
			{
				return this.m_LockedTech;
			}
			set
			{
				this.m_LockedTech = value;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000364 RID: 868 RVA: 0x000076D7 File Offset: 0x000058D7
		// (set) Token: 0x06000365 RID: 869 RVA: 0x000076DF File Offset: 0x000058DF
		AIArchetypeAttributes[] CommanderAttributes.AIArchetypes
		{
			get
			{
				return this.m_AIArchetypes;
			}
			set
			{
				this.m_AIArchetypes = value;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000366 RID: 870 RVA: 0x000076E8 File Offset: 0x000058E8
		public AIArchetypeAttributesAsset[] AIArchetypeAssets
		{
			get
			{
				return this.m_AIArchetypeAssets;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000367 RID: 871 RVA: 0x000076F0 File Offset: 0x000058F0
		UnitTypeBuff[] CommanderAttributes.StartingBuffs
		{
			get
			{
				return this.m_StartingBuffs;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000368 RID: 872 RVA: 0x000076F8 File Offset: 0x000058F8
		string[] CommanderAttributes.StartingGrantedAbilities
		{
			get
			{
				return this.m_StartingGrantedAbilities;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00007700 File Offset: 0x00005900
		// (set) Token: 0x0600036A RID: 874 RVA: 0x00007708 File Offset: 0x00005908
		string[] CommanderAttributes.StoryArtifacts
		{
			get
			{
				return this.m_StoryArtifacts;
			}
			set
			{
				this.m_StoryArtifacts = value;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x0600036B RID: 875 RVA: 0x00007711 File Offset: 0x00005911
		// (set) Token: 0x0600036C RID: 876 RVA: 0x00007719 File Offset: 0x00005919
		string[] CommanderAttributes.StartingArtifactsPool
		{
			get
			{
				return this.m_StartingArtifactsPool;
			}
			set
			{
				this.m_StartingArtifactsPool = value;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00007722 File Offset: 0x00005922
		int CommanderAttributes.NumStartingArtifacts
		{
			get
			{
				return this.m_NumStartingArtifacts;
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000772C File Offset: 0x0000592C
		public CommanderAttributesData()
		{
		}

		// Token: 0x0400030E RID: 782
		[SerializeField]
		private string m_Name = "_unset_";

		// Token: 0x0400030F RID: 783
		[SerializeField]
		private string m_NameLocID;

		// Token: 0x04000310 RID: 784
		[SerializeField]
		[Tooltip("The starting popcap for multiplayer/skirmish, and for Medium Difficulty in single player")]
		[FormerlySerializedAs("m_MaxPopCap")]
		private int m_StartingPopCap = 20;

		// Token: 0x04000311 RID: 785
		[SerializeField]
		[Tooltip("The maximum popcap for multiplayer/skirmish, and for Medium Difficulty in single player")]
		private int m_MaximumPopCap = 1000;

		// Token: 0x04000312 RID: 786
		[Tooltip("The starting popcap for Easy Difficulty (campaign only)")]
		[SerializeField]
		private int m_StartingPopCapEasyDifficulty = 20;

		// Token: 0x04000313 RID: 787
		[Tooltip("The maximum popcap for Easy Difficulty (campaign only)")]
		[SerializeField]
		private int m_MaximumPopCapEasyDifficulty = 1000;

		// Token: 0x04000314 RID: 788
		[SerializeField]
		[Tooltip("The starting popcap for Hard Difficulty (campaign only)")]
		private int m_StartingPopCapHardDifficulty = 20;

		// Token: 0x04000315 RID: 789
		[Tooltip("The maximum popcap for Hard Difficulty (campaign only)")]
		[SerializeField]
		private int m_MaximumPopCapHardDifficulty = 1000;

		// Token: 0x04000316 RID: 790
		[SerializeField]
		private float m_RefundPercentage = 1f;

		// Token: 0x04000317 RID: 791
		[SerializeField]
		private int m_StartingResource1 = 1500;

		// Token: 0x04000318 RID: 792
		[SerializeField]
		private int m_StartingResource2 = 1500;

		// Token: 0x04000319 RID: 793
		[SerializeField]
		private float m_Resource1RetirePercentage = 0.75f;

		// Token: 0x0400031A RID: 794
		[SerializeField]
		private float m_Resource2RetirePercentage = 0.75f;

		// Token: 0x0400031B RID: 795
		[SerializeField]
		private int m_TeamID = 1;

		// Token: 0x0400031C RID: 796
		[SerializeField]
		private int m_TeamColorIndex = -1;

		// Token: 0x0400031D RID: 797
		[SerializeField]
		private FactionAttributesAsset m_FactionAttributes;

		// Token: 0x0400031E RID: 798
		[SerializeField]
		private StartingUnit[] m_StartingUnits;

		// Token: 0x0400031F RID: 799
		[SerializeField]
		private bool m_HackedTransferCommander;

		// Token: 0x04000320 RID: 800
		[SerializeField]
		private bool m_BeginDeployed;

		// Token: 0x04000321 RID: 801
		[SerializeField]
		private bool m_WaitForDeployCommand;

		// Token: 0x04000322 RID: 802
		[SerializeField]
		private bool m_UseAggroAlert;

		// Token: 0x04000323 RID: 803
		[SerializeField]
		private bool m_MaintainAggroThroughFOW;

		// Token: 0x04000324 RID: 804
		[SerializeField]
		private SpawnFormationData[] m_OverriddenSpawnFormations = new SpawnFormationData[0];

		// Token: 0x04000325 RID: 805
		[ObjectName(typeof(ResearchItemAttributesAsset))]
		[SerializeField]
		private string[] m_StartingTech;

		// Token: 0x04000326 RID: 806
		[SerializeField]
		private string[] m_LockedTech;

		// Token: 0x04000327 RID: 807
		[SerializeField]
		private UnitTypeBuffData[] m_StartingBuffs;

		// Token: 0x04000328 RID: 808
		[SerializeField]
		[Tooltip("Abilities that are granted to this Commander at the start of the game, which override an ability's StartsRemovedInGameMode setting")]
		[ObjectName(typeof(AbilityAttributesAsset))]
		private string[] m_StartingGrantedAbilities;

		// Token: 0x04000329 RID: 809
		[SerializeField]
		private string[] m_StoryArtifacts = new string[0];

		// Token: 0x0400032A RID: 810
		[SerializeField]
		private string[] m_StartingArtifactsPool = new string[0];

		// Token: 0x0400032B RID: 811
		[SerializeField]
		private int m_NumStartingArtifacts;

		// Token: 0x0400032C RID: 812
		[SerializeField]
		private AIArchetypeAttributesAsset[] m_AIArchetypeAssets;

		// Token: 0x0400032D RID: 813
		private AIArchetypeAttributes[] m_AIArchetypes;
	}
}
