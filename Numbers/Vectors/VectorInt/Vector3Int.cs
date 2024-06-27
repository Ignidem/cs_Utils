using System;

namespace Utils.Numbers.Vectors.VectorInt
{
	[Serializable]
	public partial struct Vector3Int
	{
		public static readonly Vector3Int one = new Vector3Int(1, 1, 1);
		public static readonly Vector3Int zero = new Vector3Int();

		#region Add/Sub
		public static Vector3Int operator -(Vector3Int a)
		{
			return new Vector3Int(-a.x, -a.y, -a.z);
		}
		public static Vector3Int operator -(Vector3Int a, Vector3Int b)
		{
			return new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
		}
		public static Vector3Int operator +(Vector3Int a, Vector3Int b)
		{
			return new Vector3Int(a.x + b.x, a.y + b.y, a.z + b.z);
		}
		#endregion

		#region Equality
		public static bool operator !=(Vector3Int a, Vector3Int b)
		{
			return a.x != b.x || a.y != b.y || a.z != b.z;
		}
		public static bool operator ==(Vector3Int a, Vector3Int b)
		{
			return a.x == b.x && a.y == b.y && a.z == b.z;
		}
		#endregion

		public int x;
		public int y;
		public int z;

		public Vector3Int(int x = 0, int y = 0, int z = 0)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public readonly override bool Equals(object obj)
		{
			return obj is Vector3Int @int &&
				   x == @int.x &&
				   y == @int.y &&
				   z == @int.z;
		}
		public readonly override int GetHashCode()
		{
			return HashCode.Combine(x, y, z);
		}
	}
}
