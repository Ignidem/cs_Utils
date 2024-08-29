namespace Utils.Serializers.WritableObjects
{
	public interface IWriter
	{
		int Size { get; }
		int Capacity { get; }
		void Write<T>(T value);
	}
}
