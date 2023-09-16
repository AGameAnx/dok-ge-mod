using System;
using System.Collections.Generic;
using BBI.Core.ComponentModel;
using BBI.Core.Data;
using BBI.Core.Utility;
using BBI.Core.Utility.FixedPoint;
using BBI.Game.Data;
using BBI.Game.Events;

namespace BBI.Game.Simulation
{
	public sealed class Resource
	{
		internal Entity ResourceEntity
		{
			get
			{
				return this.mResourceEntity;
			}
		}

		internal int RemainingAmount
		{
			get
			{
				return Fixed64.Clamp(this.mRemainingAmount, 0, this.mMaxAmount);
			}
		}

		internal int MaxAmount
		{
			get
			{
				return this.mMaxAmount;
			}
		}

		internal ResourceType ResourceType
		{
			get
			{
				return this.mResourceType;
			}
		}

		internal int MaxHarvesters
		{
			get
			{
				return this.mMaxHarvesters;
			}
		}

		internal bool Depleted
		{
			get
			{
				return this.RemainingAmount == 0;
			}
		}

		public bool Disabled
		{
			get
			{
				return this.mDisabled;
			}
			set
			{
				if (this.mDisabled != value)
				{
					this.mDisabled = value;
					Sim.PostEvent(new ResourceToggledEvent(this.mResourceEntity, this.mDisabled));
				}
			}
		}

		internal ResourcePositionalVariations PositionalVariations
		{
			get
			{
				return this.mPositionalVariations;
			}
		}

		internal ResourceState GetState(IEnumerable<Entity> harvesterEntities)
		{
			return new ResourceState(this, harvesterEntities);
		}

		internal int Extract(Entity harvester, ResourceType type, int amount)
		{
			int num = 0;
			if (this.ResourceType == type)
			{
				switch (type)
				{
				case ResourceType.Resource1:
				case ResourceType.Resource2:
					num = this.ExtractResource(harvester, type, amount);
					break;
				case ResourceType.Resource3:
				{
					Wreck component = this.mResourceEntity.GetComponent<Wreck>(37);
					num = component.SalvageWreck(harvester, amount);
					this.mRemainingAmount -= num;
					break;
				}
				}
				if (this.mRemainingAmount < 0)
				{
					Log.Error(Log.Channel.Gameplay, "Remaining resources should always be greater or equal to zero, but was {0}", new object[]
					{
						this.mRemainingAmount
					});
				}
			}
			return num;
		}

		private int ExtractResource(Entity harvester, ResourceType type, int amount)
		{
			int num = 0;
			if (this.mRemainingAmount > 0)
			{
				num = Math.Min(amount, this.mRemainingAmount);
				this.mRemainingAmount -= num;
				Sim.PostEvent(new ResourceGatheredEvent(harvester, this.mResourceEntity, this.mRemainingAmount, num, type));
			}
			return num;
		}

		public void SetRemainingResources(int remainingResources)
		{
			this.mRemainingAmount = remainingResources;
		}

		[Obsolete("For save/load only.", true)]
		[ObjectConstructor(new string[]
		{

		})]
		private Resource()
		{
		}

		private Resource(Entity owner, int remaining, int max, ResourceType type, int maxHarvesters, bool startDisabled, ResourcePositionalVariations positionalVariations)
		{
			this.mResourceEntity = owner;
			this.mRemainingAmount = remaining;
			this.mMaxAmount = max;
			this.mResourceType = type;
			this.mMaxHarvesters = maxHarvesters;
			this.mDisabled = startDisabled;
			this.mPositionalVariations = positionalVariations;
		}

		internal static Resource Create(Entity owner, int remaining, int max, ResourceType type, int maxHarvesters, bool startDisabled, ResourcePositionalVariations positionalVariations)
		{
			return new Resource(owner, remaining, max, type, maxHarvesters, startDisabled, positionalVariations);
		}

		internal int GetChecksum()
		{
			int num = Checksum.Combine(this.mRemainingAmount, this.mMaxAmount, (int)this.mResourceType, this.mMaxHarvesters, this.mDisabled.GetHashCode());
			if (this.mResourceEntity.HasComponent(10))
			{
				Position component = this.mResourceEntity.GetComponent<Position>(10);
				num = Checksum.Combine(num, component.Position2D.GetHashCode());
			}
			return num;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Resource()
		{
		}

		public static readonly ResourceType[] sResourceTypes = (ResourceType[])Enum.GetValues(typeof(ResourceType));

		[StateData("Entity")]
		private Entity mResourceEntity = Entity.None;

		[StateData("RemainingAmount")]
		private int mRemainingAmount;

		[StateData("MaxAmount")]
		private int mMaxAmount;

		[StateData("MaxHarvesters")]
		private int mMaxHarvesters;

		[StateData("ResourceType")]
		private ResourceType mResourceType;

		[StateData("Disabled")]
		private bool mDisabled;

		[StateData("PositionalVariations")]
		private ResourcePositionalVariations mPositionalVariations;
	}
}
