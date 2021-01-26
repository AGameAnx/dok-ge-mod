using System;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Data;
using BBI.Unity.Core.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace BBI.Unity.Game.Data
{
	// Token: 0x020000C2 RID: 194
	[Serializable]
	public sealed class CommanderAttributesData : CommanderAttributes
	{
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000389 RID: 905 RVA: 0x0000414D File Offset: 0x0000234D
		public FactionAttributesAsset FactionAttributes
		{
			get
			{
				return this.m_FactionAttributes;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x0600038A RID: 906 RVA: 0x00004155 File Offset: 0x00002355
		string CommanderAttributes.Name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x0600038B RID: 907 RVA: 0x0000415D File Offset: 0x0000235D
		string CommanderAttributes.NameLocID
		{
			get
			{
				return this.m_NameLocID;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00004165 File Offset: 0x00002365
		int CommanderAttributes.StartingPopCap
		{
			get
			{
				return this.m_StartingPopCap;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x0600038D RID: 909 RVA: 0x0000416D File Offset: 0x0000236D
		int CommanderAttributes.MaximumPopCap
		{
			get
			{
				return this.m_MaximumPopCap;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600038E RID: 910 RVA: 0x00004175 File Offset: 0x00002375
		int CommanderAttributes.StartingPopCapEasyDifficulty
		{
			get
			{
				return this.m_StartingPopCapEasyDifficulty;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x0600038F RID: 911 RVA: 0x0000417D File Offset: 0x0000237D
		int CommanderAttributes.MaximumPopCapEasyDifficulty
		{
			get
			{
				return this.m_MaximumPopCapEasyDifficulty;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000390 RID: 912 RVA: 0x00004185 File Offset: 0x00002385
		int CommanderAttributes.StartingPopCapHardDifficulty
		{
			get
			{
				return this.m_StartingPopCapHardDifficulty;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0000418D File Offset: 0x0000238D
		int CommanderAttributes.MaximumPopCapHardDifficulty
		{
			get
			{
				return this.m_MaximumPopCapHardDifficulty;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000392 RID: 914 RVA: 0x00004195 File Offset: 0x00002395
		Fixed64 CommanderAttributes.RefundPercentage
		{
			get
			{
				return Fixed64.FromConstFloat(this.m_RefundPercentage);
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000393 RID: 915 RVA: 0x000041A2 File Offset: 0x000023A2
		// (set) Token: 0x06000394 RID: 916 RVA: 0x000041AA File Offset: 0x000023AA
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

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000395 RID: 917 RVA: 0x000041B3 File Offset: 0x000023B3
		// (set) Token: 0x06000396 RID: 918 RVA: 0x000041BB File Offset: 0x000023BB
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

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000397 RID: 919 RVA: 0x000041C4 File Offset: 0x000023C4
		Fixed64 CommanderAttributes.Resource1RetirePercentage
		{
			get
			{
				return Fixed64.FromConstFloat(this.m_Resource1RetirePercentage);
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000398 RID: 920 RVA: 0x000041D1 File Offset: 0x000023D1
		Fixed64 CommanderAttributes.Resource2RetirePercentage
		{
			get
			{
				return Fixed64.FromConstFloat(this.m_Resource2RetirePercentage);
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000399 RID: 921 RVA: 0x000041DE File Offset: 0x000023DE
		int CommanderAttributes.TeamID
		{
			get
			{
				return this.m_TeamID;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x0600039A RID: 922 RVA: 0x000041E6 File Offset: 0x000023E6
		int CommanderAttributes.TeamColorIndex
		{
			get
			{
				return this.m_TeamColorIndex;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x0600039B RID: 923 RVA: 0x000041EE File Offset: 0x000023EE
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

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x0600039C RID: 924 RVA: 0x0000420A File Offset: 0x0000240A
		// (set) Token: 0x0600039D RID: 925 RVA: 0x00004212 File Offset: 0x00002412
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

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x0600039E RID: 926 RVA: 0x0000421B File Offset: 0x0000241B
		bool CommanderAttributes.HackedTransferCommander
		{
			get
			{
				return this.m_HackedTransferCommander;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00004223 File Offset: 0x00002423
		bool CommanderAttributes.BeginDeployed
		{
			get
			{
				return this.m_BeginDeployed;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x0000422B File Offset: 0x0000242B
		bool CommanderAttributes.WaitForDeployCommand
		{
			get
			{
				return this.m_WaitForDeployCommand;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00004233 File Offset: 0x00002433
		bool CommanderAttributes.UseAggroAlert
		{
			get
			{
				return this.m_UseAggroAlert;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x0000423B File Offset: 0x0000243B
		bool CommanderAttributes.MaintainAggroThroughFOW
		{
			get
			{
				return this.m_MaintainAggroThroughFOW;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00004243 File Offset: 0x00002443
		SpawnFormation[] CommanderAttributes.OverriddenSpawnFormations
		{
			get
			{
				return this.m_OverriddenSpawnFormations;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0000424B File Offset: 0x0000244B
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x00004253 File Offset: 0x00002453
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

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x0000425C File Offset: 0x0000245C
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x00004264 File Offset: 0x00002464
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

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0000426D File Offset: 0x0000246D
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x00004275 File Offset: 0x00002475
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

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060003AA RID: 938 RVA: 0x0000427E File Offset: 0x0000247E
		public AIArchetypeAttributesAsset[] AIArchetypeAssets
		{
			get
			{
				return this.m_AIArchetypeAssets;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00004286 File Offset: 0x00002486
		UnitTypeBuff[] CommanderAttributes.StartingBuffs
		{
			get
			{
				return this.m_StartingBuffs;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060003AC RID: 940 RVA: 0x0000428E File Offset: 0x0000248E
		string[] CommanderAttributes.StartingGrantedAbilities
		{
			get
			{
				return this.m_StartingGrantedAbilities;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00004296 File Offset: 0x00002496
		// (set) Token: 0x060003AE RID: 942 RVA: 0x0000429E File Offset: 0x0000249E
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

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060003AF RID: 943 RVA: 0x000042A7 File Offset: 0x000024A7
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x000042AF File Offset: 0x000024AF
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

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x000042B8 File Offset: 0x000024B8
		int CommanderAttributes.NumStartingArtifacts
		{
			get
			{
				return this.m_NumStartingArtifacts;
			}
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00008304 File Offset: 0x00006504
		public CommanderAttributesData()
		{
		}

		// MOD
		public CommanderAttributesData Copy() {
			return (CommanderAttributesData)MemberwiseClone();
		}
		// MOD

		// Token: 0x04000341 RID: 833
		[SerializeField]
		public string m_Name = "_unset_";

		// Token: 0x04000342 RID: 834
		[SerializeField]
		public string m_NameLocID;

		// Token: 0x04000343 RID: 835
		[SerializeField]
		[Tooltip("The starting popcap for multiplayer/skirmish, and for Medium Difficulty in single player")]
		[FormerlySerializedAs("m_MaxPopCap")]
		public int m_StartingPopCap = 20;

		// Token: 0x04000344 RID: 836
		[SerializeField]
		[Tooltip("The maximum popcap for multiplayer/skirmish, and for Medium Difficulty in single player")]
		public int m_MaximumPopCap = 1000;

		// Token: 0x04000345 RID: 837
		[Tooltip("The starting popcap for Easy Difficulty (campaign only)")]
		[SerializeField]
		public int m_StartingPopCapEasyDifficulty = 20;

		// Token: 0x04000346 RID: 838
		[Tooltip("The maximum popcap for Easy Difficulty (campaign only)")]
		[SerializeField]
		public int m_MaximumPopCapEasyDifficulty = 1000;

		// Token: 0x04000347 RID: 839
		[SerializeField]
		[Tooltip("The starting popcap for Hard Difficulty (campaign only)")]
		public int m_StartingPopCapHardDifficulty = 20;

		// Token: 0x04000348 RID: 840
		[Tooltip("The maximum popcap for Hard Difficulty (campaign only)")]
		[SerializeField]
		public int m_MaximumPopCapHardDifficulty = 1000;

		// Token: 0x04000349 RID: 841
		[SerializeField]
		public float m_RefundPercentage = 1f;

		// Token: 0x0400034A RID: 842
		[SerializeField]
		public int m_StartingResource1 = 1500;

		// Token: 0x0400034B RID: 843
		[SerializeField]
		public int m_StartingResource2 = 1500;

		// Token: 0x0400034C RID: 844
		[SerializeField]
		public float m_Resource1RetirePercentage = 0.75f;

		// Token: 0x0400034D RID: 845
		[SerializeField]
		public float m_Resource2RetirePercentage = 0.75f;

		// Token: 0x0400034E RID: 846
		[SerializeField]
		public int m_TeamID = 1;

		// Token: 0x0400034F RID: 847
		[SerializeField]
		public int m_TeamColorIndex = -1;

		// Token: 0x04000350 RID: 848
		[SerializeField]
		public FactionAttributesAsset m_FactionAttributes;

		// Token: 0x04000351 RID: 849
		[SerializeField]
		public StartingUnit[] m_StartingUnits;

		// Token: 0x04000352 RID: 850
		[SerializeField]
		public bool m_HackedTransferCommander;

		// Token: 0x04000353 RID: 851
		[SerializeField]
		public bool m_BeginDeployed;

		// Token: 0x04000354 RID: 852
		[SerializeField]
		public bool m_WaitForDeployCommand;

		// Token: 0x04000355 RID: 853
		[SerializeField]
		public bool m_UseAggroAlert;

		// Token: 0x04000356 RID: 854
		[SerializeField]
		public bool m_MaintainAggroThroughFOW;

		// Token: 0x04000357 RID: 855
		[SerializeField]
		public SpawnFormationData[] m_OverriddenSpawnFormations = new SpawnFormationData[0];

		// Token: 0x04000358 RID: 856
		[ObjectName(typeof(ResearchItemAttributesAsset))]
		[SerializeField]
		public string[] m_StartingTech;

		// Token: 0x04000359 RID: 857
		[SerializeField]
		public string[] m_LockedTech;

		// Token: 0x0400035A RID: 858
		[SerializeField]
		private UnitTypeBuffData[] m_StartingBuffs;

		// Token: 0x0400035B RID: 859
		[SerializeField]
		[Tooltip("Abilities that are granted to this Commander at the start of the game, which override an ability's StartsRemovedInGameMode setting")]
		[ObjectName(typeof(AbilityAttributesAsset))]
		public string[] m_StartingGrantedAbilities;

		// Token: 0x0400035C RID: 860
		[SerializeField]
		public string[] m_StoryArtifacts = new string[0];

		// Token: 0x0400035D RID: 861
		[SerializeField]
		public string[] m_StartingArtifactsPool = new string[0];

		// Token: 0x0400035E RID: 862
		[SerializeField]
		public int m_NumStartingArtifacts;

		// Token: 0x0400035F RID: 863
		[SerializeField]
		public AIArchetypeAttributesAsset[] m_AIArchetypeAssets;

		// Token: 0x04000360 RID: 864
		public AIArchetypeAttributes[] m_AIArchetypes;
	}
}
