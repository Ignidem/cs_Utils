using System;
using System.Collections;

namespace Utils.Serializers.WritableObjects
{
	public interface IReader : IDisposable
	{
		long Position { get; }
		T Read<T>();
	}

	public interface IReaderEnumerator : IReader, IEnumerator
	{

	}
}
