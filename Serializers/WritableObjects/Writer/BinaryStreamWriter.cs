#define WritableDebugging
using System;
using System.Collections.Generic;
using System.IO;
#if WritableDebugging
using System.Text;
using Utils.Logger;
#endif

namespace Utils.Serializers.WritableObjects
{
	public abstract class BinaryStreamWriter<TWriter, TReader> : IWriter
	#if WritableDebugging
		, IWritableDebug
	#endif
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
		private readonly bool disposeStream;

#if WritableDebugging
		public StringBuilder DebugContent { get; } = new StringBuilder();
		public int Indent { get; set; }
#endif
		
		public long Size => stream.Position;
		public long Capacity => stream.Length;
		public byte[] Data
		{
			get
			{
				Flush();
#if WritableDebugging
				"Writer Data:".LogMessage();
				DebugContent.ToString().LogMessage();
#endif
				return stream switch
				{
					MemoryStream memory => memory.ToArray(),
					_ => throw new NotImplementedException(stream.GetType().Name)
				};
			}
		}

		public BinaryStreamWriter() : this(new MemoryStream(), true)  { }
		public BinaryStreamWriter(Stream stream, bool disposeStream)
		{
			this.disposeStream = disposeStream;
			writer = new BinaryWriter(stream);
			this.stream = stream;
		}

		public virtual void Dispose()
		{
			writer.Dispose();
			if (disposeStream)
				stream.Dispose();
			GC.SuppressFinalize(this);
		}
		public void Flush() => writer.Flush();

		public void Write<T>(T value)
		{
#if WritableDebugging
			Type writableInstance = typeof(T);
			this.StartValue(writableInstance);
			this.Value(writableInstance, value);
#endif
			
			if (writer.TryWritePrimitive(value))
			{
#if WritableDebugging
				this.EndValue(writableInstance);
#endif
				return;
			}

			if (this is not TWriter tWriter)
			{
				throw new Exception();
			}

			if (TryGetWriteFunc(out Action<TWriter, T> writeFunc))
			{
				writeFunc(tWriter, value);
#if WritableDebugging
				this.EndValue(writableInstance);
#endif
				return;
			}

			GenericWritable<TReader, TWriter>.IHandler<T> handler = GenericWritable<TReader, TWriter>.GetWritableSerializer<T>();
			handler.Write(tWriter, value);
#if WritableDebugging
			this.EndValue(writableInstance);
#endif
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
	}
}
