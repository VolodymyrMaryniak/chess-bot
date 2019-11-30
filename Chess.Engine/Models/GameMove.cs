using Chess.Engine.Enums;

namespace Chess.Engine.Models
{
	public struct GameMove
	{
		public Coordinate From { get; set; }
		public Coordinate To { get; set; }

		public Castling? Castling { get; set; }
		public ChessPieceType? CastTo { get; set; }

		public override string ToString()
		{
			return $"[From: {From}, To: {To}]";
		}
	}
}
