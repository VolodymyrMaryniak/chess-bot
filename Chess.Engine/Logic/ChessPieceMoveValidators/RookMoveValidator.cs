using Chess.Engine.Enums;
using Chess.Engine.Game;
using Chess.Engine.Logic.ChessPieceMoveValidators.Extensions;
using Chess.Engine.Logic.ChessPieceMoveValidators.InternalEnums;
using Chess.Engine.Models;
using System.Collections.Generic;

namespace Chess.Engine.Logic.ChessPieceMoveValidators
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
