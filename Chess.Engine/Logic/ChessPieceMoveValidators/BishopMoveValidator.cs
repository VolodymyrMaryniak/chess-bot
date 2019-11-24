using Chess.Engine.Enums;
using Chess.Engine.Game;
using Chess.Engine.Logic.ChessPieceMoveValidators.Extensions;
using Chess.Engine.Logic.ChessPieceMoveValidators.InternalEnums;
using Chess.Engine.Models;
using System.Collections.Generic;

namespace Chess.Engine.Logic.ChessPieceMoveValidators
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
