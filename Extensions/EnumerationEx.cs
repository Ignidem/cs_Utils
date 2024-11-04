using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Utilities.Enums;

namespace Utilities.Extensions
{
#nullable enable
	public static class EnumerationEx
	{
		public static int RandomIndex(this IList list)
		{
			return RandomIndex(list.Count);
		}
		public static int RandomIndex(int size)
		{
			return Numbers.RandomEx.Int(size);
		}

		public static (T?, V?) RandomElement<T, V>(this Dictionary<T, V> dict)
			where T : notnull
		{
			if (dict == null || dict.Count == 0) return (default, default);

			int index = RandomIndex(dict.Count);
			KeyValuePair<T, V> keypair = dict.ElementAt(index);
			return (keypair.Key, keypair.Value);
		}

		public static T? RandomElement<T>(this IList<T> list)
			=> list == null || list.Count == 0 ? default : list[RandomIndex(list.Count)];
		public static IEnumerable<int> RandomIndexes<T>(this IReadOnlyList<T> list, int count)
		{
			count = Math.Min(count, list.Count);
			IEnumerable<int> shuffled = list.Select((v, i) => i).OrderBy(u => Guid.NewGuid());
			return count == list.Count ? shuffled : shuffled.Take(count);
		}
		public static IEnumerable<T> RandomElements<T>(this IReadOnlyList<T> list, int count)
		{
			foreach (int index in list.RandomIndexes(count))
			{
				yield return list[index];
			}
		}
		public static T? RandomElement<T>(this IEnumerable<T> list)
			=> list == null || !list.Any() ? default : list.ElementAt(RandomIndex(list.Count()));

		public static T? RandomElement<T>(this T[] list) 
			=> Random(list);

		public static T? Random<T>(params T[] list)
			=> list == null || list.Length == 0 ? default : list[RandomIndex(list.Length)];

		public static T? RandomElement<T>(Random? random = null)
			where T : Enum
		{
			T[]? list = EnumUtils.GetValues<T>();
			if (list is null || list.Length == 0) return default;

			int index = random?.Next(0, list.Length) ?? RandomIndex(list.Length);
			return list[index];
		}
	}
}
