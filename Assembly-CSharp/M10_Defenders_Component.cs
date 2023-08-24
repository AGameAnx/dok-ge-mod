using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x02000885 RID: 2181
[AddComponentMenu("uScript/Graphs/M10_Defenders")]
public class M10_Defenders_Component : uScriptCode
{
	// Token: 0x0600FE56 RID: 65110 RVA: 0x00483574 File Offset: 0x00481774
	public M10_Defenders_Component()
	{
	}

	// Token: 0x170008CB RID: 2251
	// (get) Token: 0x0600FE57 RID: 65111 RVA: 0x00483588 File Offset: 0x00481788
	// (set) Token: 0x0600FE58 RID: 65112 RVA: 0x00483598 File Offset: 0x00481798
	public UnitSpawnWaveData TopAAHac
	{
		get
		{
			return this.ExposedVariables.TopAAHac;
		}
		set
		{
			this.ExposedVariables.TopAAHac = value;
		}
	}

	// Token: 0x170008CC RID: 2252
	// (get) Token: 0x0600FE59 RID: 65113 RVA: 0x004835A8 File Offset: 0x004817A8
	// (set) Token: 0x0600FE5A RID: 65114 RVA: 0x004835B8 File Offset: 0x004817B8
	public List<Entity> CommandCarrier
	{
		get
		{
			return this.ExposedVariables.CommandCarrier;
		}
		set
		{
			this.ExposedVariables.CommandCarrier = value;
		}
	}

	// Token: 0x170008CD RID: 2253
	// (get) Token: 0x0600FE5B RID: 65115 RVA: 0x004835C8 File Offset: 0x004817C8
	// (set) Token: 0x0600FE5C RID: 65116 RVA: 0x004835D8 File Offset: 0x004817D8
	public UnitSpawnWaveData ThreeHACs
	{
		get
		{
			return this.ExposedVariables.ThreeHACs;
		}
		set
		{
			this.ExposedVariables.ThreeHACs = value;
		}
	}

	// Token: 0x170008CE RID: 2254
	// (get) Token: 0x0600FE5D RID: 65117 RVA: 0x004835E8 File Offset: 0x004817E8
	// (set) Token: 0x0600FE5E RID: 65118 RVA: 0x004835F8 File Offset: 0x004817F8
	public UnitSpawnWaveData UnitTypeEscort
	{
		get
		{
			return this.ExposedVariables.UnitTypeEscort;
		}
		set
		{
			this.ExposedVariables.UnitTypeEscort = value;
		}
	}

	// Token: 0x170008CF RID: 2255
	// (get) Token: 0x0600FE5F RID: 65119 RVA: 0x00483608 File Offset: 0x00481808
	// (set) Token: 0x0600FE60 RID: 65120 RVA: 0x00483618 File Offset: 0x00481818
	public UnitSpawnWaveData UnitTypeHAC
	{
		get
		{
			return this.ExposedVariables.UnitTypeHAC;
		}
		set
		{
			this.ExposedVariables.UnitTypeHAC = value;
		}
	}

	// Token: 0x170008D0 RID: 2256
	// (get) Token: 0x0600FE61 RID: 65121 RVA: 0x00483628 File Offset: 0x00481828
	// (set) Token: 0x0600FE62 RID: 65122 RVA: 0x00483638 File Offset: 0x00481838
	public UnitSpawnWaveData Wave_LeftRamp
	{
		get
		{
			return this.ExposedVariables.Wave_LeftRamp;
		}
		set
		{
			this.ExposedVariables.Wave_LeftRamp = value;
		}
	}

	// Token: 0x170008D1 RID: 2257
	// (get) Token: 0x0600FE63 RID: 65123 RVA: 0x00483648 File Offset: 0x00481848
	// (set) Token: 0x0600FE64 RID: 65124 RVA: 0x00483658 File Offset: 0x00481858
	public UnitSpawnWaveData ThreeEscorts
	{
		get
		{
			return this.ExposedVariables.ThreeEscorts;
		}
		set
		{
			this.ExposedVariables.ThreeEscorts = value;
		}
	}

	// Token: 0x170008D2 RID: 2258
	// (get) Token: 0x0600FE65 RID: 65125 RVA: 0x00483668 File Offset: 0x00481868
	// (set) Token: 0x0600FE66 RID: 65126 RVA: 0x00483678 File Offset: 0x00481878
	public UnitSpawnWaveData TwoRailguns
	{
		get
		{
			return this.ExposedVariables.TwoRailguns;
		}
		set
		{
			this.ExposedVariables.TwoRailguns = value;
		}
	}

	// Token: 0x170008D3 RID: 2259
	// (get) Token: 0x0600FE67 RID: 65127 RVA: 0x00483688 File Offset: 0x00481888
	// (set) Token: 0x0600FE68 RID: 65128 RVA: 0x00483698 File Offset: 0x00481898
	public QueryContainer ResLeftQuery
	{
		get
		{
			return this.ExposedVariables.ResLeftQuery;
		}
		set
		{
			this.ExposedVariables.ResLeftQuery = value;
		}
	}

	// Token: 0x170008D4 RID: 2260
	// (get) Token: 0x0600FE69 RID: 65129 RVA: 0x004836A8 File Offset: 0x004818A8
	// (set) Token: 0x0600FE6A RID: 65130 RVA: 0x004836B8 File Offset: 0x004818B8
	public QueryContainer DunesSquad5
	{
		get
		{
			return this.ExposedVariables.DunesSquad5;
		}
		set
		{
			this.ExposedVariables.DunesSquad5 = value;
		}
	}

	// Token: 0x170008D5 RID: 2261
	// (get) Token: 0x0600FE6B RID: 65131 RVA: 0x004836C8 File Offset: 0x004818C8
	// (set) Token: 0x0600FE6C RID: 65132 RVA: 0x004836D8 File Offset: 0x004818D8
	public AttributeBuffSetData Buff_LeashAggro
	{
		get
		{
			return this.ExposedVariables.Buff_LeashAggro;
		}
		set
		{
			this.ExposedVariables.Buff_LeashAggro = value;
		}
	}

	// Token: 0x170008D6 RID: 2262
	// (get) Token: 0x0600FE6D RID: 65133 RVA: 0x004836E8 File Offset: 0x004818E8
	// (set) Token: 0x0600FE6E RID: 65134 RVA: 0x004836F8 File Offset: 0x004818F8
	public QueryContainer Level1Squad
	{
		get
		{
			return this.ExposedVariables.Level1Squad;
		}
		set
		{
			this.ExposedVariables.Level1Squad = value;
		}
	}

	// Token: 0x170008D7 RID: 2263
	// (get) Token: 0x0600FE6F RID: 65135 RVA: 0x00483708 File Offset: 0x00481908
	// (set) Token: 0x0600FE70 RID: 65136 RVA: 0x00483718 File Offset: 0x00481918
	public QueryContainer DunesSquad1
	{
		get
		{
			return this.ExposedVariables.DunesSquad1;
		}
		set
		{
			this.ExposedVariables.DunesSquad1 = value;
		}
	}

	// Token: 0x170008D8 RID: 2264
	// (get) Token: 0x0600FE71 RID: 65137 RVA: 0x00483728 File Offset: 0x00481928
	// (set) Token: 0x0600FE72 RID: 65138 RVA: 0x00483738 File Offset: 0x00481938
	public QueryContainer DunesSquad2
	{
		get
		{
			return this.ExposedVariables.DunesSquad2;
		}
		set
		{
			this.ExposedVariables.DunesSquad2 = value;
		}
	}

	// Token: 0x170008D9 RID: 2265
	// (get) Token: 0x0600FE73 RID: 65139 RVA: 0x00483748 File Offset: 0x00481948
	// (set) Token: 0x0600FE74 RID: 65140 RVA: 0x00483758 File Offset: 0x00481958
	public QueryContainer DunesSquad3
	{
		get
		{
			return this.ExposedVariables.DunesSquad3;
		}
		set
		{
			this.ExposedVariables.DunesSquad3 = value;
		}
	}

	// Token: 0x170008DA RID: 2266
	// (get) Token: 0x0600FE75 RID: 65141 RVA: 0x00483768 File Offset: 0x00481968
	// (set) Token: 0x0600FE76 RID: 65142 RVA: 0x00483778 File Offset: 0x00481978
	public QueryContainer DunesSquad4
	{
		get
		{
			return this.ExposedVariables.DunesSquad4;
		}
		set
		{
			this.ExposedVariables.DunesSquad4 = value;
		}
	}

	// Token: 0x170008DB RID: 2267
	// (get) Token: 0x0600FE77 RID: 65143 RVA: 0x00483788 File Offset: 0x00481988
	// (set) Token: 0x0600FE78 RID: 65144 RVA: 0x00483798 File Offset: 0x00481998
	public QueryContainer CCQueryContainer
	{
		get
		{
			return this.ExposedVariables.CCQueryContainer;
		}
		set
		{
			this.ExposedVariables.CCQueryContainer = value;
		}
	}

	// Token: 0x170008DC RID: 2268
	// (get) Token: 0x0600FE79 RID: 65145 RVA: 0x004837A8 File Offset: 0x004819A8
	// (set) Token: 0x0600FE7A RID: 65146 RVA: 0x004837B8 File Offset: 0x004819B8
	public QueryContainer QueryLowerRampSquad1
	{
		get
		{
			return this.ExposedVariables.QueryLowerRampSquad1;
		}
		set
		{
			this.ExposedVariables.QueryLowerRampSquad1 = value;
		}
	}

	// Token: 0x170008DD RID: 2269
	// (get) Token: 0x0600FE7B RID: 65147 RVA: 0x004837C8 File Offset: 0x004819C8
	// (set) Token: 0x0600FE7C RID: 65148 RVA: 0x004837D8 File Offset: 0x004819D8
	public QueryContainer QueryLowerRampSquad2
	{
		get
		{
			return this.ExposedVariables.QueryLowerRampSquad2;
		}
		set
		{
			this.ExposedVariables.QueryLowerRampSquad2 = value;
		}
	}

	// Token: 0x170008DE RID: 2270
	// (get) Token: 0x0600FE7D RID: 65149 RVA: 0x004837E8 File Offset: 0x004819E8
	// (set) Token: 0x0600FE7E RID: 65150 RVA: 0x004837F8 File Offset: 0x004819F8
	public QueryContainer QueryLowerRampSquad3
	{
		get
		{
			return this.ExposedVariables.QueryLowerRampSquad3;
		}
		set
		{
			this.ExposedVariables.QueryLowerRampSquad3 = value;
		}
	}

	// Token: 0x170008DF RID: 2271
	// (get) Token: 0x0600FE7F RID: 65151 RVA: 0x00483808 File Offset: 0x00481A08
	// (set) Token: 0x0600FE80 RID: 65152 RVA: 0x00483818 File Offset: 0x00481A18
	public QueryContainer QueryLowerRampSquad4
	{
		get
		{
			return this.ExposedVariables.QueryLowerRampSquad4;
		}
		set
		{
			this.ExposedVariables.QueryLowerRampSquad4 = value;
		}
	}

	// Token: 0x170008E0 RID: 2272
	// (get) Token: 0x0600FE81 RID: 65153 RVA: 0x00483828 File Offset: 0x00481A28
	// (set) Token: 0x0600FE82 RID: 65154 RVA: 0x00483838 File Offset: 0x00481A38
	public QueryContainer QueryLowerRampSquad5
	{
		get
		{
			return this.ExposedVariables.QueryLowerRampSquad5;
		}
		set
		{
			this.ExposedVariables.QueryLowerRampSquad5 = value;
		}
	}

	// Token: 0x170008E1 RID: 2273
	// (get) Token: 0x0600FE83 RID: 65155 RVA: 0x00483848 File Offset: 0x00481A48
	// (set) Token: 0x0600FE84 RID: 65156 RVA: 0x00483858 File Offset: 0x00481A58
	public QueryContainer QueryLowerRampSquad6
	{
		get
		{
			return this.ExposedVariables.QueryLowerRampSquad6;
		}
		set
		{
			this.ExposedVariables.QueryLowerRampSquad6 = value;
		}
	}

	// Token: 0x170008E2 RID: 2274
	// (get) Token: 0x0600FE85 RID: 65157 RVA: 0x00483868 File Offset: 0x00481A68
	// (set) Token: 0x0600FE86 RID: 65158 RVA: 0x00483878 File Offset: 0x00481A78
	public QueryContainer QueryLowerRampSquad7
	{
		get
		{
			return this.ExposedVariables.QueryLowerRampSquad7;
		}
		set
		{
			this.ExposedVariables.QueryLowerRampSquad7 = value;
		}
	}

	// Token: 0x170008E3 RID: 2275
	// (get) Token: 0x0600FE87 RID: 65159 RVA: 0x00483888 File Offset: 0x00481A88
	// (set) Token: 0x0600FE88 RID: 65160 RVA: 0x00483898 File Offset: 0x00481A98
	public QueryContainer QueryLowerRampSquad8
	{
		get
		{
			return this.ExposedVariables.QueryLowerRampSquad8;
		}
		set
		{
			this.ExposedVariables.QueryLowerRampSquad8 = value;
		}
	}

	// Token: 0x170008E4 RID: 2276
	// (get) Token: 0x0600FE89 RID: 65161 RVA: 0x004838A8 File Offset: 0x00481AA8
	// (set) Token: 0x0600FE8A RID: 65162 RVA: 0x004838B8 File Offset: 0x00481AB8
	public UnitSpawnWaveData EscortAndHACgroup
	{
		get
		{
			return this.ExposedVariables.EscortAndHACgroup;
		}
		set
		{
			this.ExposedVariables.EscortAndHACgroup = value;
		}
	}

	// Token: 0x170008E5 RID: 2277
	// (get) Token: 0x0600FE8B RID: 65163 RVA: 0x004838C8 File Offset: 0x00481AC8
	// (set) Token: 0x0600FE8C RID: 65164 RVA: 0x004838D8 File Offset: 0x00481AD8
	public UnitSpawnWaveData HACgroup
	{
		get
		{
			return this.ExposedVariables.HACgroup;
		}
		set
		{
			this.ExposedVariables.HACgroup = value;
		}
	}

	// Token: 0x170008E6 RID: 2278
	// (get) Token: 0x0600FE8D RID: 65165 RVA: 0x004838E8 File Offset: 0x00481AE8
	// (set) Token: 0x0600FE8E RID: 65166 RVA: 0x004838F8 File Offset: 0x00481AF8
	public UnitSpawnWaveData RailAndHACgroup
	{
		get
		{
			return this.ExposedVariables.RailAndHACgroup;
		}
		set
		{
			this.ExposedVariables.RailAndHACgroup = value;
		}
	}

	// Token: 0x170008E7 RID: 2279
	// (get) Token: 0x0600FE8F RID: 65167 RVA: 0x00483908 File Offset: 0x00481B08
	// (set) Token: 0x0600FE90 RID: 65168 RVA: 0x00483918 File Offset: 0x00481B18
	public AttributeBuffSetData Buff_LowAccuracy
	{
		get
		{
			return this.ExposedVariables.Buff_LowAccuracy;
		}
		set
		{
			this.ExposedVariables.Buff_LowAccuracy = value;
		}
	}

	// Token: 0x170008E8 RID: 2280
	// (get) Token: 0x0600FE91 RID: 65169 RVA: 0x00483928 File Offset: 0x00481B28
	// (set) Token: 0x0600FE92 RID: 65170 RVA: 0x00483938 File Offset: 0x00481B38
	public QueryContainer QueryAA
	{
		get
		{
			return this.ExposedVariables.QueryAA;
		}
		set
		{
			this.ExposedVariables.QueryAA = value;
		}
	}

	// Token: 0x170008E9 RID: 2281
	// (get) Token: 0x0600FE93 RID: 65171 RVA: 0x00483948 File Offset: 0x00481B48
	// (set) Token: 0x0600FE94 RID: 65172 RVA: 0x00483958 File Offset: 0x00481B58
	public UnitSpawnWaveData Railgroup
	{
		get
		{
			return this.ExposedVariables.Railgroup;
		}
		set
		{
			this.ExposedVariables.Railgroup = value;
		}
	}

	// Token: 0x170008EA RID: 2282
	// (get) Token: 0x0600FE95 RID: 65173 RVA: 0x00483968 File Offset: 0x00481B68
	// (set) Token: 0x0600FE96 RID: 65174 RVA: 0x00483978 File Offset: 0x00481B78
	public AttributeBuffSetData Buff_TopDefenders
	{
		get
		{
			return this.ExposedVariables.Buff_TopDefenders;
		}
		set
		{
			this.ExposedVariables.Buff_TopDefenders = value;
		}
	}

	// Token: 0x170008EB RID: 2283
	// (get) Token: 0x0600FE97 RID: 65175 RVA: 0x00483988 File Offset: 0x00481B88
	// (set) Token: 0x0600FE98 RID: 65176 RVA: 0x00483998 File Offset: 0x00481B98
	public AttributeBuffSetData Buff_SupportCruiserAggro
	{
		get
		{
			return this.ExposedVariables.Buff_SupportCruiserAggro;
		}
		set
		{
			this.ExposedVariables.Buff_SupportCruiserAggro = value;
		}
	}

	// Token: 0x170008EC RID: 2284
	// (get) Token: 0x0600FE99 RID: 65177 RVA: 0x004839A8 File Offset: 0x00481BA8
	// (set) Token: 0x0600FE9A RID: 65178 RVA: 0x004839B8 File Offset: 0x00481BB8
	public QueryContainer QuerySupportCruisers
	{
		get
		{
			return this.ExposedVariables.QuerySupportCruisers;
		}
		set
		{
			this.ExposedVariables.QuerySupportCruisers = value;
		}
	}

	// Token: 0x170008ED RID: 2285
	// (get) Token: 0x0600FE9B RID: 65179 RVA: 0x004839C8 File Offset: 0x00481BC8
	// (set) Token: 0x0600FE9C RID: 65180 RVA: 0x004839D8 File Offset: 0x00481BD8
	public UnitSpawnWaveData S_SupportCruiser
	{
		get
		{
			return this.ExposedVariables.S_SupportCruiser;
		}
		set
		{
			this.ExposedVariables.S_SupportCruiser = value;
		}
	}

	// Token: 0x170008EE RID: 2286
	// (get) Token: 0x0600FE9D RID: 65181 RVA: 0x004839E8 File Offset: 0x00481BE8
	// (set) Token: 0x0600FE9E RID: 65182 RVA: 0x004839F8 File Offset: 0x00481BF8
	public UnitSpawnWaveData Wave_MidRamp
	{
		get
		{
			return this.ExposedVariables.Wave_MidRamp;
		}
		set
		{
			this.ExposedVariables.Wave_MidRamp = value;
		}
	}

	// Token: 0x170008EF RID: 2287
	// (get) Token: 0x0600FE9F RID: 65183 RVA: 0x00483A08 File Offset: 0x00481C08
	// (set) Token: 0x0600FEA0 RID: 65184 RVA: 0x00483A18 File Offset: 0x00481C18
	public UnitSpawnWaveData S_ArtilleryCruiser
	{
		get
		{
			return this.ExposedVariables.S_ArtilleryCruiser;
		}
		set
		{
			this.ExposedVariables.S_ArtilleryCruiser = value;
		}
	}

	// Token: 0x170008F0 RID: 2288
	// (get) Token: 0x0600FEA1 RID: 65185 RVA: 0x00483A28 File Offset: 0x00481C28
	// (set) Token: 0x0600FEA2 RID: 65186 RVA: 0x00483A38 File Offset: 0x00481C38
	public UnitSpawnWaveData Wave_AAGuard2
	{
		get
		{
			return this.ExposedVariables.Wave_AAGuard2;
		}
		set
		{
			this.ExposedVariables.Wave_AAGuard2 = value;
		}
	}

	// Token: 0x170008F1 RID: 2289
	// (get) Token: 0x0600FEA3 RID: 65187 RVA: 0x00483A48 File Offset: 0x00481C48
	// (set) Token: 0x0600FEA4 RID: 65188 RVA: 0x00483A58 File Offset: 0x00481C58
	public UnitSpawnWaveData Wave_AAGuard1
	{
		get
		{
			return this.ExposedVariables.Wave_AAGuard1;
		}
		set
		{
			this.ExposedVariables.Wave_AAGuard1 = value;
		}
	}

	// Token: 0x170008F2 RID: 2290
	// (get) Token: 0x0600FEA5 RID: 65189 RVA: 0x00483A68 File Offset: 0x00481C68
	// (set) Token: 0x0600FEA6 RID: 65190 RVA: 0x00483A78 File Offset: 0x00481C78
	public UnitSpawnWaveData Wave_RightRamp
	{
		get
		{
			return this.ExposedVariables.Wave_RightRamp;
		}
		set
		{
			this.ExposedVariables.Wave_RightRamp = value;
		}
	}

	// Token: 0x170008F3 RID: 2291
	// (get) Token: 0x0600FEA7 RID: 65191 RVA: 0x00483A88 File Offset: 0x00481C88
	// (set) Token: 0x0600FEA8 RID: 65192 RVA: 0x00483A98 File Offset: 0x00481C98
	public int AvgPlayerPopCap
	{
		get
		{
			return this.ExposedVariables.AvgPlayerPopCap;
		}
		set
		{
			this.ExposedVariables.AvgPlayerPopCap = value;
		}
	}

	// Token: 0x170008F4 RID: 2292
	// (get) Token: 0x0600FEA9 RID: 65193 RVA: 0x00483AA8 File Offset: 0x00481CA8
	// (set) Token: 0x0600FEAA RID: 65194 RVA: 0x00483AB8 File Offset: 0x00481CB8
	public int HighPlayerPopCap
	{
		get
		{
			return this.ExposedVariables.HighPlayerPopCap;
		}
		set
		{
			this.ExposedVariables.HighPlayerPopCap = value;
		}
	}

	// Token: 0x170008F5 RID: 2293
	// (get) Token: 0x0600FEAB RID: 65195 RVA: 0x00483AC8 File Offset: 0x00481CC8
	// (set) Token: 0x0600FEAC RID: 65196 RVA: 0x00483AD8 File Offset: 0x00481CD8
	public UnitSpawnWaveData TopHac
	{
		get
		{
			return this.ExposedVariables.TopHac;
		}
		set
		{
			this.ExposedVariables.TopHac = value;
		}
	}

	// Token: 0x170008F6 RID: 2294
	// (get) Token: 0x0600FEAD RID: 65197 RVA: 0x00483AE8 File Offset: 0x00481CE8
	// (set) Token: 0x0600FEAE RID: 65198 RVA: 0x00483AF8 File Offset: 0x00481CF8
	public QueryContainer QueryAASquad3
	{
		get
		{
			return this.ExposedVariables.QueryAASquad3;
		}
		set
		{
			this.ExposedVariables.QueryAASquad3 = value;
		}
	}

	// Token: 0x170008F7 RID: 2295
	// (get) Token: 0x0600FEAF RID: 65199 RVA: 0x00483B08 File Offset: 0x00481D08
	// (set) Token: 0x0600FEB0 RID: 65200 RVA: 0x00483B18 File Offset: 0x00481D18
	public AttributeBuffSetData Buff_AA_LargeLeash
	{
		get
		{
			return this.ExposedVariables.Buff_AA_LargeLeash;
		}
		set
		{
			this.ExposedVariables.Buff_AA_LargeLeash = value;
		}
	}

	// Token: 0x170008F8 RID: 2296
	// (get) Token: 0x0600FEB1 RID: 65201 RVA: 0x00483B28 File Offset: 0x00481D28
	// (set) Token: 0x0600FEB2 RID: 65202 RVA: 0x00483B38 File Offset: 0x00481D38
	public AttributeBuffSetData Buff_AA_SmallLeash
	{
		get
		{
			return this.ExposedVariables.Buff_AA_SmallLeash;
		}
		set
		{
			this.ExposedVariables.Buff_AA_SmallLeash = value;
		}
	}

	// Token: 0x0600FEB3 RID: 65203 RVA: 0x00483B48 File Offset: 0x00481D48
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

	// Token: 0x0600FEB4 RID: 65204 RVA: 0x00483BB0 File Offset: 0x00481DB0
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600FEB5 RID: 65205 RVA: 0x00483BC0 File Offset: 0x00481DC0
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600FEB6 RID: 65206 RVA: 0x00483BD0 File Offset: 0x00481DD0
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600FEB7 RID: 65207 RVA: 0x00483BE0 File Offset: 0x00481DE0
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600FEB8 RID: 65208 RVA: 0x00483BF0 File Offset: 0x00481DF0
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04017E87 RID: 97927
	public M10_Defenders ExposedVariables = new M10_Defenders();
}
