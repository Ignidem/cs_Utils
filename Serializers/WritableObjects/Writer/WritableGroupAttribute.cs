using System;

namespace Utils.Serializers.WritableObjects
{
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct,
		AllowMultiple = false, Inherited = true)]
	public class WritableGroupAttribute : Attribute
	{
		public readonly Type groupType;
		public WritableGroupAttribute(Type groupType)
		{
			this.groupType = groupType;
		}
	}
}
