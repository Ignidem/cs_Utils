using System;

namespace Utils.Serializers.WritableObjects
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
	public class ExcludeWritableAttribute : Attribute { }
}
