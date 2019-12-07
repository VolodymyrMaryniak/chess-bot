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

		public int PositionsCalculated { get; private set; }
		public GameMove TheBestMove { get; private set; }
		public TimeSpan TimeForSearching { get; set; }

		public void StartSearch(GameState gameState)
		{
			PositionsCalculated = 0;
			_stopwatch.Start();

			var gameStateRating = new GameStateRating {GameState = gameState};

			CalculateChildren(gameStateRating);
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

			var startAgain = false;
			foreach (var item in gameStateRatingsToCalculate)
			{
				if (_stopwatch.Elapsed > TimeForSearching)
					return false;

				var oldRating = item.Rating;

				CalculateChildren(item);

				if (oldRating != item.Rating)
				{
					var changed = RecalculateParentRating(item);
					if (changed)
					{
						startAgain = true;
						break;
					}
				}
			}

			if (startAgain)
				return ContinueSearch(gameStateRating, deep);

			return true;
		}

		private bool RecalculateParentRating(GameStateRating gameStateRating)
		{
			var parent = gameStateRating.Parent;
			if (parent == null)
				return false;

			var oldRating = parent.Rating;
			parent.Rating = GetTheBestMoveRating(parent);

			if (oldRating == parent.Rating)
				return false;

			RecalculateParentRating(parent);

			if (parent.Rating == 1000 || parent.Rating == -1000)
			{
				parent.Children = parent.Parent == null
					? parent.Children.Where(x => x.Rating == parent.Rating).ToList()
					: null;

				return true;
			}

			// ???
			return false;
		}

		private IEnumerable<GameStateRating> SelectGameStateRatings(IEnumerable<GameStateRating> gameStateRatings, int deep)
		{
			var gameStateRatingsToCalculate = gameStateRatings.Where(x => x.Children != null);

			var gameStateRatingsToSelect = gameStateRatingsToCalculate.SelectMany(x => x.Children);
			return deep == 0
				? gameStateRatingsToSelect
				: SelectGameStateRatings(gameStateRatingsToSelect, deep - 1);
		}

		protected virtual void CalculateChildren(GameStateRating gameStateRating)
		{
			if (gameStateRating.GameState.GameStatus == GameStatus.Finished)
				return;

			PositionsCalculated++;
			gameStateRating.Children = new List<GameStateRating>(gameStateRating.GameState.PossibleGameMoves.Count);

			int? maxRating = null;
			GameStateRating moveRating = null;
			foreach (var move in gameStateRating.GameState.PossibleGameMoves)
			{
				var gameStateClone = (GameState)gameStateRating.GameState.Clone();
				gameStateClone.Move(move);
				var rating = _gameStateRatingCalculator.GetGameStateRating(gameStateClone);

				moveRating = new GameStateRating
				{
					Parent = gameStateRating,
					Move = move,
					GameState = gameStateClone,
					Rating = rating
				};

				gameStateRating.Children.Add(moveRating);

				if (gameStateClone.GameStatus == GameStatus.Finished && (rating == -1000 || rating == 1000))
				{
					maxRating = rating;
					break;
				}
			}

			if (maxRating.HasValue)
			{
				gameStateRating.Children = gameStateRating.Parent == null
					? new List<GameStateRating> {moveRating}
					: null;
				gameStateRating.Rating = maxRating.Value;
			}
		}

		private GameMove CalculateTheBestMove(GameStateRating gameStateRating)
		{
			var availableMovesRatings = gameStateRating.Children
				.Select(x => new
				{
					Rating = GetTheBestMoveRating(x),
					x.Move
				}).ToList();

			var rating = gameStateRating.GameState.Turn == ChessColor.White
				? availableMovesRatings.OrderByDescending(x => x.Rating).First().Rating
				: availableMovesRatings.OrderBy(x => x.Rating).First().Rating;

			var bestMoves = availableMovesRatings.Where(x => x.Rating == rating).Select(x => x.Move).ToList();
			return bestMoves[_random.Next(bestMoves.Count)];
		}

		private int GetTheBestMoveRating(GameStateRating gameStateRating)
		{
			if (gameStateRating.Children == null || !gameStateRating.Children.Any())
				return gameStateRating.Rating;

			return gameStateRating.GameState.Turn == ChessColor.White
				? gameStateRating.Children.Select(GetTheBestMoveRating).Max()
				: gameStateRating.Children.Select(GetTheBestMoveRating).Min();
		}
	}
}
