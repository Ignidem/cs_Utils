using System.Collections;
using System.Collections.Generic;

namespace Utilities.Enumerable
{
	public interface IIndexed<T>
	{
		T this[int i] { get; }
	}

	public class BasicEnumerable<TEnumerable, TValue> : IEnumerable<TValue>
		where TEnumerable : IIndexed<TValue>
	{
		private readonly TEnumerable _value;
		private readonly int count;

		public BasicEnumerable(TEnumerable value, int count) 
		{
			_value = value;
			this.count = value == null ? 0 : count;
		}

		public IEnumerator<TValue> GetEnumerator()
			=> new BasicEnumerator<TEnumerable, TValue>(_value, count);

		IEnumerator IEnumerable.GetEnumerator()
			=> new BasicEnumerator<TEnumerable, TValue>(_value, count);
	}

	public class BasicEnumerator<TEnumerable, TValue> : IEnumerator<TValue>
		where TEnumerable : IIndexed<TValue>
	{
		int index = -1;

		private TEnumerable target;

		public TValue Current => target[index];

		public int Count { get; }

		object System.Collections.IEnumerator.Current => target[index]!;

		public BasicEnumerator(TEnumerable target, int count)
		{
			this.target = target;
			Count = count;
		}

		public void Dispose()
		{
			Reset();
			target = default!;
		}

		public bool MoveNext() => ++index < Count;

		public void Reset()
		{
			index = -1;
		}
	}
}
