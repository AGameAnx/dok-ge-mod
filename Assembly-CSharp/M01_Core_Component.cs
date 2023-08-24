using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x02000808 RID: 2056
[AddComponentMenu("uScript/Graphs/M01_Core")]
public class M01_Core_Component : uScriptCode
{
	// Token: 0x06003744 RID: 14148 RVA: 0x000F3024 File Offset: 0x000F1224
	public M01_Core_Component()
	{
	}

	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x06003745 RID: 14149 RVA: 0x000F3038 File Offset: 0x000F1238
	// (set) Token: 0x06003746 RID: 14150 RVA: 0x000F3048 File Offset: 0x000F1248
	public UnitSpawnWaveData Wave_Carrier
	{
		get
		{
			return this.ExposedVariables.Wave_Carrier;
		}
		set
		{
			this.ExposedVariables.Wave_Carrier = value;
		}
	}

	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x06003747 RID: 14151 RVA: 0x000F3058 File Offset: 0x000F1258
	// (set) Token: 0x06003748 RID: 14152 RVA: 0x000F3068 File Offset: 0x000F1268
	public UnitSpawnWaveData Wave_SupportCruiser
	{
		get
		{
			return this.ExposedVariables.Wave_SupportCruiser;
		}
		set
		{
			this.ExposedVariables.Wave_SupportCruiser = value;
		}
	}

	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x06003749 RID: 14153 RVA: 0x000F3078 File Offset: 0x000F1278
	// (set) Token: 0x0600374A RID: 14154 RVA: 0x000F3088 File Offset: 0x000F1288
	public UnitSpawnWaveData Wave_Target
	{
		get
		{
			return this.ExposedVariables.Wave_Target;
		}
		set
		{
			this.ExposedVariables.Wave_Target = value;
		}
	}

	// Token: 0x170005A4 RID: 1444
	// (get) Token: 0x0600374B RID: 14155 RVA: 0x000F3098 File Offset: 0x000F1298
	// (set) Token: 0x0600374C RID: 14156 RVA: 0x000F30A8 File Offset: 0x000F12A8
	public AttributeBuffSetData Buff_Target
	{
		get
		{
			return this.ExposedVariables.Buff_Target;
		}
		set
		{
			this.ExposedVariables.Buff_Target = value;
		}
	}

	// Token: 0x170005A5 RID: 1445
	// (get) Token: 0x0600374D RID: 14157 RVA: 0x000F30B8 File Offset: 0x000F12B8
	// (set) Token: 0x0600374E RID: 14158 RVA: 0x000F30C8 File Offset: 0x000F12C8
	public UnitSpawnWaveData Wave_Rachel
	{
		get
		{
			return this.ExposedVariables.Wave_Rachel;
		}
		set
		{
			this.ExposedVariables.Wave_Rachel = value;
		}
	}

	// Token: 0x170005A6 RID: 1446
	// (get) Token: 0x0600374F RID: 14159 RVA: 0x000F30D8 File Offset: 0x000F12D8
	// (set) Token: 0x06003750 RID: 14160 RVA: 0x000F30E8 File Offset: 0x000F12E8
	public UnitSpawnWaveData Wave_Hacs
	{
		get
		{
			return this.ExposedVariables.Wave_Hacs;
		}
		set
		{
			this.ExposedVariables.Wave_Hacs = value;
		}
	}

	// Token: 0x170005A7 RID: 1447
	// (get) Token: 0x06003751 RID: 14161 RVA: 0x000F30F8 File Offset: 0x000F12F8
	// (set) Token: 0x06003752 RID: 14162 RVA: 0x000F3108 File Offset: 0x000F1308
	public AttributeBuffSetData Buff_DisableWeapons
	{
		get
		{
			return this.ExposedVariables.Buff_DisableWeapons;
		}
		set
		{
			this.ExposedVariables.Buff_DisableWeapons = value;
		}
	}

	// Token: 0x170005A8 RID: 1448
	// (get) Token: 0x06003753 RID: 14163 RVA: 0x000F3118 File Offset: 0x000F1318
	// (set) Token: 0x06003754 RID: 14164 RVA: 0x000F3128 File Offset: 0x000F1328
	public UnitSpawnWaveData Wave_Salvager
	{
		get
		{
			return this.ExposedVariables.Wave_Salvager;
		}
		set
		{
			this.ExposedVariables.Wave_Salvager = value;
		}
	}

	// Token: 0x170005A9 RID: 1449
	// (get) Token: 0x06003755 RID: 14165 RVA: 0x000F3138 File Offset: 0x000F1338
	// (set) Token: 0x06003756 RID: 14166 RVA: 0x000F3148 File Offset: 0x000F1348
	public AttributeBuffSetData Buff_Carrier
	{
		get
		{
			return this.ExposedVariables.Buff_Carrier;
		}
		set
		{
			this.ExposedVariables.Buff_Carrier = value;
		}
	}

	// Token: 0x170005AA RID: 1450
	// (get) Token: 0x06003757 RID: 14167 RVA: 0x000F3158 File Offset: 0x000F1358
	// (set) Token: 0x06003758 RID: 14168 RVA: 0x000F3168 File Offset: 0x000F1368
	public QueryContainer QueryAllUnits
	{
		get
		{
			return this.ExposedVariables.QueryAllUnits;
		}
		set
		{
			this.ExposedVariables.QueryAllUnits = value;
		}
	}

	// Token: 0x170005AB RID: 1451
	// (get) Token: 0x06003759 RID: 14169 RVA: 0x000F3178 File Offset: 0x000F1378
	// (set) Token: 0x0600375A RID: 14170 RVA: 0x000F3188 File Offset: 0x000F1388
	public AttributeBuffSetData Buff_SupportCruiserSpeed
	{
		get
		{
			return this.ExposedVariables.Buff_SupportCruiserSpeed;
		}
		set
		{
			this.ExposedVariables.Buff_SupportCruiserSpeed = value;
		}
	}

	// Token: 0x170005AC RID: 1452
	// (get) Token: 0x0600375B RID: 14171 RVA: 0x000F3198 File Offset: 0x000F1398
	// (set) Token: 0x0600375C RID: 14172 RVA: 0x000F31A8 File Offset: 0x000F13A8
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

	// Token: 0x0600375D RID: 14173 RVA: 0x000F31B8 File Offset: 0x000F13B8
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

	// Token: 0x0600375E RID: 14174 RVA: 0x000F3220 File Offset: 0x000F1420
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600375F RID: 14175 RVA: 0x000F3230 File Offset: 0x000F1430
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06003760 RID: 14176 RVA: 0x000F3240 File Offset: 0x000F1440
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06003761 RID: 14177 RVA: 0x000F3250 File Offset: 0x000F1450
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x06003762 RID: 14178 RVA: 0x000F3260 File Offset: 0x000F1460
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400444D RID: 17485
	public M01_Core ExposedVariables = new M01_Core();
}
