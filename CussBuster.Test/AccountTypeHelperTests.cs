using CussBuster.Core.Data.Static;
using CussBuster.Core.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CussBuster.Test
{
	[TestFixture]
    public class AccountTypeHelperTests
    {
		private const string _free = "Free";
		private const string _standard = "Standard";
		private const string _premium = "Premium";

		private const int _freeCallsPerMonth = 250;
		private const int _standardCallsPerMonth = 10000;
		private const int _premiumCallsPerMonth = 100000;

		private const decimal _freePricePerMonth = 0.0m;
		private const decimal _standardPricePerMonth = 25.0m;
		private const decimal _premiumPricePerMonth = 50.0m;

		private AccountTypeHelper _accountTypeHelper;

		[SetUp]
		public void SetUp()
		{
			_accountTypeHelper = new AccountTypeHelper();
		}

		[Test]
		public void GetAccountTypeBasedOnPricing_Free()
		{
			//arrange / act
			var result = _accountTypeHelper.GetAccountTypeBasedOnPricing(_freePricePerMonth, _freeCallsPerMonth);

			//assert
			Assert.True(result == _free);
		}

		[Test]
		public void GetAccountTypeBasedOnPricing_Standard()
		{
			//arrange / act
			var result = _accountTypeHelper.GetAccountTypeBasedOnPricing(_standardPricePerMonth, _standardCallsPerMonth);

			//assert
			Assert.True(result == _standard);
		}

		[Test]
		public void GetAccountTypeBasedOnPricing_Premium()
		{
			//arrange / act
			var result = _accountTypeHelper.GetAccountTypeBasedOnPricing(_premiumPricePerMonth, _premiumCallsPerMonth);

			//assert
			Assert.True(result == _premium);
		}

		[Test]
		public void GetCallsPerMonth_Free_Id()
		{
			//arrange / act
			var result = _accountTypeHelper.GetCallsPerMonth((byte)StaticData.StaticPricingTier.Free);

			//assert
			Assert.True(result == _freeCallsPerMonth);
		}

		[Test]
		public void GetCallsPerMonth_Standard_Id()
		{
			//arrange / act
			var result = _accountTypeHelper.GetCallsPerMonth((byte)StaticData.StaticPricingTier.Standard);

			//assert
			Assert.True(result == _standardCallsPerMonth);
		}

		[Test]
		public void GetCallsPerMonth_Premium_Id()
		{
			//arrange / act
			var result = _accountTypeHelper.GetCallsPerMonth((byte)StaticData.StaticPricingTier.Premium);

			//assert
			Assert.True(result == _premiumCallsPerMonth);
		}

		[Test]
		public void GetCallsPerMonth_Free_String()
		{
			//arrange / act
			var result = _accountTypeHelper.GetCallsPerMonth(_free);

			//assert
			Assert.True(result == _freeCallsPerMonth);
		}

		[Test]
		public void GetCallsPerMonth_Standard_String()
		{
			//arrange / act
			var result = _accountTypeHelper.GetCallsPerMonth(_standard);

			//assert
			Assert.True(result == _standardCallsPerMonth);
		}

		[Test]
		public void GetCallsPerMonth_Premium_String()
		{
			//arrange / act
			var result = _accountTypeHelper.GetCallsPerMonth(_premium);

			//assert
			Assert.True(result == _premiumCallsPerMonth);
		}

		[Test]
		public void GetPricePerMonth_Free_Id()
		{
			//arrange / act
			var result = _accountTypeHelper.GetPricePerMonth((byte)StaticData.StaticPricingTier.Free);

			//assert
			Assert.True(result == _freePricePerMonth);
		}

		[Test]
		public void GetPricePerMonth_Standard_Id()
		{
			//arrange / act
			var result = _accountTypeHelper.GetPricePerMonth((byte)StaticData.StaticPricingTier.Standard);

			//assert
			Assert.True(result == _standardPricePerMonth);
		}

		[Test]
		public void GetPricePerMonth_Premium_Id()
		{
			//arrange / act
			var result = _accountTypeHelper.GetPricePerMonth((byte)StaticData.StaticPricingTier.Premium);

			//assert
			Assert.True(result == _premiumPricePerMonth);
		}

		[Test]
		public void GetPricePerMonth_Free_String()
		{
			//arrange / act
			var result = _accountTypeHelper.GetPricePerMonth(_free);

			//assert
			Assert.True(result == _freePricePerMonth);
		}

		[Test]
		public void GetPricePerMonth_Standard_String()
		{
			//arrange / act
			var result = _accountTypeHelper.GetPricePerMonth(_standard);

			//assert
			Assert.True(result == _standardPricePerMonth);
		}

		[Test]
		public void GetPricePerMonth_Premium_String()
		{
			//arrange / act
			var result = _accountTypeHelper.GetPricePerMonth(_premium);

			//assert
			Assert.True(result == _premiumPricePerMonth);
		}

	}
}
