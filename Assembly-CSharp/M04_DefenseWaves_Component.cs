using System;
using BBI.Game.Data.Scripting;
using UnityEngine;

// Token: 0x02000826 RID: 2086
[AddComponentMenu("uScript/Graphs/M04_DefenseWaves")]
public class M04_DefenseWaves_Component : uScriptCode
{
	// Token: 0x06007048 RID: 28744 RVA: 0x001FB788 File Offset: 0x001F9988
	public M04_DefenseWaves_Component()
	{
	}

	// Token: 0x17000672 RID: 1650
	// (get) Token: 0x06007049 RID: 28745 RVA: 0x001FB79C File Offset: 0x001F999C
	// (set) Token: 0x0600704A RID: 28746 RVA: 0x001FB7AC File Offset: 0x001F99AC
	public UnitSpawnWaveData DefenseWave2
	{
		get
		{
			return this.ExposedVariables.DefenseWave2;
		}
		set
		{
			this.ExposedVariables.DefenseWave2 = value;
		}
	}

	// Token: 0x17000673 RID: 1651
	// (get) Token: 0x0600704B RID: 28747 RVA: 0x001FB7BC File Offset: 0x001F99BC
	// (set) Token: 0x0600704C RID: 28748 RVA: 0x001FB7CC File Offset: 0x001F99CC
	public UnitSpawnWaveData DefenseWave3
	{
		get
		{
			return this.ExposedVariables.DefenseWave3;
		}
		set
		{
			this.ExposedVariables.DefenseWave3 = value;
		}
	}

	// Token: 0x17000674 RID: 1652
	// (get) Token: 0x0600704D RID: 28749 RVA: 0x001FB7DC File Offset: 0x001F99DC
	// (set) Token: 0x0600704E RID: 28750 RVA: 0x001FB7EC File Offset: 0x001F99EC
	public UnitSpawnWaveData DefenseWaveFinal
	{
		get
		{
			return this.ExposedVariables.DefenseWaveFinal;
		}
		set
		{
			this.ExposedVariables.DefenseWaveFinal = value;
		}
	}

	// Token: 0x17000675 RID: 1653
	// (get) Token: 0x0600704F RID: 28751 RVA: 0x001FB7FC File Offset: 0x001F99FC
	// (set) Token: 0x06007050 RID: 28752 RVA: 0x001FB80C File Offset: 0x001F9A0C
	public UnitSpawnWaveData DefenseWave4
	{
		get
		{
			return this.ExposedVariables.DefenseWave4;
		}
		set
		{
			this.ExposedVariables.DefenseWave4 = value;
		}
	}

	// Token: 0x17000676 RID: 1654
	// (get) Token: 0x06007051 RID: 28753 RVA: 0x001FB81C File Offset: 0x001F9A1C
	// (set) Token: 0x06007052 RID: 28754 RVA: 0x001FB82C File Offset: 0x001F9A2C
	public UnitSpawnWaveData DefenseWave5
	{
		get
		{
			return this.ExposedVariables.DefenseWave5;
		}
		set
		{
			this.ExposedVariables.DefenseWave5 = value;
		}
	}

	// Token: 0x17000677 RID: 1655
	// (get) Token: 0x06007053 RID: 28755 RVA: 0x001FB83C File Offset: 0x001F9A3C
	// (set) Token: 0x06007054 RID: 28756 RVA: 0x001FB84C File Offset: 0x001F9A4C
	public UnitSpawnWaveData DefenseWave1
	{
		get
		{
			return this.ExposedVariables.DefenseWave1;
		}
		set
		{
			this.ExposedVariables.DefenseWave1 = value;
		}
	}

	// Token: 0x17000678 RID: 1656
	// (get) Token: 0x06007055 RID: 28757 RVA: 0x001FB85C File Offset: 0x001F9A5C
	// (set) Token: 0x06007056 RID: 28758 RVA: 0x001FB86C File Offset: 0x001F9A6C
	public UnitSpawnWaveData Gaalsi_Rock
	{
		get
		{
			return this.ExposedVariables.Gaalsi_Rock;
		}
		set
		{
			this.ExposedVariables.Gaalsi_Rock = value;
		}
	}

	// Token: 0x17000679 RID: 1657
	// (get) Token: 0x06007057 RID: 28759 RVA: 0x001FB87C File Offset: 0x001F9A7C
	// (set) Token: 0x06007058 RID: 28760 RVA: 0x001FB88C File Offset: 0x001F9A8C
	public UnitSpawnWaveData Gaalsi_Paper
	{
		get
		{
			return this.ExposedVariables.Gaalsi_Paper;
		}
		set
		{
			this.ExposedVariables.Gaalsi_Paper = value;
		}
	}

	// Token: 0x1700067A RID: 1658
	// (get) Token: 0x06007059 RID: 28761 RVA: 0x001FB89C File Offset: 0x001F9A9C
	// (set) Token: 0x0600705A RID: 28762 RVA: 0x001FB8AC File Offset: 0x001F9AAC
	public UnitSpawnWaveData Gaalsi_Scissors
	{
		get
		{
			return this.ExposedVariables.Gaalsi_Scissors;
		}
		set
		{
			this.ExposedVariables.Gaalsi_Scissors = value;
		}
	}

	// Token: 0x0600705B RID: 28763 RVA: 0x001FB8BC File Offset: 0x001F9ABC
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

	// Token: 0x0600705C RID: 28764 RVA: 0x001FB924 File Offset: 0x001F9B24
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0600705D RID: 28765 RVA: 0x001FB934 File Offset: 0x001F9B34
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600705E RID: 28766 RVA: 0x001FB944 File Offset: 0x001F9B44
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600705F RID: 28767 RVA: 0x001FB954 File Offset: 0x001F9B54
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06007060 RID: 28768 RVA: 0x001FB964 File Offset: 0x001F9B64
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0400A01F RID: 40991
	public M04_DefenseWaves ExposedVariables = new M04_DefenseWaves();
}
