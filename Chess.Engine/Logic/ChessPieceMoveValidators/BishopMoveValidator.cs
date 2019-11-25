using Chess.Engine.Enums;
using Chess.Engine.Game;
using Chess.Engine.Logic.ChessPieceMoveValidators.Extensions;
using Chess.Engine.Logic.ChessPieceMoveValidators.InternalEnums;
using Chess.Engine.Models;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Engine.Logic.ChessPieceMoveValidators
{
	internal class BishopMoveValidator
	{
		private static readonly MoveDirection[] _bishopDirections =
		{
			MoveDirection.UpwardsRight,
			MoveDirection.DownRight,
			MoveDirection.DownLeft,
			MoveDirection.UpwardsLeft
		};

		public List<GameMove> GetSoftValidBishopMoves(Chessboard chessboard, Coordinate coordinate, ChessColor bishopColor)
		{
			var softValidMoves = new List<GameMove>();

			foreach (var direction in _bishopDirections)
				softValidMoves.AddSoftValidMoves(chessboard, coordinate, bishopColor, direction);

			return softValidMoves;
		}

		public bool CanMove(Chessboard chessboard, GameMove move)
		{
			return _bishopDirections.Any(direction => chessboard.CanMove(direction, move));
		}
	}
}
