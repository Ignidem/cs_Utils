using System;
using Utilities.Reflection;

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
			string name = reader.Read<string>();
			return ReadType(reader, name);
		}

		public override T ReadType(TReader reader, string name)
		{
			return readers.Read(name, reader);
		}

		public override void Write(TWriter writer, T value)
		{
			if (value == null)
			{
				writer.Write(nullType);
				return;
			}

			IWritable<TWriter> writable = value switch
			{
				ISubstituedWritable<TWriter> sub => sub.Substitute,
				IWritable<TWriter> _writable => _writable,
				_ => throw new Exception($"{value.GetType()} is not {nameof(IWritable)}<{nameof(TWriter)}>")
			};

			Type type = writable.GetType();
			string name = GetName(type);
			writer.Write(name);
			writable.Write(writer);
		}
	} 
}
