namespace Utils.Results
{
	public interface IResult
	{
		string Message { get; }
		bool HasMessage { get; }
		bool IsSuccess { get; }
	}

	public interface IResult<T> : IResult
	{

	}
}
