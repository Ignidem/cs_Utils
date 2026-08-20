using System;

namespace Utils.Results
{
	public interface IResult
	{
		string Message { get; }
		Exception Exception { get; }
		bool HasMessage { get; }
		bool IsSuccess { get; }
	}

	public interface IResult<T> : IResult
	{
		T Value { get; }
	}
}
