using Chess.Engine.Enums;
using Chess.MinimaxBot.PrimitiveBot;
using System.Linq;

namespace Chess.MinimaxBot.Extensions
{
	public static class GameStateRatingExtensions
	{
		public static int GetTheBestMoveRating(this GameStateRating gameStateRating)
		{
			if (gameStateRating.Children == null || !gameStateRating.Children.Any())
				return gameStateRating.Rating;

			var moveRatings = gameStateRating.Children.Select(GetTheBestMoveRating);

			return gameStateRating.GameState.Turn == ChessColor.White
				? moveRatings.Max()
				: moveRatings.Min();
		}
	}
}
