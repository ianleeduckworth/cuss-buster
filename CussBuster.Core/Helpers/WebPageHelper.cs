using System;
using System.Linq;
using CussBuster.Core.Data.Entities;
using CussBuster.Core.Data.Static;
using CussBuster.Core.DataAccess;
using CussBuster.Core.Models;

namespace CussBuster.Core.Helpers
{
	public class WebPageHelper : IWebPageHelper
	{
		private readonly IUserManager _userManager;
		private readonly IStandardPricingTierManager _standardPricingTierManager;

		public WebPageHelper(IUserManager userManager, IStandardPricingTierManager standardPricingTierManager)
		{
			_userManager = userManager;
			_standardPricingTierManager = standardPricingTierManager;
		}

		public UserReturnModel GetUserInfo(Guid apiTokenGuid)
		{
			var user = _userManager.GetUserByApiToken(apiTokenGuid);

			string accountType;

			if (user.PricePerMonth == 0 && user.CallsPerMonth == 250)
				accountType = "Free";
			else if (user.PricePerMonth == 25 && user.CallsPerMonth == 10000)
				accountType = "Standard";
			else if (user.PricePerMonth == 50 && user.CallsPerMonth == 100000)
				accountType = "Premium";
			else
				accountType = "Unknown";

			return new UserReturnModel
			{
				ApiToken = apiTokenGuid,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				AccountLocked = user.CanCallApi,
				CallsPerMonth = user.CallsPerMonth,
				PricePerMonth = user.PricePerMonth,
				AccountType = accountType
			};
		}

		public Guid SignUp(UserSignupModel signupModel)
		{
			if (string.IsNullOrEmpty(signupModel.CreditCardNumber) && signupModel.PricingTierId != (byte)StaticData.StaticPricingTier.Free)
				throw new InvalidOperationException("A credit card number must be provided for any type of non-free account");

			var existingUser = _userManager.GetUserByEmail(signupModel.EmailAddress);
			if (existingUser != null)
				throw new InvalidOperationException($"Account already exists for email address '{signupModel.EmailAddress}'");

			var tier = _standardPricingTierManager.GetStandardPricingTier(signupModel.PricingTierId);
			if (tier == null)
				throw new InvalidOperationException($"Could not find AccountTypeId {signupModel.PricingTierId}");

			return _userManager.AddNewuser(signupModel, tier);
		}
	}
}
