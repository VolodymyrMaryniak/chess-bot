using Chess.CheckSpeed.Logger;
using Chess.CheckSpeed.SpeedTests;
using System;

namespace Chess.CheckSpeed
{
	public static class Program
	{
		private static readonly ILogger _logger = new FileLogger(true);

		public static void Main(string[] args)
		{
			CheckGetAvailableMovesSpeed();

			PressKeyToExit();
		}

		private static void CheckGetAvailableMovesSpeed()
		{
			var getAvailableMovesSpeedTest = new AvailableMovesSpeedTest(_logger);

			getAvailableMovesSpeedTest.CheckCalculatePossibleMovesSpeed();
		}

		private static void PressKeyToExit()
		{
			Console.WriteLine("Press any key to exit.");
			Console.ReadKey();
		}
	}
}
