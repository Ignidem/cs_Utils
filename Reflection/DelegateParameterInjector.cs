using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Utilities.Reflection
{
	public static class DelegateParameterInjector
	{
		public static T CreateInjectedDelegate<T>(this MethodInfo method)
			where T : Delegate
		{
			return (T)CreateInjectedDelegateInternal(method, typeof(T));
		}

		public static Delegate CreateInjectedDelegate(this MethodInfo method, Type delegateType)
		{
			if (!typeof(Delegate).IsAssignableFrom(delegateType))
				throw new ArgumentException("Provided type must be a delegate.", nameof(delegateType));
			
			return CreateInjectedDelegateInternal(method, delegateType);
		}
		
		private static Delegate CreateInjectedDelegateInternal(this MethodInfo method, Type delegateType)
		{
			MethodInfo delegateInvoke = delegateType.GetMethod("Invoke");
			if (delegateInvoke == null)
				throw new Exception($"Delegate Type {delegateType.Name} does not have a 'Invoke' method.");
			
			ParameterExpression[] delegateParams = delegateInvoke.GetParameters()
				.Select(p => Expression.Parameter(p.ParameterType)).ToArray();
			Type[] methodParams = method.GetParameters().Select(p => p.ParameterType).ToArray();

			if (delegateParams.Distinct().Count() != delegateParams.Length)
				throw new InvalidOperationException("Delegate must not have duplicate parameter types.");

			Expression instance = GetInstanceExpression(method, delegateParams);
			List<Expression> paramMap = ParameterExpressions(delegateParams, methodParams, method.IsStatic);

			Expression call = method.IsStatic
				? Expression.Call(method, paramMap)
				: Expression.Call(instance, method, paramMap);

			LambdaExpression lambda = Expression.Lambda(delegateType, call, delegateParams);
			return lambda.Compile();
		}

		private static List<Expression> ParameterExpressions(ParameterExpression[] delegateInputs, Type[] methodParams, bool isStatic)
		{
			ParameterExpression[] inputs = isStatic ? delegateInputs : delegateInputs.Skip(1).ToArray();

			var paramMap = new List<Expression>();
			foreach (Type mParam in methodParams)
			{
				ParameterExpression match = inputs.FirstOrDefault(p => p.Type == mParam);
				if (match == null)
					throw new InvalidOperationException($"No matching delegate parameter found for method parameter of type {mParam}");
				paramMap.Add(match);
			}

			return paramMap;
		}

		private static Expression GetInstanceExpression(this MethodInfo method, ParameterExpression[] delegateParams)
		{
			if (method.IsStatic) return null;
			
			if (delegateParams.Length == 0) 
				throw new InvalidOperationException("Instance methods require a first delegate parameter of or assignable from the declaring type.");

			Type declaringType = method.DeclaringType!;
			ParameterExpression delegateParam = delegateParams[0];

			if (delegateParam.Type == declaringType)
				return Expression.Parameter(declaringType);
			if (delegateParam.Type.IsAssignableFrom(declaringType))
				return Expression.Convert(delegateParam, declaringType);
			
			throw new InvalidOperationException("Instance methods require the first delegate parameter to be or be assignable from the declaring type.");
		}
	}
}