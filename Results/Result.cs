using System.Threading.Tasks;

namespace Utils.Results
{
	public interface IResult
	{
		string Message { get; }
		bool HasMessage { get; }
		bool IsSuccess { get; }
	}

	public readonly struct Result<T> : IResult
	{
		public static implicit operator Result<T>(T result) => new Result<T>(result);
		public static implicit operator Result<T>(string message) => new Result<T>(message);
		public static implicit operator bool(Result<T> result) => result.IsSuccess;
		public static implicit operator T(Result<T> result) => result.IsSuccess ? result.value : default;

		public static bool operator true(Result<T> result) => result.IsSuccess;
		public static bool operator false(Result<T> result) => !result.IsSuccess;
		public static Result<T> operator &(Result<T> left, Result<T> right) => !left ? left : right;
		public static Result<T> operator |(Result<T> left, Result<T> right) => left ? left : (right ? right : left);

		public static Result Empty = new Result(false, null);

		public static Task<Result<T>> AsTaskResult(Result<T> result)
		{
			return Task.FromResult(result);
		}

		private readonly T value;
		public bool IsSuccess { get; }
		public string Message { get; }
		public bool HasMessage => !string.IsNullOrEmpty(Message);

		public Result(T value)
		{
			this.value = value;
			IsSuccess = this.value != null;
			Message = null;
		}
		public Result(string message)
		{
			value = default;
			IsSuccess = false;
			Message = message;
		}

		public override string ToString()
		{
			return Message ?? value?.ToString() ?? (IsSuccess ? "Success" : "Failure");
		}
	}

	public readonly struct Result : IResult
	{
		public static implicit operator Result(bool result) => new Result(result, null);
		public static implicit operator Result(string message) => new Result(false, message);
		public static implicit operator bool(Result result) => result.IsSuccess;

		public static bool operator true(Result result) => result.IsSuccess;
		public static bool operator false(Result result) => !result.IsSuccess;
		public static Result operator &(Result left, Result right) => !left ? left : right;
		public static Result operator |(Result left, Result right) => left ? left : (right ? right : left);

		public static Result Empty = new Result(false, null);

		public static Task<Result> AsTaskResult(Result result)
		{
			return Task.FromResult(result);
		}

		public bool IsSuccess { get; }
		public string Message { get; }
		public bool HasMessage => !string.IsNullOrEmpty(Message);

		public Result(bool isSuccess, string message)
		{
			IsSuccess = isSuccess;
			Message = message;
		}

		public override string ToString()
		{
			return Message ?? (IsSuccess ? "Success" : "Failure");
		}
	}
}
