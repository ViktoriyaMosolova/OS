using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        private Thread str1;
        private Thread str2;
        public Form1()
        {
            InitializeComponent();

            str1 = new Thread(Mur);
            str2 = new Thread(Meow);
            label1.Text = "Priority mur: 1";
            label2.Text = "Priority meow: 1";

            label3.Text = "Endless sleep";
            Thread cat = new Thread(CatSleep);
            cat.Start();
        }

        void CatSleep()
        {
            while (true)
            {
                textBox3.Text += "Sleep" + Environment.NewLine;
                Thread.Sleep(1000);
            }
        }

        void Mur()
        {
            ulong eat = 0, sleep = 10;
            while (true)
            {
                while(eat < 10)
                {
                    eat++;
                    sleep--;
                    textBox1.Text = $"eat: {eat}, sleep: {sleep}";
                    Thread.Sleep(500);
                }
                while (sleep < 10)
                {
                    sleep++;
                    eat--;
                    textBox1.Text = $"eat: {eat}, sleep: {sleep}";
                    Thread.Sleep(500);
                }
            }
        }

        void Meow()
        {
            ulong x = 3;
            while (true)
            {
                x *= x - 1;
                textBox2.Text = x.ToString();
                Thread.Sleep(500);
            }
        }
        

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            switch (trackBar2.Value)
            {
                case 0:
                    str2.Priority = ThreadPriority.Lowest;
                    label2.Text = "Priority meow: 1";
                    break;
                case 1:
                    str2.Priority = ThreadPriority.BelowNormal;
                    label2.Text = "Priority meow: 2";
                    break;
                case 2:
                    str2.Priority = ThreadPriority.Normal;
                    label2.Text = "Priority meow: 3";
                    break;
                case 3:
                    str2.Priority = ThreadPriority.AboveNormal;
                    label2.Text = "Priority meow: 4";
                    break;
                case 4:
                    str2.Priority = ThreadPriority.Highest;
                    label2.Text = "Priority meow: 5";
                    break;
                default:
                    break;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            switch (trackBar1.Value)
            {
                case 0:
                    str1.Priority = ThreadPriority.Lowest;
                    label1.Text = "Priority mur: 1";
                    break;
                case 1:
                    str1.Priority = ThreadPriority.BelowNormal;
                    label1.Text = "Priority mur: 2";
                    break;
                case 2:
                    str1.Priority = ThreadPriority.Normal;
                    label1.Text = "Priority mur: 3";
                    break;
                case 3:
                    str1.Priority = ThreadPriority.AboveNormal;
                    label1.Text = "Priority mur: 4";
                    break;
                case 4:
                    str1.Priority = ThreadPriority.Highest;
                    label1.Text = "Priority mur: 5";
                    break;
                default:
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (str1.IsAlive)
            {
                str1.Resume();
            }
            else
            {
                str1.Start();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (str1.IsAlive)
            {
                str1.Suspend();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (str2.IsAlive)
            {
                str2.Resume();
            }
            else
            {
                str2.Start();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (str2.IsAlive)
            {
                str2.Suspend();
            }
        }
    }
}
