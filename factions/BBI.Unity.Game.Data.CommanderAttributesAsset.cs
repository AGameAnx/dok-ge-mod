using System;
using BBI.Core;
using BBI.Game.Data;
using BBI.Unity.Core.Data;
using UnityEngine;

namespace BBI.Unity.Game.Data
{
	// Token: 0x0200004F RID: 79
	public sealed class CommanderAttributesAsset : AssetBase
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00002BC0 File Offset: 0x00000DC0
		public override object Data
		{
			get
			{
				return this.CommanderAttributes;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000131 RID: 305 RVA: 0x000069A8 File Offset: 0x00004BA8
		public CommanderAttributes[] CommanderAttributes
		{
			get
			{
				// MOD
				CommanderAttributes[] commanderAttributes = this.m_CommanderAttributes;
				CommanderAttributes[] arrayOriginal = commanderAttributes;
				CommanderAttributes[] arrayMod = new CommanderAttributes[arrayOriginal.Length + 1];
				arrayOriginal.CopyTo(arrayMod, 0);
				
				// // Spectator
				// CommanderAttributesData commanderSpectator = ((CommanderAttributesData)arrayOriginal[1]).Copy(); // Copy of Coalition
				// commanderSpectator.m_Name = "SPECTATOR";
				// commanderSpectator.m_NameLocID = "SPECTATOR";
				// commanderSpectator.m_StartingPopCap = 0;
				// commanderSpectator.m_StartingResource1 = 0;
				// commanderSpectator.m_FactionAttributes = ScriptableObject.CreateInstance<FactionAttributesAsset>();
				// commanderSpectator.m_FactionAttributes.m_FactionAttributes = ((CommanderAttributesData)arrayOriginal[1]).m_FactionAttributes.m_FactionAttributes.Copy();
				// commanderSpectator.m_FactionAttributes.m_FactionAttributes.m_FactionName = "SPECTATOR";
				// commanderSpectator.m_FactionAttributes.m_FactionAttributes.m_FactionID = FactionID.None;
				// arrayMod[arrayOriginal.Length + 0] = commanderSpectator; // Add to array
				
				// // SP Gaalsien
				// CommanderAttributesData commanderSpGaalsien = ((CommanderAttributesData)arrayOriginal[1]).Copy(); // Copy of Gaalsien
				// commanderSpGaalsien.m_StartingUnits = new StartingUnit[] {
					// new StartingUnit { Unit = "G_Carrier" },
					// new StartingUnit { Unit = "G_Harvester" },
					// new StartingUnit { Unit = "G_Harvester" },
					// new StartingUnit { Unit = "G_Harvester" }, 
					// new StartingUnit { Unit = "G_Harvester" },
					// new StartingUnit { Unit = "G_SupportCruiser" },
					// new StartingUnit { Unit = "G_Baserunner" },
				// };
				// commanderSpGaalsien.m_Name = "ASHOKA";
				// commanderSpGaalsien.m_NameLocID = "ASHOKA";
				// commanderSpGaalsien.m_StartingPopCap = 125; // Give full pop-cap to make up for no pop-cap research
				// commanderSpGaalsien.m_FactionAttributes = ScriptableObject.CreateInstance<FactionAttributesAsset>();
				// commanderSpGaalsien.m_FactionAttributes.m_FactionAttributes = ((CommanderAttributesData)arrayOriginal[1]).m_FactionAttributes.m_FactionAttributes.Copy();
				// commanderSpGaalsien.m_FactionAttributes.m_FactionAttributes.m_FactionName = "ASHOKA";
				// commanderSpGaalsien.m_FactionAttributes.m_FactionAttributes.m_FactionID = FactionID.None;
				// commanderSpGaalsien.m_FactionAttributes.m_FactionAttributes.m_TechTreeAttributes = new TechTreeAttributesAsset(); // Remove tech tree
				// arrayMod[arrayOriginal.Length + 1] = commanderSpGaalsien; // Add to array
				
				// Fathership
				CommanderAttributesData commanderFathership = ((CommanderAttributesData)arrayOriginal[1]).Copy(); // Copy of Gaalsien
				commanderFathership.m_StartingUnits = new StartingUnit[] {
					new StartingUnit { Unit = "G_Fathership" },
					new StartingUnit { Unit = "G_Harvester" },
					new StartingUnit { Unit = "G_Harvester" },
					new StartingUnit { Unit = "G_Harvester" }, 
					new StartingUnit { Unit = "G_Harvester" },
					new StartingUnit { Unit = "G_SupportCruiser" },
					new StartingUnit { Unit = "G_Baserunner" },
				};
				commanderFathership.m_Name = "FATHERSHIP";
				commanderFathership.m_NameLocID = "FATHERSHIP";
				commanderFathership.m_StartingPopCap = 125; // Give full pop-cap to make up for no pop-cap research
				commanderFathership.m_FactionAttributes = ScriptableObject.CreateInstance<FactionAttributesAsset>();
				commanderFathership.m_FactionAttributes.m_FactionAttributes = ((CommanderAttributesData)arrayOriginal[1]).m_FactionAttributes.m_FactionAttributes.Copy();
				commanderFathership.m_FactionAttributes.m_FactionAttributes.m_FactionName = "FATHERSHIP";
				commanderFathership.m_FactionAttributes.m_FactionAttributes.m_FactionID = FactionID.None;
				commanderFathership.m_FactionAttributes.m_FactionAttributes.m_TechTreeAttributes = new TechTreeAttributesAsset(); // Remove tech tree
				arrayMod[arrayOriginal.Length + 0] = commanderFathership; // Add to array

				return arrayMod;
				// MOD
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00002BC8 File Offset: 0x00000DC8
		protected override void OnEnable()
		{
			base.OnEnable();
			this.FillArchetypes();
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006BE0 File Offset: 0x00004DE0
		private void FillArchetypes()
		{
			foreach (CommanderAttributesData commanderSpectator in this.m_CommanderAttributes)
			{
				CommanderAttributesAsset.PopulateAIArchetypesForCommanderAttributes(base.name, commanderSpectator.AIArchetypeAssets, commanderSpectator);
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006C18 File Offset: 0x00004E18
		public static void PopulateAIArchetypesForCommanderAttributes(string assetName, AIArchetypeAttributesAsset[] aiArchetypeAssets, CommanderAttributes commanderAttributes)
		{
			if (!aiArchetypeAssets.IsNullOrEmpty<AIArchetypeAttributesAsset>())
			{
				AIArchetypeAttributes[] arrayOriginal = new AIArchetypeAttributes[aiArchetypeAssets.Length];
				for (int i = 0; i < aiArchetypeAssets.Length; i++)
				{
					AIArchetypeAttributesAsset aiarchetypeAttributesAsset = aiArchetypeAssets[i];
					if (aiarchetypeAttributesAsset == null)
					{
						Debug.LogWarning(string.Format("{0}: warning: commander {1} is missing AI archetype asset {2}", assetName, commanderAttributes.Name, i));
					}
					else
					{
						arrayOriginal[i] = (aiarchetypeAttributesAsset.Data as AIArchetypeAttributesData);
					}
				}
				commanderAttributes.AIArchetypes = arrayOriginal;
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000020D0 File Offset: 0x000002D0
		public CommanderAttributesAsset()
		{
		}

		// Token: 0x04000103 RID: 259
		[SerializeField]
		private CommanderAttributesData[] m_CommanderAttributes;
	}
}
