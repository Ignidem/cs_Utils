namespace Utils.Serializers.WritableObjects
{
	public static class ReaderUtils
	{
		private static readonly Dictionary<Type, Delegate> binaryReaderFunctions = new()
		{
			[typeof(bool)] = (BinaryReader reader) => reader.ReadBoolean(),
			[typeof(byte)] = (BinaryReader reader) => reader.ReadByte(),
			[typeof(sbyte)] = (BinaryReader reader) => reader.ReadSByte(),

			[typeof(short)] = (BinaryReader reader) => reader.ReadInt16(),
			[typeof(ushort)] = (BinaryReader reader) => reader.ReadUInt16(),

			[typeof(int)] = (BinaryReader reader) => reader.ReadInt32(),
			[typeof(uint)] = (BinaryReader reader) => reader.ReadUInt32(),
			[typeof(float)] = (BinaryReader reader) => reader.ReadSingle(),

			[typeof(decimal)] = (BinaryReader reader) => reader.ReadDecimal(),
			[typeof(double)] = (BinaryReader reader) => reader.ReadDouble(),
			[typeof(long)] = (BinaryReader reader) => reader.ReadInt64(),
			[typeof(ulong)] = (BinaryReader reader) => reader.ReadUInt64(),

			[typeof(char)] = (BinaryReader reader) => reader.ReadChar(),
			[typeof(string)] = (BinaryReader reader) => reader.ReadString(),

			[typeof(Guid)] = (BinaryReader reader) => new Guid(reader.ReadBytes(16)),
		};
		public static bool TryRead<T>(this BinaryReader reader, out T value) 
		{
			Type type = typeof(T);
			if (!binaryReaderFunctions.TryGetValue(type, out Delegate del))
			{
				value = default;
				return false;
			}

			if (del is not Func<BinaryReader, T> readFunc)
			{
				throw new Exception("Read Function Type Mismatch! Mapping is Incorrect!");
			}

			value = readFunc(reader);
			return true;
		}

		private static readonly Dictionary<Type, Func<IReader, object>> enumReaders = new()
		{
			[typeof(sbyte)] = reader => reader.Read<sbyte>(),
			[typeof(byte)] = reader => reader.Read<byte>(),
			[typeof(short)] = reader => reader.Read<short>(),
			[typeof(ushort)] = reader => reader.Read<ushort>(),
			[typeof(int)] = reader => reader.Read<int>(),
			[typeof(uint)] = reader => reader.Read<uint>(),
			[typeof(long)] = reader => reader.Read<long>(),
			[typeof(ulong)] = reader => reader.Read<ulong>(),
		};

		public static T ReadEnum<T>(this IReader reader)
			where T : Enum
		{
			Type underlying = Enum.GetUnderlyingType(typeof(T));
			object value = enumReaders[underlying](reader);
			return (T)value;
		}

		public static T[] ReadMany<T>(this IReader reader)
		{
			int count = reader.Read<int>();
			if (count <= -1)
				return null;
			else if (count == 0)
				return Array.Empty<T>();

			T[] values = new T[count];
			for (int i = 0; i < count; i++)
			{
				values[i] = reader.Read<T>();
			}

			return values;
		}
		public static List<T> ReadList<T>(this IReader reader)
		{
			int count = reader.Read<int>();
			if (count == -1)
				return null;

			List<T> values = new List<T>(count);
			for (int i = 0; i < count; i++)
			{
				values.Add(reader.Read<T>());
			}

			return values;
		}

		public static TResult[] ReadManyAs<TValue, TResult>(this IReader reader, Func<TValue, TResult> convert)
		{
			int count = reader.Read<int>();
			if (count == -1)
				return null;

			TResult[] values = new TResult[count];
			for (int i = 0; i < count; i++)
			{
				values[i] = convert(reader.Read<TValue>());
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
