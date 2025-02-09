using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Utils.Asyncronous
{
	public class CallbackTask<T>
	{
		public T Result { get; private set; }

		private bool running = true;
		private readonly int msTimeout;

		public CallbackTask(int msTimeout = 0)
		{
			this.msTimeout = msTimeout;
		}

		public void Complete(T value)
		{
			Result = value;
			running = false;
		}

		public TaskAwaiter<T> GetAwaiter()
		{
			return AwaitResult(10).GetAwaiter();
		}

		public async Task<T> AwaitResult(int millisecondDelay)
		{
			DateTime start = DateTime.Now;
			while (running)
			{
				if (msTimeout > 0)
				{
					TimeSpan time = DateTime.Now - start;
					if (time.TotalMilliseconds > msTimeout)
						return default;
				}

				if (millisecondDelay <= 0)
					await Task.Yield();
				else
					await Task.Delay(millisecondDelay);
			}

			return Result;
		}
	}
}
