using System;
using System.Collections.Generic;

namespace Utils.Serializers.WritableObjects
{
	public static class WriterUtils
	{
		public static readonly Dictionary<Type, Action<IWriter, object>> enumReaders = new()
		{
			[typeof(sbyte)] = (writer, value) => writer.Write((sbyte)value),
			[typeof(byte)] = (writer, value) => writer.Write((byte)value),
			[typeof(short)] = (writer, value) => writer.Write((short)value),
			[typeof(ushort)] = (writer, value) => writer.Write((ushort)value),
			[typeof(int)] = (writer, value) => writer.Write((int)value),
			[typeof(uint)] = (writer, value) => writer.Write((uint)value),
			[typeof(long)] = (writer, value) => writer.Write((long)value),
			[typeof(ulong)] = (writer, value) => writer.Write((ulong)value),
		};

		public static void WriteEnum<T>(this IWriter writer, T value)
			where T : struct, Enum
		{
			Type underlying = Enum.GetUnderlyingType(typeof(T));
			enumReaders[underlying](writer, value);
		}

		public static void WriteMany<T>(this IWriter writer, IReadOnlyList<T> values)
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
