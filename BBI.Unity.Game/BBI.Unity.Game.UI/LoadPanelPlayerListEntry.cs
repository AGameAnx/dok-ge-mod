using System;
using BBI.Core.Utility;
using BBI.Game.Data;
using BBI.Game.Simulation;
using BBI.Unity.Game.Localize;
using BBI.Unity.Game.World;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	public class LoadPanelPlayerListEntry : MonoBehaviour
	{
		public PlayerSelection Player { get; private set; }

		private void Awake()
		{
			if (this.m_PlayerName == null)
			{
				Log.Error(Log.Channel.UI, "NULL PlayerName label set in {0}", new object[]
				{
					base.GetType()
				});
			}
			if (this.m_FactionName == null)
			{
				Log.Error(Log.Channel.UI, "NULL FactionName label set in {0}", new object[]
				{
					base.GetType()
				});
			}
			if (this.m_PlayerBanner == null)
			{
				Log.Error(Log.Channel.UI, "NULL PlayerBanner set in {0}", new object[]
				{
					base.GetType()
				});
			}
			if (this.m_IconLoading == null)
			{
				Log.Error(Log.Channel.UI, "NULL IconLoading sprite set in {0}", new object[]
				{
					base.GetType()
				});
			}
			if (this.m_IconLoaded == null)
			{
				Log.Error(Log.Channel.UI, "NULL IconLoaded sprite set in {0}", new object[]
				{
					base.GetType()
				});
			}
		}

		public void Setup(IGameLocalization locMan, PlayerSelection player, CommanderRelationship relationship, bool revealRandomFactions)
		{
			this.Player = player;
			Color color = Color.white;
			switch (relationship)
			{
			case CommanderRelationship.Self:
				color = this.m_TextColorSelf;
				break;
			case CommanderRelationship.Ally:
				color = this.m_TextColorAlly;
				break;
			case CommanderRelationship.Enemy:
				color = this.m_TextColorEnemy;
				break;
			}
			if (this.m_PlayerName != null)
			{
				this.m_PlayerName.text = player.Desc.LocalizedName;
				this.m_PlayerName.color = color;
			}
			if (this.m_FactionName != null)
			{
				if (player.Desc.RandomFaction)
				{
					if (revealRandomFactions)
					{
						this.m_FactionName.text = string.Format("{0} ({1})", Localization.Get(player.Attributes.Faction.FactionName), Localization.Get("ID_UI_FE_MP_RANDOM_253"));
					}
					else
					{
						this.m_FactionName.text = Localization.Get("ID_UI_FE_MP_RANDOM_253");
					}
				}
				else
				{
					this.m_FactionName.text = Localization.Get(player.Attributes.Faction.FactionName);
				}
				this.m_FactionName.width = 200;
			}
			if (this.m_PlayerBanner != null)
			{
				this.m_PlayerBanner.UpdateFromCommanderData(player.Desc);
			}
			this.SetLoadedState(player.Desc.PlayerType == PlayerType.AI);
		}

		public void SetPlayerLoaded()
		{
			this.SetLoadedState(true);
		}

		private void SetLoadedState(bool loaded)
		{
			if (this.m_IconLoading != null && this.m_IconLoaded != null)
			{
				NGUITools.SetActiveSelf(this.m_IconLoading.gameObject, !loaded);
				NGUITools.SetActiveSelf(this.m_IconLoaded.gameObject, loaded);
			}
		}

		public LoadPanelPlayerListEntry()
		{
		}

		[SerializeField]
		private Color m_TextColorSelf = Color.green;

		[SerializeField]
		private Color m_TextColorAlly = Color.yellow;

		[SerializeField]
		private Color m_TextColorEnemy = Color.red;

		[SerializeField]
		private UILabel m_PlayerName;

		[SerializeField]
		private UILabel m_FactionName;

		[SerializeField]
		private PlayerBanner m_PlayerBanner;

		[SerializeField]
		private UISprite m_IconLoading;

		[SerializeField]
		private UISprite m_IconLoaded;
	}
}
