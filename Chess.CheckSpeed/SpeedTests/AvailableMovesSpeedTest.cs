using Chess.CheckSpeed.Abstract;
using Chess.CheckSpeed.Logger;
using Chess.Engine.Game;

namespace Chess.CheckSpeed.SpeedTests
{
	public class AvailableMovesSpeedTest : ChessSpeedTestBase
	{
		private const int Times = 100000;
		private static readonly string _testItemName = $"{Times} times";
		public AvailableMovesSpeedTest(ILogger logger) : base(logger)
		{
		}

		public void CheckGetAvailableMovesSpeed()
		{
			var gameState = GameState.CreateNewGameState();

			TestStarted(_testItemName);
			RunTestMethod(gameState);
			TestFinished(_testItemName);
		}

		private void RunTestMethod(GameState gameState)
		{
			for (var i = 0; i < Times; i++)
				gameState.GetAvailableMoves();
		}
	}
}
