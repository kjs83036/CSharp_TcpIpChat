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
using System.IO;
using System.Threading;


namespace chat_winform_study_client
{
    public partial class Form1 : Form

    {
        TcpClient clientSocket = new TcpClient();

        public Form1()
        {
            InitializeComponent();

            new Thread(delegate ()
           {
               InitSocket();
           }).Start();
        }

        private void InitSocket()
        {
            try
            {
                clientSocket.Connect("127.0.0.1", 9999);
                DisplayText("Client Started");
                textBox1.Text = "ClientSoket Program - Server Connected ...";

                Thread threadHandler = new Thread(new ParameterizedThreadStart(OnAccepted));
                threadHandler.IsBackground = true;
                threadHandler.Start(clientSocket);
            }
            catch(SocketException se)
            {
                MessageBox.Show(se.Message, "Error");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }


        private void OnAccepted(object sender)
        {
            TcpClient clientSocket = sender as TcpClient;
            while (true)
            {
                try
                {
                    NetworkStream stream = clientSocket.GetStream();
                    byte[] buffer = new byte[1024];

                    //???
                    stream.Read(buffer, 0, buffer.Length);
                    string msg = Encoding.Unicode.GetString(buffer);
                    msg = msg.Substring(0, msg.IndexOf("$"));
                    DisplayText(">> Data from server - " + msg);

                    //string response = "Last Message from client - " + msg;
                    //byte[] sbuffer = Encoding.Unicode.GetBytes(response);

                    //stream.Write(sbuffer, 0, sbuffer.Length);
                    //stream.Flush();

                    //DisplayText(">>" + response);
                }
                catch (SocketException se)
                {
                    DisplayText(string.Format("OnAccepted : SocketException : {0}", se.Message));
                    break;
                }
                catch (Exception ex)
                {
                    DisplayText(string.Format("OnAccepted : Exception : {0}", ex.Message));
                    break;
                }

            }
            clientSocket.Close();
        }

        private void DisplayText(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.BeginInvoke(new MethodInvoker(delegate
                {
                    richTextBox1.AppendText(Environment.NewLine + ">>" + text);
                }));
            }
            else
                richTextBox1.AppendText(Environment.NewLine + ">>" + text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetworkStream stream = clientSocket.GetStream();
            byte[] sbuffer = Encoding.Unicode.GetBytes(richTextBox2.Text + "$");
            stream.Write(sbuffer, 0, sbuffer.Length);
            stream.Flush();

            //byte[] rbuffer = new byte[1024];
            //stream.Read(rbuffer, 0, rbuffer.Length);
            //string msg = Encoding.Unicode.GetString(rbuffer);
            //DisplayText(msg);

            richTextBox2.Text = "";
            richTextBox2.Focus();
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (clientSocket != null)
            {
                clientSocket.Close();
            }
        }
    }
}
