using System;
using System.IO;

namespace Chess.CheckSpeed.Logger
{
	public class FileLogger : ILogger
	{
		private readonly string _filePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\Chess.CheckSpeed.txt";
		private readonly bool _showOnConsole;

		public FileLogger() : this(false){ }

		public FileLogger(bool showOnConsole)
		{
			_showOnConsole = showOnConsole;
		}

		public void Log(string message)
		{
			using (var streamWriter = new StreamWriter(_filePath, true))
			{
				streamWriter.WriteLine(message);
				streamWriter.Close();
			}

			if (_showOnConsole)
				Console.WriteLine(message);
		}

		public void Log(string message, TimeSpan elapsed)
		{
			Log($"{message} | {elapsed}");
		}
	}
}
