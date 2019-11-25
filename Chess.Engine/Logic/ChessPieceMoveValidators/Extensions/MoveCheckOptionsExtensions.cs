using Chess.Engine.Enums;
using Chess.Engine.Game;
using Chess.Engine.Logic.ChessPieceMoveValidators.InternalEnums;
using System;

namespace Chess.Engine.Logic.ChessPieceMoveValidators.Extensions
{
	public static class MoveCheckOptionsExtensions
	{
		public static bool IsTrue(
			this MoveCheckOption moveCheckOption, 
			Chessboard chessboard, 
			ChessColor chessPieceColor, 
			int i,
			int j)
		{
			switch (moveCheckOption)
			{
				case MoveCheckOption.Valid: return chessboard.IsCoordinateValid(chessPieceColor, i, j);
				case MoveCheckOption.Empty: return chessboard.IsCoordinateEmpty(i, j);
				case MoveCheckOption.Kills: return chessboard.IsKilling(chessPieceColor, i, j);
				default:
					throw new ArgumentOutOfRangeException(nameof(moveCheckOption), moveCheckOption, null);
			}
		}
	}
}
