using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Collections
{
	public static class LinqEx
	{
		public static IEnumerable<Target> As<T, Target>(this IReadOnlyList<T> list)
			where T : Target
		{
			for (int i = 0; i < list.Count; i++)
				yield return list[i];
		}

		public static IEnumerable<TTarget> SelectAs<T, TTarget>(this IEnumerable<T> e)
		{
			foreach(T t in e)
			{
				if (t is TTarget _t)
					yield return _t;
			}
		}

		public static T WhereMax<T, N>(this IEnumerable<T> items, Func<T, N> getComparable)
			where N : IComparable<N>
		{
			return items.WhereComparison(getComparable, 1);
		}
		public static T WhereMin<T, N>(this IEnumerable<T> items, Func<T, N> getComparable)
			where N : IComparable<N>
		{
			return items.WhereComparison(getComparable, 0);
		}
		public static T WhereComparison<T, N>(this IEnumerable<T> items, Func<T, N> getComparable, int comparisonTarget)
			where N : IComparable<N>
		{
			IEnumerator<T> enumerator = items.GetEnumerator();

			bool first = true;
			T target = default;
			N best = default;
			while (enumerator.MoveNext())
			{
				T value = enumerator.Current;
				N other = getComparable(value);

				if (first)
					first = false;
				else if (best.CompareTo(other) != comparisonTarget)
					continue;
				
				target = value;
				best = other;
			}

			return target;
		}
	}
}
