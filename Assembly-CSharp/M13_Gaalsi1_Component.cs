using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Scripting;
using UnityEngine;

// Token: 0x020008A7 RID: 2215
[AddComponentMenu("uScript/Graphs/M13_Gaalsi1")]
public class M13_Gaalsi1_Component : uScriptCode
{
	// Token: 0x06013055 RID: 77909 RVA: 0x0055F31C File Offset: 0x0055D51C
	public M13_Gaalsi1_Component()
	{
	}

	// Token: 0x170009C5 RID: 2501
	// (get) Token: 0x06013056 RID: 77910 RVA: 0x0055F330 File Offset: 0x0055D530
	// (set) Token: 0x06013057 RID: 77911 RVA: 0x0055F340 File Offset: 0x0055D540
	public UnitSpawnWaveData TypeBomber
	{
		get
		{
			return this.ExposedVariables.TypeBomber;
		}
		set
		{
			this.ExposedVariables.TypeBomber = value;
		}
	}

	// Token: 0x170009C6 RID: 2502
	// (get) Token: 0x06013058 RID: 77912 RVA: 0x0055F350 File Offset: 0x0055D550
	// (set) Token: 0x06013059 RID: 77913 RVA: 0x0055F360 File Offset: 0x0055D560
	public UnitSpawnWaveData TypeInterceptor
	{
		get
		{
			return this.ExposedVariables.TypeInterceptor;
		}
		set
		{
			this.ExposedVariables.TypeInterceptor = value;
		}
	}

	// Token: 0x170009C7 RID: 2503
	// (get) Token: 0x0601305A RID: 77914 RVA: 0x0055F370 File Offset: 0x0055D570
	// (set) Token: 0x0601305B RID: 77915 RVA: 0x0055F380 File Offset: 0x0055D580
	public QueryContainer Q_Vanguard
	{
		get
		{
			return this.ExposedVariables.Q_Vanguard;
		}
		set
		{
			this.ExposedVariables.Q_Vanguard = value;
		}
	}

	// Token: 0x170009C8 RID: 2504
	// (get) Token: 0x0601305C RID: 77916 RVA: 0x0055F390 File Offset: 0x0055D590
	// (set) Token: 0x0601305D RID: 77917 RVA: 0x0055F3A0 File Offset: 0x0055D5A0
	public QueryContainer Q_SecondaryTargets
	{
		get
		{
			return this.ExposedVariables.Q_SecondaryTargets;
		}
		set
		{
			this.ExposedVariables.Q_SecondaryTargets = value;
		}
	}

	// Token: 0x170009C9 RID: 2505
	// (get) Token: 0x0601305E RID: 77918 RVA: 0x0055F3B0 File Offset: 0x0055D5B0
	// (set) Token: 0x0601305F RID: 77919 RVA: 0x0055F3C0 File Offset: 0x0055D5C0
	public SpawnFactoryData Spawn2
	{
		get
		{
			return this.ExposedVariables.Spawn2;
		}
		set
		{
			this.ExposedVariables.Spawn2 = value;
		}
	}

	// Token: 0x170009CA RID: 2506
	// (get) Token: 0x06013060 RID: 77920 RVA: 0x0055F3D0 File Offset: 0x0055D5D0
	// (set) Token: 0x06013061 RID: 77921 RVA: 0x0055F3E0 File Offset: 0x0055D5E0
	public SpawnFactoryData Spawn3
	{
		get
		{
			return this.ExposedVariables.Spawn3;
		}
		set
		{
			this.ExposedVariables.Spawn3 = value;
		}
	}

	// Token: 0x170009CB RID: 2507
	// (get) Token: 0x06013062 RID: 77922 RVA: 0x0055F3F0 File Offset: 0x0055D5F0
	// (set) Token: 0x06013063 RID: 77923 RVA: 0x0055F400 File Offset: 0x0055D600
	public SpawnFactoryData Spawn6
	{
		get
		{
			return this.ExposedVariables.Spawn6;
		}
		set
		{
			this.ExposedVariables.Spawn6 = value;
		}
	}

	// Token: 0x170009CC RID: 2508
	// (get) Token: 0x06013064 RID: 77924 RVA: 0x0055F410 File Offset: 0x0055D610
	// (set) Token: 0x06013065 RID: 77925 RVA: 0x0055F420 File Offset: 0x0055D620
	public SpawnFactoryData Spawn5
	{
		get
		{
			return this.ExposedVariables.Spawn5;
		}
		set
		{
			this.ExposedVariables.Spawn5 = value;
		}
	}

	// Token: 0x170009CD RID: 2509
	// (get) Token: 0x06013066 RID: 77926 RVA: 0x0055F430 File Offset: 0x0055D630
	// (set) Token: 0x06013067 RID: 77927 RVA: 0x0055F440 File Offset: 0x0055D640
	public SpawnFactoryData Spawn4
	{
		get
		{
			return this.ExposedVariables.Spawn4;
		}
		set
		{
			this.ExposedVariables.Spawn4 = value;
		}
	}

	// Token: 0x170009CE RID: 2510
	// (get) Token: 0x06013068 RID: 77928 RVA: 0x0055F450 File Offset: 0x0055D650
	// (set) Token: 0x06013069 RID: 77929 RVA: 0x0055F460 File Offset: 0x0055D660
	public AttributeBuffSetData Buff_Catamaran_Upgrade
	{
		get
		{
			return this.ExposedVariables.Buff_Catamaran_Upgrade;
		}
		set
		{
			this.ExposedVariables.Buff_Catamaran_Upgrade = value;
		}
	}

	// Token: 0x170009CF RID: 2511
	// (get) Token: 0x0601306A RID: 77930 RVA: 0x0055F470 File Offset: 0x0055D670
	// (set) Token: 0x0601306B RID: 77931 RVA: 0x0055F480 File Offset: 0x0055D680
	public SpawnFactoryData Spawn_1
	{
		get
		{
			return this.ExposedVariables.Spawn_1;
		}
		set
		{
			this.ExposedVariables.Spawn_1 = value;
		}
	}

	// Token: 0x170009D0 RID: 2512
	// (get) Token: 0x0601306C RID: 77932 RVA: 0x0055F490 File Offset: 0x0055D690
	// (set) Token: 0x0601306D RID: 77933 RVA: 0x0055F4A0 File Offset: 0x0055D6A0
	public BuffSetAttributesAsset Buff_AirUnits01
	{
		get
		{
			return this.ExposedVariables.Buff_AirUnits01;
		}
		set
		{
			this.ExposedVariables.Buff_AirUnits01 = value;
		}
	}

	// Token: 0x170009D1 RID: 2513
	// (get) Token: 0x0601306E RID: 77934 RVA: 0x0055F4B0 File Offset: 0x0055D6B0
	// (set) Token: 0x0601306F RID: 77935 RVA: 0x0055F4C0 File Offset: 0x0055D6C0
	public BuffSetAttributesAsset Buff_AirUnits02
	{
		get
		{
			return this.ExposedVariables.Buff_AirUnits02;
		}
		set
		{
			this.ExposedVariables.Buff_AirUnits02 = value;
		}
	}

	// Token: 0x170009D2 RID: 2514
	// (get) Token: 0x06013070 RID: 77936 RVA: 0x0055F4D0 File Offset: 0x0055D6D0
	// (set) Token: 0x06013071 RID: 77937 RVA: 0x0055F4E0 File Offset: 0x0055D6E0
	public QueryContainer Q_Kapisi
	{
		get
		{
			return this.ExposedVariables.Q_Kapisi;
		}
		set
		{
			this.ExposedVariables.Q_Kapisi = value;
		}
	}

	// Token: 0x170009D3 RID: 2515
	// (get) Token: 0x06013072 RID: 77938 RVA: 0x0055F4F0 File Offset: 0x0055D6F0
	// (set) Token: 0x06013073 RID: 77939 RVA: 0x0055F500 File Offset: 0x0055D700
	public AttributeBuffSetData Buff_Leash
	{
		get
		{
			return this.ExposedVariables.Buff_Leash;
		}
		set
		{
			this.ExposedVariables.Buff_Leash = value;
		}
	}

	// Token: 0x06013074 RID: 77940 RVA: 0x0055F510 File Offset: 0x0055D710
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

	// Token: 0x06013075 RID: 77941 RVA: 0x0055F578 File Offset: 0x0055D778
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x06013076 RID: 77942 RVA: 0x0055F588 File Offset: 0x0055D788
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06013077 RID: 77943 RVA: 0x0055F598 File Offset: 0x0055D798
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06013078 RID: 77944 RVA: 0x0055F5A8 File Offset: 0x0055D7A8
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06013079 RID: 77945 RVA: 0x0055F5B8 File Offset: 0x0055D7B8
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401C92B RID: 117035
	public M13_Gaalsi1 ExposedVariables = new M13_Gaalsi1();
}
