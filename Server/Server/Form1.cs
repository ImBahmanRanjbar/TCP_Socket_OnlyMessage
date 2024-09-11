using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    public partial class Form1 : Form
    {
        IPEndPoint oIPEndPoint;
        Socket oServerSocket;
        Socket oClientSocket;
        Thread otr;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                oServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                int port = int.Parse(tport.Text);
                IPAddress oIpAddress = IPAddress.Parse(tip.Text);
                oIPEndPoint = new IPEndPoint(oIpAddress, port);
                oServerSocket.Bind(oIPEndPoint);
                oServerSocket.Listen(5);
                oClientSocket = oServerSocket.Accept();
                otr = new Thread(() => GetMSG());
               // otr.IsBackground = true;
                otr.Start();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private byte[] GetByte(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void GetMSG()
        {
            while (true)
            {

            byte[] buffer = new byte[1024];
            int r = oClientSocket.Receive(buffer);
            if (r > 0)
            {
                this.Invoke(new Action(() =>
                {
                    listBox1.Items.Add(GetStr(buffer, r));
                }));
            }
            }

        }
        private string GetStr(byte[] buffer, int r)
        {
            return Encoding.UTF8.GetString(buffer, 0, r);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            oClientSocket.Send(GetByte(tmes.Text));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            otr.Abort();
            oServerSocket.Close();
            oClientSocket.Shutdown(SocketShutdown.Both);
        }
    }
}
