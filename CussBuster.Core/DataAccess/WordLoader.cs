﻿using CussBuster.Core.Data;
using CussBuster.Core.Data.Entities;
using CussBuster.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace CusBuster.Core.DataAccess
{
	public class WordLoader : IWordLoader
    {
		private readonly CussBusterContext _context;

		public WordLoader(CussBusterContext context)
		{
			_context = context;
		}

		public IEnumerable<WordModel> Load()
		{
			return _context.Word.Select(x => new WordModel
			{
				Word = x.Word1,
				WordTypeId = x.WordTypeId,
				Severity = x.Severity,
				SearchTypeId = x.SearchTypeId
			});
		}
	}
}
