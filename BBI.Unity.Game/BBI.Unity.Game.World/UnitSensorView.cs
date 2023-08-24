using System;
using UnityEngine;

namespace BBI.Unity.Game.World
{
	// Token: 0x0200038A RID: 906
	public class UnitSensorView
	{
		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001EB5 RID: 7861 RVA: 0x000B4F0A File Offset: 0x000B310A
		// (set) Token: 0x06001EB6 RID: 7862 RVA: 0x000B4F12 File Offset: 0x000B3112
		public float SensorRange
		{
			get
			{
				return this.mSensorRange;
			}
			set
			{
				this.mSensorRange = value;
				this.UpdateRangeState();
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x000B4F21 File Offset: 0x000B3121
		// (set) Token: 0x06001EB8 RID: 7864 RVA: 0x000B4F29 File Offset: 0x000B3129
		public bool Enabled
		{
			get
			{
				return this.mEnabled;
			}
			set
			{
				this.mEnabled = value;
				this.UpdateVisibilityState();
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x000B4F38 File Offset: 0x000B3138
		// (set) Token: 0x06001EBA RID: 7866 RVA: 0x000B4F40 File Offset: 0x000B3140
		public UnitSensorView.SensorMode Mode
		{
			get
			{
				return this.mMode;
			}
			set
			{
				this.mMode = value;
				this.UpdateVisibilityState();
			}
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x000B4F50 File Offset: 0x000B3150
		public UnitSensorView(Transform unitXform, float range, GameObject sensorSpherePrefab, GameObject fowPrefab)
		{
			this.mTransform = unitXform;
			this.mSensorSphereObj = UnityEngine.Object.Instantiate<GameObject>(sensorSpherePrefab);
			UnitSensorView.ParentAndSetIdentity(this.mSensorSphereObj.transform, this.mTransform);
			this.mFOWRangeObj = UnityEngine.Object.Instantiate<GameObject>(fowPrefab);
			UnitSensorView.ParentAndSetIdentity(this.mFOWRangeObj.transform, this.mTransform);
			this.mEnabled = false;
			this.mMode = UnitSensorView.SensorMode.GameView;
			this.UpdateVisibilityState();
			this.mSensorRange = range;
			this.UpdateRangeState();
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x000B4FD0 File Offset: 0x000B31D0
		private void UpdateVisibilityState()
		{
			bool flag = this.mMode == UnitSensorView.SensorMode.SensorsView;
			this.mSensorSphereObj.SetActive(this.Enabled && flag);
			this.mFOWRangeObj.SetActive(this.Enabled && !flag);
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x000B5018 File Offset: 0x000B3218
		private void UpdateRangeState()
		{
			Vector3 localScale = Vector3.one * this.mSensorRange;
			this.mSensorSphereObj.transform.localScale = localScale;
			localScale.y = 1f;
			this.mFOWRangeObj.transform.localScale = localScale;
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x000B5064 File Offset: 0x000B3264
		private static void ParentAndSetIdentity(Transform child, Transform parent)
		{
			child.parent = parent;
			child.localPosition = Vector3.zero;
			child.localRotation = Quaternion.identity;
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x000B5083 File Offset: 0x000B3283
		public bool FOWRangeObjectEquals(GameObject obj)
		{
			return this.mFOWRangeObj == obj;
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x000B5091 File Offset: 0x000B3291
		public bool SensorsSphereObjectEquals(GameObject obj)
		{
			return this.mSensorSphereObj == obj;
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x000B509F File Offset: 0x000B329F
		public void SetIdentityRotations()
		{
			this.mSensorSphereObj.transform.rotation = Quaternion.identity;
			this.mFOWRangeObj.transform.rotation = Quaternion.identity;
		}

		// Token: 0x04001923 RID: 6435
		private GameObject mSensorSphereObj;

		// Token: 0x04001924 RID: 6436
		private GameObject mFOWRangeObj;

		// Token: 0x04001925 RID: 6437
		private Transform mTransform;

		// Token: 0x04001926 RID: 6438
		private float mSensorRange;

		// Token: 0x04001927 RID: 6439
		private bool mEnabled;

		// Token: 0x04001928 RID: 6440
		private UnitSensorView.SensorMode mMode;

		// Token: 0x0200038B RID: 907
		public enum SensorMode
		{
			// Token: 0x0400192A RID: 6442
			GameView,
			// Token: 0x0400192B RID: 6443
			SensorsView
		}
	}
}
