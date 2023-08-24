using System;
using BBI.Game.Data.Queries;
using UnityEngine;

// Token: 0x0200085C RID: 2140
[AddComponentMenu("uScript/Graphs/M07_Intel")]
public class M07_Intel_Component : uScriptCode
{
	// Token: 0x0600B562 RID: 46434 RVA: 0x003488D0 File Offset: 0x00346AD0
	public M07_Intel_Component()
	{
	}

	// Token: 0x1700078C RID: 1932
	// (get) Token: 0x0600B563 RID: 46435 RVA: 0x003488E4 File Offset: 0x00346AE4
	// (set) Token: 0x0600B564 RID: 46436 RVA: 0x003488F4 File Offset: 0x00346AF4
	public QueryContainer GaalsiCarrierQuery
	{
		get
		{
			return this.ExposedVariables.GaalsiCarrierQuery;
		}
		set
		{
			this.ExposedVariables.GaalsiCarrierQuery = value;
		}
	}

	// Token: 0x1700078D RID: 1933
	// (get) Token: 0x0600B565 RID: 46437 RVA: 0x00348904 File Offset: 0x00346B04
	// (set) Token: 0x0600B566 RID: 46438 RVA: 0x00348914 File Offset: 0x00346B14
	public bool bDebugStart
	{
		get
		{
			return this.ExposedVariables.bDebugStart;
		}
		set
		{
			this.ExposedVariables.bDebugStart = value;
		}
	}

	// Token: 0x1700078E RID: 1934
	// (get) Token: 0x0600B567 RID: 46439 RVA: 0x00348924 File Offset: 0x00346B24
	// (set) Token: 0x0600B568 RID: 46440 RVA: 0x00348934 File Offset: 0x00346B34
	public QueryContainer NISIntro_AllPlayerUnits
	{
		get
		{
			return this.ExposedVariables.NISIntro_AllPlayerUnits;
		}
		set
		{
			this.ExposedVariables.NISIntro_AllPlayerUnits = value;
		}
	}

	// Token: 0x1700078F RID: 1935
	// (get) Token: 0x0600B569 RID: 46441 RVA: 0x00348944 File Offset: 0x00346B44
	// (set) Token: 0x0600B56A RID: 46442 RVA: 0x00348954 File Offset: 0x00346B54
	public QueryContainer NISIntro_SuspendUnits
	{
		get
		{
			return this.ExposedVariables.NISIntro_SuspendUnits;
		}
		set
		{
			this.ExposedVariables.NISIntro_SuspendUnits = value;
		}
	}

	// Token: 0x17000790 RID: 1936
	// (get) Token: 0x0600B56B RID: 46443 RVA: 0x00348964 File Offset: 0x00346B64
	// (set) Token: 0x0600B56C RID: 46444 RVA: 0x00348974 File Offset: 0x00346B74
	public QueryContainer OutroNIS_SuspendUnitsQ
	{
		get
		{
			return this.ExposedVariables.OutroNIS_SuspendUnitsQ;
		}
		set
		{
			this.ExposedVariables.OutroNIS_SuspendUnitsQ = value;
		}
	}

	// Token: 0x17000791 RID: 1937
	// (get) Token: 0x0600B56D RID: 46445 RVA: 0x00348984 File Offset: 0x00346B84
	// (set) Token: 0x0600B56E RID: 46446 RVA: 0x00348994 File Offset: 0x00346B94
	public QueryContainer OutroNIS_AllUnitsSuspendQ
	{
		get
		{
			return this.ExposedVariables.OutroNIS_AllUnitsSuspendQ;
		}
		set
		{
			this.ExposedVariables.OutroNIS_AllUnitsSuspendQ = value;
		}
	}

	// Token: 0x17000792 RID: 1938
	// (get) Token: 0x0600B56F RID: 46447 RVA: 0x003489A4 File Offset: 0x00346BA4
	// (set) Token: 0x0600B570 RID: 46448 RVA: 0x003489B4 File Offset: 0x00346BB4
	public QueryContainer NIS2_AllPlayerUnitsQ
	{
		get
		{
			return this.ExposedVariables.NIS2_AllPlayerUnitsQ;
		}
		set
		{
			this.ExposedVariables.NIS2_AllPlayerUnitsQ = value;
		}
	}

	// Token: 0x17000793 RID: 1939
	// (get) Token: 0x0600B571 RID: 46449 RVA: 0x003489C4 File Offset: 0x00346BC4
	// (set) Token: 0x0600B572 RID: 46450 RVA: 0x003489D4 File Offset: 0x00346BD4
	public QueryContainer NIS02_SuspendUnitsQ
	{
		get
		{
			return this.ExposedVariables.NIS02_SuspendUnitsQ;
		}
		set
		{
			this.ExposedVariables.NIS02_SuspendUnitsQ = value;
		}
	}

	// Token: 0x17000794 RID: 1940
	// (get) Token: 0x0600B573 RID: 46451 RVA: 0x003489E4 File Offset: 0x00346BE4
	// (set) Token: 0x0600B574 RID: 46452 RVA: 0x003489F4 File Offset: 0x00346BF4
	public QueryContainer PlayerCarrierQuery
	{
		get
		{
			return this.ExposedVariables.PlayerCarrierQuery;
		}
		set
		{
			this.ExposedVariables.PlayerCarrierQuery = value;
		}
	}

	// Token: 0x17000795 RID: 1941
	// (get) Token: 0x0600B575 RID: 46453 RVA: 0x00348A04 File Offset: 0x00346C04
	// (set) Token: 0x0600B576 RID: 46454 RVA: 0x00348A14 File Offset: 0x00346C14
	public QueryContainer NIS3_SuspendUnitsQ
	{
		get
		{
			return this.ExposedVariables.NIS3_SuspendUnitsQ;
		}
		set
		{
			this.ExposedVariables.NIS3_SuspendUnitsQ = value;
		}
	}

	// Token: 0x17000796 RID: 1942
	// (get) Token: 0x0600B577 RID: 46455 RVA: 0x00348A24 File Offset: 0x00346C24
	// (set) Token: 0x0600B578 RID: 46456 RVA: 0x00348A34 File Offset: 0x00346C34
	public QueryContainer PlayerRachelQuery
	{
		get
		{
			return this.ExposedVariables.PlayerRachelQuery;
		}
		set
		{
			this.ExposedVariables.PlayerRachelQuery = value;
		}
	}

	// Token: 0x17000797 RID: 1943
	// (get) Token: 0x0600B579 RID: 46457 RVA: 0x00348A44 File Offset: 0x00346C44
	// (set) Token: 0x0600B57A RID: 46458 RVA: 0x00348A54 File Offset: 0x00346C54
	public QueryContainer GaalsiFleetQuery
	{
		get
		{
			return this.ExposedVariables.GaalsiFleetQuery;
		}
		set
		{
			this.ExposedVariables.GaalsiFleetQuery = value;
		}
	}

	// Token: 0x17000798 RID: 1944
	// (get) Token: 0x0600B57B RID: 46459 RVA: 0x00348A64 File Offset: 0x00346C64
	// (set) Token: 0x0600B57C RID: 46460 RVA: 0x00348A74 File Offset: 0x00346C74
	public QueryContainer OutroNIS_AllGodUnitsQ
	{
		get
		{
			return this.ExposedVariables.OutroNIS_AllGodUnitsQ;
		}
		set
		{
			this.ExposedVariables.OutroNIS_AllGodUnitsQ = value;
		}
	}

	// Token: 0x17000799 RID: 1945
	// (get) Token: 0x0600B57D RID: 46461 RVA: 0x00348A84 File Offset: 0x00346C84
	// (set) Token: 0x0600B57E RID: 46462 RVA: 0x00348A94 File Offset: 0x00346C94
	public QueryContainer MissionFail_AllUnitsSuspendQ
	{
		get
		{
			return this.ExposedVariables.MissionFail_AllUnitsSuspendQ;
		}
		set
		{
			this.ExposedVariables.MissionFail_AllUnitsSuspendQ = value;
		}
	}

	// Token: 0x1700079A RID: 1946
	// (get) Token: 0x0600B57F RID: 46463 RVA: 0x00348AA4 File Offset: 0x00346CA4
	// (set) Token: 0x0600B580 RID: 46464 RVA: 0x00348AB4 File Offset: 0x00346CB4
	public GameObject NIS02CamGO
	{
		get
		{
			return this.ExposedVariables.NIS02CamGO;
		}
		set
		{
			this.ExposedVariables.NIS02CamGO = value;
		}
	}

	// Token: 0x1700079B RID: 1947
	// (get) Token: 0x0600B581 RID: 46465 RVA: 0x00348AC4 File Offset: 0x00346CC4
	// (set) Token: 0x0600B582 RID: 46466 RVA: 0x00348AD4 File Offset: 0x00346CD4
	public GameObject NIS03CamGO
	{
		get
		{
			return this.ExposedVariables.NIS03CamGO;
		}
		set
		{
			this.ExposedVariables.NIS03CamGO = value;
		}
	}

	// Token: 0x1700079C RID: 1948
	// (get) Token: 0x0600B583 RID: 46467 RVA: 0x00348AE4 File Offset: 0x00346CE4
	// (set) Token: 0x0600B584 RID: 46468 RVA: 0x00348AF4 File Offset: 0x00346CF4
	public GameObject NIS01CamGO
	{
		get
		{
			return this.ExposedVariables.NIS01CamGO;
		}
		set
		{
			this.ExposedVariables.NIS01CamGO = value;
		}
	}

	// Token: 0x1700079D RID: 1949
	// (get) Token: 0x0600B585 RID: 46469 RVA: 0x00348B04 File Offset: 0x00346D04
	// (set) Token: 0x0600B586 RID: 46470 RVA: 0x00348B14 File Offset: 0x00346D14
	public QueryContainer AllGodUnitsQ
	{
		get
		{
			return this.ExposedVariables.AllGodUnitsQ;
		}
		set
		{
			this.ExposedVariables.AllGodUnitsQ = value;
		}
	}

	// Token: 0x1700079E RID: 1950
	// (get) Token: 0x0600B587 RID: 46471 RVA: 0x00348B24 File Offset: 0x00346D24
	// (set) Token: 0x0600B588 RID: 46472 RVA: 0x00348B34 File Offset: 0x00346D34
	public QueryContainer NIS3_AllPlayerUnitsQ
	{
		get
		{
			return this.ExposedVariables.NIS3_AllPlayerUnitsQ;
		}
		set
		{
			this.ExposedVariables.NIS3_AllPlayerUnitsQ = value;
		}
	}

	// Token: 0x0600B589 RID: 46473 RVA: 0x00348B44 File Offset: 0x00346D44
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

	// Token: 0x0600B58A RID: 46474 RVA: 0x00348BAC File Offset: 0x00346DAC
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600B58B RID: 46475 RVA: 0x00348BBC File Offset: 0x00346DBC
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600B58C RID: 46476 RVA: 0x00348BCC File Offset: 0x00346DCC
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600B58D RID: 46477 RVA: 0x00348BDC File Offset: 0x00346DDC
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600B58E RID: 46478 RVA: 0x00348BEC File Offset: 0x00346DEC
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x040115E3 RID: 71139
	public M07_Intel ExposedVariables = new M07_Intel();
}
