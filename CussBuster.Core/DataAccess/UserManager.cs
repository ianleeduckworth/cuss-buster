using System;
using System.Linq;
using CussBuster.Core.Data.Entities;
using CussBuster.Core.Models;

namespace CussBuster.Core.DataAccess
{
	public class UserManager : IUserManager
	{
		private readonly CussBusterContext _context;

		public UserManager(CussBusterContext context)
		{
			_context = context;
		}

		public Guid AddNewuser(UserSignupModel signupModel, StandardPricingTier tier)
		{
			var apiToken = Guid.NewGuid();

			var user = new User
			{
				ApiToken = apiToken,
				CallsPerMonth = tier.CallsPerMonth,
				CanCallApi = true,
				FirstName = signupModel.FirstName,
				LastName = signupModel.LastName,
				Email = signupModel.EmailAddress,
				PricePerMonth = tier.PricePerMonth,
				CreatedBy = "test",
				CreatedDate = DateTime.UtcNow,
				UpdatedBy = "test",
				//UpdatedDate = DateTime.UtcNow
				UpdatedDate = "test",
			};

			_context.User.Add(user);
			_context.SaveChanges();

			return apiToken;
		}

		public void CheckLockAccount(User user)
		{
			var now = DateTime.UtcNow;
			var callsThisMonth = _context.CallLog.Where(x => x.UserId == user.UserId && x.EventDate.Month == now.Month && x.EventDate.Year == now.Year).Count();

			if (callsThisMonth > user.CallsPerMonth)
			{
				user.CanCallApi = false;
				_context.SaveChanges();
			}
		}

		public DateTime GetLastCallDate(User user)
		{
			return _context.CallLog.Where(x => x.UserId == user.UserId).Max(x => x.EventDate);
		}

		public User GetUserByApiToken(Guid apiToken)
		{
			return _context.User.FirstOrDefault(x => x.ApiToken == apiToken);
		}

		public User GetUserByEmail(string emailAddress)
		{
			return _context.User.FirstOrDefault(x => x.Email == emailAddress);
		}

		public void UnlockAccount(User user)
		{
			user.CanCallApi = true;
			_context.SaveChanges();
		}
	}
}
