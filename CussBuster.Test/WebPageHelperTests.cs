using CussBuster.Core.Data.Entities;
using CussBuster.Core.Data.Static;
using CussBuster.Core.DataAccess;
using CussBuster.Core.Exceptions;
using CussBuster.Core.Helpers;
using CussBuster.Core.Models;
using CussBuster.Core.Security;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CussBuster.Test
{
	[TestFixture]
    public class WebPageHelperTests
    {
		private IWebPageHelper _webPageHelper;
		private Mock<IUserManager> _userManager;
		private Mock<IAccountTypeHelper> _accountTypeHelper;
		private Mock<IStandardPricingTierManager> _standardPricingTierManager;
		private Mock<IPasswordHelper> _passwordHelper;

		[SetUp]
		public void SetUp()
		{
			_userManager = new Mock<IUserManager>();
			_standardPricingTierManager = new Mock<IStandardPricingTierManager>();
			_accountTypeHelper = new Mock<IAccountTypeHelper>();
			_passwordHelper = new Mock<IPasswordHelper>();

			_webPageHelper = new WebPageHelper(_userManager.Object, _standardPricingTierManager.Object, _accountTypeHelper.Object, _passwordHelper.Object);
		}

		[Test]
		public void GetUserInfo_VerifyReturnObject()
		{
			var apiToken = new Guid("a84165c4-d33c-44d0-9478-1651b16b361b");
			const int callsPerMonth = 100;
			const bool canCallApi = true;
			const string email = "email";
			const string firstName = "firstName";
			const string lastName = "lastName";
			const decimal pricePerMonth = 100;
			const int userId = 25;

			//arrange
			_userManager.Setup(x => x.GetUserByApiToken(apiToken)).Returns(new User
			{
				ApiToken = apiToken,
				CallsPerMonth = callsPerMonth,
				CanCallApi = canCallApi,
				Email = email,
				FirstName = firstName,
				LastName = lastName,
				PricePerMonth = pricePerMonth,
				UserId = userId
			});

			_accountTypeHelper.Setup(x => x.GetAccountTypeBasedOnPricing(pricePerMonth, callsPerMonth)).Returns("Custom");

			//act
			var model = _webPageHelper.GetUserInfo(apiToken);

			//assert
			AssertWithMessage.AreEqual(model.AccountLocked, !canCallApi, nameof(model.AccountLocked));
			AssertWithMessage.AreEqual(model.AccountType, "Custom", nameof(model.AccountType));
			AssertWithMessage.AreEqual(model.ApiToken, apiToken, nameof(model.ApiToken));
			AssertWithMessage.AreEqual(model.Email, email, nameof(model.Email));
			AssertWithMessage.AreEqual(model.FirstName, firstName, nameof(model.FirstName));
			AssertWithMessage.AreEqual(model.LastName, lastName, nameof(model.LastName));
			AssertWithMessage.AreEqual(model.CallsPerMonth, callsPerMonth, nameof(model.CallsPerMonth));
			AssertWithMessage.AreEqual(model.PricePerMonth, pricePerMonth, nameof(model.PricePerMonth));
		}

		[Test]
		public void SignUp_NoCreditCardWithNonFreeAccount()
		{
			const string user = "user";

			//arrange
			var signupModel = new UserSignupModel
			{
				CreditCardNumber = null,
				PricingTierId = (byte)StaticData.StaticPricingTier.Standard
			};

			//act / assert
			var ex = Assert.Throws<UserInputException>(() => _webPageHelper.SignUp(signupModel, user));
			AssertWithMessage.AreEqual(ex.Message, "Credit card information must be provided for any non-free account", "exception message");
		}

		[Test]
		public void SignUp_ExistingEmail()
		{
			const string user = "user";
			const string emailAddress = "emailAddress";

			//arrange
			var signupModel = new UserSignupModel
			{
				CreditCardNumber = null,
				EmailAddress = emailAddress,
				PricingTierId = (byte)StaticData.StaticPricingTier.Free
			};

			_userManager.Setup(x => x.GetUserByEmail(signupModel.EmailAddress)).Returns(new User());

			//act / assert
			var ex = Assert.Throws<UserInputException>(() => _webPageHelper.SignUp(signupModel, user));
			AssertWithMessage.AreEqual(ex.Message, $"Account already exists for email address '{signupModel.EmailAddress}'", "exception message");
		}

		[Test]
		public void SignUp_ExistingPricingTier()
		{
			const string user = "user";

			//arrange
			var signupModel = new UserSignupModel
			{
				CreditCardNumber = null,
				PricingTierId = (byte)StaticData.StaticPricingTier.Free
			};

			_userManager.Setup(x => x.GetUserByEmail(signupModel.EmailAddress)).Returns(default(User));
			_standardPricingTierManager.Setup(x => x.GetStandardPricingTier(signupModel.PricingTierId)).Returns(default(StandardPricingTier));

			//act / assert
			var ex = Assert.Throws<UserInputException>(() => _webPageHelper.SignUp(signupModel, user));
			AssertWithMessage.AreEqual(ex.Message, $"Could not find AccountTypeId {signupModel.PricingTierId}", "exception message");
		}

		[Test]
		public void SignUp_Successful()
		{
			const string user = "user";

			const string creditCardNumber = "4444333322221111";
			const string firstName = "firstName";
			const string lastName = "lastName";
			const string email = "email";
			const byte pricingTierId = (byte)StaticData.StaticPricingTier.Standard;
			const int userId = 1;
			const int callsPerMonth = 100;
			const string pricingTierName = "pricingTierName";
			const decimal pricePerMonth = 100;
			const string addressLine1 = "addressLine1";
			const string addressLine2 = "addressLine2";
			const string city = "city";
			const string state = "state";
			const string zipCode = "zipCode";
			const string cvcCode = "cvcCode";
			const string expirationDate = "expirationDate";
			const string password = "password";

			//arrange
			var signupModel = new UserSignupModel
			{
				CreditCardNumber = creditCardNumber,
				PricingTierId = pricingTierId,
				EmailAddress = email,
				FirstName = firstName,
				LastName = lastName,
				AddressLine1 = addressLine1,
				AddressLine2 = addressLine2,
				City = city,
				State = state,
				ZipCode = zipCode,
				CreditCardCvcCode = cvcCode,
				CreditCardExpirationDate = expirationDate,
				Password = password
			};

			_standardPricingTierManager.Setup(x => x.GetStandardPricingTier(pricingTierId)).Returns(new StandardPricingTier
			{
				CallsPerMonth = callsPerMonth,
				Name = pricingTierName,
				PricePerMonth = pricePerMonth,
				StandardPricingTierId = pricingTierId
			});

			_userManager.Setup(x => x.AddNewuser(It.IsAny<UserSignupModel>(), It.IsAny<StandardPricingTier>(), It.IsAny<string>())).Returns(new User
			{
				UserId = userId
			});

			//act
			_webPageHelper.SignUp(signupModel, user);

			//assert
			_userManager.Verify(x => x.AddNewuser(It.Is<UserSignupModel>(m => 
					m.CreditCardNumber == creditCardNumber &&
					m.EmailAddress == email &&
					m.FirstName == firstName &&
					m.LastName == lastName &&
					m.PricingTierId == pricingTierId &&
					m.AddressLine1 == addressLine1 &&
					m.AddressLine2 == addressLine2 &&
					m.City == city &&
					m.State == state &&
					m.ZipCode == zipCode &&
					m.CreditCardCvcCode == cvcCode &&
					m.CreditCardExpirationDate == expirationDate &&
					m.Password == password), 
				It.Is<StandardPricingTier>(m =>
					m.CallsPerMonth == callsPerMonth &&
					m.Name == pricingTierName &&
					m.PricePerMonth == pricePerMonth &&
					m.StandardPricingTierId == pricingTierId
				), user
			));

			_userManager.Verify(x => x.SetStandardSettings(userId));
		}

		[Test]
		public void UpdateUserInfo_NullUserManager()
		{
			Guid apiToken = new Guid("d2ab6d8a-d8b3-42a6-9dd4-a36a8a9b1f79");
			const string password = "password";

			//arrange
			var userUpdateModel = new UserUpdateModel();

			_userManager.Setup(x => x.GetUserByApiToken(apiToken)).Returns(default(User));

			//act / assert
			var ex = Assert.Throws<UserNotFoundException>(() => _webPageHelper.UpdateUserInfo(apiToken, password, userUpdateModel));
			AssertWithMessage.AreEqual(ex.Message, $"User could not be found where API token is {apiToken}", "exception message");
		}

		[Test]
		public void UpdateUserInfo_Success()
		{
			Guid apiToken = new Guid("d2ab6d8a-d8b3-42a6-9dd4-a36a8a9b1f79");
			const int accountTypeId = 1;
			const int callsPerMonth = 100;
			const decimal pricePerMonth = 25m;
			const string firstName = "firstName";
			const string lastName = "lastName";
			const string emailAddress = "emailAddress";
			const string password = "password";

			//arrange
			_accountTypeHelper.Setup(x => x.GetCallsPerMonth(accountTypeId)).Returns(callsPerMonth);
			_accountTypeHelper.Setup(x => x.GetPricePerMonth(accountTypeId)).Returns(pricePerMonth);
			_userManager.Setup(x => x.GetUserByApiToken(apiToken)).Returns(new User
			{
				ApiToken = apiToken,
				UserId = 1,
				UserSetting = new List<UserSetting>
				{
					new UserSetting
					{
						UserId = 1,
						WordTypeId = (byte)StaticData.WordType.RacialSlur
					}
				},
				FirstName = $"{firstName}_test",
				LastName = $"{lastName}_test",
				Email = $"{emailAddress}_test",
				CallsPerMonth = callsPerMonth + 1,
				PricePerMonth = pricePerMonth + 1,
				Password = Encoding.ASCII.GetBytes(password)
			});

			var userUpdateModel = new UserUpdateModel
			{
				FirstName = firstName,
				LastName = lastName,
				EmailAddress = emailAddress,
				PricingTierId = accountTypeId,
				Racism = false,
				Vulgarity = true,
				Sexism = true
			};

			_passwordHelper.Setup(x => x.CompareSecurePasswords(It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);

			//act
			_webPageHelper.UpdateUserInfo(apiToken, password, userUpdateModel);

			//assert
			_userManager.Verify(x => x.UpdateExistingUser(It.Is<User>(y =>
				y.FirstName == firstName &&
				y.LastName == lastName &&
				y.Email == emailAddress &&
				y.CallsPerMonth == callsPerMonth &&
				y.PricePerMonth == pricePerMonth &&
				y.ApiToken == apiToken &&
				y.UserSetting.Count == 2 &&
				y.UserSetting.FirstOrDefault(z => z.WordTypeId == (byte)StaticData.WordType.Vulgarity) != null &&
				y.UserSetting.FirstOrDefault(z => z.WordTypeId == (byte)StaticData.WordType.Sexism) != null
			)));
		}
	}
}
