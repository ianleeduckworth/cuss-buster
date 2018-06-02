using CussBuster.Core.Data.Entities;
using System;
using System.Linq;

namespace CussBuster.Core.DataAccess
{
	public class AuthChecker : IAuthChecker
	{
		private readonly CussBusterContext _context;

		public AuthChecker(CussBusterContext context)
		{
			_context = context;
		}

		public User CheckToken(Guid token)
		{
			return _context.User.FirstOrDefault(x => x.ApiToken == token); ;
		}
	}
}
