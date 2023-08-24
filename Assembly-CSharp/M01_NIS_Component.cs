using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x0200080E RID: 2062
[AddComponentMenu("uScript/Graphs/M01_NIS")]
public class M01_NIS_Component : uScriptCode
{
	// Token: 0x060042B2 RID: 17074 RVA: 0x00125144 File Offset: 0x00123344
	public M01_NIS_Component()
	{
	}

	// Token: 0x170005BA RID: 1466
	// (get) Token: 0x060042B3 RID: 17075 RVA: 0x00125158 File Offset: 0x00123358
	// (set) Token: 0x060042B4 RID: 17076 RVA: 0x00125168 File Offset: 0x00123368
	public QueryContainer RachelBRTagGet
	{
		get
		{
			return this.ExposedVariables.RachelBRTagGet;
		}
		set
		{
			this.ExposedVariables.RachelBRTagGet = value;
		}
	}

	// Token: 0x170005BB RID: 1467
	// (get) Token: 0x060042B5 RID: 17077 RVA: 0x00125178 File Offset: 0x00123378
	// (set) Token: 0x060042B6 RID: 17078 RVA: 0x00125188 File Offset: 0x00123388
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

	// Token: 0x170005BC RID: 1468
	// (get) Token: 0x060042B7 RID: 17079 RVA: 0x00125198 File Offset: 0x00123398
	// (set) Token: 0x060042B8 RID: 17080 RVA: 0x001251A8 File Offset: 0x001233A8
	public UnitSpawnWaveData Wave_Rachel
	{
		get
		{
			return this.ExposedVariables.Wave_Rachel;
		}
		set
		{
			this.ExposedVariables.Wave_Rachel = value;
		}
	}

	// Token: 0x170005BD RID: 1469
	// (get) Token: 0x060042B9 RID: 17081 RVA: 0x001251B8 File Offset: 0x001233B8
	// (set) Token: 0x060042BA RID: 17082 RVA: 0x001251C8 File Offset: 0x001233C8
	public UnitSpawnWaveData Wave_Carrier
	{
		get
		{
			return this.ExposedVariables.Wave_Carrier;
		}
		set
		{
			this.ExposedVariables.Wave_Carrier = value;
		}
	}

	// Token: 0x170005BE RID: 1470
	// (get) Token: 0x060042BB RID: 17083 RVA: 0x001251D8 File Offset: 0x001233D8
	// (set) Token: 0x060042BC RID: 17084 RVA: 0x001251E8 File Offset: 0x001233E8
	public QueryContainer QueryFirstEscort
	{
		get
		{
			return this.ExposedVariables.QueryFirstEscort;
		}
		set
		{
			this.ExposedVariables.QueryFirstEscort = value;
		}
	}

	// Token: 0x170005BF RID: 1471
	// (get) Token: 0x060042BD RID: 17085 RVA: 0x001251F8 File Offset: 0x001233F8
	// (set) Token: 0x060042BE RID: 17086 RVA: 0x00125208 File Offset: 0x00123408
	public UnitSpawnWaveData EscortType
	{
		get
		{
			return this.ExposedVariables.EscortType;
		}
		set
		{
			this.ExposedVariables.EscortType = value;
		}
	}

	// Token: 0x170005C0 RID: 1472
	// (get) Token: 0x060042BF RID: 17087 RVA: 0x00125218 File Offset: 0x00123418
	// (set) Token: 0x060042C0 RID: 17088 RVA: 0x00125228 File Offset: 0x00123428
	public QueryContainer SuspendUnitsQuery
	{
		get
		{
			return this.ExposedVariables.SuspendUnitsQuery;
		}
		set
		{
			this.ExposedVariables.SuspendUnitsQuery = value;
		}
	}

	// Token: 0x170005C1 RID: 1473
	// (get) Token: 0x060042C1 RID: 17089 RVA: 0x00125238 File Offset: 0x00123438
	// (set) Token: 0x060042C2 RID: 17090 RVA: 0x00125248 File Offset: 0x00123448
	public QueryContainer AllPlayerUnits
	{
		get
		{
			return this.ExposedVariables.AllPlayerUnits;
		}
		set
		{
			this.ExposedVariables.AllPlayerUnits = value;
		}
	}

	// Token: 0x170005C2 RID: 1474
	// (get) Token: 0x060042C3 RID: 17091 RVA: 0x00125258 File Offset: 0x00123458
	// (set) Token: 0x060042C4 RID: 17092 RVA: 0x00125268 File Offset: 0x00123468
	public QueryContainer QueryAllies
	{
		get
		{
			return this.ExposedVariables.QueryAllies;
		}
		set
		{
			this.ExposedVariables.QueryAllies = value;
		}
	}

	// Token: 0x170005C3 RID: 1475
	// (get) Token: 0x060042C5 RID: 17093 RVA: 0x00125278 File Offset: 0x00123478
	// (set) Token: 0x060042C6 RID: 17094 RVA: 0x00125288 File Offset: 0x00123488
	public QueryContainer QueryCarrier
	{
		get
		{
			return this.ExposedVariables.QueryCarrier;
		}
		set
		{
			this.ExposedVariables.QueryCarrier = value;
		}
	}

	// Token: 0x170005C4 RID: 1476
	// (get) Token: 0x060042C7 RID: 17095 RVA: 0x00125298 File Offset: 0x00123498
	// (set) Token: 0x060042C8 RID: 17096 RVA: 0x001252A8 File Offset: 0x001234A8
	public UnitSpawnWaveData ScienceBRtype
	{
		get
		{
			return this.ExposedVariables.ScienceBRtype;
		}
		set
		{
			this.ExposedVariables.ScienceBRtype = value;
		}
	}

	// Token: 0x170005C5 RID: 1477
	// (get) Token: 0x060042C9 RID: 17097 RVA: 0x001252B8 File Offset: 0x001234B8
	// (set) Token: 0x060042CA RID: 17098 RVA: 0x001252C8 File Offset: 0x001234C8
	public UnitSpawnWaveData Wave_Baserunners
	{
		get
		{
			return this.ExposedVariables.Wave_Baserunners;
		}
		set
		{
			this.ExposedVariables.Wave_Baserunners = value;
		}
	}

	// Token: 0x170005C6 RID: 1478
	// (get) Token: 0x060042CB RID: 17099 RVA: 0x001252D8 File Offset: 0x001234D8
	// (set) Token: 0x060042CC RID: 17100 RVA: 0x001252E8 File Offset: 0x001234E8
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

	// Token: 0x170005C7 RID: 1479
	// (get) Token: 0x060042CD RID: 17101 RVA: 0x001252F8 File Offset: 0x001234F8
	// (set) Token: 0x060042CE RID: 17102 RVA: 0x00125308 File Offset: 0x00123508
	public UnitSpawnWaveData Wave_Salvagers
	{
		get
		{
			return this.ExposedVariables.Wave_Salvagers;
		}
		set
		{
			this.ExposedVariables.Wave_Salvagers = value;
		}
	}

	// Token: 0x170005C8 RID: 1480
	// (get) Token: 0x060042CF RID: 17103 RVA: 0x00125318 File Offset: 0x00123518
	// (set) Token: 0x060042D0 RID: 17104 RVA: 0x00125328 File Offset: 0x00123528
	public QueryContainer QueryNIS1Units
	{
		get
		{
			return this.ExposedVariables.QueryNIS1Units;
		}
		set
		{
			this.ExposedVariables.QueryNIS1Units = value;
		}
	}

	// Token: 0x170005C9 RID: 1481
	// (get) Token: 0x060042D1 RID: 17105 RVA: 0x00125338 File Offset: 0x00123538
	// (set) Token: 0x060042D2 RID: 17106 RVA: 0x00125348 File Offset: 0x00123548
	public UnitSpawnWaveData Wave_Hacs
	{
		get
		{
			return this.ExposedVariables.Wave_Hacs;
		}
		set
		{
			this.ExposedVariables.Wave_Hacs = value;
		}
	}

	// Token: 0x170005CA RID: 1482
	// (get) Token: 0x060042D3 RID: 17107 RVA: 0x00125358 File Offset: 0x00123558
	// (set) Token: 0x060042D4 RID: 17108 RVA: 0x00125368 File Offset: 0x00123568
	public AttributeBuffSetData Buff_DisableWeapons
	{
		get
		{
			return this.ExposedVariables.Buff_DisableWeapons;
		}
		set
		{
			this.ExposedVariables.Buff_DisableWeapons = value;
		}
	}

	// Token: 0x170005CB RID: 1483
	// (get) Token: 0x060042D5 RID: 17109 RVA: 0x00125378 File Offset: 0x00123578
	// (set) Token: 0x060042D6 RID: 17110 RVA: 0x00125388 File Offset: 0x00123588
	public QueryContainer QueryRachel
	{
		get
		{
			return this.ExposedVariables.QueryRachel;
		}
		set
		{
			this.ExposedVariables.QueryRachel = value;
		}
	}

	// Token: 0x170005CC RID: 1484
	// (get) Token: 0x060042D7 RID: 17111 RVA: 0x00125398 File Offset: 0x00123598
	// (set) Token: 0x060042D8 RID: 17112 RVA: 0x001253A8 File Offset: 0x001235A8
	public QueryContainer SuspendAllUnitsQuery
	{
		get
		{
			return this.ExposedVariables.SuspendAllUnitsQuery;
		}
		set
		{
			this.ExposedVariables.SuspendAllUnitsQuery = value;
		}
	}

	// Token: 0x170005CD RID: 1485
	// (get) Token: 0x060042D9 RID: 17113 RVA: 0x001253B8 File Offset: 0x001235B8
	// (set) Token: 0x060042DA RID: 17114 RVA: 0x001253C8 File Offset: 0x001235C8
	public AttributeBuffSetData Buff_RachelSpeed
	{
		get
		{
			return this.ExposedVariables.Buff_RachelSpeed;
		}
		set
		{
			this.ExposedVariables.Buff_RachelSpeed = value;
		}
	}

	// Token: 0x170005CE RID: 1486
	// (get) Token: 0x060042DB RID: 17115 RVA: 0x001253D8 File Offset: 0x001235D8
	// (set) Token: 0x060042DC RID: 17116 RVA: 0x001253E8 File Offset: 0x001235E8
	public NavMeshAsset Nav_Script
	{
		get
		{
			return this.ExposedVariables.Nav_Script;
		}
		set
		{
			this.ExposedVariables.Nav_Script = value;
		}
	}

	// Token: 0x060042DD RID: 17117 RVA: 0x001253F8 File Offset: 0x001235F8
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

	// Token: 0x060042DE RID: 17118 RVA: 0x00125460 File Offset: 0x00123660
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x060042DF RID: 17119 RVA: 0x00125470 File Offset: 0x00123670
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x060042E0 RID: 17120 RVA: 0x00125480 File Offset: 0x00123680
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x060042E1 RID: 17121 RVA: 0x00125490 File Offset: 0x00123690
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x060042E2 RID: 17122 RVA: 0x001254A0 File Offset: 0x001236A0
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400554D RID: 21837
	public M01_NIS ExposedVariables = new M01_NIS();
}
