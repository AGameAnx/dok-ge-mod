using System;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using UnityEngine;

// Token: 0x02000854 RID: 2132
[AddComponentMenu("uScript/Graphs/M06_NIS")]
public class M06_NIS_Component : uScriptCode
{
	// Token: 0x0600A4F9 RID: 42233 RVA: 0x002F9A18 File Offset: 0x002F7C18
	public M06_NIS_Component()
	{
	}

	// Token: 0x1700072F RID: 1839
	// (get) Token: 0x0600A4FA RID: 42234 RVA: 0x002F9A2C File Offset: 0x002F7C2C
	// (set) Token: 0x0600A4FB RID: 42235 RVA: 0x002F9A3C File Offset: 0x002F7C3C
	public UnitSpawnWaveData Escort1SpawnNIS02
	{
		get
		{
			return this.ExposedVariables.Escort1SpawnNIS02;
		}
		set
		{
			this.ExposedVariables.Escort1SpawnNIS02 = value;
		}
	}

	// Token: 0x17000730 RID: 1840
	// (get) Token: 0x0600A4FC RID: 42236 RVA: 0x002F9A4C File Offset: 0x002F7C4C
	// (set) Token: 0x0600A4FD RID: 42237 RVA: 0x002F9A5C File Offset: 0x002F7C5C
	public UnitSpawnWaveData Escort2SpawnNIS02
	{
		get
		{
			return this.ExposedVariables.Escort2SpawnNIS02;
		}
		set
		{
			this.ExposedVariables.Escort2SpawnNIS02 = value;
		}
	}

	// Token: 0x17000731 RID: 1841
	// (get) Token: 0x0600A4FE RID: 42238 RVA: 0x002F9A6C File Offset: 0x002F7C6C
	// (set) Token: 0x0600A4FF RID: 42239 RVA: 0x002F9A7C File Offset: 0x002F7C7C
	public QueryContainer CarrierQuery
	{
		get
		{
			return this.ExposedVariables.CarrierQuery;
		}
		set
		{
			this.ExposedVariables.CarrierQuery = value;
		}
	}

	// Token: 0x17000732 RID: 1842
	// (get) Token: 0x0600A500 RID: 42240 RVA: 0x002F9A8C File Offset: 0x002F7C8C
	// (set) Token: 0x0600A501 RID: 42241 RVA: 0x002F9A9C File Offset: 0x002F7C9C
	public QueryContainer EmptyQuery
	{
		get
		{
			return this.ExposedVariables.EmptyQuery;
		}
		set
		{
			this.ExposedVariables.EmptyQuery = value;
		}
	}

	// Token: 0x17000733 RID: 1843
	// (get) Token: 0x0600A502 RID: 42242 RVA: 0x002F9AAC File Offset: 0x002F7CAC
	// (set) Token: 0x0600A503 RID: 42243 RVA: 0x002F9ABC File Offset: 0x002F7CBC
	public Vector3 NIS0702CameraPosition
	{
		get
		{
			return this.ExposedVariables.NIS0702CameraPosition;
		}
		set
		{
			this.ExposedVariables.NIS0702CameraPosition = value;
		}
	}

	// Token: 0x17000734 RID: 1844
	// (get) Token: 0x0600A504 RID: 42244 RVA: 0x002F9ACC File Offset: 0x002F7CCC
	// (set) Token: 0x0600A505 RID: 42245 RVA: 0x002F9ADC File Offset: 0x002F7CDC
	public Vector3 NIS0702CameraRotation
	{
		get
		{
			return this.ExposedVariables.NIS0702CameraRotation;
		}
		set
		{
			this.ExposedVariables.NIS0702CameraRotation = value;
		}
	}

	// Token: 0x17000735 RID: 1845
	// (get) Token: 0x0600A506 RID: 42246 RVA: 0x002F9AEC File Offset: 0x002F7CEC
	// (set) Token: 0x0600A507 RID: 42247 RVA: 0x002F9AFC File Offset: 0x002F7CFC
	public Vector3 NIS0703CameraRotation
	{
		get
		{
			return this.ExposedVariables.NIS0703CameraRotation;
		}
		set
		{
			this.ExposedVariables.NIS0703CameraRotation = value;
		}
	}

	// Token: 0x17000736 RID: 1846
	// (get) Token: 0x0600A508 RID: 42248 RVA: 0x002F9B0C File Offset: 0x002F7D0C
	// (set) Token: 0x0600A509 RID: 42249 RVA: 0x002F9B1C File Offset: 0x002F7D1C
	public Vector3 NIS0703CameraPosition
	{
		get
		{
			return this.ExposedVariables.NIS0703CameraPosition;
		}
		set
		{
			this.ExposedVariables.NIS0703CameraPosition = value;
		}
	}

	// Token: 0x17000737 RID: 1847
	// (get) Token: 0x0600A50A RID: 42250 RVA: 0x002F9B2C File Offset: 0x002F7D2C
	// (set) Token: 0x0600A50B RID: 42251 RVA: 0x002F9B3C File Offset: 0x002F7D3C
	public QueryContainer PlayerFleet
	{
		get
		{
			return this.ExposedVariables.PlayerFleet;
		}
		set
		{
			this.ExposedVariables.PlayerFleet = value;
		}
	}

	// Token: 0x17000738 RID: 1848
	// (get) Token: 0x0600A50C RID: 42252 RVA: 0x002F9B4C File Offset: 0x002F7D4C
	// (set) Token: 0x0600A50D RID: 42253 RVA: 0x002F9B5C File Offset: 0x002F7D5C
	public QueryContainer AllGaalsien
	{
		get
		{
			return this.ExposedVariables.AllGaalsien;
		}
		set
		{
			this.ExposedVariables.AllGaalsien = value;
		}
	}

	// Token: 0x17000739 RID: 1849
	// (get) Token: 0x0600A50E RID: 42254 RVA: 0x002F9B6C File Offset: 0x002F7D6C
	// (set) Token: 0x0600A50F RID: 42255 RVA: 0x002F9B7C File Offset: 0x002F7D7C
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

	// Token: 0x1700073A RID: 1850
	// (get) Token: 0x0600A510 RID: 42256 RVA: 0x002F9B8C File Offset: 0x002F7D8C
	// (set) Token: 0x0600A511 RID: 42257 RVA: 0x002F9B9C File Offset: 0x002F7D9C
	public UnitSpawnWaveData ReturningFleetNISSpawn
	{
		get
		{
			return this.ExposedVariables.ReturningFleetNISSpawn;
		}
		set
		{
			this.ExposedVariables.ReturningFleetNISSpawn = value;
		}
	}

	// Token: 0x1700073B RID: 1851
	// (get) Token: 0x0600A512 RID: 42258 RVA: 0x002F9BAC File Offset: 0x002F7DAC
	// (set) Token: 0x0600A513 RID: 42259 RVA: 0x002F9BBC File Offset: 0x002F7DBC
	public QueryContainer AllCommanders
	{
		get
		{
			return this.ExposedVariables.AllCommanders;
		}
		set
		{
			this.ExposedVariables.AllCommanders = value;
		}
	}

	// Token: 0x1700073C RID: 1852
	// (get) Token: 0x0600A514 RID: 42260 RVA: 0x002F9BCC File Offset: 0x002F7DCC
	// (set) Token: 0x0600A515 RID: 42261 RVA: 0x002F9BDC File Offset: 0x002F7DDC
	public QueryContainer PlayerFleetExceptRachel
	{
		get
		{
			return this.ExposedVariables.PlayerFleetExceptRachel;
		}
		set
		{
			this.ExposedVariables.PlayerFleetExceptRachel = value;
		}
	}

	// Token: 0x1700073D RID: 1853
	// (get) Token: 0x0600A516 RID: 42262 RVA: 0x002F9BEC File Offset: 0x002F7DEC
	// (set) Token: 0x0600A517 RID: 42263 RVA: 0x002F9BFC File Offset: 0x002F7DFC
	public QueryContainer EverythingExceptRachel
	{
		get
		{
			return this.ExposedVariables.EverythingExceptRachel;
		}
		set
		{
			this.ExposedVariables.EverythingExceptRachel = value;
		}
	}

	// Token: 0x1700073E RID: 1854
	// (get) Token: 0x0600A518 RID: 42264 RVA: 0x002F9C0C File Offset: 0x002F7E0C
	// (set) Token: 0x0600A519 RID: 42265 RVA: 0x002F9C1C File Offset: 0x002F7E1C
	public GameObject NIS0701Camera
	{
		get
		{
			return this.ExposedVariables.NIS0701Camera;
		}
		set
		{
			this.ExposedVariables.NIS0701Camera = value;
		}
	}

	// Token: 0x1700073F RID: 1855
	// (get) Token: 0x0600A51A RID: 42266 RVA: 0x002F9C2C File Offset: 0x002F7E2C
	// (set) Token: 0x0600A51B RID: 42267 RVA: 0x002F9C3C File Offset: 0x002F7E3C
	public GameObject NIS0703Camera
	{
		get
		{
			return this.ExposedVariables.NIS0703Camera;
		}
		set
		{
			this.ExposedVariables.NIS0703Camera = value;
		}
	}

	// Token: 0x0600A51C RID: 42268 RVA: 0x002F9C4C File Offset: 0x002F7E4C
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

	// Token: 0x0600A51D RID: 42269 RVA: 0x002F9CB4 File Offset: 0x002F7EB4
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600A51E RID: 42270 RVA: 0x002F9CC4 File Offset: 0x002F7EC4
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600A51F RID: 42271 RVA: 0x002F9CD4 File Offset: 0x002F7ED4
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600A520 RID: 42272 RVA: 0x002F9CE4 File Offset: 0x002F7EE4
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600A521 RID: 42273 RVA: 0x002F9CF4 File Offset: 0x002F7EF4
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400F8BA RID: 63674
	public M06_NIS ExposedVariables = new M06_NIS();
}
