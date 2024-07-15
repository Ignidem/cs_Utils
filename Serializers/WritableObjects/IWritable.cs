using System;

namespace Utils.Serializers.WritableObjects
{
	public interface IWritable
	{
		void Write(IWriter writer);
	}

	public interface ISubstituedWritable
	{
		IWritable Substitute { get; }
	}

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

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
	public class ExcludeWritableAttribute : Attribute { }

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
