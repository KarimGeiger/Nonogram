using System.Drawing;
using System.Windows.Forms;

namespace Nonogram
{
    public class Box : Button
    {
        public const int BUTTON_WIDTH = 33;
        public const int BUTTON_HEIGHT = 33;
        public const int PADDING = 1;

        public enum BoxState { UNKNOWN, MARKED, BLANK }
        public enum Level { Easy = 15, Normal = 11, Hard = 9 }

        private BoxState state;
        public BoxState State
        {
            get { return state; }
            set
            {
                state = value;
                BackColor = value.ToColor();
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

            BackColor = State.ToColor();
            Left = Nonogram.RASTER_START_X + (X * (BUTTON_WIDTH + PADDING));
            Top = Nonogram.RASTER_START_Y + (Y * (BUTTON_HEIGHT + PADDING));
            Width = BUTTON_WIDTH;
            Height = BUTTON_HEIGHT;
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
    }

    public static class BoxStateExtension
    {
        public static Color ToColor(this Box.BoxState state)
        {
            switch (state)
            {
                case Box.BoxState.BLANK:
                    return Color.White;
                case Box.BoxState.MARKED:
                    return Color.Black;
                default:
                    return Color.Gray;
            }
        }
    }
}
