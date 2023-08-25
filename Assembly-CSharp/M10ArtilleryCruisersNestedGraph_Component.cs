using System;
using UnityEngine;

// Token: 0x0200087D RID: 2173
[AddComponentMenu("uScript/Graphs/M10ArtilleryCruisersNestedGraph")]
public class M10ArtilleryCruisersNestedGraph_Component : uScriptCode
{
	// Token: 0x0600F29F RID: 62111 RVA: 0x0044C990 File Offset: 0x0044AB90
	public M10ArtilleryCruisersNestedGraph_Component()
	{
	}

	// Token: 0x0600F2A0 RID: 62112 RVA: 0x0044C9A4 File Offset: 0x0044ABA4
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

	// Token: 0x0600F2A1 RID: 62113 RVA: 0x0044CA0C File Offset: 0x0044AC0C
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0600F2A2 RID: 62114 RVA: 0x0044CA1C File Offset: 0x0044AC1C
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600F2A3 RID: 62115 RVA: 0x0044CA2C File Offset: 0x0044AC2C
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600F2A4 RID: 62116 RVA: 0x0044CA3C File Offset: 0x0044AC3C
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x0600F2A5 RID: 62117 RVA: 0x0044CA4C File Offset: 0x0044AC4C
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04016C1B RID: 93211
	public M10ArtilleryCruisersNestedGraph ExposedVariables = new M10ArtilleryCruisersNestedGraph();
}
