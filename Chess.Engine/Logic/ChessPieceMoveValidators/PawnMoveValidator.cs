using Chess.Engine.Enums;
using Chess.Engine.Game;
using Chess.Engine.Logic.ChessPieceMoveValidators.Extensions;
using Chess.Engine.Logic.ChessPieceMoveValidators.InternalEnums;
using Chess.Engine.Models;
using System;
using System.Collections.Generic;

namespace Chess.Engine.Logic.ChessPieceMoveValidators
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

		public bool CanMove(Chessboard chessboard, ChessColor pawnColor, GameMove move)
		{
			if (pawnColor == ChessColor.White && move.From.Number >= move.To.Number ||
			    pawnColor == ChessColor.Black && move.From.Number <= move.To.Number)
				return false;

			var lettersDiffAbs = Math.Abs(move.From.Letter - move.To.Letter);
			if (lettersDiffAbs > 1)
				return false;

			var numbersDiffAbs = Math.Abs(move.From.Number - move.To.Number);
			if (numbersDiffAbs > 1)
				return false;

			var thereIsSomeone = chessboard.GetChessPieceOrDefault(move.To).HasValue;
			if (move.To.Letter == move.From.Letter)
				return !thereIsSomeone;

			return thereIsSomeone;
		}
	}
}
