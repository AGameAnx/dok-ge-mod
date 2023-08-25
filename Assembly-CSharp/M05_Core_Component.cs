using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x02000834 RID: 2100
[AddComponentMenu("uScript/Graphs/M05_Core")]
public class M05_Core_Component : uScriptCode
{
	// Token: 0x060083C2 RID: 33730 RVA: 0x00259454 File Offset: 0x00257654
	public M05_Core_Component()
	{
	}

	// Token: 0x170006A2 RID: 1698
	// (get) Token: 0x060083C3 RID: 33731 RVA: 0x00259468 File Offset: 0x00257668
	// (set) Token: 0x060083C4 RID: 33732 RVA: 0x00259478 File Offset: 0x00257678
	public UnitSpawnWaveData PlayerScannerWave
	{
		get
		{
			return this.ExposedVariables.PlayerScannerWave;
		}
		set
		{
			this.ExposedVariables.PlayerScannerWave = value;
		}
	}

	// Token: 0x170006A3 RID: 1699
	// (get) Token: 0x060083C5 RID: 33733 RVA: 0x00259488 File Offset: 0x00257688
	// (set) Token: 0x060083C6 RID: 33734 RVA: 0x00259498 File Offset: 0x00257698
	public List<Entity> Scanner_09
	{
		get
		{
			return this.ExposedVariables.Scanner_09;
		}
		set
		{
			this.ExposedVariables.Scanner_09 = value;
		}
	}

	// Token: 0x170006A4 RID: 1700
	// (get) Token: 0x060083C7 RID: 33735 RVA: 0x002594A8 File Offset: 0x002576A8
	// (set) Token: 0x060083C8 RID: 33736 RVA: 0x002594B8 File Offset: 0x002576B8
	public List<Entity> Scanner_06
	{
		get
		{
			return this.ExposedVariables.Scanner_06;
		}
		set
		{
			this.ExposedVariables.Scanner_06 = value;
		}
	}

	// Token: 0x170006A5 RID: 1701
	// (get) Token: 0x060083C9 RID: 33737 RVA: 0x002594C8 File Offset: 0x002576C8
	// (set) Token: 0x060083CA RID: 33738 RVA: 0x002594D8 File Offset: 0x002576D8
	public List<Entity> Scanner_07
	{
		get
		{
			return this.ExposedVariables.Scanner_07;
		}
		set
		{
			this.ExposedVariables.Scanner_07 = value;
		}
	}

	// Token: 0x170006A6 RID: 1702
	// (get) Token: 0x060083CB RID: 33739 RVA: 0x002594E8 File Offset: 0x002576E8
	// (set) Token: 0x060083CC RID: 33740 RVA: 0x002594F8 File Offset: 0x002576F8
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

	// Token: 0x170006A7 RID: 1703
	// (get) Token: 0x060083CD RID: 33741 RVA: 0x00259508 File Offset: 0x00257708
	// (set) Token: 0x060083CE RID: 33742 RVA: 0x00259518 File Offset: 0x00257718
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

	// Token: 0x170006A8 RID: 1704
	// (get) Token: 0x060083CF RID: 33743 RVA: 0x00259528 File Offset: 0x00257728
	// (set) Token: 0x060083D0 RID: 33744 RVA: 0x00259538 File Offset: 0x00257738
	public UnitSpawnWaveData Player_Wreck_Turrets
	{
		get
		{
			return this.ExposedVariables.Player_Wreck_Turrets;
		}
		set
		{
			this.ExposedVariables.Player_Wreck_Turrets = value;
		}
	}

	// Token: 0x170006A9 RID: 1705
	// (get) Token: 0x060083D1 RID: 33745 RVA: 0x00259548 File Offset: 0x00257748
	// (set) Token: 0x060083D2 RID: 33746 RVA: 0x00259558 File Offset: 0x00257758
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

	// Token: 0x170006AA RID: 1706
	// (get) Token: 0x060083D3 RID: 33747 RVA: 0x00259568 File Offset: 0x00257768
	// (set) Token: 0x060083D4 RID: 33748 RVA: 0x00259578 File Offset: 0x00257778
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

	// Token: 0x170006AB RID: 1707
	// (get) Token: 0x060083D5 RID: 33749 RVA: 0x00259588 File Offset: 0x00257788
	// (set) Token: 0x060083D6 RID: 33750 RVA: 0x00259598 File Offset: 0x00257798
	public AttributeBuffSetData Scanner_TurretBuff
	{
		get
		{
			return this.ExposedVariables.Scanner_TurretBuff;
		}
		set
		{
			this.ExposedVariables.Scanner_TurretBuff = value;
		}
	}

	// Token: 0x170006AC RID: 1708
	// (get) Token: 0x060083D7 RID: 33751 RVA: 0x002595A8 File Offset: 0x002577A8
	// (set) Token: 0x060083D8 RID: 33752 RVA: 0x002595B8 File Offset: 0x002577B8
	public QueryContainer SUSP_ALL
	{
		get
		{
			return this.ExposedVariables.SUSP_ALL;
		}
		set
		{
			this.ExposedVariables.SUSP_ALL = value;
		}
	}

	// Token: 0x060083D9 RID: 33753 RVA: 0x002595C8 File Offset: 0x002577C8
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

	// Token: 0x060083DA RID: 33754 RVA: 0x00259630 File Offset: 0x00257830
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x060083DB RID: 33755 RVA: 0x00259640 File Offset: 0x00257840
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x060083DC RID: 33756 RVA: 0x00259650 File Offset: 0x00257850
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x060083DD RID: 33757 RVA: 0x00259660 File Offset: 0x00257860
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x060083DE RID: 33758 RVA: 0x00259670 File Offset: 0x00257870
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400BE42 RID: 48706
	public M05_Core ExposedVariables = new M05_Core();
}
