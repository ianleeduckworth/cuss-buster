using CussBuster.Core.Data.Entities;
using CussBuster.Core.Data.Static;
using CussBuster.Core.DataAccess;
using CussBuster.Core.ExtensionMethods;
using CussBuster.Core.Models;
using CussBuster.Core.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CussBuster.Core.Helpers
{
	public class MainHelper : IMainHelper
    {
		private readonly IAuthChecker _authChecker;
		private readonly IAuditWriter _auditWriter;
		private readonly IAppSettings _appSettings;
		private readonly IBadWordCache _badWordCache;
		private readonly IUserManager _userManager;

		public MainHelper(IBadWordCache badWordCache, IAuthChecker authChecker, IAuditWriter auditWriter, IAppSettings appSettings, IUserManager userManager)
		{
			_authChecker = authChecker;
			_auditWriter = auditWriter;
			_appSettings = appSettings;
			_badWordCache = badWordCache;
			_userManager = userManager;
		}

		public IEnumerable<ReturnModel> FindMatches(string text, User user)
		{
			_auditWriter.LogUserCall(user);

			var matches = new List<ReturnModel>();

			foreach (var w in text.Split(" "))
			{
				var word = w.RemovePunctuationAndSymbols();

				var match = _badWordCache.Words.FirstOrDefault(x =>
				{
					switch (x.SearchTypeId)
					{
						case (byte)StaticData.SearchType.Equals:
							return x.Word.Equals(word, StringComparison.CurrentCultureIgnoreCase);
						case (byte)StaticData.SearchType.Contains:
							return new CultureInfo("en-US").CompareInfo.IndexOf(word, x.Word, CompareOptions.IgnoreCase) >= 0;
						default:
							throw new ArgumentException($"Could not find SearchType for value passed in: {x.SearchTypeId}");
					}
				});

				if (match != null)
				{
					_auditWriter.WriteToAudit(match.WordId);
					if (matches.Select(x => x.Word).Any(x => x.Equals(word, StringComparison.CurrentCultureIgnoreCase)))
					{
						var item = matches.Single(x => x.Word.Equals(word, StringComparison.CurrentCultureIgnoreCase));
						item.Occurrences += 1;
					}
					else
						matches.Add(new ReturnModel
						{
							Word = word.ToLower(),
							WordTypeId = match.WordTypeId,
							WordType = Enum.GetName(typeof(StaticData.WordType), match.WordTypeId),
							Severity = match.Severity,
							Occurrences = 1,
						});
				}
			}

			_userManager.CheckLockAccount(user);
			return matches;
		}

		public User CheckAuthorization(string authToken)
		{
			if (!Guid.TryParse(authToken, out Guid guidAuthToken))
				return null;

			return _authChecker.CheckToken(guidAuthToken);
		}

		public bool CheckCharacterLimit(string text)
		{
			if (text.Length > _appSettings.CharacterLimit)
				return false;

			return true;
		}

		public bool CheckUnlockAccount(User user)
		{
			var now = DateTime.UtcNow;
			var lastCall = _userManager.GetLastCallDate(user);
			if (lastCall.Month != now.Month)
			{
				_userManager.UnlockAccount(user);
				return true;
			}

			return false;
		}
	}
}
