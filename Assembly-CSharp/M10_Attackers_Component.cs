using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Scripting;
using UnityEngine;

// Token: 0x0200087F RID: 2175
[AddComponentMenu("uScript/Graphs/M10_Attackers")]
public class M10_Attackers_Component : uScriptCode
{
	// Token: 0x0600F3ED RID: 62445 RVA: 0x004514B4 File Offset: 0x0044F6B4
	public M10_Attackers_Component()
	{
	}

	// Token: 0x170008AD RID: 2221
	// (get) Token: 0x0600F3EE RID: 62446 RVA: 0x004514C8 File Offset: 0x0044F6C8
	// (set) Token: 0x0600F3EF RID: 62447 RVA: 0x004514D8 File Offset: 0x0044F6D8
	public UnitSpawnWaveData FinalAttackers
	{
		get
		{
			return this.ExposedVariables.FinalAttackers;
		}
		set
		{
			this.ExposedVariables.FinalAttackers = value;
		}
	}

	// Token: 0x170008AE RID: 2222
	// (get) Token: 0x0600F3F0 RID: 62448 RVA: 0x004514E8 File Offset: 0x0044F6E8
	// (set) Token: 0x0600F3F1 RID: 62449 RVA: 0x004514F8 File Offset: 0x0044F6F8
	public AttributeBuffSetData Buff_SpeedDebuff
	{
		get
		{
			return this.ExposedVariables.Buff_SpeedDebuff;
		}
		set
		{
			this.ExposedVariables.Buff_SpeedDebuff = value;
		}
	}

	// Token: 0x170008AF RID: 2223
	// (get) Token: 0x0600F3F2 RID: 62450 RVA: 0x00451508 File Offset: 0x0044F708
	// (set) Token: 0x0600F3F3 RID: 62451 RVA: 0x00451518 File Offset: 0x0044F718
	public UnitSpawnWaveData Vanguard
	{
		get
		{
			return this.ExposedVariables.Vanguard;
		}
		set
		{
			this.ExposedVariables.Vanguard = value;
		}
	}

	// Token: 0x170008B0 RID: 2224
	// (get) Token: 0x0600F3F4 RID: 62452 RVA: 0x00451528 File Offset: 0x0044F728
	// (set) Token: 0x0600F3F5 RID: 62453 RVA: 0x00451538 File Offset: 0x0044F738
	public SpawnFactoryData PlateauFactory
	{
		get
		{
			return this.ExposedVariables.PlateauFactory;
		}
		set
		{
			this.ExposedVariables.PlateauFactory = value;
		}
	}

	// Token: 0x170008B1 RID: 2225
	// (get) Token: 0x0600F3F6 RID: 62454 RVA: 0x00451548 File Offset: 0x0044F748
	// (set) Token: 0x0600F3F7 RID: 62455 RVA: 0x00451558 File Offset: 0x0044F758
	public SpawnFactoryData AttackerFactory
	{
		get
		{
			return this.ExposedVariables.AttackerFactory;
		}
		set
		{
			this.ExposedVariables.AttackerFactory = value;
		}
	}

	// Token: 0x170008B2 RID: 2226
	// (get) Token: 0x0600F3F8 RID: 62456 RVA: 0x00451568 File Offset: 0x0044F768
	// (set) Token: 0x0600F3F9 RID: 62457 RVA: 0x00451578 File Offset: 0x0044F778
	public QueryContainer QueryRandomAttackers
	{
		get
		{
			return this.ExposedVariables.QueryRandomAttackers;
		}
		set
		{
			this.ExposedVariables.QueryRandomAttackers = value;
		}
	}

	// Token: 0x170008B3 RID: 2227
	// (get) Token: 0x0600F3FA RID: 62458 RVA: 0x00451588 File Offset: 0x0044F788
	// (set) Token: 0x0600F3FB RID: 62459 RVA: 0x00451598 File Offset: 0x0044F798
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

	// Token: 0x170008B4 RID: 2228
	// (get) Token: 0x0600F3FC RID: 62460 RVA: 0x004515A8 File Offset: 0x0044F7A8
	// (set) Token: 0x0600F3FD RID: 62461 RVA: 0x004515B8 File Offset: 0x0044F7B8
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

	// Token: 0x170008B5 RID: 2229
	// (get) Token: 0x0600F3FE RID: 62462 RVA: 0x004515C8 File Offset: 0x0044F7C8
	// (set) Token: 0x0600F3FF RID: 62463 RVA: 0x004515D8 File Offset: 0x0044F7D8
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

	// Token: 0x0600F400 RID: 62464 RVA: 0x004515E8 File Offset: 0x0044F7E8
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

	// Token: 0x0600F401 RID: 62465 RVA: 0x00451650 File Offset: 0x0044F850
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600F402 RID: 62466 RVA: 0x00451660 File Offset: 0x0044F860
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600F403 RID: 62467 RVA: 0x00451670 File Offset: 0x0044F870
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600F404 RID: 62468 RVA: 0x00451680 File Offset: 0x0044F880
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600F405 RID: 62469 RVA: 0x00451690 File Offset: 0x0044F890
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04016DC7 RID: 93639
	public M10_Attackers ExposedVariables = new M10_Attackers();
}
