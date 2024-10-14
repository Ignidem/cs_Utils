using System;
using System.Threading.Tasks;

namespace Utils.Asyncronous
{
	public static class TaskUtils
	{
		public static Task TaskOrCompleted(this Task task)
		{
			return task ?? Task.CompletedTask;
		}
		public static Task<T> TaskOrDefaultCompleted<T>(this Task<T> task)
		{
			return task ?? Task.FromResult<T>(default);
		}

		public static Task OnSuccess(this Task task, Action onSuccess)
		{
			AwaitSuccess(task, onSuccess);
			return task;
		}
		private static async void AwaitSuccess(this Task task, Action action)
		{
			if (task == null || action == null) return;

			try
			{
				await task;
				action();
			}
			catch { }
		}

		public static Task<T> OnSuccess<T>(this Task<T> task, Action<T> onSuccess)
		{
			AwaitSuccess(task, onSuccess);
			return task;
		}
		private static async void AwaitSuccess<T>(this Task<T> task, Action<T> action)
		{
			if (task == null || action == null) return;

			try
			{
				T result = await task;
				action(result);
			}
			catch { }
		}

		public static Task OnFailure(this Task task, Action<Exception> onFailure)
		{
			AwaitFailure(task, onFailure);
			return task;
		}
		private static async void AwaitFailure(this Task task, Action<Exception> action)
		{
			if (task == null || action == null) return;

			try
			{
				await task;
			}
			catch (Exception e)
			{
				action(e);
			}
		}
	}
}
