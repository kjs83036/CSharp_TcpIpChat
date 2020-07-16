using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace MyLibrary_2_Custom_Data_Format
{
    [Serializable]
    public class Data
    {
        public string keyword { get; set; }
        public string room_number { get; set; }
        public string user_name { get; set; }
        public string msg { get; set; }
        public List<string> user_list { get; set; }
        public List<string> room_list { get; set; }


    }

    public class User
    {

        public TcpClient tc;
        public string user_name { set; get; }
        public string room_number { set; get; }
        //public List<string> chat_text_list { set; get; }



    }

    public class Room
    {
        public int count { set; get; }
        public string room_number { set; get; }
        public List<User> cur_user_list { set; get; }
        public List<Tuple<int, string, string>> chat_text_tuple { set; get; }

    }
}
