using System;
using System.Linq;
using CussBuster.Core.Data.Entities;
using CussBuster.Core.Data.Static;
using CussBuster.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CussBuster.Core.DataAccess
{
	public class UserManager : IUserManager
	{
		private readonly CussBusterContext _context;

		public UserManager(CussBusterContext context)
		{
			_context = context;
		}

		public User AddNewuser(UserSignupModel signupModel, StandardPricingTier tier)
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
				CreditCardNumber = long.Parse(signupModel.CreditCardNumber),
				//UpdatedDate = DateTime.UtcNow
				UpdatedDate = "test",
			};

			_context.User.Add(user);
			_context.SaveChanges();

			return user;
		}

		public void CheckLockAccount(User user)
		{
			var callsThisMonth = GetCallsThisMonth(user.UserId);

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
			return _context.User.Where(x => x.ApiToken == apiToken).Include(x => x.UserSetting).Include(x => x.CallLog).FirstOrDefault();
		}

		public int GetCallsThisMonth(int userId)
		{
			var now = DateTime.UtcNow;
			return _context.CallLog.Where(x => x.UserId == userId && x.EventDate.Month == now.Month && x.EventDate.Year == now.Year).Count();
		}

		public User GetUserByEmail(string emailAddress)
		{
			return _context.User.FirstOrDefault(x => x.Email == emailAddress);
		}

		public void SetStandardSettings(int userId)
		{
			var now = DateTime.UtcNow;
			var userName = "test"; //todo fix this

			foreach (var value in Enum.GetValues(typeof(StaticData.WordType)))
			{
				_context.UserSetting.Add(new UserSetting
				{
					Severity = 10,
					UserId = userId,
					WordTypeId = (byte)(int)value,
					CreatedBy = userName,
					CreatedDate = now,
					UpdatedBy = userName,
					UpdatedDate = "test"
				});
			}

			_context.SaveChanges();
		}

		public void UnlockAccount(User user)
		{
			user.CanCallApi = true;
			_context.SaveChanges();
		}

		public User UpdateExistingUser(User user)
		{
			_context.SaveChanges();
			return user;
		}
	}
}
