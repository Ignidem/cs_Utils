using System;

namespace Utils.Numbers.Vectors.VectorInt
{
	[Serializable]
	public struct Vector3
	{
		public static implicit operator Vector3(VectorByte.Vector3 vbyte)
		{
			return new Vector3(vbyte.x, vbyte.y, vbyte.z);
		}

		public static readonly Vector3 one = new Vector3(1, 1, 1);
		public static readonly Vector3 zero = new Vector3();

		#region Add/Sub
		public static Vector3 operator -(Vector3 a)
		{
			return new Vector3(-a.x, -a.y, -a.z);
		}
		public static Vector3 operator -(Vector3 a, Vector3 b)
		{
			return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
		}
		public static Vector3 operator +(Vector3 a, Vector3 b)
		{
			return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
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

		public int x;
		public int y;
		public int z;

		public Vector3(int x = 0, int y = 0, int z = 0)
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
