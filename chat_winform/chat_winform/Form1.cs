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

namespace chat_winform
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

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = "127.0.0.1";
            this.textBox2.Text = "9999";
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
                TcpClient tc = new TcpClient(this.textBox1.Text, Int32.Parse(this.textBox2.Text));
                NetworkStream ns = tc.GetStream();
                StreamReader sr = new StreamReader(ns);
                StreamWriter sw = new StreamWriter(ns);

                sw.WriteLine(">>>To Server : 접속");
                sw.Flush();
                this.richTextBox1.Text += ">>>연결";
                this.richTextBox1.Text += sr.ReadLine();
                while (true)
                {
                    string line;
                    // 서버로부터 text 수신
                    while ((line = sr.ReadLine()) != null)
                    {
                        this.richTextBox1.Text += line;
                    }
                    if (this.exit == "exit")
                    {
                        sw.WriteLine(">>>To Server : 종료");
                        sw.Flush();
                        break;
                    }
                    //서버로 데이터 송신
                    if (this.send == "send")
                    {
                        sw.WriteLine("from server" + this.text);
                        sw.Flush();

                    }
                }
            }
            catch( Exception error)
            {
                Console.WriteLine(error);
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.exit = "exit";
        }
    }
}
