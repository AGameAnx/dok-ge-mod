using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x02000893 RID: 2195
[AddComponentMenu("uScript/Graphs/M11_Gameplay")]
public class M11_Gameplay_Component : uScriptCode
{
	// Token: 0x06011330 RID: 70448 RVA: 0x004E02C8 File Offset: 0x004DE4C8
	public M11_Gameplay_Component()
	{
	}

	// Token: 0x17000956 RID: 2390
	// (get) Token: 0x06011331 RID: 70449 RVA: 0x004E02DC File Offset: 0x004DE4DC
	// (set) Token: 0x06011332 RID: 70450 RVA: 0x004E02EC File Offset: 0x004DE4EC
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

	// Token: 0x17000957 RID: 2391
	// (get) Token: 0x06011333 RID: 70451 RVA: 0x004E02FC File Offset: 0x004DE4FC
	// (set) Token: 0x06011334 RID: 70452 RVA: 0x004E030C File Offset: 0x004DE50C
	public QueryContainer QueryPlayerSalvagers
	{
		get
		{
			return this.ExposedVariables.QueryPlayerSalvagers;
		}
		set
		{
			this.ExposedVariables.QueryPlayerSalvagers = value;
		}
	}

	// Token: 0x17000958 RID: 2392
	// (get) Token: 0x06011335 RID: 70453 RVA: 0x004E031C File Offset: 0x004DE51C
	// (set) Token: 0x06011336 RID: 70454 RVA: 0x004E032C File Offset: 0x004DE52C
	public QueryContainer QueryPlayerSupportCruisers
	{
		get
		{
			return this.ExposedVariables.QueryPlayerSupportCruisers;
		}
		set
		{
			this.ExposedVariables.QueryPlayerSupportCruisers = value;
		}
	}

	// Token: 0x17000959 RID: 2393
	// (get) Token: 0x06011337 RID: 70455 RVA: 0x004E033C File Offset: 0x004DE53C
	// (set) Token: 0x06011338 RID: 70456 RVA: 0x004E034C File Offset: 0x004DE54C
	public QueryContainer QueryRachel
	{
		get
		{
			return this.ExposedVariables.QueryRachel;
		}
		set
		{
			this.ExposedVariables.QueryRachel = value;
		}
	}

	// Token: 0x1700095A RID: 2394
	// (get) Token: 0x06011339 RID: 70457 RVA: 0x004E035C File Offset: 0x004DE55C
	// (set) Token: 0x0601133A RID: 70458 RVA: 0x004E036C File Offset: 0x004DE56C
	public QueryContainer _V2SuspendUnits
	{
		get
		{
			return this.ExposedVariables._V2SuspendUnits;
		}
		set
		{
			this.ExposedVariables._V2SuspendUnits = value;
		}
	}

	// Token: 0x1700095B RID: 2395
	// (get) Token: 0x0601133B RID: 70459 RVA: 0x004E037C File Offset: 0x004DE57C
	// (set) Token: 0x0601133C RID: 70460 RVA: 0x004E038C File Offset: 0x004DE58C
	public QueryContainer _V2GodModeUnits
	{
		get
		{
			return this.ExposedVariables._V2GodModeUnits;
		}
		set
		{
			this.ExposedVariables._V2GodModeUnits = value;
		}
	}

	// Token: 0x1700095C RID: 2396
	// (get) Token: 0x0601133D RID: 70461 RVA: 0x004E039C File Offset: 0x004DE59C
	// (set) Token: 0x0601133E RID: 70462 RVA: 0x004E03AC File Offset: 0x004DE5AC
	public UnitSpawnWaveData Wave_HeavyTurret
	{
		get
		{
			return this.ExposedVariables.Wave_HeavyTurret;
		}
		set
		{
			this.ExposedVariables.Wave_HeavyTurret = value;
		}
	}

	// Token: 0x1700095D RID: 2397
	// (get) Token: 0x0601133F RID: 70463 RVA: 0x004E03BC File Offset: 0x004DE5BC
	// (set) Token: 0x06011340 RID: 70464 RVA: 0x004E03CC File Offset: 0x004DE5CC
	public AttributeBuffSetData Buff_Turrets
	{
		get
		{
			return this.ExposedVariables.Buff_Turrets;
		}
		set
		{
			this.ExposedVariables.Buff_Turrets = value;
		}
	}

	// Token: 0x06011341 RID: 70465 RVA: 0x004E03DC File Offset: 0x004DE5DC
	private void Awake()
	{
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

	// Token: 0x06011342 RID: 70466 RVA: 0x004E0444 File Offset: 0x004DE644
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x06011343 RID: 70467 RVA: 0x004E0454 File Offset: 0x004DE654
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06011344 RID: 70468 RVA: 0x004E0464 File Offset: 0x004DE664
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06011345 RID: 70469 RVA: 0x004E0474 File Offset: 0x004DE674
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x06011346 RID: 70470 RVA: 0x004E0484 File Offset: 0x004DE684
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04019DA4 RID: 105892
	public M11_Gameplay ExposedVariables = new M11_Gameplay();
}
