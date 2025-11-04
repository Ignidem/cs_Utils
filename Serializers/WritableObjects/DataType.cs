using System;
using System.Collections;

namespace Utils.Serializers.WritableObjects
{
	public enum DataType : byte
	{
		Null,
		Object,
		Array,
		// Primitive types
		Byte,
		Boolean,
		String,
		Int,
		Float,
		Ulong,
	}

	public static class DataTypeUtils
	{
		public static Type GetType(this DataType dataType)
		{
			return dataType switch
			{
				DataType.Null or DataType.Object =>
					throw new Exception("Cannot determine System.Type for " + dataType),
				_ => throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null)
			};
		}

		public static DataType GetDataType(this object value) => GetDataType(value?.GetType());
		public static DataType GetDataType(this Type type)
		{
			if (type == null) return DataType.Null;
			if (type.IsArray || typeof(IList).IsAssignableFrom(type)) return DataType.Array;
			if (type == typeof(string)) return DataType.String;
			if (!type.IsPrimitive) return DataType.Object;
			
			if (type == typeof(byte)) return DataType.Byte;
			if (type == typeof(bool)) return DataType.Boolean;
			if (type == typeof(int)) return DataType.Int;
			if (type == typeof(float)) return DataType.Float;
			if (type == typeof(ulong)) return DataType.Ulong;
			
			throw new NotImplementedException($"System.Type {type.Name} is not mapped to a DataType");
		}
	}
}