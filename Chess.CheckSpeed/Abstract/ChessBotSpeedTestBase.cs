using Chess.CheckSpeed.Logger;
using Chess.Engine.Abstract;
using System.Diagnostics;

namespace Chess.CheckSpeed.Abstract
{
	public class ChessBotSpeedTestBase : ISpeedTest
	{
		protected readonly Stopwatch Stopwatch;
		protected readonly ILogger Logger;

		public ChessBotSpeedTestBase(ILogger logger)
		{
			Stopwatch = new Stopwatch();
			Logger = logger;
		}

		protected void TestStarted(IChessBot chessBot)
		{
			Stopwatch.Reset();
			Logger.Log($"{GetType()} Started | {chessBot}");
			Stopwatch.Start();
		}

		protected void TestFinished(IChessBot chessBot)
		{
			Stopwatch.Stop();
			Logger.Log($"{GetType()} Finished | {chessBot} ", Stopwatch.Elapsed);
		}
	}
}
