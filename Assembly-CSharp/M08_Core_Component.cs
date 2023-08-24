using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Game.Data.Queries;
using BBI.Game.Data.Scripting;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Scripting;
using UnityEngine;

// Token: 0x02000862 RID: 2146
[AddComponentMenu("uScript/Graphs/M08_Core")]
public class M08_Core_Component : uScriptCode
{
	// Token: 0x0600BE55 RID: 48725 RVA: 0x00371480 File Offset: 0x0036F680
	public M08_Core_Component()
	{
	}

	// Token: 0x170007B4 RID: 1972
	// (get) Token: 0x0600BE56 RID: 48726 RVA: 0x00371494 File Offset: 0x0036F694
	// (set) Token: 0x0600BE57 RID: 48727 RVA: 0x003714A4 File Offset: 0x0036F6A4
	public List<Entity> CommandCarrier
	{
		get
		{
			return this.ExposedVariables.CommandCarrier;
		}
		set
		{
			this.ExposedVariables.CommandCarrier = value;
		}
	}

	// Token: 0x170007B5 RID: 1973
	// (get) Token: 0x0600BE58 RID: 48728 RVA: 0x003714B4 File Offset: 0x0036F6B4
	// (set) Token: 0x0600BE59 RID: 48729 RVA: 0x003714C4 File Offset: 0x0036F6C4
	public BuffSetAttributesAsset CR_Buff
	{
		get
		{
			return this.ExposedVariables.CR_Buff;
		}
		set
		{
			this.ExposedVariables.CR_Buff = value;
		}
	}

	// Token: 0x170007B6 RID: 1974
	// (get) Token: 0x0600BE5A RID: 48730 RVA: 0x003714D4 File Offset: 0x0036F6D4
	// (set) Token: 0x0600BE5B RID: 48731 RVA: 0x003714E4 File Offset: 0x0036F6E4
	public UnitSpawnWaveData GaalsiCarrier_Wave
	{
		get
		{
			return this.ExposedVariables.GaalsiCarrier_Wave;
		}
		set
		{
			this.ExposedVariables.GaalsiCarrier_Wave = value;
		}
	}

	// Token: 0x170007B7 RID: 1975
	// (get) Token: 0x0600BE5C RID: 48732 RVA: 0x003714F4 File Offset: 0x0036F6F4
	// (set) Token: 0x0600BE5D RID: 48733 RVA: 0x00371504 File Offset: 0x0036F704
	public SpawnFactoryData Gaalsi_SB_Group2
	{
		get
		{
			return this.ExposedVariables.Gaalsi_SB_Group2;
		}
		set
		{
			this.ExposedVariables.Gaalsi_SB_Group2 = value;
		}
	}

	// Token: 0x170007B8 RID: 1976
	// (get) Token: 0x0600BE5E RID: 48734 RVA: 0x00371514 File Offset: 0x0036F714
	// (set) Token: 0x0600BE5F RID: 48735 RVA: 0x00371524 File Offset: 0x0036F724
	public SpawnFactoryData GSB_Escort2
	{
		get
		{
			return this.ExposedVariables.GSB_Escort2;
		}
		set
		{
			this.ExposedVariables.GSB_Escort2 = value;
		}
	}

	// Token: 0x170007B9 RID: 1977
	// (get) Token: 0x0600BE60 RID: 48736 RVA: 0x00371534 File Offset: 0x0036F734
	// (set) Token: 0x0600BE61 RID: 48737 RVA: 0x00371544 File Offset: 0x0036F744
	public UnitSpawnWaveData Gaalsi_BR2_Initial
	{
		get
		{
			return this.ExposedVariables.Gaalsi_BR2_Initial;
		}
		set
		{
			this.ExposedVariables.Gaalsi_BR2_Initial = value;
		}
	}

	// Token: 0x170007BA RID: 1978
	// (get) Token: 0x0600BE62 RID: 48738 RVA: 0x00371554 File Offset: 0x0036F754
	// (set) Token: 0x0600BE63 RID: 48739 RVA: 0x00371564 File Offset: 0x0036F764
	public UnitSpawnWaveData Gaalsi_Escorts2_Initial
	{
		get
		{
			return this.ExposedVariables.Gaalsi_Escorts2_Initial;
		}
		set
		{
			this.ExposedVariables.Gaalsi_Escorts2_Initial = value;
		}
	}

	// Token: 0x170007BB RID: 1979
	// (get) Token: 0x0600BE64 RID: 48740 RVA: 0x00371574 File Offset: 0x0036F774
	// (set) Token: 0x0600BE65 RID: 48741 RVA: 0x00371584 File Offset: 0x0036F784
	public UnitSpawnWaveData Gaalsi_BR3_Initial
	{
		get
		{
			return this.ExposedVariables.Gaalsi_BR3_Initial;
		}
		set
		{
			this.ExposedVariables.Gaalsi_BR3_Initial = value;
		}
	}

	// Token: 0x170007BC RID: 1980
	// (get) Token: 0x0600BE66 RID: 48742 RVA: 0x00371594 File Offset: 0x0036F794
	// (set) Token: 0x0600BE67 RID: 48743 RVA: 0x003715A4 File Offset: 0x0036F7A4
	public UnitSpawnWaveData Gaalsi_Escorts3_Initial
	{
		get
		{
			return this.ExposedVariables.Gaalsi_Escorts3_Initial;
		}
		set
		{
			this.ExposedVariables.Gaalsi_Escorts3_Initial = value;
		}
	}

	// Token: 0x170007BD RID: 1981
	// (get) Token: 0x0600BE68 RID: 48744 RVA: 0x003715B4 File Offset: 0x0036F7B4
	// (set) Token: 0x0600BE69 RID: 48745 RVA: 0x003715C4 File Offset: 0x0036F7C4
	public UnitSpawnWaveData Gaalsi_BR4_Initial
	{
		get
		{
			return this.ExposedVariables.Gaalsi_BR4_Initial;
		}
		set
		{
			this.ExposedVariables.Gaalsi_BR4_Initial = value;
		}
	}

	// Token: 0x170007BE RID: 1982
	// (get) Token: 0x0600BE6A RID: 48746 RVA: 0x003715D4 File Offset: 0x0036F7D4
	// (set) Token: 0x0600BE6B RID: 48747 RVA: 0x003715E4 File Offset: 0x0036F7E4
	public UnitSpawnWaveData Gaalsi_Escorts4_Initial
	{
		get
		{
			return this.ExposedVariables.Gaalsi_Escorts4_Initial;
		}
		set
		{
			this.ExposedVariables.Gaalsi_Escorts4_Initial = value;
		}
	}

	// Token: 0x170007BF RID: 1983
	// (get) Token: 0x0600BE6C RID: 48748 RVA: 0x003715F4 File Offset: 0x0036F7F4
	// (set) Token: 0x0600BE6D RID: 48749 RVA: 0x00371604 File Offset: 0x0036F804
	public SpawnFactoryData Gaalsi_SB_Group1
	{
		get
		{
			return this.ExposedVariables.Gaalsi_SB_Group1;
		}
		set
		{
			this.ExposedVariables.Gaalsi_SB_Group1 = value;
		}
	}

	// Token: 0x170007C0 RID: 1984
	// (get) Token: 0x0600BE6E RID: 48750 RVA: 0x00371614 File Offset: 0x0036F814
	// (set) Token: 0x0600BE6F RID: 48751 RVA: 0x00371624 File Offset: 0x0036F824
	public SpawnFactoryData GSB_Escort1
	{
		get
		{
			return this.ExposedVariables.GSB_Escort1;
		}
		set
		{
			this.ExposedVariables.GSB_Escort1 = value;
		}
	}

	// Token: 0x170007C1 RID: 1985
	// (get) Token: 0x0600BE70 RID: 48752 RVA: 0x00371634 File Offset: 0x0036F834
	// (set) Token: 0x0600BE71 RID: 48753 RVA: 0x00371644 File Offset: 0x0036F844
	public SpawnFactoryData Gaalsi_SB_Group3
	{
		get
		{
			return this.ExposedVariables.Gaalsi_SB_Group3;
		}
		set
		{
			this.ExposedVariables.Gaalsi_SB_Group3 = value;
		}
	}

	// Token: 0x170007C2 RID: 1986
	// (get) Token: 0x0600BE72 RID: 48754 RVA: 0x00371654 File Offset: 0x0036F854
	// (set) Token: 0x0600BE73 RID: 48755 RVA: 0x00371664 File Offset: 0x0036F864
	public SpawnFactoryData GSB_Escort3
	{
		get
		{
			return this.ExposedVariables.GSB_Escort3;
		}
		set
		{
			this.ExposedVariables.GSB_Escort3 = value;
		}
	}

	// Token: 0x170007C3 RID: 1987
	// (get) Token: 0x0600BE74 RID: 48756 RVA: 0x00371674 File Offset: 0x0036F874
	// (set) Token: 0x0600BE75 RID: 48757 RVA: 0x00371684 File Offset: 0x0036F884
	public SpawnFactoryData Gaalsi_SB_Group4
	{
		get
		{
			return this.ExposedVariables.Gaalsi_SB_Group4;
		}
		set
		{
			this.ExposedVariables.Gaalsi_SB_Group4 = value;
		}
	}

	// Token: 0x170007C4 RID: 1988
	// (get) Token: 0x0600BE76 RID: 48758 RVA: 0x00371694 File Offset: 0x0036F894
	// (set) Token: 0x0600BE77 RID: 48759 RVA: 0x003716A4 File Offset: 0x0036F8A4
	public SpawnFactoryData GSB_Escort4
	{
		get
		{
			return this.ExposedVariables.GSB_Escort4;
		}
		set
		{
			this.ExposedVariables.GSB_Escort4 = value;
		}
	}

	// Token: 0x170007C5 RID: 1989
	// (get) Token: 0x0600BE78 RID: 48760 RVA: 0x003716B4 File Offset: 0x0036F8B4
	// (set) Token: 0x0600BE79 RID: 48761 RVA: 0x003716C4 File Offset: 0x0036F8C4
	public UnitSpawnWaveData G_Carrier_Defenders
	{
		get
		{
			return this.ExposedVariables.G_Carrier_Defenders;
		}
		set
		{
			this.ExposedVariables.G_Carrier_Defenders = value;
		}
	}

	// Token: 0x170007C6 RID: 1990
	// (get) Token: 0x0600BE7A RID: 48762 RVA: 0x003716D4 File Offset: 0x0036F8D4
	// (set) Token: 0x0600BE7B RID: 48763 RVA: 0x003716E4 File Offset: 0x0036F8E4
	public UnitSpawnWaveData G_Carrier_Repair
	{
		get
		{
			return this.ExposedVariables.G_Carrier_Repair;
		}
		set
		{
			this.ExposedVariables.G_Carrier_Repair = value;
		}
	}

	// Token: 0x170007C7 RID: 1991
	// (get) Token: 0x0600BE7C RID: 48764 RVA: 0x003716F4 File Offset: 0x0036F8F4
	// (set) Token: 0x0600BE7D RID: 48765 RVA: 0x00371704 File Offset: 0x0036F904
	public UnitSpawnWaveData Gaalsi_Ramp_Defenders
	{
		get
		{
			return this.ExposedVariables.Gaalsi_Ramp_Defenders;
		}
		set
		{
			this.ExposedVariables.Gaalsi_Ramp_Defenders = value;
		}
	}

	// Token: 0x170007C8 RID: 1992
	// (get) Token: 0x0600BE7E RID: 48766 RVA: 0x00371714 File Offset: 0x0036F914
	// (set) Token: 0x0600BE7F RID: 48767 RVA: 0x00371724 File Offset: 0x0036F924
	public UnitSpawnWaveData NIS_Cruiser
	{
		get
		{
			return this.ExposedVariables.NIS_Cruiser;
		}
		set
		{
			this.ExposedVariables.NIS_Cruiser = value;
		}
	}

	// Token: 0x170007C9 RID: 1993
	// (get) Token: 0x0600BE80 RID: 48768 RVA: 0x00371734 File Offset: 0x0036F934
	// (set) Token: 0x0600BE81 RID: 48769 RVA: 0x00371744 File Offset: 0x0036F944
	public UnitSpawnWaveData Resource_Dropoff
	{
		get
		{
			return this.ExposedVariables.Resource_Dropoff;
		}
		set
		{
			this.ExposedVariables.Resource_Dropoff = value;
		}
	}

	// Token: 0x170007CA RID: 1994
	// (get) Token: 0x0600BE82 RID: 48770 RVA: 0x00371754 File Offset: 0x0036F954
	// (set) Token: 0x0600BE83 RID: 48771 RVA: 0x00371764 File Offset: 0x0036F964
	public QueryContainer GOD
	{
		get
		{
			return this.ExposedVariables.GOD;
		}
		set
		{
			this.ExposedVariables.GOD = value;
		}
	}

	// Token: 0x170007CB RID: 1995
	// (get) Token: 0x0600BE84 RID: 48772 RVA: 0x00371774 File Offset: 0x0036F974
	// (set) Token: 0x0600BE85 RID: 48773 RVA: 0x00371784 File Offset: 0x0036F984
	public QueryContainer SUSP
	{
		get
		{
			return this.ExposedVariables.SUSP;
		}
		set
		{
			this.ExposedVariables.SUSP = value;
		}
	}

	// Token: 0x170007CC RID: 1996
	// (get) Token: 0x0600BE86 RID: 48774 RVA: 0x00371794 File Offset: 0x0036F994
	// (set) Token: 0x0600BE87 RID: 48775 RVA: 0x003717A4 File Offset: 0x0036F9A4
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

	// Token: 0x170007CD RID: 1997
	// (get) Token: 0x0600BE88 RID: 48776 RVA: 0x003717B4 File Offset: 0x0036F9B4
	// (set) Token: 0x0600BE89 RID: 48777 RVA: 0x003717C4 File Offset: 0x0036F9C4
	public QueryContainer Rachel_Query
	{
		get
		{
			return this.ExposedVariables.Rachel_Query;
		}
		set
		{
			this.ExposedVariables.Rachel_Query = value;
		}
	}

	// Token: 0x170007CE RID: 1998
	// (get) Token: 0x0600BE8A RID: 48778 RVA: 0x003717D4 File Offset: 0x0036F9D4
	// (set) Token: 0x0600BE8B RID: 48779 RVA: 0x003717E4 File Offset: 0x0036F9E4
	public QueryContainer Kapisi_Query
	{
		get
		{
			return this.ExposedVariables.Kapisi_Query;
		}
		set
		{
			this.ExposedVariables.Kapisi_Query = value;
		}
	}

	// Token: 0x170007CF RID: 1999
	// (get) Token: 0x0600BE8C RID: 48780 RVA: 0x003717F4 File Offset: 0x0036F9F4
	// (set) Token: 0x0600BE8D RID: 48781 RVA: 0x00371804 File Offset: 0x0036FA04
	public AttributeBuffSetData GCarrier_Buff
	{
		get
		{
			return this.ExposedVariables.GCarrier_Buff;
		}
		set
		{
			this.ExposedVariables.GCarrier_Buff = value;
		}
	}

	// Token: 0x170007D0 RID: 2000
	// (get) Token: 0x0600BE8E RID: 48782 RVA: 0x00371814 File Offset: 0x0036FA14
	// (set) Token: 0x0600BE8F RID: 48783 RVA: 0x00371824 File Offset: 0x0036FA24
	public BuffSetAttributesAsset GSalvagerSpeedDebuff
	{
		get
		{
			return this.ExposedVariables.GSalvagerSpeedDebuff;
		}
		set
		{
			this.ExposedVariables.GSalvagerSpeedDebuff = value;
		}
	}

	// Token: 0x170007D1 RID: 2001
	// (get) Token: 0x0600BE90 RID: 48784 RVA: 0x00371834 File Offset: 0x0036FA34
	// (set) Token: 0x0600BE91 RID: 48785 RVA: 0x00371844 File Offset: 0x0036FA44
	public UnitSpawnWaveData Gaalsi_Turrets
	{
		get
		{
			return this.ExposedVariables.Gaalsi_Turrets;
		}
		set
		{
			this.ExposedVariables.Gaalsi_Turrets = value;
		}
	}

	// Token: 0x0600BE92 RID: 48786 RVA: 0x00371854 File Offset: 0x0036FA54
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

	// Token: 0x0600BE93 RID: 48787 RVA: 0x003718BC File Offset: 0x0036FABC
	private void Start()
	{
		this.ExposedVariables.Start();
	}

	// Token: 0x0600BE94 RID: 48788 RVA: 0x003718CC File Offset: 0x0036FACC
	private void OnEnable()
	{
		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0600BE95 RID: 48789 RVA: 0x003718DC File Offset: 0x0036FADC
	private void OnDisable()
	{
		this.ExposedVariables.OnDisable();
	}

	// Token: 0x0600BE96 RID: 48790 RVA: 0x003718EC File Offset: 0x0036FAEC
	private void Update()
	{
		this.ExposedVariables.Update();
	}

	// Token: 0x0600BE97 RID: 48791 RVA: 0x003718FC File Offset: 0x0036FAFC
	private void OnDestroy()
	{
		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x04012330 RID: 74544
	public M08_Core ExposedVariables = new M08_Core();
}
