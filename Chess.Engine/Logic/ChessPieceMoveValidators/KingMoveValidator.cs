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
			var softValidMoves = new List<GameMove>();

			foreach (var moveDirection in MoveValidatorExtensions.MoveDirections)
				softValidMoves.AddSoftValidMove(chessboard, fromCoordinate, kingColor, moveDirection);

			return softValidMoves;
		}

		public bool CanMove(Chessboard chessboard, GameMove move)
		{
			return Math.Abs(move.From.Number - move.To.Number) <= 1 && Math.Abs(move.From.Letter - move.To.Letter) <= 1;
		}
	}
}
