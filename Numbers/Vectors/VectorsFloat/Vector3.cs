using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Numbers.Vectors.VectorFloat
{
	[Serializable]
	public struct Vector3
	{
		public readonly float this[int i] => i switch 
		{
			0 => x,
			1 => y,
			2 => z,
			_ => throw new IndexOutOfRangeException(nameof(i))
		};

		public float x;
		public float y;
		public float z;

		public Vector3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public override string ToString()
		{
			return string.Format("({0}, {1}, {2})", x, y, z);
		}
	}
}
