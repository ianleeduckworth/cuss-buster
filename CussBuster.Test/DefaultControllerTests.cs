using CussBuster.Controllers;
using CussBuster.Core.Helpers;
using CussBuster.Core.Models;
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
		private Mock<IMainHelper> _mainHelper;

		[SetUp]
		public void SetUp()
		{
			_mainHelper = new Mock<IMainHelper>();

			_defaultController = new DefaultController(_mainHelper.Object);
		}

		[Test]
		public void Post_BadAuthentication()
		{
			const string authToken = "testAuthToken";

			//arrange
			_mainHelper.Setup(x => x.CheckAuthorization(authToken)).Returns(false);

			//act
			var result = _defaultController.Post("test text", authToken) as UnauthorizedResult;

			//assert
			Assert.True(result != null);
		}

		[Test]
		public void Post_BadRequest()
		{
			const string authToken = "testAuthToken";

			//arrange
			_mainHelper.Setup(x => x.CheckAuthorization(authToken)).Throws(new Exception("test exception"));

			//act
			var result = _defaultController.Post("test text", authToken) as BadRequestResult;

			//assert
			Assert.True(result != null);
		}

		[Test]
		public void Post_Ok()
		{
			const string authToken = "testAuthToken";
			const string text = "test text";

			//arrange
			_mainHelper.Setup(x => x.CheckAuthorization(authToken)).Returns(true);
			_mainHelper.Setup(x => x.FindMatches(text)).Returns(new List<ReturnModel>
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
    }
}
