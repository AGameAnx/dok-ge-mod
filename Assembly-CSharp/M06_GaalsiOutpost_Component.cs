using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using UnityEngine;

// Token: 0x0200084E RID: 2126
[AddComponentMenu("uScript/Graphs/M06_GaalsiOutpost")]
public class M06_GaalsiOutpost_Component : uScriptCode
{
	// Token: 0x06009BE7 RID: 39911 RVA: 0x002CCE68 File Offset: 0x002CB068
	public M06_GaalsiOutpost_Component()
	{
	}

	// Token: 0x1700070D RID: 1805
	// (get) Token: 0x06009BE8 RID: 39912 RVA: 0x002CCE7C File Offset: 0x002CB07C
	// (set) Token: 0x06009BE9 RID: 39913 RVA: 0x002CCE8C File Offset: 0x002CB08C
	public UnitSpawnWaveData GaalsiCatGroup
	{
		get
		{
			return this.ExposedVariables.GaalsiCatGroup;
		}
		set
		{
			this.ExposedVariables.GaalsiCatGroup = value;
		}
	}

	// Token: 0x1700070E RID: 1806
	// (get) Token: 0x06009BEA RID: 39914 RVA: 0x002CCE9C File Offset: 0x002CB09C
	// (set) Token: 0x06009BEB RID: 39915 RVA: 0x002CCEAC File Offset: 0x002CB0AC
	public UnitSpawnWaveData HarvesterWave
	{
		get
		{
			return this.ExposedVariables.HarvesterWave;
		}
		set
		{
			this.ExposedVariables.HarvesterWave = value;
		}
	}

	// Token: 0x1700070F RID: 1807
	// (get) Token: 0x06009BEC RID: 39916 RVA: 0x002CCEBC File Offset: 0x002CB0BC
	// (set) Token: 0x06009BED RID: 39917 RVA: 0x002CCECC File Offset: 0x002CB0CC
	public UnitSpawnWaveData GaalsiSupportCruiserWave
	{
		get
		{
			return this.ExposedVariables.GaalsiSupportCruiserWave;
		}
		set
		{
			this.ExposedVariables.GaalsiSupportCruiserWave = value;
		}
	}

	// Token: 0x17000710 RID: 1808
	// (get) Token: 0x06009BEE RID: 39918 RVA: 0x002CCEDC File Offset: 0x002CB0DC
	// (set) Token: 0x06009BEF RID: 39919 RVA: 0x002CCEEC File Offset: 0x002CB0EC
	public UnitSpawnWaveData TypeSandskimmer
	{
		get
		{
			return this.ExposedVariables.TypeSandskimmer;
		}
		set
		{
			this.ExposedVariables.TypeSandskimmer = value;
		}
	}

	// Token: 0x17000711 RID: 1809
	// (get) Token: 0x06009BF0 RID: 39920 RVA: 0x002CCEFC File Offset: 0x002CB0FC
	// (set) Token: 0x06009BF1 RID: 39921 RVA: 0x002CCF0C File Offset: 0x002CB10C
	public QueryContainer GaalsiShipbreakersQueryAll
	{
		get
		{
			return this.ExposedVariables.GaalsiShipbreakersQueryAll;
		}
		set
		{
			this.ExposedVariables.GaalsiShipbreakersQueryAll = value;
		}
	}

	// Token: 0x17000712 RID: 1810
	// (get) Token: 0x06009BF2 RID: 39922 RVA: 0x002CCF1C File Offset: 0x002CB11C
	// (set) Token: 0x06009BF3 RID: 39923 RVA: 0x002CCF2C File Offset: 0x002CB12C
	public bool bGaalsiShipbreakerFallbackHappend
	{
		get
		{
			return this.ExposedVariables.bGaalsiShipbreakerFallbackHappend;
		}
		set
		{
			this.ExposedVariables.bGaalsiShipbreakerFallbackHappend = value;
		}
	}

	// Token: 0x17000713 RID: 1811
	// (get) Token: 0x06009BF4 RID: 39924 RVA: 0x002CCF3C File Offset: 0x002CB13C
	// (set) Token: 0x06009BF5 RID: 39925 RVA: 0x002CCF4C File Offset: 0x002CB14C
	public QueryContainer GaalsiShipbreakersQuery1
	{
		get
		{
			return this.ExposedVariables.GaalsiShipbreakersQuery1;
		}
		set
		{
			this.ExposedVariables.GaalsiShipbreakersQuery1 = value;
		}
	}

	// Token: 0x17000714 RID: 1812
	// (get) Token: 0x06009BF6 RID: 39926 RVA: 0x002CCF5C File Offset: 0x002CB15C
	// (set) Token: 0x06009BF7 RID: 39927 RVA: 0x002CCF6C File Offset: 0x002CB16C
	public QueryContainer GaalsiShipbreakersQuery2
	{
		get
		{
			return this.ExposedVariables.GaalsiShipbreakersQuery2;
		}
		set
		{
			this.ExposedVariables.GaalsiShipbreakersQuery2 = value;
		}
	}

	// Token: 0x17000715 RID: 1813
	// (get) Token: 0x06009BF8 RID: 39928 RVA: 0x002CCF7C File Offset: 0x002CB17C
	// (set) Token: 0x06009BF9 RID: 39929 RVA: 0x002CCF8C File Offset: 0x002CB18C
	public QueryContainer GaalsiShipbreakersQuery3
	{
		get
		{
			return this.ExposedVariables.GaalsiShipbreakersQuery3;
		}
		set
		{
			this.ExposedVariables.GaalsiShipbreakersQuery3 = value;
		}
	}

	// Token: 0x17000716 RID: 1814
	// (get) Token: 0x06009BFA RID: 39930 RVA: 0x002CCF9C File Offset: 0x002CB19C
	// (set) Token: 0x06009BFB RID: 39931 RVA: 0x002CCFAC File Offset: 0x002CB1AC
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

	// Token: 0x17000717 RID: 1815
	// (get) Token: 0x06009BFC RID: 39932 RVA: 0x002CCFBC File Offset: 0x002CB1BC
	// (set) Token: 0x06009BFD RID: 39933 RVA: 0x002CCFCC File Offset: 0x002CB1CC
	public UnitSpawnWaveData PaltryGaalsienSpawn
	{
		get
		{
			return this.ExposedVariables.PaltryGaalsienSpawn;
		}
		set
		{
			this.ExposedVariables.PaltryGaalsienSpawn = value;
		}
	}

	// Token: 0x17000718 RID: 1816
	// (get) Token: 0x06009BFE RID: 39934 RVA: 0x002CCFDC File Offset: 0x002CB1DC
	// (set) Token: 0x06009BFF RID: 39935 RVA: 0x002CCFEC File Offset: 0x002CB1EC
	public QueryContainer QueryPlayerCombatUnits
	{
		get
		{
			return this.ExposedVariables.QueryPlayerCombatUnits;
		}
		set
		{
			this.ExposedVariables.QueryPlayerCombatUnits = value;
		}
	}

	// Token: 0x17000719 RID: 1817
	// (get) Token: 0x06009C00 RID: 39936 RVA: 0x002CCFFC File Offset: 0x002CB1FC
	// (set) Token: 0x06009C01 RID: 39937 RVA: 0x002CD00C File Offset: 0x002CB20C
	public bool BasePatrolDestroyed
	{
		get
		{
			return this.ExposedVariables.BasePatrolDestroyed;
		}
		set
		{
			this.ExposedVariables.BasePatrolDestroyed = value;
		}
	}

	// Token: 0x06009C02 RID: 39938 RVA: 0x002CD01C File Offset: 0x002CB21C
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

	// Token: 0x06009C03 RID: 39939 RVA: 0x002CD084 File Offset: 0x002CB284
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x06009C04 RID: 39940 RVA: 0x002CD094 File Offset: 0x002CB294
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06009C05 RID: 39941 RVA: 0x002CD0A4 File Offset: 0x002CB2A4
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06009C06 RID: 39942 RVA: 0x002CD0B4 File Offset: 0x002CB2B4
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x06009C07 RID: 39943 RVA: 0x002CD0C4 File Offset: 0x002CB2C4
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400E810 RID: 59408
	public M06_GaalsiOutpost ExposedVariables = new M06_GaalsiOutpost();
}
