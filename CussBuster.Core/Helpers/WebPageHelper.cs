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
		private readonly CussBusterContext _context;
		private readonly IUserManager _userManager;
		private readonly IStandardPricingTierManager _standardPricingTierManager;

		public WebPageHelper(CussBusterContext context, IUserManager userManager, IStandardPricingTierManager standardPricingTierManager)
		{
			_context = context;
			_userManager = userManager;
			_standardPricingTierManager = standardPricingTierManager;
		}

		public Guid SignUp(SignupModel signupModel)
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
