using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Utils.Asyncronous
{
	public readonly struct MinimumTimedTask : IDisposable
	{
		private readonly Stopwatch watch;
		private readonly int miliseconds;

		public MinimumTimedTask(int miliseconds)
		{
			this.miliseconds = miliseconds;
			watch = miliseconds > 0 ? new Stopwatch() : null;
			watch?.Start();
		}

		public readonly void Dispose()
		{
			watch?.Stop();
		}

		public readonly TaskAwaiter GetAwaiter()
		{
			if (miliseconds <= 0) return Task.CompletedTask.GetAwaiter();

			int delta = (miliseconds - (int)watch.ElapsedMilliseconds);
			Task task = delta > 0 ? Task.Delay(delta) : Task.CompletedTask;
			watch.Restart();
			return task.GetAwaiter();
		}
	}
}
