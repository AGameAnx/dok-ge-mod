using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using UnityEngine;

// Token: 0x0200085E RID: 2142
[AddComponentMenu("uScript/Graphs/M07_RPTracking")]
public class M07_RPTracking_Component : uScriptCode
{
	// Token: 0x0600B766 RID: 46950 RVA: 0x00351A6C File Offset: 0x0034FC6C
	public M07_RPTracking_Component()
	{
	}

	// Token: 0x1700079F RID: 1951
	// (get) Token: 0x0600B767 RID: 46951 RVA: 0x00351A80 File Offset: 0x0034FC80
	// (set) Token: 0x0600B768 RID: 46952 RVA: 0x00351A90 File Offset: 0x0034FC90
	public QueryContainer RP1TagQuery
	{
		get
		{
			return this.ExposedVariables.RP1TagQuery;
		}
		set
		{
			this.ExposedVariables.RP1TagQuery = value;
		}
	}

	// Token: 0x170007A0 RID: 1952
	// (get) Token: 0x0600B769 RID: 46953 RVA: 0x00351AA0 File Offset: 0x0034FCA0
	// (set) Token: 0x0600B76A RID: 46954 RVA: 0x00351AB0 File Offset: 0x0034FCB0
	public QueryContainer RP2TagQuery
	{
		get
		{
			return this.ExposedVariables.RP2TagQuery;
		}
		set
		{
			this.ExposedVariables.RP2TagQuery = value;
		}
	}

	// Token: 0x170007A1 RID: 1953
	// (get) Token: 0x0600B76B RID: 46955 RVA: 0x00351AC0 File Offset: 0x0034FCC0
	// (set) Token: 0x0600B76C RID: 46956 RVA: 0x00351AD0 File Offset: 0x0034FCD0
	public QueryContainer RP3TagQuery
	{
		get
		{
			return this.ExposedVariables.RP3TagQuery;
		}
		set
		{
			this.ExposedVariables.RP3TagQuery = value;
		}
	}

	// Token: 0x170007A2 RID: 1954
	// (get) Token: 0x0600B76D RID: 46957 RVA: 0x00351AE0 File Offset: 0x0034FCE0
	// (set) Token: 0x0600B76E RID: 46958 RVA: 0x00351AF0 File Offset: 0x0034FCF0
	public QueryContainer RP4TagQuery
	{
		get
		{
			return this.ExposedVariables.RP4TagQuery;
		}
		set
		{
			this.ExposedVariables.RP4TagQuery = value;
		}
	}

	// Token: 0x170007A3 RID: 1955
	// (get) Token: 0x0600B76F RID: 46959 RVA: 0x00351B00 File Offset: 0x0034FD00
	// (set) Token: 0x0600B770 RID: 46960 RVA: 0x00351B10 File Offset: 0x0034FD10
	public QueryContainer RP5TagQuery
	{
		get
		{
			return this.ExposedVariables.RP5TagQuery;
		}
		set
		{
			this.ExposedVariables.RP5TagQuery = value;
		}
	}

	// Token: 0x170007A4 RID: 1956
	// (get) Token: 0x0600B771 RID: 46961 RVA: 0x00351B20 File Offset: 0x0034FD20
	// (set) Token: 0x0600B772 RID: 46962 RVA: 0x00351B30 File Offset: 0x0034FD30
	public QueryContainer RP6TagQuery
	{
		get
		{
			return this.ExposedVariables.RP6TagQuery;
		}
		set
		{
			this.ExposedVariables.RP6TagQuery = value;
		}
	}

	// Token: 0x170007A5 RID: 1957
	// (get) Token: 0x0600B773 RID: 46963 RVA: 0x00351B40 File Offset: 0x0034FD40
	// (set) Token: 0x0600B774 RID: 46964 RVA: 0x00351B50 File Offset: 0x0034FD50
	public QueryContainer RP7TagQuery
	{
		get
		{
			return this.ExposedVariables.RP7TagQuery;
		}
		set
		{
			this.ExposedVariables.RP7TagQuery = value;
		}
	}

	// Token: 0x170007A6 RID: 1958
	// (get) Token: 0x0600B775 RID: 46965 RVA: 0x00351B60 File Offset: 0x0034FD60
	// (set) Token: 0x0600B776 RID: 46966 RVA: 0x00351B70 File Offset: 0x0034FD70
	public IEnumerable<Entity> RP3Entities
	{
		get
		{
			return this.ExposedVariables.RP3Entities;
		}
		set
		{
			this.ExposedVariables.RP3Entities = value;
		}
	}

	// Token: 0x170007A7 RID: 1959
	// (get) Token: 0x0600B777 RID: 46967 RVA: 0x00351B80 File Offset: 0x0034FD80
	// (set) Token: 0x0600B778 RID: 46968 RVA: 0x00351B90 File Offset: 0x0034FD90
	public IEnumerable<Entity> RP2Entities
	{
		get
		{
			return this.ExposedVariables.RP2Entities;
		}
		set
		{
			this.ExposedVariables.RP2Entities = value;
		}
	}

	// Token: 0x170007A8 RID: 1960
	// (get) Token: 0x0600B779 RID: 46969 RVA: 0x00351BA0 File Offset: 0x0034FDA0
	// (set) Token: 0x0600B77A RID: 46970 RVA: 0x00351BB0 File Offset: 0x0034FDB0
	public IEnumerable<Entity> RP5Entities
	{
		get
		{
			return this.ExposedVariables.RP5Entities;
		}
		set
		{
			this.ExposedVariables.RP5Entities = value;
		}
	}

	// Token: 0x170007A9 RID: 1961
	// (get) Token: 0x0600B77B RID: 46971 RVA: 0x00351BC0 File Offset: 0x0034FDC0
	// (set) Token: 0x0600B77C RID: 46972 RVA: 0x00351BD0 File Offset: 0x0034FDD0
	public IEnumerable<Entity> RP7Entities
	{
		get
		{
			return this.ExposedVariables.RP7Entities;
		}
		set
		{
			this.ExposedVariables.RP7Entities = value;
		}
	}

	// Token: 0x170007AA RID: 1962
	// (get) Token: 0x0600B77D RID: 46973 RVA: 0x00351BE0 File Offset: 0x0034FDE0
	// (set) Token: 0x0600B77E RID: 46974 RVA: 0x00351BF0 File Offset: 0x0034FDF0
	public IEnumerable<Entity> RP6Entities
	{
		get
		{
			return this.ExposedVariables.RP6Entities;
		}
		set
		{
			this.ExposedVariables.RP6Entities = value;
		}
	}

	// Token: 0x170007AB RID: 1963
	// (get) Token: 0x0600B77F RID: 46975 RVA: 0x00351C00 File Offset: 0x0034FE00
	// (set) Token: 0x0600B780 RID: 46976 RVA: 0x00351C10 File Offset: 0x0034FE10
	public IEnumerable<Entity> RP1Entities
	{
		get
		{
			return this.ExposedVariables.RP1Entities;
		}
		set
		{
			this.ExposedVariables.RP1Entities = value;
		}
	}

	// Token: 0x170007AC RID: 1964
	// (get) Token: 0x0600B781 RID: 46977 RVA: 0x00351C20 File Offset: 0x0034FE20
	// (set) Token: 0x0600B782 RID: 46978 RVA: 0x00351C30 File Offset: 0x0034FE30
	public IEnumerable<Entity> RP4Entities
	{
		get
		{
			return this.ExposedVariables.RP4Entities;
		}
		set
		{
			this.ExposedVariables.RP4Entities = value;
		}
	}

	// Token: 0x170007AD RID: 1965
	// (get) Token: 0x0600B783 RID: 46979 RVA: 0x00351C40 File Offset: 0x0034FE40
	// (set) Token: 0x0600B784 RID: 46980 RVA: 0x00351C50 File Offset: 0x0034FE50
	public UnitSpawnWaveData AshokaGuardPostWave
	{
		get
		{
			return this.ExposedVariables.AshokaGuardPostWave;
		}
		set
		{
			this.ExposedVariables.AshokaGuardPostWave = value;
		}
	}

	// Token: 0x170007AE RID: 1966
	// (get) Token: 0x0600B785 RID: 46981 RVA: 0x00351C60 File Offset: 0x0034FE60
	// (set) Token: 0x0600B786 RID: 46982 RVA: 0x00351C70 File Offset: 0x0034FE70
	public QueryContainer AshokaSSAAQuery
	{
		get
		{
			return this.ExposedVariables.AshokaSSAAQuery;
		}
		set
		{
			this.ExposedVariables.AshokaSSAAQuery = value;
		}
	}

	// Token: 0x170007AF RID: 1967
	// (get) Token: 0x0600B787 RID: 46983 RVA: 0x00351C80 File Offset: 0x0034FE80
	// (set) Token: 0x0600B788 RID: 46984 RVA: 0x00351C90 File Offset: 0x0034FE90
	public QueryContainer AshokaQueryRP
	{
		get
		{
			return this.ExposedVariables.AshokaQueryRP;
		}
		set
		{
			this.ExposedVariables.AshokaQueryRP = value;
		}
	}

	// Token: 0x170007B0 RID: 1968
	// (get) Token: 0x0600B789 RID: 46985 RVA: 0x00351CA0 File Offset: 0x0034FEA0
	// (set) Token: 0x0600B78A RID: 46986 RVA: 0x00351CB0 File Offset: 0x0034FEB0
	public UnitSpawnWaveData AshokaGuardPostWaveHard
	{
		get
		{
			return this.ExposedVariables.AshokaGuardPostWaveHard;
		}
		set
		{
			this.ExposedVariables.AshokaGuardPostWaveHard = value;
		}
	}

	// Token: 0x170007B1 RID: 1969
	// (get) Token: 0x0600B78B RID: 46987 RVA: 0x00351CC0 File Offset: 0x0034FEC0
	// (set) Token: 0x0600B78C RID: 46988 RVA: 0x00351CD0 File Offset: 0x0034FED0
	public UnitSpawnWaveData AshokaGuardPostWaveEasy
	{
		get
		{
			return this.ExposedVariables.AshokaGuardPostWaveEasy;
		}
		set
		{
			this.ExposedVariables.AshokaGuardPostWaveEasy = value;
		}
	}

	// Token: 0x0600B78D RID: 46989 RVA: 0x00351CE0 File Offset: 0x0034FEE0
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

	// Token: 0x0600B78E RID: 46990 RVA: 0x00351D48 File Offset: 0x0034FF48
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600B78F RID: 46991 RVA: 0x00351D58 File Offset: 0x0034FF58
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600B790 RID: 46992 RVA: 0x00351D68 File Offset: 0x0034FF68
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600B791 RID: 46993 RVA: 0x00351D78 File Offset: 0x0034FF78
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600B792 RID: 46994 RVA: 0x00351D88 File Offset: 0x0034FF88
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401193A RID: 71994
	public M07_RPTracking ExposedVariables = new M07_RPTracking();
}
