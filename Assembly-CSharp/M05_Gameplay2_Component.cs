using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x0200083D RID: 2109
[AddComponentMenu("uScript/Graphs/M05_Gameplay2")]
public class M05_Gameplay2_Component : uScriptCode
{
	// Token: 0x06008D74 RID: 36212 RVA: 0x00289350 File Offset: 0x00287550
	public M05_Gameplay2_Component()
	{
	}

	// Token: 0x170006B9 RID: 1721
	// (get) Token: 0x06008D75 RID: 36213 RVA: 0x00289364 File Offset: 0x00287564
	// (set) Token: 0x06008D76 RID: 36214 RVA: 0x00289374 File Offset: 0x00287574
	public int MinesPlaced
	{
		get
		{
			return this.ExposedVariables.MinesPlaced;
		}
		set
		{
			this.ExposedVariables.MinesPlaced = value;
		}
	}

	// Token: 0x170006BA RID: 1722
	// (get) Token: 0x06008D77 RID: 36215 RVA: 0x00289384 File Offset: 0x00287584
	// (set) Token: 0x06008D78 RID: 36216 RVA: 0x00289394 File Offset: 0x00287594
	public UnitSpawnWaveData Gaalsi_Carrier
	{
		get
		{
			return this.ExposedVariables.Gaalsi_Carrier;
		}
		set
		{
			this.ExposedVariables.Gaalsi_Carrier = value;
		}
	}

	// Token: 0x170006BB RID: 1723
	// (get) Token: 0x06008D79 RID: 36217 RVA: 0x002893A4 File Offset: 0x002875A4
	// (set) Token: 0x06008D7A RID: 36218 RVA: 0x002893B4 File Offset: 0x002875B4
	public UnitSpawnWaveData Siidim_Carrier
	{
		get
		{
			return this.ExposedVariables.Siidim_Carrier;
		}
		set
		{
			this.ExposedVariables.Siidim_Carrier = value;
		}
	}

	// Token: 0x170006BC RID: 1724
	// (get) Token: 0x06008D7B RID: 36219 RVA: 0x002893C4 File Offset: 0x002875C4
	// (set) Token: 0x06008D7C RID: 36220 RVA: 0x002893D4 File Offset: 0x002875D4
	public UnitSpawnWaveData Siidim_Defenders
	{
		get
		{
			return this.ExposedVariables.Siidim_Defenders;
		}
		set
		{
			this.ExposedVariables.Siidim_Defenders = value;
		}
	}

	// Token: 0x170006BD RID: 1725
	// (get) Token: 0x06008D7D RID: 36221 RVA: 0x002893E4 File Offset: 0x002875E4
	// (set) Token: 0x06008D7E RID: 36222 RVA: 0x002893F4 File Offset: 0x002875F4
	public List<Entity> S_Carrier
	{
		get
		{
			return this.ExposedVariables.S_Carrier;
		}
		set
		{
			this.ExposedVariables.S_Carrier = value;
		}
	}

	// Token: 0x170006BE RID: 1726
	// (get) Token: 0x06008D7F RID: 36223 RVA: 0x00289404 File Offset: 0x00287604
	// (set) Token: 0x06008D80 RID: 36224 RVA: 0x00289414 File Offset: 0x00287614
	public string[] M05_Gaalsi_Carrier
	{
		get
		{
			return this.ExposedVariables.M05_Gaalsi_Carrier;
		}
		set
		{
			this.ExposedVariables.M05_Gaalsi_Carrier = value;
		}
	}

	// Token: 0x170006BF RID: 1727
	// (get) Token: 0x06008D81 RID: 36225 RVA: 0x00289424 File Offset: 0x00287624
	// (set) Token: 0x06008D82 RID: 36226 RVA: 0x00289434 File Offset: 0x00287634
	public UnitSpawnWaveData GaalsiFinalWave_Epic1
	{
		get
		{
			return this.ExposedVariables.GaalsiFinalWave_Epic1;
		}
		set
		{
			this.ExposedVariables.GaalsiFinalWave_Epic1 = value;
		}
	}

	// Token: 0x170006C0 RID: 1728
	// (get) Token: 0x06008D83 RID: 36227 RVA: 0x00289444 File Offset: 0x00287644
	// (set) Token: 0x06008D84 RID: 36228 RVA: 0x00289454 File Offset: 0x00287654
	public UnitSpawnWaveData Wreck2_Gaalsi
	{
		get
		{
			return this.ExposedVariables.Wreck2_Gaalsi;
		}
		set
		{
			this.ExposedVariables.Wreck2_Gaalsi = value;
		}
	}

	// Token: 0x170006C1 RID: 1729
	// (get) Token: 0x06008D85 RID: 36229 RVA: 0x00289464 File Offset: 0x00287664
	// (set) Token: 0x06008D86 RID: 36230 RVA: 0x00289474 File Offset: 0x00287674
	public UnitSpawnWaveData Wreck3_Gaalsi
	{
		get
		{
			return this.ExposedVariables.Wreck3_Gaalsi;
		}
		set
		{
			this.ExposedVariables.Wreck3_Gaalsi = value;
		}
	}

	// Token: 0x170006C2 RID: 1730
	// (get) Token: 0x06008D87 RID: 36231 RVA: 0x00289484 File Offset: 0x00287684
	// (set) Token: 0x06008D88 RID: 36232 RVA: 0x00289494 File Offset: 0x00287694
	public UnitSpawnWaveData Wreck4_Gaalsi
	{
		get
		{
			return this.ExposedVariables.Wreck4_Gaalsi;
		}
		set
		{
			this.ExposedVariables.Wreck4_Gaalsi = value;
		}
	}

	// Token: 0x170006C3 RID: 1731
	// (get) Token: 0x06008D89 RID: 36233 RVA: 0x002894A4 File Offset: 0x002876A4
	// (set) Token: 0x06008D8A RID: 36234 RVA: 0x002894B4 File Offset: 0x002876B4
	public QueryContainer M05_Player_Carrier
	{
		get
		{
			return this.ExposedVariables.M05_Player_Carrier;
		}
		set
		{
			this.ExposedVariables.M05_Player_Carrier = value;
		}
	}

	// Token: 0x170006C4 RID: 1732
	// (get) Token: 0x06008D8B RID: 36235 RVA: 0x002894C4 File Offset: 0x002876C4
	// (set) Token: 0x06008D8C RID: 36236 RVA: 0x002894D4 File Offset: 0x002876D4
	public UnitSpawnWaveData GaalsiInitialWave_01
	{
		get
		{
			return this.ExposedVariables.GaalsiInitialWave_01;
		}
		set
		{
			this.ExposedVariables.GaalsiInitialWave_01 = value;
		}
	}

	// Token: 0x170006C5 RID: 1733
	// (get) Token: 0x06008D8D RID: 36237 RVA: 0x002894E4 File Offset: 0x002876E4
	// (set) Token: 0x06008D8E RID: 36238 RVA: 0x002894F4 File Offset: 0x002876F4
	public UnitSpawnWaveData Gaalsi_Probe
	{
		get
		{
			return this.ExposedVariables.Gaalsi_Probe;
		}
		set
		{
			this.ExposedVariables.Gaalsi_Probe = value;
		}
	}

	// Token: 0x170006C6 RID: 1734
	// (get) Token: 0x06008D8F RID: 36239 RVA: 0x00289504 File Offset: 0x00287704
	// (set) Token: 0x06008D90 RID: 36240 RVA: 0x00289514 File Offset: 0x00287714
	public AttributeBuffSetData SpeedAndInvulnerableBUFF
	{
		get
		{
			return this.ExposedVariables.SpeedAndInvulnerableBUFF;
		}
		set
		{
			this.ExposedVariables.SpeedAndInvulnerableBUFF = value;
		}
	}

	// Token: 0x170006C7 RID: 1735
	// (get) Token: 0x06008D91 RID: 36241 RVA: 0x00289524 File Offset: 0x00287724
	// (set) Token: 0x06008D92 RID: 36242 RVA: 0x00289534 File Offset: 0x00287734
	public QueryContainer Remove_Ground
	{
		get
		{
			return this.ExposedVariables.Remove_Ground;
		}
		set
		{
			this.ExposedVariables.Remove_Ground = value;
		}
	}

	// Token: 0x170006C8 RID: 1736
	// (get) Token: 0x06008D93 RID: 36243 RVA: 0x00289544 File Offset: 0x00287744
	// (set) Token: 0x06008D94 RID: 36244 RVA: 0x00289554 File Offset: 0x00287754
	public QueryContainer Remove_KapisiAttackers
	{
		get
		{
			return this.ExposedVariables.Remove_KapisiAttackers;
		}
		set
		{
			this.ExposedVariables.Remove_KapisiAttackers = value;
		}
	}

	// Token: 0x170006C9 RID: 1737
	// (get) Token: 0x06008D95 RID: 36245 RVA: 0x00289564 File Offset: 0x00287764
	// (set) Token: 0x06008D96 RID: 36246 RVA: 0x00289574 File Offset: 0x00287774
	public QueryContainer Remove_WreckGroups
	{
		get
		{
			return this.ExposedVariables.Remove_WreckGroups;
		}
		set
		{
			this.ExposedVariables.Remove_WreckGroups = value;
		}
	}

	// Token: 0x170006CA RID: 1738
	// (get) Token: 0x06008D97 RID: 36247 RVA: 0x00289584 File Offset: 0x00287784
	// (set) Token: 0x06008D98 RID: 36248 RVA: 0x00289594 File Offset: 0x00287794
	public QueryContainer Remove_ProbeGroups
	{
		get
		{
			return this.ExposedVariables.Remove_ProbeGroups;
		}
		set
		{
			this.ExposedVariables.Remove_ProbeGroups = value;
		}
	}

	// Token: 0x170006CB RID: 1739
	// (get) Token: 0x06008D99 RID: 36249 RVA: 0x002895A4 File Offset: 0x002877A4
	// (set) Token: 0x06008D9A RID: 36250 RVA: 0x002895B4 File Offset: 0x002877B4
	public QueryContainer APU
	{
		get
		{
			return this.ExposedVariables.APU;
		}
		set
		{
			this.ExposedVariables.APU = value;
		}
	}

	// Token: 0x170006CC RID: 1740
	// (get) Token: 0x06008D9B RID: 36251 RVA: 0x002895C4 File Offset: 0x002877C4
	// (set) Token: 0x06008D9C RID: 36252 RVA: 0x002895D4 File Offset: 0x002877D4
	public int Wrecks_Remaining
	{
		get
		{
			return this.ExposedVariables.Wrecks_Remaining;
		}
		set
		{
			this.ExposedVariables.Wrecks_Remaining = value;
		}
	}

	// Token: 0x170006CD RID: 1741
	// (get) Token: 0x06008D9D RID: 36253 RVA: 0x002895E4 File Offset: 0x002877E4
	// (set) Token: 0x06008D9E RID: 36254 RVA: 0x002895F4 File Offset: 0x002877F4
	public QueryContainer SUSP
	{
		get
		{
			return this.ExposedVariables.SUSP;
		}
		set
		{
			this.ExposedVariables.SUSP = value;
		}
	}

	// Token: 0x170006CE RID: 1742
	// (get) Token: 0x06008D9F RID: 36255 RVA: 0x00289604 File Offset: 0x00287804
	// (set) Token: 0x06008DA0 RID: 36256 RVA: 0x00289614 File Offset: 0x00287814
	public QueryContainer GOD
	{
		get
		{
			return this.ExposedVariables.GOD;
		}
		set
		{
			this.ExposedVariables.GOD = value;
		}
	}

	// Token: 0x170006CF RID: 1743
	// (get) Token: 0x06008DA1 RID: 36257 RVA: 0x00289624 File Offset: 0x00287824
	// (set) Token: 0x06008DA2 RID: 36258 RVA: 0x00289634 File Offset: 0x00287834
	public UnitSpawnWaveData Siddim_Burger
	{
		get
		{
			return this.ExposedVariables.Siddim_Burger;
		}
		set
		{
			this.ExposedVariables.Siddim_Burger = value;
		}
	}

	// Token: 0x170006D0 RID: 1744
	// (get) Token: 0x06008DA3 RID: 36259 RVA: 0x00289644 File Offset: 0x00287844
	// (set) Token: 0x06008DA4 RID: 36260 RVA: 0x00289654 File Offset: 0x00287854
	public UnitSpawnWaveData Gaalsi_Burger
	{
		get
		{
			return this.ExposedVariables.Gaalsi_Burger;
		}
		set
		{
			this.ExposedVariables.Gaalsi_Burger = value;
		}
	}

	// Token: 0x170006D1 RID: 1745
	// (get) Token: 0x06008DA5 RID: 36261 RVA: 0x00289664 File Offset: 0x00287864
	// (set) Token: 0x06008DA6 RID: 36262 RVA: 0x00289674 File Offset: 0x00287874
	public AttributeBuffSetData Siddim_Buff
	{
		get
		{
			return this.ExposedVariables.Siddim_Buff;
		}
		set
		{
			this.ExposedVariables.Siddim_Buff = value;
		}
	}

	// Token: 0x170006D2 RID: 1746
	// (get) Token: 0x06008DA7 RID: 36263 RVA: 0x00289684 File Offset: 0x00287884
	// (set) Token: 0x06008DA8 RID: 36264 RVA: 0x00289694 File Offset: 0x00287894
	public UnitSpawnWaveData GaalsiFinalWave_EpicWeakWave
	{
		get
		{
			return this.ExposedVariables.GaalsiFinalWave_EpicWeakWave;
		}
		set
		{
			this.ExposedVariables.GaalsiFinalWave_EpicWeakWave = value;
		}
	}

	// Token: 0x170006D3 RID: 1747
	// (get) Token: 0x06008DA9 RID: 36265 RVA: 0x002896A4 File Offset: 0x002878A4
	// (set) Token: 0x06008DAA RID: 36266 RVA: 0x002896B4 File Offset: 0x002878B4
	public QueryContainer SUSP_ALLCOMMANDERS
	{
		get
		{
			return this.ExposedVariables.SUSP_ALLCOMMANDERS;
		}
		set
		{
			this.ExposedVariables.SUSP_ALLCOMMANDERS = value;
		}
	}

	// Token: 0x170006D4 RID: 1748
	// (get) Token: 0x06008DAB RID: 36267 RVA: 0x002896C4 File Offset: 0x002878C4
	// (set) Token: 0x06008DAC RID: 36268 RVA: 0x002896D4 File Offset: 0x002878D4
	public List<Entity> G_Carrier
	{
		get
		{
			return this.ExposedVariables.G_Carrier;
		}
		set
		{
			this.ExposedVariables.G_Carrier = value;
		}
	}

	// Token: 0x170006D5 RID: 1749
	// (get) Token: 0x06008DAD RID: 36269 RVA: 0x002896E4 File Offset: 0x002878E4
	// (set) Token: 0x06008DAE RID: 36270 RVA: 0x002896F4 File Offset: 0x002878F4
	public UnitSpawnWaveData GCarrier_EMH_Defenders
	{
		get
		{
			return this.ExposedVariables.GCarrier_EMH_Defenders;
		}
		set
		{
			this.ExposedVariables.GCarrier_EMH_Defenders = value;
		}
	}

	// Token: 0x170006D6 RID: 1750
	// (get) Token: 0x06008DAF RID: 36271 RVA: 0x00289704 File Offset: 0x00287904
	// (set) Token: 0x06008DB0 RID: 36272 RVA: 0x00289714 File Offset: 0x00287914
	public QueryContainer Sid_Query
	{
		get
		{
			return this.ExposedVariables.Sid_Query;
		}
		set
		{
			this.ExposedVariables.Sid_Query = value;
		}
	}

	// Token: 0x170006D7 RID: 1751
	// (get) Token: 0x06008DB1 RID: 36273 RVA: 0x00289724 File Offset: 0x00287924
	// (set) Token: 0x06008DB2 RID: 36274 RVA: 0x00289734 File Offset: 0x00287934
	public UnitSpawnWaveData Siidim_Popcorn
	{
		get
		{
			return this.ExposedVariables.Siidim_Popcorn;
		}
		set
		{
			this.ExposedVariables.Siidim_Popcorn = value;
		}
	}

	// Token: 0x170006D8 RID: 1752
	// (get) Token: 0x06008DB3 RID: 36275 RVA: 0x00289744 File Offset: 0x00287944
	// (set) Token: 0x06008DB4 RID: 36276 RVA: 0x00289754 File Offset: 0x00287954
	public AttributeBuffSetData SidPopcornBuff
	{
		get
		{
			return this.ExposedVariables.SidPopcornBuff;
		}
		set
		{
			this.ExposedVariables.SidPopcornBuff = value;
		}
	}

	// Token: 0x170006D9 RID: 1753
	// (get) Token: 0x06008DB5 RID: 36277 RVA: 0x00289764 File Offset: 0x00287964
	// (set) Token: 0x06008DB6 RID: 36278 RVA: 0x00289774 File Offset: 0x00287974
	public AttributeBuffSetData ProductionBuff
	{
		get
		{
			return this.ExposedVariables.ProductionBuff;
		}
		set
		{
			this.ExposedVariables.ProductionBuff = value;
		}
	}

	// Token: 0x170006DA RID: 1754
	// (get) Token: 0x06008DB7 RID: 36279 RVA: 0x00289784 File Offset: 0x00287984
	// (set) Token: 0x06008DB8 RID: 36280 RVA: 0x00289794 File Offset: 0x00287994
	public QueryContainer Gaalsien_Carrier_Query
	{
		get
		{
			return this.ExposedVariables.Gaalsien_Carrier_Query;
		}
		set
		{
			this.ExposedVariables.Gaalsien_Carrier_Query = value;
		}
	}

	// Token: 0x170006DB RID: 1755
	// (get) Token: 0x06008DB9 RID: 36281 RVA: 0x002897A4 File Offset: 0x002879A4
	// (set) Token: 0x06008DBA RID: 36282 RVA: 0x002897B4 File Offset: 0x002879B4
	public AttributeBuffSetData LeashDebuff
	{
		get
		{
			return this.ExposedVariables.LeashDebuff;
		}
		set
		{
			this.ExposedVariables.LeashDebuff = value;
		}
	}

	// Token: 0x170006DC RID: 1756
	// (get) Token: 0x06008DBB RID: 36283 RVA: 0x002897C4 File Offset: 0x002879C4
	// (set) Token: 0x06008DBC RID: 36284 RVA: 0x002897D4 File Offset: 0x002879D4
	public BuffSetAttributesAsset Buff_GaalsiAA
	{
		get
		{
			return this.ExposedVariables.Buff_GaalsiAA;
		}
		set
		{
			this.ExposedVariables.Buff_GaalsiAA = value;
		}
	}

	// Token: 0x06008DBD RID: 36285 RVA: 0x002897E4 File Offset: 0x002879E4
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

	// Token: 0x06008DBE RID: 36286 RVA: 0x0028984C File Offset: 0x00287A4C
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06008DBF RID: 36287 RVA: 0x0028985C File Offset: 0x00287A5C
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06008DC0 RID: 36288 RVA: 0x0028986C File Offset: 0x00287A6C
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06008DC1 RID: 36289 RVA: 0x0028987C File Offset: 0x00287A7C
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06008DC2 RID: 36290 RVA: 0x0028988C File Offset: 0x00287A8C
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400CF88 RID: 53128
	public M05_Gameplay2 ExposedVariables = new M05_Gameplay2();
}
