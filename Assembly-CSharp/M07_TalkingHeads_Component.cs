using System;
using UnityEngine;

// Token: 0x02000860 RID: 2144
[AddComponentMenu("uScript/Graphs/M07_TalkingHeads")]
public class M07_TalkingHeads_Component : uScriptCode
{
	// Token: 0x0600BA24 RID: 47652 RVA: 0x0035B664 File Offset: 0x00359864
	public M07_TalkingHeads_Component()
	{
	}

	// Token: 0x170007B2 RID: 1970
	// (get) Token: 0x0600BA25 RID: 47653 RVA: 0x0035B678 File Offset: 0x00359878
	// (set) Token: 0x0600BA26 RID: 47654 RVA: 0x0035B688 File Offset: 0x00359888
	public bool bStopEMP
	{
		get
		{
			return this.ExposedVariables.bStopEMP;
		}
		set
		{
			this.ExposedVariables.bStopEMP = value;
		}
	}

	// Token: 0x170007B3 RID: 1971
	// (get) Token: 0x0600BA27 RID: 47655 RVA: 0x0035B698 File Offset: 0x00359898
	// (set) Token: 0x0600BA28 RID: 47656 RVA: 0x0035B6A8 File Offset: 0x003598A8
	public bool bPatrolKilled
	{
		get
		{
			return this.ExposedVariables.bPatrolKilled;
		}
		set
		{
			this.ExposedVariables.bPatrolKilled = value;
		}
	}

	// Token: 0x0600BA29 RID: 47657 RVA: 0x0035B6B8 File Offset: 0x003598B8
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

	// Token: 0x0600BA2A RID: 47658 RVA: 0x0035B720 File Offset: 0x00359920
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0600BA2B RID: 47659 RVA: 0x0035B730 File Offset: 0x00359930
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600BA2C RID: 47660 RVA: 0x0035B740 File Offset: 0x00359940
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600BA2D RID: 47661 RVA: 0x0035B750 File Offset: 0x00359950
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x0600BA2E RID: 47662 RVA: 0x0035B760 File Offset: 0x00359960
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04011C0D RID: 72717
	public M07_TalkingHeads ExposedVariables = new M07_TalkingHeads();
}
