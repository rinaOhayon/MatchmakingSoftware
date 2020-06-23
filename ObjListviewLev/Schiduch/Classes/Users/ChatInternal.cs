using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Schiduch.Classes.Program;

namespace Schiduch
{
    class ChatInternal
    {
        int LastId = 0; // For Case of several second diff between pcs
        [StructLayout(LayoutKind.Sequential)]

        public struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME st);

        public DateTime LastCheck;
        public int USERID;
        private string LastCheckFile = Application.StartupPath + "//LastChat.bin";
        SqlDataReader reader;
        SqlConnection connect;
        SqlCommand command=new SqlCommand();
        SqlConnection DBConnectionReturn()
        {
            /*
            string con = "U2VydmVyPXRjcDpybWM0MmVpeXprLmRhdGFiYXNlLndpbmRvd3MubmV0LDE0MzM7RGF0YWJhc2U9c2hpZHVjaDtVc2VyIElEPWVsaWNoZW5Acm1jNDJlaXl6aztFbmNyeXB0PVRydWU7UGFzc3dvcmQ9RUxJNDcyMTNlbGk7Q29ubmVjdGlvbiBUaW1lb3V0PTMwOw==";
            connect = new SqlConnection(Encoding.UTF8.GetString(Convert.FromBase64String(con)));*/
            return DBFunction.dbcon.connect;
        }

        public void FixComputerClock()
        {
            if (!File.Exists("Clock")) { 
            DateTime clock=new DateTime();
            string sql = "SELECT GETUTCDATE() AT TIME ZONE 'Israel Standard Time' as clock";
            command.CommandText = sql;
            reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                clock = DateTime.Parse(reader["clock"].ToString());
            }
                reader.Close();
                
                SYSTEMTIME st = new SYSTEMTIME();
                st.wYear = short.Parse(clock.Year.ToString()); // must be short
                st.wMonth = short.Parse(clock.Month.ToString());
                st.wDay = short.Parse(clock.Day.ToString());
                st.wHour = short.Parse(clock.Hour.ToString());
                st.wMinute = short.Parse(clock.Minute.ToString());
                st.wSecond = short.Parse(clock.Second.ToString());
                SetSystemTime(ref st);
                File.Create("Clock").Close();

            }

        }
        public ChatInternal()
        {
            if (GLOBALVARS.LastChatChangeDB > GLOBALVARS.LastChatChangeFile) {
                File.Delete(LastCheckFile);
                File.Delete("Chat.bin");
                GLOBALVARS.LastChatChangeFile = DateTime.Now;
                UpdateManager.UpdateLastTimeCheck();
            }
            connect = DBConnectionReturn();
          //  connect.Open();
            command.Connection = connect;
            FixComputerClock();
            CheckExistFiles();
            LoadLastTimeCheck();
        }
        public List<ChatMessage> GetAllMessagesFromDataBase()
        {
            int temp = 0;
            List<ChatMessage> lst = new List<ChatMessage>();
            
            SqlParameter dateprm = new SqlParameter("@LASTCHECK", LastId);
            
            AppendLastTimeCheck();
            string txtcommand = "select [Chatid],[date],[from],[to],[content] from chats where ([TO]=" + USERID + " OR [TO] = 0) AND [Chatid] > @LASTCHECK order by date";

            command.CommandText = txtcommand;
            command.Parameters.Clear();
            command.Parameters.Add(dateprm);
            reader=command.ExecuteReader();
            
            while (reader.Read())
            {
                ChatMessage msg = new ChatMessage();
                msg.Date = reader.GetDateTime(1);
                msg.From = (int)reader["From"];
                msg.To = (int)reader["To"];
                msg.Content = (string)reader["Content"];
                temp = (int)reader["Chatid"];
                if (LastId <= temp)
                    LastId = temp;
                lst.Add(msg);
                AppendMsgToFile(msg);
            }
            if(!reader.IsClosed)
                reader.Close();
           
            // need to add thread timeing for read again in one second
            return lst;
        }

        public List<KeyValueClass> GetAllMyMessages()
        {
            List<KeyValueClass> lst = new List<KeyValueClass>();
         //   if(GLOBALVARS.MyUser.Control!=User.TypeControl.Admin)
                command.CommandText = "select [chatid],[content],[from] from chats where [from]=" + GLOBALVARS.MyUser.ID;
         //   else
         //       command.CommandText = "select [chatid],[content],[from] from chats";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                lst.Add(new KeyValueClass(reader["Content"].ToString(), (int)reader["chatid"]));
            }
            reader.Close();
            return lst;
        }
        void AppendMsgToFile(ChatMessage msg)
        {
            string content;
            content = msg.Date.ToString() + "|" + msg.Content.Replace("\r\n","<br />") + "|" + msg.From + "|" + msg.To + "\r\n";
            File.AppendAllText(Application.StartupPath + "//Chat.bin",content);

        }

        void AppendMsgToFile(string Msg,int From,int To)
        {
            string content;
            content = DateTime.Now+ "|" + Msg.Replace("\r\n", "<br />") + "|" + From + "|" + To  + "\r\n";
            File.AppendAllText(Application.StartupPath + "//Chat.bin", content);

        }

        void AppendLastTimeCheck()
        {
            File.WriteAllText(LastCheckFile,LastId.ToString());
        }

        void LoadLastTimeCheck()
        {
            if (!File.Exists(LastCheckFile))
                File.WriteAllText(LastCheckFile, "0");
            LastId = int.Parse(File.ReadAllText(LastCheckFile));
        }
        public List<ChatMessage> GetAllMessagesFromFile()
        {
            List<ChatMessage> lst = new List<ChatMessage>();
            string[] msgs= File.ReadAllLines(Application.StartupPath + "//Chat.bin");
            foreach(string str in msgs)
            {
                string[] linesplt=str.Split('|');
                ChatMessage msg = new ChatMessage();
                msg.Date = DateTime.Parse(linesplt[0]);
                msg.Content = linesplt[1];
                msg.From = int.Parse(linesplt[2]);
                msg.To = int.Parse(linesplt[3]);
                lst.Add(msg);
            }
            return lst;
        }

        private void CheckExistFiles()
        {
            try
            {
                if (!File.Exists(Application.StartupPath + "\\LastChat.bin"))
                    File.WriteAllText(Application.StartupPath + "\\LastChat.bin", "0");
                if (!File.Exists(Application.StartupPath + "\\Chat.bin"))
                    File.Create(Application.StartupPath + "\\Chat.bin").Close();
                if (File.Exists(Application.StartupPath + "\\bg.jpg") &&
                    !File.Exists(Path.GetPathRoot(Environment.SystemDirectory) + "\\bg.jpg"))
                    File.Copy(Application.StartupPath + "\\bg.jpg",
                        Path.GetPathRoot(Environment.SystemDirectory) + "\\bg.jpg", true);
            }
            catch {
                MessageBox.Show("אירעה שגיאה באחד מההגדרות שהתוכנה הגדירה, מומלץ להפעיל את התוכנה עם הרשאות מנהל בכדי לפתור את הבעיה");
            }
        }
        public void Close()
        {
            if(connect.State!=System.Data.ConnectionState.Closed)
                connect.Close();
        }

        public bool UpdateMsg(string stxt,int schat,int suser)
        {
            SqlParameter prm = new SqlParameter("@txt", stxt);
            command.CommandText = "Update chats set content=@txt where chatid=" + schat + " and [from]=" + suser;
            command.Parameters.Add(prm);
            int x=command.ExecuteNonQuery();
            command.Parameters.Clear();
            if (x > 0)
                return true;
            return false;
        }
        public bool DeleteMsg(int schat, int suser)
        {
            command.CommandText = "delete from chats where chatid=" + schat + " and [from]=" + suser;

            int x = command.ExecuteNonQuery();

            if (x > 0)
                return true;
            return false;
        }
        public void InsertMessageToDatabase(string Msg,int To,bool Fake=false)
        {
            SqlParameter prm = new SqlParameter("@msg", Msg);
            SqlParameter dateprm = new SqlParameter("@date", DateTime.Now);
            command.Parameters.Clear();
            command.Parameters.Add(prm);
            command.Parameters.Add(dateprm);
            command.CommandText = "Insert Into Chats([date],[content],[from],[to]) values(@date,@msg," + USERID + "," + To + ")";
            command.ExecuteNonQuery();
            //AppendMsgToFile(Msg, USERID, To);
        }
    }

    
    class ChatMessage
    {
        public string Content = "";
        public DateTime Date;
        public int To;
        public int From;
        public int ID;

    }
}
