using System;

namespace Utils.Serializers.WritableObjects
{
	public class WritableHandler<T, TReader, TWriter> : BaseTypeHandler<T, TReader, TWriter>
		where TReader : IReader
		where TWriter : IWriter
	{
		private readonly Constructor constructor;
		private readonly Type writerType;
		public WritableHandler()
		{
			Type type = typeof(T);
			writerType = typeof(TWriter);
			constructor = CreateConstructor(type);
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
			return constructor(reader);
		}
		public override T ReadType(TReader reader, string name)
		{
			return constructor(reader);
		}
	}
}
