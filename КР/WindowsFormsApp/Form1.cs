using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string pathdir = @"C:\Users\1\Desktop\OS\КР\WindowsFormsApp\bin\Debug\Памагите\";
        string[] arr = Directory.GetFiles(pathdir);

        private void button1_Click(object sender, EventArgs e)
        {
            MergeFiles(arr, pathdir+@"merge.part");
        }

        public static void MergeFiles(string[] inputFilePaths, string outputFilePath)
        {
            try
            {
                using (var outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
                {
                    foreach (var inputFilePath in inputFilePaths)
                    {
                        using (var inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                        {
                            inputStream.CopyTo(outputStream);
                        }
                    }
                }
                System.Diagnostics.Process.Start("explorer", pathdir);
                MessageBox.Show("Готово!!!");
            }
            catch
            {
                MessageBox.Show("Такой файл уже есть))");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Как это нет...");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(@"C:\Users\1\Desktop\OS\КР\WindowsFormsApp\Resources\Cat.jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
