using System;

namespace Utils.Serializers.WritableObjects
{
	public class WritableHandler<T, TReader, TWriter> : BaseTypeHandler<T, TReader, TWriter>
		where TReader : IReader
		where TWriter : IWriter
	{
		private readonly Constructor cntr;
		private readonly Type type;
		//private readonly Type groupType;
		public WritableHandler()
		{
			type = typeof(T);
			cntr = CreateConstructor(type);
			/*
			if (type.TryGetAttribute(out WritableGroupAttribute group) && group.groupType != type) 
			{
				groupType = group.groupType;
			}
			*/
		}

		public override void Write(TWriter writer, T value)
		{
			if (value is not IWritable<TWriter> writable)
				throw new Exception($"{value.GetType()} is not {nameof(IWritable)}<{nameof(TWriter)}>");

			writable.Write(writer);
		}

		public override T Read(TReader reader)
		{
			/* This may no longer be needed if we can write the type name in Serializer.Write function instead of Writable.Write
			if (groupType != null)
			{
				//Type is written with type name to recognize it while reading interfaces or abstract types.
				//Using NetworkReader means we are reading the type directly and not the type group.
				//So the name won't be used in the type constrcutor. We must read it or it will mess up the readings in the constrcutor.
				string name = reader.Read<string>();
				if (name != type.Name)
				{
					string message = $"Read {name}. Expected type name {type.Name} while reading with group {groupType.Name}.";
					Exception exception = new Exception(message);
					exception.LogException();
					throw exception;
				}
			}
			*/

			return cntr(reader);
		}
		public override T ReadType(TReader reader, string name)
		{
			return cntr(reader);
		}
	}
}
