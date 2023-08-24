using System;
using BBI.Game.Data.Scripting;
using UnityEngine;

// Token: 0x0200081A RID: 2074
[AddComponentMenu("uScript/Graphs/M02_TalkingHeads")]
public class M02_TalkingHeads_Component : uScriptCode
{
	// Token: 0x06005712 RID: 22290 RVA: 0x0017E7CC File Offset: 0x0017C9CC
	public M02_TalkingHeads_Component()
	{
	}

	// Token: 0x17000617 RID: 1559
	// (get) Token: 0x06005713 RID: 22291 RVA: 0x0017E7E0 File Offset: 0x0017C9E0
	// (set) Token: 0x06005714 RID: 22292 RVA: 0x0017E7F0 File Offset: 0x0017C9F0
	public string ID_M02_BONEYARD_HED_99_INT_17374
	{
		get
		{
			return this.ExposedVariables.ID_M02_BONEYARD_HED_99_INT_17374;
		}
		set
		{
			this.ExposedVariables.ID_M02_BONEYARD_HED_99_INT_17374 = value;
		}
	}

	// Token: 0x17000618 RID: 1560
	// (get) Token: 0x06005715 RID: 22293 RVA: 0x0017E800 File Offset: 0x0017CA00
	// (set) Token: 0x06005716 RID: 22294 RVA: 0x0017E810 File Offset: 0x0017CA10
	public UnitSpawnWaveData FlyByWave1
	{
		get
		{
			return this.ExposedVariables.FlyByWave1;
		}
		set
		{
			this.ExposedVariables.FlyByWave1 = value;
		}
	}

	// Token: 0x17000619 RID: 1561
	// (get) Token: 0x06005717 RID: 22295 RVA: 0x0017E820 File Offset: 0x0017CA20
	// (set) Token: 0x06005718 RID: 22296 RVA: 0x0017E830 File Offset: 0x0017CA30
	public UnitSpawnWaveData FlyByWave2
	{
		get
		{
			return this.ExposedVariables.FlyByWave2;
		}
		set
		{
			this.ExposedVariables.FlyByWave2 = value;
		}
	}

	// Token: 0x1700061A RID: 1562
	// (get) Token: 0x06005719 RID: 22297 RVA: 0x0017E840 File Offset: 0x0017CA40
	// (set) Token: 0x0600571A RID: 22298 RVA: 0x0017E850 File Offset: 0x0017CA50
	public bool bTalkingHeadActive
	{
		get
		{
			return this.ExposedVariables.bTalkingHeadActive;
		}
		set
		{
			this.ExposedVariables.bTalkingHeadActive = value;
		}
	}

	// Token: 0x0600571B RID: 22299 RVA: 0x0017E860 File Offset: 0x0017CA60
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

	// Token: 0x0600571C RID: 22300 RVA: 0x0017E8C8 File Offset: 0x0017CAC8
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600571D RID: 22301 RVA: 0x0017E8D8 File Offset: 0x0017CAD8
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600571E RID: 22302 RVA: 0x0017E8E8 File Offset: 0x0017CAE8
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600571F RID: 22303 RVA: 0x0017E8F8 File Offset: 0x0017CAF8
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x06005720 RID: 22304 RVA: 0x0017E908 File Offset: 0x0017CB08
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x040074A2 RID: 29858
	public M02_TalkingHeads ExposedVariables = new M02_TalkingHeads();
}
