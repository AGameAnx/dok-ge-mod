using System;
using BBI.Core;
using BBI.Game.Data;
using BBI.Unity.Core.Data;
using UnityEngine;

namespace BBI.Unity.Game.Data
{
	// Token: 0x0200004B RID: 75
	public sealed class CommanderAttributesAsset : AssetBase
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600010F RID: 271 RVA: 0x0000446E File Offset: 0x0000266E
		public override object Data
		{
			get
			{
				return this.m_CommanderAttributes;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00004476 File Offset: 0x00002676
		public CommanderAttributes[] CommanderAttributes
		{
			get
			{
				BBI.Game.Data.CommanderAttributes[] mCommanderAttributes = this.m_CommanderAttributes;
				BBI.Game.Data.CommanderAttributes[] commanderAttributesArray = new BBI.Game.Data.CommanderAttributes[(int)mCommanderAttributes.Length + 1];
				mCommanderAttributes.CopyTo(commanderAttributesArray, 0);
				CommanderAttributesData startingUnit = ((CommanderAttributesData)mCommanderAttributes[1]).Copy();
				startingUnit.m_StartingUnits = new StartingUnit[] {
					new StartingUnit() { Unit = "G_Fathership" },
					new StartingUnit() { Unit = "G_Harvester" },
					new StartingUnit() { Unit = "G_Harvester" },
					new StartingUnit() { Unit = "G_Harvester" },
					new StartingUnit() { Unit = "G_Harvester" },
					new StartingUnit() { Unit = "G_SupportCruiser" },
					new StartingUnit() { Unit = "G_Baserunner" }
				};
				startingUnit.m_Name = "FATHERSHIP";
				startingUnit.m_NameLocID = "FATHERSHIP";
				startingUnit.m_StartingPopCap = 125;
				startingUnit.m_FactionAttributes = ScriptableObject.CreateInstance<FactionAttributesAsset>();
				startingUnit.m_FactionAttributes.m_FactionAttributes = ((CommanderAttributesData)mCommanderAttributes[1]).m_FactionAttributes.m_FactionAttributes.Copy();
				startingUnit.m_FactionAttributes.m_FactionAttributes.m_FactionName = "FATHERSHIP";
				startingUnit.m_FactionAttributes.m_FactionAttributes.m_FactionID = FactionID.None;
				startingUnit.m_FactionAttributes.m_FactionAttributes.m_TechTreeAttributes = new TechTreeAttributesAsset();
				commanderAttributesArray[(int)mCommanderAttributes.Length] = startingUnit;
				return commanderAttributesArray;
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000447E File Offset: 0x0000267E
		protected override void OnEnable()
		{
			base.OnEnable();
			this.FillArchetypes();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000448C File Offset: 0x0000268C
		private void FillArchetypes()
		{
			foreach (CommanderAttributesData commanderAttributesData in this.m_CommanderAttributes)
			{
				CommanderAttributesAsset.PopulateAIArchetypesForCommanderAttributes(base.name, commanderAttributesData.AIArchetypeAssets, commanderAttributesData);
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000044C4 File Offset: 0x000026C4
		public static void PopulateAIArchetypesForCommanderAttributes(string assetName, AIArchetypeAttributesAsset[] aiArchetypeAssets, CommanderAttributes commanderAttributes)
		{
			if (!aiArchetypeAssets.IsNullOrEmpty<AIArchetypeAttributesAsset>())
			{
				AIArchetypeAttributes[] array = new AIArchetypeAttributes[aiArchetypeAssets.Length];
				for (int i = 0; i < aiArchetypeAssets.Length; i++)
				{
					AIArchetypeAttributesAsset aiarchetypeAttributesAsset = aiArchetypeAssets[i];
					if (aiarchetypeAttributesAsset == null)
					{
						Debug.LogWarning(string.Format("{0}: warning: commander {1} is missing AI archetype asset {2}", assetName, commanderAttributes.Name, i));
					}
					else
					{
						array[i] = (aiarchetypeAttributesAsset.Data as AIArchetypeAttributesData);
					}
				}
				commanderAttributes.AIArchetypes = array;
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004530 File Offset: 0x00002730
		public CommanderAttributesAsset()
		{
		}

		// Token: 0x040000EE RID: 238
		[SerializeField]
		private CommanderAttributesData[] m_CommanderAttributes;
	}
}
