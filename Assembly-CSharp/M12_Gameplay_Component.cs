using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x0200089B RID: 2203
[AddComponentMenu("uScript/Graphs/M12_Gameplay")]
public class M12_Gameplay_Component : uScriptCode
{
	// Token: 0x06011EAA RID: 73386 RVA: 0x005176F8 File Offset: 0x005158F8
	public M12_Gameplay_Component()
	{
	}

	// Token: 0x17000978 RID: 2424
	// (get) Token: 0x06011EAB RID: 73387 RVA: 0x0051770C File Offset: 0x0051590C
	// (set) Token: 0x06011EAC RID: 73388 RVA: 0x0051771C File Offset: 0x0051591C
	public QueryContainer Q_PlayerFleet
	{
		get
		{
			return this.ExposedVariables.Q_PlayerFleet;
		}
		set
		{
			this.ExposedVariables.Q_PlayerFleet = value;
		}
	}

	// Token: 0x17000979 RID: 2425
	// (get) Token: 0x06011EAD RID: 73389 RVA: 0x0051772C File Offset: 0x0051592C
	// (set) Token: 0x06011EAE RID: 73390 RVA: 0x0051773C File Offset: 0x0051593C
	public QueryContainer Q_PlayerCruisers
	{
		get
		{
			return this.ExposedVariables.Q_PlayerCruisers;
		}
		set
		{
			this.ExposedVariables.Q_PlayerCruisers = value;
		}
	}

	// Token: 0x1700097A RID: 2426
	// (get) Token: 0x06011EAF RID: 73391 RVA: 0x0051774C File Offset: 0x0051594C
	// (set) Token: 0x06011EB0 RID: 73392 RVA: 0x0051775C File Offset: 0x0051595C
	public UnitSpawnWaveData RachelEscort
	{
		get
		{
			return this.ExposedVariables.RachelEscort;
		}
		set
		{
			this.ExposedVariables.RachelEscort = value;
		}
	}

	// Token: 0x1700097B RID: 2427
	// (get) Token: 0x06011EB1 RID: 73393 RVA: 0x0051776C File Offset: 0x0051596C
	// (set) Token: 0x06011EB2 RID: 73394 RVA: 0x0051777C File Offset: 0x0051597C
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

	// Token: 0x1700097C RID: 2428
	// (get) Token: 0x06011EB3 RID: 73395 RVA: 0x0051778C File Offset: 0x0051598C
	// (set) Token: 0x06011EB4 RID: 73396 RVA: 0x0051779C File Offset: 0x0051599C
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

	// Token: 0x1700097D RID: 2429
	// (get) Token: 0x06011EB5 RID: 73397 RVA: 0x005177AC File Offset: 0x005159AC
	// (set) Token: 0x06011EB6 RID: 73398 RVA: 0x005177BC File Offset: 0x005159BC
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

	// Token: 0x1700097E RID: 2430
	// (get) Token: 0x06011EB7 RID: 73399 RVA: 0x005177CC File Offset: 0x005159CC
	// (set) Token: 0x06011EB8 RID: 73400 RVA: 0x005177DC File Offset: 0x005159DC
	public QueryContainer Q_SupportCruisers
	{
		get
		{
			return this.ExposedVariables.Q_SupportCruisers;
		}
		set
		{
			this.ExposedVariables.Q_SupportCruisers = value;
		}
	}

	// Token: 0x1700097F RID: 2431
	// (get) Token: 0x06011EB9 RID: 73401 RVA: 0x005177EC File Offset: 0x005159EC
	// (set) Token: 0x06011EBA RID: 73402 RVA: 0x005177FC File Offset: 0x005159FC
	public QueryContainer Q_InvulnerableDuringIM03
	{
		get
		{
			return this.ExposedVariables.Q_InvulnerableDuringIM03;
		}
		set
		{
			this.ExposedVariables.Q_InvulnerableDuringIM03 = value;
		}
	}

	// Token: 0x17000980 RID: 2432
	// (get) Token: 0x06011EBB RID: 73403 RVA: 0x0051780C File Offset: 0x00515A0C
	// (set) Token: 0x06011EBC RID: 73404 RVA: 0x0051781C File Offset: 0x00515A1C
	public QueryContainer Q_InvulnerableDuringIM04
	{
		get
		{
			return this.ExposedVariables.Q_InvulnerableDuringIM04;
		}
		set
		{
			this.ExposedVariables.Q_InvulnerableDuringIM04 = value;
		}
	}

	// Token: 0x17000981 RID: 2433
	// (get) Token: 0x06011EBD RID: 73405 RVA: 0x0051782C File Offset: 0x00515A2C
	// (set) Token: 0x06011EBE RID: 73406 RVA: 0x0051783C File Offset: 0x00515A3C
	public QueryContainer Q_Sakala
	{
		get
		{
			return this.ExposedVariables.Q_Sakala;
		}
		set
		{
			this.ExposedVariables.Q_Sakala = value;
		}
	}

	// Token: 0x17000982 RID: 2434
	// (get) Token: 0x06011EBF RID: 73407 RVA: 0x0051784C File Offset: 0x00515A4C
	// (set) Token: 0x06011EC0 RID: 73408 RVA: 0x0051785C File Offset: 0x00515A5C
	public UnitSpawnWaveData TypeScanner
	{
		get
		{
			return this.ExposedVariables.TypeScanner;
		}
		set
		{
			this.ExposedVariables.TypeScanner = value;
		}
	}

	// Token: 0x17000983 RID: 2435
	// (get) Token: 0x06011EC1 RID: 73409 RVA: 0x0051786C File Offset: 0x00515A6C
	// (set) Token: 0x06011EC2 RID: 73410 RVA: 0x0051787C File Offset: 0x00515A7C
	public UnitSpawnWaveData TypeTurret
	{
		get
		{
			return this.ExposedVariables.TypeTurret;
		}
		set
		{
			this.ExposedVariables.TypeTurret = value;
		}
	}

	// Token: 0x17000984 RID: 2436
	// (get) Token: 0x06011EC3 RID: 73411 RVA: 0x0051788C File Offset: 0x00515A8C
	// (set) Token: 0x06011EC4 RID: 73412 RVA: 0x0051789C File Offset: 0x00515A9C
	public AttributeBuffSetData NoAutoTarget
	{
		get
		{
			return this.ExposedVariables.NoAutoTarget;
		}
		set
		{
			this.ExposedVariables.NoAutoTarget = value;
		}
	}

	// Token: 0x17000985 RID: 2437
	// (get) Token: 0x06011EC5 RID: 73413 RVA: 0x005178AC File Offset: 0x00515AAC
	// (set) Token: 0x06011EC6 RID: 73414 RVA: 0x005178BC File Offset: 0x00515ABC
	public GameObject NIS12_1_BlendCamera
	{
		get
		{
			return this.ExposedVariables.NIS12_1_BlendCamera;
		}
		set
		{
			this.ExposedVariables.NIS12_1_BlendCamera = value;
		}
	}

	// Token: 0x17000986 RID: 2438
	// (get) Token: 0x06011EC7 RID: 73415 RVA: 0x005178CC File Offset: 0x00515ACC
	// (set) Token: 0x06011EC8 RID: 73416 RVA: 0x005178DC File Offset: 0x00515ADC
	public GameObject NIS1202Camera
	{
		get
		{
			return this.ExposedVariables.NIS1202Camera;
		}
		set
		{
			this.ExposedVariables.NIS1202Camera = value;
		}
	}

	// Token: 0x17000987 RID: 2439
	// (get) Token: 0x06011EC9 RID: 73417 RVA: 0x005178EC File Offset: 0x00515AEC
	// (set) Token: 0x06011ECA RID: 73418 RVA: 0x005178FC File Offset: 0x00515AFC
	public GameObject NIS1203Camera
	{
		get
		{
			return this.ExposedVariables.NIS1203Camera;
		}
		set
		{
			this.ExposedVariables.NIS1203Camera = value;
		}
	}

	// Token: 0x06011ECB RID: 73419 RVA: 0x0051790C File Offset: 0x00515B0C
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

	// Token: 0x06011ECC RID: 73420 RVA: 0x00517974 File Offset: 0x00515B74
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x06011ECD RID: 73421 RVA: 0x00517984 File Offset: 0x00515B84
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06011ECE RID: 73422 RVA: 0x00517994 File Offset: 0x00515B94
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06011ECF RID: 73423 RVA: 0x005179A4 File Offset: 0x00515BA4
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x06011ED0 RID: 73424 RVA: 0x005179B4 File Offset: 0x00515BB4
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401AFA9 RID: 110505
	public M12_Gameplay ExposedVariables = new M12_Gameplay();
}
