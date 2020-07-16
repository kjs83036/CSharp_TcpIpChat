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

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        //소캣선언
        //각 클라이언트의 tcpclient를 모을 컬렉션 선언
        //쓰레드에 데이터를 옮길 튜플 선언

        TcpListener tl;
        TcpClient tc;
        Dictionary<TcpClient, string> dic_tc = new Dictionary<TcpClient, string>();
        int count;
        Tuple<TcpClient, string> tuple;


        public Form1()
        {
            InitializeComponent();

            //워크 스레드에 소캣 초기화후 스타트
            Thread t = new Thread(InitSocket);
            t.IsBackground = true;
            t.Start();

        }

        private void InitSocket()
        {
            tl = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            tl.Start();
            DisplayText(">>> server start");

            while (true)
            {
                try
                {
                    // 클라이언트 접속까지대기
                    tc = tl.AcceptTcpClient();
                    // 각 클라이언트 별 tcpclient를 컬렉션에 보관
                    dic_tc.Add(tc, "tc" + count.ToString());
                    
                    DisplayText(">>> client connected");

                    //튜플을 활용하여 다수의 데이터를 단수의 파라미터로 쓰레드에 전설
                    tuple = new Tuple<TcpClient, string>(tc,dic_tc[tc]);
                    Thread t = new Thread(new ParameterizedThreadStart(OnAccepted));
                    t.IsBackground = true;
                    t.Start(tuple);

                    count++;
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
            tl.Stop();
             
            
        }

        private void DisplayText(string text)
        {
            //스레드가 다르면 invokeRequired가 true
            if (richTextBox1.InvokeRequired)
            {
                //beginInvoke 는 비동기식
                richTextBox1.BeginInvoke(new MethodInvoker(delegate
                {
                    richTextBox1.AppendText(text + "\n");
                }));
            }
            else
            {
                richTextBox1.AppendText(text + "\n");
            }

        }

        //실시간으로 계속 데이터를 받기위하여 무한루프
        private void OnAccepted(object sender)
        {

            Tuple<TcpClient, string> tuple = (Tuple<TcpClient, string>)sender;
            TcpClient tc = tuple.Item1;
            string name = tuple.Item2;

            

            while (true)
            {
                try
                {
                    NetworkStream ns = tc.GetStream();
                    byte[] buffer = new byte[1024];

                    ns.Read(buffer, 0, buffer.Length);
                    string msg = Encoding.Unicode.GetString(buffer);
                    msg = msg.Substring(0, msg.IndexOf("$"));
                    DisplayText("from client name " + name + ": " + msg);

                    //자기자신의 클라이언트를 제외한 모든 클라이언트에 메세지 전송
                    foreach (var d in dic_tc)
                    {
                        //자기자신 제외
                        if(d.Value != name)
                        {
                            ns = d.Key.GetStream();
                            buffer = Encoding.Unicode.GetBytes("from server to " + d.Value + " : " + msg + "$");
                            ns.Write(buffer, 0, buffer.Length);
                            ns.Flush();
                        }
                        
                    }
                }
                catch(SocketException se)
                {
                    Console.WriteLine(se);
                    break;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    break;
                }
                
            }
            tc.Close();

        }

        //서버 -> 클라이언트 데이터 전송
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var d in dic_tc)
                {
                    NetworkStream ns = d.Key.GetStream();
                    byte[] buffer = new byte[1024];
                    buffer = Encoding.Unicode.GetBytes("from server to " + d.Value + " : " + richTextBox2.Text + "$");
                    ns.Write(buffer, 0, buffer.Length);
                    ns.Flush();
                }
                

                richTextBox2.Text = "";
                richTextBox2.Focus();
            }
            catch(Exception error)
            {
                Console.WriteLine(error);
            }
            

        }

        //종료시 소캣 클로즈
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (tc != null)
            {
                tc.Close();
            }
            if  (tl != null)
            {
                tl.Stop();
            }
        }
    }
}
