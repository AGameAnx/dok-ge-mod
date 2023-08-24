using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Scripting;
using UnityEngine;

// Token: 0x02000890 RID: 2192
[AddComponentMenu("uScript/Graphs/M11_Gameplay2")]
public class M11_Gameplay2_Component : uScriptCode
{
	// Token: 0x06010FED RID: 69613 RVA: 0x004D1950 File Offset: 0x004CFB50
	public M11_Gameplay2_Component()
	{
	}

	// Token: 0x17000918 RID: 2328
	// (get) Token: 0x06010FEE RID: 69614 RVA: 0x004D1964 File Offset: 0x004CFB64
	// (set) Token: 0x06010FEF RID: 69615 RVA: 0x004D1974 File Offset: 0x004CFB74
	public bool bWave1Live
	{
		get
		{
			return this.ExposedVariables.bWave1Live;
		}
		set
		{
			this.ExposedVariables.bWave1Live = value;
		}
	}

	// Token: 0x17000919 RID: 2329
	// (get) Token: 0x06010FF0 RID: 69616 RVA: 0x004D1984 File Offset: 0x004CFB84
	// (set) Token: 0x06010FF1 RID: 69617 RVA: 0x004D1994 File Offset: 0x004CFB94
	public UnitSpawnWaveData SiidimAttackWave1Wave
	{
		get
		{
			return this.ExposedVariables.SiidimAttackWave1Wave;
		}
		set
		{
			this.ExposedVariables.SiidimAttackWave1Wave = value;
		}
	}

	// Token: 0x1700091A RID: 2330
	// (get) Token: 0x06010FF2 RID: 69618 RVA: 0x004D19A4 File Offset: 0x004CFBA4
	// (set) Token: 0x06010FF3 RID: 69619 RVA: 0x004D19B4 File Offset: 0x004CFBB4
	public QueryContainer QueryAtFinalMarker
	{
		get
		{
			return this.ExposedVariables.QueryAtFinalMarker;
		}
		set
		{
			this.ExposedVariables.QueryAtFinalMarker = value;
		}
	}

	// Token: 0x1700091B RID: 2331
	// (get) Token: 0x06010FF4 RID: 69620 RVA: 0x004D19C4 File Offset: 0x004CFBC4
	// (set) Token: 0x06010FF5 RID: 69621 RVA: 0x004D19D4 File Offset: 0x004CFBD4
	public bool bWave3Live
	{
		get
		{
			return this.ExposedVariables.bWave3Live;
		}
		set
		{
			this.ExposedVariables.bWave3Live = value;
		}
	}

	// Token: 0x1700091C RID: 2332
	// (get) Token: 0x06010FF6 RID: 69622 RVA: 0x004D19E4 File Offset: 0x004CFBE4
	// (set) Token: 0x06010FF7 RID: 69623 RVA: 0x004D19F4 File Offset: 0x004CFBF4
	public bool bWave4Live
	{
		get
		{
			return this.ExposedVariables.bWave4Live;
		}
		set
		{
			this.ExposedVariables.bWave4Live = value;
		}
	}

	// Token: 0x1700091D RID: 2333
	// (get) Token: 0x06010FF8 RID: 69624 RVA: 0x004D1A04 File Offset: 0x004CFC04
	// (set) Token: 0x06010FF9 RID: 69625 RVA: 0x004D1A14 File Offset: 0x004CFC14
	public bool bWave5Live
	{
		get
		{
			return this.ExposedVariables.bWave5Live;
		}
		set
		{
			this.ExposedVariables.bWave5Live = value;
		}
	}

	// Token: 0x1700091E RID: 2334
	// (get) Token: 0x06010FFA RID: 69626 RVA: 0x004D1A24 File Offset: 0x004CFC24
	// (set) Token: 0x06010FFB RID: 69627 RVA: 0x004D1A34 File Offset: 0x004CFC34
	public bool bWave6Live
	{
		get
		{
			return this.ExposedVariables.bWave6Live;
		}
		set
		{
			this.ExposedVariables.bWave6Live = value;
		}
	}

	// Token: 0x1700091F RID: 2335
	// (get) Token: 0x06010FFC RID: 69628 RVA: 0x004D1A44 File Offset: 0x004CFC44
	// (set) Token: 0x06010FFD RID: 69629 RVA: 0x004D1A54 File Offset: 0x004CFC54
	public UnitSpawnWaveData SiidimAttackWave2Wave
	{
		get
		{
			return this.ExposedVariables.SiidimAttackWave2Wave;
		}
		set
		{
			this.ExposedVariables.SiidimAttackWave2Wave = value;
		}
	}

	// Token: 0x17000920 RID: 2336
	// (get) Token: 0x06010FFE RID: 69630 RVA: 0x004D1A64 File Offset: 0x004CFC64
	// (set) Token: 0x06010FFF RID: 69631 RVA: 0x004D1A74 File Offset: 0x004CFC74
	public bool bWave2Live
	{
		get
		{
			return this.ExposedVariables.bWave2Live;
		}
		set
		{
			this.ExposedVariables.bWave2Live = value;
		}
	}

	// Token: 0x17000921 RID: 2337
	// (get) Token: 0x06011000 RID: 69632 RVA: 0x004D1A84 File Offset: 0x004CFC84
	// (set) Token: 0x06011001 RID: 69633 RVA: 0x004D1A94 File Offset: 0x004CFC94
	public int iNumLiveWaves
	{
		get
		{
			return this.ExposedVariables.iNumLiveWaves;
		}
		set
		{
			this.ExposedVariables.iNumLiveWaves = value;
		}
	}

	// Token: 0x17000922 RID: 2338
	// (get) Token: 0x06011002 RID: 69634 RVA: 0x004D1AA4 File Offset: 0x004CFCA4
	// (set) Token: 0x06011003 RID: 69635 RVA: 0x004D1AB4 File Offset: 0x004CFCB4
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

	// Token: 0x17000923 RID: 2339
	// (get) Token: 0x06011004 RID: 69636 RVA: 0x004D1AC4 File Offset: 0x004CFCC4
	// (set) Token: 0x06011005 RID: 69637 RVA: 0x004D1AD4 File Offset: 0x004CFCD4
	public QueryContainer QueryWave3
	{
		get
		{
			return this.ExposedVariables.QueryWave3;
		}
		set
		{
			this.ExposedVariables.QueryWave3 = value;
		}
	}

	// Token: 0x17000924 RID: 2340
	// (get) Token: 0x06011006 RID: 69638 RVA: 0x004D1AE4 File Offset: 0x004CFCE4
	// (set) Token: 0x06011007 RID: 69639 RVA: 0x004D1AF4 File Offset: 0x004CFCF4
	public QueryContainer QueryWave4
	{
		get
		{
			return this.ExposedVariables.QueryWave4;
		}
		set
		{
			this.ExposedVariables.QueryWave4 = value;
		}
	}

	// Token: 0x17000925 RID: 2341
	// (get) Token: 0x06011008 RID: 69640 RVA: 0x004D1B04 File Offset: 0x004CFD04
	// (set) Token: 0x06011009 RID: 69641 RVA: 0x004D1B14 File Offset: 0x004CFD14
	public QueryContainer QueryWave5
	{
		get
		{
			return this.ExposedVariables.QueryWave5;
		}
		set
		{
			this.ExposedVariables.QueryWave5 = value;
		}
	}

	// Token: 0x17000926 RID: 2342
	// (get) Token: 0x0601100A RID: 69642 RVA: 0x004D1B24 File Offset: 0x004CFD24
	// (set) Token: 0x0601100B RID: 69643 RVA: 0x004D1B34 File Offset: 0x004CFD34
	public QueryContainer QueryWave6
	{
		get
		{
			return this.ExposedVariables.QueryWave6;
		}
		set
		{
			this.ExposedVariables.QueryWave6 = value;
		}
	}

	// Token: 0x17000927 RID: 2343
	// (get) Token: 0x0601100C RID: 69644 RVA: 0x004D1B44 File Offset: 0x004CFD44
	// (set) Token: 0x0601100D RID: 69645 RVA: 0x004D1B54 File Offset: 0x004CFD54
	public QueryContainer QueryWave9
	{
		get
		{
			return this.ExposedVariables.QueryWave9;
		}
		set
		{
			this.ExposedVariables.QueryWave9 = value;
		}
	}

	// Token: 0x17000928 RID: 2344
	// (get) Token: 0x0601100E RID: 69646 RVA: 0x004D1B64 File Offset: 0x004CFD64
	// (set) Token: 0x0601100F RID: 69647 RVA: 0x004D1B74 File Offset: 0x004CFD74
	public QueryContainer QueryWave10
	{
		get
		{
			return this.ExposedVariables.QueryWave10;
		}
		set
		{
			this.ExposedVariables.QueryWave10 = value;
		}
	}

	// Token: 0x17000929 RID: 2345
	// (get) Token: 0x06011010 RID: 69648 RVA: 0x004D1B84 File Offset: 0x004CFD84
	// (set) Token: 0x06011011 RID: 69649 RVA: 0x004D1B94 File Offset: 0x004CFD94
	public QueryContainer QueryWave7
	{
		get
		{
			return this.ExposedVariables.QueryWave7;
		}
		set
		{
			this.ExposedVariables.QueryWave7 = value;
		}
	}

	// Token: 0x1700092A RID: 2346
	// (get) Token: 0x06011012 RID: 69650 RVA: 0x004D1BA4 File Offset: 0x004CFDA4
	// (set) Token: 0x06011013 RID: 69651 RVA: 0x004D1BB4 File Offset: 0x004CFDB4
	public UnitSpawnWaveData Wave_Interceptor
	{
		get
		{
			return this.ExposedVariables.Wave_Interceptor;
		}
		set
		{
			this.ExposedVariables.Wave_Interceptor = value;
		}
	}

	// Token: 0x1700092B RID: 2347
	// (get) Token: 0x06011014 RID: 69652 RVA: 0x004D1BC4 File Offset: 0x004CFDC4
	// (set) Token: 0x06011015 RID: 69653 RVA: 0x004D1BD4 File Offset: 0x004CFDD4
	public string Speech_Bombers1
	{
		get
		{
			return this.ExposedVariables.Speech_Bombers1;
		}
		set
		{
			this.ExposedVariables.Speech_Bombers1 = value;
		}
	}

	// Token: 0x1700092C RID: 2348
	// (get) Token: 0x06011016 RID: 69654 RVA: 0x004D1BE4 File Offset: 0x004CFDE4
	// (set) Token: 0x06011017 RID: 69655 RVA: 0x004D1BF4 File Offset: 0x004CFDF4
	public QueryContainer QueryWave8
	{
		get
		{
			return this.ExposedVariables.QueryWave8;
		}
		set
		{
			this.ExposedVariables.QueryWave8 = value;
		}
	}

	// Token: 0x1700092D RID: 2349
	// (get) Token: 0x06011018 RID: 69656 RVA: 0x004D1C04 File Offset: 0x004CFE04
	// (set) Token: 0x06011019 RID: 69657 RVA: 0x004D1C14 File Offset: 0x004CFE14
	public QueryContainer QueryRailguns
	{
		get
		{
			return this.ExposedVariables.QueryRailguns;
		}
		set
		{
			this.ExposedVariables.QueryRailguns = value;
		}
	}

	// Token: 0x1700092E RID: 2350
	// (get) Token: 0x0601101A RID: 69658 RVA: 0x004D1C24 File Offset: 0x004CFE24
	// (set) Token: 0x0601101B RID: 69659 RVA: 0x004D1C34 File Offset: 0x004CFE34
	public QueryContainer QueryHarvesters
	{
		get
		{
			return this.ExposedVariables.QueryHarvesters;
		}
		set
		{
			this.ExposedVariables.QueryHarvesters = value;
		}
	}

	// Token: 0x1700092F RID: 2351
	// (get) Token: 0x0601101C RID: 69660 RVA: 0x004D1C44 File Offset: 0x004CFE44
	// (set) Token: 0x0601101D RID: 69661 RVA: 0x004D1C54 File Offset: 0x004CFE54
	public QueryContainer QueryAllAttackers
	{
		get
		{
			return this.ExposedVariables.QueryAllAttackers;
		}
		set
		{
			this.ExposedVariables.QueryAllAttackers = value;
		}
	}

	// Token: 0x17000930 RID: 2352
	// (get) Token: 0x0601101E RID: 69662 RVA: 0x004D1C64 File Offset: 0x004CFE64
	// (set) Token: 0x0601101F RID: 69663 RVA: 0x004D1C74 File Offset: 0x004CFE74
	public UnitSpawnWaveData Wave_Bombers
	{
		get
		{
			return this.ExposedVariables.Wave_Bombers;
		}
		set
		{
			this.ExposedVariables.Wave_Bombers = value;
		}
	}

	// Token: 0x17000931 RID: 2353
	// (get) Token: 0x06011020 RID: 69664 RVA: 0x004D1C84 File Offset: 0x004CFE84
	// (set) Token: 0x06011021 RID: 69665 RVA: 0x004D1C94 File Offset: 0x004CFE94
	public UnitSpawnWaveData Wave_P1Final
	{
		get
		{
			return this.ExposedVariables.Wave_P1Final;
		}
		set
		{
			this.ExposedVariables.Wave_P1Final = value;
		}
	}

	// Token: 0x17000932 RID: 2354
	// (get) Token: 0x06011022 RID: 69666 RVA: 0x004D1CA4 File Offset: 0x004CFEA4
	// (set) Token: 0x06011023 RID: 69667 RVA: 0x004D1CB4 File Offset: 0x004CFEB4
	public AttributeBuffSetData Buff_P1Final
	{
		get
		{
			return this.ExposedVariables.Buff_P1Final;
		}
		set
		{
			this.ExposedVariables.Buff_P1Final = value;
		}
	}

	// Token: 0x17000933 RID: 2355
	// (get) Token: 0x06011024 RID: 69668 RVA: 0x004D1CC4 File Offset: 0x004CFEC4
	// (set) Token: 0x06011025 RID: 69669 RVA: 0x004D1CD4 File Offset: 0x004CFED4
	public AttributeBuffSetData Buff_P1Final2
	{
		get
		{
			return this.ExposedVariables.Buff_P1Final2;
		}
		set
		{
			this.ExposedVariables.Buff_P1Final2 = value;
		}
	}

	// Token: 0x17000934 RID: 2356
	// (get) Token: 0x06011026 RID: 69670 RVA: 0x004D1CE4 File Offset: 0x004CFEE4
	// (set) Token: 0x06011027 RID: 69671 RVA: 0x004D1CF4 File Offset: 0x004CFEF4
	public UnitSpawnWaveData Wave_ProductionCruiser
	{
		get
		{
			return this.ExposedVariables.Wave_ProductionCruiser;
		}
		set
		{
			this.ExposedVariables.Wave_ProductionCruiser = value;
		}
	}

	// Token: 0x17000935 RID: 2357
	// (get) Token: 0x06011028 RID: 69672 RVA: 0x004D1D04 File Offset: 0x004CFF04
	// (set) Token: 0x06011029 RID: 69673 RVA: 0x004D1D14 File Offset: 0x004CFF14
	public SpawnFactoryData Factory_ProdCruisers
	{
		get
		{
			return this.ExposedVariables.Factory_ProdCruisers;
		}
		set
		{
			this.ExposedVariables.Factory_ProdCruisers = value;
		}
	}

	// Token: 0x17000936 RID: 2358
	// (get) Token: 0x0601102A RID: 69674 RVA: 0x004D1D24 File Offset: 0x004CFF24
	// (set) Token: 0x0601102B RID: 69675 RVA: 0x004D1D34 File Offset: 0x004CFF34
	public AttributeBuffSetData Buff_BigAggro
	{
		get
		{
			return this.ExposedVariables.Buff_BigAggro;
		}
		set
		{
			this.ExposedVariables.Buff_BigAggro = value;
		}
	}

	// Token: 0x17000937 RID: 2359
	// (get) Token: 0x0601102C RID: 69676 RVA: 0x004D1D44 File Offset: 0x004CFF44
	// (set) Token: 0x0601102D RID: 69677 RVA: 0x004D1D54 File Offset: 0x004CFF54
	public QueryContainer QueryPlayerScanners
	{
		get
		{
			return this.ExposedVariables.QueryPlayerScanners;
		}
		set
		{
			this.ExposedVariables.QueryPlayerScanners = value;
		}
	}

	// Token: 0x17000938 RID: 2360
	// (get) Token: 0x0601102E RID: 69678 RVA: 0x004D1D64 File Offset: 0x004CFF64
	// (set) Token: 0x0601102F RID: 69679 RVA: 0x004D1D74 File Offset: 0x004CFF74
	public QueryContainer QueryFromProd1
	{
		get
		{
			return this.ExposedVariables.QueryFromProd1;
		}
		set
		{
			this.ExposedVariables.QueryFromProd1 = value;
		}
	}

	// Token: 0x17000939 RID: 2361
	// (get) Token: 0x06011030 RID: 69680 RVA: 0x004D1D84 File Offset: 0x004CFF84
	// (set) Token: 0x06011031 RID: 69681 RVA: 0x004D1D94 File Offset: 0x004CFF94
	public QueryContainer QueryFromProd2
	{
		get
		{
			return this.ExposedVariables.QueryFromProd2;
		}
		set
		{
			this.ExposedVariables.QueryFromProd2 = value;
		}
	}

	// Token: 0x1700093A RID: 2362
	// (get) Token: 0x06011032 RID: 69682 RVA: 0x004D1DA4 File Offset: 0x004CFFA4
	// (set) Token: 0x06011033 RID: 69683 RVA: 0x004D1DB4 File Offset: 0x004CFFB4
	public QueryContainer QueryFromProd3
	{
		get
		{
			return this.ExposedVariables.QueryFromProd3;
		}
		set
		{
			this.ExposedVariables.QueryFromProd3 = value;
		}
	}

	// Token: 0x1700093B RID: 2363
	// (get) Token: 0x06011034 RID: 69684 RVA: 0x004D1DC4 File Offset: 0x004CFFC4
	// (set) Token: 0x06011035 RID: 69685 RVA: 0x004D1DD4 File Offset: 0x004CFFD4
	public QueryContainer QueryFromProd4
	{
		get
		{
			return this.ExposedVariables.QueryFromProd4;
		}
		set
		{
			this.ExposedVariables.QueryFromProd4 = value;
		}
	}

	// Token: 0x06011036 RID: 69686 RVA: 0x004D1DE4 File Offset: 0x004CFFE4
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

	// Token: 0x06011037 RID: 69687 RVA: 0x004D1E4C File Offset: 0x004D004C
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x06011038 RID: 69688 RVA: 0x004D1E5C File Offset: 0x004D005C
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x06011039 RID: 69689 RVA: 0x004D1E6C File Offset: 0x004D006C
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0601103A RID: 69690 RVA: 0x004D1E7C File Offset: 0x004D007C
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0601103B RID: 69691 RVA: 0x004D1E8C File Offset: 0x004D008C
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x040198BE RID: 104638
	public M11_Gameplay2 ExposedVariables = new M11_Gameplay2();
}
