using System;
using BBI.Game.Data.Queries;
using UnityEngine;

// Token: 0x02000842 RID: 2114
[AddComponentMenu("uScript/Graphs/M05_NIS_01")]
public class M05_NIS_01_Component : uScriptCode
{
	// Token: 0x06009131 RID: 37169 RVA: 0x0029B128 File Offset: 0x00299328
	public M05_NIS_01_Component()
	{
	}

	// Token: 0x170006E5 RID: 1765
	// (get) Token: 0x06009132 RID: 37170 RVA: 0x0029B13C File Offset: 0x0029933C
	// (set) Token: 0x06009133 RID: 37171 RVA: 0x0029B14C File Offset: 0x0029934C
	public QueryContainer Kapisi
	{
		get
		{
			return this.ExposedVariables.Kapisi;
		}
		set
		{
			this.ExposedVariables.Kapisi = value;
		}
	}

	// Token: 0x170006E6 RID: 1766
	// (get) Token: 0x06009134 RID: 37172 RVA: 0x0029B15C File Offset: 0x0029935C
	// (set) Token: 0x06009135 RID: 37173 RVA: 0x0029B16C File Offset: 0x0029936C
	public QueryContainer GOD
	{
		get
		{
			return this.ExposedVariables.GOD;
		}
		set
		{
			this.ExposedVariables.GOD = value;
		}
	}

	// Token: 0x170006E7 RID: 1767
	// (get) Token: 0x06009136 RID: 37174 RVA: 0x0029B17C File Offset: 0x0029937C
	// (set) Token: 0x06009137 RID: 37175 RVA: 0x0029B18C File Offset: 0x0029938C
	public QueryContainer SUSP
	{
		get
		{
			return this.ExposedVariables.SUSP;
		}
		set
		{
			this.ExposedVariables.SUSP = value;
		}
	}

	// Token: 0x170006E8 RID: 1768
	// (get) Token: 0x06009138 RID: 37176 RVA: 0x0029B19C File Offset: 0x0029939C
	// (set) Token: 0x06009139 RID: 37177 RVA: 0x0029B1AC File Offset: 0x002993AC
	public QueryContainer Rachel_Query
	{
		get
		{
			return this.ExposedVariables.Rachel_Query;
		}
		set
		{
			this.ExposedVariables.Rachel_Query = value;
		}
	}

	// Token: 0x0600913A RID: 37178 RVA: 0x0029B1BC File Offset: 0x002993BC
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

	// Token: 0x0600913B RID: 37179 RVA: 0x0029B224 File Offset: 0x00299424
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600913C RID: 37180 RVA: 0x0029B234 File Offset: 0x00299434
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600913D RID: 37181 RVA: 0x0029B244 File Offset: 0x00299444
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600913E RID: 37182 RVA: 0x0029B254 File Offset: 0x00299454
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600913F RID: 37183 RVA: 0x0029B264 File Offset: 0x00299464
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400D5F2 RID: 54770
	public M05_NIS_01 ExposedVariables = new M05_NIS_01();
}
