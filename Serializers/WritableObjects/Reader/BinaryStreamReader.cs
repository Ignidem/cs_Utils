using System;
using System.Collections.Generic;
using System.IO;

namespace Utils.Serializers.WritableObjects.Reader
{
	public abstract class BinaryStreamReader<TReader, TWriter> : IReader
		where TReader : IReader
		where TWriter : IWriter
	{
		private static readonly Dictionary<Type, Delegate> readerFunctions = new();
		public static void SetFunction<T>(Func<TReader, T> func)
		{
			readerFunctions[typeof(T)] = func;
		}

		protected readonly Stream stream;
		protected readonly BinaryReader reader;
		private readonly bool disposeStream;

		public BinaryStreamReader(byte[] data) : this(new MemoryStream(data), true) { }
		public BinaryStreamReader(Stream stream, bool disposeStream)
		{
			this.disposeStream = disposeStream;
			reader = new BinaryReader(stream);
			this.stream = stream;
		}

		public virtual void Dispose()
		{
			reader.Dispose();
			if (disposeStream)
				stream.Dispose();
			GC.SuppressFinalize(this);
		}

		public T Read<T>()
		{
			if (reader.TryRead(out T value))
				return value;

			return ReadNonPrimitive<T>();
		}

		public T ReadType<T>(string name)
		{
			GenericWritable<TReader, TWriter>.IHandler<T> handler = GenericWritable<TReader, TWriter>.GetWritableSerializer<T>(); 

			if (this is not TReader reader)
			{
				throw new Exception();
			}

			return handler.ReadType(reader, name);
		}
		protected T ReadNonPrimitive<T>()
		{
			if (this is not TReader reader)
			{
				throw new Exception();
			}

			if (TryGetReadFunc(out Func<TReader, T> readFunc))
				return readFunc(reader);

			GenericWritable<TReader, TWriter>.IHandler<T> handler = GenericWritable<TReader, TWriter>.GetWritableSerializer<T>();
			return handler.Read(reader);
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
	}
}
