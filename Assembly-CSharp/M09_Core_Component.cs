using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Queries;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x0200086A RID: 2154
[AddComponentMenu("uScript/Graphs/M09_Core")]
public class M09_Core_Component : uScriptCode
{
	// Token: 0x0600CA69 RID: 51817 RVA: 0x003A8FBC File Offset: 0x003A71BC
	public M09_Core_Component()
	{
	}

	// Token: 0x170007F3 RID: 2035
	// (get) Token: 0x0600CA6A RID: 51818 RVA: 0x003A8FD0 File Offset: 0x003A71D0
	// (set) Token: 0x0600CA6B RID: 51819 RVA: 0x003A8FE0 File Offset: 0x003A71E0
	public List<Entity> CommandCarrier
	{
		get
		{
			return this.ExposedVariables.CommandCarrier;
		}
		set
		{
			this.ExposedVariables.CommandCarrier = value;
		}
	}

	// Token: 0x170007F4 RID: 2036
	// (get) Token: 0x0600CA6C RID: 51820 RVA: 0x003A8FF0 File Offset: 0x003A71F0
	// (set) Token: 0x0600CA6D RID: 51821 RVA: 0x003A9000 File Offset: 0x003A7200
	public QueryContainer CCQueryContainer
	{
		get
		{
			return this.ExposedVariables.CCQueryContainer;
		}
		set
		{
			this.ExposedVariables.CCQueryContainer = value;
		}
	}

	// Token: 0x170007F5 RID: 2037
	// (get) Token: 0x0600CA6E RID: 51822 RVA: 0x003A9010 File Offset: 0x003A7210
	// (set) Token: 0x0600CA6F RID: 51823 RVA: 0x003A9020 File Offset: 0x003A7220
	public BuffSetAttributesAsset CRBuff
	{
		get
		{
			return this.ExposedVariables.CRBuff;
		}
		set
		{
			this.ExposedVariables.CRBuff = value;
		}
	}

	// Token: 0x170007F6 RID: 2038
	// (get) Token: 0x0600CA70 RID: 51824 RVA: 0x003A9030 File Offset: 0x003A7230
	// (set) Token: 0x0600CA71 RID: 51825 RVA: 0x003A9040 File Offset: 0x003A7240
	public BuffSetAttributesAsset Buff_HalfDamage
	{
		get
		{
			return this.ExposedVariables.Buff_HalfDamage;
		}
		set
		{
			this.ExposedVariables.Buff_HalfDamage = value;
		}
	}

	// Token: 0x170007F7 RID: 2039
	// (get) Token: 0x0600CA72 RID: 51826 RVA: 0x003A9050 File Offset: 0x003A7250
	// (set) Token: 0x0600CA73 RID: 51827 RVA: 0x003A9060 File Offset: 0x003A7260
	public BuffSetAttributesAsset Buff_SiidimAlly
	{
		get
		{
			return this.ExposedVariables.Buff_SiidimAlly;
		}
		set
		{
			this.ExposedVariables.Buff_SiidimAlly = value;
		}
	}

	// Token: 0x170007F8 RID: 2040
	// (get) Token: 0x0600CA74 RID: 51828 RVA: 0x003A9070 File Offset: 0x003A7270
	// (set) Token: 0x0600CA75 RID: 51829 RVA: 0x003A9080 File Offset: 0x003A7280
	public QueryContainer GodModeUnits
	{
		get
		{
			return this.ExposedVariables.GodModeUnits;
		}
		set
		{
			this.ExposedVariables.GodModeUnits = value;
		}
	}

	// Token: 0x170007F9 RID: 2041
	// (get) Token: 0x0600CA76 RID: 51830 RVA: 0x003A9090 File Offset: 0x003A7290
	// (set) Token: 0x0600CA77 RID: 51831 RVA: 0x003A90A0 File Offset: 0x003A72A0
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

	// Token: 0x170007FA RID: 2042
	// (get) Token: 0x0600CA78 RID: 51832 RVA: 0x003A90B0 File Offset: 0x003A72B0
	// (set) Token: 0x0600CA79 RID: 51833 RVA: 0x003A90C0 File Offset: 0x003A72C0
	public QueryContainer CarrierSearch
	{
		get
		{
			return this.ExposedVariables.CarrierSearch;
		}
		set
		{
			this.ExposedVariables.CarrierSearch = value;
		}
	}

	// Token: 0x170007FB RID: 2043
	// (get) Token: 0x0600CA7A RID: 51834 RVA: 0x003A90D0 File Offset: 0x003A72D0
	// (set) Token: 0x0600CA7B RID: 51835 RVA: 0x003A90E0 File Offset: 0x003A72E0
	public QueryContainer RachelSearch
	{
		get
		{
			return this.ExposedVariables.RachelSearch;
		}
		set
		{
			this.ExposedVariables.RachelSearch = value;
		}
	}

	// Token: 0x0600CA7C RID: 51836 RVA: 0x003A90F0 File Offset: 0x003A72F0
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

	// Token: 0x0600CA7D RID: 51837 RVA: 0x003A9158 File Offset: 0x003A7358
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0600CA7E RID: 51838 RVA: 0x003A9168 File Offset: 0x003A7368
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600CA7F RID: 51839 RVA: 0x003A9178 File Offset: 0x003A7378
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600CA80 RID: 51840 RVA: 0x003A9188 File Offset: 0x003A7388
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x0600CA81 RID: 51841 RVA: 0x003A9198 File Offset: 0x003A7398
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x040136D6 RID: 79574
	public M09_Core ExposedVariables = new M09_Core();
}
