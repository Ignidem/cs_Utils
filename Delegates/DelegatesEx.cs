using System.Reflection;

namespace Utils.Delegates
{
#nullable disable
	public static class DelegateEx
	{
		public static TDelegate ToDelegate<TDelegate>(this MethodInfo method, object instance = null)
			where TDelegate : Delegate
		{
			return (TDelegate)method.CreateDelegate(typeof(TDelegate), instance);
		}

		public static IEnumerable<(MethodInfo, TAttribute)> GetAttributedMethods<TAttribute>(this object target,
			BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic)
			where TAttribute : Attribute
		{
			Type type = target.GetType();
			MethodInfo[] methods = type.GetMethods(bindingFlags);

			static (MethodInfo, TAttribute) Select(MethodInfo method)
			{
				TAttribute attribute = method.GetCustomAttribute<TAttribute>();

				if (attribute == null)
					return (null, null);

				return (method, attribute);
			}

			static bool NotNull((MethodInfo, TAttribute) result)
			{
				return result.Item1 != null && result.Item2 != null;
			}

			return methods.Select(Select).Where(NotNull);
		}

		public static void ForeachMethodWithAttribute<TAttribute>(this object target,
			Action<MethodInfo, TAttribute> callbacks,
			BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic)
			where TAttribute : Attribute
		{
			Type type = target.GetType();
			MethodInfo[] methods = type.GetMethods(bindingFlags);

			for (int i = 0; i < methods.Length; i++)
			{
				MethodInfo method = methods[i];
				TAttribute attribute = method.GetCustomAttribute<TAttribute>();

				if (attribute == null)
					continue;

				callbacks(method, attribute);
			}
		}
	}
#nullable restore
}

