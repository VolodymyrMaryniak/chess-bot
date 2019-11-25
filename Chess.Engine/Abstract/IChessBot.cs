using Chess.Engine.Game;
using Chess.Engine.Models;

namespace Chess.Engine.Abstract
{
	public interface IChessBot
	{
		GameMove GetTheBestMove(GameState gameState);
	}
}
