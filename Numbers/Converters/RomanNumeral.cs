using System;

namespace Utils.Numbers
{
	public static class RomanNumeral
	{
		// Precomputed strings for every possible digit (0-9) at each place value.
		private static readonly string[] Thousands = { "", "M", "MM", "MMM" };
		private static readonly string[] Hundreds  = { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
		private static readonly string[] Tens      = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
		private static readonly string[] Ones      = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };

		public static string ToRoman(this int number)
		{
			if (number < 1 || number > 3999)
				throw new ArgumentOutOfRangeException(nameof(number), $"Value {number} must be between 1 and 3999.");

			// Direct digit extraction — no loops, no comparisons against value tables.
			string thousands = Thousands[number / 1000];
			string hundreds  = Hundreds[number / 100 % 10];
			string tens      = Tens[number / 10 % 10];
			string ones      = Ones[number % 10];

			return string.Concat(thousands, hundreds, tens, ones);
		}
	}
}