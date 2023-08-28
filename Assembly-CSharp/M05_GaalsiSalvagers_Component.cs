using System;
using UnityEngine;

// Token: 0x0200083A RID: 2106
[AddComponentMenu("uScript/Graphs/M05_GaalsiSalvagers")]
public class M05_GaalsiSalvagers_Component : uScriptCode
{
	// Token: 0x06008657 RID: 34391 RVA: 0x00267C60 File Offset: 0x00265E60
	public M05_GaalsiSalvagers_Component()
	{
	}

	// Token: 0x06008658 RID: 34392 RVA: 0x00267C74 File Offset: 0x00265E74
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

	// Token: 0x06008659 RID: 34393 RVA: 0x00267CDC File Offset: 0x00265EDC
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0600865A RID: 34394 RVA: 0x00267CEC File Offset: 0x00265EEC
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600865B RID: 34395 RVA: 0x00267CFC File Offset: 0x00265EFC
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600865C RID: 34396 RVA: 0x00267D0C File Offset: 0x00265F0C
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x0600865D RID: 34397 RVA: 0x00267D1C File Offset: 0x00265F1C
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400C33B RID: 49979
	public M05_GaalsiSalvagers ExposedVariables = new M05_GaalsiSalvagers();
}
