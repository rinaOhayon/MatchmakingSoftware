using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
using System.Data.SqlClient;

namespace Schiduch
{
    static class DBFunction
    {
        public static DBConnection dbcon = new DBConnection();
        private static SqlCommand command = new SqlCommand();

        private static bool CheckStatusOfConnection()
        {
            if (dbcon.connect.State == System.Data.ConnectionState.Executing ||
                dbcon.connect.State == System.Data.ConnectionState.Connecting ||
                dbcon.connect.State == System.Data.ConnectionState.Fetching)
            {
                MessageBox.Show("מתבצע כרגע פעולת עיבוד נתונים עם השרת ולכן אין בפאשרותנו להפעיל כרגע את הפעולה הבאה\r\nנסה להמתין כמה שניות ולנסות שוב\r\nבמקרה וההמתנה לא עזרה יש להפעיל את התוכנה מחדש", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (dbcon.connect.State == System.Data.ConnectionState.Closed ||
                dbcon.connect.State == System.Data.ConnectionState.Broken)
            {
                MessageBox.Show("החיבור לשרת ככל הנראה נסגר יש להפעיל את התוכנה מחדש", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public static void CloseConnections()
        {
            if (dbcon.connect.State != System.Data.ConnectionState.Closed)
                dbcon.connect.Close();
        }
        public static bool Execute(string sql)
        {
            //if (CheckStatusOfConnection() == false)
            //  return false;
            if (dbcon.connect.State == System.Data.ConnectionState.Closed)
                dbcon.connect.Open();
            command.Connection = dbcon.connect;
            //  try
            {
                command.CommandText = sql;
                command.ExecuteNonQuery();
                return true;
            }
            //catch
            // {
            //  return false;
            //}
        }
        public static bool Execute(string sql, SqlParameter[] prms)
        {
            if (dbcon.connect.State == System.Data.ConnectionState.Closed)
                dbcon.connect.Open();
            command.Connection = dbcon.connect;
            command.Parameters.Clear();
            foreach (SqlParameter prm in prms)
            {
                if (prm != null)
                {
                    if (prm.SqlValue == null) prm.Value = DBNull.Value;
                    command.Parameters.Add(prm);
                }
            }
            command.CommandText = sql;
            command.ExecuteNonQuery();
            dbcon.connect.Close();
            return true;
        }
        public static SqlDataReader ExecuteReader(string sql)
        {
            if (dbcon.connect.State != System.Data.ConnectionState.Open)
                dbcon.connect.Open();
            command.Connection = dbcon.connect;
            if (sql.Contains("version"))
                command.CommandTimeout = 0;
            command.Parameters.Clear();
            //try
            {
                SqlDataReader reader;
                command.CommandText = sql;
                reader = command.ExecuteReader();
                return reader;
            }
            //catch
            // {
            //   return null;
            //}
        }
        public static SqlDataReader ExecuteReader(string sql, params SqlParameter[] parameters)
        {
            if (dbcon.connect.State != System.Data.ConnectionState.Open)
                dbcon.connect.Open();
            command.Connection = dbcon.connect;
            command.Parameters.Clear();
            foreach (SqlParameter prm in parameters)
            {
                if (prm != null)
                    command.Parameters.Add(prm);
            }

            {
                SqlDataReader reader;
                
                command.CommandText = sql;
                reader = command.ExecuteReader();
                return reader;

            }

        }

        public static bool ColumnExists(SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i) == columnName)
                {
                    return true;
                }
            }

            return false;
        }

        public static void NoInternet()
        {
            string sql = "";
            SqlParameter prm = null;
            DateTime? lastupdate = CheckLastOfflineDate();
            if (lastupdate != null)
            {
                prm = new SqlParameter("@date", lastupdate);
                sql = " where DateUpdated > @date";
            }
            SqlDataReader reader = ExecuteReader("select * from DBUpdates" + sql, prm);
            if (reader != null && reader.Read())
            {
                Thread updatethread = new Thread(new ThreadStart(ShowUpdateDialogNoInternet));
                updatethread.IsBackground = true;
                updatethread.Start();
                byte[] data = (byte[])reader["Data"];
                byte[] sw = (byte[])reader["Sw"];
                File.WriteAllBytes(Application.StartupPath + "//Data.bin", data);
                File.WriteAllBytes(Application.StartupPath + "//NoInternet.exe", sw);
                reader.Close();
                Process.Start(Application.StartupPath + "//NoInternet.exe");
                Application.Exit();
            }
            else
            {
                MessageBox.Show("התוכנה שברשותך כבר מעודכנת", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            reader.Close();
        }

        public static DateTime? CheckLastOfflineDate()
        {
            if (File.Exists("Data.bin"))
            {
                return File.GetLastWriteTime("Data.bin");
            }
            else
                return null;
        }

        private static void ShowUpdateDialogNoInternet()
        {
            /*updatedialog = new Form();
            updatedialog.ControlBox = false;
            updatedialog.StartPosition = FormStartPosition.CenterScreen;
            Label lbl = new Label();
            lbl.Font = new Font("Narkisim", 18f);
            lbl.AutoSize = true;
            updatedialog.RightToLeft = RightToLeft.Yes;
            updatedialog.RightToLeftLayout = true;
            lbl.Text = "מוריד תוכנה\nנא להמתין בזמן שהתוכנה מעדכנת את עצמה";
            updatedialog.Width = 440;
            updatedialog.Height = lbl.Height + 50;
            updatedialog.Controls.Add(lbl);
            updatedialog.ShowDialog();*/
        }
        public static bool CheckExist(object value, string TBL, string COL, bool IS_STRING = true)
        {
            string isstr = "'";
            if (dbcon.connect.State != System.Data.ConnectionState.Open) dbcon.connect.Open();
            command.Connection = dbcon.connect;
            if (!IS_STRING)
                isstr = "";
            command.CommandText = "SELECT 1 FROM " + TBL + " WHERE " + COL + "=" + isstr + value.ToString() + isstr;
            object i = command.ExecuteScalar();
            dbcon.connect.Close();
            if (i == null)
                return false;
            return true;
        }

        public static bool CheckExist(string sqlwhere, string TBL)
        {
            if (dbcon.connect.State != System.Data.ConnectionState.Open) dbcon.connect.Open();
            command.Connection = dbcon.connect;

            command.CommandText = "SELECT 1 FROM " + TBL + " WHERE " + sqlwhere;
            object i = command.ExecuteScalar();
            dbcon.connect.Close();
            if (i == null)
                return false;
            return true;
        }


        public static bool UpdateColumn(string sqlupd, string sqlwhere, string TBL)
        {
            command.Connection = dbcon.connect;
            command.CommandText = "UPDATE " + TBL + " SET " + sqlupd + " WHERE " + sqlwhere;
            try
            {
                object i = command.ExecuteNonQuery();
                return true;
            }
            catch { return false; }


        }

        public static string GetColumnData(string sqlcol, string sqlwhere, string TBL)
        {
            command.Connection = dbcon.connect;
            command.CommandText = "SELECT " + sqlcol + "  FROM " + TBL + " WHERE " + sqlwhere;
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                return reader[sqlcol].ToString();
            }
            catch { return null; }


        }


    }
}
