using CusBuster.Core.DataAccess;
using CussBuster.Core.Data;
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
		private readonly IWordLoader _wordLoader;
		private readonly IAuthChecker _authChecker;
		private readonly IAuditWriter _auditWriter;
		private readonly IAppSettings _appSettings;

		public MainHelper(IWordLoader wordLoader, IAuthChecker authChecker, IAuditWriter auditWriter, IAppSettings appSettings)
		{
			_wordLoader = wordLoader;
			_authChecker = authChecker;
			_auditWriter = auditWriter;
			_appSettings = appSettings;
		}

		public IEnumerable<ReturnModel> FindMatches(string text)
		{
			var dataset = _wordLoader.Load();

			var matches = new List<ReturnModel>();

			foreach (var w in text.Split(" "))
			{
				var word = w.RemovePunctuationAndSymbols();

				var match = dataset.FirstOrDefault(x =>
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

			return matches;
		}

		public bool CheckAuthorization(string authToken)
		{
			if (!Guid.TryParse(authToken, out Guid guidAuthToken))
				return false;

			return _authChecker.CheckToken(guidAuthToken);
		}

		public bool CheckCharacterLimit(string text)
		{
			if (text.Length > _appSettings.CharacterLimit)
				return false;

			return true;
		}
	}
}
