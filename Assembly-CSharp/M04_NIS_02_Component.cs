using System;
using BBI.Game.Data.Queries;
using UnityEngine;

// Token: 0x0200082E RID: 2094
[AddComponentMenu("uScript/Graphs/M04_NIS_02")]
public class M04_NIS_02_Component : uScriptCode
{
	// Token: 0x06008075 RID: 32885 RVA: 0x002474A4 File Offset: 0x002456A4
	public M04_NIS_02_Component()
	{
	}

	// Token: 0x1700069C RID: 1692
	// (get) Token: 0x06008076 RID: 32886 RVA: 0x002474B8 File Offset: 0x002456B8
	// (set) Token: 0x06008077 RID: 32887 RVA: 0x002474C8 File Offset: 0x002456C8
	public QueryContainer Rachel
	{
		get
		{
			return this.ExposedVariables.Rachel;
		}
		set
		{
			this.ExposedVariables.Rachel = value;
		}
	}

	// Token: 0x1700069D RID: 1693
	// (get) Token: 0x06008078 RID: 32888 RVA: 0x002474D8 File Offset: 0x002456D8
	// (set) Token: 0x06008079 RID: 32889 RVA: 0x002474E8 File Offset: 0x002456E8
	public QueryContainer SearchFleet
	{
		get
		{
			return this.ExposedVariables.SearchFleet;
		}
		set
		{
			this.ExposedVariables.SearchFleet = value;
		}
	}

	// Token: 0x0600807A RID: 32890 RVA: 0x002474F8 File Offset: 0x002456F8
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

	// Token: 0x0600807B RID: 32891 RVA: 0x00247560 File Offset: 0x00245760
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0600807C RID: 32892 RVA: 0x00247570 File Offset: 0x00245770
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600807D RID: 32893 RVA: 0x00247580 File Offset: 0x00245780
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600807E RID: 32894 RVA: 0x00247590 File Offset: 0x00245790
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x0600807F RID: 32895 RVA: 0x002475A0 File Offset: 0x002457A0
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400B859 RID: 47193
	public M04_NIS_02 ExposedVariables = new M04_NIS_02();
}
