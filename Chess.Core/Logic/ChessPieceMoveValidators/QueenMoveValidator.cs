using Chess.Core.Enums;
using Chess.Core.Game;
using Chess.Core.Logic.ChessPieceMoveValidators.Extensions;
using Chess.Core.Logic.ChessPieceMoveValidators.InternalEnums;
using Chess.Core.Models;
using System.Collections.Generic;

namespace Chess.Core.Logic.ChessPieceMoveValidators
{
	internal class QueenMoveValidator
	{
		public List<GameMove> GetSoftValidQueenMoves(Chessboard chessboard, Coordinate coordinate, ChessColor queenColor)
		{
			var softValidMoves = new List<GameMove>();
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, queenColor, MoveDirection.Upwards);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, queenColor, MoveDirection.UpwardsRight);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, queenColor, MoveDirection.Right);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, queenColor, MoveDirection.DownRight);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, queenColor, MoveDirection.Down);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, queenColor, MoveDirection.DownLeft);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, queenColor, MoveDirection.Left);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, queenColor, MoveDirection.UpwardsLeft);

			return softValidMoves;
		}
	}
}
