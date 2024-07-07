using System;

namespace Utils.Numbers.Vectors.VectorInt
{
	[Serializable]
	public partial struct Vector2Int
	{
		public static readonly Vector2Int one = new Vector2Int(1, 1);
		public static readonly Vector2Int zero = new Vector2Int();

		public static Vector2Int operator -(Vector2Int a)
		{
			return new Vector2Int(-a.x, -a.y);
		}
		public static Vector2Int operator -(Vector2Int a, Vector2Int b)
		{
			return new Vector2Int(a.x - b.x, a.y - b.y);
		}

		#region Equality
		public static bool operator !=(Vector2Int a, Vector2Int b)
		{
			return a.x != b.x || a.y != b.y;
		}
		public static bool operator ==(Vector2Int a, Vector2Int b)
		{
			return a.x == b.x && a.y == b.y;
		}
		#endregion

		public int x;
		public int y;

		public Vector2Int(int x = 0, int y = 0)
		{
			this.x = x;
			this.y = y;
		}

		public readonly override bool Equals(object obj)
		{
			return obj is Vector3Int @int &&
				   x == @int.x &&
				   y == @int.y;
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
