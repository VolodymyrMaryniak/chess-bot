using Chess.Engine.Enums;

namespace Chess.Engine.Models
{
	public struct ChessPiece
	{
		public ChessColor Owner { get; set; }
		public ChessPieceType Type { get; set; }

		public override string ToString()
		{
			return $"[{Owner} {Type}]";
		}
	}
}
