using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using UnityEngine;

// Token: 0x02000856 RID: 2134
[AddComponentMenu("uScript/Graphs/M07_Core")]
public class M07_Core_Component : uScriptCode
{
	// Token: 0x0600A815 RID: 43029 RVA: 0x00307178 File Offset: 0x00305378
	public M07_Core_Component()
	{
	}

	// Token: 0x17000740 RID: 1856
	// (get) Token: 0x0600A816 RID: 43030 RVA: 0x0030718C File Offset: 0x0030538C
	// (set) Token: 0x0600A817 RID: 43031 RVA: 0x0030719C File Offset: 0x0030539C
	public UnitSpawnWaveData AshokaCatWave
	{
		get
		{
			return this.ExposedVariables.AshokaCatWave;
		}
		set
		{
			this.ExposedVariables.AshokaCatWave = value;
		}
	}

	// Token: 0x17000741 RID: 1857
	// (get) Token: 0x0600A818 RID: 43032 RVA: 0x003071AC File Offset: 0x003053AC
	// (set) Token: 0x0600A819 RID: 43033 RVA: 0x003071BC File Offset: 0x003053BC
	public UnitSpawnWaveData AshokaSHWave
	{
		get
		{
			return this.ExposedVariables.AshokaSHWave;
		}
		set
		{
			this.ExposedVariables.AshokaSHWave = value;
		}
	}

	// Token: 0x17000742 RID: 1858
	// (get) Token: 0x0600A81A RID: 43034 RVA: 0x003071CC File Offset: 0x003053CC
	// (set) Token: 0x0600A81B RID: 43035 RVA: 0x003071DC File Offset: 0x003053DC
	public UnitSpawnWaveData AshokaCatU1Wave
	{
		get
		{
			return this.ExposedVariables.AshokaCatU1Wave;
		}
		set
		{
			this.ExposedVariables.AshokaCatU1Wave = value;
		}
	}

	// Token: 0x17000743 RID: 1859
	// (get) Token: 0x0600A81C RID: 43036 RVA: 0x003071EC File Offset: 0x003053EC
	// (set) Token: 0x0600A81D RID: 43037 RVA: 0x003071FC File Offset: 0x003053FC
	public UnitSpawnWaveData AshokaSHU1Wave
	{
		get
		{
			return this.ExposedVariables.AshokaSHU1Wave;
		}
		set
		{
			this.ExposedVariables.AshokaSHU1Wave = value;
		}
	}

	// Token: 0x17000744 RID: 1860
	// (get) Token: 0x0600A81E RID: 43038 RVA: 0x0030720C File Offset: 0x0030540C
	// (set) Token: 0x0600A81F RID: 43039 RVA: 0x0030721C File Offset: 0x0030541C
	public UnitSpawnWaveData AshokaSSWave
	{
		get
		{
			return this.ExposedVariables.AshokaSSWave;
		}
		set
		{
			this.ExposedVariables.AshokaSSWave = value;
		}
	}

	// Token: 0x17000745 RID: 1861
	// (get) Token: 0x0600A820 RID: 43040 RVA: 0x0030722C File Offset: 0x0030542C
	// (set) Token: 0x0600A821 RID: 43041 RVA: 0x0030723C File Offset: 0x0030543C
	public QueryContainer AshokaQuery
	{
		get
		{
			return this.ExposedVariables.AshokaQuery;
		}
		set
		{
			this.ExposedVariables.AshokaQuery = value;
		}
	}

	// Token: 0x17000746 RID: 1862
	// (get) Token: 0x0600A822 RID: 43042 RVA: 0x0030724C File Offset: 0x0030544C
	// (set) Token: 0x0600A823 RID: 43043 RVA: 0x0030725C File Offset: 0x0030545C
	public QueryContainer AshokaDefendersWaveQuery
	{
		get
		{
			return this.ExposedVariables.AshokaDefendersWaveQuery;
		}
		set
		{
			this.ExposedVariables.AshokaDefendersWaveQuery = value;
		}
	}

	// Token: 0x17000747 RID: 1863
	// (get) Token: 0x0600A824 RID: 43044 RVA: 0x0030726C File Offset: 0x0030546C
	// (set) Token: 0x0600A825 RID: 43045 RVA: 0x0030727C File Offset: 0x0030547C
	public UnitSpawnWaveData ProductionCruiser1Wave
	{
		get
		{
			return this.ExposedVariables.ProductionCruiser1Wave;
		}
		set
		{
			this.ExposedVariables.ProductionCruiser1Wave = value;
		}
	}

	// Token: 0x17000748 RID: 1864
	// (get) Token: 0x0600A826 RID: 43046 RVA: 0x0030728C File Offset: 0x0030548C
	// (set) Token: 0x0600A827 RID: 43047 RVA: 0x0030729C File Offset: 0x0030549C
	public UnitSpawnWaveData ProductionCruiser2Wave
	{
		get
		{
			return this.ExposedVariables.ProductionCruiser2Wave;
		}
		set
		{
			this.ExposedVariables.ProductionCruiser2Wave = value;
		}
	}

	// Token: 0x17000749 RID: 1865
	// (get) Token: 0x0600A828 RID: 43048 RVA: 0x003072AC File Offset: 0x003054AC
	// (set) Token: 0x0600A829 RID: 43049 RVA: 0x003072BC File Offset: 0x003054BC
	public UnitSpawnWaveData ProductionCruiser3Wave
	{
		get
		{
			return this.ExposedVariables.ProductionCruiser3Wave;
		}
		set
		{
			this.ExposedVariables.ProductionCruiser3Wave = value;
		}
	}

	// Token: 0x1700074A RID: 1866
	// (get) Token: 0x0600A82A RID: 43050 RVA: 0x003072CC File Offset: 0x003054CC
	// (set) Token: 0x0600A82B RID: 43051 RVA: 0x003072DC File Offset: 0x003054DC
	public List<Entity> SC3SpawnedAttackWave
	{
		get
		{
			return this.ExposedVariables.SC3SpawnedAttackWave;
		}
		set
		{
			this.ExposedVariables.SC3SpawnedAttackWave = value;
		}
	}

	// Token: 0x1700074B RID: 1867
	// (get) Token: 0x0600A82C RID: 43052 RVA: 0x003072EC File Offset: 0x003054EC
	// (set) Token: 0x0600A82D RID: 43053 RVA: 0x003072FC File Offset: 0x003054FC
	public List<Entity> SC2SpawnedAttackWave
	{
		get
		{
			return this.ExposedVariables.SC2SpawnedAttackWave;
		}
		set
		{
			this.ExposedVariables.SC2SpawnedAttackWave = value;
		}
	}

	// Token: 0x1700074C RID: 1868
	// (get) Token: 0x0600A82E RID: 43054 RVA: 0x0030730C File Offset: 0x0030550C
	// (set) Token: 0x0600A82F RID: 43055 RVA: 0x0030731C File Offset: 0x0030551C
	public List<Entity> SC1SpawnedAttackWave
	{
		get
		{
			return this.ExposedVariables.SC1SpawnedAttackWave;
		}
		set
		{
			this.ExposedVariables.SC1SpawnedAttackWave = value;
		}
	}

	// Token: 0x1700074D RID: 1869
	// (get) Token: 0x0600A830 RID: 43056 RVA: 0x0030732C File Offset: 0x0030552C
	// (set) Token: 0x0600A831 RID: 43057 RVA: 0x0030733C File Offset: 0x0030553C
	public UnitSpawnWaveData ProductionCruiserDefenceWave
	{
		get
		{
			return this.ExposedVariables.ProductionCruiserDefenceWave;
		}
		set
		{
			this.ExposedVariables.ProductionCruiserDefenceWave = value;
		}
	}

	// Token: 0x1700074E RID: 1870
	// (get) Token: 0x0600A832 RID: 43058 RVA: 0x0030734C File Offset: 0x0030554C
	// (set) Token: 0x0600A833 RID: 43059 RVA: 0x0030735C File Offset: 0x0030555C
	public QueryContainer AshokaAttackWaveQuery
	{
		get
		{
			return this.ExposedVariables.AshokaAttackWaveQuery;
		}
		set
		{
			this.ExposedVariables.AshokaAttackWaveQuery = value;
		}
	}

	// Token: 0x1700074F RID: 1871
	// (get) Token: 0x0600A834 RID: 43060 RVA: 0x0030736C File Offset: 0x0030556C
	// (set) Token: 0x0600A835 RID: 43061 RVA: 0x0030737C File Offset: 0x0030557C
	public QueryContainer KapisiQuery
	{
		get
		{
			return this.ExposedVariables.KapisiQuery;
		}
		set
		{
			this.ExposedVariables.KapisiQuery = value;
		}
	}

	// Token: 0x17000750 RID: 1872
	// (get) Token: 0x0600A836 RID: 43062 RVA: 0x0030738C File Offset: 0x0030558C
	// (set) Token: 0x0600A837 RID: 43063 RVA: 0x0030739C File Offset: 0x0030559C
	public int iMaxDefSHs
	{
		get
		{
			return this.ExposedVariables.iMaxDefSHs;
		}
		set
		{
			this.ExposedVariables.iMaxDefSHs = value;
		}
	}

	// Token: 0x17000751 RID: 1873
	// (get) Token: 0x0600A838 RID: 43064 RVA: 0x003073AC File Offset: 0x003055AC
	// (set) Token: 0x0600A839 RID: 43065 RVA: 0x003073BC File Offset: 0x003055BC
	public int iMaxDefCatsU1
	{
		get
		{
			return this.ExposedVariables.iMaxDefCatsU1;
		}
		set
		{
			this.ExposedVariables.iMaxDefCatsU1 = value;
		}
	}

	// Token: 0x17000752 RID: 1874
	// (get) Token: 0x0600A83A RID: 43066 RVA: 0x003073CC File Offset: 0x003055CC
	// (set) Token: 0x0600A83B RID: 43067 RVA: 0x003073DC File Offset: 0x003055DC
	public int iMaxDefCats
	{
		get
		{
			return this.ExposedVariables.iMaxDefCats;
		}
		set
		{
			this.ExposedVariables.iMaxDefCats = value;
		}
	}

	// Token: 0x17000753 RID: 1875
	// (get) Token: 0x0600A83C RID: 43068 RVA: 0x003073EC File Offset: 0x003055EC
	// (set) Token: 0x0600A83D RID: 43069 RVA: 0x003073FC File Offset: 0x003055FC
	public int iMaxDefSSs
	{
		get
		{
			return this.ExposedVariables.iMaxDefSSs;
		}
		set
		{
			this.ExposedVariables.iMaxDefSSs = value;
		}
	}

	// Token: 0x17000754 RID: 1876
	// (get) Token: 0x0600A83E RID: 43070 RVA: 0x0030740C File Offset: 0x0030560C
	// (set) Token: 0x0600A83F RID: 43071 RVA: 0x0030741C File Offset: 0x0030561C
	public int iAshokaDefenceWaveMaxCount
	{
		get
		{
			return this.ExposedVariables.iAshokaDefenceWaveMaxCount;
		}
		set
		{
			this.ExposedVariables.iAshokaDefenceWaveMaxCount = value;
		}
	}

	// Token: 0x0600A840 RID: 43072 RVA: 0x0030742C File Offset: 0x0030562C
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

	// Token: 0x0600A841 RID: 43073 RVA: 0x00307494 File Offset: 0x00305694
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600A842 RID: 43074 RVA: 0x003074A4 File Offset: 0x003056A4
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600A843 RID: 43075 RVA: 0x003074B4 File Offset: 0x003056B4
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600A844 RID: 43076 RVA: 0x003074C4 File Offset: 0x003056C4
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600A845 RID: 43077 RVA: 0x003074D4 File Offset: 0x003056D4
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400FE23 RID: 65059
	public M07_Core ExposedVariables = new M07_Core();
}
