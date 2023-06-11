using IBS_Web.External.CS.Utils.Reflection;
using System;
using System.Reflection;
using Utilities.Reflection;

namespace Utilities.Conversions
{
#nullable enable
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

		public static bool TryConvertTo(this object obj, Type type, out object? convertedObj)
		{
			Type currentType = obj.GetType();

			if (currentType == type)
				return (convertedObj = obj) != null;

			return
				TryConvertEnum(obj, type, out convertedObj) ||
				TryChangeType(obj, type, out convertedObj) ||
				TryOperatorCast(obj, type, out convertedObj);
		}

		public static bool TryConvertEnum(this object obj, Type type, out object? convertedObj)
		{
			if (!type.IsEnum)
			{
				convertedObj = null;
				return false;
			}

			try
			{
				Type under = Enum.GetUnderlyingType(type);

				return obj switch
				{
					string str => Enum.TryParse(type, str, true, out convertedObj),
					_ => (obj.GetType().Inherits(under) && obj.TryChangeType(type, out convertedObj)) 
						 ||
						 ((convertedObj = null) != null),
				};
			}
			catch
			{
				convertedObj = null;
				return false;
			}
		}

		private static bool TryOperatorCast(object obj, Type type, out object? convertedObj)
		{
			MethodInfo? method = type.CastingOperator(obj.GetType());

			if (method is null)
			{
				convertedObj = null;
				return false;
			}

			try
			{
				convertedObj = method.Invoke(null, new object[] { obj });
				return true;
			}
			catch
			{
				convertedObj = null;
				return false;
			}
		}

		public static bool TryChangeType(this object obj, Type type, out object convertedObj)
		{
			try
			{
				convertedObj = Convert.ChangeType(obj, type);
				return true;
			}
			catch
			{
				convertedObj = null!;
				return false;
			}
		}
	}
}
