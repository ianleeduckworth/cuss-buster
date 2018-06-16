using CussBuster.Core.Data.Entities;
using CussBuster.Core.Data.Static;
using CussBuster.Core.DataAccess;
using CussBuster.Core.Helpers;
using CussBuster.Core.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CussBuster.Test
{
	[TestFixture]
    public class WebPageHelperTests
    {
		private IWebPageHelper _webPageHelper;
		private Mock<IUserManager> _userManager;
		private Mock<IStandardPricingTierManager> _standardPricingTierManager;

		[SetUp]
		public void SetUp()
		{
			_userManager = new Mock<IUserManager>();
			_standardPricingTierManager = new Mock<IStandardPricingTierManager>();

			_webPageHelper = new WebPageHelper(_userManager.Object, _standardPricingTierManager.Object);
		}

		[Test]
		public void GetUserInfo_AccountTypeFree()
		{
			var apiToken = new Guid("a84165c4-d33c-44d0-9478-1651b16b361b");

			//arrange
			_userManager.Setup(x => x.GetUserByApiToken(apiToken)).Returns(new User
			{
				PricePerMonth = 0,
				CallsPerMonth = 250
			});

			//act
			var model = _webPageHelper.GetUserInfo(apiToken);

			//assert
			Assert.True(model.AccountType == "Free");
		}

		[Test]
		public void GetUserInfo_AccountTypeStandard()
		{
			var apiToken = new Guid("a84165c4-d33c-44d0-9478-1651b16b361b");

			//arrange
			_userManager.Setup(x => x.GetUserByApiToken(apiToken)).Returns(new User
			{
				PricePerMonth = 25,
				CallsPerMonth = 10000
			});

			//act
			var model = _webPageHelper.GetUserInfo(apiToken);

			//assert
			Assert.True(model.AccountType == "Standard");
		}

		[Test]
		public void GetUserInfo_AccountTypePremium()
		{
			var apiToken = new Guid("a84165c4-d33c-44d0-9478-1651b16b361b");

			//arrange
			_userManager.Setup(x => x.GetUserByApiToken(apiToken)).Returns(new User
			{
				PricePerMonth = 50,
				CallsPerMonth = 100000
			});

			//act
			var model = _webPageHelper.GetUserInfo(apiToken);

			//assert
			Assert.True(model.AccountType == "Premium");
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

			//act
			var model = _webPageHelper.GetUserInfo(apiToken);

			//assert
			Assert.True(model.AccountLocked == canCallApi);
			Assert.True(model.AccountType == "Unknown");
			Assert.True(model.ApiToken == apiToken);
			Assert.True(model.Email == email);
			Assert.True(model.FirstName == firstName);
			Assert.True(model.LastName == lastName);
			Assert.True(model.CallsPerMonth == callsPerMonth);
			Assert.True(model.PricePerMonth == pricePerMonth);
		}

		[Test]
		public void SignUp_NoCreditCardWithNonFreeAccount()
		{
			//arrange
			var signupModel = new UserSignupModel
			{
				CreditCardNumber = null,
				PricingTierId = (byte)StaticData.StaticPricingTier.Standard
			};

			//act / assert
			var ex = Assert.Throws<InvalidOperationException>(() => _webPageHelper.SignUp(signupModel));
			Assert.True(ex.Message == "A credit card number must be provided for any type of non-free account");
		}

		[Test]
		public void SignUp_ExistingEmail()
		{
			//arrange
			var signupModel = new UserSignupModel
			{
				CreditCardNumber = null,
				PricingTierId = (byte)StaticData.StaticPricingTier.Free
			};

			_userManager.Setup(x => x.GetUserByEmail(signupModel.EmailAddress)).Returns(new User());

			//act / assert
			var ex = Assert.Throws<InvalidOperationException>(() => _webPageHelper.SignUp(signupModel));
			Assert.True(ex.Message == $"Account already exists for email address '{signupModel.EmailAddress}'");
		}

		[Test]
		public void SignUp_ExistingPricingTier()
		{
			//arrange
			var signupModel = new UserSignupModel
			{
				CreditCardNumber = null,
				PricingTierId = (byte)StaticData.StaticPricingTier.Free
			};

			_userManager.Setup(x => x.GetUserByEmail(signupModel.EmailAddress)).Returns(default(User));
			_standardPricingTierManager.Setup(x => x.GetStandardPricingTier(signupModel.PricingTierId)).Returns(default(StandardPricingTier));

			//act / assert
			var ex = Assert.Throws<InvalidOperationException>(() => _webPageHelper.SignUp(signupModel));
			Assert.True(ex.Message == $"Could not find AccountTypeId {signupModel.PricingTierId}");
		}

		[Test]
		public void SignUp_Successful()
		{
			const string creditCardNumber = "4444333322221111";
			const string firstName = "firstName";
			const string lastName = "lastName";
			const string email = "email";
			const byte pricingTierId = (byte)StaticData.StaticPricingTier.Standard;

			const int callsPerMonth = 100;
			const string pricingTierName = "pricingTierName";
			const decimal pricePerMonth = 100;

			//arrange
			var signupModel = new UserSignupModel
			{
				CreditCardNumber = creditCardNumber,
				PricingTierId = pricingTierId,
				EmailAddress = email,
				FirstName = firstName,
				LastName = lastName
			};

			_standardPricingTierManager.Setup(x => x.GetStandardPricingTier(pricingTierId)).Returns(new StandardPricingTier
			{
				CallsPerMonth = callsPerMonth,
				Name = pricingTierName,
				PricePerMonth = pricePerMonth,
				StandardPricingTierId = pricingTierId
			});

			//act
			_webPageHelper.SignUp(signupModel);

			//assert
			_userManager.Verify(x => x.AddNewuser(It.Is<UserSignupModel>(m => 
					m.CreditCardNumber == creditCardNumber &&
					m.EmailAddress == email &&
					m.FirstName == firstName &&
					m.LastName == lastName &&
					m.PricingTierId == pricingTierId), 
				It.Is<StandardPricingTier>(m =>
					m.CallsPerMonth == callsPerMonth &&
					m.Name == pricingTierName &&
					m.PricePerMonth == pricePerMonth &&
					m.StandardPricingTierId == pricingTierId
			)));
		}
	}
}
