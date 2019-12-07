using Chess.CheckSpeed.Logger;
using Chess.CheckSpeed.SpeedTests;
using Chess.Engine.Abstract;
using Chess.Engine.Enums;
using Chess.Engine.Game;
using Chess.MinimaxBot.PrimitiveBot;
using System;
using System.Diagnostics;

namespace Chess.CheckSpeed
{
	public static class Program
	{
		private static readonly ILogger _logger = new FileLogger(true);

		public static void Main(string[] args)
		{
			StartGame();

			PressKeyToExit();
		}

		private static void StartGame()
		{
			var primitiveBot = new PrimitiveBot
			{
				TimeForSearching = TimeSpan.FromMinutes(1)
			};

			var quiescenceSearchBot = new PrimitiveBot
			{
				TimeForSearching = TimeSpan.FromMinutes(1)
			};

			var primitiveBotStopwatch = new Stopwatch();
			var quiescenceSearchBotStopwatch = new Stopwatch();
			var gameState = GameState.CreateNewGameState(true);

			try
			{
				while (gameState.GameStatus != GameStatus.Finished)
				{
					if (gameState.Turn == ChessColor.White)
					{
						ProcessMove(gameState, primitiveBot, primitiveBotStopwatch);
					}
					else
					{
						ProcessMove(gameState, quiescenceSearchBot, quiescenceSearchBotStopwatch);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);

				if (e.Message == "The game is finished.")
					Console.WriteLine(gameState.GetGameResult());
			}

			_logger.Log(string.Empty);
			_logger.Log(string.Empty);
			_logger.Log($"Game result: {gameState.GetGameResult()}");
		}

		private static void ProcessMove(GameState gameState, IChessBot chessBot, Stopwatch stopwatch)
		{
			stopwatch.Start();

			chessBot.StartSearch(gameState);
			var move = chessBot.TheBestMove;

			gameState.Move(move);

			_logger.Log($"{chessBot.GetType()}. {stopwatch.ElapsedMilliseconds}");
			_logger.Log($"Move: {move}");
			_logger.Log(string.Empty);

			stopwatch.Stop();
		}

		private static void CheckGetAvailableMovesSpeed()
		{
			var getAvailableMovesSpeedTest = new AvailableMovesSpeedTest(_logger);

			getAvailableMovesSpeedTest.CheckCalculatePossibleMovesSpeed();
		}

		private static void PressKeyToExit()
		{
			Console.WriteLine("Press any key to exit.");
			Console.ReadKey();
		}
	}
}
