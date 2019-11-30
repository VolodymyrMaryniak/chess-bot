﻿using Chess.Engine.Enums;
using Chess.Engine.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.MinimaxBot
{
	public class GameStateRatingCalculator
	{
		public double GetGameStateRating(GameState gameState)
		{
			if (gameState.GameStatus == GameStatus.Finished)
				return GetGameRatingFromGameResult(gameState.GetGameResult());

			var chessPieces = gameState.Chessboard.ChessPieces;

			var whiteRating = GetChessPiecesRating(chessPieces.Where(x => x.Owner == ChessColor.White).Select(x => x.Type).ToList());
			var blackRating = GetChessPiecesRating(chessPieces.Where(x => x.Owner == ChessColor.Black).Select(x => x.Type).ToList());

			return whiteRating - blackRating;
		}

		private double GetChessPiecesRating(List<ChessPieceType> chessPieces)
		{
			var rating = chessPieces.Sum(GetChessPieceRating);
			if (chessPieces.Count(x => x == ChessPieceType.Bishop) == 2)
				rating += 1;

			return rating;
		}

		private double GetChessPieceRating(ChessPieceType chessPiece)
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
