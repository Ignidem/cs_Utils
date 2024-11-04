using System;
using System.Collections.Generic;

namespace Utils.Delegates
{
	public static class PropertyDelegatesUtils
	{
		public static IdProperty<TComponent, TValue> GetIdProperty<TComponent, TValue>(
			this Dictionary<Type, IPropertyDelegate> delegates)
		{
			if (!delegates.TryGetValue(typeof(TValue), out IPropertyDelegate prop))
			{
				throw new Exception($"PropertyDelegates for {typeof(TComponent).Name} {typeof(TValue).Name} was not defined.");
			}

			if (prop is not IdProperty<TComponent, TValue> propDelegates)
			{
				throw new Exception($"PropertyDelegates for {typeof(TComponent).Name} {typeof(TValue).Name} is not the expected type.");
			}

			return propDelegates;
		}

		public static Property<TComponent, TValue> GetProperty<TComponent, TValue>(
			this Dictionary<Type, IPropertyDelegate> delegates)
		{
			if (!delegates.TryGetValue(typeof(TValue), out IPropertyDelegate prop))
			{
				throw new Exception($"PropertyDelegates for {typeof(TComponent).Name} {typeof(TValue).Name} was not defined.");
			}

			if (prop is not Property<TComponent, TValue> propDelegates)
			{
				throw new Exception($"PropertyDelegates for {typeof(TComponent).Name} {typeof(TValue).Name} is not the expected type.");
			}

			return propDelegates;
		}
	}
}
