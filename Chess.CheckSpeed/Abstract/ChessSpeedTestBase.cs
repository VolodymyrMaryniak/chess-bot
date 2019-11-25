using Chess.CheckSpeed.Logger;
using System.Diagnostics;

namespace Chess.CheckSpeed.Abstract
{
	public class ChessSpeedTestBase : ISpeedTest
	{
		protected readonly Stopwatch Stopwatch;
		protected readonly ILogger Logger;

		public ChessSpeedTestBase(ILogger logger)
		{
			Stopwatch = new Stopwatch();
			Logger = logger;
		}

		protected void TestStarted(string testItemName)
		{
			Stopwatch.Reset();
			Logger.Log($"{GetType()} Started | {testItemName}");
			Stopwatch.Start();
		}

		protected void TestFinished(string testItemName)
		{
			Stopwatch.Stop();
			Logger.Log($"{GetType()} Finished | {testItemName} ", Stopwatch.Elapsed);
		}
	}
}
