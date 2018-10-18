using CussBuster.Core.Data.Entities;
using CussBuster.Core.DataAccess;
using CussBuster.Core.Exceptions;
using CussBuster.Core.Helpers;
using CussBuster.Core.Models;
using CussBuster.Core.Security;
using Moq;
using NUnit.Framework;
using System;
using System.Text;

namespace CussBuster.Test
{
	[TestFixture]
	public class SigninHelperTests
	{
		private Mock<IUserManager> _userManager;
		private Mock<IPasswordHelper> _passwordHelper;
		private Mock<IWebPageHelper> _webPageHelper;
		private ISigninHelper _signinHelper;

		[SetUp]
		public void SetUp()
		{
			_userManager = new Mock<IUserManager>();
			_passwordHelper = new Mock<IPasswordHelper>();
			_webPageHelper = new Mock<IWebPageHelper>();
			_signinHelper = new SigninHelper(_userManager.Object, _passwordHelper.Object, _webPageHelper.Object);
		}

		[Test]
		public void Signin_NullUser()
		{
			const string email = "email";
			const string password = "password";

			//arrange
			_userManager.Setup(x => x.GetUserByEmail(email)).Returns(default(User));

			//act / assert
			Assert.Throws<UserNotFoundException>(() => _signinHelper.Signin(email, password), $"Could not find user where email address is {email}");
		}

		[Test]
		public void Signin_PasswordNotFound()
		{
			const string email = "email";
			byte[] enteredPassword = Encoding.ASCII.GetBytes("enteredPassword");
			byte[] storedPassword = Encoding.ASCII.GetBytes("storedPassword");

			//arrange
			_userManager.Setup(x => x.GetUserByEmail(email)).Returns(new User
			{
				Password = storedPassword
			});

			_passwordHelper.Setup(x => x.CompareSecurePasswords(enteredPassword, storedPassword)).Returns(false);

			//act / assert
			Assert.Throws<UnauthorizedAccessException>(() => _signinHelper.Signin(email, enteredPassword.ToString()), $"Password entered was incorrect");
		}

		[Test]
		public void Signin_ValidPassword()
		{
			const string email = "email";
			byte[] enteredPassword = Encoding.ASCII.GetBytes("enteredPassword");
			byte[] storedPassword = Encoding.ASCII.GetBytes("storedPassword");
			Guid apiToken = new Guid("126c85d3-d1dd-4a80-ba93-b49ea9601f01");

			//arrange
			_userManager.Setup(x => x.GetUserByEmail(email)).Returns(new User
			{
				ApiToken = apiToken,
				Password = storedPassword,
			});

			_passwordHelper.Setup(x => x.CompareSecurePasswords(enteredPassword, storedPassword)).Returns(true);

			_webPageHelper.Setup(x => x.MapUserToModel(It.IsAny<User>())).Returns(new UserReturnModel
			{
				ApiToken = apiToken
			});

			//act
			var result = _signinHelper.Signin(email, Encoding.ASCII.GetString(enteredPassword));

			//assert
			Assert.True(result.ApiToken == apiToken);
		}
	}
}
