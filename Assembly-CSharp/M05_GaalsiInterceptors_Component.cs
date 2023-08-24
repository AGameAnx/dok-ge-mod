using System;
using BBI.Game.Data.Scripting;
using UnityEngine;

// Token: 0x02000838 RID: 2104
[AddComponentMenu("uScript/Graphs/M05_GaalsiInterceptors")]
public class M05_GaalsiInterceptors_Component : uScriptCode
{
	// Token: 0x0600862B RID: 34347 RVA: 0x0026783C File Offset: 0x00265A3C
	public M05_GaalsiInterceptors_Component()
	{
	}

	// Token: 0x170006B1 RID: 1713
	// (get) Token: 0x0600862C RID: 34348 RVA: 0x00267850 File Offset: 0x00265A50
	// (set) Token: 0x0600862D RID: 34349 RVA: 0x00267860 File Offset: 0x00265A60
	public UnitSpawnWaveData Gaalsi_Interceptor_Squad
	{
		get
		{
			return this.ExposedVariables.Gaalsi_Interceptor_Squad;
		}
		set
		{
			this.ExposedVariables.Gaalsi_Interceptor_Squad = value;
		}
	}

	// Token: 0x170006B2 RID: 1714
	// (get) Token: 0x0600862E RID: 34350 RVA: 0x00267870 File Offset: 0x00265A70
	// (set) Token: 0x0600862F RID: 34351 RVA: 0x00267880 File Offset: 0x00265A80
	public bool Wreck4_Occupied
	{
		get
		{
			return this.ExposedVariables.Wreck4_Occupied;
		}
		set
		{
			this.ExposedVariables.Wreck4_Occupied = value;
		}
	}

	// Token: 0x170006B3 RID: 1715
	// (get) Token: 0x06008630 RID: 34352 RVA: 0x00267890 File Offset: 0x00265A90
	// (set) Token: 0x06008631 RID: 34353 RVA: 0x002678A0 File Offset: 0x00265AA0
	public bool Wreck2_Occupied
	{
		get
		{
			return this.ExposedVariables.Wreck2_Occupied;
		}
		set
		{
			this.ExposedVariables.Wreck2_Occupied = value;
		}
	}

	// Token: 0x170006B4 RID: 1716
	// (get) Token: 0x06008632 RID: 34354 RVA: 0x002678B0 File Offset: 0x00265AB0
	// (set) Token: 0x06008633 RID: 34355 RVA: 0x002678C0 File Offset: 0x00265AC0
	public bool Wreck3_Occupied
	{
		get
		{
			return this.ExposedVariables.Wreck3_Occupied;
		}
		set
		{
			this.ExposedVariables.Wreck3_Occupied = value;
		}
	}

	// Token: 0x170006B5 RID: 1717
	// (get) Token: 0x06008634 RID: 34356 RVA: 0x002678D0 File Offset: 0x00265AD0
	// (set) Token: 0x06008635 RID: 34357 RVA: 0x002678E0 File Offset: 0x00265AE0
	public int Player_Wreck_Score
	{
		get
		{
			return this.ExposedVariables.Player_Wreck_Score;
		}
		set
		{
			this.ExposedVariables.Player_Wreck_Score = value;
		}
	}

	// Token: 0x170006B6 RID: 1718
	// (get) Token: 0x06008636 RID: 34358 RVA: 0x002678F0 File Offset: 0x00265AF0
	// (set) Token: 0x06008637 RID: 34359 RVA: 0x00267900 File Offset: 0x00265B00
	public float MinDelay
	{
		get
		{
			return this.ExposedVariables.MinDelay;
		}
		set
		{
			this.ExposedVariables.MinDelay = value;
		}
	}

	// Token: 0x170006B7 RID: 1719
	// (get) Token: 0x06008638 RID: 34360 RVA: 0x00267910 File Offset: 0x00265B10
	// (set) Token: 0x06008639 RID: 34361 RVA: 0x00267920 File Offset: 0x00265B20
	public float MaxDelay
	{
		get
		{
			return this.ExposedVariables.MaxDelay;
		}
		set
		{
			this.ExposedVariables.MaxDelay = value;
		}
	}

	// Token: 0x170006B8 RID: 1720
	// (get) Token: 0x0600863A RID: 34362 RVA: 0x00267930 File Offset: 0x00265B30
	// (set) Token: 0x0600863B RID: 34363 RVA: 0x00267940 File Offset: 0x00265B40
	public float G_AttackDelay
	{
		get
		{
			return this.ExposedVariables.G_AttackDelay;
		}
		set
		{
			this.ExposedVariables.G_AttackDelay = value;
		}
	}

	// Token: 0x0600863C RID: 34364 RVA: 0x00267950 File Offset: 0x00265B50
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

	// Token: 0x0600863D RID: 34365 RVA: 0x002679B8 File Offset: 0x00265BB8
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600863E RID: 34366 RVA: 0x002679C8 File Offset: 0x00265BC8
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600863F RID: 34367 RVA: 0x002679D8 File Offset: 0x00265BD8
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06008640 RID: 34368 RVA: 0x002679E8 File Offset: 0x00265BE8
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x06008641 RID: 34369 RVA: 0x002679F8 File Offset: 0x00265BF8
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400C330 RID: 49968
	public M05_GaalsiInterceptors ExposedVariables = new M05_GaalsiInterceptors();
}
