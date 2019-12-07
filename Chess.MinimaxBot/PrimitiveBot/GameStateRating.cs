using Chess.Engine.Game;
using Chess.Engine.Models;
using System.Collections.Generic;

namespace Chess.MinimaxBot.PrimitiveBot
{
	public class GameStateRating
	{
		public GameStateRating Parent { get; set; }
		public GameState GameState { get; set; }
		public int Rating { get; set; }
		public List<GameStateRating> Children { get; set; }
		public bool AllPossibleMovesCalculated { get; set; }
		public GameMove Move { get; set; }

		public override string ToString()
		{
			return Parent != null
				? $"{Parent}->Move={Move},Rating={Rating}"
				: $"Rating={Rating}";
		}
	}
}
