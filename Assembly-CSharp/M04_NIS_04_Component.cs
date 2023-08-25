using System;
using BBI.Game.Data.Queries;
using UnityEngine;

// Token: 0x02000832 RID: 2098
[AddComponentMenu("uScript/Graphs/M04_NIS_04")]
public class M04_NIS_04_Component : uScriptCode
{
	// Token: 0x06008277 RID: 33399 RVA: 0x002506E4 File Offset: 0x0024E8E4
	public M04_NIS_04_Component()
	{
	}

	// Token: 0x170006A1 RID: 1697
	// (get) Token: 0x06008278 RID: 33400 RVA: 0x002506F8 File Offset: 0x0024E8F8
	// (set) Token: 0x06008279 RID: 33401 RVA: 0x00250708 File Offset: 0x0024E908
	public QueryContainer Rachel
	{
		get
		{
			return this.ExposedVariables.Rachel;
		}
		set
		{
			this.ExposedVariables.Rachel = value;
		}
	}

	// Token: 0x0600827A RID: 33402 RVA: 0x00250718 File Offset: 0x0024E918
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

	// Token: 0x0600827B RID: 33403 RVA: 0x00250780 File Offset: 0x0024E980
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0600827C RID: 33404 RVA: 0x00250790 File Offset: 0x0024E990
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600827D RID: 33405 RVA: 0x002507A0 File Offset: 0x0024E9A0
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600827E RID: 33406 RVA: 0x002507B0 File Offset: 0x0024E9B0
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x0600827F RID: 33407 RVA: 0x002507C0 File Offset: 0x0024E9C0
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400BBB6 RID: 48054
	public M04_NIS_04 ExposedVariables = new M04_NIS_04();
}
