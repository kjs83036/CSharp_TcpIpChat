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
using MyLibrary_1_winform_view_thread;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using MyLibrary_2_Custom_Data_Format;

namespace Client
{
    public partial class Form1 : Form
    {

        TcpCommonClient tcpCommonClient;
        //Chat chat;
        Winform_Thread_Display winformThreadDisplay;
        public Form1()
        {
            InitializeComponent();

            winformThreadDisplay = new Winform_Thread_Display();
            tcpCommonClient = new TcpCommonClient(winformThreadDisplay, richTextBoxMain, richTextBoxCurUser, textBoxAllRoom, textBoxCurRoom, textBoxName);
            //chat = new Chat(richTextBoxMain, richTextBoxCurUser, textBoxAllRoom, textBoxCurRoom, textBoxName, winformThreadDisplay, dataFromServer);
            TcpClient tc = tcpCommonClient.InitSocket();
            Thread t = new Thread(new ParameterizedThreadStart(tcpCommonClient.RecieveFromServer));
            t.IsBackground = true;
            t.Start(tc);
        }


        private void textBox1_Click(object sender, EventArgs e)
        {
            textBoxRoomNumber.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 방참여 jr

            string msg = textBoxRoomNumber.Text;
            tcpCommonClient.SendToServer("jr", msg);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //방 나가기 ro
            tcpCommonClient.SendToServer("ro", "");
            winformThreadDisplay.DisplayClear(richTextBoxCurUser);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //대화 전송 ms
            string msg = richTextBoxInput.Text;
            tcpCommonClient.SendToServer("ms", msg);

            richTextBoxInput.Text = "";
            richTextBoxInput.Focus();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //방생성 nr
            string after_room_number = textBoxRoomNumber.Text;
            tcpCommonClient.SendToServer("nr", after_room_number);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //방확인 rc
            tcpCommonClient.SendToServer("rc", "");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //이름 변경 nc
            string after_user_name = textBoxName.Text;
            tcpCommonClient.SendToServer("nc", after_user_name);
        }



        class Chat
        {

            RichTextBox richTextBoxMain;
            RichTextBox richTextBoxCurUser;
            TextBox textBoxAllRoom;
            TextBox textBoxCurRoom;
            TextBox textBox_name;
            Data dataFromServer;

            public Chat(RichTextBox richTextBoxMain, RichTextBox richTextBoxCurUser, TextBox textBoxAllRoom, TextBox textBoxCurRoom, TextBox textBox_name, Data dataFromServer)
            {
                this.richTextBoxMain = richTextBoxMain;
                this.richTextBoxCurUser = richTextBoxCurUser;
                this.textBoxAllRoom = textBoxAllRoom;
                this.textBoxCurRoom = textBoxCurRoom;
                this.textBox_name = textBox_name;
                this.dataFromServer = dataFromServer;
            }


        }

        private void formClosing(object sender, FormClosingEventArgs e)
        {
            tcpCommonClient.SendToServer("uc", "");
            tcpCommonClient.unConnect();
        }
    }

    public class TcpCommonClient
    {
        Winform_Thread_Display winformThreadDisplay;
        RichTextBox richTextBoxMain;
        private RichTextBox richTextBoxCurUser;
        private TextBox textBoxAllRoom;
        private TextBox textBoxCurRoom;
        private TextBox textBox_name;
        TcpClient tc;
        User user;

        public TcpCommonClient(Winform_Thread_Display winformThreadDisplay, RichTextBox richTextBoxMain, RichTextBox richTextBoxCurUser, TextBox textBoxAllRoom, TextBox textBoxCurRoom, TextBox textBox_name)
        {
            this.richTextBoxMain = richTextBoxMain;
            this.richTextBoxCurUser = richTextBoxCurUser;
            this.textBoxAllRoom = textBoxAllRoom;
            this.textBoxCurRoom = textBoxCurRoom;
            this.textBox_name = textBox_name;
            this.winformThreadDisplay = winformThreadDisplay;
            user = new User();
        }

        public TcpClient InitSocket()
        {
            try
            {
                tc = new TcpClient("127.0.0.1", 9999);
                winformThreadDisplay.DisplayText(">>> server connected", richTextBoxMain);

                NetworkStream ns = tc.GetStream();
                //byte[] buffer = new byte[1024];

                //ns.Read(buffer, 0, buffer.Length);
                //string msg = Encoding.Unicode.GetString(buffer);

                BinaryFormatter bf = new BinaryFormatter();

                object test_obj = bf.Deserialize(ns) as object;
                Data dataFromServer = (Data)test_obj;
                user.user_name = dataFromServer.user_name;
                user.room_number = dataFromServer.room_number;

                winformThreadDisplay.DisplayText(dataFromServer.user_name, textBox_name);
                winformThreadDisplay.DisplayText(dataFromServer.room_number, textBoxCurRoom);


                return tc;

            }
            catch (SocketException se)
            {
                Console.WriteLine(se);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        private void OnAccepted(object sender)
        {
            TcpClient result = sender as TcpClient;

            while (true)
            {
                try
                {
                    NetworkStream ns = tc.GetStream();

                    BinaryFormatter bf = new BinaryFormatter();

                    object test_obj = bf.Deserialize(ns) as object;
                    Data dataFromServer = (Data)test_obj;

                    if (dataFromServer.keyword == "jr")
                    {
                        if (dataFromServer.room_number != "000")
                        {
                            winformThreadDisplay.DisplayClear(textBoxCurRoom);
                            winformThreadDisplay.DisplayText(dataFromServer.room_number, textBoxCurRoom);
                        }

                        winformThreadDisplay.DisplayClear(richTextBoxCurUser);
                        List<string> user_list_string = dataFromServer.user_list;

                        foreach (var u in user_list_string)
                        {
                            winformThreadDisplay.DisplayText(u, richTextBoxCurUser);
                        }
                        winformThreadDisplay.DisplayText(dataFromServer.msg, richTextBoxMain);
                        user.room_number = dataFromServer.room_number;
                    }

                    else if (dataFromServer.keyword == "rc")
                    {
                        List<string> room_list = dataFromServer.room_list;

                        winformThreadDisplay.DisplayClear(textBoxAllRoom);
                        foreach (var r in room_list)
                        {
                            winformThreadDisplay.DisplayText(r + " ", textBoxAllRoom);
                        }
                    }
                    else if (dataFromServer.keyword == "ms")
                    {
                        winformThreadDisplay.DisplayText(dataFromServer.msg, richTextBoxMain);
                    }
                    else if (dataFromServer.keyword == "nr")
                    {
                        winformThreadDisplay.DisplayClear(textBoxCurRoom);
                        winformThreadDisplay.DisplayText(dataFromServer.room_number, textBoxCurRoom);
                        winformThreadDisplay.DisplayText(dataFromServer.msg, richTextBoxMain);

                        List<string> user_list_string = dataFromServer.user_list;

                        winformThreadDisplay.DisplayClear(richTextBoxCurUser);
                        foreach (var u in user_list_string)
                        {
                            winformThreadDisplay.DisplayText(u, richTextBoxCurUser);
                        }
                        user.room_number = dataFromServer.room_number;

                    }
                    else if (dataFromServer.keyword == "ro")
                    {
                        if (dataFromServer.room_number != "000")
                        {
                            List<string> user_list_string = dataFromServer.user_list;

                            winformThreadDisplay.DisplayClear(richTextBoxCurUser);
                            foreach (var u in user_list_string)
                            {
                                winformThreadDisplay.DisplayText(u, richTextBoxCurUser);
                            }

                        }
                        else
                        {
                            winformThreadDisplay.DisplayClear(richTextBoxMain);
                            winformThreadDisplay.DisplayClear(richTextBoxCurUser);
                        }
                        user.room_number = dataFromServer.room_number;
                    }
                    else if (dataFromServer.keyword == "nc")
                    {
                        winformThreadDisplay.DisplayClear(textBox_name);
                        winformThreadDisplay.DisplayText(dataFromServer.user_name, textBox_name);
                        winformThreadDisplay.DisplayClear(richTextBoxCurUser);
                        if (dataFromServer.user_list != null)
                        {
                            foreach (var u in dataFromServer.user_list)
                            {
                                winformThreadDisplay.DisplayText(u, richTextBoxCurUser);
                            }
                        }
                        user.user_name = dataFromServer.user_name;

                    }
                    else if (dataFromServer.keyword == "ul")
                    {
                        winformThreadDisplay.DisplayClear(richTextBoxCurUser);
                        if (dataFromServer.user_list != null)
                        {
                            foreach (var u in dataFromServer.user_list)
                            {
                                winformThreadDisplay.DisplayText(u, richTextBoxCurUser);
                            }
                        }
                    }


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

        }

        public void SendToServer(string keyword, string msg)
        {
            try
            {
                NetworkStream ns = tc.GetStream();

                Data data = new Data()
                {
                    keyword = keyword,
                    room_number = user.room_number,
                    user_name = user.user_name,
                    user_list = null,
                    room_list = null,
                    msg = msg
                };

                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ns, data);

                //ns.Flush();

            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }


        public void RecieveFromServer(object obj)
        {
            try
            {
                Thread t = new Thread(new ParameterizedThreadStart(OnAccepted));
                t.IsBackground = true;
                t.Start(obj);
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

        public void unConnect()
        {
            tc.Close();
        }


    }
}
