using Chess.Core.Enums;
using Chess.Core.Game;
using Chess.Core.Logic.ChessPieceMoveValidators.InternalEnums;
using Chess.Core.Models;
using System;
using System.Collections.Generic;

namespace Chess.Core.Logic.ChessPieceMoveValidators.Extensions
{
	internal static class Extensions
	{
		public static void AddMoveIfCoordinateIsSoftValid(
			this List<GameMove> list, 
			Chessboard chessboard, 
			Coordinate from,
			ChessColor chessPieceColor, 
			int i, 
			int j)
		{
			if (IsCoordinateSoftValid(chessboard, chessPieceColor, i, j))
				list.Add(new GameMove {From = from, To = Chessboard.GetCoordinate(i, j)});
		}

		public static void AddSoftValidMoves(
			this List<GameMove> list, 
			Chessboard chessboard, 
			Coordinate from,
			ChessColor chessPieceColor,
			MoveDirection direction)
		{
			var i = from.Number - 1;
			var j = from.Letter - 'A';

			Move(direction, ref i, ref j);
			while (IsCoordinateSoftValid(chessboard, chessPieceColor, i, j))
			{
				list.Add(new GameMove {From = from, To = Chessboard.GetCoordinate(i, j)});
			}
		}

		private static void Move(MoveDirection direction, ref int i, ref int j)
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

		private static bool IsCoordinateSoftValid(Chessboard chessboard, ChessColor chessPieceColor, int i, int j)
		{
			if (!Chessboard.IsCoordinateValid(i, j))
				return false;

			var chessPieceInPosition = chessboard.Board[i, j];
			return !chessPieceInPosition.HasValue || chessPieceInPosition.Value.Owner != chessPieceColor;
		}
	}
}
