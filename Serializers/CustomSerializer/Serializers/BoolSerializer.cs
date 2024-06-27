using System.Collections.Generic;
using Utils.Numbers;

namespace Utils.Serializers.CustomSerializers
{
	public class BoolListSerializer : Serializer<IList<bool>, byte[]>
	{
		protected override byte[] Serialize(IList<bool> input) => input.ToBytes();

		protected override IList<bool> Deserialize(byte[] value) => value.ToBooleans();
	}

	public class BoolSerializer : Serializer<bool, int>
	{
		protected override bool Deserialize(int value)
		{
			return value != 0;
		}

		protected override int Serialize(bool input)
		{
			return input ? 1 : 0;
		}
	}
}

