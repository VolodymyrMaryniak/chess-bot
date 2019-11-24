using Chess.Engine.Game;
using System.Collections.Generic;

namespace Chess.PlayWithHuman.Extensions
{
	internal static class ChessboardFieldLabelExtensions
	{
		public static void SetChessboardLabelsStyles(this List<ChessboardFieldLabel> chessboardFieldLabels, Chessboard chessboard)
		{
			chessboardFieldLabels.ForEach(x => x.SetChessboardLabelStyles(chessboard));
		}

		private static void SetChessboardLabelStyles(this ChessboardFieldLabel chessboardFieldLabel, Chessboard chessboard)
		{
			var label = chessboardFieldLabel.Label;
			var chessPiece = chessboard.GetChessPieceOrDefault(chessboardFieldLabel.Coordinate);

			label.Image = chessPiece.GetImage();
		}
	}
}
