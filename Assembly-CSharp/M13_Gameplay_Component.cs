using System;
using BBI.Game.Data.Queries;
using UnityEngine;

// Token: 0x020008B1 RID: 2225
[AddComponentMenu("uScript/Graphs/M13_Gameplay")]
public class M13_Gameplay_Component : uScriptCode
{
	// Token: 0x0601400B RID: 81931 RVA: 0x005AD190 File Offset: 0x005AB390
	public M13_Gameplay_Component()
	{
	}

	// Token: 0x17000A0B RID: 2571
	// (get) Token: 0x0601400C RID: 81932 RVA: 0x005AD1A4 File Offset: 0x005AB3A4
	// (set) Token: 0x0601400D RID: 81933 RVA: 0x005AD1B4 File Offset: 0x005AB3B4
	public QueryContainer Q_PlayerFleet
	{
		get
		{
			return this.ExposedVariables.Q_PlayerFleet;
		}
		set
		{
			this.ExposedVariables.Q_PlayerFleet = value;
		}
	}

	// Token: 0x17000A0C RID: 2572
	// (get) Token: 0x0601400E RID: 81934 RVA: 0x005AD1C4 File Offset: 0x005AB3C4
	// (set) Token: 0x0601400F RID: 81935 RVA: 0x005AD1D4 File Offset: 0x005AB3D4
	public QueryContainer Q_Rachel
	{
		get
		{
			return this.ExposedVariables.Q_Rachel;
		}
		set
		{
			this.ExposedVariables.Q_Rachel = value;
		}
	}

	// Token: 0x17000A0D RID: 2573
	// (get) Token: 0x06014010 RID: 81936 RVA: 0x005AD1E4 File Offset: 0x005AB3E4
	// (set) Token: 0x06014011 RID: 81937 RVA: 0x005AD1F4 File Offset: 0x005AB3F4
	public QueryContainer Q_Interceptors
	{
		get
		{
			return this.ExposedVariables.Q_Interceptors;
		}
		set
		{
			this.ExposedVariables.Q_Interceptors = value;
		}
	}

	// Token: 0x17000A0E RID: 2574
	// (get) Token: 0x06014012 RID: 81938 RVA: 0x005AD204 File Offset: 0x005AB404
	// (set) Token: 0x06014013 RID: 81939 RVA: 0x005AD214 File Offset: 0x005AB414
	public QueryContainer Q_ImmuneOnMissionFail
	{
		get
		{
			return this.ExposedVariables.Q_ImmuneOnMissionFail;
		}
		set
		{
			this.ExposedVariables.Q_ImmuneOnMissionFail = value;
		}
	}

	// Token: 0x17000A0F RID: 2575
	// (get) Token: 0x06014014 RID: 81940 RVA: 0x005AD224 File Offset: 0x005AB424
	// (set) Token: 0x06014015 RID: 81941 RVA: 0x005AD234 File Offset: 0x005AB434
	public QueryContainer Q_SuspendOnMissionFail
	{
		get
		{
			return this.ExposedVariables.Q_SuspendOnMissionFail;
		}
		set
		{
			this.ExposedVariables.Q_SuspendOnMissionFail = value;
		}
	}

	// Token: 0x17000A10 RID: 2576
	// (get) Token: 0x06014016 RID: 81942 RVA: 0x005AD244 File Offset: 0x005AB444
	// (set) Token: 0x06014017 RID: 81943 RVA: 0x005AD254 File Offset: 0x005AB454
	public QueryContainer Q_Kapisi
	{
		get
		{
			return this.ExposedVariables.Q_Kapisi;
		}
		set
		{
			this.ExposedVariables.Q_Kapisi = value;
		}
	}

	// Token: 0x06014018 RID: 81944 RVA: 0x005AD264 File Offset: 0x005AB464
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

	// Token: 0x06014019 RID: 81945 RVA: 0x005AD2CC File Offset: 0x005AB4CC
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0601401A RID: 81946 RVA: 0x005AD2DC File Offset: 0x005AB4DC
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0601401B RID: 81947 RVA: 0x005AD2EC File Offset: 0x005AB4EC
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0601401C RID: 81948 RVA: 0x005AD2FC File Offset: 0x005AB4FC
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x0601401D RID: 81949 RVA: 0x005AD30C File Offset: 0x005AB50C
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401EC07 RID: 125959
	public M13_Gameplay ExposedVariables = new M13_Gameplay();
}
