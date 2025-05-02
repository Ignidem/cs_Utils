using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Numbers
{
	public static class Functions
	{
		[Obsolete]
		public static double BoxedExponent(this double value, double height, double width, double curve)
		{
			return Exponent(value, width, height, curve);
		}
		/// <summary>
		/// Exponential function within ranges of [0-max].
		/// </summary>
		/// <param name="x">Input x value in range [0,<paramref name="xMax"/>]</param>
		/// <param name="xMax">The maximum expected x value</param>
		/// <param name="yMax">The maximum returned y value</param>
		/// <param name="curve">Adjusts the curvature of the function with ranges [0,1[ U ]1,oo[.
		/// Under 1: Higher Y distribution /-.
		/// Near 1: Linear /, 1 is converted to 1.00001.
		/// Above 1: Lower Y distribution _/.
		/// </param>
		/// <returns>Output y value in range [0,<paramref name="yMax"/>]</returns>
		public static double Exponent(this double x, double xMax, double yMax, double curve)
		{
			if (curve == 1)
				curve = 1.00001;

			double a = yMax / (Math.Pow(curve, xMax) - 1);
			return (a * Math.Pow(curve, x)) - a;
		}

		[Obsolete]
		public static double BoxedCubic(this double value, double height, double width)
		{
			return Cubic(value, width, height);
		}
		/// <summary>
		/// Exponential function within ranges of [0-max].
		/// </summary>
		/// <param name="x">Input x value in range [0-<paramref name="xMax"/>]</param>
		/// <param name="xMax">The maximum expected x value</param>
		/// <param name="yMax">The maximum returned y value</param>
		/// <returns>Output y value in range [0-<paramref name="yMax"/>]</returns>
		public static double Cubic(this double x, double xMax, double yMax)
		{
			double w = xMax / 2;
			double a = yMax / (2 * Math.Pow(w, 3));
			return a * Math.Pow(x - w, 3) + (yMax / 2);
		}

		/// <summary>
		/// Logarithmic function within the ranges of [0-max].
		/// </summary>
		/// <param name="x">Input x value in range [0-<paramref name="xMax"/>]</param>
		/// <param name="xMax">The maximum expected x value</param>
		/// <param name="yMax">The maximum returned y value</param>
		/// <param name="curve">Adjusts the curviture of the function</param>
		/// <returns>Output y value in range [0-<paramref name="yMax"/>]</returns>
		public static double Log(this double x, double xMax, double yMax, double curve)
		{
			double a = yMax / Math.Log(curve + 1);
			double b = curve / xMax;
			return a * Math.Log((b * x) + 1);
		}

		public static double WeightedCubicExponent(this double value, double weight, double yMax, double xMax, double curve)
		{
			double exp = Exponent(value, xMax, yMax, curve);
			double cube = Cubic(value, xMax, yMax);

			return (1 - weight) * exp + (weight * cube);
		}
	}
}
