using System;
using System.Reflection;

namespace IBS_Web.External.CS.Utils.Reflection
{
#nullable enable
	public static class Operators
	{
		public static class Names
		{
			public const string Equality = "op_Equality";
			public const string Inequality = "op_Inequality";
			public const string GreaterThan = "op_GreaterThan";
			public const string LessThan = "op_LessThan";
			public const string GreaterThanOrEqual = "op_GreaterThanOrEqual";
			public const string LessThanOrEqual = "op_LessThanOrEqual";
			public const string BitwiseAnd = "op_BitwiseAnd";
			public const string BitwiseOr = "op_BitwiseOr";
			public const string Addition = "op_Addition";
			public const string Subtraction = "op_Subtraction";
			public const string Division = "op_Division";
			public const string Modulus = "op_Modulus";
			public const string Multiply = "op_Multiply";
			public const string LeftShift = "op_LeftShift";
			public const string RightShift = "op_RightShift";
			public const string ExclusiveOr = "op_ExclusiveOr";
			public const string UnaryNegation = "op_UnaryNegation";
			public const string UnaryPlus = "op_UnaryPlus";
			public const string LogicalNot = "op_LogicalNot";
			public const string OnesComplement = "op_OnesComplement";
			public const string Increment = "op_Increment";
			public const string Decrement = "op_Decrement";

			public const string Implicit = "op_Implicit";
			public const string Explicit = "op_Explicit";
		}

		public static MethodInfo? CastingOperator(this Type from, Type to)
			=> InternalGetConverterOperator(from, to, true)
			?? InternalGetConverterOperator(to, from, false);

		private static MethodInfo? InternalGetConverterOperator(Type from, Type to, bool checkInFrom)
		{
			const BindingFlags flags = BindingFlags.Static | BindingFlags.Public;

			bool IsTargetOp(MethodInfo method)
			{
				if (method.Name is not Names.Implicit and not Names.Explicit)
					return false;

				if (method.ReturnType != to)
					return false;

				ParameterInfo[] parameters = method.GetParameters();

				return parameters.Length == 1 && parameters[0].ParameterType == from;
			}

			MethodInfo[] methods = (checkInFrom ? from : to).GetMethods(flags);
			for (int i = 0; i < methods.Length; i++)
			{
				MethodInfo? method = methods[i];
				if (IsTargetOp(method))
					return method;
			}

			return null;
		}
	}
}
