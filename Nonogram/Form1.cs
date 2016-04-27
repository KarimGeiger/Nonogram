using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nonogram
{
    public partial class Form1 : Form
    {
        public const int BUTTON_WIDTH = 33;
        public const int BUTTON_HEIGHT = 33;
        public const int RASTER_START_X = 150;
        public const int RASTER_START_Y = 150;
        public const int PADDING = 1;

        List<List<Point>> pointsX = new List<List<Point>>();
        List<List<Point>> pointsY = new List<List<Point>>();
        List<Button> buttons = new List<Button>();
        List<Label> labels = new List<Label>();
        List<List<int>> numbersX = new List<List<int>>();
        List<List<int>> numbersY = new List<List<int>>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbLevel.Items.Add(Point.Level.Easy);
            cbLevel.Items.Add(Point.Level.Normal);
            cbLevel.Items.Add(Point.Level.Hard);
            cbLevel.SelectedIndex = 1;

            DrawNonogram(10, 10, (int)Point.Level.Normal);
        }

        private void DrawNonogram(int sizeX, int sizeY, int probability)
        {
            for (int y = 0; y < sizeY; y++)
            {
                pointsX.Add(new List<Point>());
                for (int x = 0; x < sizeX; x++)
                {
                    if (pointsY.Count == x)
                    {
                        pointsY.Add(new List<Point>());
                    }
                    var point = new Point(probability) { x = x, y = y };

                    pointsX[y].Add(point);
                    pointsY[x].Add(point);

                    buttons.Add(point.Button());
                    Controls.Add(point.Button());
                }

                numbersX.Add(GetNumbers(pointsX[y], true));
                var label = new Label
                {
                    Text = String.Join("  ", numbersX.Last()),
                    Top = RASTER_START_Y + (y * (BUTTON_HEIGHT + 1)),
                    Left = RASTER_START_X - 210,
                    Width = 200,
                    Height = BUTTON_HEIGHT,
                    TextAlign = ContentAlignment.MiddleRight
                };
                labels.Add(label);
                this.Controls.Add(label);
            }

            for (int x = 0; x < sizeX; x++)
            {
                numbersY.Add(GetNumbers(pointsY[x], true));

                var label = new Label
                {
                    Text = String.Join("\n", numbersY.Last()),
                    Top = RASTER_START_X - 410,
                    Left = RASTER_START_Y + (x * (BUTTON_WIDTH + 1)),
                    Width = BUTTON_WIDTH,
                    Height = 400,
                    TextAlign = ContentAlignment.BottomCenter
                };
                labels.Add(label);
                this.Controls.Add(label);
            }
        }

        private List<int> GetNumbers(List<Point> list, bool forExpected)
        {
            var lastIndex = -2;
            var numbers = new List<int>();

            for (int i = 0; i < list.Count; i++)
            {
                if ((forExpected ? list[i].expected : list[i].state) != Point.State.MARKED)
                {
                    lastIndex = -2;
                }
                else if (lastIndex == i - 1)
                {
                    numbers[numbers.Count - 1] = numbers.Last() + 1;
                    lastIndex = i;
                }
                else
                {
                    lastIndex = i;
                    numbers.Add(1);
                }
            }

            if (numbers.Count == 0)
            {
                numbers.Add(0);
            }

            return numbers;

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "If you do this, your current Nonogram will be lost.", 
                "Please Confirm", 
                MessageBoxButtons.YesNo
                ) != DialogResult.Yes)
            {
                return;
            }

            // Clean up old Nonogram
            labels.ForEach(label =>
            {
                this.Controls.Remove(label);
            });

            buttons.ForEach(button =>
            {
                this.Controls.Remove(button);
            });

            pointsX.Clear();
            pointsY.Clear();
            numbersX.Clear();
            numbersY.Clear();
            buttons.Clear();
            labels.Clear();

            DrawNonogram((int)numSizeX.Value, (int)numSizeY.Value, (int)cbLevel.SelectedItem);
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                MessageBox.Show("Congratulations, you solved it!", "😄");
            }
            else
            {
                MessageBox.Show("Congratulations, you suck!", "😄");
            }
        }

        private bool IsValid()
        {
            for (int x = 0; x < numbersY.Count; x++)
            {
                if (!numbersY[x].SequenceEqual(GetNumbers(pointsY[x], false)))
                {
                    return false;
                }
            }

            for (int y = 0; y < numbersX.Count; y++)
            {
                if (!numbersX[y].SequenceEqual(GetNumbers(pointsX[y], false)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
