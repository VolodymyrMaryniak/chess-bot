using Chess.Core.Enums;
using Chess.Core.Game;
using Chess.Core.Logic.ChessPieceMoveValidators.InternalEnums;
using Chess.Core.Models;
using System;
using System.Collections.Generic;

namespace Chess.Core.Logic.ChessPieceMoveValidators.Extensions
{
	internal static class MoveValidatorExtensions
	{
		public static bool AddIfCoordinateIsValid(
			this List<GameMove> list, 
			Chessboard chessboard, 
			Coordinate from,
			ChessColor chessPieceColor, 
			int i, 
			int j)
		{
			if (!chessboard.IsCoordinateValid(chessPieceColor, i, j))
				return false;

			list.Add(new GameMove { From = @from, To = Chessboard.GetCoordinate(i, j) });
			return true;
		}

		public static bool AddIfCoordinateIsEmpty(this List<GameMove> list, Chessboard chessboard, Coordinate from, int i, int j)
		{
			if (!chessboard.IsCoordinateEmpty(i, j))
				return false;

			list.Add(new GameMove { From = from, To = Chessboard.GetCoordinate(i, j) });
			return true;
		}

		public static bool AddIfCoordinateIsEmpty(this List<GameMove> list, Chessboard chessboard, GameMove move)
		{
			if (chessboard.GetChessPieceOrDefault(move.To).HasValue)
				return false;

			list.Add(move);
			return true;
		}

		public static void AddIfPawnKills(
			this List<GameMove> list,
			Chessboard chessboard,
			GameMove? previousMove,
			Coordinate from,
			ChessColor chessPieceColor,
			MoveDirection direction)
		{
			from.ToArrayIndexes(out var i, out var j);

			Move(direction, ref i, ref j);
			if (!Chessboard.IsCoordinateValid(i, j))
				return;

			if (!IsPawnKilling(chessboard, previousMove, chessPieceColor, i, j))
				return;

			var toCoordinate = Chessboard.GetCoordinate(i, j);
			if (chessPieceColor == ChessColor.White && toCoordinate.Number == 8 ||
			    chessPieceColor == ChessColor.White && toCoordinate.Number == 1)
				list.AddMovesWithAllCastToOptions(new GameMove {From = from, To = toCoordinate});
			else
				list.Add(new GameMove {From = from, To = toCoordinate});
		}

		public static void AddSoftValidMoves(
			this List<GameMove> list, 
			Chessboard chessboard, 
			Coordinate from,
			ChessColor chessPieceColor,
			MoveDirection direction)
		{
			from.ToArrayIndexes(out var i, out var j);

			Move(direction, ref i, ref j);
			while (list.AddIfCoordinateIsEmpty(chessboard, from, i, j))
			{
				Move(direction, ref i, ref j);
			}

			list.AddIfCoordinateIsValid(chessboard, from, chessPieceColor, i, j);
		}

		public static bool IsCoordinateValid(this Chessboard chessboard, ChessColor chessPieceColor, int i, int j)
		{
			if (!Chessboard.IsCoordinateValid(i, j))
				return false;

			var chessPieceInPosition = chessboard.Board[i, j];
			return !chessPieceInPosition.HasValue || chessPieceInPosition.Value.Owner != chessPieceColor;
		}

		public static bool IsCoordinateEmpty(this Chessboard chessboard, int i, int j)
		{
			if (!Chessboard.IsCoordinateValid(i, j))
				return false;

			return !chessboard.Board[i, j].HasValue;
		}

		public static void AddMovesWithAllCastToOptions(this List<GameMove> list, GameMove move)
		{
			move.CastTo = ChessPieceType.Queen;
			list.Add(move);
			move.CastTo = ChessPieceType.Rook;
			list.Add(move);
			move.CastTo = ChessPieceType.Bishop;
			list.Add(move);
			move.CastTo = ChessPieceType.Knight;
		}

		public static void Move(MoveDirection direction, ref int i, ref int j)
		{
			switch (direction)
			{
				case MoveDirection.Upwards:
					i++;
					break;
				case MoveDirection.UpwardsRight:
					i++;
					j++;
					break;
				case MoveDirection.Right:
					j++;
					break;
				case MoveDirection.DownRight:
					i--;
					j++;
					break;
				case MoveDirection.Down:
					i--;
					break;
				case MoveDirection.DownLeft:
					i--;
					j--;
					break;
				case MoveDirection.Left:
					j--;
					break;
				case MoveDirection.UpwardsLeft:
					i++;
					j--;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}
		}

		private static bool IsPawnKilling(Chessboard chessboard, GameMove? previousMove, ChessColor chessPieceColor, int i, int j)
		{
			var chessPieceOnPotentialVictimPosition = chessboard.Board[i, j];
			if (chessPieceOnPotentialVictimPosition.HasValue)
				return chessPieceOnPotentialVictimPosition.Value.Owner != chessPieceColor;

			// "En Passant" checking

			if (!previousMove.HasValue ||
			    chessPieceColor == ChessColor.White && i != 5 ||
			    chessPieceColor == ChessColor.Black && i != 2)
				return false;

			var previousMoveChessPiece = chessboard.GetChessPieceOrDefault(previousMove.Value.To);
			if (!previousMoveChessPiece.HasValue || previousMoveChessPiece.Value.Type != ChessPieceType.Pawn)
				return false;

			previousMove.Value.From.ToArrayIndexes(out var prevMoveFromI, out var prevMoveFromJ);
			previousMove.Value.To.ToArrayIndexes(out var prevMoveToI, out _);
			var prevMoveOwner = previousMoveChessPiece.Value.Owner;

			if (prevMoveFromJ != j)
				return false;

			return prevMoveOwner == ChessColor.White && prevMoveFromI == 1 && prevMoveToI == 3 ||
			       prevMoveOwner == ChessColor.Black && prevMoveFromI == 6 && prevMoveToI == 4;
		}
	}
}
