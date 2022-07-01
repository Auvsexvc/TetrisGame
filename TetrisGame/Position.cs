namespace TetrisGame
{
    internal class Position
    {
        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Column { get; set; }
        public int Row { get; set; }
    }
}