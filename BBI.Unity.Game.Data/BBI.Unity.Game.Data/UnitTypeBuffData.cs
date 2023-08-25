using System;
using BBI.Game.Data;
using UnityEngine;

namespace BBI.Unity.Game.Data
{
	[Serializable]
	public class UnitTypeBuffData : UnitTypeBuff
	{
		string UnitTypeBuff.UnitType
		{
			get
			{
				return this.m_UnitTypePrefix;
			}
		}

		bool UnitTypeBuff.UseAsPrefix
		{
			get
			{
				return this.m_UseAsPrefix;
			}
		}

		UnitClass UnitTypeBuff.UnitClass
		{
			get
			{
				return this.m_UnitClass;
			}
		}

		FlagOperator UnitTypeBuff.ClassOperator
		{
			get
			{
				return this.m_ClassOperator;
			}
		}

		AttributeBuffSet UnitTypeBuff.BuffSet
		{
			get
			{
				if (this.m_BuffSet == null)
				{
					return null;
				}
				return this.m_BuffSet.BuffSet;
			}
		}

		public UnitTypeBuffData()
		{
		}

		[SerializeField]
		private string m_UnitTypePrefix;

		[SerializeField]
		private bool m_UseAsPrefix = true;

		[SerializeField]
		private UnitClass m_UnitClass;

		[SerializeField]
		private FlagOperator m_ClassOperator = FlagOperator.And;

		[SerializeField]
		private BuffSetAttributesAsset m_BuffSet;
	}
}
