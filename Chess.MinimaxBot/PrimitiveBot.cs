using Chess.Engine.Abstract;
using Chess.Engine.Enums;
using Chess.Engine.Game;
using Chess.Engine.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Chess.MinimaxBot
{
	public class PrimitiveBot : IChessBot
	{
		private readonly GameStateRatingCalculator _gameStateRatingCalculator;
		private readonly Stopwatch _stopwatch;

		public PrimitiveBot()
		{
			_gameStateRatingCalculator = new GameStateRatingCalculator();
			_stopwatch = new Stopwatch();
		}

		public GameMove TheBestMove { get; private set; }
		public TimeSpan TimeSpanForSearching { get; set; }

		public void StartSearch(GameState gameState)
		{
			_stopwatch.Start();

			var gameStateRating = new GameStateRating
			{
				GameState = gameState,
				Rating = _gameStateRatingCalculator.GetGameStateRating(gameState)
			};

			CalculateChildGameStateRatings(gameStateRating);
			var deep = 0;
			while (ContinueSearch(gameStateRating, deep)) deep++;

			TheBestMove = CalculateTheBestMove(gameStateRating);
			_stopwatch.Reset();
		}

		private bool ContinueSearch(GameStateRating gameStateRating, int deep)
		{
			foreach (var item in SelectGameStateRatings(new List<GameStateRating> {gameStateRating}, deep))
			{
				if (_stopwatch.Elapsed > TimeSpanForSearching)
					return false;

				CalculateChildGameStateRatings(item);
			}

			return true;
		}

		private IEnumerable<GameStateRating> SelectGameStateRatings(IEnumerable<GameStateRating> gameStateRatings, int deep)
		{
			return deep == 0
				? gameStateRatings.SelectMany(x => x.PossibleMoves.Select(m => m.Value))
				: SelectGameStateRatings(gameStateRatings.SelectMany(x => x.PossibleMoves.Select(m => m.Value)), deep - 1);
		}

		private void CalculateChildGameStateRatings(GameStateRating gameStateRating)
		{
			gameStateRating.PossibleMoves = gameStateRating.GameState.PossibleGameMoves.Select(move =>
			{
				var gameStateClone = (GameState) gameStateRating.GameState.Clone();
				gameStateClone.Move(move);

				return new
				{
					Move = move,
					GameState = gameStateClone
				};
			}).ToDictionary(x => x.Move, x => new GameStateRating
			{
				GameState = x.GameState,
				Rating = _gameStateRatingCalculator.GetGameStateRating(x.GameState)
			});
		}

		private GameMove CalculateTheBestMove(GameStateRating gameStateRating)
		{
			var availableMovesRatings = gameStateRating.PossibleMoves.Select(move => new
			{
				Rating = GetTheBestMoveRating(move.Value),
				Move = move.Key
			});

			return gameStateRating.GameState.Turn == ChessColor.White
				? availableMovesRatings.OrderByDescending(x => x.Rating).First().Move
				: availableMovesRatings.OrderBy(x => x.Rating).First().Move;
		}

		private double GetTheBestMoveRating(GameStateRating gameStateRating)
		{
			if (gameStateRating.PossibleMoves == null || !gameStateRating.PossibleMoves.Any())
				return gameStateRating.Rating;

			return gameStateRating.GameState.Turn == ChessColor.White
				? gameStateRating.PossibleMoves.Select(x => GetTheBestMoveRating(x.Value)).Max()
				: gameStateRating.PossibleMoves.Select(x => GetTheBestMoveRating(x.Value)).Min();
		}
	}
}
