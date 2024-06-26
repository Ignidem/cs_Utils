using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utilities.Collections;

namespace Utilities.Reflection
{
#nullable enable
	public static class SubTypes
    {
        private static readonly Dictionary<Type, Type[]> subTypes = new Dictionary<Type, Type[]>();

        public static Type[] GetSubTypes(this Type type)
        {
            if (subTypes.TryGetValue(type, out Type[]? types)) 
                return types;

            types = type.GetImplementations().ToArray();
            subTypes[type] = types;
            return types;
        }

        public static IEnumerable<Type> GetImplementations(this Type type, params Assembly[] assemblies)
        {
			if (assemblies == null || assemblies.Length == 0)
				assemblies = AppDomain.CurrentDomain.GetAssemblies();

			return assemblies.SelectMany(a => a.GetTypes().Where(t =>
                !t.IsInterface && !t.IsAbstract
                && (t == type || t.Inherits(type)))
			);
        }
    }
}
