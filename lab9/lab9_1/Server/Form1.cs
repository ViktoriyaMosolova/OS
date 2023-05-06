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

namespace Server
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

            private TcpListener listener;
            private Task listeningTask;
            private bool isListening = false;

            private void StartButton_Click(object sender, EventArgs e)
            {
                int port = 8000;

                // создаем TcpListener на заданном порту
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                isListening = true;

                // запускаем асинхронную задачу для ожидания подключений
                listeningTask = ListenAsync();

                StatusLabel.Text = "Listening on port " + port;
                StartButton.Enabled = false;
                StopButton.Enabled = true;
            }

            private void StopButton_Click(object sender, EventArgs e)
            {
                // останавливаем прослушивание порта
                if(listener!=null) listener.Stop();
                isListening = false;

                // ожидаем завершения асинхронной задачи
                listeningTask.Wait();

                // обновляем UI элементы
                StatusLabel.Text = "Stopped";
                StartButton.Enabled = true;
                StopButton.Enabled = false;
            }

            private async Task ListenAsync()
            {
                while (isListening)
                {
                    // ожидаем подключения клиента
                    TcpClient client = await listener.AcceptTcpClientAsync();

                    // обрабатываем подключение в отдельном потоке
                    await Task.Run(() => HandleClientAsync(client));
                }
            }

            private async Task HandleClientAsync(TcpClient client)
            {
                // получаем поток для чтения данных от клиента
                NetworkStream stream = client.GetStream();

                byte[] buffer = new byte[10024];
                int bytesRead;
                string message;

                while (isListening)
                {
                    try
                    {
                        // считываем данные от клиента
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        // выводим сообщение в лог
                        LogListBox.Invoke(new Action(() =>
                        {
                            LogListBox.Items.Add(message);
                        }));

                        // отправляем сообщение обратно клиенту
                        byte[] responseBuffer = Encoding.UTF8.GetBytes("Server: " + message);
                        await stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
                    }
                    catch (Exception ex)
                    {
                    // произошла ошибка при чтении данных
                    // закрываем соединение с клиентом
                        MessageBox.Show(ex.Message);
                        client.Close();
                        return;
                    }
                }
                // закрываем соединение с клиентом
                client.Close();
            }
        }
}
