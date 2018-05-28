using CusBuster.Core.DataAccess;
using CussBuster.Core.DataAccess;
using CussBuster.Core.Helpers;
using CussBuster.Core.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CussBuster.Test
{
	[TestFixture]
    public class MainHelperTests
    {
		private Mock<IWordLoader> _wordLoader;
		private Mock<IAuthChecker> _authChecker;
		private MainHelper _mainHelper;

		[SetUp]
		public void SetUp()
		{
			_wordLoader = new Mock<IWordLoader>();
			_authChecker = new Mock<IAuthChecker>();

			_mainHelper = new MainHelper(_wordLoader.Object, _authChecker.Object);
		}

		[Test]
		public void FindMatches_SingleMatch()
		{
			//arrange
			_wordLoader.Setup(x => x.Load()).Returns(new List<WordModel>
			{
				new WordModel
				{
					Word = "test",
					SearchTypeId = 1,
					Severity = 1,
					WordTypeId = 1
				}
			});

			//act
			var result = _mainHelper.FindMatches("this is a test");

			//assert
			var item = result.First();

			Assert.True(result.Count() == 1);
			Assert.True(item.Occurrences == 1);
			Assert.True(item.Severity == 1);
			Assert.True(item.Word == "test");
			Assert.True(item.WordType == "Vulgarity");
		}

		[Test]
		public void FindMatches_NoMatches()
		{
			//arrange
			_wordLoader.Setup(x => x.Load()).Returns(new List<WordModel>
			{
				new WordModel
				{
					Word = "test",
					SearchTypeId = 1,
					Severity = 1,
					WordTypeId = 1
				}
			});

			//act
			var result = _mainHelper.FindMatches("this is a foo");

			//assert
			Assert.True(result.Count() == 0);
		}
    }
}
