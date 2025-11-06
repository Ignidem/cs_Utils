#define WritableDebugging
using System;
using System.Collections.Generic;
using System.IO;
using Utils.Logger;
#if WritableDebugging
using System.Collections;
using System.Text;
#endif

namespace Utils.Serializers.WritableObjects.Reader
{
	public abstract class BinaryStreamReader<TReader, TWriter> : IReader
#if WritableDebugging
		, IWritableDebug
#endif
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
		
#if WritableDebugging
		public StringBuilder DebugContent { get; } = new StringBuilder();
		public int Indent { get; set; }
#endif
		
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
#if WritableDebugging
			DebugContent.ToString().LogMessage();
#endif
		}

		public T ReadType<T>(string name)
		{
			GenericWritable<TReader, TWriter>.IHandler<T> handler =
				GenericWritable<TReader, TWriter>.GetWritableSerializer<T>();
			if (this is not TReader reader) throw new Exception();
			return handler.ReadType(reader, name);
		}

		public T Read<T>()
		{
			try
			{
#if WritableDebugging
				Type writableInstance = typeof(T);
				this.StartValue(writableInstance);
#endif
				
				if (!reader.TryRead(out T value))
					value = ReadNonPrimitive<T>();
				
#if WritableDebugging
				this.Value(writableInstance, value);
				this.EndValue(writableInstance);
#endif
				return value;
			}
			catch (Exception e)
			{
				e.LogException();
				throw new Exception("Error while reading " + typeof(T).Name);
			}
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
