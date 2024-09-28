using System;
using System.Threading.Tasks;

namespace Utils.Asyncronous
{
	public class TaskDelayer
	{
		private readonly int ms;
		private readonly Func<Task> function;

		private bool firstCall;
		private DateTime lastCall;
		private bool rerun;
		private Task delay;
		private Task ongoing;

		public TaskDelayer(int milliseconds, Func<Task> function, bool waitForFirstRun = false)
		{
			this.ms = milliseconds;
			this.function = function;
			firstCall = !waitForFirstRun;
		}

		public void TryRun()
		{
			lastCall = DateTime.Now;

			if (firstCall)
			{
				firstCall = false;
				ongoing = Invoke();
				return;
			}

			if (ongoing != null && !ongoing.IsCompleted)
			{
				rerun = true;
				return;
			}

			if (delay != null && !delay.IsCompleted)
				return;

			delay = DelayInvoke();
		}

		private async Task DelayInvoke()
		{
			while ((DateTime.Now - lastCall).TotalMilliseconds < ms)
			{
				await Task.Delay(ms);
			}

			ongoing = Invoke();
		}

		private async Task Invoke()
		{
			await function();

			if (rerun)
			{
				TryRun();
			}
		}
	}
}
