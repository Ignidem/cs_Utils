using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Reflection;

namespace Utils.Logger
{
	public static class LoggerUtils
	{
		public static ILogger Logger = FindLogger();

		private static ILogger FindLogger()
		{
			Type type = typeof(ILogger).GetImplementations().FirstOrDefault() 
				?? throw new Exception("At least on logger must be defined in the application!");

			return (ILogger)Activator.CreateInstance(type);
		}

		public static void LogMessage(this string message)
		{
			if (!string.IsNullOrEmpty(message))
				Logger.Log(Severity.Log, message);
		}

		public static void LogException(this Exception e)
		{
			if (e != null)
				Logger.Log(e);
		}

		public static async void LogException(this Task task)
		{
			if (task == null)
				return;

			if (task.IsFaulted)
			{
				task.Exception.LogException();
				return;
			}

			if (task.IsCompleted || task.IsCanceled)
				return;

			try
			{
				await task;
			}
			catch (Exception e)
			{
				e.LogException();
			}
		}
	}
}
