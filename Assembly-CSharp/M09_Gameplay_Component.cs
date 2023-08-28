using System;
using BBI.Game.Data.Queries;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x02000870 RID: 2160
[AddComponentMenu("uScript/Graphs/M09_Gameplay")]
public class M09_Gameplay_Component : uScriptCode
{
	// Token: 0x0600DF9E RID: 57246 RVA: 0x003FDE24 File Offset: 0x003FC024
	public M09_Gameplay_Component()
	{
	}

	// Token: 0x17000859 RID: 2137
	// (get) Token: 0x0600DF9F RID: 57247 RVA: 0x003FDE38 File Offset: 0x003FC038
	// (set) Token: 0x0600DFA0 RID: 57248 RVA: 0x003FDE48 File Offset: 0x003FC048
	public AttributeBuffSetData Buff_Perch
	{
		get
		{
			return this.ExposedVariables.Buff_Perch;
		}
		set
		{
			this.ExposedVariables.Buff_Perch = value;
		}
	}

	// Token: 0x1700085A RID: 2138
	// (get) Token: 0x0600DFA1 RID: 57249 RVA: 0x003FDE58 File Offset: 0x003FC058
	// (set) Token: 0x0600DFA2 RID: 57250 RVA: 0x003FDE68 File Offset: 0x003FC068
	public QueryContainer PlayerUnits
	{
		get
		{
			return this.ExposedVariables.PlayerUnits;
		}
		set
		{
			this.ExposedVariables.PlayerUnits = value;
		}
	}

	// Token: 0x1700085B RID: 2139
	// (get) Token: 0x0600DFA3 RID: 57251 RVA: 0x003FDE78 File Offset: 0x003FC078
	// (set) Token: 0x0600DFA4 RID: 57252 RVA: 0x003FDE88 File Offset: 0x003FC088
	public QueryContainer SuspendUnits
	{
		get
		{
			return this.ExposedVariables.SuspendUnits;
		}
		set
		{
			this.ExposedVariables.SuspendUnits = value;
		}
	}

	// Token: 0x1700085C RID: 2140
	// (get) Token: 0x0600DFA5 RID: 57253 RVA: 0x003FDE98 File Offset: 0x003FC098
	// (set) Token: 0x0600DFA6 RID: 57254 RVA: 0x003FDEA8 File Offset: 0x003FC0A8
	public QueryContainer SearchforSunder
	{
		get
		{
			return this.ExposedVariables.SearchforSunder;
		}
		set
		{
			this.ExposedVariables.SearchforSunder = value;
		}
	}

	// Token: 0x1700085D RID: 2141
	// (get) Token: 0x0600DFA7 RID: 57255 RVA: 0x003FDEB8 File Offset: 0x003FC0B8
	// (set) Token: 0x0600DFA8 RID: 57256 RVA: 0x003FDEC8 File Offset: 0x003FC0C8
	public QueryContainer SearchforRet
	{
		get
		{
			return this.ExposedVariables.SearchforRet;
		}
		set
		{
			this.ExposedVariables.SearchforRet = value;
		}
	}

	// Token: 0x0600DFA9 RID: 57257 RVA: 0x003FDED8 File Offset: 0x003FC0D8
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

	// Token: 0x0600DFAA RID: 57258 RVA: 0x003FDF40 File Offset: 0x003FC140
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0600DFAB RID: 57259 RVA: 0x003FDF50 File Offset: 0x003FC150
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600DFAC RID: 57260 RVA: 0x003FDF60 File Offset: 0x003FC160
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600DFAD RID: 57261 RVA: 0x003FDF70 File Offset: 0x003FC170
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x0600DFAE RID: 57262 RVA: 0x003FDF80 File Offset: 0x003FC180
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401534F RID: 86863
	public M09_Gameplay ExposedVariables = new M09_Gameplay();
}
