using Chess.Engine.Game;
using Chess.Engine.Models;
using System.Collections.Generic;

namespace Chess.MinimaxBot.PrimitiveBot
{
	public class GameStateRating
	{
		public GameState GameState { get; set; }
		public int Rating { get; set; }
		public Dictionary<GameMove, GameStateRating> PossibleMoves { get; set; }
		public bool DoNotCalculate { get; set; }
	}
}
