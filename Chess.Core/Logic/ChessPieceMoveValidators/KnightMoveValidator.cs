using Chess.Core.Enums;
using Chess.Core.Game;
using Chess.Core.Logic.ChessPieceMoveValidators.Extensions;
using Chess.Core.Logic.ChessPieceMoveValidators.InternalEnums;
using Chess.Core.Models;
using System.Collections.Generic;

namespace Chess.Core.Logic.ChessPieceMoveValidators
{
	internal class KnightMoveValidator
	{
		public List<GameMove> GetSoftValidKnightMoves(Chessboard chessboard, Coordinate coordinate, ChessColor knightColor)
		{
			var softValidMoves = new List<GameMove>();
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, knightColor, MoveDirection.Upwards);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, knightColor, MoveDirection.UpwardsRight);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, knightColor, MoveDirection.Right);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, knightColor, MoveDirection.DownRight);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, knightColor, MoveDirection.Down);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, knightColor, MoveDirection.DownLeft);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, knightColor, MoveDirection.Left);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, knightColor, MoveDirection.UpwardsLeft);

			return softValidMoves;
		}
	}
}
