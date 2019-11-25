using Chess.Engine.Enums;
using Chess.Engine.Game;
using Chess.Engine.Logic.ChessPieceMoveValidators.Extensions;
using Chess.Engine.Logic.ChessPieceMoveValidators.InternalEnums;
using Chess.Engine.Models;
using System;
using System.Collections.Generic;

namespace Chess.Engine.Logic.ChessPieceMoveValidators
{
	internal class KnightMoveValidator
	{
		private static readonly int[][] _knightMoves = 
		{
			new[] {2, 1},
			new[] {1, 2},
			new[] {-1, 2},
			new[] {-2, 1},
			new[] {-2, -1},
			new[] {-1, -2},
			new[] {1, -2},
			new[] {2, -1}
		};

		public List<GameMove> GetSoftValidKnightMoves(Chessboard chessboard, Coordinate fromCoordinate, ChessColor knightColor)
		{
			fromCoordinate.ToArrayIndexes(out var i, out var j);
			var softValidMoves = new List<GameMove>();

			foreach (var knightMove in _knightMoves)
				softValidMoves.AddIf(chessboard, fromCoordinate, knightColor, i + knightMove[0], j + knightMove[1], MoveCheckOption.Valid);

			return softValidMoves;
		}

		public bool CanMove(Chessboard chessboard, GameMove move)
		{
			var letterDiffAbs = Math.Abs(move.From.Letter - move.To.Letter);
			var numberDiffAbs = Math.Abs(move.From.Number - move.To.Number);

			return letterDiffAbs == 2 && numberDiffAbs == 1 || letterDiffAbs == 1 && numberDiffAbs == 2;
		}
	}
}
