using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        Process[] Gprocesseslist;
        List<IntPtr> ListName = new List<IntPtr>();
        delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        [Flags]
        private enum ProcessAccessFlags : uint
        {
            QueryLimitedInformation = 0x00001000
        }

        [DllImport("user32.dll")]
        static extern bool SetWindowTextA(IntPtr hWnd, string text);
        [DllImport("user32.dll")]
        static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childafter, string lclass, string WindowTitle);
        [DllImport("user32.dll")]
        static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool QueryFullProcessImageName(
        [In] IntPtr hProcess,
        [In] int dwFlags,
        [Out] StringBuilder lpExeName,
        ref int lpdwSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

        static List<IntPtr> EnumerateProcessWindowHandles(Process process)
        {
            var handles = new List<IntPtr>();
            foreach (ProcessThread thread in process.Threads)
                EnumThreadWindows(thread.Id,(hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);
            return handles;
        }
        public Form1()
        {
            InitializeComponent();
            UpdatePage();
        }

        private void UpdatePage()
        {
            listBox1.Items.Clear();
            Gprocesseslist = Process.GetProcesses();
            ListName.Clear();
            foreach (Process process in Gprocesseslist)
            {
                if (process.ProcessName != "Idle")
                {
                    ListName.AddRange(EnumerateProcessWindowHandles(process));
                }
            }
            for (int i = ListName.Count - 1; i >= 0; i--)
            {
                if (GetWindowTextLength(ListName[i]) == 0)
                {
                    ListName.RemoveAt(i);
                }
            }
            foreach (IntPtr handle in ListName)
            {
                StringBuilder window_text = new StringBuilder(GetWindowTextLength(handle) + 1);
                GetWindowText(handle, window_text, window_text.Capacity);
                listBox1.Items.Add(window_text.ToString());
            }
        }

        private void rename_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                SetWindowTextA(ListName[listBox1.SelectedIndex], textBox1.Text);
                UpdatePage();
                textBox1.Text = "";
            }
            else
            {
                MessageBox.Show("FFFFFFFFFFF");
            }
        }
        
        private void update_Click_1(object sender, EventArgs e)
        {
            UpdatePage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*if (listBox1.SelectedIndex != -1)
            {
                Gprocesseslist[listBox1.SelectedIndex].Kill();
                UpdatePage();
            }
            else
            {
                MessageBox.Show("У тебя лапки...");
            }*/
            MessageBox.Show("У меня лапки...");
        }
    }
}
