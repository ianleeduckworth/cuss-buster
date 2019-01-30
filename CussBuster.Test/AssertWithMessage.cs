using NUnit.Framework;
using System;

namespace CussBuster.Test
{
	public static class AssertWithMessage
    {
		/// <summary>
		/// Compares two results and does an NUnit.Assert on the two objects as well as formats a standard message for better readability
		/// </summary>
		/// <param name="actual">The result obtained from running a test</param>
		/// <param name="expected">The expected result</param>
		public static void AreEqual(object actual, object expected)
		{
			Verify(actual, expected);
			Assert.True(actual.Equals(expected), Format(actual, expected, "outcome"));
			
		}

		/// <summary>
		/// Compares two results and does an NUnit.Assert on the two objects as well as formats a standard message for better readability
		/// </summary>
		/// <param name="actual">The result obtained from running a test</param>
		/// <param name="expected">The expected result</param>
		/// <param name="what">The thing being compared.  Example: "status code", "exception message".  Should be singular for best readability</param>
		public static void AreEqual(object actual, object expected, string what)
		{
			Verify(actual, expected, what);
			Assert.True(actual.Equals(expected), Format(actual, expected, what));

		}

		/// <summary>
		/// Compares the type of two results and does an NUnit.Assert to make sure the type of actual is the expected type as well as formats a standard message for better readability
		/// </summary>
		/// <param name="actual">The result obtained from running a test</param>
		/// <param name="expected">The expected type of result</param>
		public static void IsOfType(object actual, Type expected)
		{
			Assert.True(actual.GetType().Equals(expected), Format(actual.GetType().ToString(), expected.ToString(), "result type"));
		}

		/// <summary>
		/// Compares the type of two results and does an NUnit.Assert to make sure the type of actual is the expected type as well as formats a standard message for better readability
		/// </summary>
		/// <param name="actual">The result obtained from running a test</param>
		/// <param name="expected">The expected type of result</param>
		/// <param name="what">The thing being compared.  Example: "status code", "exception message".  Should be singular for best readability</param>
		public static void IsOfType(object actual, Type expected, string what)
		{
			Assert.True(actual.GetType().Equals(expected), Format(actual.GetType().ToString(), expected.ToString(), what));
		}

		/// <summary>
		/// Checks if a returned result is null and does an NUnit.Assert as well as formats a standard message for better readability
		/// </summary>
		/// <param name="actual">The result obtained from running a test</param>
		public static void IsNull(object actual)
		{
			Assert.True(actual == null, Format(actual, "null", "result"));
		}

		/// <summary>
		/// Checks if a returned result is null and does an NUnit.Assert as well as formats a standard message for better readability
		/// </summary>
		/// <param name="actual">The result obtained from running a test</param>
		/// <param name="what">The thing being compared.  Example: "status code", "exception message".  Should be singular for best readability</param>
		public static void IsNull(object actual, string what)
		{
			Assert.True(actual == null, Format(actual, "null", what));
		}

		private static string Format(object actual, object expected, string what)
		{
			return $"Was expecting {what} to be '{expected}', but was '{actual}' instead.";
		}

		private static void Verify(object actual, object expected, string what = null)
		{
			if (actual.GetType() != expected.GetType())
			{
				var message = $"Cannot compare two objects of different types.  {nameof(actual)} type: {actual.GetType().ToString()}.  {nameof(expected)} type: {expected.GetType().ToString()}.";

				if (what != null)
					message += $"  Value being compared: '{what}'";

				throw new ArgumentException(message);
			}
				
		}
	}
}
