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

			var opponentChessPieceCoordinates = chessBoardCopy.ChessPieceCoordinates.Where(x => x.ChessPiece.Owner != chessPiece.Value.Owner);

			var isKingInDanger = opponentChessPieceCoordinates.Any(chessPieceCoordinate =>
				GetSoftValidMoves(chessBoardCopy, chessPieceCoordinate, gameHistoryCopy).Any(m => m.To == kingCoordinate));

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

			availableMoves.AddRange(new List<GameMove>()); // TODO: Add possible castling

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
	}
}
