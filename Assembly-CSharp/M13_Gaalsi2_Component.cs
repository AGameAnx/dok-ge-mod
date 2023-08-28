using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x020008A9 RID: 2217
[AddComponentMenu("uScript/Graphs/M13_Gaalsi2")]
public class M13_Gaalsi2_Component : uScriptCode
{
	// Token: 0x06013541 RID: 79169 RVA: 0x005785DC File Offset: 0x005767DC
	public M13_Gaalsi2_Component()
	{
	}

	// Token: 0x170009D4 RID: 2516
	// (get) Token: 0x06013542 RID: 79170 RVA: 0x005785F0 File Offset: 0x005767F0
	// (set) Token: 0x06013543 RID: 79171 RVA: 0x00578600 File Offset: 0x00576800
	public UnitSpawnWaveData TypeCarrier
	{
		get
		{
			return this.ExposedVariables.TypeCarrier;
		}
		set
		{
			this.ExposedVariables.TypeCarrier = value;
		}
	}

	// Token: 0x170009D5 RID: 2517
	// (get) Token: 0x06013544 RID: 79172 RVA: 0x00578610 File Offset: 0x00576810
	// (set) Token: 0x06013545 RID: 79173 RVA: 0x00578620 File Offset: 0x00576820
	public QueryContainer Q_CruisersF
	{
		get
		{
			return this.ExposedVariables.Q_CruisersF;
		}
		set
		{
			this.ExposedVariables.Q_CruisersF = value;
		}
	}

	// Token: 0x170009D6 RID: 2518
	// (get) Token: 0x06013546 RID: 79174 RVA: 0x00578630 File Offset: 0x00576830
	// (set) Token: 0x06013547 RID: 79175 RVA: 0x00578640 File Offset: 0x00576840
	public QueryContainer Q_CruisersG
	{
		get
		{
			return this.ExposedVariables.Q_CruisersG;
		}
		set
		{
			this.ExposedVariables.Q_CruisersG = value;
		}
	}

	// Token: 0x170009D7 RID: 2519
	// (get) Token: 0x06013548 RID: 79176 RVA: 0x00578650 File Offset: 0x00576850
	// (set) Token: 0x06013549 RID: 79177 RVA: 0x00578660 File Offset: 0x00576860
	public QueryContainer Q_CarrierEscort
	{
		get
		{
			return this.ExposedVariables.Q_CarrierEscort;
		}
		set
		{
			this.ExposedVariables.Q_CarrierEscort = value;
		}
	}

	// Token: 0x170009D8 RID: 2520
	// (get) Token: 0x0601354A RID: 79178 RVA: 0x00578670 File Offset: 0x00576870
	// (set) Token: 0x0601354B RID: 79179 RVA: 0x00578680 File Offset: 0x00576880
	public UnitSpawnWaveData WaveCruisersG
	{
		get
		{
			return this.ExposedVariables.WaveCruisersG;
		}
		set
		{
			this.ExposedVariables.WaveCruisersG = value;
		}
	}

	// Token: 0x170009D9 RID: 2521
	// (get) Token: 0x0601354C RID: 79180 RVA: 0x00578690 File Offset: 0x00576890
	// (set) Token: 0x0601354D RID: 79181 RVA: 0x005786A0 File Offset: 0x005768A0
	public UnitSpawnWaveData WaveGuardG
	{
		get
		{
			return this.ExposedVariables.WaveGuardG;
		}
		set
		{
			this.ExposedVariables.WaveGuardG = value;
		}
	}

	// Token: 0x170009DA RID: 2522
	// (get) Token: 0x0601354E RID: 79182 RVA: 0x005786B0 File Offset: 0x005768B0
	// (set) Token: 0x0601354F RID: 79183 RVA: 0x005786C0 File Offset: 0x005768C0
	public UnitSpawnWaveData WaveCruisersF
	{
		get
		{
			return this.ExposedVariables.WaveCruisersF;
		}
		set
		{
			this.ExposedVariables.WaveCruisersF = value;
		}
	}

	// Token: 0x170009DB RID: 2523
	// (get) Token: 0x06013550 RID: 79184 RVA: 0x005786D0 File Offset: 0x005768D0
	// (set) Token: 0x06013551 RID: 79185 RVA: 0x005786E0 File Offset: 0x005768E0
	public UnitSpawnWaveData WaveGuardS
	{
		get
		{
			return this.ExposedVariables.WaveGuardS;
		}
		set
		{
			this.ExposedVariables.WaveGuardS = value;
		}
	}

	// Token: 0x170009DC RID: 2524
	// (get) Token: 0x06013552 RID: 79186 RVA: 0x005786F0 File Offset: 0x005768F0
	// (set) Token: 0x06013553 RID: 79187 RVA: 0x00578700 File Offset: 0x00576900
	public UnitSpawnWaveData WaveGuardF
	{
		get
		{
			return this.ExposedVariables.WaveGuardF;
		}
		set
		{
			this.ExposedVariables.WaveGuardF = value;
		}
	}

	// Token: 0x170009DD RID: 2525
	// (get) Token: 0x06013554 RID: 79188 RVA: 0x00578710 File Offset: 0x00576910
	// (set) Token: 0x06013555 RID: 79189 RVA: 0x00578720 File Offset: 0x00576920
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

	// Token: 0x170009DE RID: 2526
	// (get) Token: 0x06013556 RID: 79190 RVA: 0x00578730 File Offset: 0x00576930
	// (set) Token: 0x06013557 RID: 79191 RVA: 0x00578740 File Offset: 0x00576940
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

	// Token: 0x170009DF RID: 2527
	// (get) Token: 0x06013558 RID: 79192 RVA: 0x00578750 File Offset: 0x00576950
	// (set) Token: 0x06013559 RID: 79193 RVA: 0x00578760 File Offset: 0x00576960
	public QueryContainer Q_SuspendedUnits
	{
		get
		{
			return this.ExposedVariables.Q_SuspendedUnits;
		}
		set
		{
			this.ExposedVariables.Q_SuspendedUnits = value;
		}
	}

	// Token: 0x170009E0 RID: 2528
	// (get) Token: 0x0601355A RID: 79194 RVA: 0x00578770 File Offset: 0x00576970
	// (set) Token: 0x0601355B RID: 79195 RVA: 0x00578780 File Offset: 0x00576980
	public QueryContainer Q_PrimaryTargets
	{
		get
		{
			return this.ExposedVariables.Q_PrimaryTargets;
		}
		set
		{
			this.ExposedVariables.Q_PrimaryTargets = value;
		}
	}

	// Token: 0x170009E1 RID: 2529
	// (get) Token: 0x0601355C RID: 79196 RVA: 0x00578790 File Offset: 0x00576990
	// (set) Token: 0x0601355D RID: 79197 RVA: 0x005787A0 File Offset: 0x005769A0
	public UnitSpawnWaveData WaveGuardE
	{
		get
		{
			return this.ExposedVariables.WaveGuardE;
		}
		set
		{
			this.ExposedVariables.WaveGuardE = value;
		}
	}

	// Token: 0x170009E2 RID: 2530
	// (get) Token: 0x0601355E RID: 79198 RVA: 0x005787B0 File Offset: 0x005769B0
	// (set) Token: 0x0601355F RID: 79199 RVA: 0x005787C0 File Offset: 0x005769C0
	public UnitSpawnWaveData WaveCruisersE
	{
		get
		{
			return this.ExposedVariables.WaveCruisersE;
		}
		set
		{
			this.ExposedVariables.WaveCruisersE = value;
		}
	}

	// Token: 0x170009E3 RID: 2531
	// (get) Token: 0x06013560 RID: 79200 RVA: 0x005787D0 File Offset: 0x005769D0
	// (set) Token: 0x06013561 RID: 79201 RVA: 0x005787E0 File Offset: 0x005769E0
	public AttributeBuffSetData Buff_Carrier
	{
		get
		{
			return this.ExposedVariables.Buff_Carrier;
		}
		set
		{
			this.ExposedVariables.Buff_Carrier = value;
		}
	}

	// Token: 0x170009E4 RID: 2532
	// (get) Token: 0x06013562 RID: 79202 RVA: 0x005787F0 File Offset: 0x005769F0
	// (set) Token: 0x06013563 RID: 79203 RVA: 0x00578800 File Offset: 0x00576A00
	public QueryContainer Q_SecondaryTargets
	{
		get
		{
			return this.ExposedVariables.Q_SecondaryTargets;
		}
		set
		{
			this.ExposedVariables.Q_SecondaryTargets = value;
		}
	}

	// Token: 0x170009E5 RID: 2533
	// (get) Token: 0x06013564 RID: 79204 RVA: 0x00578810 File Offset: 0x00576A10
	// (set) Token: 0x06013565 RID: 79205 RVA: 0x00578820 File Offset: 0x00576A20
	public QueryContainer Q_CruisersE
	{
		get
		{
			return this.ExposedVariables.Q_CruisersE;
		}
		set
		{
			this.ExposedVariables.Q_CruisersE = value;
		}
	}

	// Token: 0x170009E6 RID: 2534
	// (get) Token: 0x06013566 RID: 79206 RVA: 0x00578830 File Offset: 0x00576A30
	// (set) Token: 0x06013567 RID: 79207 RVA: 0x00578840 File Offset: 0x00576A40
	public AttributeBuffSetData Buff_CarrierEscort
	{
		get
		{
			return this.ExposedVariables.Buff_CarrierEscort;
		}
		set
		{
			this.ExposedVariables.Buff_CarrierEscort = value;
		}
	}

	// Token: 0x170009E7 RID: 2535
	// (get) Token: 0x06013568 RID: 79208 RVA: 0x00578850 File Offset: 0x00576A50
	// (set) Token: 0x06013569 RID: 79209 RVA: 0x00578860 File Offset: 0x00576A60
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

	// Token: 0x170009E8 RID: 2536
	// (get) Token: 0x0601356A RID: 79210 RVA: 0x00578870 File Offset: 0x00576A70
	// (set) Token: 0x0601356B RID: 79211 RVA: 0x00578880 File Offset: 0x00576A80
	public AttributeBuffSetData Buff_Leash
	{
		get
		{
			return this.ExposedVariables.Buff_Leash;
		}
		set
		{
			this.ExposedVariables.Buff_Leash = value;
		}
	}

	// Token: 0x170009E9 RID: 2537
	// (get) Token: 0x0601356C RID: 79212 RVA: 0x00578890 File Offset: 0x00576A90
	// (set) Token: 0x0601356D RID: 79213 RVA: 0x005788A0 File Offset: 0x00576AA0
	public AttributeBuffSetData Buff_Aggro
	{
		get
		{
			return this.ExposedVariables.Buff_Aggro;
		}
		set
		{
			this.ExposedVariables.Buff_Aggro = value;
		}
	}

	// Token: 0x0601356E RID: 79214 RVA: 0x005788B0 File Offset: 0x00576AB0
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

	// Token: 0x0601356F RID: 79215 RVA: 0x00578918 File Offset: 0x00576B18
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06013570 RID: 79216 RVA: 0x00578928 File Offset: 0x00576B28
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06013571 RID: 79217 RVA: 0x00578938 File Offset: 0x00576B38
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06013572 RID: 79218 RVA: 0x00578948 File Offset: 0x00576B48
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06013573 RID: 79219 RVA: 0x00578958 File Offset: 0x00576B58
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401D563 RID: 120163
	public M13_Gaalsi2 ExposedVariables = new M13_Gaalsi2();
}
