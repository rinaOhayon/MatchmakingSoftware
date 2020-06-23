using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

using System.Data.SqlClient;
using System.Drawing;
using System.Data;
using Schiduch.Classes.Program;

namespace Schiduch
{
    public enum SexsType { Male = 1, Female = 2 };
    public class People
    {
        public string FindMatch = "מצא התאמה";
        public string OpenCard = "פרטים מלאים";
        public string AgeCorrect = "גיל מדויק";
        public Color Itemcolor = Color.Black;
        public enum ShowTypes { Show = 0, HideDetails = 1, HideFromUsers, VIP = 4, Personal = 5, Delete = 8 }
        public enum ReasonType { Wedding = 1, Edtinging = 0, ShadChan = 2, AllowLimited = 3 };
        public SexsType sexsType;
        public bool OpenDetailsForAdd;
        public bool OpenForTrashPeople;
        public bool OpenForPersonalAdd;
        public bool OpenForPersonalEdit;
        public bool Temp;
        public string FirstName;
        public string Lasname;
        public int Reason;
        public int ByUser;
        public int TempId;
        public int Sexs;
        public float Age;
        public float Tall;
        public string Weight;//fat
        public string FaceColor;
        public string Looks;
        public string WorkPlace=""; //added
        public string Beard = "";
        public string City="";
        public string Zerem="";
        public string Eda="";
        public bool FutureLearn = false;
        public string LearnStaus = "";
        public string Background = "";
        public string DadWork = "";
        public string CoverHead = "";
        public string GorTorN = "";
        public string TneedE = "";
        public string StakeM = "";
        public int OpenHead;
        public string Status = "";
        public int RealId;
        public int Show = 0;
        public RegisterInfo Register;
        public PeopleDetails Details;
        public List<NotesOfPeople> Note;
        public string TempNotes;
        public string Chadchan = ""; // need connet to chandan class;

        public string Tz = "";
        public string KindChasidut = "";
        public string ShiducNum = "";
        public string HealthStatus = "";
        public string HealthDetails = "";
        public string ZeremMom = "";
        public string BirthDayHebrew = "";
        public string DeleteReason = "";
        public int ID;
        public People()
        {
            this.Register = new RegisterInfo();
            this.Details = new PeopleDetails();
            this.Note = new List<NotesOfPeople>();
        }
        public static bool InsretNew(People people,out int ID)
        {
            int n = people.Note.Count * 5;
            SqlParameter[] prms = new SqlParameter[92 + n];
            string sql, sqlpeoples, sqldetails, sqlregister, sqlNotes = "";
            sqlpeoples = "INSERT INTO Peoples VALUES(" +
                BuildSql.InsertSql(out prms[0], people.FirstName) +
                BuildSql.InsertSql(out prms[1], people.Lasname) +
                BuildSql.InsertSql(out prms[2], people.Sexs) +
                BuildSql.InsertSql(out prms[3], people.Age) +
                BuildSql.InsertSql(out prms[4], people.Tall) +
                BuildSql.InsertSql(out prms[5], people.Weight) +
                BuildSql.InsertSql(out prms[6], people.FaceColor) +
                BuildSql.InsertSql(out prms[7], people.Looks) +
                BuildSql.InsertSql(out prms[8], people.Beard) +
                BuildSql.InsertSql(out prms[9], people.City) +
                BuildSql.InsertSql(out prms[10], people.Zerem) +
                BuildSql.InsertSql(out prms[11], people.Eda) +
                BuildSql.InsertSql(out prms[12], people.FutureLearn) +
                BuildSql.InsertSql(out prms[13], people.Background) +
                BuildSql.InsertSql(out prms[14], people.DadWork) +
                BuildSql.InsertSql(out prms[15], people.CoverHead) +
                BuildSql.InsertSql(out prms[16], people.GorTorN) +
                BuildSql.InsertSql(out prms[17], people.TneedE) +
                BuildSql.InsertSql(out prms[18], people.StakeM) +
                BuildSql.InsertSql(out prms[19], people.OpenHead) +
                BuildSql.InsertSql(out prms[20], people.Status) +
                people.Show + "," +
                 BuildSql.InsertSql(out prms[21], people.LearnStaus) +
                BuildSql.InsertSql(out prms[22], people.Tz) +
                BuildSql.InsertSql(out prms[23], people.KindChasidut) +
                BuildSql.InsertSql(out prms[24], people.ShiducNum) +
                BuildSql.InsertSql(out prms[25], people.HealthStatus) +
                BuildSql.InsertSql(out prms[26], people.HealthDetails) +
                BuildSql.InsertSql(out prms[27], people.ZeremMom) +

                BuildSql.InsertSql(out prms[28], people.BirthDayHebrew) +
                BuildSql.InsertSql(out prms[29], people.DeleteReason) +
                BuildSql.InsertSql(out prms[30], people.Temp) +
            BuildSql.InsertSql(out prms[31], people.Chadchan, true) +
                 ");";

            sqldetails = "INSERT INTO PeopleDetails VALUES(" +
            BuildSql.InsertSql(out prms[32], people.Details.Street) +
            BuildSql.InsertSql(out prms[33], people.Details.Schools) +
            BuildSql.InsertSql(out prms[34], people.Details.Tel1) +
            BuildSql.InsertSql(out prms[35], people.Details.Tel2) +
            BuildSql.InsertSql(out prms[36], people.Details.WhoAmI) +
            BuildSql.InsertSql(out prms[37], people.Details.WhoIWant) +
            BuildSql.InsertSql(out prms[39], people.Details.DadName) +
            BuildSql.InsertSql(out prms[40], people.Details.MomName) +
            BuildSql.InsertSql(out prms[41], people.Details.ChildrenCount) +
            BuildSql.InsertSql(out prms[42], people.Details.SiblingsSchools) +
            BuildSql.InsertSql(out prms[43], people.Details.MomLname) +
            BuildSql.InsertSql(out prms[44], people.Details.MomWork) +
            BuildSql.InsertSql(out prms[45], people.Details.MoneyGives) +
            BuildSql.InsertSql(out prms[46], people.Details.MoneyRequired) +
            BuildSql.InsertSql(out prms[47], people.Details.MoneyNotesFlex) +
            BuildSql.InsertSql(out prms[48], people.Details.HomeRav) +
            BuildSql.InsertSql(out prms[49], people.Details.MechutanimNames) + "@DataID," +
            BuildSql.InsertSql(out prms[50], people.Details.ZevetInfo) +
            BuildSql.InsertSql(out prms[51], people.Details.FriendsInfo) +
            BuildSql.InsertSql(out prms[54], people.Details.Notes) +
            BuildSql.InsertSql(out prms[55], people.Details.OwnChildrenCount) +
            BuildSql.InsertSql(out prms[56], people.WorkPlace) +
            BuildSql.InsertSql(out prms[57], people.Details.MoneyToShadchan) +
            BuildSql.InsertSql(out prms[58], people.Details.YesivaKorHighSchool) +
            BuildSql.InsertSql(out prms[59], people.Details.YeshivaGorSeminary) +
            BuildSql.InsertSql(out prms[60], people.Details.KibutzorMaslul) +
            BuildSql.InsertSql(out prms[61], people.Details.Licence) +
            BuildSql.InsertSql(out prms[62], people.Details.Smoker) +
            BuildSql.InsertSql(out prms[63], people.Details.EdaExpectation) +
            BuildSql.InsertSql(out prms[64], people.Details.AgeExpectation) +
            BuildSql.InsertSql(out prms[65], people.Details.DadYeshiva) +
            BuildSql.InsertSql(out prms[66], people.Details.MomSeminary) +
            BuildSql.InsertSql(out prms[69], people.Details.StatusParents) +
            BuildSql.InsertSql(out prms[70], people.Details.CommunityTo) +
            BuildSql.InsertSql(out prms[71], people.Details.ParentHealth) +
            BuildSql.InsertSql(out prms[72], people.Details.ParentHealthDetails) +
            BuildSql.InsertSql(out prms[73], people.Details.LocationChild) +
            BuildSql.InsertSql(out prms[74], people.Details.NumMarriedSibilings) +
            BuildSql.InsertSql(out prms[75], people.Details.ContactShiduch) +
            BuildSql.InsertSql(out prms[76], people.Details.ContactPhone) +
            BuildSql.InsertSql(out prms[77], people.Details.FamilyAbout) +
            BuildSql.InsertSql(out prms[78], people.Details.Telephone) +
            BuildSql.InsertSql(out prms[79], people.Details.PhoneOfBachur) +
            BuildSql.InsertSql(out prms[80], people.Details.PhoneKosherLevel) +

            BuildSql.InsertSql(out prms[81], people.Details.Mail, true) +
            ");";


            sqlregister = "INSERT INTO RegisterInfo VALUES(" +
                 BuildSql.InsertSql(out prms[82], DateTime.Now) +
                 "@DataID" + "," +
                 GLOBALVARS.MyUser.ID + "," +
                  BuildSql.InsertSql(out prms[83], GLOBALVARS.MyUser.Name) +
                  BuildSql.InsertSql(out prms[84], DateTime.Now, true)
                  + ");";
            int iPrm = 90;
            foreach (var item in people.Note)
            {
                sqlNotes += "INSERT INTO NotesOfPeople VALUES(" +
                        BuildSql.InsertSql(out prms[iPrm++], GLOBALVARS.MyUser.ID) +
                        BuildSql.InsertSql(out prms[iPrm++], GLOBALVARS.MyUser.Name) +
                        "@DataID" + "," +
                        BuildSql.InsertSql(out prms[iPrm++], item.NoteText) +
                        BuildSql.InsertSql(out prms[iPrm++], DateTime.Now, true)
                        + "); ";
            }

            prms[85] = new SqlParameter("@D", SqlDbType.Int);
            prms[85].Direction = ParameterDirection.Output;
            sql = "BEGIN TRANSACTION " +
            "DECLARE @DataID int;" +
            sqlpeoples +
            "SELECT @DataID = scope_identity();" +
            "SELECT @D = scope_identity();" +
            sqldetails +
            sqlregister +
            sqlNotes+
            "COMMIT";
            int length = sql.Length;
            // return DBFunction.Execute(sql, prms);
            bool f = DBFunction.Execute(sql, prms);
            ID = 0;
            if (prms[85].Value != DBNull.Value)
                ID = Convert.ToInt32(prms[85].Value);
            ShiduchActivity Activity = new ShiduchActivity();
            Activity.Date = DateTime.Now;
            Activity.UserId = GLOBALVARS.MyUser.ID;
            Activity.PeopleId = ID;
            Activity.Action = (int)ShiduchActivity.ActionType.reception;
            ShiduchActivity.insertActivity(Activity);
            return f;
        }
        public static bool InsertNewNotes(NotesOfPeople note)
        {
            SqlParameter[] prms = new SqlParameter[10];
            string sqlnotes, sql;
            sqlnotes = "INSERT INTO NotesOfPeople VALUES(" +
            BuildSql.InsertSql(out prms[0], GLOBALVARS.MyUser.ID) +
            BuildSql.InsertSql(out prms[1], GLOBALVARS.MyUser.Name) +
            BuildSql.InsertSql(out prms[2], note.PeopleId) +
            BuildSql.InsertSql(out prms[3], note.NoteText) +
            BuildSql.InsertSql(out prms[4], DateTime.Now, true)
            + ");";
            sql = "BEGIN TRANSACTION " +
           sqlnotes +
           "COMMIT";
            return DBFunction.Execute(sql, prms);

        }
        public static bool UpdateNotes(NotesOfPeople note)
        {
            SqlParameter[] prms = new SqlParameter[10];
            string sqlnotes, sql;
            sql = "BEGIN TRANSACTION ";

            sql += "update NotesOfPeople SET " +
            BuildSql.UpdateSql(out prms[0], note.UserId, "UserId") +
            BuildSql.UpdateSql(out prms[1], note.UserName, "UserName") +
            BuildSql.UpdateSql(out prms[2], note.PeopleId, "PeopleId") +
            BuildSql.UpdateSql(out prms[3], note.NoteText, "NoteText") +
            BuildSql.UpdateSql(out prms[4], note.NoteDate, "NoteDate", true)
            + " where NoteId=" + note.NoteId + "; COMMIT";
            return DBFunction.Execute(sql, prms);

        }
        public static SqlDataReader ReadAll(int temp = 0, bool all = false, bool login = false, bool persnoal_vip = false, bool vip = false, bool openForActivity = false, People p = null)
        {
            //try
            //{
                string show = " AND show <> 8 AND (show <2 or (show=5 and chadchan like '%{" + GLOBALVARS.MyUser.ID + "}%') or (show=4 and chadchan like '%{" + GLOBALVARS.MyUser.ID + "}%'))";
                if (persnoal_vip)
                    show = " where (show=5 and chadchan like '%{" + GLOBALVARS.MyUser.ID + "}%') or (show=4 and chadchan like '%{" + GLOBALVARS.MyUser.ID + "}%')";
                if (GLOBALVARS.MyUser.Control == User.TypeControl.Manger || GLOBALVARS.MyUser.Control == User.TypeControl.Admin)
                {
                    show = " where show <> 8";
                    if (persnoal_vip)
                        show = " where show > 0 and show <> 8";
                }
                show += " and Temp='false'";
                string sql = "select * from peoples p inner join peopledetails pd on p.ID=pd.relatedid inner join " +
                    "registerinfo r on pd.relatedid=r.relatedid " + show;
                if (openForActivity)
                {
                    string where = " and sexs=";
                    if (p.Sexs == 1)
                        where += "2 ";
                    else
                        where += "1 ";
                    sql = "select FirstName, LastName, YeshivaGorSeminary,Street,City,ID from peoples p inner join" +
                        " peopledetails pd on p.ID=pd.relatedid " + show + where;
                }
                if (temp == 1)
                {
                    sql = "select * from peoples p left join peopledetails pd on p.ID=pd.relatedid where temp='true'" + show;
                }
                if (all)
                    sql = "select  p.ID,p.FirstName+' '+p.LastName as FullName from peoples p " + show;
                if (login)
                    sql = "select top 50 id,schools,sexs,firstname,lastname,tall,age,City,fat," +
                                    "FaceColor,Looks,WorkPlace,Beard,Zerem,Eda,LearnStatus,DadWork," +
                                    "Background,Status,Tz,KindChasidut,HealthStatus,ZeremMom,Street," +
                                    "DadName,MomName,MomWork  from peoples p inner join peopledetails pd on p.ID=pd.relatedid " + show + " order by id desc";
                if (persnoal_vip)
                    sql = "select top 50 id,schools,sexs,firstname,lastname,tall,age,City,fat," +
                                    "FaceColor,Looks,WorkPlace,Beard,Zerem,Eda,LearnStatus,DadWork," +
                                    "Background,Status,Tz,KindChasidut,HealthStatus,ZeremMom,Street," +
                                    "DadName,MomName,MomWork  from peoples p inner join peopledetails pd on p.ID=pd.relatedid " + show + " order by show desc";
                if (vip)
                    sql = "select MONEYTOSHADCHAN,chadchan,id,show,schools,sexs,firstname,lastname,tall,age,notes,City,Tel1,Tel2,Telephone,PhoneOfBachur from peoples p inner join peopledetails pd on p.ID=pd.relatedid " + show + " order by show desc";
                SqlDataReader reader = DBFunction.ExecuteReader(sql);
                return reader;
            //}
            //catch (Exception)
            //{
            //    return null;
            //}

        }

        public static SqlDataReader ReadById(int ID)
        {
            try
            {
                string sql = "select * from peoples p inner join peopledetails pd on p.ID = pd.relatedid inner join " +
                    "registerinfo r on pd.relatedid=r.relatedid where p.ID=" + ID;
                SqlDataReader reader = DBFunction.ExecuteReader(sql);
                return reader;
            }
            catch (Exception ex)
            {
                MessageBox.Show("השגיאה הבאה התרחשה \n" + ex.Message);
                return null;
            }
        }
        public static bool UpdatePeople(People p, bool Wedding, bool Shadchan = false, string Notes = null, bool PublishClient = false)
        {

            string sql = "";

            string where = " where id=" + p.ID + " ";
            string Rwhere = " where relatedid=" + p.ID + " ";
            bool PlusTblReg = true;
            SqlParameter[] prms = new SqlParameter[100];

            //if (p.Show != 5)
            //{  // check is not personal user 

            //    PlusTblReg = true; // for future use

            //    // if (Shadchan)
            //  //   return ShadchanUpdate();

            //    if (Wedding)
            //        return WeddingUpdate(p);

            //    //if (!GLOBALVARS.MyUser.CanEdit)
            //    //    return UpdateTemp(Notes);
            //}
            //else
            //{
            //    if (!PublishClient)
            //        p.Show = 5;
            //    p.Chadchan = "{" + GLOBALVARS.MyUser.ID.ToString() + "}";
            //}
            sql = "BEGIN TRANSACTION ";

            sql += "update peoples SET " +
                BuildSql.UpdateSql(out prms[0], p.Age, "age") +
                BuildSql.UpdateSql(out prms[1], p.Background, "background") +
                BuildSql.UpdateSql(out prms[2], p.Beard, "Beard") +
                BuildSql.UpdateSql(out prms[3], p.City, "City") +
                BuildSql.UpdateSql(out prms[4], p.CoverHead, "CoverHead") +
                BuildSql.UpdateSql(out prms[5], p.DadWork, "DadWork") +
                BuildSql.UpdateSql(out prms[6], p.Eda, "eda") +
                BuildSql.UpdateSql(out prms[7], p.FaceColor, "FaceColor") +
                BuildSql.UpdateSql(out prms[8], p.FirstName, "FirstName") +
                BuildSql.UpdateSql(out prms[9], p.FutureLearn, "FutureLearn") +
                BuildSql.UpdateSql(out prms[10], p.GorTorN, "GorTorN") +
                BuildSql.UpdateSql(out prms[11], p.Lasname, "Lastname") +
                BuildSql.UpdateSql(out prms[12], p.Looks, "Looks") +
                BuildSql.UpdateSql(out prms[13], p.OpenHead, "OpenHead") +
                BuildSql.UpdateSql(out prms[15], p.Sexs, "Sexs") +
                BuildSql.UpdateSql(out prms[16], p.Show, "show") +
                BuildSql.UpdateSql(out prms[17], p.StakeM, "StakeM") +
                BuildSql.UpdateSql(out prms[18], p.Status, "Status") +
                BuildSql.UpdateSql(out prms[19], p.Tall, "Tall") +
                BuildSql.UpdateSql(out prms[20], p.TneedE, "TneedE") +
                BuildSql.UpdateSql(out prms[59], p.LearnStaus, "LearnStatus") +
                BuildSql.UpdateSql(out prms[21], p.Zerem, "Zerem") +
                BuildSql.UpdateSql(out prms[70], p.Tz, "Tz") +
                BuildSql.UpdateSql(out prms[71], p.KindChasidut, "KindChasidut") +
                BuildSql.UpdateSql(out prms[72], p.ShiducNum, "ShiducNum") +
                BuildSql.UpdateSql(out prms[73], p.HealthStatus, "HealthStatus") +
                BuildSql.UpdateSql(out prms[74], p.HealthDetails, "HealthDetails") +
                BuildSql.UpdateSql(out prms[88], p.ZeremMom, "ZeremMom") +
                BuildSql.UpdateSql(out prms[91], p.BirthDayHebrew, "BirthDayHebrew") +

                BuildSql.UpdateSql(out prms[23], p.Chadchan, "Chadchan") +
                BuildSql.UpdateSql(out prms[93], p.Temp, "Temp") +
                BuildSql.UpdateSql(out prms[22], p.Weight, "fat", true) + where + ";";

            sql += " update peopledetails SET " +
                BuildSql.UpdateSql(out prms[24], p.Details.ChildrenCount, "ChildrenCount") +
                BuildSql.UpdateSql(out prms[25], p.Details.DadName, "DadName") +
                BuildSql.UpdateSql(out prms[27], p.Details.FriendsInfo, "FriendsInfo") +
                BuildSql.UpdateSql(out prms[28], p.Details.HomeRav, "HomeRav") +
                BuildSql.UpdateSql(out prms[29], p.Details.MechutanimNames, "MechutanimNames") +
                BuildSql.UpdateSql(out prms[30], p.Details.MomLname, "MomLname") +
                BuildSql.UpdateSql(out prms[31], p.Details.MomName, "MomName") +
                BuildSql.UpdateSql(out prms[32], p.Details.MomWork, "MomWork") +
                BuildSql.UpdateSql(out prms[33], p.Details.MoneyGives, "MoneyGives") +
                BuildSql.UpdateSql(out prms[34], p.Details.MoneyNotesFlex, "MoneyNotesFlex") +
                BuildSql.UpdateSql(out prms[35], p.Details.MoneyRequired, "MoneyRequired") +
                BuildSql.UpdateSql(out prms[36], p.Details.Notes, "Notes") +
                BuildSql.UpdateSql(out prms[37], p.Details.OwnChildrenCount, "OwnChildrenCount") +
                BuildSql.UpdateSql(out prms[38], p.Details.RelatedId, "RelatedId") +
                BuildSql.UpdateSql(out prms[39], p.Details.Schools, "Schools") +
                BuildSql.UpdateSql(out prms[40], p.Details.SiblingsSchools, "SiblingsSchools") +
                BuildSql.UpdateSql(out prms[41], p.Details.Street, "Street") +
                BuildSql.UpdateSql(out prms[42], p.Details.Tel1, "Tel1") +
                BuildSql.UpdateSql(out prms[43], p.Details.Tel2, "Tel2") +
                BuildSql.UpdateSql(out prms[44], p.Details.WhoAmI, "WhoAmI") +
                BuildSql.UpdateSql(out prms[45], p.Details.WhoIWant, "WhoIWant") +

                BuildSql.UpdateSql(out prms[48], p.WorkPlace, "WorkPlace") +
                BuildSql.UpdateSql(out prms[49], p.Details.ZevetInfo, "ZevetInfo") +
                BuildSql.UpdateSql(out prms[75], p.Details.YesivaKorHighSchool, "YesivaKorHighSchool") +
                BuildSql.UpdateSql(out prms[76], p.Details.YeshivaGorSeminary, "YeshivaGorSeminary") +
                BuildSql.UpdateSql(out prms[77], p.Details.KibutzorMaslul, "KibutzorMaslul") +
                BuildSql.UpdateSql(out prms[78], p.Details.Licence, "Licence") +
                BuildSql.UpdateSql(out prms[79], p.Details.Smoker, "Smoker") +
                BuildSql.UpdateSql(out prms[80], p.Details.EdaExpectation, "EdaExpectation") +
                BuildSql.UpdateSql(out prms[81], p.Details.AgeExpectation, "AgeExpectation") +
                BuildSql.UpdateSql(out prms[82], p.Details.DadYeshiva, "DadYeshiva") +
                BuildSql.UpdateSql(out prms[83], p.Details.MomSeminary, "MomSeminary") +
                BuildSql.UpdateSql(out prms[84], p.Details.StatusParents, "StatusParents") +
                BuildSql.UpdateSql(out prms[85], p.Details.CommunityTo, "CommunityTo") +
                BuildSql.UpdateSql(out prms[86], p.Details.ParentHealth, "ParentHealth") +
                BuildSql.UpdateSql(out prms[87], p.Details.ParentHealthDetails, "ParentHealthDetails") +
                BuildSql.UpdateSql(out prms[89], p.Details.LocationChild, "LocationChild") +
                BuildSql.UpdateSql(out prms[90], p.Details.NumMarriedSibilings, "NumMarriedSibilings") +
                BuildSql.UpdateSql(out prms[63], p.Details.ContactShiduch, "ContactShiduch") +
                BuildSql.UpdateSql(out prms[64], p.Details.ContactPhone, "ContactPhone") +
                BuildSql.UpdateSql(out prms[65], p.Details.FamilyAbout, "FamilyAbout") +
                BuildSql.UpdateSql(out prms[66], p.Details.Telephone, "Telephone") +
                BuildSql.UpdateSql(out prms[67], p.Details.PhoneOfBachur, "PhoneOfBachur") +
                BuildSql.UpdateSql(out prms[68], p.Details.PhoneKosherLevel, "PhoneKosherLevel") +
                BuildSql.UpdateSql(out prms[69], p.Details.Mail, "Mail") +
                BuildSql.UpdateSql(out prms[61], p.Details.MoneyToShadchan, "MoneyToShadchan", true)
                + Rwhere + ";";
            // ^ it right
            sql += " update RegisterInfo SET ";

            sql += BuildSql.UpdateSql(out prms[62], DateTime.Now.Date, "LastUpdate", true) + Rwhere + "; ";
            sql += "COMMIT";

            DBFunction.Execute(sql, prms);

            PopUpMessage(false);
            ShiduchActivity Activity = new ShiduchActivity();
            Activity.Date = DateTime.Now;
            Activity.UserId = GLOBALVARS.MyUser.ID;
            Activity.PeopleId = p.ID;
            Activity.Action = (int)ShiduchActivity.ActionType.update;
            ShiduchActivity.insertActivity(Activity);
            return true;


        }


        private static bool WeddingUpdate(People p)
        {
            bool ret = false;
            ret = DBFunction.Execute("insert into notes values (" + GLOBALVARS.MyUser.ID + "," + p.ID + ",''," + (int)People.ReasonType.Wedding + ")");
            PopUpMessage(true);
            return ret;
        }


        public static bool AllowLimited(People p)
        {
            bool ret = false;
            ret = DBFunction.Execute("insert into notes values (" + GLOBALVARS.MyUser.ID + "," + p.ID + ",'אפשר לי לראות את החסוי הזה'," + (int)People.ReasonType.AllowLimited + ")");
            PopUpMessage(true);
            return ret;
        }

        private static void PopUpMessage(bool temp = false)
        {
            if (temp)
                MessageBox.Show("השינוי נשלח למערכת \n המערכת תאשר את השינוי בקרוב", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("שונה בהצלחה", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool DeletePeople(int peopleid, bool ask = true, bool perment = false)
        {
            try
            {
                int id = peopleid;
                DialogResult yesno = DialogResult.Yes;
                if (ask)
                    yesno = MessageBox.Show("האם אתה בטוח שברצונך למחוק", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                Forms.DeleteForm delf = new Forms.DeleteForm();
                if (yesno == DialogResult.Yes)
                {
                    string sql = "";
                    if (!perment)
                    {
                        delf.ShowDialog();
                        sql = "update peoples set show=8,DeleteReason=N'" + delf.ReasonDelete + "' where ID=" + id;
                    }
                    else
                    {
                        sql = "BEGIN TRANSACTION delete from peoples where ID=" + id + "; " +
                        "delete from peopledetails where relatedid=" + id + "; " +
                        "delete from registerinfo where relatedid=" + id + "; COMMIT";
                    }
                    if (DBFunction.Execute(sql))
                    {
                        if (!perment)
                        {
                            ShiduchActivity.insertActivity(
                            new ShiduchActivity()
                            {
                                Action = (int)ShiduchActivity.ActionType.delete,
                                Date = DateTime.Now,
                                PeopleId = id,
                                UserId = GLOBALVARS.MyUser.ID,
                            });
                            MessageBox.Show("נמחק בהצלחה, תוכל למצוא את הכרטיס בסל המחזור", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show("הכרטיס נמחק לצמיתות", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        return true;

                    }
                    else
                        MessageBox.Show("אירעה שגיאה", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            catch { MessageBox.Show("אירעה שגיאה", "", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
        }
    }
}
