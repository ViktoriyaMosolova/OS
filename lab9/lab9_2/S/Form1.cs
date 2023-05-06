using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SimpleTCP;
using System.Net.NetworkInformation;

namespace S
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GetLocalIPV4();
            FindIP();
            textBox2.Text = 8000.ToString();
        }
        SimpleTcpServer server;
        private IPAddress GetIPv4fList(IPAddress[] addresses)
        {
            IPAddress iPAddress = addresses[0];
            foreach (IPAddress ip in addresses)
            {
                iPAddress = ip;
                comboBox1.Items.Add(ip.ToString());
            }

            return iPAddress;
        }
        private IPAddress GetLocalIPV4()
        {
            return GetIPv4fList(Dns.GetHostByName(Dns.GetHostName()).AddressList);
        }
        private void FindIP()
        {
            // Получаем все сетевые интерфейсы
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            // Ищем беспроводной интерфейс
            foreach (NetworkInterface ni in interfaces)
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && ni.OperationalStatus == OperationalStatus.Up)
                {
                    // Получаем IP-адреса беспроводного интерфейса
                    IPInterfaceProperties properties = ni.GetIPProperties();
                    foreach (UnicastIPAddressInformation ip in properties.UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            //Console.WriteLine("IP-адрес беспроводной сети: " + ip.Address.ToString());
                            comboBox1.Items.Add(ip.Address.ToString());
                            comboBox1.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        private void start_Click(object sender, EventArgs e)
        {
            textBox3.Text = "Server starting...";
            System.Net.IPAddress ip = System.Net.IPAddress.Parse(comboBox1.Text);
            server.Start(ip, Convert.ToInt32(textBox2.Text));
        }

        private void stop_Click(object sender, EventArgs e)
        {
            if (server.IsStarted)
                textBox3.Text = "Server stop...";
                server.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            server = new SimpleTcpServer();
            server.Delimiter = 0x13;//enter
            server.StringEncoder = Encoding.UTF8;
            server.DataReceived += Server_DataReceived;
        }
        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            textBox3.Invoke((MethodInvoker)delegate ()
            {
                textBox3.Text = e.MessageString;
                e.ReplyLine(string.Format("You said: {0}", e.MessageString));
            });
        }
    }
}
