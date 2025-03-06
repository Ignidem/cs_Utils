using System;
using System.Collections.Generic;

namespace Utilities.Enums
{
	public static class EnumUtils
	{
		public static bool IsSingleBit<T>(T value)
			where T : Enum
		{
			long val = Convert.ToInt64(value);
			return val != 0 && (val & (val - 1)) == 0;
		}
		
		public static T[] GetValues<T>() where T : Enum
		{
			return (T[])Enum.GetValues(typeof(T));
		}

		public static IEnumerable<T> GetFlags<T>(this T flags)
			where T : Enum
		{
			T[] values = GetValues<T>();
			foreach (T flag in Enum.GetValues(typeof(T)))
			{
				if (flags.HasFlag(flag) && IsSingleBit(flag))
					yield return flag;
			}
		}
		
		public static IEnumerable<T> GetFlags<T>(this T flags, params T[] baseValues)
			where T : Enum
		{
			for (int i = 0; i < baseValues.Length; i++)
			{
				T baseValue = baseValues[i];
				if (!flags.HasFlag(baseValue))
					continue;

				yield return baseValue;
			}
		}
	}
}
