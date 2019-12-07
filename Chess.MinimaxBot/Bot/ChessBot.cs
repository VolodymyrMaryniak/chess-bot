using Chess.Engine.Abstract;
using Chess.Engine.Enums;
using Chess.Engine.Game;
using Chess.Engine.Models;
using Chess.MinimaxBot.Extensions;
using Chess.MinimaxBot.PrimitiveBot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Chess.MinimaxBot.Bot
{
	public class ChessBot : IChessBot
	{
		private readonly GameStateRatingCalculator _gameStateRatingCalculator;
		private readonly Stopwatch _stopwatch;
		private readonly Random _random;

		public ChessBot(GameStateRatingCalculator gameStateRatingCalculator, int fullCalculationLevel, int interestingCalculationLevel)
		{
			_gameStateRatingCalculator = gameStateRatingCalculator;
			FullCalculationLevel = fullCalculationLevel;
			InterestingCalculationLevel = interestingCalculationLevel;
			_stopwatch = new Stopwatch();
			_random = new Random();
		}

		public int PositionsCalculated { get; private set; }
		public GameMove TheBestMove { get; private set; }
		public TimeSpan TimeForSearching { get; set; }

		public int FullCalculationLevel { get; }
		public int InterestingCalculationLevel { get; }

		public void StartSearch(GameState gameState)
		{
			PositionsCalculated = 0;
			_stopwatch.Start();

			var root = new GameStateRating { GameState = gameState };
			PositionsCalculated = _gameStateRatingCalculator.CalculateChildren(root, true, out _);
			if (root.Children.Count > 1)
			{
				try
				{
					Search(root);
				}
				catch (TimeIsUpException) { }
				catch (TreeIsOpenedException) { }
			}

			TheBestMove = CalculateTheBestMove(root);

			_stopwatch.Reset();
		}

		private void Search(GameStateRating root)
		{
			for (var level = 1; level <= FullCalculationLevel; level++)
				CalculateChildren(root, level);

			for (var level = FullCalculationLevel + 1; level <= FullCalculationLevel + InterestingCalculationLevel; level++)
				CalculateInterestingChildren(root, level);

			var fullCalculationLevel = FullCalculationLevel;
			var interestingCalculationLevel = FullCalculationLevel + InterestingCalculationLevel;
			while (true)
			{
				fullCalculationLevel++;
				interestingCalculationLevel++;

				CalculateChildren(root, fullCalculationLevel);
				CalculateInterestingChildren(root, interestingCalculationLevel);
			}
			// ReSharper disable once FunctionNeverReturns
		}

		private void CalculateChildren(GameStateRating gameStateRating, int deep)
		{
			var gameStateRatings = SelectGameStateRatings(gameStateRating, deep);
			foreach (var item in gameStateRatings)
			{
				CalculateChildren(item, true, out var @continue);
				if (@continue) continue;

				GoUp(item);
				CalculateChildren(gameStateRating, deep);
			}
		}

		private void CalculateInterestingChildren(GameStateRating gameStateRating, int deep)
		{
			var gameStateRatings = SelectGameStateRatings(gameStateRating, deep).ToList();
			foreach (var item in gameStateRatings)
			{
				CalculateChildren(item, false, out var @continue);
				if (@continue) continue;

				GoUp(item);
				CalculateInterestingChildren(gameStateRating, deep);
			}
		}

		private void GoUp(GameStateRating gameStateRating)
		{
			var parent = gameStateRating.Parent;
			if (parent == null)
				throw new TreeIsOpenedException();
			var wonRating = _gameStateRatingCalculator.WonRating;
			var rating = gameStateRating.Rating;

			if (rating == wonRating && parent.GameState.Turn == ChessColor.Black ||
			    rating == -wonRating && parent.GameState.Turn == ChessColor.White)
			{
				parent.Children.Remove(gameStateRating);

				if (parent.AllPossibleMovesCalculated)
					return;

				if (parent.Children.Any())
					return;

				parent.Children = null;
				parent.Rating = rating;
			}
			else if (rating == wonRating && parent.GameState.Turn == ChessColor.White ||
			         rating == -wonRating && parent.GameState.Turn == ChessColor.Black)
			{
				parent.Children = null;
				parent.Rating = rating;
			}
			else return;

			if (parent.Parent == null)
			{
				parent.Children = new List<GameStateRating> {gameStateRating};
				throw new TreeIsOpenedException();
			}

			GoUp(parent);
		}

		private void CalculateChildren(GameStateRating gameStateRating, bool calculateAllPossible, out bool @continue)
		{
			PositionsCalculated += _gameStateRatingCalculator.CalculateChildren(gameStateRating, calculateAllPossible, out @continue);
			CheckTime();
		}

		private IEnumerable<GameStateRating> SelectGameStateRatings(GameStateRating gameStateRating, int deep)
		{
			return SelectGameStateRatings(new List<GameStateRating> {gameStateRating}, deep);
		}

		private IEnumerable<GameStateRating> SelectGameStateRatings(IEnumerable<GameStateRating> gameStateRatings, int deep)
		{
			var gameStateRatingsToCalculate = gameStateRatings.Where(x => x.Children != null);

			var gameStateRatingsToSelect = gameStateRatingsToCalculate.SelectMany(x => x.Children);
			return deep == 1
				? gameStateRatingsToSelect
				: SelectGameStateRatings(gameStateRatingsToSelect, deep - 1);
		}

		private GameMove CalculateTheBestMove(GameStateRating gameStateRating)
		{
			var availableMovesRatings = gameStateRating.Children
				.Select(x => new
				{
					Rating = x.GetTheBestMoveRating(),
					x.Move
				}).ToList();

			var movesOrderedByRating = gameStateRating.GameState.Turn == ChessColor.White
				? availableMovesRatings.OrderByDescending(x => x.Rating)
				: availableMovesRatings.OrderBy(x => x.Rating);

			var bestRating = movesOrderedByRating.First().Rating;

			var bestMoves = availableMovesRatings.Where(x => x.Rating == bestRating).Select(x => x.Move).ToList();
			return bestMoves[_random.Next(bestMoves.Count)];
		}

		private void CheckTime()
		{
			if (_stopwatch.Elapsed > TimeForSearching)
				throw new TimeIsUpException();
		}

		protected class TimeIsUpException : Exception { }
		protected class TreeIsOpenedException : Exception { }
	}
}
