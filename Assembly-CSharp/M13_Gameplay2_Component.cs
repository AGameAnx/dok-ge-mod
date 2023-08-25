using System;
using BBI.Game.Data.Queries;
using UnityEngine;

// Token: 0x020008AE RID: 2222
[AddComponentMenu("uScript/Graphs/M13_Gameplay2")]
public class M13_Gameplay2_Component : uScriptCode
{
	// Token: 0x06013F46 RID: 81734 RVA: 0x005AA3C8 File Offset: 0x005A85C8
	public M13_Gameplay2_Component()
	{
	}

	// Token: 0x170009FF RID: 2559
	// (get) Token: 0x06013F47 RID: 81735 RVA: 0x005AA3DC File Offset: 0x005A85DC
	// (set) Token: 0x06013F48 RID: 81736 RVA: 0x005AA3EC File Offset: 0x005A85EC
	public bool ScannerNetworkComplete
	{
		get
		{
			return this.ExposedVariables.ScannerNetworkComplete;
		}
		set
		{
			this.ExposedVariables.ScannerNetworkComplete = value;
		}
	}

	// Token: 0x17000A00 RID: 2560
	// (get) Token: 0x06013F49 RID: 81737 RVA: 0x005AA3FC File Offset: 0x005A85FC
	// (set) Token: 0x06013F4A RID: 81738 RVA: 0x005AA40C File Offset: 0x005A860C
	public int SiteAScannerCount
	{
		get
		{
			return this.ExposedVariables.SiteAScannerCount;
		}
		set
		{
			this.ExposedVariables.SiteAScannerCount = value;
		}
	}

	// Token: 0x17000A01 RID: 2561
	// (get) Token: 0x06013F4B RID: 81739 RVA: 0x005AA41C File Offset: 0x005A861C
	// (set) Token: 0x06013F4C RID: 81740 RVA: 0x005AA42C File Offset: 0x005A862C
	public int SiteBScannerCount
	{
		get
		{
			return this.ExposedVariables.SiteBScannerCount;
		}
		set
		{
			this.ExposedVariables.SiteBScannerCount = value;
		}
	}

	// Token: 0x17000A02 RID: 2562
	// (get) Token: 0x06013F4D RID: 81741 RVA: 0x005AA43C File Offset: 0x005A863C
	// (set) Token: 0x06013F4E RID: 81742 RVA: 0x005AA44C File Offset: 0x005A864C
	public int SiteCScannerCount
	{
		get
		{
			return this.ExposedVariables.SiteCScannerCount;
		}
		set
		{
			this.ExposedVariables.SiteCScannerCount = value;
		}
	}

	// Token: 0x17000A03 RID: 2563
	// (get) Token: 0x06013F4F RID: 81743 RVA: 0x005AA45C File Offset: 0x005A865C
	// (set) Token: 0x06013F50 RID: 81744 RVA: 0x005AA46C File Offset: 0x005A866C
	public int SiteAJammerCount
	{
		get
		{
			return this.ExposedVariables.SiteAJammerCount;
		}
		set
		{
			this.ExposedVariables.SiteAJammerCount = value;
		}
	}

	// Token: 0x17000A04 RID: 2564
	// (get) Token: 0x06013F51 RID: 81745 RVA: 0x005AA47C File Offset: 0x005A867C
	// (set) Token: 0x06013F52 RID: 81746 RVA: 0x005AA48C File Offset: 0x005A868C
	public int SiteCJammerCount
	{
		get
		{
			return this.ExposedVariables.SiteCJammerCount;
		}
		set
		{
			this.ExposedVariables.SiteCJammerCount = value;
		}
	}

	// Token: 0x17000A05 RID: 2565
	// (get) Token: 0x06013F53 RID: 81747 RVA: 0x005AA49C File Offset: 0x005A869C
	// (set) Token: 0x06013F54 RID: 81748 RVA: 0x005AA4AC File Offset: 0x005A86AC
	public int SiteBJammerCount
	{
		get
		{
			return this.ExposedVariables.SiteBJammerCount;
		}
		set
		{
			this.ExposedVariables.SiteBJammerCount = value;
		}
	}

	// Token: 0x17000A06 RID: 2566
	// (get) Token: 0x06013F55 RID: 81749 RVA: 0x005AA4BC File Offset: 0x005A86BC
	// (set) Token: 0x06013F56 RID: 81750 RVA: 0x005AA4CC File Offset: 0x005A86CC
	public int SitesWithScanners
	{
		get
		{
			return this.ExposedVariables.SitesWithScanners;
		}
		set
		{
			this.ExposedVariables.SitesWithScanners = value;
		}
	}

	// Token: 0x17000A07 RID: 2567
	// (get) Token: 0x06013F57 RID: 81751 RVA: 0x005AA4DC File Offset: 0x005A86DC
	// (set) Token: 0x06013F58 RID: 81752 RVA: 0x005AA4EC File Offset: 0x005A86EC
	public QueryContainer Q_ImmortailDuringIntelMoments
	{
		get
		{
			return this.ExposedVariables.Q_ImmortailDuringIntelMoments;
		}
		set
		{
			this.ExposedVariables.Q_ImmortailDuringIntelMoments = value;
		}
	}

	// Token: 0x17000A08 RID: 2568
	// (get) Token: 0x06013F59 RID: 81753 RVA: 0x005AA4FC File Offset: 0x005A86FC
	// (set) Token: 0x06013F5A RID: 81754 RVA: 0x005AA50C File Offset: 0x005A870C
	public QueryContainer Q_PlayerUnits
	{
		get
		{
			return this.ExposedVariables.Q_PlayerUnits;
		}
		set
		{
			this.ExposedVariables.Q_PlayerUnits = value;
		}
	}

	// Token: 0x17000A09 RID: 2569
	// (get) Token: 0x06013F5B RID: 81755 RVA: 0x005AA51C File Offset: 0x005A871C
	// (set) Token: 0x06013F5C RID: 81756 RVA: 0x005AA52C File Offset: 0x005A872C
	public QueryContainer Q_PlayerTurrets
	{
		get
		{
			return this.ExposedVariables.Q_PlayerTurrets;
		}
		set
		{
			this.ExposedVariables.Q_PlayerTurrets = value;
		}
	}

	// Token: 0x17000A0A RID: 2570
	// (get) Token: 0x06013F5D RID: 81757 RVA: 0x005AA53C File Offset: 0x005A873C
	// (set) Token: 0x06013F5E RID: 81758 RVA: 0x005AA54C File Offset: 0x005A874C
	public QueryContainer Q_Cruisers
	{
		get
		{
			return this.ExposedVariables.Q_Cruisers;
		}
		set
		{
			this.ExposedVariables.Q_Cruisers = value;
		}
	}

	// Token: 0x06013F5F RID: 81759 RVA: 0x005AA55C File Offset: 0x005A875C
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

	// Token: 0x06013F60 RID: 81760 RVA: 0x005AA5C4 File Offset: 0x005A87C4
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06013F61 RID: 81761 RVA: 0x005AA5D4 File Offset: 0x005A87D4
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06013F62 RID: 81762 RVA: 0x005AA5E4 File Offset: 0x005A87E4
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06013F63 RID: 81763 RVA: 0x005AA5F4 File Offset: 0x005A87F4
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06013F64 RID: 81764 RVA: 0x005AA604 File Offset: 0x005A8804
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401EB06 RID: 125702
	public M13_Gameplay2 ExposedVariables = new M13_Gameplay2();
}
