using System;
using System.Collections.Generic;
using System.IO;

namespace Utils.Serializers.WritableObjects.Reader
{
	public class BinaryStreamReader<TReader, TWriter> : IReader
		where TReader : IReader
		where TWriter : IWriter
	{
		private static readonly Dictionary<Type, Delegate> readerFunctions = new();

		protected readonly Stream stream;
		protected readonly BinaryReader reader;

		public BinaryStreamReader(byte[] data) : this(new MemoryStream(data)) { }
		public BinaryStreamReader(Stream stream)
		{
			reader = new BinaryReader(stream);
			this.stream = stream;
		}

		public virtual void Dispose()
		{
			reader.Dispose();
			stream.Dispose();
			GC.SuppressFinalize(this);
		}

		public T Read<T>()
		{
			if (reader.TryRead(out T value))
				return value;

			return ReadNonPrimitive<T>();
		}

		protected T ReadNonPrimitive<T>()
		{
			if (!TryGetReadFunc(out Func<TReader, T> readFunc))
				readFunc = CreateReadFunc<T>();

			if (this is not TReader reader)
			{
				throw new Exception();
			}

			return readFunc(reader);
		}

		protected bool TryGetReadFunc<T>(out Func<TReader, T> reader)
		{
			if (!readerFunctions.TryGetValue(typeof(T), out Delegate delg) || delg is not Func<TReader, T> _reader)
			{
				reader = null;
				return false;
			}

			reader = _reader;
			return true;
		}
		protected Func<TReader, T> CreateReadFunc<T>()
		{
			Func<TReader, T> readFunc = GenericWritable<TReader, TWriter>.GetReader<T>();
			readerFunctions[typeof(T)] = readFunc;
			return readFunc;
		}
	}
}
