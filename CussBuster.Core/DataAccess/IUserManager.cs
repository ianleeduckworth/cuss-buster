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
		int GetCallsThisMonth(int userId);
		User AddNewuser(UserSignupModel signupModel, StandardPricingTier pricingTier, string userName);
		User UpdateExistingUser(User user);
		void SetStandardSettings(int userId);
	}
}
