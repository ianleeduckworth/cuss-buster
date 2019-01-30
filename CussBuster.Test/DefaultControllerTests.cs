using CussBuster.Controllers;
using CussBuster.Core.Data.Entities;
using CussBuster.Core.DataAccess;
using CussBuster.Core.Helpers;
using CussBuster.Core.Models;
using CussBuster.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Language.Flow;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CussBuster.Test
{
	[TestFixture]
    public class DefaultControllerTests
    {
		private DefaultController _defaultController;
		private Mock<IAppSettings> _appSettings;
		private Mock<IMainHelper> _mainHelper;
		private Mock<IUserManager> _userManager;

		[SetUp]
		public void SetUp()
		{
			_mainHelper = new Mock<IMainHelper>();
			_appSettings = new Mock<IAppSettings>();
			_userManager = new Mock<IUserManager>();

			_defaultController = new DefaultController(_mainHelper.Object, _appSettings.Object, _userManager.Object);
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
			var result = _defaultController.Post(text, authToken);

			//assert
			AssertWithMessage.IsOfType(result, typeof(UnauthorizedResult));
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
			var result = _defaultController.Post(text, authToken);

			//assert
			AssertWithMessage.IsOfType(result, typeof(BadRequestResult));
		}

		[Test]
		public void Post_Ok()
		{
			const string authToken = "testAuthToken";
			const string text = "text";
			const int occurances = 1;
			const byte severity = 2;
			const string word = "word";
			const string wordType = "wordType";
			const byte wordTypeId = 3;


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
					Occurrences = occurances,
					Severity = severity,
					Word = word,
					WordType = wordType,
					WordTypeId = wordTypeId
				}
			});

			//act
			var result = _defaultController.Post(text, authToken);

			//assert
			AssertWithMessage.IsOfType(result, typeof(OkObjectResult));
			var response = result as OkObjectResult;

			AssertWithMessage.IsOfType(response.Value, typeof(List<ReturnModel>));
			var list = response.Value as List<ReturnModel>;

			AssertWithMessage.AreEqual(list.Count, 1, "number of items in return model");

			var value = ((result as OkObjectResult).Value as List<ReturnModel>).First();

			AssertWithMessage.AreEqual(value.Occurrences, occurances, nameof(value.Occurrences));
			AssertWithMessage.AreEqual(value.Severity, severity, nameof(value.Severity));
			AssertWithMessage.AreEqual(value.Word, word, nameof(value.Word));
			AssertWithMessage.AreEqual(value.WordType, wordType, nameof(value.WordType));
			AssertWithMessage.AreEqual(value.WordTypeId, wordTypeId, nameof(value.WordTypeId));
		}

		[Test]
		public void Get_Default()
		{
			//arrange /act
			var result = _defaultController.Default();

			//assert
			AssertWithMessage.IsOfType(result, typeof(OkResult));
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
			var result = _defaultController.Post(text, string.Empty);
			AssertWithMessage.IsOfType(result, typeof(BadRequestObjectResult));

			var response = result as BadRequestObjectResult;

			//assert
			AssertWithMessage.AreEqual(response.Value.ToString(), $"Text passed in is longer than the {characterLimit} character limit.  Text length: {text.Length}.", "response value");
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
			_userManager.Setup(x => x.CheckUnlockAccount(It.IsAny<User>())).Returns(false);

			//act
			var result = _defaultController.Post(text, authToken);

			//assert
			AssertWithMessage.IsOfType(result, typeof(ObjectResult));

			var response = result as ObjectResult;

			AssertWithMessage.AreEqual(response.StatusCode, (int)HttpStatusCode.PaymentRequired, "status code");
			AssertWithMessage.AreEqual(response.Value.ToString(), "You have reached your call limit for the month.  Please contact support for more information", "response value");
		}

		[Test]
		public void Post_CheckAccountLock()
		{
			const string text = "test text";
			const string authToken = "testAuthToken";

			var user = new User
			{
				CanCallApi = true
			};

			//arrange
			_mainHelper.Setup(x => x.CheckCharacterLimit(text)).Returns(true);
			_mainHelper.Setup(x => x.CheckAuthorization(authToken)).Returns(user);
			_userManager.Setup(x => x.CheckLockAccount(It.IsAny<User>())).Callback(() => user.CanCallApi = true);
			_userManager.Setup(x => x.CheckUnlockAccount(It.IsAny<User>())).Returns(false);

			//act
			var result = _defaultController.Post(text, authToken);

			//assert
			AssertWithMessage.IsOfType(result, typeof(OkObjectResult));
		}

		[Test]
		public void Post_FirstOfMonth()
		{
			const string text = "test text";
			const string authToken = "testAuthToken";

			var user = new User
			{
				CanCallApi = true
			};

			//arrange
			_mainHelper.Setup(x => x.CheckCharacterLimit(text)).Returns(true);
			_mainHelper.Setup(x => x.CheckAuthorization(authToken)).Returns(user);
			_userManager.Setup(x => x.CheckLockAccount(It.IsAny<User>())).Callback(() => user.CanCallApi = false);
			_userManager.Setup(x => x.CheckUnlockAccount(It.IsAny<User>())).Returns(true);

			//act
			var result = _defaultController.Post(text, authToken);

			//assert
			AssertWithMessage.IsOfType(result, typeof(OkObjectResult));
		}
	}
}
