using System;
using System.Linq;
using System.Reflection;

namespace Utilities.Extensions
{
	public static class AttributeUtils
	{
		public static bool TryGetAttribute<T>(this MemberInfo member, out T attribute)
		{
			Type t = typeof(T);
			if (!typeof(Attribute).IsAssignableFrom(t))
			{
				if (!t.IsInterface)
				{
					attribute = default;
					return false;
				}
				else if (Attribute.GetCustomAttributes(member).FirstOrDefault(_attr => _attr is T) is T attr)
				{
					attribute = attr;
					return true;
				}
			}
			else if (Attribute.GetCustomAttribute(member, t) is T attr)
			{
				attribute = attr;
				return true;
			}

			attribute = default;
			return false;
		}
	}
}
