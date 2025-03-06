namespace Utils.Results;

public static class ResultUtils
{
    public static bool IsFailed(this ref Result result) 
    {
        return !result;
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
}