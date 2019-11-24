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

		private GameState()
		{
			_gameMoveValidator = new GameMoveValidator();
		}

		public static GameState GetNewGameState()
		{
			return new GameState
			{
				Chessboard = new Chessboard(),
				GameStatus = GameStatus.NotStarted,
				History = new GameHistory(),
				Turn = ChessColor.White
			};
		}

		public object Clone()
		{
			return new GameState
			{
				Chessboard = (Chessboard) Chessboard.Clone(),
				History = (GameHistory) History.Clone(),
				Turn = Turn,
				GameStatus = GameStatus,
				_gameResult = _gameResult
			};
		}

		public Chessboard Chessboard { get; private set; }
		public GameHistory History { get; private set; }
		public ChessColor Turn { get; private set; }
		public GameStatus GameStatus { get; private set; }

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
				Chessboard.Move(move);
			}

			if (_gameMoveValidator.IsGameFinished(Chessboard, History, Turn))
			{
				GameStatus = GameStatus.Finished;
				_gameResult = _gameMoveValidator.GetGameResult(Chessboard, History, Turn);
			}
			else
			{
				if (GameStatus == GameStatus.NotStarted)
					GameStatus = GameStatus.Continues;

				Turn = Turn.GetOppositeChessColor();
			}
		}

		public List<GameMove> GetAvailableMoves()
		{
			var possibleMoves = _gameMoveValidator.GetAvailableMoves(Chessboard, Turn, History);

			if (!possibleMoves.Any())
				throw new Exception("No moves available!");

			return possibleMoves;
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
	}
}
