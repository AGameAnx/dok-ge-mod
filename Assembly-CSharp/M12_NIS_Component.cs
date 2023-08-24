using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using UnityEngine;

// Token: 0x0200089D RID: 2205
[AddComponentMenu("uScript/Graphs/M12_NIS")]
public class M12_NIS_Component : uScriptCode
{
	// Token: 0x060120F0 RID: 73968 RVA: 0x00521C48 File Offset: 0x0051FE48
	public M12_NIS_Component()
	{
	}

	// Token: 0x17000988 RID: 2440
	// (get) Token: 0x060120F1 RID: 73969 RVA: 0x00521C5C File Offset: 0x0051FE5C
	// (set) Token: 0x060120F2 RID: 73970 RVA: 0x00521C6C File Offset: 0x0051FE6C
	public UnitSpawnWaveData NIS01_Siidim
	{
		get
		{
			return this.ExposedVariables.NIS01_Siidim;
		}
		set
		{
			this.ExposedVariables.NIS01_Siidim = value;
		}
	}

	// Token: 0x17000989 RID: 2441
	// (get) Token: 0x060120F3 RID: 73971 RVA: 0x00521C7C File Offset: 0x0051FE7C
	// (set) Token: 0x060120F4 RID: 73972 RVA: 0x00521C8C File Offset: 0x0051FE8C
	public QueryContainer MissionCompleteNISGodQ
	{
		get
		{
			return this.ExposedVariables.MissionCompleteNISGodQ;
		}
		set
		{
			this.ExposedVariables.MissionCompleteNISGodQ = value;
		}
	}

	// Token: 0x1700098A RID: 2442
	// (get) Token: 0x060120F5 RID: 73973 RVA: 0x00521C9C File Offset: 0x0051FE9C
	// (set) Token: 0x060120F6 RID: 73974 RVA: 0x00521CAC File Offset: 0x0051FEAC
	public QueryContainer MissionCompleteNISSuspendQ
	{
		get
		{
			return this.ExposedVariables.MissionCompleteNISSuspendQ;
		}
		set
		{
			this.ExposedVariables.MissionCompleteNISSuspendQ = value;
		}
	}

	// Token: 0x1700098B RID: 2443
	// (get) Token: 0x060120F7 RID: 73975 RVA: 0x00521CBC File Offset: 0x0051FEBC
	// (set) Token: 0x060120F8 RID: 73976 RVA: 0x00521CCC File Offset: 0x0051FECC
	public QueryContainer NIS3_GodQ
	{
		get
		{
			return this.ExposedVariables.NIS3_GodQ;
		}
		set
		{
			this.ExposedVariables.NIS3_GodQ = value;
		}
	}

	// Token: 0x1700098C RID: 2444
	// (get) Token: 0x060120F9 RID: 73977 RVA: 0x00521CDC File Offset: 0x0051FEDC
	// (set) Token: 0x060120FA RID: 73978 RVA: 0x00521CEC File Offset: 0x0051FEEC
	public QueryContainer NIS2_GodQ
	{
		get
		{
			return this.ExposedVariables.NIS2_GodQ;
		}
		set
		{
			this.ExposedVariables.NIS2_GodQ = value;
		}
	}

	// Token: 0x1700098D RID: 2445
	// (get) Token: 0x060120FB RID: 73979 RVA: 0x00521CFC File Offset: 0x0051FEFC
	// (set) Token: 0x060120FC RID: 73980 RVA: 0x00521D0C File Offset: 0x0051FF0C
	public QueryContainer NIS2_SuspendQ
	{
		get
		{
			return this.ExposedVariables.NIS2_SuspendQ;
		}
		set
		{
			this.ExposedVariables.NIS2_SuspendQ = value;
		}
	}

	// Token: 0x1700098E RID: 2446
	// (get) Token: 0x060120FD RID: 73981 RVA: 0x00521D1C File Offset: 0x0051FF1C
	// (set) Token: 0x060120FE RID: 73982 RVA: 0x00521D2C File Offset: 0x0051FF2C
	public QueryContainer NIS1_SuspendQ
	{
		get
		{
			return this.ExposedVariables.NIS1_SuspendQ;
		}
		set
		{
			this.ExposedVariables.NIS1_SuspendQ = value;
		}
	}

	// Token: 0x1700098F RID: 2447
	// (get) Token: 0x060120FF RID: 73983 RVA: 0x00521D3C File Offset: 0x0051FF3C
	// (set) Token: 0x06012100 RID: 73984 RVA: 0x00521D4C File Offset: 0x0051FF4C
	public QueryContainer NIS1_GodQ
	{
		get
		{
			return this.ExposedVariables.NIS1_GodQ;
		}
		set
		{
			this.ExposedVariables.NIS1_GodQ = value;
		}
	}

	// Token: 0x17000990 RID: 2448
	// (get) Token: 0x06012101 RID: 73985 RVA: 0x00521D5C File Offset: 0x0051FF5C
	// (set) Token: 0x06012102 RID: 73986 RVA: 0x00521D6C File Offset: 0x0051FF6C
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

	// Token: 0x17000991 RID: 2449
	// (get) Token: 0x06012103 RID: 73987 RVA: 0x00521D7C File Offset: 0x0051FF7C
	// (set) Token: 0x06012104 RID: 73988 RVA: 0x00521D8C File Offset: 0x0051FF8C
	public QueryContainer Q_Entourage
	{
		get
		{
			return this.ExposedVariables.Q_Entourage;
		}
		set
		{
			this.ExposedVariables.Q_Entourage = value;
		}
	}

	// Token: 0x17000992 RID: 2450
	// (get) Token: 0x06012105 RID: 73989 RVA: 0x00521D9C File Offset: 0x0051FF9C
	// (set) Token: 0x06012106 RID: 73990 RVA: 0x00521DAC File Offset: 0x0051FFAC
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

	// Token: 0x17000993 RID: 2451
	// (get) Token: 0x06012107 RID: 73991 RVA: 0x00521DBC File Offset: 0x0051FFBC
	// (set) Token: 0x06012108 RID: 73992 RVA: 0x00521DCC File Offset: 0x0051FFCC
	public QueryContainer Q_HiddenDuringNIS03_Allies
	{
		get
		{
			return this.ExposedVariables.Q_HiddenDuringNIS03_Allies;
		}
		set
		{
			this.ExposedVariables.Q_HiddenDuringNIS03_Allies = value;
		}
	}

	// Token: 0x17000994 RID: 2452
	// (get) Token: 0x06012109 RID: 73993 RVA: 0x00521DDC File Offset: 0x0051FFDC
	// (set) Token: 0x0601210A RID: 73994 RVA: 0x00521DEC File Offset: 0x0051FFEC
	public QueryContainer Q_HiddenDuringNIS03_Enemies
	{
		get
		{
			return this.ExposedVariables.Q_HiddenDuringNIS03_Enemies;
		}
		set
		{
			this.ExposedVariables.Q_HiddenDuringNIS03_Enemies = value;
		}
	}

	// Token: 0x17000995 RID: 2453
	// (get) Token: 0x0601210B RID: 73995 RVA: 0x00521DFC File Offset: 0x0051FFFC
	// (set) Token: 0x0601210C RID: 73996 RVA: 0x00521E0C File Offset: 0x0052000C
	public QueryContainer OpeningNIS_SuspendQ
	{
		get
		{
			return this.ExposedVariables.OpeningNIS_SuspendQ;
		}
		set
		{
			this.ExposedVariables.OpeningNIS_SuspendQ = value;
		}
	}

	// Token: 0x17000996 RID: 2454
	// (get) Token: 0x0601210D RID: 73997 RVA: 0x00521E1C File Offset: 0x0052001C
	// (set) Token: 0x0601210E RID: 73998 RVA: 0x00521E2C File Offset: 0x0052002C
	public QueryContainer OpeningNIS_GodQ
	{
		get
		{
			return this.ExposedVariables.OpeningNIS_GodQ;
		}
		set
		{
			this.ExposedVariables.OpeningNIS_GodQ = value;
		}
	}

	// Token: 0x17000997 RID: 2455
	// (get) Token: 0x0601210F RID: 73999 RVA: 0x00521E3C File Offset: 0x0052003C
	// (set) Token: 0x06012110 RID: 74000 RVA: 0x00521E4C File Offset: 0x0052004C
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

	// Token: 0x17000998 RID: 2456
	// (get) Token: 0x06012111 RID: 74001 RVA: 0x00521E5C File Offset: 0x0052005C
	// (set) Token: 0x06012112 RID: 74002 RVA: 0x00521E6C File Offset: 0x0052006C
	public QueryContainer AllCommanderQuery
	{
		get
		{
			return this.ExposedVariables.AllCommanderQuery;
		}
		set
		{
			this.ExposedVariables.AllCommanderQuery = value;
		}
	}

	// Token: 0x17000999 RID: 2457
	// (get) Token: 0x06012113 RID: 74003 RVA: 0x00521E7C File Offset: 0x0052007C
	// (set) Token: 0x06012114 RID: 74004 RVA: 0x00521E8C File Offset: 0x0052008C
	public int ModifiedResource1Amount
	{
		get
		{
			return this.ExposedVariables.ModifiedResource1Amount;
		}
		set
		{
			this.ExposedVariables.ModifiedResource1Amount = value;
		}
	}

	// Token: 0x1700099A RID: 2458
	// (get) Token: 0x06012115 RID: 74005 RVA: 0x00521E9C File Offset: 0x0052009C
	// (set) Token: 0x06012116 RID: 74006 RVA: 0x00521EAC File Offset: 0x005200AC
	public int ModifiedResource2Amount
	{
		get
		{
			return this.ExposedVariables.ModifiedResource2Amount;
		}
		set
		{
			this.ExposedVariables.ModifiedResource2Amount = value;
		}
	}

	// Token: 0x06012117 RID: 74007 RVA: 0x00521EBC File Offset: 0x005200BC
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

	// Token: 0x06012118 RID: 74008 RVA: 0x00521F24 File Offset: 0x00520124
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x06012119 RID: 74009 RVA: 0x00521F34 File Offset: 0x00520134
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0601211A RID: 74010 RVA: 0x00521F44 File Offset: 0x00520144
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0601211B RID: 74011 RVA: 0x00521F54 File Offset: 0x00520154
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0601211C RID: 74012 RVA: 0x00521F64 File Offset: 0x00520164
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401B32A RID: 111402
	public M12_NIS ExposedVariables = new M12_NIS();
}
