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

		public static void WriteNullable<T>(this IWriter writer, T value)
			where T : class
		{
			bool hasValue = value != null;
			writer.Write(hasValue);
			if (hasValue) writer.Write(value);
		}
		
		public static void WriteEnum<T>(this IWriter writer, T value)
			where T : struct, Enum
		{
			Type underlying = Enum.GetUnderlyingType(typeof(T));
			enumReaders[underlying](writer, value);
		}

		public static int WriteCount<T>(this IWriter writer, IReadOnlyList<T> values)
		{
			int count = values?.Count ?? -1;
			writer.Write(count);
			return count;
		}
		public static void WriteArray<T>(this IWriter writer, IReadOnlyList<T> values)
		{
			int count = writer.WriteCount(values);
			for (int i = 0; i < count; i++)
				writer.Write(values[i]);
		}
		public static void WriteList<T>(this IWriter writer, IList<T> values)
		{
			if (values == null)
			{
				writer.Write(-1);
				return;
			}

			writer.Write(values.Count);
			for (int i = 0; i < values.Count; i++)
				writer.Write(values[i]);
		}	
		public static IEnumerable<T> WriteMany<T>(this IWriter writer, IReadOnlyList<T> values)
		{
			int count = writer.WriteCount(values);
			for (int i = 0; i < count; i++)
				yield return values[i];
		}
		public static void WriteMany<T>(this IWriter writer, IReadOnlyList<T> values, Action<T> write)
		{
			int count = writer.WriteCount(values);
			for (int i = 0; i < count; i++)
				write(values[i]);
		}
		public static void WriteManyAs<TValue, TResult>(this IWriter writer, 
			IReadOnlyList<TValue> values, Func<TValue, TResult> converter)
		{
			int count = writer.WriteCount(values);
			for (int i = 0; i < count; i++)
			{
				TResult value = converter(values[i]);
				writer.Write(value);
			}
		}

		public static void WriteManyArray<T>(this IWriter writer, T[,] values)
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
