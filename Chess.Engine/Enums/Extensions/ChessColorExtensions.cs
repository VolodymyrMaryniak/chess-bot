using System;

namespace Chess.Engine.Enums.Extensions
{
	public static class ChessColorExtensions
	{
		public static ChessColor GetOppositeChessColor(this ChessColor chessColor)
		{
			switch (chessColor)
			{
				case ChessColor.White:
					return ChessColor.Black;
				case ChessColor.Black:
					return ChessColor.White;
				default:
					throw new ArgumentOutOfRangeException(nameof(chessColor), chessColor, null);
			}
		}
	}
}