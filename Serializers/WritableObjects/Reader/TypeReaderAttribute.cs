using System;
using Utilities.Reflection;

namespace Utils.Serializers.WritableObjects
{
	public class ReaderTypeAttribute : Attribute
	{
		public readonly Type type;
		public readonly bool buildSelf;

		public ReaderTypeAttribute(Type type, bool buildSelf = false)
		{
			if (!type.TryGetAttribute<ExternalReaderAttribute>(out _))
				throw new Exception($"{type} must have {nameof(ExternalReaderAttribute)}");

			this.type = type;
			this.buildSelf = buildSelf;
		}
	}

	/// <summary>
	/// The class is a IWritable but is not used for reader. Another class with the ReaderType attribute is used to reader it.
	/// </summary>
	public class ExternalReaderAttribute : Attribute { }
}
