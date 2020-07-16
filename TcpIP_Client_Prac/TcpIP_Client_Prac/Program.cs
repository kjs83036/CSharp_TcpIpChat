using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace TcpIP_Client_Prac
{
    class Program
    {
        static void Main(string[] args)
        {
            string recvMsg;
            string sendMsg;

            try
            {
            TcpClient tcp_clint = new TcpClient("127.0.0.1", 9999);
            NetworkStream ns = tcp_clint.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);

            recvMsg = sr.ReadLine();
            Console.WriteLine(recvMsg);

            while(true)
            {
                sendMsg = Console.ReadLine();
                sw.WriteLine(sendMsg);
                sw.Flush();
                if (sendMsg == "exit")
                {
                    break;
                }
                recvMsg = sr.ReadLine();
                Console.WriteLine(recvMsg);
            }
            sr.Close();
            sw.Close();
            ns.Close();
            tcp_clint.Close();
            Console.WriteLine("접속종료");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
