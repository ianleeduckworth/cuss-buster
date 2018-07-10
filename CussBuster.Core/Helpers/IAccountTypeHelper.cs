using System;
using System.Collections.Generic;
using System.Text;

namespace CussBuster.Core.Helpers
{
    public interface IAccountTypeHelper
    {
		string GetAccountTypeBasedOnPricing(decimal pricePerMonth, int callsPerMonth);
		decimal GetPricePerMonth(string accountType);
		int GetCallsPerMonth(string accountType);
		decimal GetPricePerMonth(int accountTypeId);
		int GetCallsPerMonth(int accountTypeId);
	}
}
