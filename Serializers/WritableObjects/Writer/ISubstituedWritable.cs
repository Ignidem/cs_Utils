namespace Utils.Serializers.WritableObjects
{
	public interface ISubstituedWritable
	{
		IWritable Substitute { get; }
	}
}
