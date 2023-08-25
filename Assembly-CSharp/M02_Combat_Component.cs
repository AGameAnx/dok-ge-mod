using System;
using UnityEngine;

// Token: 0x02000810 RID: 2064
[AddComponentMenu("uScript/Graphs/M02_Combat")]
public class M02_Combat_Component : uScriptCode
{
	// Token: 0x060042EF RID: 17135 RVA: 0x0012551C File Offset: 0x0012371C
	public M02_Combat_Component()
	{
	}

	// Token: 0x060042F0 RID: 17136 RVA: 0x00125530 File Offset: 0x00123730
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

	// Token: 0x060042F1 RID: 17137 RVA: 0x00125598 File Offset: 0x00123798
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x060042F2 RID: 17138 RVA: 0x001255A8 File Offset: 0x001237A8
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x060042F3 RID: 17139 RVA: 0x001255B8 File Offset: 0x001237B8
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x060042F4 RID: 17140 RVA: 0x001255C8 File Offset: 0x001237C8
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x060042F5 RID: 17141 RVA: 0x001255D8 File Offset: 0x001237D8
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04005551 RID: 21841
	public M02_Combat ExposedVariables = new M02_Combat();
}
