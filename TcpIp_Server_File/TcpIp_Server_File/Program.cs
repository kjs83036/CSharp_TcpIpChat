using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace TcpIp_server
{
    class TcpServer
    {
        void fileSend()
        {

        }
        void fileReceive()
        {

        }
        void runServer()
        {

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TcpListener socket_server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
                socket_server.Start();
                Console.WriteLine("server start");
                TcpClient tcp_client = socket_server.AcceptTcpClient();
                NetworkStream ns = tcp_client.GetStream();
                StreamReader sr = new StreamReader(ns);
                StreamWriter sw = new StreamWriter(ns);



                sw.WriteLine("서버 접속 성공");
                sw.Flush();

                string msg;
                while(true)
                {
                    msg = sr.ReadLine();
                    Console.WriteLine(msg);
                    if (msg =="file")
                    {
                        //sw.WriteLine("file process");
                        //sw.Flush();
                        using (StreamWriter file_sw = new StreamWriter("test2.txt"))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (line =="exit_file")
                                {
                                    break;
                                }
                                //Console.WriteLine(line);
                                file_sw.WriteLine(line);
                            }
                            

                        }
                        Console.WriteLine("receiving end");
                        //sw.WriteLine("done");
                        //sw.Flush();
                        //break;
                    }
                    
                    if (msg == "exit")
                    {
                        break;
                    }
                    sw.WriteLine("done from server");
                    sw.Flush();
                }
                
                sw.Close();
                sr.Close();
                ns.Close();
                socket_server.Stop();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
