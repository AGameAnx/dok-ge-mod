using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Queries;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x0200087A RID: 2170
[AddComponentMenu("uScript/Graphs/M09_Test")]
public class M09_Test_Component : uScriptCode
{
	// Token: 0x0600F1E8 RID: 61928 RVA: 0x0044A2B4 File Offset: 0x004484B4
	public M09_Test_Component()
	{
	}

	// Token: 0x170008AA RID: 2218
	// (get) Token: 0x0600F1E9 RID: 61929 RVA: 0x0044A2C8 File Offset: 0x004484C8
	// (set) Token: 0x0600F1EA RID: 61930 RVA: 0x0044A2D8 File Offset: 0x004484D8
	public QueryContainer Query_InDamageVolume
	{
		get
		{
			return this.ExposedVariables.Query_InDamageVolume;
		}
		set
		{
			this.ExposedVariables.Query_InDamageVolume = value;
		}
	}

	// Token: 0x170008AB RID: 2219
	// (get) Token: 0x0600F1EB RID: 61931 RVA: 0x0044A2E8 File Offset: 0x004484E8
	// (set) Token: 0x0600F1EC RID: 61932 RVA: 0x0044A2F8 File Offset: 0x004484F8
	public AttributeBuffSetData DamageVolumeEffects
	{
		get
		{
			return this.ExposedVariables.DamageVolumeEffects;
		}
		set
		{
			this.ExposedVariables.DamageVolumeEffects = value;
		}
	}

	// Token: 0x170008AC RID: 2220
	// (get) Token: 0x0600F1ED RID: 61933 RVA: 0x0044A308 File Offset: 0x00448508
	// (set) Token: 0x0600F1EE RID: 61934 RVA: 0x0044A318 File Offset: 0x00448518
	public List<Entity> Gaalsi_StartingCarrier1
	{
		get
		{
			return this.ExposedVariables.Gaalsi_StartingCarrier1;
		}
		set
		{
			this.ExposedVariables.Gaalsi_StartingCarrier1 = value;
		}
	}

	// Token: 0x0600F1EF RID: 61935 RVA: 0x0044A328 File Offset: 0x00448528
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

	// Token: 0x0600F1F0 RID: 61936 RVA: 0x0044A390 File Offset: 0x00448590
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0600F1F1 RID: 61937 RVA: 0x0044A3A0 File Offset: 0x004485A0
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600F1F2 RID: 61938 RVA: 0x0044A3B0 File Offset: 0x004485B0
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600F1F3 RID: 61939 RVA: 0x0044A3C0 File Offset: 0x004485C0
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x0600F1F4 RID: 61940 RVA: 0x0044A3D0 File Offset: 0x004485D0
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04016B24 RID: 92964
	public M09_Test ExposedVariables = new M09_Test();
}
