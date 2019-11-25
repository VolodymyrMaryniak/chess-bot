using Chess.Engine.Enums;
using Chess.Engine.Models;
using System;
using System.Collections.Generic;

namespace Chess.Engine.Game
{
	public class Chessboard : ICloneable
	{
		public ChessPiece?[,] Board { get; }
		public List<ChessPiece> ChessPieces => GetChessPieces();
		public List<ChessPieceCoordinate> ChessPieceCoordinates => GetChessPieceCoordinates();

		public Chessboard()
		{
			Board = new ChessPiece?[8, 8];
			SetChessPiecesToTheirOriginalPositions();
		}

		private Chessboard(ChessPiece?[,] chessPiecesArray)
		{
			Board = chessPiecesArray;
		}

		public ChessPiece GetChessPiece(Coordinate coordinate)
		{
			var chessPiece = GetChessPieceOrDefault(coordinate);
			if (chessPiece == null)
				throw new Exception("Chess piece not found.");

			return chessPiece.Value;
		}

		public ChessPiece? GetChessPieceOrDefault(Coordinate coordinate)
		{
			return Board[coordinate.Number - 1, coordinate.Letter - 'A'];
		}

		public Coordinate GetCoordinate(ChessPiece chessPiece)
		{
			var coordinate = GetCoordinateOrDefault(chessPiece);
			if (coordinate == null)
				throw new Exception("Chess piece not found.");

			return coordinate.Value;
		}

		public Coordinate? GetCoordinateOrDefault(ChessPiece chessPiece)
		{
			for (var i = 0; i < 8; i++)
			for (var j = 0; j < 8; j++)
			{
				var boardChessPiece = Board[i, j];
				if (boardChessPiece.HasValue && boardChessPiece.Value.Owner == chessPiece.Owner && boardChessPiece.Value.Type == chessPiece.Type)
					return new Coordinate((char)(j + 'A'), i + 1);
			}

			return null;
		}

		public void Move(GameMove move)
		{
			var chessPiece = GetChessPiece(move.From);

			Set(null, move.From);
			Set(chessPiece, move.To);
		}

		public object Clone()
		{
			return new Chessboard((ChessPiece?[,]) Board.Clone());
		}

		public override bool Equals(object obj)
		{
			return obj is Chessboard chessboard && Board.Equals(chessboard.Board);
		}

		public override int GetHashCode()
		{
			var cacheCode = 0;
			for (var i = 0; i < 8; i++)
			for (var j = 0; j < 8; j++)
				cacheCode ^= Board[i, j].HasValue
					? i * 100000 + j * 10000 + (int) Board[i, j].Value.Owner * 1000 + (int) Board[i, j].Value.Type * 100
					: i * 100000 + j * 1000;

			return cacheCode;
		}

		public static bool IsCoordinateValid(int i, int j)
		{
			return i >= 0 && i <= 7 && j >= 0 && j <= 7;
		}

		public static Coordinate GetCoordinate(int i, int j)
		{
			return new Coordinate((char)(j + 'A'), i + 1);
		}

		public void Set(ChessPiece? chessPiece, Coordinate toCoordinate)
		{
			Board[toCoordinate.Number - 1, toCoordinate.Letter - 'A'] = chessPiece;
		}

		#region private methods

		private void SetChessPiecesToTheirOriginalPositions()
		{
			Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Rook }, new Coordinate('A', 1));
			Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Knight }, new Coordinate('B', 1));
			Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Bishop }, new Coordinate('C', 1));
			Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Queen }, new Coordinate('D', 1));
			Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.King }, new Coordinate('E', 1));
			Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Bishop }, new Coordinate('F', 1));
			Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Knight }, new Coordinate('G', 1));
			Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Rook }, new Coordinate('H', 1));

			Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Pawn }, new Coordinate('A', 2));
			Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Pawn }, new Coordinate('B', 2));
			Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Pawn }, new Coordinate('C', 2));
			Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Pawn }, new Coordinate('D', 2));
			Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Pawn }, new Coordinate('E', 2));
			Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Pawn }, new Coordinate('F', 2));
			Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Pawn }, new Coordinate('G', 2));
			Set(new ChessPiece { Owner = ChessColor.White, Type = ChessPieceType.Pawn }, new Coordinate('H', 2));

			Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Pawn }, new Coordinate('A', 7));
			Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Pawn }, new Coordinate('B', 7));
			Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Pawn }, new Coordinate('C', 7));
			Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Pawn }, new Coordinate('D', 7));
			Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Pawn }, new Coordinate('E', 7));
			Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Pawn }, new Coordinate('F', 7));
			Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Pawn }, new Coordinate('G', 7));
			Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Pawn }, new Coordinate('H', 7));

			Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Rook }, new Coordinate('A', 8));
			Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Knight }, new Coordinate('B', 8));
			Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Bishop }, new Coordinate('C', 8));
			Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Queen }, new Coordinate('D', 8));
			Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.King }, new Coordinate('E', 8));
			Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Bishop }, new Coordinate('F', 8));
			Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Knight }, new Coordinate('G', 8));
			Set(new ChessPiece { Owner = ChessColor.Black, Type = ChessPieceType.Rook }, new Coordinate('H', 8));
		}

		private List<ChessPiece> GetChessPieces()
		{
			var chessPieces = new List<ChessPiece>();

			for(var i = 0; i < 8; i++)
			for (var j = 0; j < 8; j++)
				if (Board[i, j].HasValue)
					chessPieces.Add(Board[i, j].Value);

			return chessPieces;
		}

		private List<ChessPieceCoordinate> GetChessPieceCoordinates()
		{
			var chessPieces = new List<ChessPieceCoordinate>();

			for (var i = 0; i < 8; i++)
			for (var j = 0; j < 8; j++)
			{
				if (Board[i, j].HasValue)
				{
					chessPieces.Add(new ChessPieceCoordinate
					{
						ChessPiece = Board[i, j].Value,
						Coordinate = new Coordinate((char) (j + 'A'), i + 1)
					});
				}
			}

			return chessPieces;
		}

		#endregion
	}
}
