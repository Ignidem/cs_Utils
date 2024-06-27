using System;
using System.Collections.Generic;
using System.Numerics;
using Utilities.Reflection;
using Utils.Numbers.Vectors.VectorInt;

namespace Utils.Serializers.CustomSerializers
{
	public class SerializerAttribute : Attribute
	{
		public class _DefaultSerializers
		{
			public SerializerAttribute this[Type type]
			{
				get
				{
					if (_serializers.TryGetValue(type, out SerializerAttribute serializer))
						return serializer;

					if (type.TryGetAttribute(out serializer))
						return DefaultSerializers[type] = serializer;

					if (type.IsEnum)
					{

					}

					return null;
				}

				set
				{
					_serializers[type] = value;
				}
			}

			private Dictionary<Type, SerializerAttribute> _serializers = new()
			{
				[typeof(Vector2Int)] = new SerializerAttribute(typeof(Vector2IntSerializer)),
				[typeof(Vector3Int)] = new SerializerAttribute(typeof(Vector3IntSerializer)),
				[typeof(Vector2)] = new SerializerAttribute(typeof(Vector2Serializer)),
				[typeof(Vector3)] = new SerializerAttribute(typeof(Vector3Serializer)),
				[typeof(Guid)] = new SerializerAttribute(typeof(GuidSerializer)),
				[typeof(bool)] = new SerializerAttribute(typeof(BoolSerializer)),
				[typeof(bool[])] = new SerializerAttribute(typeof(BoolListSerializer)),
			};

			public bool TryGet(Type type, out SerializerAttribute attr)
			{
				return (attr = this[type]) != null;
			}
		}

		public readonly static _DefaultSerializers DefaultSerializers = new();

		private static readonly SerializersCache cache = new();

		public bool IsValid => serializerType != null;

		public readonly Type serializerType;

		public ISerializer Serializer => IsValid ? cache[serializerType] : null;

		public SerializerAttribute(Type deserializedType, Type serializedType) 
		{
			serializerType = typeof(Serializer<,>).MakeGenericType(deserializedType, serializedType);
		}

		public SerializerAttribute(Type serializerType)
		{
			if (!serializerType.Inherits<ISerializer>())
				throw new Exception($"Invalid Serializer " + serializerType);

			this.serializerType = serializerType;
		}
	}
}
