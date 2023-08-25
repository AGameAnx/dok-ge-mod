using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x02000859 RID: 2137
[AddComponentMenu("uScript/Graphs/M07_Gameplay2")]
public class M07_Gameplay2_Component : uScriptCode
{
	// Token: 0x0600B124 RID: 45348 RVA: 0x0033475C File Offset: 0x0033295C
	public M07_Gameplay2_Component()
	{
	}

	// Token: 0x17000755 RID: 1877
	// (get) Token: 0x0600B125 RID: 45349 RVA: 0x00334770 File Offset: 0x00332970
	// (set) Token: 0x0600B126 RID: 45350 RVA: 0x00334780 File Offset: 0x00332980
	public UnitSpawnWaveData GaalsiCarrierEscortAAWaveEasy
	{
		get
		{
			return this.ExposedVariables.GaalsiCarrierEscortAAWaveEasy;
		}
		set
		{
			this.ExposedVariables.GaalsiCarrierEscortAAWaveEasy = value;
		}
	}

	// Token: 0x17000756 RID: 1878
	// (get) Token: 0x0600B127 RID: 45351 RVA: 0x00334790 File Offset: 0x00332990
	// (set) Token: 0x0600B128 RID: 45352 RVA: 0x003347A0 File Offset: 0x003329A0
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

	// Token: 0x17000757 RID: 1879
	// (get) Token: 0x0600B129 RID: 45353 RVA: 0x003347B0 File Offset: 0x003329B0
	// (set) Token: 0x0600B12A RID: 45354 RVA: 0x003347C0 File Offset: 0x003329C0
	public UnitSpawnWaveData GaalsiAircraftAttackWaveLevel2
	{
		get
		{
			return this.ExposedVariables.GaalsiAircraftAttackWaveLevel2;
		}
		set
		{
			this.ExposedVariables.GaalsiAircraftAttackWaveLevel2 = value;
		}
	}

	// Token: 0x17000758 RID: 1880
	// (get) Token: 0x0600B12B RID: 45355 RVA: 0x003347D0 File Offset: 0x003329D0
	// (set) Token: 0x0600B12C RID: 45356 RVA: 0x003347E0 File Offset: 0x003329E0
	public UnitSpawnWaveData GaalsiAircraftAttackWaveLevel3
	{
		get
		{
			return this.ExposedVariables.GaalsiAircraftAttackWaveLevel3;
		}
		set
		{
			this.ExposedVariables.GaalsiAircraftAttackWaveLevel3 = value;
		}
	}

	// Token: 0x17000759 RID: 1881
	// (get) Token: 0x0600B12D RID: 45357 RVA: 0x003347F0 File Offset: 0x003329F0
	// (set) Token: 0x0600B12E RID: 45358 RVA: 0x00334800 File Offset: 0x00332A00
	public QueryContainer GaalsiLargeAirAttackTargetQuery
	{
		get
		{
			return this.ExposedVariables.GaalsiLargeAirAttackTargetQuery;
		}
		set
		{
			this.ExposedVariables.GaalsiLargeAirAttackTargetQuery = value;
		}
	}

	// Token: 0x1700075A RID: 1882
	// (get) Token: 0x0600B12F RID: 45359 RVA: 0x00334810 File Offset: 0x00332A10
	// (set) Token: 0x0600B130 RID: 45360 RVA: 0x00334820 File Offset: 0x00332A20
	public UnitSpawnWaveData GaalsiCarrierBRsWave
	{
		get
		{
			return this.ExposedVariables.GaalsiCarrierBRsWave;
		}
		set
		{
			this.ExposedVariables.GaalsiCarrierBRsWave = value;
		}
	}

	// Token: 0x1700075B RID: 1883
	// (get) Token: 0x0600B131 RID: 45361 RVA: 0x00334830 File Offset: 0x00332A30
	// (set) Token: 0x0600B132 RID: 45362 RVA: 0x00334840 File Offset: 0x00332A40
	public QueryContainer GaalsiHarvestingTestQuery
	{
		get
		{
			return this.ExposedVariables.GaalsiHarvestingTestQuery;
		}
		set
		{
			this.ExposedVariables.GaalsiHarvestingTestQuery = value;
		}
	}

	// Token: 0x1700075C RID: 1884
	// (get) Token: 0x0600B133 RID: 45363 RVA: 0x00334850 File Offset: 0x00332A50
	// (set) Token: 0x0600B134 RID: 45364 RVA: 0x00334860 File Offset: 0x00332A60
	public UnitSpawnWaveData GaalsiCarrierWave
	{
		get
		{
			return this.ExposedVariables.GaalsiCarrierWave;
		}
		set
		{
			this.ExposedVariables.GaalsiCarrierWave = value;
		}
	}

	// Token: 0x1700075D RID: 1885
	// (get) Token: 0x0600B135 RID: 45365 RVA: 0x00334870 File Offset: 0x00332A70
	// (set) Token: 0x0600B136 RID: 45366 RVA: 0x00334880 File Offset: 0x00332A80
	public UnitSpawnWaveData GaalsiCarrierAttackWave
	{
		get
		{
			return this.ExposedVariables.GaalsiCarrierAttackWave;
		}
		set
		{
			this.ExposedVariables.GaalsiCarrierAttackWave = value;
		}
	}

	// Token: 0x1700075E RID: 1886
	// (get) Token: 0x0600B137 RID: 45367 RVA: 0x00334890 File Offset: 0x00332A90
	// (set) Token: 0x0600B138 RID: 45368 RVA: 0x003348A0 File Offset: 0x00332AA0
	public AttributeBuffSetData Buff_CarrierGuard
	{
		get
		{
			return this.ExposedVariables.Buff_CarrierGuard;
		}
		set
		{
			this.ExposedVariables.Buff_CarrierGuard = value;
		}
	}

	// Token: 0x1700075F RID: 1887
	// (get) Token: 0x0600B139 RID: 45369 RVA: 0x003348B0 File Offset: 0x00332AB0
	// (set) Token: 0x0600B13A RID: 45370 RVA: 0x003348C0 File Offset: 0x00332AC0
	public UnitSpawnWaveData LargeGaalsiAircraftWave
	{
		get
		{
			return this.ExposedVariables.LargeGaalsiAircraftWave;
		}
		set
		{
			this.ExposedVariables.LargeGaalsiAircraftWave = value;
		}
	}

	// Token: 0x17000760 RID: 1888
	// (get) Token: 0x0600B13B RID: 45371 RVA: 0x003348D0 File Offset: 0x00332AD0
	// (set) Token: 0x0600B13C RID: 45372 RVA: 0x003348E0 File Offset: 0x00332AE0
	public UnitSpawnWaveData GaalsiCarrierEscortWave
	{
		get
		{
			return this.ExposedVariables.GaalsiCarrierEscortWave;
		}
		set
		{
			this.ExposedVariables.GaalsiCarrierEscortWave = value;
		}
	}

	// Token: 0x17000761 RID: 1889
	// (get) Token: 0x0600B13D RID: 45373 RVA: 0x003348F0 File Offset: 0x00332AF0
	// (set) Token: 0x0600B13E RID: 45374 RVA: 0x00334900 File Offset: 0x00332B00
	public UnitSpawnWaveData GaalsiCarrierAttackWaveHard
	{
		get
		{
			return this.ExposedVariables.GaalsiCarrierAttackWaveHard;
		}
		set
		{
			this.ExposedVariables.GaalsiCarrierAttackWaveHard = value;
		}
	}

	// Token: 0x17000762 RID: 1890
	// (get) Token: 0x0600B13F RID: 45375 RVA: 0x00334910 File Offset: 0x00332B10
	// (set) Token: 0x0600B140 RID: 45376 RVA: 0x00334920 File Offset: 0x00332B20
	public UnitSpawnWaveData GaalsiCarrierAttackWaveEasy
	{
		get
		{
			return this.ExposedVariables.GaalsiCarrierAttackWaveEasy;
		}
		set
		{
			this.ExposedVariables.GaalsiCarrierAttackWaveEasy = value;
		}
	}

	// Token: 0x17000763 RID: 1891
	// (get) Token: 0x0600B141 RID: 45377 RVA: 0x00334930 File Offset: 0x00332B30
	// (set) Token: 0x0600B142 RID: 45378 RVA: 0x00334940 File Offset: 0x00332B40
	public UnitSpawnWaveData GaalsiCarrierEscortAAWave
	{
		get
		{
			return this.ExposedVariables.GaalsiCarrierEscortAAWave;
		}
		set
		{
			this.ExposedVariables.GaalsiCarrierEscortAAWave = value;
		}
	}

	// Token: 0x17000764 RID: 1892
	// (get) Token: 0x0600B143 RID: 45379 RVA: 0x00334950 File Offset: 0x00332B50
	// (set) Token: 0x0600B144 RID: 45380 RVA: 0x00334960 File Offset: 0x00332B60
	public UnitSpawnWaveData GaalsiCarrierEscortAAWaveHard
	{
		get
		{
			return this.ExposedVariables.GaalsiCarrierEscortAAWaveHard;
		}
		set
		{
			this.ExposedVariables.GaalsiCarrierEscortAAWaveHard = value;
		}
	}

	// Token: 0x17000765 RID: 1893
	// (get) Token: 0x0600B145 RID: 45381 RVA: 0x00334970 File Offset: 0x00332B70
	// (set) Token: 0x0600B146 RID: 45382 RVA: 0x00334980 File Offset: 0x00332B80
	public List<Entity> GaalsiCarrier1
	{
		get
		{
			return this.ExposedVariables.GaalsiCarrier1;
		}
		set
		{
			this.ExposedVariables.GaalsiCarrier1 = value;
		}
	}

	// Token: 0x0600B147 RID: 45383 RVA: 0x00334990 File Offset: 0x00332B90
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

	// Token: 0x0600B148 RID: 45384 RVA: 0x003349F8 File Offset: 0x00332BF8
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0600B149 RID: 45385 RVA: 0x00334A08 File Offset: 0x00332C08
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600B14A RID: 45386 RVA: 0x00334A18 File Offset: 0x00332C18
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600B14B RID: 45387 RVA: 0x00334A28 File Offset: 0x00332C28
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x0600B14C RID: 45388 RVA: 0x00334A38 File Offset: 0x00332C38
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04010F2C RID: 69420
	public M07_Gameplay2 ExposedVariables = new M07_Gameplay2();
}
