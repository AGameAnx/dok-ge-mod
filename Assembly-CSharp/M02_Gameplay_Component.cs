using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x02000816 RID: 2070
[AddComponentMenu("uScript/Graphs/M02_Gameplay")]
public class M02_Gameplay_Component : uScriptCode
{
	// Token: 0x06004DF6 RID: 19958 RVA: 0x00157FA0 File Offset: 0x001561A0
	public M02_Gameplay_Component()
	{
	}

	// Token: 0x170005F6 RID: 1526
	// (get) Token: 0x06004DF7 RID: 19959 RVA: 0x00157FB4 File Offset: 0x001561B4
	// (set) Token: 0x06004DF8 RID: 19960 RVA: 0x00157FC4 File Offset: 0x001561C4
	public UnitSpawnWaveData KapisiSpawnWave
	{
		get
		{
			return this.ExposedVariables.KapisiSpawnWave;
		}
		set
		{
			this.ExposedVariables.KapisiSpawnWave = value;
		}
	}

	// Token: 0x170005F7 RID: 1527
	// (get) Token: 0x06004DF9 RID: 19961 RVA: 0x00157FD4 File Offset: 0x001561D4
	// (set) Token: 0x06004DFA RID: 19962 RVA: 0x00157FE4 File Offset: 0x001561E4
	public QueryContainer CUGroup1Query
	{
		get
		{
			return this.ExposedVariables.CUGroup1Query;
		}
		set
		{
			this.ExposedVariables.CUGroup1Query = value;
		}
	}

	// Token: 0x170005F8 RID: 1528
	// (get) Token: 0x06004DFB RID: 19963 RVA: 0x00157FF4 File Offset: 0x001561F4
	// (set) Token: 0x06004DFC RID: 19964 RVA: 0x00158004 File Offset: 0x00156204
	public QueryContainer MothballUnitQuery1
	{
		get
		{
			return this.ExposedVariables.MothballUnitQuery1;
		}
		set
		{
			this.ExposedVariables.MothballUnitQuery1 = value;
		}
	}

	// Token: 0x170005F9 RID: 1529
	// (get) Token: 0x06004DFD RID: 19965 RVA: 0x00158014 File Offset: 0x00156214
	// (set) Token: 0x06004DFE RID: 19966 RVA: 0x00158024 File Offset: 0x00156224
	public AttributeBuffSetData DisableAttacksBuff
	{
		get
		{
			return this.ExposedVariables.DisableAttacksBuff;
		}
		set
		{
			this.ExposedVariables.DisableAttacksBuff = value;
		}
	}

	// Token: 0x170005FA RID: 1530
	// (get) Token: 0x06004DFF RID: 19967 RVA: 0x00158034 File Offset: 0x00156234
	// (set) Token: 0x06004E00 RID: 19968 RVA: 0x00158044 File Offset: 0x00156244
	public int NumMothballsReactivated
	{
		get
		{
			return this.ExposedVariables.NumMothballsReactivated;
		}
		set
		{
			this.ExposedVariables.NumMothballsReactivated = value;
		}
	}

	// Token: 0x170005FB RID: 1531
	// (get) Token: 0x06004E01 RID: 19969 RVA: 0x00158054 File Offset: 0x00156254
	// (set) Token: 0x06004E02 RID: 19970 RVA: 0x00158064 File Offset: 0x00156264
	public bool CUGroup2Harvested
	{
		get
		{
			return this.ExposedVariables.CUGroup2Harvested;
		}
		set
		{
			this.ExposedVariables.CUGroup2Harvested = value;
		}
	}

	// Token: 0x170005FC RID: 1532
	// (get) Token: 0x06004E03 RID: 19971 RVA: 0x00158074 File Offset: 0x00156274
	// (set) Token: 0x06004E04 RID: 19972 RVA: 0x00158084 File Offset: 0x00156284
	public QueryContainer CUGroup2Query
	{
		get
		{
			return this.ExposedVariables.CUGroup2Query;
		}
		set
		{
			this.ExposedVariables.CUGroup2Query = value;
		}
	}

	// Token: 0x170005FD RID: 1533
	// (get) Token: 0x06004E05 RID: 19973 RVA: 0x00158094 File Offset: 0x00156294
	// (set) Token: 0x06004E06 RID: 19974 RVA: 0x001580A4 File Offset: 0x001562A4
	public bool CUGroup3Harvested
	{
		get
		{
			return this.ExposedVariables.CUGroup3Harvested;
		}
		set
		{
			this.ExposedVariables.CUGroup3Harvested = value;
		}
	}

	// Token: 0x170005FE RID: 1534
	// (get) Token: 0x06004E07 RID: 19975 RVA: 0x001580B4 File Offset: 0x001562B4
	// (set) Token: 0x06004E08 RID: 19976 RVA: 0x001580C4 File Offset: 0x001562C4
	public QueryContainer CUGroup3Query
	{
		get
		{
			return this.ExposedVariables.CUGroup3Query;
		}
		set
		{
			this.ExposedVariables.CUGroup3Query = value;
		}
	}

	// Token: 0x170005FF RID: 1535
	// (get) Token: 0x06004E09 RID: 19977 RVA: 0x001580D4 File Offset: 0x001562D4
	// (set) Token: 0x06004E0A RID: 19978 RVA: 0x001580E4 File Offset: 0x001562E4
	public UnitSpawnWaveData ScannerBR
	{
		get
		{
			return this.ExposedVariables.ScannerBR;
		}
		set
		{
			this.ExposedVariables.ScannerBR = value;
		}
	}

	// Token: 0x17000600 RID: 1536
	// (get) Token: 0x06004E0B RID: 19979 RVA: 0x001580F4 File Offset: 0x001562F4
	// (set) Token: 0x06004E0C RID: 19980 RVA: 0x00158104 File Offset: 0x00156304
	public UnitSpawnWaveData TurretBR
	{
		get
		{
			return this.ExposedVariables.TurretBR;
		}
		set
		{
			this.ExposedVariables.TurretBR = value;
		}
	}

	// Token: 0x17000601 RID: 1537
	// (get) Token: 0x06004E0D RID: 19981 RVA: 0x00158114 File Offset: 0x00156314
	// (set) Token: 0x06004E0E RID: 19982 RVA: 0x00158124 File Offset: 0x00156324
	public QueryContainer BoneyardScannerTurretQuery
	{
		get
		{
			return this.ExposedVariables.BoneyardScannerTurretQuery;
		}
		set
		{
			this.ExposedVariables.BoneyardScannerTurretQuery = value;
		}
	}

	// Token: 0x17000602 RID: 1538
	// (get) Token: 0x06004E0F RID: 19983 RVA: 0x00158134 File Offset: 0x00156334
	// (set) Token: 0x06004E10 RID: 19984 RVA: 0x00158144 File Offset: 0x00156344
	public AttributeBuffSetData BoneyardScannerTurretBuff
	{
		get
		{
			return this.ExposedVariables.BoneyardScannerTurretBuff;
		}
		set
		{
			this.ExposedVariables.BoneyardScannerTurretBuff = value;
		}
	}

	// Token: 0x17000603 RID: 1539
	// (get) Token: 0x06004E11 RID: 19985 RVA: 0x00158154 File Offset: 0x00156354
	// (set) Token: 0x06004E12 RID: 19986 RVA: 0x00158164 File Offset: 0x00156364
	public QueryContainer AllHarvesterQuery
	{
		get
		{
			return this.ExposedVariables.AllHarvesterQuery;
		}
		set
		{
			this.ExposedVariables.AllHarvesterQuery = value;
		}
	}

	// Token: 0x17000604 RID: 1540
	// (get) Token: 0x06004E13 RID: 19987 RVA: 0x00158174 File Offset: 0x00156374
	// (set) Token: 0x06004E14 RID: 19988 RVA: 0x00158184 File Offset: 0x00156384
	public List<Entity> CommandCarrier
	{
		get
		{
			return this.ExposedVariables.CommandCarrier;
		}
		set
		{
			this.ExposedVariables.CommandCarrier = value;
		}
	}

	// Token: 0x17000605 RID: 1541
	// (get) Token: 0x06004E15 RID: 19989 RVA: 0x00158194 File Offset: 0x00156394
	// (set) Token: 0x06004E16 RID: 19990 RVA: 0x001581A4 File Offset: 0x001563A4
	public BuffSetAttributesAsset GaalsiAggroBuff
	{
		get
		{
			return this.ExposedVariables.GaalsiAggroBuff;
		}
		set
		{
			this.ExposedVariables.GaalsiAggroBuff = value;
		}
	}

	// Token: 0x17000606 RID: 1542
	// (get) Token: 0x06004E17 RID: 19991 RVA: 0x001581B4 File Offset: 0x001563B4
	// (set) Token: 0x06004E18 RID: 19992 RVA: 0x001581C4 File Offset: 0x001563C4
	public bool bEscortBuilt
	{
		get
		{
			return this.ExposedVariables.bEscortBuilt;
		}
		set
		{
			this.ExposedVariables.bEscortBuilt = value;
		}
	}

	// Token: 0x17000607 RID: 1543
	// (get) Token: 0x06004E19 RID: 19993 RVA: 0x001581D4 File Offset: 0x001563D4
	// (set) Token: 0x06004E1A RID: 19994 RVA: 0x001581E4 File Offset: 0x001563E4
	public BuffSetAttributesAsset CoalitionSensorBuff
	{
		get
		{
			return this.ExposedVariables.CoalitionSensorBuff;
		}
		set
		{
			this.ExposedVariables.CoalitionSensorBuff = value;
		}
	}

	// Token: 0x17000608 RID: 1544
	// (get) Token: 0x06004E1B RID: 19995 RVA: 0x001581F4 File Offset: 0x001563F4
	// (set) Token: 0x06004E1C RID: 19996 RVA: 0x00158204 File Offset: 0x00156404
	public UnitSpawnWaveData HACWave
	{
		get
		{
			return this.ExposedVariables.HACWave;
		}
		set
		{
			this.ExposedVariables.HACWave = value;
		}
	}

	// Token: 0x17000609 RID: 1545
	// (get) Token: 0x06004E1D RID: 19997 RVA: 0x00158214 File Offset: 0x00156414
	// (set) Token: 0x06004E1E RID: 19998 RVA: 0x00158224 File Offset: 0x00156424
	public QueryContainer MothballProxyQuery
	{
		get
		{
			return this.ExposedVariables.MothballProxyQuery;
		}
		set
		{
			this.ExposedVariables.MothballProxyQuery = value;
		}
	}

	// Token: 0x1700060A RID: 1546
	// (get) Token: 0x06004E1F RID: 19999 RVA: 0x00158234 File Offset: 0x00156434
	// (set) Token: 0x06004E20 RID: 20000 RVA: 0x00158244 File Offset: 0x00156444
	public AttributeBuffSetData Buff_IncreaseAggro
	{
		get
		{
			return this.ExposedVariables.Buff_IncreaseAggro;
		}
		set
		{
			this.ExposedVariables.Buff_IncreaseAggro = value;
		}
	}

	// Token: 0x06004E21 RID: 20001 RVA: 0x00158254 File Offset: 0x00156454
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

	// Token: 0x06004E22 RID: 20002 RVA: 0x001582BC File Offset: 0x001564BC
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06004E23 RID: 20003 RVA: 0x001582CC File Offset: 0x001564CC
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06004E24 RID: 20004 RVA: 0x001582DC File Offset: 0x001564DC
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06004E25 RID: 20005 RVA: 0x001582EC File Offset: 0x001564EC
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06004E26 RID: 20006 RVA: 0x001582FC File Offset: 0x001564FC
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x040067AD RID: 26541
	public M02_Gameplay ExposedVariables = new M02_Gameplay();
}
