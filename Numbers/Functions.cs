using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Numbers
{
	public static class Functions
	{
		public static double BoxedExponent(this double value, double height, double width, double curve)
		{
			if (curve == 1)
				curve = 1.00001;

			double a = height / (Math.Pow(curve, width) - 1);
			return (a * Math.Pow(curve, value)) - a;
		}

		public static double BoxedCubic(this double value, double height, double width)
		{
			double w = width / 2;
			double a = height / (2 * Math.Pow(w, 3));
			return a * Math.Pow(value - w, 3) + (height / 2);
		}
	}
}
