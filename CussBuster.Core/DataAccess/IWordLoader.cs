using CussBuster.Core.Models;
using System.Collections.Generic;

namespace CusBuster.Core.DataAccess
{
	public interface IWordLoader
	{
		IEnumerable<WordModel> Load();
	}
}