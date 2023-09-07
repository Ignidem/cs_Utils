﻿namespace Utils.Results
{
	public readonly struct Result
	{
		public static implicit operator Result(bool result) => new Result(result, null);
		public static implicit operator Result(string message) => new Result(false, message);
		public static implicit operator bool(Result result) => result.IsSuccess;

		public static bool operator true(Result result) => result.IsSuccess;
		public static bool operator false(Result result) => !result.IsSuccess;
		public static Result operator &(Result left, Result right) => !left ? left : right;
		public static Result operator |(Result left, Result right) => left ? left : (right ? right : left);

		public readonly bool IsSuccess;
		public readonly string Message;
		public bool HasMessage => !string.IsNullOrEmpty(Message);

		public Result(bool isSuccess, string message)
		{
			IsSuccess = isSuccess;
			Message = message;
		}
	}
}