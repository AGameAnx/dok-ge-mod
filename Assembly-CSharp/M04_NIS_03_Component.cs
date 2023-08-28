using System;
using BBI.Game.Data.Scripting;
using UnityEngine;

// Token: 0x02000830 RID: 2096
[AddComponentMenu("uScript/Graphs/M04_NIS_03")]
public class M04_NIS_03_Component : uScriptCode
{
	// Token: 0x06008245 RID: 33349 RVA: 0x0024FDB8 File Offset: 0x0024DFB8
	public M04_NIS_03_Component()
	{
	}

	// Token: 0x1700069E RID: 1694
	// (get) Token: 0x06008246 RID: 33350 RVA: 0x0024FDCC File Offset: 0x0024DFCC
	// (set) Token: 0x06008247 RID: 33351 RVA: 0x0024FDDC File Offset: 0x0024DFDC
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

	// Token: 0x1700069F RID: 1695
	// (get) Token: 0x06008248 RID: 33352 RVA: 0x0024FDEC File Offset: 0x0024DFEC
	// (set) Token: 0x06008249 RID: 33353 RVA: 0x0024FDFC File Offset: 0x0024DFFC
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

	// Token: 0x170006A0 RID: 1696
	// (get) Token: 0x0600824A RID: 33354 RVA: 0x0024FE0C File Offset: 0x0024E00C
	// (set) Token: 0x0600824B RID: 33355 RVA: 0x0024FE1C File Offset: 0x0024E01C
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

	// Token: 0x0600824C RID: 33356 RVA: 0x0024FE2C File Offset: 0x0024E02C
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

	// Token: 0x0600824D RID: 33357 RVA: 0x0024FE94 File Offset: 0x0024E094
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0600824E RID: 33358 RVA: 0x0024FEA4 File Offset: 0x0024E0A4
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600824F RID: 33359 RVA: 0x0024FEB4 File Offset: 0x0024E0B4
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06008250 RID: 33360 RVA: 0x0024FEC4 File Offset: 0x0024E0C4
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06008251 RID: 33361 RVA: 0x0024FED4 File Offset: 0x0024E0D4
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400BB8F RID: 48015
	public M04_NIS_03 ExposedVariables = new M04_NIS_03();
}
