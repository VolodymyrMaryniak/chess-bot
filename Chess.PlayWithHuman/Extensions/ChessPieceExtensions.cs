using Chess.Engine.Models;
using Chess.PlayWithHuman.Helpers;
using System.Drawing;

namespace Chess.PlayWithHuman.Extensions
{
	internal static class ChessPieceExtensions
	{
		public static Image GetImage(this ChessPiece? chessPiece)
		{
			return chessPiece.HasValue
				? ImageHelper.GetImage(chessPiece.Value)
				: null;
		}
	}
}
