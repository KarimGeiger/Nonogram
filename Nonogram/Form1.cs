using System;
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

        private Nonogram nonogram;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbLevel.DataSource = Enum.GetValues(typeof(Point.Level));
            cbLevel.SelectedIndex = 1;

            CreateAndDrawNonogram(10, 10, (int)Point.Level.Normal);
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
            nonogram.Labels.ForEach(x => Controls.Remove(x));
            nonogram.PointsX.ForEach(x => x.ForEach(y => Controls.Remove(y.GetButton())));

            CreateAndDrawNonogram((int)numSizeX.Value, (int)numSizeY.Value, (int)cbLevel.SelectedItem);
        }

        private void CreateAndDrawNonogram(int sizeX, int sizeY, int probability)
        {
            nonogram = new Nonogram(sizeX, sizeY, probability);

            Controls.AddRange(nonogram.Labels.ToArray());
            nonogram.PointsX.ForEach(x => x.ForEach(y => Controls.Add(y.GetButton())));
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Congratulations, " + (nonogram.IsValid() ? "You solved it!" : "You suck!"), "😄");
        }
    }
}
