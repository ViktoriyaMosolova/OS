using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ClassLibrary1
{
    public class InfoAboutDrive
    {
        public static List<string> OutputInfoAboutDrive()
        {
            List<string> list = new List<string>();
            string result = "";
            foreach (var d in DriveInfo.GetDrives())
            {
                result += $"Drive {d.Name}" + Environment.NewLine;
                result += $"Drive type: {d.DriveType}" + Environment.NewLine;
                if (d.IsReady == true)
                {
                    result += $"Volume label: {d.VolumeLabel}" + Environment.NewLine;
                    result += $"File system: {d.DriveFormat}" + Environment.NewLine;
                    result += $"{"Available space to current user:",-40}{d.AvailableFreeSpace, -15} bytes" + Environment.NewLine;
                    result += $"{"Total available space:",-40}{d.TotalFreeSpace,-15} bytes" + Environment.NewLine;
                    result += $"{"Total size of drive:",-40}{d.TotalSize,-15} bytes" + Environment.NewLine + Environment.NewLine;
                }
            }
            list.Add(result);
            return list;
        }
    }

    public class LogicalDrives
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GetLogicalDrives();
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GetDriveType(string s);
        public static List<string> OutputGetLogicalDrives()
        {
            string result = "";
            List<string> list = new List<string>();
            int dr = GetLogicalDrives();
            for (int i = 0; i < 26; i++)
            {
                int d = (dr >> i) & 1;
                if (d == 1)
                {
                    string s = (char)(65 + i) + ":\\";
                    int t = GetDriveType(s);
                    string tp = "";
                    if (t == 2) tp = " сменный диск";
                    if (t == 3) tp = " жесткий диск";
                    if (t == 4) tp = " сетевой диск";
                    if (t == 5) tp = " CD-ROM";
                    if (t == 6) tp = " RAM-диск";
                    result += s + tp + "#" + Environment.NewLine;
                }
            }
            list.Add(result);
            return list;
        }
    }
}
