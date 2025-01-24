using System;
using System.Collections.Generic;
using System.IO;

namespace Utils.Serializers.WritableObjects
{
	public static class BinaryReaderUtils
	{
		private static readonly Dictionary<Type, Delegate> binaryReaderFunctions = new()
		{
			[typeof(bool)] = (Func<BinaryReader, bool>)((BinaryReader reader) => reader.ReadBoolean()),
			[typeof(byte)] = (Func<BinaryReader, byte>)((BinaryReader reader) => reader.ReadByte()),
			[typeof(sbyte)] = (Func<BinaryReader, sbyte>)((BinaryReader reader) => reader.ReadSByte()),

			[typeof(short)] = (Func<BinaryReader, short>)((BinaryReader reader) => reader.ReadInt16()),
			[typeof(ushort)] = (Func<BinaryReader, ushort>)((BinaryReader reader) => reader.ReadUInt16()),

			[typeof(int)] = (Func<BinaryReader, int>)((BinaryReader reader) => reader.ReadInt32()),
			[typeof(uint)] = (Func<BinaryReader, uint>)((BinaryReader reader) => reader.ReadUInt32()),
			[typeof(float)] = (Func<BinaryReader, float>)((BinaryReader reader) => reader.ReadSingle()),

			[typeof(decimal)] = (Func<BinaryReader, decimal>)((BinaryReader reader) => reader.ReadDecimal()),
			[typeof(double)] = (Func<BinaryReader, double>)((BinaryReader reader) => reader.ReadDouble()),
			[typeof(long)] = (Func<BinaryReader, long>)((BinaryReader reader) => reader.ReadInt64()),
			[typeof(ulong)] = (Func<BinaryReader, ulong>)((BinaryReader reader) => reader.ReadUInt64()),

			[typeof(char)] = (Func<BinaryReader, char>)((BinaryReader reader) => reader.ReadChar()),
			[typeof(string)] = (Func<BinaryReader, string>)((BinaryReader reader) => reader.ReadString()),

			[typeof(Guid)] = (Func<BinaryReader, Guid>)((BinaryReader reader) => new Guid(reader.ReadBytes(16))),
			[typeof(DateTime)] = (Func<BinaryReader, DateTime>)((BinaryReader reader) => new DateTime(reader.ReadInt64())),
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

	}
}
