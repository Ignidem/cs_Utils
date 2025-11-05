#define WritableDebugging


using System.Collections;
#if WritableDebugging
using System.Text;
using Utils.Logger;
#endif

namespace Utils.Serializers.WritableObjects
{
	public abstract class BinaryStreamWriter<TWriter, TReader> : IWriter
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
		private readonly StringBuilder debugContent = new StringBuilder();
		private int indent;

		private string GetIndent()
		{
			return new string('\t', indent);
		}
		private void OnWrite(Type type, object value)
		{
			debugContent.Append(GetIndent());
			if (value == null)
			{
				debugContent.AppendFormat("{0}: null\n", type.Name);
				return;
			}

			if (type.IsValueType || type == typeof(string))
			{
				debugContent.AppendFormat("{0}: {1}\n", type, value);
				return;
			}

			indent++;
			if (type.IsArray || typeof(IList).IsAssignableFrom(type))
				debugContent.Append('[');
			else
				debugContent.AppendFormat("{0}: {{", value.GetType().Name);

			debugContent.Append('\n');
		}
		private void AfterWrite(Type type)
		{
			if (type.IsValueType || type == typeof(string))
				return;
			
			indent--;
			
			if (type.IsArray || typeof(IList).IsAssignableFrom(type))
				debugContent.Append(GetIndent()).Append("]\n");
			else 
				debugContent.AppendFormat(GetIndent()).Append("}\n");
		}
		#endif
		
		public long Size => stream.Position;
		public long Capacity => stream.Length;
		public byte[] Data
		{
			get
			{
				Flush();
#if WritableDebugging
				debugContent.ToString().LogMessage();
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
			OnWrite(writableInstance, value);
#endif
			
			if (writer.TryWritePrimitive(value))
			{
#if WritableDebugging
				AfterWrite(writableInstance);
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
				AfterWrite(writableInstance);
#endif
				return;
			}

			GenericWritable<TReader, TWriter>.IHandler<T> handler = GenericWritable<TReader, TWriter>.GetWritableSerializer<T>();
			handler.Write(tWriter, value);
#if WritableDebugging
			AfterWrite(writableInstance);
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
