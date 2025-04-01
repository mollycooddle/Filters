using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Bitmap image;
        public Form1()
        {
            InitializeComponent();
       
        }

        private void sepiyaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sepiya filter = new Sepiya();
            Bitmap resultImage = filter.Execute(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files|*.png;*.jpg;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(openFileDialog.FileName);

                PictureBox pictureBox1 = new PictureBox();
            }

            pictureBox1.Image = image;
            pictureBox1.Refresh();
        }

        private void inversionToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            InvertFilter filter = new InvertFilter();
            Bitmap resultImage = filter.Execute(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GrayScaleFilter filter = new GrayScaleFilter();
            Bitmap resultImage = filter.Execute(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void brightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Brightness filter = new Brightness();
            Bitmap resultImage = filter.Execute(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void shiftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Shift filter = new Shift();
            Bitmap resultImage = filter.Execute(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void embossingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Embossing filter = new Embossing();
            Bitmap resultImage = filter.Execute(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void blurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Blur filter = new Blur();
            Bitmap resultImage = filter.Execute(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void greyWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GreyWorld filter = new GreyWorld();
            Bitmap resultImage = filter.Execute(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void autolevelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Autolevels filter = new Autolevels();
            Bitmap resultImage = filter.Execute(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void perfectReflectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PerfectReflector filter = new PerfectReflector();
            Bitmap resultImage = filter.Execute(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void expansionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Expansion filter = new Expansion();
            Bitmap resultImage = filter.Execute(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void narrowingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Narrowing filter = new Narrowing();
            Bitmap resultImage = filter.Execute(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void medianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Median filter = new Median();
            Bitmap resultImage = filter.Execute(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void sobelFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SobelFilter filter = new SobelFilter();
            Bitmap resultImage = filter.Execute(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void scharraFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScharraFilter filter = new ScharraFilter();
            Bitmap resultImage = filter.Execute(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }
    }
}