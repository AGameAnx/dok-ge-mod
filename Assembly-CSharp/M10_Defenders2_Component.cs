using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x02000884 RID: 2180
[AddComponentMenu("uScript/Graphs/M10_Defenders2")]
public class M10_Defenders2_Component : uScriptCode
{
	// Token: 0x0600FE31 RID: 65073 RVA: 0x004832C8 File Offset: 0x004814C8
	public M10_Defenders2_Component()
	{
	}

	// Token: 0x170008BC RID: 2236
	// (get) Token: 0x0600FE32 RID: 65074 RVA: 0x004832DC File Offset: 0x004814DC
	// (set) Token: 0x0600FE33 RID: 65075 RVA: 0x004832EC File Offset: 0x004814EC
	public UnitSpawnWaveData S_Hardpoint1
	{
		get
		{
			return this.ExposedVariables.S_Hardpoint1;
		}
		set
		{
			this.ExposedVariables.S_Hardpoint1 = value;
		}
	}

	// Token: 0x170008BD RID: 2237
	// (get) Token: 0x0600FE34 RID: 65076 RVA: 0x004832FC File Offset: 0x004814FC
	// (set) Token: 0x0600FE35 RID: 65077 RVA: 0x0048330C File Offset: 0x0048150C
	public UnitSpawnWaveData S_Hardpoint2
	{
		get
		{
			return this.ExposedVariables.S_Hardpoint2;
		}
		set
		{
			this.ExposedVariables.S_Hardpoint2 = value;
		}
	}

	// Token: 0x170008BE RID: 2238
	// (get) Token: 0x0600FE36 RID: 65078 RVA: 0x0048331C File Offset: 0x0048151C
	// (set) Token: 0x0600FE37 RID: 65079 RVA: 0x0048332C File Offset: 0x0048152C
	public UnitSpawnWaveData S_Hardpoint3
	{
		get
		{
			return this.ExposedVariables.S_Hardpoint3;
		}
		set
		{
			this.ExposedVariables.S_Hardpoint3 = value;
		}
	}

	// Token: 0x170008BF RID: 2239
	// (get) Token: 0x0600FE38 RID: 65080 RVA: 0x0048333C File Offset: 0x0048153C
	// (set) Token: 0x0600FE39 RID: 65081 RVA: 0x0048334C File Offset: 0x0048154C
	public UnitSpawnWaveData S_Hardpoint4
	{
		get
		{
			return this.ExposedVariables.S_Hardpoint4;
		}
		set
		{
			this.ExposedVariables.S_Hardpoint4 = value;
		}
	}

	// Token: 0x170008C0 RID: 2240
	// (get) Token: 0x0600FE3A RID: 65082 RVA: 0x0048335C File Offset: 0x0048155C
	// (set) Token: 0x0600FE3B RID: 65083 RVA: 0x0048336C File Offset: 0x0048156C
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

	// Token: 0x170008C1 RID: 2241
	// (get) Token: 0x0600FE3C RID: 65084 RVA: 0x0048337C File Offset: 0x0048157C
	// (set) Token: 0x0600FE3D RID: 65085 RVA: 0x0048338C File Offset: 0x0048158C
	public UnitSpawnWaveData AA_Catamaran
	{
		get
		{
			return this.ExposedVariables.AA_Catamaran;
		}
		set
		{
			this.ExposedVariables.AA_Catamaran = value;
		}
	}

	// Token: 0x170008C2 RID: 2242
	// (get) Token: 0x0600FE3E RID: 65086 RVA: 0x0048339C File Offset: 0x0048159C
	// (set) Token: 0x0600FE3F RID: 65087 RVA: 0x004833AC File Offset: 0x004815AC
	public AttributeBuffSetData Buff_EMPShooters
	{
		get
		{
			return this.ExposedVariables.Buff_EMPShooters;
		}
		set
		{
			this.ExposedVariables.Buff_EMPShooters = value;
		}
	}

	// Token: 0x170008C3 RID: 2243
	// (get) Token: 0x0600FE40 RID: 65088 RVA: 0x004833BC File Offset: 0x004815BC
	// (set) Token: 0x0600FE41 RID: 65089 RVA: 0x004833CC File Offset: 0x004815CC
	public AttributeBuffSetData Buff_GaalsiTurrets
	{
		get
		{
			return this.ExposedVariables.Buff_GaalsiTurrets;
		}
		set
		{
			this.ExposedVariables.Buff_GaalsiTurrets = value;
		}
	}

	// Token: 0x170008C4 RID: 2244
	// (get) Token: 0x0600FE42 RID: 65090 RVA: 0x004833DC File Offset: 0x004815DC
	// (set) Token: 0x0600FE43 RID: 65091 RVA: 0x004833EC File Offset: 0x004815EC
	public UnitSpawnWaveData G_Turret
	{
		get
		{
			return this.ExposedVariables.G_Turret;
		}
		set
		{
			this.ExposedVariables.G_Turret = value;
		}
	}

	// Token: 0x170008C5 RID: 2245
	// (get) Token: 0x0600FE44 RID: 65092 RVA: 0x004833FC File Offset: 0x004815FC
	// (set) Token: 0x0600FE45 RID: 65093 RVA: 0x0048340C File Offset: 0x0048160C
	public UnitSpawnWaveData Wave_Tower
	{
		get
		{
			return this.ExposedVariables.Wave_Tower;
		}
		set
		{
			this.ExposedVariables.Wave_Tower = value;
		}
	}

	// Token: 0x170008C6 RID: 2246
	// (get) Token: 0x0600FE46 RID: 65094 RVA: 0x0048341C File Offset: 0x0048161C
	// (set) Token: 0x0600FE47 RID: 65095 RVA: 0x0048342C File Offset: 0x0048162C
	public QueryContainer QueryAllTurrets
	{
		get
		{
			return this.ExposedVariables.QueryAllTurrets;
		}
		set
		{
			this.ExposedVariables.QueryAllTurrets = value;
		}
	}

	// Token: 0x170008C7 RID: 2247
	// (get) Token: 0x0600FE48 RID: 65096 RVA: 0x0048343C File Offset: 0x0048163C
	// (set) Token: 0x0600FE49 RID: 65097 RVA: 0x0048344C File Offset: 0x0048164C
	public AttributeBuffSetData UnitHealthAndArmourBuff
	{
		get
		{
			return this.ExposedVariables.UnitHealthAndArmourBuff;
		}
		set
		{
			this.ExposedVariables.UnitHealthAndArmourBuff = value;
		}
	}

	// Token: 0x170008C8 RID: 2248
	// (get) Token: 0x0600FE4A RID: 65098 RVA: 0x0048345C File Offset: 0x0048165C
	// (set) Token: 0x0600FE4B RID: 65099 RVA: 0x0048346C File Offset: 0x0048166C
	public AttributeBuffSetData TurretGroupDebuff
	{
		get
		{
			return this.ExposedVariables.TurretGroupDebuff;
		}
		set
		{
			this.ExposedVariables.TurretGroupDebuff = value;
		}
	}

	// Token: 0x170008C9 RID: 2249
	// (get) Token: 0x0600FE4C RID: 65100 RVA: 0x0048347C File Offset: 0x0048167C
	// (set) Token: 0x0600FE4D RID: 65101 RVA: 0x0048348C File Offset: 0x0048168C
	public UnitSpawnWaveData S_Bombers
	{
		get
		{
			return this.ExposedVariables.S_Bombers;
		}
		set
		{
			this.ExposedVariables.S_Bombers = value;
		}
	}

	// Token: 0x170008CA RID: 2250
	// (get) Token: 0x0600FE4E RID: 65102 RVA: 0x0048349C File Offset: 0x0048169C
	// (set) Token: 0x0600FE4F RID: 65103 RVA: 0x004834AC File Offset: 0x004816AC
	public AttributeBuffSetData TurretRebuff
	{
		get
		{
			return this.ExposedVariables.TurretRebuff;
		}
		set
		{
			this.ExposedVariables.TurretRebuff = value;
		}
	}

	// Token: 0x0600FE50 RID: 65104 RVA: 0x004834BC File Offset: 0x004816BC
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

	// Token: 0x0600FE51 RID: 65105 RVA: 0x00483524 File Offset: 0x00481724
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600FE52 RID: 65106 RVA: 0x00483534 File Offset: 0x00481734
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600FE53 RID: 65107 RVA: 0x00483544 File Offset: 0x00481744
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600FE54 RID: 65108 RVA: 0x00483554 File Offset: 0x00481754
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600FE55 RID: 65109 RVA: 0x00483564 File Offset: 0x00481764
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04017E86 RID: 97926
	public M10_Defenders2 ExposedVariables = new M10_Defenders2();
}
