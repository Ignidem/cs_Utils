using System;

namespace Utils.Delegates
{
	public class MissingPropertyException : Exception
	{
		private const string messageFormat = "{0} {1} does not have property {2} of type {3}";
		private static string FormatMessage(object component, int id, Type type)
		{
			string typeName = component.GetType().Name;
			string componentName = component.ToString();
			return string.Format(messageFormat, typeName, componentName, id, type.Name);
		}

		public readonly object component;
		public readonly int id;

		public MissingPropertyException(object component, int id, Type type)
			: base(FormatMessage(component, id, type))
		{
			this.component = component;
			this.id = id;
		}
	}
}
