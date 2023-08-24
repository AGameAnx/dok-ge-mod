using System;
using BBI.Game.Data.Queries;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x02000820 RID: 2080
[AddComponentMenu("uScript/Graphs/M03_NIS")]
public class M03_NIS_Component : uScriptCode
{
	// Token: 0x0600669C RID: 26268 RVA: 0x001CBBA4 File Offset: 0x001C9DA4
	public M03_NIS_Component()
	{
	}

	// Token: 0x1700064C RID: 1612
	// (get) Token: 0x0600669D RID: 26269 RVA: 0x001CBBB8 File Offset: 0x001C9DB8
	// (set) Token: 0x0600669E RID: 26270 RVA: 0x001CBBC8 File Offset: 0x001C9DC8
	public QueryContainer CCQueryContainer
	{
		get
		{
			return this.ExposedVariables.CCQueryContainer;
		}
		set
		{
			this.ExposedVariables.CCQueryContainer = value;
		}
	}

	// Token: 0x1700064D RID: 1613
	// (get) Token: 0x0600669F RID: 26271 RVA: 0x001CBBD8 File Offset: 0x001C9DD8
	// (set) Token: 0x060066A0 RID: 26272 RVA: 0x001CBBE8 File Offset: 0x001C9DE8
	public Vector3 CinematicCameraPosition
	{
		get
		{
			return this.ExposedVariables.CinematicCameraPosition;
		}
		set
		{
			this.ExposedVariables.CinematicCameraPosition = value;
		}
	}

	// Token: 0x1700064E RID: 1614
	// (get) Token: 0x060066A1 RID: 26273 RVA: 0x001CBBF8 File Offset: 0x001C9DF8
	// (set) Token: 0x060066A2 RID: 26274 RVA: 0x001CBC08 File Offset: 0x001C9E08
	public Vector3 CinematicCameraRotation
	{
		get
		{
			return this.ExposedVariables.CinematicCameraRotation;
		}
		set
		{
			this.ExposedVariables.CinematicCameraRotation = value;
		}
	}

	// Token: 0x1700064F RID: 1615
	// (get) Token: 0x060066A3 RID: 26275 RVA: 0x001CBC18 File Offset: 0x001C9E18
	// (set) Token: 0x060066A4 RID: 26276 RVA: 0x001CBC28 File Offset: 0x001C9E28
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

	// Token: 0x17000650 RID: 1616
	// (get) Token: 0x060066A5 RID: 26277 RVA: 0x001CBC38 File Offset: 0x001C9E38
	// (set) Token: 0x060066A6 RID: 26278 RVA: 0x001CBC48 File Offset: 0x001C9E48
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

	// Token: 0x17000651 RID: 1617
	// (get) Token: 0x060066A7 RID: 26279 RVA: 0x001CBC58 File Offset: 0x001C9E58
	// (set) Token: 0x060066A8 RID: 26280 RVA: 0x001CBC68 File Offset: 0x001C9E68
	public bool WreckFoundNISStarted
	{
		get
		{
			return this.ExposedVariables.WreckFoundNISStarted;
		}
		set
		{
			this.ExposedVariables.WreckFoundNISStarted = value;
		}
	}

	// Token: 0x17000652 RID: 1618
	// (get) Token: 0x060066A9 RID: 26281 RVA: 0x001CBC78 File Offset: 0x001C9E78
	// (set) Token: 0x060066AA RID: 26282 RVA: 0x001CBC88 File Offset: 0x001C9E88
	public QueryContainer PlayerFleetExceptRachelandKapisi
	{
		get
		{
			return this.ExposedVariables.PlayerFleetExceptRachelandKapisi;
		}
		set
		{
			this.ExposedVariables.PlayerFleetExceptRachelandKapisi = value;
		}
	}

	// Token: 0x17000653 RID: 1619
	// (get) Token: 0x060066AB RID: 26283 RVA: 0x001CBC98 File Offset: 0x001C9E98
	// (set) Token: 0x060066AC RID: 26284 RVA: 0x001CBCA8 File Offset: 0x001C9EA8
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

	// Token: 0x17000654 RID: 1620
	// (get) Token: 0x060066AD RID: 26285 RVA: 0x001CBCB8 File Offset: 0x001C9EB8
	// (set) Token: 0x060066AE RID: 26286 RVA: 0x001CBCC8 File Offset: 0x001C9EC8
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

	// Token: 0x17000655 RID: 1621
	// (get) Token: 0x060066AF RID: 26287 RVA: 0x001CBCD8 File Offset: 0x001C9ED8
	// (set) Token: 0x060066B0 RID: 26288 RVA: 0x001CBCE8 File Offset: 0x001C9EE8
	public QueryContainer HarvestersEscortsAndHacsQuery
	{
		get
		{
			return this.ExposedVariables.HarvestersEscortsAndHacsQuery;
		}
		set
		{
			this.ExposedVariables.HarvestersEscortsAndHacsQuery = value;
		}
	}

	// Token: 0x17000656 RID: 1622
	// (get) Token: 0x060066B1 RID: 26289 RVA: 0x001CBCF8 File Offset: 0x001C9EF8
	// (set) Token: 0x060066B2 RID: 26290 RVA: 0x001CBD08 File Offset: 0x001C9F08
	public GameObject NIS0304Camera
	{
		get
		{
			return this.ExposedVariables.NIS0304Camera;
		}
		set
		{
			this.ExposedVariables.NIS0304Camera = value;
		}
	}

	// Token: 0x17000657 RID: 1623
	// (get) Token: 0x060066B3 RID: 26291 RVA: 0x001CBD18 File Offset: 0x001C9F18
	// (set) Token: 0x060066B4 RID: 26292 RVA: 0x001CBD28 File Offset: 0x001C9F28
	public GameObject NIS0302Camera
	{
		get
		{
			return this.ExposedVariables.NIS0302Camera;
		}
		set
		{
			this.ExposedVariables.NIS0302Camera = value;
		}
	}

	// Token: 0x17000658 RID: 1624
	// (get) Token: 0x060066B5 RID: 26293 RVA: 0x001CBD38 File Offset: 0x001C9F38
	// (set) Token: 0x060066B6 RID: 26294 RVA: 0x001CBD48 File Offset: 0x001C9F48
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

	// Token: 0x17000659 RID: 1625
	// (get) Token: 0x060066B7 RID: 26295 RVA: 0x001CBD58 File Offset: 0x001C9F58
	// (set) Token: 0x060066B8 RID: 26296 RVA: 0x001CBD68 File Offset: 0x001C9F68
	public QueryContainer SupportCruiserQuery
	{
		get
		{
			return this.ExposedVariables.SupportCruiserQuery;
		}
		set
		{
			this.ExposedVariables.SupportCruiserQuery = value;
		}
	}

	// Token: 0x1700065A RID: 1626
	// (get) Token: 0x060066B9 RID: 26297 RVA: 0x001CBD78 File Offset: 0x001C9F78
	// (set) Token: 0x060066BA RID: 26298 RVA: 0x001CBD88 File Offset: 0x001C9F88
	public AttributeBuffSetData TempSlowDebuff
	{
		get
		{
			return this.ExposedVariables.TempSlowDebuff;
		}
		set
		{
			this.ExposedVariables.TempSlowDebuff = value;
		}
	}

	// Token: 0x060066BB RID: 26299 RVA: 0x001CBD98 File Offset: 0x001C9F98
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

	// Token: 0x060066BC RID: 26300 RVA: 0x001CBE00 File Offset: 0x001CA000
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x060066BD RID: 26301 RVA: 0x001CBE10 File Offset: 0x001CA010
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x060066BE RID: 26302 RVA: 0x001CBE20 File Offset: 0x001CA020
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x060066BF RID: 26303 RVA: 0x001CBE30 File Offset: 0x001CA030
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x060066C0 RID: 26304 RVA: 0x001CBE40 File Offset: 0x001CA040
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04009182 RID: 37250
	public M03_NIS ExposedVariables = new M03_NIS();
}
