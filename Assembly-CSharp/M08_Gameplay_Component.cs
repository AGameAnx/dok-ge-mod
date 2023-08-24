using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Scripting;
using UnityEngine;

// Token: 0x02000866 RID: 2150
[AddComponentMenu("uScript/Graphs/M08_Gameplay")]
public class M08_Gameplay_Component : uScriptCode
{
	// Token: 0x0600C6F5 RID: 50933 RVA: 0x00399220 File Offset: 0x00397420
	public M08_Gameplay_Component()
	{
	}

	// Token: 0x170007DB RID: 2011
	// (get) Token: 0x0600C6F6 RID: 50934 RVA: 0x00399234 File Offset: 0x00397434
	// (set) Token: 0x0600C6F7 RID: 50935 RVA: 0x00399244 File Offset: 0x00397444
	public int Wreck_Commander
	{
		get
		{
			return this.ExposedVariables.Wreck_Commander;
		}
		set
		{
			this.ExposedVariables.Wreck_Commander = value;
		}
	}

	// Token: 0x170007DC RID: 2012
	// (get) Token: 0x0600C6F8 RID: 50936 RVA: 0x00399254 File Offset: 0x00397454
	// (set) Token: 0x0600C6F9 RID: 50937 RVA: 0x00399264 File Offset: 0x00397464
	public SpawnFactoryData WreckStrike_Repeater
	{
		get
		{
			return this.ExposedVariables.WreckStrike_Repeater;
		}
		set
		{
			this.ExposedVariables.WreckStrike_Repeater = value;
		}
	}

	// Token: 0x170007DD RID: 2013
	// (get) Token: 0x0600C6FA RID: 50938 RVA: 0x00399274 File Offset: 0x00397474
	// (set) Token: 0x0600C6FB RID: 50939 RVA: 0x00399284 File Offset: 0x00397484
	public SpawnFactoryData WreckStrike_Repeater2
	{
		get
		{
			return this.ExposedVariables.WreckStrike_Repeater2;
		}
		set
		{
			this.ExposedVariables.WreckStrike_Repeater2 = value;
		}
	}

	// Token: 0x170007DE RID: 2014
	// (get) Token: 0x0600C6FC RID: 50940 RVA: 0x00399294 File Offset: 0x00397494
	// (set) Token: 0x0600C6FD RID: 50941 RVA: 0x003992A4 File Offset: 0x003974A4
	public SpawnFactoryData WreckStrike_Repeater3
	{
		get
		{
			return this.ExposedVariables.WreckStrike_Repeater3;
		}
		set
		{
			this.ExposedVariables.WreckStrike_Repeater3 = value;
		}
	}

	// Token: 0x170007DF RID: 2015
	// (get) Token: 0x0600C6FE RID: 50942 RVA: 0x003992B4 File Offset: 0x003974B4
	// (set) Token: 0x0600C6FF RID: 50943 RVA: 0x003992C4 File Offset: 0x003974C4
	public SpawnFactoryData WreckStrike_Repeater4
	{
		get
		{
			return this.ExposedVariables.WreckStrike_Repeater4;
		}
		set
		{
			this.ExposedVariables.WreckStrike_Repeater4 = value;
		}
	}

	// Token: 0x170007E0 RID: 2016
	// (get) Token: 0x0600C700 RID: 50944 RVA: 0x003992D4 File Offset: 0x003974D4
	// (set) Token: 0x0600C701 RID: 50945 RVA: 0x003992E4 File Offset: 0x003974E4
	public QueryContainer Player_Salvagers
	{
		get
		{
			return this.ExposedVariables.Player_Salvagers;
		}
		set
		{
			this.ExposedVariables.Player_Salvagers = value;
		}
	}

	// Token: 0x170007E1 RID: 2017
	// (get) Token: 0x0600C702 RID: 50946 RVA: 0x003992F4 File Offset: 0x003974F4
	// (set) Token: 0x0600C703 RID: 50947 RVA: 0x00399304 File Offset: 0x00397504
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

	// Token: 0x170007E2 RID: 2018
	// (get) Token: 0x0600C704 RID: 50948 RVA: 0x00399314 File Offset: 0x00397514
	// (set) Token: 0x0600C705 RID: 50949 RVA: 0x00399324 File Offset: 0x00397524
	public QueryContainer APU
	{
		get
		{
			return this.ExposedVariables.APU;
		}
		set
		{
			this.ExposedVariables.APU = value;
		}
	}

	// Token: 0x170007E3 RID: 2019
	// (get) Token: 0x0600C706 RID: 50950 RVA: 0x00399334 File Offset: 0x00397534
	// (set) Token: 0x0600C707 RID: 50951 RVA: 0x00399344 File Offset: 0x00397544
	public string Artifact_Label
	{
		get
		{
			return this.ExposedVariables.Artifact_Label;
		}
		set
		{
			this.ExposedVariables.Artifact_Label = value;
		}
	}

	// Token: 0x170007E4 RID: 2020
	// (get) Token: 0x0600C708 RID: 50952 RVA: 0x00399354 File Offset: 0x00397554
	// (set) Token: 0x0600C709 RID: 50953 RVA: 0x00399364 File Offset: 0x00397564
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

	// Token: 0x170007E5 RID: 2021
	// (get) Token: 0x0600C70A RID: 50954 RVA: 0x00399374 File Offset: 0x00397574
	// (set) Token: 0x0600C70B RID: 50955 RVA: 0x00399384 File Offset: 0x00397584
	public QueryContainer Artifact_Query
	{
		get
		{
			return this.ExposedVariables.Artifact_Query;
		}
		set
		{
			this.ExposedVariables.Artifact_Query = value;
		}
	}

	// Token: 0x170007E6 RID: 2022
	// (get) Token: 0x0600C70C RID: 50956 RVA: 0x00399394 File Offset: 0x00397594
	// (set) Token: 0x0600C70D RID: 50957 RVA: 0x003993A4 File Offset: 0x003975A4
	public UnitSpawnWaveData Art_Pick_Crew
	{
		get
		{
			return this.ExposedVariables.Art_Pick_Crew;
		}
		set
		{
			this.ExposedVariables.Art_Pick_Crew = value;
		}
	}

	// Token: 0x170007E7 RID: 2023
	// (get) Token: 0x0600C70E RID: 50958 RVA: 0x003993B4 File Offset: 0x003975B4
	// (set) Token: 0x0600C70F RID: 50959 RVA: 0x003993C4 File Offset: 0x003975C4
	public QueryContainer SupportC_Query
	{
		get
		{
			return this.ExposedVariables.SupportC_Query;
		}
		set
		{
			this.ExposedVariables.SupportC_Query = value;
		}
	}

	// Token: 0x170007E8 RID: 2024
	// (get) Token: 0x0600C710 RID: 50960 RVA: 0x003993D4 File Offset: 0x003975D4
	// (set) Token: 0x0600C711 RID: 50961 RVA: 0x003993E4 File Offset: 0x003975E4
	public QueryContainer SUSP_ALLCOMMANDERS
	{
		get
		{
			return this.ExposedVariables.SUSP_ALLCOMMANDERS;
		}
		set
		{
			this.ExposedVariables.SUSP_ALLCOMMANDERS = value;
		}
	}

	// Token: 0x170007E9 RID: 2025
	// (get) Token: 0x0600C712 RID: 50962 RVA: 0x003993F4 File Offset: 0x003975F4
	// (set) Token: 0x0600C713 RID: 50963 RVA: 0x00399404 File Offset: 0x00397604
	public QueryContainer KapisiQuery
	{
		get
		{
			return this.ExposedVariables.KapisiQuery;
		}
		set
		{
			this.ExposedVariables.KapisiQuery = value;
		}
	}

	// Token: 0x170007EA RID: 2026
	// (get) Token: 0x0600C714 RID: 50964 RVA: 0x00399414 File Offset: 0x00397614
	// (set) Token: 0x0600C715 RID: 50965 RVA: 0x00399424 File Offset: 0x00397624
	public UnitSpawnWaveData ArtCrewEscorts
	{
		get
		{
			return this.ExposedVariables.ArtCrewEscorts;
		}
		set
		{
			this.ExposedVariables.ArtCrewEscorts = value;
		}
	}

	// Token: 0x0600C716 RID: 50966 RVA: 0x00399434 File Offset: 0x00397634
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

	// Token: 0x0600C717 RID: 50967 RVA: 0x0039949C File Offset: 0x0039769C
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600C718 RID: 50968 RVA: 0x003994AC File Offset: 0x003976AC
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600C719 RID: 50969 RVA: 0x003994BC File Offset: 0x003976BC
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600C71A RID: 50970 RVA: 0x003994CC File Offset: 0x003976CC
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600C71B RID: 50971 RVA: 0x003994DC File Offset: 0x003976DC
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04013161 RID: 78177
	public M08_Gameplay ExposedVariables = new M08_Gameplay();
}
