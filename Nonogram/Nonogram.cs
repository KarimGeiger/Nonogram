using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Nonogram
{
    class Nonogram
    {
        public const int BUTTON_WIDTH = 33;
        public const int BUTTON_HEIGHT = 33;
        public const int RASTER_START_X = 150;
        public const int RASTER_START_Y = 150;
        public const int PADDING = 1;

        public List<List<Box>> PointsX { get; set; } = new List<List<Box>>();
        public List<List<Box>> PointsY { get; set; } = new List<List<Box>>();
        public List<Label> Labels { get; set; } = new List<Label>();
        private List<List<int>> numbersX = new List<List<int>>();
        private List<List<int>> numbersY = new List<List<int>>();
        private static Random random = new Random();

        public Nonogram(int sizeX, int sizeY, int probability)
        {
            for (int y = 0; y < sizeY; y++)
            {
                PointsX.Add(new List<Box>());

                for (int x = 0; x < sizeX; x++)
                {
                    if (PointsY.Count == x)
                    {
                        PointsY.Add(new List<Box>());
                    }
                    var point = new Box(x, y, random.Next(0, probability) >= 5 ? Box.BoxState.MARKED : Box.BoxState.BLANK);

                    // TODO: These lists can be merged to one list, since they contain exactly the same points
                    PointsX[y].Add(point);
                    PointsY[x].Add(point);
                }

                numbersX.Add(GetNumbers(PointsX[y], true));
                Labels.Add(new Label
                {
                    Text = String.Join("  ", numbersX.Last()),
                    Top = RASTER_START_Y + (y * (BUTTON_HEIGHT + 1)),
                    Left = RASTER_START_X - 210,
                    Width = 200,
                    Height = BUTTON_HEIGHT,
                    TextAlign = ContentAlignment.MiddleRight
                });
            }

            for (int x = 0; x < sizeX; x++)
            {
                numbersY.Add(GetNumbers(PointsY[x], true));
                Labels.Add(new Label
                {
                    Text = String.Join("\n", numbersY.Last()),
                    Top = RASTER_START_X - 410,
                    Left = RASTER_START_Y + (x * (BUTTON_WIDTH + 1)),
                    Width = BUTTON_WIDTH,
                    Height = 400,
                    TextAlign = ContentAlignment.BottomCenter
                });
            }
        }

        private static List<int> GetNumbers(List<Box> list, bool forExpected)
        {
            var lastIndex = -2;
            var numbers = new List<int>();

            for (int i = 0; i < list.Count; i++)
            {
                if ((forExpected ? list[i].Expected : list[i].State) != Box.BoxState.MARKED)
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
