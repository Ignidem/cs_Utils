using System;
using System.Collections.Generic;
using System.IO;

namespace Utils.Serializers.WritableObjects
{
	public static class BinaryWriterUtils
	{
		private static readonly Dictionary<Type, Delegate> writeFunctions = new()
		{
			[typeof(bool)] = (Action<BinaryWriter, bool>)((BinaryWriter writer, bool value) => writer.Write(value)),
			[typeof(byte)] = (Action<BinaryWriter, byte>)((BinaryWriter writer, byte value) => writer.Write(value)),
			[typeof(sbyte)] = (Action<BinaryWriter, sbyte>)((BinaryWriter writer, sbyte value) => writer.Write(value)),

			[typeof(short)] = (Action<BinaryWriter, short>)((BinaryWriter writer, short value) => writer.Write(value)),
			[typeof(ushort)] = (Action<BinaryWriter, ushort>)((BinaryWriter writer, ushort value) => writer.Write(value)),

			[typeof(int)] = (Action<BinaryWriter, int>)((BinaryWriter writer, int value) => writer.Write(value)),
			[typeof(uint)] = (Action<BinaryWriter, uint>)((BinaryWriter writer, uint value) => writer.Write(value)),
			[typeof(float)] = (Action<BinaryWriter, float>)((BinaryWriter writer, float value) => writer.Write(value)),

			[typeof(decimal)] = (Action<BinaryWriter, decimal>)((BinaryWriter writer, decimal value) => writer.Write(value)),
			[typeof(double)] = (Action<BinaryWriter, double>)((BinaryWriter writer, double value) => writer.Write(value)),
			[typeof(long)] = (Action<BinaryWriter, long>)((BinaryWriter writer, long value) => writer.Write(value)),
			[typeof(ulong)] = (Action<BinaryWriter, ulong>)((BinaryWriter writer, ulong value) => writer.Write(value)),

			[typeof(char)] = (Action<BinaryWriter, char>)((BinaryWriter writer, char value) => writer.Write(value)),
			[typeof(string)] = (Action<BinaryWriter, string>)((BinaryWriter writer, string value) =>
			{
				bool isNull = value == null;
				writer.Write(isNull);
				if (!isNull)
					writer.Write(value);
			}),

			[typeof(Guid)] = (Action<BinaryWriter, Guid>)((BinaryWriter writer, Guid value) => writer.Write(value.ToByteArray())),
			[typeof(DateTime)] = (Action<BinaryWriter, DateTime>)((BinaryWriter writer, DateTime value) => writer.Write(value.Ticks)),
		};

		public static bool TryWritePrimitive<T>(this BinaryWriter writer, T value)
		{
			if (writeFunctions.TryGetValue(typeof(T), out var delg) && delg is Action<BinaryWriter, T> writeFunc)
			{
				writeFunc(writer, value);
				return true;
			}

			return false;
		}
	}
}
