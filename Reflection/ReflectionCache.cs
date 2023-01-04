using System.Reflection;
using Utilities.Conversions;
using Utilities.Reflection.Members;

namespace Utilities.Reflection
{
	public class ReflectionCache<T> : ReflectionCache where T : class
	{
		private readonly Dictionary<string, Member> instanceMembers;

		public ReflectionCache() : base(typeof(T))
		{
			instanceMembers = new Dictionary<string, Member>();
		}

		internal R? Run<R>(string name, T instance, params object[] parameters)
		{
			object? result = GetMember<MethodInfo>(name, instanceMembers)?
				.Invoke(instance, parameters);

			return result.ConvertTo<R>();
		}

		internal void Run(string name, T instance, params object[] parameters)
			=> GetMember<MethodInfo>(name, instanceMembers)?.Invoke(instance, parameters);

		internal R? GetValue<R>(string name, T instance)
		{
			Member? member = GetMember(name, instanceMembers);

			return member == null ? default : member.Read<R>(instance);
		}

		internal R? SetValue<R>(string name, R value, T instance)
		{
			Member? member = GetMember(name, instanceMembers);
			if (member == null) return default;

			member.Write(instance, value);
			return member.Read<R>(instance);
		}
	}

	public class ReflectionCache
	{
		private readonly Type type;

		private Dictionary<string, Member> staticMembers;

		public ReflectionCache(Type t)
		{
			staticMembers = new Dictionary<string, Member>();

			this.type = t;
		}

		internal M? GetMember<M>(string name, Dictionary<string, Member> dict)
			where M : MemberInfo
		{
			if (dict.TryGetValue(name, out Member? member)) 
				return member.ConvertTo<M>();

			member = type.GetMethod(name);
			if (member != null) dict.Add(name, member);

			return member.ConvertTo<M>();
		}

		internal Member? GetMember(string name, Dictionary<string, Member> dict)
		{
			if (dict.TryGetValue(name, out Member? member))
				return member;

			member = type.GetMember(name).FirstOrDefault();
			if (member != null) dict.Add(name, member);

			return member;
		}

		#region Methods

		internal R? Run<R>(string name, params object[] parameters)
		{
			object? result = GetMember<MethodInfo>(name, staticMembers)?
				.Invoke(null, parameters);
			return result.ConvertTo<R>();
		}

		internal void Run(string name, params object[] parameters)
			=> GetMember<MethodInfo>(name, staticMembers)?.Invoke(null, parameters);
		#endregion

		#region Fields
		internal R? GetValue<R>(string name)
		{
			Member? member = GetMember(name, staticMembers);
			return member == null ? default : member.Read<R>();
		}

		internal R? SetValue<R>(string name, R value)
		{
			Member? field = GetMember(name, staticMembers);
			if (field == null) return default;

			field.Write(value);
			return field.Read<R>(null);
		}
		#endregion
	}
}
