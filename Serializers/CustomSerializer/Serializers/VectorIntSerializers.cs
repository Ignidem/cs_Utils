using System.Numerics;
using Utils.Numbers.Vectors.VectorInt;

namespace Utils.Serializers.CustomSerializers
{
	public class Vector2IntSerializer : Serializer<Numbers.Vectors.VectorInt.Vector2, string>
	{
		protected override Numbers.Vectors.VectorInt.Vector2 Deserialize(string value)
		{
			string[] split = value.Split(',');
			int Parse(int i) => int.TryParse(split[i], out int v) ? v : 0;
			return new Numbers.Vectors.VectorInt.Vector2(Parse(0), Parse(1));
		}

		protected override string Serialize(Numbers.Vectors.VectorInt.Vector2 input)
		{
			return $"{input.x},{input.y}";
		}
	}

	public class Vector3IntSerializer : Serializer<Vector3Int, string>
	{
		protected override Vector3Int Deserialize(string value)
		{
			string[] split = value.Split(',');
			int Parse(int i) => int.TryParse(split[i], out int v) ? v : 0;
			return new Vector3Int(Parse(0), Parse(1), Parse(2));
		}

		protected override string Serialize(Vector3Int input)
		{
			return $"{input.x},{input.y},{input.z}";
		}
	}

	public class Vector2Serializer : Serializer<System.Numerics.Vector2, string>
	{
		protected override System.Numerics.Vector2 Deserialize(string value)
		{
			string[] split = value.Split(',');
			float Parse(int i) => float.TryParse(split[i], out float v) ? v : 0;
			return new System.Numerics.Vector2(Parse(0), Parse(1));
		}

		protected override string Serialize(System.Numerics.Vector2 input)
		{
			return $"{input.X},{input.Y}";
		}
	}

	public class Vector3Serializer : Serializer<Vector3, string>
	{
		protected override Vector3 Deserialize(string value)
		{
			string[] split = value.Split(',');
			float Parse(int i) => float.TryParse(split[i], out float v) ? v : 0;
			return new Vector3(Parse(0), Parse(1), Parse(2));
		}

		protected override string Serialize(Vector3 input)
		{
			return $"{input.X},{input.Y},{input.Z}";
		}
	}
}
