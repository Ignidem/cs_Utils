using System;
using System.Text;

namespace Utilities.Collections
{
#nullable enable
	public static class ArrayEx
	{
		public static int IndexOf<T>(this T[] array, Predicate<T> predicate)
		{
			for (int i = 0; i < array.Length; i++)
			{
				T? element = array[i];

				if (predicate(element)) return i;
			}

			return -1;
		}

		public static bool TryIndexOf<T>(this T[] array, Predicate<T> predicate, out int index)
		{
			for (int i = 0; i < array.Length; i++)
			{
				T? element = array[i];

				if (predicate(element))
				{
					index = i;
					return true;
				}
			}

			index = -1;
			return false;
		}

		public static T[] AddElement<T>(this T[] array, T element)
		{
			Array.Resize(ref array, array.Length + 1);
			array[^1] = element;
			return array;
		}

		public static T? RandomElement<T>(this T[] array, Random random)
		{
			if (array == null || array.Length == 0) return default;

			int index = random.Next(0, array.Length);
			return array[index];
		}

		public static StringBuilder JoinRandoms(Random random, char separator, params string[][] arrays)
		{
			StringBuilder text = new();

			for (int i = 0; i < arrays.Length; i++)
			{
				if (i != 0) text.Append(separator);

				string[] array = arrays[i];

				text.Append(array.RandomElement(random));
			}

			return text;
		}
	}
}
