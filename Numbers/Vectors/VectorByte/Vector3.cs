using System;

namespace Utils.Numbers.Vectors.VectorByte
{
	[Serializable]
	public struct Vector3
	{
		public static implicit operator Vector3(VectorInt.Vector3 vint)
		{
			return new Vector3((byte)vint.x, (byte)vint.y, (byte)vint.z);
		}

		public static readonly Vector3 one = new Vector3(1, 1, 1);
		public static readonly Vector3 zero = new Vector3();

		#region Add/Sub
		public static Vector3 operator -(Vector3 a, Vector3 b)
		{
			return new Vector3((byte)(a.x - b.x), (byte)(a.y - b.y), (byte)(a.z - b.z));
		}
		public static Vector3 operator +(Vector3 a, Vector3 b)
		{
			return new Vector3((byte)(a.x + b.x), (byte)(a.y + b.y), (byte)(a.z + b.z));
		}
		#endregion

		#region Equality
		public static bool operator !=(Vector3 a, Vector3 b)
		{
			return a.x != b.x || a.y != b.y || a.z != b.z;
		}
		public static bool operator ==(Vector3 a, Vector3 b)
		{
			return a.x == b.x && a.y == b.y && a.z == b.z;
		}
		#endregion

		public byte x;
		public byte y;
		public byte z;

		public Vector3(byte x = 0, byte y = 0, byte z = 0)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public readonly override bool Equals(object obj)
		{
			return obj is Vector3 @int &&
				   x == @int.x &&
				   y == @int.y &&
				   z == @int.z;
		}
		public readonly override int GetHashCode()
		{
			return HashCode.Combine(x, y, z);
		}

		public override readonly string ToString()
		{
			return $"({x}, {y}, {z})";
		}
	}
}
