using Chess.Engine.Models;
using Chess.PlayWithHuman.Properties;
using System.Drawing;

namespace Chess.PlayWithHuman.Helpers
{
	public static class ImageHelper
	{
		public static Image GetImage(ChessPiece chessPiece)
		{
			return (Image)Resources.ResourceManager.GetObject($"{chessPiece.Owner}_{chessPiece.Type}");
		}
	}
}
