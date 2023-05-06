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
using SimpleTCP;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.NetworkInformation;

namespace С
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GetLocalIPV4();
            FindIP();
        }
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

        SimpleTcpClient client;
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
                            //comboBox1.Items.Add(ip.Address.ToString());
                            //comboBox1.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.Connect(comboBox1.Text, Convert.ToInt32(textBox2.Text));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
            client.DataReceived += Client_DataReceived;
        }

        private void Send_Click(object sender, EventArgs e)
        {
            client.WriteLineAndGetReply(textBox3.Text, TimeSpan.FromSeconds(3));
        }

        private void Client_DataReceived(object sender, SimpleTCP.Message e)
        {
            textBox4.Invoke((MethodInvoker)delegate ()
            {
                textBox4.Text += e.MessageString;
            });
        }
    }
}
