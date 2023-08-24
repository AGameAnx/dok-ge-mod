using System;
using System.Collections.Generic;
using BBI.Core;
using BBI.Core.ComponentModel;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Data;
using BBI.Game.Events;
using BBI.Game.Utility;

namespace BBI.Game.Simulation
{
	// Token: 0x02000387 RID: 903
	internal static class DetectableProcessor
	{
		// Token: 0x0600119C RID: 4508 RVA: 0x0005B844 File Offset: 0x00059A44
		internal static void RefreshAllDetectablesOnLoad()
		{
			DetectableProcessor.sCommanderDetectableMap.Clear();
			DetectableProcessor.sCommanderSensorMap.Clear();
			DetectableProcessor.sUnsensedUnitsScratchPad.Clear();
			DetectableProcessor.sSensorsToAdd.Clear();
			DetectableProcessor.sDetectablesToAdd.Clear();
			foreach (CommanderID key in Sim.Instance.CommanderManager.CommanderIDs)
			{
				DetectableProcessor.sCommanderDetectableMap.Add(key, new DetectableProcessor.SortedEntityList(21));
				DetectableProcessor.sCommanderSensorMap.Add(key, new DetectableProcessor.SortedEntityList(20));
			}
			IEnumerable<Entity> enumerable = Sim.Instance.EntitySystem.Query().Has(20);
			foreach (Entity sensorEntity in enumerable)
			{
				DetectableProcessor.AddNewSensor(sensorEntity);
			}
			IEnumerable<Entity> enumerable2 = Sim.Instance.EntitySystem.Query().Has(21);
			foreach (Entity detectableEntity in enumerable2)
			{
				DetectableProcessor.AddNewDetectable(detectableEntity);
			}
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0005B99C File Offset: 0x00059B9C
		internal static void ResetDetectables()
		{
			foreach (DetectableProcessor.SortedEntityList sortedEntityList in DetectableProcessor.sCommanderDetectableMap.Values<CommanderID, DetectableProcessor.SortedEntityList>())
			{
				foreach (Entity entity in sortedEntityList)
				{
					Detectable component = entity.GetComponent(21);
					component.SwapDetectionMaps();
				}
			}
			bool flag = Sim.Instance.CommanderManager.ActiveCommanders.Count() == 0;
			CommanderID localPlayerCommanderID = SimController.LocalPlayerCommanderID;
			Commander commanderFromID = Sim.Instance.CommanderManager.GetCommanderFromID(localPlayerCommanderID);
			if (!flag && commanderFromID != null)
			{
				flag = commanderFromID.FogOfWarDisabled;
			}
			foreach (CommanderID commanderID in DetectableProcessor.sCommanderDetectableMap.Keys<CommanderID, DetectableProcessor.SortedEntityList>())
			{
				DetectableProcessor.SortedEntityList sortedEntityList2 = DetectableProcessor.sCommanderDetectableMap[commanderID];
				foreach (CommanderID commanderID2 in DetectableProcessor.sCommanderSensorMap.Keys<CommanderID, DetectableProcessor.SortedEntityList>())
				{
					bool flag2 = Sim.Instance.InteractionProvider.AreFriendly(commanderID, commanderID2);
					foreach (Entity entity2 in sortedEntityList2)
					{
						Detectable component2 = entity2.GetComponent(21);
						if (!entity2.HasComponent(10))
						{
							component2.SetDetectionState(commanderID2, DetectionState.Hidden);
						}
						else if (!component2.SetForcedState(commanderID2))
						{
							DetectionState detectionState = DetectionState.Hidden;
							Entity entity3;
							bool flag3 = component2.LastSensedByEntity.TryGetValue(commanderID2, out entity3);
							if (!flag3 && !flag2 && component2.LastSensedByEntity.Count > 0)
							{
								foreach (CommanderID commanderID3 in DetectableProcessor.sCommanderSensorMap.Keys<CommanderID, DetectableProcessor.SortedEntityList>())
								{
									if (commanderID3 != commanderID && commanderID3 != commanderID2 && Sim.Instance.InteractionProvider.AreFriendly(commanderID3, commanderID2))
									{
										flag3 = component2.LastSensedByEntity.TryGetValue(commanderID3, out entity3);
										if (flag3)
										{
											break;
										}
									}
								}
							}
							if (flag3)
							{
								if (!flag2 && !Sim.IsEntityDead(entity3) && entity3.HasComponent(10) && entity3.HasComponent(20))
								{
									Sensor component3 = entity3.GetComponent(20);
									Position component4 = entity3.GetComponent(10);
									Position component5 = entity2.GetComponent(10);
									detectionState = component3.GetEntityDetectionState(component4, component5);
								}
								if (detectionState != DetectionState.Sensed)
								{
									component2.LastSensedByEntity.Remove(commanderID2);
								}
							}
							if (detectionState != DetectionState.Sensed)
							{
								detectionState = component2.GetDefaultState(flag, entity2, commanderID, flag2);
							}
							component2.SetDetectionState(commanderID2, detectionState);
							if (detectionState < DetectionState.Sensed)
							{
								Dictionary<CommanderID, List<Entity>> dictionary;
								if (!DetectableProcessor.sUnsensedUnitsScratchPad.TryGetValue(commanderID2, out dictionary))
								{
									dictionary = new Dictionary<CommanderID, List<Entity>>(Sim.Instance.CommanderManager.CommanderCount);
									DetectableProcessor.sUnsensedUnitsScratchPad[commanderID2] = dictionary;
								}
								List<Entity> list;
								if (!dictionary.TryGetValue(commanderID, out list))
								{
									list = new List<Entity>(200);
									dictionary[commanderID] = list;
								}
								list.Add(entity2);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0005BD94 File Offset: 0x00059F94
		internal static void UpdateDetectionState()
		{
			List<Entity> list = TransientLists.GetList<Entity>();
			foreach (CommanderID commanderID in DetectableProcessor.sCommanderSensorMap.Keys<CommanderID, DetectableProcessor.SortedEntityList>())
			{
				DetectableProcessor.SortedEntityList sortedEntityList = DetectableProcessor.sCommanderSensorMap[commanderID];
				Dictionary<CommanderID, List<Entity>> dictionary;
				if (DetectableProcessor.sUnsensedUnitsScratchPad.TryGetValue(commanderID, out dictionary))
				{
					foreach (CommanderID key in DetectableProcessor.sCommanderDetectableMap.Keys<CommanderID, DetectableProcessor.SortedEntityList>())
					{
						List<Entity> list2;
						if (dictionary.TryGetValue(key, out list2) && list2.Count != 0)
						{
							foreach (Entity entity in sortedEntityList)
							{
								Sensor component = entity.GetComponent(20);
								Position component2 = entity.GetComponent(10);
								Fixed64 y = Fixed64.Max(component.ContactRadius, component.SensorRadius);
								Fixed64 y2 = component2.Position2D.X - y;
								Fixed64 y3 = component2.Position2D.X + y;
								Fixed64 y4 = component2.Position2D.Y - y;
								Fixed64 y5 = component2.Position2D.Y + y;
								foreach (Entity entity2 in list2)
								{
									Position component3 = entity2.GetComponent(10);
									if (component3.Position2D.X > y3)
									{
										break;
									}
									if (component3.Position2D.X >= y2 && component3.Position2D.Y >= y4 && component3.Position2D.Y <= y5)
									{
										Detectable component4 = entity2.GetComponent(21);
										DetectionState entityDetectionState = component.GetEntityDetectionState(component2, component3);
										if (entityDetectionState > DetectionState.Hidden)
										{
											DetectionState rawDetectionState = component4.GetRawDetectionState(commanderID);
											if (entityDetectionState > rawDetectionState)
											{
												component4.SetDetectionState(commanderID, entityDetectionState);
												if (entityDetectionState == DetectionState.Sensed)
												{
													component4.LastSensedByEntity[commanderID] = entity;
													list.Add(entity2);
												}
											}
										}
									}
								}
								if (list.Count > 0)
								{
									foreach (Entity item in list)
									{
										list2.Remove(item);
									}
									list.Clear();
									if (list2.Count == 0)
									{
										break;
									}
								}
							}
						}
					}
				}
			}
			TransientLists.ReturnList<Entity>(list);
			foreach (Dictionary<CommanderID, List<Entity>> dictionary2 in DetectableProcessor.sUnsensedUnitsScratchPad.Values<CommanderID, Dictionary<CommanderID, List<Entity>>>())
			{
				foreach (List<Entity> list3 in dictionary2.Values<CommanderID, List<Entity>>())
				{
					list3.Clear();
				}
			}
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0005C16C File Offset: 0x0005A36C
		internal static void UpdateSensorList()
		{
			DetectableProcessor.UpdateListHelper(DetectableProcessor.sSensorsToAdd, DetectableProcessor.sCommanderSensorMap, 20);
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x0005C17F File Offset: 0x0005A37F
		internal static void UpdateDetectableList()
		{
			DetectableProcessor.UpdateListHelper(DetectableProcessor.sDetectablesToAdd, DetectableProcessor.sCommanderDetectableMap, 21);
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0005C194 File Offset: 0x0005A394
		internal static void UpdateListHelper(List<Entity> newEntities, Dictionary<CommanderID, DetectableProcessor.SortedEntityList> correspondingMap, int componentID)
		{
			if (newEntities.Count > 0)
			{
				for (int i = newEntities.Count - 1; i >= 0; i--)
				{
					Entity entity = newEntities[i];
					CommanderID entityCommanderID = Sim.GetEntityCommanderID(entity);
					DetectableProcessor.AddToCommanderMapHelper(entityCommanderID, entity, correspondingMap, componentID);
				}
				newEntities.Clear();
			}
			List<Entity> list = TransientLists.GetList<Entity>();
			List<CommanderID> list2 = TransientLists.GetList<CommanderID>();
			foreach (KeyValuePair<CommanderID, DetectableProcessor.SortedEntityList> keyValuePair in correspondingMap)
			{
				keyValuePair.Value.Upkeep(keyValuePair.Key, list, list2);
			}
			if (list.Count > 0)
			{
				for (int j = list.Count - 1; j >= 0; j--)
				{
					DetectableProcessor.AddToCommanderMapHelper(list2[j], list[j], correspondingMap, componentID);
				}
			}
			TransientLists.ReturnList<Entity>(list);
			TransientLists.ReturnList<CommanderID>(list2);
			foreach (DetectableProcessor.SortedEntityList sortedEntityList in correspondingMap.Values<CommanderID, DetectableProcessor.SortedEntityList>())
			{
				sortedEntityList.SortHorizontally();
			}
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x0005C2C8 File Offset: 0x0005A4C8
		private static void AddToCommanderMapHelper(CommanderID commander, Entity entity, Dictionary<CommanderID, DetectableProcessor.SortedEntityList> correspondingMap, int componentID)
		{
			DetectableProcessor.SortedEntityList sortedEntityList;
			if (!correspondingMap.TryGetValue(commander, out sortedEntityList))
			{
				sortedEntityList = new DetectableProcessor.SortedEntityList(componentID);
				correspondingMap.Add(commander, sortedEntityList);
			}
			sortedEntityList.Add(entity);
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x0005C2F8 File Offset: 0x0005A4F8
		internal static void Initialize()
		{
			DetectableProcessor.sCommanderSensorMap = new Dictionary<CommanderID, DetectableProcessor.SortedEntityList>(6);
			DetectableProcessor.sCommanderDetectableMap = new Dictionary<CommanderID, DetectableProcessor.SortedEntityList>(6);
			DetectableProcessor.sUnsensedUnitsScratchPad = new Dictionary<CommanderID, Dictionary<CommanderID, List<Entity>>>(6);
			DetectableProcessor.sSensorsToAdd = new List<Entity>(200);
			if (DetectableProcessor.sDetectablesToAdd == null)
			{
				DetectableProcessor.sDetectablesToAdd = new List<Entity>(200);
			}
			DetectableProcessor.sMapMidwayPoint = (Sim.Instance.Map.Min.X + Sim.Instance.Map.Max.X) * Fixed64.Half;
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x0005C388 File Offset: 0x0005A588
		internal static void Shutdown()
		{
			if (DetectableProcessor.sCommanderSensorMap != null)
			{
				DetectableProcessor.sCommanderSensorMap.Clear();
				DetectableProcessor.sCommanderSensorMap = null;
			}
			if (DetectableProcessor.sCommanderDetectableMap != null)
			{
				DetectableProcessor.sCommanderDetectableMap.Clear();
				DetectableProcessor.sCommanderDetectableMap = null;
			}
			if (DetectableProcessor.sUnsensedUnitsScratchPad != null)
			{
				DetectableProcessor.sUnsensedUnitsScratchPad.Clear();
				DetectableProcessor.sUnsensedUnitsScratchPad = null;
			}
			if (DetectableProcessor.sSensorsToAdd != null)
			{
				DetectableProcessor.sSensorsToAdd.Clear();
				DetectableProcessor.sSensorsToAdd = null;
			}
			if (DetectableProcessor.sDetectablesToAdd != null)
			{
				DetectableProcessor.sDetectablesToAdd.Clear();
				DetectableProcessor.sDetectablesToAdd = null;
			}
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0005C408 File Offset: 0x0005A608
		internal static void PostDetectionStateEvents(Entity self)
		{
			Detectable component = self.GetComponent(21);
			DetectionState from;
			DetectionState to;
			if (component.HasSharedDetectionStateChangedSinceLastFrame(SimController.LocalPlayerCommanderID, out from, out to))
			{
				Sim.PostEvent(new VisibilityChangedEvent(self, from, to));
			}
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0005C43C File Offset: 0x0005A63C
		internal static void AddNewSensor(Entity sensorEntity)
		{
			DetectableProcessor.sSensorsToAdd.Add(sensorEntity);
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x0005C449 File Offset: 0x0005A649
		internal static void AddNewDetectable(Entity detectableEntity)
		{
			if (DetectableProcessor.sDetectablesToAdd == null)
			{
				DetectableProcessor.sDetectablesToAdd = new List<Entity>(200);
			}
			DetectableProcessor.sDetectablesToAdd.Add(detectableEntity);
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x0005C46C File Offset: 0x0005A66C
		// Note: this type is marked as 'beforefieldinit'.
		static DetectableProcessor()
		{
		}

		// Token: 0x04000EA6 RID: 3750
		private const int kInitializeSize = 200;

		// Token: 0x04000EA7 RID: 3751
		private static Dictionary<CommanderID, DetectableProcessor.SortedEntityList> sCommanderSensorMap;

		// Token: 0x04000EA8 RID: 3752
		private static Dictionary<CommanderID, DetectableProcessor.SortedEntityList> sCommanderDetectableMap;

		// Token: 0x04000EA9 RID: 3753
		private static Dictionary<CommanderID, Dictionary<CommanderID, List<Entity>>> sUnsensedUnitsScratchPad;

		// Token: 0x04000EAA RID: 3754
		private static List<Entity> sSensorsToAdd = null;

		// Token: 0x04000EAB RID: 3755
		private static List<Entity> sDetectablesToAdd = null;

		// Token: 0x04000EAC RID: 3756
		private static Fixed64 sMapMidwayPoint;

		// Token: 0x02000388 RID: 904
		internal class SortedEntityList : List<Entity>
		{
			// Token: 0x060011A9 RID: 4521 RVA: 0x0005C47A File Offset: 0x0005A67A
			public SortedEntityList(int componentID) : base(200)
			{
				this.mComponentID = componentID;
			}

			// Token: 0x060011AA RID: 4522 RVA: 0x0005C490 File Offset: 0x0005A690
			internal void Upkeep(CommanderID owner, List<Entity> switchedCommanderList, List<CommanderID> switchedCommanderMap)
			{
				for (int i = base.Count - 1; i >= 0; i--)
				{
					Entity entity = base[i];
					if (Sim.IsEntityDead(entity) || !entity.HasComponent(this.mComponentID))
					{
						base.RemoveAt(i);
					}
					else
					{
						CommanderID entityCommanderID = Sim.GetEntityCommanderID(entity);
						if (entityCommanderID != owner)
						{
							switchedCommanderList.Add(entity);
							switchedCommanderMap.Add(entityCommanderID);
							base.RemoveAt(i);
						}
					}
				}
			}

			// Token: 0x060011AB RID: 4523 RVA: 0x0005C4FC File Offset: 0x0005A6FC
			internal void SortHorizontally()
			{
				bool flag = false;
				int count = base.Count;
				int num = count;
				int i = 0;
				while (i < count - 1)
				{
					int index = i;
					int num2 = i + 1;
					Entity entity = base[index];
					Entity entity2 = base[num2];
					int num3 = DetectableProcessor.SortedEntityList.CompareEntities(entity, entity2);
					if (num3 > 0)
					{
						base[index] = entity2;
						base[num2] = entity;
						if (i != 0)
						{
							if (!flag)
							{
								num = num2;
								flag = true;
							}
							i--;
							continue;
						}
					}
					if (flag)
					{
						i = num;
						flag = false;
					}
					else
					{
						i++;
					}
				}
			}

			// Token: 0x060011AC RID: 4524 RVA: 0x0005C57C File Offset: 0x0005A77C
			private static int CompareEntities(Entity e1, Entity e2)
			{
				Position component = e1.GetComponent(10);
				Position component2 = e2.GetComponent(10);
				if (component != null && component2 != null)
				{
					Fixed64 x = component.Position2D.X;
					return x.CompareTo(component2.Position2D.X);
				}
				if (component != null)
				{
					return -1;
				}
				if (component2 != null)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x04000EAD RID: 3757
			private readonly int mComponentID;
		}
	}
}
