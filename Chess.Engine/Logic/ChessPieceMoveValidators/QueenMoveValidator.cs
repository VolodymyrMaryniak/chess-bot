using Chess.Engine.Enums;
using Chess.Engine.Game;
using Chess.Engine.Logic.ChessPieceMoveValidators.Extensions;
using Chess.Engine.Logic.ChessPieceMoveValidators.InternalEnums;
using Chess.Engine.Models;
using System.Collections.Generic;

namespace Chess.Engine.Logic.ChessPieceMoveValidators
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
