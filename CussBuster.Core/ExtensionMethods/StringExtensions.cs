﻿using System.Linq;

namespace CussBuster.Core.ExtensionMethods
{
	public static class StringExtensions
    {
		public static string RemovePunctuationAndSymbols(this string input)
		{
			return new string(input.Where(c => !char.IsPunctuation(c) && !char.IsSymbol(c)).ToArray()).Trim();
		}
    }
}
