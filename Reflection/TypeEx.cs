using System;
using System.Reflection;

namespace Utilities.Reflection
{
	public static class TypeEx
	{
		public static bool IsStruct(this Type type)
			=> type != null && type.IsValueType && !type.IsPrimitive && !type.IsEnum;

		public static object GetDefault(this Type type)
		{
			if (type.IsStruct())
				return Activator.CreateInstance(type);

			return null;
		}

		public static bool Inherits<T>(this Type type)
			=> Inherits(type, typeof(T));

		public static bool Inherits(this Type type, Type otherType)
		{
			if (type == null && otherType == null) return true;

			if (type == null || otherType == null) return false;

			if (type == otherType) return true;

			return otherType.IsInterface ? otherType.IsAssignableFrom(type) : type.IsSubclassOf(otherType);
		}

		public static bool TryGetAttribute<T>(this Type type, out T attribute)
			where T : Attribute
		{
#pragma warning disable CS8601 // Possible null reference assignment.
			attribute = Attribute.GetCustomAttribute(type, typeof(T)) as T;
#pragma warning restore CS8601 // Possible null reference assignment.
			return attribute != null;
		}

		public static bool TryGetBase<T>(this Type type, out Type result)
		{
			result = GetBase(type, typeof(T));
			return result != null;
		}

		public static Type GetBase(this Type type, Type targetType)
		{
			Type baseType = type.BaseType;

			if (baseType == null) return null;

			if (targetType == baseType) return baseType;

			if (targetType.IsInterface)
			{
				InterfaceMapping map = type.GetInterfaceMap(targetType);
				return map.TargetType;
			}

			if (targetType.IsGenericType && baseType.GetGenericTypeDefinition() == targetType)
			{
				return baseType;
			}

			return baseType.GetBase(targetType);
		}

		public static Type BuildGeneric(this Type type, params Type[] genericArgs)
		{
			if (type.IsGenericType)
				type = type.GetGenericTypeDefinition();

			if (!type.IsGenericTypeDefinition)
				return type;

			return type.MakeGenericType(genericArgs);
		}
	}
}
