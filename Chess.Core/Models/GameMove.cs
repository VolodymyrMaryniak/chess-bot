using Chess.Core.Enums;

namespace Chess.Core.Models
{
	public struct GameMove
	{
		public Coordinate From { get; set; }
		public Coordinate To { get; set; }

		public Castling? Castling { get; set; }
	}
}
