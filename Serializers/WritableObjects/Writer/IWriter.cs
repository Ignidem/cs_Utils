using System;

namespace Utils.Serializers.WritableObjects
{
	public interface IWriter : IDisposable
	{
		int Size { get; }
		int Capacity { get; }
		void Write<T>(T value);
	}
}
