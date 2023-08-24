using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using UnityEngine;

// Token: 0x02000874 RID: 2164
[AddComponentMenu("uScript/Graphs/M09_NIS")]
public class M09_NIS_Component : uScriptCode
{
	// Token: 0x0600E537 RID: 58679 RVA: 0x00414E48 File Offset: 0x00413048
	public M09_NIS_Component()
	{
	}

	// Token: 0x17000863 RID: 2147
	// (get) Token: 0x0600E538 RID: 58680 RVA: 0x00414E5C File Offset: 0x0041305C
	// (set) Token: 0x0600E539 RID: 58681 RVA: 0x00414E6C File Offset: 0x0041306C
	public UnitSpawnWaveData Wave_NIS1Carrier1
	{
		get
		{
			return this.ExposedVariables.Wave_NIS1Carrier1;
		}
		set
		{
			this.ExposedVariables.Wave_NIS1Carrier1 = value;
		}
	}

	// Token: 0x17000864 RID: 2148
	// (get) Token: 0x0600E53A RID: 58682 RVA: 0x00414E7C File Offset: 0x0041307C
	// (set) Token: 0x0600E53B RID: 58683 RVA: 0x00414E8C File Offset: 0x0041308C
	public UnitSpawnWaveData Wave_NIS1Carrier2
	{
		get
		{
			return this.ExposedVariables.Wave_NIS1Carrier2;
		}
		set
		{
			this.ExposedVariables.Wave_NIS1Carrier2 = value;
		}
	}

	// Token: 0x17000865 RID: 2149
	// (get) Token: 0x0600E53C RID: 58684 RVA: 0x00414E9C File Offset: 0x0041309C
	// (set) Token: 0x0600E53D RID: 58685 RVA: 0x00414EAC File Offset: 0x004130AC
	public QueryContainer QueryNISCarrier
	{
		get
		{
			return this.ExposedVariables.QueryNISCarrier;
		}
		set
		{
			this.ExposedVariables.QueryNISCarrier = value;
		}
	}

	// Token: 0x17000866 RID: 2150
	// (get) Token: 0x0600E53E RID: 58686 RVA: 0x00414EBC File Offset: 0x004130BC
	// (set) Token: 0x0600E53F RID: 58687 RVA: 0x00414ECC File Offset: 0x004130CC
	public UnitSpawnWaveData Wave_NIS1Carrier3
	{
		get
		{
			return this.ExposedVariables.Wave_NIS1Carrier3;
		}
		set
		{
			this.ExposedVariables.Wave_NIS1Carrier3 = value;
		}
	}

	// Token: 0x17000867 RID: 2151
	// (get) Token: 0x0600E540 RID: 58688 RVA: 0x00414EDC File Offset: 0x004130DC
	// (set) Token: 0x0600E541 RID: 58689 RVA: 0x00414EEC File Offset: 0x004130EC
	public UnitSpawnWaveData Wave_NIS1Carrier0
	{
		get
		{
			return this.ExposedVariables.Wave_NIS1Carrier0;
		}
		set
		{
			this.ExposedVariables.Wave_NIS1Carrier0 = value;
		}
	}

	// Token: 0x17000868 RID: 2152
	// (get) Token: 0x0600E542 RID: 58690 RVA: 0x00414EFC File Offset: 0x004130FC
	// (set) Token: 0x0600E543 RID: 58691 RVA: 0x00414F0C File Offset: 0x0041310C
	public UnitSpawnWaveData Wave_NIS1Entourage2
	{
		get
		{
			return this.ExposedVariables.Wave_NIS1Entourage2;
		}
		set
		{
			this.ExposedVariables.Wave_NIS1Entourage2 = value;
		}
	}

	// Token: 0x17000869 RID: 2153
	// (get) Token: 0x0600E544 RID: 58692 RVA: 0x00414F1C File Offset: 0x0041311C
	// (set) Token: 0x0600E545 RID: 58693 RVA: 0x00414F2C File Offset: 0x0041312C
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

	// Token: 0x1700086A RID: 2154
	// (get) Token: 0x0600E546 RID: 58694 RVA: 0x00414F3C File Offset: 0x0041313C
	// (set) Token: 0x0600E547 RID: 58695 RVA: 0x00414F4C File Offset: 0x0041314C
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

	// Token: 0x1700086B RID: 2155
	// (get) Token: 0x0600E548 RID: 58696 RVA: 0x00414F5C File Offset: 0x0041315C
	// (set) Token: 0x0600E549 RID: 58697 RVA: 0x00414F6C File Offset: 0x0041316C
	public QueryContainer SuspendAll
	{
		get
		{
			return this.ExposedVariables.SuspendAll;
		}
		set
		{
			this.ExposedVariables.SuspendAll = value;
		}
	}

	// Token: 0x1700086C RID: 2156
	// (get) Token: 0x0600E54A RID: 58698 RVA: 0x00414F7C File Offset: 0x0041317C
	// (set) Token: 0x0600E54B RID: 58699 RVA: 0x00414F8C File Offset: 0x0041318C
	public QueryContainer SearchGCAR1
	{
		get
		{
			return this.ExposedVariables.SearchGCAR1;
		}
		set
		{
			this.ExposedVariables.SearchGCAR1 = value;
		}
	}

	// Token: 0x1700086D RID: 2157
	// (get) Token: 0x0600E54C RID: 58700 RVA: 0x00414F9C File Offset: 0x0041319C
	// (set) Token: 0x0600E54D RID: 58701 RVA: 0x00414FAC File Offset: 0x004131AC
	public QueryContainer SearchGCAR2
	{
		get
		{
			return this.ExposedVariables.SearchGCAR2;
		}
		set
		{
			this.ExposedVariables.SearchGCAR2 = value;
		}
	}

	// Token: 0x1700086E RID: 2158
	// (get) Token: 0x0600E54E RID: 58702 RVA: 0x00414FBC File Offset: 0x004131BC
	// (set) Token: 0x0600E54F RID: 58703 RVA: 0x00414FCC File Offset: 0x004131CC
	public QueryContainer SearchKapisi
	{
		get
		{
			return this.ExposedVariables.SearchKapisi;
		}
		set
		{
			this.ExposedVariables.SearchKapisi = value;
		}
	}

	// Token: 0x1700086F RID: 2159
	// (get) Token: 0x0600E550 RID: 58704 RVA: 0x00414FDC File Offset: 0x004131DC
	// (set) Token: 0x0600E551 RID: 58705 RVA: 0x00414FEC File Offset: 0x004131EC
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

	// Token: 0x17000870 RID: 2160
	// (get) Token: 0x0600E552 RID: 58706 RVA: 0x00414FFC File Offset: 0x004131FC
	// (set) Token: 0x0600E553 RID: 58707 RVA: 0x0041500C File Offset: 0x0041320C
	public QueryContainer LastGCarrier
	{
		get
		{
			return this.ExposedVariables.LastGCarrier;
		}
		set
		{
			this.ExposedVariables.LastGCarrier = value;
		}
	}

	// Token: 0x17000871 RID: 2161
	// (get) Token: 0x0600E554 RID: 58708 RVA: 0x0041501C File Offset: 0x0041321C
	// (set) Token: 0x0600E555 RID: 58709 RVA: 0x0041502C File Offset: 0x0041322C
	public QueryContainer FindAllUnits
	{
		get
		{
			return this.ExposedVariables.FindAllUnits;
		}
		set
		{
			this.ExposedVariables.FindAllUnits = value;
		}
	}

	// Token: 0x17000872 RID: 2162
	// (get) Token: 0x0600E556 RID: 58710 RVA: 0x0041503C File Offset: 0x0041323C
	// (set) Token: 0x0600E557 RID: 58711 RVA: 0x0041504C File Offset: 0x0041324C
	public bool AniDone
	{
		get
		{
			return this.ExposedVariables.AniDone;
		}
		set
		{
			this.ExposedVariables.AniDone = value;
		}
	}

	// Token: 0x0600E558 RID: 58712 RVA: 0x0041505C File Offset: 0x0041325C
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

	// Token: 0x0600E559 RID: 58713 RVA: 0x004150C4 File Offset: 0x004132C4
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600E55A RID: 58714 RVA: 0x004150D4 File Offset: 0x004132D4
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600E55B RID: 58715 RVA: 0x004150E4 File Offset: 0x004132E4
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600E55C RID: 58716 RVA: 0x004150F4 File Offset: 0x004132F4
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600E55D RID: 58717 RVA: 0x00415104 File Offset: 0x00413304
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04015A9F RID: 88735
	public M09_NIS ExposedVariables = new M09_NIS();
}
