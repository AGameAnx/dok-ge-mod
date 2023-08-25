using System;
using BBI.Game.Data.Queries;
using BBI.Unity.Game.World;
using UnityEngine;

// Token: 0x02000895 RID: 2197
[AddComponentMenu("uScript/Graphs/M11_Intel")]
public class M11_Intel_Component : uScriptCode
{
	// Token: 0x0601184F RID: 71759 RVA: 0x004F8438 File Offset: 0x004F6638
	public M11_Intel_Component()
	{
	}

	// Token: 0x1700095E RID: 2398
	// (get) Token: 0x06011850 RID: 71760 RVA: 0x004F844C File Offset: 0x004F664C
	// (set) Token: 0x06011851 RID: 71761 RVA: 0x004F845C File Offset: 0x004F665C
	public UserCamera.CinematicControllerToken CamToken2
	{
		get
		{
			return this.ExposedVariables.CamToken2;
		}
		set
		{
			this.ExposedVariables.CamToken2 = value;
		}
	}

	// Token: 0x1700095F RID: 2399
	// (get) Token: 0x06011852 RID: 71762 RVA: 0x004F846C File Offset: 0x004F666C
	// (set) Token: 0x06011853 RID: 71763 RVA: 0x004F847C File Offset: 0x004F667C
	public UserCamera.CinematicControllerToken CamToken4
	{
		get
		{
			return this.ExposedVariables.CamToken4;
		}
		set
		{
			this.ExposedVariables.CamToken4 = value;
		}
	}

	// Token: 0x17000960 RID: 2400
	// (get) Token: 0x06011854 RID: 71764 RVA: 0x004F848C File Offset: 0x004F668C
	// (set) Token: 0x06011855 RID: 71765 RVA: 0x004F849C File Offset: 0x004F669C
	public UserCamera.CinematicControllerToken CamToken5
	{
		get
		{
			return this.ExposedVariables.CamToken5;
		}
		set
		{
			this.ExposedVariables.CamToken5 = value;
		}
	}

	// Token: 0x17000961 RID: 2401
	// (get) Token: 0x06011856 RID: 71766 RVA: 0x004F84AC File Offset: 0x004F66AC
	// (set) Token: 0x06011857 RID: 71767 RVA: 0x004F84BC File Offset: 0x004F66BC
	public string Speech_RunwayDamaged3
	{
		get
		{
			return this.ExposedVariables.Speech_RunwayDamaged3;
		}
		set
		{
			this.ExposedVariables.Speech_RunwayDamaged3 = value;
		}
	}

	// Token: 0x17000962 RID: 2402
	// (get) Token: 0x06011858 RID: 71768 RVA: 0x004F84CC File Offset: 0x004F66CC
	// (set) Token: 0x06011859 RID: 71769 RVA: 0x004F84DC File Offset: 0x004F66DC
	public string Speech_Bombers1
	{
		get
		{
			return this.ExposedVariables.Speech_Bombers1;
		}
		set
		{
			this.ExposedVariables.Speech_Bombers1 = value;
		}
	}

	// Token: 0x17000963 RID: 2403
	// (get) Token: 0x0601185A RID: 71770 RVA: 0x004F84EC File Offset: 0x004F66EC
	// (set) Token: 0x0601185B RID: 71771 RVA: 0x004F84FC File Offset: 0x004F66FC
	public string Speech_RunwayDamaged1
	{
		get
		{
			return this.ExposedVariables.Speech_RunwayDamaged1;
		}
		set
		{
			this.ExposedVariables.Speech_RunwayDamaged1 = value;
		}
	}

	// Token: 0x17000964 RID: 2404
	// (get) Token: 0x0601185C RID: 71772 RVA: 0x004F850C File Offset: 0x004F670C
	// (set) Token: 0x0601185D RID: 71773 RVA: 0x004F851C File Offset: 0x004F671C
	public string Speech_RunwayDamaged2
	{
		get
		{
			return this.ExposedVariables.Speech_RunwayDamaged2;
		}
		set
		{
			this.ExposedVariables.Speech_RunwayDamaged2 = value;
		}
	}

	// Token: 0x17000965 RID: 2405
	// (get) Token: 0x0601185E RID: 71774 RVA: 0x004F852C File Offset: 0x004F672C
	// (set) Token: 0x0601185F RID: 71775 RVA: 0x004F853C File Offset: 0x004F673C
	public QueryContainer NIS2SuspendQ
	{
		get
		{
			return this.ExposedVariables.NIS2SuspendQ;
		}
		set
		{
			this.ExposedVariables.NIS2SuspendQ = value;
		}
	}

	// Token: 0x17000966 RID: 2406
	// (get) Token: 0x06011860 RID: 71776 RVA: 0x004F854C File Offset: 0x004F674C
	// (set) Token: 0x06011861 RID: 71777 RVA: 0x004F855C File Offset: 0x004F675C
	public QueryContainer NIS2AllPlayerUnitsQ
	{
		get
		{
			return this.ExposedVariables.NIS2AllPlayerUnitsQ;
		}
		set
		{
			this.ExposedVariables.NIS2AllPlayerUnitsQ = value;
		}
	}

	// Token: 0x17000967 RID: 2407
	// (get) Token: 0x06011862 RID: 71778 RVA: 0x004F856C File Offset: 0x004F676C
	// (set) Token: 0x06011863 RID: 71779 RVA: 0x004F857C File Offset: 0x004F677C
	public QueryContainer NIS3AllPlayerUnitsQ
	{
		get
		{
			return this.ExposedVariables.NIS3AllPlayerUnitsQ;
		}
		set
		{
			this.ExposedVariables.NIS3AllPlayerUnitsQ = value;
		}
	}

	// Token: 0x17000968 RID: 2408
	// (get) Token: 0x06011864 RID: 71780 RVA: 0x004F858C File Offset: 0x004F678C
	// (set) Token: 0x06011865 RID: 71781 RVA: 0x004F859C File Offset: 0x004F679C
	public QueryContainer NIS3SuspendQ
	{
		get
		{
			return this.ExposedVariables.NIS3SuspendQ;
		}
		set
		{
			this.ExposedVariables.NIS3SuspendQ = value;
		}
	}

	// Token: 0x17000969 RID: 2409
	// (get) Token: 0x06011866 RID: 71782 RVA: 0x004F85AC File Offset: 0x004F67AC
	// (set) Token: 0x06011867 RID: 71783 RVA: 0x004F85BC File Offset: 0x004F67BC
	public QueryContainer NIS3SuspendAllQ
	{
		get
		{
			return this.ExposedVariables.NIS3SuspendAllQ;
		}
		set
		{
			this.ExposedVariables.NIS3SuspendAllQ = value;
		}
	}

	// Token: 0x1700096A RID: 2410
	// (get) Token: 0x06011868 RID: 71784 RVA: 0x004F85CC File Offset: 0x004F67CC
	// (set) Token: 0x06011869 RID: 71785 RVA: 0x004F85DC File Offset: 0x004F67DC
	public GameObject M11KapisiTracker
	{
		get
		{
			return this.ExposedVariables.M11KapisiTracker;
		}
		set
		{
			this.ExposedVariables.M11KapisiTracker = value;
		}
	}

	// Token: 0x1700096B RID: 2411
	// (get) Token: 0x0601186A RID: 71786 RVA: 0x004F85EC File Offset: 0x004F67EC
	// (set) Token: 0x0601186B RID: 71787 RVA: 0x004F85FC File Offset: 0x004F67FC
	public UserCamera.CinematicControllerToken CamToken3
	{
		get
		{
			return this.ExposedVariables.CamToken3;
		}
		set
		{
			this.ExposedVariables.CamToken3 = value;
		}
	}

	// Token: 0x1700096C RID: 2412
	// (get) Token: 0x0601186C RID: 71788 RVA: 0x004F860C File Offset: 0x004F680C
	// (set) Token: 0x0601186D RID: 71789 RVA: 0x004F861C File Offset: 0x004F681C
	public GameObject NIS02_Camera
	{
		get
		{
			return this.ExposedVariables.NIS02_Camera;
		}
		set
		{
			this.ExposedVariables.NIS02_Camera = value;
		}
	}

	// Token: 0x1700096D RID: 2413
	// (get) Token: 0x0601186E RID: 71790 RVA: 0x004F862C File Offset: 0x004F682C
	// (set) Token: 0x0601186F RID: 71791 RVA: 0x004F863C File Offset: 0x004F683C
	public float Intel2_NISBlendTime
	{
		get
		{
			return this.ExposedVariables.Intel2_NISBlendTime;
		}
		set
		{
			this.ExposedVariables.Intel2_NISBlendTime = value;
		}
	}

	// Token: 0x1700096E RID: 2414
	// (get) Token: 0x06011870 RID: 71792 RVA: 0x004F864C File Offset: 0x004F684C
	// (set) Token: 0x06011871 RID: 71793 RVA: 0x004F865C File Offset: 0x004F685C
	public float Intel2_EaseInTime
	{
		get
		{
			return this.ExposedVariables.Intel2_EaseInTime;
		}
		set
		{
			this.ExposedVariables.Intel2_EaseInTime = value;
		}
	}

	// Token: 0x1700096F RID: 2415
	// (get) Token: 0x06011872 RID: 71794 RVA: 0x004F866C File Offset: 0x004F686C
	// (set) Token: 0x06011873 RID: 71795 RVA: 0x004F867C File Offset: 0x004F687C
	public float Intel2_TransitionTime
	{
		get
		{
			return this.ExposedVariables.Intel2_TransitionTime;
		}
		set
		{
			this.ExposedVariables.Intel2_TransitionTime = value;
		}
	}

	// Token: 0x17000970 RID: 2416
	// (get) Token: 0x06011874 RID: 71796 RVA: 0x004F868C File Offset: 0x004F688C
	// (set) Token: 0x06011875 RID: 71797 RVA: 0x004F869C File Offset: 0x004F689C
	public float Intel2_BlackPolyDuration
	{
		get
		{
			return this.ExposedVariables.Intel2_BlackPolyDuration;
		}
		set
		{
			this.ExposedVariables.Intel2_BlackPolyDuration = value;
		}
	}

	// Token: 0x17000971 RID: 2417
	// (get) Token: 0x06011876 RID: 71798 RVA: 0x004F86AC File Offset: 0x004F68AC
	// (set) Token: 0x06011877 RID: 71799 RVA: 0x004F86BC File Offset: 0x004F68BC
	public UserCamera.CinematicControllerToken CamToken1
	{
		get
		{
			return this.ExposedVariables.CamToken1;
		}
		set
		{
			this.ExposedVariables.CamToken1 = value;
		}
	}

	// Token: 0x17000972 RID: 2418
	// (get) Token: 0x06011878 RID: 71800 RVA: 0x004F86CC File Offset: 0x004F68CC
	// (set) Token: 0x06011879 RID: 71801 RVA: 0x004F86DC File Offset: 0x004F68DC
	public QueryContainer NIS1AllPlayerUnitsQ
	{
		get
		{
			return this.ExposedVariables.NIS1AllPlayerUnitsQ;
		}
		set
		{
			this.ExposedVariables.NIS1AllPlayerUnitsQ = value;
		}
	}

	// Token: 0x17000973 RID: 2419
	// (get) Token: 0x0601187A RID: 71802 RVA: 0x004F86EC File Offset: 0x004F68EC
	// (set) Token: 0x0601187B RID: 71803 RVA: 0x004F86FC File Offset: 0x004F68FC
	public QueryContainer NIS1SuspendAllQ
	{
		get
		{
			return this.ExposedVariables.NIS1SuspendAllQ;
		}
		set
		{
			this.ExposedVariables.NIS1SuspendAllQ = value;
		}
	}

	// Token: 0x0601187C RID: 71804 RVA: 0x004F870C File Offset: 0x004F690C
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

	// Token: 0x0601187D RID: 71805 RVA: 0x004F8774 File Offset: 0x004F6974
	private void Start()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Start();
	}

	// Token: 0x0601187E RID: 71806 RVA: 0x004F8784 File Offset: 0x004F6984
	private void OnEnable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnEnable();
	}

	// Token: 0x0601187F RID: 71807 RVA: 0x004F8794 File Offset: 0x004F6994
	private void OnDisable()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDisable();
	}

	// Token: 0x06011880 RID: 71808 RVA: 0x004F87A4 File Offset: 0x004F69A4
	private void Update()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.Update();
	}

	// Token: 0x06011881 RID: 71809 RVA: 0x004F87B4 File Offset: 0x004F69B4
	private void OnDestroy()
	{
		if (MapModManager.CustomLayout)
			return;

		this.ExposedVariables.OnDestroy();
	}

	// Token: 0x0401A45D RID: 107613
	public M11_Intel ExposedVariables = new M11_Intel();
}
