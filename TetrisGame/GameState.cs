using System;

namespace TetrisGame
{
    internal class GameState
    {
        private Block currentBlock;

        public GameState()
        {
            Grid = new Grid(22, 10);
            BlockQueue = new BlockQueue();
            currentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
        }

        public BlockQueue BlockQueue { get; }

        public bool CanHold { get; private set; }

        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();

                for (int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);

                    if (!BlockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        }

        public bool GameOver { get; private set; }
        public Grid Grid { get; }
        public Block? HeldBlock { get; private set; }
        public int Score { get; private set; }

        public int BlockDropDistance()
        {
            int drop = Grid.Rows;

            foreach (var item in CurrentBlock.TilePositions())
            {
                drop = Math.Min(drop, TileDropDistance(item));
            }

            return drop;
        }

        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }

        public void HoldBlock()
        {
            if (!CanHold)
                return;

            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAndUpdate();
            }
            else
            {
                (HeldBlock, CurrentBlock) = (CurrentBlock, HeldBlock);
            }

            CanHold = false;
        }

        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);

            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }

        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, 1);
            }
        }

        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1);
            }
        }

        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCW();

            if (!BlockFits())
            {
                CurrentBlock.RotateCW();
            }
        }

        public void RotateBlockCW()
        {
            CurrentBlock.RotateCW();

            if (!BlockFits())
            {
                CurrentBlock.RotateCCW();
            }
        }

        private bool BlockFits()
        {
            foreach (var item in CurrentBlock.TilePositions())
            {
                if (!Grid.IsEmpty(item.Row, item.Column))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsGameOver() => !(Grid.IsRowEmpty(0) && Grid.IsRowEmpty(1));

        private void PlaceBlock()
        {
            foreach (var item in CurrentBlock.TilePositions())
            {
                Grid[item.Row, item.Column] = CurrentBlock.Id;
            }

            Score += Grid.ClearFullRows();

            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock = BlockQueue.GetAndUpdate();
                CanHold = true;
            }
        }

        private int TileDropDistance(Position p)
        {
            int drop = 0;

            while (Grid.IsEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }

            return drop;
        }
    }
}