using Chess.Core.Enums;
using Chess.Core.Game;
using Chess.Core.Logic.ChessPieceMoveValidators.Extensions;
using Chess.Core.Logic.ChessPieceMoveValidators.InternalEnums;
using Chess.Core.Models;
using System.Collections.Generic;

namespace Chess.Core.Logic.ChessPieceMoveValidators
{
	internal class BishopMoveValidator
	{
		public List<GameMove> GetSoftValidBishopMoves(Chessboard chessboard, Coordinate coordinate, ChessColor bishopColor)
		{
			var softValidMoves = new List<GameMove>();
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, bishopColor, MoveDirection.UpwardsRight);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, bishopColor, MoveDirection.DownRight);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, bishopColor, MoveDirection.DownLeft);
			softValidMoves.AddSoftValidMoves(chessboard, coordinate, bishopColor, MoveDirection.UpwardsLeft);

			return softValidMoves;
		}
	}
}
