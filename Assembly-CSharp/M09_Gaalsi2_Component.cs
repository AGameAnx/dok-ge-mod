using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Scripting;
using UnityEngine;

// Token: 0x0200086D RID: 2157
[AddComponentMenu("uScript/Graphs/M09_Gaalsi2")]
public class M09_Gaalsi2_Component : uScriptCode
{
	// Token: 0x0600DD04 RID: 56580 RVA: 0x003F5460 File Offset: 0x003F3660
	public M09_Gaalsi2_Component()
	{
	}

	// Token: 0x170007FC RID: 2044
	// (get) Token: 0x0600DD05 RID: 56581 RVA: 0x003F5474 File Offset: 0x003F3674
	// (set) Token: 0x0600DD06 RID: 56582 RVA: 0x003F5484 File Offset: 0x003F3684
	public UnitSpawnWaveData GaalsiAircraft
	{
		get
		{
			return this.ExposedVariables.GaalsiAircraft;
		}
		set
		{
			this.ExposedVariables.GaalsiAircraft = value;
		}
	}

	// Token: 0x170007FD RID: 2045
	// (get) Token: 0x0600DD07 RID: 56583 RVA: 0x003F5494 File Offset: 0x003F3694
	// (set) Token: 0x0600DD08 RID: 56584 RVA: 0x003F54A4 File Offset: 0x003F36A4
	public UnitSpawnWaveData G_CarrierGuard2
	{
		get
		{
			return this.ExposedVariables.G_CarrierGuard2;
		}
		set
		{
			this.ExposedVariables.G_CarrierGuard2 = value;
		}
	}

	// Token: 0x170007FE RID: 2046
	// (get) Token: 0x0600DD09 RID: 56585 RVA: 0x003F54B4 File Offset: 0x003F36B4
	// (set) Token: 0x0600DD0A RID: 56586 RVA: 0x003F54C4 File Offset: 0x003F36C4
	public UnitSpawnWaveData G_StarCatSupportGroup
	{
		get
		{
			return this.ExposedVariables.G_StarCatSupportGroup;
		}
		set
		{
			this.ExposedVariables.G_StarCatSupportGroup = value;
		}
	}

	// Token: 0x170007FF RID: 2047
	// (get) Token: 0x0600DD0B RID: 56587 RVA: 0x003F54D4 File Offset: 0x003F36D4
	// (set) Token: 0x0600DD0C RID: 56588 RVA: 0x003F54E4 File Offset: 0x003F36E4
	public UnitSpawnWaveData G_SkimmerGroup
	{
		get
		{
			return this.ExposedVariables.G_SkimmerGroup;
		}
		set
		{
			this.ExposedVariables.G_SkimmerGroup = value;
		}
	}

	// Token: 0x17000800 RID: 2048
	// (get) Token: 0x0600DD0D RID: 56589 RVA: 0x003F54F4 File Offset: 0x003F36F4
	// (set) Token: 0x0600DD0E RID: 56590 RVA: 0x003F5504 File Offset: 0x003F3704
	public QueryContainer QueryGaalsiCarrier
	{
		get
		{
			return this.ExposedVariables.QueryGaalsiCarrier;
		}
		set
		{
			this.ExposedVariables.QueryGaalsiCarrier = value;
		}
	}

	// Token: 0x17000801 RID: 2049
	// (get) Token: 0x0600DD0F RID: 56591 RVA: 0x003F5514 File Offset: 0x003F3714
	// (set) Token: 0x0600DD10 RID: 56592 RVA: 0x003F5524 File Offset: 0x003F3724
	public UnitSpawnWaveData GC2_PatrolGroup
	{
		get
		{
			return this.ExposedVariables.GC2_PatrolGroup;
		}
		set
		{
			this.ExposedVariables.GC2_PatrolGroup = value;
		}
	}

	// Token: 0x17000802 RID: 2050
	// (get) Token: 0x0600DD11 RID: 56593 RVA: 0x003F5534 File Offset: 0x003F3734
	// (set) Token: 0x0600DD12 RID: 56594 RVA: 0x003F5544 File Offset: 0x003F3744
	public QueryContainer SearchGCAR1
	{
		get
		{
			return this.ExposedVariables.SearchGCAR1;
		}
		set
		{
			this.ExposedVariables.SearchGCAR1 = value;
		}
	}

	// Token: 0x17000803 RID: 2051
	// (get) Token: 0x0600DD13 RID: 56595 RVA: 0x003F5554 File Offset: 0x003F3754
	// (set) Token: 0x0600DD14 RID: 56596 RVA: 0x003F5564 File Offset: 0x003F3764
	public QueryContainer SearchGCAR2
	{
		get
		{
			return this.ExposedVariables.SearchGCAR2;
		}
		set
		{
			this.ExposedVariables.SearchGCAR2 = value;
		}
	}

	// Token: 0x17000804 RID: 2052
	// (get) Token: 0x0600DD15 RID: 56597 RVA: 0x003F5574 File Offset: 0x003F3774
	// (set) Token: 0x0600DD16 RID: 56598 RVA: 0x003F5584 File Offset: 0x003F3784
	public QueryContainer SearchSakala
	{
		get
		{
			return this.ExposedVariables.SearchSakala;
		}
		set
		{
			this.ExposedVariables.SearchSakala = value;
		}
	}

	// Token: 0x17000805 RID: 2053
	// (get) Token: 0x0600DD17 RID: 56599 RVA: 0x003F5594 File Offset: 0x003F3794
	// (set) Token: 0x0600DD18 RID: 56600 RVA: 0x003F55A4 File Offset: 0x003F37A4
	public QueryContainer SearchKapisi
	{
		get
		{
			return this.ExposedVariables.SearchKapisi;
		}
		set
		{
			this.ExposedVariables.SearchKapisi = value;
		}
	}

	// Token: 0x17000806 RID: 2054
	// (get) Token: 0x0600DD19 RID: 56601 RVA: 0x003F55B4 File Offset: 0x003F37B4
	// (set) Token: 0x0600DD1A RID: 56602 RVA: 0x003F55C4 File Offset: 0x003F37C4
	public UnitSpawnWaveData SupportCruiserDefensiveUnits
	{
		get
		{
			return this.ExposedVariables.SupportCruiserDefensiveUnits;
		}
		set
		{
			this.ExposedVariables.SupportCruiserDefensiveUnits = value;
		}
	}

	// Token: 0x17000807 RID: 2055
	// (get) Token: 0x0600DD1B RID: 56603 RVA: 0x003F55D4 File Offset: 0x003F37D4
	// (set) Token: 0x0600DD1C RID: 56604 RVA: 0x003F55E4 File Offset: 0x003F37E4
	public UnitSpawnWaveData G1PC_Patrol
	{
		get
		{
			return this.ExposedVariables.G1PC_Patrol;
		}
		set
		{
			this.ExposedVariables.G1PC_Patrol = value;
		}
	}

	// Token: 0x17000808 RID: 2056
	// (get) Token: 0x0600DD1D RID: 56605 RVA: 0x003F55F4 File Offset: 0x003F37F4
	// (set) Token: 0x0600DD1E RID: 56606 RVA: 0x003F5604 File Offset: 0x003F3804
	public UnitSpawnWaveData GC1_PatrolGroup
	{
		get
		{
			return this.ExposedVariables.GC1_PatrolGroup;
		}
		set
		{
			this.ExposedVariables.GC1_PatrolGroup = value;
		}
	}

	// Token: 0x17000809 RID: 2057
	// (get) Token: 0x0600DD1F RID: 56607 RVA: 0x003F5614 File Offset: 0x003F3814
	// (set) Token: 0x0600DD20 RID: 56608 RVA: 0x003F5624 File Offset: 0x003F3824
	public UnitSpawnWaveData G2PC_Patrol
	{
		get
		{
			return this.ExposedVariables.G2PC_Patrol;
		}
		set
		{
			this.ExposedVariables.G2PC_Patrol = value;
		}
	}

	// Token: 0x1700080A RID: 2058
	// (get) Token: 0x0600DD21 RID: 56609 RVA: 0x003F5634 File Offset: 0x003F3834
	// (set) Token: 0x0600DD22 RID: 56610 RVA: 0x003F5644 File Offset: 0x003F3844
	public UnitSpawnWaveData TypeInterceptor
	{
		get
		{
			return this.ExposedVariables.TypeInterceptor;
		}
		set
		{
			this.ExposedVariables.TypeInterceptor = value;
		}
	}

	// Token: 0x1700080B RID: 2059
	// (get) Token: 0x0600DD23 RID: 56611 RVA: 0x003F5654 File Offset: 0x003F3854
	// (set) Token: 0x0600DD24 RID: 56612 RVA: 0x003F5664 File Offset: 0x003F3864
	public QueryContainer LookForEnemies
	{
		get
		{
			return this.ExposedVariables.LookForEnemies;
		}
		set
		{
			this.ExposedVariables.LookForEnemies = value;
		}
	}

	// Token: 0x1700080C RID: 2060
	// (get) Token: 0x0600DD25 RID: 56613 RVA: 0x003F5674 File Offset: 0x003F3874
	// (set) Token: 0x0600DD26 RID: 56614 RVA: 0x003F5684 File Offset: 0x003F3884
	public SpawnFactoryData Carrier1SpawnFactory
	{
		get
		{
			return this.ExposedVariables.Carrier1SpawnFactory;
		}
		set
		{
			this.ExposedVariables.Carrier1SpawnFactory = value;
		}
	}

	// Token: 0x1700080D RID: 2061
	// (get) Token: 0x0600DD27 RID: 56615 RVA: 0x003F5694 File Offset: 0x003F3894
	// (set) Token: 0x0600DD28 RID: 56616 RVA: 0x003F56A4 File Offset: 0x003F38A4
	public SpawnFactoryData Carrier2SpawnFactory
	{
		get
		{
			return this.ExposedVariables.Carrier2SpawnFactory;
		}
		set
		{
			this.ExposedVariables.Carrier2SpawnFactory = value;
		}
	}

	// Token: 0x1700080E RID: 2062
	// (get) Token: 0x0600DD29 RID: 56617 RVA: 0x003F56B4 File Offset: 0x003F38B4
	// (set) Token: 0x0600DD2A RID: 56618 RVA: 0x003F56C4 File Offset: 0x003F38C4
	public UnitSpawnWaveData TypeBomber
	{
		get
		{
			return this.ExposedVariables.TypeBomber;
		}
		set
		{
			this.ExposedVariables.TypeBomber = value;
		}
	}

	// Token: 0x1700080F RID: 2063
	// (get) Token: 0x0600DD2B RID: 56619 RVA: 0x003F56D4 File Offset: 0x003F38D4
	// (set) Token: 0x0600DD2C RID: 56620 RVA: 0x003F56E4 File Offset: 0x003F38E4
	public UnitSpawnWaveData DesperationUnits
	{
		get
		{
			return this.ExposedVariables.DesperationUnits;
		}
		set
		{
			this.ExposedVariables.DesperationUnits = value;
		}
	}

	// Token: 0x17000810 RID: 2064
	// (get) Token: 0x0600DD2D RID: 56621 RVA: 0x003F56F4 File Offset: 0x003F38F4
	// (set) Token: 0x0600DD2E RID: 56622 RVA: 0x003F5704 File Offset: 0x003F3904
	public QueryContainer FindGCarrier
	{
		get
		{
			return this.ExposedVariables.FindGCarrier;
		}
		set
		{
			this.ExposedVariables.FindGCarrier = value;
		}
	}

	// Token: 0x17000811 RID: 2065
	// (get) Token: 0x0600DD2F RID: 56623 RVA: 0x003F5714 File Offset: 0x003F3914
	// (set) Token: 0x0600DD30 RID: 56624 RVA: 0x003F5724 File Offset: 0x003F3924
	public UnitSpawnWaveData GaalsiAirDefense
	{
		get
		{
			return this.ExposedVariables.GaalsiAirDefense;
		}
		set
		{
			this.ExposedVariables.GaalsiAirDefense = value;
		}
	}

	// Token: 0x17000812 RID: 2066
	// (get) Token: 0x0600DD31 RID: 56625 RVA: 0x003F5734 File Offset: 0x003F3934
	// (set) Token: 0x0600DD32 RID: 56626 RVA: 0x003F5744 File Offset: 0x003F3944
	public QueryContainer GAAUnitsfromStart
	{
		get
		{
			return this.ExposedVariables.GAAUnitsfromStart;
		}
		set
		{
			this.ExposedVariables.GAAUnitsfromStart = value;
		}
	}

	// Token: 0x17000813 RID: 2067
	// (get) Token: 0x0600DD33 RID: 56627 RVA: 0x003F5754 File Offset: 0x003F3954
	// (set) Token: 0x0600DD34 RID: 56628 RVA: 0x003F5764 File Offset: 0x003F3964
	public QueryContainer SearchGaalsiProd
	{
		get
		{
			return this.ExposedVariables.SearchGaalsiProd;
		}
		set
		{
			this.ExposedVariables.SearchGaalsiProd = value;
		}
	}

	// Token: 0x17000814 RID: 2068
	// (get) Token: 0x0600DD35 RID: 56629 RVA: 0x003F5774 File Offset: 0x003F3974
	// (set) Token: 0x0600DD36 RID: 56630 RVA: 0x003F5784 File Offset: 0x003F3984
	public UnitSpawnWaveData GProdC
	{
		get
		{
			return this.ExposedVariables.GProdC;
		}
		set
		{
			this.ExposedVariables.GProdC = value;
		}
	}

	// Token: 0x17000815 RID: 2069
	// (get) Token: 0x0600DD37 RID: 56631 RVA: 0x003F5794 File Offset: 0x003F3994
	// (set) Token: 0x0600DD38 RID: 56632 RVA: 0x003F57A4 File Offset: 0x003F39A4
	public UnitSpawnWaveData GC1ReactAirUnits
	{
		get
		{
			return this.ExposedVariables.GC1ReactAirUnits;
		}
		set
		{
			this.ExposedVariables.GC1ReactAirUnits = value;
		}
	}

	// Token: 0x17000816 RID: 2070
	// (get) Token: 0x0600DD39 RID: 56633 RVA: 0x003F57B4 File Offset: 0x003F39B4
	// (set) Token: 0x0600DD3A RID: 56634 RVA: 0x003F57C4 File Offset: 0x003F39C4
	public QueryContainer RemoveCarrier
	{
		get
		{
			return this.ExposedVariables.RemoveCarrier;
		}
		set
		{
			this.ExposedVariables.RemoveCarrier = value;
		}
	}

	// Token: 0x17000817 RID: 2071
	// (get) Token: 0x0600DD3B RID: 56635 RVA: 0x003F57D4 File Offset: 0x003F39D4
	// (set) Token: 0x0600DD3C RID: 56636 RVA: 0x003F57E4 File Offset: 0x003F39E4
	public UnitSpawnWaveData GC2ReactAirUnits
	{
		get
		{
			return this.ExposedVariables.GC2ReactAirUnits;
		}
		set
		{
			this.ExposedVariables.GC2ReactAirUnits = value;
		}
	}

	// Token: 0x17000818 RID: 2072
	// (get) Token: 0x0600DD3D RID: 56637 RVA: 0x003F57F4 File Offset: 0x003F39F4
	// (set) Token: 0x0600DD3E RID: 56638 RVA: 0x003F5804 File Offset: 0x003F3A04
	public SpawnFactoryData GC2ReactAntiAirFac
	{
		get
		{
			return this.ExposedVariables.GC2ReactAntiAirFac;
		}
		set
		{
			this.ExposedVariables.GC2ReactAntiAirFac = value;
		}
	}

	// Token: 0x17000819 RID: 2073
	// (get) Token: 0x0600DD3F RID: 56639 RVA: 0x003F5814 File Offset: 0x003F3A14
	// (set) Token: 0x0600DD40 RID: 56640 RVA: 0x003F5824 File Offset: 0x003F3A24
	public SpawnFactoryData GC2ReactionGroundForceFactory
	{
		get
		{
			return this.ExposedVariables.GC2ReactionGroundForceFactory;
		}
		set
		{
			this.ExposedVariables.GC2ReactionGroundForceFactory = value;
		}
	}

	// Token: 0x1700081A RID: 2074
	// (get) Token: 0x0600DD41 RID: 56641 RVA: 0x003F5834 File Offset: 0x003F3A34
	// (set) Token: 0x0600DD42 RID: 56642 RVA: 0x003F5844 File Offset: 0x003F3A44
	public SpawnFactoryData GC1ReactAirUnitFac
	{
		get
		{
			return this.ExposedVariables.GC1ReactAirUnitFac;
		}
		set
		{
			this.ExposedVariables.GC1ReactAirUnitFac = value;
		}
	}

	// Token: 0x1700081B RID: 2075
	// (get) Token: 0x0600DD43 RID: 56643 RVA: 0x003F5854 File Offset: 0x003F3A54
	// (set) Token: 0x0600DD44 RID: 56644 RVA: 0x003F5864 File Offset: 0x003F3A64
	public SpawnFactoryData GC1ReactGForceFac
	{
		get
		{
			return this.ExposedVariables.GC1ReactGForceFac;
		}
		set
		{
			this.ExposedVariables.GC1ReactGForceFac = value;
		}
	}

	// Token: 0x1700081C RID: 2076
	// (get) Token: 0x0600DD45 RID: 56645 RVA: 0x003F5874 File Offset: 0x003F3A74
	// (set) Token: 0x0600DD46 RID: 56646 RVA: 0x003F5884 File Offset: 0x003F3A84
	public QueryContainer PlayerHACQuery
	{
		get
		{
			return this.ExposedVariables.PlayerHACQuery;
		}
		set
		{
			this.ExposedVariables.PlayerHACQuery = value;
		}
	}

	// Token: 0x1700081D RID: 2077
	// (get) Token: 0x0600DD47 RID: 56647 RVA: 0x003F5894 File Offset: 0x003F3A94
	// (set) Token: 0x0600DD48 RID: 56648 RVA: 0x003F58A4 File Offset: 0x003F3AA4
	public UnitSpawnWaveData FinalGaalsiSandskimmerWave
	{
		get
		{
			return this.ExposedVariables.FinalGaalsiSandskimmerWave;
		}
		set
		{
			this.ExposedVariables.FinalGaalsiSandskimmerWave = value;
		}
	}

	// Token: 0x1700081E RID: 2078
	// (get) Token: 0x0600DD49 RID: 56649 RVA: 0x003F58B4 File Offset: 0x003F3AB4
	// (set) Token: 0x0600DD4A RID: 56650 RVA: 0x003F58C4 File Offset: 0x003F3AC4
	public UnitSpawnWaveData FinalGaalsiStarhullWave
	{
		get
		{
			return this.ExposedVariables.FinalGaalsiStarhullWave;
		}
		set
		{
			this.ExposedVariables.FinalGaalsiStarhullWave = value;
		}
	}

	// Token: 0x1700081F RID: 2079
	// (get) Token: 0x0600DD4B RID: 56651 RVA: 0x003F58D4 File Offset: 0x003F3AD4
	// (set) Token: 0x0600DD4C RID: 56652 RVA: 0x003F58E4 File Offset: 0x003F3AE4
	public UnitSpawnWaveData LeftW1GSH
	{
		get
		{
			return this.ExposedVariables.LeftW1GSH;
		}
		set
		{
			this.ExposedVariables.LeftW1GSH = value;
		}
	}

	// Token: 0x17000820 RID: 2080
	// (get) Token: 0x0600DD4D RID: 56653 RVA: 0x003F58F4 File Offset: 0x003F3AF4
	// (set) Token: 0x0600DD4E RID: 56654 RVA: 0x003F5904 File Offset: 0x003F3B04
	public UnitSpawnWaveData LeftW1GCAT
	{
		get
		{
			return this.ExposedVariables.LeftW1GCAT;
		}
		set
		{
			this.ExposedVariables.LeftW1GCAT = value;
		}
	}

	// Token: 0x17000821 RID: 2081
	// (get) Token: 0x0600DD4F RID: 56655 RVA: 0x003F5914 File Offset: 0x003F3B14
	// (set) Token: 0x0600DD50 RID: 56656 RVA: 0x003F5924 File Offset: 0x003F3B24
	public UnitSpawnWaveData LeftW1GSS
	{
		get
		{
			return this.ExposedVariables.LeftW1GSS;
		}
		set
		{
			this.ExposedVariables.LeftW1GSS = value;
		}
	}

	// Token: 0x17000822 RID: 2082
	// (get) Token: 0x0600DD51 RID: 56657 RVA: 0x003F5934 File Offset: 0x003F3B34
	// (set) Token: 0x0600DD52 RID: 56658 RVA: 0x003F5944 File Offset: 0x003F3B44
	public UnitSpawnWaveData LeftW1GSHU1
	{
		get
		{
			return this.ExposedVariables.LeftW1GSHU1;
		}
		set
		{
			this.ExposedVariables.LeftW1GSHU1 = value;
		}
	}

	// Token: 0x17000823 RID: 2083
	// (get) Token: 0x0600DD53 RID: 56659 RVA: 0x003F5954 File Offset: 0x003F3B54
	// (set) Token: 0x0600DD54 RID: 56660 RVA: 0x003F5964 File Offset: 0x003F3B64
	public QueryContainer LookforLSquad
	{
		get
		{
			return this.ExposedVariables.LookforLSquad;
		}
		set
		{
			this.ExposedVariables.LookforLSquad = value;
		}
	}

	// Token: 0x17000824 RID: 2084
	// (get) Token: 0x0600DD55 RID: 56661 RVA: 0x003F5974 File Offset: 0x003F3B74
	// (set) Token: 0x0600DD56 RID: 56662 RVA: 0x003F5984 File Offset: 0x003F3B84
	public QueryContainer Look4ProdC
	{
		get
		{
			return this.ExposedVariables.Look4ProdC;
		}
		set
		{
			this.ExposedVariables.Look4ProdC = value;
		}
	}

	// Token: 0x17000825 RID: 2085
	// (get) Token: 0x0600DD57 RID: 56663 RVA: 0x003F5994 File Offset: 0x003F3B94
	// (set) Token: 0x0600DD58 RID: 56664 RVA: 0x003F59A4 File Offset: 0x003F3BA4
	public QueryContainer CallG2Units
	{
		get
		{
			return this.ExposedVariables.CallG2Units;
		}
		set
		{
			this.ExposedVariables.CallG2Units = value;
		}
	}

	// Token: 0x17000826 RID: 2086
	// (get) Token: 0x0600DD59 RID: 56665 RVA: 0x003F59B4 File Offset: 0x003F3BB4
	// (set) Token: 0x0600DD5A RID: 56666 RVA: 0x003F59C4 File Offset: 0x003F3BC4
	public QueryContainer CallG1Units
	{
		get
		{
			return this.ExposedVariables.CallG1Units;
		}
		set
		{
			this.ExposedVariables.CallG1Units = value;
		}
	}

	// Token: 0x0600DD5B RID: 56667 RVA: 0x003F59D4 File Offset: 0x003F3BD4
	private void Awake()
	{
		base.useGUILayout = false;
		this.ExposedVariables.Awake();
		this.ExposedVariables.SetParent(base.gameObject);
		if ("1.CMR" != uScript_MasterComponent.Version)
		{
			uScriptDebug.Log("The generated code is not compatible with your current uScript Runtime " + uScript_MasterComponent.Version, uScriptDebug.Type.Error);
			this.ExposedVariables = null;
			Debug.Break();
		}
	}

	// Token: 0x0600DD5C RID: 56668 RVA: 0x003F5A3C File Offset: 0x003F3C3C
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600DD5D RID: 56669 RVA: 0x003F5A4C File Offset: 0x003F3C4C
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600DD5E RID: 56670 RVA: 0x003F5A5C File Offset: 0x003F3C5C
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600DD5F RID: 56671 RVA: 0x003F5A6C File Offset: 0x003F3C6C
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600DD60 RID: 56672 RVA: 0x003F5A7C File Offset: 0x003F3C7C
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401508E RID: 86158
	public M09_Gaalsi2 ExposedVariables = new M09_Gaalsi2();
}
