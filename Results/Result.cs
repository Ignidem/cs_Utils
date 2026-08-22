using System;
using System.Threading.Tasks;

namespace Utils.Results
{
	public readonly struct Result<T> : IResult<T>
	{
		public static implicit operator Result<T>(T result) => new Result<T>(result);
		public static implicit operator Result<T>(string message) => new Result<T>(message);
		public static implicit operator Result<T>(Exception e) => new Result<T>(default, e == null, null, e);
		public static implicit operator bool(Result<T> result) => result.IsSuccess;
		public static implicit operator T(Result<T> result) => result.Value;

		public static implicit operator Result(Result<T> result) => new Result(result.IsSuccess, result.Message);
		public static implicit operator Result<T>(Result result) => result.IsSuccess ? new Result<T>(default, true) : new Result<T>(result.Message);

		public static bool operator true(Result<T> result) => result.IsSuccess;
		public static bool operator false(Result<T> result) => !result.IsSuccess;
		public static Result<T> operator &(Result<T> left, Result<T> right) => !left ? left : right;
		public static Result<T> operator &(Result left, Result<T> right) => !left ? left : right;       
		public static Result<T> operator |(Result<T> left, Result<T> right)
		{
			return ResultUtils.FirstSuccessOrWithContent(left, right);
		}

		public static Result<T> Empty = new Result<T>(default(T));

		public static Task<Result<T>> AsTaskResult(Result<T> result)
		{
			return Task.FromResult(result);
		}

		public T Value { get; }
		public bool IsSuccess { get; }
		public string Message { get; }
		public Exception Exception { get; }
		public bool HasMessage => !string.IsNullOrEmpty(Message);

		public Result(T value) : this (value, value != null) { }
		public Result(T value, bool isSuccess) : this (value, isSuccess, null, null) { }
		public Result(string message) : this(default, false, message) { }
		public Result(T value, bool isSuccess, string message = null, Exception e = null)
		{
			IsSuccess = isSuccess;
			Value = value;
			Message = message;
			this.Exception = e;
		}

		public override string ToString()
		{
			return Message ?? Value?.ToString() ?? (IsSuccess ? "Success" : "Failure");
		}
	}

	public readonly struct Result : IResult
	{
		public static implicit operator Result(bool result) => new Result(result, null);
		public static implicit operator Result(string message) => new Result(false, message);
		public static implicit operator Result(Exception e) => new Result(e == null, null, e);
		public static implicit operator bool(Result result) => result.IsSuccess;

		public static bool operator true(Result result) => result.IsSuccess;
		public static bool operator false(Result result) => !result.IsSuccess;
		public static Result operator &(Result left, Result right) => !left ? left : right;
		public static Result operator |(Result left, Result right)
		{
			return ResultUtils.FirstSuccessOrWithContent(left, right);
		}

		public static Result Empty = new Result(false, null);

		public static Task<Result> Task(Result result)
		{
			return System.Threading.Tasks.Task.FromResult(result);
		}

		public bool IsSuccess { get; }
		public string Message { get; }
		
		public Exception Exception { get; }
		
		public bool HasMessage => !string.IsNullOrEmpty(Message);
		
		public Result(bool isSuccess, string message) : this (isSuccess, message, null) { }
		
		public Result(bool isSuccess, string message = null, Exception e = null)
		{
			IsSuccess = isSuccess;
			this.Message = message;
			this.Exception = e;
		}

		public override string ToString()
		{
			return Message ?? (IsSuccess ? "Success" : "Failure");
		}
	}
}
