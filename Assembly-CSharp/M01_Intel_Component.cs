using System;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x0200080C RID: 2060
[AddComponentMenu("uScript/Graphs/M01_Intel")]
public class M01_Intel_Component : uScriptCode
{
	// Token: 0x06003D1B RID: 15643 RVA: 0x0010C0D4 File Offset: 0x0010A2D4
	public M01_Intel_Component()
	{
	}

	// Token: 0x170005B9 RID: 1465
	// (get) Token: 0x06003D1C RID: 15644 RVA: 0x0010C0E8 File Offset: 0x0010A2E8
	// (set) Token: 0x06003D1D RID: 15645 RVA: 0x0010C0F8 File Offset: 0x0010A2F8
	public uIntelTransitionAnimationCurves.IntelAnimationCurves IntelCurveType1
	{
		get
		{
			return this.ExposedVariables.IntelCurveType1;
		}
		set
		{
			this.ExposedVariables.IntelCurveType1 = value;
		}
	}

	// Token: 0x06003D1E RID: 15646 RVA: 0x0010C108 File Offset: 0x0010A308
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

	// Token: 0x06003D1F RID: 15647 RVA: 0x0010C170 File Offset: 0x0010A370
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x06003D20 RID: 15648 RVA: 0x0010C180 File Offset: 0x0010A380
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06003D21 RID: 15649 RVA: 0x0010C190 File Offset: 0x0010A390
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06003D22 RID: 15650 RVA: 0x0010C1A0 File Offset: 0x0010A3A0
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x06003D23 RID: 15651 RVA: 0x0010C1B0 File Offset: 0x0010A3B0
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04004D8C RID: 19852
	public M01_Intel ExposedVariables = new M01_Intel();
}
