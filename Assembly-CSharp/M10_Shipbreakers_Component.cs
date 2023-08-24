using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x0200088B RID: 2187
[AddComponentMenu("uScript/Graphs/M10_Shipbreakers")]
public class M10_Shipbreakers_Component : uScriptCode
{
	// Token: 0x0601075B RID: 67419 RVA: 0x004ABCF8 File Offset: 0x004A9EF8
	public M10_Shipbreakers_Component()
	{
	}

	// Token: 0x17000910 RID: 2320
	// (get) Token: 0x0601075C RID: 67420 RVA: 0x004ABD0C File Offset: 0x004A9F0C
	// (set) Token: 0x0601075D RID: 67421 RVA: 0x004ABD1C File Offset: 0x004A9F1C
	public UnitSpawnWaveData G_SupportCruiser
	{
		get
		{
			return this.ExposedVariables.G_SupportCruiser;
		}
		set
		{
			this.ExposedVariables.G_SupportCruiser = value;
		}
	}

	// Token: 0x17000911 RID: 2321
	// (get) Token: 0x0601075E RID: 67422 RVA: 0x004ABD2C File Offset: 0x004A9F2C
	// (set) Token: 0x0601075F RID: 67423 RVA: 0x004ABD3C File Offset: 0x004A9F3C
	public UnitSpawnWaveData G_Baserunner
	{
		get
		{
			return this.ExposedVariables.G_Baserunner;
		}
		set
		{
			this.ExposedVariables.G_Baserunner = value;
		}
	}

	// Token: 0x17000912 RID: 2322
	// (get) Token: 0x06010760 RID: 67424 RVA: 0x004ABD4C File Offset: 0x004A9F4C
	// (set) Token: 0x06010761 RID: 67425 RVA: 0x004ABD5C File Offset: 0x004A9F5C
	public AttributeBuffSetData Buff_GaalsiBaserunners
	{
		get
		{
			return this.ExposedVariables.Buff_GaalsiBaserunners;
		}
		set
		{
			this.ExposedVariables.Buff_GaalsiBaserunners = value;
		}
	}

	// Token: 0x17000913 RID: 2323
	// (get) Token: 0x06010762 RID: 67426 RVA: 0x004ABD6C File Offset: 0x004A9F6C
	// (set) Token: 0x06010763 RID: 67427 RVA: 0x004ABD7C File Offset: 0x004A9F7C
	public QueryContainer QueryGaalsiBRs
	{
		get
		{
			return this.ExposedVariables.QueryGaalsiBRs;
		}
		set
		{
			this.ExposedVariables.QueryGaalsiBRs = value;
		}
	}

	// Token: 0x06010764 RID: 67428 RVA: 0x004ABD8C File Offset: 0x004A9F8C
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

	// Token: 0x06010765 RID: 67429 RVA: 0x004ABDF4 File Offset: 0x004A9FF4
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x06010766 RID: 67430 RVA: 0x004ABE04 File Offset: 0x004AA004
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06010767 RID: 67431 RVA: 0x004ABE14 File Offset: 0x004AA014
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06010768 RID: 67432 RVA: 0x004ABE24 File Offset: 0x004AA024
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x06010769 RID: 67433 RVA: 0x004ABE34 File Offset: 0x004AA034
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04018B1C RID: 101148
	public M10_Shipbreakers ExposedVariables = new M10_Shipbreakers();
}
