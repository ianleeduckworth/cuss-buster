using CussBuster.Core.ExtensionMethods;
using NUnit.Framework;
using System.Collections.Generic;

namespace CussBuster.Test
{
	public class ExtensionMethodTests
    {
		[Test]
		public void IsNullOrZero_Null()
		{
			//arrange
			var list = default(List<string>);

			//act
			var result = list.IsNullOrZero();

			//assert
			Assert.True(result == true);
		}

		[Test]
		public void IsNullOrZero_Zero()
		{
			//arrange
			var list = new List<string>();

			//act
			var result = list.IsNullOrZero();

			//assert
			Assert.True(result == true);
		}

		[Test]
		public void IsNullOrZero_NotNullOrZero()
		{
			//arrange
			var list = new List<string>
			{
				"test"
			};

			//act
			var result = list.IsNullOrZero();

			//assert
			Assert.True(result == false);
		}

		[Test]
		public void RemovePunctuationAndSymbols_NoPunctuation()
		{
			//arrange
			const string test = "this is a test string";

			//act
			var result = test.RemovePunctuationAndSymbols();

			//assert
			Assert.True(result == test);
		}

		[Test]
		public void RemovePunctuationAndSymbols_Punctuation()
		{
			//arrange
			const string test = @"test!@#$%^&*()_+-={}|[]<\>?,./;':""";
			const string expectedResult = "test";

			//act
			var result = test.RemovePunctuationAndSymbols();

			//assert
			Assert.True(result == expectedResult);
		}
	}
}
