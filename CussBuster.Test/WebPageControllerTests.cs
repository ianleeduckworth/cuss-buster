using CussBuster.Controllers;
using CussBuster.Core.Data.Static;
using CussBuster.Core.Helpers;
using CussBuster.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Net;

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

			AssertWithMessage.IsOfType(result, typeof(OkResult));
			var response = result as OkResult;

			AssertWithMessage.AreEqual(response.StatusCode, (int)HttpStatusCode.OK);
		}

		[Test]
		public void Get_BadApiToken()
		{
			const string apiToken = "badGuid";

			//arrange / act
			var result = _webPageController.Get(apiToken);

			//assert
			AssertWithMessage.IsOfType(result, typeof(ObjectResult));
			var response = result as ObjectResult;

			AssertWithMessage.AreEqual(response.StatusCode, (int)HttpStatusCode.BadRequest);
			AssertWithMessage.AreEqual(response.Value.ToString(), $"Could not parse API token passed in into a GUID.  API token: {apiToken}", "exception message");
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
			var result = _webPageController.Get(apiToken.ToString());

			//assert
			AssertWithMessage.IsOfType(result, typeof(OkObjectResult));
			var response = result as OkObjectResult;

			AssertWithMessage.IsOfType(response.Value, typeof(UserReturnModel));
			var value = response.Value as UserReturnModel;

			AssertWithMessage.AreEqual(value.AccountLocked, accountLocked, nameof(value.AccountLocked));
			AssertWithMessage.AreEqual(value.AccountType, accountType, nameof(value.AccountType));
			AssertWithMessage.AreEqual(value.ApiToken, apiToken, nameof(value.ApiToken));
			AssertWithMessage.AreEqual(value.CallsPerMonth, callsPerMonth, nameof(value.CallsPerMonth));
			AssertWithMessage.AreEqual(value.Email, email, nameof(value.Email));
			AssertWithMessage.AreEqual(value.FirstName, firstName, nameof(value.FirstName));
			AssertWithMessage.AreEqual(value.LastName, lastName, nameof(value.LastName));
			AssertWithMessage.AreEqual(value.PricePerMonth, pricePerMonth, nameof(value.PricePerMonth));
		}

		[Test]
		public void Put_BadApiToken()
		{
			const string apiToken = "badGuid";
			const string password = "password";

			var updateModel = new UserUpdateModel
			{
				EmailAddress = "test@test.test"
			};

			//arrange / act
			var result = _webPageController.Put(apiToken, password, updateModel);

			//assert
			AssertWithMessage.IsOfType(result, typeof(ObjectResult));
			var response = result as ObjectResult;

			AssertWithMessage.AreEqual(response.StatusCode, (int)HttpStatusCode.BadRequest, "status code");
			AssertWithMessage.AreEqual(response.Value.ToString(), $"Could not parse API token passed in into a GUID.  API token: {apiToken}", "exception message");
		}

		[Test]
		public void Put_Null()
		{
			Guid apiToken = new Guid("35b22f54-965c-4811-837a-feaafa728ef3");
			const string password = "password";

			var updateModel = new UserUpdateModel
			{
				EmailAddress = "test@test.test"
			};

			//arrange
			_webPageHelper.Setup(x => x.UpdateUserInfo(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<UserUpdateModel>())).Returns(default(UserReturnModel));

			//act
			var result = _webPageController.Put(apiToken.ToString(), password, updateModel);

			//assert
			AssertWithMessage.IsOfType(result, typeof(NotFoundResult));
		}

		[Test]
		public void Put_Exception()
		{
			Guid apiToken = new Guid("35b22f54-965c-4811-837a-feaafa728ef3");
			const string password = "password";

			var updateModel = new UserUpdateModel
			{
				EmailAddress = "test@test.test"
			};

			const string  exceptionMessage = "exceptionMessage";

			//arrange
			_webPageHelper.Setup(x => x.UpdateUserInfo(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<UserUpdateModel>())).Throws(new Exception(exceptionMessage));

			//act
			var result = _webPageController.Put(apiToken.ToString(), password, updateModel);

			//assert
			AssertWithMessage.IsOfType(result, typeof(ObjectResult));
			var response = result as ObjectResult;

			AssertWithMessage.AreEqual(response.StatusCode, (int)HttpStatusCode.InternalServerError, "status code");
			AssertWithMessage.AreEqual(response.Value.ToString(), exceptionMessage, "exception message");
		}

		[Test]
		public void Put_Ok()
		{
			Guid apiToken = new Guid("35b22f54-965c-4811-837a-feaafa728ef3");
			const string creditCardNumber = "creditCardNumber";
			const string emailAddress = "test@test.test";
			const string firstName = "firstName";
			const string lastName = "lastName";
			const int pricingTierId = (byte)StaticData.StaticPricingTier.Standard;
			const string accountType = "Standard";
			const string password = "password";
			const bool racism = true;
			const bool sexism = true;
			const bool vulgarity = true;
			const bool accountLocked = false;
			const int callsPerMonth = 100;
			const int callsThisMonth = 50;
			const decimal pricePerMonth = 50;
			const byte racismSeverity = 4;
			const byte sexismSeverity = 5;
			const byte vulgaritySeverity = 6;

			var userReturnModel = new UserReturnModel
			{
				AccountLocked = accountLocked,
				AccountType = accountType,
				ApiToken = apiToken,
				CallsPerMonth = callsPerMonth,
				CallsThisMonth = callsThisMonth,
				CreditCardNumber = creditCardNumber,
				Email = emailAddress,
				FirstName = firstName,
				LastName = lastName,
				PricePerMonth = pricePerMonth,
				Racism = racism,
				RacismSeverity = racismSeverity,
				Sexism = sexism,
				SexismSeverity = sexismSeverity,
				Vulgarity = vulgarity,
				VulgaritySeverity = vulgaritySeverity,
			};

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
			_webPageHelper.Setup(x => x.UpdateUserInfo(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<UserUpdateModel>())).Returns(userReturnModel);

			//act
			var result = _webPageController.Put(apiToken.ToString(), password, userUpdateModel);

			//assert
			AssertWithMessage.IsOfType(result, typeof(OkObjectResult));
			var response = result as OkObjectResult;

			AssertWithMessage.IsOfType(response.Value, typeof(UserReturnModel));
			var value = response.Value as UserReturnModel;

			AssertWithMessage.AreEqual(value.AccountLocked, accountLocked, nameof(value.AccountLocked));
			AssertWithMessage.AreEqual(value.AccountType, accountType, nameof(value.AccountType));
			AssertWithMessage.AreEqual(value.ApiToken, apiToken, nameof(value.ApiToken));
			AssertWithMessage.AreEqual(value.CallsPerMonth, callsPerMonth, nameof(value.CallsPerMonth));
			AssertWithMessage.AreEqual(value.CallsThisMonth, callsThisMonth, nameof(value.CallsThisMonth));
			AssertWithMessage.AreEqual(value.CreditCardNumber, creditCardNumber, nameof(value.CreditCardNumber));
			AssertWithMessage.AreEqual(value.Email, emailAddress, nameof(value.Email));
			AssertWithMessage.AreEqual(value.FirstName, firstName, nameof(value.FirstName));
			AssertWithMessage.AreEqual(value.LastName, lastName, nameof(value.LastName));
			AssertWithMessage.AreEqual(value.PricePerMonth, pricePerMonth, nameof(value.PricePerMonth));
			AssertWithMessage.AreEqual(value.Racism, racism, nameof(value.Racism));
			AssertWithMessage.AreEqual(value.RacismSeverity, racismSeverity, nameof(value.RacismSeverity));
			AssertWithMessage.AreEqual(value.Sexism, sexism, nameof(value.Sexism));
			AssertWithMessage.AreEqual(value.SexismSeverity, sexismSeverity, nameof(value.SexismSeverity));
			AssertWithMessage.AreEqual(value.Vulgarity, vulgarity, nameof(value.Vulgarity));
			AssertWithMessage.AreEqual(value.VulgaritySeverity, vulgaritySeverity, nameof(value.VulgaritySeverity));
		}

		[Test]
		public void Post_BadPassword()
		{
			//arrange
			var model = new UserSignupModel
			{
				Password = "1234567"
			};

			//act
			var result = _webPageController.Post(model);

			//assert
			AssertWithMessage.IsOfType(result, typeof(ObjectResult));
			var response = result as ObjectResult;

			AssertWithMessage.AreEqual(response.StatusCode, (int)HttpStatusCode.NotAcceptable, "status code");
			AssertWithMessage.AreEqual(response.Value.ToString(), "Password must be at least 8 characters", "exception message");
		}

		[Test]
		public void Post_GoodPassword()
		{
			//arrange
			var model = new UserSignupModel
			{
				Password = "12345678"
			};

			//act
			var result = _webPageController.Post(model);

			//assert
			AssertWithMessage.IsOfType(result, typeof(OkObjectResult));
		}
	}
}
