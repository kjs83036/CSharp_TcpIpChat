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
using System.IO;


// 대부분 서버와 동일, 모듈화 시켜서 중복 줄일 필요 있음
namespace client
{
    
    public partial class Form1 : Form
    {
        TcpClient tc;

        public Form1()
        {
            InitializeComponent();

            new Thread(InitSocket).Start();
        }

        private void InitSocket()
        {
            try
            {
                tc = new TcpClient("127.0.0.1", 9999);
                DisplayText(">>> server connected");

                Thread t = new Thread(new ParameterizedThreadStart(OnAccepted));
                t.IsBackground = true;
                t.Start(tc);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                NetworkStream ns = tc.GetStream();
                byte[] buffer = new byte[1024];
                buffer = Encoding.Unicode.GetBytes(richTextBox2.Text + "$");
                ns.Write(buffer, 0, buffer.Length);
                ns.Flush();

                richTextBox2.Text = "";
                richTextBox2.Focus();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }

        private void Forn_Closing(object sender, FormClosingEventArgs e)
        {
            if (tc != null)
            {
                tc.Close();
            }
        }
        private void DisplayText(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.BeginInvoke(new MethodInvoker(delegate
                {
                    richTextBox1.AppendText(text + Environment.NewLine);
                }));
            }
            else
            {
                richTextBox1.AppendText(text + Environment.NewLine);
            }

        }
        private void OnAccepted(object sender)
        {
            TcpClient tc = sender as TcpClient;

            while (true)
            {
                try
                {
                    NetworkStream ns = tc.GetStream();
                    byte[] buffer = new byte[1024];

                    ns.Read(buffer, 0, buffer.Length);
                    string msg = Encoding.Unicode.GetString(buffer);
                    msg = msg.Substring(0, msg.IndexOf("$"));
                    DisplayText(msg);
                }
                catch (SocketException se)
                {
                    Console.WriteLine(se);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    break;
                }

            }
            tc.Close();

        }

    }
}
