using CusBuster.Core.DataAccess;
using CussBuster.Core.DataAccess;
using CussBuster.Core.Helpers;
using CussBuster.Core.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CussBuster.Test
{
	[TestFixture]
    public class MainHelperTests
    {
		private Mock<IWordLoader> _wordLoader;
		private Mock<IAuthChecker> _authChecker;
		private Mock<IAuditWriter> _auditWriter;
		private MainHelper _mainHelper;

		[SetUp]
		public void SetUp()
		{
			_wordLoader = new Mock<IWordLoader>();
			_authChecker = new Mock<IAuthChecker>();
			_auditWriter = new Mock<IAuditWriter>();

			_mainHelper = new MainHelper(_wordLoader.Object, _authChecker.Object, _auditWriter.Object);
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
		public void FindMatches_SingleMatch_CaseMismatch()
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
			var result = _mainHelper.FindMatches("this is a TEST");

			//assert
			var item = result.First();

			Assert.True(result.Count() == 1);
			Assert.True(item.Occurrences == 1);
			Assert.True(item.Severity == 1);
			Assert.True(item.Word == "test");
			Assert.True(item.WordType == "Vulgarity");
		}

		[Test]
		public void FindMatches_SingleMatch_EqualsSearch_Negative()
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
			var result = _mainHelper.FindMatches("we are testing");

			//assert
			Assert.True(result.Count() == 0);
		}

		[Test]
		public void FindMatches_SingleMatch_ContainsSearch()
		{
			//arrange
			_wordLoader.Setup(x => x.Load()).Returns(new List<WordModel>
			{
				new WordModel
				{
					Word = "test",
					SearchTypeId = 2,
					Severity = 1,
					WordTypeId = 1
				}
			});

			//act
			var result = _mainHelper.FindMatches("we are testing");

			//assert
			var item = result.First();

			Assert.True(result.Count() == 1);
			Assert.True(item.Occurrences == 1);
			Assert.True(item.Severity == 1);
			Assert.True(item.Word == "testing");
			Assert.True(item.WordType == "Vulgarity");
		}

		[Test]
		public void FindMatches_SingleMatch_MultipleOccurances()
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
			var result = _mainHelper.FindMatches("this is a test test");

			//assert
			var item = result.First();

			Assert.True(result.Count() == 1);
			Assert.True(item.Occurrences == 2);
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

		[Test]
		public void CheckAuthorization_NonGuid()
		{
			//arrange / act
			var result = _mainHelper.CheckAuthorization("test");

			//assert
			Assert.True(result == false);
		}

		[Test]
		public void CheckAuthorization_NonExistantGuid()
		{
			var guidString = "ab9767dd-3c56-4750-a669-76564e057f83";
			var guid = new Guid(guidString);

			//arrange
			_authChecker.Setup(x => x.CheckToken(guid)).Returns(false);

			//act
			var result = _mainHelper.CheckAuthorization(guidString);

			//assert
			Assert.True(result == false);
		}

		[Test]
		public void CheckAuthorization_ExistingGuid()
		{
			var guidString = "ab9767dd-3c56-4750-a669-76564e057f83";
			var guid = new Guid(guidString);

			//arrange
			_authChecker.Setup(x => x.CheckToken(guid)).Returns(true);

			//act
			var result = _mainHelper.CheckAuthorization(guidString);

			//assert
			Assert.True(result == true);
		}
	}
}
