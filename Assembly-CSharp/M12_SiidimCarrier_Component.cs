using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Scripting;
using UnityEngine;

// Token: 0x0200089F RID: 2207
[AddComponentMenu("uScript/Graphs/M12_SiidimCarrier")]
public class M12_SiidimCarrier_Component : uScriptCode
{
	// Token: 0x06012937 RID: 76087 RVA: 0x00545630 File Offset: 0x00543830
	public M12_SiidimCarrier_Component()
	{
	}

	// Token: 0x1700099B RID: 2459
	// (get) Token: 0x06012938 RID: 76088 RVA: 0x00545644 File Offset: 0x00543844
	// (set) Token: 0x06012939 RID: 76089 RVA: 0x00545654 File Offset: 0x00543854
	public UnitSpawnWaveData WaveCruiserEscort
	{
		get
		{
			return this.ExposedVariables.WaveCruiserEscort;
		}
		set
		{
			this.ExposedVariables.WaveCruiserEscort = value;
		}
	}

	// Token: 0x1700099C RID: 2460
	// (get) Token: 0x0601293A RID: 76090 RVA: 0x00545664 File Offset: 0x00543864
	// (set) Token: 0x0601293B RID: 76091 RVA: 0x00545674 File Offset: 0x00543874
	public UnitSpawnWaveData TypeSakala
	{
		get
		{
			return this.ExposedVariables.TypeSakala;
		}
		set
		{
			this.ExposedVariables.TypeSakala = value;
		}
	}

	// Token: 0x1700099D RID: 2461
	// (get) Token: 0x0601293C RID: 76092 RVA: 0x00545684 File Offset: 0x00543884
	// (set) Token: 0x0601293D RID: 76093 RVA: 0x00545694 File Offset: 0x00543894
	public UnitSpawnWaveData TypeHAC
	{
		get
		{
			return this.ExposedVariables.TypeHAC;
		}
		set
		{
			this.ExposedVariables.TypeHAC = value;
		}
	}

	// Token: 0x1700099E RID: 2462
	// (get) Token: 0x0601293E RID: 76094 RVA: 0x005456A4 File Offset: 0x005438A4
	// (set) Token: 0x0601293F RID: 76095 RVA: 0x005456B4 File Offset: 0x005438B4
	public UnitSpawnWaveData TypeHACUpgrade
	{
		get
		{
			return this.ExposedVariables.TypeHACUpgrade;
		}
		set
		{
			this.ExposedVariables.TypeHACUpgrade = value;
		}
	}

	// Token: 0x1700099F RID: 2463
	// (get) Token: 0x06012940 RID: 76096 RVA: 0x005456C4 File Offset: 0x005438C4
	// (set) Token: 0x06012941 RID: 76097 RVA: 0x005456D4 File Offset: 0x005438D4
	public UnitSpawnWaveData WaveSkirmishers
	{
		get
		{
			return this.ExposedVariables.WaveSkirmishers;
		}
		set
		{
			this.ExposedVariables.WaveSkirmishers = value;
		}
	}

	// Token: 0x170009A0 RID: 2464
	// (get) Token: 0x06012942 RID: 76098 RVA: 0x005456E4 File Offset: 0x005438E4
	// (set) Token: 0x06012943 RID: 76099 RVA: 0x005456F4 File Offset: 0x005438F4
	public UnitSpawnWaveData TypeRailgun
	{
		get
		{
			return this.ExposedVariables.TypeRailgun;
		}
		set
		{
			this.ExposedVariables.TypeRailgun = value;
		}
	}

	// Token: 0x170009A1 RID: 2465
	// (get) Token: 0x06012944 RID: 76100 RVA: 0x00545704 File Offset: 0x00543904
	// (set) Token: 0x06012945 RID: 76101 RVA: 0x00545714 File Offset: 0x00543914
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

	// Token: 0x170009A2 RID: 2466
	// (get) Token: 0x06012946 RID: 76102 RVA: 0x00545724 File Offset: 0x00543924
	// (set) Token: 0x06012947 RID: 76103 RVA: 0x00545734 File Offset: 0x00543934
	public UnitSpawnWaveData TypeBaserunner
	{
		get
		{
			return this.ExposedVariables.TypeBaserunner;
		}
		set
		{
			this.ExposedVariables.TypeBaserunner = value;
		}
	}

	// Token: 0x170009A3 RID: 2467
	// (get) Token: 0x06012948 RID: 76104 RVA: 0x00545744 File Offset: 0x00543944
	// (set) Token: 0x06012949 RID: 76105 RVA: 0x00545754 File Offset: 0x00543954
	public SpawnFactoryData SpawnCruiserSquadA
	{
		get
		{
			return this.ExposedVariables.SpawnCruiserSquadA;
		}
		set
		{
			this.ExposedVariables.SpawnCruiserSquadA = value;
		}
	}

	// Token: 0x170009A4 RID: 2468
	// (get) Token: 0x0601294A RID: 76106 RVA: 0x00545764 File Offset: 0x00543964
	// (set) Token: 0x0601294B RID: 76107 RVA: 0x00545774 File Offset: 0x00543974
	public QueryContainer Q_MissileTargets
	{
		get
		{
			return this.ExposedVariables.Q_MissileTargets;
		}
		set
		{
			this.ExposedVariables.Q_MissileTargets = value;
		}
	}

	// Token: 0x170009A5 RID: 2469
	// (get) Token: 0x0601294C RID: 76108 RVA: 0x00545784 File Offset: 0x00543984
	// (set) Token: 0x0601294D RID: 76109 RVA: 0x00545794 File Offset: 0x00543994
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

	// Token: 0x170009A6 RID: 2470
	// (get) Token: 0x0601294E RID: 76110 RVA: 0x005457A4 File Offset: 0x005439A4
	// (set) Token: 0x0601294F RID: 76111 RVA: 0x005457B4 File Offset: 0x005439B4
	public SpawnFactoryData SpawnGuardA
	{
		get
		{
			return this.ExposedVariables.SpawnGuardA;
		}
		set
		{
			this.ExposedVariables.SpawnGuardA = value;
		}
	}

	// Token: 0x170009A7 RID: 2471
	// (get) Token: 0x06012950 RID: 76112 RVA: 0x005457C4 File Offset: 0x005439C4
	// (set) Token: 0x06012951 RID: 76113 RVA: 0x005457D4 File Offset: 0x005439D4
	public QueryContainer Q_CruiserSquadA
	{
		get
		{
			return this.ExposedVariables.Q_CruiserSquadA;
		}
		set
		{
			this.ExposedVariables.Q_CruiserSquadA = value;
		}
	}

	// Token: 0x170009A8 RID: 2472
	// (get) Token: 0x06012952 RID: 76114 RVA: 0x005457E4 File Offset: 0x005439E4
	// (set) Token: 0x06012953 RID: 76115 RVA: 0x005457F4 File Offset: 0x005439F4
	public SpawnFactoryData SpawnCruiserSquadB
	{
		get
		{
			return this.ExposedVariables.SpawnCruiserSquadB;
		}
		set
		{
			this.ExposedVariables.SpawnCruiserSquadB = value;
		}
	}

	// Token: 0x170009A9 RID: 2473
	// (get) Token: 0x06012954 RID: 76116 RVA: 0x00545804 File Offset: 0x00543A04
	// (set) Token: 0x06012955 RID: 76117 RVA: 0x00545814 File Offset: 0x00543A14
	public QueryContainer Q_CruiserSquadB
	{
		get
		{
			return this.ExposedVariables.Q_CruiserSquadB;
		}
		set
		{
			this.ExposedVariables.Q_CruiserSquadB = value;
		}
	}

	// Token: 0x170009AA RID: 2474
	// (get) Token: 0x06012956 RID: 76118 RVA: 0x00545824 File Offset: 0x00543A24
	// (set) Token: 0x06012957 RID: 76119 RVA: 0x00545834 File Offset: 0x00543A34
	public SpawnFactoryData SpawnGuardB
	{
		get
		{
			return this.ExposedVariables.SpawnGuardB;
		}
		set
		{
			this.ExposedVariables.SpawnGuardB = value;
		}
	}

	// Token: 0x170009AB RID: 2475
	// (get) Token: 0x06012958 RID: 76120 RVA: 0x00545844 File Offset: 0x00543A44
	// (set) Token: 0x06012959 RID: 76121 RVA: 0x00545854 File Offset: 0x00543A54
	public SpawnFactoryData SpawnCruiserSquadC
	{
		get
		{
			return this.ExposedVariables.SpawnCruiserSquadC;
		}
		set
		{
			this.ExposedVariables.SpawnCruiserSquadC = value;
		}
	}

	// Token: 0x170009AC RID: 2476
	// (get) Token: 0x0601295A RID: 76122 RVA: 0x00545864 File Offset: 0x00543A64
	// (set) Token: 0x0601295B RID: 76123 RVA: 0x00545874 File Offset: 0x00543A74
	public SpawnFactoryData SpawnGuardC
	{
		get
		{
			return this.ExposedVariables.SpawnGuardC;
		}
		set
		{
			this.ExposedVariables.SpawnGuardC = value;
		}
	}

	// Token: 0x170009AD RID: 2477
	// (get) Token: 0x0601295C RID: 76124 RVA: 0x00545884 File Offset: 0x00543A84
	// (set) Token: 0x0601295D RID: 76125 RVA: 0x00545894 File Offset: 0x00543A94
	public SpawnFactoryData SpawnGuardS
	{
		get
		{
			return this.ExposedVariables.SpawnGuardS;
		}
		set
		{
			this.ExposedVariables.SpawnGuardS = value;
		}
	}

	// Token: 0x170009AE RID: 2478
	// (get) Token: 0x0601295E RID: 76126 RVA: 0x005458A4 File Offset: 0x00543AA4
	// (set) Token: 0x0601295F RID: 76127 RVA: 0x005458B4 File Offset: 0x00543AB4
	public SpawnFactoryData SpawnPesterSquad
	{
		get
		{
			return this.ExposedVariables.SpawnPesterSquad;
		}
		set
		{
			this.ExposedVariables.SpawnPesterSquad = value;
		}
	}

	// Token: 0x170009AF RID: 2479
	// (get) Token: 0x06012960 RID: 76128 RVA: 0x005458C4 File Offset: 0x00543AC4
	// (set) Token: 0x06012961 RID: 76129 RVA: 0x005458D4 File Offset: 0x00543AD4
	public AttributeBuffSetData BuffSakala
	{
		get
		{
			return this.ExposedVariables.BuffSakala;
		}
		set
		{
			this.ExposedVariables.BuffSakala = value;
		}
	}

	// Token: 0x170009B0 RID: 2480
	// (get) Token: 0x06012962 RID: 76130 RVA: 0x005458E4 File Offset: 0x00543AE4
	// (set) Token: 0x06012963 RID: 76131 RVA: 0x005458F4 File Offset: 0x00543AF4
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

	// Token: 0x170009B1 RID: 2481
	// (get) Token: 0x06012964 RID: 76132 RVA: 0x00545904 File Offset: 0x00543B04
	// (set) Token: 0x06012965 RID: 76133 RVA: 0x00545914 File Offset: 0x00543B14
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

	// Token: 0x170009B2 RID: 2482
	// (get) Token: 0x06012966 RID: 76134 RVA: 0x00545924 File Offset: 0x00543B24
	// (set) Token: 0x06012967 RID: 76135 RVA: 0x00545934 File Offset: 0x00543B34
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

	// Token: 0x170009B3 RID: 2483
	// (get) Token: 0x06012968 RID: 76136 RVA: 0x00545944 File Offset: 0x00543B44
	// (set) Token: 0x06012969 RID: 76137 RVA: 0x00545954 File Offset: 0x00543B54
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

	// Token: 0x170009B4 RID: 2484
	// (get) Token: 0x0601296A RID: 76138 RVA: 0x00545964 File Offset: 0x00543B64
	// (set) Token: 0x0601296B RID: 76139 RVA: 0x00545974 File Offset: 0x00543B74
	public UnitSpawnWaveData SpeedyEscortSpawn
	{
		get
		{
			return this.ExposedVariables.SpeedyEscortSpawn;
		}
		set
		{
			this.ExposedVariables.SpeedyEscortSpawn = value;
		}
	}

	// Token: 0x170009B5 RID: 2485
	// (get) Token: 0x0601296C RID: 76140 RVA: 0x00545984 File Offset: 0x00543B84
	// (set) Token: 0x0601296D RID: 76141 RVA: 0x00545994 File Offset: 0x00543B94
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

	// Token: 0x170009B6 RID: 2486
	// (get) Token: 0x0601296E RID: 76142 RVA: 0x005459A4 File Offset: 0x00543BA4
	// (set) Token: 0x0601296F RID: 76143 RVA: 0x005459B4 File Offset: 0x00543BB4
	public QueryContainer Q_AirstrikeTargets
	{
		get
		{
			return this.ExposedVariables.Q_AirstrikeTargets;
		}
		set
		{
			this.ExposedVariables.Q_AirstrikeTargets = value;
		}
	}

	// Token: 0x170009B7 RID: 2487
	// (get) Token: 0x06012970 RID: 76144 RVA: 0x005459C4 File Offset: 0x00543BC4
	// (set) Token: 0x06012971 RID: 76145 RVA: 0x005459D4 File Offset: 0x00543BD4
	public UnitSpawnWaveData TypeSupportCruiser
	{
		get
		{
			return this.ExposedVariables.TypeSupportCruiser;
		}
		set
		{
			this.ExposedVariables.TypeSupportCruiser = value;
		}
	}

	// Token: 0x170009B8 RID: 2488
	// (get) Token: 0x06012972 RID: 76146 RVA: 0x005459E4 File Offset: 0x00543BE4
	// (set) Token: 0x06012973 RID: 76147 RVA: 0x005459F4 File Offset: 0x00543BF4
	public QueryContainer Q_RachelAndKapisi
	{
		get
		{
			return this.ExposedVariables.Q_RachelAndKapisi;
		}
		set
		{
			this.ExposedVariables.Q_RachelAndKapisi = value;
		}
	}

	// Token: 0x06012974 RID: 76148 RVA: 0x00545A04 File Offset: 0x00543C04
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

	// Token: 0x06012975 RID: 76149 RVA: 0x00545A6C File Offset: 0x00543C6C
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06012976 RID: 76150 RVA: 0x00545A7C File Offset: 0x00543C7C
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06012977 RID: 76151 RVA: 0x00545A8C File Offset: 0x00543C8C
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06012978 RID: 76152 RVA: 0x00545A9C File Offset: 0x00543C9C
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06012979 RID: 76153 RVA: 0x00545AAC File Offset: 0x00543CAC
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401C076 RID: 114806
	public M12_SiidimCarrier ExposedVariables = new M12_SiidimCarrier();
}
