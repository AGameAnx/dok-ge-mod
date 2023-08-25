using System;
using BBI.Game.Data.Queries;
using UnityEngine;

// Token: 0x020008B3 RID: 2227
[AddComponentMenu("uScript/Graphs/M13_NIS")]
public class M13_NIS_Component : uScriptCode
{
	// Token: 0x06014192 RID: 82322 RVA: 0x005B3F8C File Offset: 0x005B218C
	public M13_NIS_Component()
	{
	}

	// Token: 0x17000A11 RID: 2577
	// (get) Token: 0x06014193 RID: 82323 RVA: 0x005B3FA0 File Offset: 0x005B21A0
	// (set) Token: 0x06014194 RID: 82324 RVA: 0x005B3FB0 File Offset: 0x005B21B0
	public QueryContainer Q_Rachel
	{
		get
		{
			return this.ExposedVariables.Q_Rachel;
		}
		set
		{
			this.ExposedVariables.Q_Rachel = value;
		}
	}

	// Token: 0x17000A12 RID: 2578
	// (get) Token: 0x06014195 RID: 82325 RVA: 0x005B3FC0 File Offset: 0x005B21C0
	// (set) Token: 0x06014196 RID: 82326 RVA: 0x005B3FD0 File Offset: 0x005B21D0
	public QueryContainer Q_Fathership
	{
		get
		{
			return this.ExposedVariables.Q_Fathership;
		}
		set
		{
			this.ExposedVariables.Q_Fathership = value;
		}
	}

	// Token: 0x17000A13 RID: 2579
	// (get) Token: 0x06014197 RID: 82327 RVA: 0x005B3FE0 File Offset: 0x005B21E0
	// (set) Token: 0x06014198 RID: 82328 RVA: 0x005B3FF0 File Offset: 0x005B21F0
	public QueryContainer Q_ImmuneDuringNIS03
	{
		get
		{
			return this.ExposedVariables.Q_ImmuneDuringNIS03;
		}
		set
		{
			this.ExposedVariables.Q_ImmuneDuringNIS03 = value;
		}
	}

	// Token: 0x17000A14 RID: 2580
	// (get) Token: 0x06014199 RID: 82329 RVA: 0x005B4000 File Offset: 0x005B2200
	// (set) Token: 0x0601419A RID: 82330 RVA: 0x005B4010 File Offset: 0x005B2210
	public QueryContainer Q_ImmuneDuringNIS04
	{
		get
		{
			return this.ExposedVariables.Q_ImmuneDuringNIS04;
		}
		set
		{
			this.ExposedVariables.Q_ImmuneDuringNIS04 = value;
		}
	}

	// Token: 0x17000A15 RID: 2581
	// (get) Token: 0x0601419B RID: 82331 RVA: 0x005B4020 File Offset: 0x005B2220
	// (set) Token: 0x0601419C RID: 82332 RVA: 0x005B4030 File Offset: 0x005B2230
	public QueryContainer Q_ImmuneDuringNIS05
	{
		get
		{
			return this.ExposedVariables.Q_ImmuneDuringNIS05;
		}
		set
		{
			this.ExposedVariables.Q_ImmuneDuringNIS05 = value;
		}
	}

	// Token: 0x17000A16 RID: 2582
	// (get) Token: 0x0601419D RID: 82333 RVA: 0x005B4040 File Offset: 0x005B2240
	// (set) Token: 0x0601419E RID: 82334 RVA: 0x005B4050 File Offset: 0x005B2250
	public QueryContainer Q_HiddenDuringNIS05
	{
		get
		{
			return this.ExposedVariables.Q_HiddenDuringNIS05;
		}
		set
		{
			this.ExposedVariables.Q_HiddenDuringNIS05 = value;
		}
	}

	// Token: 0x17000A17 RID: 2583
	// (get) Token: 0x0601419F RID: 82335 RVA: 0x005B4060 File Offset: 0x005B2260
	// (set) Token: 0x060141A0 RID: 82336 RVA: 0x005B4070 File Offset: 0x005B2270
	public QueryContainer Q_HiddenDuringNIS04
	{
		get
		{
			return this.ExposedVariables.Q_HiddenDuringNIS04;
		}
		set
		{
			this.ExposedVariables.Q_HiddenDuringNIS04 = value;
		}
	}

	// Token: 0x17000A18 RID: 2584
	// (get) Token: 0x060141A1 RID: 82337 RVA: 0x005B4080 File Offset: 0x005B2280
	// (set) Token: 0x060141A2 RID: 82338 RVA: 0x005B4090 File Offset: 0x005B2290
	public QueryContainer Q_HiddenDuringNIS03
	{
		get
		{
			return this.ExposedVariables.Q_HiddenDuringNIS03;
		}
		set
		{
			this.ExposedVariables.Q_HiddenDuringNIS03 = value;
		}
	}

	// Token: 0x17000A19 RID: 2585
	// (get) Token: 0x060141A3 RID: 82339 RVA: 0x005B40A0 File Offset: 0x005B22A0
	// (set) Token: 0x060141A4 RID: 82340 RVA: 0x005B40B0 File Offset: 0x005B22B0
	public QueryContainer Q_HiddenDuringNIS02
	{
		get
		{
			return this.ExposedVariables.Q_HiddenDuringNIS02;
		}
		set
		{
			this.ExposedVariables.Q_HiddenDuringNIS02 = value;
		}
	}

	// Token: 0x17000A1A RID: 2586
	// (get) Token: 0x060141A5 RID: 82341 RVA: 0x005B40C0 File Offset: 0x005B22C0
	// (set) Token: 0x060141A6 RID: 82342 RVA: 0x005B40D0 File Offset: 0x005B22D0
	public QueryContainer Q_ImmuneDuringNIS02
	{
		get
		{
			return this.ExposedVariables.Q_ImmuneDuringNIS02;
		}
		set
		{
			this.ExposedVariables.Q_ImmuneDuringNIS02 = value;
		}
	}

	// Token: 0x17000A1B RID: 2587
	// (get) Token: 0x060141A7 RID: 82343 RVA: 0x005B40E0 File Offset: 0x005B22E0
	// (set) Token: 0x060141A8 RID: 82344 RVA: 0x005B40F0 File Offset: 0x005B22F0
	public QueryContainer Q_FathershipEscort
	{
		get
		{
			return this.ExposedVariables.Q_FathershipEscort;
		}
		set
		{
			this.ExposedVariables.Q_FathershipEscort = value;
		}
	}

	// Token: 0x17000A1C RID: 2588
	// (get) Token: 0x060141A9 RID: 82345 RVA: 0x005B4100 File Offset: 0x005B2300
	// (set) Token: 0x060141AA RID: 82346 RVA: 0x005B4110 File Offset: 0x005B2310
	public QueryContainer Q_HiddenDuringNIS01
	{
		get
		{
			return this.ExposedVariables.Q_HiddenDuringNIS01;
		}
		set
		{
			this.ExposedVariables.Q_HiddenDuringNIS01 = value;
		}
	}

	// Token: 0x17000A1D RID: 2589
	// (get) Token: 0x060141AB RID: 82347 RVA: 0x005B4120 File Offset: 0x005B2320
	// (set) Token: 0x060141AC RID: 82348 RVA: 0x005B4130 File Offset: 0x005B2330
	public QueryContainer Q_NoUnits
	{
		get
		{
			return this.ExposedVariables.Q_NoUnits;
		}
		set
		{
			this.ExposedVariables.Q_NoUnits = value;
		}
	}

	// Token: 0x17000A1E RID: 2590
	// (get) Token: 0x060141AD RID: 82349 RVA: 0x005B4140 File Offset: 0x005B2340
	// (set) Token: 0x060141AE RID: 82350 RVA: 0x005B4150 File Offset: 0x005B2350
	public QueryContainer Q_Kapisi
	{
		get
		{
			return this.ExposedVariables.Q_Kapisi;
		}
		set
		{
			this.ExposedVariables.Q_Kapisi = value;
		}
	}

	// Token: 0x060141AF RID: 82351 RVA: 0x005B4160 File Offset: 0x005B2360
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

	// Token: 0x060141B0 RID: 82352 RVA: 0x005B41C8 File Offset: 0x005B23C8
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x060141B1 RID: 82353 RVA: 0x005B41D8 File Offset: 0x005B23D8
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x060141B2 RID: 82354 RVA: 0x005B41E8 File Offset: 0x005B23E8
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x060141B3 RID: 82355 RVA: 0x005B41F8 File Offset: 0x005B23F8
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x060141B4 RID: 82356 RVA: 0x005B4208 File Offset: 0x005B2408
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401EE17 RID: 126487
	public M13_NIS ExposedVariables = new M13_NIS();
}
