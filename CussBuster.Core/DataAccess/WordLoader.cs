using CussBuster.Core.Data;
using CussBuster.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CusBuster.Core.DataAccess
{
    public class WordLoader : IWordLoader
    {
		public IEnumerable<WordModel> Load()
		{
			return new List<WordModel>
			{
				new WordModel
				{
					Word = "shit",
					WordTypeId = (byte)StaticData.WordType.Vulgarity,
					Severity = 5,
					SearchTypeId = (byte)StaticData.SearchType.Contains,
				},
				new WordModel
				{
					Word = "fuck",
					WordTypeId = (byte)StaticData.WordType.Vulgarity,
					Severity = 9,
					SearchTypeId = (byte)StaticData.SearchType.Contains,
				},
				new WordModel
				{
					Word = "asshole",
					WordTypeId = (byte)StaticData.WordType.Vulgarity,
					Severity = 4,
					SearchTypeId = (byte)StaticData.SearchType.Equals
				}
			};
		}
	}
}
