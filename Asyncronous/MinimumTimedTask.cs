using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Utils.Asyncronous
{
	public readonly struct MinimumTimedTask : IDisposable
	{
		private readonly Stopwatch watch;
		private readonly int minimumTime;

		public MinimumTimedTask(int minimumTime)
		{
			this.minimumTime = minimumTime;
			watch = new Stopwatch();
			watch.Start();
		}

		public readonly void Dispose()
		{
			watch.Stop();
		}

		public readonly TaskAwaiter GetAwaiter()
		{
			int delta = (minimumTime - (int)watch.ElapsedMilliseconds);
			Task task = delta > 0 ? Task.Delay(delta) : Task.CompletedTask;
			watch.Restart();
			return task.GetAwaiter();
		}
	}
}
