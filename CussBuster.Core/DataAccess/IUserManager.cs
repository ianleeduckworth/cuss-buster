using System;
using System.Collections.Generic;
using System.Text;
using CussBuster.Core.Data.Entities;

namespace CussBuster.Core.DataAccess
{
	public interface IUserManager
	{
		DateTime GetLastCallDate(User user);
		void UnlockAccount(User user);
		void CheckLockAccount(User user);
	}
}
