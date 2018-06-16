using System;
using CussBuster.Core.Data.Entities;
using CussBuster.Core.Models;

namespace CussBuster.Core.DataAccess
{
	public interface IUserManager
	{
		DateTime GetLastCallDate(User user);
		void UnlockAccount(User user);
		void CheckLockAccount(User user);
		User GetUserByEmail(string emailAddress);
		User GetUserByApiToken(Guid ApiToken);
		Guid AddNewuser(UserSignupModel signupModel, StandardPricingTier pricingTier);
	}
}
