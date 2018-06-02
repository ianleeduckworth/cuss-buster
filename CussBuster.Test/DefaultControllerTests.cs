using CussBuster.Controllers;
using CussBuster.Core.Data.Entities;
using CussBuster.Core.Helpers;
using CussBuster.Core.Models;
using CussBuster.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CussBuster.Test
{
	[TestFixture]
    public class DefaultControllerTests
    {
		private DefaultController _defaultController;
		private Mock<IAppSettings> _appSettings;
		private Mock<IMainHelper> _mainHelper;

		[SetUp]
		public void SetUp()
		{
			_mainHelper = new Mock<IMainHelper>();
			_appSettings = new Mock<IAppSettings>();

			_defaultController = new DefaultController(_mainHelper.Object, _appSettings.Object);
		}

		[Test]
		public void Post_BadAuthentication()
		{
			const string authToken = "testAuthToken";
			const string text = "test text";

			//arrange
			_mainHelper.Setup(x => x.CheckAuthorization(authToken)).Returns(default(User));
			_mainHelper.Setup(x => x.CheckCharacterLimit(text)).Returns(true);

			//act
			var result = _defaultController.Post(text, authToken) as UnauthorizedResult;

			//assert
			Assert.True(result != null);
		}

		[Test]
		public void Post_BadRequest()
		{
			const string authToken = "testAuthToken";
			const string text = "test text";

			//arrange
			_mainHelper.Setup(x => x.CheckAuthorization(authToken)).Throws(new Exception("test exception"));
			_mainHelper.Setup(x => x.CheckCharacterLimit(text)).Returns(true);

			//act
			var result = _defaultController.Post(text, authToken) as BadRequestResult;

			//assert
			Assert.True(result != null);
		}

		[Test]
		public void Post_Ok()
		{
			const string authToken = "testAuthToken";
			const string text = "test text";
			User user = new User
			{
				CanCallApi = true
			};

			//arrange
			_mainHelper.Setup(x => x.CheckAuthorization(authToken)).Returns(user);
			_mainHelper.Setup(x => x.CheckCharacterLimit(text)).Returns(true);
			_mainHelper.Setup(x => x.FindMatches(text, user)).Returns(new List<ReturnModel>
			{
				new ReturnModel
				{
					Occurrences = 1,
					Severity = 1,
					Word = "text",
					WordType = "Vulgarity",
					WordTypeId = 1
				}
			});

			//act
			var result = _defaultController.Post(text, authToken) as OkObjectResult;

			//assert
			Assert.True(result != null);

			var value = (result.Value as IEnumerable<ReturnModel>).First();

			Assert.True(value.Occurrences == 1);
			Assert.True(value.Severity == 1);
			Assert.True(value.Word == "text");
			Assert.True(value.WordType == "Vulgarity");
			Assert.True(value.WordTypeId == 1);
		}

		[Test]
		public void Get_Default()
		{
			//arrange /act
			var result = _defaultController.Default() as OkResult;

			//assert
			Assert.True(result != null);
		}

		[Test]
		public void Post_OverCharacterLimit()
		{
			const string text = "test text";
			const int characterLimit = 5;

			//arrange
			_mainHelper.Setup(x => x.CheckCharacterLimit(text)).Returns(false);
			_appSettings.SetupGet(x => x.CharacterLimit).Returns(characterLimit);

			//act
			var result = _defaultController.Post(text, string.Empty) as BadRequestObjectResult;

			//assert
			Assert.True(result != null);
			Assert.True(result.Value.ToString() == $"Text passed in is longer than the {characterLimit} character limit.  Text length: {text.Length}.");
		}

		[Test]
		public void Post_CannotCallApi()
		{
			const string text = "test text";
			const string authToken = "testAuthToken";

			//arrange
			_mainHelper.Setup(x => x.CheckCharacterLimit(text)).Returns(true);
			_mainHelper.Setup(x => x.CheckAuthorization(authToken)).Returns(new User
			{
				CanCallApi = false
			});

			//act
			var result = _defaultController.Post(text, authToken) as BadRequestObjectResult;

			//assert
			Assert.True(result != null);
			Assert.True(result.Value.ToString() == "You have reached your call limit for the month.  Please contact support for more information");
		}
    }
}
