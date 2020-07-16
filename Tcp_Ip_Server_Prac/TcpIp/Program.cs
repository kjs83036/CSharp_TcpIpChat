using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace TcpIp
{
    class Program
    {
        static void Main(string[] args)
        {
            String msg;
            try
            {
                TcpListener socket_server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
                socket_server.Start();
                Console.WriteLine("start");
                TcpClient client = socket_server.AcceptTcpClient();
                NetworkStream ns = client.GetStream();
                StreamReader sr = new StreamReader(ns);
                StreamWriter sw = new StreamWriter(ns);

                String successMsg = "서버 접속 성공";
                sw.WriteLine(successMsg);
                sw.Flush();

                while(true)
                {
                    msg = sr.ReadLine();
                    if (msg == "exit")
                        break;
                    Console.WriteLine(msg);
                    sw.WriteLine(msg);
                    sw.Flush();

                }
                sw.Close();
                sr.Close();
                ns.Close();
                socket_server.Stop();
                Console.WriteLine("종료");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
