using System;
using System.Collections.Generic;
using BBI.Core.Network;
using BBI.Core.Utility;
using BBI.Game.Simulation;
using BBI.Unity.Game.Localize;
using BBI.Unity.Game.World;
using UnityEngine;

namespace BBI.Unity.Game.UI
{
	public class LoadPanelPlayerList : MonoBehaviour
	{
		private void Awake()
		{
			if (this.m_TeamLabel == null)
			{
				Log.Error(Log.Channel.UI, "NULL Team Label set in {0}", new object[]
				{
					base.GetType()
				});
			}
			if (this.m_PlayerList == null)
			{
				Log.Error(Log.Channel.UI, "NULL Player List Grid set in {0}", new object[]
				{
					base.GetType()
				});
			}
			if (string.IsNullOrEmpty(this.m_LocIdTeam))
			{
				Log.Error(Log.Channel.UI, "Missing LocId: Team in {0}", new object[]
				{
					base.GetType()
				});
			}
			if (this.m_EntryPrefab == null)
			{
				Log.Error(Log.Channel.UI, "NULL ListEntryPrefab set in {0}", new object[]
				{
					base.GetType()
				});
			}
		}

		public void SetupForFFA(IGameLocalization locMan, List<PlayerSelection> players, CommanderID localCommanderID)
		{
			if (this.m_TeamLabel != null)
			{
				this.m_TeamLabel.text = string.Empty;
			}
			this.SetupPlayers(locMan, players, false, localCommanderID);
		}

		public void SetupForTeams(IGameLocalization locMan, TeamID team, List<PlayerSelection> players, bool isFriendlyTeam, CommanderID localCommanderID)
		{
			if (locMan != null && this.m_TeamLabel != null)
			{
				string text = locMan.Format(this.m_LocIdTeam, new object[]
				{
					team.ID
				});
				this.m_TeamLabel.text = text;
			}
			this.SetupPlayers(locMan, players, isFriendlyTeam, localCommanderID);
		}

		public void SetPlayerLoaded(NetworkPlayerID playerID)
		{
			foreach (LoadPanelPlayerListEntry loadPanelPlayerListEntry in this.mEntries)
			{
				if (loadPanelPlayerListEntry.Player.Desc.NetworkPlayerID == playerID)
				{
					loadPanelPlayerListEntry.SetPlayerLoaded();
					break;
				}
			}
		}

		private void SetupPlayers(IGameLocalization locMan, List<PlayerSelection> players, bool isFriendlyTeam, CommanderID localCommanderID)
		{
			if (this.m_PlayerList != null && this.m_EntryPrefab != null)
			{
				this.mEntries.Clear();
				BlackbirdPanelBase.ClearGrid(this.m_PlayerList);
				foreach (PlayerSelection player in players)
				{
					CommanderRelationship relationship = CommanderRelationship.Enemy;
					if (player.Desc.CommanderID == localCommanderID)
					{
						relationship = CommanderRelationship.Self;
					}
					else if (isFriendlyTeam)
					{
						relationship = CommanderRelationship.Ally;
					}
					GameObject gameObject = NGUITools.AddChild(this.m_PlayerList.gameObject, this.m_EntryPrefab.gameObject);
					if (gameObject != null)
					{
						LoadPanelPlayerListEntry component = gameObject.GetComponent<LoadPanelPlayerListEntry>();
						if (component != null)
						{
							component.Setup(locMan, player, relationship, this.m_RevealRandomFactions);
							this.mEntries.Add(component);
						}
						else
						{
							Log.Error(Log.Channel.UI, "No LoadPanelPlayerListEntry component found in ListEntryPrefab for {0}!", new object[]
							{
								base.GetType()
							});
						}
					}
					else
					{
						Log.Error(Log.Channel.UI, "Failed to spawn ListEntryPrefab in {0}", new object[]
						{
							base.GetType()
						});
					}
				}
				this.m_PlayerList.Reposition();
				this.m_PlayerList.repositionNow = true;
			}
		}

		public LoadPanelPlayerList()
		{
		}

		[SerializeField]
		private UILabel m_TeamLabel;

		[SerializeField]
		private UIGrid m_PlayerList;

		[SerializeField]
		private string m_LocIdTeam;

		[SerializeField]
		private LoadPanelPlayerListEntry m_EntryPrefab;

		[SerializeField]
		private bool m_RevealRandomFactions;

		private List<LoadPanelPlayerListEntry> mEntries = new List<LoadPanelPlayerListEntry>();
	}
}
