using System.Collections.Generic;
using System.Linq;

namespace CussBuster.Core.ExtensionMethods
{
	public static class EnumerableExtensions
    {
		public static bool IsNullOrZero<T>(this IEnumerable<T> enumerable)
		{
			return enumerable == null || enumerable.Count() == 0;
		}
    }
}
