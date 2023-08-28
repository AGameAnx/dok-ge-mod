using System;
using BBI.Game.Data.Queries;
using UnityEngine;

// Token: 0x020008A5 RID: 2213
[AddComponentMenu("uScript/Graphs/M13_Dialog")]
public class M13_Dialog_Component : uScriptCode
{
	// Token: 0x06012DE1 RID: 77281 RVA: 0x0055495C File Offset: 0x00552B5C
	public M13_Dialog_Component()
	{
	}

	// Token: 0x170009C3 RID: 2499
	// (get) Token: 0x06012DE2 RID: 77282 RVA: 0x00554970 File Offset: 0x00552B70
	// (set) Token: 0x06012DE3 RID: 77283 RVA: 0x00554980 File Offset: 0x00552B80
	public QueryContainer Q_Rachel
	{
		get
		{
			return this.ExposedVariables.Q_Rachel;
		}
		set
		{
			this.ExposedVariables.Q_Rachel = value;
		}
	}

	// Token: 0x170009C4 RID: 2500
	// (get) Token: 0x06012DE4 RID: 77284 RVA: 0x00554990 File Offset: 0x00552B90
	// (set) Token: 0x06012DE5 RID: 77285 RVA: 0x005549A0 File Offset: 0x00552BA0
	public QueryContainer Q_Kapisi
	{
		get
		{
			return this.ExposedVariables.Q_Kapisi;
		}
		set
		{
			this.ExposedVariables.Q_Kapisi = value;
		}
	}

	// Token: 0x06012DE6 RID: 77286 RVA: 0x005549B0 File Offset: 0x00552BB0
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

	// Token: 0x06012DE7 RID: 77287 RVA: 0x00554A18 File Offset: 0x00552C18
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06012DE8 RID: 77288 RVA: 0x00554A28 File Offset: 0x00552C28
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06012DE9 RID: 77289 RVA: 0x00554A38 File Offset: 0x00552C38
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06012DEA RID: 77290 RVA: 0x00554A48 File Offset: 0x00552C48
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06012DEB RID: 77291 RVA: 0x00554A58 File Offset: 0x00552C58
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401C510 RID: 115984
	public M13_Dialog ExposedVariables = new M13_Dialog();
}
