using CussBuster.Core.Data.Entities;
using CussBuster.Core.Models;
using System.Collections.Generic;

namespace CussBuster.Core.Helpers
{
	public interface IMainHelper
	{
		IEnumerable<ReturnModel> FindMatches(string text, User user);
		User CheckAuthorization(string authToken);
		bool CheckCharacterLimit(string text);
		bool CheckUnlockAccount(User user);
	}
}