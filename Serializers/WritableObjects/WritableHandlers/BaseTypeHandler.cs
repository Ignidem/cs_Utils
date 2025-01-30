using System;
using System.Linq.Expressions;
using Utils.Logger;

namespace Utils.Serializers.WritableObjects
{
	public abstract class BaseTypeHandler<T, TReader, TWriter> : GenericWritable<TReader, TWriter>.IHandler<T>
		where TReader : IReader
		where TWriter : IWriter
	{
		protected const string nullType = "__null";
		protected delegate T Constructor(TReader reader);
		private static readonly Type[] argType = new Type[] { typeof(TReader) };
		protected static Constructor CreateConstructor(Type type)
		{
			System.Reflection.ConstructorInfo cnt = type.GetConstructor(argType);
			if (cnt == null)
			{
				Exception exception = new NullReferenceException($"{type.Name} has no reader constructor");
				exception.LogException();
				return null;
			}

			ParameterExpression param = Expression.Parameter(argType[0]);
			NewExpression cntr = Expression.New(cnt, param);
			UnaryExpression convert = Expression.Convert(cntr, typeof(T));
			Expression<Constructor> expression = Expression.Lambda<Constructor>(convert, param);
			return expression.Compile();
		}

		public abstract T Read(TReader reader);
		public abstract T ReadType(TReader reader, string name);
		public abstract void Write(TWriter reader, T value);
	}
}
