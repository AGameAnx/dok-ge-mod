using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using UnityEngine;

// Token: 0x02000889 RID: 2185
[AddComponentMenu("uScript/Graphs/M10_NIS")]
public class M10_NIS_Component : uScriptCode
{
	// Token: 0x060105E4 RID: 67044 RVA: 0x004A3B10 File Offset: 0x004A1D10
	public M10_NIS_Component()
	{
	}

	// Token: 0x17000901 RID: 2305
	// (get) Token: 0x060105E5 RID: 67045 RVA: 0x004A3B24 File Offset: 0x004A1D24
	// (set) Token: 0x060105E6 RID: 67046 RVA: 0x004A3B34 File Offset: 0x004A1D34
	public UnitSpawnWaveData Wave_Catamaran
	{
		get
		{
			return this.ExposedVariables.Wave_Catamaran;
		}
		set
		{
			this.ExposedVariables.Wave_Catamaran = value;
		}
	}

	// Token: 0x17000902 RID: 2306
	// (get) Token: 0x060105E7 RID: 67047 RVA: 0x004A3B44 File Offset: 0x004A1D44
	// (set) Token: 0x060105E8 RID: 67048 RVA: 0x004A3B54 File Offset: 0x004A1D54
	public UnitSpawnWaveData Wave_CatamaranAA
	{
		get
		{
			return this.ExposedVariables.Wave_CatamaranAA;
		}
		set
		{
			this.ExposedVariables.Wave_CatamaranAA = value;
		}
	}

	// Token: 0x17000903 RID: 2307
	// (get) Token: 0x060105E9 RID: 67049 RVA: 0x004A3B64 File Offset: 0x004A1D64
	// (set) Token: 0x060105EA RID: 67050 RVA: 0x004A3B74 File Offset: 0x004A1D74
	public QueryContainer Query_Carrier
	{
		get
		{
			return this.ExposedVariables.Query_Carrier;
		}
		set
		{
			this.ExposedVariables.Query_Carrier = value;
		}
	}

	// Token: 0x17000904 RID: 2308
	// (get) Token: 0x060105EB RID: 67051 RVA: 0x004A3B84 File Offset: 0x004A1D84
	// (set) Token: 0x060105EC RID: 67052 RVA: 0x004A3B94 File Offset: 0x004A1D94
	public QueryContainer Query_PlayerRailgun
	{
		get
		{
			return this.ExposedVariables.Query_PlayerRailgun;
		}
		set
		{
			this.ExposedVariables.Query_PlayerRailgun = value;
		}
	}

	// Token: 0x17000905 RID: 2309
	// (get) Token: 0x060105ED RID: 67053 RVA: 0x004A3BA4 File Offset: 0x004A1DA4
	// (set) Token: 0x060105EE RID: 67054 RVA: 0x004A3BB4 File Offset: 0x004A1DB4
	public QueryContainer Query_PlayerHAC
	{
		get
		{
			return this.ExposedVariables.Query_PlayerHAC;
		}
		set
		{
			this.ExposedVariables.Query_PlayerHAC = value;
		}
	}

	// Token: 0x17000906 RID: 2310
	// (get) Token: 0x060105EF RID: 67055 RVA: 0x004A3BC4 File Offset: 0x004A1DC4
	// (set) Token: 0x060105F0 RID: 67056 RVA: 0x004A3BD4 File Offset: 0x004A1DD4
	public QueryContainer Query_PlayerSC
	{
		get
		{
			return this.ExposedVariables.Query_PlayerSC;
		}
		set
		{
			this.ExposedVariables.Query_PlayerSC = value;
		}
	}

	// Token: 0x17000907 RID: 2311
	// (get) Token: 0x060105F1 RID: 67057 RVA: 0x004A3BE4 File Offset: 0x004A1DE4
	// (set) Token: 0x060105F2 RID: 67058 RVA: 0x004A3BF4 File Offset: 0x004A1DF4
	public UnitSpawnWaveData Wave_Escorts
	{
		get
		{
			return this.ExposedVariables.Wave_Escorts;
		}
		set
		{
			this.ExposedVariables.Wave_Escorts = value;
		}
	}

	// Token: 0x17000908 RID: 2312
	// (get) Token: 0x060105F3 RID: 67059 RVA: 0x004A3C04 File Offset: 0x004A1E04
	// (set) Token: 0x060105F4 RID: 67060 RVA: 0x004A3C14 File Offset: 0x004A1E14
	public QueryContainer Query_02Escort1
	{
		get
		{
			return this.ExposedVariables.Query_02Escort1;
		}
		set
		{
			this.ExposedVariables.Query_02Escort1 = value;
		}
	}

	// Token: 0x17000909 RID: 2313
	// (get) Token: 0x060105F5 RID: 67061 RVA: 0x004A3C24 File Offset: 0x004A1E24
	// (set) Token: 0x060105F6 RID: 67062 RVA: 0x004A3C34 File Offset: 0x004A1E34
	public bool IntelActive
	{
		get
		{
			return this.ExposedVariables.IntelActive;
		}
		set
		{
			this.ExposedVariables.IntelActive = value;
		}
	}

	// Token: 0x1700090A RID: 2314
	// (get) Token: 0x060105F7 RID: 67063 RVA: 0x004A3C44 File Offset: 0x004A1E44
	// (set) Token: 0x060105F8 RID: 67064 RVA: 0x004A3C54 File Offset: 0x004A1E54
	public QueryContainer IntroNISAllUnitsQ
	{
		get
		{
			return this.ExposedVariables.IntroNISAllUnitsQ;
		}
		set
		{
			this.ExposedVariables.IntroNISAllUnitsQ = value;
		}
	}

	// Token: 0x1700090B RID: 2315
	// (get) Token: 0x060105F9 RID: 67065 RVA: 0x004A3C64 File Offset: 0x004A1E64
	// (set) Token: 0x060105FA RID: 67066 RVA: 0x004A3C74 File Offset: 0x004A1E74
	public QueryContainer IntroNISSuspendUnitsQ
	{
		get
		{
			return this.ExposedVariables.IntroNISSuspendUnitsQ;
		}
		set
		{
			this.ExposedVariables.IntroNISSuspendUnitsQ = value;
		}
	}

	// Token: 0x1700090C RID: 2316
	// (get) Token: 0x060105FB RID: 67067 RVA: 0x004A3C84 File Offset: 0x004A1E84
	// (set) Token: 0x060105FC RID: 67068 RVA: 0x004A3C94 File Offset: 0x004A1E94
	public QueryContainer NIS2_AllUnitsQ
	{
		get
		{
			return this.ExposedVariables.NIS2_AllUnitsQ;
		}
		set
		{
			this.ExposedVariables.NIS2_AllUnitsQ = value;
		}
	}

	// Token: 0x1700090D RID: 2317
	// (get) Token: 0x060105FD RID: 67069 RVA: 0x004A3CA4 File Offset: 0x004A1EA4
	// (set) Token: 0x060105FE RID: 67070 RVA: 0x004A3CB4 File Offset: 0x004A1EB4
	public QueryContainer NIS2_SuspendUnitsQ
	{
		get
		{
			return this.ExposedVariables.NIS2_SuspendUnitsQ;
		}
		set
		{
			this.ExposedVariables.NIS2_SuspendUnitsQ = value;
		}
	}

	// Token: 0x1700090E RID: 2318
	// (get) Token: 0x060105FF RID: 67071 RVA: 0x004A3CC4 File Offset: 0x004A1EC4
	// (set) Token: 0x06010600 RID: 67072 RVA: 0x004A3CD4 File Offset: 0x004A1ED4
	public QueryContainer NIS2_SuspendAllUnitsQ
	{
		get
		{
			return this.ExposedVariables.NIS2_SuspendAllUnitsQ;
		}
		set
		{
			this.ExposedVariables.NIS2_SuspendAllUnitsQ = value;
		}
	}

	// Token: 0x1700090F RID: 2319
	// (get) Token: 0x06010601 RID: 67073 RVA: 0x004A3CE4 File Offset: 0x004A1EE4
	// (set) Token: 0x06010602 RID: 67074 RVA: 0x004A3CF4 File Offset: 0x004A1EF4
	public UnitSpawnWaveData Wave_SupportCruiser
	{
		get
		{
			return this.ExposedVariables.Wave_SupportCruiser;
		}
		set
		{
			this.ExposedVariables.Wave_SupportCruiser = value;
		}
	}

	// Token: 0x06010603 RID: 67075 RVA: 0x004A3D04 File Offset: 0x004A1F04
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

	// Token: 0x06010604 RID: 67076 RVA: 0x004A3D6C File Offset: 0x004A1F6C
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06010605 RID: 67077 RVA: 0x004A3D7C File Offset: 0x004A1F7C
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06010606 RID: 67078 RVA: 0x004A3D8C File Offset: 0x004A1F8C
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06010607 RID: 67079 RVA: 0x004A3D9C File Offset: 0x004A1F9C
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06010608 RID: 67080 RVA: 0x004A3DAC File Offset: 0x004A1FAC
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x040187EA RID: 100330
	public M10_NIS ExposedVariables = new M10_NIS();
}
