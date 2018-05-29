using CussBuster.Core.Data.Entities;
using System;

namespace CussBuster.Core.DataAccess
{
	public interface IAuthChecker
	{
		User CheckToken(Guid token);
	}
}