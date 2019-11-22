using Chess.Core.Enums;
using Chess.Core.Game;
using Chess.Core.Logic.ChessPieceMoveValidators.Extensions;
using Chess.Core.Models;
using System.Collections.Generic;

namespace Chess.Core.Logic.ChessPieceMoveValidators
{
	internal class KnightMoveValidator
	{
		public List<GameMove> GetSoftValidKnightMoves(Chessboard chessboard, Coordinate fromCoordinate, ChessColor knightColor)
		{
			var softValidMoves = new List<GameMove>();
			fromCoordinate.ToArrayIndexes(out var i, out var j);

			softValidMoves.AddIfCoordinateIsValid(chessboard, fromCoordinate, knightColor, i + 2, j + 1);
			softValidMoves.AddIfCoordinateIsValid(chessboard, fromCoordinate, knightColor, i + 1, j + 2);
			softValidMoves.AddIfCoordinateIsValid(chessboard, fromCoordinate, knightColor, i - 1, j + 2);
			softValidMoves.AddIfCoordinateIsValid(chessboard, fromCoordinate, knightColor, i - 2, j + 1);
			softValidMoves.AddIfCoordinateIsValid(chessboard, fromCoordinate, knightColor, i - 2, j - 1);
			softValidMoves.AddIfCoordinateIsValid(chessboard, fromCoordinate, knightColor, i - 1, j - 2);
			softValidMoves.AddIfCoordinateIsValid(chessboard, fromCoordinate, knightColor, i + 1, j - 2);
			softValidMoves.AddIfCoordinateIsValid(chessboard, fromCoordinate, knightColor, i + 2, j - 1);

			return softValidMoves;
		}
	}
}
