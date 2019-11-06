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

		public GameMoveValidator()
		{
			_kingMoveValidator = new KingMoveValidator();
			_queenMoveValidator = new QueenMoveValidator();
			_rookMoveValidator = new RookMoveValidator();
			_bishopMoveValidator = new BishopMoveValidator();
		}

		public bool IsValid(Chessboard chessboard, GameMove move)
		{
			var chessPiece = chessboard.GetChessPiece(move.From);
			if (!chessPiece.HasValue)
				return false;

			if (!IsMoveSoftValid(chessboard, move, chessPiece.Value))
				return false;

			var kingCoordinate = chessboard.GetCoordinate(new ChessPiece {Owner = chessPiece.Value.Owner, Type = ChessPieceType.King});
			if (!kingCoordinate.HasValue)
				return false; // There is no King?


			var opponentChessPieceCoordinates = chessboard.ChessPieceCoordinates.Where(x => x.ChessPiece.Owner != chessPiece.Value.Owner);
			var isKingInDanger = opponentChessPieceCoordinates.Any(x =>
				IsMoveSoftValid(chessboard, new GameMove {From = x.Coordinate, To = kingCoordinate.Value}, x.ChessPiece));

			return !isKingInDanger;
		}

		public List<GameMove> GetAvailableMoves(Chessboard chessboard, ChessColor turn, GameMove? previousMove)
		{
			throw new NotImplementedException();
		}

		public bool IsGameFinished(Chessboard chessboard, GameHistory gameHistory)
		{
			throw new NotImplementedException();
		}

		public ChessGameResult? GetGameResult(Chessboard chessboard, GameHistory gameHistory)
		{
			throw new NotImplementedException();
		}

		private bool IsMoveSoftValid(Chessboard chessboard, GameMove move, ChessPiece chessPiece)
		{
			switch (chessPiece.Type)
			{
				case ChessPieceType.King:
					return _kingMoveValidator.IsKingMoveSoftValid(chessboard, move, chessPiece.Owner);
				case ChessPieceType.Queen:
					break;
				case ChessPieceType.Rook:
					break;
				case ChessPieceType.Bishop:
					break;
				case ChessPieceType.Knight:
					break;
				case ChessPieceType.Pawn:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(chessPiece.Type), chessPiece.Type, null);
			}

			throw new NotImplementedException();
		}

		private List<GameMove> GetSoftValidMoves(Chessboard chessboard, ChessPieceCoordinate chessPieceCoordinate)
		{
			var coordinate = chessPieceCoordinate.Coordinate;
			var chessPiece = chessPieceCoordinate.ChessPiece;

			switch (chessPiece.Type)
			{
				case ChessPieceType.King:
					return _kingMoveValidator.GetSoftValidKingMoves(chessboard, coordinate, chessPiece.Owner);
				case ChessPieceType.Queen:
					return _queenMoveValidator.GetSoftValidQueenMoves(chessboard, coordinate, chessPiece.Owner);
				case ChessPieceType.Rook:
					return _rookMoveValidator.GetSoftValidRookMoves(chessboard, coordinate, chessPiece.Owner);
				case ChessPieceType.Bishop:
					return _bishopMoveValidator.GetSoftValidBishopMoves(chessboard, coordinate, chessPiece.Owner);
				case ChessPieceType.Knight:
					break;
				case ChessPieceType.Pawn:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(chessPiece.Type), chessPiece.Type, null);
			}

			throw new NotImplementedException();
		}
	}
}
