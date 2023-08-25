using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Scripting;
using UnityEngine;

// Token: 0x02000829 RID: 2089
[AddComponentMenu("uScript/Graphs/M04_Gameplay2")]
public class M04_Gameplay2_Component : uScriptCode
{
	// Token: 0x06007D82 RID: 32130 RVA: 0x0023BA58 File Offset: 0x00239C58
	public M04_Gameplay2_Component()
	{
	}

	// Token: 0x1700067B RID: 1659
	// (get) Token: 0x06007D83 RID: 32131 RVA: 0x0023BA6C File Offset: 0x00239C6C
	// (set) Token: 0x06007D84 RID: 32132 RVA: 0x0023BA7C File Offset: 0x00239C7C
	public UnitSpawnWaveData GaalsiAttackerWave
	{
		get
		{
			return this.ExposedVariables.GaalsiAttackerWave;
		}
		set
		{
			this.ExposedVariables.GaalsiAttackerWave = value;
		}
	}

	// Token: 0x1700067C RID: 1660
	// (get) Token: 0x06007D85 RID: 32133 RVA: 0x0023BA8C File Offset: 0x00239C8C
	// (set) Token: 0x06007D86 RID: 32134 RVA: 0x0023BA9C File Offset: 0x00239C9C
	public UnitSpawnWaveData GaalsiPatrolWave_02
	{
		get
		{
			return this.ExposedVariables.GaalsiPatrolWave_02;
		}
		set
		{
			this.ExposedVariables.GaalsiPatrolWave_02 = value;
		}
	}

	// Token: 0x1700067D RID: 1661
	// (get) Token: 0x06007D87 RID: 32135 RVA: 0x0023BAAC File Offset: 0x00239CAC
	// (set) Token: 0x06007D88 RID: 32136 RVA: 0x0023BABC File Offset: 0x00239CBC
	public UnitSpawnWaveData GaalsiPatrolWave_03
	{
		get
		{
			return this.ExposedVariables.GaalsiPatrolWave_03;
		}
		set
		{
			this.ExposedVariables.GaalsiPatrolWave_03 = value;
		}
	}

	// Token: 0x1700067E RID: 1662
	// (get) Token: 0x06007D89 RID: 32137 RVA: 0x0023BACC File Offset: 0x00239CCC
	// (set) Token: 0x06007D8A RID: 32138 RVA: 0x0023BADC File Offset: 0x00239CDC
	public UnitSpawnWaveData GaalsiPatrolWave_04
	{
		get
		{
			return this.ExposedVariables.GaalsiPatrolWave_04;
		}
		set
		{
			this.ExposedVariables.GaalsiPatrolWave_04 = value;
		}
	}

	// Token: 0x1700067F RID: 1663
	// (get) Token: 0x06007D8B RID: 32139 RVA: 0x0023BAEC File Offset: 0x00239CEC
	// (set) Token: 0x06007D8C RID: 32140 RVA: 0x0023BAFC File Offset: 0x00239CFC
	public UnitSpawnWaveData GaalsiPatrolWave_01
	{
		get
		{
			return this.ExposedVariables.GaalsiPatrolWave_01;
		}
		set
		{
			this.ExposedVariables.GaalsiPatrolWave_01 = value;
		}
	}

	// Token: 0x17000680 RID: 1664
	// (get) Token: 0x06007D8D RID: 32141 RVA: 0x0023BB0C File Offset: 0x00239D0C
	// (set) Token: 0x06007D8E RID: 32142 RVA: 0x0023BB1C File Offset: 0x00239D1C
	public bool calloutActive
	{
		get
		{
			return this.ExposedVariables.calloutActive;
		}
		set
		{
			this.ExposedVariables.calloutActive = value;
		}
	}

	// Token: 0x17000681 RID: 1665
	// (get) Token: 0x06007D8F RID: 32143 RVA: 0x0023BB2C File Offset: 0x00239D2C
	// (set) Token: 0x06007D90 RID: 32144 RVA: 0x0023BB3C File Offset: 0x00239D3C
	public SpawnFactoryData GaalsiScanStrike
	{
		get
		{
			return this.ExposedVariables.GaalsiScanStrike;
		}
		set
		{
			this.ExposedVariables.GaalsiScanStrike = value;
		}
	}

	// Token: 0x17000682 RID: 1666
	// (get) Token: 0x06007D91 RID: 32145 RVA: 0x0023BB4C File Offset: 0x00239D4C
	// (set) Token: 0x06007D92 RID: 32146 RVA: 0x0023BB5C File Offset: 0x00239D5C
	public AttributeBuffSetData StrikeDebuff
	{
		get
		{
			return this.ExposedVariables.StrikeDebuff;
		}
		set
		{
			this.ExposedVariables.StrikeDebuff = value;
		}
	}

	// Token: 0x17000683 RID: 1667
	// (get) Token: 0x06007D93 RID: 32147 RVA: 0x0023BB6C File Offset: 0x00239D6C
	// (set) Token: 0x06007D94 RID: 32148 RVA: 0x0023BB7C File Offset: 0x00239D7C
	public int NumActiveScanners
	{
		get
		{
			return this.ExposedVariables.NumActiveScanners;
		}
		set
		{
			this.ExposedVariables.NumActiveScanners = value;
		}
	}

	// Token: 0x17000684 RID: 1668
	// (get) Token: 0x06007D95 RID: 32149 RVA: 0x0023BB8C File Offset: 0x00239D8C
	// (set) Token: 0x06007D96 RID: 32150 RVA: 0x0023BB9C File Offset: 0x00239D9C
	public QueryContainer GaalsiNIS03Units
	{
		get
		{
			return this.ExposedVariables.GaalsiNIS03Units;
		}
		set
		{
			this.ExposedVariables.GaalsiNIS03Units = value;
		}
	}

	// Token: 0x17000685 RID: 1669
	// (get) Token: 0x06007D97 RID: 32151 RVA: 0x0023BBAC File Offset: 0x00239DAC
	// (set) Token: 0x06007D98 RID: 32152 RVA: 0x0023BBBC File Offset: 0x00239DBC
	public QueryContainer PlayerUnits
	{
		get
		{
			return this.ExposedVariables.PlayerUnits;
		}
		set
		{
			this.ExposedVariables.PlayerUnits = value;
		}
	}

	// Token: 0x17000686 RID: 1670
	// (get) Token: 0x06007D99 RID: 32153 RVA: 0x0023BBCC File Offset: 0x00239DCC
	// (set) Token: 0x06007D9A RID: 32154 RVA: 0x0023BBDC File Offset: 0x00239DDC
	public QueryContainer NIS04PlayerUnits
	{
		get
		{
			return this.ExposedVariables.NIS04PlayerUnits;
		}
		set
		{
			this.ExposedVariables.NIS04PlayerUnits = value;
		}
	}

	// Token: 0x17000687 RID: 1671
	// (get) Token: 0x06007D9B RID: 32155 RVA: 0x0023BBEC File Offset: 0x00239DEC
	// (set) Token: 0x06007D9C RID: 32156 RVA: 0x0023BBFC File Offset: 0x00239DFC
	public QueryContainer NIS04Suspend
	{
		get
		{
			return this.ExposedVariables.NIS04Suspend;
		}
		set
		{
			this.ExposedVariables.NIS04Suspend = value;
		}
	}

	// Token: 0x17000688 RID: 1672
	// (get) Token: 0x06007D9D RID: 32157 RVA: 0x0023BC0C File Offset: 0x00239E0C
	// (set) Token: 0x06007D9E RID: 32158 RVA: 0x0023BC1C File Offset: 0x00239E1C
	public QueryContainer ProdCruiserSearch
	{
		get
		{
			return this.ExposedVariables.ProdCruiserSearch;
		}
		set
		{
			this.ExposedVariables.ProdCruiserSearch = value;
		}
	}

	// Token: 0x17000689 RID: 1673
	// (get) Token: 0x06007D9F RID: 32159 RVA: 0x0023BC2C File Offset: 0x00239E2C
	// (set) Token: 0x06007DA0 RID: 32160 RVA: 0x0023BC3C File Offset: 0x00239E3C
	public UnitSpawnWaveData ProdCruiser
	{
		get
		{
			return this.ExposedVariables.ProdCruiser;
		}
		set
		{
			this.ExposedVariables.ProdCruiser = value;
		}
	}

	// Token: 0x1700068A RID: 1674
	// (get) Token: 0x06007DA1 RID: 32161 RVA: 0x0023BC4C File Offset: 0x00239E4C
	// (set) Token: 0x06007DA2 RID: 32162 RVA: 0x0023BC5C File Offset: 0x00239E5C
	public QueryContainer nis04_suspendall
	{
		get
		{
			return this.ExposedVariables.nis04_suspendall;
		}
		set
		{
			this.ExposedVariables.nis04_suspendall = value;
		}
	}

	// Token: 0x1700068B RID: 1675
	// (get) Token: 0x06007DA3 RID: 32163 RVA: 0x0023BC6C File Offset: 0x00239E6C
	// (set) Token: 0x06007DA4 RID: 32164 RVA: 0x0023BC7C File Offset: 0x00239E7C
	public QueryContainer FindTurrets
	{
		get
		{
			return this.ExposedVariables.FindTurrets;
		}
		set
		{
			this.ExposedVariables.FindTurrets = value;
		}
	}

	// Token: 0x1700068C RID: 1676
	// (get) Token: 0x06007DA5 RID: 32165 RVA: 0x0023BC8C File Offset: 0x00239E8C
	// (set) Token: 0x06007DA6 RID: 32166 RVA: 0x0023BC9C File Offset: 0x00239E9C
	public QueryContainer ScannerSearch
	{
		get
		{
			return this.ExposedVariables.ScannerSearch;
		}
		set
		{
			this.ExposedVariables.ScannerSearch = value;
		}
	}

	// Token: 0x1700068D RID: 1677
	// (get) Token: 0x06007DA7 RID: 32167 RVA: 0x0023BCAC File Offset: 0x00239EAC
	// (set) Token: 0x06007DA8 RID: 32168 RVA: 0x0023BCBC File Offset: 0x00239EBC
	public QueryContainer GaalsienQuery
	{
		get
		{
			return this.ExposedVariables.GaalsienQuery;
		}
		set
		{
			this.ExposedVariables.GaalsienQuery = value;
		}
	}

	// Token: 0x06007DA9 RID: 32169 RVA: 0x0023BCCC File Offset: 0x00239ECC
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

	// Token: 0x06007DAA RID: 32170 RVA: 0x0023BD34 File Offset: 0x00239F34
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06007DAB RID: 32171 RVA: 0x0023BD44 File Offset: 0x00239F44
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06007DAC RID: 32172 RVA: 0x0023BD54 File Offset: 0x00239F54
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06007DAD RID: 32173 RVA: 0x0023BD64 File Offset: 0x00239F64
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06007DAE RID: 32174 RVA: 0x0023BD74 File Offset: 0x00239F74
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x06007DAF RID: 32175 RVA: 0x0023BD84 File Offset: 0x00239F84
	private void OnGUI()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnGUI();
	}

	// Token: 0x0400B4FA RID: 46330
	public M04_Gameplay2 ExposedVariables = new M04_Gameplay2();
}
