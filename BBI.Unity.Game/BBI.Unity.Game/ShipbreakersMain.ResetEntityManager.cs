using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using BBI.Core.Collections;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Events;
using BBI.Core.Localization;
using BBI.Core.Network;
using BBI.Core.Threading;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Commands;
using BBI.Game.Data;
using BBI.Game.Events;
using BBI.Game.Network;
using BBI.Game.Replay;
using BBI.Game.SaveLoad;
using BBI.Game.Simulation;
using BBI.Steam;
using BBI.Unity.Core.Data;
using BBI.Unity.Core.DLC;
using BBI.Unity.Core.Input;
using BBI.Unity.Core.Rendering;
using BBI.Unity.Core.Utility;
using BBI.Unity.Core.World;
using BBI.Unity.Game.Audio;
using BBI.Unity.Game.Data;
using BBI.Unity.Game.DLC;
using BBI.Unity.Game.Events;
using BBI.Unity.Game.HUD;
using BBI.Unity.Game.Localize;
using BBI.Unity.Game.Network;
using BBI.Unity.Game.NewsFeed;
using BBI.Unity.Game.Rendering;
using BBI.Unity.Game.Scripting;
using BBI.Unity.Game.ToReplace;
using BBI.Unity.Game.UI;
using BBI.Unity.Game.World;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Connect;
using PlayEveryWare.EpicOnlineServices;
using UnityEngine;
using Subsystem;

namespace BBI.Unity.Game
{
	public sealed partial class ShipbreakersMain : MonoBehaviour, ICoroutineHost
	{
		private void ResetEntityManager()
		{
			ShipbreakersMain.sEntityTypes.Clear();
			ShipbreakersMain.sEntityTypes = null;
			ShipbreakersMain.EntitySystem.Teardown();
			HashSet<ScriptableObject> hashSet = new HashSet<ScriptableObject>();
			foreach (DLCAssetBundleBase dlcassetBundleBase in ShipbreakersMain.sDLCManager.GetAllLoadedHeadersOfType(DLCType.Faction))
			{
				UnitFactionDLCPack unitFactionDLCPack = dlcassetBundleBase as UnitFactionDLCPack;
				if (unitFactionDLCPack != null)
				{
					foreach (UnitFactionDLCPack.ReplacementMapping replacementMapping in unitFactionDLCPack.NewEntities)
					{
						if (replacementMapping.NewEntityType is IEntityTypeRegistrationRequired)
						{
							hashSet.Add(replacementMapping.NewEntityType);
						}
					}
					CommanderAttributesData commanderAttributesData = unitFactionDLCPack.CommanderAttrs as CommanderAttributesData;
					if (commanderAttributesData != null)
					{
						FactionAttributesData factionAttributesData = commanderAttributesData.FactionAttributes.Data as FactionAttributesData;
						if (factionAttributesData != null)
						{
							ScriptableObject techTree = factionAttributesData.TechTree;
							if (techTree != null)
							{
								hashSet.Add(techTree);
							}
						}
					}
				}
			}
			ShipbreakersMain.InitializeEntityManager(this.m_RegistryAsset, hashSet);

			if (MapModManager.GameType != GameMode.SinglePlayer)
			{
				new AttributeLoader().LoadAttributes(ShipbreakersMain.sEntityTypes, "Managed/fathership.json"); // Default game-wide fathership modifications
			}
			new AttributeLoader().LoadAttributes(ShipbreakersMain.sEntityTypes);
		}
	}
}
