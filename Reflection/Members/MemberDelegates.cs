using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Reflection.Members
{
	public static class MemberDelegates
	{
		public static Func<object, object> ToGetDelegate(this FieldInfo field)
		{
			//instance parameter as object
			ParameterExpression instanceExpr = Expression.Parameter(typeof(object), "instance");
			MemberExpression fieldExpr = field.ToExpression(instanceExpr);
			return fieldExpr.ToGetDelegate(instanceExpr);
		}

		public static MemberExpression ToExpression(this FieldInfo field, ParameterExpression instance)
		{
			//convert instance to class/struct type
			UnaryExpression convertInstanceExpr = Expression.Convert(instance, field.DeclaringType);
			return Expression.Field(convertInstanceExpr, field);
		}

		public static MemberExpression ToExpression(this PropertyInfo prop, ParameterExpression instance)
		{
			UnaryExpression convertInstanceExpr = Expression.Convert(instance, prop.DeclaringType);
			return Expression.Property(convertInstanceExpr, prop);
		}

		public static Func<object, object> ToGetDelegate(this MemberExpression expr, ParameterExpression instance)
		{
			UnaryExpression convertReturnExpr = Expression.Convert(expr, typeof(object));
			Expression<Func<object, object>> getter = Expression.Lambda<Func<object, object>>(convertReturnExpr, instance);
			return getter.Compile();
		}

		public static Action<object, object> ToSetDelegate(this MemberExpression expr, ParameterExpression instance)
		{
			//the value to set is as object.
			ParameterExpression value = Expression.Parameter(typeof(object), nameof(value));
			//convert value to the field type
			UnaryExpression convert = Expression.Convert(value, expr.Member.MemberType switch 
			{
				MemberTypes.Property => (expr.Member as PropertyInfo).GetMethod.ReturnType,
				MemberTypes.Field => (expr.Member as FieldInfo).FieldType,
				_ => throw new NotImplementedException()
			});
			//assign the converted value to the field
			BinaryExpression assign = Expression.Assign(expr, convert);
			Expression<Action<object, object>> setter = Expression.Lambda<Action<object, object>>(assign, instance, value);
			return setter.Compile();
		}
	}
}
