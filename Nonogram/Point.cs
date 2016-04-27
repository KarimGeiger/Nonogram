using System.Drawing;
using System.Windows.Forms;

namespace Nonogram
{
    class Box : Button
    {
        public enum BoxState { UNKNOWN, MARKED, BLANK }
        public enum Level { Easy = 15, Normal = 11, Hard = 9 }

        private BoxState state;
        public BoxState State
        {
            get { return state; }
            set
            {
                state = value;
                BackColor = GetColor();
            }
        }

        /// <summary>
        /// Gets the expected state that the nonogramm can be declared as solved.
        /// </summary>
        public BoxState Expected { get; }

        public int X { get; }
        public int Y { get; }

        public Box(int x, int y, BoxState expected)
        {
            X = x;
            Y = y;
            Expected = expected;

            BackColor = GetColor();
            Left = Nonogram.RASTER_START_X + (X * (Nonogram.BUTTON_WIDTH + Nonogram.PADDING));
            Top = Nonogram.RASTER_START_Y + (Y * (Nonogram.BUTTON_HEIGHT + Nonogram.PADDING));
            Width = Nonogram.BUTTON_WIDTH;
            Height = Nonogram.BUTTON_HEIGHT;
            Margin = Padding.Empty;
            FlatStyle = FlatStyle.Flat;
            TabStop = false;
            FlatAppearance.BorderSize = 0;
            MouseClick += ButtonClick;
        }

        public void ButtonClick(object sender, MouseEventArgs e)
        {
            if (State == BoxState.BLANK)
            {
                State = BoxState.UNKNOWN;
            }
            else
            {
                State++;
            }
        }

        private Color GetColor()
        {
            switch (State)
            {
                case BoxState.BLANK:
                    return Color.White;
                case BoxState.MARKED:
                    return Color.Black;
                default:
                    return Color.Gray;
            }
        }
    }
}
