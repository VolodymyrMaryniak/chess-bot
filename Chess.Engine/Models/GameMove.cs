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

		public static bool operator ==(GameMove gameMoveA, GameMove gameMoveB)
		{
			return gameMoveA.From == gameMoveB.From &&
			       gameMoveA.To == gameMoveB.To &&
			       gameMoveA.Castling == gameMoveB.Castling &&
			       gameMoveA.CastTo == gameMoveB.CastTo;
		}

		public static bool operator !=(GameMove gameMoveA, GameMove gameMoveB)
		{
			return !(gameMoveA == gameMoveB);
		}
	}
}
