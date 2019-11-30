using Chess.Engine.Enums;
using Chess.Engine.Game;
using Chess.Engine.Models;
using Chess.MinimaxBot.PrimitiveBot;
using Chess.PlayWithHuman.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess.PlayWithHuman
{
	public partial class PlayWithHumanView : Form
	{
		private readonly GameState _gameState;
		private readonly List<ChessboardFieldLabel> _chessboardFieldLabels;

		private readonly PrimitiveBot _primitiveBot;
		private readonly ChessColor _humanPlayer;

		private Coordinate? _fromCoordinate;

		public PlayWithHumanView()
		{
			_gameState = GameState.CreateNewGameState();
			InitializeComponent();

			_chessboardFieldLabels = new List<ChessboardFieldLabel>();
			SetLabels();
			_primitiveBot = new PrimitiveBot {TimeSpanForSearching = TimeSpan.FromSeconds(trackBar.Value)};
			_humanPlayer = ChessColor.White;
		}

		private async void LabelOnClick(object sender, EventArgs args)
		{
			if (_gameState.GameStatus == GameStatus.Finished)
				return;

			if (_gameState.Turn != _humanPlayer)
				return;

			var chessboardFieldLabel = _chessboardFieldLabels.First(x => x.Label == sender);

			if (!_fromCoordinate.HasValue)
			{
				if (_gameState.PossibleGameMoves.Any(x => x.From == chessboardFieldLabel.Coordinate))
					_fromCoordinate = chessboardFieldLabel.Coordinate;
			}
			else
			{
				var moves = _gameState.PossibleGameMoves
					.Where(x => x.From == _fromCoordinate && x.To == chessboardFieldLabel.Coordinate)
					.ToList();

				if (moves.Any())
				{
					if (moves.Count > 1)
					{
						var choseChessPieceTypeView = new ChoseChessPieceTypeView();
						choseChessPieceTypeView.ShowDialog(this);
						moves = moves.Where(x => x.CastTo == choseChessPieceTypeView.ChosenChessPieceType).ToList();
					}

					await ProcessMoveAsync(moves.Single());
					_fromCoordinate = null;
				}
				else
					_fromCoordinate = chessboardFieldLabel.Coordinate;
			}
		}

		private async Task ProcessMoveAsync(GameMove move)
		{
			_gameState.Move(move);
			SetChessboardLabelsStyles();

			if (_gameState.Turn != _humanPlayer)
			{
				await Task.Run(() =>
				{
					_primitiveBot.StartSearch(_gameState);
					return ProcessMoveAsync(_primitiveBot.TheBestMove);
				});
			}
		}

		private void SetLabels()
		{
			for (var i = 0; i < 8; i++)
			for (var j = 0; j < 8; j++)
			{
				var label = CreateLabel(i, j);
				_chessboardFieldLabels.Add(new ChessboardFieldLabel { Coordinate = Chessboard.GetCoordinate(i, j), Label = label });

				ToTableLayoutPanelCoordinates(i, j, out var column, out var row);
				tableLayoutPanel.Controls.Add(label, column, row);
			}

			SetChessboardLabelsStyles();
		}

		private void SetChessboardLabelsStyles()
		{
			_chessboardFieldLabels.SetChessboardLabelsStyles(_gameState.Chessboard);

		}

		private Label CreateLabel(int i, int j)
		{
			var odd = (i + j) % 2 == 0;
			var label = new Label
			{
				Dock = DockStyle.Fill,
				Margin = GetPadding(i, j),
				TextAlign = ContentAlignment.MiddleCenter,
				BackColor = odd ? Color.BurlyWood : Color.SaddleBrown
			};

			label.Click += LabelOnClick;

			return label;
		}

		private Padding GetPadding(int i, int j)
		{
			const int thinPadding = 1;
			if (i == 0 && j == 0) return new Padding(thinPadding * 2, thinPadding, thinPadding, thinPadding * 2);
			if (i == 0 && j == 7) return new Padding(thinPadding, thinPadding, thinPadding * 2, thinPadding * 2);
			if (i == 0) return new Padding(thinPadding, thinPadding, thinPadding, thinPadding * 2);
			if (i == 7 && j == 0) return new Padding(thinPadding * 2, thinPadding * 2, thinPadding, thinPadding);
			if (i == 7 && j == 7) return new Padding(thinPadding, thinPadding * 2, thinPadding * 2, thinPadding);
			if (i == 7) return new Padding(thinPadding, thinPadding * 2, thinPadding, thinPadding);
			if (j == 0) return new Padding(thinPadding * 2, thinPadding, thinPadding, thinPadding);
			if (j == 7) return new Padding(thinPadding, thinPadding, thinPadding * 2, thinPadding);
			return new Padding(thinPadding);
		}

		private static void ToTableLayoutPanelCoordinates(int i, int j, out int column, out int row)
		{
			column = j;
			row = 7 - i;
		}

		private void TrackBar_Scroll(object sender, EventArgs e)
		{
			_primitiveBot.TimeSpanForSearching = TimeSpan.FromSeconds(trackBar.Value);
		}
	}
}
