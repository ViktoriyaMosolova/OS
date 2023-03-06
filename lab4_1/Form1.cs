using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ///PrintMain();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            for (int i = 1; i < 6; i++)
            {
                Thread thread1 = new(Print);
                Thread myThread = thread;
                myThread.Name = $"Поток {i}";
                myThread.Start();
            }
        }
            void Print()
            {
            int x = 0;
            object locker = new();  // объект-заглушка
                                    // запускаем пять потоков
            lock (locker)
                {
                    x = 1;
                    for (int i = 1; i < 6; i++)
                    {
                        Console.WriteLine($"{Thread.CurrentThread.Name}: {x}");
                        x++;
                        Thread.Sleep(100);
                    }
                }
            }
    }
}
