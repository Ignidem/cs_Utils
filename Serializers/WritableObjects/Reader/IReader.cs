namespace Utils.Serializers.WritableObjects
{
	public interface IReader
	{
		T Read<T>();
	}
}
