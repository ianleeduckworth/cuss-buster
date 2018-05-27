using System;

namespace CussBuster.Core.DataAccess
{
	public interface IAuthChecker
	{
		bool CheckToken(Guid token);
	}
}