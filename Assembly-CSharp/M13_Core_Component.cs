using System;
using UnityEngine;

// Token: 0x020008A3 RID: 2211
[AddComponentMenu("uScript/Graphs/M13_Core")]
public class M13_Core_Component : uScriptCode
{
	// Token: 0x06012B4C RID: 76620 RVA: 0x0054D1B8 File Offset: 0x0054B3B8
	public M13_Core_Component()
	{
	}

	// Token: 0x06012B4D RID: 76621 RVA: 0x0054D1CC File Offset: 0x0054B3CC
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

	// Token: 0x06012B4E RID: 76622 RVA: 0x0054D234 File Offset: 0x0054B434
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x06012B4F RID: 76623 RVA: 0x0054D244 File Offset: 0x0054B444
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06012B50 RID: 76624 RVA: 0x0054D254 File Offset: 0x0054B454
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06012B51 RID: 76625 RVA: 0x0054D264 File Offset: 0x0054B464
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x06012B52 RID: 76626 RVA: 0x0054D274 File Offset: 0x0054B474
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401C32E RID: 115502
	public M13_Core ExposedVariables = new M13_Core();
}
