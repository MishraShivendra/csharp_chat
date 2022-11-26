using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientNs;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClientNs
{
    public partial class server : Form
    {
        TcpListener tcpServer;
        private string serverData;
        private NetworkStream stream;
        public server()
        {
            Thread thread = new Thread(startServer);
            thread.Start();
            InitializeComponent();
            client c = new client();
            c.Show();
        }

        public void AppendTextBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            recvTextBox.Text += value;
            recvTextBox.AppendText(Environment.NewLine);
        }

        private void startServer()
        {            
            try
            {
                tcpServer = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);
                tcpServer.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {                    
                    TcpClient client = tcpServer.AcceptTcpClient();
                    data = null;                    
                    stream = client.GetStream();
                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        AppendTextBox( data);
                                             
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                tcpServer.Stop();
            }
            
        }

        private void recvTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            stream.Write(Encoding.ASCII.GetBytes(sendTextBox.Text), 0, sendTextBox.Text.Length);
        }
    }
}
