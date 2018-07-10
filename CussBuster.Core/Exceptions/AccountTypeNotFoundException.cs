using System;
using System.Collections.Generic;
using System.Text;

namespace CussBuster.Core.Exceptions
{
    public class AccountTypeNotFoundException : Exception
    {
		public AccountTypeNotFoundException(string message) : base(message)
		{
		}
    }
}
