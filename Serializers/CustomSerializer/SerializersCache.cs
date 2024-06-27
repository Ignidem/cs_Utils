using System;
using System.Collections.Generic;

namespace Utils.Serializers.CustomSerializers
{
	public class SerializersCache
	{
		private readonly Dictionary<Type, ISerializer> serializers = new();

		public ISerializer this[Type type]
		{
			get
			{
				if (!serializers.TryGetValue(type, out ISerializer serializer))
					serializers[type] = serializer = (ISerializer)Activator.CreateInstance(type, new object[0]);

				return serializer;
			}
		}
	}
}
