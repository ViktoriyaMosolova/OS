using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;

namespace RegistryEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Добавить поле в реестр?", "Редактор реестра",MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (res == DialogResult.OK)
            {
                Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\TEST", textBox1.Text, textBox2.Text);
                MessageBox.Show("Поле добавлено");
            }
            else
            {
                MessageBox.Show("Добавление поля отменено");
            }
            textBox1.Text = String.Empty;
            textBox2.Text = String.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string res = (string)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\TEST", textBox1.Text, "");
            string text = textBox1.Text;
            if (res != null && res == textBox2.Text)
            {
                if (res == "") res = "'Пустое'";
                if (text == "") text = "'Пустое'";
                MessageBox.Show("Поле " + text + " есть в реестре"+Environment.NewLine+"Значение: " + res);
            }
            else
            {
                MessageBox.Show("Поля нет в реестре(");
            }
            textBox1.Text = String.Empty;
            textBox2.Text = String.Empty;
        }
    }
}

