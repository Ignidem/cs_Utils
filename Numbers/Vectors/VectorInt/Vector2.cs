using System;
using Utils.Serializers.WritableObjects;

namespace Utils.Numbers.Vectors.VectorInt
{
	[Serializable]
	public struct Vector2 : IWritable
	{
		public static readonly Vector2 one = new Vector2(1, 1);
		public static readonly Vector2 zero = new Vector2();

		public static Vector2 operator -(Vector2 a)
		{
			return new Vector2(-a.x, -a.y);
		}
		public static Vector2 operator -(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x - b.x, a.y - b.y);
		}

		#region Equality
		public static bool operator !=(Vector2 a, Vector2 b)
		{
			return a.x != b.x || a.y != b.y;
		}
		public static bool operator ==(Vector2 a, Vector2 b)
		{
			return a.x == b.x && a.y == b.y;
		}
		#endregion

		public int x;
		public int y;

		public Vector2(int x = 0, int y = 0)
		{
			this.x = x;
			this.y = y;
		}
		public Vector2(IReader reader)
		{
			this.x = reader.Read<int>();
			this.y = reader.Read<int>();
		}
		public readonly void Write(IWriter writer)
		{
			writer.Write(x);
			writer.Write(y);
		}

		public readonly override bool Equals(object obj)
		{
			return obj is Vector2 v && this == v
				||
				obj is Vector3 v3 && v3.z == 0 && x == v3.x && y == v3.y;
		}
		public readonly override int GetHashCode()
		{
			return HashCode.Combine(x, y);
		}

		public override readonly string ToString()
		{
			return $"({x}, {y})";
		}
	}
}
