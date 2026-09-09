namespace Utilities.Numbers
{
	public static class FloatEx
	{
		public static float Remap(this float value, float min, float max, float tomin, float tomax)
		{
			float r = (tomax - tomin) / (max - min);
			return tomin + (value - min) * r;
		}

		public static float SafeDivide(this float numerator, float denomerator)
		{
			return denomerator == 0 ? 0 : numerator / denomerator;
		}

		public static float GetWidthFromHeight(this float ratio, float height)
		{
			return height * ratio;
		}
		public static float GetHeightFromWidth(this float ratio, float width)
		{
			return width.SafeDivide(ratio);
		}
	}
}
