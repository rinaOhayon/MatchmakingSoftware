using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

using System.Data.SqlClient;
using System.IO;
using Schiduch.Forms;
using Schiduch.Classes.Program;

namespace Schiduch
{

    public class User
    {
        public enum TypeControl { User = 0, Manger = 1, Admin = 2, Lock = 4, Delete = 5, WaitToCConfirm };
        public string UserName;
        public string Password;
        public string Email;
        public int ID;
        public string Name;
        public string Tel;
        public bool TempPersonal;//מאגר אישי זמני
        public bool TempGeneral;//מאגר כללי זמני
        public string Sector;
        public string SectorView = "";
        public TypeControl Control;
        public static User GetUser(string suser, string spass)
        {
            User retuser = null;
            SqlDataReader reader;
            spass = SetPassword(spass);
            SqlParameter puser = new SqlParameter("@user", suser);
            SqlParameter ppass = new SqlParameter("@pass", spass);
            reader = DBFunction.ExecuteReader("SELECT * from users where username=@user and password=@pass", puser, ppass);
            if (reader != null && reader.Read())
            {
                retuser = new User();
                retuser.Name = (string)reader["name"];
                retuser.UserName = (string)reader["username"];
                retuser.Password = ReturnPassword(reader["password"].ToString());
                retuser.Email = (string)reader["email"];
                retuser.ID = int.Parse(reader["id"].ToString());
                retuser.Tel = (string)reader["tel"];
                retuser.Control = (User.TypeControl)reader["control"];
                retuser.TempPersonal = (bool)reader["TempPersonal"];
                retuser.TempGeneral = (bool)reader["TempGeneral"];
                retuser.Sector = (string)reader["Sector"];
                 
                GLOBALVARS.LastUserChangeFile = DateTime.Now;
                UpdateManager.UpdateLastTimeCheck();
            }
            reader.Close();
            DBFunction.CloseConnections();
            return retuser;
        }
        public static User GetUser(int id)
        {
            SqlDataReader reader;
            User retuser = null;
            SqlParameter pid = new SqlParameter("@id", id);
            reader = DBFunction.ExecuteReader("select * from users where id=@id", pid);
            if (reader != null && reader.Read())
            {
                retuser = new User();
                retuser.Name = (string)reader["name"];
                retuser.UserName = (string)reader["username"];
                retuser.Password = reader["password"].ToString().Split('^')[1];
                retuser.Email = (string)reader["email"];
                retuser.ID = int.Parse(reader["id"].ToString());
                retuser.Tel = (string)reader["tel"];
                retuser.Control = (User.TypeControl)reader["control"];
                retuser.TempPersonal = (bool)reader["TempPersonal"];
                retuser.TempGeneral = (bool)reader["TempGeneral"];
                retuser.Sector = (string)reader["Sector"];
            }
            reader.Close();
            return retuser;
        }
        private static void WriteUserToFile(User u)
        {
            string user = "";
            byte[] forbase = Encoding.UTF8.GetBytes(u.Name);
            user = Convert.ToBase64String(forbase) + "\r\n";
            user += SplachScreen.Encrypt(u.UserName) + "\r\n" + SplachScreen.Encrypt(u.Password) + "\r\n";

            forbase = Encoding.UTF8.GetBytes(u.Email);
            user += Convert.ToBase64String(forbase) + "\r\n";
            forbase = Encoding.UTF8.GetBytes(u.ID.ToString());
            user += Convert.ToBase64String(forbase) + "\r\n";
            forbase = Encoding.UTF8.GetBytes(u.Tel);
            user += Convert.ToBase64String(forbase) + "\r\n";

            user += SplachScreen.Encrypt(u.Control.ToString()) + "\r\n";
            user += Convert.ToBase64String(forbase);
            File.WriteAllText("User", user);

            //string[] fileuser = File.ReadAllLines("User");

            //if (SplachScreen.Encrypt(suser) == fileuser[1] && SplachScreen.Encrypt(spass) == fileuser[2])
            //{
            //    retuser = new User();
            //    retuser.Name = Encoding.UTF8.GetString(Convert.FromBase64String(fileuser[0]));
            //    retuser.UserName = suser;
            //    retuser.Password = spass;
            //    retuser.Email = Encoding.UTF8.GetString(Convert.FromBase64String(fileuser[3]));
            //    retuser.ID = int.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(fileuser[4])));
            //    retuser.Tel = Encoding.UTF8.GetString(Convert.FromBase64String(fileuser[5]));
            //    for (int i = 0; i != 7; i++)
            //    {

            //        if (SplachScreen.Encrypt(((User.TypeControl)i).ToString()) == fileuser[6])
            //            retuser.Control = (User.TypeControl)i;
            //    }
            //}
        }
        public static void RemoveHandler(int id)
        {
            try
            {
                if (id == 0)
                    return;
                DBFunction.Execute("update peopledetails set chadchan=0 where chadchan=" + id);
                MessageBox.Show("הוסר בהצלחה");
            }
            catch
            {
                MessageBox.Show("אירעה שגיאה בהסרת השדכן מטפל");
            }
        }
       
        public static string ReturnPassword(string FullPass)
        {
            return FullPass.Split('^')[1];
        }
    }
}
