using System;
using BBI.Game.Data.Queries;
using UnityEngine;

// Token: 0x02000887 RID: 2183
[AddComponentMenu("uScript/Graphs/M10_Gameplay")]
public class M10_Gameplay_Component : uScriptCode
{
	// Token: 0x060101F0 RID: 66032 RVA: 0x00494EE8 File Offset: 0x004930E8
	public M10_Gameplay_Component()
	{
	}

	// Token: 0x170008F9 RID: 2297
	// (get) Token: 0x060101F1 RID: 66033 RVA: 0x00494EFC File Offset: 0x004930FC
	// (set) Token: 0x060101F2 RID: 66034 RVA: 0x00494F0C File Offset: 0x0049310C
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

	// Token: 0x170008FA RID: 2298
	// (get) Token: 0x060101F3 RID: 66035 RVA: 0x00494F1C File Offset: 0x0049311C
	// (set) Token: 0x060101F4 RID: 66036 RVA: 0x00494F2C File Offset: 0x0049312C
	public QueryContainer TopSquads
	{
		get
		{
			return this.ExposedVariables.TopSquads;
		}
		set
		{
			this.ExposedVariables.TopSquads = value;
		}
	}

	// Token: 0x170008FB RID: 2299
	// (get) Token: 0x060101F5 RID: 66037 RVA: 0x00494F3C File Offset: 0x0049313C
	// (set) Token: 0x060101F6 RID: 66038 RVA: 0x00494F4C File Offset: 0x0049314C
	public QueryContainer QueryGaalsiAA
	{
		get
		{
			return this.ExposedVariables.QueryGaalsiAA;
		}
		set
		{
			this.ExposedVariables.QueryGaalsiAA = value;
		}
	}

	// Token: 0x170008FC RID: 2300
	// (get) Token: 0x060101F7 RID: 66039 RVA: 0x00494F5C File Offset: 0x0049315C
	// (set) Token: 0x060101F8 RID: 66040 RVA: 0x00494F6C File Offset: 0x0049316C
	public QueryContainer QueryAASquad1
	{
		get
		{
			return this.ExposedVariables.QueryAASquad1;
		}
		set
		{
			this.ExposedVariables.QueryAASquad1 = value;
		}
	}

	// Token: 0x170008FD RID: 2301
	// (get) Token: 0x060101F9 RID: 66041 RVA: 0x00494F7C File Offset: 0x0049317C
	// (set) Token: 0x060101FA RID: 66042 RVA: 0x00494F8C File Offset: 0x0049318C
	public QueryContainer QueryAASquad3
	{
		get
		{
			return this.ExposedVariables.QueryAASquad3;
		}
		set
		{
			this.ExposedVariables.QueryAASquad3 = value;
		}
	}

	// Token: 0x170008FE RID: 2302
	// (get) Token: 0x060101FB RID: 66043 RVA: 0x00494F9C File Offset: 0x0049319C
	// (set) Token: 0x060101FC RID: 66044 RVA: 0x00494FAC File Offset: 0x004931AC
	public QueryContainer QueryAASquad5
	{
		get
		{
			return this.ExposedVariables.QueryAASquad5;
		}
		set
		{
			this.ExposedVariables.QueryAASquad5 = value;
		}
	}

	// Token: 0x170008FF RID: 2303
	// (get) Token: 0x060101FD RID: 66045 RVA: 0x00494FBC File Offset: 0x004931BC
	// (set) Token: 0x060101FE RID: 66046 RVA: 0x00494FCC File Offset: 0x004931CC
	public float Intel1StartTime
	{
		get
		{
			return this.ExposedVariables.Intel1StartTime;
		}
		set
		{
			this.ExposedVariables.Intel1StartTime = value;
		}
	}

	// Token: 0x17000900 RID: 2304
	// (get) Token: 0x060101FF RID: 66047 RVA: 0x00494FDC File Offset: 0x004931DC
	// (set) Token: 0x06010200 RID: 66048 RVA: 0x00494FEC File Offset: 0x004931EC
	public GameObject NIS01_Camera
	{
		get
		{
			return this.ExposedVariables.NIS01_Camera;
		}
		set
		{
			this.ExposedVariables.NIS01_Camera = value;
		}
	}

	// Token: 0x06010201 RID: 66049 RVA: 0x00494FFC File Offset: 0x004931FC
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

	// Token: 0x06010202 RID: 66050 RVA: 0x00495064 File Offset: 0x00493264
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06010203 RID: 66051 RVA: 0x00495074 File Offset: 0x00493274
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06010204 RID: 66052 RVA: 0x00495084 File Offset: 0x00493284
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06010205 RID: 66053 RVA: 0x00495094 File Offset: 0x00493294
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06010206 RID: 66054 RVA: 0x004950A4 File Offset: 0x004932A4
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x040183E0 RID: 99296
	public M10_Gameplay ExposedVariables = new M10_Gameplay();
}
