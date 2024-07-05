using System;

namespace Utils.Serializers.CustomSerializers
{
	public class GuidSerializer : Serializer<Guid, byte[]>
	{
		protected override bool CanDeserializeNull => true;
		protected override bool CanSerializeNull => true;

		protected override Guid Deserialize(byte[] value)
		{
			return value == null ? default : new Guid(value);
		}

		protected override byte[] Serialize(Guid input)
		{
			return input == default ? null : input.ToByteArray();
		}
	}
}

