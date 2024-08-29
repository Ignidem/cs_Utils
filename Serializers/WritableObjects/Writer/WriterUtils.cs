using System.Collections.Generic;

namespace Utils.Serializers.WritableObjects
{
	public static class WriterUtils
	{
		public static void WriteMany<T>(this IWriter writer, IList<T> values)
		{
			int count = values == null ? -1 : values.Count;
			writer.Write(count);
			for (int i = 0; i < count; i++)
			{
				T value = values[i];
				writer.Write(value);
			}
		}

		public static void WriteArray<T>(this IWriter writer, T[,] values)
		{
			if (values == null)
			{
				writer.Write(-1);
				return;
			}

			int x = values.GetLength(0), y = values.GetLength(1);
			writer.Write(x); writer.Write(y);
			for (int i = 0; i < x; i++)
			{
				for (int k = 0; k < y; k++)
				{
					T value = values[i, k];
					writer.Write(value);
				}
			}
		}
	}
}
