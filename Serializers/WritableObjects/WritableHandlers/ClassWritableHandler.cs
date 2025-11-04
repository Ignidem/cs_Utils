using System;
using Utilities.Reflection;
using Utils.Logger;

namespace Utils.Serializers.WritableObjects
{
	public class ClassWritableHandler<T, TReader, TWriter> : BaseTypeHandler<T, TReader, TWriter>
		where TReader : IReader
		where TWriter : IWriter
	{
		private readonly ReadableConstructors<T, TReader> readers = new();

		private static string GetName(Type type)
		{
			return type.TryGetAttribute(out WritableNameAttribute naming) ? naming.name : type.Name;
		}

		public override T Read(TReader reader)
		{
			DataType dataType = (DataType)reader.Read<byte>();
			if (dataType == DataType.Null)
				return default;
			
			string name = reader.Read<string>();
			if (!string.IsNullOrEmpty(name))
				return ReadType(reader, name);
			
			Exception exception = new ArgumentNullException($"{typeof(T).Name} 'name' read is null for DataType {dataType}");
			exception.LogException();
			throw exception;
		}

		public override T ReadType(TReader reader, string name)
		{		
			if (string.IsNullOrEmpty(name))
			{
				Exception exception = new ArgumentNullException($"{typeof(T).Name} 'name' read is null");
				throw exception;
			}
			
			return readers.Read(name, reader);
		}

		public override void Write(TWriter writer, T value)
		{
			if (value == null)
			{
				writer.Write((byte)DataType.Null);
				return;
			}

			IWritable<TWriter> writable = value switch
			{
				ISubstituedWritable<TWriter> sub => sub.Substitute,
				IWritable<TWriter> _writable => _writable,
				_ => throw new Exception($"{value.GetType()} is not {nameof(IWritable)}<{typeof(TWriter).Name}>")
			};

			writer.Write((byte)DataType.Object);
			string name = GetName(writable.GetType());
			writer.Write(name);
			writable.Write(writer);
		}
	} 
}
