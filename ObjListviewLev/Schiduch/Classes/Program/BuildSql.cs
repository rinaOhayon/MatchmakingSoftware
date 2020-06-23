using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Schiduch
{
    public class BuildSql
    {

        public enum SqlKind { LIKE = 0, EQUAL = 1, AEQUAL, BEQUAL, BETWEEN, ABEQUAL };
        //, bool StartOrCondition = false, bool EndOrCondition = false
        public static string GetSql(out SqlParameter prm, object obj, string Col, SqlKind kind, bool AddAndAfter = true, object forbetween = null, bool AfterOr = false, string ManualAdd = null)
        {
            string Sql = "";
            string Aftersql = " ";
            if (AfterOr)
                Aftersql = " OR ";
            if (ManualAdd != null)
                Aftersql = ManualAdd;
            string Column = Col;
            string PrmName = "";
            if (obj.ToString() == "")
            {
                prm = null;
                return "";
            }

            /*switch (obj.GetType().Name)
            {
                case "String":
                    //ForType = "'";
                    break;
                case "DateTime":
                    ForType = "#";
                    break;
            }*/
            prm = null;

            PrmName = "P" + TempId;
            if (AddAndAfter)
                Aftersql = " AND ";
            switch (kind)
            {
                case SqlKind.EQUAL:
                    prm = new SqlParameter("@" + PrmName, obj.ToString());
                    Sql = Col + "=@" + PrmName + Aftersql;
                    break;
                case SqlKind.LIKE:
                    prm = new SqlParameter("@" + PrmName, "%" + obj.ToString() + "%");
                    Sql = Col + " LIKE @" + PrmName + Aftersql;
                    break;
                case SqlKind.BEQUAL:
                    prm = new SqlParameter("@" + PrmName, obj.ToString() + "*");
                    Sql = Col + "=@" + PrmName + Aftersql;
                    break;
                case SqlKind.AEQUAL:
                    prm = new SqlParameter("@" + PrmName, "*" + obj.ToString());
                    Sql = Col + "=@" + PrmName + Aftersql;
                    break;
                case SqlKind.ABEQUAL:
                    prm = new SqlParameter("@" + PrmName, "*" + obj.ToString() + "*");
                    Sql = Col + "=@" + PrmName + Aftersql;
                    break;
                case SqlKind.BETWEEN:
                    prm = new SqlParameter("@" + PrmName, obj.ToString());
                    Sql = Col + " BETWEEN @" + PrmName + " AND " + forbetween.ToString() + Aftersql;
                    break;
            }
            //if (StartOrCondition) Sql = " ( " + Sql;
            //if (EndOrCondition)
            //{
            //    Sql = Sql.Substring(Sql.IndexOf(Aftersql));
            //    Sql += " ) " + Aftersql;
            //}
            TempId++;
            return Sql;
        }
        public static int TempId;
        public static string InsertSql(out SqlParameter prm, object obj, bool Last = false)
        {
            string PrmName = "@P" + TempId;
            string NoLast = ",";
            if (Last)
                NoLast = "";
            if (obj.GetType().Name == "string" && (string)obj == "sample49")
            {
                prm = new SqlParameter(PrmName, System.Data.SqlDbType.Image);
                prm.Value = DBNull.Value;
            }
            else
                prm = new SqlParameter(PrmName, obj);
            TempId++;
            return PrmName + NoLast;
        }
        public static string CheckForLastAnd(ref string SQL)
        {
            if (string.IsNullOrEmpty(SQL)) return SQL;
            string g = SQL.Substring(SQL.Length - 4);
            string ret = SQL;
            if (SQL.Substring(SQL.Length - 4) == "AND ")
                ret = SQL.Substring(0, SQL.Length - 4);
            //   if (SQL.Substring(SQL.Length - 6) == "where ")
            //  ret = SQL.Substring(0, SQL.Length - 6);
            return ret;
        }
        public static string UpdateSql(out SqlParameter prm, object obj, string Col, bool Last = false)
        {
            string PrmName = "@P" + TempId;
            string NoLast = ",";
            if (Last)
                NoLast = "";
            prm = new SqlParameter(PrmName, obj);
            TempId++;
            return Col + "=" + PrmName + NoLast;
        }

    }

}
