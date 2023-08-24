using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Scripting;
using UnityEngine;

// Token: 0x02000892 RID: 2194
[AddComponentMenu("uScript/Graphs/M11_Gameplay3")]
public class M11_Gameplay3_Component : uScriptCode
{
	// Token: 0x060112F5 RID: 70389 RVA: 0x004DFEBC File Offset: 0x004DE0BC
	public M11_Gameplay3_Component()
	{
	}

	// Token: 0x1700093C RID: 2364
	// (get) Token: 0x060112F6 RID: 70390 RVA: 0x004DFED0 File Offset: 0x004DE0D0
	// (set) Token: 0x060112F7 RID: 70391 RVA: 0x004DFEE0 File Offset: 0x004DE0E0
	public UnitSpawnWaveData SiidimACGroup1EscortWave3
	{
		get
		{
			return this.ExposedVariables.SiidimACGroup1EscortWave3;
		}
		set
		{
			this.ExposedVariables.SiidimACGroup1EscortWave3 = value;
		}
	}

	// Token: 0x1700093D RID: 2365
	// (get) Token: 0x060112F8 RID: 70392 RVA: 0x004DFEF0 File Offset: 0x004DE0F0
	// (set) Token: 0x060112F9 RID: 70393 RVA: 0x004DFF00 File Offset: 0x004DE100
	public UnitSpawnWaveData SiidimACGroup2EscortWave1
	{
		get
		{
			return this.ExposedVariables.SiidimACGroup2EscortWave1;
		}
		set
		{
			this.ExposedVariables.SiidimACGroup2EscortWave1 = value;
		}
	}

	// Token: 0x1700093E RID: 2366
	// (get) Token: 0x060112FA RID: 70394 RVA: 0x004DFF10 File Offset: 0x004DE110
	// (set) Token: 0x060112FB RID: 70395 RVA: 0x004DFF20 File Offset: 0x004DE120
	public UnitSpawnWaveData SiidimACGroup1AAEscortWave
	{
		get
		{
			return this.ExposedVariables.SiidimACGroup1AAEscortWave;
		}
		set
		{
			this.ExposedVariables.SiidimACGroup1AAEscortWave = value;
		}
	}

	// Token: 0x1700093F RID: 2367
	// (get) Token: 0x060112FC RID: 70396 RVA: 0x004DFF30 File Offset: 0x004DE130
	// (set) Token: 0x060112FD RID: 70397 RVA: 0x004DFF40 File Offset: 0x004DE140
	public int iNumArtilleryBarragesFired
	{
		get
		{
			return this.ExposedVariables.iNumArtilleryBarragesFired;
		}
		set
		{
			this.ExposedVariables.iNumArtilleryBarragesFired = value;
		}
	}

	// Token: 0x17000940 RID: 2368
	// (get) Token: 0x060112FE RID: 70398 RVA: 0x004DFF50 File Offset: 0x004DE150
	// (set) Token: 0x060112FF RID: 70399 RVA: 0x004DFF60 File Offset: 0x004DE160
	public UnitSpawnWaveData SiidimACWave1
	{
		get
		{
			return this.ExposedVariables.SiidimACWave1;
		}
		set
		{
			this.ExposedVariables.SiidimACWave1 = value;
		}
	}

	// Token: 0x17000941 RID: 2369
	// (get) Token: 0x06011300 RID: 70400 RVA: 0x004DFF70 File Offset: 0x004DE170
	// (set) Token: 0x06011301 RID: 70401 RVA: 0x004DFF80 File Offset: 0x004DE180
	public UnitSpawnWaveData SiidimACGroup2AAEscortWave
	{
		get
		{
			return this.ExposedVariables.SiidimACGroup2AAEscortWave;
		}
		set
		{
			this.ExposedVariables.SiidimACGroup2AAEscortWave = value;
		}
	}

	// Token: 0x17000942 RID: 2370
	// (get) Token: 0x06011302 RID: 70402 RVA: 0x004DFF90 File Offset: 0x004DE190
	// (set) Token: 0x06011303 RID: 70403 RVA: 0x004DFFA0 File Offset: 0x004DE1A0
	public UnitSpawnWaveData SiidimACWave2
	{
		get
		{
			return this.ExposedVariables.SiidimACWave2;
		}
		set
		{
			this.ExposedVariables.SiidimACWave2 = value;
		}
	}

	// Token: 0x17000943 RID: 2371
	// (get) Token: 0x06011304 RID: 70404 RVA: 0x004DFFB0 File Offset: 0x004DE1B0
	// (set) Token: 0x06011305 RID: 70405 RVA: 0x004DFFC0 File Offset: 0x004DE1C0
	public UnitSpawnWaveData SiidimACGroup2EscortWave2
	{
		get
		{
			return this.ExposedVariables.SiidimACGroup2EscortWave2;
		}
		set
		{
			this.ExposedVariables.SiidimACGroup2EscortWave2 = value;
		}
	}

	// Token: 0x17000944 RID: 2372
	// (get) Token: 0x06011306 RID: 70406 RVA: 0x004DFFD0 File Offset: 0x004DE1D0
	// (set) Token: 0x06011307 RID: 70407 RVA: 0x004DFFE0 File Offset: 0x004DE1E0
	public UnitSpawnWaveData SiidimACGroup1EscortWave2
	{
		get
		{
			return this.ExposedVariables.SiidimACGroup1EscortWave2;
		}
		set
		{
			this.ExposedVariables.SiidimACGroup1EscortWave2 = value;
		}
	}

	// Token: 0x17000945 RID: 2373
	// (get) Token: 0x06011308 RID: 70408 RVA: 0x004DFFF0 File Offset: 0x004DE1F0
	// (set) Token: 0x06011309 RID: 70409 RVA: 0x004E0000 File Offset: 0x004DE200
	public UnitSpawnWaveData SiidimACGroup1EscortWave1
	{
		get
		{
			return this.ExposedVariables.SiidimACGroup1EscortWave1;
		}
		set
		{
			this.ExposedVariables.SiidimACGroup1EscortWave1 = value;
		}
	}

	// Token: 0x17000946 RID: 2374
	// (get) Token: 0x0601130A RID: 70410 RVA: 0x004E0010 File Offset: 0x004DE210
	// (set) Token: 0x0601130B RID: 70411 RVA: 0x004E0020 File Offset: 0x004DE220
	public UnitSpawnWaveData SiidimACGroup2EscortWave3
	{
		get
		{
			return this.ExposedVariables.SiidimACGroup2EscortWave3;
		}
		set
		{
			this.ExposedVariables.SiidimACGroup2EscortWave3 = value;
		}
	}

	// Token: 0x17000947 RID: 2375
	// (get) Token: 0x0601130C RID: 70412 RVA: 0x004E0030 File Offset: 0x004DE230
	// (set) Token: 0x0601130D RID: 70413 RVA: 0x004E0040 File Offset: 0x004DE240
	public AttributeBuffSetData Buff_ArtilleryCruisers
	{
		get
		{
			return this.ExposedVariables.Buff_ArtilleryCruisers;
		}
		set
		{
			this.ExposedVariables.Buff_ArtilleryCruisers = value;
		}
	}

	// Token: 0x17000948 RID: 2376
	// (get) Token: 0x0601130E RID: 70414 RVA: 0x004E0050 File Offset: 0x004DE250
	// (set) Token: 0x0601130F RID: 70415 RVA: 0x004E0060 File Offset: 0x004DE260
	public UnitSpawnWaveData Wave_GaalsiFinal1
	{
		get
		{
			return this.ExposedVariables.Wave_GaalsiFinal1;
		}
		set
		{
			this.ExposedVariables.Wave_GaalsiFinal1 = value;
		}
	}

	// Token: 0x17000949 RID: 2377
	// (get) Token: 0x06011310 RID: 70416 RVA: 0x004E0070 File Offset: 0x004DE270
	// (set) Token: 0x06011311 RID: 70417 RVA: 0x004E0080 File Offset: 0x004DE280
	public SpawnFactoryData Factory_FinalAttackers
	{
		get
		{
			return this.ExposedVariables.Factory_FinalAttackers;
		}
		set
		{
			this.ExposedVariables.Factory_FinalAttackers = value;
		}
	}

	// Token: 0x1700094A RID: 2378
	// (get) Token: 0x06011312 RID: 70418 RVA: 0x004E0090 File Offset: 0x004DE290
	// (set) Token: 0x06011313 RID: 70419 RVA: 0x004E00A0 File Offset: 0x004DE2A0
	public string StopMainAttackers
	{
		get
		{
			return this.ExposedVariables.StopMainAttackers;
		}
		set
		{
			this.ExposedVariables.StopMainAttackers = value;
		}
	}

	// Token: 0x1700094B RID: 2379
	// (get) Token: 0x06011314 RID: 70420 RVA: 0x004E00B0 File Offset: 0x004DE2B0
	// (set) Token: 0x06011315 RID: 70421 RVA: 0x004E00C0 File Offset: 0x004DE2C0
	public QueryContainer QueryGaalsiBombers
	{
		get
		{
			return this.ExposedVariables.QueryGaalsiBombers;
		}
		set
		{
			this.ExposedVariables.QueryGaalsiBombers = value;
		}
	}

	// Token: 0x1700094C RID: 2380
	// (get) Token: 0x06011316 RID: 70422 RVA: 0x004E00D0 File Offset: 0x004DE2D0
	// (set) Token: 0x06011317 RID: 70423 RVA: 0x004E00E0 File Offset: 0x004DE2E0
	public AttributeBuffSetData Buff_GaalsiFinal
	{
		get
		{
			return this.ExposedVariables.Buff_GaalsiFinal;
		}
		set
		{
			this.ExposedVariables.Buff_GaalsiFinal = value;
		}
	}

	// Token: 0x1700094D RID: 2381
	// (get) Token: 0x06011318 RID: 70424 RVA: 0x004E00F0 File Offset: 0x004DE2F0
	// (set) Token: 0x06011319 RID: 70425 RVA: 0x004E0100 File Offset: 0x004DE300
	public AttributeBuffSetData Buff_ArtyEscorts
	{
		get
		{
			return this.ExposedVariables.Buff_ArtyEscorts;
		}
		set
		{
			this.ExposedVariables.Buff_ArtyEscorts = value;
		}
	}

	// Token: 0x1700094E RID: 2382
	// (get) Token: 0x0601131A RID: 70426 RVA: 0x004E0110 File Offset: 0x004DE310
	// (set) Token: 0x0601131B RID: 70427 RVA: 0x004E0120 File Offset: 0x004DE320
	public UnitSpawnWaveData Wave_Sakala
	{
		get
		{
			return this.ExposedVariables.Wave_Sakala;
		}
		set
		{
			this.ExposedVariables.Wave_Sakala = value;
		}
	}

	// Token: 0x1700094F RID: 2383
	// (get) Token: 0x0601131C RID: 70428 RVA: 0x004E0130 File Offset: 0x004DE330
	// (set) Token: 0x0601131D RID: 70429 RVA: 0x004E0140 File Offset: 0x004DE340
	public AttributeBuffSetData Buff_SakalaSpeed
	{
		get
		{
			return this.ExposedVariables.Buff_SakalaSpeed;
		}
		set
		{
			this.ExposedVariables.Buff_SakalaSpeed = value;
		}
	}

	// Token: 0x17000950 RID: 2384
	// (get) Token: 0x0601131E RID: 70430 RVA: 0x004E0150 File Offset: 0x004DE350
	// (set) Token: 0x0601131F RID: 70431 RVA: 0x004E0160 File Offset: 0x004DE360
	public QueryContainer QueryPlayerCarrier
	{
		get
		{
			return this.ExposedVariables.QueryPlayerCarrier;
		}
		set
		{
			this.ExposedVariables.QueryPlayerCarrier = value;
		}
	}

	// Token: 0x17000951 RID: 2385
	// (get) Token: 0x06011320 RID: 70432 RVA: 0x004E0170 File Offset: 0x004DE370
	// (set) Token: 0x06011321 RID: 70433 RVA: 0x004E0180 File Offset: 0x004DE380
	public QueryContainer QuerySakala
	{
		get
		{
			return this.ExposedVariables.QuerySakala;
		}
		set
		{
			this.ExposedVariables.QuerySakala = value;
		}
	}

	// Token: 0x17000952 RID: 2386
	// (get) Token: 0x06011322 RID: 70434 RVA: 0x004E0190 File Offset: 0x004DE390
	// (set) Token: 0x06011323 RID: 70435 RVA: 0x004E01A0 File Offset: 0x004DE3A0
	public QueryContainer QuerySakalaAttackers
	{
		get
		{
			return this.ExposedVariables.QuerySakalaAttackers;
		}
		set
		{
			this.ExposedVariables.QuerySakalaAttackers = value;
		}
	}

	// Token: 0x17000953 RID: 2387
	// (get) Token: 0x06011324 RID: 70436 RVA: 0x004E01B0 File Offset: 0x004DE3B0
	// (set) Token: 0x06011325 RID: 70437 RVA: 0x004E01C0 File Offset: 0x004DE3C0
	public SpawnFactoryData Factory_SidAttackers
	{
		get
		{
			return this.ExposedVariables.Factory_SidAttackers;
		}
		set
		{
			this.ExposedVariables.Factory_SidAttackers = value;
		}
	}

	// Token: 0x17000954 RID: 2388
	// (get) Token: 0x06011326 RID: 70438 RVA: 0x004E01D0 File Offset: 0x004DE3D0
	// (set) Token: 0x06011327 RID: 70439 RVA: 0x004E01E0 File Offset: 0x004DE3E0
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

	// Token: 0x17000955 RID: 2389
	// (get) Token: 0x06011328 RID: 70440 RVA: 0x004E01F0 File Offset: 0x004DE3F0
	// (set) Token: 0x06011329 RID: 70441 RVA: 0x004E0200 File Offset: 0x004DE400
	public UnitSpawnWaveData Wave_GaalsiFinalHard
	{
		get
		{
			return this.ExposedVariables.Wave_GaalsiFinalHard;
		}
		set
		{
			this.ExposedVariables.Wave_GaalsiFinalHard = value;
		}
	}

	// Token: 0x0601132A RID: 70442 RVA: 0x004E0210 File Offset: 0x004DE410
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

	// Token: 0x0601132B RID: 70443 RVA: 0x004E0278 File Offset: 0x004DE478
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0601132C RID: 70444 RVA: 0x004E0288 File Offset: 0x004DE488
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0601132D RID: 70445 RVA: 0x004E0298 File Offset: 0x004DE498
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0601132E RID: 70446 RVA: 0x004E02A8 File Offset: 0x004DE4A8
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0601132F RID: 70447 RVA: 0x004E02B8 File Offset: 0x004DE4B8
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04019DA3 RID: 105891
	public M11_Gameplay3 ExposedVariables = new M11_Gameplay3();
}
