using Utils.Reflection;
using System;
using System.Reflection;
using Utilities.Reflection;
using Newtonsoft.Json;

namespace Utilities.Conversions
{
#nullable enable
	public static class ConvertibleUtils
	{
		public static bool TryToJson<T>(this T instance, out string json)
		{
			try
			{
				json = JsonConvert.SerializeObject(instance);
				return true;
			}
			catch(Exception) 
			{
				json = null!;
				return false;
			}
		}

		public static bool TryParseJson<T>(this string json, out T result)
		{
			try
			{
				result = JsonConvert.DeserializeObject<T>(json)!;
				return true;
			}
			catch (Exception) { }

			result = default!;
			return false;
		}

		public static bool TryConvertTo<T>(this object? obj, out T result)
		{
			if (obj == null)
			{
				result = default!;
				return false;
			}

			if (obj is T r)
			{
				result = r;
				return true;
			}

			if (obj is IConvertible<T> convertible)
			{
				result = convertible.Convert();
				return true;
			}

			try
			{
				result = (T)obj;
				return true;
			}
			catch (Exception) { }
			try
			{
				result = (T)Convert.ChangeType(obj, typeof(T));
				return true;
			}
			catch (Exception) { }

			result = default!;
			return false;
		}

		public static T ConvertTo<T>(this object? obj)
		{
			return TryConvertTo(obj, out T result) ? result : default!;
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
