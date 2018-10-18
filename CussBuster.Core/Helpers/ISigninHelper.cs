using CussBuster.Core.Data.Entities;
using CussBuster.Core.Models;
using System;

namespace CussBuster.Core.Helpers
{
	public interface ISigninHelper
	{
		UserReturnModel Signin(string email, string password);
		UserReturnModel Signin(User user, string password);
	}
}
