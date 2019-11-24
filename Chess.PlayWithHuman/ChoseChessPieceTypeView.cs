using Chess.Engine.Enums;
using System;
using System.Windows.Forms;

namespace Chess.PlayWithHuman
{
	public partial class ChoseChessPieceTypeView : Form
	{
		public ChessPieceType ChosenChessPieceType { get; private set; }
		public ChoseChessPieceTypeView()
		{
			InitializeComponent();
			ChosenChessPieceType = ChessPieceType.Queen;
		}

		private void BtnQueen_Click(object sender, EventArgs e)
		{
			ChosenChessPieceType = ChessPieceType.Queen;
			Close();
		}

		private void BtnRook_Click(object sender, EventArgs e)
		{
			ChosenChessPieceType = ChessPieceType.Rook;
			Close();
		}

		private void BtnBishop_Click(object sender, EventArgs e)
		{
			ChosenChessPieceType = ChessPieceType.Bishop;
			Close();
		}

		private void BtnKnight_Click(object sender, EventArgs e)
		{
			ChosenChessPieceType = ChessPieceType.Knight;
			Close();
		}
	}
}
