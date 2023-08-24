using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x02000806 RID: 2054
[AddComponentMenu("uScript/Graphs/M01_BaseUnits")]
public class M01_BaseUnits_Component : uScriptCode
{
	// Token: 0x0600358A RID: 13706 RVA: 0x000EB8E0 File Offset: 0x000E9AE0
	public M01_BaseUnits_Component()
	{
	}

	// Token: 0x1700059D RID: 1437
	// (get) Token: 0x0600358B RID: 13707 RVA: 0x000EB8F4 File Offset: 0x000E9AF4
	// (set) Token: 0x0600358C RID: 13708 RVA: 0x000EB904 File Offset: 0x000E9B04
	public AttributeBuffSetData Buff_BaseUnits
	{
		get
		{
			return this.ExposedVariables.Buff_BaseUnits;
		}
		set
		{
			this.ExposedVariables.Buff_BaseUnits = value;
		}
	}

	// Token: 0x1700059E RID: 1438
	// (get) Token: 0x0600358D RID: 13709 RVA: 0x000EB914 File Offset: 0x000E9B14
	// (set) Token: 0x0600358E RID: 13710 RVA: 0x000EB924 File Offset: 0x000E9B24
	public QueryContainer QueryBaseUnits
	{
		get
		{
			return this.ExposedVariables.QueryBaseUnits;
		}
		set
		{
			this.ExposedVariables.QueryBaseUnits = value;
		}
	}

	// Token: 0x1700059F RID: 1439
	// (get) Token: 0x0600358F RID: 13711 RVA: 0x000EB934 File Offset: 0x000E9B34
	// (set) Token: 0x06003590 RID: 13712 RVA: 0x000EB944 File Offset: 0x000E9B44
	public UnitSpawnWaveData Wave_Harvester
	{
		get
		{
			return this.ExposedVariables.Wave_Harvester;
		}
		set
		{
			this.ExposedVariables.Wave_Harvester = value;
		}
	}

	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x06003591 RID: 13713 RVA: 0x000EB954 File Offset: 0x000E9B54
	// (set) Token: 0x06003592 RID: 13714 RVA: 0x000EB964 File Offset: 0x000E9B64
	public UnitSpawnWaveData Wave_Escorts
	{
		get
		{
			return this.ExposedVariables.Wave_Escorts;
		}
		set
		{
			this.ExposedVariables.Wave_Escorts = value;
		}
	}

	// Token: 0x06003593 RID: 13715 RVA: 0x000EB974 File Offset: 0x000E9B74
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

	// Token: 0x06003594 RID: 13716 RVA: 0x000EB9DC File Offset: 0x000E9BDC
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x06003595 RID: 13717 RVA: 0x000EB9EC File Offset: 0x000E9BEC
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06003596 RID: 13718 RVA: 0x000EB9FC File Offset: 0x000E9BFC
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06003597 RID: 13719 RVA: 0x000EBA0C File Offset: 0x000E9C0C
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x06003598 RID: 13720 RVA: 0x000EBA1C File Offset: 0x000E9C1C
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x040041B3 RID: 16819
	public M01_BaseUnits ExposedVariables = new M01_BaseUnits();
}
