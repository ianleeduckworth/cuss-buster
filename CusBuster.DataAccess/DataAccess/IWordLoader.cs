using CussBuster.Core.Models;
using System.Collections.Generic;

namespace CusBuster.DataAccess.DataAccess
{
	public interface IWordLoader
	{
		IEnumerable<WordModel> Load();
	}
}