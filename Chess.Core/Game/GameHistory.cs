using Chess.Core.Enums;
using Chess.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Core.Game
{
	public class GameHistory : ICloneable
	{
		public bool WhiteShortCastlingPossible { get; private set; }
		public bool WhiteLongCastlingPossible { get; private set; }
		public bool BlackShortCastlingPossible { get; private set; }
		public bool BlackLongCastlingPossible { get; private set; }


		private List<GameMove> WhiteMoves { get; set; }
		private List<GameMove> BlackMoves { get; set; }

		public GameHistory()
		{
			WhiteMoves = new List<GameMove>();
			BlackMoves = new List<GameMove>();
		}

		/// <summary>
		/// Add move into history before doing it on chessboard!
		/// </summary>
		/// <param name="move"></param>
		/// <param name="chessboard"></param>
		public void Add(GameMove move, Chessboard chessboard)
		{
			var chessPiece = chessboard.GetChessPiece(move.From);

			if (chessPiece.Owner == ChessColor.White)
				WhiteMoves.Add(move);
			else
				BlackMoves.Add(move);

			CheckCastlingPossibility(chessPiece, move);
		}

		public GameMove? GetLastMove()
		{
			if (WhiteMoves.Count == BlackMoves.Count && WhiteMoves.Any())
				return BlackMoves.Last();

			return WhiteMoves.LastOrDefault();
		}

		public object Clone()
		{
			return new GameHistory
			{
				WhiteMoves = WhiteMoves.ToList(),
				BlackMoves = BlackMoves.ToList()
			};
		}

		private void CheckCastlingPossibility(ChessPiece chessPiece, GameMove move)
		{
			switch (chessPiece.Owner)
			{
				case ChessColor.White when (WhiteShortCastlingPossible || WhiteLongCastlingPossible):
				{
					if (move.Castling.HasValue || chessPiece.Type == ChessPieceType.King)
						WhiteLongCastlingPossible = WhiteShortCastlingPossible = false;
					else if (chessPiece.Type == ChessPieceType.Rook)
					{
						if (move.From.Letter == 'A' && move.From.Number == 1 && WhiteLongCastlingPossible)
							WhiteLongCastlingPossible = false;
						else if (move.From.Letter == 'H' && move.From.Number == 1 && WhiteShortCastlingPossible)
							WhiteShortCastlingPossible = false;
					}

					break;
				}
				case ChessColor.Black when (BlackShortCastlingPossible || BlackLongCastlingPossible):
				{
					if (move.Castling.HasValue || chessPiece.Type == ChessPieceType.King)
						BlackLongCastlingPossible = BlackShortCastlingPossible = false;
					else if (chessPiece.Type == ChessPieceType.Rook)
					{
						if (move.From.Letter == 'A' && move.From.Number == 1 && BlackLongCastlingPossible)
							BlackLongCastlingPossible = false;
						else if (move.From.Letter == 'H' && move.From.Number == 1 && BlackShortCastlingPossible)
							BlackShortCastlingPossible = false;
					}

					break;
				}
			}
		}
	}
}
