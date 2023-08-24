using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Scripting;
using UnityEngine;

// Token: 0x02000878 RID: 2168
[AddComponentMenu("uScript/Graphs/M09_Siidim")]
public class M09_Siidim_Component : uScriptCode
{
	// Token: 0x0600F0FD RID: 61693 RVA: 0x00445B9C File Offset: 0x00443D9C
	public M09_Siidim_Component()
	{
	}

	// Token: 0x17000883 RID: 2179
	// (get) Token: 0x0600F0FE RID: 61694 RVA: 0x00445BB0 File Offset: 0x00443DB0
	// (set) Token: 0x0600F0FF RID: 61695 RVA: 0x00445BC0 File Offset: 0x00443DC0
	public AttributeBuffSetData Buff_Sakala
	{
		get
		{
			return this.ExposedVariables.Buff_Sakala;
		}
		set
		{
			this.ExposedVariables.Buff_Sakala = value;
		}
	}

	// Token: 0x17000884 RID: 2180
	// (get) Token: 0x0600F100 RID: 61696 RVA: 0x00445BD0 File Offset: 0x00443DD0
	// (set) Token: 0x0600F101 RID: 61697 RVA: 0x00445BE0 File Offset: 0x00443DE0
	public UnitSpawnWaveData SiidimStart_Carrier
	{
		get
		{
			return this.ExposedVariables.SiidimStart_Carrier;
		}
		set
		{
			this.ExposedVariables.SiidimStart_Carrier = value;
		}
	}

	// Token: 0x17000885 RID: 2181
	// (get) Token: 0x0600F102 RID: 61698 RVA: 0x00445BF0 File Offset: 0x00443DF0
	// (set) Token: 0x0600F103 RID: 61699 RVA: 0x00445C00 File Offset: 0x00443E00
	public UnitSpawnWaveData SiidimStart_BC
	{
		get
		{
			return this.ExposedVariables.SiidimStart_BC;
		}
		set
		{
			this.ExposedVariables.SiidimStart_BC = value;
		}
	}

	// Token: 0x17000886 RID: 2182
	// (get) Token: 0x0600F104 RID: 61700 RVA: 0x00445C10 File Offset: 0x00443E10
	// (set) Token: 0x0600F105 RID: 61701 RVA: 0x00445C20 File Offset: 0x00443E20
	public List<Entity> Siidim_StartingACs
	{
		get
		{
			return this.ExposedVariables.Siidim_StartingACs;
		}
		set
		{
			this.ExposedVariables.Siidim_StartingACs = value;
		}
	}

	// Token: 0x17000887 RID: 2183
	// (get) Token: 0x0600F106 RID: 61702 RVA: 0x00445C30 File Offset: 0x00443E30
	// (set) Token: 0x0600F107 RID: 61703 RVA: 0x00445C40 File Offset: 0x00443E40
	public UnitSpawnWaveData SiidimStart_SC
	{
		get
		{
			return this.ExposedVariables.SiidimStart_SC;
		}
		set
		{
			this.ExposedVariables.SiidimStart_SC = value;
		}
	}

	// Token: 0x17000888 RID: 2184
	// (get) Token: 0x0600F108 RID: 61704 RVA: 0x00445C50 File Offset: 0x00443E50
	// (set) Token: 0x0600F109 RID: 61705 RVA: 0x00445C60 File Offset: 0x00443E60
	public List<Entity> Siidim_StartingRailguns
	{
		get
		{
			return this.ExposedVariables.Siidim_StartingRailguns;
		}
		set
		{
			this.ExposedVariables.Siidim_StartingRailguns = value;
		}
	}

	// Token: 0x17000889 RID: 2185
	// (get) Token: 0x0600F10A RID: 61706 RVA: 0x00445C70 File Offset: 0x00443E70
	// (set) Token: 0x0600F10B RID: 61707 RVA: 0x00445C80 File Offset: 0x00443E80
	public QueryContainer QueryAllSiidim
	{
		get
		{
			return this.ExposedVariables.QueryAllSiidim;
		}
		set
		{
			this.ExposedVariables.QueryAllSiidim = value;
		}
	}

	// Token: 0x1700088A RID: 2186
	// (get) Token: 0x0600F10C RID: 61708 RVA: 0x00445C90 File Offset: 0x00443E90
	// (set) Token: 0x0600F10D RID: 61709 RVA: 0x00445CA0 File Offset: 0x00443EA0
	public UnitSpawnWaveData Siidim_Reinforcements
	{
		get
		{
			return this.ExposedVariables.Siidim_Reinforcements;
		}
		set
		{
			this.ExposedVariables.Siidim_Reinforcements = value;
		}
	}

	// Token: 0x1700088B RID: 2187
	// (get) Token: 0x0600F10E RID: 61710 RVA: 0x00445CB0 File Offset: 0x00443EB0
	// (set) Token: 0x0600F10F RID: 61711 RVA: 0x00445CC0 File Offset: 0x00443EC0
	public List<Entity> Siidim_StartingBCs
	{
		get
		{
			return this.ExposedVariables.Siidim_StartingBCs;
		}
		set
		{
			this.ExposedVariables.Siidim_StartingBCs = value;
		}
	}

	// Token: 0x1700088C RID: 2188
	// (get) Token: 0x0600F110 RID: 61712 RVA: 0x00445CD0 File Offset: 0x00443ED0
	// (set) Token: 0x0600F111 RID: 61713 RVA: 0x00445CE0 File Offset: 0x00443EE0
	public UnitSpawnWaveData SiidimStart_HAC
	{
		get
		{
			return this.ExposedVariables.SiidimStart_HAC;
		}
		set
		{
			this.ExposedVariables.SiidimStart_HAC = value;
		}
	}

	// Token: 0x1700088D RID: 2189
	// (get) Token: 0x0600F112 RID: 61714 RVA: 0x00445CF0 File Offset: 0x00443EF0
	// (set) Token: 0x0600F113 RID: 61715 RVA: 0x00445D00 File Offset: 0x00443F00
	public AttributeBuffSetData Buff_SidAttackers
	{
		get
		{
			return this.ExposedVariables.Buff_SidAttackers;
		}
		set
		{
			this.ExposedVariables.Buff_SidAttackers = value;
		}
	}

	// Token: 0x1700088E RID: 2190
	// (get) Token: 0x0600F114 RID: 61716 RVA: 0x00445D10 File Offset: 0x00443F10
	// (set) Token: 0x0600F115 RID: 61717 RVA: 0x00445D20 File Offset: 0x00443F20
	public UnitSpawnWaveData SiidimStart_Escort
	{
		get
		{
			return this.ExposedVariables.SiidimStart_Escort;
		}
		set
		{
			this.ExposedVariables.SiidimStart_Escort = value;
		}
	}

	// Token: 0x1700088F RID: 2191
	// (get) Token: 0x0600F116 RID: 61718 RVA: 0x00445D30 File Offset: 0x00443F30
	// (set) Token: 0x0600F117 RID: 61719 RVA: 0x00445D40 File Offset: 0x00443F40
	public UnitSpawnWaveData SiidimStart_Railgun
	{
		get
		{
			return this.ExposedVariables.SiidimStart_Railgun;
		}
		set
		{
			this.ExposedVariables.SiidimStart_Railgun = value;
		}
	}

	// Token: 0x17000890 RID: 2192
	// (get) Token: 0x0600F118 RID: 61720 RVA: 0x00445D50 File Offset: 0x00443F50
	// (set) Token: 0x0600F119 RID: 61721 RVA: 0x00445D60 File Offset: 0x00443F60
	public AttributeBuffSetData Buff_SidDefenders
	{
		get
		{
			return this.ExposedVariables.Buff_SidDefenders;
		}
		set
		{
			this.ExposedVariables.Buff_SidDefenders = value;
		}
	}

	// Token: 0x17000891 RID: 2193
	// (get) Token: 0x0600F11A RID: 61722 RVA: 0x00445D70 File Offset: 0x00443F70
	// (set) Token: 0x0600F11B RID: 61723 RVA: 0x00445D80 File Offset: 0x00443F80
	public QueryContainer GodModeUnits
	{
		get
		{
			return this.ExposedVariables.GodModeUnits;
		}
		set
		{
			this.ExposedVariables.GodModeUnits = value;
		}
	}

	// Token: 0x17000892 RID: 2194
	// (get) Token: 0x0600F11C RID: 61724 RVA: 0x00445D90 File Offset: 0x00443F90
	// (set) Token: 0x0600F11D RID: 61725 RVA: 0x00445DA0 File Offset: 0x00443FA0
	public QueryContainer SuspendAll
	{
		get
		{
			return this.ExposedVariables.SuspendAll;
		}
		set
		{
			this.ExposedVariables.SuspendAll = value;
		}
	}

	// Token: 0x17000893 RID: 2195
	// (get) Token: 0x0600F11E RID: 61726 RVA: 0x00445DB0 File Offset: 0x00443FB0
	// (set) Token: 0x0600F11F RID: 61727 RVA: 0x00445DC0 File Offset: 0x00443FC0
	public QueryContainer SakalaSearch
	{
		get
		{
			return this.ExposedVariables.SakalaSearch;
		}
		set
		{
			this.ExposedVariables.SakalaSearch = value;
		}
	}

	// Token: 0x17000894 RID: 2196
	// (get) Token: 0x0600F120 RID: 61728 RVA: 0x00445DD0 File Offset: 0x00443FD0
	// (set) Token: 0x0600F121 RID: 61729 RVA: 0x00445DE0 File Offset: 0x00443FE0
	public UnitSpawnWaveData S_Sal1
	{
		get
		{
			return this.ExposedVariables.S_Sal1;
		}
		set
		{
			this.ExposedVariables.S_Sal1 = value;
		}
	}

	// Token: 0x17000895 RID: 2197
	// (get) Token: 0x0600F122 RID: 61730 RVA: 0x00445DF0 File Offset: 0x00443FF0
	// (set) Token: 0x0600F123 RID: 61731 RVA: 0x00445E00 File Offset: 0x00444000
	public UnitSpawnWaveData S_Sal2
	{
		get
		{
			return this.ExposedVariables.S_Sal2;
		}
		set
		{
			this.ExposedVariables.S_Sal2 = value;
		}
	}

	// Token: 0x17000896 RID: 2198
	// (get) Token: 0x0600F124 RID: 61732 RVA: 0x00445E10 File Offset: 0x00444010
	// (set) Token: 0x0600F125 RID: 61733 RVA: 0x00445E20 File Offset: 0x00444020
	public UnitSpawnWaveData SProdCruiser
	{
		get
		{
			return this.ExposedVariables.SProdCruiser;
		}
		set
		{
			this.ExposedVariables.SProdCruiser = value;
		}
	}

	// Token: 0x17000897 RID: 2199
	// (get) Token: 0x0600F126 RID: 61734 RVA: 0x00445E30 File Offset: 0x00444030
	// (set) Token: 0x0600F127 RID: 61735 RVA: 0x00445E40 File Offset: 0x00444040
	public UnitSpawnWaveData SiidimStart_Bombers
	{
		get
		{
			return this.ExposedVariables.SiidimStart_Bombers;
		}
		set
		{
			this.ExposedVariables.SiidimStart_Bombers = value;
		}
	}

	// Token: 0x17000898 RID: 2200
	// (get) Token: 0x0600F128 RID: 61736 RVA: 0x00445E50 File Offset: 0x00444050
	// (set) Token: 0x0600F129 RID: 61737 RVA: 0x00445E60 File Offset: 0x00444060
	public UnitSpawnWaveData GAAUnits
	{
		get
		{
			return this.ExposedVariables.GAAUnits;
		}
		set
		{
			this.ExposedVariables.GAAUnits = value;
		}
	}

	// Token: 0x17000899 RID: 2201
	// (get) Token: 0x0600F12A RID: 61738 RVA: 0x00445E70 File Offset: 0x00444070
	// (set) Token: 0x0600F12B RID: 61739 RVA: 0x00445E80 File Offset: 0x00444080
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

	// Token: 0x1700089A RID: 2202
	// (get) Token: 0x0600F12C RID: 61740 RVA: 0x00445E90 File Offset: 0x00444090
	// (set) Token: 0x0600F12D RID: 61741 RVA: 0x00445EA0 File Offset: 0x004440A0
	public QueryContainer SearchforSunder
	{
		get
		{
			return this.ExposedVariables.SearchforSunder;
		}
		set
		{
			this.ExposedVariables.SearchforSunder = value;
		}
	}

	// Token: 0x1700089B RID: 2203
	// (get) Token: 0x0600F12E RID: 61742 RVA: 0x00445EB0 File Offset: 0x004440B0
	// (set) Token: 0x0600F12F RID: 61743 RVA: 0x00445EC0 File Offset: 0x004440C0
	public UnitSpawnWaveData EscortDefense
	{
		get
		{
			return this.ExposedVariables.EscortDefense;
		}
		set
		{
			this.ExposedVariables.EscortDefense = value;
		}
	}

	// Token: 0x1700089C RID: 2204
	// (get) Token: 0x0600F130 RID: 61744 RVA: 0x00445ED0 File Offset: 0x004440D0
	// (set) Token: 0x0600F131 RID: 61745 RVA: 0x00445EE0 File Offset: 0x004440E0
	public SpawnFactoryData FirstDefenders
	{
		get
		{
			return this.ExposedVariables.FirstDefenders;
		}
		set
		{
			this.ExposedVariables.FirstDefenders = value;
		}
	}

	// Token: 0x1700089D RID: 2205
	// (get) Token: 0x0600F132 RID: 61746 RVA: 0x00445EF0 File Offset: 0x004440F0
	// (set) Token: 0x0600F133 RID: 61747 RVA: 0x00445F00 File Offset: 0x00444100
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

	// Token: 0x1700089E RID: 2206
	// (get) Token: 0x0600F134 RID: 61748 RVA: 0x00445F10 File Offset: 0x00444110
	// (set) Token: 0x0600F135 RID: 61749 RVA: 0x00445F20 File Offset: 0x00444120
	public SpawnFactoryData FirstDefenders2
	{
		get
		{
			return this.ExposedVariables.FirstDefenders2;
		}
		set
		{
			this.ExposedVariables.FirstDefenders2 = value;
		}
	}

	// Token: 0x1700089F RID: 2207
	// (get) Token: 0x0600F136 RID: 61750 RVA: 0x00445F30 File Offset: 0x00444130
	// (set) Token: 0x0600F137 RID: 61751 RVA: 0x00445F40 File Offset: 0x00444140
	public QueryContainer Look4Starhulls
	{
		get
		{
			return this.ExposedVariables.Look4Starhulls;
		}
		set
		{
			this.ExposedVariables.Look4Starhulls = value;
		}
	}

	// Token: 0x170008A0 RID: 2208
	// (get) Token: 0x0600F138 RID: 61752 RVA: 0x00445F50 File Offset: 0x00444150
	// (set) Token: 0x0600F139 RID: 61753 RVA: 0x00445F60 File Offset: 0x00444160
	public QueryContainer FindSidRanged
	{
		get
		{
			return this.ExposedVariables.FindSidRanged;
		}
		set
		{
			this.ExposedVariables.FindSidRanged = value;
		}
	}

	// Token: 0x170008A1 RID: 2209
	// (get) Token: 0x0600F13A RID: 61754 RVA: 0x00445F70 File Offset: 0x00444170
	// (set) Token: 0x0600F13B RID: 61755 RVA: 0x00445F80 File Offset: 0x00444180
	public QueryContainer FindSidArms
	{
		get
		{
			return this.ExposedVariables.FindSidArms;
		}
		set
		{
			this.ExposedVariables.FindSidArms = value;
		}
	}

	// Token: 0x170008A2 RID: 2210
	// (get) Token: 0x0600F13C RID: 61756 RVA: 0x00445F90 File Offset: 0x00444190
	// (set) Token: 0x0600F13D RID: 61757 RVA: 0x00445FA0 File Offset: 0x004441A0
	public QueryContainer FindSidLAVs
	{
		get
		{
			return this.ExposedVariables.FindSidLAVs;
		}
		set
		{
			this.ExposedVariables.FindSidLAVs = value;
		}
	}

	// Token: 0x170008A3 RID: 2211
	// (get) Token: 0x0600F13E RID: 61758 RVA: 0x00445FB0 File Offset: 0x004441B0
	// (set) Token: 0x0600F13F RID: 61759 RVA: 0x00445FC0 File Offset: 0x004441C0
	public QueryContainer FindSiidimUnits
	{
		get
		{
			return this.ExposedVariables.FindSiidimUnits;
		}
		set
		{
			this.ExposedVariables.FindSiidimUnits = value;
		}
	}

	// Token: 0x170008A4 RID: 2212
	// (get) Token: 0x0600F140 RID: 61760 RVA: 0x00445FD0 File Offset: 0x004441D0
	// (set) Token: 0x0600F141 RID: 61761 RVA: 0x00445FE0 File Offset: 0x004441E0
	public List<Entity> Siidim_StartingSCs
	{
		get
		{
			return this.ExposedVariables.Siidim_StartingSCs;
		}
		set
		{
			this.ExposedVariables.Siidim_StartingSCs = value;
		}
	}

	// Token: 0x170008A5 RID: 2213
	// (get) Token: 0x0600F142 RID: 61762 RVA: 0x00445FF0 File Offset: 0x004441F0
	// (set) Token: 0x0600F143 RID: 61763 RVA: 0x00446000 File Offset: 0x00444200
	public QueryContainer FindSunder
	{
		get
		{
			return this.ExposedVariables.FindSunder;
		}
		set
		{
			this.ExposedVariables.FindSunder = value;
		}
	}

	// Token: 0x170008A6 RID: 2214
	// (get) Token: 0x0600F144 RID: 61764 RVA: 0x00446010 File Offset: 0x00444210
	// (set) Token: 0x0600F145 RID: 61765 RVA: 0x00446020 File Offset: 0x00444220
	public QueryContainer FindRetribution
	{
		get
		{
			return this.ExposedVariables.FindRetribution;
		}
		set
		{
			this.ExposedVariables.FindRetribution = value;
		}
	}

	// Token: 0x170008A7 RID: 2215
	// (get) Token: 0x0600F146 RID: 61766 RVA: 0x00446030 File Offset: 0x00444230
	// (set) Token: 0x0600F147 RID: 61767 RVA: 0x00446040 File Offset: 0x00444240
	public List<Entity> Siidim_StartingCarrier
	{
		get
		{
			return this.ExposedVariables.Siidim_StartingCarrier;
		}
		set
		{
			this.ExposedVariables.Siidim_StartingCarrier = value;
		}
	}

	// Token: 0x170008A8 RID: 2216
	// (get) Token: 0x0600F148 RID: 61768 RVA: 0x00446050 File Offset: 0x00444250
	// (set) Token: 0x0600F149 RID: 61769 RVA: 0x00446060 File Offset: 0x00444260
	public QueryContainer FindGaalsienTargetsforCM
	{
		get
		{
			return this.ExposedVariables.FindGaalsienTargetsforCM;
		}
		set
		{
			this.ExposedVariables.FindGaalsienTargetsforCM = value;
		}
	}

	// Token: 0x170008A9 RID: 2217
	// (get) Token: 0x0600F14A RID: 61770 RVA: 0x00446070 File Offset: 0x00444270
	// (set) Token: 0x0600F14B RID: 61771 RVA: 0x00446080 File Offset: 0x00444280
	public UnitSpawnWaveData SpawnLAVs
	{
		get
		{
			return this.ExposedVariables.SpawnLAVs;
		}
		set
		{
			this.ExposedVariables.SpawnLAVs = value;
		}
	}

	// Token: 0x0600F14C RID: 61772 RVA: 0x00446090 File Offset: 0x00444290
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

	// Token: 0x0600F14D RID: 61773 RVA: 0x004460F8 File Offset: 0x004442F8
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600F14E RID: 61774 RVA: 0x00446108 File Offset: 0x00444308
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600F14F RID: 61775 RVA: 0x00446118 File Offset: 0x00444318
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600F150 RID: 61776 RVA: 0x00446128 File Offset: 0x00444328
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600F151 RID: 61777 RVA: 0x00446138 File Offset: 0x00444338
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x040169A4 RID: 92580
	public M09_Siidim ExposedVariables = new M09_Siidim();
}
