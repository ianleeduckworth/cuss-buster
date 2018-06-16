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
    }
}
