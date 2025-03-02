using System;
using Utilities.Reflection;

namespace Utils.Serializers.WritableObjects
{
	public class WritableHandler<T, TReader, TWriter> : BaseTypeHandler<T, TReader, TWriter>
		where TReader : IReader
		where TWriter : IWriter
	{
		private readonly Constructor cntr;
		private readonly Type type;
		private readonly Type writerType;
		private readonly Type writableType;
		public WritableHandler()
		{
			type = typeof(T);
			writerType = typeof(TWriter);
			writableType = typeof(IWritable<TWriter>);
			cntr = CreateConstructor(type);
		}

		public override void Write(TWriter writer, T value)
		{
#if UNITY_ANDROID
			try
			{
				((IWritable<TWriter>)value).Write(writer);
				return;
			}
			catch (InvalidCastException) { }
#else
			if (value is IWritable<TWriter> writable)
			{
				writable.Write(writer);
				return;
			}
#endif
			throw new Exception($"{value.GetType()} is not IWritable<{writerType.Name}>");
		}

		public override T Read(TReader reader)
		{
			return cntr(reader);
		}
		public override T ReadType(TReader reader, string name)
		{
			return cntr(reader);
		}
	}
}
