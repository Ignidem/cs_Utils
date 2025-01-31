using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utilities.Collections;
using Utils.Logger;

namespace Utilities.Reflection
{
	public static class SubTypes
    {
        private static readonly Dictionary<Type, Type[]> subTypes = new Dictionary<Type, Type[]>();

        public static Type[] GetSubTypes(this Type type)
        {
            if (subTypes.TryGetValue(type, out Type[] types)) 
                return types;

			types = type.GetImplementations().ToArray();
            subTypes[type] = types;
            return types;
        }

        public static IEnumerable<Type> GetImplementations(this Type type, params Assembly[] assemblies)
		{
			bool isEmpty = assemblies == null || assemblies.Length == 0;
			IEnumerable<Assembly> assembliesCollection = isEmpty ? null : assemblies;
			return GetImplementations(type, assembliesCollection);
		}
        public static IEnumerable<Type> GetImplementations(this Type type, IEnumerable<Assembly> assemblies)
        {
			assemblies ??= AppDomain.CurrentDomain.GetAssemblies();

			IEnumerable<Type> GetTypes(Assembly assembly)
			{
				try
				{
					return assembly.GetTypes();
				}
				catch (Exception e)
				{
					e.LogException();
					return System.Linq.Enumerable.Empty<Type>();
				}
			}

			bool ValidateType(Type t)
			{
				try
				{
					return !t.IsInterface && !t.IsAbstract && (t == type || t.Inherits(type));
				}
				catch (Exception e)
				{
					e.LogException();
					return false;
				}
			}

			return assemblies.SelectMany(GetTypes).Where(ValidateType);
        }
    }
}
