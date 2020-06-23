using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;
using System.Reflection;
using Schiduch.Classes.Program;

namespace Schiduch
{
    class DBConnection
    {
        public SqlConnection connect;

        public DBConnection()
        {
            //////////חיבור לאלי כהן
            /*ConnectionString = "Server=tcp:rmc42eiyzk.database.windows.net,1433;
             * Database=levone;User ID=elichen@rmc42eiyzk;Encrypt=False;Password=ELI47213eli;
             * Connection Timeout=30;"*/
            /*
            /*
             //// חיבור ללב אחד
            "Server=tcp:rmc42eiyzk.database.windows.net,1433;" +
                "Database=shiduch;User ID=levone@rmc42eiyzk;Encrypt=True;Password=NeF987&#*vtJ9%5S;" +
                "Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
          LevOne1@Rina     levone1@0356  */
           // Farm Server IP: 192.168.30.11
            string con = "";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+ "\\My_Computer_JBLU-900599.txt";
            if (File.Exists(path))
            {
                con = "Server=192.168.30.11\\SQLEXPRESS;Database=MekimiTest; User ID=dev;Password=M7pb5FQefesk7xpM;";
                // GLOBALVARS.IsDeveloper = true;
                //SO76mAtJ2z
            }
            else
                con = "Server=192.168.30.11\\SQLEXPRESS;Database=Mekimi; User ID=mekimi;Password=wHKej7AMekZYveKQ;";
            string con3 = "Server=192.168.30.11\\SQLEXPRESS;Database=levOneDB;  Encrypt=False; User ID=LevOne;Password=lo1234;Connection Timeout=30";
            string con2 = @"Server=tcp:azurelevone1server.database.windows.net,1433;
             Database=LevOne; User ID=LevOne1@9876@azurelevone1server; Encrypt = False; Password=levone1@6789;
            Connection Timeout = 60; ";
            string connectEncryptSharat = "U2VydmVyPXRjcDpTSEFSQVQsNDkxNzI7RGF0YWJhc2U9bGV2T25lRGV2O1VzZXIgSUQ9TGV2T25lO1Bhc3N3b3JkPWxvMTIzNDtDb25uZWN0aW9uIFRpbWVvdXQ9MzA7RW5jcnlwdD1UcnVlO1RydXN0U2VydmVyQ2VydGlmaWNhdGU9VHJ1ZQ==";
            // string con = "U2VydmVyPXRjcDpybWM0MmVpeXprLmRhdGFiYXNlLndpbmRvd3MubmV0LDE0MzM7RGF0YWJhc2U9bGV2b25lO1VzZXIgSUQ9ZWxpY2hlbkBybWM0MmVpeXprO0VuY3J5cHQ9RmFsc2U7UGFzc3dvcmQ9RUxJNDcyMTNlbGk7Q29ubmVjdGlvbiBUaW1lb3V0PTMwOw==";
            // string con = "U2VydmVyPXRjcDpybWM0MmVpeXprLmRhdGFiYXNlLndpbmRvd3MubmV0LDE0MzM7RGF0YWJhc2U9c2hpZHVjaDtVc2VyIElEPWVsaWNoZW5Acm1jNDJlaXl6aztFbmNyeXB0PUZhbHNlO1Bhc3N3b3JkPUVMSTQ3MjEzZWxpO0Nvbm5lY3Rpb24gVGltZW91dD0zMDs=";
            ///========== המרה של הקידוד
            // connect = new SqlConnection(Encoding.UTF8.GetString(Convert.FromBase64String(con)));
            connect = new SqlConnection(con);
            connect.Open();
            //connect.StateChange += Connect_StateChange;
            connect.Close();
            //string strHostName = "";
            //strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            //IPAddress[] addr = ipEntry.AddressList;
            //string dddd= addr[addr.Length - 1].ToString();

        }

        private void Connect_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            //if (e.CurrentState == System.Data.ConnectionState.Closed)
            //    ConnectAgain();
        }


        public static void ConnectAgain()
        {

            if (MessageBox.Show("החיבור נותק, האם תרצה שהתוכנה תנסה להתחבר שוב", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DBFunction.dbcon = null;
                DBFunction.dbcon = new DBConnection();
            }
        }




    }
}
