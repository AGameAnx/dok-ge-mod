using System;
using System.Collections.Generic;
using BBI.Core;
using BBI.Core.ComponentModel;
using BBI.Core.Events;
using BBI.Core.Localization;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Commands;
using BBI.Game.Data;
using BBI.Game.Simulation;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.Events;
using BBI.Unity.Game.Localize;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	// Token: 0x0200014C RID: 332
	public class InformationPanelController : IDisposable
	{
		// Token: 0x06000C4E RID: 3150 RVA: 0x00056AF0 File Offset: 0x00054CF0
		public InformationPanelController(InformationPanelController.InformationPanelSettings settings, UnitHUDInteractionAttributes unitInteractionAttributes, UnitInterfaceController interfaceCon, BlackbirdPanelBase.BlackbirdPanelGlobalLifetimeDependencyContainer globalDependencies, BlackbirdPanelBase.BlackbirdPanelSessionDependencyContainer sessionDependencies)
		{
			this.mSettings = settings;
			this.mUnitInteractionAttributes = unitInteractionAttributes;
			this.mInterfaceController = interfaceCon;
			this.mInterfaceController.NewSelection += this.OnUnitSelectionChanged;
			this.mInterfaceController.NewFrameFromSim += this.OnNewStateFrame;
			this.mInterfaceController.NewLeadUnitSelected += this.OnNewLeadUnitSelected;
			globalDependencies.Get<IGameLocalization>(out this.mLocMan);
			sessionDependencies.Get<ICommanderManager>(out this.mCommanderManager);
			sessionDependencies.Get<ICommanderInteractionProvider>(out this.mInteractionProvider);
			NGUITools.SetActiveSelf(this.mSettings.InformationPanelContainer, false);
			ShipbreakersMain.PresentationEventSystem.AddHandler<UIHighlightPanelEvent>(new BBI.Core.Events.EventHandler<UIHighlightPanelEvent>(this.OnUIHighlightPanelEvent));
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x0000A4BB File Offset: 0x000086BB
		private void OnNewLeadUnitSelected(Entity selection)
		{
			if (selection.IsValid())
			{
				NGUITools.SetActiveSelf(this.mSettings.InformationPanelContainer, true);
				this.PopulatePanelForUnit(selection);
				return;
			}
			this.mSelectedEntity = Entity.None;
			NGUITools.SetActiveSelf(this.mSettings.InformationPanelContainer, false);
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00056BDC File Offset: 0x00054DDC
		public void TogglePanel(bool value)
		{
			if (this.mSettings != null && this.mSettings.TogglePanelUIGameObjectReference != null)
			{
				this.mSettings.TogglePanelUIGameObjectReference.SetActive(value);
				return;
			}
			Log.Error(Log.Channel.UI, "Could not Toggle Panel in {0} TogglePanel because TogglePanelUIGameObjectReference is NULL!", new object[]
			{
				base.GetType()
			});
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00056C34 File Offset: 0x00054E34
		public bool PanelIsActive()
		{
			if (this.mSettings != null && this.mSettings.TogglePanelUIGameObjectReference != null)
			{
				return this.mSettings.TogglePanelUIGameObjectReference.activeSelf;
			}
			Log.Error(Log.Channel.UI, "Could not determine if Information panel is active in {0} because TogglePanelUIGameObjectReference is NULL!", new object[]
			{
				base.GetType()
			});
			return true;
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00056C8C File Offset: 0x00054E8C
		private void OnUnitSelectionChanged(IList<Entity> selection, SimStateFrame atFrame)
		{
			if (selection.Count > 0 && this.mInterfaceController.LeadUnit.IsValid())
			{
				NGUITools.SetActiveSelf(this.mSettings.InformationPanelContainer, true);
				this.PopulatePanelForUnit(this.mInterfaceController.LeadUnit);
				return;
			}
			this.mSelectedEntity = Entity.None;
			NGUITools.SetActiveSelf(this.mSettings.InformationPanelContainer, false);
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00056CF4 File Offset: 0x00054EF4
		private void OnUIHighlightPanelEvent(UIHighlightPanelEvent ev)
		{
			if (ev.PanelToHighlight == UIPanelToHighlight.UnitInformation)
			{
				if (this.mSettings.HighlightPanelGameObject != null)
				{
					this.mSettings.HighlightPanelGameObject.SetActive(ev.Enabled);
					return;
				}
				Log.Error(Log.Channel.UI, "Could not toggle Highlight for UnitInformation Panel in {0} because m_HighlightPanelGameObject is unassigned!", new object[]
				{
					base.GetType()
				});
			}
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00056D54 File Offset: 0x00054F54
		private void PopulatePanelForUnit(Entity entity)
		{
			string typeName = entity.GetTypeName();
			if (string.IsNullOrEmpty(typeName))
			{
				return;
			}
			UnitHUDAttributes entityTypeAttributes = ShipbreakersMain.GetEntityTypeAttributes<UnitHUDAttributes>(typeName);
			if (entityTypeAttributes == null)
			{
				Log.Error(Log.Channel.UI, "Unable to get HUD attributes for entity {0}", new object[]
				{
					entity.ToFriendlyString()
				});
			}
			this.mSettings.UnitNameLabel.text = this.mSettings.UnitNameLocID.TokenFormat(new object[]
			{
				(entityTypeAttributes == null) ? typeName : entityTypeAttributes.UnitLocalizedNameStringID
			}, this.mLocMan);
			if (this.mSettings.UnitIcon != null)
			{
				this.mSettings.UnitIcon.spriteName = ((entityTypeAttributes == null) ? string.Empty : entityTypeAttributes.IconSettings.UnitInfoPanelIconName);
			}
			else
			{
				Log.Warn(Log.Channel.UI, "Unit Icon is missing from the info panel.", new object[0]);
			}
			if (this.mSettings.UnitShortDescriptionValueLabel != null)
			{
				this.mLastExperienceState = null;
				this.mSettings.UnitShortDescriptionValueLabel.text = this.mSettings.UnitNameLocID.TokenFormat(new object[]
				{
					(entityTypeAttributes == null) ? string.Empty : entityTypeAttributes.UnitLocalizedShortDescriptionStringID
				}, this.mLocMan);
			}
			else
			{
				Log.Warn(Log.Channel.UI, "ShortDescription Value Label is missing from the info panel.", new object[0]);
			}
			if (this.mSettings.UnitHealthBar != null && this.mInteractionProvider != null && this.mInteractionProvider != null)
			{
				SimStateFrame currentSimFrame = ShipbreakersMain.CurrentSimFrame;
				UnitState unitState = currentSimFrame.FindObject<UnitState>(entity);
				CommanderRelationship relationship = this.mInteractionProvider.GetRelationship(unitState.OwnerCommander, this.mCommanderManager.LocalCommanderID);
				if (relationship != CommanderRelationship.Ally)
				{
					if (relationship != CommanderRelationship.Enemy)
					{
						this.mSettings.UnitHealthBar.foregroundWidget.color = this.mUnitInteractionAttributes.PlayerIconColours.SelectedColour;
					}
					else
					{
						this.mSettings.UnitHealthBar.foregroundWidget.color = this.mUnitInteractionAttributes.EnemyIconColours.SelectedColour;
					}
				}
				else
				{
					this.mSettings.UnitHealthBar.foregroundWidget.color = this.mUnitInteractionAttributes.AllyIconColours.SelectedColour;
				}
			}
			if (this.mSettings.UnitLongDescriptionValueLabel != null)
			{
				this.mSettings.UnitLongDescriptionValueLabel.text = this.mSettings.UnitNameLocID.TokenFormat(new object[]
				{
					(entityTypeAttributes == null) ? string.Empty : entityTypeAttributes.UnitLocalizedLongDescriptionStringID
				}, this.mLocMan);
			}
			else
			{
				Log.Warn(Log.Channel.UI, "LongDescription Value Label is missing from the info panel.", new object[0]);
			}
			this.mSelectedEntity = entity;
			this.UpdateDynamicDataForUnit(entity);
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00056FE4 File Offset: 0x000551E4
		private void UpdateDynamicDataForUnit(Entity entity)
		{
			UnitState unitState = ShipbreakersMain.CurrentSimFrame.FindObject<UnitState>(entity);
			if (unitState == null)
			{
				return;
			}
			if (this.mSelectedEntity.IsValid())
			{
				if (this.mLastUnitState == unitState)
				{
					return;
				}
				if (this.mSettings.UnitHealthValueLabel == null || this.mSettings.UnitHealthBar == null || this.mSettings.UnitArmorValueLabel == null || this.mSettings.UnitFireRateLabel == null || this.mSettings.UnitDamageValueLabel == null || this.mSettings.SpeedValueLabel == null || this.mSettings.UnitShortDescriptionValueLabel == null)
				{
					Log.Warn(Log.Channel.UI, "Information Panel: An information label was not configured correctly.  Check Unity Config.   Skipping Info Panel Update", new object[0]);
					return;
				}
				string typeID = unitState.TypeID;
				bool flag = this.mLastUnitState == null || this.mLastUnitState.EntityID != this.mSelectedEntity;
				ExperienceViewAttributes entityTypeAttributes = ShipbreakersMain.GetEntityTypeAttributes<ExperienceViewAttributes>(typeID);
				if (entityTypeAttributes != null)
				{
					ExperienceState experienceState = ShipbreakersMain.CurrentSimFrame.FindObject<ExperienceState>(entity);
					if (experienceState != null && experienceState.Level > 0 && (flag || this.mLastExperienceState == null || this.mLastExperienceState.Level != experienceState.Level))
					{
						if (!string.IsNullOrEmpty(experienceState.NameSuffix))
						{
							RankViewAttributes rankViewAttributes = entityTypeAttributes.RankViews[experienceState.Level];
							this.mTempLocalizationFormatObjects[0] = rankViewAttributes.ShortRankName;
							this.mTempLocalizationFormatObjects[1] = experienceState.NameSuffix;
							this.mSettings.UnitShortDescriptionValueLabel.text = this.mSettings.UnitLevelFormat.TokenFormat(this.mTempLocalizationFormatObjects, this.mLocMan);
						}
						this.mLastExperienceState = experienceState;
					}
				}
				if (flag || unitState.OwnerCommander != this.mLastUnitState.OwnerCommander)
				{
					Commander commanderFromID = this.mCommanderManager.GetCommanderFromID(unitState.OwnerCommander);
					CommanderDirectorAttributes commanderDirectorFromID = this.mCommanderManager.GetCommanderDirectorFromID(unitState.OwnerCommander);
					this.mTempLocalizationFormatObjects[0] = ((commanderFromID != null) ? SharedLocIDConstants.GetLocalizedCommanderName(commanderDirectorFromID.PlayerType, commanderFromID.Name, commanderDirectorFromID.AIDifficulty) : string.Empty);
					this.mTempLocalizationFormatObjects[1] = null;
					this.mSettings.PlayerNameLabel.text = this.mSettings.PlayerNameLocID.TokenFormat(this.mTempLocalizationFormatObjects, this.mLocMan);
				}
				UnitAttributes typeAttributes = this.mSelectedEntity.GetTypeAttributes<UnitAttributes>();
				if (typeAttributes == null)
				{
					Log.Warn(Log.Channel.UI, "Information Panel: Attribute data was missing for unit type {0}. Skipping info panel update", new object[]
					{
						this.mSelectedEntity.ToFriendlyString()
					});
					return;
				}
				UnitAttributes entityTypeAttributes2 = ShipbreakersMain.GetEntityTypeAttributes<UnitAttributes>(typeID);
				if (entityTypeAttributes2 == null)
				{
					Log.Error(Log.Channel.Data | Log.Channel.UI, "Failed to find base UnitAttributes for entity type {0}", new object[]
					{
						typeID
					});
				}
				if (flag || unitState.Hitpoints != this.mLastUnitState.Hitpoints)
				{
					this.mTempLocalizationFormatObjects[0] = unitState.Hitpoints;
					this.mTempLocalizationFormatObjects[1] = unitState.MaxHitpoints;
					this.mSettings.UnitHealthValueLabel.text = ((unitState.MaxHitpoints > 0) ? this.mSettings.UnitHealthFormat.TokenFormat(this.mTempLocalizationFormatObjects, this.mLocMan) : string.Empty);
					this.mSettings.UnitHealthBar.value = ((unitState.MaxHitpoints > 0) ? ((float)unitState.Hitpoints / (float)unitState.MaxHitpoints) : 0f);
					int buffComparison = 0;
					if (entityTypeAttributes2 != null)
					{
						buffComparison = unitState.MaxHitpoints.CompareTo(entityTypeAttributes2.MaxHealth);
					}
					this.mSettings.UnitHealthValueLabel.color = this.mUnitInteractionAttributes.BuffInfo.BuffColor(buffComparison);
				}
				this.mSelectedEntity.GetTypeAttributes<UnitMovementAttributes>();
				int num = Fixed64.IntValue(unitState.MaxSpeed);
				if (flag || num != Fixed64.IntValue(this.mLastUnitState.MaxSpeed))
				{
					this.mTempLocalizationFormatObjects[0] = num;
					this.mTempLocalizationFormatObjects[1] = null;
					this.mSettings.SpeedValueLabel.text = this.mSettings.UnitSpeedFormat.TokenFormat(this.mTempLocalizationFormatObjects, this.mLocMan);
					int buffComparison2 = 0;
					UnitMovementAttributes entityTypeAttributes3 = ShipbreakersMain.GetEntityTypeAttributes<UnitMovementAttributes>(typeID);
					UnitDynamicsAttributes unitDynamicsAttributes = (entityTypeAttributes3 != null) ? entityTypeAttributes3.Dynamics : null;
					if (unitDynamicsAttributes != null)
					{
						buffComparison2 = unitState.MaxSpeed.CompareTo(unitDynamicsAttributes.MaxSpeed);
					}
					else
					{
						Log.Error(Log.Channel.Data | Log.Channel.UI, "Failed to find base UnitDynamicsAttributes for entity type {0}", new object[]
						{
							typeID
						});
					}
					this.mSettings.SpeedValueLabel.color = this.mUnitInteractionAttributes.BuffInfo.BuffColor(buffComparison2);
				}
				int buffComparison3 = 0;
				int baseDamagePerRound = 0;
				int damagePacketsPerShot = 1;
				if (!typeAttributes.WeaponLoadout.IsNullOrEmpty<WeaponBinding>())
				{
					WeaponBinding weaponBinding = typeAttributes.WeaponLoadout[0];
					if (weaponBinding != null)
					{
						damagePacketsPerShot = weaponBinding.Weapon.DamagePacketsPerShot;
						baseDamagePerRound = Fixed64.IntValue(weaponBinding.Weapon.BaseDamagePerRound);
						WeaponAttributes entityTypeAttributes4 = ShipbreakersMain.GetEntityTypeAttributes<WeaponAttributes>(typeID, weaponBinding.Weapon.Name);
						if (entityTypeAttributes4 != null)
						{
							buffComparison3 = baseDamagePerRound.CompareTo(Fixed64.IntValue(entityTypeAttributes4.BaseDamagePerRound));
						}
						else
						{
							Log.Error(Log.Channel.Data | Log.Channel.UI, "Failed to find base WeaponAttributes with name {0} for entity type {1}", new object[]
							{
								weaponBinding.Weapon.Name,
								typeID
							});
						}
					}
					else
					{
						Log.Error(Log.Channel.Data | Log.Channel.UI, "First WeaponBinding in WeaponLoadout for unit type {0} is null! Unable to determine damage to show on info panel", new object[]
						{
							typeID
						});
					}
				}
				if (flag || this.mLastWeaponDamageValue != baseDamagePerRound || this.mLastWeaponPacketsValue != damagePacketsPerShot)
				{
					this.mSettings.UnitDamageValueLabel.text = damagePacketsPerShot != 1 ? string.Format("{0} | {1}", baseDamagePerRound, damagePacketsPerShot) : string.Format("{0}", baseDamagePerRound);
					this.mLastWeaponDamageValue = baseDamagePerRound;
					this.mLastWeaponPacketsValue = damagePacketsPerShot;
					this.mSettings.UnitDamageValueLabel.color = this.mUnitInteractionAttributes.BuffInfo.BuffColor(buffComparison3);
				}
				if (flag || this.mLastUnitArmourValue != typeAttributes.Armour)
				{
					this.mTempLocalizationFormatObjects[0] = typeAttributes.Armour;
					this.mTempLocalizationFormatObjects[1] = null;
					this.mSettings.UnitArmorValueLabel.text = this.mSettings.UnitArmorFormat.TokenFormat(this.mTempLocalizationFormatObjects, this.mLocMan);
					this.mLastUnitArmourValue = typeAttributes.Armour;
					int buffComparison4 = 0;
					if (entityTypeAttributes2 != null)
					{
						buffComparison4 = typeAttributes.Armour.CompareTo(entityTypeAttributes2.Armour);
					}
					this.mSettings.UnitArmorValueLabel.color = this.mUnitInteractionAttributes.BuffInfo.BuffColor(buffComparison4);
				}
				if (flag || this.mLastFireRateDisplay != typeAttributes.FireRateDisplay)
				{
					this.mTempLocalizationFormatObjects[0] = InformationPanelController.InformationPanelSettings.FireRateLocIDs[typeAttributes.FireRateDisplay];
					this.mTempLocalizationFormatObjects[1] = null;
					this.mSettings.UnitFireRateLabel.text = this.mSettings.UnitFireRateFormat.TokenFormat(this.mTempLocalizationFormatObjects, this.mLocMan);
					this.mLastFireRateDisplay = typeAttributes.FireRateDisplay;
					int buffComparison5 = 0;
					if (entityTypeAttributes2 != null)
					{
						buffComparison5 = typeAttributes.FireRateDisplay.CompareTo(entityTypeAttributes2.FireRateDisplay);
					}
					this.mSettings.UnitFireRateLabel.color = this.mUnitInteractionAttributes.BuffInfo.BuffColor(buffComparison5);
				}
				this.mLastUnitState = unitState;
			}
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0000A4FA File Offset: 0x000086FA
		private void OnNewStateFrame(SimStateFrame newFrame)
		{
			this.UpdateDynamicDataForUnit(this.mSelectedEntity);
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x0005773C File Offset: 0x0005593C
		public void Dispose()
		{
			ShipbreakersMain.PresentationEventSystem.RemoveHandler<UIHighlightPanelEvent>(new BBI.Core.Events.EventHandler<UIHighlightPanelEvent>(this.OnUIHighlightPanelEvent));
			if (this.mSettings.InformationPanelContainer != null)
			{
				NGUITools.SetActiveSelf(this.mSettings.InformationPanelContainer.gameObject, false);
			}
			this.mSettings = null;
			this.mUnitInteractionAttributes = null;
			this.mInterfaceController.NewSelection -= this.OnUnitSelectionChanged;
			this.mInterfaceController.NewFrameFromSim -= this.OnNewStateFrame;
			this.mInterfaceController.NewLeadUnitSelected -= this.OnNewLeadUnitSelected;
			this.mInterfaceController = null;
			this.mLocMan = null;
			this.mCommanderManager = null;
		}

		// Token: 0x04000AA8 RID: 2728
		private const int kMaxFormattingObjects = 2;

		// Token: 0x04000AA9 RID: 2729
		private InformationPanelController.InformationPanelSettings mSettings;

		// Token: 0x04000AAA RID: 2730
		private UnitHUDInteractionAttributes mUnitInteractionAttributes;

		// Token: 0x04000AAB RID: 2731
		private UnitInterfaceController mInterfaceController;

		// Token: 0x04000AAC RID: 2732
		private Entity mSelectedEntity = Entity.None;

		// Token: 0x04000AAD RID: 2733
		private IGameLocalization mLocMan;

		// Token: 0x04000AAE RID: 2734
		private ICommanderManager mCommanderManager;

		// Token: 0x04000AAF RID: 2735
		private ICommanderInteractionProvider mInteractionProvider;

		// Token: 0x04000AB0 RID: 2736
		private object[] mTempLocalizationFormatObjects = new object[2];

		// Token: 0x04000AB1 RID: 2737
		private UnitState mLastUnitState;

		// Token: 0x04000AB2 RID: 2738
		private ExperienceState mLastExperienceState;

		// Token: 0x04000AB3 RID: 2739
		private int mLastWeaponDamageValue = -1;
		private int mLastWeaponPacketsValue = -1;

		// Token: 0x04000AB4 RID: 2740
		private int mLastUnitArmourValue = -1;

		// Token: 0x04000AB5 RID: 2741
		private int mLastFireRateDisplay = -1;

		// Token: 0x0200014D RID: 333
		[Serializable]
		public class InformationPanelSettings
		{
			// Token: 0x17000267 RID: 615
			// (get) Token: 0x06000C58 RID: 3160 RVA: 0x0000A508 File Offset: 0x00008708
			public GameObject InformationPanelContainer
			{
				get
				{
					return this.mInformationPanelContainer;
				}
			}

			// Token: 0x17000268 RID: 616
			// (get) Token: 0x06000C59 RID: 3161 RVA: 0x0000A510 File Offset: 0x00008710
			public GameObject TogglePanelUIGameObjectReference
			{
				get
				{
					return this.mTogglePanelUIGameObjectReference;
				}
			}

			// Token: 0x17000269 RID: 617
			// (get) Token: 0x06000C5A RID: 3162 RVA: 0x0000A518 File Offset: 0x00008718
			public GameObject InformationPanelView
			{
				get
				{
					return this.mInformationPanelView;
				}
			}

			// Token: 0x1700026A RID: 618
			// (get) Token: 0x06000C5B RID: 3163 RVA: 0x0000A520 File Offset: 0x00008720
			public string UnitNameLocID
			{
				get
				{
					return this.mUnitNameLocID;
				}
			}

			// Token: 0x1700026B RID: 619
			// (get) Token: 0x06000C5C RID: 3164 RVA: 0x0000A528 File Offset: 0x00008728
			public UILabel UnitNameLabel
			{
				get
				{
					return this.mUnitNameLabel;
				}
			}

			// Token: 0x1700026C RID: 620
			// (get) Token: 0x06000C5D RID: 3165 RVA: 0x0000A530 File Offset: 0x00008730
			public UISprite UnitIcon
			{
				get
				{
					return this.m_UnitIcon;
				}
			}

			// Token: 0x1700026D RID: 621
			// (get) Token: 0x06000C5E RID: 3166 RVA: 0x0000A538 File Offset: 0x00008738
			public string PlayerNameLocID
			{
				get
				{
					return this.mPlayerNameLocID;
				}
			}

			// Token: 0x1700026E RID: 622
			// (get) Token: 0x06000C5F RID: 3167 RVA: 0x0000A540 File Offset: 0x00008740
			public UILabel PlayerNameLabel
			{
				get
				{
					return this.mPlayerNameLabel;
				}
			}

			// Token: 0x1700026F RID: 623
			// (get) Token: 0x06000C60 RID: 3168 RVA: 0x0000A548 File Offset: 0x00008748
			public string UnitLevelFormat
			{
				get
				{
					return this.mUnitLevelFormat;
				}
			}

			// Token: 0x17000270 RID: 624
			// (get) Token: 0x06000C61 RID: 3169 RVA: 0x0000A550 File Offset: 0x00008750
			public string UnitHealthFormat
			{
				get
				{
					return this.mUnitHealthFormat;
				}
			}

			// Token: 0x17000271 RID: 625
			// (get) Token: 0x06000C62 RID: 3170 RVA: 0x0000A558 File Offset: 0x00008758
			public UILabel UnitHealthValueLabel
			{
				get
				{
					return this.mUnitHealthValueLabel;
				}
			}

			// Token: 0x17000272 RID: 626
			// (get) Token: 0x06000C63 RID: 3171 RVA: 0x0000A560 File Offset: 0x00008760
			public UIProgressBar UnitHealthBar
			{
				get
				{
					return this.m_UnitHealthBar;
				}
			}

			// Token: 0x17000273 RID: 627
			// (get) Token: 0x06000C64 RID: 3172 RVA: 0x0000A568 File Offset: 0x00008768
			public string UnitSpeedFormat
			{
				get
				{
					return this.mUnitSpeedFormat;
				}
			}

			// Token: 0x17000274 RID: 628
			// (get) Token: 0x06000C65 RID: 3173 RVA: 0x0000A570 File Offset: 0x00008770
			public UILabel SpeedValueLabel
			{
				get
				{
					return this.mSpeedValueLabel;
				}
			}

			// Token: 0x17000275 RID: 629
			// (get) Token: 0x06000C66 RID: 3174 RVA: 0x0000A578 File Offset: 0x00008778
			public string UnitDamageFormat
			{
				get
				{
					return this.mUnitDamageFormat;
				}
			}

			// Token: 0x17000276 RID: 630
			// (get) Token: 0x06000C67 RID: 3175 RVA: 0x0000A580 File Offset: 0x00008780
			public UILabel UnitDamageValueLabel
			{
				get
				{
					return this.mUnitDamageValueLabel;
				}
			}

			// Token: 0x17000277 RID: 631
			// (get) Token: 0x06000C68 RID: 3176 RVA: 0x0000A588 File Offset: 0x00008788
			public string UnitArmorFormat
			{
				get
				{
					return this.mUnitArmorFormat;
				}
			}

			// Token: 0x17000278 RID: 632
			// (get) Token: 0x06000C69 RID: 3177 RVA: 0x0000A590 File Offset: 0x00008790
			public UILabel UnitArmorValueLabel
			{
				get
				{
					return this.mUnitArmorValueLabel;
				}
			}

			// Token: 0x17000279 RID: 633
			// (get) Token: 0x06000C6A RID: 3178 RVA: 0x0000A598 File Offset: 0x00008798
			public string UnitFireRateFormat
			{
				get
				{
					return this.mUnitFireRateFormat;
				}
			}

			// Token: 0x1700027A RID: 634
			// (get) Token: 0x06000C6B RID: 3179 RVA: 0x0000A5A0 File Offset: 0x000087A0
			public UILabel UnitFireRateLabel
			{
				get
				{
					return this.mUnitFireRateLabel;
				}
			}

			// Token: 0x1700027B RID: 635
			// (get) Token: 0x06000C6C RID: 3180 RVA: 0x0000A5A8 File Offset: 0x000087A8
			public string UnitKillsFormat
			{
				get
				{
					return this.mUnitKillsFormat;
				}
			}

			// Token: 0x1700027C RID: 636
			// (get) Token: 0x06000C6D RID: 3181 RVA: 0x0000A5B0 File Offset: 0x000087B0
			public UILabel UnitKillsValueLabel
			{
				get
				{
					return this.mUnitKillsValueLabel;
				}
			}

			// Token: 0x1700027D RID: 637
			// (get) Token: 0x06000C6E RID: 3182 RVA: 0x0000A5B8 File Offset: 0x000087B8
			public UILabel UnitShortDescriptionValueLabel
			{
				get
				{
					return this.mUnitShortDescriptionValueLabel;
				}
			}

			// Token: 0x1700027E RID: 638
			// (get) Token: 0x06000C6F RID: 3183 RVA: 0x0000A5C0 File Offset: 0x000087C0
			public UILabel UnitLongDescriptionValueLabel
			{
				get
				{
					return this.mUnitLongDescriptionValueLabel;
				}
			}

			// Token: 0x1700027F RID: 639
			// (get) Token: 0x06000C70 RID: 3184 RVA: 0x0000A5C8 File Offset: 0x000087C8
			public GameObject HighlightPanelGameObject
			{
				get
				{
					return this.m_HighlightPanelGameObject;
				}
			}

			// Token: 0x04000AB6 RID: 2742
			[SerializeField]
			private GameObject mInformationPanelContainer;

			// Token: 0x04000AB7 RID: 2743
			[SerializeField]
			private GameObject mTogglePanelUIGameObjectReference;

			// Token: 0x04000AB8 RID: 2744
			[SerializeField]
			private GameObject mInformationPanelView;

			// Token: 0x04000AB9 RID: 2745
			[SerializeField]
			private string mUnitNameLocID = "ID_INFOPANEL_UNITNAME";

			// Token: 0x04000ABA RID: 2746
			[SerializeField]
			private UILabel mUnitNameLabel;

			// Token: 0x04000ABB RID: 2747
			[SerializeField]
			private UISprite m_UnitIcon;

			// Token: 0x04000ABC RID: 2748
			[SerializeField]
			private string mPlayerNameLocID = "ID_INFOPANEL_PLAYERNAME";

			// Token: 0x04000ABD RID: 2749
			[SerializeField]
			private UILabel mPlayerNameLabel;

			// Token: 0x04000ABE RID: 2750
			[SerializeField]
			private string mUnitLevelFormat = "ID_INFOPANEL_LEVEL_FORMAT";

			// Token: 0x04000ABF RID: 2751
			[SerializeField]
			private string mUnitHealthFormat = "ID_INFOPANEL_HEALTH_VALUE_FORMAT";

			// Token: 0x04000AC0 RID: 2752
			[SerializeField]
			private UILabel mUnitHealthValueLabel;

			// Token: 0x04000AC1 RID: 2753
			[SerializeField]
			private UIProgressBar m_UnitHealthBar;

			// Token: 0x04000AC2 RID: 2754
			[SerializeField]
			private string mUnitSpeedFormat = "ID_INFOPANEL_SPEED_VALUE_FORMAT";

			// Token: 0x04000AC3 RID: 2755
			[SerializeField]
			private UILabel mSpeedValueLabel;

			// Token: 0x04000AC4 RID: 2756
			[SerializeField]
			private string mUnitDamageFormat = "ID_INFOPANEL_DAMAGE_VALUE_FORMAT";

			// Token: 0x04000AC5 RID: 2757
			[SerializeField]
			private UILabel mUnitDamageValueLabel;

			// Token: 0x04000AC6 RID: 2758
			[SerializeField]
			private string mUnitArmorFormat = "ID_INFOPANEL_ARMOR_VALUE_FORMAT";

			// Token: 0x04000AC7 RID: 2759
			[SerializeField]
			private UILabel mUnitArmorValueLabel;

			// Token: 0x04000AC8 RID: 2760
			[SerializeField]
			private string mUnitFireRateFormat = "ID_UI_IG_CTRLSTP_INFOPANEL_FIRERATE_VALUE_FORMAT_3923";

			// Token: 0x04000AC9 RID: 2761
			[SerializeField]
			private UILabel mUnitFireRateLabel;

			// Token: 0x04000ACA RID: 2762
			[SerializeField]
			private string mUnitKillsFormat = "ID_INFOPANEL_KILLS_VALUE_FORMAT";

			// Token: 0x04000ACB RID: 2763
			[SerializeField]
			private UILabel mUnitKillsValueLabel;

			// Token: 0x04000ACC RID: 2764
			[SerializeField]
			private UILabel mUnitShortDescriptionValueLabel;

			// Token: 0x04000ACD RID: 2765
			[SerializeField]
			private UILabel mUnitLongDescriptionValueLabel;

			// Token: 0x04000ACE RID: 2766
			[SerializeField]
			private GameObject m_HighlightPanelGameObject;

			// Token: 0x04000ACF RID: 2767
			public static Dictionary<int, string> FireRateLocIDs = new Dictionary<int, string>
			{
				{
					0,
					"ID_UI_IG_CTRLSTP_INFOPANEL_FIRERATE_NA_3916"
				},
				{
					1,
					"ID_UI_IG_CTRLSTP_INFOPANEL_FIRERATE_VLOW_3917"
				},
				{
					2,
					"ID_UI_IG_CTRLSTP_INFOPANEL_FIRERATE_LOW_3918"
				},
				{
					3,
					"ID_UI_IG_CTRLSTP_INFOPANEL_FIRERATE_MED_3919"
				},
				{
					4,
					"ID_UI_IG_CTRLSTP_INFOPANEL_FIRERATE_HIGH_3920"
				},
				{
					5,
					"ID_UI_IG_CTRLSTP_INFOPANEL_FIRERATE_VHIGH_3921"
				}
			};
		}
	}
}
