using CussBuster.Controllers;
using CussBuster.Core.Exceptions;
using CussBuster.Core.Helpers;
using CussBuster.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Net;

namespace CussBuster.Test
{
	[TestFixture]
    public class SigninControllerTests
    {
		private Mock<ISigninHelper> _signinHelper;
		private SigninController _signinController;

		[SetUp]
		public void SetUp()
		{
			_signinHelper = new Mock<ISigninHelper>();
			_signinController = new SigninController(_signinHelper.Object);
		}

		[Test]
		public void Post_Ok()
		{
			const string email = "email";
			const string password = "password";
			bool accountLocked = true;
			string accountType = "accountType";
			Guid apiToken = new Guid("650d3ed8-1c78-4cbe-90ee-f0490e267923");
			const string creditCardNumber = "creditCardNumber";
			const string firstName = "firstName";
			const string lastName = "lastName";
			const decimal pricePerMonth = 23.45m;
			const bool racism = true;
			const byte racismSeverity = 5;
			const bool sexism = true;
			const byte sexismSeverity = 6;
			const bool vulgarity = true;
			const byte vulgaritySeverity = 7;
			int callsPerMonth = 77;
			int callsThisMonth = 88;

			UserReturnModel returnValue = new UserReturnModel
			{
				AccountLocked = accountLocked,
				AccountType = accountType,
				ApiToken = apiToken,
				CallsPerMonth = callsPerMonth,
				CallsThisMonth = callsThisMonth,
				CreditCardNumber = creditCardNumber,
				Email = email,
				FirstName = firstName,
				LastName = lastName,
				PricePerMonth = pricePerMonth,
				Racism = racism,
				RacismSeverity = racismSeverity,
				Sexism = sexism,
				SexismSeverity = sexismSeverity,
				Vulgarity = vulgarity,
				VulgaritySeverity = vulgaritySeverity
			};

			//arrange
			var model = new SigninModel
			{
				EmailAddress = email,
				Password = password
			};

			_signinHelper.Setup(x => x.Signin(email, password)).Returns(returnValue);

			//act
			var result = _signinController.Post(model);

			//assert
			AssertWithMessage.IsOfType(result, typeof(OkObjectResult));

			var response = result as OkObjectResult;

			AssertWithMessage.IsOfType(response.Value, typeof(UserReturnModel));

			var returnModel = response.Value as UserReturnModel;

			AssertWithMessage.AreEqual(returnModel.AccountLocked, accountLocked, nameof(returnModel.AccountLocked));
			AssertWithMessage.AreEqual(returnModel.AccountType, accountType, nameof(returnModel.AccountType));
			AssertWithMessage.AreEqual(returnModel.ApiToken, apiToken, nameof(returnModel.ApiToken));
			AssertWithMessage.AreEqual(returnModel.CallsPerMonth, callsPerMonth, nameof(returnModel.CallsPerMonth));
			AssertWithMessage.AreEqual(returnModel.CallsThisMonth, callsThisMonth, nameof(returnModel.CallsThisMonth));
			AssertWithMessage.AreEqual(returnModel.CreditCardNumber, creditCardNumber, nameof(returnModel.CreditCardNumber));
			AssertWithMessage.AreEqual(returnModel.Email, email, nameof(returnModel.Email));
			AssertWithMessage.AreEqual(returnModel.FirstName, firstName, nameof(returnModel.FirstName));
			AssertWithMessage.AreEqual(returnModel.LastName, lastName, nameof(returnModel.LastName));
			AssertWithMessage.AreEqual(returnModel.PricePerMonth, pricePerMonth, nameof(returnModel.PricePerMonth));
			AssertWithMessage.AreEqual(returnModel.Racism, racism, nameof(returnModel.Racism));
			AssertWithMessage.AreEqual(returnModel.RacismSeverity, racismSeverity, nameof(returnModel.RacismSeverity));
			AssertWithMessage.AreEqual(returnModel.Sexism, sexism, nameof(returnModel.Sexism));
			AssertWithMessage.AreEqual(returnModel.SexismSeverity, sexismSeverity, nameof(returnModel.SexismSeverity));
			AssertWithMessage.AreEqual(returnModel.Vulgarity, vulgarity, nameof(returnModel.Vulgarity));
			AssertWithMessage.AreEqual(returnModel.VulgaritySeverity, vulgaritySeverity, nameof(returnModel.VulgaritySeverity));
		}

		[Test]
		public void Post_UserNotFoundException()
		{
			const string email = "email";
			const string password = "password";
			const string exceptionText = "exceptionText";

			//arrange
			var model = new SigninModel
			{
				EmailAddress = email,
				Password = password
			};

			_signinHelper.Setup(x => x.Signin(email, password)).Throws(new UserNotFoundException(exceptionText));

			//act
			var result = _signinController.Post(model);

			//assert
			AssertWithMessage.IsOfType(result, typeof(BadRequestObjectResult));
		}

		[Test]
		public void Post_UnauthorizedAccessException()
		{
			const string email = "email";
			const string password = "password";
			const string exceptionText = "exceptionText";

			//arrange
			var model = new SigninModel
			{
				EmailAddress = email,
				Password = password
			};

			_signinHelper.Setup(x => x.Signin(email, password)).Throws(new UnauthorizedAccessException(exceptionText));

			//act
			var result = _signinController.Post(model);

			//assert
			AssertWithMessage.IsOfType(result, typeof(ObjectResult));

			var response = result as ObjectResult;

			AssertWithMessage.AreEqual(response.StatusCode, (int)HttpStatusCode.Unauthorized, "status code");
			AssertWithMessage.AreEqual(response.Value.ToString(), exceptionText, "exception message");
		}

		[Test]
		public void Post_Exception()
		{
			const string email = "email";
			const string password = "password";
			const string exceptionText = "exceptionText";

			//arrange
			var model = new SigninModel
			{
				EmailAddress = email,
				Password = password
			};

			_signinHelper.Setup(x => x.Signin(email, password)).Throws(new Exception(exceptionText));

			//act
			var result = _signinController.Post(model);

			//assert
			AssertWithMessage.IsOfType(result, typeof(ObjectResult));

			var response = result as ObjectResult;

			AssertWithMessage.AreEqual(response.StatusCode, (int)HttpStatusCode.InternalServerError, "status code");
			AssertWithMessage.AreEqual(response.Value.ToString(), $"Unhandled exception occurred while attempting to sign in: {exceptionText}", "exception message");
		}
	}
}
