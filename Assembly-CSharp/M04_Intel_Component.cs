using System;
using BBI.Game.Data.Queries;
using UnityEngine;

// Token: 0x0200082C RID: 2092
[AddComponentMenu("uScript/Graphs/M04_Intel")]
public class M04_Intel_Component : uScriptCode
{
	// Token: 0x06007FD8 RID: 32728 RVA: 0x002451B4 File Offset: 0x002433B4
	public M04_Intel_Component()
	{
	}

	// Token: 0x17000696 RID: 1686
	// (get) Token: 0x06007FD9 RID: 32729 RVA: 0x002451C8 File Offset: 0x002433C8
	// (set) Token: 0x06007FDA RID: 32730 RVA: 0x002451D8 File Offset: 0x002433D8
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

	// Token: 0x17000697 RID: 1687
	// (get) Token: 0x06007FDB RID: 32731 RVA: 0x002451E8 File Offset: 0x002433E8
	// (set) Token: 0x06007FDC RID: 32732 RVA: 0x002451F8 File Offset: 0x002433F8
	public QueryContainer EnemyUnits
	{
		get
		{
			return this.ExposedVariables.EnemyUnits;
		}
		set
		{
			this.ExposedVariables.EnemyUnits = value;
		}
	}

	// Token: 0x17000698 RID: 1688
	// (get) Token: 0x06007FDD RID: 32733 RVA: 0x00245208 File Offset: 0x00243408
	// (set) Token: 0x06007FDE RID: 32734 RVA: 0x00245218 File Offset: 0x00243418
	public QueryContainer Carrier
	{
		get
		{
			return this.ExposedVariables.Carrier;
		}
		set
		{
			this.ExposedVariables.Carrier = value;
		}
	}

	// Token: 0x17000699 RID: 1689
	// (get) Token: 0x06007FDF RID: 32735 RVA: 0x00245228 File Offset: 0x00243428
	// (set) Token: 0x06007FE0 RID: 32736 RVA: 0x00245238 File Offset: 0x00243438
	public QueryContainer SuspendFleetLessKapisi
	{
		get
		{
			return this.ExposedVariables.SuspendFleetLessKapisi;
		}
		set
		{
			this.ExposedVariables.SuspendFleetLessKapisi = value;
		}
	}

	// Token: 0x1700069A RID: 1690
	// (get) Token: 0x06007FE1 RID: 32737 RVA: 0x00245248 File Offset: 0x00243448
	// (set) Token: 0x06007FE2 RID: 32738 RVA: 0x00245258 File Offset: 0x00243458
	public QueryContainer RachelSearch
	{
		get
		{
			return this.ExposedVariables.RachelSearch;
		}
		set
		{
			this.ExposedVariables.RachelSearch = value;
		}
	}

	// Token: 0x1700069B RID: 1691
	// (get) Token: 0x06007FE3 RID: 32739 RVA: 0x00245268 File Offset: 0x00243468
	// (set) Token: 0x06007FE4 RID: 32740 RVA: 0x00245278 File Offset: 0x00243478
	public QueryContainer KapisiQ
	{
		get
		{
			return this.ExposedVariables.KapisiQ;
		}
		set
		{
			this.ExposedVariables.KapisiQ = value;
		}
	}

	// Token: 0x06007FE5 RID: 32741 RVA: 0x00245288 File Offset: 0x00243488
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

	// Token: 0x06007FE6 RID: 32742 RVA: 0x002452F0 File Offset: 0x002434F0
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06007FE7 RID: 32743 RVA: 0x00245300 File Offset: 0x00243500
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06007FE8 RID: 32744 RVA: 0x00245310 File Offset: 0x00243510
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06007FE9 RID: 32745 RVA: 0x00245320 File Offset: 0x00243520
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06007FEA RID: 32746 RVA: 0x00245330 File Offset: 0x00243530
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400B7B3 RID: 47027
	public M04_Intel ExposedVariables = new M04_Intel();
}
