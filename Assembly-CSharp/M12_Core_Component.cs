using System;
using BBI.Game.Data.Queries;
using UnityEngine;

// Token: 0x02000897 RID: 2199
[AddComponentMenu("uScript/Graphs/M12_Core")]
public class M12_Core_Component : uScriptCode
{
	// Token: 0x06011937 RID: 71991 RVA: 0x004FB91C File Offset: 0x004F9B1C
	public M12_Core_Component()
	{
	}

	// Token: 0x17000974 RID: 2420
	// (get) Token: 0x06011938 RID: 71992 RVA: 0x004FB930 File Offset: 0x004F9B30
	// (set) Token: 0x06011939 RID: 71993 RVA: 0x004FB940 File Offset: 0x004F9B40
	public QueryContainer MissionFailureGodUnitsQ
	{
		get
		{
			return this.ExposedVariables.MissionFailureGodUnitsQ;
		}
		set
		{
			this.ExposedVariables.MissionFailureGodUnitsQ = value;
		}
	}

	// Token: 0x17000975 RID: 2421
	// (get) Token: 0x0601193A RID: 71994 RVA: 0x004FB950 File Offset: 0x004F9B50
	// (set) Token: 0x0601193B RID: 71995 RVA: 0x004FB960 File Offset: 0x004F9B60
	public QueryContainer RachelCoreQuery
	{
		get
		{
			return this.ExposedVariables.RachelCoreQuery;
		}
		set
		{
			this.ExposedVariables.RachelCoreQuery = value;
		}
	}

	// Token: 0x17000976 RID: 2422
	// (get) Token: 0x0601193C RID: 71996 RVA: 0x004FB970 File Offset: 0x004F9B70
	// (set) Token: 0x0601193D RID: 71997 RVA: 0x004FB980 File Offset: 0x004F9B80
	public QueryContainer KapisiCoreQuery
	{
		get
		{
			return this.ExposedVariables.KapisiCoreQuery;
		}
		set
		{
			this.ExposedVariables.KapisiCoreQuery = value;
		}
	}

	// Token: 0x17000977 RID: 2423
	// (get) Token: 0x0601193E RID: 71998 RVA: 0x004FB990 File Offset: 0x004F9B90
	// (set) Token: 0x0601193F RID: 71999 RVA: 0x004FB9A0 File Offset: 0x004F9BA0
	public QueryContainer AllCommanderQuery
	{
		get
		{
			return this.ExposedVariables.AllCommanderQuery;
		}
		set
		{
			this.ExposedVariables.AllCommanderQuery = value;
		}
	}

	// Token: 0x06011940 RID: 72000 RVA: 0x004FB9B0 File Offset: 0x004F9BB0
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

	// Token: 0x06011941 RID: 72001 RVA: 0x004FBA18 File Offset: 0x004F9C18
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06011942 RID: 72002 RVA: 0x004FBA28 File Offset: 0x004F9C28
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06011943 RID: 72003 RVA: 0x004FBA38 File Offset: 0x004F9C38
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06011944 RID: 72004 RVA: 0x004FBA48 File Offset: 0x004F9C48
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06011945 RID: 72005 RVA: 0x004FBA58 File Offset: 0x004F9C58
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401A50A RID: 107786
	public M12_Core ExposedVariables = new M12_Core();
}
