using Chess.Engine.Models;

namespace Chess.Engine.Game
{
	public class Game
	{
		private readonly GameState _gameState;
		private readonly Player _whitePlayer;
		private readonly Player _blackPlayer;

		public Game(Player whitePlayer, Player blackPlayer)
		{
			_whitePlayer = whitePlayer;
			_blackPlayer = blackPlayer;

			_gameState = GameState.CreateNewGameState();
		}

		public void Move(GameMove gameMove)
		{
			_gameState.Move(gameMove);
		}
	}
}
