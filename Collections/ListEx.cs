using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Collections
{
	public static class ListEx
	{
		public static IEnumerable<T> ToEnumerable<T>(this IList list)
		{
			foreach (var e in list)
			{
				if (e is not T _t) continue;
				yield return _t;
			}
		}

		public static T Pop<T>(this List<T> list)
		{
			int index = list.Count - 1;
			T value = list[index];
			list.RemoveAt(index);
			return value;
		}

		public static bool TryPop<T>(this List<T> list, out T item)
		{
			if (TryPeek(list, out item))
			{
				list.RemoveAt(list.Count - 1);
				return true;
			}

			return false;
		}

		public static bool TryPeek<T>(this List<T> list, out T item)
		{
			if (list.Count == 0)
			{
				item = default;
				return false;
			}

			item = list[^1];
			return true;
		}

		public static bool TryPop<T>(this List<T> list, Predicate<T> predicate, out T item)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				T _item = list[i];
				if (!predicate(_item)) continue; 
				
				item = _item;
				list.RemoveAt(i);
				return true;
			}

			item = default;
			return false;
		}

		public static int IndexOf<T>(this IReadOnlyList<T> self, T target)
		{
			for (int i = 0; i < self.Count; i++)
			{
				T element = self[i];
				if (Equals(element, target))
					return i;
			}

			return -1;
		}

		public static int IndexOf<T>(this IReadOnlyList<T> self, Predicate<T> predicate)
		{
			for (int i = 0; i < self.Count; i++)
			{
				T element = self[i];
				if (predicate(element))
					return i;
			}

			return -1;
		}
	}
}
