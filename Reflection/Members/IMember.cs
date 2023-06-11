using System.Reflection;

namespace Utilities.Reflection.Members
{
#nullable enable
	public interface IMember
	{
		MemberTypes MemberType { get; }
		bool CanRead { get; }
		bool CanWrite { get; }

		object? Read(object? instance = null);

		T? Read<T>(object? instance = null);

		void Write(object? value);

		void Write(object instance, object? value);
	}
}
