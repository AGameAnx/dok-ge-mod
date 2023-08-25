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
	public class InformationPanelController : IDisposable
	{
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
				UnitState unitState = ShipbreakersMain.CurrentSimFrame.FindObject<UnitState>(entity);
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
					this.mTempLocalizationFormatObjects[0] = ((commanderFromID != null) ? SharedLocIDConstants.GetLocalizedCommanderName(commanderDirectorFromID.PlayerType, commanderFromID.Name, commanderDirectorFromID.PlayerID, commanderDirectorFromID.AIDifficulty) : string.Empty);
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

		private void OnNewStateFrame(SimStateFrame newFrame)
		{
			this.UpdateDynamicDataForUnit(this.mSelectedEntity);
		}

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

		private const int kMaxFormattingObjects = 2;

		private InformationPanelController.InformationPanelSettings mSettings;

		private UnitHUDInteractionAttributes mUnitInteractionAttributes;

		private UnitInterfaceController mInterfaceController;

		private Entity mSelectedEntity = Entity.None;

		private IGameLocalization mLocMan;

		private ICommanderManager mCommanderManager;

		private ICommanderInteractionProvider mInteractionProvider;

		private object[] mTempLocalizationFormatObjects = new object[2];

		private UnitState mLastUnitState;

		private ExperienceState mLastExperienceState;

		private int mLastWeaponDamageValue = -1;
		private int mLastWeaponPacketsValue = -1;

		private int mLastUnitArmourValue = -1;

		private int mLastFireRateDisplay = -1;

		[Serializable]
		public class InformationPanelSettings
		{
			public GameObject InformationPanelContainer
			{
				get
				{
					return this.mInformationPanelContainer;
				}
			}

			public GameObject TogglePanelUIGameObjectReference
			{
				get
				{
					return this.mTogglePanelUIGameObjectReference;
				}
			}

			public GameObject InformationPanelView
			{
				get
				{
					return this.mInformationPanelView;
				}
			}

			public string UnitNameLocID
			{
				get
				{
					return this.mUnitNameLocID;
				}
			}

			public UILabel UnitNameLabel
			{
				get
				{
					return this.mUnitNameLabel;
				}
			}

			public UISprite UnitIcon
			{
				get
				{
					return this.m_UnitIcon;
				}
			}

			public string PlayerNameLocID
			{
				get
				{
					return this.mPlayerNameLocID;
				}
			}

			public UILabel PlayerNameLabel
			{
				get
				{
					return this.mPlayerNameLabel;
				}
			}

			public string UnitLevelFormat
			{
				get
				{
					return this.mUnitLevelFormat;
				}
			}

			public string UnitHealthFormat
			{
				get
				{
					return this.mUnitHealthFormat;
				}
			}

			public UILabel UnitHealthValueLabel
			{
				get
				{
					return this.mUnitHealthValueLabel;
				}
			}

			public UIProgressBar UnitHealthBar
			{
				get
				{
					return this.m_UnitHealthBar;
				}
			}

			public string UnitSpeedFormat
			{
				get
				{
					return this.mUnitSpeedFormat;
				}
			}

			public UILabel SpeedValueLabel
			{
				get
				{
					return this.mSpeedValueLabel;
				}
			}

			public string UnitDamageFormat
			{
				get
				{
					return this.mUnitDamageFormat;
				}
			}

			public UILabel UnitDamageValueLabel
			{
				get
				{
					return this.mUnitDamageValueLabel;
				}
			}

			public string UnitArmorFormat
			{
				get
				{
					return this.mUnitArmorFormat;
				}
			}

			public UILabel UnitArmorValueLabel
			{
				get
				{
					return this.mUnitArmorValueLabel;
				}
			}

			public string UnitFireRateFormat
			{
				get
				{
					return this.mUnitFireRateFormat;
				}
			}

			public UILabel UnitFireRateLabel
			{
				get
				{
					return this.mUnitFireRateLabel;
				}
			}

			public string UnitKillsFormat
			{
				get
				{
					return this.mUnitKillsFormat;
				}
			}

			public UILabel UnitKillsValueLabel
			{
				get
				{
					return this.mUnitKillsValueLabel;
				}
			}

			public UILabel UnitShortDescriptionValueLabel
			{
				get
				{
					return this.mUnitShortDescriptionValueLabel;
				}
			}

			public UILabel UnitLongDescriptionValueLabel
			{
				get
				{
					return this.mUnitLongDescriptionValueLabel;
				}
			}

			public GameObject HighlightPanelGameObject
			{
				get
				{
					return this.m_HighlightPanelGameObject;
				}
			}

			public InformationPanelSettings()
			{
			}

			static InformationPanelSettings()
			{
			}

			[SerializeField]
			private GameObject mInformationPanelContainer;

			[SerializeField]
			private GameObject mTogglePanelUIGameObjectReference;

			[SerializeField]
			private GameObject mInformationPanelView;

			[SerializeField]
			private string mUnitNameLocID = "ID_INFOPANEL_UNITNAME";

			[SerializeField]
			private UILabel mUnitNameLabel;

			[SerializeField]
			private UISprite m_UnitIcon;

			[SerializeField]
			private string mPlayerNameLocID = "ID_INFOPANEL_PLAYERNAME";

			[SerializeField]
			private UILabel mPlayerNameLabel;

			[SerializeField]
			private string mUnitLevelFormat = "ID_INFOPANEL_LEVEL_FORMAT";

			[SerializeField]
			private string mUnitHealthFormat = "ID_INFOPANEL_HEALTH_VALUE_FORMAT";

			[SerializeField]
			private UILabel mUnitHealthValueLabel;

			[SerializeField]
			private UIProgressBar m_UnitHealthBar;

			[SerializeField]
			private string mUnitSpeedFormat = "ID_INFOPANEL_SPEED_VALUE_FORMAT";

			[SerializeField]
			private UILabel mSpeedValueLabel;

			[SerializeField]
			private string mUnitDamageFormat = "ID_INFOPANEL_DAMAGE_VALUE_FORMAT";

			[SerializeField]
			private UILabel mUnitDamageValueLabel;

			[SerializeField]
			private string mUnitArmorFormat = "ID_INFOPANEL_ARMOR_VALUE_FORMAT";

			[SerializeField]
			private UILabel mUnitArmorValueLabel;

			[SerializeField]
			private string mUnitFireRateFormat = "ID_UI_IG_CTRLSTP_INFOPANEL_FIRERATE_VALUE_FORMAT_3923";

			[SerializeField]
			private UILabel mUnitFireRateLabel;

			[SerializeField]
			private string mUnitKillsFormat = "ID_INFOPANEL_KILLS_VALUE_FORMAT";

			[SerializeField]
			private UILabel mUnitKillsValueLabel;

			[SerializeField]
			private UILabel mUnitShortDescriptionValueLabel;

			[SerializeField]
			private UILabel mUnitLongDescriptionValueLabel;

			[SerializeField]
			private GameObject m_HighlightPanelGameObject;

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
