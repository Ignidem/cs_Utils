using System;
using System.Collections.Generic;

namespace Utilities.Numbers
{

	public static class RandomEx
    {
        private static readonly Random rng = new Random();

		public static bool Chance(int chance) => Chance(rng, chance);
        public static bool Chance(this Random rng, int chance)
            => chance > 0 && rng.Next(101) <= chance;

		public static bool Chance(float chance) => Chance(rng, chance);
        public static bool Chance(this Random rng, float chance)
            => chance > 0 && (rng.Next(100) + rng.NextDouble()) <= chance;

        internal static int Int(int v1, int v2)
            => rng.Next(v1, v2 + 1);
    }
}
