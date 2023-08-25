using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Scripting;
using UnityEngine;

// Token: 0x02000877 RID: 2167
[AddComponentMenu("uScript/Graphs/M09_Siidim2")]
public class M09_Siidim2_Component : uScriptCode
{
	// Token: 0x0600F0D5 RID: 61653 RVA: 0x004458C0 File Offset: 0x00443AC0
	public M09_Siidim2_Component()
	{
	}

	// Token: 0x17000873 RID: 2163
	// (get) Token: 0x0600F0D6 RID: 61654 RVA: 0x004458D4 File Offset: 0x00443AD4
	// (set) Token: 0x0600F0D7 RID: 61655 RVA: 0x004458E4 File Offset: 0x00443AE4
	public List<Entity> Siidim_StartingCarrier
	{
		get
		{
			return this.ExposedVariables.Siidim_StartingCarrier;
		}
		set
		{
			this.ExposedVariables.Siidim_StartingCarrier = value;
		}
	}

	// Token: 0x17000874 RID: 2164
	// (get) Token: 0x0600F0D8 RID: 61656 RVA: 0x004458F4 File Offset: 0x00443AF4
	// (set) Token: 0x0600F0D9 RID: 61657 RVA: 0x00445904 File Offset: 0x00443B04
	public QueryContainer SearchSakala
	{
		get
		{
			return this.ExposedVariables.SearchSakala;
		}
		set
		{
			this.ExposedVariables.SearchSakala = value;
		}
	}

	// Token: 0x17000875 RID: 2165
	// (get) Token: 0x0600F0DA RID: 61658 RVA: 0x00445914 File Offset: 0x00443B14
	// (set) Token: 0x0600F0DB RID: 61659 RVA: 0x00445924 File Offset: 0x00443B24
	public UnitSpawnWaveData SMoveAttackers
	{
		get
		{
			return this.ExposedVariables.SMoveAttackers;
		}
		set
		{
			this.ExposedVariables.SMoveAttackers = value;
		}
	}

	// Token: 0x17000876 RID: 2166
	// (get) Token: 0x0600F0DC RID: 61660 RVA: 0x00445934 File Offset: 0x00443B34
	// (set) Token: 0x0600F0DD RID: 61661 RVA: 0x00445944 File Offset: 0x00443B44
	public QueryContainer SearchGaalsiUnits
	{
		get
		{
			return this.ExposedVariables.SearchGaalsiUnits;
		}
		set
		{
			this.ExposedVariables.SearchGaalsiUnits = value;
		}
	}

	// Token: 0x17000877 RID: 2167
	// (get) Token: 0x0600F0DE RID: 61662 RVA: 0x00445954 File Offset: 0x00443B54
	// (set) Token: 0x0600F0DF RID: 61663 RVA: 0x00445964 File Offset: 0x00443B64
	public UnitSpawnWaveData SMoveGuards
	{
		get
		{
			return this.ExposedVariables.SMoveGuards;
		}
		set
		{
			this.ExposedVariables.SMoveGuards = value;
		}
	}

	// Token: 0x17000878 RID: 2168
	// (get) Token: 0x0600F0E0 RID: 61664 RVA: 0x00445974 File Offset: 0x00443B74
	// (set) Token: 0x0600F0E1 RID: 61665 RVA: 0x00445984 File Offset: 0x00443B84
	public UnitSpawnWaveData Siidim_Reinforcements
	{
		get
		{
			return this.ExposedVariables.Siidim_Reinforcements;
		}
		set
		{
			this.ExposedVariables.Siidim_Reinforcements = value;
		}
	}

	// Token: 0x17000879 RID: 2169
	// (get) Token: 0x0600F0E2 RID: 61666 RVA: 0x00445994 File Offset: 0x00443B94
	// (set) Token: 0x0600F0E3 RID: 61667 RVA: 0x004459A4 File Offset: 0x00443BA4
	public UnitSpawnWaveData Siidim_Reinforcements2
	{
		get
		{
			return this.ExposedVariables.Siidim_Reinforcements2;
		}
		set
		{
			this.ExposedVariables.Siidim_Reinforcements2 = value;
		}
	}

	// Token: 0x1700087A RID: 2170
	// (get) Token: 0x0600F0E4 RID: 61668 RVA: 0x004459B4 File Offset: 0x00443BB4
	// (set) Token: 0x0600F0E5 RID: 61669 RVA: 0x004459C4 File Offset: 0x00443BC4
	public UnitSpawnWaveData Siidim_Reinforcements3
	{
		get
		{
			return this.ExposedVariables.Siidim_Reinforcements3;
		}
		set
		{
			this.ExposedVariables.Siidim_Reinforcements3 = value;
		}
	}

	// Token: 0x1700087B RID: 2171
	// (get) Token: 0x0600F0E6 RID: 61670 RVA: 0x004459D4 File Offset: 0x00443BD4
	// (set) Token: 0x0600F0E7 RID: 61671 RVA: 0x004459E4 File Offset: 0x00443BE4
	public UnitSpawnWaveData SiidimSupportCruiser
	{
		get
		{
			return this.ExposedVariables.SiidimSupportCruiser;
		}
		set
		{
			this.ExposedVariables.SiidimSupportCruiser = value;
		}
	}

	// Token: 0x1700087C RID: 2172
	// (get) Token: 0x0600F0E8 RID: 61672 RVA: 0x004459F4 File Offset: 0x00443BF4
	// (set) Token: 0x0600F0E9 RID: 61673 RVA: 0x00445A04 File Offset: 0x00443C04
	public QueryContainer FindInitialDefenders
	{
		get
		{
			return this.ExposedVariables.FindInitialDefenders;
		}
		set
		{
			this.ExposedVariables.FindInitialDefenders = value;
		}
	}

	// Token: 0x1700087D RID: 2173
	// (get) Token: 0x0600F0EA RID: 61674 RVA: 0x00445A14 File Offset: 0x00443C14
	// (set) Token: 0x0600F0EB RID: 61675 RVA: 0x00445A24 File Offset: 0x00443C24
	public SpawnFactoryData SidAAReaction
	{
		get
		{
			return this.ExposedVariables.SidAAReaction;
		}
		set
		{
			this.ExposedVariables.SidAAReaction = value;
		}
	}

	// Token: 0x1700087E RID: 2174
	// (get) Token: 0x0600F0EC RID: 61676 RVA: 0x00445A34 File Offset: 0x00443C34
	// (set) Token: 0x0600F0ED RID: 61677 RVA: 0x00445A44 File Offset: 0x00443C44
	public QueryContainer RemoveCarrier
	{
		get
		{
			return this.ExposedVariables.RemoveCarrier;
		}
		set
		{
			this.ExposedVariables.RemoveCarrier = value;
		}
	}

	// Token: 0x1700087F RID: 2175
	// (get) Token: 0x0600F0EE RID: 61678 RVA: 0x00445A54 File Offset: 0x00443C54
	// (set) Token: 0x0600F0EF RID: 61679 RVA: 0x00445A64 File Offset: 0x00443C64
	public UnitSpawnWaveData SidReactAirUnits
	{
		get
		{
			return this.ExposedVariables.SidReactAirUnits;
		}
		set
		{
			this.ExposedVariables.SidReactAirUnits = value;
		}
	}

	// Token: 0x17000880 RID: 2176
	// (get) Token: 0x0600F0F0 RID: 61680 RVA: 0x00445A74 File Offset: 0x00443C74
	// (set) Token: 0x0600F0F1 RID: 61681 RVA: 0x00445A84 File Offset: 0x00443C84
	public SpawnFactoryData SidReactBuildGroundForces
	{
		get
		{
			return this.ExposedVariables.SidReactBuildGroundForces;
		}
		set
		{
			this.ExposedVariables.SidReactBuildGroundForces = value;
		}
	}

	// Token: 0x17000881 RID: 2177
	// (get) Token: 0x0600F0F2 RID: 61682 RVA: 0x00445A94 File Offset: 0x00443C94
	// (set) Token: 0x0600F0F3 RID: 61683 RVA: 0x00445AA4 File Offset: 0x00443CA4
	public QueryContainer SearchForSakalaDefenders
	{
		get
		{
			return this.ExposedVariables.SearchForSakalaDefenders;
		}
		set
		{
			this.ExposedVariables.SearchForSakalaDefenders = value;
		}
	}

	// Token: 0x17000882 RID: 2178
	// (get) Token: 0x0600F0F4 RID: 61684 RVA: 0x00445AB4 File Offset: 0x00443CB4
	// (set) Token: 0x0600F0F5 RID: 61685 RVA: 0x00445AC4 File Offset: 0x00443CC4
	public QueryContainer LookforReactionUnits
	{
		get
		{
			return this.ExposedVariables.LookforReactionUnits;
		}
		set
		{
			this.ExposedVariables.LookforReactionUnits = value;
		}
	}

	// Token: 0x0600F0F6 RID: 61686 RVA: 0x00445AD4 File Offset: 0x00443CD4
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

	// Token: 0x0600F0F7 RID: 61687 RVA: 0x00445B3C File Offset: 0x00443D3C
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0600F0F8 RID: 61688 RVA: 0x00445B4C File Offset: 0x00443D4C
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600F0F9 RID: 61689 RVA: 0x00445B5C File Offset: 0x00443D5C
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600F0FA RID: 61690 RVA: 0x00445B6C File Offset: 0x00443D6C
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x0600F0FB RID: 61691 RVA: 0x00445B7C File Offset: 0x00443D7C
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0600F0FC RID: 61692 RVA: 0x00445B8C File Offset: 0x00443D8C
	private void OnGUI()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnGUI();
	}

	// Token: 0x040169A3 RID: 92579
	public M09_Siidim2 ExposedVariables = new M09_Siidim2();
}
