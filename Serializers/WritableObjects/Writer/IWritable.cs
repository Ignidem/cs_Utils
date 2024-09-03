namespace Utils.Serializers.WritableObjects
{
	public interface IWritable
	{
		void Write(IWriter writer);
	}

	public interface IWritable<TWriter>
		where TWriter : IWriter
	{
		void Write(TWriter writer);
	}
}
