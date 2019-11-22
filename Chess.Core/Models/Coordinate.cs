namespace Chess.Core.Models
{
	public struct Coordinate
	{
		public char Letter { get; set; }
		public int Number { get; set; }

		public Coordinate(char letter, int number)
		{
			Letter = letter;
			Number = number;
		}

		public static bool operator ==(Coordinate coordinateA, Coordinate coordinateB)
		{
			return coordinateA.Letter == coordinateB.Letter && coordinateA.Number == coordinateB.Number;
		}

		public static bool operator !=(Coordinate coordinateA, Coordinate coordinateB)
		{
			return !(coordinateA == coordinateB);
		}

		public void ToArrayIndexes(out int i, out int j)
		{
			i = Number - 1;
			j = Letter - 'A';
		}
	}
}
