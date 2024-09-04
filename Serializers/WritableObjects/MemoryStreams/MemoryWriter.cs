using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Serializers.WritableObjects.MemoryStreams
{
	public class MemoryWriter : IMemoryWriter, IMemoryReader
	{
		public int Size => throw new NotImplementedException();

		public int Capacity => stream.Capacity;

		private readonly System.IO.MemoryStream stream;

		public MemoryWriter(int capacity)
		{
			stream = new System.IO.MemoryStream(capacity);
		}

		public T Read<T>()
		{
			throw new NotImplementedException();
		}

		public void Write<T>(T value)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			stream.Dispose();
		}
	}
}
