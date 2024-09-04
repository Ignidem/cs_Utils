using System.Collections.Generic;

namespace Utils.Serializers.WritableObjects.CollectionStreams
{
	public class CollectionWriter : ICollectionWriter
	{
		public int Size => values.Count;
		public int Capacity => values.Capacity;

		private readonly List<object> values;

		public CollectionWriter(int capacity)
		{
			values = new List<object>(capacity);
		}

		public IReader GetReader()
		{
			return new CollectionReader(values);
		}

		public void Write<T>(T value)
		{
			values.Add(value);
		}

		public void Dispose() { }
	}
}
