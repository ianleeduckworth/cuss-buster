using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CussBuster.Core.Data.Entities;
using CussBuster.Core.Data.Static;
using CussBuster.Core.Models;
using CussBuster.Core.Security;
using Microsoft.EntityFrameworkCore;

namespace CussBuster.Core.DataAccess
{
	public class UserManager : IUserManager
	{
		private readonly CussBusterContext _context;
		private readonly IPasswordHelper _passwordHelper;

		public UserManager(CussBusterContext context, IPasswordHelper passwordHelper)
		{
			_context = context;
			_passwordHelper = passwordHelper;
		}

		public User AddNewuser(UserSignupModel signupModel, StandardPricingTier tier, string userName)
		{
			var apiToken = Guid.NewGuid();

			var passwordBytes = Encoding.ASCII.GetBytes(signupModel.Password);
			var password = _passwordHelper.GenerateSecurePassword(passwordBytes);

			var user = new User
			{
				ApiToken = apiToken,
				CallsPerMonth = tier.CallsPerMonth,
				CanCallApi = true,
				FirstName = signupModel.FirstName,
				LastName = signupModel.LastName,
				Email = signupModel.EmailAddress,
				PricePerMonth = tier.PricePerMonth,
				Password = password,
				CreatedBy = userName,
				CreatedDate = DateTime.UtcNow,
				UpdatedBy = userName,
				CreditCardNumber = !string.IsNullOrEmpty(signupModel.CreditCardNumber) ? long.Parse(signupModel.CreditCardNumber) : default(decimal?),
				CreditCardCvcCode = signupModel.CreditCardCvcCode,
				CreditCardExpirationDate = signupModel.CreditCardExpirationDate,
				UpdatedDate = DateTime.UtcNow,
				AddressLine1 = signupModel.AddressLine1,
				AddressLine2 = signupModel.AddressLine2,
				City = signupModel.City,
				State = signupModel.State,
				ZipCode = signupModel.ZipCode
			};

			_context.User.Add(user);
			_context.SaveChanges();

			return user;
		}

		public void CheckLockAccount(User user)
		{
			if (GetCallsThisMonth(user.UserId) > user.CallsPerMonth)
			{
				user.CanCallApi = false;
				_context.SaveChanges();
			}
		}

		public DateTime GetLastCallDate(User user)
		{
			return _context.CallLog.Where(x => x.UserId == user.UserId).Max(x => x.EventDate);
		}

		public int GetCallsThisMonth(int userId)
		{
			var now = DateTime.UtcNow;
			return _context.CallLog.Where(x => x.UserId == userId && x.EventDate.Month == now.Month && x.EventDate.Year == now.Year).Count();
		}

		public User GetUserByApiToken(Guid apiToken)
		{
			return GetUser(x => x.ApiToken == apiToken);
		}

		public User GetUserByEmail(string emailAddress)
		{
			return GetUser(x => x.Email == emailAddress);
		}

		private User GetUser(Expression<Func<User, bool>> whereClause)
		{
			return _context.User.Where(whereClause).Include(x => x.UserSetting).Include(x => x.CallLog).FirstOrDefault();
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
