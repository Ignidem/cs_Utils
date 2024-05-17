namespace Utilities.Numbers
{
	public static class MathfEx
	{
		public static float Remap(this float value, float min, float max, float tomin, float tomax)
		{
			float r = (tomax - tomin) / (max - min);
			return tomin + (value - min) * r;
		}
	}
}
