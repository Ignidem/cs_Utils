namespace Utilities.Conversions
{
	public interface IConvertible<T>
	{
		T Convert();
	}

	public static class ConvertibleUtils
	{
		public static T? ConvertTo<T>(this object? obj)
		{
			if (obj == null) return default;

			if (obj is T r) return r;

			if (obj is IConvertible<T> convertible)
				return convertible.Convert();

			try
			{
				return (T)obj;
			} 
			catch (Exception) { }
			try
			{
				return (T)Convert.ChangeType(obj, typeof(T));
			}
			catch (Exception) { }
			
			return default;
		}
	}
}
