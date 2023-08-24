using System;
using BBI.Game.Data.Queries;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x0200088D RID: 2189
[AddComponentMenu("uScript/Graphs/M11_Core")]
public class M11_Core_Component : uScriptCode
{
	// Token: 0x060107D0 RID: 67536 RVA: 0x004AD510 File Offset: 0x004AB710
	public M11_Core_Component()
	{
	}

	// Token: 0x17000914 RID: 2324
	// (get) Token: 0x060107D1 RID: 67537 RVA: 0x004AD524 File Offset: 0x004AB724
	// (set) Token: 0x060107D2 RID: 67538 RVA: 0x004AD534 File Offset: 0x004AB734
	public BuffSetAttributesAsset Buff_Gaalsi
	{
		get
		{
			return this.ExposedVariables.Buff_Gaalsi;
		}
		set
		{
			this.ExposedVariables.Buff_Gaalsi = value;
		}
	}

	// Token: 0x17000915 RID: 2325
	// (get) Token: 0x060107D3 RID: 67539 RVA: 0x004AD544 File Offset: 0x004AB744
	// (set) Token: 0x060107D4 RID: 67540 RVA: 0x004AD554 File Offset: 0x004AB754
	public QueryContainer MissionFailureCarrierGodUnitsQ
	{
		get
		{
			return this.ExposedVariables.MissionFailureCarrierGodUnitsQ;
		}
		set
		{
			this.ExposedVariables.MissionFailureCarrierGodUnitsQ = value;
		}
	}

	// Token: 0x17000916 RID: 2326
	// (get) Token: 0x060107D5 RID: 67541 RVA: 0x004AD564 File Offset: 0x004AB764
	// (set) Token: 0x060107D6 RID: 67542 RVA: 0x004AD574 File Offset: 0x004AB774
	public QueryContainer MissionFailureCarrierSuspendUnitsQ
	{
		get
		{
			return this.ExposedVariables.MissionFailureCarrierSuspendUnitsQ;
		}
		set
		{
			this.ExposedVariables.MissionFailureCarrierSuspendUnitsQ = value;
		}
	}

	// Token: 0x17000917 RID: 2327
	// (get) Token: 0x060107D7 RID: 67543 RVA: 0x004AD584 File Offset: 0x004AB784
	// (set) Token: 0x060107D8 RID: 67544 RVA: 0x004AD594 File Offset: 0x004AB794
	public QueryContainer CCQueryContainer
	{
		get
		{
			return this.ExposedVariables.CCQueryContainer;
		}
		set
		{
			this.ExposedVariables.CCQueryContainer = value;
		}
	}

	// Token: 0x060107D9 RID: 67545 RVA: 0x004AD5A4 File Offset: 0x004AB7A4
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

	// Token: 0x060107DA RID: 67546 RVA: 0x004AD60C File Offset: 0x004AB80C
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x060107DB RID: 67547 RVA: 0x004AD61C File Offset: 0x004AB81C
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x060107DC RID: 67548 RVA: 0x004AD62C File Offset: 0x004AB82C
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x060107DD RID: 67549 RVA: 0x004AD63C File Offset: 0x004AB83C
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x060107DE RID: 67550 RVA: 0x004AD64C File Offset: 0x004AB84C
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04018B83 RID: 101251
	public M11_Core ExposedVariables = new M11_Core();
}
