﻿using System;

namespace Utils.Serializers.CustomSerializers
{
	public class SerializerInvalidTypeException : Exception
	{
		public readonly Type serializerType;
		public readonly Type expectedType;
		public readonly Type receivedType;

		public SerializerInvalidTypeException(Type serializerType, Type expectedType, Type receivedType, string action) 
			: base ($"{serializerType.Name} expected {expectedType?.Name} while {action} {receivedType?.Name}.")
		{
			this.serializerType = serializerType;
			this.expectedType = expectedType;
			this.receivedType = receivedType;
		}
	}

	public class SerializerFailedConversionException : Exception
	{
		public readonly Type serializerType;
		public readonly Type targetType;
		public readonly Type receivedType;

		public SerializerFailedConversionException(
			Type serializerType, Type targetType, Type receivedType, string method) 
			: base($"{serializerType.Name} failed to convert {receivedType?.Name} to {targetType?.Name} using {method}")
		{
			this.serializerType = serializerType;
			this.targetType = targetType;
			this.receivedType = receivedType;
		}
	}
}
