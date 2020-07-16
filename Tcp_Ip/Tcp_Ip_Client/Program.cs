using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;

namespace Tcp_Ip_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string strRecvMsg;
                string strSendMsg;

                TcpClient sockClient = new TcpClient("127.0.0.1", 9090); //소켓생성,커넥트
                NetworkStream ns = sockClient.GetStream();
                StreamReader sr = new StreamReader(ns);
                StreamWriter sw = new StreamWriter(ns);

                strRecvMsg = sr.ReadLine();         //서버로부터 접속 성공 메시지 수신
                Console.WriteLine(strRecvMsg);

                while (true)
                {
                    strSendMsg = Console.ReadLine();

                    sw.WriteLine(strSendMsg);
                    sw.Flush();
                    if (strSendMsg == "exit")
                    {
                        break;
                    }
                    strRecvMsg = sr.ReadLine();
                    Console.WriteLine(strRecvMsg);
                }
                sr.Close();
                sw.Close();
                ns.Close();
                sockClient.Close();

                Console.WriteLine("접속 종료!");
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
