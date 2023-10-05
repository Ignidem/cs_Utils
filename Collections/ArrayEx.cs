using System;
using System.Linq;
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

		public static T[] RemoveAt<T>(this T[] array, int index)
		{
			T[] dest = new T[array.Length - 1];
			if (index > 0)
				Array.Copy(array, 0, dest, 0, index);

			if (index < array.Length - 1)
				Array.Copy(array, index + 1, dest, index, array.Length - index - 1);

			return dest;
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

		public static T[] Concat<T>(this T[] initial, params T[][] arrays)
		{
			if (arrays == null) return initial;

			int initialLength = initial?.Length ?? 0;
			int totalLength = initialLength + arrays.Sum(a => a?.Length ?? 0);
			T[] result = new T[totalLength];

			Array.Copy(initial, result, initialLength);
			int resultIndex = initialLength;

			for (int i = 0; i < arrays.Length; i++)
			{
				T[] array = arrays[i];
				if (array == null) continue;
				Array.Copy(array, 0, result, resultIndex, array.Length);
				resultIndex += array.Length;
			}

			return result;
		}
	}
}
