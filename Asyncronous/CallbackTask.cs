using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Utils.Asyncronous
{
	public class CallbackTask<T>
	{
		public T Result { get; private set; }

		private bool running = true;

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
			while (running)
			{
				if (millisecondDelay <= 0)
					await Task.Yield();
				else
					await Task.Delay(millisecondDelay);
			}

			return Result;
		}
	}
}
