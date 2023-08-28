using System;
using BBI.Game.Data.Queries;
using UnityEngine;

// Token: 0x02000881 RID: 2177
[AddComponentMenu("uScript/Graphs/M10_Core")]
public class M10_Core_Component : uScriptCode
{
	// Token: 0x0600F499 RID: 62617 RVA: 0x00453E8C File Offset: 0x0045208C
	public M10_Core_Component()
	{
	}

	// Token: 0x170008B6 RID: 2230
	// (get) Token: 0x0600F49A RID: 62618 RVA: 0x00453EA0 File Offset: 0x004520A0
	// (set) Token: 0x0600F49B RID: 62619 RVA: 0x00453EB0 File Offset: 0x004520B0
	public QueryContainer CCQueryContainer
	{
		get
		{
			return this.ExposedVariables.CCQueryContainer;
		}
		set
		{
			this.ExposedVariables.CCQueryContainer = value;
		}
	}

	// Token: 0x170008B7 RID: 2231
	// (get) Token: 0x0600F49C RID: 62620 RVA: 0x00453EC0 File Offset: 0x004520C0
	// (set) Token: 0x0600F49D RID: 62621 RVA: 0x00453ED0 File Offset: 0x004520D0
	public QueryContainer MissionFailureCarrierGodUnitsQ
	{
		get
		{
			return this.ExposedVariables.MissionFailureCarrierGodUnitsQ;
		}
		set
		{
			this.ExposedVariables.MissionFailureCarrierGodUnitsQ = value;
		}
	}

	// Token: 0x170008B8 RID: 2232
	// (get) Token: 0x0600F49E RID: 62622 RVA: 0x00453EE0 File Offset: 0x004520E0
	// (set) Token: 0x0600F49F RID: 62623 RVA: 0x00453EF0 File Offset: 0x004520F0
	public QueryContainer MissionFailureCarrierSuspendUnitsQ
	{
		get
		{
			return this.ExposedVariables.MissionFailureCarrierSuspendUnitsQ;
		}
		set
		{
			this.ExposedVariables.MissionFailureCarrierSuspendUnitsQ = value;
		}
	}

	// Token: 0x170008B9 RID: 2233
	// (get) Token: 0x0600F4A0 RID: 62624 RVA: 0x00453F00 File Offset: 0x00452100
	// (set) Token: 0x0600F4A1 RID: 62625 RVA: 0x00453F10 File Offset: 0x00452110
	public QueryContainer MissionFailureRachelGodUnitsQ
	{
		get
		{
			return this.ExposedVariables.MissionFailureRachelGodUnitsQ;
		}
		set
		{
			this.ExposedVariables.MissionFailureRachelGodUnitsQ = value;
		}
	}

	// Token: 0x170008BA RID: 2234
	// (get) Token: 0x0600F4A2 RID: 62626 RVA: 0x00453F20 File Offset: 0x00452120
	// (set) Token: 0x0600F4A3 RID: 62627 RVA: 0x00453F30 File Offset: 0x00452130
	public QueryContainer MissionFailureRachelSuspendUnitsQ
	{
		get
		{
			return this.ExposedVariables.MissionFailureRachelSuspendUnitsQ;
		}
		set
		{
			this.ExposedVariables.MissionFailureRachelSuspendUnitsQ = value;
		}
	}

	// Token: 0x170008BB RID: 2235
	// (get) Token: 0x0600F4A4 RID: 62628 RVA: 0x00453F40 File Offset: 0x00452140
	// (set) Token: 0x0600F4A5 RID: 62629 RVA: 0x00453F50 File Offset: 0x00452150
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

	// Token: 0x0600F4A6 RID: 62630 RVA: 0x00453F60 File Offset: 0x00452160
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

	// Token: 0x0600F4A7 RID: 62631 RVA: 0x00453FC8 File Offset: 0x004521C8
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0600F4A8 RID: 62632 RVA: 0x00453FD8 File Offset: 0x004521D8
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600F4A9 RID: 62633 RVA: 0x00453FE8 File Offset: 0x004521E8
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600F4AA RID: 62634 RVA: 0x00453FF8 File Offset: 0x004521F8
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x0600F4AB RID: 62635 RVA: 0x00454008 File Offset: 0x00452208
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04016E6D RID: 93805
	public M10_Core ExposedVariables = new M10_Core();
}
