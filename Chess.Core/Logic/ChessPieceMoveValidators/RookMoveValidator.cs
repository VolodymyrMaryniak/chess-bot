using Chess.Core.Enums;
using Chess.Core.Game;
using Chess.Core.Logic.ChessPieceMoveValidators.Extensions;
using Chess.Core.Logic.ChessPieceMoveValidators.InternalEnums;
using Chess.Core.Models;
using System.Collections.Generic;

namespace Chess.Core.Logic.ChessPieceMoveValidators
{
	internal class RookMoveValidator
	{
		public List<GameMove> GetSoftValidRookMoves(Chessboard chessboard, Coordinate coordinate, ChessColor rookColor)
		{
			var softValidMoves = new List<GameMove>();
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, rookColor, MoveDirection.Upwards);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, rookColor, MoveDirection.Right);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, rookColor, MoveDirection.Down);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, rookColor, MoveDirection.Left);

			return softValidMoves;
		}
	}
}
