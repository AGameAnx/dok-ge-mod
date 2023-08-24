using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x02000815 RID: 2069
[AddComponentMenu("uScript/Graphs/M02_Gameplay2")]
public class M02_Gameplay2_Component : uScriptCode
{
	// Token: 0x06004DA9 RID: 19881 RVA: 0x00157A74 File Offset: 0x00155C74
	public M02_Gameplay2_Component()
	{
	}

	// Token: 0x170005D3 RID: 1491
	// (get) Token: 0x06004DAA RID: 19882 RVA: 0x00157A88 File Offset: 0x00155C88
	// (set) Token: 0x06004DAB RID: 19883 RVA: 0x00157A98 File Offset: 0x00155C98
	public UnitSpawnWaveData CoalitionWave1Wave
	{
		get
		{
			return this.ExposedVariables.CoalitionWave1Wave;
		}
		set
		{
			this.ExposedVariables.CoalitionWave1Wave = value;
		}
	}

	// Token: 0x170005D4 RID: 1492
	// (get) Token: 0x06004DAC RID: 19884 RVA: 0x00157AA8 File Offset: 0x00155CA8
	// (set) Token: 0x06004DAD RID: 19885 RVA: 0x00157AB8 File Offset: 0x00155CB8
	public UnitSpawnWaveData CoalitionAttackersWave1Wave
	{
		get
		{
			return this.ExposedVariables.CoalitionAttackersWave1Wave;
		}
		set
		{
			this.ExposedVariables.CoalitionAttackersWave1Wave = value;
		}
	}

	// Token: 0x170005D5 RID: 1493
	// (get) Token: 0x06004DAE RID: 19886 RVA: 0x00157AC8 File Offset: 0x00155CC8
	// (set) Token: 0x06004DAF RID: 19887 RVA: 0x00157AD8 File Offset: 0x00155CD8
	public UnitSpawnWaveData CoalitionAttackersWave2Wave
	{
		get
		{
			return this.ExposedVariables.CoalitionAttackersWave2Wave;
		}
		set
		{
			this.ExposedVariables.CoalitionAttackersWave2Wave = value;
		}
	}

	// Token: 0x170005D6 RID: 1494
	// (get) Token: 0x06004DB0 RID: 19888 RVA: 0x00157AE8 File Offset: 0x00155CE8
	// (set) Token: 0x06004DB1 RID: 19889 RVA: 0x00157AF8 File Offset: 0x00155CF8
	public UnitSpawnWaveData CoalitionWave2Wave
	{
		get
		{
			return this.ExposedVariables.CoalitionWave2Wave;
		}
		set
		{
			this.ExposedVariables.CoalitionWave2Wave = value;
		}
	}

	// Token: 0x170005D7 RID: 1495
	// (get) Token: 0x06004DB2 RID: 19890 RVA: 0x00157B08 File Offset: 0x00155D08
	// (set) Token: 0x06004DB3 RID: 19891 RVA: 0x00157B18 File Offset: 0x00155D18
	public UnitSpawnWaveData CoalitionAttackersWave3Wave
	{
		get
		{
			return this.ExposedVariables.CoalitionAttackersWave3Wave;
		}
		set
		{
			this.ExposedVariables.CoalitionAttackersWave3Wave = value;
		}
	}

	// Token: 0x170005D8 RID: 1496
	// (get) Token: 0x06004DB4 RID: 19892 RVA: 0x00157B28 File Offset: 0x00155D28
	// (set) Token: 0x06004DB5 RID: 19893 RVA: 0x00157B38 File Offset: 0x00155D38
	public UnitSpawnWaveData CoalitionWave3Wave
	{
		get
		{
			return this.ExposedVariables.CoalitionWave3Wave;
		}
		set
		{
			this.ExposedVariables.CoalitionWave3Wave = value;
		}
	}

	// Token: 0x170005D9 RID: 1497
	// (get) Token: 0x06004DB6 RID: 19894 RVA: 0x00157B48 File Offset: 0x00155D48
	// (set) Token: 0x06004DB7 RID: 19895 RVA: 0x00157B58 File Offset: 0x00155D58
	public UnitSpawnWaveData GaalsSCAttackWave1
	{
		get
		{
			return this.ExposedVariables.GaalsSCAttackWave1;
		}
		set
		{
			this.ExposedVariables.GaalsSCAttackWave1 = value;
		}
	}

	// Token: 0x170005DA RID: 1498
	// (get) Token: 0x06004DB8 RID: 19896 RVA: 0x00157B68 File Offset: 0x00155D68
	// (set) Token: 0x06004DB9 RID: 19897 RVA: 0x00157B78 File Offset: 0x00155D78
	public UnitSpawnWaveData FirstGaalsiAttackWave
	{
		get
		{
			return this.ExposedVariables.FirstGaalsiAttackWave;
		}
		set
		{
			this.ExposedVariables.FirstGaalsiAttackWave = value;
		}
	}

	// Token: 0x170005DB RID: 1499
	// (get) Token: 0x06004DBA RID: 19898 RVA: 0x00157B88 File Offset: 0x00155D88
	// (set) Token: 0x06004DBB RID: 19899 RVA: 0x00157B98 File Offset: 0x00155D98
	public UnitSpawnWaveData SecondGaalsiAttackWave
	{
		get
		{
			return this.ExposedVariables.SecondGaalsiAttackWave;
		}
		set
		{
			this.ExposedVariables.SecondGaalsiAttackWave = value;
		}
	}

	// Token: 0x170005DC RID: 1500
	// (get) Token: 0x06004DBC RID: 19900 RVA: 0x00157BA8 File Offset: 0x00155DA8
	// (set) Token: 0x06004DBD RID: 19901 RVA: 0x00157BB8 File Offset: 0x00155DB8
	public UnitSpawnWaveData ThirdGaalsiAttackWave
	{
		get
		{
			return this.ExposedVariables.ThirdGaalsiAttackWave;
		}
		set
		{
			this.ExposedVariables.ThirdGaalsiAttackWave = value;
		}
	}

	// Token: 0x170005DD RID: 1501
	// (get) Token: 0x06004DBE RID: 19902 RVA: 0x00157BC8 File Offset: 0x00155DC8
	// (set) Token: 0x06004DBF RID: 19903 RVA: 0x00157BD8 File Offset: 0x00155DD8
	public UnitSpawnWaveData FifthGaalsiAttackWave
	{
		get
		{
			return this.ExposedVariables.FifthGaalsiAttackWave;
		}
		set
		{
			this.ExposedVariables.FifthGaalsiAttackWave = value;
		}
	}

	// Token: 0x170005DE RID: 1502
	// (get) Token: 0x06004DC0 RID: 19904 RVA: 0x00157BE8 File Offset: 0x00155DE8
	// (set) Token: 0x06004DC1 RID: 19905 RVA: 0x00157BF8 File Offset: 0x00155DF8
	public UnitSpawnWaveData SixthGaalsiAttackWave
	{
		get
		{
			return this.ExposedVariables.SixthGaalsiAttackWave;
		}
		set
		{
			this.ExposedVariables.SixthGaalsiAttackWave = value;
		}
	}

	// Token: 0x170005DF RID: 1503
	// (get) Token: 0x06004DC2 RID: 19906 RVA: 0x00157C08 File Offset: 0x00155E08
	// (set) Token: 0x06004DC3 RID: 19907 RVA: 0x00157C18 File Offset: 0x00155E18
	public UnitSpawnWaveData SeventhGaalsiAttackWave
	{
		get
		{
			return this.ExposedVariables.SeventhGaalsiAttackWave;
		}
		set
		{
			this.ExposedVariables.SeventhGaalsiAttackWave = value;
		}
	}

	// Token: 0x170005E0 RID: 1504
	// (get) Token: 0x06004DC4 RID: 19908 RVA: 0x00157C28 File Offset: 0x00155E28
	// (set) Token: 0x06004DC5 RID: 19909 RVA: 0x00157C38 File Offset: 0x00155E38
	public UnitSpawnWaveData EighthGaalsiAttackWave
	{
		get
		{
			return this.ExposedVariables.EighthGaalsiAttackWave;
		}
		set
		{
			this.ExposedVariables.EighthGaalsiAttackWave = value;
		}
	}

	// Token: 0x170005E1 RID: 1505
	// (get) Token: 0x06004DC6 RID: 19910 RVA: 0x00157C48 File Offset: 0x00155E48
	// (set) Token: 0x06004DC7 RID: 19911 RVA: 0x00157C58 File Offset: 0x00155E58
	public UnitSpawnWaveData GaalsSCAttackWave2
	{
		get
		{
			return this.ExposedVariables.GaalsSCAttackWave2;
		}
		set
		{
			this.ExposedVariables.GaalsSCAttackWave2 = value;
		}
	}

	// Token: 0x170005E2 RID: 1506
	// (get) Token: 0x06004DC8 RID: 19912 RVA: 0x00157C68 File Offset: 0x00155E68
	// (set) Token: 0x06004DC9 RID: 19913 RVA: 0x00157C78 File Offset: 0x00155E78
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

	// Token: 0x170005E3 RID: 1507
	// (get) Token: 0x06004DCA RID: 19914 RVA: 0x00157C88 File Offset: 0x00155E88
	// (set) Token: 0x06004DCB RID: 19915 RVA: 0x00157C98 File Offset: 0x00155E98
	public UnitSpawnWaveData GaalsSCAttackWave3
	{
		get
		{
			return this.ExposedVariables.GaalsSCAttackWave3;
		}
		set
		{
			this.ExposedVariables.GaalsSCAttackWave3 = value;
		}
	}

	// Token: 0x170005E4 RID: 1508
	// (get) Token: 0x06004DCC RID: 19916 RVA: 0x00157CA8 File Offset: 0x00155EA8
	// (set) Token: 0x06004DCD RID: 19917 RVA: 0x00157CB8 File Offset: 0x00155EB8
	public List<Entity> CoalitionAttackersWave1
	{
		get
		{
			return this.ExposedVariables.CoalitionAttackersWave1;
		}
		set
		{
			this.ExposedVariables.CoalitionAttackersWave1 = value;
		}
	}

	// Token: 0x170005E5 RID: 1509
	// (get) Token: 0x06004DCE RID: 19918 RVA: 0x00157CC8 File Offset: 0x00155EC8
	// (set) Token: 0x06004DCF RID: 19919 RVA: 0x00157CD8 File Offset: 0x00155ED8
	public UnitSpawnWaveData PreSurvivorGaalsiAttackWave
	{
		get
		{
			return this.ExposedVariables.PreSurvivorGaalsiAttackWave;
		}
		set
		{
			this.ExposedVariables.PreSurvivorGaalsiAttackWave = value;
		}
	}

	// Token: 0x170005E6 RID: 1510
	// (get) Token: 0x06004DD0 RID: 19920 RVA: 0x00157CE8 File Offset: 0x00155EE8
	// (set) Token: 0x06004DD1 RID: 19921 RVA: 0x00157CF8 File Offset: 0x00155EF8
	public UnitSpawnWaveData NinethGaalsiAttackWave
	{
		get
		{
			return this.ExposedVariables.NinethGaalsiAttackWave;
		}
		set
		{
			this.ExposedVariables.NinethGaalsiAttackWave = value;
		}
	}

	// Token: 0x170005E7 RID: 1511
	// (get) Token: 0x06004DD2 RID: 19922 RVA: 0x00157D08 File Offset: 0x00155F08
	// (set) Token: 0x06004DD3 RID: 19923 RVA: 0x00157D18 File Offset: 0x00155F18
	public QueryContainer PlayerEscortQuery
	{
		get
		{
			return this.ExposedVariables.PlayerEscortQuery;
		}
		set
		{
			this.ExposedVariables.PlayerEscortQuery = value;
		}
	}

	// Token: 0x170005E8 RID: 1512
	// (get) Token: 0x06004DD4 RID: 19924 RVA: 0x00157D28 File Offset: 0x00155F28
	// (set) Token: 0x06004DD5 RID: 19925 RVA: 0x00157D38 File Offset: 0x00155F38
	public QueryContainer PlayerHACQuery
	{
		get
		{
			return this.ExposedVariables.PlayerHACQuery;
		}
		set
		{
			this.ExposedVariables.PlayerHACQuery = value;
		}
	}

	// Token: 0x170005E9 RID: 1513
	// (get) Token: 0x06004DD6 RID: 19926 RVA: 0x00157D48 File Offset: 0x00155F48
	// (set) Token: 0x06004DD7 RID: 19927 RVA: 0x00157D58 File Offset: 0x00155F58
	public List<Entity> CoalitionWave1
	{
		get
		{
			return this.ExposedVariables.CoalitionWave1;
		}
		set
		{
			this.ExposedVariables.CoalitionWave1 = value;
		}
	}

	// Token: 0x170005EA RID: 1514
	// (get) Token: 0x06004DD8 RID: 19928 RVA: 0x00157D68 File Offset: 0x00155F68
	// (set) Token: 0x06004DD9 RID: 19929 RVA: 0x00157D78 File Offset: 0x00155F78
	public int iNumSurvivorsRescued
	{
		get
		{
			return this.ExposedVariables.iNumSurvivorsRescued;
		}
		set
		{
			this.ExposedVariables.iNumSurvivorsRescued = value;
		}
	}

	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x06004DDA RID: 19930 RVA: 0x00157D88 File Offset: 0x00155F88
	// (set) Token: 0x06004DDB RID: 19931 RVA: 0x00157D98 File Offset: 0x00155F98
	public AttributeBuffSetData SurvivorsArmorBuff
	{
		get
		{
			return this.ExposedVariables.SurvivorsArmorBuff;
		}
		set
		{
			this.ExposedVariables.SurvivorsArmorBuff = value;
		}
	}

	// Token: 0x170005EC RID: 1516
	// (get) Token: 0x06004DDC RID: 19932 RVA: 0x00157DA8 File Offset: 0x00155FA8
	// (set) Token: 0x06004DDD RID: 19933 RVA: 0x00157DB8 File Offset: 0x00155FB8
	public UnitSpawnWaveData FourthGaalsiAttackWave
	{
		get
		{
			return this.ExposedVariables.FourthGaalsiAttackWave;
		}
		set
		{
			this.ExposedVariables.FourthGaalsiAttackWave = value;
		}
	}

	// Token: 0x170005ED RID: 1517
	// (get) Token: 0x06004DDE RID: 19934 RVA: 0x00157DC8 File Offset: 0x00155FC8
	// (set) Token: 0x06004DDF RID: 19935 RVA: 0x00157DD8 File Offset: 0x00155FD8
	public UnitSpawnWaveData FinalGaalsiStarhullWave
	{
		get
		{
			return this.ExposedVariables.FinalGaalsiStarhullWave;
		}
		set
		{
			this.ExposedVariables.FinalGaalsiStarhullWave = value;
		}
	}

	// Token: 0x170005EE RID: 1518
	// (get) Token: 0x06004DE0 RID: 19936 RVA: 0x00157DE8 File Offset: 0x00155FE8
	// (set) Token: 0x06004DE1 RID: 19937 RVA: 0x00157DF8 File Offset: 0x00155FF8
	public UnitSpawnWaveData FinalGaalsiSandskimmerWave
	{
		get
		{
			return this.ExposedVariables.FinalGaalsiSandskimmerWave;
		}
		set
		{
			this.ExposedVariables.FinalGaalsiSandskimmerWave = value;
		}
	}

	// Token: 0x170005EF RID: 1519
	// (get) Token: 0x06004DE2 RID: 19938 RVA: 0x00157E08 File Offset: 0x00156008
	// (set) Token: 0x06004DE3 RID: 19939 RVA: 0x00157E18 File Offset: 0x00156018
	public AttributeBuffSetData CoalitionAttackersWave2SlowBuff
	{
		get
		{
			return this.ExposedVariables.CoalitionAttackersWave2SlowBuff;
		}
		set
		{
			this.ExposedVariables.CoalitionAttackersWave2SlowBuff = value;
		}
	}

	// Token: 0x170005F0 RID: 1520
	// (get) Token: 0x06004DE4 RID: 19940 RVA: 0x00157E28 File Offset: 0x00156028
	// (set) Token: 0x06004DE5 RID: 19941 RVA: 0x00157E38 File Offset: 0x00156038
	public UnitSpawnWaveData GaalsSCAttackWave1Med
	{
		get
		{
			return this.ExposedVariables.GaalsSCAttackWave1Med;
		}
		set
		{
			this.ExposedVariables.GaalsSCAttackWave1Med = value;
		}
	}

	// Token: 0x170005F1 RID: 1521
	// (get) Token: 0x06004DE6 RID: 19942 RVA: 0x00157E48 File Offset: 0x00156048
	// (set) Token: 0x06004DE7 RID: 19943 RVA: 0x00157E58 File Offset: 0x00156058
	public UnitSpawnWaveData GaalsSCAttackWave1Easy
	{
		get
		{
			return this.ExposedVariables.GaalsSCAttackWave1Easy;
		}
		set
		{
			this.ExposedVariables.GaalsSCAttackWave1Easy = value;
		}
	}

	// Token: 0x170005F2 RID: 1522
	// (get) Token: 0x06004DE8 RID: 19944 RVA: 0x00157E68 File Offset: 0x00156068
	// (set) Token: 0x06004DE9 RID: 19945 RVA: 0x00157E78 File Offset: 0x00156078
	public UnitSpawnWaveData GaalsSCAttackWave2Med
	{
		get
		{
			return this.ExposedVariables.GaalsSCAttackWave2Med;
		}
		set
		{
			this.ExposedVariables.GaalsSCAttackWave2Med = value;
		}
	}

	// Token: 0x170005F3 RID: 1523
	// (get) Token: 0x06004DEA RID: 19946 RVA: 0x00157E88 File Offset: 0x00156088
	// (set) Token: 0x06004DEB RID: 19947 RVA: 0x00157E98 File Offset: 0x00156098
	public UnitSpawnWaveData GaalsSCAttackWave2Easy
	{
		get
		{
			return this.ExposedVariables.GaalsSCAttackWave2Easy;
		}
		set
		{
			this.ExposedVariables.GaalsSCAttackWave2Easy = value;
		}
	}

	// Token: 0x170005F4 RID: 1524
	// (get) Token: 0x06004DEC RID: 19948 RVA: 0x00157EA8 File Offset: 0x001560A8
	// (set) Token: 0x06004DED RID: 19949 RVA: 0x00157EB8 File Offset: 0x001560B8
	public UnitSpawnWaveData GaalsSCAttackWave3Med
	{
		get
		{
			return this.ExposedVariables.GaalsSCAttackWave3Med;
		}
		set
		{
			this.ExposedVariables.GaalsSCAttackWave3Med = value;
		}
	}

	// Token: 0x170005F5 RID: 1525
	// (get) Token: 0x06004DEE RID: 19950 RVA: 0x00157EC8 File Offset: 0x001560C8
	// (set) Token: 0x06004DEF RID: 19951 RVA: 0x00157ED8 File Offset: 0x001560D8
	public UnitSpawnWaveData GaalsSCAttackWave3Easy
	{
		get
		{
			return this.ExposedVariables.GaalsSCAttackWave3Easy;
		}
		set
		{
			this.ExposedVariables.GaalsSCAttackWave3Easy = value;
		}
	}

	// Token: 0x06004DF0 RID: 19952 RVA: 0x00157EE8 File Offset: 0x001560E8
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

	// Token: 0x06004DF1 RID: 19953 RVA: 0x00157F50 File Offset: 0x00156150
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x06004DF2 RID: 19954 RVA: 0x00157F60 File Offset: 0x00156160
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06004DF3 RID: 19955 RVA: 0x00157F70 File Offset: 0x00156170
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06004DF4 RID: 19956 RVA: 0x00157F80 File Offset: 0x00156180
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x06004DF5 RID: 19957 RVA: 0x00157F90 File Offset: 0x00156190
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x040067AC RID: 26540
	public M02_Gameplay2 ExposedVariables = new M02_Gameplay2();
}
