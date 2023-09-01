using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Reflection
{
	public static class Cloning
	{
		private const string constructorClone_MissingConstructorFormat 
			= "Cannot clone using constructors. Type {0} has no constructor taking in a {0}.";

		private static Dictionary<Type, ConstructorInfo> constructorCloneCache = new();

		public static T ConstructorClone<T>(this T item)
		{
			Type baseType = typeof(T);
			Type type = item.GetType();

			if (!constructorCloneCache.TryGetValue(type, out ConstructorInfo cnstr))
				cnstr = constructorCloneCache[type] = type.GetConstructor(new Type[] { baseType });

			if (cnstr == null)
			{
				string message = string.Format(constructorClone_MissingConstructorFormat, type.Name, baseType.Name);
				throw new Exception(message);
			}

			return (T)cnstr.Invoke(new object[] { item });
		}
	}
}
