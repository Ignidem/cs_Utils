using System;

namespace Utils.Results
{
	public static class ResultUtils
	{
		public static bool IsFailed(this ref Result result)
		{
			return !result;
		}

		public static bool IsFailed(this Result result, out string message)
		{
			message = result.Message;
			return !result.IsSuccess;
		}

		public static bool IsSuccess<T>(this IResult<T> result, out T value)
		{
			if (result.IsSuccess)
			{
				value = result.Value;
				return true;
			}

			value = default;
			return false;
		}

		public static bool IsSuccess<T>(this IResult<T> result, out T value, out string message)
		{
			message = result.Message;
			return result.IsSuccess(out value);
		}
		
		public static Result<T> ToResult<T>(this IResult result, T value = default)
		{
			return new Result<T>(value, result.IsSuccess, result.Message, result.Exception);
		}

		public static T FirstSuccessOrWithContent<T>(T left, T right)
			where T : IResult
		{
			if (left.IsSuccess) return left;
			if (right.IsSuccess) return right;

			if (left.HasContent()) return left;
			if (right.HasContent()) return right;
			return left;
		}

		public static bool HasContent(this IResult result)
		{
			return result.Exception != null || !string.IsNullOrEmpty(result.Message);
		}

		public static string GetContent(this IResult result)
		{
			if (result.HasMessage) return result.Message;

			Exception root = result.Exception.GetBaseException();
			return root.Message;
		}

		public static void ThrowOrIgnore(this IResult result)
		{
			if (result.Exception != null)
				throw result.Exception;
		}
		
		public static Exception ToException(this IResult result)
		{
			if (result.Exception != null)
			{
				return result.Exception;
			}
			else if (!string.IsNullOrEmpty(result.Message))
			{
				return new Exception(result.Message);
			}
			else
			{
				return new Exception(result.ToString());
			}
		}
	}
}