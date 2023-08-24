using System;
using UnityEngine;

// Token: 0x02000848 RID: 2120
[AddComponentMenu("uScript/Graphs/M05_Tornado")]
public class M05_Tornado_Component : uScriptCode
{
	// Token: 0x06009527 RID: 38183 RVA: 0x002AE0CC File Offset: 0x002AC2CC
	public M05_Tornado_Component()
	{
	}

	// Token: 0x06009528 RID: 38184 RVA: 0x002AE0E0 File Offset: 0x002AC2E0
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

	// Token: 0x06009529 RID: 38185 RVA: 0x002AE148 File Offset: 0x002AC348
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600952A RID: 38186 RVA: 0x002AE158 File Offset: 0x002AC358
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600952B RID: 38187 RVA: 0x002AE168 File Offset: 0x002AC368
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600952C RID: 38188 RVA: 0x002AE178 File Offset: 0x002AC378
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600952D RID: 38189 RVA: 0x002AE188 File Offset: 0x002AC388
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400DD12 RID: 56594
	public M05_Tornado ExposedVariables = new M05_Tornado();
}
