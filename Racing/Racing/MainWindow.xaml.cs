using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Racing
{
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        List<Block> blocks = new List<Block>();
        List<Point> points = new List<Point>();
        Car car;

        public MainWindow()
        {
            InitializeComponent();
            StartGame();
        }

        private void StartGame()
        {
            UpdateGrid();
            points.Clear();
            blocks.Clear();
            blocks.Add(GetNewBlock());
            car = new Car();
            if (timer != null)
                timer.Stop();
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e)
        {
            UpdateGrid();
            foreach (var point in points)
            {
                DrawElement(point);
                point.Move();
            }
            foreach (var block in blocks)
            {
                DrawElement(block);
                block.Move();
            }
            DrawElement(car);
            CheckNewBlock();
            CheckNewPoint();
            CheckCrash();
            CheckScore();
            if (new Random().Next(100) == 1)
                blocks.Add(GetNewBlock());
            if (new Random().Next(100) == 1)
                points.Add(GetNewPoint());
            if (blocks.Count > 2) blocks.Clear();
            DrawBorder();
        }

        private void DrawBorder()
        {
            for (int i = 0; i < 19; i += 2)
            {
                Rectangle rect1 = new Rectangle();
                rect1.VerticalAlignment = VerticalAlignment.Stretch;
                rect1.HorizontalAlignment = HorizontalAlignment.Stretch;
                rect1.Fill = Brushes.Black;
                MainGrid.Children.Add(rect1);
                Grid.SetRow(rect1, i);
                Grid.SetColumn(rect1, 0);

                Rectangle rect2 = new Rectangle();
                rect2.VerticalAlignment = VerticalAlignment.Stretch;
                rect2.HorizontalAlignment = HorizontalAlignment.Stretch;
                rect2.Fill = Brushes.Black;
                MainGrid.Children.Add(rect2);
                Grid.SetRow(rect2, i);
                Grid.SetColumn(rect2, 9);
            }
        }

        private void CheckNewPoint()
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Cells.All(c => c.Row > 19))
                {
                    points.Remove(points[i]);
                    points.Add(GetNewPoint());
                    return;
                }
            }
        }

        private Point GetNewPoint()
        {
            return new Point(new Random().Next(1, 8));
        }

        private void CheckScore()
        {
            for (int i = 0; i < points.Count; i++)
            {
                foreach (var cell in points[i].Cells)
                {
                    foreach (var cellCar in car.Cells)
                    {
                        if (cell.Row == cellCar.Row && cell.Col == cellCar.Col)
                        {
                            points.Remove(points[i]);
                            points.Add(GetNewPoint());
                            car.AddPoint();
                            Score.Text = "Score: " + car.Points;
                            return;
                        }
                    }
                }
            }
        }

        private void CheckCrash()
        {
            foreach (var block in blocks)
            {
                foreach (var cell in block.Cells)
                {
                    foreach (var cellCar in car.Cells)
                    {
                        if (cell.Row == cellCar.Row && cell.Col == cellCar.Col)
                        {
                            timer.Stop();
                            MessageBox.Show("You died");
                            StartGame();
                            return;
                        }
                    }
                }
            }
        }

        private void CheckNewBlock()
        {
            for(int i = 0; i < blocks.Count; i++)
            {
                if (blocks[i].Cells.All(c => c.Row > 19))
                {
                    blocks.Remove(blocks[i]);
                    blocks.Add(GetNewBlock());
                    return;
                }
            }
        }

        private Block GetNewBlock()
        {
            return new Block(new Random().Next(1, 8));
        }

        private void DrawElement(GraphicsElement element)
        {
            foreach(var cell in element.Cells)
            {
                Rectangle rect = new Rectangle();
                rect.VerticalAlignment = VerticalAlignment.Stretch;
                rect.HorizontalAlignment = HorizontalAlignment.Stretch;
                rect.Fill = element.Color;
                MainGrid.Children.Add(rect);
                Grid.SetRow(rect, cell.Row);
                Grid.SetColumn(rect, cell.Col);
            }
        }

        private void UpdateGrid()
        {
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();
            MainGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < 10; i++)
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < 20; i++)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition());
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.A:
                    car.Offset(-1, blocks);
                    break;
                case System.Windows.Input.Key.D:
                    car.Offset(1, blocks);
                    break;
                case System.Windows.Input.Key.Space:
                    if (timer.IsEnabled)
                        timer.Stop();
                    else timer.Start();
                    break;
            }
        }
    }
}