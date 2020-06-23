using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Schiduch.Classes.Program;

namespace Schiduch
{
    class Messages
    {
        public int MsgId;
        public int ByUser;
        public string Topic;
        public int UnReadUser = 0;
        public int UnReadManger = 0;
        public static Messages[] LoadAllMessages()
        {
            if (GLOBALVARS.MyUser.Control != User.TypeControl.Admin)
                return LoadMessages();
            else if (GLOBALVARS.MyUser.Control == User.TypeControl.Admin)
                return LoadMessages(true);
            return null;
             
        }

        private static Messages[] LoadMessages(bool ForManger=false)
        {
            int count = 0;
            int temp = 0;
            Messages[] msg=null;
            SqlDataReader reader;
            if(ForManger)
                reader = DBFunction.ExecuteReader("select count(byuser) as res_count,* from msgs");
            else
            reader = DBFunction.ExecuteReader("select count(byuser) as res_count,* from msgs where byuser=" + GLOBALVARS.MyUser.ID);

            while (reader.Read())
            {
                if (count == 0) {
                    count = (int)reader["res_count"];
                    msg = new Messages[count];
                  }
                msg[temp] = new Messages();
                msg[temp].ByUser = (int)reader["byuser"];
                msg[temp].Topic = reader["topic"].ToString();
                msg[temp].MsgId = (int)reader["msgid"];
            }
            reader.Close();
            return msg;
        }

        public void SendMessage()
        {
            SendMessage(this, this.Topic);
        }
        public static void SendMessage(Messages msg,string Content)
        {
            string sql = "";
            MessageDetails.MsgYype messageto=MessageDetails.MsgYype.UserToManger;
            SqlParameter[] prms = new SqlParameter[5];
            prms[1] = new SqlParameter("topic", msg.Topic);
            prms[2] = new SqlParameter("content", Content);
            prms[3] = new SqlParameter("byuser", msg.ByUser);
            prms[4] = new SqlParameter("date", DateTime.Now);
            int UnReadUser = 0;
            int UnReadManger = 0;

            if (GLOBALVARS.MyUser.Control == User.TypeControl.Admin) { 
                messageto = MessageDetails.MsgYype.MangerToUser;
                UnReadUser = 1;
            }
            else { UnReadManger = 1; }


            sql = "BEGIN TRANSACTION " +
            "DECLARE @DataID int;" +
            DBFunction.Execute("insert into [msgs] values(@byuser,@topic," + UnReadUser + "," + UnReadManger + 
            ");SELECT @DataID = scope_identity(); insert into msgdetails values(" +
            (int)messageto + "," +
            "@DataID,@date,0,@content); COMMIT");
            DBFunction.Execute(sql, prms);
        }
    }
    class MessageDetails
    {
        public enum MsgYype {UserToManger,MangerToUser };
        public int Type;
        public int RelatedId;
        public int Id;
        public string Content;
        public bool Read;
        public DateTime AddDate;
    }
}
