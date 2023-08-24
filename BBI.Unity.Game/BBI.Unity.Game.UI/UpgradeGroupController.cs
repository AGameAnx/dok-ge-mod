using System;
using System.Collections.Generic;
using BBI.Core;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Game.Simulation;
using BBI.Unity.Game.Localize;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	// Token: 0x02000210 RID: 528
	public class UpgradeGroupController : MonoBehaviour
	{
		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x00045072 File Offset: 0x00043272
		public IEnumerable<ResearchButtonController> ResearchButtons
		{
			get
			{
				return this.mResearchButtons;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x0004507A File Offset: 0x0004327A
		public int NumAvailableUpgrades
		{
			get
			{
				return this.mNumAvailableUpgrades;
			}
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00045084 File Offset: 0x00043284
		private void Awake()
		{
			if (this.m_BackgroundSprite == null)
			{
				Log.Warn(Log.Channel.UI, "NULL BackgroundSprite in {0}", new object[]
				{
					base.GetType()
				});
			}
			if (this.m_IconSprite == null)
			{
				Log.Warn(Log.Channel.UI, "NULL IconSprite in {0}", new object[]
				{
					base.GetType()
				});
			}
			if (this.m_ButtonGrid == null)
			{
				Log.Warn(Log.Channel.UI, "NULL ButtonGrid in {0}", new object[]
				{
					base.GetType()
				});
			}
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0004511C File Offset: 0x0004331C
		public void Setup(TechTree techTree, ResearchButtonController prefab, IGameLocalization localizationManager)
		{
			this.mTechTree = techTree;
			if (techTree == null || techTree.Upgrades.IsNullOrEmpty<TechUpgrade>() || prefab == null)
			{
				return;
			}
			this.mResearchButtonHeight = prefab.Size.y;
			if (this.m_IconSprite != null && !string.IsNullOrEmpty(this.mTechTree.IconSpriteName))
			{
				this.m_IconSprite.spriteName = this.mTechTree.IconSpriteName;
			}
			foreach (TechUpgrade upgrade in techTree.Upgrades)
			{
				this.AddResearchItem(upgrade, prefab, localizationManager);
			}
			this.RepositionButtons();
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x000451B9 File Offset: 0x000433B9
		public void Hide()
		{
			NGUITools.SetActiveSelf(base.gameObject, false);
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x000451C8 File Offset: 0x000433C8
		public void UpdateVisibility(CommanderState commanderState, ResearchItemAttributes showOnlyItem)
		{
			if (commanderState == null)
			{
				return;
			}
			int num = 0;
			this.mNumAvailableUpgrades = 0;
			foreach (ResearchButtonController researchButtonController in this.mResearchButtons)
			{
				if (!(researchButtonController == null) && researchButtonController.ResearchItem != null)
				{
					ResearchItemAttributes researchItem = researchButtonController.ResearchItem;
					bool flag = ResearchHelperShared.AreResearchDependenciesMet(commanderState, researchItem) && !ResearchHelperShared.IsResearchCompleted(researchItem.Name, commanderState) && !ResearchHelperShared.IsResearchLocked(commanderState, researchItem.Name);
					if (flag)
					{
						this.mNumAvailableUpgrades++;
					}
					bool flag2 = flag;
					if (showOnlyItem != null)
					{
						flag2 = (researchItem == showOnlyItem);
					}
					if (flag2)
					{
						NGUITools.SetActiveSelf(researchButtonController.gameObject, true);
						researchButtonController.Size = new Vector2(this.m_ResearchButtonWidth, this.mResearchButtonHeight);
						num++;
					}
					else
					{
						NGUITools.SetActiveSelf(researchButtonController.gameObject, false);
					}
				}
			}
			if (num > 0 && this.m_ButtonGrid != null)
			{
				this.RepositionButtons();
				if (this.m_BackgroundSprite != null)
				{
					this.m_BackgroundSprite.height = Mathf.CeilToInt(this.m_ButtonGrid.cellHeight) * num + this.m_VerticalPadding * 2;
				}
			}
			NGUITools.SetActiveSelf(base.gameObject, num > 0);
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00045324 File Offset: 0x00043524
		private void AddResearchItem(TechUpgrade upgrade, ResearchButtonController prefab, IGameLocalization localizationManager)
		{
			if (this.m_ButtonGrid == null)
			{
				return;
			}
			ResearchItemAttributes entityTypeAttributes = ShipbreakersMain.GetEntityTypeAttributes<ResearchItemAttributes>(upgrade.ResearchItem);
			if (entityTypeAttributes == null)
			{
				Log.Error(Log.Channel.UI, "Unable to find research item {0}. Please make sure that it is added to the entity list in master!", new object[]
				{
					upgrade.ResearchItem
				});
				return;
			}
			ResearchButtonController component = NGUITools.AddChild(this.m_ButtonGrid.gameObject, prefab.gameObject).GetComponent<ResearchButtonController>();
			component.gameObject.name = string.Format("Upgrade - {0}", upgrade.ResearchItem);
			component.InitializeResearchItem(entityTypeAttributes, localizationManager);
			component.IsActiveResearch = false;
			this.mResearchButtons.Add(component);
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x000453C2 File Offset: 0x000435C2
		private void RepositionButtons()
		{
			if (this.m_ButtonGrid != null)
			{
				this.m_ButtonGrid.Reposition();
				this.m_ButtonGrid.repositionNow = true;
			}
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x000453E9 File Offset: 0x000435E9
		public UpgradeGroupController()
		{
		}

		// Token: 0x04000C3C RID: 3132
		[SerializeField]
		private UISprite m_BackgroundSprite;

		// Token: 0x04000C3D RID: 3133
		[SerializeField]
		private UISprite m_IconSprite;

		// Token: 0x04000C3E RID: 3134
		[SerializeField]
		private UIGrid m_ButtonGrid;

		// Token: 0x04000C3F RID: 3135
		[SerializeField]
		private float m_ResearchButtonWidth = 300f;

		// Token: 0x04000C40 RID: 3136
		[Tooltip("The distance from the top of the group background to the top of the first research button. Also applied at bottom.")]
		[SerializeField]
		private int m_VerticalPadding = 4;

		// Token: 0x04000C41 RID: 3137
		private TechTree mTechTree;

		// Token: 0x04000C42 RID: 3138
		private List<ResearchButtonController> mResearchButtons = new List<ResearchButtonController>();

		// Token: 0x04000C43 RID: 3139
		private int mNumAvailableUpgrades;

		// Token: 0x04000C44 RID: 3140
		private float mResearchButtonHeight = 30f;
	}
}
