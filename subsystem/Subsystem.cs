using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using BBI.Core.Data;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Data;
using LitJson;
using Subsystem.Patch;
// Additions to Subsystem to make it compatible with the map mod

using Subsystem.Wrappers;
using UnityEngine;

namespace Subsystem
{
	// Token: 0x02000002 RID: 2
	public class AttributeLoader
	{
		// SSSS's additions
		
		public static string PatchOverrideData { get; set; } = "";
		
		// Catches certain errors in the patch file
		public static bool IsPatchValid(string patch)
		{
			try
			{
				AttributesPatch attributesPatch = JsonMapper.ToObject<AttributesPatch>(patch); // GetPatchData() replaces File.ReadAllText
			}
			catch
			{
				return false;
			}
			return true;
		}
		
		// Get the JSON string data
		public static string GetPatchData()
		{
			if (PatchOverrideData != "") return PatchOverrideData;
			if (MapModManager.GameType == GameMode.Multiplayer) return ""; // Disable file loading in multiplayer
			try
			{
				string path = Path.Combine(Application.dataPath, (Application.platform == RuntimePlatform.OSXPlayer) ? "Resources/Data" : "");
				return File.ReadAllText(Path.Combine(path, "patch.json"));
			}
			catch (Exception arg)
			{
				Debug.LogWarning(string.Format("[SUBSYSTEM] Patch file not found"));
				return "";
			}
		}
	
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public void LoadAttributes(EntityTypeCollection entityTypeCollection)
		{
			try
			{
				AttributesPatch attributesPatch = JsonMapper.ToObject<AttributesPatch>(GetPatchData()); // GetPatchData() replaces File.ReadAllText
				new AttributeLoader().ApplyAttributesPatch(entityTypeCollection, attributesPatch);
			}
			catch (Exception arg)
			{
				Debug.LogWarning(string.Format("[SUBSYSTEM] Error applying patch file: {0}", arg));
			}
		}
		
		// SSSS's additions (end)
	}
}
