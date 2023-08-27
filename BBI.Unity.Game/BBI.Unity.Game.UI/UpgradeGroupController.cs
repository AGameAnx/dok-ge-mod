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
	public class UpgradeGroupController : MonoBehaviour
	{
		public IEnumerable<ResearchButtonController> ResearchButtons
		{
			get
			{
				return this.mResearchButtons;
			}
		}

		public int NumAvailableUpgrades
		{
			get
			{
				return this.mNumAvailableUpgrades;
			}
		}

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

		public void Hide()
		{
			NGUITools.SetActiveSelf(base.gameObject, false);
		}

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

		private void RepositionButtons()
		{
			if (this.m_ButtonGrid != null)
			{
				this.m_ButtonGrid.Reposition();
				this.m_ButtonGrid.repositionNow = true;
			}
		}

		public UpgradeGroupController()
		{
		}

		public void UpdateVisibility(CommanderState commanderState, ResearchItemAttributes showOnlyItem, TechTreeAttributes commanderTechTree)
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
					bool flag = ResearchHelperShared.IsResearchItemInUpgrades(researchItem, commanderTechTree) && ResearchHelperShared.AreResearchDependenciesMet(commanderState, researchItem) && !ResearchHelperShared.IsResearchCompleted(researchItem.Name, commanderState) && !ResearchHelperShared.IsResearchLocked(commanderState, researchItem.Name);
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

		[SerializeField]
		private UISprite m_BackgroundSprite;

		[SerializeField]
		private UISprite m_IconSprite;

		[SerializeField]
		private UIGrid m_ButtonGrid;

		[SerializeField]
		private float m_ResearchButtonWidth = 300f;

		[Tooltip("The distance from the top of the group background to the top of the first research button. Also applied at bottom.")]
		[SerializeField]
		private int m_VerticalPadding = 4;

		private TechTree mTechTree;

		private List<ResearchButtonController> mResearchButtons = new List<ResearchButtonController>();

		private int mNumAvailableUpgrades;

		private float mResearchButtonHeight = 30f;
	}
}
