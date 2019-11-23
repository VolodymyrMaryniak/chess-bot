using Chess.Core.Enums;
using Chess.Core.Game;
using Chess.Core.Logic.ChessPieceMoveValidators;
using Chess.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Core.Logic
{
	public class GameMoveValidator
	{
		private readonly KingMoveValidator _kingMoveValidator;
		private readonly QueenMoveValidator _queenMoveValidator;
		private readonly RookMoveValidator _rookMoveValidator;
		private readonly BishopMoveValidator _bishopMoveValidator;
		private readonly KnightMoveValidator _knightMoveValidator;
		private readonly PawnMoveValidator _pawnMoveValidator;

		public GameMoveValidator()
		{
			_kingMoveValidator = new KingMoveValidator();
			_queenMoveValidator = new QueenMoveValidator();
			_rookMoveValidator = new RookMoveValidator();
			_bishopMoveValidator = new BishopMoveValidator();
			_knightMoveValidator = new KnightMoveValidator();
			_pawnMoveValidator = new PawnMoveValidator();
		}

		public bool IsValid(Chessboard chessboard, GameMove move, GameHistory gameHistory)
		{
			var chessPiece = chessboard.GetChessPieceOrDefault(move.From);
			if (!chessPiece.HasValue)
				return false;

			var chessBoardCopy = (Chessboard) chessboard.Clone();
			var gameHistoryCopy = (GameHistory) gameHistory.Clone();

			gameHistoryCopy.Add(move, chessBoardCopy);
			chessBoardCopy.Move(move);

			var kingCoordinate = chessBoardCopy.GetCoordinate(new ChessPiece {Owner = chessPiece.Value.Owner, Type = ChessPieceType.King});
			var isKingInDanger = IsCoordinateInDanger(chessBoardCopy, chessPiece.Value.Owner, gameHistoryCopy, kingCoordinate);

			return !isKingInDanger;
		}

		public List<GameMove> GetAvailableMoves(Chessboard chessboard, ChessColor turn, GameHistory gameHistory)
		{
			var chessPieceCoordinates = chessboard.ChessPieceCoordinates.Where(x => x.ChessPiece.Owner == turn).ToList();

			var softValidMoves = chessPieceCoordinates
				.SelectMany(chessPieceCoordinate => GetSoftValidMoves(chessboard, chessPieceCoordinate, gameHistory))
				.ToList();

			var pawnForwardMoves = chessPieceCoordinates
				.Where(x => x.ChessPiece.Type == ChessPieceType.Pawn)
				.SelectMany(x => _pawnMoveValidator.GetPawnForwardMoves(chessboard, x.Coordinate, turn))
				.ToList();
			softValidMoves.AddRange(pawnForwardMoves);

			var availableMoves = softValidMoves
				.Where(x => IsValid(chessboard, x, gameHistory))
				.ToList();

			availableMoves.AddRange(GetPossibleCastlingMoves(chessboard, turn, gameHistory));
			
			return availableMoves;
		}

		public bool IsGameFinished(Chessboard chessboard, GameHistory gameHistory)
		{
			throw new NotImplementedException();
		}

		public ChessGameResult? GetGameResult(Chessboard chessboard, GameHistory gameHistory)
		{
			throw new NotImplementedException();
		}

		private List<GameMove> GetSoftValidMoves(Chessboard chessboard, ChessPieceCoordinate chessPieceCoordinate, GameHistory history)
		{
			var fromCoordinate = chessPieceCoordinate.Coordinate;
			var chessPiece = chessPieceCoordinate.ChessPiece;

			switch (chessPiece.Type)
			{
				case ChessPieceType.King:
					return _kingMoveValidator.GetSoftValidKingMoves(chessboard, fromCoordinate, chessPiece.Owner);
				case ChessPieceType.Queen:
					return _queenMoveValidator.GetSoftValidQueenMoves(chessboard, fromCoordinate, chessPiece.Owner);
				case ChessPieceType.Rook:
					return _rookMoveValidator.GetSoftValidRookMoves(chessboard, fromCoordinate, chessPiece.Owner);
				case ChessPieceType.Bishop:
					return _bishopMoveValidator.GetSoftValidBishopMoves(chessboard, fromCoordinate, chessPiece.Owner);
				case ChessPieceType.Knight:
					return _knightMoveValidator.GetSoftValidKnightMoves(chessboard, fromCoordinate, chessPiece.Owner);
				case ChessPieceType.Pawn:
					return _pawnMoveValidator.GetPawnKillMoves(chessboard, fromCoordinate, chessPiece.Owner, history.GetLastMove());
				default:
					throw new ArgumentOutOfRangeException(nameof(chessPiece.Type), chessPiece.Type, null);
			}
		}

		private bool IsCoordinateInDanger(Chessboard chessboard, ChessColor turn, GameHistory gameHistory, Coordinate coordinate)
		{
			var opponentChessPieceCoordinates = chessboard.ChessPieceCoordinates.Where(x => x.ChessPiece.Owner != turn);

			var isCoordinateInDanger = opponentChessPieceCoordinates.Any(chessPieceCoordinate =>
				GetSoftValidMoves(chessboard, chessPieceCoordinate, gameHistory).Any(m => m.To == coordinate));

			return isCoordinateInDanger;
		}

		private List<GameMove> GetPossibleCastlingMoves(Chessboard chessboard, ChessColor turn, GameHistory gameHistory)
		{
			var possibleCastlingMoves = new List<GameMove>();

			bool IsAnyCoordinateInDanger(int number, params char[] letters)
			{
				return letters.Any(x => IsCoordinateInDanger(chessboard, turn, gameHistory, new Coordinate(x, number)));
			}

			if (turn == ChessColor.White)
			{
				if (gameHistory.WhiteLongCastlingPossible && !IsAnyCoordinateInDanger(1, 'A', 'B', 'C', 'D', 'E'))
					possibleCastlingMoves.Add(new GameMove {Castling = Castling.Long});

				if (gameHistory.WhiteShortCastlingPossible && !IsAnyCoordinateInDanger(1, 'E', 'F', 'G', 'H'))
					possibleCastlingMoves.Add(new GameMove {Castling = Castling.Short});
			}
			else
			{
				if (gameHistory.BlackLongCastlingPossible && !IsAnyCoordinateInDanger(8, 'A', 'B', 'C', 'D', 'E'))
					possibleCastlingMoves.Add(new GameMove {Castling = Castling.Long});

				if (gameHistory.BlackShortCastlingPossible && !IsAnyCoordinateInDanger(8, 'E', 'F', 'G', 'H'))
					possibleCastlingMoves.Add(new GameMove {Castling = Castling.Short});
			}

			return possibleCastlingMoves;
		}
	}
}
