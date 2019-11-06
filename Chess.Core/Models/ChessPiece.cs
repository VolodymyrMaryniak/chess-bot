using Chess.Core.Enums;

namespace Chess.Core.Models
{
	public struct ChessPiece
	{
		public ChessColor Owner { get; set; }
		public ChessPieceType Type { get; set; }
	}
}
