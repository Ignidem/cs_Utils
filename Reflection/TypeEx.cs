using Database.Mongo;
using System;
using System.Linq.Expressions;

namespace Utilities.Reflection
{
	public static class TypeEx
	{
		public static bool IsStruct(this Type type)
			=> type.IsValueType && !type.IsPrimitive && !type.IsEnum;

		public static bool Inherits<T>(this Type type)
			=> Inherits(type, typeof(T));

		public static bool Inherits(this Type type, Type otherType)
			=> otherType.IsInterface ? otherType.IsAssignableFrom(type) : type.IsSubclassOf(otherType);

		public static bool TryGetAttribute<T>(this Type type, out T attribute)
			where T : Attribute
		{
#pragma warning disable CS8601 // Possible null reference assignment.
			attribute = Attribute.GetCustomAttribute(type, typeof(T)) as T;
#pragma warning restore CS8601 // Possible null reference assignment.
			return attribute != null;
		}
	}
}
