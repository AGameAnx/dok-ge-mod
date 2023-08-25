using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x0200084C RID: 2124
[AddComponentMenu("uScript/Graphs/M06_GaalsiDefense")]
public class M06_GaalsiDefense_Component : uScriptCode
{
	// Token: 0x06009995 RID: 39317 RVA: 0x002C2F68 File Offset: 0x002C1168
	public M06_GaalsiDefense_Component()
	{
	}

	// Token: 0x170006F8 RID: 1784
	// (get) Token: 0x06009996 RID: 39318 RVA: 0x002C2F7C File Offset: 0x002C117C
	// (set) Token: 0x06009997 RID: 39319 RVA: 0x002C2F8C File Offset: 0x002C118C
	public UnitSpawnWaveData ExcavationDefensePatrol
	{
		get
		{
			return this.ExposedVariables.ExcavationDefensePatrol;
		}
		set
		{
			this.ExposedVariables.ExcavationDefensePatrol = value;
		}
	}

	// Token: 0x170006F9 RID: 1785
	// (get) Token: 0x06009998 RID: 39320 RVA: 0x002C2F9C File Offset: 0x002C119C
	// (set) Token: 0x06009999 RID: 39321 RVA: 0x002C2FAC File Offset: 0x002C11AC
	public UnitSpawnWaveData GaalsiAttackWave01
	{
		get
		{
			return this.ExposedVariables.GaalsiAttackWave01;
		}
		set
		{
			this.ExposedVariables.GaalsiAttackWave01 = value;
		}
	}

	// Token: 0x170006FA RID: 1786
	// (get) Token: 0x0600999A RID: 39322 RVA: 0x002C2FBC File Offset: 0x002C11BC
	// (set) Token: 0x0600999B RID: 39323 RVA: 0x002C2FCC File Offset: 0x002C11CC
	public QueryContainer CoalitionCarrierQuery
	{
		get
		{
			return this.ExposedVariables.CoalitionCarrierQuery;
		}
		set
		{
			this.ExposedVariables.CoalitionCarrierQuery = value;
		}
	}

	// Token: 0x170006FB RID: 1787
	// (get) Token: 0x0600999C RID: 39324 RVA: 0x002C2FDC File Offset: 0x002C11DC
	// (set) Token: 0x0600999D RID: 39325 RVA: 0x002C2FEC File Offset: 0x002C11EC
	public bool bStarhullSquadKilled
	{
		get
		{
			return this.ExposedVariables.bStarhullSquadKilled;
		}
		set
		{
			this.ExposedVariables.bStarhullSquadKilled = value;
		}
	}

	// Token: 0x170006FC RID: 1788
	// (get) Token: 0x0600999E RID: 39326 RVA: 0x002C2FFC File Offset: 0x002C11FC
	// (set) Token: 0x0600999F RID: 39327 RVA: 0x002C300C File Offset: 0x002C120C
	public QueryContainer AllTurretsQuery
	{
		get
		{
			return this.ExposedVariables.AllTurretsQuery;
		}
		set
		{
			this.ExposedVariables.AllTurretsQuery = value;
		}
	}

	// Token: 0x170006FD RID: 1789
	// (get) Token: 0x060099A0 RID: 39328 RVA: 0x002C301C File Offset: 0x002C121C
	// (set) Token: 0x060099A1 RID: 39329 RVA: 0x002C302C File Offset: 0x002C122C
	public UnitSpawnWaveData TurretControlTower
	{
		get
		{
			return this.ExposedVariables.TurretControlTower;
		}
		set
		{
			this.ExposedVariables.TurretControlTower = value;
		}
	}

	// Token: 0x170006FE RID: 1790
	// (get) Token: 0x060099A2 RID: 39330 RVA: 0x002C303C File Offset: 0x002C123C
	// (set) Token: 0x060099A3 RID: 39331 RVA: 0x002C304C File Offset: 0x002C124C
	public AttributeBuffSetData UnitCannotDealDamageBuff
	{
		get
		{
			return this.ExposedVariables.UnitCannotDealDamageBuff;
		}
		set
		{
			this.ExposedVariables.UnitCannotDealDamageBuff = value;
		}
	}

	// Token: 0x170006FF RID: 1791
	// (get) Token: 0x060099A4 RID: 39332 RVA: 0x002C305C File Offset: 0x002C125C
	// (set) Token: 0x060099A5 RID: 39333 RVA: 0x002C306C File Offset: 0x002C126C
	public UnitSpawnWaveData RPTypeTurret
	{
		get
		{
			return this.ExposedVariables.RPTypeTurret;
		}
		set
		{
			this.ExposedVariables.RPTypeTurret = value;
		}
	}

	// Token: 0x17000700 RID: 1792
	// (get) Token: 0x060099A6 RID: 39334 RVA: 0x002C307C File Offset: 0x002C127C
	// (set) Token: 0x060099A7 RID: 39335 RVA: 0x002C308C File Offset: 0x002C128C
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

	// Token: 0x17000701 RID: 1793
	// (get) Token: 0x060099A8 RID: 39336 RVA: 0x002C309C File Offset: 0x002C129C
	// (set) Token: 0x060099A9 RID: 39337 RVA: 0x002C30AC File Offset: 0x002C12AC
	public AttributeBuffSetData TemporarySpeedBuff
	{
		get
		{
			return this.ExposedVariables.TemporarySpeedBuff;
		}
		set
		{
			this.ExposedVariables.TemporarySpeedBuff = value;
		}
	}

	// Token: 0x17000702 RID: 1794
	// (get) Token: 0x060099AA RID: 39338 RVA: 0x002C30BC File Offset: 0x002C12BC
	// (set) Token: 0x060099AB RID: 39339 RVA: 0x002C30CC File Offset: 0x002C12CC
	public QueryContainer TurretGroup01Query
	{
		get
		{
			return this.ExposedVariables.TurretGroup01Query;
		}
		set
		{
			this.ExposedVariables.TurretGroup01Query = value;
		}
	}

	// Token: 0x17000703 RID: 1795
	// (get) Token: 0x060099AC RID: 39340 RVA: 0x002C30DC File Offset: 0x002C12DC
	// (set) Token: 0x060099AD RID: 39341 RVA: 0x002C30EC File Offset: 0x002C12EC
	public QueryContainer TurretGroup02Query
	{
		get
		{
			return this.ExposedVariables.TurretGroup02Query;
		}
		set
		{
			this.ExposedVariables.TurretGroup02Query = value;
		}
	}

	// Token: 0x17000704 RID: 1796
	// (get) Token: 0x060099AE RID: 39342 RVA: 0x002C30FC File Offset: 0x002C12FC
	// (set) Token: 0x060099AF RID: 39343 RVA: 0x002C310C File Offset: 0x002C130C
	public QueryContainer TurretGroup03Query
	{
		get
		{
			return this.ExposedVariables.TurretGroup03Query;
		}
		set
		{
			this.ExposedVariables.TurretGroup03Query = value;
		}
	}

	// Token: 0x17000705 RID: 1797
	// (get) Token: 0x060099B0 RID: 39344 RVA: 0x002C311C File Offset: 0x002C131C
	// (set) Token: 0x060099B1 RID: 39345 RVA: 0x002C312C File Offset: 0x002C132C
	public QueryContainer TurretGroup04Query
	{
		get
		{
			return this.ExposedVariables.TurretGroup04Query;
		}
		set
		{
			this.ExposedVariables.TurretGroup04Query = value;
		}
	}

	// Token: 0x17000706 RID: 1798
	// (get) Token: 0x060099B2 RID: 39346 RVA: 0x002C313C File Offset: 0x002C133C
	// (set) Token: 0x060099B3 RID: 39347 RVA: 0x002C314C File Offset: 0x002C134C
	public QueryContainer TurretGroup05Query
	{
		get
		{
			return this.ExposedVariables.TurretGroup05Query;
		}
		set
		{
			this.ExposedVariables.TurretGroup05Query = value;
		}
	}

	// Token: 0x17000707 RID: 1799
	// (get) Token: 0x060099B4 RID: 39348 RVA: 0x002C315C File Offset: 0x002C135C
	// (set) Token: 0x060099B5 RID: 39349 RVA: 0x002C316C File Offset: 0x002C136C
	public QueryContainer TurretGroup06Query
	{
		get
		{
			return this.ExposedVariables.TurretGroup06Query;
		}
		set
		{
			this.ExposedVariables.TurretGroup06Query = value;
		}
	}

	// Token: 0x17000708 RID: 1800
	// (get) Token: 0x060099B6 RID: 39350 RVA: 0x002C317C File Offset: 0x002C137C
	// (set) Token: 0x060099B7 RID: 39351 RVA: 0x002C318C File Offset: 0x002C138C
	public QueryContainer TurretGroup07Query
	{
		get
		{
			return this.ExposedVariables.TurretGroup07Query;
		}
		set
		{
			this.ExposedVariables.TurretGroup07Query = value;
		}
	}

	// Token: 0x17000709 RID: 1801
	// (get) Token: 0x060099B8 RID: 39352 RVA: 0x002C319C File Offset: 0x002C139C
	// (set) Token: 0x060099B9 RID: 39353 RVA: 0x002C31AC File Offset: 0x002C13AC
	public QueryContainer TurretGroup08Query
	{
		get
		{
			return this.ExposedVariables.TurretGroup08Query;
		}
		set
		{
			this.ExposedVariables.TurretGroup08Query = value;
		}
	}

	// Token: 0x1700070A RID: 1802
	// (get) Token: 0x060099BA RID: 39354 RVA: 0x002C31BC File Offset: 0x002C13BC
	// (set) Token: 0x060099BB RID: 39355 RVA: 0x002C31CC File Offset: 0x002C13CC
	public AttributeBuffSetData TurretGroupDebuff
	{
		get
		{
			return this.ExposedVariables.TurretGroupDebuff;
		}
		set
		{
			this.ExposedVariables.TurretGroupDebuff = value;
		}
	}

	// Token: 0x1700070B RID: 1803
	// (get) Token: 0x060099BC RID: 39356 RVA: 0x002C31DC File Offset: 0x002C13DC
	// (set) Token: 0x060099BD RID: 39357 RVA: 0x002C31EC File Offset: 0x002C13EC
	public AttributeBuffSetData UnitHealthAndArmourBuff
	{
		get
		{
			return this.ExposedVariables.UnitHealthAndArmourBuff;
		}
		set
		{
			this.ExposedVariables.UnitHealthAndArmourBuff = value;
		}
	}

	// Token: 0x1700070C RID: 1804
	// (get) Token: 0x060099BE RID: 39358 RVA: 0x002C31FC File Offset: 0x002C13FC
	// (set) Token: 0x060099BF RID: 39359 RVA: 0x002C320C File Offset: 0x002C140C
	public UnitSpawnWaveData GaalsiAttackWave02
	{
		get
		{
			return this.ExposedVariables.GaalsiAttackWave02;
		}
		set
		{
			this.ExposedVariables.GaalsiAttackWave02 = value;
		}
	}

	// Token: 0x060099C0 RID: 39360 RVA: 0x002C321C File Offset: 0x002C141C
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

	// Token: 0x060099C1 RID: 39361 RVA: 0x002C3284 File Offset: 0x002C1484
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x060099C2 RID: 39362 RVA: 0x002C3294 File Offset: 0x002C1494
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x060099C3 RID: 39363 RVA: 0x002C32A4 File Offset: 0x002C14A4
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x060099C4 RID: 39364 RVA: 0x002C32B4 File Offset: 0x002C14B4
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x060099C5 RID: 39365 RVA: 0x002C32C4 File Offset: 0x002C14C4
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400E4B7 RID: 58551
	public M06_GaalsiDefense ExposedVariables = new M06_GaalsiDefense();
}
