using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x02000824 RID: 2084
[AddComponentMenu("uScript/Graphs/M04_Core")]
public class M04_Core_Component : uScriptCode
{
	// Token: 0x06006D65 RID: 28005 RVA: 0x001EB90C File Offset: 0x001E9B0C
	public M04_Core_Component()
	{
	}

	// Token: 0x17000663 RID: 1635
	// (get) Token: 0x06006D66 RID: 28006 RVA: 0x001EB920 File Offset: 0x001E9B20
	// (set) Token: 0x06006D67 RID: 28007 RVA: 0x001EB930 File Offset: 0x001E9B30
	public BuffSetAttributesAsset CR_Buff
	{
		get
		{
			return this.ExposedVariables.CR_Buff;
		}
		set
		{
			this.ExposedVariables.CR_Buff = value;
		}
	}

	// Token: 0x17000664 RID: 1636
	// (get) Token: 0x06006D68 RID: 28008 RVA: 0x001EB940 File Offset: 0x001E9B40
	// (set) Token: 0x06006D69 RID: 28009 RVA: 0x001EB950 File Offset: 0x001E9B50
	public BuffSetAttributesAsset Raiders_Debuff
	{
		get
		{
			return this.ExposedVariables.Raiders_Debuff;
		}
		set
		{
			this.ExposedVariables.Raiders_Debuff = value;
		}
	}

	// Token: 0x17000665 RID: 1637
	// (get) Token: 0x06006D6A RID: 28010 RVA: 0x001EB960 File Offset: 0x001E9B60
	// (set) Token: 0x06006D6B RID: 28011 RVA: 0x001EB970 File Offset: 0x001E9B70
	public IEnumerable<Entity> PlayerCarrier
	{
		get
		{
			return this.ExposedVariables.PlayerCarrier;
		}
		set
		{
			this.ExposedVariables.PlayerCarrier = value;
		}
	}

	// Token: 0x17000666 RID: 1638
	// (get) Token: 0x06006D6C RID: 28012 RVA: 0x001EB980 File Offset: 0x001E9B80
	// (set) Token: 0x06006D6D RID: 28013 RVA: 0x001EB990 File Offset: 0x001E9B90
	public UnitSpawnWaveData Cheat_StrikeGroup
	{
		get
		{
			return this.ExposedVariables.Cheat_StrikeGroup;
		}
		set
		{
			this.ExposedVariables.Cheat_StrikeGroup = value;
		}
	}

	// Token: 0x17000667 RID: 1639
	// (get) Token: 0x06006D6E RID: 28014 RVA: 0x001EB9A0 File Offset: 0x001E9BA0
	// (set) Token: 0x06006D6F RID: 28015 RVA: 0x001EB9B0 File Offset: 0x001E9BB0
	public UnitSpawnWaveData G_Skirmishers
	{
		get
		{
			return this.ExposedVariables.G_Skirmishers;
		}
		set
		{
			this.ExposedVariables.G_Skirmishers = value;
		}
	}

	// Token: 0x17000668 RID: 1640
	// (get) Token: 0x06006D70 RID: 28016 RVA: 0x001EB9C0 File Offset: 0x001E9BC0
	// (set) Token: 0x06006D71 RID: 28017 RVA: 0x001EB9D0 File Offset: 0x001E9BD0
	public UnitSpawnWaveData G_Cats
	{
		get
		{
			return this.ExposedVariables.G_Cats;
		}
		set
		{
			this.ExposedVariables.G_Cats = value;
		}
	}

	// Token: 0x17000669 RID: 1641
	// (get) Token: 0x06006D72 RID: 28018 RVA: 0x001EB9E0 File Offset: 0x001E9BE0
	// (set) Token: 0x06006D73 RID: 28019 RVA: 0x001EB9F0 File Offset: 0x001E9BF0
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

	// Token: 0x1700066A RID: 1642
	// (get) Token: 0x06006D74 RID: 28020 RVA: 0x001EBA00 File Offset: 0x001E9C00
	// (set) Token: 0x06006D75 RID: 28021 RVA: 0x001EBA10 File Offset: 0x001E9C10
	public AttributeBuffSetData Cat_Buff
	{
		get
		{
			return this.ExposedVariables.Cat_Buff;
		}
		set
		{
			this.ExposedVariables.Cat_Buff = value;
		}
	}

	// Token: 0x1700066B RID: 1643
	// (get) Token: 0x06006D76 RID: 28022 RVA: 0x001EBA20 File Offset: 0x001E9C20
	// (set) Token: 0x06006D77 RID: 28023 RVA: 0x001EBA30 File Offset: 0x001E9C30
	public AttributeBuffSetData SS_Buff
	{
		get
		{
			return this.ExposedVariables.SS_Buff;
		}
		set
		{
			this.ExposedVariables.SS_Buff = value;
		}
	}

	// Token: 0x1700066C RID: 1644
	// (get) Token: 0x06006D78 RID: 28024 RVA: 0x001EBA40 File Offset: 0x001E9C40
	// (set) Token: 0x06006D79 RID: 28025 RVA: 0x001EBA50 File Offset: 0x001E9C50
	public UnitSpawnWaveData Debug_ProbeBR
	{
		get
		{
			return this.ExposedVariables.Debug_ProbeBR;
		}
		set
		{
			this.ExposedVariables.Debug_ProbeBR = value;
		}
	}

	// Token: 0x1700066D RID: 1645
	// (get) Token: 0x06006D7A RID: 28026 RVA: 0x001EBA60 File Offset: 0x001E9C60
	// (set) Token: 0x06006D7B RID: 28027 RVA: 0x001EBA70 File Offset: 0x001E9C70
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

	// Token: 0x1700066E RID: 1646
	// (get) Token: 0x06006D7C RID: 28028 RVA: 0x001EBA80 File Offset: 0x001E9C80
	// (set) Token: 0x06006D7D RID: 28029 RVA: 0x001EBA90 File Offset: 0x001E9C90
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

	// Token: 0x1700066F RID: 1647
	// (get) Token: 0x06006D7E RID: 28030 RVA: 0x001EBAA0 File Offset: 0x001E9CA0
	// (set) Token: 0x06006D7F RID: 28031 RVA: 0x001EBAB0 File Offset: 0x001E9CB0
	public QueryContainer ProbeSearch
	{
		get
		{
			return this.ExposedVariables.ProbeSearch;
		}
		set
		{
			this.ExposedVariables.ProbeSearch = value;
		}
	}

	// Token: 0x17000670 RID: 1648
	// (get) Token: 0x06006D80 RID: 28032 RVA: 0x001EBAC0 File Offset: 0x001E9CC0
	// (set) Token: 0x06006D81 RID: 28033 RVA: 0x001EBAD0 File Offset: 0x001E9CD0
	public AttributeBuffSetData BuffEscorts
	{
		get
		{
			return this.ExposedVariables.BuffEscorts;
		}
		set
		{
			this.ExposedVariables.BuffEscorts = value;
		}
	}

	// Token: 0x17000671 RID: 1649
	// (get) Token: 0x06006D82 RID: 28034 RVA: 0x001EBAE0 File Offset: 0x001E9CE0
	// (set) Token: 0x06006D83 RID: 28035 RVA: 0x001EBAF0 File Offset: 0x001E9CF0
	public AttributeBuffSetData RestoreEscortSpeed
	{
		get
		{
			return this.ExposedVariables.RestoreEscortSpeed;
		}
		set
		{
			this.ExposedVariables.RestoreEscortSpeed = value;
		}
	}

	// Token: 0x06006D84 RID: 28036 RVA: 0x001EBB00 File Offset: 0x001E9D00
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

	// Token: 0x06006D85 RID: 28037 RVA: 0x001EBB68 File Offset: 0x001E9D68
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06006D86 RID: 28038 RVA: 0x001EBB78 File Offset: 0x001E9D78
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06006D87 RID: 28039 RVA: 0x001EBB88 File Offset: 0x001E9D88
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06006D88 RID: 28040 RVA: 0x001EBB98 File Offset: 0x001E9D98
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06006D89 RID: 28041 RVA: 0x001EBBA8 File Offset: 0x001E9DA8
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04009BEF RID: 39919
	public M04_Core ExposedVariables = new M04_Core();
}
