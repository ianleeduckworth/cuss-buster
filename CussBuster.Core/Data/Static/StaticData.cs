﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CussBuster.Core.Data.Static
{
    public static class StaticData
    {
		public enum SearchType
		{
			Equals = 1,
			Contains = 2,
		}

		public enum WordType
		{
			Vulgarity = 1,
			RacialSlur = 2,
			Sexism = 3,
		}

		public enum StaticPricingTier
		{
			Free = 1,
			Standard = 2,
			Premium = 3
		}
    }
}
