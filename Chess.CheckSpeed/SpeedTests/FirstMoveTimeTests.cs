using Chess.CheckSpeed.Abstract;
using Chess.CheckSpeed.Logger;
using Chess.Engine.Abstract;
using Chess.Engine.Game;

namespace Chess.CheckSpeed.SpeedTests
{
	public class FirstMoveTimeTests : ChessBotSpeedTestBase
	{
		public FirstMoveTimeTests(ILogger logger) : base(logger)
		{
		}

		public void CheckFirstMoveTime(IChessBot chessBot)
		{
			var gameState = GameState.CreateNewGameState();

			TestStarted(chessBot);
			chessBot.GetTheBestMove(gameState);
			TestFinished(chessBot);
		}
	}
}
