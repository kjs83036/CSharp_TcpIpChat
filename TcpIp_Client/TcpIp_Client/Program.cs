using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;

namespace TcpIp_Client
{
    class Client
    {
        void send()
        {
            
        }
        void receive()
        {

        }
        void run_client()
        {

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TcpClient tcp_client = new TcpClient("127.0.0.1", 9999);
                NetworkStream ns = tcp_client.GetStream();
                StreamReader sr = new StreamReader(ns);
                StreamWriter sw = new StreamWriter(ns);

                Console.WriteLine(sr.ReadLine());
                while(true)
                {


                    String sendMsg = Console.ReadLine();
                    sw.WriteLine(sendMsg);
                    sw.Flush();
                    if (sendMsg == "exit")
                        break;
                    
                    Console.WriteLine(sr.ReadLine());
                }
                sr.Close();
                sw.Close();
                ns.Close();
                tcp_client.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
