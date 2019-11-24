using Chess.Engine.Enums;
using Chess.Engine.Game;
using Chess.Engine.Logic.ChessPieceMoveValidators.Extensions;
using Chess.Engine.Models;
using System;
using System.Collections.Generic;

namespace Chess.Engine.Logic.ChessPieceMoveValidators
{
	internal class KingMoveValidator
	{
		public List<GameMove> GetSoftValidKingMoves(Chessboard chessboard, Coordinate fromCoordinate, ChessColor kingColor)
		{
			fromCoordinate.ToArrayIndexes(out var i, out var j);

			var softValidMoves = new List<GameMove>();
			softValidMoves.AddIfCoordinateIsValid(chessboard, fromCoordinate, kingColor, i - 1, j - 1);
			softValidMoves.AddIfCoordinateIsValid(chessboard, fromCoordinate, kingColor, i - 1, j);
			softValidMoves.AddIfCoordinateIsValid(chessboard, fromCoordinate, kingColor, i - 1, j + 1);
			softValidMoves.AddIfCoordinateIsValid(chessboard, fromCoordinate, kingColor, i, j - 1);
			softValidMoves.AddIfCoordinateIsValid(chessboard, fromCoordinate, kingColor, i, j + 1);
			softValidMoves.AddIfCoordinateIsValid(chessboard, fromCoordinate, kingColor, i + 1, j - 1);
			softValidMoves.AddIfCoordinateIsValid(chessboard, fromCoordinate, kingColor, i + 1, j);
			softValidMoves.AddIfCoordinateIsValid(chessboard, fromCoordinate, kingColor, i + 1, j + 1);

			return softValidMoves;
		}

		public bool IsKingMoveSoftValid(Chessboard chessboard, GameMove move, ChessColor kingColor)
		{
			return Chessboard.IsCoordinateValid(move.To) &&
			       Math.Abs(move.To.Letter - move.From.Letter) <= 1 &&
			       Math.Abs(move.To.Number - move.From.Number) <= 1;
		}
	}
}
