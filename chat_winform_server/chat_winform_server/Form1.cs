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
using System.Net;
using System.Net.Sockets;

namespace chat_winform_server
{
    public partial class Form1 : Form
    {

        string exit;
        string send;
        string text;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.text = this.richTextBox2.Text;
            this.send = "send";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                TcpListener tl = new TcpListener(IPAddress.Parse(this.textBox1.Text), Int32.Parse(this.textBox2.Text));
                Console.WriteLine(Int32.Parse(this.textBox2.Text));
                tl.Start();
                this.richTextBox1.Text += ">>>서버 오픈";
                TcpClient tc = tl.AcceptTcpClient();
                NetworkStream ns = tc.GetStream();
                StreamReader sr = new StreamReader(ns);
                StreamWriter sw = new StreamWriter(ns);

                sw.WriteLine(">>>To Client : 연결");
                sw.Flush();
                
                this.richTextBox1.Text += sr.ReadLine();

                while (true)
                {
                    string line;
                    // 클라로부터 text 수신
                    while ((line = sr.ReadLine()) != null)
                    {
                        this.richTextBox1.Text += line;
                    }
                    if (this.exit == "exit")
                    {
                        sw.WriteLine(">>>To Client : 종료");
                        sw.Flush();
                        break;
                    }
                    //클라이언트로 데이터 송신
                    if (this.send == "send")
                    {
                        sw.WriteLine("from server : " + this.text);
                        sw.Flush();

                    }
                }
            }
            catch(Exception error)
            {
                Console.WriteLine(error);
                Console.WriteLine("???");
            }
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.exit = "exit";
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            this.textBox1.Text = "127.0.0.1";
            this.textBox2.Text = "9999";
        }
    }
}
