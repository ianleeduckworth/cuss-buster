using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CussBuster.Core.Data.Entities;

namespace CussBuster.Core.DataAccess
{
	public class StandardPricingTierManager : IStandardPricingTierManager
	{
		private readonly CussBusterContext _context;

		public StandardPricingTierManager(CussBusterContext context)
		{
			_context = context;
		}

		public StandardPricingTier GetStandardPricingTier(int standardPricingTierId)
		{
			return _context.StandardPricingTier.FirstOrDefault(x => x.StandardPricingTierId == standardPricingTierId);
		}
	}
}
