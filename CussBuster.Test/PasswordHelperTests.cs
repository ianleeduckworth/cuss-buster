using CussBuster.Core.Security;
using NUnit.Framework;
using System;
using System.Text;

namespace CussBuster.Test
{
	[TestFixture]
    public class PasswordHelperTests
    {
		private IPasswordHelper _passwordHelper;

		[SetUp]
		public void SetUp()
		{
			_passwordHelper = new PasswordHelper();
		}

		[TestCase("ajefliaj3akj3fkaj3fkajfka4j5")]
		[TestCase("457t89w7hf98fd9h8ret9h")]
		[TestCase("3i4utiw3tujw4kgjwekjk")]
		[TestCase("akjfakjfekajfekffkjaef%^")]
		public void GeneratePassword(string password)
		{
			//arrange / act
			var result = _passwordHelper.GenerateSecurePassword(Encoding.ASCII.GetBytes(password));

			//assert
			Assert.True(result.Length == 256);
		}

		[TestCase("q4krqk4jkgjkgjkbb", "jaekfjaekfj5bnakefjakefj")]
		[TestCase("4rjq3k4th3kth45wkyj54wy", "askdfj427askdfjkjrkjk")]
		[TestCase("askjvsdfkbjsihuegirgj", "j67y8rt7h8s7gh8f7fn7")]
		[TestCase("bkjkajbkjwekfjae%^", "857643586g78j78y")]
		public void ComparePasswords_NoMatch(string storedPassword, string enteredPassword)
		{
			//arrange
			var hashedStoredPassword = _passwordHelper.GenerateSecurePassword(Encoding.ASCII.GetBytes(storedPassword));

			//act
			var result = _passwordHelper.CompareSecurePasswords(Encoding.ASCII.GetBytes(enteredPassword), hashedStoredPassword);

			//assert
			Assert.False(result);
		}

		[TestCase("kajefkajefkajefkjaekjf", "kajefkajefkajefkjaekjf")]
		[TestCase("kjgk4tj3ktjw4k5yjlkjl6", "kjgk4tj3ktjw4k5yjlkjl6")]
		[TestCase("kfjkjkj5k6j34k7j48k", "kfjkjkj5k6j34k7j48k")]
		[TestCase("lk7l8ki6l7ku23dsft3%^", "lk7l8ki6l7ku23dsft3%^")]
		public void ComparePasswords_Match(string storedPassword, string enteredPassword)
		{
			//arrange
			var secureStoredPassword = _passwordHelper.GenerateSecurePassword(Encoding.ASCII.GetBytes(enteredPassword));

			//act
			var result = _passwordHelper.CompareSecurePasswords(Encoding.ASCII.GetBytes(enteredPassword), secureStoredPassword);

			//assert
			Assert.True(result);
		}

		[Test]
		public void GenerateHashedPassword()
		{
			//arrange
			const string password = "password123";

			//act
			var hashedPassword = _passwordHelper.GenerateSecurePassword(Encoding.ASCII.GetBytes(password));

			//assert
			Console.WriteLine(hashedPassword);
		}
    }
}
