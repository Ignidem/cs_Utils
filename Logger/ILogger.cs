using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Logger
{
	public interface ILogger
	{
		void Log(Severity severity, string content);
		void Log(Exception e);
	}
}
