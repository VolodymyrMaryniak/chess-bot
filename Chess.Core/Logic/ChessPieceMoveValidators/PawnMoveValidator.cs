using Chess.Core.Enums;
using Chess.Core.Game;
using Chess.Core.Logic.ChessPieceMoveValidators.Extensions;
using Chess.Core.Logic.ChessPieceMoveValidators.InternalEnums;
using Chess.Core.Models;
using System.Collections.Generic;

namespace Chess.Core.Logic.ChessPieceMoveValidators
{
	internal class PawnMoveValidator
	{
		public List<GameMove> GetPawnKillMoves(Chessboard chessboard, Coordinate fromCoordinate, ChessColor pawnColor, GameMove? previousMove)
		{
			var pawnKillMoves = new List<GameMove>();

			if (pawnColor == ChessColor.White)
			{
				pawnKillMoves.AddIfPawnKills(chessboard, previousMove, fromCoordinate, pawnColor, MoveDirection.UpwardsLeft);
				pawnKillMoves.AddIfPawnKills(chessboard, previousMove, fromCoordinate, pawnColor, MoveDirection.UpwardsRight);
			}
			else
			{
				pawnKillMoves.AddIfPawnKills(chessboard, previousMove, fromCoordinate, pawnColor, MoveDirection.DownRight);
				pawnKillMoves.AddIfPawnKills(chessboard, previousMove, fromCoordinate, pawnColor, MoveDirection.DownLeft);
			}

			return pawnKillMoves;
		}

		public List<GameMove> GetPawnForwardMoves(Chessboard chessboard, Coordinate fromCoordinate, ChessColor pawnColor)
		{
			var pawnForwardMoves = new List<GameMove>();

			fromCoordinate.ToArrayIndexes(out var fromI, out var fromJ);
			var direction = pawnColor == ChessColor.White ? MoveDirection.Upwards : MoveDirection.Down;

			var toI = fromI;
			var toJ = fromJ;

			MoveValidatorExtensions.Move(direction, ref toI, ref toJ);

			var forwardMove = new GameMove
			{
				From = fromCoordinate,
				To = Chessboard.GetCoordinate(toI, toJ)
			};

			switch (fromI)
			{
				case 1 when pawnColor == ChessColor.White:
				case 6 when pawnColor == ChessColor.Black:
				{
					if (!pawnForwardMoves.AddIfCoordinateIsEmpty(chessboard, forwardMove))
						return pawnForwardMoves;

					MoveValidatorExtensions.Move(direction, ref toI, ref toJ);
					pawnForwardMoves.AddIfCoordinateIsEmpty(chessboard, fromCoordinate, toI, toJ);

					return pawnForwardMoves;
				}
				case 6 when pawnColor == ChessColor.White:
				case 1 when pawnColor == ChessColor.Black:
					pawnForwardMoves.AddMovesWithAllCastToOptions(forwardMove);

					return pawnForwardMoves;
				default:
					pawnForwardMoves.Add(forwardMove);

					return pawnForwardMoves;
			}
		}
	}
}
