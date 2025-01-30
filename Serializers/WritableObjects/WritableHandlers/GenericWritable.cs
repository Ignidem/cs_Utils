using System;
using System.Collections.Generic;

namespace Utils.Serializers.WritableObjects
{
	public class GenericWritable<TReader, TWriter>
		where TReader : IReader
		where TWriter : IWriter
	{
		public interface IHandler<T> : IWritableHandler
		{
			T Read(TReader reader);
			T ReadType(TReader reader, string name);
			void Write(TWriter reader, T value);
		}

		private static readonly Type writableType = typeof(IWritable<TWriter>);
		private static readonly Dictionary<Type, IWritableHandler> serializers = new();
		public static bool IsWritable(Type type)
		{
			return type.IsEnum || writableType.IsAssignableFrom(type);
		}

		#region T Writable
		public static IHandler<T> GetWritableSerializer<T>()
		{
			Type type = typeof(T);
			if (serializers.TryGetValue(type, out IWritableHandler _serializer))
				return (IHandler<T>)_serializer;

			IHandler<T> serializer = type.IsInterface || type.IsAbstract
				? new ClassWritableHandler<T, TReader, TWriter>()
				: new WritableHandler<T, TReader, TWriter>();

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
