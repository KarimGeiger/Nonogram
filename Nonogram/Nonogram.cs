using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Nonogram
{
    class Nonogram
    {
        public List<List<Point>> PointsX { get; set; } = new List<List<Point>>();
        public List<List<Point>> PointsY { get; set; } = new List<List<Point>>();
        public List<Label> Labels { get; set; } = new List<Label>();
        private List<List<int>> numbersX = new List<List<int>>();
        private List<List<int>> numbersY = new List<List<int>>();
        private static Random random = new Random();

        public Nonogram(int sizeX, int sizeY, int probability)
        {
            for (int y = 0; y < sizeY; y++)
            {
                PointsX.Add(new List<Point>());

                for (int x = 0; x < sizeX; x++)
                {
                    if (PointsY.Count == x)
                    {
                        PointsY.Add(new List<Point>());
                    }
                    var point = new Point
                    {
                        X = x,
                        Y = y,
                        Expected = random.Next(0, probability) >= 5 ? Point.State.MARKED : Point.State.BLANK
                    };

                    // TODO: These lists can be merged to one list, since they contain exactly the same points
                    PointsX[y].Add(point);
                    PointsY[x].Add(point);
                }

                numbersX.Add(GetNumbers(PointsX[y], true));
                Labels.Add(new Label
                {
                    Text = String.Join("  ", numbersX.Last()),
                    Top = Form1.RASTER_START_Y + (y * (Form1.BUTTON_HEIGHT + 1)),
                    Left = Form1.RASTER_START_X - 210,
                    Width = 200,
                    Height = Form1.BUTTON_HEIGHT,
                    TextAlign = ContentAlignment.MiddleRight
                });
            }

            for (int x = 0; x < sizeX; x++)
            {
                numbersY.Add(GetNumbers(PointsY[x], true));
                Labels.Add(new Label
                {
                    Text = String.Join("\n", numbersY.Last()),
                    Top = Form1.RASTER_START_X - 410,
                    Left = Form1.RASTER_START_Y + (x * (Form1.BUTTON_WIDTH + 1)),
                    Width = Form1.BUTTON_WIDTH,
                    Height = 400,
                    TextAlign = ContentAlignment.BottomCenter
                });
            }
        }

        private static List<int> GetNumbers(List<Point> list, bool forExpected)
        {
            var lastIndex = -2;
            var numbers = new List<int>();

            for (int i = 0; i < list.Count; i++)
            {
                if ((forExpected ? list[i].Expected : list[i].state) != Point.State.MARKED)
                {
                    lastIndex = -2;
                    continue;
                }
                if (lastIndex == i - 1)
                {
                    numbers[numbers.Count - 1]++;
                }
                else
                {
                    numbers.Add(1);
                }
                lastIndex = i;
            }

            if (numbers.Count == 0)
            {
                numbers.Add(0);
            }

            return numbers;
        }

        public bool IsValid()
        {
            for (int x = 0; x < numbersY.Count; x++)
            {
                if (!numbersY[x].SequenceEqual(GetNumbers(PointsY[x], false)))
                {
                    return false;
                }
            }

            for (int y = 0; y < numbersX.Count; y++)
            {
                if (!numbersX[y].SequenceEqual(GetNumbers(PointsX[y], false)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
