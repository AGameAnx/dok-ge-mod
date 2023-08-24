using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Scripting;
using UnityEngine;

// Token: 0x0200086E RID: 2158
[AddComponentMenu("uScript/Graphs/M09_Gaalsi")]
public class M09_Gaalsi_Component : uScriptCode
{
	// Token: 0x0600DD61 RID: 56673 RVA: 0x003F5A8C File Offset: 0x003F3C8C
	public M09_Gaalsi_Component()
	{
	}

	// Token: 0x17000827 RID: 2087
	// (get) Token: 0x0600DD62 RID: 56674 RVA: 0x003F5AA0 File Offset: 0x003F3CA0
	// (set) Token: 0x0600DD63 RID: 56675 RVA: 0x003F5AB0 File Offset: 0x003F3CB0
	public UnitSpawnWaveData GaalsiStart_Railgun1
	{
		get
		{
			return this.ExposedVariables.GaalsiStart_Railgun1;
		}
		set
		{
			this.ExposedVariables.GaalsiStart_Railgun1 = value;
		}
	}

	// Token: 0x17000828 RID: 2088
	// (get) Token: 0x0600DD64 RID: 56676 RVA: 0x003F5AC0 File Offset: 0x003F3CC0
	// (set) Token: 0x0600DD65 RID: 56677 RVA: 0x003F5AD0 File Offset: 0x003F3CD0
	public List<Entity> Gaalsi_StartingEscorts2
	{
		get
		{
			return this.ExposedVariables.Gaalsi_StartingEscorts2;
		}
		set
		{
			this.ExposedVariables.Gaalsi_StartingEscorts2 = value;
		}
	}

	// Token: 0x17000829 RID: 2089
	// (get) Token: 0x0600DD66 RID: 56678 RVA: 0x003F5AE0 File Offset: 0x003F3CE0
	// (set) Token: 0x0600DD67 RID: 56679 RVA: 0x003F5AF0 File Offset: 0x003F3CF0
	public UnitSpawnWaveData GaalsiStart_Escort1
	{
		get
		{
			return this.ExposedVariables.GaalsiStart_Escort1;
		}
		set
		{
			this.ExposedVariables.GaalsiStart_Escort1 = value;
		}
	}

	// Token: 0x1700082A RID: 2090
	// (get) Token: 0x0600DD68 RID: 56680 RVA: 0x003F5B00 File Offset: 0x003F3D00
	// (set) Token: 0x0600DD69 RID: 56681 RVA: 0x003F5B10 File Offset: 0x003F3D10
	public List<Entity> Gaalsi_StartingRailguns2
	{
		get
		{
			return this.ExposedVariables.Gaalsi_StartingRailguns2;
		}
		set
		{
			this.ExposedVariables.Gaalsi_StartingRailguns2 = value;
		}
	}

	// Token: 0x1700082B RID: 2091
	// (get) Token: 0x0600DD6A RID: 56682 RVA: 0x003F5B20 File Offset: 0x003F3D20
	// (set) Token: 0x0600DD6B RID: 56683 RVA: 0x003F5B30 File Offset: 0x003F3D30
	public List<Entity> Gaalsi_StartingRailguns1
	{
		get
		{
			return this.ExposedVariables.Gaalsi_StartingRailguns1;
		}
		set
		{
			this.ExposedVariables.Gaalsi_StartingRailguns1 = value;
		}
	}

	// Token: 0x1700082C RID: 2092
	// (get) Token: 0x0600DD6C RID: 56684 RVA: 0x003F5B40 File Offset: 0x003F3D40
	// (set) Token: 0x0600DD6D RID: 56685 RVA: 0x003F5B50 File Offset: 0x003F3D50
	public List<Entity> Gaalsi_StartingHACs1
	{
		get
		{
			return this.ExposedVariables.Gaalsi_StartingHACs1;
		}
		set
		{
			this.ExposedVariables.Gaalsi_StartingHACs1 = value;
		}
	}

	// Token: 0x1700082D RID: 2093
	// (get) Token: 0x0600DD6E RID: 56686 RVA: 0x003F5B60 File Offset: 0x003F3D60
	// (set) Token: 0x0600DD6F RID: 56687 RVA: 0x003F5B70 File Offset: 0x003F3D70
	public UnitSpawnWaveData GaalsiStart_HAC2
	{
		get
		{
			return this.ExposedVariables.GaalsiStart_HAC2;
		}
		set
		{
			this.ExposedVariables.GaalsiStart_HAC2 = value;
		}
	}

	// Token: 0x1700082E RID: 2094
	// (get) Token: 0x0600DD70 RID: 56688 RVA: 0x003F5B80 File Offset: 0x003F3D80
	// (set) Token: 0x0600DD71 RID: 56689 RVA: 0x003F5B90 File Offset: 0x003F3D90
	public List<Entity> Gaalsi_StartingEscorts1
	{
		get
		{
			return this.ExposedVariables.Gaalsi_StartingEscorts1;
		}
		set
		{
			this.ExposedVariables.Gaalsi_StartingEscorts1 = value;
		}
	}

	// Token: 0x1700082F RID: 2095
	// (get) Token: 0x0600DD72 RID: 56690 RVA: 0x003F5BA0 File Offset: 0x003F3DA0
	// (set) Token: 0x0600DD73 RID: 56691 RVA: 0x003F5BB0 File Offset: 0x003F3DB0
	public UnitSpawnWaveData GaalsiStart_Carrier1
	{
		get
		{
			return this.ExposedVariables.GaalsiStart_Carrier1;
		}
		set
		{
			this.ExposedVariables.GaalsiStart_Carrier1 = value;
		}
	}

	// Token: 0x17000830 RID: 2096
	// (get) Token: 0x0600DD74 RID: 56692 RVA: 0x003F5BC0 File Offset: 0x003F3DC0
	// (set) Token: 0x0600DD75 RID: 56693 RVA: 0x003F5BD0 File Offset: 0x003F3DD0
	public UnitSpawnWaveData GaalsiStart_Railgun2
	{
		get
		{
			return this.ExposedVariables.GaalsiStart_Railgun2;
		}
		set
		{
			this.ExposedVariables.GaalsiStart_Railgun2 = value;
		}
	}

	// Token: 0x17000831 RID: 2097
	// (get) Token: 0x0600DD76 RID: 56694 RVA: 0x003F5BE0 File Offset: 0x003F3DE0
	// (set) Token: 0x0600DD77 RID: 56695 RVA: 0x003F5BF0 File Offset: 0x003F3DF0
	public UnitSpawnWaveData GaalsiStart_Carrier2
	{
		get
		{
			return this.ExposedVariables.GaalsiStart_Carrier2;
		}
		set
		{
			this.ExposedVariables.GaalsiStart_Carrier2 = value;
		}
	}

	// Token: 0x17000832 RID: 2098
	// (get) Token: 0x0600DD78 RID: 56696 RVA: 0x003F5C00 File Offset: 0x003F3E00
	// (set) Token: 0x0600DD79 RID: 56697 RVA: 0x003F5C10 File Offset: 0x003F3E10
	public UnitSpawnWaveData GaalsiStart_Escort2
	{
		get
		{
			return this.ExposedVariables.GaalsiStart_Escort2;
		}
		set
		{
			this.ExposedVariables.GaalsiStart_Escort2 = value;
		}
	}

	// Token: 0x17000833 RID: 2099
	// (get) Token: 0x0600DD7A RID: 56698 RVA: 0x003F5C20 File Offset: 0x003F3E20
	// (set) Token: 0x0600DD7B RID: 56699 RVA: 0x003F5C30 File Offset: 0x003F3E30
	public List<Entity> Gaalsi_StartingHACs2
	{
		get
		{
			return this.ExposedVariables.Gaalsi_StartingHACs2;
		}
		set
		{
			this.ExposedVariables.Gaalsi_StartingHACs2 = value;
		}
	}

	// Token: 0x17000834 RID: 2100
	// (get) Token: 0x0600DD7C RID: 56700 RVA: 0x003F5C40 File Offset: 0x003F3E40
	// (set) Token: 0x0600DD7D RID: 56701 RVA: 0x003F5C50 File Offset: 0x003F3E50
	public List<Entity> Gaalsi_StartingBaserunners2
	{
		get
		{
			return this.ExposedVariables.Gaalsi_StartingBaserunners2;
		}
		set
		{
			this.ExposedVariables.Gaalsi_StartingBaserunners2 = value;
		}
	}

	// Token: 0x17000835 RID: 2101
	// (get) Token: 0x0600DD7E RID: 56702 RVA: 0x003F5C60 File Offset: 0x003F3E60
	// (set) Token: 0x0600DD7F RID: 56703 RVA: 0x003F5C70 File Offset: 0x003F3E70
	public UnitSpawnWaveData TwoStarhulls
	{
		get
		{
			return this.ExposedVariables.TwoStarhulls;
		}
		set
		{
			this.ExposedVariables.TwoStarhulls = value;
		}
	}

	// Token: 0x17000836 RID: 2102
	// (get) Token: 0x0600DD80 RID: 56704 RVA: 0x003F5C80 File Offset: 0x003F3E80
	// (set) Token: 0x0600DD81 RID: 56705 RVA: 0x003F5C90 File Offset: 0x003F3E90
	public UnitSpawnWaveData GaalsiStart_HAC1
	{
		get
		{
			return this.ExposedVariables.GaalsiStart_HAC1;
		}
		set
		{
			this.ExposedVariables.GaalsiStart_HAC1 = value;
		}
	}

	// Token: 0x17000837 RID: 2103
	// (get) Token: 0x0600DD82 RID: 56706 RVA: 0x003F5CA0 File Offset: 0x003F3EA0
	// (set) Token: 0x0600DD83 RID: 56707 RVA: 0x003F5CB0 File Offset: 0x003F3EB0
	public UnitSpawnWaveData GaalsiStart_AA
	{
		get
		{
			return this.ExposedVariables.GaalsiStart_AA;
		}
		set
		{
			this.ExposedVariables.GaalsiStart_AA = value;
		}
	}

	// Token: 0x17000838 RID: 2104
	// (get) Token: 0x0600DD84 RID: 56708 RVA: 0x003F5CC0 File Offset: 0x003F3EC0
	// (set) Token: 0x0600DD85 RID: 56709 RVA: 0x003F5CD0 File Offset: 0x003F3ED0
	public SpawnFactoryData FirstAttackers1
	{
		get
		{
			return this.ExposedVariables.FirstAttackers1;
		}
		set
		{
			this.ExposedVariables.FirstAttackers1 = value;
		}
	}

	// Token: 0x17000839 RID: 2105
	// (get) Token: 0x0600DD86 RID: 56710 RVA: 0x003F5CE0 File Offset: 0x003F3EE0
	// (set) Token: 0x0600DD87 RID: 56711 RVA: 0x003F5CF0 File Offset: 0x003F3EF0
	public SpawnFactoryData FirstAttackers2
	{
		get
		{
			return this.ExposedVariables.FirstAttackers2;
		}
		set
		{
			this.ExposedVariables.FirstAttackers2 = value;
		}
	}

	// Token: 0x1700083A RID: 2106
	// (get) Token: 0x0600DD88 RID: 56712 RVA: 0x003F5D00 File Offset: 0x003F3F00
	// (set) Token: 0x0600DD89 RID: 56713 RVA: 0x003F5D10 File Offset: 0x003F3F10
	public UnitSpawnWaveData G_CanyonPatrol
	{
		get
		{
			return this.ExposedVariables.G_CanyonPatrol;
		}
		set
		{
			this.ExposedVariables.G_CanyonPatrol = value;
		}
	}

	// Token: 0x1700083B RID: 2107
	// (get) Token: 0x0600DD8A RID: 56714 RVA: 0x003F5D20 File Offset: 0x003F3F20
	// (set) Token: 0x0600DD8B RID: 56715 RVA: 0x003F5D30 File Offset: 0x003F3F30
	public List<Entity> Gaalsi_StartingSC1
	{
		get
		{
			return this.ExposedVariables.Gaalsi_StartingSC1;
		}
		set
		{
			this.ExposedVariables.Gaalsi_StartingSC1 = value;
		}
	}

	// Token: 0x1700083C RID: 2108
	// (get) Token: 0x0600DD8C RID: 56716 RVA: 0x003F5D40 File Offset: 0x003F3F40
	// (set) Token: 0x0600DD8D RID: 56717 RVA: 0x003F5D50 File Offset: 0x003F3F50
	public UnitSpawnWaveData GaalsiStart_SupportCruiser
	{
		get
		{
			return this.ExposedVariables.GaalsiStart_SupportCruiser;
		}
		set
		{
			this.ExposedVariables.GaalsiStart_SupportCruiser = value;
		}
	}

	// Token: 0x1700083D RID: 2109
	// (get) Token: 0x0600DD8E RID: 56718 RVA: 0x003F5D60 File Offset: 0x003F3F60
	// (set) Token: 0x0600DD8F RID: 56719 RVA: 0x003F5D70 File Offset: 0x003F3F70
	public AttributeBuffSetData Buff_MissileBarrage1
	{
		get
		{
			return this.ExposedVariables.Buff_MissileBarrage1;
		}
		set
		{
			this.ExposedVariables.Buff_MissileBarrage1 = value;
		}
	}

	// Token: 0x1700083E RID: 2110
	// (get) Token: 0x0600DD90 RID: 56720 RVA: 0x003F5D80 File Offset: 0x003F3F80
	// (set) Token: 0x0600DD91 RID: 56721 RVA: 0x003F5D90 File Offset: 0x003F3F90
	public SpawnFactoryData FactorySkimmers
	{
		get
		{
			return this.ExposedVariables.FactorySkimmers;
		}
		set
		{
			this.ExposedVariables.FactorySkimmers = value;
		}
	}

	// Token: 0x1700083F RID: 2111
	// (get) Token: 0x0600DD92 RID: 56722 RVA: 0x003F5DA0 File Offset: 0x003F3FA0
	// (set) Token: 0x0600DD93 RID: 56723 RVA: 0x003F5DB0 File Offset: 0x003F3FB0
	public QueryContainer QueryFirstAttackers
	{
		get
		{
			return this.ExposedVariables.QueryFirstAttackers;
		}
		set
		{
			this.ExposedVariables.QueryFirstAttackers = value;
		}
	}

	// Token: 0x17000840 RID: 2112
	// (get) Token: 0x0600DD94 RID: 56724 RVA: 0x003F5DC0 File Offset: 0x003F3FC0
	// (set) Token: 0x0600DD95 RID: 56725 RVA: 0x003F5DD0 File Offset: 0x003F3FD0
	public UnitSpawnWaveData SupportCruiser1Units
	{
		get
		{
			return this.ExposedVariables.SupportCruiser1Units;
		}
		set
		{
			this.ExposedVariables.SupportCruiser1Units = value;
		}
	}

	// Token: 0x17000841 RID: 2113
	// (get) Token: 0x0600DD96 RID: 56726 RVA: 0x003F5DE0 File Offset: 0x003F3FE0
	// (set) Token: 0x0600DD97 RID: 56727 RVA: 0x003F5DF0 File Offset: 0x003F3FF0
	public UnitSpawnWaveData SupportCruiser2Units
	{
		get
		{
			return this.ExposedVariables.SupportCruiser2Units;
		}
		set
		{
			this.ExposedVariables.SupportCruiser2Units = value;
		}
	}

	// Token: 0x17000842 RID: 2114
	// (get) Token: 0x0600DD98 RID: 56728 RVA: 0x003F5E00 File Offset: 0x003F4000
	// (set) Token: 0x0600DD99 RID: 56729 RVA: 0x003F5E10 File Offset: 0x003F4010
	public QueryContainer AttackSakala
	{
		get
		{
			return this.ExposedVariables.AttackSakala;
		}
		set
		{
			this.ExposedVariables.AttackSakala = value;
		}
	}

	// Token: 0x17000843 RID: 2115
	// (get) Token: 0x0600DD9A RID: 56730 RVA: 0x003F5E20 File Offset: 0x003F4020
	// (set) Token: 0x0600DD9B RID: 56731 RVA: 0x003F5E30 File Offset: 0x003F4030
	public QueryContainer GCarrier2Search
	{
		get
		{
			return this.ExposedVariables.GCarrier2Search;
		}
		set
		{
			this.ExposedVariables.GCarrier2Search = value;
		}
	}

	// Token: 0x17000844 RID: 2116
	// (get) Token: 0x0600DD9C RID: 56732 RVA: 0x003F5E40 File Offset: 0x003F4040
	// (set) Token: 0x0600DD9D RID: 56733 RVA: 0x003F5E50 File Offset: 0x003F4050
	public QueryContainer GCarrier1Search
	{
		get
		{
			return this.ExposedVariables.GCarrier1Search;
		}
		set
		{
			this.ExposedVariables.GCarrier1Search = value;
		}
	}

	// Token: 0x17000845 RID: 2117
	// (get) Token: 0x0600DD9E RID: 56734 RVA: 0x003F5E60 File Offset: 0x003F4060
	// (set) Token: 0x0600DD9F RID: 56735 RVA: 0x003F5E70 File Offset: 0x003F4070
	public UnitSpawnWaveData Initial_G_Railguns
	{
		get
		{
			return this.ExposedVariables.Initial_G_Railguns;
		}
		set
		{
			this.ExposedVariables.Initial_G_Railguns = value;
		}
	}

	// Token: 0x17000846 RID: 2118
	// (get) Token: 0x0600DDA0 RID: 56736 RVA: 0x003F5E80 File Offset: 0x003F4080
	// (set) Token: 0x0600DDA1 RID: 56737 RVA: 0x003F5E90 File Offset: 0x003F4090
	public UnitSpawnWaveData Initial_G_Cats
	{
		get
		{
			return this.ExposedVariables.Initial_G_Cats;
		}
		set
		{
			this.ExposedVariables.Initial_G_Cats = value;
		}
	}

	// Token: 0x17000847 RID: 2119
	// (get) Token: 0x0600DDA2 RID: 56738 RVA: 0x003F5EA0 File Offset: 0x003F40A0
	// (set) Token: 0x0600DDA3 RID: 56739 RVA: 0x003F5EB0 File Offset: 0x003F40B0
	public UnitSpawnWaveData Initial_G_SandSkimmers
	{
		get
		{
			return this.ExposedVariables.Initial_G_SandSkimmers;
		}
		set
		{
			this.ExposedVariables.Initial_G_SandSkimmers = value;
		}
	}

	// Token: 0x17000848 RID: 2120
	// (get) Token: 0x0600DDA4 RID: 56740 RVA: 0x003F5EC0 File Offset: 0x003F40C0
	// (set) Token: 0x0600DDA5 RID: 56741 RVA: 0x003F5ED0 File Offset: 0x003F40D0
	public UnitSpawnWaveData Initial_BCruiser
	{
		get
		{
			return this.ExposedVariables.Initial_BCruiser;
		}
		set
		{
			this.ExposedVariables.Initial_BCruiser = value;
		}
	}

	// Token: 0x17000849 RID: 2121
	// (get) Token: 0x0600DDA6 RID: 56742 RVA: 0x003F5EE0 File Offset: 0x003F40E0
	// (set) Token: 0x0600DDA7 RID: 56743 RVA: 0x003F5EF0 File Offset: 0x003F40F0
	public QueryContainer FindSakala
	{
		get
		{
			return this.ExposedVariables.FindSakala;
		}
		set
		{
			this.ExposedVariables.FindSakala = value;
		}
	}

	// Token: 0x1700084A RID: 2122
	// (get) Token: 0x0600DDA8 RID: 56744 RVA: 0x003F5F00 File Offset: 0x003F4100
	// (set) Token: 0x0600DDA9 RID: 56745 RVA: 0x003F5F10 File Offset: 0x003F4110
	public UnitSpawnWaveData PatrolProdCruiser
	{
		get
		{
			return this.ExposedVariables.PatrolProdCruiser;
		}
		set
		{
			this.ExposedVariables.PatrolProdCruiser = value;
		}
	}

	// Token: 0x1700084B RID: 2123
	// (get) Token: 0x0600DDAA RID: 56746 RVA: 0x003F5F20 File Offset: 0x003F4120
	// (set) Token: 0x0600DDAB RID: 56747 RVA: 0x003F5F30 File Offset: 0x003F4130
	public QueryContainer FindKapisi
	{
		get
		{
			return this.ExposedVariables.FindKapisi;
		}
		set
		{
			this.ExposedVariables.FindKapisi = value;
		}
	}

	// Token: 0x1700084C RID: 2124
	// (get) Token: 0x0600DDAC RID: 56748 RVA: 0x003F5F40 File Offset: 0x003F4140
	// (set) Token: 0x0600DDAD RID: 56749 RVA: 0x003F5F50 File Offset: 0x003F4150
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

	// Token: 0x1700084D RID: 2125
	// (get) Token: 0x0600DDAE RID: 56750 RVA: 0x003F5F60 File Offset: 0x003F4160
	// (set) Token: 0x0600DDAF RID: 56751 RVA: 0x003F5F70 File Offset: 0x003F4170
	public List<Entity> Gaalsi_StartingBaserunners1
	{
		get
		{
			return this.ExposedVariables.Gaalsi_StartingBaserunners1;
		}
		set
		{
			this.ExposedVariables.Gaalsi_StartingBaserunners1 = value;
		}
	}

	// Token: 0x1700084E RID: 2126
	// (get) Token: 0x0600DDB0 RID: 56752 RVA: 0x003F5F80 File Offset: 0x003F4180
	// (set) Token: 0x0600DDB1 RID: 56753 RVA: 0x003F5F90 File Offset: 0x003F4190
	public UnitSpawnWaveData EarlyKapisiAttack
	{
		get
		{
			return this.ExposedVariables.EarlyKapisiAttack;
		}
		set
		{
			this.ExposedVariables.EarlyKapisiAttack = value;
		}
	}

	// Token: 0x1700084F RID: 2127
	// (get) Token: 0x0600DDB2 RID: 56754 RVA: 0x003F5FA0 File Offset: 0x003F41A0
	// (set) Token: 0x0600DDB3 RID: 56755 RVA: 0x003F5FB0 File Offset: 0x003F41B0
	public QueryContainer LastGCarrierLeft
	{
		get
		{
			return this.ExposedVariables.LastGCarrierLeft;
		}
		set
		{
			this.ExposedVariables.LastGCarrierLeft = value;
		}
	}

	// Token: 0x17000850 RID: 2128
	// (get) Token: 0x0600DDB4 RID: 56756 RVA: 0x003F5FC0 File Offset: 0x003F41C0
	// (set) Token: 0x0600DDB5 RID: 56757 RVA: 0x003F5FD0 File Offset: 0x003F41D0
	public UnitSpawnWaveData GaalsiStart_AA2
	{
		get
		{
			return this.ExposedVariables.GaalsiStart_AA2;
		}
		set
		{
			this.ExposedVariables.GaalsiStart_AA2 = value;
		}
	}

	// Token: 0x17000851 RID: 2129
	// (get) Token: 0x0600DDB6 RID: 56758 RVA: 0x003F5FE0 File Offset: 0x003F41E0
	// (set) Token: 0x0600DDB7 RID: 56759 RVA: 0x003F5FF0 File Offset: 0x003F41F0
	public AttributeBuffSetData BuffGC1Armor
	{
		get
		{
			return this.ExposedVariables.BuffGC1Armor;
		}
		set
		{
			this.ExposedVariables.BuffGC1Armor = value;
		}
	}

	// Token: 0x17000852 RID: 2130
	// (get) Token: 0x0600DDB8 RID: 56760 RVA: 0x003F6000 File Offset: 0x003F4200
	// (set) Token: 0x0600DDB9 RID: 56761 RVA: 0x003F6010 File Offset: 0x003F4210
	public AttributeBuffSetData BuffGC2Armor
	{
		get
		{
			return this.ExposedVariables.BuffGC2Armor;
		}
		set
		{
			this.ExposedVariables.BuffGC2Armor = value;
		}
	}

	// Token: 0x17000853 RID: 2131
	// (get) Token: 0x0600DDBA RID: 56762 RVA: 0x003F6020 File Offset: 0x003F4220
	// (set) Token: 0x0600DDBB RID: 56763 RVA: 0x003F6030 File Offset: 0x003F4230
	public AttributeBuffSetData Buff_GaalsiCarrier
	{
		get
		{
			return this.ExposedVariables.Buff_GaalsiCarrier;
		}
		set
		{
			this.ExposedVariables.Buff_GaalsiCarrier = value;
		}
	}

	// Token: 0x17000854 RID: 2132
	// (get) Token: 0x0600DDBC RID: 56764 RVA: 0x003F6040 File Offset: 0x003F4240
	// (set) Token: 0x0600DDBD RID: 56765 RVA: 0x003F6050 File Offset: 0x003F4250
	public UnitSpawnWaveData SupportCruiserUnits
	{
		get
		{
			return this.ExposedVariables.SupportCruiserUnits;
		}
		set
		{
			this.ExposedVariables.SupportCruiserUnits = value;
		}
	}

	// Token: 0x17000855 RID: 2133
	// (get) Token: 0x0600DDBE RID: 56766 RVA: 0x003F6060 File Offset: 0x003F4260
	// (set) Token: 0x0600DDBF RID: 56767 RVA: 0x003F6070 File Offset: 0x003F4270
	public List<Entity> Gaalsi_StartingSC2
	{
		get
		{
			return this.ExposedVariables.Gaalsi_StartingSC2;
		}
		set
		{
			this.ExposedVariables.Gaalsi_StartingSC2 = value;
		}
	}

	// Token: 0x17000856 RID: 2134
	// (get) Token: 0x0600DDC0 RID: 56768 RVA: 0x003F6080 File Offset: 0x003F4280
	// (set) Token: 0x0600DDC1 RID: 56769 RVA: 0x003F6090 File Offset: 0x003F4290
	public SpawnFactoryData FirstAttackers1Hard
	{
		get
		{
			return this.ExposedVariables.FirstAttackers1Hard;
		}
		set
		{
			this.ExposedVariables.FirstAttackers1Hard = value;
		}
	}

	// Token: 0x17000857 RID: 2135
	// (get) Token: 0x0600DDC2 RID: 56770 RVA: 0x003F60A0 File Offset: 0x003F42A0
	// (set) Token: 0x0600DDC3 RID: 56771 RVA: 0x003F60B0 File Offset: 0x003F42B0
	public SpawnFactoryData FirstAttackers2Hard
	{
		get
		{
			return this.ExposedVariables.FirstAttackers2Hard;
		}
		set
		{
			this.ExposedVariables.FirstAttackers2Hard = value;
		}
	}

	// Token: 0x17000858 RID: 2136
	// (get) Token: 0x0600DDC4 RID: 56772 RVA: 0x003F60C0 File Offset: 0x003F42C0
	// (set) Token: 0x0600DDC5 RID: 56773 RVA: 0x003F60D0 File Offset: 0x003F42D0
	public SpawnFactoryData FactorySkimmersHard
	{
		get
		{
			return this.ExposedVariables.FactorySkimmersHard;
		}
		set
		{
			this.ExposedVariables.FactorySkimmersHard = value;
		}
	}

	// Token: 0x0600DDC6 RID: 56774 RVA: 0x003F60E0 File Offset: 0x003F42E0
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

	// Token: 0x0600DDC7 RID: 56775 RVA: 0x003F6148 File Offset: 0x003F4348
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600DDC8 RID: 56776 RVA: 0x003F6158 File Offset: 0x003F4358
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600DDC9 RID: 56777 RVA: 0x003F6168 File Offset: 0x003F4368
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600DDCA RID: 56778 RVA: 0x003F6178 File Offset: 0x003F4378
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600DDCB RID: 56779 RVA: 0x003F6188 File Offset: 0x003F4388
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0600DDCC RID: 56780 RVA: 0x003F6198 File Offset: 0x003F4398
	private void OnGUI()
	{
		this.ExposedVariables.OnGUI();
	}

	// Token: 0x0401508F RID: 86159
	public M09_Gaalsi ExposedVariables = new M09_Gaalsi();
}
