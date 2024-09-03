using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Utilities.Reflection;
using Utils.Logger;

namespace Utils.Serializers.WritableObjects.WritersSerializers
{
	public class Readable<T, TReader>
		where TReader : IReader
	{
		public const string nullType = "__null";
		public delegate T Constructor(TReader reader);
		private static readonly Type[] argType = new Type[] { typeof(TReader) };
		private static readonly Dictionary<string, Constructor> constructors = CreateConstructors();

		private static void LogError(string message)
		{
			LoggerUtils.Logger.Log(Severity.Error, message);
		}
		private static Dictionary<string, Constructor> CreateConstructors()
		{
			Type[] types = typeof(T).GetSubTypes();
			Dictionary<Type, bool> externalReaderTypes = new();
			Dictionary<string, Constructor> constructors = new();
			for (int i = 0; i < types.Length; i++)
			{
				Type type = types[i];
				string name = GetName(type);

				if (type.TryGetAttribute<ExternalReaderAttribute>(out _))
				{
					if (!externalReaderTypes.ContainsKey(type))
						externalReaderTypes[type] = false;

					continue;
				}

				if (type.TryGetAttribute(out ReaderTypeAttribute readerType))
				{
					//This type is used as a reader for another type;
					string subName = GetName(readerType.type);
					AddConstructor(constructors, subName, type);
					externalReaderTypes[readerType.type] = true;

					if (!readerType.buildSelf)
						continue;
				}

				AddConstructor(constructors, name, type);
			}

			return constructors;
		}
		private static string GetName(Type type)
		{
			return type.TryGetAttribute(out WritableNameAttribute naming) ? naming.name : type.Name;
		}
		private static void AddConstructor(Dictionary<string, Constructor> constructors, string name, Type type)
		{
			if (constructors.TryGetValue(name, out var cntr))
			{
				LogError($"Type name of {type.FullName} is already used by {cntr.Target.GetType().FullName}");
			}

			if (type.TryGetAttribute(out ExcludeWritableAttribute _))
				return;

			if (type.IsGenericType)
			{
				LogError($"Generic Type {type.FullName} may mot be used for class serialization " +
					$"and requires the {nameof(ExcludeWritableAttribute)} attribute.");
			}

			Constructor expression = CreateConstructor(type);
			if (expression != null)
				constructors.Add(name, expression);
		}
		private static Constructor CreateConstructor(Type type)
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

		public T Read(string name, TReader reader)
		{
			if (name == nullType)
				return default;

			if (string.IsNullOrEmpty(name))
			{
				Exception exception = new ArgumentNullException($"{typeof(T).Name} 'name' read is null");
				throw exception;
			}

			if (!constructors.TryGetValue(name, out Constructor cnt))
			{
				string message = $"{typeof(T).Name}.{name} is invalid or has no network reader constructor!";
				throw new Exception(message);
			}

			return cnt(reader);
		}
	}
}
