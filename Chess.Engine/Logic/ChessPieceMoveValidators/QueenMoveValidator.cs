using Chess.Engine.Enums;
using Chess.Engine.Game;
using Chess.Engine.Logic.ChessPieceMoveValidators.Extensions;
using Chess.Engine.Models;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Engine.Logic.ChessPieceMoveValidators
{
	internal class QueenMoveValidator
	{
		public List<GameMove> GetSoftValidQueenMoves(Chessboard chessboard, Coordinate fromCoordinate, ChessColor queenColor)
		{
			var softValidMoves = new List<GameMove>();

			foreach (var moveDirection in MoveValidatorExtensions.MoveDirections)
				softValidMoves.AddSoftValidMoves(chessboard, fromCoordinate, queenColor, moveDirection);

			return softValidMoves;
		}

		public bool CanMove(Chessboard chessboard, GameMove move)
		{
			return MoveValidatorExtensions.MoveDirections.Any(direction => chessboard.CanMove(direction, move));
		}
	}
}
