using Schiduch.Classes.Program;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace Schiduch
{
    public class ShiduchActivity
    {
        public enum ActionType { proposal, date, details, other, ActivityNoSave, openForms, reception, update, delete, recycling };
        public enum ActionStatus { inCare = 0, completed, noRelevant };
        //משתני המחלקה
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int PeopleId { get; set; }
        public int Action { get; set; }
        public int IdSideB { get; set; } = 0;
        public int Status { get; set; } = 0;
        public string NotesSummary { get; set; } = "";
        public bool HideDelete { get; set; } = false;
        public string UserName { get; set; }
        public ReminderActivity reminder { get; set; }
        //לנוחות משתמש
        public string FullNameA { get; set; }
        public string FullNameB { get; set; }
        public string ActionConvert { get; set; }
        public string StatusConvert { get; set; }
        public string SqlString { get; set; }
        public bool openReminder { get; set; }
        public bool SaveOpenReminder { get; set; }

        public ShiduchActivity()
        {
            reminder = new ReminderActivity();
        }
        public static int insertActivity(ShiduchActivity Activity)
        {
            SqlParameter[] prms = new SqlParameter[10];
            string sqlAct, sql;

            sqlAct = "insert into ShiduchActivity values(" +
                BuildSql.InsertSql(out prms[0], Activity.Date) +
                BuildSql.InsertSql(out prms[1], Activity.UserId) +
                BuildSql.InsertSql(out prms[2], Activity.PeopleId) +
                BuildSql.InsertSql(out prms[3], Activity.Action) +
                BuildSql.InsertSql(out prms[4], Activity.IdSideB) +
                BuildSql.InsertSql(out prms[5], Activity.Status) +
                BuildSql.InsertSql(out prms[6], Activity.NotesSummary) +
                BuildSql.InsertSql(out prms[8], Activity.HideDelete, true)
                + ");";
            prms[7] = new SqlParameter("@D", SqlDbType.Int);
            prms[7].Direction = ParameterDirection.Output;
            sql = "BEGIN TRANSACTION " +
                sqlAct +
                "SELECT @D = scope_identity();" +
                "COMMIT";

            DBFunction.Execute(sql, prms);
            int ID = 0;
            if (prms[7].Value != DBNull.Value)
                ID = Convert.ToInt32(prms[7].Value);
            return ID;
        }
        public static bool updateActivity(ShiduchActivity Activity)
        {
            SqlParameter[] prms = new SqlParameter[10];
            string sqlAct, sql;

            sqlAct = "update ShiduchActivity SET " +
                BuildSql.UpdateSql(out prms[0], Activity.Date, "Date") +
                BuildSql.UpdateSql(out prms[1], Activity.UserId, "UserId") +
                BuildSql.UpdateSql(out prms[2], Activity.PeopleId, "PeopleId") +
                BuildSql.UpdateSql(out prms[3], Activity.Action, "Action") +
                BuildSql.UpdateSql(out prms[4], Activity.IdSideB, "IdSideB") +
                BuildSql.UpdateSql(out prms[5], Activity.Status, "Status") +
                BuildSql.UpdateSql(out prms[6], Activity.NotesSummary, "NotesSummary") +
                BuildSql.UpdateSql(out prms[7], Activity.HideDelete, "HideDelete", true)
                + " where Id=" + Activity.Id + ";";
            sql = "BEGIN TRANSACTION " +
                sqlAct +
                "COMMIT";

            return DBFunction.Execute(sql, prms);

        }

        public static SqlDataReader GetActivities(bool my, People p = null, bool all = false, bool inCare = false, bool allForManagerReport = false,
            DateTime date1 = default(DateTime), DateTime date2 = default(DateTime), int peopleId = 0,
            int userId = 0, bool MyAllForDiary = false, int action = -1, int status = -1, bool DiaryManager = false)
        {
            List<ShiduchActivity> list = new List<ShiduchActivity>();
            ShiduchActivity item;
            if (p == null) p = new People();
            //פעילויות שלי
            string select, from, where = "", sql, orderBy = "", sActionNUmber = " s.Action<4 ";
            select = "select s.*,p2.FirstName+' '+p2.LastName as FullNameB ";

            from = "from ShiduchActivity s left join Peoples p2 on s.IdSideB=p2.ID ";
            where = " where s.PeopleId=" + p.ID;
            if (my)//פעיליות שהשדכן עשה לכרטיס זה
            {
                if (GLOBALVARS.MyUser.Control == User.TypeControl.Admin || GLOBALVARS.MyUser.Control == User.TypeControl.Manger)
                    where += " and " + sActionNUmber;
                else
                    where += " and s.UserId=" + GLOBALVARS.MyUser.ID + " and s.action<4";
            }
            if (all)//פעילויות של כלל השדכנים לכרטיס זה לא כולל פגישה
            {
                select += ",u.Name ";
                from += " inner join users u on u.id =s.userid ";
                if (GLOBALVARS.MyUser.Control != User.TypeControl.Admin && GLOBALVARS.MyUser.Control != User.TypeControl.Manger)
                    where += " and s.Action in(0,2,3,5)";
            }
            if (inCare)
            {
                select += " ,p.FirstName+' '+p.LastName as FullNameA,r.Date as remindDate ";
                from += " left join Peoples p on s.peopleid = p.id inner join ReminderActivity r on r.IdActivity=s.Id ";
                where = " where s.UserId=" + GLOBALVARS.MyUser.ID + " and s.action<4 and s.Status=0 and r.Done=0 and r.Date<=getdate() ";
            }
            if (allForManagerReport)
            {
                if (DiaryManager)
                {
                    sActionNUmber = "s.action<5";
                    if (action > 0)//-1 לא נבחר 0 הכל
                       sActionNUmber="s.Action=" + (action - 1);
                      //אם זה מנהל שבודק פעילות של שדכן מסוים אז הוא יכול לראות את הכל
                    if (status > 0)//אם לא בחר סטטוס מסוים אז מביאים את הכל
                        where += " and s.Status=" + (status - 1);
                }
                select += " ,p.FirstName+' '+p.LastName as FullNameA,u.Name,u.id as userID," +
                    "r.Date as remindDate,r.id as remindID ,r.IdUser as remindIdUser ,Done  ";
                from += " left join Peoples p on s.peopleid = p.id inner join users u on u.id =s.userid" +
                    " inner join ReminderActivity r on r.IdActivity=s.Id ";
                where = " where " + sActionNUmber + " and s.Date between'" + date1.ToString("yyyy-MM-dd h:mm tt") +
                    "' and '" + date2.ToString("yyyy-MM-dd h:mm tt") + "' ";
                if (peopleId != 0)
                    where += " and s.PeopleId=" + peopleId;
                if (userId != 0)
                    where += " and s.UserId=" + userId;
                orderBy = " order by s.Action,s.Status ";
            }
            if (MyAllForDiary)
            {
                select += ",r.Date as remindDate,r.Done,p.FirstName+' '+p.LastName as FullNameA,u.Name ";
                from += " left join Peoples p on s.peopleid = p.id inner join ReminderActivity r on" +
                    " r.IdActivity=s.Id inner join users u on u.id =s.userid ";

                where = " where s.UserId=" + userId + " and s.Date between'" + date1.ToString("yyyy-MM-dd h:mm tt")
                    + "' and '" + date2.ToString("yyyy-MM-dd h:mm tt") + "' ";
                if (action > 0)//-1 לא נבחר 0 הכל
                    where += " and s.Action=" + (action - 1);
                else if (DiaryManager)//אם זה מנהל שבודק פעילות של שדכן מסוים אז הוא יכול לראות את הכל
                    where += " and s.action<5";
                else
                    where += " and s.action<4";
                if (status > 0)//אם לא בחר סטטוס מסוים אז מביאים את הכל
                    where += " and s.Status=" + (status - 1);


            }
            sql = select + from + where + orderBy;
            SqlDataReader reader = DBFunction.ExecuteReader(sql);
            return reader;
        }

        public static SqlDataReader GetDiaryActiviy()
        {
            SqlDataReader reader = null;

            return reader;
        }
        public static ShiduchActivity ReadById(int id)
        {
            string sql = "select s.*,r.id as remindID, r.Date as remindDate,r.Done,r.IdUser " +
                "as remindIdUser from ShiduchActivity s " +
                "inner join ReminderActivity r on s.Id=r.IdActivity where s.Id=" + id;
            SqlDataReader reader = DBFunction.ExecuteReader(sql);
            ShiduchActivity activity = new ShiduchActivity();
            if (reader.Read())
                readerToShiduchActivity(ref reader, ref activity);
            reader.Close();
            return activity;
        }
        public static void readerToShiduchActivity(ref SqlDataReader reader, ref ShiduchActivity shiduch)
        {
            shiduch.Id = int.Parse(reader["Id"].ToString());
            shiduch.UserId = int.Parse(reader["UserId"].ToString());
            shiduch.PeopleId = int.Parse(reader["PeopleId"].ToString());
            shiduch.IdSideB = int.Parse(reader["IdSideB"].ToString());
            shiduch.Action = int.Parse(reader["Action"].ToString());
            shiduch.Status = int.Parse(reader["Status"].ToString());
            shiduch.Date = DateTime.Parse(reader["Date"].ToString());
            shiduch.NotesSummary = (string)reader["NotesSummary"];
            shiduch.HideDelete = (bool)reader["HideDelete"];
            shiduch.reminder.Date = DateTime.Parse(reader["remindDate"].ToString());
            shiduch.reminder.Done = (bool)reader["Done"];
            shiduch.reminder.IdActivity = shiduch.Id;
            try
            {
                if (DBFunction.ColumnExists(reader, "remindID"))
                {
                    shiduch.reminder.Id = int.Parse(reader["remindID"].ToString());
                    shiduch.reminder.IdUser = int.Parse(reader["remindIdUser"].ToString());
                }
            }
            catch { }

        }
        public static string ConvertAction(ActionType a, SqlDataReader reader = null)
        {
            string action = "";
            switch (a)
            {
                case ActionType.date:
                    action = "פגישה";
                    break;
                case ActionType.proposal:
                    action = "הצעה";
                    break;
                case ActionType.details:
                    action = "בירור פרטים";
                    break;
                case ActionType.reception:
                    action = "קליטה";
                    break;
                case ActionType.update:
                    action = "עדכון";
                    break;
                case ActionType.openForms:
                    action = "פתיחת כרטיס לקוח";
                    break;
                case ActionType.delete:
                    action = "כרטיס נמחק";
                    break;
                case ActionType.recycling:
                    action = "כרטיס שוחזר";
                    break;
                case ActionType.ActivityNoSave:
                    action = "פעילות לא נשמרה";
                    break;
                default:
                    action = "אחר - " +
                        reader["NotesSummary"].ToString().Substring(0, reader["NotesSummary"].ToString().IndexOf('^'));
                    break;

            }
            return action;
        }
        public static string ConvertStatus(ActionStatus a)
        {
            string status = "";
            switch (a)
            {
                case ActionStatus.completed:
                    status = "הושלם בהצלחה";
                    break;
                case ActionStatus.inCare:
                    status = "בטיפול";
                    break;
                default:
                    status = "לא רלוונטי";
                    break;
            }
            return status;
        }
        ShiduchActivity s;
        public void openShiduchActivityForm(ListView lst, People MyPeople = null)
        {
            //עדכון פעילות
            if (lst.SelectedItems.Count <= 0)
                return;
            if (lst.Name == "lstReminder")
                openReminder = true;
            int idActivity = int.Parse(lst.SelectedItems[0].Tag.ToString());
            // int idSideB = int.Parse(lstMyActivity.SelectedItems[0].SubItems[5].Text);
            s = ReadById(idActivity);
            //להביא את ההערות של הכרטיס השני
            string notesSide = removeFromString(s.NotesSummary) + "\r\n" +
                "=====צד ב'=====" + Environment.NewLine;
            string sql = "select NotesSummary from ShiduchActivity s " +
                        "where s.UserId=" + s.UserId + " and PeopleId=" +
                        s.IdSideB + " and IdSideB=" + s.PeopleId + " and Action=" + s.Action +
                       " and abs(DATEDIFF(day,s.Date,'" + s.Date.ToString("yyyy-MM-dd h:mm tt") + "'))" +
                       " between 0 and 15";
            SqlDataReader reader = DBFunction.ExecuteReader(sql);
            if (reader.Read())
                notesSide += removeFromString(reader["NotesSummary"].ToString());
            reader.Close();
            s.NotesSummary = notesSide;
            if (MyPeople == null)
            {
                MyPeople = new People();
                SqlDataReader reader1 = People.ReadById(s.PeopleId);
                if (reader1.Read())
                    PeopleManipulations.ReaderToPeople(ref MyPeople, ref reader1);
                reader1.Close();
            }
            ShiduchActivityForm sForm;
            if (openReminder)//אם נפתח דרך התזכורות
                sForm = new ShiduchActivityForm(s, MyPeople, false, true, false, true);
            else
                sForm = new ShiduchActivityForm(s, MyPeople, false, true, false);
            sForm.Show();
            sForm.FormClosed += SForm_FormClosed;
        }
        public void SForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ShiduchActivityForm f = (sender as ShiduchActivityForm);
            ShiduchActivityForm form2;
            if (f.save)
            {
                SaveOpenReminder = true;
                if (f.OpenNewActivity)
                {
                    ShiduchActivity s = new ShiduchActivity();
                    s.UserId = f.Activity.UserId;
                    s.PeopleId = f.Activity.PeopleId;
                    s.IdSideB = f.Activity.IdSideB;
                    form2 = new ShiduchActivityForm(s, f.MyPeople);
                    form2.isNew_Active_From_Complete_Active = true;
                    form2.Show();
                    form2.FormClosed += SForm_FormClosed;
                }
            }
            if ((sender as ShiduchActivityForm).OpenSideB)//אם רוצה לפתוח פעילות של הצד השני
            {

                //f.Close();
                People person = f.Shiduch;
                People shiduch = f.MyPeople;
                string sql = "select s.*,r.id as remindID, r.Date as remindDate,r.Done,r.IdUser as remindIdUser from ShiduchActivity s inner join ReminderActivity r " +
                        "on s.Id=r.IdActivity " +
                        "where s.UserId=" + f.Activity.UserId + " and PeopleId=" +
                        person.ID + " and IdSideB=" + shiduch.ID + " and Action=" + f.Activity.Action +
                        " and abs(DATEDIFF(day,s.Date,'" + f.Activity.Date.ToString("yyyy-MM-dd h:mm tt") + "'))" +
                        " between 0 and 15";
                //        " and ( convert(varchar(10), s.Date, 103)='" + f.Activity.Date.ToShortDateString() + "' "+
                //"or convert(varchar(10), s.Date, 103)>'" + f.Activity.Date.ToShortDateString() + 
                //        "' or s.Date >= DATEADD(DAY, -14,'" + f.Activity.Date.ToString("yyyy-MM-dd h:mm tt") + "'))";
                SqlDataReader reader = DBFunction.ExecuteReader(sql);
                ShiduchActivity s = new ShiduchActivity();
                string notesSide = removeFromString(f.Activity.NotesSummary);
                if (reader.Read())
                {
                    ShiduchActivity.readerToShiduchActivity(ref reader, ref s);
                    s.NotesSummary = removeFromString(s.NotesSummary) + "=====צד ב'=====" + Environment.NewLine + notesSide;
                    form2 = new ShiduchActivityForm(s, person, false, true);
                }
                else
                {
                    s.IdSideB = shiduch.ID;
                    s.Action = f.Activity.Action;
                    s.UserId = f.Activity.UserId;
                    s.NotesSummary += "\r\n =====צד ב'=====\r\n" + notesSide;
                    form2 = new ShiduchActivityForm(s, person, true, false, true);
                }
                reader.Close();
                form2.Show();
                form2.FormClosed += SForm_FormClosed;

                //אם יש כבר פעילות דומה אז לפתוח אותה
                //אחרת לפתוח טופס חדש של פעילות
            }
            if ((sender as ShiduchActivityForm).save)
            {
                //foreach (Form frm in Application.OpenForms)
                //{
                //    if (frm.GetType() ==typeof( MainForm))
                //    {
                //        (frm as MainForm).LoadReminder();
                //    }
                //}
                //saveReminder = true;

            }
        }
        public static string removeFromString(string s)
        {
            if (s.IndexOf("=====צד ב'=====") >= 0)
                return s.Substring(0, s.IndexOf("=====צד ב'====="));
            else return s;
        }
    }
}
