using Chess.Engine.Game;
using Chess.Engine.Models;
using System.Collections.Generic;

namespace Chess.MinimaxBot
{
	public class GameStateRating
	{
		public GameState GameState { get; set; }
		public double Rating { get; set; }
		public Dictionary<GameMove, GameStateRating> PossibleMoves { get; set; }
	}
}
