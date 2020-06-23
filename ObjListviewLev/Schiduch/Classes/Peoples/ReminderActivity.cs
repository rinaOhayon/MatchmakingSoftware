using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Schiduch
{
    public class ReminderActivity
    {
        public int Id;
        public int IdActivity;
        public int IdUser;
        public DateTime Date;
        public bool Done = false;

        public static void InsertReminder(ShiduchActivity Activity)
        {
            SqlParameter[] prms = new SqlParameter[10];
            string sqlAct, sql;
            sqlAct = "insert into ReminderActivity values(" +
                BuildSql.InsertSql(out prms[0], Activity.Id) +
                BuildSql.InsertSql(out prms[1], Activity.reminder.IdUser) +
                BuildSql.InsertSql(out prms[2], Activity.reminder.Date) +
                BuildSql.InsertSql(out prms[3], false, true)
                + ");";
            sql = "BEGIN TRANSACTION " +
                sqlAct +
                "COMMIT";
            DBFunction.Execute(sql, prms);
        }
        public static void UpdateReminder(ShiduchActivity Activity)
        {
            SqlParameter[] prms = new SqlParameter[10];
            string sqlAct, sql;
            sqlAct = "update ReminderActivity SET " +
                BuildSql.UpdateSql(out prms[0], Activity.reminder.IdActivity, "IdActivity") +
                BuildSql.UpdateSql(out prms[1], Activity.reminder.IdUser, "IdUser") +
                BuildSql.UpdateSql(out prms[2], Activity.reminder.Date,"Date") +
                BuildSql.UpdateSql(out prms[3], Activity.reminder.Done,"Done", true)
                + " where Id="+Activity.reminder.Id+";";
            sql = "BEGIN TRANSACTION " +
                sqlAct +
                "COMMIT";
            DBFunction.Execute(sql, prms);
        }
    }
}