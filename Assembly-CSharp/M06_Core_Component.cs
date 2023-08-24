using System;
using BBI.Game.Data.Queries;
using UnityEngine;

// Token: 0x0200084A RID: 2122
[AddComponentMenu("uScript/Graphs/M06_Core")]
public class M06_Core_Component : uScriptCode
{
	// Token: 0x060095ED RID: 38381 RVA: 0x002B15F8 File Offset: 0x002AF7F8
	public M06_Core_Component()
	{
	}

	// Token: 0x170006F5 RID: 1781
	// (get) Token: 0x060095EE RID: 38382 RVA: 0x002B160C File Offset: 0x002AF80C
	// (set) Token: 0x060095EF RID: 38383 RVA: 0x002B161C File Offset: 0x002AF81C
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

	// Token: 0x170006F6 RID: 1782
	// (get) Token: 0x060095F0 RID: 38384 RVA: 0x002B162C File Offset: 0x002AF82C
	// (set) Token: 0x060095F1 RID: 38385 RVA: 0x002B163C File Offset: 0x002AF83C
	public QueryContainer EmptyQuery
	{
		get
		{
			return this.ExposedVariables.EmptyQuery;
		}
		set
		{
			this.ExposedVariables.EmptyQuery = value;
		}
	}

	// Token: 0x170006F7 RID: 1783
	// (get) Token: 0x060095F2 RID: 38386 RVA: 0x002B164C File Offset: 0x002AF84C
	// (set) Token: 0x060095F3 RID: 38387 RVA: 0x002B165C File Offset: 0x002AF85C
	public QueryContainer AllCommanders
	{
		get
		{
			return this.ExposedVariables.AllCommanders;
		}
		set
		{
			this.ExposedVariables.AllCommanders = value;
		}
	}

	// Token: 0x060095F4 RID: 38388 RVA: 0x002B166C File Offset: 0x002AF86C
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

	// Token: 0x060095F5 RID: 38389 RVA: 0x002B16D4 File Offset: 0x002AF8D4
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x060095F6 RID: 38390 RVA: 0x002B16E4 File Offset: 0x002AF8E4
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x060095F7 RID: 38391 RVA: 0x002B16F4 File Offset: 0x002AF8F4
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x060095F8 RID: 38392 RVA: 0x002B1704 File Offset: 0x002AF904
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x060095F9 RID: 38393 RVA: 0x002B1714 File Offset: 0x002AF914
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400DE2B RID: 56875
	public M06_Core ExposedVariables = new M06_Core();
}
