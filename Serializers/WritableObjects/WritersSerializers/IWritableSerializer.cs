namespace Utils.Serializers.WritableObjects.WritersSerializers
{
	public interface IWritableSerializer<T, TReader, TWriter>
		where T : IWritable<TWriter>
		where TReader : IReader
		where TWriter : IWriter
	{
		T Read(TReader reader);
		void Write(TWriter reader, T value);
	}
}
