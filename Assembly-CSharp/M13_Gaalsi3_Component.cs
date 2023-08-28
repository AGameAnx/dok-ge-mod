using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x020008AB RID: 2219
[AddComponentMenu("uScript/Graphs/M13_Gaalsi3")]
public class M13_Gaalsi3_Component : uScriptCode
{
	// Token: 0x06013A1C RID: 80412 RVA: 0x005902E0 File Offset: 0x0058E4E0
	public M13_Gaalsi3_Component()
	{
	}

	// Token: 0x170009EA RID: 2538
	// (get) Token: 0x06013A1D RID: 80413 RVA: 0x005902F4 File Offset: 0x0058E4F4
	// (set) Token: 0x06013A1E RID: 80414 RVA: 0x00590304 File Offset: 0x0058E504
	public UnitSpawnWaveData TypeFathership
	{
		get
		{
			return this.ExposedVariables.TypeFathership;
		}
		set
		{
			this.ExposedVariables.TypeFathership = value;
		}
	}

	// Token: 0x170009EB RID: 2539
	// (get) Token: 0x06013A1F RID: 80415 RVA: 0x00590314 File Offset: 0x0058E514
	// (set) Token: 0x06013A20 RID: 80416 RVA: 0x00590324 File Offset: 0x0058E524
	public QueryContainer Q_CruisersK
	{
		get
		{
			return this.ExposedVariables.Q_CruisersK;
		}
		set
		{
			this.ExposedVariables.Q_CruisersK = value;
		}
	}

	// Token: 0x170009EC RID: 2540
	// (get) Token: 0x06013A21 RID: 80417 RVA: 0x00590334 File Offset: 0x0058E534
	// (set) Token: 0x06013A22 RID: 80418 RVA: 0x00590344 File Offset: 0x0058E544
	public UnitSpawnWaveData WaveCruisersL
	{
		get
		{
			return this.ExposedVariables.WaveCruisersL;
		}
		set
		{
			this.ExposedVariables.WaveCruisersL = value;
		}
	}

	// Token: 0x170009ED RID: 2541
	// (get) Token: 0x06013A23 RID: 80419 RVA: 0x00590354 File Offset: 0x0058E554
	// (set) Token: 0x06013A24 RID: 80420 RVA: 0x00590364 File Offset: 0x0058E564
	public UnitSpawnWaveData WaveCruisersK
	{
		get
		{
			return this.ExposedVariables.WaveCruisersK;
		}
		set
		{
			this.ExposedVariables.WaveCruisersK = value;
		}
	}

	// Token: 0x170009EE RID: 2542
	// (get) Token: 0x06013A25 RID: 80421 RVA: 0x00590374 File Offset: 0x0058E574
	// (set) Token: 0x06013A26 RID: 80422 RVA: 0x00590384 File Offset: 0x0058E584
	public UnitSpawnWaveData WaveGuardL
	{
		get
		{
			return this.ExposedVariables.WaveGuardL;
		}
		set
		{
			this.ExposedVariables.WaveGuardL = value;
		}
	}

	// Token: 0x170009EF RID: 2543
	// (get) Token: 0x06013A27 RID: 80423 RVA: 0x00590394 File Offset: 0x0058E594
	// (set) Token: 0x06013A28 RID: 80424 RVA: 0x005903A4 File Offset: 0x0058E5A4
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

	// Token: 0x170009F0 RID: 2544
	// (get) Token: 0x06013A29 RID: 80425 RVA: 0x005903B4 File Offset: 0x0058E5B4
	// (set) Token: 0x06013A2A RID: 80426 RVA: 0x005903C4 File Offset: 0x0058E5C4
	public QueryContainer Q_CruisersL
	{
		get
		{
			return this.ExposedVariables.Q_CruisersL;
		}
		set
		{
			this.ExposedVariables.Q_CruisersL = value;
		}
	}

	// Token: 0x170009F1 RID: 2545
	// (get) Token: 0x06013A2B RID: 80427 RVA: 0x005903D4 File Offset: 0x0058E5D4
	// (set) Token: 0x06013A2C RID: 80428 RVA: 0x005903E4 File Offset: 0x0058E5E4
	public UnitSpawnWaveData WaveGuardK
	{
		get
		{
			return this.ExposedVariables.WaveGuardK;
		}
		set
		{
			this.ExposedVariables.WaveGuardK = value;
		}
	}

	// Token: 0x170009F2 RID: 2546
	// (get) Token: 0x06013A2D RID: 80429 RVA: 0x005903F4 File Offset: 0x0058E5F4
	// (set) Token: 0x06013A2E RID: 80430 RVA: 0x00590404 File Offset: 0x0058E604
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

	// Token: 0x170009F3 RID: 2547
	// (get) Token: 0x06013A2F RID: 80431 RVA: 0x00590414 File Offset: 0x0058E614
	// (set) Token: 0x06013A30 RID: 80432 RVA: 0x00590424 File Offset: 0x0058E624
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

	// Token: 0x170009F4 RID: 2548
	// (get) Token: 0x06013A31 RID: 80433 RVA: 0x00590434 File Offset: 0x0058E634
	// (set) Token: 0x06013A32 RID: 80434 RVA: 0x00590444 File Offset: 0x0058E644
	public UnitSpawnWaveData WaveCruisersJ
	{
		get
		{
			return this.ExposedVariables.WaveCruisersJ;
		}
		set
		{
			this.ExposedVariables.WaveCruisersJ = value;
		}
	}

	// Token: 0x170009F5 RID: 2549
	// (get) Token: 0x06013A33 RID: 80435 RVA: 0x00590454 File Offset: 0x0058E654
	// (set) Token: 0x06013A34 RID: 80436 RVA: 0x00590464 File Offset: 0x0058E664
	public UnitSpawnWaveData WaveGuardJ1
	{
		get
		{
			return this.ExposedVariables.WaveGuardJ1;
		}
		set
		{
			this.ExposedVariables.WaveGuardJ1 = value;
		}
	}

	// Token: 0x170009F6 RID: 2550
	// (get) Token: 0x06013A35 RID: 80437 RVA: 0x00590474 File Offset: 0x0058E674
	// (set) Token: 0x06013A36 RID: 80438 RVA: 0x00590484 File Offset: 0x0058E684
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

	// Token: 0x170009F7 RID: 2551
	// (get) Token: 0x06013A37 RID: 80439 RVA: 0x00590494 File Offset: 0x0058E694
	// (set) Token: 0x06013A38 RID: 80440 RVA: 0x005904A4 File Offset: 0x0058E6A4
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

	// Token: 0x170009F8 RID: 2552
	// (get) Token: 0x06013A39 RID: 80441 RVA: 0x005904B4 File Offset: 0x0058E6B4
	// (set) Token: 0x06013A3A RID: 80442 RVA: 0x005904C4 File Offset: 0x0058E6C4
	public UnitSpawnWaveData WaveGuardJ2
	{
		get
		{
			return this.ExposedVariables.WaveGuardJ2;
		}
		set
		{
			this.ExposedVariables.WaveGuardJ2 = value;
		}
	}

	// Token: 0x170009F9 RID: 2553
	// (get) Token: 0x06013A3B RID: 80443 RVA: 0x005904D4 File Offset: 0x0058E6D4
	// (set) Token: 0x06013A3C RID: 80444 RVA: 0x005904E4 File Offset: 0x0058E6E4
	public QueryContainer Q_PlayerVehicles
	{
		get
		{
			return this.ExposedVariables.Q_PlayerVehicles;
		}
		set
		{
			this.ExposedVariables.Q_PlayerVehicles = value;
		}
	}

	// Token: 0x170009FA RID: 2554
	// (get) Token: 0x06013A3D RID: 80445 RVA: 0x005904F4 File Offset: 0x0058E6F4
	// (set) Token: 0x06013A3E RID: 80446 RVA: 0x00590504 File Offset: 0x0058E704
	public AttributeBuffSetData Buff_Catamaran_Upgrade
	{
		get
		{
			return this.ExposedVariables.Buff_Catamaran_Upgrade;
		}
		set
		{
			this.ExposedVariables.Buff_Catamaran_Upgrade = value;
		}
	}

	// Token: 0x170009FB RID: 2555
	// (get) Token: 0x06013A3F RID: 80447 RVA: 0x00590514 File Offset: 0x0058E714
	// (set) Token: 0x06013A40 RID: 80448 RVA: 0x00590524 File Offset: 0x0058E724
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

	// Token: 0x170009FC RID: 2556
	// (get) Token: 0x06013A41 RID: 80449 RVA: 0x00590534 File Offset: 0x0058E734
	// (set) Token: 0x06013A42 RID: 80450 RVA: 0x00590544 File Offset: 0x0058E744
	public AttributeBuffSetData Buff_Slow1
	{
		get
		{
			return this.ExposedVariables.Buff_Slow1;
		}
		set
		{
			this.ExposedVariables.Buff_Slow1 = value;
		}
	}

	// Token: 0x170009FD RID: 2557
	// (get) Token: 0x06013A43 RID: 80451 RVA: 0x00590554 File Offset: 0x0058E754
	// (set) Token: 0x06013A44 RID: 80452 RVA: 0x00590564 File Offset: 0x0058E764
	public AttributeBuffSetData Buff_Slow2
	{
		get
		{
			return this.ExposedVariables.Buff_Slow2;
		}
		set
		{
			this.ExposedVariables.Buff_Slow2 = value;
		}
	}

	// Token: 0x170009FE RID: 2558
	// (get) Token: 0x06013A45 RID: 80453 RVA: 0x00590574 File Offset: 0x0058E774
	// (set) Token: 0x06013A46 RID: 80454 RVA: 0x00590584 File Offset: 0x0058E784
	public AttributeBuffSetData Buff_Slow3
	{
		get
		{
			return this.ExposedVariables.Buff_Slow3;
		}
		set
		{
			this.ExposedVariables.Buff_Slow3 = value;
		}
	}

	// Token: 0x06013A47 RID: 80455 RVA: 0x00590594 File Offset: 0x0058E794
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

	// Token: 0x06013A48 RID: 80456 RVA: 0x005905FC File Offset: 0x0058E7FC
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06013A49 RID: 80457 RVA: 0x0059060C File Offset: 0x0058E80C
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06013A4A RID: 80458 RVA: 0x0059061C File Offset: 0x0058E81C
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06013A4B RID: 80459 RVA: 0x0059062C File Offset: 0x0058E82C
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06013A4C RID: 80460 RVA: 0x0059063C File Offset: 0x0058E83C
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401E0CB RID: 123083
	public M13_Gaalsi3 ExposedVariables = new M13_Gaalsi3();
}
