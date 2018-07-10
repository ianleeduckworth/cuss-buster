using CussBuster.Core.Models;
using System;

namespace CussBuster.Core.Helpers
{
	public interface IWebPageHelper
	{
		Guid SignUp(UserSignupModel signupModel);
		UserReturnModel GetUserInfo(Guid apiTokenGuid);
		UserUpdateModel UpdateUserInfo(Guid apiTokenGuid, UserUpdateModel userSignupModel);
	}
}