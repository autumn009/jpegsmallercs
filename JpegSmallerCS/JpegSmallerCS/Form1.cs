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
using System.Threading;
using System.Xml.Schema;

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
            textBoxOutputFileNameHeader.Text = DateTime.Now.ToString("MMdd_");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dirDialog = new FolderBrowserDialog();
            dirDialog.SelectedPath = textBoxOutpurDir.Text;
            DialogResult result = dirDialog.ShowDialog();
            // OK button was pressed.
            if (result == DialogResult.OK)
            {
                textBoxOutpurDir.Text = dirDialog.SelectedPath;
            }
        }

        private void resizeImageWhileMaintainingAspectRatio(System.Drawing.Imaging.ImageFormat imageFormat)
        {
            int smallerTo;
            int.TryParse(textBoxSmallerTo.Text, out smallerTo);
            if (smallerTo == 0)
            {
                MessageBox.Show(this, "Bad Size");
                return;
            }
            int total = 0;
            // clear log
            foreach (var sourceFile in Directory.EnumerateFiles(Path.GetDirectoryName(textBoxInputFiles.Text), Path.GetFileName(textBoxInputFiles.Text)))
            {
                int count = 1;
                string destinationFile;
                for (; ; )
                {
                    destinationFile = Path.Combine(textBoxOutpurDir.Text, textBoxOutputFileNameHeader.Text + count + ".jpg");
                    if (!File.Exists(destinationFile)) break;
                    count++;
                }
                // add log
                // サイズ変更する画像ファイルを開く
                using (Image image = Image.FromFile(sourceFile))
                {
                    // 変更倍率を取得する
                    float scale = (float)smallerTo / (float)image.Width;
                    // 変更サイズを取得する
                    int widthToScale = (int)(image.Width * scale);
                    int heightToScale = (int)(image.Height * scale);
                    // サイズ変更した画像を作成する
                    using (Bitmap bitmap = new Bitmap(widthToScale, heightToScale))
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        // 背景色を塗る
                        SolidBrush solidBrush = new SolidBrush(Color.Black);
                        graphics.FillRectangle(solidBrush, new RectangleF(0, 0, widthToScale, heightToScale));

                        // サイズ変更した画像に、左上を起点に変更する画像を描画する
                        graphics.DrawImage(image, 0, 0, widthToScale, heightToScale);

                        // サイズ変更した画像を保存する
                        bitmap.Save(destinationFile, imageFormat);
                    }
                }
                total++;
            }
            MessageBox.Show(this, $"Converted {total} files.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            resizeImageWhileMaintainingAspectRatio(System.Drawing.Imaging.ImageFormat.Jpeg);
            Cursor = Cursors.Default;
        }
    }
}
