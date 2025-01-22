using System;
using System.Collections.Generic;
using System.IO;

namespace Utils.Serializers.WritableObjects
{
	public class BinaryStreamWriter<TWriter, TReader> : IWriter
		where TWriter : IWriter
		where TReader : IReader
	{
		private static readonly Dictionary<Type, Delegate> writerFunctions = new();
		public static void SetFunction<T>(Action<TWriter, T> func)
		{
			writerFunctions[typeof(T)] = func;
		}

		protected readonly Stream stream;
		protected readonly BinaryWriter writer;

		public long Size => stream.Position;
		public long Capacity => stream.Length;

		public BinaryStreamWriter(Stream stream)
		{
			writer = new BinaryWriter(stream);
			this.stream = stream;
		}

		public virtual void Dispose()
		{
			writer.Dispose();
			stream.Dispose();
			GC.SuppressFinalize(this);
		}

		public void Write<T>(T value)
		{
			if (writer.TryWritePrimitive(value))
				return;

			WriteNonPrimitive(value);
		}

		protected void WriteNonPrimitive<T>(T value)
		{
			if (!TryGetWriteFunc(out Action<TWriter, T> writeFunc))
				writeFunc = CreateReadFunc<T>();

			if (this is not TWriter writer)
			{
				throw new Exception();
			}

			writeFunc(writer, value);
		}

		protected bool TryGetWriteFunc<T>(out Action<TWriter, T> writeFunc)
		{
			if (!writerFunctions.TryGetValue(typeof(T), out Delegate delg) || delg is not Action<TWriter, T> _writeFunc)
			{
				writeFunc = null;
				return false;
			}

			writeFunc = _writeFunc;
			return true;
		}
		protected Action<TWriter, T> CreateReadFunc<T>()
		{
			Action<TWriter, T> readFunc = GenericWritable<TReader, TWriter>.GetWriter<T>();
			writerFunctions[typeof(T)] = readFunc;
			return readFunc;
		}
	}
}
