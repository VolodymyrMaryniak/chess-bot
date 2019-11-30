using Chess.Engine.Abstract;
using Chess.Engine.Enums;
using Chess.Engine.Game;
using Chess.Engine.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Chess.MinimaxBot.PrimitiveBot
{
	public class PrimitiveBot : IChessBot
	{
		private readonly GameStateRatingCalculator _gameStateRatingCalculator;
		private readonly Stopwatch _stopwatch;
		private readonly Random _random;

		public PrimitiveBot()
		{
			_gameStateRatingCalculator = new GameStateRatingCalculator();
			_stopwatch = new Stopwatch();
			_random = new Random();
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
			var gameStateRatingsToCalculate = SelectGameStateRatings(new List<GameStateRating> {gameStateRating}, deep).ToList();
			if (!gameStateRatingsToCalculate.Any())
				return false;

			foreach (var item in gameStateRatingsToCalculate)
			{
				if (_stopwatch.Elapsed > TimeSpanForSearching)
					return false;

				CalculateChildGameStateRatings(item);
			}

			return true;
		}

		private List<GameStateRating> SelectGameStateRatings(List<GameStateRating> gameStateRatings, int deep)
		{
			var gameStateRatingsToCalculate = gameStateRatings.Where(x => !x.DoNotCalculate);

			if (deep == 0)
			{
				return gameStateRatingsToCalculate
					.SelectMany(x => x.PossibleMoves.Select(m => m.Value))
					.ToList();
			}
			else
			{
				var gameStateRatingsToSelect = gameStateRatingsToCalculate
					.SelectMany(x => x.PossibleMoves.Select(m => m.Value))
					.ToList();

				return SelectGameStateRatings(gameStateRatingsToSelect, deep - 1);
			}
		}

		private void CalculateChildGameStateRatings(GameStateRating gameStateRating)
		{
			gameStateRating.PossibleMoves = gameStateRating.GameState.PossibleGameMoves
				.Select(move =>
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

			if (gameStateRating.PossibleMoves.Count >= 2)
				return;

			foreach (var item in gameStateRating.PossibleMoves.Values)
			{
				item.DoNotCalculate = true;
			}
		}

		private GameMove CalculateTheBestMove(GameStateRating gameStateRating)
		{
			var availableMovesRatings = gameStateRating.PossibleMoves
				.Select(move => new
				{
					Rating = GetTheBestMoveRating(move.Value),
					Move = move.Key
				}).ToList();

			var rating = gameStateRating.GameState.Turn == ChessColor.White
				? availableMovesRatings.OrderByDescending(x => x.Rating).First().Rating
				: availableMovesRatings.OrderBy(x => x.Rating).First().Rating;

			var bestMoves = availableMovesRatings.Where(x => x.Rating == rating).Select(x => x.Move).ToList();
			return bestMoves[_random.Next(bestMoves.Count)];
		}

		private int GetTheBestMoveRating(GameStateRating gameStateRating)
		{
			if (gameStateRating.PossibleMoves == null || !gameStateRating.PossibleMoves.Any())
				return gameStateRating.Rating;

			return gameStateRating.GameState.Turn == ChessColor.White
				? gameStateRating.PossibleMoves.Select(x => GetTheBestMoveRating(x.Value)).Max()
				: gameStateRating.PossibleMoves.Select(x => GetTheBestMoveRating(x.Value)).Min();
		}
	}
}
