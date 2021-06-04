using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace JpegSmallerCS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(textBoxInputFiles.Text);
                openFileDialog.Filter = "JPEG files (*.jpg)|*.jpg|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    textBoxInputFiles.Text = openFileDialog.FileName;
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            JpegSmallerCS.Properties.Settings.Default.InputFileName = textBoxInputFiles.Text;
            JpegSmallerCS.Properties.Settings.Default.OutputDir = textBoxOutpurDir.Text;
            JpegSmallerCS.Properties.Settings.Default.SmallerTo = textBoxSmallerTo.Text;
            JpegSmallerCS.Properties.Settings.Default.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxInputFiles.Text = JpegSmallerCS.Properties.Settings.Default.InputFileName;
            textBoxOutpurDir.Text = JpegSmallerCS.Properties.Settings.Default.OutputDir;
            textBoxSmallerTo.Text = JpegSmallerCS.Properties.Settings.Default.SmallerTo;
        }
    }
}
