using CussBuster.Core.Models;
using System.Collections.Generic;

namespace CussBuster.Core.DataAccess
{
	public interface IBadWordCache
	{
		IEnumerable<WordModel> Words { get; set; }
	}
}