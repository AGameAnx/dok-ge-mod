using System;
using BBI.Game.Data.Queries;
using BBI.Unity.Game.Data;
using UnityEngine;

// Token: 0x0200081C RID: 2076
[AddComponentMenu("uScript/Graphs/M03_Core")]
public class M03_Core_Component : uScriptCode
{
	// Token: 0x0600582E RID: 22574 RVA: 0x00182C58 File Offset: 0x00180E58
	public M03_Core_Component()
	{
	}

	// Token: 0x1700061B RID: 1563
	// (get) Token: 0x0600582F RID: 22575 RVA: 0x00182C6C File Offset: 0x00180E6C
	// (set) Token: 0x06005830 RID: 22576 RVA: 0x00182C7C File Offset: 0x00180E7C
	public QueryContainer LargePlayerUnitQuery
	{
		get
		{
			return this.ExposedVariables.LargePlayerUnitQuery;
		}
		set
		{
			this.ExposedVariables.LargePlayerUnitQuery = value;
		}
	}

	// Token: 0x1700061C RID: 1564
	// (get) Token: 0x06005831 RID: 22577 RVA: 0x00182C8C File Offset: 0x00180E8C
	// (set) Token: 0x06005832 RID: 22578 RVA: 0x00182C9C File Offset: 0x00180E9C
	public AttributeBuffSetData ContactRangeDebuff
	{
		get
		{
			return this.ExposedVariables.ContactRangeDebuff;
		}
		set
		{
			this.ExposedVariables.ContactRangeDebuff = value;
		}
	}

	// Token: 0x1700061D RID: 1565
	// (get) Token: 0x06005833 RID: 22579 RVA: 0x00182CAC File Offset: 0x00180EAC
	// (set) Token: 0x06005834 RID: 22580 RVA: 0x00182CBC File Offset: 0x00180EBC
	public AttributeBuffSetData SensorRangeDebuff
	{
		get
		{
			return this.ExposedVariables.SensorRangeDebuff;
		}
		set
		{
			this.ExposedVariables.SensorRangeDebuff = value;
		}
	}

	// Token: 0x1700061E RID: 1566
	// (get) Token: 0x06005835 RID: 22581 RVA: 0x00182CCC File Offset: 0x00180ECC
	// (set) Token: 0x06005836 RID: 22582 RVA: 0x00182CDC File Offset: 0x00180EDC
	public QueryContainer AllGaalsienQuery
	{
		get
		{
			return this.ExposedVariables.AllGaalsienQuery;
		}
		set
		{
			this.ExposedVariables.AllGaalsienQuery = value;
		}
	}

	// Token: 0x1700061F RID: 1567
	// (get) Token: 0x06005837 RID: 22583 RVA: 0x00182CEC File Offset: 0x00180EEC
	// (set) Token: 0x06005838 RID: 22584 RVA: 0x00182CFC File Offset: 0x00180EFC
	public AttributeBuffSetData GaalsienSensorDebuff
	{
		get
		{
			return this.ExposedVariables.GaalsienSensorDebuff;
		}
		set
		{
			this.ExposedVariables.GaalsienSensorDebuff = value;
		}
	}

	// Token: 0x17000620 RID: 1568
	// (get) Token: 0x06005839 RID: 22585 RVA: 0x00182D0C File Offset: 0x00180F0C
	// (set) Token: 0x0600583A RID: 22586 RVA: 0x00182D1C File Offset: 0x00180F1C
	public AttributeBuffSetData GaalsienDamageDebuff
	{
		get
		{
			return this.ExposedVariables.GaalsienDamageDebuff;
		}
		set
		{
			this.ExposedVariables.GaalsienDamageDebuff = value;
		}
	}

	// Token: 0x0600583B RID: 22587 RVA: 0x00182D2C File Offset: 0x00180F2C
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

	// Token: 0x0600583C RID: 22588 RVA: 0x00182D94 File Offset: 0x00180F94
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600583D RID: 22589 RVA: 0x00182DA4 File Offset: 0x00180FA4
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600583E RID: 22590 RVA: 0x00182DB4 File Offset: 0x00180FB4
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600583F RID: 22591 RVA: 0x00182DC4 File Offset: 0x00180FC4
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x06005840 RID: 22592 RVA: 0x00182DD4 File Offset: 0x00180FD4
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04007606 RID: 30214
	public M03_Core ExposedVariables = new M03_Core();
}
