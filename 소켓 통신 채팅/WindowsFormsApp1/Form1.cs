using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;   //아래 3개 역시 마찬가지.
using System.Net.Sockets;
using System.Threading;







namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        Socket client_socket;
        bool isConnected;
        byte[] bytes = new byte[1024];
        string data;

        public Form1()
        {
            InitializeComponent();
            isConnected = false;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isConnected == false)
                return;
            byte[] msg = Encoding.UTF8.GetBytes(textBox1.Text + "<eof>");
            int bytesSent = client_socket.Send(msg);
            listBox1.Items.Add(String.Format("나 : {0}",textBox1.Text));
            textBox1.Clear();
            textBox1.Text = "";

 
        }

    private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
  

            if (isConnected == true)
                return;
            client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client_socket.Connect(new IPEndPoint(IPAddress.Parse(textBox2.Text), 80));
            listBox1.Items.Add(String.Format("소켓 연결이 되었습니다 {0}", client_socket.RemoteEndPoint.ToString()));
            isConnected = true;
            //아래 두개는 do_receive 함수를 위한 쓰레드입니다.
            //쓰레드가 있어야만 연결된다고 해야할까요.
            Thread listen_thread = new Thread(do_receive);
            listen_thread.Start();

        }

    private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        void do_receive()
        {
            while (isConnected)
            {
                while (true)
                {
                    byte[] bytes = new byte[1024];
                    int bytesRec = client_socket.Receive(bytes);
                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("<eof>") > -1)
                        break;
                }
                data = data.Substring(0, data.Length - 5);
                Invoke((MethodInvoker)delegate
                {
                    listBox1.Items.Add(data);
                }
                );
                data = "";
            }
        }
    }


    }

