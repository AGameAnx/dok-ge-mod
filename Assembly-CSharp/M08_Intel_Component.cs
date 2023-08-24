using System;
using BBI.Game.Data.Queries;
using UnityEngine;

// Token: 0x02000868 RID: 2152
[AddComponentMenu("uScript/Graphs/M08_Intel")]
public class M08_Intel_Component : uScriptCode
{
	// Token: 0x0600C939 RID: 51513 RVA: 0x003A3E4C File Offset: 0x003A204C
	public M08_Intel_Component()
	{
	}

	// Token: 0x170007EB RID: 2027
	// (get) Token: 0x0600C93A RID: 51514 RVA: 0x003A3E60 File Offset: 0x003A2060
	// (set) Token: 0x0600C93B RID: 51515 RVA: 0x003A3E70 File Offset: 0x003A2070
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

	// Token: 0x170007EC RID: 2028
	// (get) Token: 0x0600C93C RID: 51516 RVA: 0x003A3E80 File Offset: 0x003A2080
	// (set) Token: 0x0600C93D RID: 51517 RVA: 0x003A3E90 File Offset: 0x003A2090
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

	// Token: 0x170007ED RID: 2029
	// (get) Token: 0x0600C93E RID: 51518 RVA: 0x003A3EA0 File Offset: 0x003A20A0
	// (set) Token: 0x0600C93F RID: 51519 RVA: 0x003A3EB0 File Offset: 0x003A20B0
	public QueryContainer Renza_Query
	{
		get
		{
			return this.ExposedVariables.Renza_Query;
		}
		set
		{
			this.ExposedVariables.Renza_Query = value;
		}
	}

	// Token: 0x170007EE RID: 2030
	// (get) Token: 0x0600C940 RID: 51520 RVA: 0x003A3EC0 File Offset: 0x003A20C0
	// (set) Token: 0x0600C941 RID: 51521 RVA: 0x003A3ED0 File Offset: 0x003A20D0
	public QueryContainer AllPlayer
	{
		get
		{
			return this.ExposedVariables.AllPlayer;
		}
		set
		{
			this.ExposedVariables.AllPlayer = value;
		}
	}

	// Token: 0x170007EF RID: 2031
	// (get) Token: 0x0600C942 RID: 51522 RVA: 0x003A3EE0 File Offset: 0x003A20E0
	// (set) Token: 0x0600C943 RID: 51523 RVA: 0x003A3EF0 File Offset: 0x003A20F0
	public QueryContainer RachelQuery
	{
		get
		{
			return this.ExposedVariables.RachelQuery;
		}
		set
		{
			this.ExposedVariables.RachelQuery = value;
		}
	}

	// Token: 0x170007F0 RID: 2032
	// (get) Token: 0x0600C944 RID: 51524 RVA: 0x003A3F00 File Offset: 0x003A2100
	// (set) Token: 0x0600C945 RID: 51525 RVA: 0x003A3F10 File Offset: 0x003A2110
	public QueryContainer Kapisi_Query
	{
		get
		{
			return this.ExposedVariables.Kapisi_Query;
		}
		set
		{
			this.ExposedVariables.Kapisi_Query = value;
		}
	}

	// Token: 0x170007F1 RID: 2033
	// (get) Token: 0x0600C946 RID: 51526 RVA: 0x003A3F20 File Offset: 0x003A2120
	// (set) Token: 0x0600C947 RID: 51527 RVA: 0x003A3F30 File Offset: 0x003A2130
	public QueryContainer Rachel_Teleport_Query
	{
		get
		{
			return this.ExposedVariables.Rachel_Teleport_Query;
		}
		set
		{
			this.ExposedVariables.Rachel_Teleport_Query = value;
		}
	}

	// Token: 0x170007F2 RID: 2034
	// (get) Token: 0x0600C948 RID: 51528 RVA: 0x003A3F40 File Offset: 0x003A2140
	// (set) Token: 0x0600C949 RID: 51529 RVA: 0x003A3F50 File Offset: 0x003A2150
	public QueryContainer NIS02_Suspend
	{
		get
		{
			return this.ExposedVariables.NIS02_Suspend;
		}
		set
		{
			this.ExposedVariables.NIS02_Suspend = value;
		}
	}

	// Token: 0x0600C94A RID: 51530 RVA: 0x003A3F60 File Offset: 0x003A2160
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

	// Token: 0x0600C94B RID: 51531 RVA: 0x003A3FC8 File Offset: 0x003A21C8
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600C94C RID: 51532 RVA: 0x003A3FD8 File Offset: 0x003A21D8
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600C94D RID: 51533 RVA: 0x003A3FE8 File Offset: 0x003A21E8
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600C94E RID: 51534 RVA: 0x003A3FF8 File Offset: 0x003A21F8
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600C94F RID: 51535 RVA: 0x003A4008 File Offset: 0x003A2208
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401352D RID: 79149
	public M08_Intel ExposedVariables = new M08_Intel();
}
