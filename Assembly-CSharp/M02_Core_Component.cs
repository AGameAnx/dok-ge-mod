using System;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x02000812 RID: 2066
[AddComponentMenu("uScript/Graphs/M02_Core")]
public class M02_Core_Component : uScriptCode
{
	// Token: 0x06004383 RID: 17283 RVA: 0x0012769C File Offset: 0x0012589C
	public M02_Core_Component()
	{
	}

	// Token: 0x170005CF RID: 1487
	// (get) Token: 0x06004384 RID: 17284 RVA: 0x001276B0 File Offset: 0x001258B0
	// (set) Token: 0x06004385 RID: 17285 RVA: 0x001276C0 File Offset: 0x001258C0
	public UnitSpawnWaveData DebugMoveTestWave
	{
		get
		{
			return this.ExposedVariables.DebugMoveTestWave;
		}
		set
		{
			this.ExposedVariables.DebugMoveTestWave = value;
		}
	}

	// Token: 0x170005D0 RID: 1488
	// (get) Token: 0x06004386 RID: 17286 RVA: 0x001276D0 File Offset: 0x001258D0
	// (set) Token: 0x06004387 RID: 17287 RVA: 0x001276E0 File Offset: 0x001258E0
	public BuffSetAttributesAsset Buff_GaalsiEasy
	{
		get
		{
			return this.ExposedVariables.Buff_GaalsiEasy;
		}
		set
		{
			this.ExposedVariables.Buff_GaalsiEasy = value;
		}
	}

	// Token: 0x170005D1 RID: 1489
	// (get) Token: 0x06004388 RID: 17288 RVA: 0x001276F0 File Offset: 0x001258F0
	// (set) Token: 0x06004389 RID: 17289 RVA: 0x00127700 File Offset: 0x00125900
	public BuffSetAttributesAsset Buff_GaalsiNormal
	{
		get
		{
			return this.ExposedVariables.Buff_GaalsiNormal;
		}
		set
		{
			this.ExposedVariables.Buff_GaalsiNormal = value;
		}
	}

	// Token: 0x170005D2 RID: 1490
	// (get) Token: 0x0600438A RID: 17290 RVA: 0x00127710 File Offset: 0x00125910
	// (set) Token: 0x0600438B RID: 17291 RVA: 0x00127720 File Offset: 0x00125920
	public BuffSetAttributesAsset Buff_GaalsiHard
	{
		get
		{
			return this.ExposedVariables.Buff_GaalsiHard;
		}
		set
		{
			this.ExposedVariables.Buff_GaalsiHard = value;
		}
	}

	// Token: 0x0600438C RID: 17292 RVA: 0x00127730 File Offset: 0x00125930
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

	// Token: 0x0600438D RID: 17293 RVA: 0x00127798 File Offset: 0x00125998
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600438E RID: 17294 RVA: 0x001277A8 File Offset: 0x001259A8
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600438F RID: 17295 RVA: 0x001277B8 File Offset: 0x001259B8
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06004390 RID: 17296 RVA: 0x001277C8 File Offset: 0x001259C8
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x06004391 RID: 17297 RVA: 0x001277D8 File Offset: 0x001259D8
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04005601 RID: 22017
	public M02_Core ExposedVariables = new M02_Core();
}
