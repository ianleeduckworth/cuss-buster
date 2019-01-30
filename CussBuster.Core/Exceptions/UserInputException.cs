using System;
using System.Collections.Generic;
using System.Text;

namespace CussBuster.Core.Exceptions
{
    public class UserInputException : Exception
    {
		public UserInputException(string message) : base(message)
		{
		}

		public UserInputException(string message, Exception innerException) : base(message, innerException)
		{
		}
    }
}
