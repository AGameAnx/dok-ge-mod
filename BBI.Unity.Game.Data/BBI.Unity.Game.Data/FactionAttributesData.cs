using System;
using BBI.Core;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Unity.Core.Data;
using BBI.Unity.Core.PropertyAttributes;
using BBI.Unity.Core.Utility;
using UnityEngine;

namespace BBI.Unity.Game.Data
{
	// Token: 0x020000EE RID: 238
	[Serializable]
	public class FactionAttributesData : NamedObjectBase, FactionAttributes, INamed
	{
		// Token: 0x1700032D RID: 813
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x00008B23 File Offset: 0x00006D23
		string FactionAttributes.FactionName
		{
			get
			{
				return this.m_FactionName;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x00008B2B File Offset: 0x00006D2B
		FactionID FactionAttributes.ID
		{
			get
			{
				return this.m_FactionID;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x00008B33 File Offset: 0x00006D33
		string FactionAttributes.VOName
		{
			get
			{
				return this.m_VOName;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x00008B3B File Offset: 0x00006D3B
		string FactionAttributes.VOAssetBundle
		{
			get
			{
				return this.m_VOAssetBundle;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x00008B43 File Offset: 0x00006D43
		string FactionAttributes.MusicTrigger
		{
			get
			{
				return this.m_MusicTrigger;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x00008B4B File Offset: 0x00006D4B
		TechTreeAttributes FactionAttributes.TechTree
		{
			get
			{
				if (this.m_TechTreeAttributes != null)
				{
					return this.m_TechTreeAttributes.Data as TechTreeAttributes;
				}
				return null;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x00008B67 File Offset: 0x00006D67
		bool FactionAttributes.ScaleWithDynamicDifficulty
		{
			get
			{
				return this.m_ScaleWithDynamicDifficulty;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x00008B6F File Offset: 0x00006D6F
		UnitTypeBuff[] FactionAttributes.GlobalBuffs
		{
			get
			{
				return this.m_Buffs;
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00008B78 File Offset: 0x00006D78
		UnitTypeBuff[] FactionAttributes.GetDifficultyBuffs(Difficulty difficulty)
		{
			UnitTypeBuff[] result = null;
			switch (difficulty)
			{
			case Difficulty.None:
				break;
			case Difficulty.Easy:
				result = this.m_EasyBuffs;
				break;
			case Difficulty.Medium:
				result = this.m_MediumBuffs;
				break;
			case Difficulty.Hard:
				result = this.m_HardBuffs;
				break;
			default:
				Log.Warn(Log.Channel.Data, "Unsupported Difficulty type {0}", new object[]
				{
					difficulty
				});
				break;
			}
			return result;
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x00008BDA File Offset: 0x00006DDA
		public AssetBase NotificationAudio
		{
			get
			{
				return this.m_NotificationAudio;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x00008BE2 File Offset: 0x00006DE2
		public AssetBase TechTree
		{
			get
			{
				return this.m_TechTreeAttributes;
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00008BEA File Offset: 0x00006DEA
		public FactionAttributesData()
		{
		}

		public FactionAttributesData Copy()
		{
			return (FactionAttributesData)base.MemberwiseClone();
		}

		// Token: 0x04000480 RID: 1152
		[SerializeField]
		[Tooltip("Faction name LocID for display purposes. Also used for CommanderAttributes matching for saves/replays.")]
		public string m_FactionName;

		// Token: 0x04000481 RID: 1153
		[Tooltip("FactionID for gameplay ID purposes, such as achievements")]
		[SerializeField]
		public FactionID m_FactionID;

		// Token: 0x04000482 RID: 1154
		[SerializeField]
		[Tooltip("Faction name for VO purposes")]
		public string m_VOName;

		// Token: 0x04000483 RID: 1155
		[SerializeField]
		[AssetBundlePath]
		public string m_VOAssetBundle;

		// Token: 0x04000484 RID: 1156
		[StreamableAssetTypeProperty(typeof(GameObject))]
		[SerializeField]
		public StreamableAssetContainer m_VOCoreSpeechHierarchy = new StreamableAssetContainer();

		// Token: 0x04000485 RID: 1157
		[FabricClipSelector]
		[SerializeField]
		public string m_MusicTrigger;

		// Token: 0x04000486 RID: 1158
		[SerializeField]
		public NotificationAudioAttributesAsset m_NotificationAudio;

		// Token: 0x04000487 RID: 1159
		[SerializeField]
		public TechTreeAttributesAsset m_TechTreeAttributes;

		// Token: 0x04000488 RID: 1160
		[SerializeField]
		public bool m_ScaleWithDynamicDifficulty;

		// Token: 0x04000489 RID: 1161
		[SerializeField]
		[Tooltip("Buffs that are applied to all units of this faction, regardless of difficulty")]
		private UnitTypeBuffData[] m_Buffs = new UnitTypeBuffData[0];

		// Token: 0x0400048A RID: 1162
		[SerializeField]
		[Header("Difficulty-based Buffs")]
		[Tooltip("Buffs that are applied to all units of this faction for Easy difficulty")]
		private UnitTypeBuffData[] m_EasyBuffs;

		// Token: 0x0400048B RID: 1163
		[SerializeField]
		[Tooltip("Buffs that are applied to all units of this faction for Medium difficulty")]
		private UnitTypeBuffData[] m_MediumBuffs;

		// Token: 0x0400048C RID: 1164
		[SerializeField]
		[Tooltip("Buffs that are applied to all units of this faction for Hard difficulty")]
		private UnitTypeBuffData[] m_HardBuffs;
	}
}
