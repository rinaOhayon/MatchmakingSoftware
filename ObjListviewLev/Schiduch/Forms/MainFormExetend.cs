/*
-----------------------------------------------INFORMATION ABOUT THIS CLASS-------------------------------------------------------
this mainform exetend class will go over all loading data from db we need to do
*/
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;
using System.Runtime;
using System.Threading;
using BrightIdeasSoftware;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Schiduch.Classes.Program;
using Schiduch.Forms.Manager;

namespace Schiduch
{
    public partial class MainForm : Form
    {

        public void LoadShadchanHandlers(int chadchanid = 0, ListView lstvw = null, bool mkcolor = false)
        {
            string addsql = "";
            if (chadchanid != 0)
                addsql = " and chadchan =" + chadchanid;
            lstvw.Items.Clear();
            SqlDataReader reader = DBFunction.ExecuteReader("select users.name,peoples.chadchan,peoples.firstname,peoples.id as pid,peoples.lastname,users.id,relatedid from peoples" +
                "  inner join peopledetails on ID=relatedid inner join users on peoples.chadchan=users.id  where chadchan <> 0" + addsql);
            while (reader.Read())
            {
                ListViewItem item = new ListViewItem(new string[] {
                    reader["firstname"].ToString(),
                    reader["lastname"].ToString(),
                    "שדכן מטפל : " + reader["name"].ToString(),
                    reader["pid"].ToString(),
                    reader["id"].ToString()
                }, 4);
                if (mkcolor) item.ForeColor = Color.Blue;
                lstvw.Items.Add(item);
            }
            reader.Close();
        }
        public void LoadShadcanim()
        {
            string sql;
            //    string txt="";
            int controlhide = 1;
            GLOBALVARS.Shadchanim = new ArrayList();
            GLOBALVARS.Users = new List<User>();
            User u;
            // cmbusers.Items.Add(new KeyValueClass("הכל", 0));
            if (GLOBALVARS.MyUser.Control == User.TypeControl.Admin)
                controlhide = 100;
            sql = @"select u.ID,u.name,u.tel,u.email,count(r.ByUser) as d,t1.mdate,t2.mdateexit,Sector
                    from users u left join RegisterInfo r on u.ID=r.ByUser left join
                    (select l.UserId, max(l.date) mdate from log l where l.Action=1 group by l.UserId ) t1 on t1.UserId=u.ID left join
                    (select l.UserId, max(l.DateExit) mdateexit from log l where l.Action=1  group by l.UserId ) t2 on t2.UserId=u.ID
                    where u.Control<" + controlhide + @"
                    group by u.ID,u.name,u.tel,u.email,t1.mdate,t2.mdateexit,Sector
                    order by d desc";
            SqlDataReader reader;
            reader = DBFunction.ExecuteReader(sql);
            lstchadcan.Items.Clear();
            ConnetKnow = 0;
            while (reader != null && reader.Read())
            {
                u = new User();
                u.Name = (string)reader["name"];
                u.ID = (int)reader["ID"];
                u.Tel = (string)reader["tel"];
                u.Email = (string)reader["email"];
                u.Sector = (string)reader["Sector"];
                if (u.Sector.Length > 0)
                {
                    string[] s = u.Sector.Split(',');
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (u.SectorView.Length > 0)
                            u.SectorView += ", ";
                        u.SectorView += GLOBALVARS.Sectors.ToArray()[int.Parse(s[i])].SectorName;
                    }
                }
                GLOBALVARS.Users.Add(u);
                KeyValueClass temp = new KeyValueClass((string)reader["name"], (int)reader["ID"]);
                GLOBALVARS.Shadchanim.Add(temp);
                lstchadcan.Items.Add(new ListViewItem(new string[] { (string)reader["name"],
                          (string)reader["tel"],(string)reader["email"],reader["ID"].ToString(),
                      reader["d"].ToString(),Connect(ref reader)
                  }, 2));
            }
            reader.Close();
            DBFunction.CloseConnections();
            if (controlhide != 1)
            {
                lblconnect.Visible = true;
                lblconnect.Text += ConnetKnow + " מתוך " + GLOBALVARS.Shadchanim.Count;
            }
        }
        public int ConnetKnow { get; set; } = 0;
        private string Connect(ref SqlDataReader reader)
        {
            string name = (string)reader["name"];
            DateTime date = reader["mdate"] != DBNull.Value ? DateTime.Parse(reader["mdate"].ToString()) : new DateTime();
            DateTime dateExit = reader["mdateexit"] != DBNull.Value ? DateTime.Parse(reader["mdateexit"].ToString()) : new DateTime();
            //אם הכניסה והיציאה שווים
            //וגם התאריך הוא כמו התאריך של היום
            //או של אתמול
            if (date.CompareTo(dateExit) == 0 &&
                (date.ToShortDateString() == DateTime.Now.ToShortDateString() ||
                date.AddDays(1).ToShortDateString() == DateTime.Now.ToShortDateString()))
            {
                ConnetKnow += 1;
                return "מחובר כעת";
            }
            return "";
        }
        public void LoadClients()
        {
            string sql;
            GLOBALVARS.Clients = new ArrayList();
            sql = "select firstname + ' ' + lastname as allname,id from peoples where show=0 order by firstname ";
            SqlDataReader reader;
            reader = DBFunction.ExecuteReader(sql);
            while (reader != null && reader.Read())
            {
                KeyValueClass temp = new KeyValueClass((string)reader["allname"], (int)reader["ID"]);
                GLOBALVARS.Clients.Add(temp);
            }
            reader.Close();
        }
        public List<KeyValueClass> ListNameColumnPeople { get; set; }
        public List<People> lst = new List<People>();
        public bool LoadData(bool reset = false)
        {
            ListNameColumnPeople = new List<KeyValueClass>();
            ListNameColumnPeople.Add(new KeyValueClass("Status", 0));
            ListNameColumnPeople.Add(new KeyValueClass("Tz", 0));
            ListNameColumnPeople.Add(new KeyValueClass("HealthStatus", 0));
            List<People> lst = new List<People>();
            SqlDataReader myreader;

            myreader = People.ReadAll(0, false, true);

            while (myreader.Read())
            {
                People p = new People();
                PeopleManipulations.ReaderToPeople(ref p, ref myreader, PeopleManipulations.RtpFor.ForSearch);
                lst.Add(p);
            }
            olstpeople.BeginUpdate();
            olstpeople.SetObjects(lst);
            olstpeople.EndUpdate();
            myreader.Close();
            DBFunction.CloseConnections();
            formloadmsg.Close();
            return true;
        }

        private string[] LoadDataFromFile()
        {
            if (File.Exists("People.bin"))
            {
                return File.ReadAllLines("People.bin");
            }
            return null;
        }

        private void WriteDataToFile(string speople)
        {

            if (!File.Exists("People.bin"))
                File.Create("People.bin").Close();
            File.WriteAllText("People.bin", speople);
            return;
        }
        public void LoadReminder()
        {
            SqlDataReader reader = ShiduchActivity.GetActivities(false, null, false, true);
            lstReminder.Items.Clear();
            ListView lst = lstReminder;
            ListViewItem item;
            ShiduchActivity.ActionType action;
            lstReminder.BeginUpdate();
            while (reader.Read())
            {
                string name = reader["FullNameB"] != System.DBNull.Value ? (string)reader["FullNameB"] : "";
                action = (ShiduchActivity.ActionType)(int)reader["Action"];
                item = new ListViewItem(new string[] {
                 DateTime.Parse(reader["Date"].ToString()).ToShortDateString(),
                 (string)reader["FullNameA"],
                  ShiduchActivity.ConvertAction(action, reader),
                  name
                });
                item.Tag = reader["Id"];
                //item.ImageKey = "phone-icon (1).png";
                switch (action)
                {
                    case ShiduchActivity.ActionType.proposal:
                        item.BackColor = Color.FromArgb(234, 195, 152);
                        break;
                    case ShiduchActivity.ActionType.date:
                        item.BackColor = Color.FromArgb(234, 133, 129);
                        break;
                    case ShiduchActivity.ActionType.details:
                        item.BackColor = Color.FromArgb(234,206,187);
                        break;
                    case ShiduchActivity.ActionType.other:
                        item.BackColor = Color.FromArgb(183, 183, 183);
                        break;
                }
                lst.Items.Add(item);
            }
            lstReminder.EndUpdate();
            reader.Close();
        }
        public void lstReminder_DoubleClick(object sender, EventArgs e)
        {
            ShiduchActivity s = new ShiduchActivity();
            for (int i = 0; i < 1; i++)
            {
                s.openShiduchActivityForm(lstReminder);
            }

            if (s.openReminder)
            {
                LoadReminder();
            }
        }
        /// <summary>
        /// set user prefer of columns in olstpeople
        /// </summary>
        private void LoadSettings()
        {
            if (!File.Exists("Column_Setting_OlstPeople.bin"))
                WriteSettings();
            for (int i = 0; i < 1; i++)
            {
                olstpeople.RestoreState(File.ReadAllBytes("Column_Setting_OlstPeople.bin"));
                olstpeople.RebuildColumns();
            }
            SetChooseCheckBox();
        }
        private void WriteSettings()
        {
            File.WriteAllBytes("Column_Setting_OlstPeople.bin", olstpeople.SaveState());
        }


        private void button12_Click(object sender, EventArgs e)
        {
            dtActivityDateto.Value = DateTime.Now;
            switch ((string)((Button)sender).Tag)
            {
                case "D":
                    dtActivityDatefrom.Value = DateTime.Now.AddDays(-1);
                    break;
                case "W":
                    dtActivityDatefrom.Value = DateTime.Now.AddDays(-7);
                    break;
                case "M":
                    dtActivityDatefrom.Value = DateTime.Now.AddDays(-30);
                    break;
                //todo לשנות לתאריך ההפצה של התוכנה
                case "E":
                    dtActivityDatefrom.Value = DateTime.Now.AddYears(-2);
                    break;
            }
        }



        private void olstActivityDiary_FormatRow(object sender, FormatRowEventArgs e)
        {
            ShiduchActivity s = (ShiduchActivity)e.Model;
            if (s.HideDelete)
                e.Item.BackColor = Color.Gray;

        }



        private void olstActivityDiary_SubItemChecking(object sender, SubItemCheckingEventArgs e)
        {
            e.NewValue = e.CurrentValue;
        }


        private void LoadTab9(object objd)
        {

            Tab9 = true;
            LoadClients();
            DictinorayRow.LoadDictonary();

            foreach (object obj in GLOBALVARS.Clients)
            {
                cmb_matchclient.Items.Add(obj);
            }
            picmatch.Visible = false;


        }

        private void btnAllCard_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        private void olstpeople_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int x = olstpeople.Columns.IndexOfKey("olvColumn1");
            ObjectListView o = sender as ObjectListView;
            ColumnHeader h = o.Columns[e.Column];
            if (h.Index == 0)
                lstCheckBoxChooseColumn.Visible = !lstCheckBoxChooseColumn.Visible;
            else
                lstCheckBoxChooseColumn.Visible = false;

        }

        //העמודה הראשונה היא העמודה של בחירת העמודות ולכן צריך 
        //להוסיף אחד או לדלג על העמודה הראשונה
        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            (olstpeople.AllColumns[e.Index + 1] as OLVColumn).IsVisible = e.NewValue == CheckState.Checked;
            olstpeople.RebuildColumns();
        }
        /// <summary>
        /// set the checked items in checkBoxList
        /// </summary>
        private void SetChooseCheckBox()
        {
            for (int i = 1; i < olstpeople.AllColumns.Count; i++)
            {
                if (olstpeople.AllColumns[i].IsVisible)
                {
                    lstCheckBoxChooseColumn.SetItemChecked(i - 1, true);
                }

            }

        }

        private void olstmatch_DoubleClick(object sender, EventArgs e)
        {
            if (olstmatch.SelectedObject != null)
            {
                Match p = olstmatch.SelectedObject as Match;
                bool show = true;
                Log.AddAction(Log.ActionType.ClientOpen, new Log(Log.ActionType.ClientOpen,
                    p.Name + "^" + p.ID.ToString()));
                ShiduchActivity.insertActivity(
                   new ShiduchActivity()
                   {
                       Action = (int)ShiduchActivity.ActionType.openForms,
                       Date = DateTime.Now,
                       PeopleId = p.ID,
                       UserId = GLOBALVARS.MyUser.ID,

                   });
                if (sender is bool && (bool)sender == false)
                    show = false;
                OpenDetails(p.ID, show);
            }
        }

        private void olstpeople_ColumnReordered(object sender, ColumnReorderedEventArgs e)
        {
            int x = 5;

        }





    }
}
