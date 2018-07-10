using CussBuster.Controllers;
using CussBuster.Core.Helpers;
using CussBuster.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;

namespace CussBuster.Test
{
	public class WebPageControllerTests
    {
		private WebPageController _webPageController;
		private Mock<IWebPageHelper> _webPageHelper;

		[SetUp]
		public void SetUp()
		{
			_webPageHelper = new Mock<IWebPageHelper>();
			_webPageController = new WebPageController(_webPageHelper.Object);
		}

		[Test]
		public void Options_Ok()
		{
			//arrange / act
			var result = _webPageController.Options();

			//assert
			Assert.True(result is OkResult);
		}

		[Test]
		public void Get_BadApiToken()
		{
			const string apiToken = "badGuid";

			//arrange / act
			var returnVal = _webPageController.Get(apiToken);

			//assert
			Assert.True(returnVal is ObjectResult);
			var result = returnVal as ObjectResult;

			Assert.True(result.StatusCode == 500);
			Assert.True(result.Value.ToString() == $"Could not parse API token passed in into a GUID.  API token: {apiToken}");
		}

		[Test]
		public void Get_Ok()
		{
			const bool accountLocked = false;
			const string accountType = "accountType";
			Guid apiToken = new Guid("35b22f54-965c-4811-837a-feaafa728ef3");
			const int callsPerMonth = 100;
			const string email = "email";
			const string firstName = "firstName";
			const string lastName = "lastName";
			const decimal pricePerMonth = 150;

			//arrange
			_webPageHelper.Setup(x => x.GetUserInfo(apiToken)).Returns(new UserReturnModel
			{
				AccountLocked = accountLocked,
				AccountType = accountType,
				ApiToken = apiToken,
				CallsPerMonth = callsPerMonth,
				Email = email,
				FirstName = firstName,
				LastName = lastName,
				PricePerMonth = pricePerMonth
			});

			//act
			var result = _webPageController.Get(apiToken.ToString()) as OkObjectResult;

			//assert
			Assert.True(result != null);
			var value = result.Value as UserReturnModel;

			Assert.True(value.AccountLocked == accountLocked);
			Assert.True(value.AccountType == accountType);
			Assert.True(value.ApiToken == apiToken);
			Assert.True(value.CallsPerMonth == callsPerMonth);
			Assert.True(value.Email == email);
			Assert.True(value.FirstName == firstName);
			Assert.True(value.LastName == lastName);
			Assert.True(value.PricePerMonth == pricePerMonth);
		}

		[Test]
		public void Put_BadApiToken()
		{
			const string apiToken = "badGuid";

			var updateModel = new UserUpdateModel
			{
				EmailAddress = "test@test.test"
			};

			//arrange / act
			var returnVal = _webPageController.Put(apiToken, updateModel);

			//assert
			Assert.True(returnVal is ObjectResult);
			var result = returnVal as ObjectResult;

			Assert.True(result.StatusCode == 500);
			Assert.True(result.Value.ToString() == $"Could not parse API token passed in into a GUID.  API token: {apiToken}");
		}

		[Test]
		public void Put_Null()
		{
			Guid apiToken = new Guid("35b22f54-965c-4811-837a-feaafa728ef3");

			var updateModel = new UserUpdateModel
			{
				EmailAddress = "test@test.test"
			};

			//arrange
			_webPageHelper.Setup(x => x.UpdateUserInfo(It.IsAny<Guid>(), It.IsAny<UserUpdateModel>())).Returns(default(UserUpdateModel));

			//act
			var result = _webPageController.Put(apiToken.ToString(), updateModel);

			//assert
			Assert.True(result is NotFoundResult);
		}

		[Test]
		public void Put_Exception()
		{
			Guid apiToken = new Guid("35b22f54-965c-4811-837a-feaafa728ef3");

			var updateModel = new UserUpdateModel
			{
				EmailAddress = "test@test.test"
			};

			const string  exceptionMessage = "exceptionMessage";

			//arrange
			_webPageHelper.Setup(x => x.UpdateUserInfo(apiToken, updateModel)).Throws(new Exception(exceptionMessage));

			//act
			var returnVal = _webPageController.Put(apiToken.ToString(), updateModel);

			//assert
			Assert.True(returnVal is ObjectResult);
			var result = returnVal as ObjectResult;

			Assert.True(result.StatusCode == 500);
			Assert.True(result.Value.ToString() == exceptionMessage);
		}

		[Test]
		public void Put_Ok()
		{
			Guid apiToken = new Guid("35b22f54-965c-4811-837a-feaafa728ef3");
			const string creditCardNumber = "creditCardNumber";
			const string emailAddress = "test@test.test";
			const string firstName = "firstName";
			const string lastName = "lastName";
			const int pricingTierId = 1;
			const bool racism = true;
			const bool sexism = true;
			const bool vulgarity = true;

			var userUpdateModel = new UserUpdateModel
			{
				CreditCardNumber = creditCardNumber,
				EmailAddress = emailAddress,
				FirstName = firstName,
				LastName = lastName,
				PricingTierId = pricingTierId,
				Racism = racism,
				Sexism = sexism,
				Vulgarity = vulgarity
			};

			//arrange
			_webPageHelper.Setup(x => x.UpdateUserInfo(It.IsAny<Guid>(), It.IsAny<UserUpdateModel>())).Returns(userUpdateModel);

			//act
			var returnVal = _webPageController.Put(apiToken.ToString(), userUpdateModel);

			//assert
			Assert.True(returnVal is OkObjectResult);
			var result = returnVal as OkObjectResult;
			var value = (UserUpdateModel)result.Value;

			Assert.True(value.CreditCardNumber == creditCardNumber);
			Assert.True(value.EmailAddress == emailAddress);
			Assert.True(value.FirstName == firstName);
			Assert.True(value.LastName == lastName);
			Assert.True(value.PricingTierId == pricingTierId);
			Assert.True(value.Racism == racism);
			Assert.True(value.Sexism == sexism);
			Assert.True(value.Vulgarity == vulgarity);
		}
    }
}
