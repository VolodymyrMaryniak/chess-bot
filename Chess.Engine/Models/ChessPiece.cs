using Chess.Engine.Enums;

namespace Chess.Engine.Models
{
	public struct ChessPiece
	{
		public ChessColor Owner { get; set; }
		public ChessPieceType Type { get; set; }


		public static bool operator ==(ChessPiece chessPieceA, ChessPiece chessPieceB)
		{
			return chessPieceA.Owner == chessPieceB.Owner && chessPieceA.Type == chessPieceB.Type;
		}

		public static bool operator !=(ChessPiece chessPieceA, ChessPiece chessPieceB)
		{
			return !(chessPieceA == chessPieceB);
		}

		public override string ToString()
		{
			return $"[{Owner} {Type}]";
		}
	}
}
