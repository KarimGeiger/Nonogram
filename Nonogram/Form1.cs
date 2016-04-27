using System;
using System.Windows.Forms;

namespace Nonogram
{
    public partial class Form1 : Form
    {
        private Nonogram nonogram;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbLevel.DataSource = Enum.GetValues(typeof(Box.Level));
            cbLevel.SelectedIndex = 1;

            CreateAndDrawNonogram(10, 10, (int)Box.Level.Normal);
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
            nonogram.PointsX.ForEach(x => x.ForEach(y => Controls.Remove(y)));

            CreateAndDrawNonogram((int)numSizeX.Value, (int)numSizeY.Value, (int)cbLevel.SelectedItem);
        }

        private void CreateAndDrawNonogram(int sizeX, int sizeY, int probability)
        {
            nonogram = new Nonogram(sizeX, sizeY, probability);

            Controls.AddRange(nonogram.Labels.ToArray());
            nonogram.PointsX.ForEach(x => x.ForEach(y => Controls.Add(y)));
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Congratulations, " + (nonogram.IsValid() ? "you solved it!" : "you suck!"), "😄");
        }
    }
}
