using Chess.Engine.Enums;
using Chess.Engine.Models;

namespace Chess.Engine.Game.Extensions
{
	public static class ChessboardExtensions
	{
		public static void SetCustomPosition1(this Chessboard chessboard)
		{
			chessboard.CleanChessboard();

			chessboard.Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.King }, new Coordinate('F', 1));
			chessboard.Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Queen }, new Coordinate('G', 1));

			chessboard.Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.King }, new Coordinate('H', 3));
			chessboard.Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Pawn }, new Coordinate('H', 4));
			chessboard.Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Knight }, new Coordinate('E', 1));
		}

		public static void SetCustomPosition2(this Chessboard chessboard)
		{
			chessboard.CleanChessboard();

			chessboard.Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.King }, new Coordinate('B', 1));
			chessboard.Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Rook }, new Coordinate('D', 1));

			chessboard.Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Pawn }, new Coordinate('B', 2));
			chessboard.Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Rook }, new Coordinate('E', 2));
			chessboard.Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Pawn }, new Coordinate('G', 2));

			chessboard.Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Queen }, new Coordinate('D', 3));
			chessboard.Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Pawn }, new Coordinate('F', 3));
			chessboard.Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Pawn }, new Coordinate('H', 2));

			chessboard.Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Pawn }, new Coordinate('B', 3));

			chessboard.Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Pawn }, new Coordinate('B', 4));
			chessboard.Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Queen }, new Coordinate('C', 4));

			chessboard.Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Pawn }, new Coordinate('G', 6));

			chessboard.Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Pawn }, new Coordinate('H', 7));
			chessboard.Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Pawn }, new Coordinate('F', 7));

			chessboard.Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Rook }, new Coordinate('A', 8));
			chessboard.Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Rook }, new Coordinate('C', 8));
			chessboard.Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.King }, new Coordinate('G', 8));
		}

		private static void CleanChessboard(this Chessboard chessboard)
		{
			for (var i = 0; i < 8; i++)
			for (var j = 0; j < 8; j++)
				chessboard.Board[i, j] = null;
		}
	}
}
