using CussBuster.Core.Data.Static;
using CussBuster.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CussBuster.Core.Helpers
{
	public class AccountTypeHelper : IAccountTypeHelper
	{
		private const string _free = "Free";
		private const string _standard = "Standard";
		private const string _premium = "Premium";
		private const string _custom = "Custom";

		private const int _freeCallsPerMonth = 250;
		private const int _standardCallsPerMonth = 10000;
		private const int _premiumCallsPerMonth = 100000;

		private const decimal _freePricePerMonth = 0.0m;
		private const decimal _standardPricePerMonth = 25.0m;
		private const decimal _premiumPricePerMonth = 50.0m;

		public string GetAccountTypeBasedOnPricing(decimal pricePerMonth, int callsPerMonth)
		{
			string accountType;
			if (pricePerMonth == _freePricePerMonth && callsPerMonth == _freeCallsPerMonth)
				accountType = _free;
			else if (pricePerMonth == _standardPricePerMonth && callsPerMonth == _standardCallsPerMonth)
				accountType = _standard;
			else if (pricePerMonth == _premiumPricePerMonth && callsPerMonth == _premiumCallsPerMonth)
				accountType = _premium;
			else
				accountType = _custom;

			return accountType;
		}

		public int GetCallsPerMonth(string accountType)
		{
			if (accountType == _free)
				return _freeCallsPerMonth;

			if (accountType == _standard)
				return _standardCallsPerMonth;

			if (accountType == _premium)
				return _premiumCallsPerMonth;

			throw new AccountTypeNotFoundException($"Could not find account type {accountType}");
		}

		public int GetCallsPerMonth(int accountTypeId)
		{
			if (accountTypeId == (byte)StaticData.StaticPricingTier.Free)
				return _freeCallsPerMonth;

			if (accountTypeId == (byte)StaticData.StaticPricingTier.Standard)
				return _standardCallsPerMonth;

			if (accountTypeId == (byte)StaticData.StaticPricingTier.Premium)
				return _premiumCallsPerMonth;

			throw new AccountTypeNotFoundException($"Could not find account type where account type id is {accountTypeId}");
		}

		public decimal GetPricePerMonth(string accountType)
		{
			if (accountType == _free)
				return _freePricePerMonth;

			if (accountType == _standard)
				return _standardPricePerMonth;

			if (accountType == _premium)
				return _premiumPricePerMonth;

			throw new AccountTypeNotFoundException($"Could not find account type {accountType}");
		}

		public decimal GetPricePerMonth(int accountTypeId)
		{
			if (accountTypeId == (byte)StaticData.StaticPricingTier.Free)
				return _freePricePerMonth;

			if (accountTypeId == (byte)StaticData.StaticPricingTier.Standard)
				return _standardPricePerMonth;

			if (accountTypeId == (byte)StaticData.StaticPricingTier.Premium)
				return _premiumPricePerMonth;

			throw new AccountTypeNotFoundException($"Could not find account type where account type id is {accountTypeId}");
		}
	}
}
