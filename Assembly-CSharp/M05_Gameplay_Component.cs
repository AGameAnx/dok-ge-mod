using System;
using UnityEngine;

// Token: 0x0200083E RID: 2110
[AddComponentMenu("uScript/Graphs/M05_Gameplay")]
public class M05_Gameplay_Component : uScriptCode
{
	// Token: 0x06008DC3 RID: 36291 RVA: 0x0028989C File Offset: 0x00287A9C
	public M05_Gameplay_Component()
	{
	}

	// Token: 0x06008DC4 RID: 36292 RVA: 0x002898B0 File Offset: 0x00287AB0
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

	// Token: 0x06008DC5 RID: 36293 RVA: 0x00289918 File Offset: 0x00287B18
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06008DC6 RID: 36294 RVA: 0x00289928 File Offset: 0x00287B28
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06008DC7 RID: 36295 RVA: 0x00289938 File Offset: 0x00287B38
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06008DC8 RID: 36296 RVA: 0x00289948 File Offset: 0x00287B48
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06008DC9 RID: 36297 RVA: 0x00289958 File Offset: 0x00287B58
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400CF89 RID: 53129
	public M05_Gameplay ExposedVariables = new M05_Gameplay();
}
