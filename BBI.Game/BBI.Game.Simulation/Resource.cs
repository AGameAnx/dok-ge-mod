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
	// Token: 0x020003CC RID: 972
	public sealed class Resource
	{
		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001410 RID: 5136 RVA: 0x0006FD36 File Offset: 0x0006DF36
		internal Entity ResourceEntity
		{
			get
			{
				return this.mResourceEntity;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001411 RID: 5137 RVA: 0x0006FD3E File Offset: 0x0006DF3E
		internal int RemainingAmount
		{
			get
			{
				return Fixed64.Clamp(this.mRemainingAmount, 0, this.mMaxAmount);
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x0006FD52 File Offset: 0x0006DF52
		internal int MaxAmount
		{
			get
			{
				return this.mMaxAmount;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001413 RID: 5139 RVA: 0x0006FD5A File Offset: 0x0006DF5A
		internal ResourceType ResourceType
		{
			get
			{
				return this.mResourceType;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x0006FD62 File Offset: 0x0006DF62
		internal int MaxHarvesters
		{
			get
			{
				return this.mMaxHarvesters;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06001415 RID: 5141 RVA: 0x0006FD6A File Offset: 0x0006DF6A
		internal bool Depleted
		{
			get
			{
				return this.RemainingAmount == 0;
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x0006FD75 File Offset: 0x0006DF75
		// (set) Token: 0x06001417 RID: 5143 RVA: 0x0006FD7D File Offset: 0x0006DF7D
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

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06001418 RID: 5144 RVA: 0x0006FDA5 File Offset: 0x0006DFA5
		internal ResourcePositionalVariations PositionalVariations
		{
			get
			{
				return this.mPositionalVariations;
			}
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x0006FDAD File Offset: 0x0006DFAD
		internal ResourceState GetState(IEnumerable<Entity> harvesterEntities)
		{
			return new ResourceState(this, harvesterEntities);
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x0006FDB8 File Offset: 0x0006DFB8
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

		// Token: 0x0600141B RID: 5147 RVA: 0x0006FE48 File Offset: 0x0006E048
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

		// Token: 0x0600141C RID: 5148 RVA: 0x0006FE95 File Offset: 0x0006E095
		public void SetRemainingResources(int remainingResources)
		{
			this.mRemainingAmount = remainingResources;
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0006FE9E File Offset: 0x0006E09E
		[Obsolete("For save/load only.", true)]
		[ObjectConstructor(new string[]
		{

		})]
		private Resource()
		{
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0006FEB4 File Offset: 0x0006E0B4
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

		// Token: 0x0600141F RID: 5151 RVA: 0x0006FF07 File Offset: 0x0006E107
		internal static Resource Create(Entity owner, int remaining, int max, ResourceType type, int maxHarvesters, bool startDisabled, ResourcePositionalVariations positionalVariations)
		{
			return new Resource(owner, remaining, max, type, maxHarvesters, startDisabled, positionalVariations);
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x0006FF18 File Offset: 0x0006E118
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

		// Token: 0x06001421 RID: 5153 RVA: 0x0006FF87 File Offset: 0x0006E187
		// Note: this type is marked as 'beforefieldinit'.
		static Resource()
		{
		}

		// Token: 0x0400105F RID: 4191
		public static readonly ResourceType[] sResourceTypes = (ResourceType[])Enum.GetValues(typeof(ResourceType));

		// Token: 0x04001060 RID: 4192
		[StateData("Entity")]
		private Entity mResourceEntity = Entity.None;

		// Token: 0x04001061 RID: 4193
		[StateData("RemainingAmount")]
		private int mRemainingAmount;

		// Token: 0x04001062 RID: 4194
		[StateData("MaxAmount")]
		private int mMaxAmount;

		// Token: 0x04001063 RID: 4195
		[StateData("MaxHarvesters")]
		private int mMaxHarvesters;

		// Token: 0x04001064 RID: 4196
		[StateData("ResourceType")]
		private ResourceType mResourceType;

		// Token: 0x04001065 RID: 4197
		[StateData("Disabled")]
		private bool mDisabled;

		// Token: 0x04001066 RID: 4198
		[StateData("PositionalVariations")]
		private ResourcePositionalVariations mPositionalVariations;
	}
}
