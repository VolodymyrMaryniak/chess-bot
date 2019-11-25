using System;

namespace Chess.CheckSpeed.Logger
{
	public interface ILogger
	{
		void Log(string message);
		void Log(string message, TimeSpan elapsed);
	}
}
