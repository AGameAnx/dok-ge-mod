using System;
using BBI.Core.ComponentModel;
using BBI.Core.Utility;
using BBI.Game.Simulation;
using BBI.Unity.Game.HUD;
using UnityEngine;

namespace BBI.Unity.Game.World
{
	// Token: 0x0200006D RID: 109
	public partial class ExtractionZoneView
	{
		// Token: 0x0600039D RID: 925 RVA: 0x000261E8 File Offset: 0x000243E8
		private void CreateArrow(HUDSystem hudSystem, Entity entity, GameObject iconPrefab, SceneExtractionZoneEntity sceneObject, float altitudeOffset, string labelText, Color iconColour)
		{
			// MOD: disable the arrow above ezs
			return;
			// MOD
		}
	}
}
