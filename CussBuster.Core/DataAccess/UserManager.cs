using System;
using System.Linq;
using CussBuster.Core.Data.Entities;

namespace CussBuster.Core.DataAccess
{
	public class UserManager : IUserManager
	{
		private readonly CussBusterContext _context;

		public UserManager(CussBusterContext context)
		{
			_context = context;
		}

		public void CheckLockAccount(User user)
		{
			var now = DateTime.UtcNow;
			var callsThisMonth = _context.CallLog.Where(x => x.UserId == user.UserId && x.EventDate.Month == now.Month && x.EventDate.Year == now.Year).Count();

			if (callsThisMonth > /*user.CallsPerMonth*/ 250) //todo fix once scaffolding is redone
			{
				user.CanCallApi = false;
				_context.SaveChanges();
			}
		}

		public DateTime GetLastCallDate(User user)
		{
			return _context.CallLog.Where(x => x.UserId == user.UserId).Max(x => x.EventDate);
		}

		public void UnlockAccount(User user)
		{
			user.CanCallApi = true;
			_context.SaveChanges();
		}
	}
}
