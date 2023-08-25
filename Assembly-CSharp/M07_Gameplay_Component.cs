using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Scripting;
using UnityEngine;

// Token: 0x0200085A RID: 2138
[AddComponentMenu("uScript/Graphs/M07_Gameplay")]
public class M07_Gameplay_Component : uScriptCode
{
	// Token: 0x0600B14D RID: 45389 RVA: 0x00334A48 File Offset: 0x00332C48
	public M07_Gameplay_Component()
	{
	}

	// Token: 0x17000766 RID: 1894
	// (get) Token: 0x0600B14E RID: 45390 RVA: 0x00334A5C File Offset: 0x00332C5C
	// (set) Token: 0x0600B14F RID: 45391 RVA: 0x00334A6C File Offset: 0x00332C6C
	public UnitSpawnWaveData GaalsiGuardPost2Wave
	{
		get
		{
			return this.ExposedVariables.GaalsiGuardPost2Wave;
		}
		set
		{
			this.ExposedVariables.GaalsiGuardPost2Wave = value;
		}
	}

	// Token: 0x17000767 RID: 1895
	// (get) Token: 0x0600B150 RID: 45392 RVA: 0x00334A7C File Offset: 0x00332C7C
	// (set) Token: 0x0600B151 RID: 45393 RVA: 0x00334A8C File Offset: 0x00332C8C
	public UnitSpawnWaveData GaalsiGuardPost3Wave
	{
		get
		{
			return this.ExposedVariables.GaalsiGuardPost3Wave;
		}
		set
		{
			this.ExposedVariables.GaalsiGuardPost3Wave = value;
		}
	}

	// Token: 0x17000768 RID: 1896
	// (get) Token: 0x0600B152 RID: 45394 RVA: 0x00334A9C File Offset: 0x00332C9C
	// (set) Token: 0x0600B153 RID: 45395 RVA: 0x00334AAC File Offset: 0x00332CAC
	public UnitSpawnWaveData GaalsiPatrolWave
	{
		get
		{
			return this.ExposedVariables.GaalsiPatrolWave;
		}
		set
		{
			this.ExposedVariables.GaalsiPatrolWave = value;
		}
	}

	// Token: 0x17000769 RID: 1897
	// (get) Token: 0x0600B154 RID: 45396 RVA: 0x00334ABC File Offset: 0x00332CBC
	// (set) Token: 0x0600B155 RID: 45397 RVA: 0x00334ACC File Offset: 0x00332CCC
	public List<Entity> GaalsiPatrol3
	{
		get
		{
			return this.ExposedVariables.GaalsiPatrol3;
		}
		set
		{
			this.ExposedVariables.GaalsiPatrol3 = value;
		}
	}

	// Token: 0x1700076A RID: 1898
	// (get) Token: 0x0600B156 RID: 45398 RVA: 0x00334ADC File Offset: 0x00332CDC
	// (set) Token: 0x0600B157 RID: 45399 RVA: 0x00334AEC File Offset: 0x00332CEC
	public List<Entity> GaalsiPatrol2
	{
		get
		{
			return this.ExposedVariables.GaalsiPatrol2;
		}
		set
		{
			this.ExposedVariables.GaalsiPatrol2 = value;
		}
	}

	// Token: 0x1700076B RID: 1899
	// (get) Token: 0x0600B158 RID: 45400 RVA: 0x00334AFC File Offset: 0x00332CFC
	// (set) Token: 0x0600B159 RID: 45401 RVA: 0x00334B0C File Offset: 0x00332D0C
	public UnitSpawnWaveData GaalsiGuardPost1Wave
	{
		get
		{
			return this.ExposedVariables.GaalsiGuardPost1Wave;
		}
		set
		{
			this.ExposedVariables.GaalsiGuardPost1Wave = value;
		}
	}

	// Token: 0x1700076C RID: 1900
	// (get) Token: 0x0600B15A RID: 45402 RVA: 0x00334B1C File Offset: 0x00332D1C
	// (set) Token: 0x0600B15B RID: 45403 RVA: 0x00334B2C File Offset: 0x00332D2C
	public UnitSpawnWaveData GaalsiGuardPost4Wave
	{
		get
		{
			return this.ExposedVariables.GaalsiGuardPost4Wave;
		}
		set
		{
			this.ExposedVariables.GaalsiGuardPost4Wave = value;
		}
	}

	// Token: 0x1700076D RID: 1901
	// (get) Token: 0x0600B15C RID: 45404 RVA: 0x00334B3C File Offset: 0x00332D3C
	// (set) Token: 0x0600B15D RID: 45405 RVA: 0x00334B4C File Offset: 0x00332D4C
	public UnitSpawnWaveData GaalsiResourceGroupsBRsWave
	{
		get
		{
			return this.ExposedVariables.GaalsiResourceGroupsBRsWave;
		}
		set
		{
			this.ExposedVariables.GaalsiResourceGroupsBRsWave = value;
		}
	}

	// Token: 0x1700076E RID: 1902
	// (get) Token: 0x0600B15E RID: 45406 RVA: 0x00334B5C File Offset: 0x00332D5C
	// (set) Token: 0x0600B15F RID: 45407 RVA: 0x00334B6C File Offset: 0x00332D6C
	public IEnumerable<Entity> GaalsiResourceGroupAll
	{
		get
		{
			return this.ExposedVariables.GaalsiResourceGroupAll;
		}
		set
		{
			this.ExposedVariables.GaalsiResourceGroupAll = value;
		}
	}

	// Token: 0x1700076F RID: 1903
	// (get) Token: 0x0600B160 RID: 45408 RVA: 0x00334B7C File Offset: 0x00332D7C
	// (set) Token: 0x0600B161 RID: 45409 RVA: 0x00334B8C File Offset: 0x00332D8C
	public IEnumerable<Entity> GaalsiEscortsGroupAll
	{
		get
		{
			return this.ExposedVariables.GaalsiEscortsGroupAll;
		}
		set
		{
			this.ExposedVariables.GaalsiEscortsGroupAll = value;
		}
	}

	// Token: 0x17000770 RID: 1904
	// (get) Token: 0x0600B162 RID: 45410 RVA: 0x00334B9C File Offset: 0x00332D9C
	// (set) Token: 0x0600B163 RID: 45411 RVA: 0x00334BAC File Offset: 0x00332DAC
	public UnitSpawnWaveData GaalsiGuardPost5Wave
	{
		get
		{
			return this.ExposedVariables.GaalsiGuardPost5Wave;
		}
		set
		{
			this.ExposedVariables.GaalsiGuardPost5Wave = value;
		}
	}

	// Token: 0x17000771 RID: 1905
	// (get) Token: 0x0600B164 RID: 45412 RVA: 0x00334BBC File Offset: 0x00332DBC
	// (set) Token: 0x0600B165 RID: 45413 RVA: 0x00334BCC File Offset: 0x00332DCC
	public UnitSpawnWaveData GaalsiResourceGroupsSCWave
	{
		get
		{
			return this.ExposedVariables.GaalsiResourceGroupsSCWave;
		}
		set
		{
			this.ExposedVariables.GaalsiResourceGroupsSCWave = value;
		}
	}

	// Token: 0x17000772 RID: 1906
	// (get) Token: 0x0600B166 RID: 45414 RVA: 0x00334BDC File Offset: 0x00332DDC
	// (set) Token: 0x0600B167 RID: 45415 RVA: 0x00334BEC File Offset: 0x00332DEC
	public QueryContainer GaalsiBaserunnerQuery
	{
		get
		{
			return this.ExposedVariables.GaalsiBaserunnerQuery;
		}
		set
		{
			this.ExposedVariables.GaalsiBaserunnerQuery = value;
		}
	}

	// Token: 0x17000773 RID: 1907
	// (get) Token: 0x0600B168 RID: 45416 RVA: 0x00334BFC File Offset: 0x00332DFC
	// (set) Token: 0x0600B169 RID: 45417 RVA: 0x00334C0C File Offset: 0x00332E0C
	public AttributeBuffSetData Buff_GaalsiPatrol
	{
		get
		{
			return this.ExposedVariables.Buff_GaalsiPatrol;
		}
		set
		{
			this.ExposedVariables.Buff_GaalsiPatrol = value;
		}
	}

	// Token: 0x17000774 RID: 1908
	// (get) Token: 0x0600B16A RID: 45418 RVA: 0x00334C1C File Offset: 0x00332E1C
	// (set) Token: 0x0600B16B RID: 45419 RVA: 0x00334C2C File Offset: 0x00332E2C
	public List<Entity> NIS01SingleBaserunner
	{
		get
		{
			return this.ExposedVariables.NIS01SingleBaserunner;
		}
		set
		{
			this.ExposedVariables.NIS01SingleBaserunner = value;
		}
	}

	// Token: 0x17000775 RID: 1909
	// (get) Token: 0x0600B16C RID: 45420 RVA: 0x00334C3C File Offset: 0x00332E3C
	// (set) Token: 0x0600B16D RID: 45421 RVA: 0x00334C4C File Offset: 0x00332E4C
	public int NumGaalsiSCsKilled
	{
		get
		{
			return this.ExposedVariables.NumGaalsiSCsKilled;
		}
		set
		{
			this.ExposedVariables.NumGaalsiSCsKilled = value;
		}
	}

	// Token: 0x17000776 RID: 1910
	// (get) Token: 0x0600B16E RID: 45422 RVA: 0x00334C5C File Offset: 0x00332E5C
	// (set) Token: 0x0600B16F RID: 45423 RVA: 0x00334C6C File Offset: 0x00332E6C
	public UnitSpawnWaveData GaalsiResourcerReinforcementWave
	{
		get
		{
			return this.ExposedVariables.GaalsiResourcerReinforcementWave;
		}
		set
		{
			this.ExposedVariables.GaalsiResourcerReinforcementWave = value;
		}
	}

	// Token: 0x17000777 RID: 1911
	// (get) Token: 0x0600B170 RID: 45424 RVA: 0x00334C7C File Offset: 0x00332E7C
	// (set) Token: 0x0600B171 RID: 45425 RVA: 0x00334C8C File Offset: 0x00332E8C
	public List<Entity> GaalsiResourceGroup1SC
	{
		get
		{
			return this.ExposedVariables.GaalsiResourceGroup1SC;
		}
		set
		{
			this.ExposedVariables.GaalsiResourceGroup1SC = value;
		}
	}

	// Token: 0x17000778 RID: 1912
	// (get) Token: 0x0600B172 RID: 45426 RVA: 0x00334C9C File Offset: 0x00332E9C
	// (set) Token: 0x0600B173 RID: 45427 RVA: 0x00334CAC File Offset: 0x00332EAC
	public List<Entity> GaalsiResourceGroup2SC
	{
		get
		{
			return this.ExposedVariables.GaalsiResourceGroup2SC;
		}
		set
		{
			this.ExposedVariables.GaalsiResourceGroup2SC = value;
		}
	}

	// Token: 0x17000779 RID: 1913
	// (get) Token: 0x0600B174 RID: 45428 RVA: 0x00334CBC File Offset: 0x00332EBC
	// (set) Token: 0x0600B175 RID: 45429 RVA: 0x00334CCC File Offset: 0x00332ECC
	public List<Entity> GaalsiResourceGroup3SC
	{
		get
		{
			return this.ExposedVariables.GaalsiResourceGroup3SC;
		}
		set
		{
			this.ExposedVariables.GaalsiResourceGroup3SC = value;
		}
	}

	// Token: 0x1700077A RID: 1914
	// (get) Token: 0x0600B176 RID: 45430 RVA: 0x00334CDC File Offset: 0x00332EDC
	// (set) Token: 0x0600B177 RID: 45431 RVA: 0x00334CEC File Offset: 0x00332EEC
	public QueryContainer GaalsAircraftTargetQuery
	{
		get
		{
			return this.ExposedVariables.GaalsAircraftTargetQuery;
		}
		set
		{
			this.ExposedVariables.GaalsAircraftTargetQuery = value;
		}
	}

	// Token: 0x1700077B RID: 1915
	// (get) Token: 0x0600B178 RID: 45432 RVA: 0x00334CFC File Offset: 0x00332EFC
	// (set) Token: 0x0600B179 RID: 45433 RVA: 0x00334D0C File Offset: 0x00332F0C
	public UnitSpawnWaveData GaalsiInterceptorWaveNoCarrier1
	{
		get
		{
			return this.ExposedVariables.GaalsiInterceptorWaveNoCarrier1;
		}
		set
		{
			this.ExposedVariables.GaalsiInterceptorWaveNoCarrier1 = value;
		}
	}

	// Token: 0x1700077C RID: 1916
	// (get) Token: 0x0600B17A RID: 45434 RVA: 0x00334D1C File Offset: 0x00332F1C
	// (set) Token: 0x0600B17B RID: 45435 RVA: 0x00334D2C File Offset: 0x00332F2C
	public IEnumerable<Entity> GaalsiResourceGroup3
	{
		get
		{
			return this.ExposedVariables.GaalsiResourceGroup3;
		}
		set
		{
			this.ExposedVariables.GaalsiResourceGroup3 = value;
		}
	}

	// Token: 0x1700077D RID: 1917
	// (get) Token: 0x0600B17C RID: 45436 RVA: 0x00334D3C File Offset: 0x00332F3C
	// (set) Token: 0x0600B17D RID: 45437 RVA: 0x00334D4C File Offset: 0x00332F4C
	public List<Entity> GaalsiPatrol1
	{
		get
		{
			return this.ExposedVariables.GaalsiPatrol1;
		}
		set
		{
			this.ExposedVariables.GaalsiPatrol1 = value;
		}
	}

	// Token: 0x1700077E RID: 1918
	// (get) Token: 0x0600B17E RID: 45438 RVA: 0x00334D5C File Offset: 0x00332F5C
	// (set) Token: 0x0600B17F RID: 45439 RVA: 0x00334D6C File Offset: 0x00332F6C
	public UnitSpawnWaveData NIS01SingleBaserunnerWave
	{
		get
		{
			return this.ExposedVariables.NIS01SingleBaserunnerWave;
		}
		set
		{
			this.ExposedVariables.NIS01SingleBaserunnerWave = value;
		}
	}

	// Token: 0x1700077F RID: 1919
	// (get) Token: 0x0600B180 RID: 45440 RVA: 0x00334D7C File Offset: 0x00332F7C
	// (set) Token: 0x0600B181 RID: 45441 RVA: 0x00334D8C File Offset: 0x00332F8C
	public UnitSpawnWaveData PlayerSiidimBombers
	{
		get
		{
			return this.ExposedVariables.PlayerSiidimBombers;
		}
		set
		{
			this.ExposedVariables.PlayerSiidimBombers = value;
		}
	}

	// Token: 0x17000780 RID: 1920
	// (get) Token: 0x0600B182 RID: 45442 RVA: 0x00334D9C File Offset: 0x00332F9C
	// (set) Token: 0x0600B183 RID: 45443 RVA: 0x00334DAC File Offset: 0x00332FAC
	public UnitSpawnWaveData GaalsiResourcerGuardWave
	{
		get
		{
			return this.ExposedVariables.GaalsiResourcerGuardWave;
		}
		set
		{
			this.ExposedVariables.GaalsiResourcerGuardWave = value;
		}
	}

	// Token: 0x17000781 RID: 1921
	// (get) Token: 0x0600B184 RID: 45444 RVA: 0x00334DBC File Offset: 0x00332FBC
	// (set) Token: 0x0600B185 RID: 45445 RVA: 0x00334DCC File Offset: 0x00332FCC
	public UnitSpawnWaveData GaalsiResourcerGuardWithAAWave
	{
		get
		{
			return this.ExposedVariables.GaalsiResourcerGuardWithAAWave;
		}
		set
		{
			this.ExposedVariables.GaalsiResourcerGuardWithAAWave = value;
		}
	}

	// Token: 0x17000782 RID: 1922
	// (get) Token: 0x0600B186 RID: 45446 RVA: 0x00334DDC File Offset: 0x00332FDC
	// (set) Token: 0x0600B187 RID: 45447 RVA: 0x00334DEC File Offset: 0x00332FEC
	public IEnumerable<Entity> PlayerSupportCruisers
	{
		get
		{
			return this.ExposedVariables.PlayerSupportCruisers;
		}
		set
		{
			this.ExposedVariables.PlayerSupportCruisers = value;
		}
	}

	// Token: 0x17000783 RID: 1923
	// (get) Token: 0x0600B188 RID: 45448 RVA: 0x00334DFC File Offset: 0x00332FFC
	// (set) Token: 0x0600B189 RID: 45449 RVA: 0x00334E0C File Offset: 0x0033300C
	public IEnumerable<Entity> PlayerRachel
	{
		get
		{
			return this.ExposedVariables.PlayerRachel;
		}
		set
		{
			this.ExposedVariables.PlayerRachel = value;
		}
	}

	// Token: 0x17000784 RID: 1924
	// (get) Token: 0x0600B18A RID: 45450 RVA: 0x00334E1C File Offset: 0x0033301C
	// (set) Token: 0x0600B18B RID: 45451 RVA: 0x00334E2C File Offset: 0x0033302C
	public IEnumerable<Entity> PlayerBaserunner
	{
		get
		{
			return this.ExposedVariables.PlayerBaserunner;
		}
		set
		{
			this.ExposedVariables.PlayerBaserunner = value;
		}
	}

	// Token: 0x17000785 RID: 1925
	// (get) Token: 0x0600B18C RID: 45452 RVA: 0x00334E3C File Offset: 0x0033303C
	// (set) Token: 0x0600B18D RID: 45453 RVA: 0x00334E4C File Offset: 0x0033304C
	public bool bSupportCruiser2Killed
	{
		get
		{
			return this.ExposedVariables.bSupportCruiser2Killed;
		}
		set
		{
			this.ExposedVariables.bSupportCruiser2Killed = value;
		}
	}

	// Token: 0x17000786 RID: 1926
	// (get) Token: 0x0600B18E RID: 45454 RVA: 0x00334E5C File Offset: 0x0033305C
	// (set) Token: 0x0600B18F RID: 45455 RVA: 0x00334E6C File Offset: 0x0033306C
	public bool bSupportCruiser3Killed
	{
		get
		{
			return this.ExposedVariables.bSupportCruiser3Killed;
		}
		set
		{
			this.ExposedVariables.bSupportCruiser3Killed = value;
		}
	}

	// Token: 0x17000787 RID: 1927
	// (get) Token: 0x0600B190 RID: 45456 RVA: 0x00334E7C File Offset: 0x0033307C
	// (set) Token: 0x0600B191 RID: 45457 RVA: 0x00334E8C File Offset: 0x0033308C
	public bool bSupportCruiser1Killed
	{
		get
		{
			return this.ExposedVariables.bSupportCruiser1Killed;
		}
		set
		{
			this.ExposedVariables.bSupportCruiser1Killed = value;
		}
	}

	// Token: 0x17000788 RID: 1928
	// (get) Token: 0x0600B192 RID: 45458 RVA: 0x00334E9C File Offset: 0x0033309C
	// (set) Token: 0x0600B193 RID: 45459 RVA: 0x00334EAC File Offset: 0x003330AC
	public QueryContainer GaalsiPatrolPermanentQuery
	{
		get
		{
			return this.ExposedVariables.GaalsiPatrolPermanentQuery;
		}
		set
		{
			this.ExposedVariables.GaalsiPatrolPermanentQuery = value;
		}
	}

	// Token: 0x17000789 RID: 1929
	// (get) Token: 0x0600B194 RID: 45460 RVA: 0x00334EBC File Offset: 0x003330BC
	// (set) Token: 0x0600B195 RID: 45461 RVA: 0x00334ECC File Offset: 0x003330CC
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

	// Token: 0x1700078A RID: 1930
	// (get) Token: 0x0600B196 RID: 45462 RVA: 0x00334EDC File Offset: 0x003330DC
	// (set) Token: 0x0600B197 RID: 45463 RVA: 0x00334EEC File Offset: 0x003330EC
	public SpawnFactoryData Factory_Attackers
	{
		get
		{
			return this.ExposedVariables.Factory_Attackers;
		}
		set
		{
			this.ExposedVariables.Factory_Attackers = value;
		}
	}

	// Token: 0x1700078B RID: 1931
	// (get) Token: 0x0600B198 RID: 45464 RVA: 0x00334EFC File Offset: 0x003330FC
	// (set) Token: 0x0600B199 RID: 45465 RVA: 0x00334F0C File Offset: 0x0033310C
	public QueryContainer QueryPlayerCarrier
	{
		get
		{
			return this.ExposedVariables.QueryPlayerCarrier;
		}
		set
		{
			this.ExposedVariables.QueryPlayerCarrier = value;
		}
	}

	// Token: 0x0600B19A RID: 45466 RVA: 0x00334F1C File Offset: 0x0033311C
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

	// Token: 0x0600B19B RID: 45467 RVA: 0x00334F84 File Offset: 0x00333184
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0600B19C RID: 45468 RVA: 0x00334F94 File Offset: 0x00333194
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600B19D RID: 45469 RVA: 0x00334FA4 File Offset: 0x003331A4
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600B19E RID: 45470 RVA: 0x00334FB4 File Offset: 0x003331B4
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x0600B19F RID: 45471 RVA: 0x00334FC4 File Offset: 0x003331C4
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04010F2D RID: 69421
	public M07_Gameplay ExposedVariables = new M07_Gameplay();
}
