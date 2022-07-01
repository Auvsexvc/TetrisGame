using System;

namespace TetrisGame
{
    internal class BlockQueue
    {
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };

        private readonly Random random = new();

        public BlockQueue()
        {
            NextBlock = RandomBlock();
        }

        public Block NextBlock { get; private set; }

        public Block GetAndUpdate()
        {
            Block block = NextBlock;

            do
            {
                NextBlock = RandomBlock();
            }
            while (block.Id == NextBlock.Id);

            return block;
        }

        private Block RandomBlock() => blocks[random.Next(blocks.Length)];
    }
}