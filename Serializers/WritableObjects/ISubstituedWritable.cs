namespace Utils.Serializers.WritableObjects
{
	public interface ISubstituedWritable<T>
		where T : IWriter
	{
		IWritable<T> Substitute { get; }
	}
}
