using CussBuster.Core.Data.Entities;
using CussBuster.Core.Data.Static;
using CussBuster.Core.DataAccess;
using CussBuster.Core.Helpers;
using CussBuster.Core.Models;
using CussBuster.Core.Settings;
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
		private Mock<IBadWordCache> _badWordCache;
		private Mock<IAuthChecker> _authChecker;
		private Mock<IAuditWriter> _auditWriter;
		private Mock<IAppSettings> _appSettings;
		private Mock<IUserManager> _userManager;
		private MainHelper _mainHelper;

		[SetUp]
		public void SetUp()
		{
			_badWordCache = new Mock<IBadWordCache>();
			_authChecker = new Mock<IAuthChecker>();
			_auditWriter = new Mock<IAuditWriter>();
			_appSettings = new Mock<IAppSettings>();
			_userManager = new Mock<IUserManager>();

			_mainHelper = new MainHelper(_badWordCache.Object, _authChecker.Object, _auditWriter.Object, _appSettings.Object, _userManager.Object);
		}

		[Test]
		public void FindMatches_SingleMatch()
		{
			const string word = "test";
			const byte searchTypeId = (byte)StaticData.SearchType.Equals;
			const byte severity = 2;
			const byte wordTypeId = (byte)StaticData.WordType.Vulgarity;

			//arrange
			_badWordCache.SetupGet(x => x.Words).Returns(new List<WordModel>
			{
				new WordModel
				{
					Word = word,
					SearchTypeId = searchTypeId,
					Severity = severity,
					WordTypeId = wordTypeId
				}
			});

			User user = new User();

			//act
			var result = _mainHelper.FindMatches($"this is a {word}", user);

			//assert
			AssertWithMessage.AreEqual(result.Count(), 1, "item count");
			var item = result.First();
			
			AssertWithMessage.AreEqual(item.Occurrences, 1, nameof(item.Occurrences));
			AssertWithMessage.AreEqual(item.Word, word, nameof(item.Word));
			AssertWithMessage.AreEqual(item.Severity, severity, nameof(item.Severity));
			AssertWithMessage.AreEqual(item.WordTypeId, wordTypeId, nameof(item.WordTypeId));
		}

		[Test]
		public void FindMatches_SingleMatch_CaseMismatch()
		{
			const string word = "test";
			const byte searchTypeId = (byte)StaticData.SearchType.Equals;
			const byte severity = 2;
			const byte wordTypeId = (byte)StaticData.WordType.Vulgarity;

			//arrange
			_badWordCache.SetupGet(x => x.Words).Returns(new List<WordModel>
			{
				new WordModel
				{
					Word = word,
					SearchTypeId = searchTypeId,
					Severity = severity,
					WordTypeId = wordTypeId
				}
			});

			User user = new User();

			//act
			var result = _mainHelper.FindMatches($"this is a {word.ToUpper()}", user);

			//assert
			AssertWithMessage.AreEqual(result.Count(), 1, "item count");
			var item = result.First();

			AssertWithMessage.AreEqual(item.Occurrences, 1, nameof(item.Occurrences));
			AssertWithMessage.AreEqual(item.Word, word, nameof(item.Word));
			AssertWithMessage.AreEqual(item.Severity, severity, nameof(item.Severity));
			AssertWithMessage.AreEqual(item.WordTypeId, wordTypeId, nameof(item.WordTypeId));
		}

		[Test]
		public void FindMatches_SingleMatch_EqualsSearch_Negative()
		{
			const string word = "test";
			const byte searchTypeId = (byte)StaticData.SearchType.Equals;
			const byte severity = 2;
			const byte wordTypeId = (byte)StaticData.WordType.Vulgarity;

			//arrange
			_badWordCache.SetupGet(x => x.Words).Returns(new List<WordModel>
			{
				new WordModel
				{
					Word = word,
					SearchTypeId = searchTypeId,
					Severity = severity,
					WordTypeId = wordTypeId
				}
			});

			User user = new User();

			//act
			var result = _mainHelper.FindMatches($"we are {word}ing", user);

			//assert
			AssertWithMessage.AreEqual(result.Count(), 0, "item count");
		}

		[Test]
		public void FindMatches_SingleMatch_ContainsSearch()
		{
			const string word = "test";
			const byte searchTypeId = (byte)StaticData.SearchType.Contains;
			const byte severity = 2;
			const byte wordTypeId = (byte)StaticData.WordType.Vulgarity;

			//arrange
			_badWordCache.SetupGet(x => x.Words).Returns(new List<WordModel>
			{
				new WordModel
				{
					Word = word,
					SearchTypeId = searchTypeId,
					Severity = severity,
					WordTypeId = wordTypeId
				}
			});

			User user = new User();

			//act
			var result = _mainHelper.FindMatches($"we are {word}ing", user);

			//assert
			AssertWithMessage.AreEqual(result.Count(), 1, "item count");
			var item = result.First();

			AssertWithMessage.AreEqual(item.Occurrences, 1, nameof(item.Occurrences));
			AssertWithMessage.AreEqual(item.Word, $"{word}ing", nameof(item.Word));
			AssertWithMessage.AreEqual(item.Severity, severity, nameof(item.Severity));
			AssertWithMessage.AreEqual(item.WordTypeId, wordTypeId, nameof(item.WordTypeId));
		}

		[Test]
		public void FindMatches_SingleMatch_MultipleOccurances()
		{
			const string word = "test";
			const byte searchTypeId = (byte)StaticData.SearchType.Equals;
			const byte severity = 2;
			const byte wordTypeId = (byte)StaticData.WordType.Vulgarity;

			//arrange
			_badWordCache.SetupGet(x => x.Words).Returns(new List<WordModel>
			{
				new WordModel
				{
					Word = word,
					SearchTypeId = searchTypeId,
					Severity = severity,
					WordTypeId = wordTypeId
				}
			});

			User user = new User();

			//act
			var result = _mainHelper.FindMatches($"this is a {word} {word}", user);

			//assert
			AssertWithMessage.AreEqual(result.Count(), 1, "item count");
			var item = result.First();
			
			AssertWithMessage.AreEqual(item.Occurrences, 2, nameof(item.Occurrences));
			AssertWithMessage.AreEqual(item.Word, word, nameof(item.Word));
			AssertWithMessage.AreEqual(item.Severity, severity, nameof(item.Severity));
			AssertWithMessage.AreEqual(item.WordTypeId, wordTypeId, nameof(item.WordTypeId));
		}

		[Test]
		public void FindMatches_NoMatches()
		{
			const string word = "test";
			const string searchWord = "foo";
			const byte searchTypeId = (byte)StaticData.SearchType.Equals;
			const byte severity = 2;
			const byte wordTypeId = (byte)StaticData.WordType.Vulgarity;

			//arrange
			_badWordCache.SetupGet(x => x.Words).Returns(new List<WordModel>
			{
				new WordModel
				{
					Word = word,
					SearchTypeId = searchTypeId,
					Severity = severity,
					WordTypeId = wordTypeId
				}
			});

			User user = new User();

			//act
			var result = _mainHelper.FindMatches($"this is a {searchWord}", user);

			//assert
			AssertWithMessage.AreEqual(result.Count(), 0, "number of matches");
		}

		[Test]
		public void CheckAuthorization_NonGuid()
		{
			//arrange / act
			var result = _mainHelper.CheckAuthorization("test");
			
			//assert
			AssertWithMessage.IsNull(result, "user");
		}

		[Test]
		public void CheckAuthorization_NonExistantGuid()
		{
			var guidString = "ab9767dd-3c56-4750-a669-76564e057f83";
			var guid = new Guid(guidString);

			//arrange
			_authChecker.Setup(x => x.CheckToken(guid)).Returns(default(User));

			//act
			var result = _mainHelper.CheckAuthorization(guidString);

			//assert
			AssertWithMessage.IsNull(result, "user");
		}

		[Test]
		public void CheckAuthorization_ExistingGuid()
		{
			const string guidString = "ab9767dd-3c56-4750-a669-76564e057f83";
			const int userId = 1;

			Guid guid = new Guid(guidString);
			User user = new User
			{
				UserId = userId,
			};

			//arrange
			_authChecker.Setup(x => x.CheckToken(guid)).Returns(user);

			//act
			var result = _mainHelper.CheckAuthorization(guidString);

			//assert
			AssertWithMessage.AreEqual(result.UserId, userId, nameof(result.UserId));
		}

		[Test]
		public void CheckCharacterLimit_UnderLimit()
		{
			const int characterLimit = 5;
			var word = string.Empty;

			for (int i = 0; i < characterLimit - 1; i++)
				word += "a";

			//arrange
			_appSettings.Setup(x => x.CharacterLimit).Returns(characterLimit);

			//act
			var result = _mainHelper.CheckCharacterLimit(word);

			//assert
			AssertWithMessage.AreEqual(result, true, "character limit");
		}

		[Test]
		public void CheckCharacterLimit_AtLimit()
		{
			const int characterLimit = 5;
			var word = string.Empty;

			for (int i = 0; i < characterLimit; i++)
				word += "a";

			//arrange
			_appSettings.Setup(x => x.CharacterLimit).Returns(characterLimit);

			//act
			var result = _mainHelper.CheckCharacterLimit(word);

			//assert
			AssertWithMessage.AreEqual(result, true, "character limit");
		}

		[Test]
		public void CheckCharacterLimit_OverLimit()
		{
			const int characterLimit = 5;
			var word = string.Empty;

			for (int i = 0; i < characterLimit + 1; i++)
				word += "a";

			//arrange
			_appSettings.Setup(x => x.CharacterLimit).Returns(characterLimit);

			//act
			var result = _mainHelper.CheckCharacterLimit(word);

			//assert
			AssertWithMessage.AreEqual(result, false, "character limit");
		}

		[Test]
		public void FindMatches_VerifyAuditCalled()
		{
			//arrange
			var user = new User();

			//act
			_mainHelper.FindMatches("test text", user);

			//assert
			_auditWriter.Verify(x => x.LogUserCall(user), $"{nameof(_auditWriter.Verify)} was not called");
		}
	}
}