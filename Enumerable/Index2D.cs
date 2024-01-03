using System.Collections;
using System.Collections.Generic;

namespace Utilities.Enumerable
{
	public readonly struct Index2D<T>
	{
		public readonly int x;
		public readonly int y;
		public readonly T value;

		public Index2D(int x, int y, T value)
		{
			this.x = x;
			this.y = y;
			this.value = value;
		}
	}

	public static class EnumratorUtils
	{
		public static IEnumerable<Index2D<T>> Enumerate2D<T>(this T[,] array)
		{
			int xl = array?.GetLength(0) ?? 0;
			int yl = array?.GetLength(1) ?? 0;
			for(int x = 0; x < xl; x++) for (int y = 0; y < yl; y++)
			{
				yield return new Index2D<T>(x, y, array[x, y]);
			}
		}

		public static IEnumerable<Index2D<T>> Enumerate2D<T, R>(this R[,] array)
			where R : T
		{
			int xl = array?.GetLength(0) ?? 0;
			int yl = array?.GetLength(1) ?? 0;
			for (int x = 0; x < xl; x++) for (int y = 0; y < yl; y++)
			{
				yield return new Index2D<T>(x, y, array[x, y]);
			}
		}
	}
}
