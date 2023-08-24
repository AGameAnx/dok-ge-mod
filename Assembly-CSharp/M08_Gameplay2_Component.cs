using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Scripting;
using UnityEngine;

// Token: 0x02000865 RID: 2149
[AddComponentMenu("uScript/Graphs/M08_Gameplay2")]
public class M08_Gameplay2_Component : uScriptCode
{
	// Token: 0x0600C6DC RID: 50908 RVA: 0x00399034 File Offset: 0x00397234
	public M08_Gameplay2_Component()
	{
	}

	// Token: 0x170007D2 RID: 2002
	// (get) Token: 0x0600C6DD RID: 50909 RVA: 0x00399048 File Offset: 0x00397248
	// (set) Token: 0x0600C6DE RID: 50910 RVA: 0x00399058 File Offset: 0x00397258
	public SpawnFactoryData Gaalsi_Artillery_Teams
	{
		get
		{
			return this.ExposedVariables.Gaalsi_Artillery_Teams;
		}
		set
		{
			this.ExposedVariables.Gaalsi_Artillery_Teams = value;
		}
	}

	// Token: 0x170007D3 RID: 2003
	// (get) Token: 0x0600C6DF RID: 50911 RVA: 0x00399068 File Offset: 0x00397268
	// (set) Token: 0x0600C6E0 RID: 50912 RVA: 0x00399078 File Offset: 0x00397278
	public QueryContainer G_ArtyMarkerQuery
	{
		get
		{
			return this.ExposedVariables.G_ArtyMarkerQuery;
		}
		set
		{
			this.ExposedVariables.G_ArtyMarkerQuery = value;
		}
	}

	// Token: 0x170007D4 RID: 2004
	// (get) Token: 0x0600C6E1 RID: 50913 RVA: 0x00399088 File Offset: 0x00397288
	// (set) Token: 0x0600C6E2 RID: 50914 RVA: 0x00399098 File Offset: 0x00397298
	public UnitSpawnWaveData Harass_1
	{
		get
		{
			return this.ExposedVariables.Harass_1;
		}
		set
		{
			this.ExposedVariables.Harass_1 = value;
		}
	}

	// Token: 0x170007D5 RID: 2005
	// (get) Token: 0x0600C6E3 RID: 50915 RVA: 0x003990A8 File Offset: 0x003972A8
	// (set) Token: 0x0600C6E4 RID: 50916 RVA: 0x003990B8 File Offset: 0x003972B8
	public UnitSpawnWaveData Harass_2
	{
		get
		{
			return this.ExposedVariables.Harass_2;
		}
		set
		{
			this.ExposedVariables.Harass_2 = value;
		}
	}

	// Token: 0x170007D6 RID: 2006
	// (get) Token: 0x0600C6E5 RID: 50917 RVA: 0x003990C8 File Offset: 0x003972C8
	// (set) Token: 0x0600C6E6 RID: 50918 RVA: 0x003990D8 File Offset: 0x003972D8
	public UnitSpawnWaveData Harass_Interceptor
	{
		get
		{
			return this.ExposedVariables.Harass_Interceptor;
		}
		set
		{
			this.ExposedVariables.Harass_Interceptor = value;
		}
	}

	// Token: 0x170007D7 RID: 2007
	// (get) Token: 0x0600C6E7 RID: 50919 RVA: 0x003990E8 File Offset: 0x003972E8
	// (set) Token: 0x0600C6E8 RID: 50920 RVA: 0x003990F8 File Offset: 0x003972F8
	public UnitSpawnWaveData GaalsienAA
	{
		get
		{
			return this.ExposedVariables.GaalsienAA;
		}
		set
		{
			this.ExposedVariables.GaalsienAA = value;
		}
	}

	// Token: 0x170007D8 RID: 2008
	// (get) Token: 0x0600C6E9 RID: 50921 RVA: 0x00399108 File Offset: 0x00397308
	// (set) Token: 0x0600C6EA RID: 50922 RVA: 0x00399118 File Offset: 0x00397318
	public QueryContainer G_Carrier_Query
	{
		get
		{
			return this.ExposedVariables.G_Carrier_Query;
		}
		set
		{
			this.ExposedVariables.G_Carrier_Query = value;
		}
	}

	// Token: 0x170007D9 RID: 2009
	// (get) Token: 0x0600C6EB RID: 50923 RVA: 0x00399128 File Offset: 0x00397328
	// (set) Token: 0x0600C6EC RID: 50924 RVA: 0x00399138 File Offset: 0x00397338
	public UnitSpawnWaveData Wave_RampDefenders
	{
		get
		{
			return this.ExposedVariables.Wave_RampDefenders;
		}
		set
		{
			this.ExposedVariables.Wave_RampDefenders = value;
		}
	}

	// Token: 0x170007DA RID: 2010
	// (get) Token: 0x0600C6ED RID: 50925 RVA: 0x00399148 File Offset: 0x00397348
	// (set) Token: 0x0600C6EE RID: 50926 RVA: 0x00399158 File Offset: 0x00397358
	public UnitSpawnWaveData Wave_PatrollerTier2
	{
		get
		{
			return this.ExposedVariables.Wave_PatrollerTier2;
		}
		set
		{
			this.ExposedVariables.Wave_PatrollerTier2 = value;
		}
	}

	// Token: 0x0600C6EF RID: 50927 RVA: 0x00399168 File Offset: 0x00397368
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

	// Token: 0x0600C6F0 RID: 50928 RVA: 0x003991D0 File Offset: 0x003973D0
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600C6F1 RID: 50929 RVA: 0x003991E0 File Offset: 0x003973E0
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600C6F2 RID: 50930 RVA: 0x003991F0 File Offset: 0x003973F0
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600C6F3 RID: 50931 RVA: 0x00399200 File Offset: 0x00397400
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600C6F4 RID: 50932 RVA: 0x00399210 File Offset: 0x00397410
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04013160 RID: 78176
	public M08_Gameplay2 ExposedVariables = new M08_Gameplay2();
}
