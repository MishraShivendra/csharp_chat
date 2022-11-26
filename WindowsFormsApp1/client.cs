using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace ClientNs
{
    public partial class client : Form
    {
        private string clientData;
        private NetworkStream stream;
        public client()
        {
            
            Task.Factory.StartNew(() => RunClient(recvTextBox));
            InitializeComponent();
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void RunClient(System.Windows.Forms.TextBox recvTextBoxf)
        {
            try
            {               
                TcpClient client = new TcpClient("127.0.0.1", 5000);
                stream = client.GetStream();
                String responseData = String.Empty;

                while(true)
                {
                    Byte[] data = new Byte[256];
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    AppendTextBox( responseData+"\n" );
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            stream.Write(Encoding.ASCII.GetBytes(sendTextBox.Text), 0, sendTextBox.Text.Length);
        }
    }
}
