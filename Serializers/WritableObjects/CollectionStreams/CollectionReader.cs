using System.Collections.Generic;

namespace Utils.Serializers.WritableObjects.CollectionStreams
{
	public struct CollectionReader : ICollectionReader
	{
		public readonly object Current => values[index];
		private readonly List<object> values;

		private int index;

		public CollectionReader(List<object> objects)
		{
			this.values = objects;
			index = -1;
		}

		public bool MoveNext()
		{
			index++;
			return index < values.Count;
		}

		public T Read<T>()
		{
			MoveNext();
			return (T)Current;
		}

		public void Reset()
		{
			index = -1;
		}

		public void Dispose()
		{
			Reset();
		}
	}
}
