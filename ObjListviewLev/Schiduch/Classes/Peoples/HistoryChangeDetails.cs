using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Schiduch
{
    public class HistoryChangeDetails
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int IdUser { get; set; }
        public int IdPeople { get; set; }
        public string NameUser { get; set; }
        public string FieldName { get; set; }
        public string CurrentValue { get; set; }
        public string NewValue { get; set; }

        public HistoryChangeDetails(int idUser, int idPeople, string nameUser, string fieldName, string currentValue, string newValue)
        {
            IdUser = idUser;
            IdPeople = idPeople;
            NameUser = nameUser;
            FieldName = fieldName;
            CurrentValue = currentValue;
            NewValue = newValue;
            Date = DateTime.Now;
        }

        public HistoryChangeDetails()
        {

        }

        public void InserHistory()
        {
            SqlParameter[] prms = new SqlParameter[10];
            string sqlHistory, sql;
            sqlHistory = "INSERT INTO HistoryChangeDetails VALUES(" +
            BuildSql.InsertSql(out prms[1], this.Date) +
            BuildSql.InsertSql(out prms[2], this.IdUser) +
            BuildSql.InsertSql(out prms[3], this.IdPeople) +
            BuildSql.InsertSql(out prms[4], this.NameUser) +
            BuildSql.InsertSql(out prms[5], this.FieldName) +
            BuildSql.InsertSql(out prms[6], this.CurrentValue) +
            BuildSql.InsertSql(out prms[7], this.NewValue, true)
            + ");";
            sql = "BEGIN TRANSACTION " +
           sqlHistory +
           "COMMIT";
            DBFunction.Execute(sql, prms);
        }
        public static void InserListHistory(List<HistoryChangeDetails> h)
        {
            int n = h.Count * 7 + 1,i=0;
            SqlParameter[] prms = new SqlParameter[n];
            string sqlHistory="", sql;
            foreach (HistoryChangeDetails item in h)
            {
                sqlHistory += "INSERT INTO HistoryChangeDetails VALUES(" +
                           BuildSql.InsertSql(out prms[i++], item.Date) +
                           BuildSql.InsertSql(out prms[i++], item.IdUser) +
                           BuildSql.InsertSql(out prms[i++], item.IdPeople) +
                           BuildSql.InsertSql(out prms[i++], item.NameUser) +
                           BuildSql.InsertSql(out prms[i++], item.FieldName) +
                           BuildSql.InsertSql(out prms[i++], item.CurrentValue) +
                           BuildSql.InsertSql(out prms[i++], item.NewValue, true)
                           + "); ";
            }

            sql = "BEGIN TRANSACTION " +
           sqlHistory +
           "COMMIT";
            DBFunction.Execute(sql, prms);
        }
        public static SqlDataReader GetHistory(bool card, People p)
        {
            string sql = "select * from HistoryChangeDetails h where h.IdPeople=" + p.ID;
            return DBFunction.ExecuteReader(sql);
        }
        public static HistoryChangeDetails ReaderToHistoryChangeDetails(ref SqlDataReader reader)
        {
            HistoryChangeDetails h = new HistoryChangeDetails();
            h.Date = DateTime.Parse(reader["Date"].ToString());
            h.NameUser = (string)reader["NameUser"];
            h.FieldName = (string)reader["FieldName"];
            h.CurrentValue = (string)reader["CurrentValue"];
            h.NewValue = (string)reader["NewValue"];
            return h;
        }
    }
}
