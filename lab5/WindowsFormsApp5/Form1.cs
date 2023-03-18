using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Xml.Linq;
using ClassLibrary1;
using ClassLibrary2;
using Timer = System.Threading.Timer;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {

        public static Timer tmr1, tmr3;
        public readonly Thread InfoDrive;
        public readonly Thread Exp;
        public readonly Thread str3;
        public Form1()
        {
            InitializeComponent(); 
            textBox1.Visible = false; 
            button2.Visible = false;
            treeView1.Visible = false;
            InfoDrive = new Thread(Cat);
            Exp = new Thread(Alpaca);
            str3 = new Thread(Capibara);
        }

        public void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button2.Visible = true;
            treeView1.Visible = false;
            try
            {
                tmr1.Dispose();
                tmr3.Dispose();
            }
            catch
            {

            }
            if (listBox1.SelectedIndex == 0)
            {
                textBox1.Visible = true;
                tmr1 = new Timer(Cat, "tick1...", 0, 100);

            }
            if (listBox1.SelectedIndex == 1)
            {
                treeView1.Visible = true;
                treeView1.Nodes.Clear();
                Alpaca();
            }
            if (listBox1.SelectedIndex == 2)
            {
                textBox1.Visible = true;
                tmr3 = new Timer(Capibara, "tick3...", 0, 100);
            }
        }

        void Cat(object data)
        {
            textBox1.Text = "";
            Action showMethod = delegate () { foreach (string item in InfoAboutDrive.OutputInfoAboutDrive()) { textBox1.Text += item; }};
            textBox1.BeginInvoke(showMethod);
        }

        void Alpaca()
        {
            treeView1.BeginInvoke((Action)(() => FileExplorer.OpenFileExplorer(treeView1)));
        }
        void Capibara(object data)
        {
            textBox1.Text = "";
            Action showMethod = delegate () { foreach (string item in LogicalDrives.OutputGetLogicalDrives()) { textBox1.Text += item; } };
            textBox1.BeginInvoke(showMethod);
        }

        public void button2_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button2.Visible = false;
            treeView1.Visible = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            InfoDrive.Abort();
            Exp.Abort();
            str3.Abort();
        }
    }
}
