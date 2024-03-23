using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Numbers
{
	public static class ValueByteConversions
	{
		public static byte[] ToBytes(this IList<bool> values)
		{
			BitArray bits = new BitArray(values.Count);
			for (int i = 0; i < values.Count; i++)
			{
				bits[i] = values[i];
			}

			byte[] byteArray = new byte[1];
			bits.CopyTo(byteArray, 0);
			return byteArray;
		}
		public static bool[] ToBooleans(this byte[] bytes)
		{
			BitArray bits = new BitArray(bytes);
			bool[] bools = new bool[bits.Length];
			for (int i = 0; i < bits.Length; i++)
			{
				bools[i] = bits[i];
			}

			return bools;
		}
	}
}
