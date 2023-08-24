using System;
using BBI.Game.Data;
using BBI.Unity.Core.Data;
using UnityEngine;

namespace BBI.Unity.Game.Data
{
	internal sealed class TechTreeAttributesAsset : AssetBase, IEntityTypeRegistrationRequired
	{
		public override object Data
		{
			get
			{
				return this.m_Attributes;
			}
		}

		public TechTreeAttributesAsset()
		{
		}

		[SerializeField]
		private TechTreeAttributesData m_Attributes;
	}
}
