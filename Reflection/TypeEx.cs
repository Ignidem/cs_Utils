namespace Utilities.Reflection
{
	public static class TypeEx
	{
		public static bool IsStruct(this Type type)
			=> type.IsValueType && !type.IsPrimitive && !type.IsEnum;

		public static bool Implements<T>(this Type type)
			=> Implements(type, typeof(T));

		public static bool Implements(this Type type, Type otherType)
			=> otherType.IsInterface ? otherType.IsAssignableFrom(type) : type.IsSubclassOf(otherType);
	}
}
