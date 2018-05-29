using CussBuster.Core.Models;
using System.Collections.Generic;

namespace CussBuster.Core.Helpers
{
	public interface IMainHelper
	{
		IEnumerable<ReturnModel> FindMatches(string text);
		bool CheckAuthorization(string authToken);
		bool CheckCharacterLimit(string text);
	}
}