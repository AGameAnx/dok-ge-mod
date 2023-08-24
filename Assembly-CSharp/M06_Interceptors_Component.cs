using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using UnityEngine;

// Token: 0x02000852 RID: 2130
[AddComponentMenu("uScript/Graphs/M06_Interceptors")]
public class M06_Interceptors_Component : uScriptCode
{
	// Token: 0x06009EA9 RID: 40617 RVA: 0x002D97DC File Offset: 0x002D79DC
	public M06_Interceptors_Component()
	{
	}

	// Token: 0x1700072C RID: 1836
	// (get) Token: 0x06009EAA RID: 40618 RVA: 0x002D97F0 File Offset: 0x002D79F0
	// (set) Token: 0x06009EAB RID: 40619 RVA: 0x002D9800 File Offset: 0x002D7A00
	public QueryContainer GaalsiInterceptorTAG
	{
		get
		{
			return this.ExposedVariables.GaalsiInterceptorTAG;
		}
		set
		{
			this.ExposedVariables.GaalsiInterceptorTAG = value;
		}
	}

	// Token: 0x1700072D RID: 1837
	// (get) Token: 0x06009EAC RID: 40620 RVA: 0x002D9810 File Offset: 0x002D7A10
	// (set) Token: 0x06009EAD RID: 40621 RVA: 0x002D9820 File Offset: 0x002D7A20
	public UnitSpawnWaveData GaalsiInterceptorGroup1
	{
		get
		{
			return this.ExposedVariables.GaalsiInterceptorGroup1;
		}
		set
		{
			this.ExposedVariables.GaalsiInterceptorGroup1 = value;
		}
	}

	// Token: 0x1700072E RID: 1838
	// (get) Token: 0x06009EAE RID: 40622 RVA: 0x002D9830 File Offset: 0x002D7A30
	// (set) Token: 0x06009EAF RID: 40623 RVA: 0x002D9840 File Offset: 0x002D7A40
	public QueryContainer CarrierQuery
	{
		get
		{
			return this.ExposedVariables.CarrierQuery;
		}
		set
		{
			this.ExposedVariables.CarrierQuery = value;
		}
	}

	// Token: 0x06009EB0 RID: 40624 RVA: 0x002D9850 File Offset: 0x002D7A50
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

	// Token: 0x06009EB1 RID: 40625 RVA: 0x002D98B8 File Offset: 0x002D7AB8
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x06009EB2 RID: 40626 RVA: 0x002D98C8 File Offset: 0x002D7AC8
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06009EB3 RID: 40627 RVA: 0x002D98D8 File Offset: 0x002D7AD8
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06009EB4 RID: 40628 RVA: 0x002D98E8 File Offset: 0x002D7AE8
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x06009EB5 RID: 40629 RVA: 0x002D98F8 File Offset: 0x002D7AF8
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400ECB7 RID: 60599
	public M06_Interceptors ExposedVariables = new M06_Interceptors();
}
