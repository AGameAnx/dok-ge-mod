using System;
using BBI.Game.Data.Queries;
using UnityEngine;

// Token: 0x02000840 RID: 2112
[AddComponentMenu("uScript/Graphs/M05_Intel")]
public class M05_Intel_Component : uScriptCode
{
	// Token: 0x06009064 RID: 36964 RVA: 0x00297414 File Offset: 0x00295614
	public M05_Intel_Component()
	{
	}

	// Token: 0x170006DD RID: 1757
	// (get) Token: 0x06009065 RID: 36965 RVA: 0x00297428 File Offset: 0x00295628
	// (set) Token: 0x06009066 RID: 36966 RVA: 0x00297438 File Offset: 0x00295638
	public float Radius
	{
		get
		{
			return this.ExposedVariables.Radius;
		}
		set
		{
			this.ExposedVariables.Radius = value;
		}
	}

	// Token: 0x170006DE RID: 1758
	// (get) Token: 0x06009067 RID: 36967 RVA: 0x00297448 File Offset: 0x00295648
	// (set) Token: 0x06009068 RID: 36968 RVA: 0x00297458 File Offset: 0x00295658
	public QueryContainer Rachel_check
	{
		get
		{
			return this.ExposedVariables.Rachel_check;
		}
		set
		{
			this.ExposedVariables.Rachel_check = value;
		}
	}

	// Token: 0x170006DF RID: 1759
	// (get) Token: 0x06009069 RID: 36969 RVA: 0x00297468 File Offset: 0x00295668
	// (set) Token: 0x0600906A RID: 36970 RVA: 0x00297478 File Offset: 0x00295678
	public int Charges_Deployed
	{
		get
		{
			return this.ExposedVariables.Charges_Deployed;
		}
		set
		{
			this.ExposedVariables.Charges_Deployed = value;
		}
	}

	// Token: 0x170006E0 RID: 1760
	// (get) Token: 0x0600906B RID: 36971 RVA: 0x00297488 File Offset: 0x00295688
	// (set) Token: 0x0600906C RID: 36972 RVA: 0x00297498 File Offset: 0x00295698
	public QueryContainer DemoChargeQuery
	{
		get
		{
			return this.ExposedVariables.DemoChargeQuery;
		}
		set
		{
			this.ExposedVariables.DemoChargeQuery = value;
		}
	}

	// Token: 0x170006E1 RID: 1761
	// (get) Token: 0x0600906D RID: 36973 RVA: 0x002974A8 File Offset: 0x002956A8
	// (set) Token: 0x0600906E RID: 36974 RVA: 0x002974B8 File Offset: 0x002956B8
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

	// Token: 0x170006E2 RID: 1762
	// (get) Token: 0x0600906F RID: 36975 RVA: 0x002974C8 File Offset: 0x002956C8
	// (set) Token: 0x06009070 RID: 36976 RVA: 0x002974D8 File Offset: 0x002956D8
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

	// Token: 0x170006E3 RID: 1763
	// (get) Token: 0x06009071 RID: 36977 RVA: 0x002974E8 File Offset: 0x002956E8
	// (set) Token: 0x06009072 RID: 36978 RVA: 0x002974F8 File Offset: 0x002956F8
	public QueryContainer DemoChargesQuery
	{
		get
		{
			return this.ExposedVariables.DemoChargesQuery;
		}
		set
		{
			this.ExposedVariables.DemoChargesQuery = value;
		}
	}

	// Token: 0x170006E4 RID: 1764
	// (get) Token: 0x06009073 RID: 36979 RVA: 0x00297508 File Offset: 0x00295708
	// (set) Token: 0x06009074 RID: 36980 RVA: 0x00297518 File Offset: 0x00295718
	public GameObject NIS04_Camera
	{
		get
		{
			return this.ExposedVariables.NIS04_Camera;
		}
		set
		{
			this.ExposedVariables.NIS04_Camera = value;
		}
	}

	// Token: 0x06009075 RID: 36981 RVA: 0x00297528 File Offset: 0x00295728
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

	// Token: 0x06009076 RID: 36982 RVA: 0x00297590 File Offset: 0x00295790
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x06009077 RID: 36983 RVA: 0x002975A0 File Offset: 0x002957A0
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06009078 RID: 36984 RVA: 0x002975B0 File Offset: 0x002957B0
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06009079 RID: 36985 RVA: 0x002975C0 File Offset: 0x002957C0
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600907A RID: 36986 RVA: 0x002975D0 File Offset: 0x002957D0
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400D4A3 RID: 54435
	public M05_Intel ExposedVariables = new M05_Intel();
}
