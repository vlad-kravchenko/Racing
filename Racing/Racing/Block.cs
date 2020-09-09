using System.Collections.Generic;
using System.Windows.Media;

namespace Racing
{
    public class Block : GraphicsElement
    {

        public Block(int col)
        {
            Cells = new List<Cell>()
            {
                new Cell{Row = 0, Col = col},
                new Cell{Row = 0, Col = col + 1},
                new Cell{Row = 1, Col = col},
                new Cell{Row = 1, Col = col + 1},
            };
            Color = Brushes.Black;
        }

        public void Move()
        {
            Cells.ForEach(c => c.Row++);
        }
    }
}