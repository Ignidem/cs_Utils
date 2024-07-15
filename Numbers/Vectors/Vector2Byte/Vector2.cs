using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Serializers.WritableObjects;

namespace Utils.Numbers.Vectors.VectorByte
{
	[Serializable]
	public partial struct Vector2 : IWritable
	{
		public static readonly Vector2 one = new Vector2(1, 1);
		public static readonly Vector2 zero = new Vector2();

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

		public byte x;
		public byte y;

		public Vector2(byte x = 0, byte y = 0)
		{
			this.x = x;
			this.y = y;
		}
		public Vector2(IReader reader)
		{
			this.x = reader.Read<byte>();
			this.y = reader.Read<byte>();
		}
		public readonly void Write(IWriter writer)
		{
			writer.Write(x);
			writer.Write(y);
		}

		public readonly override bool Equals(object obj)
		{
			return obj is Vector2 v && x == v.x && y == v.y;
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
