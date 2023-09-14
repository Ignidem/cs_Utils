using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Collections
{
	public static class LinqEx
	{
		public static T WhereMin<T, N>(this IEnumerable<T> items, Func<T, N> getComparable)
			where N : IComparable<N>
		{
			IEnumerator<T> enumerator = items.GetEnumerator();

			bool first = true;
			T target = default;
			N min = default;
			while (enumerator.MoveNext())
			{
				T value = enumerator.Current;
				N other = getComparable(value);

				if (first)
					first = false;
				else if (min.CompareTo(other) >= 0)
					continue;
				
				target = value;
				min = other;
			}

			return target;
		}
	}
}
