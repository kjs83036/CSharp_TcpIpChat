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
using MyLibrary_2_Custom_Data_Format;
using ProtoBuf;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using MyLibrary_1_winform_view_thread;

namespace Server
{
    public partial class Form1 : Form
    {
        Chat chat;
        Tcp_Server tcp_server;
        Winform_Thread_Display winform_thread_display;

        public Form1()
        {
            InitializeComponent();


            winform_thread_display = new Winform_Thread_Display();
            chat = new Chat(winform_thread_display, richTextBox1, richTextBox2, richTextBox3,richTextBox4, listBox1);
            tcp_server = new Tcp_Server(winform_thread_display, chat, richTextBox1);

            Thread t = new Thread(tcp_server.InitSocket);
            t.IsBackground = true;
            t.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string room_number = listBox1.GetItemText(listBox1.SelectedItem);
            chat.Display_Room_Text(room_number);
            chat.Display_UserList(room_number);
        }
    }

    class Chat
    {
        public List<User> user_list { set; get; } = new List<User>();
        public List<Room> room_list { set; get; } = new List<Room>();
        public List<string> log_list { set; get; } = new List<string>(); // 로그 기록하려면 richbox1을 display한 후 해당 내용을 list에 담으면됨(winform display 라이브러리 상속한 새로운 클래스로?)
        Winform_Thread_Display winform_thread_display;
        RichTextBox richTextBox1;
        RichTextBox richTextBox2;
        RichTextBox richTextBox3;
        RichTextBox richTextBox4;
        ListBox listBox;

        public Chat(Winform_Thread_Display winform_thread_display, RichTextBox richTextBox1, RichTextBox richTextBox2, RichTextBox richTextBox3, RichTextBox richTextBox4, ListBox listBox)
        {
            this.winform_thread_display = winform_thread_display;
            this.richTextBox1 = richTextBox1;
            this.richTextBox2 = richTextBox2;
            this.richTextBox3 = richTextBox3;
            this.richTextBox4 = richTextBox4;
            this.listBox = listBox;
        }

        public void Make_Room(Data data_from_client)
        {

            
            if (data_from_client.keyword == "nr")
            {
                
                User user = Find_User(data_from_client.user_name);
                //이미 방이 존재
                if (Find_Room(data_from_client.msg) != null)
                {
                    Data data = new Data()
                    {
                        keyword = "ms",
                        room_number = user.room_number ?? "000",
                        user_name = user.user_name,
                        user_list = null,
                        room_list = null,
                        msg = "from server to " + user.user_name + " : " + data_from_client.msg + "room exist"
                    };
                    Send_To_Client(user.tc ,data);
                }
                else if (data_from_client.msg == "000")
                {
                    Data data = new Data()
                    {
                        keyword = "ms",
                        room_number = user.room_number ?? "000",
                        user_name = user.user_name,
                        user_list = null,
                        room_list = null,
                        msg = "from server to " + user.user_name + " : " + data_from_client.msg + "can't make 0 room "
                    };
                    Send_To_Client(user.tc, data);
                }
                else
                {
                    // 기존에 참가한 방이있는지 확인
                    // 있으면 방에 방에있는 모두에게 메시지 + 방나가기 + 방인원 새로고침
                    string msg = "from server to " + user.user_name + " leave room : " + user.room_number;
                    Room room_before = Find_Room(user.room_number);
                    if (If_Join_Room(user))
                    {
                        string after_room_number = data_from_client.room_number;
                        data_from_client.room_number = user.room_number;

                        data_from_client.keyword = "ro";
                        Room_Out(data_from_client);
                        data_from_client.keyword = "nr";

                        Send_MS_All(room_before, msg);
                        

                        data_from_client.room_number = after_room_number;

                        

                    }

                    // 방만들기
                    Room room = new Room();
                    room.chat_text_tuple = new List<Tuple<int, string, string>>();
                    room.cur_user_list = new List<User>();
                    room.room_number = data_from_client.msg;
                    room.cur_user_list.Add(user);
                    room_list.Add(room);
                    user.room_number = data_from_client.msg;
                    Refresh_User(room_before);
                    Send_UL_All(room_before);

                    Display_RoomList(room.room_number);


                    

                    Data data = new Data()
                    {
                        keyword = "nr",
                        room_number = data_from_client.msg ?? "000",
                        user_name = user.user_name,
                        user_list = Make_User_List(room),
                        room_list = null,
                        msg = "you make room : " + user.room_number
                    };

                    winform_thread_display.DisplayText("from " + user.user_name + " make room : " + user.room_number, richTextBox1);
                    Send_To_Client(user.tc, data);
                    
                    
                        


                }

            }

            
        }

        

        public void Join_Room(Data data_from_client)
        {
            if (data_from_client.keyword == "jr")
            {
                User user = Find_User(data_from_client.user_name);
                Room room = Find_Room(data_from_client.msg);

                if (room !=  null && room.room_number == data_from_client.room_number)
                {
                    Data data = new Data()
                    {
                        keyword = "ms",
                        room_number = user.room_number ?? "000",
                        user_name = user.user_name,
                        user_list = null,
                        room_list = null,
                        msg = ""
                    };

                    Send_To_Client(user.tc, data);
                }

                else if (room != null )
                {

                    room.cur_user_list.Add(user);
                    
                    winform_thread_display.DisplayText(user.user_name + " : join " + user.room_number + " room", richTextBox1);

                    string msg = "from server to : " + user.user_name + " join " + user.room_number + " room";
                    Send_MS_All(Find_Room(data_from_client.msg), msg);
                    if (If_Join_Room(user))
                    {
                        msg = "from server to " + user.user_name + " leave " + user.room_number + " room";
                        Send_MS_All(Find_Room(user.room_number), msg);
                        string after_room_number = data_from_client.room_number;
                        data_from_client.room_number = user.room_number;

                        data_from_client.keyword = "ro";
                        Room_Out(data_from_client);
                        data_from_client.keyword = "jr";

                        data_from_client.room_number = after_room_number;


                    }
                    room.cur_user_list.Add(user);
                    user.room_number = data_from_client.msg;
                    //이전방 ul초기화
                    Room room_before = Find_Room(user.room_number);
                    Refresh_User(room_before);
                    Send_UL_All(room_before);
                    Data data = new Data()
                    {
                        keyword = "jr",
                        room_number = data_from_client.msg,
                        user_name = user.user_name,
                        user_list = Make_User_List(room),
                        room_list = null,
                        msg = "from server to : " + user.user_name + " join " + user.room_number + " room"
                    };

                    Send_To_Client(user.tc, data);

                }
                
                else
                {

                    Data data = new Data()
                    {
                        keyword = "ms",
                        room_number = user.room_number ?? "000",
                        user_name = user.user_name,
                        user_list = null,
                        room_list = null,
                        msg = "from server to " + user.user_name + " : " + "no room"
                    };

                    Send_To_Client(user.tc, data);

                }
                
            }
        }

        

        public void Message(Data data_from_client)
        {
            
            if (data_from_client.keyword == "ms")
            {
                User user = Find_User(data_from_client.user_name);
                winform_thread_display.DisplayText("from " + user.user_name + " : " + data_from_client.msg, richTextBox1);
                //user.chat_text_list.Add(data_from_client.msg); //보류
                Room room = Find_Room(user.room_number);
                if (room != null)
                {
                    room.chat_text_tuple.Add(new Tuple<int, string, string>(room.count,user.user_name, data_from_client.msg));
                    room.count++;

                    if (room != null)
                    {
                        foreach (var u in user_list)
                        {
                            if (u.room_number == room.room_number)
                            {
                                List<string> user_list_string = Make_User_List(room);
                                Data data = new Data()
                                {
                                    keyword = "ms",
                                    room_number = u.room_number ?? "000",
                                    user_name = u.user_name,
                                    user_list = user_list_string,
                                    room_list = null,
                                    msg = user.user_name + " : "+ data_from_client.msg
                                };

                            Send_To_Client(u.tc, data);
                            };

                        }
                    }
                }

            }
            
        }

        public void Room_Check(Data data_from_client)
        {

            if (data_from_client.keyword == "rc")
            {
                User user = Find_User(data_from_client.user_name);

                List<string> room_list_string = Make_Room_List();


                Data data = new Data()
                {
                    keyword = "rc",
                    room_number = user.room_number ?? "000",
                    user_name = user.user_name,
                    user_list = null,
                    room_list = room_list_string,
                    msg = ""
                };

                Send_To_Client(user.tc, data);
            }
            
        }

        public void Room_Out(Data data_from_client)
        {
            if(data_from_client.keyword == "ro")
            {
                User user = Find_User(data_from_client.user_name);

                if (If_Join_Room(user))
                {
                    //기존방에서 삭제
                    Room room = Find_Room(data_from_client.room_number);
                    room.cur_user_list.Remove(user);

                    //보류
                    // user.chat_text_list.Clear();

                    string out_room_number = user.room_number;
                    user.room_number = "000";
                    Data data;
                    Refresh_User(room);

                    string msg = user.user_name + " : room_number " + user.room_number + "out";
                    Send_MS_All(room, msg);
                    Send_UL_All(room);

                    data = new Data()
                    {
                        keyword = "ro",
                        room_number = user.room_number ?? "000",
                        user_name = user.user_name,
                        user_list = null,
                        room_list = null,
                        msg = "from server to " + user.user_name + " : " + " room_out"
                    };

                    Send_To_Client(user.tc, data);

                    Clear_Room_Zero(room);
                }
                else
                {
                    Data data = new Data()
                    {
                        keyword = "ms",
                        room_number = user.room_number ?? "000",
                        user_name = user.user_name,
                        user_list = null,
                        room_list = null,
                        msg = "from server to " + user.user_name + " : " + "you didn't join room"
                    };

                    Send_To_Client(user.tc, data);
                }
            }
            
                

        }

        public void Name_Change(Data data_from_client)
        {
            if (data_from_client.keyword == "nc")
            {
                User user = Find_User(data_from_client.user_name);
                if(Find_User(data_from_client.msg) == null)
                {

                    user.user_name = data_from_client.msg;
                    List<string> user_list_string = new List<string>();
                    Room room = Find_Room(user.room_number);

                    if (data_from_client.room_number != "000")
                    {

                        
                        if (room != null)
                        {
                            Refresh_User(room);
                            user_list_string = Make_User_List(room);
                        }
                    }
                    
                    
                     

                    Data data = new Data()
                    {
                        keyword = "nc",
                        room_number = user.room_number ?? "000",
                        user_name = user.user_name,
                        user_list = user_list_string,
                        room_list = null,
                        msg = ""
                    };

                    Send_To_Client(user.tc, data);
                    Send_UL_All(room);
                }
                else
                {
                    Data data = new Data()
                    {
                        keyword = "ms",
                        room_number = user.room_number ?? "000",
                        user_name = user.user_name,
                        user_list = null,
                        room_list = null,
                        msg = "from server to " + user.user_name + "exist name"
                    };

                    Send_To_Client(user.tc, data);
                }
                

                

            }
        }

        public void UnConnect(Data datafromclient)
        {
            
            
            if (datafromclient.keyword == "uc")
            {
                user_list.Remove(Find_User(datafromclient.user_name));
                Display_ServerOnUser_List();
            }
        }


        //내부 함수


        private void Refresh_User(Room room)
        {
            if (room != null)
            {
                room.cur_user_list.Clear();
                foreach (var u in user_list)
                {
                    if (u.room_number == room.room_number)
                    {
                        room.cur_user_list.Add(u);
                    }
                }
            }
            
        }


        private bool If_Join_Room(User user)
        {
            if (user.room_number == "000" || user.room_number == null)
            {
                return false;
            }
            return true;
        }

        private void Send_UL_All(Room room)
        {

            if (room != null)
            {
                foreach (var u in user_list)
                {
                    if (u.room_number == room.room_number)
                    {
                        List<string> user_list_string = Make_User_List(room);
                        Data data = new Data()
                        {
                            keyword = "ul",
                            room_number = u.room_number ?? "000",
                            user_name = u.user_name,
                            user_list = user_list_string,
                            room_list = null,
                            msg = ""
                        };

                        Send_To_Client(u.tc, data);
                    }
                }
            }

        }

        private void Send_MS_All(Room room, string msg)
        {

            if (room != null)
            {
                foreach (var u in user_list)
                {
                    if (u.room_number == room.room_number)
                    {
                        List<string> user_list_string = Make_User_List(room);
                        Data data = new Data()
                        {
                            keyword = "ms",
                            room_number = u.room_number ?? "000",
                            user_name = u.user_name,
                            user_list = user_list_string,
                            room_list = null,
                            msg = msg
                        };

                        Send_To_Client(u.tc, data);
                    }
                }
            }
            
        }


        private List<string> Make_User_List(Room room)
        {
            if (room != null)
            {
                List<string> user_list = new List<string>();
                foreach (var u in room.cur_user_list)
                {
                    user_list.Add(u.user_name);
                }


                return user_list;
            }
            return null;
        }

        private List<string> Make_Room_List()
        {

            List<string> room_list_string = new List<string>();
            foreach (var r in room_list)
            {
                room_list_string.Add(r.room_number);
            }


            return room_list_string;
        }

        private void Clear_Room_Zero(Room room)
        {
            if (room.cur_user_list.Count() == 0)
            {
                room_list.Remove(room);
                winform_thread_display.DisplayClear(listBox);

                foreach( var r in room_list)
                {
                    winform_thread_display.DisplayText(r.room_number, listBox);
                }

            }
        }

        public Room Find_Room(string room_number)
        {
            foreach (var r in room_list)
            {
                if (r.room_number == room_number)
                {
                    return r;
                }

            }
            return null;
        }

        public User Find_User(string user_name)
        {
            foreach (var u in user_list)
            {
                if (u.user_name == user_name)
                {
                    return u;
                }

            }
            return null;
        }

        public void Send_To_Client(TcpClient tc, Data data)
        {
            NetworkStream ns = tc.GetStream();


            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ns, data);


            //byte[] buffer = Encoding.Unicode.GetBytes(data);
            //ns.Write(buffer, 0, buffer.Length);
            //ns.Flush();

            
        }

        public void Display_RoomList(string room_number)
        {
            winform_thread_display.DisplayText(room_number, listBox);
        }

        public void Display_Room_Text(string room_number)
        {
            
            foreach (var r in room_list)
            {
                if (r.room_number == room_number)
                {
                    winform_thread_display.DisplayClear(richTextBox2);
                    foreach ( var t in r.chat_text_tuple)
                    {
                        winform_thread_display.DisplayText(t.Item1 + " " + t.Item2 +" " + t.Item3, richTextBox2);
                    }

                }
            }
        }
        public void Display_UserList(string room_number)
        {
            if (room_number != "")
            {
                Room room = Find_Room(room_number);

                winform_thread_display.DisplayClear(richTextBox3);
                List<string> user_list = Make_User_List(room);
                foreach (var u in user_list)
                {
                    winform_thread_display.DisplayText(u, richTextBox3);
                }
            }
            
        }

        public void Display_ServerOnUser_List()
        {
            winform_thread_display.DisplayClear(richTextBox4);
            foreach ( var u in user_list)
            {
                winform_thread_display.DisplayText(u.user_name, richTextBox4);
            }
        }


    }

    class Tcp_Server
    {
        Winform_Thread_Display winform_thread_display;
        Chat chat;
        int count;
        private RichTextBox richTextBox1;

        public Tcp_Server(Winform_Thread_Display winform_thread_display, Chat chat, RichTextBox richTextBox1)
        {
            this.winform_thread_display = winform_thread_display;
            this.chat = chat;
            this.richTextBox1 = richTextBox1;
        }

        public void InitSocket()
        {
            TcpListener tl = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            tl.Start();
            winform_thread_display.DisplayText(">>> server start", richTextBox1);

            while (true)
            {
                try
                {
                    // 클라이언트 접속까지대기
                    TcpClient tc = tl.AcceptTcpClient();

                    User user = new User();
                    //user.chat_text_list = new List<string>();
                    user.tc = tc;
                    chat.user_list.Add(user);
                    // 각 클라이언트 별 clinet_number 컬렉션에 보관
                    user.user_name = count.ToString();
                    chat.Display_ServerOnUser_List();
                    count++;

                    winform_thread_display.DisplayText(">>> client connected", richTextBox1);

                    Data data = new Data()
                    {
                        keyword = "ms",
                        room_number = "000",
                        user_name = user.user_name,
                        user_list = null,
                        room_list = null,
                        msg = ""
                    };
                    chat.Send_To_Client(tc, data);

                    Thread t = new Thread(new ParameterizedThreadStart(OnAccepted));
                    t.IsBackground = true;
                    t.Start(user);

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

        private void OnAccepted(object sender)
        {

            User user = (User)sender;
            TcpClient tc = user.tc;

            while (true)
            {
                try
                {

                    NetworkStream ns = tc.GetStream();
                    //byte[] buffer = new byte[1024];

                    //ns.Read(buffer, 0, buffer.Length);
                    //string data = Encoding.Unicode.GetString(buffer);

                    BinaryFormatter bf = new BinaryFormatter();

                    object test_obj = bf.Deserialize(ns) as object;
                    Data data_from_client = (Data)test_obj;

                    // 유저 체크 
                    user = chat.Find_User(data_from_client.user_name);
                    if (user == null)
                    {
                    }
                    else
                    {
                        //nr
                        chat.Make_Room(data_from_client);
                        //jr
                        chat.Join_Room(data_from_client);
                        //ms
                        chat.Message(data_from_client);
                        //rc
                        chat.Room_Check(data_from_client);
                        //ro
                        chat.Room_Out(data_from_client);
                        //nc
                        chat.Name_Change(data_from_client);

                        chat.UnConnect(data_from_client);
                    }
                    chat.Display_ServerOnUser_List();
                    

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

    }

}