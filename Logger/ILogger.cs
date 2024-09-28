using System;

namespace Utils.Logger
{
	public interface ILogger
	{
		void Log(Severity severity, string content);
		void Log(Exception e);
	}
}
