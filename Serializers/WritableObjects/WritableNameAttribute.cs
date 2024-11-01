using System;

namespace Utils.Serializers.WritableObjects
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
	public class WritableNameAttribute : Attribute 
	{
		public readonly string name;
		public WritableNameAttribute(string name)
		{
			this.name = name;
		}
	}
}
