﻿using System.Drawing;
using System.Windows.Forms;

namespace Nonogram
{
    class Point
    {
        public enum State { UNKNOWN = 0, MARKED = 1, BLANK = 2 };
        public enum Level { Easy = 15, Normal = 11, Hard = 9 };

        public State state = State.UNKNOWN;
        public State expected; // Set state to expected to show solved Nonogram
        public int x;
        public int y;
        private Button button;

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
                    Height = Form1.BUTTON_HEIGHT,
                    Margin = Padding.Empty,
                    FlatStyle = FlatStyle.Flat,
                    TabStop = false,
                };
                button.FlatAppearance.BorderSize = 0;
                button.MouseClick += ButtonClick;
            }

            return button;
        }
    }
}
