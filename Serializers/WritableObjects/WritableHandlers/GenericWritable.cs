using System;
using System.Collections.Generic;
using Utilities.Reflection;
using Utils.Logger;

namespace Utils.Serializers.WritableObjects
{
	public class GenericWritable<TReader, TWriter>
		where TReader : IReader
		where TWriter : IWriter
	{
		public interface IHandler : IWritableHandler
		{
			object ReadObject(TReader reader);
			void WriteObject(TWriter writer, object value);
		}

		public interface IHandler<T> : IHandler
		{
			object IHandler.ReadObject(TReader reader) => Read(reader);
			T Read(TReader reader);
			T ReadType(TReader reader, string name);

			void IHandler.WriteObject(TWriter writer, object value)
			{
				if (value is not T _value)
					throw new Exception("Type mismatch.");

				Write(writer, _value);
			}
			void Write(TWriter writer, T value);
		}

		private static readonly Type writableType = typeof(IWritable<TWriter>);
		private static readonly Dictionary<Type, IHandler> serializers = new();
		public static bool IsWritable(Type type)
		{
			return type.IsEnum || writableType.IsAssignableFrom(type);
		}

		#region T Writable
		public static void AddWritableSerializer<T>(IHandler<T> handler)
		{
			Type type = typeof(T);
			serializers[type] = handler;
		}
		public static IHandler<T> GetWritableSerializer<T>()
		{
			Type type = typeof(T);
			if (serializers.TryGetValue(type, out IHandler _serializer))
				return (IHandler<T>)_serializer;

			IHandler<T> serializer = type.IsInterface || type.IsAbstract
				? new ClassWritableHandler<T, TReader, TWriter>()
				: CreateWritableHandler<T>();

			serializers[type] = serializer;
			return serializer;
		}

		private static IHandler<T> CreateWritableHandler<T>()
		{
			Type writableType = typeof(IWritable<>).MakeGenericType(typeof(TWriter));
			Type valueType = typeof(T);
			if (!writableType.IsAssignableFrom(valueType))
				throw new Exception($"{valueType} is not IWritable<{typeof(TWriter).Name}>");
			
			if (valueType.IsStruct())
				LoggerUtils.Logger.Log(Severity.Warning, $"Writable Handler {typeof(TWriter)} should not be used with struct value type {valueType}.");

			Type handlerType = typeof(WritableHandler<,,>).MakeGenericType(typeof(T), typeof(TReader), typeof(TWriter));
			return (IHandler<T>)Activator.CreateInstance(handlerType);
		}
		public static IHandler GetWritableSerializer(Type type)
		{
			if (serializers.TryGetValue(type, out IHandler _serializer))
				return _serializer;

			Type reader = typeof(TReader);
			Type writer = typeof(TWriter);

			Type handlerType = type.IsInterface || type.IsAbstract
				? typeof(ClassWritableHandler<,,>)
				: typeof(WritableHandler<,,>);

			handlerType = handlerType.MakeGenericType(type, reader, writer);

			IHandler serializer = (IHandler)Activator.CreateInstance(handlerType);

			serializers[type] = serializer;
			return serializer;
		}
		public static void WriteWritable<T>(TWriter writer, T value)
		{
			IHandler<T> serializer = GetWritableSerializer<T>();
			serializer.Write(writer, value);
		}
		public static T ReadWritable<T>(TReader reader)
		{
			IHandler<T> serializer = GetWritableSerializer<T>();
			return serializer.Read(reader);
		}
		public static Action<TWriter, T> GetWriter<T>()
		{
			if (!IsWritable(typeof(T)))
				return null;

			return WriteWritable;
		}
		public static Func<TReader, T> GetReader<T>()
		{
			if (!IsWritable(typeof(T)))
				return null;

			return ReadWritable<T>;
		}
		#endregion
	}
}
