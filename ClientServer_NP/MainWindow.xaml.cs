using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientServer_NP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string countryName = txtBoxCountryName.Text;

            string result = SendRequestToServer(countryName);

            MessageBox.Show($"Результат: {result}");
        }

        private string SendRequestToServer(string countryName)
        {
            string serverIP = "127.0.0.1";
            int serverPort = 1234;

            using (TcpClient client = new TcpClient(serverIP, serverPort))
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(countryName);
                    stream.Write(buffer, 0, buffer.Length);

                    byte[] responseBuffer = new byte[client.ReceiveBufferSize];
                    int bytesRead = stream.Read(responseBuffer, 0, client.ReceiveBufferSize);
                    string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);


                    return response;
                }
            }
        }
    }
}
