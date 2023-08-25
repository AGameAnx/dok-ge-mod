using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using UnityEngine;

// Token: 0x02000822 RID: 2082
[AddComponentMenu("uScript/Graphs/M03_VehicleAnims")]
public class M03_VehicleAnims_Component : uScriptCode
{
	// Token: 0x06006847 RID: 26695 RVA: 0x001D24A0 File Offset: 0x001D06A0
	public M03_VehicleAnims_Component()
	{
	}

	// Token: 0x1700065B RID: 1627
	// (get) Token: 0x06006848 RID: 26696 RVA: 0x001D24B4 File Offset: 0x001D06B4
	// (set) Token: 0x06006849 RID: 26697 RVA: 0x001D24C4 File Offset: 0x001D06C4
	public UnitSpawnWaveData SmallerGaalsiUnitsNIS
	{
		get
		{
			return this.ExposedVariables.SmallerGaalsiUnitsNIS;
		}
		set
		{
			this.ExposedVariables.SmallerGaalsiUnitsNIS = value;
		}
	}

	// Token: 0x1700065C RID: 1628
	// (get) Token: 0x0600684A RID: 26698 RVA: 0x001D24D4 File Offset: 0x001D06D4
	// (set) Token: 0x0600684B RID: 26699 RVA: 0x001D24E4 File Offset: 0x001D06E4
	public QueryContainer KapisiQuery
	{
		get
		{
			return this.ExposedVariables.KapisiQuery;
		}
		set
		{
			this.ExposedVariables.KapisiQuery = value;
		}
	}

	// Token: 0x1700065D RID: 1629
	// (get) Token: 0x0600684C RID: 26700 RVA: 0x001D24F4 File Offset: 0x001D06F4
	// (set) Token: 0x0600684D RID: 26701 RVA: 0x001D2504 File Offset: 0x001D0704
	public QueryContainer PlayerSCQuery
	{
		get
		{
			return this.ExposedVariables.PlayerSCQuery;
		}
		set
		{
			this.ExposedVariables.PlayerSCQuery = value;
		}
	}

	// Token: 0x1700065E RID: 1630
	// (get) Token: 0x0600684E RID: 26702 RVA: 0x001D2514 File Offset: 0x001D0714
	// (set) Token: 0x0600684F RID: 26703 RVA: 0x001D2524 File Offset: 0x001D0724
	public UnitSpawnWaveData GaalsiRefineryCruiserNIS
	{
		get
		{
			return this.ExposedVariables.GaalsiRefineryCruiserNIS;
		}
		set
		{
			this.ExposedVariables.GaalsiRefineryCruiserNIS = value;
		}
	}

	// Token: 0x1700065F RID: 1631
	// (get) Token: 0x06006850 RID: 26704 RVA: 0x001D2534 File Offset: 0x001D0734
	// (set) Token: 0x06006851 RID: 26705 RVA: 0x001D2544 File Offset: 0x001D0744
	public QueryContainer RachelNISQuery
	{
		get
		{
			return this.ExposedVariables.RachelNISQuery;
		}
		set
		{
			this.ExposedVariables.RachelNISQuery = value;
		}
	}

	// Token: 0x17000660 RID: 1632
	// (get) Token: 0x06006852 RID: 26706 RVA: 0x001D2554 File Offset: 0x001D0754
	// (set) Token: 0x06006853 RID: 26707 RVA: 0x001D2564 File Offset: 0x001D0764
	public UnitSpawnWaveData DerelictRailgunSpawn
	{
		get
		{
			return this.ExposedVariables.DerelictRailgunSpawn;
		}
		set
		{
			this.ExposedVariables.DerelictRailgunSpawn = value;
		}
	}

	// Token: 0x17000661 RID: 1633
	// (get) Token: 0x06006854 RID: 26708 RVA: 0x001D2574 File Offset: 0x001D0774
	// (set) Token: 0x06006855 RID: 26709 RVA: 0x001D2584 File Offset: 0x001D0784
	public UnitSpawnWaveData DerelictHACSpawn
	{
		get
		{
			return this.ExposedVariables.DerelictHACSpawn;
		}
		set
		{
			this.ExposedVariables.DerelictHACSpawn = value;
		}
	}

	// Token: 0x17000662 RID: 1634
	// (get) Token: 0x06006856 RID: 26710 RVA: 0x001D2594 File Offset: 0x001D0794
	// (set) Token: 0x06006857 RID: 26711 RVA: 0x001D25A4 File Offset: 0x001D07A4
	public UnitSpawnWaveData DerelictHarvesterSpawn
	{
		get
		{
			return this.ExposedVariables.DerelictHarvesterSpawn;
		}
		set
		{
			this.ExposedVariables.DerelictHarvesterSpawn = value;
		}
	}

	// Token: 0x06006858 RID: 26712 RVA: 0x001D25B4 File Offset: 0x001D07B4
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

	// Token: 0x06006859 RID: 26713 RVA: 0x001D261C File Offset: 0x001D081C
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0600685A RID: 26714 RVA: 0x001D262C File Offset: 0x001D082C
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600685B RID: 26715 RVA: 0x001D263C File Offset: 0x001D083C
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600685C RID: 26716 RVA: 0x001D264C File Offset: 0x001D084C
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x0600685D RID: 26717 RVA: 0x001D265C File Offset: 0x001D085C
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400937D RID: 37757
	public M03_VehicleAnims ExposedVariables = new M03_VehicleAnims();
}
