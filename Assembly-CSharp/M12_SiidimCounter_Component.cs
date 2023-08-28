using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using UnityEngine;

// Token: 0x020008A1 RID: 2209
[AddComponentMenu("uScript/Graphs/M12_SiidimCounter")]
public class M12_SiidimCounter_Component : uScriptCode
{
	// Token: 0x06012ABD RID: 76477 RVA: 0x0054B5B4 File Offset: 0x005497B4
	public M12_SiidimCounter_Component()
	{
	}

	// Token: 0x170009B9 RID: 2489
	// (get) Token: 0x06012ABE RID: 76478 RVA: 0x0054B5C8 File Offset: 0x005497C8
	// (set) Token: 0x06012ABF RID: 76479 RVA: 0x0054B5D8 File Offset: 0x005497D8
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

	// Token: 0x170009BA RID: 2490
	// (get) Token: 0x06012AC0 RID: 76480 RVA: 0x0054B5E8 File Offset: 0x005497E8
	// (set) Token: 0x06012AC1 RID: 76481 RVA: 0x0054B5F8 File Offset: 0x005497F8
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

	// Token: 0x170009BB RID: 2491
	// (get) Token: 0x06012AC2 RID: 76482 RVA: 0x0054B608 File Offset: 0x00549808
	// (set) Token: 0x06012AC3 RID: 76483 RVA: 0x0054B618 File Offset: 0x00549818
	public UnitSpawnWaveData TypeEscort
	{
		get
		{
			return this.ExposedVariables.TypeEscort;
		}
		set
		{
			this.ExposedVariables.TypeEscort = value;
		}
	}

	// Token: 0x170009BC RID: 2492
	// (get) Token: 0x06012AC4 RID: 76484 RVA: 0x0054B628 File Offset: 0x00549828
	// (set) Token: 0x06012AC5 RID: 76485 RVA: 0x0054B638 File Offset: 0x00549838
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

	// Token: 0x170009BD RID: 2493
	// (get) Token: 0x06012AC6 RID: 76486 RVA: 0x0054B648 File Offset: 0x00549848
	// (set) Token: 0x06012AC7 RID: 76487 RVA: 0x0054B658 File Offset: 0x00549858
	public QueryContainer Q_Vanguard
	{
		get
		{
			return this.ExposedVariables.Q_Vanguard;
		}
		set
		{
			this.ExposedVariables.Q_Vanguard = value;
		}
	}

	// Token: 0x170009BE RID: 2494
	// (get) Token: 0x06012AC8 RID: 76488 RVA: 0x0054B668 File Offset: 0x00549868
	// (set) Token: 0x06012AC9 RID: 76489 RVA: 0x0054B678 File Offset: 0x00549878
	public UnitSpawnWaveData BigGroupRimSpawn
	{
		get
		{
			return this.ExposedVariables.BigGroupRimSpawn;
		}
		set
		{
			this.ExposedVariables.BigGroupRimSpawn = value;
		}
	}

	// Token: 0x170009BF RID: 2495
	// (get) Token: 0x06012ACA RID: 76490 RVA: 0x0054B688 File Offset: 0x00549888
	// (set) Token: 0x06012ACB RID: 76491 RVA: 0x0054B698 File Offset: 0x00549898
	public UnitSpawnWaveData TypeHACAA
	{
		get
		{
			return this.ExposedVariables.TypeHACAA;
		}
		set
		{
			this.ExposedVariables.TypeHACAA = value;
		}
	}

	// Token: 0x170009C0 RID: 2496
	// (get) Token: 0x06012ACC RID: 76492 RVA: 0x0054B6A8 File Offset: 0x005498A8
	// (set) Token: 0x06012ACD RID: 76493 RVA: 0x0054B6B8 File Offset: 0x005498B8
	public UnitSpawnWaveData RailgunsAndEscorts
	{
		get
		{
			return this.ExposedVariables.RailgunsAndEscorts;
		}
		set
		{
			this.ExposedVariables.RailgunsAndEscorts = value;
		}
	}

	// Token: 0x170009C1 RID: 2497
	// (get) Token: 0x06012ACE RID: 76494 RVA: 0x0054B6C8 File Offset: 0x005498C8
	// (set) Token: 0x06012ACF RID: 76495 RVA: 0x0054B6D8 File Offset: 0x005498D8
	public QueryContainer Q_RandomRimSpawnEnemies
	{
		get
		{
			return this.ExposedVariables.Q_RandomRimSpawnEnemies;
		}
		set
		{
			this.ExposedVariables.Q_RandomRimSpawnEnemies = value;
		}
	}

	// Token: 0x170009C2 RID: 2498
	// (get) Token: 0x06012AD0 RID: 76496 RVA: 0x0054B6E8 File Offset: 0x005498E8
	// (set) Token: 0x06012AD1 RID: 76497 RVA: 0x0054B6F8 File Offset: 0x005498F8
	public QueryContainer Q_RachelEscort
	{
		get
		{
			return this.ExposedVariables.Q_RachelEscort;
		}
		set
		{
			this.ExposedVariables.Q_RachelEscort = value;
		}
	}

	// Token: 0x06012AD2 RID: 76498 RVA: 0x0054B708 File Offset: 0x00549908
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

	// Token: 0x06012AD3 RID: 76499 RVA: 0x0054B770 File Offset: 0x00549970
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06012AD4 RID: 76500 RVA: 0x0054B780 File Offset: 0x00549980
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06012AD5 RID: 76501 RVA: 0x0054B790 File Offset: 0x00549990
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06012AD6 RID: 76502 RVA: 0x0054B7A0 File Offset: 0x005499A0
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06012AD7 RID: 76503 RVA: 0x0054B7B0 File Offset: 0x005499B0
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401C2B2 RID: 115378
	public M12_SiidimCounter ExposedVariables = new M12_SiidimCounter();
}
