using Chess.Engine.Enums;
using Chess.Engine.Game;
using Chess.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.MinimaxBot.PrimitiveBot
{
	public class GameStateRatingCalculator
	{
		public readonly int WonRating = 1000;

		public int CalculateChildren(GameStateRating gameStateRating, bool calculateAllPossible, out bool @continue)
		{
			var positionsCalculated = 0;
			@continue = true;

			var gameState = gameStateRating.GameState;
			if (gameState.GameStatus == GameStatus.Finished)
				return positionsCalculated;

			gameStateRating.Children = gameStateRating.Children ?? new List<GameStateRating>(gameState.PossibleGameMoves.Count);

			IEnumerable<GameMove> moves;
			if (calculateAllPossible)
			{
				moves = gameState.PossibleGameMoves.Where(m => gameStateRating.Children.All(r => r.Move != m));
				gameStateRating.AllPossibleMovesCalculated = true;
			}
			else
			{
				moves = gameState.InterestingGameMoves;
				gameStateRating.AllPossibleMovesCalculated = gameState.PossibleGameMoves.Count == gameState.InterestingGameMoves.Count;
			}

			foreach (var move in moves)
			{
				positionsCalculated++;

				@continue = AddChildren(gameStateRating, move);
				if (@continue) continue;

				SetExtremumGameRating(gameStateRating);
				break;
			}

			return positionsCalculated;
		}

		public void SetExtremumGameRating(GameStateRating gameStateRating)
		{
			gameStateRating.AllPossibleMovesCalculated = true;
			gameStateRating.Rating = gameStateRating.GameState.Turn == ChessColor.White ? WonRating : -WonRating;
			gameStateRating.Children = null;
		}

		public int GetGameStateRating(GameState gameState)
		{
			if (gameState.GameStatus == GameStatus.Finished)
				return GetGameRatingFromGameResult(gameState.GetGameResult());

			var chessPieces = gameState.Chessboard.ChessPieces;

			var whiteRating = GetChessPiecesRating(chessPieces.Where(x => x.Owner == ChessColor.White).Select(x => x.Type).ToList());
			var blackRating = GetChessPiecesRating(chessPieces.Where(x => x.Owner == ChessColor.Black).Select(x => x.Type).ToList());

			return whiteRating - blackRating;
		}

		private bool AddChildren(GameStateRating gameStateRating, GameMove move)
		{
			var gameStateClone = (GameState)gameStateRating.GameState.Clone();
			gameStateClone.Move(move);
			var rating = GetGameStateRating(gameStateClone);

			var child = new GameStateRating
			{
				Parent = gameStateRating,
				Move = move,
				GameState = gameStateClone,
				Rating = rating
			};

			gameStateRating.Children.Add(child);

			return gameStateClone.GameStatus != GameStatus.Finished || Math.Abs(child.Rating) != WonRating;
		}

		private int GetChessPiecesRating(List<ChessPieceType> chessPieces)
		{
			var rating = chessPieces.Sum(GetChessPieceRating);
			if (chessPieces.Count(x => x == ChessPieceType.Bishop) == 2)
				rating += 1;

			return rating;
		}

		private int GetChessPieceRating(ChessPieceType chessPiece)
		{
			switch (chessPiece)
			{
				case ChessPieceType.King:
					return 0;
				case ChessPieceType.Queen:
					return 9;
				case ChessPieceType.Rook:
					return 5;
				case ChessPieceType.Bishop:
					return 3;
				case ChessPieceType.Knight:
					return 3;
				case ChessPieceType.Pawn:
					return 1;
				default:
					throw new ArgumentOutOfRangeException(nameof(chessPiece), chessPiece, null);
			}
		}

		private int GetGameRatingFromGameResult(ChessGameResult gameResult)
		{
			switch (gameResult)
			{
				case ChessGameResult.WhiteWon:
					return WonRating;
				case ChessGameResult.BlackWon:
					return -WonRating;
				case ChessGameResult.Draw:
					return 0;
				default:
					throw new ArgumentOutOfRangeException(nameof(gameResult), gameResult, null);
			}
		}
	}
}
