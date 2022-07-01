using System.Collections.Generic;

namespace TetrisGame
{
    internal abstract class Block
    {
        private readonly Position offset;
        private int rotationState;

        protected Block()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        public abstract int Id { get; }
        protected abstract Position StartOffset { get; }
        protected abstract Position[][] Tiles { get; }

        public void Move(int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }

        public void Reset()
        {
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }

        public void RotateCCW()
        {
            if (rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }

        public void RotateCW()
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }

        public IEnumerable<Position> TilePositions()
        {
            foreach (var item in Tiles[rotationState])
            {
                yield return new Position(item.Row + offset.Row, item.Column + offset.Column);
            }
        }
    }
}