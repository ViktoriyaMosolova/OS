using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string hostName = Dns.GetHostName();
            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

            foreach (IPAddress address in hostEntry.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork) // выбираем IPv4-адрес
                {
                    Console.WriteLine("Server host: {0}", address);
                    comboBox1.Items.Add(address.ToString());
                }
            }
        }

        private TcpClient client;
        private bool isListening = false;
        private async Task ReceiveMessagesAsync()
        {
            byte[] buffer = new byte[10024];
            int bytesRead;
            string message;

            while (client.Connected)
            {
                try
                {
                    // считываем данные от сервера
                    bytesRead = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                    message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // выводим сообщение в лог
                    LogListBox.Invoke(new Action(() =>
                    {
                        LogListBox.Items.Add(message);
                    }));
                }
                catch (Exception ex)
                {
                    // произошла ошибка при чтении данных
                    // закрываем соединение с сервером
                    MessageBox.Show(ex.Message);
                    client.Close();
                    return;
                }
            }
        }

        private async void ConnectButton_Click_1(object sender, EventArgs e)
        {
            string host = HostTextBox.Text;
            int serverPort = 8000;
            int clientPort = Convert.ToInt32(PortClient.Text);

            // подключаемся к серверу
            client = new TcpClient();
            await client.ConnectAsync(host, serverPort);
            isListening = true;
            // запускаем цикл чтения сообщений от сервера
            await Task.Run(() => ReceiveMessagesAsync());

            // создаем TcpListener для прослушивания входящих сообщений от сервера
            IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, clientPort);
            TcpListener listener = new TcpListener(clientEndPoint);
            listener.Start();


            // запускаем цикл для прослушивания входящих соединений от сервера
            await Task.Run(async () =>
            {
                while (isListening)
                {
                    TcpClient serverClient = await listener.AcceptTcpClientAsync();
                    label1.Text = "Server responded";

                    await Task.Run(async () =>
                    {
                        NetworkStream serverStream = serverClient.GetStream();
                        byte[] serverBuffer = new byte[1024];

                        while (isListening)
                        {
                            int serverBytesRead = await serverStream.ReadAsync(serverBuffer, 0, serverBuffer.Length);
                            string serverMessage = Encoding.UTF8.GetString(serverBuffer, 0, serverBytesRead);
                            label1.Text = "Received message from server: " + serverMessage;

                            // выводим сообщение в лог
                            LogListBox.Invoke(new Action(() =>
                            {
                                LogListBox.Items.Add(serverMessage);
                            }));
                        }
                    });
                }
            });

            // обновляем UI элементы
            label1.Text = "Connected to " + host + ":" + serverPort;
            ConnectButton.Enabled = false;
            DisconnectButton.Enabled = true;
            SendButton.Enabled = true;
        }

        private async void SendButton_Click_1(object sender, EventArgs e)
        {
            // отправляем сообщение на сервер
            string message = MessageTextBox.Text;
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await client.GetStream().WriteAsync(buffer, 0, buffer.Length);

            // очищаем текстовое поле сообщения
            MessageTextBox.Text = "";
        }

        private void DisconnectButton_Click_1(object sender, EventArgs e)
        {
            // закрываем соединение с сервером
            client.Close();
            isListening = false;
            // обновляем UI элементы
            label1.Text = "Disconnected";
            ConnectButton.Enabled = true;
            DisconnectButton.Enabled = false;
            SendButton.Enabled = false;
        }
    }
}
