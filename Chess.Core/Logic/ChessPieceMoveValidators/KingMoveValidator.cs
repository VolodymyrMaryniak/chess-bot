using Chess.Core.Enums;
using Chess.Core.Game;
using Chess.Core.Logic.ChessPieceMoveValidators.Extensions;
using Chess.Core.Models;
using System;
using System.Collections.Generic;

namespace Chess.Core.Logic.ChessPieceMoveValidators
{
	internal class KingMoveValidator
	{
		public List<GameMove> GetSoftValidKingMoves(Chessboard chessboard, Coordinate fromCoordinate, ChessColor kingColor)
		{
			var i = fromCoordinate.Number - 1;
			var j = fromCoordinate.Letter - 'A';

			var softValidMoves = new List<GameMove>();
			softValidMoves.AddMoveIfCoordinateIsSoftValid(chessboard, fromCoordinate, kingColor, i - 1, j - 1);
			softValidMoves.AddMoveIfCoordinateIsSoftValid(chessboard, fromCoordinate, kingColor, i - 1, j);
			softValidMoves.AddMoveIfCoordinateIsSoftValid(chessboard, fromCoordinate, kingColor, i - 1, j + 1);
			softValidMoves.AddMoveIfCoordinateIsSoftValid(chessboard, fromCoordinate, kingColor, i, j - 1);
			softValidMoves.AddMoveIfCoordinateIsSoftValid(chessboard, fromCoordinate, kingColor, i, j + 1);
			softValidMoves.AddMoveIfCoordinateIsSoftValid(chessboard, fromCoordinate, kingColor, i + 1, j - 1);
			softValidMoves.AddMoveIfCoordinateIsSoftValid(chessboard, fromCoordinate, kingColor, i + 1, j);
			softValidMoves.AddMoveIfCoordinateIsSoftValid(chessboard, fromCoordinate, kingColor, i + 1, j + 1);

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
