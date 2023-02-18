using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;


namespace lab2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                textBox1.Text = Convert.ToString("Drive " + d.Name);
                textBox1.Text += Convert.ToString("  Drive type: " + d.DriveType);
                if (d.IsReady == true)
                {
                    textBox1.Text = Convert.ToString("  Volume label: " + d.VolumeLabel);
                    textBox1.Text += Convert.ToString("  File system: " + d.DriveFormat);
                    textBox1.Text += Convert.ToString("  Available space to current user:" + d.AvailableFreeSpace + " bytes");
                    textBox1.Text += Convert.ToString("  Total available space:          " + d.TotalFreeSpace + " bytes");
                    textBox1.Text += Convert.ToString("  Total size of drive:            " + d.TotalSize + " bytes");
                }
            }

        }
    }
}
