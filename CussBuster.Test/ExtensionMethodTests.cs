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
			Assert.True(result, "Was expecting result to be true since list was null, but was false instead.");
		}

		[Test]
		public void IsNullOrZero_Zero()
		{
			//arrange
			var list = new List<string>();

			//act
			var result = list.IsNullOrZero();

			//assert
			Assert.True(result, "Was expecting result to be true since list was empty, but was false instead.");
		}

		[Test]
		public void IsNullOrZero_NotNullOrZero()
		{
			//arrange
			var list = new List<string> { "test" };

			//act
			var result = list.IsNullOrZero();

			//assert
			Assert.True(!result, "Was expecting result to be false since list was populated, but was true instead.");
		}

		[Test]
		public void RemovePunctuationAndSymbols_NoPunctuation()
		{
			//arrange
			const string test = "this is a test string";

			//act
			var result = test.RemovePunctuationAndSymbols();

			//assert
			AssertWithMessage.AreEqual(result, test, "string");
		}

		[Test]
		public void RemovePunctuationAndSymbols_Punctuation()
		{
			//arrange
			const string test = @"~`!@#$%^&*()_+test-={}|[]<\>?,./;':""";
			const string expectedResult = "test";

			//act
			var result = test.RemovePunctuationAndSymbols();

			//assert
			AssertWithMessage.AreEqual(result, expectedResult, "string");
		}
	}
}
