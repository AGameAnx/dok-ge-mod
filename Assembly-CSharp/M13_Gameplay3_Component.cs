using System;
using UnityEngine;

// Token: 0x020008B0 RID: 2224
[AddComponentMenu("uScript/Graphs/M13_Gameplay3")]
public class M13_Gameplay3_Component : uScriptCode
{
	// Token: 0x06014004 RID: 81924 RVA: 0x005AD0C4 File Offset: 0x005AB2C4
	public M13_Gameplay3_Component()
	{
	}

	// Token: 0x06014005 RID: 81925 RVA: 0x005AD0D8 File Offset: 0x005AB2D8
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

	// Token: 0x06014006 RID: 81926 RVA: 0x005AD140 File Offset: 0x005AB340
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x06014007 RID: 81927 RVA: 0x005AD150 File Offset: 0x005AB350
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06014008 RID: 81928 RVA: 0x005AD160 File Offset: 0x005AB360
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06014009 RID: 81929 RVA: 0x005AD170 File Offset: 0x005AB370
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0601400A RID: 81930 RVA: 0x005AD180 File Offset: 0x005AB380
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401EC06 RID: 125958
	public M13_Gameplay3 ExposedVariables = new M13_Gameplay3();
}
