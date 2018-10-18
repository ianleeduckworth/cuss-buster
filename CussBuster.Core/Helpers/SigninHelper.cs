using CussBuster.Core.Data.Entities;
using CussBuster.Core.DataAccess;
using CussBuster.Core.Exceptions;
using CussBuster.Core.Models;
using CussBuster.Core.Security;
using System;
using System.Text;

namespace CussBuster.Core.Helpers
{
	public class SigninHelper : ISigninHelper
	{
		private readonly IUserManager _userManager;
		private readonly IPasswordHelper _passwordHelper;
		private readonly IWebPageHelper _webPageHelper;

		public SigninHelper(IUserManager userManager, IPasswordHelper passwordHelper, IWebPageHelper webPageHelper)
		{
			_userManager = userManager;
			_passwordHelper = passwordHelper;
			_webPageHelper = webPageHelper;
		}

		public UserReturnModel Signin(string email, string password)
		{
			var user = _userManager.GetUserByEmail(email);

			if (user == null)
				throw new UserNotFoundException($"Could not find user where email address is {email}");

			if (_passwordHelper.CompareSecurePasswords(Encoding.ASCII.GetBytes(password), user.Password))
				return _webPageHelper.MapUserToModel(user);

			throw new UnauthorizedAccessException("Password entered was incorrect");
		}

		public UserReturnModel Signin(User user, string password)
		{
			return Signin(user.Email, password);
		}
	}
}
