namespace Utils.Serializers.WritableObjects
{
	public interface IReader
	{
		T Read<T>();
	}

	public static class ReaderUtils
	{
		public static T[] ReadMany<T>(this IReader reader)
		{
			int count = reader.Read<int>();
			if (count == -1)
				return null;

			T[] values = new T[count];
			for (int i = 0; i < count; i++)
			{
				values[i] = reader.Read<T>();
			}

			return values;
		}

		public static T[,] ReadArray<T>(this IReader reader)
		{
			int x = reader.Read<int>();
			if (x == -1)
				return null;

			int y = reader.Read<int>();
			T[,] values = new T[x, y];
			for (int i = 0; i < x; i++)
			{
				for (int k = 0; k < y; k++)
				{
					values[i, k] = reader.Read<T>();
				}
			}

			return values;
		}
	}
}
