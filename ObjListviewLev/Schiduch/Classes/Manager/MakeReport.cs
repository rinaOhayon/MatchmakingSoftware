using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Collections;
using System.Resources;
using System.Reflection;
using System.Windows.Forms;
namespace Schiduch
{
    class MakeReport
    {
        Random rnd = new Random();
        Dictionary<Log.ActionType, KeyValueClass> sw = null;
        Dictionary<string, KeyValueClass> user = null;
        Dictionary<string, KeyValueClass> client = null;
        private ArrayList chadcan_action = new ArrayList();
        public enum ReportClientType { All, WhoCallMe, WhoIsOpenMe, MyDates };
        private string path = Application.StartupPath + "\\RepV1.rpt";
        private const string START_TABLE_ROW = "<tr>";
        private const string END_TABLE_ROW = "<tr>";
        private int count_openclient = 0;
        private int count_clientProposal = 0;
        private int count_clientDate = 0;
        private int count_clientDetails = 0;
        private int count_clientOther = 0;
        private int count_callnoanswer = 0;
        public enum ReportType { Client, User, SW, Dates };
        ReportType rep_type;
        private string clientlog_open = "<div ><table class='table table-bordered'><caption" +
            " style='text-align:right'>פתחו כרטיס לקוח</caption><thead><tr><th style='text-align:right;width:50%'" +
            ">שדכן</th><th style='text-align:right;width:50%'>בתאריך</th></tr></thead><tbody>";
        private string clientlog_callfail = "<div ><table class='table table-bordered'><caption" +
            " style='text-align:right'>התקשרו הציעו ולא יצא</caption><thead><tr><th style='text-align:right;width:50%'" +
            ">שדכן</th><th style='text-align:right;width:50%'>בתאריך</th></tr></thead><tbody>";
        private string clientlog_datecall = "<div ><table class='table table-bordered'><caption" +
            " style='text-align:right'>התקשרו הציעו</caption><thead><tr><th style='text-align:right;width:50%'" +
            ">שדכן</th><th style='text-align:right;width:50%'>בתאריך</th></tr></thead><tbody>";
        private string clientlog_callnoanswer = "<div ><table class='table table-bordered'><caption" +
            " style='text-align:right'>התקשרו ולא ענו</caption><thead><tr><th style='text-align:right;width:50%'" +
            ">שדכן</th><th style='text-align:right;width:50%'>בתאריך</th></tr></thead><tbody>";
        private string clientlog_telopen = "<div ><table class='table table-bordered'><caption" +
            " style='text-align:right'>בדקו מה הטלפון</caption><thead><tr><th style='text-align:right;width:50%'" +
            ">שדכן</th><th style='text-align:right;width:50%'>בתאריך</th></tr></thead><tbody>";
        private string clientlog_startdate = "<div ><table class='table table-bordered'><caption" +
            " style='text-align:right'>התקשרו לפגישה</caption><thead><tr><th style='text-align:right;width:50%'" +
            ">שדכן</th><th style='text-align:right;width:50%'>בתאריך</th></tr></thead><tbody>";
        private string clientAction_proposal = "<div ><table class='table table-bordered'><caption" +
           " style='text-align:right'>הצעות</caption><thead><tr><th style='text-align:right;width:20%'" +
           ">בתאריך</th><th style='text-align:right;width:20%'>שדכן</th>" +
            "<th style='text-align:right;width:20%'>סטטוס</th>" +
            "<th style='text-align:right;width:20%'>צד ב'</th>" +
            "<th style='text-align:right;width:20%'>תאריך תזכורת</th></tr></thead><tbody>";
        private string clientAction_date = "<div ><table class='table table-bordered'><caption" +
          " style='text-align:right'>פגישות</caption><thead><tr><th style='text-align:right;width:20%'" +
          ">בתאריך</th><th style='text-align:right;width:20%'>שדכן</th>" +
           "<th style='text-align:right;width:20%'>סטטוס</th>" +
           "<th style='text-align:right;width:20%'>צד ב'</th>" +
           "<th style='text-align:right;width:20%'>תאריך תזכורת</th></tr></thead><tbody>";
        private string clientAction_details = "<div ><table class='table table-bordered'><caption" +
          " style='text-align:right'>פרטים</caption><thead><tr><th style='text-align:right;width:20%'" +
          ">בתאריך</th><th style='text-align:right;width:20%'>שדכן</th>" +
           "<th style='text-align:right;width:20%'>סטטוס</th>" +
           "<th style='text-align:right;width:20%'>צד ב'</th>" +
           "<th style='text-align:right;width:20%'>תאריך תזכורת</th></tr></thead><tbody>";
        private string clientAction_other = "<div ><table class='table table-bordered'><caption" +
          " style='text-align:right'>אחר</caption><thead><tr><th style='text-align:right;width:20%'" +
          ">בתאריך</th><th style='text-align:right;width:20%'>שדכן</th>" +
           "<th style='text-align:right;width:20%'>סטטוס</th>" +
           "<th style='text-align:right;width:20%'>צד ב'</th>" +
           "<th style='text-align:right;width:20%'>תאריך תזכורת</th></tr></thead><tbody>";
        private string userAction_proposal = "<div ><table class='table table-bordered'><caption" +
          " style='text-align:right'>הצעות</caption><thead><tr><th style='text-align:right;width:20%'" +
          ">בתאריך</th><th style='text-align:right;width:20%'>לקוח</th>" +
           "<th style='text-align:right;width:20%'>סטטוס</th>" +
           "<th style='text-align:right;width:20%'>צד ב'</th>" +
           "<th style='text-align:right;width:20%'>תאריך תזכורת</th></tr></thead><tbody>";
        private string userAction_date = "<div ><table class='table table-bordered'><caption" +
          " style='text-align:right'>פגישות</caption><thead><tr><th style='text-align:right;width:20%'" +
          ">בתאריך</th><th style='text-align:right;width:20%'>לקוח</th>" +
           "<th style='text-align:right;width:20%'>סטטוס</th>" +
           "<th style='text-align:right;width:20%'>צד ב'</th>" +
           "<th style='text-align:right;width:20%'>תאריך תזכורת</th></tr></thead><tbody>";
        private string userAction_details = "<div ><table class='table table-bordered'><caption" +
          " style='text-align:right'>פרטים</caption><thead><tr><th style='text-align:right;width:20%'" +
          ">בתאריך</th><th style='text-align:right;width:20%'>לקוח</th>" +
           "<th style='text-align:right;width:20%'>סטטוס</th>" +
           "<th style='text-align:right;width:20%'>צד ב'</th>" +
           "<th style='text-align:right;width:20%'>תאריך תזכורת</th></tr></thead><tbody>";
        private string userAction_other = "<div ><table class='table table-bordered'><caption" +
          " style='text-align:right'>אחר</caption><thead><tr><th style='text-align:right;width:20%'" +
          ">בתאריך</th><th style='text-align:right;width:20%'>לקוח</th>" +
           "<th style='text-align:right;width:20%'>סטטוס</th>" +
           "<th style='text-align:right;width:20%'>צד ב'</th>" +
           "<th style='text-align:right;width:20%'>תאריך תזכורת</th></tr></thead><tbody>";
        public MakeReport(ReportType type)
        {
            rep_type = type;

        }
        public string CreateClientReport(string clientname, int userid, DateTime dt_start, DateTime dt_end)
        {

            string clienttableheader = global::Schiduch.Properties.Resources.ClientTableHeader;
            string html = "";
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("dt_start", dt_start);
            prms[1] = new SqlParameter("dt_end", dt_end);

            string moreinfo = "<b>לקוח:</b> " + clientname + ", <b>מזהה לקוח:</b> " + userid.ToString() + "</br>";
            string sqlLog = "select name,users.id as xid,info,action,userid,date,level from log LEFT JOIN USERS ON users.id=log.userid where log.info like '%" + userid.ToString() + "'";

            if (dt_start != null && dt_end != null)
            {
                sqlLog += " and date between @dt_start and @dt_end";
                moreinfo += " מתאריך " + dt_start.ToShortDateString() + " עד לתאריך " + dt_end.ToShortDateString();
            }
            sqlLog += " order BY log.ACTION ";

            html += CreateHtmlReport("לקוח", moreinfo);

            html += RegisterDateToReport(userid);
            SqlDataReader reader = DBFunction.ExecuteReader(sqlLog, prms);
            html += "<u><h2>פירוט</h2></u><div class='row' style='width:80%'>";
            while (reader.Read())
            {
                Log.ActionType a_type = (Log.ActionType)int.Parse(reader["ACTION"].ToString());

                switch (a_type)
                {
                    case Log.ActionType.ClientOpen:
                        clientlog_open += START_TABLE_ROW + CreateCol("", reader["name"]) + CreateCol(null, reader["date"]) + END_TABLE_ROW;
                        InsertUserAction(reader["name"].ToString(), (int)reader["userid"], ShiduchActivity.ActionType.openForms, ShiduchActivity.ActionStatus.completed);
                        count_openclient++;
                        break;
                }
            }
            reader.Close();
            reader = ShiduchActivity.GetActivities(false,null, false, false, true,dt_start,dt_end, userid);
            while (reader.Read())
            {
                ShiduchActivity.ActionType action = (ShiduchActivity.ActionType)int.Parse(reader["Action"].ToString());
                switch (action)
                {
                    case ShiduchActivity.ActionType.proposal:
                        clientAction_proposal += START_TABLE_ROW + CreateColAction(ref reader) + END_TABLE_ROW;
                        InsertUserAction(reader["Name"].ToString(), (int)reader["userID"], ShiduchActivity.ActionType.proposal, (ShiduchActivity.ActionStatus)(int)reader["Status"]);
                        count_clientProposal++;
                        break;
                    case ShiduchActivity.ActionType.date:
                        clientAction_date += START_TABLE_ROW + CreateColAction(ref reader) + END_TABLE_ROW;
                        InsertUserAction(reader["Name"].ToString(), (int)reader["userID"], ShiduchActivity.ActionType.date, (ShiduchActivity.ActionStatus)(int)reader["Status"]);
                        count_clientDate++;
                        break;
                    case ShiduchActivity.ActionType.details:
                        clientAction_details += START_TABLE_ROW + CreateColAction(ref reader) + END_TABLE_ROW;
                        InsertUserAction(reader["Name"].ToString(), (int)reader["userID"], ShiduchActivity.ActionType.details, (ShiduchActivity.ActionStatus)(int)reader["Status"]);
                        count_clientDetails++;
                        break;
                    case ShiduchActivity.ActionType.other:
                        clientAction_other += START_TABLE_ROW + CreateColAction(ref reader) + END_TABLE_ROW;
                        InsertUserAction(reader["Name"].ToString(), (int)reader["userID"], ShiduchActivity.ActionType.other, (ShiduchActivity.ActionStatus)(int)reader["Status"]);
                        count_clientOther++;
                        break;
                }
            }
            reader.Close();

            clientlog_open += "</tbody></table></div>";
            clientAction_proposal += "</tbody></table></div>";
            clientAction_date += "</tbody></table></div>";
            clientAction_details += "</tbody></table></div>";
            clientAction_other += "</tbody></table></div>";
            html += clientlog_open + clientAction_proposal + clientAction_date + clientAction_details + clientAction_other;
            html += "</div><hr>"; // end div of all info tables and create hr
            html += Schiduch.Properties.Resources.ClientSumAction;
            html += SumClientsTableList();
            html += "</tbody></table><hr><u><h2>סך הכל</h2></u>";
            html += SumClientsData();
            html += global::Schiduch.Properties.Resources.ReportEnd;
            using (TextWriter txtwrite = File.CreateText(path))
            {
                txtwrite.Write(html);
            }
            return path;
        }

        public string CreateClientReport(string clientname, int clientid, DateTime dt_start, DateTime dt_end, ReportClientType type)
        {
            //  string clienttableheader = global::Schiduch.Properties.Resources.ClientTableHeader;
            //  string html = "";
            // SqlParameter[] prms = new SqlParameter[2];
            //  prms[0] = new SqlParameter("dt_start", dt_start);
            // prms[1] = new SqlParameter("dt_end", dt_end);

            return null;
        }
        public string CreateUserReport(string username, int userid, DateTime dt_start, DateTime dt_end)
        {
            // string clienttableheader = global::Schiduch.Properties.Resources.ClientTableHeader;
            string html = "";
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("dt_start", dt_start);
            prms[1] = new SqlParameter("dt_end", dt_end);

            string moreinfo = "<b>שדכן:</b> " + username + ", <b>מזהה שדכן:</b> " + userid.ToString() + "</br>";

            string sqlLog = "select  firstname + ' ' + lastname as allname, peoples.ID ,action,userid,date " +
                           " from log JOIN peoples ON log.info not like '' and SUBSTRING(log.info, CHARINDEX('^',log.Info)+1,DATALENGTH(log.info) - 1)= Peoples.ID" +
                           " where UserId = " + userid + " and action = 2 ";
            if (dt_start != null && dt_end != null)
            {
                sqlLog += " and date between @dt_start and @dt_end";
                moreinfo += " מתאריך " + dt_start.ToShortDateString() + " עד לתאריך " + dt_end.ToShortDateString();
            }
            html += CreateHtmlReport("שדכן", moreinfo);

            html += RegisterDateToReport(userid);
            SqlDataReader reader = DBFunction.ExecuteReader(sqlLog, prms);
            html += "<u><h2>פירוט</h2></u><div class='row' style='width:80%'>";
            while (reader.Read())
            {
                Log.ActionType a_type = (Log.ActionType)int.Parse(reader["ACTION"].ToString());

                switch (a_type)
                {
                    case Log.ActionType.ClientOpen:
                        clientlog_open += START_TABLE_ROW + CreateCol("", reader["allname"]) + CreateCol(null, reader["date"]) + END_TABLE_ROW;
                        InsertUserAction(reader["allname"].ToString(), (int)reader["ID"], ShiduchActivity.ActionType.openForms, ShiduchActivity.ActionStatus.completed);
                        count_openclient++;
                        break;
                }
            }
            reader.Close();
            reader = ShiduchActivity.GetActivities(false,null, false, false, true,dt_start,dt_end, 0,userid);
            while (reader.Read())
            {
                ShiduchActivity.ActionType action = (ShiduchActivity.ActionType)int.Parse(reader["Action"].ToString());
                switch (action)
                {
                    case ShiduchActivity.ActionType.proposal:
                        userAction_proposal += START_TABLE_ROW + CreateColAction(ref reader) + END_TABLE_ROW;
                        InsertUserAction(reader["FullNameA"].ToString(), (int)reader["PeopleId"], ShiduchActivity.ActionType.proposal, (ShiduchActivity.ActionStatus)(int)reader["Status"]);
                        count_clientProposal++;
                        break;
                    case ShiduchActivity.ActionType.date:
                        userAction_date += START_TABLE_ROW + CreateColAction(ref reader) + END_TABLE_ROW;
                        InsertUserAction(reader["FullNameA"].ToString(), (int)reader["PeopleId"], ShiduchActivity.ActionType.date, (ShiduchActivity.ActionStatus)(int)reader["Status"]);
                        count_clientDate++;
                        break;
                    case ShiduchActivity.ActionType.details:
                        userAction_details += START_TABLE_ROW + CreateColAction(ref reader) + END_TABLE_ROW;
                        InsertUserAction(reader["FullNameA"].ToString(), (int)reader["PeopleId"], ShiduchActivity.ActionType.details, (ShiduchActivity.ActionStatus)(int)reader["Status"]);
                        count_clientDetails++;
                        break;
                    case ShiduchActivity.ActionType.other:
                        userAction_other += START_TABLE_ROW + CreateColAction(ref reader) + END_TABLE_ROW;
                        InsertUserAction(reader["FullNameA"].ToString(), (int)reader["PeopleId"], ShiduchActivity.ActionType.other, (ShiduchActivity.ActionStatus)(int)reader["Status"]);
                        count_clientOther++;
                        break;
                }
            }
            reader.Close();

            clientlog_open += "</tbody></table></div>";
            userAction_proposal += "</tbody></table></div>";
            userAction_date += "</tbody></table></div>";
            userAction_details += "</tbody></table></div>";
            userAction_other += "</tbody></table></div>";
            html += clientlog_open + userAction_proposal + userAction_date + userAction_details + userAction_other;
            html += "</div><hr>"; // end div of all info tables and create hr
            html += Schiduch.Properties.Resources.ClientSumAction.Replace("שדכן","לקוח");
            html += SumClientsTableList();
            html += "</tbody></table><hr><u><h2>סך הכל</h2></u>";
            html += SumClientsData();
            html += global::Schiduch.Properties.Resources.ReportEnd;
            using (TextWriter txtwrite = File.CreateText(path))
            {
                txtwrite.Write(html);
            }
            return path;
        }

        public string CreateDatesReport(DateTime dt_start, DateTime dt_end)
        {
            string html = "";
            int dates = 0;
            int dates_unpaid = 0;

            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("dt_start", dt_start);
            prms[1] = new SqlParameter("dt_end", dt_end);
            string moreinfo = "";
            bool paid = false;
            string nopay = "<span class='label label-danger'> לא שילם </span>";
            string lbl = "";
            string sql = "select date,info,action,userid,name,replace(left(info,CHARINDEX(N'^',info,0)),'^','') as clientname" +
                " from Log left join users on userid = users.id left" +
                " join RegisterInfo on relatedid = replace(substring(info, CHARINDEX('^', info), 6), '^', '')" +
                " where (action = 13 or action = 10) and replace(substring(info, CHARINDEX('^', info), 6), '^', '') > 2750 ";

            if (dt_start != null && dt_end != null)
            {
                sql += " and date between @dt_start and @dt_end";
                moreinfo = " מתאריך " + dt_start.ToShortDateString() + " עד לתאריך " + dt_end.ToShortDateString();
            }
            sql += " order by action,date desc";
            html += CreateHtmlReport("פגישות", moreinfo);
            html += Properties.Resources.ShadchanStartDatesHeader;
            SqlDataReader reader = DBFunction.ExecuteReader(sql, prms);
            while (reader.Read())
            {
                Log.ActionType a_type = (Log.ActionType)int.Parse(reader["ACTION"].ToString());
                paid = reader["paid"] as bool? ?? false;
                if (!paid) { lbl = nopay; dates_unpaid++; }
                else lbl = "";
                //switch (a_type)
                //{
                //    case Log.ActionType.GoodDateCall:
                //        html += START_TABLE_ROW + "<td class='success'>" + reader["clientname"] + " " + lbl + "</td>" +
                //          CreateCol("class='success'", reader["name"])
                //        + CreateCol("class='success'", reader["date"]) + END_TABLE_ROW;
                //        dates++;
                //        break;
                //    case Log.ActionType.StartDate:
                //        html += START_TABLE_ROW + "<td class='active'>" + reader["clientname"] + " " + lbl + "</td>" +
                //              CreateCol("class='active'", reader["name"])
                //            + CreateCol("class='active'", reader["date"]) + END_TABLE_ROW;
                //        dates++;
                //        break;
                //}
            }
            reader.Close();
            html += Schiduch.Properties.Resources.ShadchanStartDatesFooter;
            html += "<h1>הפגישות שנעשו בזמן הזה : <label class='label label-default'>" + dates.ToString() + "</label></h1>";
            html += "<h1>פגישות לכאלה שלא שילמו : <label class='label label-default'>" + dates_unpaid.ToString() + "</label></h1>";
            html += "</div></div></body></html>";
            using (TextWriter txtwrite = File.CreateText(path))
            {
                txtwrite.Write(html);
            }
            return path;
        }

        private string CreateHtmlReport(string title, string moreinfo)
        {
            string rep = "";
            rep += global::Schiduch.Properties.Resources.ReportHeader;
            rep += "<h1>דו\"ח " + title + "</h1><p>" + moreinfo + "</p></div>";
            return rep;
        }
        private string RegisterDateToReport(int id)
        {
            string sql = "";
            string rep = "";
            switch (rep_type)
            {
                case ReportType.Client:
                    sql = "select regdate from registerinfo where relatedid=" + id;
                    break;
                case ReportType.User:
                    sql = "select dateadded from users where id=" + id;
                    break;
            }
            if (string.IsNullOrEmpty(sql)) return rep;
            SqlDataReader reader = DBFunction.ExecuteReader(sql);
            if (reader.Read())
            {
                rep = "<h3>קיים במאגר מ  <span class='label label-default'>" + reader.GetDateTime(0).ToShortDateString() + "</span></h3><hr>";
            }
            reader.Close();
            return rep;
        }
        public Dictionary<string, KeyValueClass> ActionSum = new Dictionary<string, KeyValueClass>();
        private void InsertUserAction(string name, int id, ShiduchActivity.ActionType action, ShiduchActivity.ActionStatus status)
        {
            //חישוב סך הפעילויות לפי סטטוס פעילות
            //הרעיון הוא כמו במטריצה שיש ארבע שורות של פעילויות
            //ושלש עמודות של סטטוס
            //במקום זה עשיתי מערך בגודל  12 כשלחל פעילות יש 3 תאים לבטיפול הושלם ולא רלוונטי
            //אם אני רוצה לעשות הצעה בטיפול אז הצעה זה מזהה 0 ובטיפול זה גם מזהה 0
            //אז יוצא המקום 0 במערך
            //אם אני רוצה פגישה הושלם בהצלחה
            //פגישה זה מזהה 1 והושלם בהצלחה זה מזהה 1 
            //כדי לעבור לשלישיה הבאה אז 1*3 הגעתי לשלישיה של הפעילות פגישה ועוד 1 זה המקום 
            //של פגישה שהושלמה בהצלחה
            //חסכתי בזה את כל התנאים ואת השימוש בהרבה משתנים
            //מקום ה13 מיועד למספר הפעמים שנפתחה התוכנה
            int a = (int)action;
            int s = (int)status;
            int sum = a * 3 + s;
            //int t = ((int[])ActionSum[name].Value)[sum];
            if (!ActionSum.ContainsKey(id.ToString()))
                ActionSum.Add(id.ToString(), new KeyValueClass(name, new int[13]));

            if (action != ShiduchActivity.ActionType.openForms)
                ((int[])ActionSum[id.ToString()].Value)[sum] = ++(((int[])ActionSum[id.ToString()].Value)[sum]);
            else
                ((int[])ActionSum[id.ToString()].Value)[12] = ++(((int[])ActionSum[id.ToString()].Value)[12]);

        }

        private string SumClientsTableList()
        {
            int[] arr = new int[13];
            string ret = "";
            foreach (var item in ActionSum)
            {
                ret += "<tr><td>" + item.Value.Text + "</td>";
                ret += "<td>" + ((int[])item.Value.Value)[12];
                arr[12] += ((int[])item.Value.Value)[12];

                for (int i = 0; i < 12; i++)
                {
                    ret += "<td>" + ((int[])item.Value.Value)[i] + "</td>";
                    arr[i] += ((int[])item.Value.Value)[i];
                }
            }
            ret += "<tr><td><b>סך הכל</b></td><td>"+arr[12]+"</td>";

            for (int i = 0; i < 12; i++)
            {
                ret += "<td>" + arr[i] + "</td>";
            }
            return ret;
        }
        private string SumClientsData()
        {
            string ret = "<h4>פתחו את הכרטיס<b><i> " + count_openclient + "</i></b></h4>" +
                "<h4>הצעות<b><i> " + count_clientProposal + "</i></b></h4></h4>" +
                "<h4>פגישות  <b><i>" + count_clientDate + "</i></b></h4></h4>" +
                "<h4>בירור פרטים  <b><i>" + count_clientDetails + "</i></b></h4></h4>" +
                "<h4>אחר <b><i>" + count_clientOther + "</i></b></h4></h4>";

            return ret;
        }

        private string CreateCol(string tdstyle = "", params object[] data)
        {

            string temp = "";
            foreach (object obj in data)
            {
                temp += "<td + " + tdstyle + ">" + obj.ToString() + "</td>";
            }
            return temp;
        }
        private string CreateColAction(ref SqlDataReader reader, string tdstyle = "")
        {

            string temp = "";
            temp += "<td + " + tdstyle + ">" + reader["Date"].ToString() + "</td>";
            temp += "<td + " + tdstyle + ">" + reader["Name"].ToString() + "</td>";
            temp += "<td + " + tdstyle + ">" + ShiduchActivity.ConvertStatus((ShiduchActivity.ActionStatus)(int)reader["Status"]) + "</td>";
            temp += "<td + " + tdstyle + ">" + reader["FullNameB"].ToString() + "</td>";
            temp += "<td + " + tdstyle + ">" + reader["remindDate"].ToString() + "</td>";

            return temp;
        }

        public string CreateGeneralReport(DateTime? dtstart = null, DateTime? dtend = null)
        {
            int temp = 0;
            SqlParameter date1 = new SqlParameter("@dtstart", dtstart.Value);
            SqlParameter date2 = new SqlParameter("@dtend", dtend.Value);
            string sfilter = " CAST(CURRENT_TIMESTAMP AS DATE) = CAST(date AS DATE) ";
            if (dtstart != null && dtend != null)
            {
                sfilter = " (date between @dtstart and @dtend) ";
            }
            string uglysql = "REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(info, '0', '')," +
"'1', ''),'2', ''),'3', ''),'4', ''),'5', ''),'6', ''),'7', ''),'8', ''),'9', ''),'^', '') as clientname";
            SqlDataReader reader = DBFunction.ExecuteReader("select name,users.id as uid," + uglysql + ",action,userid,date,level from log LEFT JOIN USERS ON users.id=log.userid where " + sfilter + " and (action=" + (int)Log.ActionType.ClientOpen +
                " or action=" + (int)Log.ActionType.Login +
             //   " or action=" + (int)Log.ActionType.PhoneFormOpen + " or action=" + (int)Log.ActionType.GoodDateCall +
               // " or action=" + (int)Log.ActionType.StartDate + " or action=" + (int)Log.ActionType.FailCall +
               /* " or action=" + (int)Log.ActionType.FailDateCall */ ")", date1, date2);
            sw = new Dictionary<Log.ActionType, KeyValueClass>();
            sw.Add(Log.ActionType.ClientOpen, new KeyValueClass("", 0));
            sw.Add(Log.ActionType.Login, new KeyValueClass("", 0));
          //  sw.Add(Log.ActionType.PhoneFormOpen, new KeyValueClass("", 0));
          //  sw.Add(Log.ActionType.GoodDateCall, new KeyValueClass("", 0));
          //  sw.Add(Log.ActionType.StartDate, new KeyValueClass("", 0));
           // sw.Add(Log.ActionType.FailCall, new KeyValueClass("", 0));
          //  sw.Add(Log.ActionType.FailDateCall, new KeyValueClass("", 0));
            user = new Dictionary<string, KeyValueClass>();
            client = new Dictionary<string, KeyValueClass>();
            while (reader.Read())
            {
                switch ((Log.ActionType)reader["action"])
                {
                    case Log.ActionType.Login:
                        if (sw.ContainsKey(Log.ActionType.Login))
                        {
                            temp = (int)sw[Log.ActionType.Login].Value;
                            sw[Log.ActionType.Login].Value = ++temp;
                            sw[Log.ActionType.Login].Text += "<tr><td>" + reader["name"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                        }
                        // else      //////// need to create default value for all types of action
                        //  {

                        //     sw.Add(Log.ActionType.Login, new KeyValueClass("<tr><td>" + reader["name"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>",
                        //         1));
                        //  }
                        if (user.ContainsKey(reader["name"].ToString()))
                        {
                            TempData tobj = new TempData();
                            tobj = (TempData)user[reader["name"].ToString()].Value;
                            tobj.opensw++;
                            tobj.openswdata += "<tr><td>" + reader["name"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                            user[reader["name"].ToString()].Value = tobj;
                        }
                        else
                        {
                            TempData tobj = new TempData();
                            tobj.opensw++;
                            tobj.openswdata += "<tr><td>" + reader["name"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                            user.Add(reader["name"].ToString(), new KeyValueClass("",
                                tobj));
                        }
                        break;
                    //case Log.ActionType.PhoneFormOpen:
                    //    temp = (int)sw[Log.ActionType.PhoneFormOpen].Value;
                    //    sw[Log.ActionType.PhoneFormOpen].Value = ++temp;
                    //    sw[Log.ActionType.PhoneFormOpen].Text += "<tr><td>" + reader["name"].ToString() + "<td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                    //    if (user.ContainsKey(reader["name"].ToString()))
                    //    {
                    //        TempData tobj = new TempData();
                    //        tobj = (TempData)user[reader["name"].ToString()].Value;
                    //        tobj.opentel++;
                    //        tobj.openteldata += "<tr><td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                    //        user[reader["name"].ToString()].Value = tobj;
                    //    }
                    //    else
                    //    {
                    //        TempData tobj = new TempData();
                    //        tobj.opentel++;
                    //        tobj.openteldata += "<tr><td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                    //        user.Add(reader["name"].ToString(), new KeyValueClass("",
                    //            tobj));
                    //    }
                    //    break;
                    case Log.ActionType.ClientOpen:
                        temp = (int)sw[Log.ActionType.ClientOpen].Value;
                        sw[Log.ActionType.ClientOpen].Value = ++temp;
                        sw[Log.ActionType.ClientOpen].Text += "<tr><td>" + reader["name"].ToString() + "<td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                        if (user.ContainsKey(reader["name"].ToString()))
                        {
                            TempData tobj = new TempData();
                            tobj = (TempData)user[reader["name"].ToString()].Value;
                            tobj.openclient++;
                            tobj.openclientdata += "<tr><td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                            user[reader["name"].ToString()].Value = tobj;
                        }
                        else
                        {
                            TempData tobj = new TempData();
                            tobj.openclient++;
                            tobj.openclientdata += "<tr><td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                            user.Add(reader["name"].ToString(), new KeyValueClass("",
                                tobj));
                        }
                        break;
                    //case Log.ActionType.GoodDateCall:
                    //    temp = (int)sw[Log.ActionType.GoodDateCall].Value;
                    //    sw[Log.ActionType.GoodDateCall].Value = ++temp;
                    //    sw[Log.ActionType.GoodDateCall].Text += "<tr><td>" + reader["name"].ToString() + "<td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                    //    if (user.ContainsKey(reader["name"].ToString()))
                    //    {
                    //        TempData tobj = new TempData();
                    //        tobj = (TempData)user[reader["name"].ToString()].Value;
                    //        tobj.datecall++;
                    //        tobj.datecalldatat += "<tr><td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                    //        user[reader["name"].ToString()].Value = tobj;
                    //    }
                    //    else
                    //    {
                    //        TempData tobj = new TempData();
                    //        tobj.datecall++;
                    //        tobj.datecalldatat += "<tr><td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                    //        user.Add(reader["name"].ToString(), new KeyValueClass("",
                    //            tobj));
                    //    }
                    //    break;
                    //case Log.ActionType.StartDate:
                    //    temp = (int)sw[Log.ActionType.StartDate].Value;
                    //    sw[Log.ActionType.StartDate].Value = ++temp;
                    //    sw[Log.ActionType.StartDate].Text += "<tr><td>" + reader["name"].ToString() + "<td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                    //    if (user.ContainsKey(reader["name"].ToString()))
                    //    {
                    //        TempData tobj = new TempData();
                    //        tobj = (TempData)user[reader["name"].ToString()].Value;
                    //        tobj.startdate++;
                    //        tobj.startdatedata += "<tr><td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                    //        user[reader["name"].ToString()].Value = tobj;
                    //    }
                    //    else
                    //    {
                    //        TempData tobj = new TempData();
                    //        tobj.startdate++;
                    //        tobj.startdatedata += "<tr><td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                    //        user.Add(reader["name"].ToString(), new KeyValueClass("",
                    //            tobj));
                    //    }
                    //    break;
                    //case Log.ActionType.FailDateCall:
                    //    temp = (int)sw[Log.ActionType.StartDate].Value;
                    //    sw[Log.ActionType.FailDateCall].Value = ++temp;
                    //    sw[Log.ActionType.FailDateCall].Text += "<tr><td>" + reader["name"].ToString() + "<td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                    //    if (user.ContainsKey(reader["name"].ToString()))
                    //    {
                    //        TempData tobj = new TempData();
                    //        tobj = (TempData)user[reader["name"].ToString()].Value;
                    //        tobj.callfail++;
                    //        tobj.callfaildata += "<tr><td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                    //        user[reader["name"].ToString()].Value = tobj;
                    //    }
                    //    else
                    //    {
                    //        TempData tobj = new TempData();
                    //        tobj.callfail++;
                    //        tobj.callfaildata += "<tr><td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                    //        user.Add(reader["name"].ToString(), new KeyValueClass("",
                    //            tobj));
                    //    }
                    //    break;
                    //case Log.ActionType.FailCall:
                    //    temp = (int)sw[Log.ActionType.StartDate].Value;
                    //    sw[Log.ActionType.FailDateCall].Value = ++temp;
                    //    sw[Log.ActionType.FailDateCall].Text += "<tr><td>" + reader["name"].ToString() + "<td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                    //    if (user.ContainsKey(reader["name"].ToString()))
                    //    {
                    //        TempData tobj = new TempData();
                    //        tobj = (TempData)user[reader["name"].ToString()].Value;
                    //        tobj.callnoanswer++;
                    //        tobj.callnoanswerdata += "<tr><td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                    //        user[reader["name"].ToString()].Value = tobj;
                    //    }
                    //    else
                    //    {
                    //        TempData tobj = new TempData();
                    //        tobj.callnoanswer++;
                    //        tobj.callnoanswerdata += "<tr><td>" + reader["clientname"].ToString() + "</td>" + "<td>" + reader["date"].ToString() + "</td></tr>";
                    //        user.Add(reader["name"].ToString(), new KeyValueClass("",
                    //            tobj));
                    //    }
                    //    break;

                }
            }
            reader.Close();

            return AnalyzeData();
        }
        private string CreateModal(string id, string data, int type)
        {
            string txt = "<div  id='" + id + "' class='modal fade' role='dialog'><div class='modal-dialog'><div class='modal-content'><div class='modal-header'>";
            txt += " <button style='float:left' type='button' class='close' data-dismiss='modal'>&times;</button><h4 class='modal-title'>פירוט</h4>";
            txt += "</div><div class='modal-body'><div><table class='table table-striped' dir='rtl'><tbody>";
            if (type == 0)
                txt += "<th align='right'>שדכן</th><th>ב</th>";
            else if (type == 1)
                txt += "<th align='right'>שדכן</th><th>ללקוח</th><th>ב</th>";
            else if (type == 2)
                txt += "<th align='right'>לקוח</th><th>ב</th>";
            txt += data;
            txt += "</tbody> </table></div> </div><div class='modal-footer'><button type='button' class='btn btn-default' data-dismiss='modal'>סגור</button></div></div></div></div>";
            return txt;

        }
        private string AnalyzeData()
        {
            int temp = rnd.Next(10000);
            string dialogs = "";
            string txt = Schiduch.Properties.Resources.AnotherReportHeader;
            txt += "<style>th{text-align:right}caption{    text-align: right;}</style>";
            txt += "<body dir=rtl><div class='container'><div class='col-sm-4'><table class='table table-striped' dir='rtl'><tbody><caption><h3>דוח תוכנה</h3></caption>";
            txt += "<tr data-toggle='modal' data-target='#" + temp + "'>" + "<th align='right'> מספר הפעמים שנפתחה התוכנה</th>" + "<td>" +
            ((int)sw[Log.ActionType.Login].Value).ToString() + "</td></tr>";
            dialogs += CreateModal(temp.ToString(), sw[Log.ActionType.Login].Text, 0);

            //temp = rnd.Next(10000);
            //txt += "<tr data-toggle='modal' data-target='#" + temp + "'>" + "<th align='right'>מספר הפעמים שנבדקו טלפונים</th>" + "<td>" +
            //((int)sw[Log.ActionType.PhoneFormOpen].Value).ToString() + "</td></tr>";
            //dialogs += CreateModal(temp.ToString(), sw[Log.ActionType.PhoneFormOpen].Text, 1);

            temp = rnd.Next(10000);
            txt += "<tr data-toggle='modal' data-target='#" + temp + "'>" + "<th align='right'> מספר הפעמים שנפתחו כרטיסי לקוח</th>" + "<td>" +
            ((int)sw[Log.ActionType.ClientOpen].Value).ToString() + "</td></tr>";
            dialogs += CreateModal(temp.ToString(), sw[Log.ActionType.ClientOpen].Text, 1);

            //temp = rnd.Next(10000);
            //txt += "<tr data-toggle='modal' data-target='#" + temp + "'>" + "<th align='right'> מספר הפעמים שהתקשרו והציעו הצעה</th>" + "<td>" +
            //((int)sw[Log.ActionType.GoodDateCall].Value).ToString() + "</td></tr>";
            //dialogs += CreateModal(temp.ToString(), sw[Log.ActionType.GoodDateCall].Text, 1);

            //temp = rnd.Next(10000);
            //txt += "<tr data-toggle='modal' data-target='#" + temp + "'>" + "<th align='right'> מספר הפעמים שהוציאו לפגישה</th>" + "<td>" +
            //((int)sw[Log.ActionType.StartDate].Value).ToString() + "</td></tr>";
            //dialogs += CreateModal(temp.ToString(), sw[Log.ActionType.StartDate].Text, 1);

            //temp = rnd.Next(10000);
            //txt += "<tr data-toggle='modal' data-target='#" + temp + "'>" + "<th align='right'> מספר הפעמים שהתקשרו הציעו ולא יצא</th>" + "<td>" +
            //((int)sw[Log.ActionType.FailDateCall].Value).ToString() + "</td></tr>";
            //dialogs += CreateModal(temp.ToString(), sw[Log.ActionType.FailDateCall].Text, 1);

            //temp = rnd.Next(10000);
            //txt += "<tr data-toggle='modal' data-target='#" + temp + "'>" + "<th align='right'> מספר הפעמים שהתקשרו ולא ענו</th>" + "<td>" +
            //((int)sw[Log.ActionType.FailCall].Value).ToString() + "</td></tr>";
            //dialogs += CreateModal(temp.ToString(), sw[Log.ActionType.FailCall].Text, 1);

            txt += "</tbody></table></div>"; // end of table sw report

            txt += "<div class='col-sm-6'>"; // start of user report
            txt += "<table class='table table-striped' dir='rtl'><tbody><caption><h3>דו'ח שדכנים</h3></caption><th align='right'>השדכן</th><th>פתח תוכנה</th><th>פתח כרטיס לקוח</th><th>בדק מה הטלפון</th>";
            txt += "<th>התחיל פגישות</th><th>הציע ויצא</th>";

            foreach (string it in user.Keys)
            {
                temp = rnd.Next(10000);
                txt += "<tr><td>" +
                it + "</td><td data-toggle='modal' data-target='#a" + temp + "'>" +
                ((TempData)user[it].Value).opensw.ToString() + "</td><td data-toggle='modal' data-target='#b" + temp + "'>" +
                ((TempData)user[it].Value).openclient.ToString() + "</td><td data-toggle='modal' data-target='#c" + temp + "'>" +
                ((TempData)user[it].Value).opentel.ToString() + "</td><td data-toggle='modal' data-target='#d" + temp + "'>" +
                ((TempData)user[it].Value).startdate.ToString() + "</td><td data-toggle='modal' data-target='#e" + temp + "'>" +
                ((TempData)user[it].Value).datecall.ToString() + "</td>";



                dialogs += CreateModal("a" + temp.ToString(), ((TempData)user[it].Value).openswdata, 2);
                dialogs += CreateModal("b" + temp.ToString(), ((TempData)user[it].Value).openclientdata, 2);
                dialogs += CreateModal("c" + temp.ToString(), ((TempData)user[it].Value).openteldata, 2);
                dialogs += CreateModal("d" + temp.ToString(), ((TempData)user[it].Value).startdatedata, 2);
                dialogs += CreateModal("e" + temp.ToString(), ((TempData)user[it].Value).datecalldatat, 2);

            }


            txt += "</tbody></table></div>"; // end of table user report

            txt += dialogs;
            txt += Schiduch.Properties.Resources.ReportEnd;
            return txt;
        }
        private string CreateTH(string thstyle = "", params object[] data)
        {

            string temp = "";
            foreach (object obj in data)
            {
                temp += "<th + " + thstyle + ">" + obj.ToString() + "</th>";
            }
            return temp;
        }
    }

    class TempData
    {
        public string name;
        public int id = 0;
        public int opensw = 0;
        public string openswdata = "";
        public string openteldata = "";
        public string openclientdata = "";
        public int openclient = 0;
        public int opentel = 0;
        public int datecall = 0;
        public string datecalldatat = "";
        public int callfail = 0;
        public string callfaildata = "";
        public int callnoanswer = 0;
        public string callnoanswerdata = "";
        public int startdate = 0;
        public string startdatedata = "";
    }
    class TempDataAction
    {
        public string name;
        public int id = 0;
        public int opensw = 0;
        public string openswdata = "";

        public string openclientdata = "";
        public int openclient = 0;


        public int proposalInCare = 0;
        public int proposalComplete = 0;
        public int proposalNoRelevant = 0;

        public int dateInCare = 0;
        public int dateComplete = 0;
        public int dateNoRelevant = 0;

        public int detailsInCare = 0;
        public int detailsComplete = 0;
        public int detailsNoRelevant = 0;

        public int otherInCare = 0;
        public int otherComplete = 0;
        public int otherNoRelevant = 0;

    }


}
