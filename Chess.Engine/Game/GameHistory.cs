using Chess.Engine.Enums;
using Chess.Engine.Extensions;
using Chess.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Engine.Game
{
	public class GameHistory : ICloneable
	{
		public bool WhiteShortCastlingPossible { get; private set; }
		public bool WhiteLongCastlingPossible { get; private set; }
		public bool BlackShortCastlingPossible { get; private set; }
		public bool BlackLongCastlingPossible { get; private set; }

		public bool IsPositionRepeatedThreeTimes => PositionsRepeatedTimes.Any(keyValuePair => keyValuePair.Value == 3);

		private Dictionary<int, int> PositionsRepeatedTimes { get; set; }
		public GameMove? LastMove { get; private set; }

		public GameHistory(bool castlingsPossible = true)
		{
			PositionsRepeatedTimes = new Dictionary<int, int>();
			BlackShortCastlingPossible = BlackLongCastlingPossible = WhiteShortCastlingPossible = WhiteLongCastlingPossible = castlingsPossible;
		}

		/// <summary>
		/// Add move into history before doing it on chessboard!
		/// </summary>
		/// <param name="move"></param>
		/// <param name="turn"></param>
		/// <param name="chessboard"></param>
		public void Add(GameMove move, ChessColor turn, Chessboard chessboard)
		{
			var chessPiece = chessboard.GetChessPiece(move.From);
			LastMove = move;

			CheckCastlingPossibility(chessPiece, move);

			var cacheCode = chessboard.GetHashCode();
			if (PositionsRepeatedTimes.ContainsKey(cacheCode))
				PositionsRepeatedTimes[cacheCode]++;
			else
				PositionsRepeatedTimes.Add(cacheCode, 1);
		}

		public object Clone()
		{
			return new GameHistory
			{
				PositionsRepeatedTimes = PositionsRepeatedTimes.CloneDictionary(),
				LastMove = LastMove,
				WhiteShortCastlingPossible = WhiteShortCastlingPossible,
				WhiteLongCastlingPossible = WhiteLongCastlingPossible,
				BlackShortCastlingPossible = BlackShortCastlingPossible,
				BlackLongCastlingPossible = BlackLongCastlingPossible
			};
		}

		// TODO: Refactor
		private void CheckCastlingPossibility(ChessPiece chessPiece, GameMove move)
		{
			switch (chessPiece.Owner)
			{
				case ChessColor.White:
				{
					if (WhiteLongCastlingPossible || WhiteShortCastlingPossible)
					{
						if (move.Castling.HasValue || chessPiece.Type == ChessPieceType.King)
							WhiteLongCastlingPossible = WhiteShortCastlingPossible = false;
						else if (chessPiece.Type == ChessPieceType.Rook)
						{
							if (move.From == new Coordinate('A', 1) && WhiteLongCastlingPossible)
								WhiteLongCastlingPossible = false;
							else if (move.From == new Coordinate('H', 1) && WhiteShortCastlingPossible)
								WhiteShortCastlingPossible = false;
						}
					}

					if (move.To == new Coordinate('A', 8))
						BlackLongCastlingPossible = false;
					else if (move.To == new Coordinate('H', 8))
						BlackShortCastlingPossible = false;
					break;
				}
				case ChessColor.Black:
				{
					if (BlackShortCastlingPossible || BlackLongCastlingPossible)
					{
						if (move.Castling.HasValue || chessPiece.Type == ChessPieceType.King)
							BlackLongCastlingPossible = BlackShortCastlingPossible = false;
						else if (chessPiece.Type == ChessPieceType.Rook)
						{
							if (move.From == new Coordinate('A', 8) && BlackLongCastlingPossible)
								BlackLongCastlingPossible = false;
							else if (move.From == new Coordinate('H', 8) && BlackShortCastlingPossible)
								BlackShortCastlingPossible = false;
						}
					}

					if (move.To == new Coordinate('A', 1))
						WhiteLongCastlingPossible = false;
					else if (move.To == new Coordinate('H', 1))
						WhiteShortCastlingPossible = false;
					break;
				}
			}
		}
	}
}
