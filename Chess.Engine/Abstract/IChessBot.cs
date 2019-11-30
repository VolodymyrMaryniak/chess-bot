using Chess.Engine.Game;
using Chess.Engine.Models;
using System;

namespace Chess.Engine.Abstract
{
	public interface IChessBot
	{
		void StartSearch(GameState gameState);
		GameMove TheBestMove { get; }
		TimeSpan TimeSpanForSearching { set; }
	}
}
