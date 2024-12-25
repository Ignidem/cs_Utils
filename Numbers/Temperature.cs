using System;

namespace Utils.Numbers
{
	public readonly struct Temperature
	{
		public enum Type
		{
			Celcius,
			Fahrenheit,
			//Kelvin,
			//Rankine
			//Reaumur
			//Delisle
			//Newton
			//Rømer
			//Planck
		}

		private readonly Type type;
		private readonly float value;

		public Temperature(Type type, float value)
		{
			this.type = type;
			this.value = value;
		}

		public readonly Temperature To(Type type)
		{
			if (this.type == type)
				return this;

			float value = Get(type);
			return new Temperature(type, value);
		}

		public readonly float Get(Type type)
		{
			if (this.type == type)
				return value;

			return this.type switch
			{
				Type.Celcius => CelciusTo(type),
				Type.Fahrenheit => FahrenheitTo(type),
				_ => throw new NotImplementedException(),
			};
		}

		private readonly float CelciusTo(Type type)
		{
			return type switch
			{
				Type.Celcius => value,
				Type.Fahrenheit => (value * 9 / 5) + 32,
				_ => throw new NotImplementedException(),
			};
		}
		private readonly float FahrenheitTo(Type type)
		{
			return type switch
			{
				Type.Celcius => (value - 32) * 5 / 9,
				Type.Fahrenheit => value,
				_ => throw new NotImplementedException(),
			};
		}
	}
}
