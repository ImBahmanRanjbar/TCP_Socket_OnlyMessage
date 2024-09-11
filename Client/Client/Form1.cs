using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Threading;
using System.Net;
using System.Net.Sockets;
namespace Client
{
    public partial class Form1 : Form
    {
        IPEndPoint oIPEndPoint;
        Socket oServerSocket;
        Thread otr;
        public Form1()
        {
            InitializeComponent();
            
        }
        private void GetMSG()
        {
            while (true)
            {

            byte[] buffer = new byte[1024];
            int r = oServerSocket.Receive(buffer);
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
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int port = int.Parse(tport.Text);
                IPAddress oIpAddress = IPAddress.Parse(tip.Text);
                oIPEndPoint = new IPEndPoint(oIpAddress, port);
                oServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                oServerSocket.Connect(oIPEndPoint);
                otr = new Thread(() => GetMSG());
                otr.IsBackground = true;
                otr.Start();
            }
            catch (Exception eee)
            {

                MessageBox.Show(eee.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            oServerSocket.Send(GetByte(tmes.Text));
        }
        private byte[] GetByte(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            otr.Abort();
            oServerSocket.Shutdown(SocketShutdown.Both);
        }
    }
}
