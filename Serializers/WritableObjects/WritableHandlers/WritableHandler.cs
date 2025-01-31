using System;

namespace Utils.Serializers.WritableObjects
{
	public class WritableHandler<T, TReader, TWriter> : BaseTypeHandler<T, TReader, TWriter>
		where TReader : IReader
		where TWriter : IWriter
	{
		private readonly Constructor cntr;
		private readonly Type type;
		//private readonly Type groupType;
		public WritableHandler()
		{
			type = typeof(T);
			cntr = CreateConstructor(type);
		}

		public override void Write(TWriter writer, T value)
		{
			if (value is not IWritable<TWriter> writable)
				throw new Exception($"{value.GetType()} is not {nameof(IWritable)}<{nameof(TWriter)}>");

			writable.Write(writer);
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
