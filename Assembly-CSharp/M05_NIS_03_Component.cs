using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x02000846 RID: 2118
[AddComponentMenu("uScript/Graphs/M05_NIS_03")]
public class M05_NIS_03_Component : uScriptCode
{
	// Token: 0x060094F7 RID: 38135 RVA: 0x002ADC68 File Offset: 0x002ABE68
	public M05_NIS_03_Component()
	{
	}

	// Token: 0x170006EB RID: 1771
	// (get) Token: 0x060094F8 RID: 38136 RVA: 0x002ADC7C File Offset: 0x002ABE7C
	// (set) Token: 0x060094F9 RID: 38137 RVA: 0x002ADC8C File Offset: 0x002ABE8C
	public UnitSpawnWaveData NIS_03_CAT_WAVE
	{
		get
		{
			return this.ExposedVariables.NIS_03_CAT_WAVE;
		}
		set
		{
			this.ExposedVariables.NIS_03_CAT_WAVE = value;
		}
	}

	// Token: 0x170006EC RID: 1772
	// (get) Token: 0x060094FA RID: 38138 RVA: 0x002ADC9C File Offset: 0x002ABE9C
	// (set) Token: 0x060094FB RID: 38139 RVA: 0x002ADCAC File Offset: 0x002ABEAC
	public UnitSpawnWaveData NIS_03_SS_WAVE
	{
		get
		{
			return this.ExposedVariables.NIS_03_SS_WAVE;
		}
		set
		{
			this.ExposedVariables.NIS_03_SS_WAVE = value;
		}
	}

	// Token: 0x170006ED RID: 1773
	// (get) Token: 0x060094FC RID: 38140 RVA: 0x002ADCBC File Offset: 0x002ABEBC
	// (set) Token: 0x060094FD RID: 38141 RVA: 0x002ADCCC File Offset: 0x002ABECC
	public UnitSpawnWaveData NIS_03_Tank_WAVE
	{
		get
		{
			return this.ExposedVariables.NIS_03_Tank_WAVE;
		}
		set
		{
			this.ExposedVariables.NIS_03_Tank_WAVE = value;
		}
	}

	// Token: 0x170006EE RID: 1774
	// (get) Token: 0x060094FE RID: 38142 RVA: 0x002ADCDC File Offset: 0x002ABEDC
	// (set) Token: 0x060094FF RID: 38143 RVA: 0x002ADCEC File Offset: 0x002ABEEC
	public UnitSpawnWaveData NIS03_CruiserGroup
	{
		get
		{
			return this.ExposedVariables.NIS03_CruiserGroup;
		}
		set
		{
			this.ExposedVariables.NIS03_CruiserGroup = value;
		}
	}

	// Token: 0x170006EF RID: 1775
	// (get) Token: 0x06009500 RID: 38144 RVA: 0x002ADCFC File Offset: 0x002ABEFC
	// (set) Token: 0x06009501 RID: 38145 RVA: 0x002ADD0C File Offset: 0x002ABF0C
	public QueryContainer Cruiser1_Lookup
	{
		get
		{
			return this.ExposedVariables.Cruiser1_Lookup;
		}
		set
		{
			this.ExposedVariables.Cruiser1_Lookup = value;
		}
	}

	// Token: 0x170006F0 RID: 1776
	// (get) Token: 0x06009502 RID: 38146 RVA: 0x002ADD1C File Offset: 0x002ABF1C
	// (set) Token: 0x06009503 RID: 38147 RVA: 0x002ADD2C File Offset: 0x002ABF2C
	public QueryContainer Cruiser2_Lookup
	{
		get
		{
			return this.ExposedVariables.Cruiser2_Lookup;
		}
		set
		{
			this.ExposedVariables.Cruiser2_Lookup = value;
		}
	}

	// Token: 0x170006F1 RID: 1777
	// (get) Token: 0x06009504 RID: 38148 RVA: 0x002ADD3C File Offset: 0x002ABF3C
	// (set) Token: 0x06009505 RID: 38149 RVA: 0x002ADD4C File Offset: 0x002ABF4C
	public UnitSpawnWaveData Gaalsi_Bomb_Targets
	{
		get
		{
			return this.ExposedVariables.Gaalsi_Bomb_Targets;
		}
		set
		{
			this.ExposedVariables.Gaalsi_Bomb_Targets = value;
		}
	}

	// Token: 0x170006F2 RID: 1778
	// (get) Token: 0x06009506 RID: 38150 RVA: 0x002ADD5C File Offset: 0x002ABF5C
	// (set) Token: 0x06009507 RID: 38151 RVA: 0x002ADD6C File Offset: 0x002ABF6C
	public QueryContainer G_CarrierLookup
	{
		get
		{
			return this.ExposedVariables.G_CarrierLookup;
		}
		set
		{
			this.ExposedVariables.G_CarrierLookup = value;
		}
	}

	// Token: 0x170006F3 RID: 1779
	// (get) Token: 0x06009508 RID: 38152 RVA: 0x002ADD7C File Offset: 0x002ABF7C
	// (set) Token: 0x06009509 RID: 38153 RVA: 0x002ADD8C File Offset: 0x002ABF8C
	public QueryContainer PreviousCruisers
	{
		get
		{
			return this.ExposedVariables.PreviousCruisers;
		}
		set
		{
			this.ExposedVariables.PreviousCruisers = value;
		}
	}

	// Token: 0x170006F4 RID: 1780
	// (get) Token: 0x0600950A RID: 38154 RVA: 0x002ADD9C File Offset: 0x002ABF9C
	// (set) Token: 0x0600950B RID: 38155 RVA: 0x002ADDAC File Offset: 0x002ABFAC
	public AttributeBuffSetData Leash
	{
		get
		{
			return this.ExposedVariables.Leash;
		}
		set
		{
			this.ExposedVariables.Leash = value;
		}
	}

	// Token: 0x0600950C RID: 38156 RVA: 0x002ADDBC File Offset: 0x002ABFBC
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

	// Token: 0x0600950D RID: 38157 RVA: 0x002ADE24 File Offset: 0x002AC024
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0600950E RID: 38158 RVA: 0x002ADE34 File Offset: 0x002AC034
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600950F RID: 38159 RVA: 0x002ADE44 File Offset: 0x002AC044
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06009510 RID: 38160 RVA: 0x002ADE54 File Offset: 0x002AC054
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06009511 RID: 38161 RVA: 0x002ADE64 File Offset: 0x002AC064
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400DD07 RID: 56583
	public M05_NIS_03 ExposedVariables = new M05_NIS_03();
}
