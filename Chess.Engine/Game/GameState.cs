using Chess.Engine.Enums;
using Chess.Engine.Enums.Extensions;
using Chess.Engine.Logic;
using Chess.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Engine.Game
{
	public class GameState : ICloneable
	{
		private readonly GameMoveValidator _gameMoveValidator;
		private ChessGameResult? _gameResult;
		private Lazy<List<GameMove>> _lazyInterestingGameMoves;

		public Chessboard Chessboard { get; }
		public GameHistory History { get; }
		public ChessColor Turn { get; private set; }
		public GameStatus GameStatus { get; private set; }
		public List<GameMove> PossibleGameMoves { get; private set; }
		public List<GameMove> InterestingGameMoves => _lazyInterestingGameMoves.Value;

		private GameState(Chessboard chessboard, GameStatus gameStatus, GameHistory history, ChessColor turn)
		{
			Chessboard = chessboard;
			GameStatus = gameStatus;
			History = history;
			Turn = turn;

			_gameMoveValidator = new GameMoveValidator();

			CalculatePossibleGameMoves();
		}

		public static GameState CreateGameStateFromPosition(Chessboard chessboard, bool castlingsPossible, ChessColor turn)
		{
			return new GameState(chessboard, GameStatus.Continues, new GameHistory(castlingsPossible), turn);
		}

		public static GameState CreateNewGameState()
		{
			return new GameState(new Chessboard(), GameStatus.NotStarted, new GameHistory(), ChessColor.White);
		}

		public object Clone()
		{
			var gameState = new GameState((Chessboard) Chessboard.Clone(), GameStatus, (GameHistory) History.Clone(), Turn);

			gameState._gameResult = _gameResult;
			gameState.PossibleGameMoves = PossibleGameMoves.ToList();
			gameState._lazyInterestingGameMoves = _lazyInterestingGameMoves;

			return gameState;
		}

		public ChessGameResult GetGameResult()
		{
			if (GameStatus != GameStatus.Finished)
				throw new Exception("Game is not finished yet.");

			if (!_gameResult.HasValue)
				throw new Exception("Game result has no value! Game is finished.");

			return _gameResult.Value;
		}

		public void Move(GameMove move)
		{
			if (GameStatus == GameStatus.Finished)
				throw new Exception("The game is finished.");

			History.Add(move, Turn, Chessboard);

			if (move.Castling.HasValue)
			{
				DoCastling(move);
			}
			else
			{
				var chessPiece = Chessboard.GetChessPiece(move.From);
				if (chessPiece.Type == ChessPieceType.Pawn && move.From.Letter != move.To.Letter)
				{
					var victim = Chessboard.GetChessPieceOrDefault(move.To);
					if (!victim.HasValue)
						Chessboard.Set(null, new Coordinate(move.To.Letter, chessPiece.Owner == ChessColor.White ? 5 : 4));
				}

				Chessboard.Move(move);
				if (move.CastTo.HasValue)
					Chessboard.Set(new ChessPiece {Type = move.CastTo.Value, Owner = Turn}, move.To);
			}

			CalculatePossibleGameMoves(Turn.GetOppositeChessColor());

			if (!PossibleGameMoves.Any() || History.IsPositionRepeatedThreeTimes)
			{
				GameStatus = GameStatus.Finished;
				if (History.IsPositionRepeatedThreeTimes)
					_gameResult = ChessGameResult.Draw;
				else
				{
					if (_gameMoveValidator.IsCoordinateInDanger(Chessboard, Turn.GetOppositeChessColor(),
						Chessboard.GetCoordinate(new ChessPiece {Owner = Turn.GetOppositeChessColor(), Type = ChessPieceType.King})))
						_gameResult = Turn == ChessColor.White ? ChessGameResult.WhiteWon : ChessGameResult.BlackWon;
					else
						_gameResult = ChessGameResult.Draw;
				}
			}
			else
			{
				if (GameStatus == GameStatus.NotStarted)
					GameStatus = GameStatus.Continues;

				Turn = Turn.GetOppositeChessColor();
			}
		}

		private void DoCastling(GameMove move)
		{
			if (!move.Castling.HasValue)
				throw new ArgumentNullException(nameof(move.Castling));

			var castling = move.Castling.Value;
			var king = Chessboard.GetChessPiece(move.From);

			var lineNumber = king.Owner == ChessColor.White ? 1 : 8;

			if (castling == Castling.Short)
			{
				Chessboard.Move(new GameMove { From = new Coordinate('E', lineNumber), To = new Coordinate('G', lineNumber) });
				Chessboard.Move(new GameMove { From = new Coordinate('H', lineNumber), To = new Coordinate('F', lineNumber) });
			}
			else
			{
				Chessboard.Move(new GameMove { From = new Coordinate('E', lineNumber), To = new Coordinate('C', lineNumber) });
				Chessboard.Move(new GameMove { From = new Coordinate('A', lineNumber), To = new Coordinate('D', lineNumber) });
			}
		}

		internal void CalculatePossibleGameMoves(ChessColor? turn = null)
		{
			PossibleGameMoves = _gameMoveValidator.GetAvailableMoves(Chessboard, turn ?? Turn, History);
			_lazyInterestingGameMoves = new Lazy<List<GameMove>>(() => _gameMoveValidator.GetAvailableMoves(Chessboard, turn ?? Turn, History, true));
		}
	}
}
