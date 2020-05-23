using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;

namespace Client1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // creat client Socket  to send
            TcpClient Sending = new TcpClient("localhost", 7110);
            byte[] data = Encoding.ASCII.GetBytes(textBox1.Text);
            NetworkStream datastream = Sending.GetStream();
            datastream.Write(data, 0, data.Length);

            data = new byte[256];
            datastream.Read(data, 0, data.Length);
            string ResponseData;
            ResponseData = Encoding.ASCII.GetString(data, 0, data.Length);

            textBox2.Text = textBox2.Text + Environment.NewLine + ResponseData;
            textBox1.Text = "";
            Sending.Close();
        }
    }
}
