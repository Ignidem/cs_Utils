using System;
using System.Collections.Generic;

namespace Utilities.Collections
{
	public static class ReadOnlyListEx
	{
		public static bool Contains<T>(this IReadOnlyList<T> list, T item)
		{
			for (int i = 0; i < list.Count; i++)
			{
				T entry = list[i];
				if (item == null)
				{
					if (entry == null || entry.Equals(item))
						return true;
				}
				else if (item.Equals(entry))
				{
					return true;
				}
			}

			return false;
		}
		public static int IndexOf<T>(this IReadOnlyList<T> self, T value)
		{
			return self.IndexOf(v => v.Equals(value));
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
		public static IReadOnlyList<T> Add<T>(this IReadOnlyList<T> list, T item)
		{
			T[] arr = new T[list.Count + 1];
			for (int i = 0; i < list.Count; i++)
			{
				arr[i] = list[i];
			}

			arr[^1] = item;
			return arr;
		}
		public static IReadOnlyList<T> RemoveAt<T>(this IReadOnlyList<T> list, int index)
		{
			if (index < 0 || index >= list.Count)
				return list;

			//The only index is removed, so the result will be empty.
			if (list.Count == 1)
				return Array.Empty<T>();

			T[] arr = new T[list.Count - 1];
			for (int i = 0; i < list.Count; i++)
			{
				if (i == index) continue;
				int k = i >= index ? i - 1 : i;
				arr[k] = list[i];
			}

			return arr;
		}
		public static IReadOnlyList<T> SetAt<T>(this IReadOnlyList<T> list, T item, int index)
		{
			if (list == null || list.Count == 0)
				return new T[] { item };

			if (index < 0)
				return list;

			if (index >= list.Count)
				return list.Add(item);

			T[] arr = new T[list.Count];
			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] = index == i ? item : list[i];
			}

			return arr;
		}
		public static bool TryGetAt<T>(this IReadOnlyList<T> list, int index, out T item)
		{
			if (index < 0 || index >= list.Count)
			{
				item = default;
				return false;
			}

			item = list[index];
			return true;
		}
	}
}
