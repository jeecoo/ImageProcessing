using ImageProcess2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;


namespace ImageProcessing
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed, subtracted;
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice device;
        int mode;
        int value;
        public Form1()
        {
            mode = 0;
            value = 0;
            InitializeComponent();
        }

        private void dIPToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pixelCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            for(int x= 0; x < loaded.Width; x++)
            {
                for(int y=0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, y, pixel);
                }
            }
            pictureBox2.Image = processed;
        }


        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            loaded = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loaded;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void mirrorHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            BasicDIP.MirrorHorizontal(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        private void mirrorVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            BasicDIP.MirrorVertical(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterinfo in filterInfoCollection)
            {
                comboBox1.Items.Add(filterinfo.Name);
            }
            comboBox1.SelectedIndex = 0;
            device = new VideoCaptureDevice();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void greyscalingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            BasicDIP.Greyscale(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        private void inversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            BasicDIP.Invert(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        private void histToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicDIP.Hist(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if(device.IsRunning == true)
            {
                if(mode == 3)
                {
                    value = trackBar1.Value;
                }
            }
            else
            {
                BasicDIP.Brightness(ref loaded, ref processed, trackBar1.Value);
                pictureBox2.Image = processed;
            }
        }

        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if(device.IsRunning == true)
            {
                if(mode == 2)
                {
                    value = trackBar2.Value;
                }
            }
            else
            {
                BasicDIP.Rotate(ref loaded, ref processed, trackBar2.Value);
                pictureBox2.Image = processed;
            }
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            BasicDIP.Sepiafy(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            if(device.IsRunning == true)
            {
                if(mode == 8)
                {
                    value = trackBar3.Value;
                }
            }
            else
            {
                if (loaded == null)
                    return;

                Bitmap copy = (Bitmap)loaded.Clone();
                BitmapFilter.Contrast(copy, (SByte)trackBar3.Value);

                pictureBox2.Image = copy;
            }
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            BasicDIP.Scale(ref loaded, ref processed, trackBar4.Value);
            pictureBox2.Image = processed;
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            BasicDIP.BinaryStuff(ref loaded, ref processed, trackBar5.Value);
            pictureBox2.Image = processed;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox2.Image = new Bitmap(openFileDialog2.FileName);
                    processed = new Bitmap(openFileDialog2.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message);
                }
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            device = new VideoCaptureDevice(filterInfoCollection[comboBox1.SelectedIndex].MonikerString);
            device.NewFrame += VideoCaptureDevice_NewFrame;
            device.Start();
        }

        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();
            Bitmap filteredFrame = (Bitmap)frame.Clone();
            switch (mode)
            {
                case 0:

                    break;
                case 1:
                    BasicDIP.Greyscale(ref frame, ref filteredFrame);
                    break;
                case 2:
                    BasicDIP.Rotate(ref frame, ref filteredFrame, value);
                    break;
                case 3:
                    BasicDIP.Brightness(ref frame, ref filteredFrame, value);
                    break;
                case 4:
                    BasicDIP.Invert(ref frame, ref filteredFrame);
                    break;
                case 5:
                    BasicDIP.Sepiafy(ref frame, ref filteredFrame);
                    break;
                case 6:
                    BasicDIP.MirrorHorizontal(ref frame, ref filteredFrame);
                    break;
                case 7:
                    BasicDIP.MirrorVertical(ref frame, ref filteredFrame);
                    break;
                case 8:
                    Bitmap temp = new Bitmap(frame);
                    BasicDIP.Contrast(ref temp, (SByte)value);
                    filteredFrame = temp;
                    break;
            }
            if(mode != 0)
            {
                pictureBox2.Image = filteredFrame;
            }
            pictureBox1.Image = frame;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(device.IsRunning == true)
            {
                device.Stop();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (device.IsRunning == true)
            {
                device.Stop();
            }
        }

        private void grayScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = 1;
        }

        private void rotationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = 2;
        }

        private void brightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = 3;
        }

        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = 4;
        }

        private void sepiaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            mode = 5;
        }

        private void mirrorHorizontalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            mode = 6;
        }

        private void mirrorVerticalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            mode = 7;
        }

        private void contrastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = 8;
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (loaded == null || processed == null)
            {
                MessageBox.Show("Both loaded and processed images are required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (processed.Width != loaded.Width || processed.Height != loaded.Height)
            {
                Bitmap resizedProcessed = new Bitmap(loaded.Width, loaded.Height);
                using (Graphics g = Graphics.FromImage(resizedProcessed))
                {
                    g.DrawImage(processed, 0, 0, loaded.Width, loaded.Height);
                }
                processed = resizedProcessed;
            }

            // Threshold to determine if a pixel is "green enough"
            int greenThreshold = 50;
            subtracted = new Bitmap(loaded.Width, loaded.Height);

            for (int i = 0; i < loaded.Width && i < processed.Width; i++)
            {
                for (int j = 0; j < loaded.Height && j < processed.Height; j++)
                {
                    Color pixel = loaded.GetPixel(i, j);
                    if (pixel.G > pixel.R + greenThreshold && pixel.G > pixel.B + greenThreshold)
                    {
                        subtracted.SetPixel(i, j, processed.GetPixel(i, j));
                    }
                    else
                    {
                        subtracted.SetPixel(i, j, pixel);
                    }
                }
            }
            pictureBox3.Image = subtracted;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
    }
}
