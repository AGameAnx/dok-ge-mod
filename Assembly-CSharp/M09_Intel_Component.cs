using System;
using BBI.Game.Data.Queries;
using UnityEngine;

// Token: 0x02000872 RID: 2162
[AddComponentMenu("uScript/Graphs/M09_Intel")]
public class M09_Intel_Component : uScriptCode
{
	// Token: 0x0600E33B RID: 58171 RVA: 0x0040CC4C File Offset: 0x0040AE4C
	public M09_Intel_Component()
	{
	}

	// Token: 0x1700085E RID: 2142
	// (get) Token: 0x0600E33C RID: 58172 RVA: 0x0040CC60 File Offset: 0x0040AE60
	// (set) Token: 0x0600E33D RID: 58173 RVA: 0x0040CC70 File Offset: 0x0040AE70
	public bool IntroEventDone
	{
		get
		{
			return this.ExposedVariables.IntroEventDone;
		}
		set
		{
			this.ExposedVariables.IntroEventDone = value;
		}
	}

	// Token: 0x1700085F RID: 2143
	// (get) Token: 0x0600E33E RID: 58174 RVA: 0x0040CC80 File Offset: 0x0040AE80
	// (set) Token: 0x0600E33F RID: 58175 RVA: 0x0040CC90 File Offset: 0x0040AE90
	public QueryContainer AllPlayerUnits
	{
		get
		{
			return this.ExposedVariables.AllPlayerUnits;
		}
		set
		{
			this.ExposedVariables.AllPlayerUnits = value;
		}
	}

	// Token: 0x17000860 RID: 2144
	// (get) Token: 0x0600E340 RID: 58176 RVA: 0x0040CCA0 File Offset: 0x0040AEA0
	// (set) Token: 0x0600E341 RID: 58177 RVA: 0x0040CCB0 File Offset: 0x0040AEB0
	public QueryContainer SuspendUnits
	{
		get
		{
			return this.ExposedVariables.SuspendUnits;
		}
		set
		{
			this.ExposedVariables.SuspendUnits = value;
		}
	}

	// Token: 0x17000861 RID: 2145
	// (get) Token: 0x0600E342 RID: 58178 RVA: 0x0040CCC0 File Offset: 0x0040AEC0
	// (set) Token: 0x0600E343 RID: 58179 RVA: 0x0040CCD0 File Offset: 0x0040AED0
	public QueryContainer KapisiSearch
	{
		get
		{
			return this.ExposedVariables.KapisiSearch;
		}
		set
		{
			this.ExposedVariables.KapisiSearch = value;
		}
	}

	// Token: 0x17000862 RID: 2146
	// (get) Token: 0x0600E344 RID: 58180 RVA: 0x0040CCE0 File Offset: 0x0040AEE0
	// (set) Token: 0x0600E345 RID: 58181 RVA: 0x0040CCF0 File Offset: 0x0040AEF0
	public GameObject NIS0901CAMVariable
	{
		get
		{
			return this.ExposedVariables.NIS0901CAMVariable;
		}
		set
		{
			this.ExposedVariables.NIS0901CAMVariable = value;
		}
	}

	// Token: 0x0600E346 RID: 58182 RVA: 0x0040CD00 File Offset: 0x0040AF00
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

	// Token: 0x0600E347 RID: 58183 RVA: 0x0040CD68 File Offset: 0x0040AF68
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0600E348 RID: 58184 RVA: 0x0040CD78 File Offset: 0x0040AF78
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600E349 RID: 58185 RVA: 0x0040CD88 File Offset: 0x0040AF88
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600E34A RID: 58186 RVA: 0x0040CD98 File Offset: 0x0040AF98
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x0600E34B RID: 58187 RVA: 0x0040CDA8 File Offset: 0x0040AFA8
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x040157FA RID: 88058
	public M09_Intel ExposedVariables = new M09_Intel();
}
