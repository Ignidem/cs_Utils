using System;
using Utilities.Reflection;
using Utils.Logger;

namespace Utils.Serializers.WritableObjects
{
	public class WritableHandler<T, TReader, TWriter> : BaseTypeHandler<T, TReader, TWriter>
		where T : IWritable<TWriter>
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
			value.Write(writer);
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
