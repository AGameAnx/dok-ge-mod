using System;
using BBI.Unity.Game.Scripting;
using UnityEngine;

// Token: 0x02000836 RID: 2102
[AddComponentMenu("uScript/Graphs/M05_GaalsiAttackers")]
public class M05_GaalsiAttackers_Component : uScriptCode
{
	// Token: 0x06008527 RID: 34087 RVA: 0x002611EC File Offset: 0x0025F3EC
	public M05_GaalsiAttackers_Component()
	{
	}

	// Token: 0x170006AD RID: 1709
	// (get) Token: 0x06008528 RID: 34088 RVA: 0x00261200 File Offset: 0x0025F400
	// (set) Token: 0x06008529 RID: 34089 RVA: 0x00261210 File Offset: 0x0025F410
	public SpawnFactoryData G_Attackers_Wave
	{
		get
		{
			return this.ExposedVariables.G_Attackers_Wave;
		}
		set
		{
			this.ExposedVariables.G_Attackers_Wave = value;
		}
	}

	// Token: 0x170006AE RID: 1710
	// (get) Token: 0x0600852A RID: 34090 RVA: 0x00261220 File Offset: 0x0025F420
	// (set) Token: 0x0600852B RID: 34091 RVA: 0x00261230 File Offset: 0x0025F430
	public SpawnFactoryData SB2_Kapisi_Attackers
	{
		get
		{
			return this.ExposedVariables.SB2_Kapisi_Attackers;
		}
		set
		{
			this.ExposedVariables.SB2_Kapisi_Attackers = value;
		}
	}

	// Token: 0x170006AF RID: 1711
	// (get) Token: 0x0600852C RID: 34092 RVA: 0x00261240 File Offset: 0x0025F440
	// (set) Token: 0x0600852D RID: 34093 RVA: 0x00261250 File Offset: 0x0025F450
	public GameObject G_Roam2
	{
		get
		{
			return this.ExposedVariables.G_Roam2;
		}
		set
		{
			this.ExposedVariables.G_Roam2 = value;
		}
	}

	// Token: 0x170006B0 RID: 1712
	// (get) Token: 0x0600852E RID: 34094 RVA: 0x00261260 File Offset: 0x0025F460
	// (set) Token: 0x0600852F RID: 34095 RVA: 0x00261270 File Offset: 0x0025F470
	public GameObject G_Roam1
	{
		get
		{
			return this.ExposedVariables.G_Roam1;
		}
		set
		{
			this.ExposedVariables.G_Roam1 = value;
		}
	}

	// Token: 0x06008530 RID: 34096 RVA: 0x00261280 File Offset: 0x0025F480
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

	// Token: 0x06008531 RID: 34097 RVA: 0x002612E8 File Offset: 0x0025F4E8
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06008532 RID: 34098 RVA: 0x002612F8 File Offset: 0x0025F4F8
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06008533 RID: 34099 RVA: 0x00261308 File Offset: 0x0025F508
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06008534 RID: 34100 RVA: 0x00261318 File Offset: 0x0025F518
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06008535 RID: 34101 RVA: 0x00261328 File Offset: 0x0025F528
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400C0C3 RID: 49347
	public M05_GaalsiAttackers ExposedVariables = new M05_GaalsiAttackers();
}
