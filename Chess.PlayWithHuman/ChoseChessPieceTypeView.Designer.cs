namespace Chess.PlayWithHuman
{
	partial class ChoseChessPieceTypeView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnKnight = new System.Windows.Forms.Button();
			this.btnQueen = new System.Windows.Forms.Button();
			this.btnRook = new System.Windows.Forms.Button();
			this.btnBishop = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnKnight
			// 
			this.btnKnight.Location = new System.Drawing.Point(8, 116);
			this.btnKnight.Name = "btnKnight";
			this.btnKnight.Size = new System.Drawing.Size(249, 31);
			this.btnKnight.TabIndex = 3;
			this.btnKnight.Text = "Knight";
			this.btnKnight.UseVisualStyleBackColor = true;
			this.btnKnight.Click += new System.EventHandler(this.BtnKnight_Click);
			// 
			// btnQueen
			// 
			this.btnQueen.Location = new System.Drawing.Point(8, 12);
			this.btnQueen.Name = "btnQueen";
			this.btnQueen.Size = new System.Drawing.Size(249, 31);
			this.btnQueen.TabIndex = 4;
			this.btnQueen.Text = "Queen";
			this.btnQueen.UseVisualStyleBackColor = true;
			this.btnQueen.Click += new System.EventHandler(this.BtnQueen_Click);
			// 
			// btnRook
			// 
			this.btnRook.Location = new System.Drawing.Point(8, 47);
			this.btnRook.Name = "btnRook";
			this.btnRook.Size = new System.Drawing.Size(249, 31);
			this.btnRook.TabIndex = 5;
			this.btnRook.Text = "Rook";
			this.btnRook.UseVisualStyleBackColor = true;
			this.btnRook.Click += new System.EventHandler(this.BtnRook_Click);
			// 
			// btnBishop
			// 
			this.btnBishop.Location = new System.Drawing.Point(8, 81);
			this.btnBishop.Name = "btnBishop";
			this.btnBishop.Size = new System.Drawing.Size(249, 31);
			this.btnBishop.TabIndex = 6;
			this.btnBishop.Text = "Bishop";
			this.btnBishop.UseVisualStyleBackColor = true;
			this.btnBishop.Click += new System.EventHandler(this.BtnBishop_Click);
			// 
			// ChoseChessPieceTypeView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(270, 170);
			this.Controls.Add(this.btnBishop);
			this.Controls.Add(this.btnRook);
			this.Controls.Add(this.btnQueen);
			this.Controls.Add(this.btnKnight);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "ChoseChessPieceTypeView";
			this.Text = "ChoseChessPieceTypeView";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnKnight;
		private System.Windows.Forms.Button btnQueen;
		private System.Windows.Forms.Button btnRook;
		private System.Windows.Forms.Button btnBishop;
	}
}