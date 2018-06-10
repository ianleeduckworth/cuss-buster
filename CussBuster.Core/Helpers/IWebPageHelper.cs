using CussBuster.Core.Models;
using System;

namespace CussBuster.Core.Helpers
{
	public interface IWebPageHelper
	{
		Guid SignUp(SignupModel signupModel);
	}
}