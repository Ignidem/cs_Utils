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
		public static IEnumerator<Index2D<T>> Enumerate2D<T>(this T[,] array)
		{
			return new Enumerator2D<T>(array);
		}
	}

	public class Enumerator2D<T> : IEnumerator<Index2D<T>>
	{
		int x = 0;
		int y = -1;
		private readonly int xSize;
		private readonly int ySize;
		private T[,] _values;

		public Index2D<T> Current
		{
			get
			{
				if (x >= xSize || y >= ySize)
				{
					UnityEngine.Debug.Log($"{x} {y}");
				}

				return new Index2D<T>(x, y, _values[x, y]);
			}
		}

		object IEnumerator.Current => Current;

		public Enumerator2D(T[,] values)
		{
			_values = values;
			if (_values == null) return;
			xSize = _values.GetLength(0);
			ySize = _values.GetLength(1);
		}

		public bool MoveNext()
		{
			if (_values == null) return false;

			y++;
			if (y >= ySize)
			{
				x++;
				if (x >= xSize) return false;

				y = 0;
			}

			return x < xSize && y < ySize;
		}

		public void Reset()
		{
			x = 0;
			y = -1;
		}

		public void Dispose()
		{
			_values = null;
		}
	}
}
