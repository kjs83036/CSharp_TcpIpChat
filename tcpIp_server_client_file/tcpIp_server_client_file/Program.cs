using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcpIp_server_client_file
{
    
    class ServerClient
    {

        public void send(TcpClient tcp_client, NetworkStream ns, StreamReader sr, StreamWriter sw)
        {
            while (true)
            {
                string sendMsg = Console.ReadLine();
                sw.WriteLine(sendMsg);
                sw.Flush();
                //Console.WriteLine(sr.ReadLine());
                if (sendMsg == "file")
                {
                    //Console.WriteLine(sr.ReadLine());

                    using (StreamReader file_sr = new StreamReader("test.txt"))
                    {
                        string line;
                        Console.WriteLine("test");
                        while ((line = file_sr.ReadLine()) != null)
                        {
                            sw.WriteLine(line);
                            sw.Flush();
                            Console.WriteLine("sending");
                            //sw.Flush();
                        }
                    }
                    sw.WriteLine("exit_file");
                    sw.Flush();
                    Console.WriteLine("please 1 enter");

                    //break;


                }
                if (sendMsg == "exit")
                {
                    break;
                }

            }
            sr.Close();
            sw.Close();
            ns.Close();
            tcp_client.Close();
        }
        public void receive(TcpClient tcp_client, NetworkStream ns, StreamReader sr, StreamWriter sw)
        {
            string msg;
            while (true)
            {
                msg = sr.ReadLine();
                Console.WriteLine(msg);
                if (msg == "file")
                {
                    //sw.WriteLine("file process");
                    //sw.Flush();
                    using (StreamWriter file_sw = new StreamWriter("test2.txt"))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line == "exit_file")
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
            
        }
        public void runServer(out TcpListener socket_server, out TcpClient tcp_client, out NetworkStream ns, out StreamReader sr, out StreamWriter sw)
        {
            socket_server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            socket_server.Start();
            Console.WriteLine("server start");
            tcp_client = socket_server.AcceptTcpClient();
            ns = tcp_client.GetStream();
            sr = new StreamReader(ns);
            sw = new StreamWriter(ns);



            sw.WriteLine("서버 접속 성공");
            sw.Flush();
                
        }
        public void runClient(out TcpClient tcp_client, out NetworkStream ns, out StreamReader sr, out StreamWriter sw)
        {
            tcp_client = new TcpClient("127.0.0.1", 9999);
            ns = tcp_client.GetStream();
            sr = new StreamReader(ns);
            sw = new StreamWriter(ns);


            Console.WriteLine(sr.ReadLine());
            
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            ServerClient sc = new ServerClient();
            TcpListener socket_server;
            TcpClient tcp_client;
            NetworkStream ns;
            StreamReader sr;
            StreamWriter sw;
            Console.WriteLine("input server or client");
            string text = Console.ReadLine();
            Console.WriteLine("input send or receive");
            string text2 = Console.ReadLine();
            try
            {
                if (text == "server")
                {
                    sc.runServer(out socket_server, out tcp_client, out ns, out sr, out sw);

                    if (text2 == "send")
                    {
                        sc.send(tcp_client, ns, sr, sw);
                    }
                    else
                    {
                        sc.receive(tcp_client, ns, sr, sw);
                    }
                    socket_server.Stop();
                }
                else
                {
                    sc.runClient(out tcp_client, out ns, out sr, out sw);
                    if (text2 == "send")
                    {
                        sc.send(tcp_client, ns, sr, sw);
                    }
                    else
                    {
                        sc.receive(tcp_client, ns, sr, sw);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            

            

        }
    }
}
