using Chess.Engine.Enums;
using Chess.Engine.Game;
using Chess.Engine.Logic.ChessPieceMoveValidators.Extensions;
using Chess.Engine.Logic.ChessPieceMoveValidators.InternalEnums;
using Chess.Engine.Models;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Engine.Logic.ChessPieceMoveValidators
{
	internal class RookMoveValidator
	{
		private static readonly MoveDirection[] _rookDirections =
		{
			MoveDirection.Upwards,
			MoveDirection.Right,
			MoveDirection.Down,
			MoveDirection.Left
		};

		public List<GameMove> GetSoftValidRookMoves(Chessboard chessboard, Coordinate coordinate, ChessColor rookColor)
		{
			var softValidMoves = new List<GameMove>();

			foreach (var direction in _rookDirections)
				softValidMoves.AddSoftValidMoves(chessboard, coordinate, rookColor, direction);

			return softValidMoves;
		}

		public bool CanMove(Chessboard chessboard, GameMove move)
		{
			return _rookDirections.Any(direction => chessboard.CanMove(direction, move));
		}
	}
}
