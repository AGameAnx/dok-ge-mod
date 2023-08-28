using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using UnityEngine;

// Token: 0x0200082A RID: 2090
[AddComponentMenu("uScript/Graphs/M04_Gameplay")]
public class M04_Gameplay_Component : uScriptCode
{
	// Token: 0x06007DB0 RID: 32176 RVA: 0x0023BD94 File Offset: 0x00239F94
	public M04_Gameplay_Component()
	{
	}

	// Token: 0x1700068E RID: 1678
	// (get) Token: 0x06007DB1 RID: 32177 RVA: 0x0023BDA8 File Offset: 0x00239FA8
	// (set) Token: 0x06007DB2 RID: 32178 RVA: 0x0023BDB8 File Offset: 0x00239FB8
	public UnitSpawnWaveData TornadoType
	{
		get
		{
			return this.ExposedVariables.TornadoType;
		}
		set
		{
			this.ExposedVariables.TornadoType = value;
		}
	}

	// Token: 0x1700068F RID: 1679
	// (get) Token: 0x06007DB3 RID: 32179 RVA: 0x0023BDC8 File Offset: 0x00239FC8
	// (set) Token: 0x06007DB4 RID: 32180 RVA: 0x0023BDD8 File Offset: 0x00239FD8
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

	// Token: 0x17000690 RID: 1680
	// (get) Token: 0x06007DB5 RID: 32181 RVA: 0x0023BDE8 File Offset: 0x00239FE8
	// (set) Token: 0x06007DB6 RID: 32182 RVA: 0x0023BDF8 File Offset: 0x00239FF8
	public QueryContainer SuspendUnits
	{
		get
		{
			return this.ExposedVariables.SuspendUnits;
		}
		set
		{
			this.ExposedVariables.SuspendUnits = value;
		}
	}

	// Token: 0x17000691 RID: 1681
	// (get) Token: 0x06007DB7 RID: 32183 RVA: 0x0023BE08 File Offset: 0x0023A008
	// (set) Token: 0x06007DB8 RID: 32184 RVA: 0x0023BE18 File Offset: 0x0023A018
	public QueryContainer Commander1Search
	{
		get
		{
			return this.ExposedVariables.Commander1Search;
		}
		set
		{
			this.ExposedVariables.Commander1Search = value;
		}
	}

	// Token: 0x17000692 RID: 1682
	// (get) Token: 0x06007DB9 RID: 32185 RVA: 0x0023BE28 File Offset: 0x0023A028
	// (set) Token: 0x06007DBA RID: 32186 RVA: 0x0023BE38 File Offset: 0x0023A038
	public QueryContainer ScannerSearch
	{
		get
		{
			return this.ExposedVariables.ScannerSearch;
		}
		set
		{
			this.ExposedVariables.ScannerSearch = value;
		}
	}

	// Token: 0x17000693 RID: 1683
	// (get) Token: 0x06007DBB RID: 32187 RVA: 0x0023BE48 File Offset: 0x0023A048
	// (set) Token: 0x06007DBC RID: 32188 RVA: 0x0023BE58 File Offset: 0x0023A058
	public QueryContainer KapisiSearch
	{
		get
		{
			return this.ExposedVariables.KapisiSearch;
		}
		set
		{
			this.ExposedVariables.KapisiSearch = value;
		}
	}

	// Token: 0x17000694 RID: 1684
	// (get) Token: 0x06007DBD RID: 32189 RVA: 0x0023BE68 File Offset: 0x0023A068
	// (set) Token: 0x06007DBE RID: 32190 RVA: 0x0023BE78 File Offset: 0x0023A078
	public QueryContainer Rachel
	{
		get
		{
			return this.ExposedVariables.Rachel;
		}
		set
		{
			this.ExposedVariables.Rachel = value;
		}
	}

	// Token: 0x17000695 RID: 1685
	// (get) Token: 0x06007DBF RID: 32191 RVA: 0x0023BE88 File Offset: 0x0023A088
	// (set) Token: 0x06007DC0 RID: 32192 RVA: 0x0023BE98 File Offset: 0x0023A098
	public QueryContainer NIS_NonRachel_Query
	{
		get
		{
			return this.ExposedVariables.NIS_NonRachel_Query;
		}
		set
		{
			this.ExposedVariables.NIS_NonRachel_Query = value;
		}
	}

	// Token: 0x06007DC1 RID: 32193 RVA: 0x0023BEA8 File Offset: 0x0023A0A8
	private void Awake()
	{
		if (MapModManager.CustomLayout)
			return;

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

	// Token: 0x06007DC2 RID: 32194 RVA: 0x0023BF10 File Offset: 0x0023A110
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06007DC3 RID: 32195 RVA: 0x0023BF20 File Offset: 0x0023A120
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06007DC4 RID: 32196 RVA: 0x0023BF30 File Offset: 0x0023A130
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06007DC5 RID: 32197 RVA: 0x0023BF40 File Offset: 0x0023A140
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06007DC6 RID: 32198 RVA: 0x0023BF50 File Offset: 0x0023A150
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400B4FB RID: 46331
	public M04_Gameplay ExposedVariables = new M04_Gameplay();
}
