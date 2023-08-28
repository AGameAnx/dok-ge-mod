using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Scripting;
using UnityEngine;

// Token: 0x02000850 RID: 2128
[AddComponentMenu("uScript/Graphs/M06_Gameplay2")]
public class M06_Gameplay2_Component : uScriptCode
{
	// Token: 0x06009E1F RID: 40479 RVA: 0x002D7B34 File Offset: 0x002D5D34
	public M06_Gameplay2_Component()
	{
	}

	// Token: 0x1700071A RID: 1818
	// (get) Token: 0x06009E20 RID: 40480 RVA: 0x002D7B48 File Offset: 0x002D5D48
	// (set) Token: 0x06009E21 RID: 40481 RVA: 0x002D7B58 File Offset: 0x002D5D58
	public UnitSpawnWaveData ReturningFleetArmoured
	{
		get
		{
			return this.ExposedVariables.ReturningFleetArmoured;
		}
		set
		{
			this.ExposedVariables.ReturningFleetArmoured = value;
		}
	}

	// Token: 0x1700071B RID: 1819
	// (get) Token: 0x06009E22 RID: 40482 RVA: 0x002D7B68 File Offset: 0x002D5D68
	// (set) Token: 0x06009E23 RID: 40483 RVA: 0x002D7B78 File Offset: 0x002D5D78
	public QueryContainer PlayerCarrierQuery
	{
		get
		{
			return this.ExposedVariables.PlayerCarrierQuery;
		}
		set
		{
			this.ExposedVariables.PlayerCarrierQuery = value;
		}
	}

	// Token: 0x1700071C RID: 1820
	// (get) Token: 0x06009E24 RID: 40484 RVA: 0x002D7B88 File Offset: 0x002D5D88
	// (set) Token: 0x06009E25 RID: 40485 RVA: 0x002D7B98 File Offset: 0x002D5D98
	public UnitSpawnWaveData GaalsienBattleCruiserWave
	{
		get
		{
			return this.ExposedVariables.GaalsienBattleCruiserWave;
		}
		set
		{
			this.ExposedVariables.GaalsienBattleCruiserWave = value;
		}
	}

	// Token: 0x1700071D RID: 1821
	// (get) Token: 0x06009E26 RID: 40486 RVA: 0x002D7BA8 File Offset: 0x002D5DA8
	// (set) Token: 0x06009E27 RID: 40487 RVA: 0x002D7BB8 File Offset: 0x002D5DB8
	public QueryContainer RachelQuery
	{
		get
		{
			return this.ExposedVariables.RachelQuery;
		}
		set
		{
			this.ExposedVariables.RachelQuery = value;
		}
	}

	// Token: 0x1700071E RID: 1822
	// (get) Token: 0x06009E28 RID: 40488 RVA: 0x002D7BC8 File Offset: 0x002D5DC8
	// (set) Token: 0x06009E29 RID: 40489 RVA: 0x002D7BD8 File Offset: 0x002D5DD8
	public QueryContainer AllGaalsienQuery
	{
		get
		{
			return this.ExposedVariables.AllGaalsienQuery;
		}
		set
		{
			this.ExposedVariables.AllGaalsienQuery = value;
		}
	}

	// Token: 0x1700071F RID: 1823
	// (get) Token: 0x06009E2A RID: 40490 RVA: 0x002D7BE8 File Offset: 0x002D5DE8
	// (set) Token: 0x06009E2B RID: 40491 RVA: 0x002D7BF8 File Offset: 0x002D5DF8
	public AttributeBuffSetData GaalsienNoDamage
	{
		get
		{
			return this.ExposedVariables.GaalsienNoDamage;
		}
		set
		{
			this.ExposedVariables.GaalsienNoDamage = value;
		}
	}

	// Token: 0x17000720 RID: 1824
	// (get) Token: 0x06009E2C RID: 40492 RVA: 0x002D7C08 File Offset: 0x002D5E08
	// (set) Token: 0x06009E2D RID: 40493 RVA: 0x002D7C18 File Offset: 0x002D5E18
	public QueryContainer EmptyQuery
	{
		get
		{
			return this.ExposedVariables.EmptyQuery;
		}
		set
		{
			this.ExposedVariables.EmptyQuery = value;
		}
	}

	// Token: 0x17000721 RID: 1825
	// (get) Token: 0x06009E2E RID: 40494 RVA: 0x002D7C28 File Offset: 0x002D5E28
	// (set) Token: 0x06009E2F RID: 40495 RVA: 0x002D7C38 File Offset: 0x002D5E38
	public QueryContainer GetFleet1ProdCruiser
	{
		get
		{
			return this.ExposedVariables.GetFleet1ProdCruiser;
		}
		set
		{
			this.ExposedVariables.GetFleet1ProdCruiser = value;
		}
	}

	// Token: 0x17000722 RID: 1826
	// (get) Token: 0x06009E30 RID: 40496 RVA: 0x002D7C48 File Offset: 0x002D5E48
	// (set) Token: 0x06009E31 RID: 40497 RVA: 0x002D7C58 File Offset: 0x002D5E58
	public QueryContainer GetFleet2ProdCruiser
	{
		get
		{
			return this.ExposedVariables.GetFleet2ProdCruiser;
		}
		set
		{
			this.ExposedVariables.GetFleet2ProdCruiser = value;
		}
	}

	// Token: 0x17000723 RID: 1827
	// (get) Token: 0x06009E32 RID: 40498 RVA: 0x002D7C68 File Offset: 0x002D5E68
	// (set) Token: 0x06009E33 RID: 40499 RVA: 0x002D7C78 File Offset: 0x002D5E78
	public QueryContainer GetAllFleet2
	{
		get
		{
			return this.ExposedVariables.GetAllFleet2;
		}
		set
		{
			this.ExposedVariables.GetAllFleet2 = value;
		}
	}

	// Token: 0x17000724 RID: 1828
	// (get) Token: 0x06009E34 RID: 40500 RVA: 0x002D7C88 File Offset: 0x002D5E88
	// (set) Token: 0x06009E35 RID: 40501 RVA: 0x002D7C98 File Offset: 0x002D5E98
	public QueryContainer GetAllFleet1
	{
		get
		{
			return this.ExposedVariables.GetAllFleet1;
		}
		set
		{
			this.ExposedVariables.GetAllFleet1 = value;
		}
	}

	// Token: 0x17000725 RID: 1829
	// (get) Token: 0x06009E36 RID: 40502 RVA: 0x002D7CA8 File Offset: 0x002D5EA8
	// (set) Token: 0x06009E37 RID: 40503 RVA: 0x002D7CB8 File Offset: 0x002D5EB8
	public SpawnFactoryData ReplenishmentFactory2
	{
		get
		{
			return this.ExposedVariables.ReplenishmentFactory2;
		}
		set
		{
			this.ExposedVariables.ReplenishmentFactory2 = value;
		}
	}

	// Token: 0x17000726 RID: 1830
	// (get) Token: 0x06009E38 RID: 40504 RVA: 0x002D7CC8 File Offset: 0x002D5EC8
	// (set) Token: 0x06009E39 RID: 40505 RVA: 0x002D7CD8 File Offset: 0x002D5ED8
	public SpawnFactoryData ReplenishmentFactory1
	{
		get
		{
			return this.ExposedVariables.ReplenishmentFactory1;
		}
		set
		{
			this.ExposedVariables.ReplenishmentFactory1 = value;
		}
	}

	// Token: 0x17000727 RID: 1831
	// (get) Token: 0x06009E3A RID: 40506 RVA: 0x002D7CE8 File Offset: 0x002D5EE8
	// (set) Token: 0x06009E3B RID: 40507 RVA: 0x002D7CF8 File Offset: 0x002D5EF8
	public AttributeBuffSetData NoAutoTargetHonourguard
	{
		get
		{
			return this.ExposedVariables.NoAutoTargetHonourguard;
		}
		set
		{
			this.ExposedVariables.NoAutoTargetHonourguard = value;
		}
	}

	// Token: 0x17000728 RID: 1832
	// (get) Token: 0x06009E3C RID: 40508 RVA: 0x002D7D08 File Offset: 0x002D5F08
	// (set) Token: 0x06009E3D RID: 40509 RVA: 0x002D7D18 File Offset: 0x002D5F18
	public AttributeBuffSetData ReturningFleetSpeedBuff
	{
		get
		{
			return this.ExposedVariables.ReturningFleetSpeedBuff;
		}
		set
		{
			this.ExposedVariables.ReturningFleetSpeedBuff = value;
		}
	}

	// Token: 0x17000729 RID: 1833
	// (get) Token: 0x06009E3E RID: 40510 RVA: 0x002D7D28 File Offset: 0x002D5F28
	// (set) Token: 0x06009E3F RID: 40511 RVA: 0x002D7D38 File Offset: 0x002D5F38
	public QueryContainer AllCommanders
	{
		get
		{
			return this.ExposedVariables.AllCommanders;
		}
		set
		{
			this.ExposedVariables.AllCommanders = value;
		}
	}

	// Token: 0x1700072A RID: 1834
	// (get) Token: 0x06009E40 RID: 40512 RVA: 0x002D7D48 File Offset: 0x002D5F48
	// (set) Token: 0x06009E41 RID: 40513 RVA: 0x002D7D58 File Offset: 0x002D5F58
	public AttributeBuffSetData OnlyNoAutoTarget
	{
		get
		{
			return this.ExposedVariables.OnlyNoAutoTarget;
		}
		set
		{
			this.ExposedVariables.OnlyNoAutoTarget = value;
		}
	}

	// Token: 0x1700072B RID: 1835
	// (get) Token: 0x06009E42 RID: 40514 RVA: 0x002D7D68 File Offset: 0x002D5F68
	// (set) Token: 0x06009E43 RID: 40515 RVA: 0x002D7D78 File Offset: 0x002D5F78
	public UnitSpawnWaveData ReturningFleetRanged
	{
		get
		{
			return this.ExposedVariables.ReturningFleetRanged;
		}
		set
		{
			this.ExposedVariables.ReturningFleetRanged = value;
		}
	}

	// Token: 0x06009E44 RID: 40516 RVA: 0x002D7D88 File Offset: 0x002D5F88
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

	// Token: 0x06009E45 RID: 40517 RVA: 0x002D7DF0 File Offset: 0x002D5FF0
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06009E46 RID: 40518 RVA: 0x002D7E00 File Offset: 0x002D6000
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06009E47 RID: 40519 RVA: 0x002D7E10 File Offset: 0x002D6010
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06009E48 RID: 40520 RVA: 0x002D7E20 File Offset: 0x002D6020
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06009E49 RID: 40521 RVA: 0x002D7E30 File Offset: 0x002D6030
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400EC25 RID: 60453
	public M06_Gameplay2 ExposedVariables = new M06_Gameplay2();
}
