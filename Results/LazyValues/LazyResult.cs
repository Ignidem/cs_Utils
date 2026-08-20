using System;

namespace Utils.Results
{
	public abstract class BaseLazyResult<T> : IResult<T>
	{
		public static implicit operator T(BaseLazyResult<T> lazyResult) => lazyResult.Value;
		public abstract T Value { get; }
		public Exception Exception { get; protected set; }
		public string Message => null;
		public bool HasMessage => false;
		public bool IsSuccess => Value != null;
	}
	
	public class LazyResult<TKey, TValue> : BaseLazyResult<TValue>
	{
	#nullable enable
		public override TValue Value
		{
			get
			{
				if (_value == null)
					_value = factory(key);
				
				return _value;
			}
		}
		private TValue? _value;
	#nullable restore
		
		private readonly TKey key;
		private readonly Func<TKey, TValue> factory;

		public LazyResult(TKey key, Func<TKey, TValue> factory)
		{
			this.key = key;
			this.factory = factory;
		}
	}
	
	public class LazyResult<TValue> : BaseLazyResult<TValue>
	{
#nullable enable
		public override TValue Value
		{
			get
			{
				if (_value == null)
					_value = factory();
				
				return _value;
			}
		}
		private TValue? _value;
#nullable restore
		
		private readonly Func<TValue> factory;

		public LazyResult(Func<TValue> factory)
		{
			this.factory = factory;
		}
	}
}