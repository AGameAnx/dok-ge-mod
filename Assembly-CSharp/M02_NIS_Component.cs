using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x02000818 RID: 2072
[AddComponentMenu("uScript/Graphs/M02_NIS")]
public class M02_NIS_Component : uScriptCode
{
	// Token: 0x06005325 RID: 21285 RVA: 0x0017089C File Offset: 0x0016EA9C
	public M02_NIS_Component()
	{
	}

	// Token: 0x1700060B RID: 1547
	// (get) Token: 0x06005326 RID: 21286 RVA: 0x001708B0 File Offset: 0x0016EAB0
	// (set) Token: 0x06005327 RID: 21287 RVA: 0x001708C0 File Offset: 0x0016EAC0
	public UnitSpawnWaveData NISRachelWave
	{
		get
		{
			return this.ExposedVariables.NISRachelWave;
		}
		set
		{
			this.ExposedVariables.NISRachelWave = value;
		}
	}

	// Token: 0x1700060C RID: 1548
	// (get) Token: 0x06005328 RID: 21288 RVA: 0x001708D0 File Offset: 0x0016EAD0
	// (set) Token: 0x06005329 RID: 21289 RVA: 0x001708E0 File Offset: 0x0016EAE0
	public UnitSpawnWaveData NISSandskimmerWave
	{
		get
		{
			return this.ExposedVariables.NISSandskimmerWave;
		}
		set
		{
			this.ExposedVariables.NISSandskimmerWave = value;
		}
	}

	// Token: 0x1700060D RID: 1549
	// (get) Token: 0x0600532A RID: 21290 RVA: 0x001708F0 File Offset: 0x0016EAF0
	// (set) Token: 0x0600532B RID: 21291 RVA: 0x00170900 File Offset: 0x0016EB00
	public QueryContainer TurretQuery
	{
		get
		{
			return this.ExposedVariables.TurretQuery;
		}
		set
		{
			this.ExposedVariables.TurretQuery = value;
		}
	}

	// Token: 0x1700060E RID: 1550
	// (get) Token: 0x0600532C RID: 21292 RVA: 0x00170910 File Offset: 0x0016EB10
	// (set) Token: 0x0600532D RID: 21293 RVA: 0x00170920 File Offset: 0x0016EB20
	public AttributeBuffSetData NISletNoFireBuff
	{
		get
		{
			return this.ExposedVariables.NISletNoFireBuff;
		}
		set
		{
			this.ExposedVariables.NISletNoFireBuff = value;
		}
	}

	// Token: 0x1700060F RID: 1551
	// (get) Token: 0x0600532E RID: 21294 RVA: 0x00170930 File Offset: 0x0016EB30
	// (set) Token: 0x0600532F RID: 21295 RVA: 0x00170940 File Offset: 0x0016EB40
	public AttributeBuffSetData CoalitionHACNoCDBuff
	{
		get
		{
			return this.ExposedVariables.CoalitionHACNoCDBuff;
		}
		set
		{
			this.ExposedVariables.CoalitionHACNoCDBuff = value;
		}
	}

	// Token: 0x17000610 RID: 1552
	// (get) Token: 0x06005330 RID: 21296 RVA: 0x00170950 File Offset: 0x0016EB50
	// (set) Token: 0x06005331 RID: 21297 RVA: 0x00170960 File Offset: 0x0016EB60
	public QueryContainer NIS02TurretQuery
	{
		get
		{
			return this.ExposedVariables.NIS02TurretQuery;
		}
		set
		{
			this.ExposedVariables.NIS02TurretQuery = value;
		}
	}

	// Token: 0x17000611 RID: 1553
	// (get) Token: 0x06005332 RID: 21298 RVA: 0x00170970 File Offset: 0x0016EB70
	// (set) Token: 0x06005333 RID: 21299 RVA: 0x00170980 File Offset: 0x0016EB80
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

	// Token: 0x17000612 RID: 1554
	// (get) Token: 0x06005334 RID: 21300 RVA: 0x00170990 File Offset: 0x0016EB90
	// (set) Token: 0x06005335 RID: 21301 RVA: 0x001709A0 File Offset: 0x0016EBA0
	public QueryContainer BSEmptyQuery
	{
		get
		{
			return this.ExposedVariables.BSEmptyQuery;
		}
		set
		{
			this.ExposedVariables.BSEmptyQuery = value;
		}
	}

	// Token: 0x17000613 RID: 1555
	// (get) Token: 0x06005336 RID: 21302 RVA: 0x001709B0 File Offset: 0x0016EBB0
	// (set) Token: 0x06005337 RID: 21303 RVA: 0x001709C0 File Offset: 0x0016EBC0
	public QueryContainer KapisiSuspendQuery
	{
		get
		{
			return this.ExposedVariables.KapisiSuspendQuery;
		}
		set
		{
			this.ExposedVariables.KapisiSuspendQuery = value;
		}
	}

	// Token: 0x17000614 RID: 1556
	// (get) Token: 0x06005338 RID: 21304 RVA: 0x001709D0 File Offset: 0x0016EBD0
	// (set) Token: 0x06005339 RID: 21305 RVA: 0x001709E0 File Offset: 0x0016EBE0
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

	// Token: 0x17000615 RID: 1557
	// (get) Token: 0x0600533A RID: 21306 RVA: 0x001709F0 File Offset: 0x0016EBF0
	// (set) Token: 0x0600533B RID: 21307 RVA: 0x00170A00 File Offset: 0x0016EC00
	public QueryContainer AllGaalsiQuery
	{
		get
		{
			return this.ExposedVariables.AllGaalsiQuery;
		}
		set
		{
			this.ExposedVariables.AllGaalsiQuery = value;
		}
	}

	// Token: 0x17000616 RID: 1558
	// (get) Token: 0x0600533C RID: 21308 RVA: 0x00170A10 File Offset: 0x0016EC10
	// (set) Token: 0x0600533D RID: 21309 RVA: 0x00170A20 File Offset: 0x0016EC20
	public GameObject NIS02FinalCam
	{
		get
		{
			return this.ExposedVariables.NIS02FinalCam;
		}
		set
		{
			this.ExposedVariables.NIS02FinalCam = value;
		}
	}

	// Token: 0x0600533E RID: 21310 RVA: 0x00170A30 File Offset: 0x0016EC30
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

	// Token: 0x0600533F RID: 21311 RVA: 0x00170A98 File Offset: 0x0016EC98
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x06005340 RID: 21312 RVA: 0x00170AA8 File Offset: 0x0016ECA8
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06005341 RID: 21313 RVA: 0x00170AB8 File Offset: 0x0016ECB8
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06005342 RID: 21314 RVA: 0x00170AC8 File Offset: 0x0016ECC8
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x06005343 RID: 21315 RVA: 0x00170AD8 File Offset: 0x0016ECD8
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04007044 RID: 28740
	public M02_NIS ExposedVariables = new M02_NIS();
}
