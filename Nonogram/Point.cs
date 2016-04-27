using System.Drawing;
using System.Windows.Forms;

namespace Nonogram
{
    class Point
    {
        public enum CaseState { UNKNOWN, MARKED, BLANK }
        public enum Level { Easy = 15, Normal = 11, Hard = 9 }

        private CaseState state;
        public CaseState State
        {
            get { return state; }
            set
            {
                state = value;
                button.BackColor = GetColor();
            }
        }

        /// <summary>
        /// Gets or sets the expected state that the nonogramm can be declared as solved.
        /// </summary>
        public CaseState Expected { get; }

        public int X { get; }
        public int Y { get; }

        public Point(int x, int y, CaseState expected)
        {
            X = x;
            Y = y;
            Expected = expected;
        }

        private Button button;

        public void ButtonClick(object sender, MouseEventArgs e)
        {
            if (State == CaseState.BLANK)
            {
                State = CaseState.UNKNOWN;
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
                case CaseState.BLANK:
                    return Color.White;
                case CaseState.MARKED:
                    return Color.Black;
                default:
                    return Color.Gray;
            }
        }

        public Button GetButton()
        {
            if (button != null) return button;

            button = new Button
            {
                BackColor = GetColor(),
                Left = Form1.RASTER_START_X + (X * (Form1.BUTTON_WIDTH + Form1.PADDING)),
                Top = Form1.RASTER_START_Y + (Y * (Form1.BUTTON_HEIGHT + Form1.PADDING)),
                Width = Form1.BUTTON_WIDTH,
                Height = Form1.BUTTON_HEIGHT,
                Margin = Padding.Empty,
                FlatStyle = FlatStyle.Flat,
                TabStop = false
            };
            button.FlatAppearance.BorderSize = 0;
            button.MouseClick += ButtonClick;

            return button;
        }
    }
}
