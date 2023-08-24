using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x0200080A RID: 2058
[AddComponentMenu("uScript/Graphs/M01_Gameplay")]
public class M01_Gameplay_Component : uScriptCode
{
	// Token: 0x06003A7C RID: 14972 RVA: 0x00101008 File Offset: 0x000FF208
	public M01_Gameplay_Component()
	{
	}

	// Token: 0x170005AD RID: 1453
	// (get) Token: 0x06003A7D RID: 14973 RVA: 0x0010101C File Offset: 0x000FF21C
	// (set) Token: 0x06003A7E RID: 14974 RVA: 0x0010102C File Offset: 0x000FF22C
	public QueryContainer QueryPlayerUnits
	{
		get
		{
			return this.ExposedVariables.QueryPlayerUnits;
		}
		set
		{
			this.ExposedVariables.QueryPlayerUnits = value;
		}
	}

	// Token: 0x170005AE RID: 1454
	// (get) Token: 0x06003A7F RID: 14975 RVA: 0x0010103C File Offset: 0x000FF23C
	// (set) Token: 0x06003A80 RID: 14976 RVA: 0x0010104C File Offset: 0x000FF24C
	public AttributeBuffSetData Buff_Salvager
	{
		get
		{
			return this.ExposedVariables.Buff_Salvager;
		}
		set
		{
			this.ExposedVariables.Buff_Salvager = value;
		}
	}

	// Token: 0x170005AF RID: 1455
	// (get) Token: 0x06003A81 RID: 14977 RVA: 0x0010105C File Offset: 0x000FF25C
	// (set) Token: 0x06003A82 RID: 14978 RVA: 0x0010106C File Offset: 0x000FF26C
	public int dronesDestroyed
	{
		get
		{
			return this.ExposedVariables.dronesDestroyed;
		}
		set
		{
			this.ExposedVariables.dronesDestroyed = value;
		}
	}

	// Token: 0x170005B0 RID: 1456
	// (get) Token: 0x06003A83 RID: 14979 RVA: 0x0010107C File Offset: 0x000FF27C
	// (set) Token: 0x06003A84 RID: 14980 RVA: 0x0010108C File Offset: 0x000FF28C
	public int escortsProduced
	{
		get
		{
			return this.ExposedVariables.escortsProduced;
		}
		set
		{
			this.ExposedVariables.escortsProduced = value;
		}
	}

	// Token: 0x170005B1 RID: 1457
	// (get) Token: 0x06003A85 RID: 14981 RVA: 0x0010109C File Offset: 0x000FF29C
	// (set) Token: 0x06003A86 RID: 14982 RVA: 0x001010AC File Offset: 0x000FF2AC
	public QueryContainer QueryDrones
	{
		get
		{
			return this.ExposedVariables.QueryDrones;
		}
		set
		{
			this.ExposedVariables.QueryDrones = value;
		}
	}

	// Token: 0x170005B2 RID: 1458
	// (get) Token: 0x06003A87 RID: 14983 RVA: 0x001010BC File Offset: 0x000FF2BC
	// (set) Token: 0x06003A88 RID: 14984 RVA: 0x001010CC File Offset: 0x000FF2CC
	public QueryContainer QueryBuiltEscorts
	{
		get
		{
			return this.ExposedVariables.QueryBuiltEscorts;
		}
		set
		{
			this.ExposedVariables.QueryBuiltEscorts = value;
		}
	}

	// Token: 0x170005B3 RID: 1459
	// (get) Token: 0x06003A89 RID: 14985 RVA: 0x001010DC File Offset: 0x000FF2DC
	// (set) Token: 0x06003A8A RID: 14986 RVA: 0x001010EC File Offset: 0x000FF2EC
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

	// Token: 0x170005B4 RID: 1460
	// (get) Token: 0x06003A8B RID: 14987 RVA: 0x001010FC File Offset: 0x000FF2FC
	// (set) Token: 0x06003A8C RID: 14988 RVA: 0x0010110C File Offset: 0x000FF30C
	public AttributeBuffSetData Buff_Drones
	{
		get
		{
			return this.ExposedVariables.Buff_Drones;
		}
		set
		{
			this.ExposedVariables.Buff_Drones = value;
		}
	}

	// Token: 0x170005B5 RID: 1461
	// (get) Token: 0x06003A8D RID: 14989 RVA: 0x0010111C File Offset: 0x000FF31C
	// (set) Token: 0x06003A8E RID: 14990 RVA: 0x0010112C File Offset: 0x000FF32C
	public UnitSpawnWaveData Wave_Drones
	{
		get
		{
			return this.ExposedVariables.Wave_Drones;
		}
		set
		{
			this.ExposedVariables.Wave_Drones = value;
		}
	}

	// Token: 0x170005B6 RID: 1462
	// (get) Token: 0x06003A8F RID: 14991 RVA: 0x0010113C File Offset: 0x000FF33C
	// (set) Token: 0x06003A90 RID: 14992 RVA: 0x0010114C File Offset: 0x000FF34C
	public AttributeBuffSetData Buff_EnableRepair
	{
		get
		{
			return this.ExposedVariables.Buff_EnableRepair;
		}
		set
		{
			this.ExposedVariables.Buff_EnableRepair = value;
		}
	}

	// Token: 0x170005B7 RID: 1463
	// (get) Token: 0x06003A91 RID: 14993 RVA: 0x0010115C File Offset: 0x000FF35C
	// (set) Token: 0x06003A92 RID: 14994 RVA: 0x0010116C File Offset: 0x000FF36C
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

	// Token: 0x170005B8 RID: 1464
	// (get) Token: 0x06003A93 RID: 14995 RVA: 0x0010117C File Offset: 0x000FF37C
	// (set) Token: 0x06003A94 RID: 14996 RVA: 0x0010118C File Offset: 0x000FF38C
	public AttributeBuffSetData Buff_Salvager_extract_rate
	{
		get
		{
			return this.ExposedVariables.Buff_Salvager_extract_rate;
		}
		set
		{
			this.ExposedVariables.Buff_Salvager_extract_rate = value;
		}
	}

	// Token: 0x06003A95 RID: 14997 RVA: 0x0010119C File Offset: 0x000FF39C
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

	// Token: 0x06003A96 RID: 14998 RVA: 0x00101204 File Offset: 0x000FF404
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x06003A97 RID: 14999 RVA: 0x00101214 File Offset: 0x000FF414
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06003A98 RID: 15000 RVA: 0x00101224 File Offset: 0x000FF424
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06003A99 RID: 15001 RVA: 0x00101234 File Offset: 0x000FF434
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x06003A9A RID: 15002 RVA: 0x00101244 File Offset: 0x000FF444
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400499A RID: 18842
	public M01_Gameplay ExposedVariables = new M01_Gameplay();
}
