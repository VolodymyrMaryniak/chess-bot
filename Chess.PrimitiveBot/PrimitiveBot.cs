using Chess.Core.Enums;
using Chess.Core.Enums.Extensions;
using Chess.Core.Game;
using Chess.Core.Models;
using System;
using System.Linq;

namespace Chess.PrimitiveBot
{
	public class PrimitiveBot
	{
		private readonly GameStateRatingCalculator _gameStateRatingCalculator;
		private readonly int _botLevel;
		public PrimitiveBot(int botLevel)
		{
			_gameStateRatingCalculator = new GameStateRatingCalculator();
			_botLevel = botLevel;
		}

		public GameMove GetTheBestMove(GameState gameState, ChessColor botColor)
		{
			var availableMovesRatings = gameState.GetAvailableMoves()
				.Select(move =>
				{
					var gameStateClone = (GameState) gameState.Clone();
					gameStateClone.Move(move);

					return new
					{
						Rating = GetTheBestMoveRating(gameStateClone, botColor.GetOppositeChessColor(), _botLevel - 1),
						Move = move
					};
				});

			switch (botColor)
			{
				case ChessColor.White:
					return availableMovesRatings.OrderByDescending(x => x.Rating).First().Move;
				case ChessColor.Black:
					return availableMovesRatings.OrderBy(x => x.Rating).First().Move;
				default:
					throw new ArgumentOutOfRangeException(nameof(botColor));
			}
		}

		private double GetTheBestMoveRating(GameState gameState, ChessColor botColor, int deep)
		{
			if (gameState.GameStatus == GameStatus.Finished)
				return GetGameRatingFromGameResult(gameState.GetGameResult());

			var availableMovesRatings = gameState.GetAvailableMoves()
				.Select(move =>
				{
					var gameStateClone = (GameState) gameState.Clone();
					gameStateClone.Move(move);

					return deep == 0
						? _gameStateRatingCalculator.GetGameStateRating(gameStateClone)
						: GetTheBestMoveRating(gameStateClone, botColor.GetOppositeChessColor(), deep - 1);
				});

			switch (botColor)
			{
				case ChessColor.White:
					return availableMovesRatings.Max();
				case ChessColor.Black:
					return availableMovesRatings.Min();
				default:
					throw new ArgumentOutOfRangeException(nameof(botColor));
			}
		}

		private double GetGameRatingFromGameResult(ChessGameResult gameResult)
		{
			switch (gameResult)
			{
				case ChessGameResult.WhiteWon:
					return 100;
				case ChessGameResult.BlackWon:
					return -100;
				case ChessGameResult.Draw:
					return 0;
				default:
					throw new ArgumentOutOfRangeException(nameof(gameResult), gameResult, null);
			}
		}
	}
}
