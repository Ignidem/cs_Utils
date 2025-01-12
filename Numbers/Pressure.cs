using System;

namespace Utils.Numbers
{
	public readonly struct Pressure
	{
		public enum Type
		{
			Normalized,
			Hectopascal
		}

		private readonly Type type;
		private readonly float value;

		public Pressure(Type type, float value)
		{
			this.type = type;
			this.value = value;
		}

		public readonly Pressure To(Type type)
		{
			if (this.type == type)
				return this;

			float value = Get(type);
			return new Pressure(type, value);
		}

		public readonly float Get(Type type)
		{
			if (this.type == type)
				return value;

			return this.type switch
			{
				Type.Normalized => NormalizedTo(type),
				Type.Hectopascal => HectopascalTo(type),
				_ => throw new NotImplementedException(),
			};
		}

		private readonly float NormalizedTo(Type type)
		{
			return type switch
			{
				Type.Normalized => value,
				Type.Hectopascal => (value * 25f) + 1000f,
				_ => throw new NotImplementedException(),
			};
		}
		private readonly float HectopascalTo(Type type)
		{
			return type switch
			{
				Type.Hectopascal => (value - 32) * 5 / 9,
				Type.Normalized => (value - 1000f) / 25f,
				_ => throw new NotImplementedException(),
			};
		}
	}
}
