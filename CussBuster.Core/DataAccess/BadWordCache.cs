using CussBuster.Core.Models;
using System.Collections.Generic;

namespace CussBuster.Core.DataAccess
{
	public class BadWordCache : IBadWordCache
    {
		public IEnumerable<WordModel> Words { get; set; }
    }
}
