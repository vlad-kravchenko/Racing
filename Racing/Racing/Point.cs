using System.Collections.Generic;
using System.Windows.Media;

namespace Racing
{
    public class Point : GraphicsElement
    {
        public Point(int col)
        {
            Cells = new List<Cell>
            {
                new Cell{Row = 0, Col = col}
            };
            Color = Brushes.Red;
        }

        public void Move()
        {
            Cells.ForEach(c => c.Row++);
        }
    }
}