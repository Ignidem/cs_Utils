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
	}
}
