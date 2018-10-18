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
			const int racismSeverity = 5;
			const bool sexism = true;
			const int sexismSeverity = 6;
			const bool vulgarity = true;
			const int vulgaritySeverity = 7;
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
			Assert.True(result is OkObjectResult);

			var response = result as OkObjectResult;

			Assert.True(response.Value is UserReturnModel);

			var returnModel = response.Value as UserReturnModel;

			Assert.True(returnModel.AccountLocked == accountLocked);
			Assert.True(returnModel.AccountType == accountType);
			Assert.True(returnModel.ApiToken == apiToken);
			Assert.True(returnModel.CallsPerMonth == callsPerMonth);
			Assert.True(returnModel.CallsThisMonth == callsThisMonth);
			Assert.True(returnModel.CreditCardNumber == creditCardNumber);
			Assert.True(returnModel.Email == email);
			Assert.True(returnModel.FirstName == firstName);
			Assert.True(returnModel.LastName == lastName);
			Assert.True(returnModel.PricePerMonth == pricePerMonth);
			Assert.True(returnModel.Racism == racism);
			Assert.True(returnModel.RacismSeverity == racismSeverity);
			Assert.True(returnModel.Sexism == sexism);
			Assert.True(returnModel.SexismSeverity == sexismSeverity);
			Assert.True(returnModel.Vulgarity == vulgarity);
			Assert.True(returnModel.VulgaritySeverity == vulgaritySeverity);
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
			Assert.True(result is StatusCodeResult);

			var response = result as StatusCodeResult;

			Assert.True(response.StatusCode == (byte)HttpStatusCode.NoContent);
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
			Assert.True(result is ObjectResult);

			var response = result as ObjectResult;

			Assert.True(response.StatusCode == (int)HttpStatusCode.Unauthorized);
			Assert.True(response.Value.ToString() == $"Unauthorized access occurred.  Email: {email}");
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
			Assert.True(result is ObjectResult);

			var response = result as ObjectResult;

			Assert.True(response.StatusCode == (int)HttpStatusCode.InternalServerError);
			Assert.True(response.Value.ToString() == $"Unhandled exception occurred while attempting to sign in: {exceptionText}");
		}

		[Test]
		public void Options_Ok()
		{
			//arrange / act
			var result = _signinController.Options();

			Assert.True(result is OkResult);
			Assert.True((result as OkResult).StatusCode == (int)HttpStatusCode.OK);
		}
	}
}
