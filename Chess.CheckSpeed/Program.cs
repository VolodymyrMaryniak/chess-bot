using Chess.CheckSpeed.Logger;
using Chess.CheckSpeed.SpeedTests;
using Chess.MinimaxBot;
using System;

namespace Chess.CheckSpeed
{
	public static class Program
	{
		private static readonly ILogger _logger = new FileLogger(true);

		public static void Main(string[] args)
		{
			CheckFirstMoveTime();

			PressKeyToExit();
		}

		private static void CheckFirstMoveTime()
		{
			var firstMoveTimeTests = new FirstMoveTimeTests(_logger);

			firstMoveTimeTests.CheckFirstMoveTime(new PrimitiveBot(1));
			firstMoveTimeTests.CheckFirstMoveTime(new PrimitiveBot(2));
			firstMoveTimeTests.CheckFirstMoveTime(new PrimitiveBot(3));
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
