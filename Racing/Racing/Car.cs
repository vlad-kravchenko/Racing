using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Racing
{
    public class Car : GraphicsElement
    {
        public int Points { get; private set; }

        public Car()
        {
            Cells = new List<Cell>()
            {
                new Cell{Row = 16, Col = 4},
                new Cell{Row = 16, Col = 5},
                new Cell{Row = 17, Col = 4},
                new Cell{Row = 17, Col = 5},
                new Cell{Row = 18, Col = 4},
                new Cell{Row = 18, Col = 5},
            };
            Color = Brushes.Green;
            Points = 0;
        }

        public void AddPoint() => Points++;

        public void Offset(int step, List<Block> blocks)
        {
            if (step > 0)
            {
                if (Cells.Max(c => c.Col) < 8)
                {
                    if (!Hit(blocks, step))
                        Cells.ForEach(c => c.Col++);
                }
            }
            else
            {
                if (Cells.Min(c => c.Col) > 1)
                {
                    if (!Hit(blocks, step))
                        Cells.ForEach(c => c.Col--);
                }
            }
        }

        private bool Hit(List<Block> blocks, int step)
        {
            foreach (var block in blocks)
            {
                foreach (var cell in block.Cells)
                {
                    foreach (var cellCar in Cells)
                    {
                        if (cell.Row == cellCar.Row + step && cell.Col == cellCar.Col + step)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}