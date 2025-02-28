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

		public readonly bool IsElapsed()
		{
			return miliseconds <= 0 || watch.ElapsedMilliseconds >= miliseconds;
		}

		public readonly void Restart() => watch?.Restart();

		public readonly TaskAwaiter GetAwaiter()
		{
			if (miliseconds <= 0) return Task.CompletedTask.GetAwaiter();

			int delta = (miliseconds - (int)watch.ElapsedMilliseconds);
			Task task = delta > 0 ? Task.Delay(delta) : Task.CompletedTask;
			Restart();
			return task.GetAwaiter();
		}
	}
}
