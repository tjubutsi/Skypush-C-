using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Skypush
{
    public partial class Grabber : Form
    {
        private CustomApplicationContext main;

        int selectX; //Location when starting to select
        int selectY;

        int leftX; //Current mostleft bound
        int leftY;

        int selectWidth; //Size of selected area
        int selectHeight;

        bool selectStarted = false;

        public Grabber(CustomApplicationContext Main)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            main = Main; //save main form to show notification bubble
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            pictureBox1.Image = Helpers.GetAllMonitorsScreenshot();
            this.Size = pictureBox1.Image.Size;
            pictureBox1.Size = pictureBox1.Image.Size;
            var screenLocation = Helpers.GetTopLeftLocation();
            this.Left = screenLocation.Horizontal;
            this.Top = screenLocation.Vertical;
            this.Show();
            this.Activate();
            this.Cursor = Cursors.Cross;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                selectX = e.X;
                selectY = e.Y;
                leftX = e.X;
                leftY = e.Y;
                selectStarted = true;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (selectStarted)
            {
                using (var selectPen = new SolidBrush(Color.FromArgb(50, Color.Black)))
                {
                    e.Graphics.FillRectangle(selectPen, leftX, leftY, selectWidth, selectHeight);
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectStarted)
            {
                var oldRectangle = new Rectangle(leftX, leftY, selectWidth, selectHeight);
                if (e.X < selectX)
                {
                    leftX = e.X;
                    selectWidth = selectX - e.X;
                }
                else
                {
                    leftX = selectX;
                    selectWidth = e.X - selectX;
                }

                if (e.Y < selectY)
                {
                    leftY = e.Y;
                    selectHeight = selectY - e.Y;
                }
                else
                {
                    leftY = selectY;
                    selectHeight = e.Y - selectY;
                }
                pictureBox1.Invalidate(oldRectangle);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (selectStarted)
            {
                if (e.Button == MouseButtons.Left)
                {
                    selectStarted = false;
                    if (selectWidth > 0)
                    {
                        Rectangle selection = new Rectangle(leftX, leftY, selectWidth, selectHeight);
                        Bitmap originalImage = new Bitmap(pictureBox1.Image);
                        Bitmap imageSelection = originalImage.Clone(selection, originalImage.PixelFormat);
                        this.Hide();
                        main.SaveToClipboard(imageSelection);
                        this.Close();
                    }
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
