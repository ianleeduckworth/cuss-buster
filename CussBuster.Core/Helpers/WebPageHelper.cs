using System;
using System.Linq;
using System.Text;
using CussBuster.Core.Data.Entities;
using CussBuster.Core.Data.Static;
using CussBuster.Core.DataAccess;
using CussBuster.Core.Exceptions;
using CussBuster.Core.Models;
using CussBuster.Core.Security;

namespace CussBuster.Core.Helpers
{
	public class WebPageHelper : IWebPageHelper
	{
		private readonly IUserManager _userManager;
		private readonly IStandardPricingTierManager _standardPricingTierManager;
		private readonly IAccountTypeHelper _accountTypeHelper;
		private readonly IPasswordHelper _passwordHelper;

		public WebPageHelper(IUserManager userManager, IStandardPricingTierManager standardPricingTierManager, IAccountTypeHelper accountTypeHelper, IPasswordHelper passwordHelper)
		{
			_userManager = userManager;
			_standardPricingTierManager = standardPricingTierManager;
			_accountTypeHelper = accountTypeHelper;
			_passwordHelper = passwordHelper;
		}

		public UserReturnModel GetUserInfo(Guid apiTokenGuid)
		{
			var user = _userManager.GetUserByApiToken(apiTokenGuid);

			if (user == null)
				return null;

			return MapUserToModel(user);
		}

		public UserReturnModel MapUserToModel(User user)
		{
			return new UserReturnModel
			{
				ApiToken = user.ApiToken,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				AccountLocked = !user.CanCallApi,
				CallsPerMonth = user.CallsPerMonth,
				PricePerMonth = user.PricePerMonth,
				AccountType = _accountTypeHelper.GetAccountTypeBasedOnPricing(user.PricePerMonth, user.CallsPerMonth),
				CreditCardNumber = user.CreditCardNumber != null ? $"************{(user.CreditCardNumber % 10000).ToString().PadLeft(4, '0')}" : null,
				CallsThisMonth = _userManager.GetCallsThisMonth(user),
				Racism = user?.UserSetting?.FirstOrDefault(x => x.WordTypeId == (byte)StaticData.WordType.RacialSlur) != null ? true : false,
				RacismSeverity = user?.UserSetting?.FirstOrDefault(x => x.WordTypeId == (byte)StaticData.WordType.RacialSlur)?.Severity,
				Vulgarity = user?.UserSetting?.FirstOrDefault(x => x.WordTypeId == (byte)StaticData.WordType.Vulgarity) != null ? true : false,
				VulgaritySeverity = user?.UserSetting?.FirstOrDefault(x => x.WordTypeId == (byte)StaticData.WordType.Vulgarity)?.Severity,
				Sexism = user?.UserSetting?.FirstOrDefault(x => x.WordTypeId == (byte)StaticData.WordType.Sexism) != null ? true : false,
				SexismSeverity = user?.UserSetting?.FirstOrDefault(x => x.WordTypeId == (byte)StaticData.WordType.Sexism)?.Severity
			};
		}

		public UserReturnModel SignUp(UserSignupModel signupModel, string userName)
		{
			if (!CheckCreditCardInformation(signupModel))
				throw new UserInputException("Credit card information must be provided for any non-free account");

			var existingUser = _userManager.GetUserByEmail(signupModel.EmailAddress);
			if (existingUser != null)
				throw new UserInputException($"Account already exists for email address '{signupModel.EmailAddress}'");

			var tier = _standardPricingTierManager.GetStandardPricingTier(signupModel.PricingTierId);
			if (tier == null)
				throw new UserInputException($"Could not find AccountTypeId {signupModel.PricingTierId}");

			var user = _userManager.AddNewuser(signupModel, tier, userName);
			_userManager.SetStandardSettings(user.UserId);

			return MapUserToModel(user);
		}

		public UserReturnModel UpdateUserInfo(Guid apiTokenGuid, string password, UserUpdateModel userUpdateModel)
		{
			var user = _userManager.GetUserByApiToken(apiTokenGuid);

			if (user == null)
				throw new UserNotFoundException($"User could not be found where API token is {apiTokenGuid}");

			if (!_passwordHelper.CompareSecurePasswords(Encoding.ASCII.GetBytes(password), user.Password))
				throw new UnauthorizedAccessException("Password entered was incorrect");

			user.FirstName = userUpdateModel.FirstName;
			user.LastName = userUpdateModel.LastName;
			user.Email = userUpdateModel.EmailAddress;

			HandleUserSettings(user, (byte)StaticData.WordType.RacialSlur, userUpdateModel.Racism);
			HandleUserSettings(user, (byte)StaticData.WordType.Sexism, userUpdateModel.Sexism);
			HandleUserSettings(user, (byte)StaticData.WordType.Vulgarity, userUpdateModel.Vulgarity);

			user.CallsPerMonth = _accountTypeHelper.GetCallsPerMonth(userUpdateModel.PricingTierId);
			user.PricePerMonth = _accountTypeHelper.GetPricePerMonth(userUpdateModel.PricingTierId);

			_userManager.UpdateExistingUser(user);

			return MapUserToModel(user);
		}

		private bool CheckCreditCardInformation(UserSignupModel userSignupModel)
		{
			if (userSignupModel.PricingTierId == (byte)StaticData.StaticPricingTier.Free)
				return true;

			if (string.IsNullOrEmpty(userSignupModel.FirstName) ||
				string.IsNullOrEmpty(userSignupModel.LastName) ||
				string.IsNullOrEmpty(userSignupModel.AddressLine1) ||
				string.IsNullOrEmpty(userSignupModel.City) ||
				string.IsNullOrEmpty(userSignupModel.State) ||
				string.IsNullOrEmpty(userSignupModel.ZipCode) ||
				string.IsNullOrEmpty(userSignupModel.CreditCardNumber) ||
				string.IsNullOrEmpty(userSignupModel.CreditCardExpirationDate) ||
				string.IsNullOrEmpty(userSignupModel.CreditCardCvcCode))
				return false;

			return true;
		}

		private void HandleUserSettings (User user, byte wordTypeId, bool propertySetting)
		{
			var now = DateTime.Now;

			var setting = user?.UserSetting?.FirstOrDefault(x => x.WordTypeId == wordTypeId);
			if (setting == null && propertySetting)
			{
				user.UserSetting.Add(new UserSetting
				{
					WordTypeId = wordTypeId,
					UserId = user.UserId,
					Severity = 10,
					CreatedDate = now,
					CreatedBy = "test",
					UpdatedDate = now,
					UpdatedBy = "test"
				});
			}
			else if (setting != null && propertySetting)
			{
				//nothing to update yet
			}
			else if (setting != null && !propertySetting)
			{
				user.UserSetting.Remove(setting);
			}
		}
	}
}
