using Chess.Engine.Enums;
using Chess.Engine.Enums.Extensions;
using Chess.Engine.Game;
using Chess.Engine.Logic.ChessPieceMoveValidators;
using Chess.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Engine.Logic
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

		public List<GameMove> GetAvailableMoves(
			Chessboard chessboard, 
			ChessColor turn, 
			GameHistory gameHistory,
			bool onlyInteresting = false)
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
				.Where(x => onlyInteresting ? IsInteresting(chessboard, x) : IsValid(chessboard, x))
				.ToList();

			availableMoves.AddRange(GetPossibleCastlingMoves(chessboard, turn, gameHistory));

			return availableMoves;
		}

		private bool IsValid(Chessboard chessboard, GameMove move)
		{
			var chessPiece = chessboard.GetChessPieceOrDefault(move.From);
			if (!chessPiece.HasValue)
				return false;

			var victim = chessboard.Move(move);

			var kingCoordinate = chessboard.GetCoordinate(new ChessPiece { Owner = chessPiece.Value.Owner, Type = ChessPieceType.King });
			var isKingInDanger = IsCoordinateInDanger(chessboard, chessPiece.Value.Owner, kingCoordinate);

			chessboard.UnMove(move, victim);

			return !isKingInDanger;
		}

		private bool IsInteresting(Chessboard chessboard, GameMove move)
		{
			var isInteresting = IsInterestingInternal(chessboard, move, out var victim);
			chessboard.UnMove(move, victim);
			return isInteresting;
		}

		private bool IsInterestingInternal(Chessboard chessboard, GameMove move, out ChessPiece? victim)
		{
			var chessPiecesBefore = chessboard.ChessPieces;

			var chessPiece = chessboard.GetChessPiece(move.From);

			victim = chessboard.Move(move);

			var kingCoordinate = chessboard.GetCoordinate(new ChessPiece
				{Owner = chessPiece.Owner, Type = ChessPieceType.King});
			var isKingInDanger = IsCoordinateInDanger(chessboard, chessPiece.Owner, kingCoordinate);

			if (isKingInDanger)
				return false;

			var chessPiecesAfter = chessboard.ChessPieces;
			if (chessPiecesAfter.Count != chessPiecesBefore.Count)
				return true;

			if (chessPiecesAfter.Count(x => x.Type == ChessPieceType.Bishop) !=
			    chessPiecesBefore.Count(x => x.Type == ChessPieceType.Bishop))
				return true;

			var opponentColor = chessPiece.Owner.GetOppositeChessColor();
			var isCoordinateInDanger = IsCoordinateInDanger(
				chessboard,
				opponentColor,
				chessboard.GetCoordinate(new ChessPiece {Owner = opponentColor, Type = ChessPieceType.King}));

			return isCoordinateInDanger;
		}

		private IEnumerable<GameMove> GetSoftValidMoves(Chessboard chessboard, ChessPieceCoordinate chessPieceCoordinate,
			GameHistory history)
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
					return _pawnMoveValidator.GetPawnKillMoves(chessboard, fromCoordinate, chessPiece.Owner, history.LastMove);
				default:
					throw new ArgumentOutOfRangeException(nameof(chessPiece.Type), chessPiece.Type, null);
			}
		}

		private bool CanMove(Chessboard chessboard, ChessPiece chessPiece, GameMove move)
		{
			switch (chessPiece.Type)
			{
				case ChessPieceType.King:
					return _kingMoveValidator.CanMove(chessboard, move);
				case ChessPieceType.Queen:
					return _queenMoveValidator.CanMove(chessboard, move);
				case ChessPieceType.Rook:
					return _rookMoveValidator.CanMove(chessboard, move);
				case ChessPieceType.Bishop:
					return _bishopMoveValidator.CanMove(chessboard, move);
				case ChessPieceType.Knight:
					return _knightMoveValidator.CanMove(chessboard, move);
				case ChessPieceType.Pawn:
					return _pawnMoveValidator.CanMove(chessboard, chessPiece.Owner, move);
				default:
					throw new ArgumentOutOfRangeException(nameof(chessPiece.Type), chessPiece.Type, null);
			}
		}

		public bool IsCoordinateInDanger(Chessboard chessboard, ChessColor turn, Coordinate coordinate)
		{
			var opponentChessPieceCoordinates = chessboard.ChessPieceCoordinates.Where(x => x.ChessPiece.Owner != turn);

			var isCoordinateInDanger = opponentChessPieceCoordinates
				.Any(x => CanMove(chessboard, x.ChessPiece, new GameMove {From = x.Coordinate, To = coordinate}));

			return isCoordinateInDanger;
		}

		private List<GameMove> GetPossibleCastlingMoves(Chessboard chessboard, ChessColor turn, GameHistory gameHistory)
		{
			var possibleCastlingMoves = new List<GameMove>();

			bool IsAnyCoordinateInDanger(int number, params char[] letters)
			{
				return letters.Any(x => IsCoordinateInDanger(chessboard, turn, new Coordinate(x, number)));
			}

			bool IsAnyCoordinateNotEmpty(int number, params char[] letters)
			{
				return letters.Any(x => chessboard.GetChessPieceOrDefault(new Coordinate(x, number)).HasValue);
			}

			if (turn == ChessColor.White)
			{
				if (gameHistory.WhiteLongCastlingPossible &&
				    !IsAnyCoordinateNotEmpty(1, 'B', 'C', 'D') &&
				    !IsAnyCoordinateInDanger(1, 'A', 'B', 'C', 'D', 'E'))
					possibleCastlingMoves.Add(new GameMove
					{
						From = new Coordinate('E', 1),
						To = new Coordinate('C', 1),
						Castling = Castling.Long
					});

				if (gameHistory.WhiteShortCastlingPossible &&
					!IsAnyCoordinateNotEmpty(1, 'F', 'G') && 
					!IsAnyCoordinateInDanger(1, 'E', 'F', 'G', 'H'))
					possibleCastlingMoves.Add(new GameMove
					{
						From = new Coordinate('E', 1),
						To = new Coordinate('G', 1),
						Castling = Castling.Short
					});
			}
			else
			{
				if (gameHistory.BlackLongCastlingPossible &&
					!IsAnyCoordinateNotEmpty(8, 'B', 'C', 'D') &&
					!IsAnyCoordinateInDanger(8, 'A', 'B', 'C', 'D', 'E'))
					possibleCastlingMoves.Add(new GameMove
					{
						From = new Coordinate('E', 8),
						To = new Coordinate('C', 8),
						Castling = Castling.Long
					});

				if (gameHistory.BlackShortCastlingPossible &&
				    !IsAnyCoordinateNotEmpty(8, 'F', 'G') &&
					!IsAnyCoordinateInDanger(8, 'E', 'F', 'G', 'H'))
					possibleCastlingMoves.Add(new GameMove
					{
						From = new Coordinate('E', 8),
						To = new Coordinate('G', 8),
						Castling = Castling.Short
					});
			}

			return possibleCastlingMoves;
		}
	}
}
