using CussBuster.Core.Data.Entities;

namespace CussBuster.Core.DataAccess
{
	public interface IStandardPricingTierManager
	{
		StandardPricingTier GetStandardPricingTier(int standardPricingTierId);
	}
}