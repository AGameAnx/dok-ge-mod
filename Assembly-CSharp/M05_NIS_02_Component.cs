using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Scripting;
using UnityEngine;

// Token: 0x02000844 RID: 2116
[AddComponentMenu("uScript/Graphs/M05_NIS_02")]
public class M05_NIS_02_Component : uScriptCode
{
	// Token: 0x0600916E RID: 37230 RVA: 0x0029CA04 File Offset: 0x0029AC04
	public M05_NIS_02_Component()
	{
	}

	// Token: 0x170006E9 RID: 1769
	// (get) Token: 0x0600916F RID: 37231 RVA: 0x0029CA18 File Offset: 0x0029AC18
	// (set) Token: 0x06009170 RID: 37232 RVA: 0x0029CA28 File Offset: 0x0029AC28
	public UnitSpawnWaveData NIS_02_Baserunner
	{
		get
		{
			return this.ExposedVariables.NIS_02_Baserunner;
		}
		set
		{
			this.ExposedVariables.NIS_02_Baserunner = value;
		}
	}

	// Token: 0x170006EA RID: 1770
	// (get) Token: 0x06009171 RID: 37233 RVA: 0x0029CA38 File Offset: 0x0029AC38
	// (set) Token: 0x06009172 RID: 37234 RVA: 0x0029CA48 File Offset: 0x0029AC48
	public List<Entity> NIS_02_Baserunner_hero
	{
		get
		{
			return this.ExposedVariables.NIS_02_Baserunner_hero;
		}
		set
		{
			this.ExposedVariables.NIS_02_Baserunner_hero = value;
		}
	}

	// Token: 0x06009173 RID: 37235 RVA: 0x0029CA58 File Offset: 0x0029AC58
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

	// Token: 0x06009174 RID: 37236 RVA: 0x0029CAC0 File Offset: 0x0029ACC0
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06009175 RID: 37237 RVA: 0x0029CAD0 File Offset: 0x0029ACD0
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06009176 RID: 37238 RVA: 0x0029CAE0 File Offset: 0x0029ACE0
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06009177 RID: 37239 RVA: 0x0029CAF0 File Offset: 0x0029ACF0
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06009178 RID: 37240 RVA: 0x0029CB00 File Offset: 0x0029AD00
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400D69B RID: 54939
	public M05_NIS_02 ExposedVariables = new M05_NIS_02();
}
