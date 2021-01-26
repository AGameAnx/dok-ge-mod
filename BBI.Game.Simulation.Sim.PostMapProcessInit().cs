using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BBI.Core;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Events;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.AI;
using BBI.Game.Commands;
using BBI.Game.Data;
using BBI.Game.Replay;
using BBI.Game.SaveLoad;

namespace BBI.Game.Simulation
{
	// Token: 0x020003DB RID: 987
	public partial class Sim
	{
		// Token: 0x0600141C RID: 5148
		internal void PostMapProcessInit()
		{
			if (this.mSimInitializationState != Sim.InitializationState.MapLoaded)
			{
				return;
			}
			SimMapDependencies mapDependencies;
			this.mSimStartupDependencies.Get<SimMapDependencies>(out mapDependencies);
			this.AIManager = new AIManager(this.SimEventSystem, this.CommanderManager.CPUCommanders);
			if (this.Settings.GameMode != null)
			{
				this.Settings.GameMode.Initialize();
			}
			SessionChangeReason sessionChangeReason;
			if (!this.mSimStartupDependencies.Get<SessionChangeReason>(out sessionChangeReason))
			{
				sessionChangeReason = SessionChangeReason.NewGame;
			}
			MissionDependencies missionDependencies;
			if (this.mSimStartupDependencies.Get<MissionDependencies>(out missionDependencies) && !missionDependencies.EntityDescriptors.IsNullOrEmpty<SceneEntityDescriptor>())
			{
				SceneEntityCreator.CreateSceneEntitiesForGameSession(missionDependencies.EntityGroupDescriptors, missionDependencies.EntityDescriptors, missionDependencies.RandomWreckArtifacts, missionDependencies.MaxSpawnedWreckArtifacts, sessionChangeReason == SessionChangeReason.Transition);
			}
			AllEntityProcessor.Initialize();
			if (sessionChangeReason != SessionChangeReason.LoadGame)
			{
				foreach (KeyValuePair<CommanderID, CommanderAttributes> keyValuePair in this.Settings.CommanderAttributesMap)
				{
					Commander commanderFromID = Sim.Instance.CommanderManager.GetCommanderFromID(keyValuePair.Key);
					if (commanderFromID != null)
					{
						this.InitializeResearchForCommander(commanderFromID);
						this.ApplyInitialBuffsForCommander(commanderFromID, true);
						this.GrantStartingGrantedAbilitiesForCommander(commanderFromID, true);
						this.SpawnInitialUnitsForCommander(commanderFromID, this.Settings, mapDependencies);
						commanderFromID.GrantStartingWreckArtifacts();
					}
				}
			}
			// MOD
			MapModManager.LoadMapLayout();
			// MOD
			this.mSimInitializationState = Sim.InitializationState.Initialized;
		}
	}
}
