using Chess.Engine.Enums;
using Chess.Engine.Game;

namespace Chess.Engine.Logic.ChessPieceMoveValidators.Extensions
{
	public static class ChessboardExtensions
	{
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

		public static bool IsKilling(this Chessboard chessboard, ChessColor chessPieceColor, int i, int j)
		{
			if (!Chessboard.IsCoordinateValid(i, j))
				return false;

			var chessPieceInPosition = chessboard.Board[i, j];
			return chessPieceInPosition.HasValue && chessPieceInPosition.Value.Owner != chessPieceColor;
		}
	}
}
