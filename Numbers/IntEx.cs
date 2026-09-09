using System;

namespace External.CSharpUtils.cs_utils.Numbers
{
	public class IntEx
	{
		public static Range GetClampedRange(int totalCount, int windowSize, int selectedIndex)
		{
			if (windowSize <= 0 || totalCount <= 0)
				return default;

			if (selectedIndex < 0 || selectedIndex >= totalCount)
				throw new ArgumentOutOfRangeException(nameof(selectedIndex));

			int center = windowSize / 2; // e.g. windowSize=7 -> center=3
			int start = selectedIndex - center;

			// Clamp so the window fits entirely within [0, totalCount)
			int maxStart = totalCount - windowSize; // always >= 0 since windowSize = Min(7, N)
			int min = Math.Clamp(start, 0, maxStart);
			return new Range(min, min + windowSize);
		}
	}
}