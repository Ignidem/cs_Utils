using System;
using System.Threading.Tasks;

namespace Utils.Serializers.CustomSerializers
{
	public interface ISerializer
	{
		Type DeserializedType { get; }
		Type SerializedType { get; }

		object SerializeObject(object input);
		object DeserializeObject(object sqlEntry);

		Task<object> SerializeObjectAsync(object input);
		Task<object> DeserializeObjectAsync(object entry);
	}
}
