using CussBuster.Core.Data.Entities;
using CussBuster.Core.Models;
using System;

namespace CussBuster.Core.Helpers
{
	public interface IWebPageHelper
	{
		UserReturnModel SignUp(UserSignupModel signupModel, string userName);
		UserReturnModel GetUserInfo(Guid apiTokenGuid);
		UserReturnModel UpdateUserInfo(Guid apiTokenGuid, string password, UserUpdateModel userSignupModel);
		UserReturnModel MapUserToModel(User user);
	}
}