using System;
using System.Collections.Generic;

namespace CussBuster.Core.Data.Entities
{
    public partial class StandardPricingTier
    {
        public int StandardPricingTierId { get; set; }
        public string Name { get; set; }
        public int CallsPerMonth { get; set; }
        public decimal PricePerMonth { get; set; }
    }
}
