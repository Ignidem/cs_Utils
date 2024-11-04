using System;
using System.Security.Cryptography;

namespace Utilities.Numbers
{
	public static class RandomEx
    {
		public static byte[] Bytes(int length) 
		{
			using (var rng = RandomNumberGenerator.Create())
			{
				byte[] result = new byte[length];
				rng.GetBytes(result, 0, length);
				return result;
			}
		}

		public static int Int(int exclusiveMax) => Int(0, exclusiveMax);
		public static int Int(int min, int exclusiveMax)
		{
			return RandomNumberGenerator.GetInt32(min, exclusiveMax);
		}
		public static float Float(int inclusiveMax) => Float(0, inclusiveMax);
		public static float Float(float min, float inclusiveMax)
		{
			float value = Convert.ToSingle(Bytes(4));
			return value.Remap(0, 1, min, inclusiveMax);
		}

		public static bool Chance(int chance) 
			=> chance > 0 && Int(101) <= chance;
		public static bool Chance(this Random rng, int chance)
            => chance > 0 && rng.Next(101) <= chance;

		public static bool Chance(float chance)
			=> chance > 0 && Float(100) <= chance;
		public static bool Chance(this Random rng, float chance)
            => chance > 0 && (rng.Next(100) + rng.NextDouble()) <= chance;
    }
}
