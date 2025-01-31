using System;

namespace Utils.Serializers.WritableObjects
{
	public interface IWriter : IDisposable
	{
		long Size { get; }
		long Capacity { get; }
		void Write<T>(T value);
		void Flush()
		{
			throw new NotImplementedException(GetType().Name);
		}
	}
}
