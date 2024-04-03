using System;
using System.Linq.Expressions;
using System.Reflection;
using Utilities.Conversions;
using Utils.Reflection.Members;

namespace Utilities.Reflection.Members
{
#nullable enable
	public sealed class Member : IMember
	{
		#region To Member
		public static implicit operator Member?(FieldInfo? field)
			=> field == null ? null : new(field);

		public static implicit operator Member?(PropertyInfo? property)
			=> property == null ? null : new(property);

		public static implicit operator Member?(MethodInfo? method)
			=> method == null ? null : new(method);

		public static implicit operator Member?(MemberInfo? member)
		{
			return member?.MemberType switch
			{
				MemberTypes.Method => new((MethodInfo)member),
				MemberTypes.Field => new((FieldInfo)member),
				MemberTypes.Property => new((PropertyInfo)member),

				/* Unsupported:
				MemberTypes.Constructor => ,
				MemberTypes.Event => throw new NotImplementedException(),
				MemberTypes.TypeInfo => throw new NotImplementedException(),
				MemberTypes.Custom => throw new NotImplementedException(),
				MemberTypes.NestedType => throw new NotImplementedException(),
				MemberTypes.All => throw new NotImplementedException(),
				*/
				_ => null,
			};
		}

		#endregion

		#region From Member
		public static implicit operator FieldInfo?(Member? member)
			=> member?.MemberInfo as FieldInfo;

		public static implicit operator PropertyInfo?(Member? member)
			=> member?.MemberInfo as PropertyInfo;

		public static implicit operator MethodInfo?(Member? member)
			=> member?.MemberInfo as MethodInfo;
		#endregion

		#region Expressions
		private static ParameterExpression? GetInstanceParameter(bool isStatic)
		{
			return isStatic ? null : Expression.Parameter(typeof(object), "instance");
		}
		#endregion

		public MemberInfo MemberInfo { get; private set; }
		public MemberTypes MemberType => MemberInfo.MemberType;
		public bool CanRead { get; private set; }
		public bool CanWrite { get; private set; }

		private Action<object?, object?>? setter;
		private Func<object?, object?>? getter;

		private Member(FieldInfo field)
		{
			MemberInfo = field;
			CanRead = true;
			CanWrite = true;

			ParameterExpression? instanceExpr = GetInstanceParameter(field.IsStatic);
			MemberExpression fieldExpr = field.ToExpression(instanceExpr);
			getter = fieldExpr.ToGetDelegate(instanceExpr);
			setter = fieldExpr.ToSetDelegate(instanceExpr);
		}

		private Member(PropertyInfo prop)
		{
			MemberInfo = prop;
			CanRead = prop.CanRead;
			CanWrite = prop.CanWrite;

			ParameterExpression? instanceExpr = GetInstanceParameter(prop.GetMethod.IsStatic);
			MemberExpression fieldExpr = prop.ToExpression(instanceExpr);
			if (CanRead)
				getter = fieldExpr.ToGetDelegate(instanceExpr);
			if (CanWrite)
				setter = fieldExpr.ToSetDelegate(instanceExpr);
		}

		private Member(MethodInfo method)
		{
			MemberInfo = method;
			CanRead = true;
			CanWrite = false;
		}

		public object? Read(object? instance = null)
		{
			if (getter != null)
				return getter(instance);

			return MemberType switch
			{
				MemberTypes.Field => (MemberInfo as FieldInfo)?.GetValue(instance),
				MemberTypes.Property => (MemberInfo as PropertyInfo)?.GetValue(instance),
				MemberTypes.Method => (MemberInfo as MethodInfo)?.Invoke(instance, null),
				_ => null,
			};
		}

		public T? Read<T>(object? instance = null)
			=> Read(instance).ConvertTo<T>();

		public Type? ValueType()
		{
			return MemberType switch
			{
				MemberTypes.Field => (MemberInfo as FieldInfo)?.FieldType,
				MemberTypes.Property => (MemberInfo as PropertyInfo)?.PropertyType,
				_ => null,
			};
		}

		public void Write(object? value)
		{
			if (setter != null)
			{
				setter(null, value);
				return;
			}

			switch (MemberType)
			{
				case MemberTypes.Field:
					(MemberInfo as FieldInfo)?.SetValue(null, value);
					break;
				case MemberTypes.Property:
					(MemberInfo as PropertyInfo)?.SetValue(null, value);
					break;
				case MemberTypes.Method:
					(MemberInfo as MethodInfo)?.Invoke(null, new[] { value });
					break;
			};
		}

		public void Write(object? instance, object? value)
		{
			if (setter != null)
			{
				setter(instance, value);
				return;
			}

			switch (MemberType)
			{
				case MemberTypes.Field:
					(MemberInfo as FieldInfo)?.SetValue(instance, value);
					break;
				case MemberTypes.Property:
					(MemberInfo as PropertyInfo)?.SetValue(instance, value);
					break;
				case MemberTypes.Method:
					(MemberInfo as MethodInfo)?.Invoke(instance, new[] { value });
					break;
			};
		}

		public override string ToString()
			=> MemberInfo.ToString()!;
	}
}
