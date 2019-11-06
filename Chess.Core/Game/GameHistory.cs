using Chess.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Core.Game
{
	public class GameHistory : ICloneable
	{
		protected List<GameMove> Moves { get; private set; }

		public GameHistory()
		{
			Moves = new List<GameMove>();
		}

		public void Add(GameMove move)
		{
			Moves.Add(move);
		}

		public GameMove? LastMove => Moves.LastOrDefault();

		public object Clone()
		{
			return new GameHistory
			{
				Moves = Moves.ToList()
			};
		}
	}
}
