using System;
using System.Collections.Generic;

namespace Utilities.Reflection
{
#nullable enable
	public class TypeCases<TValue> : Cases<Type, TValue>
	{
		public TypeCases<TValue> Set<T>(Value value)
		{
			Set(typeof(T), value);

			return this;
		}

		public TypeCases<TValue> Set<T>(Func<TValue> value)
		{
			Set(typeof(T), value);

			return this;
		}
	}

	public class Cases<TKey, TValue>
	{
		public struct Value
		{
			public static implicit operator Value(TValue value)
				=> new() { value = value };

			public static implicit operator Value(Func<TKey, TValue> getter)
				=> new() { getValue = getter };

			public static implicit operator Value(Func<TValue> getter)
				=> new() { getValueKeyless = getter };

			private TValue value;

			private Func<TKey, TValue> getValue;

			private Func<TValue> getValueKeyless;

			public TValue Get(TKey key)
			{
				return 
					getValue is not null ? getValue(key) :
					getValueKeyless is not null ? getValueKeyless() :
					value;
			}
		}

#pragma warning disable CS8714 //TKey is not "notnull"
		private readonly Dictionary<TKey, Value> _values;
#pragma warning restore CS8714

		private Value? defaultValue;

		public TValue this[TKey? key]
		{
			get
			{
				if (key is null)
					throw new KeyNotFoundException();

				if (_values.TryGetValue(key, out Value value))
					return value.Get(key);

				if (defaultValue is not null)
					return defaultValue.Value.Get(key);

				throw new KeyNotFoundException();
			}

			set
			{
				if (key is null)
					defaultValue = value;
				else
					_values[key] = value;
			}
		}

#pragma warning disable CS8714
		public Cases(Dictionary<TKey, Value> values, Value? defaultValue = null)
#pragma warning restore CS8714
		{
			_values = values;
			this.defaultValue = defaultValue;
		}

		public Cases(Action<Cases<TKey, TValue>>? init = null)
		{
			_values = new();
			init?.Invoke(this);
		}

		public Cases<TKey, TValue> SetDefault(Value value)
		{
			defaultValue = value;

			return this;
		}

		public Cases<TKey, TValue> Set(TKey? key, Value value)
		{
			if (key is null)
				defaultValue = value;
			else
				_values[key] = value;

			return this;
		}
	}
}
