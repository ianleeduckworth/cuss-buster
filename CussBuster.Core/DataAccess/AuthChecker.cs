using System;
using System.Collections.Generic;
using System.Text;

namespace CussBuster.Core.DataAccess
{
	public class AuthChecker : IAuthChecker
	{
		public bool CheckToken(Guid token)
		{
			if (token != new Guid("36bed969-d86a-41ba-a289-e79183a65268"))
				return false;

			return true;
		}
	}
}
