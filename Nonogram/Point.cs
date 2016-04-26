using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Nonogram
{
    class Point
    {
        public static Random random = new Random();
        public enum State { UNKNOWN = 0, MARKED = 1, BLANK = 2 };
        public enum Level { Easy = 15, Normal = 11, Hard = 9};

        public State state = State.UNKNOWN;
        public State expected; 
        public int x;
        public int y;
        private Button button;

        public Point(int probability)
        {
            if (random.Next(0, probability) >= 5)
            {
                expected = State.MARKED;
            }
            else
            {
                expected = State.BLANK;
            }
            //state = expected; // Debug: Enable this line to solve the Nonogram on build
        }

        public void ButtonClick(object sender, MouseEventArgs e)
        {
            if (state == State.BLANK)
            {
                state = State.UNKNOWN;
            }
            else
            {
                state++;
            }

            button.BackColor = Color();
        }

        private Color Color()
        {
            switch (state)
            {
                case State.BLANK:
                    return System.Drawing.Color.White;
                case State.MARKED:
                    return System.Drawing.Color.Black;
                default:
                    return System.Drawing.Color.Gray;
            }
        }

        public Button Button()
        {
            if (button == null)
            {
                button = new Button
                {
                    BackColor = Color(),
                    Left = Form1.RASTER_START_X + (x * (Form1.BUTTON_WIDTH + Form1.PADDING)),
                    Top = Form1.RASTER_START_Y + (y * (Form1.BUTTON_HEIGHT + Form1.PADDING)),
                    Width = Form1.BUTTON_WIDTH,
                    Margin = Padding.Empty,
                    FlatStyle = FlatStyle.Flat,
                    TabStop = false
                };
                button.FlatAppearance.BorderSize = 0;
                button.MouseClick += ButtonClick;
            }

            return button;
        }
    }
}
