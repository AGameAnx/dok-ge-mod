using System;
using UnityEngine;

// Token: 0x02000899 RID: 2201
[AddComponentMenu("uScript/Graphs/M12_Dialog")]
public class M12_Dialog_Component : uScriptCode
{
	// Token: 0x06011A39 RID: 72249 RVA: 0x004FEEA4 File Offset: 0x004FD0A4
	public M12_Dialog_Component()
	{
	}

	// Token: 0x06011A3A RID: 72250 RVA: 0x004FEEB8 File Offset: 0x004FD0B8
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

	// Token: 0x06011A3B RID: 72251 RVA: 0x004FEF20 File Offset: 0x004FD120
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06011A3C RID: 72252 RVA: 0x004FEF30 File Offset: 0x004FD130
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06011A3D RID: 72253 RVA: 0x004FEF40 File Offset: 0x004FD140
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06011A3E RID: 72254 RVA: 0x004FEF50 File Offset: 0x004FD150
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06011A3F RID: 72255 RVA: 0x004FEF60 File Offset: 0x004FD160
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401A5FA RID: 108026
	public M12_Dialog ExposedVariables = new M12_Dialog();
}
