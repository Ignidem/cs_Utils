using System;
using System.Collections.Generic;

namespace Utilities.Enums
{
	public static class EnumUtils
	{
		public static T[] GetValues<T>() where T : Enum
		{
			return (T[])Enum.GetValues(typeof(T));
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
